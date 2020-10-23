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

// /// id to self explanation map
// let selfExplanationState = System.Collections.Generic.Dictionary<string,string>()

// /// Log self explanation and save its state
// let logSelfExplanation( text : string) ( id : string ) =
//   Logging.LogToServer( Logging.JupyterLogEntry082720.Create "self-explanation" ( text |> Some ) ) 
//   selfExplanationState.Add( id, text)

// /// Simplest way to connect javascript injected into code cell output to F#: make a global function in node
// let [<Global>] ``global`` : obj = jsNative
// ``global``?logSelfExplanation <- logSelfExplanation

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
            let checkboxLabel = document.createElement ("label")
            checkboxLabel.innerText <- "Notebook Sync"
            checkboxLabel.setAttribute("for", "syncCheckbox")
            buttonDiv.appendChild( syncCheckbox) |> ignore
            buttonDiv.appendChild( checkboxLabel) |> ignore

            //append all buttons in div
            this.node.appendChild ( buttonDiv ) |> ignore

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

            // //SELF EXPLANATION: probe all cells b/c listening for kernel messages requires waiting to attach (we could poll instead...)
            // // check ALL code cell outputs every time a cell changes
            // let cells = this.Notebooks.currentWidget.Value.content.widgets
            // for i = 0 to cells.length - 1 do
            //   let cell = cells.[i]
            //   if cell.model.``type`` = JupyterlabCoreutils.Nbformat.Nbformat.CellType.Code then
            //     console.log ("I am a code cell")
            //     let codeCell = cell :?> JupyterlabCells.Widget.CodeCell

            //     //check if our self-explanation response box is already present; don't create duplicates!
            //     let hasResposeBox =
            //       [| 0.0 .. codeCell.outputArea.model.length - 1.0 |] 
            //       |> Array.exists( fun i ->
            //         let model = codeCell.outputArea.model.get(i) 
            //         let html = model.data.["text/html"] |> unbox<string>
            //         html <> null && html.Contains("self-explanation") //below we enforce that <textarea> has an id containing the string "self-explanation"
            //       )
                  
            //     if not <| hasResposeBox then
            //       //conveniently we have a unique persistent id
            //       let modelId = codeCell.model.id
            //       //retrieve the stored self-explanation if it exists
            //       let selfExplanation = 
            //         match selfExplanationState.TryGetValue(modelId) with
            //           | true,se -> se //we have a stored self explanation
            //           | false,_ -> "" //nothing stored

            //       //if self explanation exists, display it in black; else display an empty textarea with red font
            //       let displayData = //
            //         createObj 
            //           [
            //             "output_type" ==> "display_data"
            //             //inject the modelId into the textarea id; insert the stored self explanation if it exists; creating a logging handler with this information
            //             "data" ==> createObj [ "text/html" ==> """<div style='display:inline-block;vertical-align: top;'><textarea id='self-explanation""" + modelId +  """' cols='60' rows='2'""" + (if selfExplanation = "" then " style='color:Tomato;'" else "") + ">" + selfExplanation  + """</textarea></div><div style='display:inline-block;vertical-align: top;'><button onclick="document.getElementById('self-explanation""" + modelId + """').style.color = 'black';logSelfExplanation(document.getElementById('self-explanation""" + modelId + """').value,'""" + modelId + """')">Enter</button></div>""" ]
            //           ] :?> JupyterlabCoreutils.Nbformat.Nbformat.IOutput
              
            //       codeCell.outputArea.model.add( displayData ) |> ignore
            //       // console.log("I added a self explanation a code cell's outputarea")
            
            // //end self-explanation
            // //*****************************************

            //log the active cell change
            if args <> null then
              // console.log("I changed cells!")
              Logging.LogToServer( Logging.JupyterLogEntry082720.Create "active-cell-change" ( args.node.outerText |> Some ) ) //None )
              let syncCheckbox = document.getElementById("syncCheckbox")
              let (isChecked : bool) = (syncCheckbox <> null) && syncCheckbox?``checked`` |> unbox //checked is a f# reserved keyword
              if isChecked && notebooks.activeCell <> null then
                //if selected cell empty, clear the workspace
                if notebooks.activeCell.model.value.text.Trim() = "" then
                  // blockly.getMainWorkspace().clear() //to avoid duplicates
                  clearBlocks()
                //otherwise try to to create blocks from cell contents (fails gracefully)
                else
                  this.RenderBlocks()
                //Update intellisense on blocks we just created
                Toolbox.UpdateAllIntellisense()
            true
           )

        /// Wait until widget shown to prevent injection from taking place before the DOM is ready
        /// Inject blockly into div and save blockly workspace to private member
        override this.onAfterAttach() =
            //try to register for code execution messages
            if this.notHooked then
                match this.Notebooks.currentWidget with
                | Some(widget) ->
                    match widget.session.kernel with
                    | Some(kernel) ->
                        let ikernel = kernel :?> IKernel
                        ikernel.iopubMessage.connect (this.onKernelExecuted, this) |> ignore
                        console.log ("Listening for kernel messages")
                        this.notHooked <- false
                    | None -> ()
                | None -> ()

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
              //for ui, get fine grain type; for var, get varId; everything else is event type and block id
              //a bit ugly b/c fable will not allow type tests against foreign interface: https://github.com/fable-compiler/Fable/issues/1580
              let evt,id =
                //ui and block change have `element` ; var changes have `varId`
                match e?element=null,e?varId=null with
                | true,true -> e?``type``,e?blockId
                | false,true -> e?element,e?blockId
                | true,false -> e?``type``,e?varId
                | false,false -> e?``type``,e?blockId //this is impossible; take default case
              Logging.LogToServer( Logging.BlocklyLogEntry082720.Create evt id )
            )
            //check if logging should occur
            Logging.CheckShouldLog()
            if Logging.shouldLog then 
              console.log ("!!! Logging select user actions to server !!!")
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
              clearBlocks()
              match xmlStringOption with
              | Some(xmlString) -> 
                    Toolbox.decodeWorkspace ( xmlString )
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
                Browser.Dom.window.alert("You are calling 'Blocks to Code' on a MARKDOWN cell. Turn off notebook sync, select the CODE cell, turn sync back on, and try again." )
            else
              notebooks.activeCell.model.value.text <-
                  code //overwrite
                  // notebooks.activeCell.model.value.text + code //append 
                  + "\n#" + Toolbox.encodeWorkspace()  //workspace as comment
              console.log ("wrote to active cell:\n" + code + "\n")
              Logging.LogToServer( Logging.JupyterLogEntry082720.Create "blocks-to-code"  ( notebooks.activeCell.model.value.text |> Some) )
          else
              console.log ("no cell active, flushed:\n" + code + "\n")

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
    | None -> app.shell.add (widget, "main")
  app.shell.activateById (widget.id) 


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
                                                   

                          ) //Func
        ]
exportDefault extension
