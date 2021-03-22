module App

// open FSharp.Core
open Fable.Core
// open Fable.Core.JS
open Fable.Core.JsInterop
// open Fable.Promise
open Browser.Types
open Browser.Dom
open Blockly
open JupyterlabServices.__kernel_messages.KernelMessage
open JupyterlabServices.__kernel_kernel.Kernel
// open JupyterlabNotebook.Tokens
// open JupyterlabApputils.Types

//tricky here: if we try to make collection of requires, F# complains they are different types unless we specify obj type
let mutable requires: obj array =
    [| JupyterlabApputils.ICommandPalette; JupyterlabNotebook.Tokens.Types.INotebookTracker; JupyterlabApplication.ILayoutRestorer |]

//https://stackoverflow.com/questions/47640263/how-to-extend-a-js-class-in-fable
[<Import("Widget", from = "@phosphor/widgets")>]
[<AbstractClass>]
type Widget() =
    class
        // only implementing the minimal members needed
        member val node: HTMLElement = null with get, set
        /// Guess only fires once after attach to DOM. onAfterShow is called after every display (e.g., switching tabs)
        abstract onAfterAttach: unit -> unit
        /// Using to resize blockly
        abstract onResize: PhosphorWidgets.Widget.ResizeMessage -> unit
    end

