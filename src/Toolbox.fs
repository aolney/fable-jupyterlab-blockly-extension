module Toolbox

open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.DynamicExtensions
open Browser.Types
open Blockly
open JupyterlabServices.__kernel_messages.KernelMessage

//TODO: 
// - make "read file" have shadow text block rather than input field
// - on list comprehension, have a "when" to add filtering conditions

//TODO: ask fable about using jsOptions to define functions
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

/// Emit "this" typed to Block as an interop workaround
[<Emit("this")>]
let thisBlock : Blockly.Block = jsNative

/// Emit "this" untyped as an interop workaround
[<Emit("this")>]
let thisObj : obj = jsNative

// This is throwing a babel error, so kludging the below
/// Emit "delete"
// [<Emit("delete $0")>]
// let delete (o : obj) : unit = jsNative

[<Emit("delete blockly.Python.definitions_")>]
let deleteDefinitions : unit = jsNative

[<Emit("delete blockly.Python.functionNames_")>]
let deleteFunctions : unit = jsNative

//Prevent Blockly from prepending variable definitions for us 
// 'id' is too heavy handed - we want imports but not definitions
// blockly?Python?finish <- id
// This allows imports but not definitions //TODO: code gen for functions seems to be broken, maybe here?
blockly?Python?finish <- System.Func<string,string>(fun code ->
  let imports = ResizeArray<string>()
  for name in JS.Constructors.Object.keys( blockly?Python?definitions_ ) do
    let ( definitions : obj ) =  blockly?Python?definitions_
    let (def : string) = definitions.[ name ] |> string
    if def.Contains("import") then
      imports.Add(def)
  deleteDefinitions
  deleteFunctions
  blockly?Python?variableDB_?reset()
  (imports |> String.concat "\n")  + "\n\n" + code)

/// Encode the current Blockly workspace as an XML string
let encodeWorkspace() =
  let xml = Blockly.xml.workspaceToDom( blockly.getMainWorkspace() );
  let xmlText = Blockly.xml.domToText( xml )
  xmlText

/// Decode an XML string and load the represented blocks into the Blockly workspace
let decodeWorkspace( xmlText ) =
  let xml = Blockly.xml.textToDom( xmlText )
  Blockly.xml.domToWorkspace( xml, blockly.getMainWorkspace() ) |> ignore

//--------------------------------------------------------------------------------------------------
// DEFINING BLOCKS BELOW
//--------------------------------------------------------------------------------------------------

// comprehension block (goes inside list block for list comprehension)
blockly?Blocks.["comprehensionForEach"] <- createObj [
  "init" ==> fun () ->
    Browser.Dom.console.log( "comprehensionForEach" + " init")
    thisBlock.appendValueInput("LIST")
      .setCheck(!^None)
      .appendField( !^"for each item"  )
      .appendField( !^(blockly.FieldVariable.Create("i") :?> Blockly.Field), "VAR"  )
      .appendField( !^"in list"  ) |> ignore
    thisBlock.appendValueInput("YIELD")
      .setCheck(!^None)
      .setAlign(blockly.ALIGN_RIGHT)
      .appendField( !^"yield"  ) |> ignore
    thisBlock.setOutput(true, !^None)
    thisBlock.setColour(!^230.0)
    thisBlock.setTooltip !^("Use this to generate a sequence of elements, also known as a comprehension. Often used for list comprehensions." )
    thisBlock.setHelpUrl !^"https://docs.python.org/3/tutorial/datastructures.html#list-comprehensions"
  ]
blockly?Python.["comprehensionForEach"] <- fun (block : Blockly.Block) -> 
  let var = blockly?Python?variableDB_?getName( block.getFieldValue("VAR").Value |> string, blockly?Variables?NAME_TYPE);
  let list = blockly?Python?valueToCode( block, "LIST", blockly?Python?ORDER_ATOMIC )
  let yieldValue = blockly?Python?valueToCode( block, "YIELD", blockly?Python?ORDER_ATOMIC )
  let code = yieldValue + " for " + var + " in " + list
  [| code; blockly?Python?ORDER_ATOMIC |] //TODO: COMPREHENSION PRECEDENCE IS ADDING () NESTING; SEE SCREENSHOT; TRY ORDER NONE?

// with as block
blockly?Blocks.["withAs"] <- createObj [
  "init" ==> fun () ->
    Browser.Dom.console.log( "withAs" + " init")
    thisBlock.appendValueInput("EXPRESSION")
      .setCheck(!^None)
      .appendField( !^"with"  ) |> ignore
    thisBlock.appendDummyInput()
      .appendField( !^"as"  ) 
      .appendField( !^(blockly.FieldVariable.Create("item") :?> Blockly.Field), "TARGET"  ) |> ignore
    thisBlock.appendStatementInput("SUITE")
      .setCheck(!^None) |> ignore
    thisBlock.setNextStatement true
    thisBlock.setPreviousStatement true
    thisBlock.setInputsInline true |> ignore
    thisBlock.setColour(!^230.0)
    thisBlock.setTooltip !^("Use this to open resources (usually file-type) in a way that automatically handles errors and disposes of them when done. May not be supported by all libraries." )
    thisBlock.setHelpUrl !^"https://docs.python.org/3/reference/compound_stmts.html#with"
  ]
blockly?Python.["withAs"] <- fun (block : Blockly.Block) -> 
  let expressionCode = blockly?Python?valueToCode( block, "EXPRESSION", blockly?Python?ORDER_ATOMIC ) |> string
  let targetCode = blockly?Python?variableDB_?getName( block.getFieldValue("TARGET").Value |> string, blockly?Variables?NAME_TYPE) |> string
  let suiteCode = blockly?Python?statementToCode( block, "SUITE" ) //|| blockly?Python?PASS 
  let code = "with " + expressionCode + " as " + targetCode + ":\n" + suiteCode.ToString()
  code
  //[| code; blockly?Python?ORDER_ATOMIC |] 


// TEXT file read block
blockly?Blocks.["textFromFile"] <- createObj [
  "init" ==> fun () ->
    Browser.Dom.console.log( "textFromFile" + " init")
    thisBlock.appendValueInput("FILENAME")
      .setCheck(!^"String")
      .appendField( !^"read text from file"  ) |> ignore
      // .appendField( !^(blockly.FieldTextInput.Create("type filename here...") :?> Blockly.Field), "FILENAME"  ) |> ignore
    thisBlock.setOutput(true, !^None)
    thisBlock.setColour(!^230.0)
    thisBlock.setTooltip !^("Use this to read a text file. It will output a string." )
    thisBlock.setHelpUrl !^"https://docs.python.org/3/tutorial/inputoutput.html"
  ]
