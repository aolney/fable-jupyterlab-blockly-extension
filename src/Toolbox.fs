module Toolbox

open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.DynamicExtensions
open Blockly

//-----------------------------------------------------
// TODO: 
// 1. ask re block init situation
// 2. refactor dynamic operators into types
//-----------------------------------------------------

// /// Stub method leading to generation of blocks by querying kernel for completions
// let GetCompletion() =
//     "hi"

// /// Stub method leading to generation of blocks by querying kernel for tooltips/introspection
// let GetTooltip() =
//     "bye"

// It seemed necessary to subclass "Block" in order to add an "init" function and any other state variables we might need
// Note this is not necessary for blocks that only have JSON definitions and no mutation, which could be loaded with "defineBlocksWithJsonArray"
// However, this fails, see below

// [<Import("Block", from="blockly")>]
// [<AbstractClass>]
// type Block() =
//   class
//     // example member implementation (if something wasn't defined in imported class)
//     //member val node : HTMLElement = null with get, set
//     //---------------------------------
//     // all abstract methods we will use/must use
//     abstract init : unit -> unit
//   end

// type ImportBlock() as this =
//   class
//     inherit Block()
//     override this.init() =
//       let thisBlock : Blockly.Block = unbox this //TODO need a better way
//       thisBlock.appendDummyInput()
//         .appendField( !^"import"  )
//         .appendField( !^(blockly.FieldTextInput__Class.Create("some library") :?> Blockly.Field), "libraryName"  )
//         .appendField( !^"as") 
//         .appendField( !^(blockly.FieldTextInput__Class.Create("variable name") :?> Blockly.Field), "libraryAlias"  ) |> ignore
//       thisBlock.setNextStatement true
//       thisBlock.setColour !^230.0
//       thisBlock.setTooltip !^"Import a python package to access functions in that package"
//       thisBlock.setHelpUrl !^"https://docs.python.org/3/reference/import.html"
//   end

//using dynamic extensions for these assignments
// blockly?Blocks.["import"] <- ImportBlock() // FAILS: seems to call the Block contrustor and fail b/c no workspace is passed in and "blockDB_" is therefore undefined

//trying to define block without explicitly calling constructor as above...
// Attempt 1: use jsOptions
// the below won't compile: The address of a value returned from the expression cannot be used at this point. This is to ensure the address of the local value does not escape its scope.F# Compiler(3228)
// blockly?Blocks.["import"] <- jsOptions<Blockly.Block>(fun o -> o.init <- fun _ -> ()  ) 
// NOTE: it doesn't matter if we define an outside function and pass it in to jsOptions; jsOptions does not like function definitions like this.
// lOOKS LIKE YOU CAN ONLY USE JSOPTIONS FOR SETTING CLASS MEMBERS, NOT FOR CALLING FUNCTIONS AND MAYBE NOT FOR DEFINING THEM
//blockly?Blocks.["import"] <- jsOptions<Blockly.Block>(fun o -> o.setTooltip !^"Import a python package to access functions in that package" )  //THIS COMPLIES BUT THROWS RUNTIME ERROR "TypeError: o.setTooltip is not a function"


//scraps from attempting to use jsOptions  
  // o.init <- fun _ -> 
      // this.appendDummyInput()
      //   .appendField( !^"import"  )
      //   .appendField( !^(blockly.FieldTextInput__Class.Create("some library") :?> Blockly.Field), "libraryName"  )
      //   .appendField( !^"as") 
      //   .appendField( !^(blockly.FieldTextInput__Class.Create("variable name") :?> Blockly.Field), "libraryAlias"  ) |> ignore
      // this.setNextStatement true
      // this.setColour !^230.0
      // this.setTooltip !^"Import a python package to access functions in that package"
      // this.setHelpUrl !^"https://docs.python.org/3/reference/import.html"
  //)


/// Emit as "this" as an interop workaround
[<Emit("this")>]
let thisBlock : Blockly.Block = jsNative