/// Wrapping blockly in a widget helps with resizing and other GUI events that affect blockly
type BlocklyWidget(notebooks: JupyterlabNotebook.Tokens.INotebookTracker) as this =
    class
        inherit Widget()
        let generator = Blockly.python //Blockly.javascript
        ///Remove blocks from workspace without affecting variable map like blockly.getMainWorkspace().clear() would
        let clearBlocks() = 
            //NOTE: should we be using this.workspace here? What about crossing notebooks?
            let workspace = blockly.getMainWorkspace()
            let blocks = workspace.getAllBlocks(false)
            for b in blocks do
              b.dispose(false)
        do
            //listen for cell changes
            notebooks.activeCellChanged.connect( this.onActiveCellChanged, this ) |> ignore

            //inject intellisense dependency into Blockly toolbox
            Toolbox.notebooks <- notebooks

            //div to hold blockly
            let div = document.createElement ("div")
            div.setAttribute("style", "height: 480px; width: 600px;") //initial but will be immediately resized
            div.id <- "blocklyDiv" //for debug and to refer to during injection
            this.node.appendChild (div) |> ignore

            //div for buttons
            let buttonDiv =  document.createElement ("div")
            buttonDiv.id <- "buttonDiv"

            //button to trigger code generation
            let blocksToCodeButton = document.createElement ("button")
            blocksToCodeButton.innerText <- "Blocks to Code"
            blocksToCodeButton.addEventListener ("click", (fun _ -> this.RenderCode()))
            buttonDiv.appendChild( blocksToCodeButton) |> ignore

            //button to reverse xml to blocks
            let codeToBlocksButton = document.createElement ("button")
            codeToBlocksButton.innerText <- "Code to Blocks"
            codeToBlocksButton.addEventListener ("click", (fun _ -> this.RenderBlocks() ))
            buttonDiv.appendChild( codeToBlocksButton) |> ignore

            //button for bug reports
            let bugReportButton = document.createElement ("button")
            bugReportButton.innerText <- "Report Bug"
            bugReportButton.addEventListener
                ("click",
                 (fun _ ->
                     let win =
                         Browser.Dom.window.``open``
                             ("https://github.com/aolney/fable-jupyterlab-blockly-extension/issues", "_blank")
                     win.focus()
                     ()))
            buttonDiv.appendChild( bugReportButton) |> ignore

            //checkbox for JLab sync (if cell is selected and has serialized blocks, decode them to workspace; if cell is empty, empty workspace)
            let syncCheckbox = document.createElement ("input")
            syncCheckbox.setAttribute("type", "checkbox")
            syncCheckbox?``checked`` <- true //turn on sync by default
            syncCheckbox.id <- "syncCheckbox"
            let syncCheckboxLabel = document.createElement ("label")
            syncCheckboxLabel.innerText <- "Notebook Sync"
            syncCheckboxLabel.setAttribute("for", "syncCheckbox")
            buttonDiv.appendChild( syncCheckbox) |> ignore
            buttonDiv.appendChild( syncCheckboxLabel) |> ignore

            //TODO: this works, but not across multiple open notebooks. Need a better understanding of how blockly implements multiple workspaces
            //checkbox for autosave (force blocks to code if moving away from cell that already has blocks to code in it)
            // let autosaveCheckbox = document.createElement ("input")
            // autosaveCheckbox.setAttribute("type", "checkbox")
            // autosaveCheckbox?``checked`` <- false //turn off autosave by default
            // autosaveCheckbox.id <- "autosaveCheckbox"
            // let autosaveCheckboxLabel = document.createElement ("label")
            // autosaveCheckboxLabel.innerText <- "Autosave"
            // autosaveCheckboxLabel.setAttribute("for", "autosaveCheckbox")
            // buttonDiv.appendChild( autosaveCheckbox) |> ignore
            // buttonDiv.appendChild( autosaveCheckboxLabel) |> ignore

            //append all buttons in div
            this.node.appendChild ( buttonDiv ) |> ignore

        member val lastCell : JupyterlabNotebook.Tokens.Cell = null with get, set
        member val blocksRendered = false with get,set

        member val workspace: Blockly.Workspace = null with get, set
        member val notHooked = true with get, set
        member this.Notebooks = notebooks

        /// Refresh intellisense when kernel executes
        member this.onKernelExecuted =
            PhosphorSignaling.Slot<IKernel, IIOPubMessage>(fun sender args ->
                // Browser.Dom.console.log( "kernel message: " + args.header.msg_type.ToString() )
                let messageType = args.header.msg_type.ToString()
                if messageType = "execute_input" then
                    Browser.Dom.console.log ("kernel executed code, updating intellisense")
                    //log executed code as string
                    Logging.LogToServer( Logging.JupyterLogEntry082720.Create "execute-code" (args.content?code |> Some) )
                    Toolbox.UpdateAllIntellisense()
                //also hook errors here; log entire error object as json
                //else if messageType = "execute_reply" && args.content?status="error" then //would require subscribing to shell channel
                else if messageType = "error" then
                  Logging.LogToServer( Logging.JupyterLogEntry082720.Create "execute-code-error" ( JS.JSON.stringify(args.content) |> Some) )
                    
                true)
        
        member this.onActiveCellChanged =
          PhosphorSignaling.Slot<JupyterlabNotebook.Tokens.INotebookTracker, JupyterlabNotebook.Tokens.Cell>(fun sender args ->  

            //log the active cell change
            if args <> null then
              // console.log("I changed cells!")
              Logging.LogToServer( Logging.JupyterLogEntry082720.Create "active-cell-change" ( args.node.outerText |> Some ) ) //None )
              let syncCheckbox = document.getElementById("syncCheckbox")
              let autosaveCheckbox = document.getElementById("autosaveCheckbox")
              let isChecked aCheckbox : bool = (aCheckbox <> null) && aCheckbox?``checked`` |> unbox //checked is a f# reserved keyword

              //if sync enabled
              if isChecked(syncCheckbox) && notebooks.activeCell <> null then
                //if selected cell empty, clear the workspace
                // if notebooks.activeCell.model.value.text.Trim() = "" && this.blocksRendered then
                //if selected cell is not serialized and blocks have been rendered, clear the workspace;
                if this.blocksRendered && this.ActiveCellSerializedBlocksWorkspaceOption().IsNone then
                  // blockly.getMainWorkspace().clear() //to avoid duplicates
                  clearBlocks() //OLD: tight sync
                  ()
                //otherwise try to to create blocks from cell contents (fails gracefully)
                else
                  this.RenderBlocks()
                //Update intellisense on blocks we just created
                Toolbox.UpdateAllIntellisense()

              // if autosave enabled, attempt to save our current blocks to the previous cell we just navigated off (to prevent losing work)
              if isChecked(autosaveCheckbox) && notebooks.activeCell <> null then
                this.RenderCodeToLastCell()
                this.lastCell <- args
            true
           )

        /// Wait until widget shown to prevent injection from taking place before the DOM is ready
        /// Inject blockly into div and save blockly workspace to private member
        /// Previously hooked kernel messages here, but moved so we could hook/log without Blockly widget display
        override this.onAfterAttach() =
            
            //NOTE: trying to move this to constructor; for other extensions is it better not to wait for attach
            //listen for active cell changes in JupyterLab
            // this.Notebooks.activeCellChanged.connect( this.onActiveCellChanged, this ) |> ignore

            //set up blockly workspace
            this.workspace <-
                blockly.inject
                    (!^"blocklyDiv",
                     // Tricky: creatObj cannot be used here. Must use jsOptions to create POJO
                     jsOptions<Blockly.BlocklyOptions> (fun o -> o.toolbox <- !^Toolbox.toolbox |> Some)
                    // THIS FAILS!
                    // ~~ [
                    //     "toolbox" ==> ~~ [ "toolbox" ==> toolbox2 ] //TODO: using toolbox2 same as using empty string here
                    // ] :?> Object
                    )
            console.log ("blockly palette initialized")

            ///Add listeners for logging; see https://developers.google.com/blockly/guides/configure/web/events
            let logListener = System.Func<Blockly.Events.Abstract__Class,unit>(fun e ->
              // mark when blocks are added so we don't delete those blocks when changing cells UNLESS they've been written to code
              // problem here: this fires when user creates blocks AND when blocks are deserialized
              if e?``type`` = "create" then
                this.blocksRendered <- false

              // "finished loading" event seems to only fire when deserializing
              if e?``type`` = "finished_loading" then
                this.blocksRendered <- true
              //console.log(e?``type``)

              //for ui, get fine grain type; for var, get varId; everything else is event type and block id
              //a bit ugly b/c fable will not allow type tests against foreign interface: https://github.com/fable-compiler/Fable/issues/1580
              // let evt,id =
              //   //ui and block change have `element` ; var changes have `varId`
              //   match e?element=null,e?varId=null with
              //   | true,true -> e?``type``,e?blockId
              //   | false,true -> e?element,e?blockId
              //   | true,false -> e?``type``,e?varId
              //   | false,false -> e?``type``,e?blockId //this is impossible; take default case
              // Scratch that - log everything until we figure out what we don't want
              Logging.LogToServer( Logging.BlocklyLogEntry082720.Create e?``type`` e ) //evt id )
            )
            //check if logging should occur
            if Logging.logUrl.IsSome then 
              console.log ("!!! Logging select blockly actions to server !!!")

            this.workspace.removeChangeListener(logListener) |> ignore  //remove if already exists; for re-entrancy
            this.workspace.addChangeListener(logListener) |> ignore         


        /// Resize blockly when widget resizes
        override this.onResize( msg : PhosphorWidgets.Widget.ResizeMessage ) =
          let blocklyDiv = document.getElementById("blocklyDiv")
          let buttonDiv = document.getElementById("buttonDiv")
          //adjust height for buttons; TODO be smarter about button height / see if we can be smarter with JupyterLab layouts
          let adjustedHeight = msg.height - 30.0 // buttonDiv.clientHeight
          //it seems we don't need parent offset; we can just use 0 
          //let parentOffset = blocklyDiv.offsetParent :?> HTMLElement
          // blocklyDiv.setAttribute("style", "position: absolute; top: " + parentOffset.offsetTop.ToString() + "px; left: " + parentOffset.offsetLeft.ToString() + "px; width: " + msg.width.ToString() + "px; height: " + adjustedHeight.ToString() + "px" )
          blocklyDiv.setAttribute("style", "position: absolute; top: " + "0" + "px; left: " + "0" + "px; width: " + msg.width.ToString() + "px; height: " + adjustedHeight.ToString() + "px" )
          buttonDiv.setAttribute("style", "position: absolute; top: " + adjustedHeight.ToString() + "px; left: " + "0" + "px; width: " + msg.width.ToString() + "px; height: " + "30" + "px" )
          blockly.svgResize( this.workspace :?> Blockly.WorkspaceSvg )
          ()

        member this.ActiveCellSerializedBlocksWorkspaceOption() : string option =
          if notebooks.activeCell <> null then
            //Get XML string if it exists
            let xmlStringOption = 
                let xmlStringComment = notebooks.activeCell.model.value.text.Split('\n') |> Array.last //xml is always the last line of cell
                if xmlStringComment.Contains("xmlns") then
                    xmlStringComment.TrimStart([| '#' |]) |> Some //remove comment marker
                else
                    None
            xmlStringOption
          else
            None

        /// Render blocks in workspace using xml. Defaults to xml present in active cell
        member this.RenderBlocks()  =
          if notebooks.activeCell <> null then
            //Get XML string if it exists
            let xmlStringOption = 
                let xmlStringComment = notebooks.activeCell.model.value.text.Split('\n') |> Array.last //xml is always the last line of cell
                if xmlStringComment.Contains("xmlns") then
                    xmlStringComment.TrimStart([| '#' |]) |> Some //remove comment marker
                else
                    None
            try
              // clearBlocks() //OLD: tight sync: cleared even if no xml
              match xmlStringOption with
              | Some(xmlString) -> 
                    clearBlocks()
                    Toolbox.decodeWorkspace ( xmlString )
                    // overwritten by logger callback
                    // this.blocksRendered <- true
                    Logging.LogToServer( Logging.JupyterLogEntry082720.Create "xml-to-blocks"  ( xmlString |> Some ) )
              | None -> ()
            with
            | e -> 
              Browser.Dom.window.alert("Unable to perform 'Code to Blocks': XML is either invald or renames existing variables. Specific error message is: " + e.Message )
              console.log ("unable to decode blocks: last line is invald xml")
          else
            console.log ("unable to decode blocks: active cell is null")

        /// Render blocks to code
        member this.RenderCode() =
          let code = generator.workspaceToCode (this.workspace)
          if notebooks.activeCell <> null then
            //prevent overwriting markdown
            if notebooks.activeCell.model |> JupyterlabCells.Model.Types.isMarkdownCellModel then
                // new alert: sloppy sync
                Browser.Dom.window.alert("You are calling 'Blocks to Code' on a MARKDOWN cell. Select an empty CODE cell and try again." )
           
                // old alert: tight sync
                // Browser.Dom.window.alert("You are calling 'Blocks to Code' on a MARKDOWN cell. Turn off notebook sync, select the CODE cell, turn sync back on, and try again." )
            else
              notebooks.activeCell.model.value.text <-
                  code //overwrite
                  // notebooks.activeCell.model.value.text + code //append 
                  + "\n#" + Toolbox.encodeWorkspace()  //workspace as comment
              console.log ("wrote to active cell:\n" + code + "\n")
              Logging.LogToServer( Logging.JupyterLogEntry082720.Create "blocks-to-code"  ( notebooks.activeCell.model.value.text |> Some) )
              this.blocksRendered <- true
          else
              console.log ("no cell active, flushed:\n" + code + "\n")

        /// Auto-save: Render blocks to code if we are on a code cell, we've previously saved to it, and have any blocks on the workspace
        member this.RenderCodeToLastCell() =
          let code = generator.workspaceToCode (this.workspace)
          if this.lastCell <> null then
            if this.lastCell.model <> null then
              if this.lastCell.model |> JupyterlabCells.Model.Types.isCodeCellModel then
                //check for existing XML comment as last line of code cell
                let alreadySerialized = try (this.lastCell.model.value.text.Split('\n') |> Array.last).Contains("xmlns") with _ -> false
                if alreadySerialized then
                  // let workspace = blockly.getMainWorkspace()
                  // let blocks = workspace.getAllBlocks(false)
                  let workspace = this.workspace
                  let blocks = this.workspace.getAllBlocks(false)
                  if blocks.Count > 0 then 
                    this.lastCell.model.value.text <-
                        code //overwrite
                        // notebooks.activeCell.model.value.text + code //append 
                        + "\n#" + Toolbox.encodeWorkspace()  //workspace as comment
                    console.log ("wrote to active cell:\n" + code + "\n")
                    Logging.LogToServer( Logging.JupyterLogEntry082720.Create "blocks-to-code-autosave"  ( notebooks.activeCell.model.value.text |> Some) )
                    //clearBlocks() //let these blocks float like any other
                // flag that we have written code
                // this.blocksWritten <- true
          else
              console.log ("no cell active, flushed instead of autosave:\n" + code + "\n")
    end
           
