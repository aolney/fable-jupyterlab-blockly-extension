module App

// open FSharp.Core
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Types
open Browser.Dom
open Blockly
open MutationObserver

//sugar for creating js objects
let inline private (~~) x = createObj x
let inline private (=>) x y = x ==> y

//hack here: if we try to make collection of requires, F# complains they are different types unless we specify obj type
let mutable requires : obj array = [| JupyterlabApputils.ICommandPalette; JupyterlabNotebook.Tokens.Types.INotebookTracker |]

//TODO: move toolbox somewhere?
let toolbox =
    """<xml xmlns="https://developers.google.com/blockly/xml" id="toolbox" style="display: none">
    <category name="%{BKY_CATLOGIC}" colour="%{BKY_LOGIC_HUE}">
      <block type="controls_if"></block>
      <block type="logic_compare"></block>
      <block type="logic_operation"></block>
      <block type="logic_negate"></block>
      <block type="logic_boolean"></block>
      <block type="logic_null"></block>
      <block type="logic_ternary"></block>
    </category>
    <category name="%{BKY_CATLOOPS}" colour="%{BKY_LOOPS_HUE}">
      <block type="controls_repeat_ext">
        <value name="TIMES">
          <shadow type="math_number">
            <field name="NUM">10</field>
          </shadow>
        </value>
      </block>
      <block type="controls_whileUntil"></block>
      <block type="controls_for">
        <value name="FROM">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
        <value name="TO">
          <shadow type="math_number">
            <field name="NUM">10</field>
          </shadow>
        </value>
        <value name="BY">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
      </block>
      <block type="controls_forEach"></block>
      <block type="controls_flow_statements"></block>
    </category>
    <category name="%{BKY_CATMATH}" colour="%{BKY_MATH_HUE}">
      <block type="math_number">
        <field name="NUM">123</field>
      </block>
      <block type="math_arithmetic">
        <value name="A">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
        <value name="B">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
      </block>
      <block type="math_single">
        <value name="NUM">
          <shadow type="math_number">
            <field name="NUM">9</field>
          </shadow>
        </value>
      </block>
      <block type="math_trig">
        <value name="NUM">
          <shadow type="math_number">
            <field name="NUM">45</field>
          </shadow>
        </value>
      </block>
      <block type="math_constant"></block>
      <block type="math_number_property">
        <value name="NUMBER_TO_CHECK">
          <shadow type="math_number">
            <field name="NUM">0</field>
          </shadow>
        </value>
      </block>
      <block type="math_round">
        <value name="NUM">
          <shadow type="math_number">
            <field name="NUM">3.1</field>
          </shadow>
        </value>
      </block>
      <block type="math_on_list"></block>
      <block type="math_modulo">
        <value name="DIVIDEND">
          <shadow type="math_number">
            <field name="NUM">64</field>
          </shadow>
        </value>
        <value name="DIVISOR">
          <shadow type="math_number">
            <field name="NUM">10</field>
          </shadow>
        </value>
      </block>
      <block type="math_constrain">
        <value name="VALUE">
          <shadow type="math_number">
            <field name="NUM">50</field>
          </shadow>
        </value>
        <value name="LOW">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
        <value name="HIGH">
          <shadow type="math_number">
            <field name="NUM">100</field>
          </shadow>
        </value>
      </block>
      <block type="math_random_int">
        <value name="FROM">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
        <value name="TO">
          <shadow type="math_number">
            <field name="NUM">100</field>
          </shadow>
        </value>
      </block>
      <block type="math_random_float"></block>
      <block type="math_atan2">
        <value name="X">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
        <value name="Y">
          <shadow type="math_number">
            <field name="NUM">1</field>
          </shadow>
        </value>
      </block>
    </category>
    <category name="%{BKY_CATTEXT}" colour="%{BKY_TEXTS_HUE}">
      <block type="text"></block>
      <block type="text_join"></block>
      <block type="text_append">
        <value name="TEXT">
          <shadow type="text"></shadow>
        </value>
      </block>
      <block type="text_length">
        <value name="VALUE">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
      <block type="text_isEmpty">
        <value name="VALUE">
          <shadow type="text">
            <field name="TEXT"></field>
          </shadow>
        </value>
      </block>
      <block type="text_indexOf">
        <value name="VALUE">
          <block type="variables_get">
            <field name="VAR">{textVariable}</field>
          </block>
        </value>
        <value name="FIND">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
      <block type="text_charAt">
        <value name="VALUE">
          <block type="variables_get">
            <field name="VAR">{textVariable}</field>
          </block>
        </value>
      </block>
      <block type="text_getSubstring">
        <value name="STRING">
          <block type="variables_get">
            <field name="VAR">{textVariable}</field>
          </block>
        </value>
      </block>
      <block type="text_changeCase">
        <value name="TEXT">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
      <block type="text_trim">
        <value name="TEXT">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
      <block type="text_print">
        <value name="TEXT">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
      <block type="text_prompt_ext">
        <value name="TEXT">
          <shadow type="text">
            <field name="TEXT">abc</field>
          </shadow>
        </value>
      </block>
    </category>
    <category name="%{BKY_CATLISTS}" colour="%{BKY_LISTS_HUE}">
      <block type="lists_create_with">
        <mutation items="0"></mutation>
      </block>
      <block type="lists_create_with"></block>
      <block type="lists_repeat">
        <value name="NUM">
          <shadow type="math_number">
            <field name="NUM">5</field>
          </shadow>
        </value>
      </block>
      <block type="lists_length"></block>
      <block type="lists_isEmpty"></block>
      <block type="lists_indexOf">
        <value name="VALUE">
          <block type="variables_get">
            <field name="VAR">{listVariable}</field>
          </block>
        </value>
      </block>
      <block type="lists_getIndex">
        <value name="VALUE">
          <block type="variables_get">
            <field name="VAR">{listVariable}</field>
          </block>
        </value>
      </block>
      <block type="lists_setIndex">
        <value name="LIST">
          <block type="variables_get">
            <field name="VAR">{listVariable}</field>
          </block>
        </value>
      </block>
      <block type="lists_getSublist">
        <value name="LIST">
          <block type="variables_get">
            <field name="VAR">{listVariable}</field>
          </block>
        </value>
      </block>
      <block type="lists_split">
        <value name="DELIM">
          <shadow type="text">
            <field name="TEXT">,</field>
          </shadow>
        </value>
      </block>
      <block type="lists_sort"></block>
    </category>
    <category name="%{BKY_CATCOLOUR}" colour="%{BKY_COLOUR_HUE}">
      <block type="colour_picker"></block>
      <block type="colour_random"></block>
      <block type="colour_rgb">
        <value name="RED">
          <shadow type="math_number">
            <field name="NUM">100</field>
          </shadow>
        </value>
        <value name="GREEN">
          <shadow type="math_number">
            <field name="NUM">50</field>
          </shadow>
        </value>
        <value name="BLUE">
          <shadow type="math_number">
            <field name="NUM">0</field>
          </shadow>
        </value>
      </block>
      <block type="colour_blend">
        <value name="COLOUR1">
          <shadow type="colour_picker">
            <field name="COLOUR">#ff0000</field>
          </shadow>
        </value>
        <value name="COLOUR2">
          <shadow type="colour_picker">
            <field name="COLOUR">#3333ff</field>
          </shadow>
        </value>
        <value name="RATIO">
          <shadow type="math_number">
            <field name="NUM">0.5</field>
          </shadow>
        </value>
      </block>
    </category>
    <sep></sep>
    <category name="%{BKY_CATVARIABLES}" colour="%{BKY_VARIABLES_HUE}" custom="VARIABLE"></category>
    <category name="%{BKY_CATFUNCTIONS}" colour="%{BKY_PROCEDURES_HUE}" custom="PROCEDURE"></category>
  </xml>"""

