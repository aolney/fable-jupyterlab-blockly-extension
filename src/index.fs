module App

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Dom

//sugar for creating js objects
let inline (!!) x = createObj x
let inline (=>) x y = x ==> y

//hack here: if we try to make collection of requires, F# complains they are different types unless we specify obj type
let mutable requires : obj array = [| JupyterlabApputils.ICommandPalette; JupyterlabNotebook.Tokens.Types.INotebookTracker |]

//TODO: hack b/c I haven't dug through the imports to understand the `requires` semantics
// [<ImportMember("@jupyterlab/apputils")>]
// let ICommandPalette : obj = jsNative

let extension =
    !! [
        "id" => "jupyterlab_blockly_extension"
        "autoStart" => true
        "requires" => requires // 
        //------------------------------------------------------------------------------------------------------------
        //NOTE: this **must** be wrapped in a Func, otherwise the arguments are tupled and Jupyter doesn't expect that
        //------------------------------------------------------------------------------------------------------------
        "activate" =>  System.Func<JupyterlabApplication.JupyterFrontEnd,JupyterlabApputils.ICommandPalette,JupyterlabNotebook.Tokens.INotebookTracker,unit>( fun app palette notebooks ->
            console.log("JupyterLab extension jupyterlab_blockly_extension is activated!");
            // Create a Blockly content widget inside of a MainAreaWidget
            // let content = PhosphorWidgets.Widget.Create();
            let content = PhosphorWidgets.Types.Widget.Create();
            let notebook = notebooks.ToString()

            //FIND ACTIVE CELL CODE
            //let current = notebooks.currentWidget;
            // let content = 
            //     if current.IsSome then
            //         let nb = current.content.Value;
            //         let model = nb.activeCell.model;
                    //THIS IS WHAT WE WANT TO REPLACE WITH BLOCKLY
                    //let content = await factory.createWidget({model});

            // let widget = JupyterlabApputils.MainAreaWidget.Create( !![ "content" => content ]);
            let widget = JupyterlabApputils.Types.MainAreaWidget.Create( !![ "content" => content ]) 
            widget.id <- "blockly-jupyterlab";
            widget.title.label <- "Blockly Palette";
            widget.title.closable <- true

            //THIS IS WHAT WE WANT TO REPLACE WITH BLOCKLY INJECTION
            //BLOCKLY SHOULD TRIGGER THE FIND ACTIVE CELL CODE ABOVE WHEN TRIGGERED
            //alternatively we could append blockly to main area widget
            // Add html text to the widget
            let p = Browser.Dom.document.createElement("p")
            p.setAttribute("style","font-size:72px")
            p.innerText <- "Hello from Blockly"
            content.node.appendChild(p) |> ignore

            // Add application command
            let command = "blockly:open"
            //TODO: using dynamic (?) b/c the imports aren't fully implemented
            app.commands?addCommand( command, 
                !![
                    "label" => "Blockly Jupyterlab Extension"
                    "execute" => fun () -> 
                        if not <| widget.isAttached then
                            app.shell?add(widget, "main")
                        app.shell?activateById(widget.id)
                ])
            //Add command to palette
            palette?addItem(
                !![
                    "command" => command
                    "category" => "Blockly"
                ]
            )
        )
 
    ]

exportDefault extension