/// Return a MainAreaWidget wrapping a BlocklyWidget
let createMainAreaWidget( bw:BlocklyWidget)= 
  let w =
      JupyterlabApputils.Types.MainAreaWidget.Create
          (createObj [ "content" ==> bw ])
  w.id <- "blockly-jupyterlab"
  w.title.label <- "Blockly Palette"
  w.title.closable <- true
  //
  w

/// Attach a MainAreaWidget by splitting the viewing area and placing in the left hand pane, if possible
let attachWidget (app:JupyterlabApplication.JupyterFrontEnd<JupyterlabApplication.LabShell>) (notebooks:JupyterlabNotebook.Tokens.INotebookTracker) (widget : JupyterlabApputils.MainAreaWidget<obj>) =
  if not <| widget.isAttached then
    match notebooks.currentWidget with
    | Some(c) ->
       let options =
           jsOptions<JupyterlabDocregistry.Registry.DocumentRegistry.IOpenOptions> (fun o ->
               o.ref <- c.id |> Some
               o.mode <-
                   PhosphorWidgets.DockLayout.InsertMode.SplitLeft |> Some)
       c.context.addSibling (widget, options) |> ignore
    | None -> 
        //Forcing a left split when there is no notebook open results in partially broken behavior, so we must add to the main area
        app.shell.add (widget, "main") //"left", options)
  app.shell.activateById (widget.id) 