let toolbox2 = """<xml><block type="controls_if"></block><block type="controls_whileUntil"></block></xml>"""

//https://stackoverflow.com/questions/47640263/how-to-extend-a-js-class-in-fable
[<Import("Widget", from="@phosphor/widgets")>]
[<AbstractClass>]
type Widget() =
  class
    // only implementing the minimal members needed
    member val node : HTMLElement = null with get, set
    abstract onAfterShow : unit -> unit
  end

/// I don't think its strictly necessary to wrap blockly in a widget
type BlocklyWidget(notebooks:JupyterlabNotebook.Tokens.INotebookTracker) as this =
  class
    inherit Widget()
    let generator = blockly.Generator.Create("Python")
    do
        // //set language to English
        // let en = Blockly.en
        // blockly.setLocale(en)

        // //for debug
        // let msg = Blockly.msg.TEXT_APPEND_VARIABLE
        // console.log( "msg is: " + msg )
        let pyGen =  blockly.Generator.Create("Python")
        let genString = pyGen.prefixLines("hi\nbye","\t")
        console.log( "gen string is: " + genString ) 
        

        //div to hold blockly
        let div = document.createElement("div")
        div.setAttribute("style", "height: 480px; width: 600px;")
        div.id <- "blocklyDiv" //for debug and to refer to during injection
        this.node.appendChild(div) |> ignore

        //button to trigger code generation
        let button = document.createElement("button")
        button.innerText <- "Generate"
        button.addEventListener("click", fun _ ->
          this.RenderCode()
        )
        this.node.appendChild(button) |> ignore

    
    /// Wait until widget shown to prevent injection from taking place before the DOM is ready
    /// Inject blockly into div and save blockly workspace to private member 
    override this.onAfterShow() = 
      // console.log("after show happened")
      this.workspace <-
        blockly.inject(
            !^"blocklyDiv",
            // Tricky: creatObj cannot be used here. Must use jsOptions to create POJO
            jsOptions<Blockly.BlocklyOptions>(fun o ->
                o.toolbox <- !^toolbox2 |> Some
            )
            // THIS FAILS!
            // ~~ [
            //     "toolbox" => ~~ [ "toolbox" => toolbox2 ] //TODO: using toolbox2 same as using empty string here
            //     // "media" => "media/" //TODO: see other configuration options; media folder in downloads
            // ] :?> Object
        ) 
      console.log("blockly palette initialized")
    member this.Notebooks = notebooks
    member this.RenderCode() =             
      let code = generator.workspaceToCode( this.workspace )
      if notebooks.activeCell <> null then
        notebooks.activeCell.model.value.text <- notebooks.activeCell.model.value.text  + code //could also overwrite
        console.log("wrote to active cell:" + code)
      else
        console.log("no cell active, flushed:" + code)
    member val workspace : Blockly.Workspace = null with get, set
  end