/// Create a Blockly/Python import block
blockly?Blocks.["import"] <- createObj [
  "init" ==> fun () -> 
    // Browser.Dom.console.log("did import block init")
    thisBlock.appendDummyInput()
      .appendField( !^"import"  )
      .appendField( !^(blockly.FieldTextInput.Create("some library") :?> Blockly.Field), "libraryName"  )
      .appendField( !^"as")
      .appendField( !^(blockly.FieldVariable.Create("variable name") :?> Blockly.Field), "libraryAlias"  ) |> ignore
    thisBlock.setNextStatement true
    thisBlock.setColour !^230.0
    thisBlock.setTooltip !^"Import a python package to access functions in that package"
    thisBlock.setHelpUrl !^"https://docs.python.org/3/reference/import.html"
  ]
/// Generate Python import code
blockly?Python.["import"] <- fun (block : Blockly.Block) -> 
  let libraryName = block.getFieldValue("libraryName").Value |> string
  let libraryAlias = blockly?Python?variableDB_?getName( block.getFieldValue("libraryAlias").Value |> string, blockly?Variables?NAME_TYPE);
  let code =  "import " + libraryName + " from " + libraryAlias
  code

/// A template for block creation, including the code generator.
let makeSingleArgFunctionBlock blockName (label:string) (outputType:string) (tooltip:string) (helpurl:string) (functionStr:string) =
  blockly?Blocks.[blockName] <- createObj [
    "init" ==> fun () -> 
      Browser.Dom.console.log( blockName + " init")
      thisBlock.appendValueInput("x")
        .setCheck(!^None)
        .appendField(!^label) |> ignore
      thisBlock.setInputsInline(true)
      thisBlock.setOutput(true, !^outputType)
      thisBlock.setColour(!^230.0)
      thisBlock.setTooltip !^tooltip
      thisBlock.setHelpUrl !^helpurl
    ]
  /// Generate Python bool conversion code
  blockly?Python.[blockName] <- fun (block : Blockly.Block) -> 
    let x = blockly?Python?valueToCode( block, "x", blockly?Python?ORDER_ATOMIC )
    let code =  functionStr + "(" + x + ")"
    [| code; blockly?Python?ORDER_FUNCTION_CALL |]

// Conversion blocks, e.g. str()
makeSingleArgFunctionBlock 
  "boolConversion"
  "as bool"
  "Boolean"
  "Convert something to Boolean."
  "https://docs.python.org/3/library/stdtypes.html#boolean-values"
  "bool"

makeSingleArgFunctionBlock
  "strConversion"
  "as str"
  "String"
  "Convert something to String."
  "https://docs.python.org/3/library/stdtypes.html#str"
  "str"

makeSingleArgFunctionBlock
  "floatConversion"
  "as float"
  "Float"
  "Convert something to Float."
  "https://docs.python.org/3/library/functions.html#float"
  "float"

makeSingleArgFunctionBlock
  "intConversion"
  "as int"
  "Int"
  "Convert something to Int."
  "https://docs.python.org/3/library/functions.html#int"
  "int"

// Get user input, e.g. input()
makeSingleArgFunctionBlock
  "getInput"
  "input"
  "String"
  "Present the given prompt to the user and wait for their typed input response."
  "https://docs.python.org/3/library/functions.html#input"
  "input"

//TODO: REFACTOR TO MAKE A FUNCTION FOR BOOL, int, float, string, BLOCKS AND GENERATOR
// PROBABLY IS GENERALIZABLE TO ARBITRARY FUNCTION CALLS
// THEN WORK DOWN LIST IN 'JUNK' TO CREATE OTHER BLOCKLY BLOCKS AND CORRECT NAMES FOR
// THEM IN TOOLBOX