// Generate Python template code
blockly?Python.["textFromFile"] <- fun (block : Blockly.Block) -> 
  let fileName = blockly?Python?valueToCode( block, "FILENAME", blockly?Python?ORDER_ATOMIC )
  // let fileName = block.getFieldValue("FILENAME").Value |> string
  let code = "open(" + fileName + ",encoding='utf-8').read()"
  [| code; blockly?Python?ORDER_FUNCTION_CALL |]

// GENERAL file read block
blockly?Blocks.["readFile"] <- createObj [
  "init" ==> fun () ->
    Browser.Dom.console.log( "readFile" + " init")
    thisBlock.appendDummyInput()
      .appendField( !^"read file"  )
      .appendField( !^(blockly.FieldTextInput.Create("type filename here...") :?> Blockly.Field), "FILENAME"  ) |> ignore
    thisBlock.setOutput(true, !^None)
    thisBlock.setColour(!^230.0)
    thisBlock.setTooltip !^("Use this to read a file. It will output a file, not a string." )
    thisBlock.setHelpUrl !^"https://docs.python.org/3/tutorial/inputoutput.html"
  ]
// Generate Python template code
blockly?Python.["readFile"] <- fun (block : Blockly.Block) -> 
  let fileName = block.getFieldValue("FILENAME").Value |> string
  let code = "open('" + fileName + "',encoding='utf-8')"
  [| code; blockly?Python?ORDER_FUNCTION_CALL |]


/// A template to create arbitrary code blocks (FREESTYLE) in these dimensions: dummy/input; output/nooutput
let makeCodeBlock (blockName:string) (hasInput: bool) (hasOutput: bool) =
  blockly?Blocks.[blockName] <- createObj [
    "init" ==> fun () ->
      let input = if hasInput then thisBlock.appendValueInput("INPUT").setCheck(!^None) else thisBlock.appendDummyInput()
      Browser.Dom.console.log( blockName + " init")
      input
        .appendField( !^(blockly.FieldTextInput.Create("type code here...") :?> Blockly.Field), "CODE"  ) |> ignore
      if hasOutput then 
        thisBlock.setOutput(true, !^None)
      else
        thisBlock.setNextStatement true
        thisBlock.setPreviousStatement true
      thisBlock.setColour(!^230.0)
      thisBlock.setTooltip !^("You can put any Python code in this block. Use this block if you " + (if hasInput then "do" else "don't") + " need to connect an input block and "+ (if hasOutput then "do" else "don't") + " need to connect an output block." )
      thisBlock.setHelpUrl !^"https://docs.python.org/3/"
    ]
  // Generate Python template code
  blockly?Python.[blockName] <- fun (block : Blockly.Block) -> 
    let userCode = block.getFieldValue("CODE").Value |> string
    let code =
      if hasInput then
        let input = blockly?Python?valueToCode( block, "INPUT", blockly?Python?ORDER_ATOMIC )
        (userCode + " " + input).Trim()
      else 
        userCode.Trim()
    if hasOutput then
      [| code; blockly?Python?ORDER_ATOMIC |] //Assumption is that freestyle should not invoke operator precedence at all
    else
      code + "\n" |> unbox

//Make all varieties of code block
makeCodeBlock "dummyOutputCodeBlock" false true
makeCodeBlock "dummyNoOutputCodeBlock" false false
makeCodeBlock "valueOutputCodeBlock" true true
makeCodeBlock "valueNoOutputCodeBlock" true false

/// Create a Blockly/Python templated import block: TODO if we make this part of the variable menu, then users will never need to rename variable after using the block
let makeImportBlock (blockName:string) (labelOne:string) (labelTwo:string)  =
  blockly?Blocks.[ blockName ] <- createObj [
    "init" ==> fun () -> 
      // Browser.Dom.console.log("did import block init")
      thisBlock.appendDummyInput()
        .appendField( !^labelOne  )
        .appendField( !^(blockly.FieldTextInput.Create("some library") :?> Blockly.Field), "libraryName"  )
        .appendField( !^labelTwo)
        .appendField( !^(blockly.FieldVariable.Create("variable name") :?> Blockly.Field), "libraryAlias"  ) |> ignore
      thisBlock.setNextStatement true
      thisBlock.setPreviousStatement true
      thisBlock.setColour !^230.0
      thisBlock.setTooltip !^"Import a python package to access functions in that package"
      thisBlock.setHelpUrl !^"https://docs.python.org/3/reference/import.html"
    ]
  /// Generate Python import code
  blockly?Python.[ blockName ] <- fun (block : Blockly.Block) -> 
    let libraryName = block.getFieldValue("libraryName").Value |> string
    let libraryAlias = blockly?Python?variableDB_?getName( block.getFieldValue("libraryAlias").Value |> string, blockly?Variables?NAME_TYPE);
    let code =  labelOne + " " + libraryName + " " + labelTwo + " " + libraryAlias + "\n"
    code

//make import as block
makeImportBlock "importAs" "import" "as"

//make from import block
makeImportBlock "importFrom" "from" "import"

/// indexer block
blockly?Blocks.[ "indexer" ] <- createObj [
  "init" ==> fun () -> 
    thisBlock.appendValueInput("INDEX")
      .appendField( !^(blockly.FieldVariable.Create("{dictVariable}") :?> Blockly.Field), "VAR"  )
      .appendField( !^"["  ) |> ignore          
    thisBlock.appendDummyInput()
      .appendField( !^"]") |> ignore
    thisBlock.setInputsInline true
    thisBlock.setOutput true
    thisBlock.setColour !^230.0
    thisBlock.setTooltip !^"Gets an item from the variable at a given index. Not supported for all variables."
    thisBlock.setHelpUrl !^"https://docs.python.org/3/reference/datamodel.html#object.__getitem__"
  ]
/// Generate Python import code
blockly?Python.[ "indexer" ] <- fun (block : Blockly.Block) -> 
  let varName = blockly?Python?variableDB_?getName( block.getFieldValue("VAR").Value |> string, blockly?Variables?NAME_TYPE);
  let input = blockly?Python?valueToCode( block, "INDEX", blockly?Python?ORDER_ATOMIC )
  let code =  varName + "[" + input + "]" //+ "\n"
  [| code; blockly?Python?ORDER_ATOMIC |]