let extension =
    ~~ [
        "id" => "jupyterlab_blockly_extension"
        "autoStart" => true
        "requires" => requires // 
        //------------------------------------------------------------------------------------------------------------
        //NOTE: this **must** be wrapped in a Func, otherwise the arguments are tupled and Jupyter doesn't expect that
        //------------------------------------------------------------------------------------------------------------
        "activate" =>  System.Func<JupyterlabApplication.JupyterFrontEnd<JupyterlabApplication.LabShell>,JupyterlabApputils.ICommandPalette,JupyterlabNotebook.Tokens.INotebookTracker,unit>( fun app palette notebooks ->
            console.log("JupyterLab extension jupyterlab_blockly_extension is activated!");
      
            //Create a blockly widget and place inside main area widget
            let blocklyWidget = BlocklyWidget(notebooks)
            let widget = JupyterlabApputils.Types.MainAreaWidget.Create( ~~[ "content" => blocklyWidget ]) 
            widget.id <- "blockly-jupyterlab";
            widget.title.label <- "Blockly Palette";
            widget.title.closable <- true

            // Add application command
            let command = "blockly:open"
            app.commands.addCommand( command, 
                ~~ [
                    "label" => "Blockly Jupyterlab Extension"
                    "execute" => fun () -> 
                        if not <| widget.isAttached then
                            app.shell.add(widget, "main") 
                        app.shell.activateById(widget.id)
                ] :?> PhosphorCommands.CommandRegistry.ICommandOptions
            ) |> ignore
            //Add command to palette
            palette?addItem(
                ~~[
                    "command" => command
                    "category" => "Blockly"
                ]
            )
        )
 
    ]

exportDefault extension