//Catch notebook changed event for enabling extension and attaching to left side when query string command is given
let runCommandOnNotebookChanged =
          PhosphorSignaling.Slot<JupyterlabApputils.IWidgetTracker<JupyterlabNotebook.Tokens.NotebookPanel>, JupyterlabNotebook.Tokens.NotebookPanel option>(fun sender args -> 
            match sender.currentWidget with
            | Some( notebook ) -> 
              // app.commands.execute(command) |> ignore
              console.log("notebook changed, autorunning blockly command")
              jsThis<JupyterlabApplication.JupyterFrontEnd<JupyterlabApplication.LabShell>>.commands.execute("blockly:open") |> ignore    
            | None -> ()
            //
            true
          )

//Start logging kernel messages if not already registered
let onKernelChanged = 
  PhosphorSignaling.Slot<JupyterlabApputils.IClientSession, JupyterlabServices.__session_session.Session.IKernelChangedArgs>(fun sender args -> 
    let widget = jsThis<BlocklyWidget> 
    if widget.notHooked then
      match sender.kernel with
      | Some(kernel) ->
          let ikernel = kernel :?> IKernel
          ikernel.iopubMessage.connect (widget.onKernelExecuted, widget ) |> ignore
          console.log ("Listening for kernel messages")
          widget.notHooked <- false
      | None -> ()
      //
      true
    else
      false
  )