/// A template for variable argument function block creation (where arguments are in a list), including the code generator.
let makeFunctionBlock (blockName:string) (label:string) (outputType:string) (tooltip:string) (helpurl:string) (functionStr:string) =
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
  /// Generate Python template conversion code
  blockly?Python.[blockName] <- fun (block : Blockly.Block) -> 
    // let x = blockly?Python?valueToCode( block, "x", blockly?Python?ORDER_ATOMIC )
    // let code =  functionStr + "(" + x + ")"
    let (args : string) = blockly?Python?valueToCode(block, "x", blockly?Python?ORDER_MEMBER) 
    let cleanArgs = System.Text.RegularExpressions.Regex.Replace(args,"^\[|\]$" , "")
    let code = functionStr + "(" +  cleanArgs + ")" 
    [| code; blockly?Python?ORDER_FUNCTION_CALL |]

// ALREADY EXISTS
// sort: TODO only accept lists, setCheck("Array")
// makeSingleArgFunctionBlock 
//   "sortBlock"
//   "sort"
//   "Array"
//   "Sort a list."
//   "https://python-reference.readthedocs.io/en/latest/docs/list/sort.html"
//   "sort"

// reversed
makeFunctionBlock 
  "reversedBlock"
  "reversed"
  "None"
  "Create a reversed iterator to reverse a list or a tuple; wrap it in a new list or tuple."
  "https://docs.python.org/3/library/functions.html#reversed"
  "reversed"

// tuple
makeFunctionBlock 
  "tupleConstructorBlock"
  "tuple"
  "None"
  "Create a tuple from a list, e.g. ['a','b'] becomes ('a','b')"
  "https://docs.python.org/3/library/stdtypes.html#tuple"
  "tuple"

// dict
makeFunctionBlock 
  "dictBlock"
  "dict"
  "None"
  "Create a dictionary from a list of tuples, e.g. [('a',1),('b',2)...]"
  "https://docs.python.org/3/tutorial/datastructures.html#dictionaries"
  "dict"

// zip
makeFunctionBlock 
  "zipBlock"
  "zip"
  "Array"
  "Zip together two or more lists"
  "https://docs.python.org/3.3/library/functions.html#zip"
  "zip"

// sorted
makeFunctionBlock 
  "sortedBlock"
  "as sorted"
  "Array"
  "Sort lists of stuff"
  "https://docs.python.org/3.3/library/functions.html#sorted"
  "sorted"

// set: TODO only accept lists, setCheck("Array")
makeFunctionBlock 
  "setBlock"
  "set"
  "Array"
  "Make a set with unique members of a list."
  "https://docs.python.org/2/library/sets.html"
  "set"

// Conversion blocks, e.g. str()
makeFunctionBlock 
  "boolConversion"
  "as bool"
  "Boolean"
  "Convert something to Boolean."
  "https://docs.python.org/3/library/stdtypes.html#boolean-values"
  "bool"

makeFunctionBlock
  "strConversion"
  "as str"
  "String"
  "Convert something to String."
  "https://docs.python.org/3/library/stdtypes.html#str"
  "str"

makeFunctionBlock
  "floatConversion"
  "as float"
  "Number"
  "Convert something to Float."
  "https://docs.python.org/3/library/functions.html#float"
  "float"

makeFunctionBlock
  "intConversion"
  "as int"
  "Number" 
  "Convert something to Int."
  "https://docs.python.org/3/library/functions.html#int"
  "int"

// Get user input, e.g. input()
makeFunctionBlock
  "getInput"
  "input"
  "String"
  "Present the given prompt to the user and wait for their typed input response."
  "https://docs.python.org/3/library/functions.html#input"
  "input"

// Tuple block; TODO use mutator to make variable length
blockly?Blocks.["tupleBlock"] <- createObj [
  "init" ==> fun () ->
    thisBlock.appendValueInput("FIRST")
        .setCheck(!^None)
        .appendField(!^"(") |> ignore
    thisBlock.appendValueInput("SECOND")
        .setCheck(!^None)
        .appendField(!^",") |> ignore
    thisBlock.appendDummyInput()
        .appendField(!^")") |> ignore
    thisBlock.setInputsInline(true);
    thisBlock.setOutput(true, !^None);
    thisBlock.setColour(!^230.0);
    thisBlock.setTooltip(!^"Use this to create a two-element tuple");
    thisBlock.setHelpUrl(!^"https://docs.python.org/3/tutorial/datastructures.html#tuples-and-sequences");
]

/// Generate Python for tuple
blockly?Python.["tupleBlock"] <- fun (block : Blockly.Block) -> 
  let firstArg = blockly?Python?valueToCode(block, "FIRST", blockly?Python?ORDER_ATOMIC) 
  let secondArg = blockly?Python?valueToCode(block, "SECOND", blockly?Python?ORDER_ATOMIC) 
  let code = "(" +  firstArg + "," + secondArg + ")" 
  [| code; blockly?Python?ORDER_NONE |]

//TODO: 
// ? OPTION FOR BOTH POSITION ONLY (PASS IN LIST OF ARGS) AND KEYWORD ARGUMENTS (PASS IN DICTIONARY)
// generalized incr
// Dictionary
// list append, list range

/// An entry for a single name (var/function/whatever)
type IntellisenseEntry =
  {
    Name : string //from user if parent and completion if child
    Info : string //from inspection
    isFunction : bool //from inspection
    isClass : bool //from inspection
  }
// An entry for a complex name, e.g. object, that has associated properties and/or methods
type IntellisenseVariable =
  {
    VariableEntry : IntellisenseEntry
    ChildEntries : IntellisenseEntry[]
  }

/// Dependency injected from JupyterLab. Needed to send intellisense requests to the kernel
let mutable ( notebooks : JupyterlabNotebook.Tokens.INotebookTracker ) = null

let GetKernel() =
  if notebooks <> null then
    match notebooks.currentWidget with
    | Some(widget) -> 
      match widget.session.kernel with
      | Some(kernel) -> Some(widget,kernel)
      | None -> None
    | None -> None
  else
    None