/// A static toolbox copied from one of Google's online demos at https://blockly-demo.appspot.com/static/demos/index.html
/// Curiously category names like "%{BKY_CATLOGIC}" not resolved by Blockly, even though the colors are, so names 
/// are replaced with English strings below
let toolbox =
    """<xml xmlns="https://developers.google.com/blockly/xml" id="toolbox" style="display: none">
    <category name="IMPORT" colour="255">
      <block type="import"></block>
    </category>
    <category name="LOGIC" colour="%{BKY_LOGIC_HUE}">
      <block type="controls_if"></block>
      <block type="logic_compare"></block>
      <block type="logic_operation"></block>
      <block type="logic_negate"></block>
      <block type="logic_boolean"></block>
      <block type="logic_null"></block>
      <block type="logic_ternary"></block>
    </category>
    <category name="LOOPS" colour="%{BKY_LOOPS_HUE}">
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
    <category name="MATH" colour="%{BKY_MATH_HUE}">
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
    <category name="TEXT" colour="%{BKY_TEXTS_HUE}">
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
      <block type="getInput">
        <value name="x">
          <shadow type="text">
            <field name="TEXT">The prompt shown to the user</field>
          </shadow>
        </value>
      </block>
    </category>
    <category name="LISTS" colour="%{BKY_LISTS_HUE}">
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
    <category name="COLOUR" colour="%{BKY_COLOUR_HUE}">
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
      </block>Conversion
    </category>
    <category name="CONVERSION" colour="120">
      <block type="boolConversion">
      </block>
      <block type="intConversion">
      </block>
      <block type="floatConversion">
      </block>
      <block type="strConversion">
      </block>
    </category>
    <sep></sep>
    <category name="VARIABLES" colour="%{BKY_VARIABLES_HUE}" custom="VARIABLE"></category>
    <category name="FUNCTIONS" colour="%{BKY_PROCEDURES_HUE}" custom="PROCEDURE"></category>
  </xml>"""


  // <!-- From BlockPY 
  //           </category>
  //       <category name="Dictionaries" colour="${BlockMirrorTextToBlocks.COLOR.DICTIONARY}">
  //           <block type="ast_Dict">
  //               <mutation items="3"></mutation>
  //               <value name="ADD0"><block type="ast_DictItem" deletable="false" movable="false">
  //                 <value name="KEY">
  //                   <shadow type="text">
  //                     <field name="TEXT">key1</field>
  //                   </shadow>
  //                 </value>
  //               </block></value>
  //               <value name="ADD1"><block type="ast_DictItem" deletable="false" movable="false">
  //                 <value name="KEY">
  //                   <shadow type="text">
  //                     <field name="TEXT">key2</field>
  //                   </shadow>
  //                 </value>
  //               </block></value>
  //               <value name="ADD2"><block type="ast_DictItem" deletable="false" movable="false">
  //                   <!-- <value name="KEY"><block type="ast_Str"><field name="TEXT">3rd key</field></block></value> -->
  //                 <value name="KEY">
  //                   <shadow type="text">
  //                     <field name="TEXT">key3</field>
  //                   </shadow>
  //                 </value>
  //               </block></value>
  //           </block>
  //           <block type="ast_Subscript">
  //               <mutation><arg name="I"></arg></mutation>
  //               <value name="INDEX0"><block type="ast_Str"><field name="TEXT">key</field></block></value>
  //           </block>
  //       </category>
  //    End from BlockPY -->


  //          <!-- From BlockPY 
  //     <block xmlns="http://www.w3.org/1999/xhtml" type="ast_Call" line_number="null" inline="true">
  //       <mutation arguments="1" returns="false" parameters="true" method="true" name=".append" message="append" premessage="to list" colour="30" module="">
  //         <arg name="UNKNOWN_ARG:0"></arg>
  //       </mutation>
  //     </block>
  //     <block xmlns="http://www.w3.org/1999/xhtml" type="ast_Call" line_number="null" inline="true">
  //       <mutation arguments="1" returns="true" parameters="true" method="false" name="range" message="range" premessage="" colour="15" module="">
  //         <arg name="UNKNOWN_ARG:0"></arg>
  //       </mutation>
  //       <value name="NUM">
  //         <shadow type="math_number">
  //           <field name="NUM">0</field>
  //         </shadow>
  //       </value>
  //       </block>
  //     End from BlockPY -->