/// On every notebook change, register to handle delayed kernel events that are not accessible at this time point
let onNotebookChanged =
          PhosphorSignaling.Slot<JupyterlabApputils.IWidgetTracker<JupyterlabNotebook.Tokens.NotebookPanel>, JupyterlabNotebook.Tokens.NotebookPanel option >(fun sender args -> 
            let blocklyWidget = jsThis<BlocklyWidget> //"this" not defined in promise context
            match sender.currentWidget with
            | Some( notebook ) -> 
              console.log("notebook changed to " + notebook.context.path )
              Logging.LogToServer( Logging.JupyterLogEntry082720.Create "notebook-changed"  ( notebook.context.path |> Some) )
              notebook.session.kernelChanged.connect( onKernelChanged, blocklyWidget ) |> ignore
            | None -> ()
            //
            true
          )

/// The extension
let extension =
    createObj
        [ "id" ==> "jupyterlab_blockly_extension"
          "autoStart" ==> true
          "requires" ==> requires //
          //------------------------------------------------------------------------------------------------------------
          //NOTE: this **must** be wrapped in a Func, otherwise the arguments are tupled and Jupyter doesn't expect that
          //------------------------------------------------------------------------------------------------------------
          "activate" ==> System.Func<JupyterlabApplication.JupyterFrontEnd<JupyterlabApplication.LabShell>, JupyterlabApputils.ICommandPalette, JupyterlabNotebook.Tokens.INotebookTracker, JupyterlabApplication.ILayoutRestorer, unit>(fun app palette notebooks restorer ->
                            console.log ("JupyterLab extension jupyterlab_blockly_extension is activated!")
                            
                            //Create a blockly widget and place inside main area widget
                            let blocklyWidget = BlocklyWidget(notebooks)
                            let mutable widget = createMainAreaWidget(blocklyWidget)

                            //Add application command to display
                            let command = "blockly:open"

                            //Set up widget tracking to restore state
                            let tracker = JupyterlabApputils.Types.WidgetTracker.Create(!!createObj [ "namespace" ==> "blockly" ])
                            restorer.restore(tracker, !!createObj [ "command" ==> command; "name" ==> fun () -> "blockly" ]) |> ignore

                            //wait until a notebook is displayed to hook kernel messages
                            notebooks.currentChanged.connect( onNotebookChanged, blocklyWidget ) |> ignore

                            //Prepare launch command for the command palette
                            app.commands.addCommand
                                (command,
                                 createObj
                                      [ 
                                        "label" ==> "Blockly Jupyterlab Extension"
                                        "execute" ==> fun () ->

                                          //Recreate the widget if the user previously closed it
                                          if widget = null || widget.isDisposed then
                                            widget <- createMainAreaWidget(blocklyWidget)

                                          //Attach the widget to the UI in a smart way
                                          widget |> attachWidget app notebooks

                                          //Track the widget to restore its state if the user does a refresh
                                          if not <| tracker.has(widget) then
                                            tracker.add(widget) |> ignore
      
                                      ] :?> PhosphorCommands.CommandRegistry.ICommandOptions)
                            |> ignore

                            //Add command to palette
                            palette?addItem (createObj
                                                 [ "command" ==> command
                                                   "category" ==> "Blockly" ])
                            
                            //If query string has bl=1, trigger the open command once the application is ready
                            let searchParams = Browser.Url.URLSearchParams.Create(  Browser.Dom.window.location.search )
                            match searchParams.get("bl") with
                            | Some(state) when state = "1" ->
                              console.log ("Blockly extension triggering open command based on query string input")
                              app.restored.``then``(fun _ -> 
                                //wait until a notebook is displayed so we dock correctly (e.g. nbgitpuller deployment)
                                //NOTE: workspaces are stateful, so the notebook must be closed, then openned in the workspace for this to fire
                                notebooks.currentChanged.connect( runCommandOnNotebookChanged, app ) |> ignore
                                //app.commands.execute(command) |> ignore
                                widget.title.closable <- false //do not allow blockly to be closed
                                ) |> ignore  
                            | _ -> ()

                            //If query string has id=xxx, store this identifier as a participant id
                            match searchParams.get("id") with
                            | Some(id) -> Logging.idOption <- Some(id)
                            | _ -> ()

                            //If query string has log=xxx, use this at the logging endpoint
                            //must include http/https in url
                            match searchParams.get("log") with
                            | Some(logUrl) -> Logging.logUrl <- Some(logUrl)
                            | _ -> ()

                          ) //Func
        ]
exportDefault extension