/// Get a completion (tab+tab) using the kernel. Typically this will be following a "." but it could also be to match a known identifier against a few initial letters.
let GetKernelCompletion( queryString : string ) =
  // Browser.Dom.console.log("Requesting completion for: " + queryString)
  match GetKernel() with
  | Some(_, kernel) -> 
    promise {
      try
        let! reply = kernel.requestComplete( !!{| code = queryString; cursor_pos = queryString.Length |} )
        let content: ICompleteReply = unbox reply.content
        return content.matches.ToArray()
      with _ -> return [| queryString + " is unavailable" |]
    }
  | None -> Promise.reject "no kernel" // () //Promise.lift( [||])


///Timeout mojo for kernel inspections
[<Emit("Promise.race($0)")>]
let race (pr: seq<JS.Promise<'T>>): JS.Promise<'T> = jsNative
//perhaps b/c of promise.all, this doesn't work; just waits until timeout triggers and then fails
let requestInspectTimeout( queryString : string) = Promise.create(fun ok er ->
            JS.setTimeout (fun () -> er( failwith ("timeout:" + queryString ) )) 100 (* ms *) |> ignore
        )
/// Get an inspection (shift+tab) using the kernel. AFAIK this only works after a complete known identifier.
let GetKernalInspection( queryString : string ) =
  // Browser.Dom.console.log("Requesting inspection for: " + queryString)
  // if queryString = "dataframe.style" then
  //   Browser.Dom.console.log("stop")
  match GetKernel() with 
  | Some( widget, kernel ) ->
    promise {
      try 
        let! reply =
          //timeouts work but are problematic b/c we never know how long to make them
          // race([
          //   kernel.requestInspect( !!{| code = queryString; cursor_pos = queryString.Length; detail_level = 0 |} ); 
          //   Promise.sleep(10000) |> Promise.bind( fun () -> //was 5000 but numpy was possibly timing out, so extended to 10000; that still timed out when there were many blocks in a single cell
          //     promise{ 
          //       let msg : IInspectReplyMsg = 
          //         createObj [
          //           "content" ==> createObj [
          //                 "status" ==> "error"
          //                 "metadata" ==> null
          //                 "found" ==> false
          //                 "data" ==> null //TODO put exception payload here?
          //             ]           
          //         ] |> unbox
          //       return msg 
          //     } 
          //   )
          //   // doesn't currently work, but might be made to work as alternative to the above
          //   // requestInspectTimeout( queryString )
          // ])
          //This doesn't work becasue "style" doesn't fail - it just never resolves; https://github.com/fable-compiler/fable-promise/blob/master/tests/PromiseTests.fs
          kernel.requestInspect( !!{| code = queryString; cursor_pos = queryString.Length; detail_level = 0 |} ) 
          // |> Promise.catchBind( fun ex -> 
          //   promise{ 
          //     let msg : IInspectReplyMsg = 
          //       createObj [
          //         "content" ==> createObj [
          //               "status" ==> "error"
          //               "metadata" ==> null
          //               "found" ==> false
          //               "data" ==> null //TODO put exception payload here?
          //           ]
          //         // "channel" ==> "shell"
          //         // "header" ==> createObj [
          //         //       "date" ==> "foo"
          //         //       "version" ==> "1"
          //         //       "session" ==> "1"
          //         //       "msg_id" ==> "1"
          //         //       "msg_type" ==> "inspect_reply"
          //         //       "username" ==> "foo"
          //         //   ]
          //         // "parent_header" ==> createObj [
          //         //       "date" ==> "foo"
          //         //       "version" ==> "1"
          //         //       "session" ==> "1"
          //         //       "msg_id" ==> "1"
          //         //       "msg_type" ==> "inspect_request"
          //         //       "username" ==> "foo"
          //         //   ]
          //         // "metadata" ==> null
          //       ] |> unbox
          //     return msg 
          //     } 
          //   ) 

        //formatting the reply is involved because it has some kind of funky ascii encoding
        let content: IInspectReply = unbox reply.content
        // if queryString = "dataframe.style" then
        //   Browser.Dom.console.log("stop")
        if content.found then
          let mimeType = widget.content.rendermime.preferredMimeType( unbox content.data);
          let renderer = widget.content.rendermime.createRenderer( mimeType.Value )
          let payload : PhosphorCoreutils.ReadonlyJSONObject = !!content.data
          let model= JupyterlabRendermime.Mimemodel.Types.MimeModel.Create( !!{| data = Some(payload)  |} )
          let! _ = renderer.renderModel(model) 
          // Browser.Dom.console.log(queryString + ":found" )
          return renderer.node.innerText
        else
          Browser.Dom.console.log(queryString + ":UNDEFINED" )
          return "UNDEFINED" //we check for this case below
      with _ ->  
        Browser.Dom.console.log(queryString + ":UNAVAILABLE" )
        return queryString + " is unavailable" //better way to handle exceptions?
    }
  | None -> 
    Browser.Dom.console.log("NOKERNEL" )
    Promise.reject "no kernel"  //()

/// Store results of resolved promises so that future synchronous calls can access. Keyed on variable name
let intellisenseLookup = new System.Collections.Generic.Dictionary<string,IntellisenseVariable>()
// V2 of the above with 2 stores: one that maps var names to docstrings, and one that maps docstrings to results of promise. Idea is that the docstring/result mapping is fairly static and will not change with var's type or renaming
//(NOTE: V2 MAY NOT BE FLAKEY, MAY HAVE FORGOTTEN TO CALL nltk.data.path.append("/y/nltk_data"))
// let docIntellisenseMap = new System.Collections.Generic.Dictionary<string,IntellisenseVariable>()
// let nameDocMap = new System.Collections.Generic.Dictionary<string,string>()

/// Determine if an entry is a function. We have separate blocks for properties and functions because only function blocks need parameters
let isFunction( info : string ) = info.Contains("Type: function")

/// Determine if an entry is a class. 
let isClass( info : string ) = info.Contains("Type: class")

/// Request an IntellisenseVariable. If the type does not descend from object, the children will be empty.
/// Sometimes we will create a variable but it will have no type until we make an assignment. 
/// We might also create a variable and then change its type.
/// So we need to check for introspections/completions repeatedly (no caching right now).
let RequestIntellisenseVariable(block : Blockly.Block) ( parentName : string ) =
  // if not <| intellisenseLookup.ContainsKey( name ) then //No caching; see above
  // Update the intellisenseLookup asynchronously. First do an info lookup. If var is not an instance type, continue to doing tooltip lookup
  promise {
    try
      let! parentInspection = GetKernalInspection( parentName )
      let parent = { Name=parentName;  Info=parentInspection; isFunction=isFunction(parentInspection); isClass=isClass(parentInspection) }
      // V2 store the name/docstring pair. This is always overwritten(*Updating*).
      // if not <| nameDocMap.ContainsKey( parentName ) then nameDocMap.Add(parentName,parentInspection) else nameDocMap.[parentName] <- parentInspection

      // V2 only search for completions if the docstring has not previously been stored
      // if not <| docIntellisenseMap.ContainsKey( parentInspection ) then
        // promise {  //if promise ce absent here, then preceding conditional is not transpiled   

      // V3 only update completions if cached parent type has changed or has no children OR if there is nothing in the cache.
      let shouldGetChildren =
        match intellisenseLookup.TryGetValue(parent.Name) with
        | true, cached -> if cached.VariableEntry.Info <> parent.Info || cached.ChildEntries.Length = 0 then true else false
        | false, _ -> true
          
      if shouldGetChildren then
        let! completions = GetKernelCompletion( parentName + "." )  //all completions that follow "name."

        //dataframe kludge; TODO not sure why this is necessary
        //if dataframe, filter members leading with _ ; else filter nothing
        let safeCompletions =
          completions
          //|> Array.truncate 169 //100 works, 150 works, 168 (std) works, 169 (style) fails --> no GUI intellisense either, 170 fails, 172 fails, 174 fails, 175 (T) fails, 200 fails
          |> Array.filter( fun s -> 
            if parent.Info.StartsWith("Signature: DataFrame") then
              not <| s.StartsWith("_") &&  not <| s.StartsWith("style") //TODO: kludge for dataframe.style since race above doesn't always work
            else
              true
              )      
        
        //Fails the same way 6/11/20
        // let completionPromises = new ResizeArray<JS.Promise<string>>()
        // for completion in safeCompletions do 
        //   completionPromises.Add( GetKernalInspection(parentName + "." + completion) )

        let! inspections = 
          // if not <| parent.isInstance then //No caching; see above
          //Fails the same way 6/11/20
          // completionPromises |> Promise.Parallel

          //Suddenly started failing 6/11/20
          safeCompletions
          |> Array.map( fun completion ->  GetKernalInspection(parentName + "." + completion) ) //all introspections of name.completion
          |> Promise.Parallel

          //Fails the same way 6/11/20
          // [| 
          //   for completion in safeCompletions do 
          //     yield GetKernalInspection(parentName + "." + completion)  
          // |] |> Promise.all
          
          // else
          //   Promise.lift [||] //No caching; see above
        let children = 
            Array.zip safeCompletions inspections 
            |> Array.map( fun (completion,inspection) -> 
              {Name=completion; Info=inspection; isFunction=isFunction(inspection); isClass=isClass(inspection) }
            ) 
        let intellisenseVariable = { VariableEntry=parent; ChildEntries=children}
        // Store so we can synchronously find results later; if we have seen this var before, overwrite.
        if intellisenseLookup.ContainsKey( parentName ) then
          intellisenseLookup.[parentName] <- intellisenseVariable
        else
          intellisenseLookup.Add( parentName, intellisenseVariable)

        // V2 - never overwritten
        // if not <| docIntellisenseMap.ContainsKey( parentInspection ) then
        // docIntellisenseMap.Add( parentInspection, intellisenseVariable)
          // } |> Promise.start
      else
        Browser.Dom.console.log("Not refreshing intellisense for " + parent.Name)

    //Call update event (sometimes fails for unknown reasons)
    // try 
      let intellisenseUpdateEvent = Blockly.events.Change.Create(block, "field", "VAR", 0, 1) 
      intellisenseUpdateEvent.group <- "INTELLISENSE"
      Browser.Dom.console.log( "event status is " + Blockly.events?disabled_ )
      Blockly.events?disabled_ <- 0 //not sure if this is needed, but sometimes events are not firing?
      Blockly.events.fire( intellisenseUpdateEvent :?> Blockly.Events.Abstract )
    with
    | e ->  Browser.Dom.console.log( "Intellisense event failed to fire; " + e.Message )
    } |> Promise.start

// let GetIntellisenseVariable( name : string ) = 
//   // Now do the lookups here. We expect to fail on first call because the promise has not resolved. We may also lag "truth" if variable type changes.
//   match intellisenseLookup.TryGetValue(name) with
//   | true, ie -> Some(ie)
//   | false,_ -> None
  //FLAKEY CACHING METHOD FOLLOWS (NOTE: MAY NOT BE FLAKEY, MAY HAVE FORGOTTEN TO CALL nltk.data.path.append("/y/nltk_data"))
  // match nameDocMap.TryGetValue(parentName) with
  // | true, doc -> 
  //   match docIntellisenseMap.TryGetValue(doc) with
  //   | true, intellisenseVariable -> Some(intellisenseVariable)
  //   | false, _ -> None
  // | false,_ -> None

//We need to get the var name in order to call the kernel to generate the list. Every time the variable changes, we should update the list
// For now, ignore performance. NOTE can we use an event to retrigger init once the promise completes?
// NOTE: this works but only on the last var created. It does not fire when the var dropdown changes
// let optionsGenerator( block : Blockly.Block ) =
//   // At this stage the VAR field is not associated with the variable name presented to the user, e.g. "x"
//   //We can get a list of variables by accessing the workspace. The last variable created is the last element in the list returned.
//   let lastVar = block.workspace.getAllVariables() |> Seq.last
let requestAndStubOptions (block : Blockly.Block) ( varName : string ) =
  if varName <> "" && not <| block.isInFlyout then //flyout restriction prevents triple requests for intellisense blocks in flyout
    //initiate an intellisense request asynchronously
    varName |> RequestIntellisenseVariable block
  //return an option stub while we wait
  if block.isInFlyout then
    [| [| " "; " " |] |]
  elif varName <> "" && varName |> intellisenseLookup.ContainsKey then
      [| [| "!Waiting for kernel to respond with options."; "!Waiting for kernel to respond with options." |] |]
  else
    [| [| "!Not defined until you execute code."; "!Not defined until you execute code." |] |]

let getIntellisenseMemberOptions(memberSelectionFunction : IntellisenseEntry -> bool) ( varName : string ) =
  match  varName |> intellisenseLookup.TryGetValue with
  | true, iv when not(iv.VariableEntry.isFunction) && iv.ChildEntries.Length > 0  -> 
      //NOTE: for dropdowns, blockly returns the label, e.g. "VAR", not the value displayed to the user. Making them identical allows us to get the value displayed to user
      iv.ChildEntries |> Array.filter memberSelectionFunction |> Array.map( fun ie -> [| ie.Name; ie.Name |] )
  | false, _ ->  [| [| "!Not defined until you execute code."; "!Not defined until you execute code." |] |]
  | true, iv when iv.VariableEntry.Info = "UNDEFINED" ->  [| [| "!Not defined until you execute code."; "!Not defined until you execute code." |] |]
  | _ -> [| [| "!No properties available."; "!No properties available." |] |]

let getIntellisenseVarTooltip( varName : string ) =
  match  varName |> intellisenseLookup.TryGetValue with
  | true, iv -> 
    iv.VariableEntry.Info
  | false, _ -> "!Not defined until you execute code."

let getIntellisenseMemberTooltip( varName : string ) (memberName : string )=
  match  varName |> intellisenseLookup.TryGetValue with
  | true, iv -> 
    match iv.ChildEntries |> Array.tryFind( fun c -> c.Name = memberName ) with
    | Some(child) -> child.Info
    | None -> "!Not defined until you execute code."
  | false, _ -> "!Not defined until you execute code."

/// Update all the blocks that use intellisense. Called after the kernel executes a cell so our intellisense in Blockly is updated.
let UpdateAllIntellisense() =
  let workspace = blockly.getMainWorkspace()
  let blocks = workspace.getBlocksByType("varGetProperty", false)
  blocks.AddRange( workspace.getBlocksByType("varDoMethod", false) )
  for b in blocks do
    b?updateIntellisense(b,None, requestAndStubOptions b) 

/// Remove a field from a block safely, even if it doesn't exist
let SafeRemoveField( block:Blockly.Block ) ( fieldName : string ) ( inputName : string )=
  match block.getField(fieldName), block.getInput(inputName) with
  | null, _ -> ()  //field doesnt exist, no op
  | _, null ->  Browser.Dom.console.log( "error removing (" + fieldName + ") from block; input (" + inputName + ") does not exist" )
  | _,input -> input.removeField( fieldName )

// TODO: MAKE BLOCK THAT ALLOWS USER TO MAKE AN ASSIGNMENT TO A PROPERTY (SETTER)
// TODO: CHANGE OUTPUT CONNECTOR DEPENDING ON INTELLISENSE: IF FUNCTION DOESN'T HAVE AN OUTPUT, REMOVE CONNECTOR
/// Make a block that has an intellisense-populated member dropdown. The member type is property or method, defined by the filter function
/// Note the "blockName" given to these is hardcoded elsewhere, e.g. the toolbox and intellisense update functions
let makeMemberIntellisenseBlock (blockName:string) (preposition:string) (verb:string) (memberSelectionFunction: IntellisenseEntry -> bool ) ( hasArgs : bool ) ( hasDot : bool )= 
  blockly?Blocks.[blockName] <- createObj [

    //Get the user-facing name of the selected variable; on creation, defaults to created name
    "varSelectionUserName" ==> fun (thisBlockClosure : Blockly.Block) (selectedOption : string option)  ->
      let fieldVariable = thisBlockClosure.getField("VAR") :?> Blockly.FieldVariable
      // let variableModel = fieldVariable.getVariable() //for test purposes -- null when not defined, similar to getText()
      // let b = Blockly.variables.getVariable(thisBlockClosure.workspace, fieldVariable.getValue() ) //for test purposes -- null when not defined, similar to getText()
      // let v = thisBlockClosure.workspace.getAllVariables() //returns all variables, but we don't know which is ours
      // fieldVariable.initModel() // kind of accesses user-selected variable name but also creates a variable with the default name
      
      //Get the last var created. Insane but works because by default, the flyout specifically lists this var in the block. User then expects to change if needed
      let lastVar = thisBlockClosure.workspace.getAllVariables() |> Seq.last 

      //Attempt to get XML serialized data
      let dataString = (thisBlockClosure?data |> string)
      let data = if dataString.Contains(":") then dataString.Split(':') else [| "" |] //var:member data

      let selectionUserName =
        match selectedOption with 
        | Some( option ) -> fieldVariable.getOptions() |> Seq.find( fun o -> o.[1] = option ) |> Seq.head
        // | None -> fieldVariable.getText() //on creation is empty string 
        | None ->
          match fieldVariable.getText(), data.[0], lastVar.name with
          | "","", l ->  l  //Previously we returned empty ""; now as a last resort we return the last var created
          | "",v,_-> v      //prefer XML data over last var when XML data exists
          | t, _,_ -> t     //prefer current var name over all others when it exists
      selectionUserName

    //back up the current member selection so it is not lost every time a cell is run
    "selectedMember" ==> ""

    //Use 'data' string to back up custom data; it is serialized to XML
    // "data" ==> ":" //We can't define this or it overwrites what is deserialized

    //Using currently selected var, update intellisense
    "updateIntellisense" ==> fun (thisBlockClosure : Blockly.Block) (selectedVarOption : string option) (optionsFunction : string -> string[][]) ->
      let input = thisBlockClosure.getInput("INPUT")
      SafeRemoveField thisBlockClosure "MEMBER" "INPUT"
      SafeRemoveField thisBlockClosure "USING" "INPUT"
      let varUserName = thisBlockClosure?varSelectionUserName(thisBlockClosure,selectedVarOption)
      let options = varUserName |> optionsFunction 

      //use intellisense to populate the member options, also use validator so that when we select a new member from the dropdown, tooltip is updated
      input.appendField( !^(blockly.FieldDropdown.Create( options, System.Func<string,obj>( fun newMemberSelection ->
        // Within validator, "this" refers to FieldVariable not block.
        let (thisFieldDropdown : Blockly.FieldDropdown) = !!thisObj
        thisFieldDropdown.setTooltip( !^( getIntellisenseMemberTooltip varUserName newMemberSelection ) )
        //back up the current member selection so it is not lost every time a cell is run; ignore status selections that start with !
        thisBlockClosure?selectedMember <- 
          match newMemberSelection.StartsWith("!"),thisBlock?selectedMember with
          | _, "" -> newMemberSelection 
          | true, _ -> thisBlock?selectedMember
          | false,_ -> newMemberSelection

        //back up to XML data if valid
        if varUserName <> "" then
          thisBlockClosure?data <- varUserName + ":" + thisBlockClosure?selectedMember //only set data when at least var name is known

        //Since we are leveraging the validator, we return the selected value without modification
        newMemberSelection |> unbox)
       ) :> Blockly.Field), "MEMBER"  ) |> ignore 

      //back up to XML data if valide; when the deserialized XML contains data, we should never overwrite it here
      if thisBlockClosure?data = null then
        thisBlockClosure?data <- varUserName + ":" + thisBlockClosure?selectedMember 

      //set up the initial member tooltip
      let memberField = thisBlockClosure.getField("MEMBER")
      memberField.setTooltip( !^( getIntellisenseMemberTooltip varUserName (memberField.getText()) ) )

      //add more fields if arguments are needed. Current strategy is to make those their own block rather than adding mutators to this block
      if hasArgs then
          input.appendField(!^"using", "USING") |> ignore
          thisBlockClosure.setInputsInline(true);

    "init" ==> fun () -> 
      Browser.Dom.console.log( blockName + " init")

      //If we need to pass "this" into a closure, we rename to work around shadowing
      let thisBlockClosure = thisBlock

      let input = if hasArgs then thisBlock.appendValueInput("INPUT") else thisBlock.appendDummyInput("INPUT")
      input
        .appendField(!^preposition) 

        //Use the validator called on variable selection to change the member dropdown so that we get correct members when variable changes
        .appendField( !^(blockly.FieldVariable.Create("variable name", System.Func<string,obj>( fun newSelection ->
          // Within validator, "this" refers to FieldVariable not block.
          let (thisFieldVariable : Blockly.FieldVariable) = !!thisObj
          // update the options FieldDropdown by recreating it with the newly selected variable name
          thisBlockClosure?updateIntellisense( thisBlockClosure, Some(newSelection), requestAndStubOptions thisBlockClosure  )
          //Since we are leveraging the validator, we return the selected value without modification
          newSelection |> unbox)
         ) :?> Blockly.Field), "VAR"  )

        .appendField( !^verb) |> ignore
        
        // Create the options FieldDropdown using "optionsGenerator" with the selected name, currently None
        // .appendField( !^(blockly.FieldDropdown.Create( thisBlock?varSelectionUserName(thisBlockClosure, None) |> requestAndStubOptions thisBlock ) :> Blockly.Field), "MEMBER"  ) |> ignore 
      thisBlockClosure?updateIntellisense( thisBlockClosure, None, requestAndStubOptions thisBlockClosure) //adds the member fields, triggering intellisense

      if hasArgs then thisBlock.setInputsInline(true)
      thisBlock.setOutput(true)
      thisBlock.setColour !^230.0
      thisBlock.setTooltip !^"!Not defined until you execute code."
      thisBlock.setHelpUrl !^""

    //Listen for intellisense ready events
    "onchange" ==> fun (e:Blockly.Events.Change) ->
      if thisBlock.workspace <> null && not <| thisBlock.isInFlyout && e.group = "INTELLISENSE" then 
        // let thisBlockClosure = thisBlock
        // update the options FieldDropdown by recreating it with the newly selected variable name
        // let input = thisBlock.getInput("INPUT")
        // SafeRemoveField thisBlock "MEMBER" "INPUT"
        // SafeRemoveField thisBlock "USING" "INPUT"
        // let varName = thisBlock?varSelectionUserName(thisBlock, None)
        // input.appendField( !^(blockly.FieldDropdown.Create( varName |> getIntellisenseMemberOptions memberSelectionFunction ) :> Blockly.Field), "MEMBER"  ) |> ignore 
        
        //deserialize data from xml, var:member
        let data = (thisBlock?data |> string).Split(':')

        // update the options FieldDropdown by recreating it with fresh intellisense
        thisBlock?updateIntellisense( thisBlock, None, getIntellisenseMemberOptions memberSelectionFunction ) //adds the member fields, triggering intellisense

        //restore previous member selection if possible
        let memberField = thisBlock.getField("MEMBER")
        // memberField.setValue( thisBlock?selectedMember) //OLD way; does not work with XML serialization 
        // memberField.setValue( data.[1] ) //NEW way; is deserialized from XML
        //prevent setting to ""
        if data.[1] <> "" then
          memberField.setValue( data.[1] ) //NEW way; is deserialized from XML

        // update tooltip
        let varName = thisBlock?varSelectionUserName(thisBlock, None) //Blockly is pretty good at recovering the variable, so we don't need to get from data
        thisBlock.setTooltip !^( varName |> getIntellisenseVarTooltip )

        //TODO NOT SOLVING PROBLEM
        //force a block rerender (blocks sometimes "click" but are offset from what they are supposed to be connected to)
        // if thisBlock.outputConnection.targetBlock() <> null then
        //   let (blockSvg : Blockly.BlockSvg ) = !!thisBlock.outputConnection.targetBlock()
        //   blockSvg.render()
        // thisBlock.workspace.sv
        // Blockly.blockSvg.
        ()
    ]
  /// Generate Python intellisense member block conversion code
  blockly?Python.[blockName] <- fun (block : Blockly.Block) -> 
    let varName = blockly?Python?variableDB_?getName( block.getFieldValue("VAR").Value |> string, blockly?Variables?NAME_TYPE);
    let memberName = block.getFieldValue("MEMBER").Value |> string
    // let x = blockly?Python?valueToCode( block, "VAR", blockly?Python?ORDER_ATOMIC )
    let code =  
      //All of the "not defined" option messages start with "!"
      if memberName.StartsWith("!") then
        ""
      else if hasArgs then
        let (args : string) = blockly?Python?valueToCode(block, "INPUT", blockly?Python?ORDER_MEMBER) 
        let cleanArgs = System.Text.RegularExpressions.Regex.Replace(args,"^\[|\]$" , "")
        varName + (if hasDot then "." else "" ) + memberName + "(" +  cleanArgs + ")" 
        // varName + (if hasDot then "." else "" ) + memberName + "(" +  args.Trim([| '['; ']' |]) + ")" //looks like a bug in Fable, brackets not getting trimmed?
      else
        varName + (if hasDot then "." else "" ) + memberName
    [| code; blockly?Python?ORDER_FUNCTION_CALL |]

//Intellisense variable get property block
makeMemberIntellisenseBlock 
  "varGetProperty"
  "from"
  "get"
  (fun (ie : IntellisenseEntry) -> not( ie.isFunction ))
  false //no arguments
  true //has dot

//Intellisense method block
makeMemberIntellisenseBlock 
  "varDoMethod"
  "with"
  "do"
  (fun (ie : IntellisenseEntry) -> ie.isFunction )
  true //has arguments
  true //has dot

//Intellisense class constructor block
makeMemberIntellisenseBlock 
  "varCreateObject"
  "with"
  "create"
  (fun (ie : IntellisenseEntry) -> ie.isClass )
  true //has arguments
  true //no dot

// Override the dynamic 'Variables' toolbox category initialized in blockly_compressed.js
// The basic idea here is that as we add vars, we extend the list of vars in the dropdowns in this category
blockly?Variables?flyoutCategoryBlocks <- fun (workspace : Blockly.Workspace) ->
  let variableModelList = workspace.getVariablesOfType("")
  let xmlList = ResizeArray<Element>()
  //Only create variable blocks if a variable has been defined
  if 0 < variableModelList.Count then
    let lastVarFieldXml = variableModelList.[variableModelList.Count - 1]
    if blockly?Blocks?variables_set then
      //variable set block
      let xml = Blockly.Utils.xml.createElement("block") 
      xml.setAttribute("type", "variables_set")
      xml.setAttribute("gap", if blockly?Blocks?math_change then "8" else "24")
      xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
      xmlList.Add(xml)
    //variable incr block : TODO REPLACE WITH GENERALIZED INCR
    if blockly?Blocks?math_change then
      let xml = Blockly.Utils.xml.createElement("block") 
      xml.setAttribute("type", "math_change")
      xml.setAttribute("gap", if blockly?Blocks?math_change then "20" else "8")
      xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
      let shadowBlockDom = Blockly.xml.textToDom("<value name='DELTA'><shadow type='math_number'><field name='NUM'>1</field></shadow></value>")
      xml.appendChild(shadowBlockDom) |> ignore
      xmlList.Add(xml)
    //variable property block
    if blockly?Blocks?varGetProperty then
      let xml = Blockly.Utils.xml.createElement("block") 
      xml.setAttribute("type", "varGetProperty")
      xml.setAttribute("gap", if blockly?Blocks?varGetProperty then "20" else "8")
      xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
      xmlList.Add(xml)
    //variable method block
    if blockly?Blocks?varDoMethod then
      let xml = Blockly.Utils.xml.createElement("block") 
      xml.setAttribute("type", "varDoMethod")
      xml.setAttribute("gap", if blockly?Blocks?varDoMethod then "20" else "8")
      xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
      xmlList.Add(xml)
    //variable create object block
    if blockly?Blocks?varCreateObject then
      let xml = Blockly.Utils.xml.createElement("block") 
      xml.setAttribute("type", "varCreateObject")
      xml.setAttribute("gap", if blockly?Blocks?varCreateObject then "20" else "8")
      xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
      xmlList.Add(xml)
    //variable indexer block
    // if blockly?Blocks?indexer then
    //   let xml = Blockly.Utils.xml.createElement("block") 
    //   xml.setAttribute("type", "indexer")
    //   xml.setAttribute("gap", if blockly?Blocks?varCreateObject then "20" else "8")
    //   xml.appendChild( Blockly.variables.generateVariableFieldDom(lastVarFieldXml)) |> ignore
    //   xmlList.Add(xml)
    //variable get block, one per variable: TODO - WHY DO WE NEED ONE PER VAR? LESS CLUTTER TO HAVE ONE WITH DROPDOWN
    if blockly?Blocks?variables_get then
      //for some reason the original "directly translated" code is passing the workspace into sort instead of the variables
      // variableModelList?sort( Blockly.variableModel.compareByName ) 
      let sortedVars = variableModelList |> Seq.sortBy( fun v -> v.name)
      for variable in sortedVars do
        let xml = Blockly.Utils.xml.createElement("block") 
        xml.setAttribute("type", "variables_get")
        xml.setAttribute("gap", "8")
        xml.appendChild( Blockly.variables.generateVariableFieldDom(variable)) |> ignore
        xmlList.Add(xml)
  xmlList


/// A static toolbox copied from one of Google's online demos at https://blockly-demo.appspot.com/static/demos/index.html
/// Curiously category names like "%{BKY_CATLOGIC}" not resolved by Blockly, even though the colors are, so names 
/// are replaced with English strings below
let toolbox =
    """<xml xmlns="https://developers.google.com/blockly/xml" id="toolbox" style="display: none">
    <category name="IMPORT" colour="255">
      <block type="importAs"></block>
      <block type="importFrom"></block>
    </category>
    <category name="FREESTYLE" colour="290">
      <block type="dummyOutputCodeBlock"></block>
      <block type="dummyNoOutputCodeBlock"></block>
      <block type="valueOutputCodeBlock"></block>
      <block type="valueNoOutputCodeBlock"></block>
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
      <block type="comprehensionForEach"></block>
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
      <block type="indexer"></block>
      <block type="lists_split">
        <value name="DELIM">
          <shadow type="text">
            <field name="TEXT">,</field>
          </shadow>
        </value>
      </block>
      <block type="lists_sort"></block>
      <block type="setBlock"></block>
      <block type="sortedBlock"></block>
      <block type="zipBlock"></block>
      <block type="dictBlock"></block>
      <block type="tupleBlock"></block>
      <block type="tupleConstructorBlock"></block>
      <block type="reversedBlock"></block>
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
    <category name="I/O" colour="190">
      <block type="withAs">
      </block>
      <block type="textFromFile">
        <value name="FILENAME">
          <shadow type="text">
            <field name="TEXT">name of file</field>
          </shadow>
        </value>
      </block>
      <block type="readFile"></block>
    </category>
    <sep></sep>
    <category name="VARIABLES" colour="%{BKY_VARIABLES_HUE}" custom="VARIABLE"></category>
    <!-- <category name="FUNCTIONS" colour="%{BKY_PROCEDURES_HUE}" custom="PROCEDURE"></category> -->
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