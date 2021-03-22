// ts2fable 0.0.0
module rec Blockly
open System
open Fable.Core
open Fable.Core.JS
open Browser
open Browser.Types

//amo
type Function = Func<string,obj>
// type Function = Func<unit>

/// [<Import("*","blockly")>] appears to call index.js which calls either browser.js or node.js; 
/// browser.js then calls core-browser, blockly, en, blocks, and javascript generator; 
/// node.js then calls  core, blockly-node, en, blocks, and all language generators;
/// This makes the imports a bit confusing because the modules are statically specified here 
/// but are dynamically discovered at runtime.
let [<Import("*","blockly")>] blockly: Blockly.IExports = jsNative
let [<Import("*","blockly")>] goog: Goog.IExports = jsNative

module Blockly =

    /// English locale
    let [<ImportAll("blockly/msg/en")>] en : SetLocaleMsg = jsNative

    //incomplete for now
    module Blocks = 
        type [<AllowNullLiteral>] IExports =
            abstract colour : obj

    //modeled on module python below; these should probably be pulled into their own interface to reduce duplication
    module JavaScript =
        type [<AllowNullLiteral>] IExports =
            inherit Blockly.Generator
            abstract ORDER_ATOMIC: obj option
            abstract ORDER_OVERRIDES: ResizeArray<ResizeArray<float>>
            /// <summary>Initialise the database of variable names.</summary>
            /// <param name="workspace">Workspace to generate code from.</param>
            abstract init: workspace: Blockly.Workspace -> unit
            abstract PASS: obj option
            /// <summary>Prepend the generated code with the variable definitions.</summary>
            /// <param name="code">Generated code.</param>
            abstract finish: code: string -> string
            /// <summary>Naked values are top-level blocks with outputs that aren't plugged into
            /// anything.</summary>
            /// <param name="line">Line of generated code.</param>
            abstract scrubNakedValue: line: string -> string
            /// <summary>Encode a string as a properly escaped Python string, complete with quotes.</summary>
            /// <param name="string">Text to encode.</param>
            abstract quote_: string: string -> string
            /// <summary>Encode a string as a properly escaped multiline Python string, complete
            /// with quotes.</summary>
            /// <param name="string">Text to encode.</param>
            abstract multiline_quote_: string: string -> string
            /// <summary>Common tasks for generating Python from blocks.
            /// Handles comments for the specified block and any connected value blocks.
            /// Calls any statements following this block.</summary>
            /// <param name="block">The current block.</param>
            /// <param name="code">The Python code created for this block.</param>
            /// <param name="opt_thisOnly">True to generate code for only this statement.</param>
            abstract scrub_: block: Blockly.Block * code: string * ?opt_thisOnly: bool -> string
            /// <summary>Gets a property and adjusts the value, taking into account indexing, and
            /// casts to an integer.</summary>
            /// <param name="block">The block.</param>
            /// <param name="atId">The property ID of the element to get.</param>
            /// <param name="opt_delta">Value to add.</param>
            /// <param name="opt_negate">Whether to negate the value.</param>
            abstract getAdjustedInt: block: Blockly.Block * atId: string * ?opt_delta: float * ?opt_negate: bool -> U2<string, float>
 
    //Generated with https://github.com/trodi/blockly-d.ts from blockly/generators/python.js and python (directory) and manually integrated here
    module Python =
        let [<Import("text","blockly/python")>] text: Text.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            inherit Blockly.Generator
            abstract ORDER_ATOMIC: obj option
            abstract ORDER_OVERRIDES: ResizeArray<ResizeArray<float>>
            /// <summary>Initialise the database of variable names.</summary>
            /// <param name="workspace">Workspace to generate code from.</param>
            abstract init: workspace: Blockly.Workspace -> unit
            abstract PASS: obj option
            /// <summary>Prepend the generated code with the variable definitions.</summary>
            /// <param name="code">Generated code.</param>
            abstract finish: code: string -> string
            /// <summary>Naked values are top-level blocks with outputs that aren't plugged into
            /// anything.</summary>
            /// <param name="line">Line of generated code.</param>
            abstract scrubNakedValue: line: string -> string
            /// <summary>Encode a string as a properly escaped Python string, complete with quotes.</summary>
            /// <param name="string">Text to encode.</param>
            abstract quote_: string: string -> string
            /// <summary>Encode a string as a properly escaped multiline Python string, complete
            /// with quotes.</summary>
            /// <param name="string">Text to encode.</param>
            abstract multiline_quote_: string: string -> string
            /// <summary>Common tasks for generating Python from blocks.
            /// Handles comments for the specified block and any connected value blocks.
            /// Calls any statements following this block.</summary>
            /// <param name="block">The current block.</param>
            /// <param name="code">The Python code created for this block.</param>
            /// <param name="opt_thisOnly">True to generate code for only this statement.</param>
            abstract scrub_: block: Blockly.Block * code: string * ?opt_thisOnly: bool -> string
            /// <summary>Gets a property and adjusts the value, taking into account indexing, and
            /// casts to an integer.</summary>
            /// <param name="block">The block.</param>
            /// <param name="atId">The property ID of the element to get.</param>
            /// <param name="opt_delta">Value to add.</param>
            /// <param name="opt_negate">Whether to negate the value.</param>
            abstract getAdjustedInt: block: Blockly.Block * atId: string * ?opt_delta: float * ?opt_negate: bool -> U2<string, float>

        module Text =
            let [<Import("forceString_","blockly/python")>] forceString_: ForceString_.IExports = jsNative

            type [<AllowNullLiteral>] IExports =
                /// <summary>Enclose the provided value in 'str(...)' function.
                /// Leave string literals alone.</summary>
                /// <param name="value">Code evaluating to a value.</param>
                abstract forceString_: value: string -> string

            module ForceString_ =

                type [<AllowNullLiteral>] IExports =
                    abstract strRegExp: obj option
    //END Generated with https://github.com/trodi/blockly-d.ts from blockly/generators/python.js and python (directory) and manually integrated here

    //Generated with https://github.com/trodi/blockly-d.ts from blockly/blocks and manually integrated here
    module Constants =
        let [<Import("Colour","blockly")>] colour: Colour.IExports = jsNative
        let [<Import("Lists","blockly")>] lists: Lists.IExports = jsNative
        let [<Import("Logic","blockly")>] logic: Logic.IExports = jsNative
        let [<Import("Loops","blockly")>] loops: Loops.IExports = jsNative
        let [<Import("Math","blockly")>] math: Math.IExports = jsNative
        let [<Import("Text","blockly")>] text: Text.IExports = jsNative
        let [<Import("Variables","blockly")>] variables: Variables.IExports = jsNative
        let [<Import("VariablesDynamic","blockly")>] variablesDynamic: VariablesDynamic.IExports = jsNative

        module Colour =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option

        module Lists =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option

        module Logic =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract TOOLTIPS_BY_OP: obj option
                abstract CONTROLS_IF_MUTATOR_MIXIN: obj option
                /// "controls_if" extension function. Adds mutator, shape updating methods, and
                /// dynamic tooltip to "controls_if" blocks.
                abstract CONTROLS_IF_TOOLTIP_EXTENSION: unit -> unit
                abstract LOGIC_COMPARE_ONCHANGE_MIXIN: obj option
                /// "logic_compare" extension function. Adds type left and right side type
                /// checking to "logic_compare" blocks.
                abstract LOGIC_COMPARE_EXTENSION: unit -> unit
                abstract LOGIC_TERNARY_ONCHANGE_MIXIN: obj option

        module Loops =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract WHILE_UNTIL_TOOLTIPS: obj option
                abstract BREAK_CONTINUE_TOOLTIPS: obj option
                abstract CUSTOM_CONTEXT_MENU_CREATE_VARIABLES_GET_MIXIN: obj option
                abstract CONTROL_FLOW_IN_LOOP_CHECK_MIXIN: obj option

        module Math =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract TOOLTIPS_BY_OP: obj option
                abstract IS_DIVISIBLEBY_MUTATOR_MIXIN: obj option
                /// 'math_is_divisibleby_mutator' extension to the 'math_property' block that
                /// can update the block shape (add/remove divisor input) based on whether
                /// property is "divisble by".
                abstract IS_DIVISIBLE_MUTATOR_EXTENSION: unit -> unit
                abstract LIST_MODES_MUTATOR_MIXIN: obj option
                /// Extension to 'math_on_list' blocks that allows support of
                /// modes operation (outputs a list of numbers).
                abstract LIST_MODES_MUTATOR_EXTENSION: unit -> unit

        module Text =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract QUOTE_IMAGE_MIXIN: obj option
                /// Wraps TEXT field with images of double quote characters.
                abstract TEXT_QUOTES_EXTENSION: unit -> unit
                abstract TEXT_JOIN_MUTATOR_MIXIN: obj option
                /// Performs final setup of a text_join block.
                abstract TEXT_JOIN_EXTENSION: unit -> unit
                /// Update the tooltip of 'text_append' block to reference the variable.
                abstract TEXT_INDEXOF_TOOLTIP_EXTENSION: unit -> unit
                abstract TEXT_CHARAT_MUTATOR_MIXIN: obj option
                /// Does the initial mutator update of text_charAt and adds the tooltip
                abstract TEXT_CHARAT_EXTENSION: unit -> unit

        module Variables =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract CUSTOM_CONTEXT_MENU_VARIABLE_GETTER_SETTER_MIXIN: obj option
                /// <summary>Callback for rename variable dropdown menu option associated with a
                /// variable getter block.</summary>
                /// <param name="block">The block with the variable to rename.</param>
                abstract RENAME_OPTION_CALLBACK_FACTORY: block: Blockly.Block -> RENAME_OPTION_CALLBACK_FACTORYReturn
                /// <summary>Callback for delete variable dropdown menu option associated with a
                /// variable getter block.</summary>
                /// <param name="block">The block with the variable to delete.</param>
                abstract DELETE_OPTION_CALLBACK_FACTORY: block: Blockly.Block -> DELETE_OPTION_CALLBACK_FACTORYReturn

            type [<AllowNullLiteral>] RENAME_OPTION_CALLBACK_FACTORYReturn =
                [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

            type [<AllowNullLiteral>] DELETE_OPTION_CALLBACK_FACTORYReturn =
                [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

        module VariablesDynamic =

            type [<AllowNullLiteral>] IExports =
                abstract HUE: obj option
                abstract CUSTOM_CONTEXT_MENU_VARIABLE_GETTER_SETTER_MIXIN: obj option
                /// <summary>Callback for rename variable dropdown menu option associated with a
                /// variable getter block.</summary>
                /// <param name="block">The block with the variable to rename.</param>
                abstract RENAME_OPTION_CALLBACK_FACTORY: block: Blockly.Block -> RENAME_OPTION_CALLBACK_FACTORYReturn
                /// <summary>Callback for delete variable dropdown menu option associated with a
                /// variable getter block.</summary>
                /// <param name="block">The block with the variable to delete.</param>
                abstract DELETE_OPTION_CALLBACK_FACTORY: block: Blockly.Block -> DELETE_OPTION_CALLBACK_FACTORYReturn

            type [<AllowNullLiteral>] RENAME_OPTION_CALLBACK_FACTORYReturn =
                [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

            type [<AllowNullLiteral>] DELETE_OPTION_CALLBACK_FACTORYReturn =
                [<Emit "$0($1...)">] abstract Invoke: unit -> obj option
    //END Generated with https://github.com/trodi/blockly-d.ts from blockly/blocks and manually integrated here

    
    let [<Import("*","blockly/javascript")>] javascript: JavaScript.IExports = jsNative //modeled on python trodi above
    let [<Import("*","blockly/python")>] python: Python.IExports = jsNative //moved from python trodi above
    let [<Import("Blocks","blockly/blocks")>] blocks: Blocks.IExports = jsNative //manual
    // original follows; these appear to all be static methods, e.g. Blockly.variableModel.compareByName
    // creation of objects appear to use blockly, e.g. blockly.VariableModel.Create
    let [<Import("ASTNode","blockly")>] aSTNode: ASTNode.IExports = jsNative
    let [<Import("blockAnimations","blockly")>] blockAnimations: BlockAnimations.IExports = jsNative
    let [<Import("blockRendering","blockly")>] blockRendering: BlockRendering.IExports = jsNative
    let [<Import("BlockSvg","blockly")>] blockSvg: BlockSvg.IExports = jsNative
    let [<Import("Bubble","blockly")>] bubble: Bubble.IExports = jsNative
    let [<Import("Component","blockly")>] ``component``: Component.IExports = jsNative
    let [<Import("Connection","blockly")>] connection: Connection.IExports = jsNative
    let [<Import("ConnectionDB","blockly")>] connectionDB: ConnectionDB.IExports = jsNative
    let [<Import("ContextMenu","blockly")>] contextMenu: ContextMenu.IExports = jsNative
    let [<Import("Css","blockly")>] css: Css.IExports = jsNative
    let [<Import("CursorSvg","blockly")>] cursorSvg: CursorSvg.IExports = jsNative
    let [<Import("DropDownDiv","blockly")>] dropDownDiv: DropDownDiv.IExports = jsNative
    let [<Import("Events","blockly")>] events: Events.IExports = jsNative
    let [<Import("Extensions","blockly")>] extensions: Extensions.IExports = jsNative
    let [<Import("Field","blockly")>] field: Field.IExports = jsNative
    let [<Import("FieldAngle","blockly")>] fieldAngle: FieldAngle.IExports = jsNative
    let [<Import("FieldCheckbox","blockly")>] fieldCheckbox: FieldCheckbox.IExports = jsNative
    let [<Import("FieldColour","blockly")>] fieldColour: FieldColour.IExports = jsNative
    let [<Import("FieldDate","blockly")>] fieldDate: FieldDate.IExports = jsNative
    let [<Import("FieldDropdown","blockly")>] fieldDropdown: FieldDropdown.IExports = jsNative
    let [<Import("FieldImage","blockly")>] fieldImage: FieldImage.IExports = jsNative
    let [<Import("FieldLabel","blockly")>] fieldLabel: FieldLabel.IExports = jsNative
    let [<Import("FieldLabelSerializable","blockly")>] fieldLabelSerializable: FieldLabelSerializable.IExports = jsNative
    let [<Import("FieldMultilineInput","blockly")>] fieldMultilineInput: FieldMultilineInput.IExports = jsNative
    let [<Import("FieldNumber","blockly")>] fieldNumber: FieldNumber.IExports = jsNative
    let [<Import("fieldRegistry","blockly")>] fieldRegistry: FieldRegistry.IExports = jsNative
    let [<Import("FieldTextInput","blockly")>] fieldTextInput: FieldTextInput.IExports = jsNative
    let [<Import("FieldVariable","blockly")>] fieldVariable: FieldVariable.IExports = jsNative
    let [<Import("FlyoutButton","blockly")>] flyoutButton: FlyoutButton.IExports = jsNative
    let [<Import("Generator","blockly")>] generator: Generator.IExports = jsNative
    let [<Import("Gesture","blockly")>] gesture: Gesture.IExports = jsNative
    let [<Import("Grid","blockly")>] grid: Grid.IExports = jsNative
    let [<Import("Msg","blockly")>] msg: Msg.IExports = jsNative
    // let [<Import("Msg","node-blockly")>] msg: Msg.IExports = jsNative
    let [<Import("Mutator","blockly")>] mutator: Mutator.IExports = jsNative
    let [<Import("Names","blockly")>] names: Names.IExports = jsNative
    let [<Import("navigation","blockly")>] navigation: Navigation.IExports = jsNative
    let [<Import("Options","blockly")>] options: Options.IExports = jsNative
    let [<Import("Procedures","blockly")>] procedures: Procedures.IExports = jsNative
    let [<Import("Scrollbar","blockly")>] scrollbar: Scrollbar.IExports = jsNative
    let [<Import("Toolbox","blockly")>] toolbox: Toolbox.IExports = jsNative
    let [<Import("Tooltip","blockly")>] tooltip: Tooltip.IExports = jsNative
    let [<Import("Touch","blockly")>] touch: Touch.IExports = jsNative
    let [<Import("TouchGesture","blockly")>] touchGesture: TouchGesture.IExports = jsNative
    let [<Import("tree","blockly")>] tree: Tree.IExports = jsNative
    let [<Import("utils","blockly")>] utils: Utils.IExports = jsNative
    let [<Import("VariableModel","blockly")>] variableModel: VariableModel.IExports = jsNative
    let [<Import("Variables","blockly")>] variables: Variables.IExports = jsNative
    let [<Import("VariablesDynamic","blockly")>] variablesDynamic: VariablesDynamic.IExports = jsNative
    let [<Import("WidgetDiv","blockly")>] widgetDiv: WidgetDiv.IExports = jsNative
    let [<Import("Workspace","blockly")>] workspace: Workspace.IExports = jsNative
    let [<Import("WorkspaceComment","blockly")>] workspaceComment: WorkspaceComment.IExports = jsNative
    let [<Import("WorkspaceCommentSvg","blockly")>] workspaceCommentSvg: WorkspaceCommentSvg.IExports = jsNative
    let [<Import("Xml","blockly")>] xml: Xml.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        /// <summary>Set the Blockly locale.
        /// Note: this method is only available in the npm release of Blockly.</summary>
        /// <param name="msg">An object of Blockly message strings in the desired
        /// language.</param>
        abstract setLocale: msg: SetLocaleMsg -> unit
        abstract Block: BlockStatic
        abstract Block__Class: Block__ClassStatic
        abstract BlockDragSurfaceSvg: BlockDragSurfaceSvgStatic
        abstract BlockDragSurfaceSvg__Class: BlockDragSurfaceSvg__ClassStatic
        abstract BlockDragger: BlockDraggerStatic
        abstract BlockDragger__Class: BlockDragger__ClassStatic
        abstract BlockSvg: BlockSvgStatic
        abstract BlockSvg__Class: BlockSvg__ClassStatic
        abstract VERSION: obj option
        abstract mainWorkspace: Blockly.Workspace
        abstract selected: Blockly.Block
        abstract cursor: Blockly.Cursor
        abstract keyboardAccessibilityMode: bool
        /// <summary>Returns the dimensions of the specified SVG image.</summary>
        /// <param name="svg">SVG image.</param>
        abstract svgSize: svg: Element -> Object
        /// <summary>Size the workspace when the contents change.  This also updates
        /// scrollbars accordingly.</summary>
        /// <param name="workspace">The workspace to resize.</param>
        abstract resizeSvgContents: workspace: Blockly.WorkspaceSvg -> unit
        /// <summary>Size the SVG image to completely fill its container. Call this when the view
        /// actually changes sizes (e.g. on a window resize/device orientation change).
        /// See Blockly.resizeSvgContents to resize the workspace when the contents
        /// change (e.g. when a block is added or removed).
        /// Record the height/width of the SVG image.</summary>
        /// <param name="workspace">Any workspace in the SVG.</param>
        abstract svgResize: workspace: Blockly.WorkspaceSvg -> unit
        /// <summary>Close tooltips, context menus, dropdown selections, etc.</summary>
        /// <param name="opt_allowToolbox">If true, don't close the toolbox.</param>
        abstract hideChaff: ?opt_allowToolbox: bool -> unit
        /// Returns the main workspace.  Returns the last used main workspace (based on
        /// focus).  Try not to use this function, particularly if there are multiple
        /// Blockly instances on a page.
        abstract getMainWorkspace: unit -> Blockly.Workspace
        /// <summary>Wrapper to window.alert() that app developers may override to
        /// provide alternatives to the modal browser window.</summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="opt_callback">The callback when the alert is dismissed.</param>
        abstract alert: message: string * ?opt_callback: AlertOpt_callback -> unit
        /// <summary>Wrapper to window.confirm() that app developers may override to
        /// provide alternatives to the modal browser window.</summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="callback">The callback for handling user response.</param>
        abstract confirm: message: string * callback: ConfirmCallback -> unit
        /// <summary>Wrapper to window.prompt() that app developers may override to provide
        /// alternatives to the modal browser window. Built-in browser prompts are
        /// often used for better text input experience on mobile device. We strongly
        /// recommend testing mobile when overriding this.</summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="defaultValue">The value to initialize the prompt with.</param>
        /// <param name="callback">The callback for handling user response.</param>
        abstract prompt: message: string * defaultValue: string * callback: PromptCallback -> unit
        /// <summary>Define blocks from an array of JSON block definitions, as might be generated
        /// by the Blockly Developer Tools.</summary>
        /// <param name="jsonArray">An array of JSON block definitions.</param>
        abstract defineBlocksWithJsonArray: jsonArray: ResizeArray<Object> -> unit
        /// <summary>Bind an event to a function call.  When calling the function, verifies that
        /// it belongs to the touch stream that is currently being processed, and splits
        /// multitouch events into multiple events as needed.</summary>
        /// <param name="node">Node upon which to listen.</param>
        /// <param name="name">Event name to listen to (e.g. 'mousedown').</param>
        /// <param name="thisObject">The value of 'this' in the function.</param>
        /// <param name="func">Function to call when event is triggered.</param>
        /// <param name="opt_noCaptureIdentifier">True if triggering on this event
        /// should not block execution of other event handlers on this touch or
        /// other simultaneous touches.  False by default.</param>
        /// <param name="opt_noPreventDefault">True if triggering on this event
        /// should prevent the default handler.  False by default.  If
        /// opt_noPreventDefault is provided, opt_noCaptureIdentifier must also be
        /// provided.</param>
        abstract bindEventWithChecks_: node: EventTarget * name: string * thisObject: Object * func: Function * ?opt_noCaptureIdentifier: bool * ?opt_noPreventDefault: bool -> ResizeArray<ResizeArray<obj option>>
        /// <summary>Bind an event to a function call.  Handles multitouch events by using the
        /// coordinates of the first changed touch, and doesn't do any safety checks for
        /// simultaneous event processing.</summary>
        /// <param name="node">Node upon which to listen.</param>
        /// <param name="name">Event name to listen to (e.g. 'mousedown').</param>
        /// <param name="thisObject">The value of 'this' in the function.</param>
        /// <param name="func">Function to call when event is triggered.</param>
        abstract bindEvent_: node: EventTarget * name: string * thisObject: Object * func: Function -> ResizeArray<ResizeArray<obj option>>
        /// <summary>Unbind one or more events event from a function call.</summary>
        /// <param name="bindData">Opaque data from bindEvent_.
        /// This list is emptied during the course of calling this function.</param>
        abstract unbindEvent_: bindData: ResizeArray<ResizeArray<obj option>> -> Function
        /// <summary>Is the given string a number (includes negative and decimals).</summary>
        /// <param name="str">Input string.</param>
        abstract isNumber: str: string -> bool
        /// <summary>Convert a hue (HSV model) into an RGB hex triplet.</summary>
        /// <param name="hue">Hue on a colour wheel (0-360).</param>
        abstract hueToHex: hue: float -> string
        /// Checks old colour constants are not overwritten by the host application.
        /// If a constant is overwritten, it prints a console warning directing the
        /// developer to use the equivalent Msg constant.
        abstract checkBlockColourConstants: unit -> unit
        abstract Bubble: BubbleStatic
        abstract Bubble__Class: Bubble__ClassStatic
        abstract BubbleDragger: BubbleDraggerStatic
        abstract BubbleDragger__Class: BubbleDragger__ClassStatic
        abstract Comment: CommentStatic
        abstract Comment__Class: Comment__ClassStatic
        abstract Connection: ConnectionStatic
        abstract Connection__Class: Connection__ClassStatic
        abstract ConnectionDB: ConnectionDBStatic
        abstract ConnectionDB__Class: ConnectionDB__ClassStatic
        abstract LINE_MODE_MULTIPLIER: float
        abstract PAGE_MODE_MULTIPLIER: float
        abstract DRAG_RADIUS: obj option
        abstract FLYOUT_DRAG_RADIUS: obj option
        abstract SNAP_RADIUS: obj option
        abstract CONNECTING_SNAP_RADIUS: obj option
        abstract CURRENT_CONNECTION_PREFERENCE: obj option
        abstract INSERTION_MARKER_COLOUR: obj option
        abstract BUMP_DELAY: obj option
        abstract BUMP_RANDOMNESS: obj option
        abstract COLLAPSE_CHARS: obj option
        abstract LONGPRESS: obj option
        abstract SOUND_LIMIT: obj option
        abstract DRAG_STACK: obj option
        abstract HSV_SATURATION: obj option
        abstract HSV_VALUE: obj option
        abstract SPRITE: obj option
        abstract INPUT_VALUE: obj option
        abstract OUTPUT_VALUE: obj option
        abstract NEXT_STATEMENT: obj option
        abstract PREVIOUS_STATEMENT: obj option
        abstract DUMMY_INPUT: obj option
        abstract ALIGN_LEFT: obj option
        abstract ALIGN_CENTRE: obj option
        abstract ALIGN_RIGHT: float
        // abstract ALIGN_RIGHT: obj option
        abstract DRAG_NONE: obj option
        abstract DRAG_STICKY: obj option
        abstract DRAG_BEGIN: obj option
        abstract DRAG_FREE: obj option
        abstract OPPOSITE_TYPE: obj option
        abstract TOOLBOX_AT_TOP: obj option
        abstract TOOLBOX_AT_BOTTOM: obj option
        abstract TOOLBOX_AT_LEFT: obj option
        abstract TOOLBOX_AT_RIGHT: obj option
        abstract DELETE_AREA_NONE: obj option
        abstract DELETE_AREA_TRASH: obj option
        abstract DELETE_AREA_TOOLBOX: obj option
        abstract VARIABLE_CATEGORY_NAME: obj option
        abstract VARIABLE_DYNAMIC_CATEGORY_NAME: obj option
        abstract PROCEDURE_CATEGORY_NAME: obj option
        abstract RENAME_VARIABLE_ID: obj option
        abstract DELETE_VARIABLE_ID: obj option
        abstract DropDownDiv: DropDownDivStatic
        abstract DropDownDiv__Class: DropDownDiv__ClassStatic
        abstract Field: FieldStatic
        abstract Field__Class: Field__ClassStatic
        abstract FieldAngle: FieldAngleStatic
        abstract FieldAngle__Class: FieldAngle__ClassStatic
        abstract FieldCheckbox: FieldCheckboxStatic
        abstract FieldCheckbox__Class: FieldCheckbox__ClassStatic
        abstract FieldColour: FieldColourStatic
        abstract FieldColour__Class: FieldColour__ClassStatic
        abstract FieldDate: FieldDateStatic
        abstract FieldDate__Class: FieldDate__ClassStatic
        abstract FieldDropdown: FieldDropdownStatic
        abstract FieldDropdown__Class: FieldDropdown__ClassStatic
        abstract FieldImage: FieldImageStatic
        abstract FieldImage__Class: FieldImage__ClassStatic
        abstract FieldLabel: FieldLabelStatic
        abstract FieldLabel__Class: FieldLabel__ClassStatic
        abstract FieldLabelSerializable: FieldLabelSerializableStatic
        abstract FieldLabelSerializable__Class: FieldLabelSerializable__ClassStatic
        abstract FieldMultilineInput: FieldMultilineInputStatic
        abstract FieldMultilineInput__Class: FieldMultilineInput__ClassStatic
        abstract FieldNumber: FieldNumberStatic
        abstract FieldNumber__Class: FieldNumber__ClassStatic
        abstract FieldTextInput: FieldTextInputStatic
        abstract FieldTextInput__Class: FieldTextInput__ClassStatic
        abstract FieldVariable: FieldVariableStatic
        abstract FieldVariable__Class: FieldVariable__ClassStatic
        abstract Flyout: FlyoutStatic
        abstract Flyout__Class: Flyout__ClassStatic
        abstract FlyoutButton: FlyoutButtonStatic
        abstract FlyoutButton__Class: FlyoutButton__ClassStatic
        abstract FlyoutDragger: FlyoutDraggerStatic
        abstract FlyoutDragger__Class: FlyoutDragger__ClassStatic
        abstract HorizontalFlyout: HorizontalFlyoutStatic
        abstract HorizontalFlyout__Class: HorizontalFlyout__ClassStatic
        abstract VerticalFlyout: VerticalFlyoutStatic
        abstract VerticalFlyout__Class: VerticalFlyout__ClassStatic
        abstract Generator: GeneratorStatic
        abstract Generator__Class: Generator__ClassStatic
        abstract Gesture: GestureStatic
        abstract Gesture__Class: Gesture__ClassStatic
        abstract Grid: GridStatic
        abstract Grid__Class: Grid__ClassStatic
        abstract Icon: IconStatic
        abstract Icon__Class: Icon__ClassStatic
        /// <summary>Inject a Blockly editor into the specified container element (usually a div).</summary>
        /// <param name="container">Containing element, or its ID,
        /// or a CSS selector.</param>
        /// <param name="opt_options">Optional dictionary of options.</param>
        abstract inject: container: U2<Element, string> * ?opt_options: BlocklyOptions -> Blockly.Workspace
        abstract Input: InputStatic
        abstract Input__Class: Input__ClassStatic
        abstract InsertionMarkerManager: InsertionMarkerManagerStatic
        abstract InsertionMarkerManager__Class: InsertionMarkerManager__ClassStatic
        abstract Mutator: MutatorStatic
        abstract Mutator__Class: Mutator__ClassStatic
        abstract Names: NamesStatic
        abstract Names__Class: Names__ClassStatic
        abstract Options: OptionsStatic
        abstract Options__Class: Options__ClassStatic
        abstract RenderedConnection: RenderedConnectionStatic
        abstract RenderedConnection__Class: RenderedConnection__ClassStatic
        abstract ScrollbarPair: ScrollbarPairStatic
        abstract ScrollbarPair__Class: ScrollbarPair__ClassStatic
        abstract Scrollbar: ScrollbarStatic
        abstract Scrollbar__Class: Scrollbar__ClassStatic
        abstract Theme: ThemeStatic
        abstract Theme__Class: Theme__ClassStatic
        abstract ThemeManager: ThemeManagerStatic
        abstract ThemeManager__Class: ThemeManager__ClassStatic
        abstract Toolbox: ToolboxStatic
        abstract Toolbox__Class: Toolbox__ClassStatic
        /// Nope, that's not a long-press.  Either touchend or touchcancel was fired,
        /// or a drag hath begun.  Kill the queued long-press task.
        abstract longStop_: unit -> unit
        abstract TouchGesture: TouchGestureStatic
        abstract TouchGesture__Class: TouchGesture__ClassStatic
        abstract Trashcan: TrashcanStatic
        abstract Trashcan__Class: Trashcan__ClassStatic
        abstract VariableMap: VariableMapStatic
        abstract VariableMap__Class: VariableMap__ClassStatic
        abstract VariableModel: VariableModelStatic
        abstract VariableModel__Class: VariableModel__ClassStatic
        abstract Warning: WarningStatic
        abstract Warning__Class: Warning__ClassStatic
        abstract Workspace: WorkspaceStatic
        abstract Workspace__Class: Workspace__ClassStatic
        abstract WorkspaceAudio: WorkspaceAudioStatic
        abstract WorkspaceAudio__Class: WorkspaceAudio__ClassStatic
        abstract WorkspaceComment: WorkspaceCommentStatic
        abstract WorkspaceComment__Class: WorkspaceComment__ClassStatic
        abstract WorkspaceCommentSvg: WorkspaceCommentSvgStatic
        abstract WorkspaceCommentSvg__Class: WorkspaceCommentSvg__ClassStatic
        abstract WorkspaceDragSurfaceSvg: WorkspaceDragSurfaceSvgStatic
        abstract WorkspaceDragSurfaceSvg__Class: WorkspaceDragSurfaceSvg__ClassStatic
        abstract WorkspaceDragger: WorkspaceDraggerStatic
        abstract WorkspaceDragger__Class: WorkspaceDragger__ClassStatic
        abstract WorkspaceSvg: WorkspaceSvgStatic
        abstract WorkspaceSvg__Class: WorkspaceSvg__ClassStatic
        abstract ZoomControls: ZoomControlsStatic
        abstract ZoomControls__Class: ZoomControls__ClassStatic
        abstract Component: ComponentStatic
        abstract Component__Class: Component__ClassStatic
        abstract Action: ActionStatic
        abstract Action__Class: Action__ClassStatic
        abstract ASTNode: ASTNodeStatic
        abstract ASTNode__Class: ASTNode__ClassStatic
        abstract Cursor: CursorStatic
        abstract Cursor__Class: Cursor__ClassStatic
        abstract CursorSvg: CursorSvgStatic
        abstract CursorSvg__Class: CursorSvg__ClassStatic
        abstract FlyoutCursor: FlyoutCursorStatic
        abstract FlyoutCursor__Class: FlyoutCursor__ClassStatic
        abstract MarkerCursor: MarkerCursorStatic
        abstract MarkerCursor__Class: MarkerCursor__ClassStatic
        abstract Menu: MenuStatic
        abstract Menu__Class: Menu__ClassStatic
        abstract MenuItem: MenuItemStatic
        abstract MenuItem__Class: MenuItem__ClassStatic

    type [<AllowNullLiteral>] SetLocaleMsg =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

    type [<AllowNullLiteral>] AlertOpt_callback =
        [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

    type [<AllowNullLiteral>] ConfirmCallback =
        [<Emit "$0($1...)">] abstract Invoke: _0: bool -> obj option

    type [<AllowNullLiteral>] PromptCallback =
        [<Emit "$0($1...)">] abstract Invoke: _0: string -> obj option

    /// Injection options
    /// https://developers.google.com/blockly/reference/js/Blockly.Options.html
    /// https://developers.google.com/blockly/guides/get-started/web#configuration
    type [<AllowNullLiteral>] BlocklyOptions =
        abstract toolbox: U2<HTMLElement, string> option with get, set
        abstract readOnly: bool option with get, set
        abstract trashcan: bool option with get, set
        abstract maxTrashcanContents: float option with get, set
        abstract collapse: bool option with get, set
        abstract comments: bool option with get, set
        abstract disable: bool option with get, set
        abstract sounds: bool option with get, set
        abstract rtl: bool option with get, set
        abstract horizontalLayout: bool option with get, set
        abstract toolboxPosition: string option with get, set
        abstract css: bool option with get, set
        abstract oneBasedIndex: bool option with get, set
        abstract media: string option with get, set
        abstract theme: Blockly.BlocklyTheme option with get, set
        abstract move: TypeLiteral_05 option with get, set
        abstract grid: TypeLiteral_06 option with get, set
        abstract zoom: TypeLiteral_07 option with get, set

    type [<AllowNullLiteral>] BlocklyTheme =
        abstract defaultBlockStyles: TypeLiteral_08 option with get, set
        abstract categoryStyles: TypeLiteral_09 option with get, set

    type [<AllowNullLiteral>] Metrics =
        abstract absoluteLeft: float with get, set
        abstract absoluteTop: float with get, set
        abstract contentHeight: float with get, set
        abstract contentLeft: float with get, set
        abstract contentTop: float with get, set
        abstract contentWidth: float with get, set
        abstract viewHeight: float with get, set
        abstract viewLeft: float with get, set
        abstract viewTop: float with get, set
        abstract viewWidth: float with get, set

    type [<AllowNullLiteral>] Block =
        inherit Block__Class

    type [<AllowNullLiteral>] BlockStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Block

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Block__Class =
        /// AMO added; seems essential in all block definitions...
        abstract init: unit -> unit
        abstract id: string with get, set
        abstract outputConnection: Blockly.Connection with get, set
        abstract nextConnection: Blockly.Connection with get, set
        abstract previousConnection: Blockly.Connection with get, set
        abstract inputList: ResizeArray<Blockly.Input> with get, set
        abstract inputsInline: U2<bool, obj option> with get, set
        abstract tooltip: U2<string, Function> with get, set
        abstract contextMenu: bool with get, set
        abstract parentBlock_: Blockly.Block with get, set
        abstract childBlocks_: ResizeArray<Blockly.Block> with get, set
        abstract collapsed_: bool with get, set
        /// A string representing the comment attached to this block.
        abstract comment: U2<string, Blockly.Comment> with get, set
        /// A model of the comment attached to this block.
        abstract commentModel: Blockly.Block.CommentModel with get, set
        abstract workspace: Blockly.Workspace with get, set
        abstract isInFlyout: bool with get, set
        abstract isInMutator: bool with get, set
        abstract RTL: bool with get, set
        /// True if this block is an insertion marker.
        abstract isInsertionMarker_: bool with get, set
        /// Name of the type of hat.
        abstract hat: U2<string, obj option> with get, set
        abstract ``type``: string with get, set
        abstract inputsInlineDefault: U2<bool, obj option> with get, set
        /// Optional text data that round-trips between blocks and XML.
        /// Has no effect. May be used by 3rd parties for meta information.
        abstract data: string with get, set
        /// Has this block been disposed of?
        abstract disposed: bool with get, set
        /// An optional serialization method for defining how to serialize the
        /// mutation state. This must be coupled with defining `domToMutation`.
        abstract mutationToDom: obj option with get, set
        /// An optional deserialization method for defining how to deserialize the
        /// mutation state. This must be coupled with defining `mutationToDom`.
        abstract domToMutation: TypeLiteral_10 with get, set
        /// An optional property for suppressing adding STATEMENT_PREFIX and
        /// STATEMENT_SUFFIX to generated code.
        abstract suppressPrefixSuffix: bool with get, set
        /// <summary>Dispose of this block.</summary>
        /// <param name="healStack">If true, then try to heal any gap by connecting
        /// the next statement with the previous statement.  Otherwise, dispose of
        /// all children of this block.</param>
        abstract dispose: healStack: bool -> unit
        /// Call initModel on all fields on the block.
        /// May be called more than once.
        /// Either initModel or initSvg must be called after creating a block and before
        /// the first interaction with it.  Interactions include UI actions
        /// (e.g. clicking and dragging) and firing events (e.g. create, delete, and
        /// change).
        abstract initModel: unit -> unit
        /// <summary>Unplug this block from its superior block.  If this block is a statement,
        /// optionally reconnect the block underneath with the block on top.</summary>
        /// <param name="opt_healStack">Disconnect child statement and reconnect
        /// stack.  Defaults to false.</param>
        abstract unplug: ?opt_healStack: bool -> unit
        /// Walks down a stack of blocks and finds the last next connection on the stack.
        abstract lastConnectionInStack: unit -> Blockly.Connection
        /// Bump unconnected blocks out of alignment.  Two blocks which aren't actually
        /// connected should not coincidentally line up on screen.
        abstract bumpNeighbours: unit -> unit
        /// Return the parent block or null if this block is at the top level.
        abstract getParent: unit -> Blockly.Block
        /// <summary>Return the input that connects to the specified block.</summary>
        /// <param name="block">A block connected to an input on this block.</param>
        abstract getInputWithBlock: block: Blockly.Block -> Blockly.Input
        /// Return the parent block that surrounds the current block, or null if this
        /// block has no surrounding block.  A parent block might just be the previous
        /// statement, whereas the surrounding block is an if statement, while loop, etc.
        abstract getSurroundParent: unit -> Blockly.Block
        /// Return the next statement block directly connected to this block.
        abstract getNextBlock: unit -> Blockly.Block
        /// Return the previous statement block directly connected to this block.
        abstract getPreviousBlock: unit -> Blockly.Block
        /// Return the connection on the first statement input on this block, or null if
        /// there are none.
        abstract getFirstStatementConnection: unit -> Blockly.Connection
        /// Return the top-most block in this block's tree.
        /// This will return itself if this block is at the top level.
        abstract getRootBlock: unit -> Blockly.Block
        /// <summary>Find all the blocks that are directly nested inside this one.
        /// Includes value and statement inputs, as well as any following statement.
        /// Excludes any connection on an output tab or any preceding statement.
        /// Blocks are optionally sorted by position; top to bottom.</summary>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getChildren: ordered: bool -> ResizeArray<Blockly.Block>
        /// <summary>Set parent of this block to be a new block or null.</summary>
        /// <param name="newParent">New parent block.</param>
        abstract setParent: newParent: Blockly.Block -> unit
        /// <summary>Find all the blocks that are directly or indirectly nested inside this one.
        /// Includes this block in the list.
        /// Includes value and statement inputs, as well as any following statements.
        /// Excludes any connection on an output tab or any preceding statements.
        /// Blocks are optionally sorted by position; top to bottom.</summary>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getDescendants: ordered: bool -> ResizeArray<Blockly.Block>
        /// Get whether this block is deletable or not.
        abstract isDeletable: unit -> bool
        /// <summary>Set whether this block is deletable or not.</summary>
        /// <param name="deletable">True if deletable.</param>
        abstract setDeletable: deletable: bool -> unit
        /// Get whether this block is movable or not.
        abstract isMovable: unit -> bool
        /// <summary>Set whether this block is movable or not.</summary>
        /// <param name="movable">True if movable.</param>
        abstract setMovable: movable: bool -> unit
        /// Get whether is block is duplicatable or not. If duplicating this block and
        /// descendants will put this block over the workspace's capacity this block is
        /// not duplicatable. If duplicating this block and descendants will put any
        /// type over their maxInstances this block is not duplicatable.
        abstract isDuplicatable: unit -> bool
        /// Get whether this block is a shadow block or not.
        abstract isShadow: unit -> bool
        /// <summary>Set whether this block is a shadow block or not.</summary>
        /// <param name="shadow">True if a shadow.</param>
        abstract setShadow: shadow: bool -> unit
        /// Get whether this block is an insertion marker block or not.
        abstract isInsertionMarker: unit -> bool
        /// <summary>Set whether this block is an insertion marker block or not.
        /// Once set this cannot be unset.</summary>
        /// <param name="insertionMarker">True if an insertion marker.</param>
        abstract setInsertionMarker: insertionMarker: bool -> unit
        /// Get whether this block is editable or not.
        abstract isEditable: unit -> bool
        /// <summary>Set whether this block is editable or not.</summary>
        /// <param name="editable">True if editable.</param>
        abstract setEditable: editable: bool -> unit
        /// <summary>Find the connection on this block that corresponds to the given connection
        /// on the other block.
        /// Used to match connections between a block and its insertion marker.</summary>
        /// <param name="otherBlock">The other block to match against.</param>
        /// <param name="conn">The other connection to match.</param>
        abstract getMatchingConnection: otherBlock: Blockly.Block * conn: Blockly.Connection -> Blockly.Connection
        /// <summary>Set the URL of this block's help page.</summary>
        /// <param name="url">URL string for block help, or function that
        /// returns a URL.  Null for no help.</param>
        abstract setHelpUrl: url: U2<string, Function> -> unit
        /// <summary>Change the tooltip text for a block.</summary>
        /// <param name="newTip">Text for tooltip or a parent element to
        /// link to for its tooltip.  May be a function that returns a string.</param>
        abstract setTooltip: newTip: U2<string, Function> -> unit
        /// Get the colour of a block.
        abstract getColour: unit -> string
        /// Get the secondary colour of a block.
        abstract getColourSecondary: unit -> string
        /// Get the tertiary colour of a block.
        abstract getColourTertiary: unit -> string
        /// Get the shadow colour of a block.
        abstract getColourShadow: unit -> string
        /// Get the border colour(s) of a block.
        abstract getColourBorder: unit -> Block__ClassGetColourBorderReturn
        /// Get the name of the block style.
        abstract getStyleName: unit -> string
        /// Get the HSV hue value of a block.  Null if hue not set.
        abstract getHue: unit -> float
        /// <summary>Change the colour of a block.</summary>
        /// <param name="colour">HSV hue value (0 to 360), #RRGGBB string,
        /// or a message reference string pointing to one of those two values.</param>
        abstract setColour: colour: U2<float, string> -> unit
        /// <summary>Set the style and colour values of a block.</summary>
        /// <param name="blockStyleName">Name of the block style</param>
        abstract setStyle: blockStyleName: string -> unit
        /// <summary>Sets a callback function to use whenever the block's parent workspace
        /// changes, replacing any prior onchange handler. This is usually only called
        /// from the constructor, the block type initializer function, or an extension
        /// initializer function.</summary>
        /// <param name="onchangeFn">The callback to call
        /// when the block's workspace changes.</param>
        abstract setOnChange: onchangeFn: Events.Change -> unit
        //AMO orginal below; type DNE anywhere else
        // abstract setOnChange: onchangeFn: Block__ClassSetOnChangeOnchangeFn -> unit
        /// <summary>Returns the named field from a block.</summary>
        /// <param name="name">The name of the field.</param>
        abstract getField: name: string -> Blockly.Field
        /// Return all variables referenced by this block.
        abstract getVars: unit -> ResizeArray<string>
        /// Return all variables referenced by this block.
        abstract getVarModels: unit -> ResizeArray<Blockly.VariableModel>
        /// <summary>Notification that a variable is renaming but keeping the same ID.  If the
        /// variable is in use on this block, rerender to show the new name.</summary>
        /// <param name="variable">The variable being renamed.</param>
        abstract updateVarName: variable: Blockly.VariableModel -> unit
        /// <summary>Notification that a variable is renaming.
        /// If the ID matches one of this block's variables, rename it.</summary>
        /// <param name="oldId">ID of variable to rename.</param>
        /// <param name="newId">ID of new variable.  May be the same as oldId, but with
        /// an updated name.</param>
        abstract renameVarById: oldId: string * newId: string -> unit
        /// <summary>Returns the language-neutral value from the field of a block.</summary>
        /// <param name="name">The name of the field.</param>
        abstract getFieldValue: name: string -> obj option
        /// <summary>Change the field value for a block (e.g. 'CHOOSE' or 'REMOVE').</summary>
        /// <param name="newValue">Value to be the new field.</param>
        /// <param name="name">The name of the field.</param>
        abstract setFieldValue: newValue: string * name: string -> unit
        /// <summary>Set whether this block can chain onto the bottom of another block.</summary>
        /// <param name="newBoolean">True if there can be a previous statement.</param>
        /// <param name="opt_check">Statement type or
        /// list of statement types.  Null/undefined if any type could be connected.</param>
        abstract setPreviousStatement: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether another block can chain onto the bottom of this block.</summary>
        /// <param name="newBoolean">True if there can be a next statement.</param>
        /// <param name="opt_check">Statement type or
        /// list of statement types.  Null/undefined if any type could be connected.</param>
        abstract setNextStatement: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether this block returns a value.</summary>
        /// <param name="newBoolean">True if there is an output.</param>
        /// <param name="opt_check">Returned type or list
        /// of returned types.  Null or undefined if any type could be returned
        /// (e.g. variable get).</param>
        abstract setOutput: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether value inputs are arranged horizontally or vertically.</summary>
        /// <param name="newBoolean">True if inputs are horizontal.</param>
        abstract setInputsInline: newBoolean: bool -> unit
        /// Get whether value inputs are arranged horizontally or vertically.
        abstract getInputsInline: unit -> bool
        /// <summary>Set whether the block is disabled or not.</summary>
        /// <param name="disabled">True if disabled.</param>
        abstract setDisabled: disabled: bool -> unit
        /// Get whether this block is enabled or not.
        abstract isEnabled: unit -> bool
        /// <summary>Set whether the block is enabled or not.</summary>
        /// <param name="enabled">True if enabled.</param>
        abstract setEnabled: enabled: bool -> unit
        /// Get whether the block is disabled or not due to parents.
        /// The block's own disabled property is not considered.
        abstract getInheritedDisabled: unit -> bool
        /// Get whether the block is collapsed or not.
        abstract isCollapsed: unit -> bool
        /// <summary>Set whether the block is collapsed or not.</summary>
        /// <param name="collapsed">True if collapsed.</param>
        abstract setCollapsed: collapsed: bool -> unit
        /// <summary>Create a human-readable text representation of this block and any children.</summary>
        /// <param name="opt_maxLength">Truncate the string to this length.</param>
        /// <param name="opt_emptyToken">The placeholder string used to denote an
        /// empty field. If not specified, '?' is used.</param>
        abstract toString: ?opt_maxLength: float * ?opt_emptyToken: string -> string
        /// <summary>Shortcut for appending a value input row.</summary>
        /// <param name="name">Language-neutral identifier which may used to find this
        /// input again.  Should be unique to this block.</param>
        abstract appendValueInput: name: string -> Blockly.Input
        /// <summary>Shortcut for appending a statement input row.</summary>
        /// <param name="name">Language-neutral identifier which may used to find this
        /// input again.  Should be unique to this block.</param>
        abstract appendStatementInput: name: string -> Blockly.Input
        /// <summary>Shortcut for appending a dummy input row.</summary>
        /// <param name="opt_name">Language-neutral identifier which may used to find
        /// this input again.  Should be unique to this block.</param>
        abstract appendDummyInput: ?opt_name: string -> Blockly.Input
        /// <summary>Initialize this block using a cross-platform, internationalization-friendly
        /// JSON description.</summary>
        /// <param name="json">Structured data describing the block.</param>
        abstract jsonInit: json: Object -> unit
        /// <summary>Add key/values from mixinObj to this block object. By default, this method
        /// will check that the keys in mixinObj will not overwrite existing values in
        /// the block, including prototype values. This provides some insurance against
        /// mixin / extension incompatibilities with future block features. This check
        /// can be disabled by passing true as the second argument.</summary>
        /// <param name="mixinObj">The key/values pairs to add to this block object.</param>
        /// <param name="opt_disableCheck">Option flag to disable overwrite checks.</param>
        abstract ``mixin``: mixinObj: Object * ?opt_disableCheck: bool -> unit
        /// <summary>Add a value input, statement input or local variable to this block.</summary>
        /// <param name="type">Either Blockly.INPUT_VALUE or Blockly.NEXT_STATEMENT or
        /// Blockly.DUMMY_INPUT.</param>
        /// <param name="name">Language-neutral identifier which may used to find this
        /// input again.  Should be unique to this block.</param>
        abstract appendInput_: ``type``: float * name: string -> Blockly.Input
        /// <summary>Move a named input to a different location on this block.</summary>
        /// <param name="name">The name of the input to move.</param>
        /// <param name="refName">Name of input that should be after the moved input,
        /// or null to be the input at the end.</param>
        abstract moveInputBefore: name: string * refName: string -> unit
        /// <summary>Move a numbered input to a different location on this block.</summary>
        /// <param name="inputIndex">Index of the input to move.</param>
        /// <param name="refIndex">Index of input that should be after the moved input.</param>
        abstract moveNumberedInputBefore: inputIndex: float * refIndex: float -> unit
        /// <summary>Remove an input from this block.</summary>
        /// <param name="name">The name of the input.</param>
        /// <param name="opt_quiet">True to prevent error if input is not present.</param>
        abstract removeInput: name: string * ?opt_quiet: bool -> unit
        /// <summary>Fetches the named input object.</summary>
        /// <param name="name">The name of the input.</param>
        abstract getInput: name: string -> Blockly.Input
        /// <summary>Fetches the block attached to the named input.</summary>
        /// <param name="name">The name of the input.</param>
        abstract getInputTargetBlock: name: string -> Blockly.Block
        /// Returns the comment on this block (or null if there is no comment).
        abstract getCommentText: unit -> string
        /// <summary>Set this block's comment text.</summary>
        /// <param name="text">The text, or null to delete.</param>
        abstract setCommentText: text: string -> unit
        /// <summary>Set this block's warning text.</summary>
        /// <param name="_text">The text, or null to delete.</param>
        /// <param name="_opt_id">An optional ID for the warning text to be able to
        /// maintain multiple warnings.</param>
        abstract setWarningText: _text: string * ?_opt_id: string -> unit
        /// <summary>Give this block a mutator dialog.</summary>
        /// <param name="_mutator">A mutator dialog instance or null to
        /// remove.</param>
        abstract setMutator: _mutator: Blockly.Mutator -> unit
        /// Return the coordinates of the top-left corner of this block relative to the
        /// drawing surface's origin (0,0), in workspace units.
        abstract getRelativeToSurfaceXY: unit -> Blockly.Utils.Coordinate
        /// <summary>Move a block by a relative offset.</summary>
        /// <param name="dx">Horizontal offset, in workspace units.</param>
        /// <param name="dy">Vertical offset, in workspace units.</param>
        abstract moveBy: dx: float * dy: float -> unit
        /// <summary>Recursively checks whether all statement and value inputs are filled with
        /// blocks. Also checks all following statement blocks in this stack.</summary>
        /// <param name="opt_shadowBlocksAreFilled">An optional argument controlling
        /// whether shadow blocks are counted as filled. Defaults to true.</param>
        abstract allInputsFilled: ?opt_shadowBlocksAreFilled: bool -> bool
        /// This method returns a string describing this Block in developer terms (type
        /// name and ID; English only).
        /// 
        /// Intended to on be used in console logs and errors. If you need a string that
        /// uses the user's native language (including block text, field values, and
        /// child blocks), use [toString()]{@link Blockly.Block#toString}.
        abstract toDevString: unit -> string

    type [<AllowNullLiteral>] Block__ClassGetColourBorderReturn =
        abstract colourDark: obj option with get, set
        abstract colourLight: obj option with get, set
        abstract colourBorder: obj option with get, set

    type [<AllowNullLiteral>] Block__ClassSetOnChangeOnchangeFn =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.Events.Abstract -> obj option

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Block__ClassStatic =
        /// <summary>Class for one block.
        /// Not normally called directly, workspace.newBlock() is preferred.</summary>
        /// <param name="workspace">The block's workspace.</param>
        /// <param name="prototypeName">Name of the language object containing
        /// type-specific functions for this block.</param>
        /// <param name="opt_id">Optional ID.  Use this ID if provided, otherwise
        /// create a new ID.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace * prototypeName: string * ?opt_id: string -> Block__Class

    module Block =

        type [<AllowNullLiteral>] CommentModel =
            abstract text: string with get, set
            abstract pinned: bool with get, set
            abstract size: Blockly.Utils.Size with get, set

    module BlockAnimations =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Play some UI effects (sound, animation) when disposing of a block.</summary>
            /// <param name="block">The block being disposed of.</param>
            abstract disposeUiEffect: block: Blockly.BlockSvg -> unit
            /// <summary>Play some UI effects (sound, ripple) after a connection has been established.</summary>
            /// <param name="block">The block being connected.</param>
            abstract connectionUiEffect: block: Blockly.BlockSvg -> unit
            /// <summary>Play some UI effects (sound, animation) when disconnecting a block.</summary>
            /// <param name="block">The block being disconnected.</param>
            abstract disconnectUiEffect: block: Blockly.BlockSvg -> unit
            /// Stop the disconnect UI animation immediately.
            abstract disconnectUiStop: unit -> unit

    type [<AllowNullLiteral>] BlockDragSurfaceSvg =
        inherit BlockDragSurfaceSvg__Class

    type [<AllowNullLiteral>] BlockDragSurfaceSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> BlockDragSurfaceSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockDragSurfaceSvg__Class =
        /// Create the drag surface and inject it into the container.
        abstract createDom: unit -> unit
        /// <summary>Set the SVG blocks on the drag surface's group and show the surface.
        /// Only one block group should be on the drag surface at a time.</summary>
        /// <param name="blocks">Block or group of blocks to place on the drag
        /// surface.</param>
        abstract setBlocksAndShow: blocks: SVGElement -> unit
        /// <summary>Translate and scale the entire drag surface group to the given position, to
        /// keep in sync with the workspace.</summary>
        /// <param name="x">X translation in workspace coordinates.</param>
        /// <param name="y">Y translation in workspace coordinates.</param>
        /// <param name="scale">Scale of the group.</param>
        abstract translateAndScaleGroup: x: float * y: float * scale: float -> unit
        /// <summary>Translate the entire drag surface during a drag.
        /// We translate the drag surface instead of the blocks inside the surface
        /// so that the browser avoids repainting the SVG.
        /// Because of this, the drag coordinates must be adjusted by scale.</summary>
        /// <param name="x">X translation for the entire surface.</param>
        /// <param name="y">Y translation for the entire surface.</param>
        abstract translateSurface: x: float * y: float -> unit
        /// Reports the surface translation in scaled workspace coordinates.
        /// Use this when finishing a drag to return blocks to the correct position.
        abstract getSurfaceTranslation: unit -> Blockly.Utils.Coordinate
        /// Provide a reference to the drag group (primarily for
        /// BlockSvg.getRelativeToSurfaceXY).
        abstract getGroup: unit -> SVGElement
        /// Get the current blocks on the drag surface, if any (primarily
        /// for BlockSvg.getRelativeToSurfaceXY).
        abstract getCurrentBlock: unit -> U2<Element, obj option>
        /// <summary>Clear the group and hide the surface; move the blocks off onto the provided
        /// element.
        /// If the block is being deleted it doesn't need to go back to the original
        /// surface, since it would be removed immediately during dispose.</summary>
        /// <param name="opt_newSurface">Surface the dragging blocks should be moved
        /// to, or null if the blocks should be removed from this surface without
        /// being moved to a different surface.</param>
        abstract clearAndHide: ?opt_newSurface: Element -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockDragSurfaceSvg__ClassStatic =
        /// <summary>Class for a drag surface for the currently dragged block. This is a separate
        /// SVG that contains only the currently moving block, or nothing.</summary>
        /// <param name="container">Containing element.</param>
        [<Emit "new $0($1...)">] abstract Create: container: Element -> BlockDragSurfaceSvg__Class

    type [<AllowNullLiteral>] BlockDragger =
        inherit BlockDragger__Class

    type [<AllowNullLiteral>] BlockDraggerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> BlockDragger

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockDragger__Class =
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// <summary>Start dragging a block.  This includes moving it to the drag surface.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at mouse down, in pixel units.</param>
        /// <param name="healStack">Whether or not to heal the stack after
        /// disconnecting.</param>
        abstract startBlockDrag: currentDragDeltaXY: Blockly.Utils.Coordinate * healStack: bool -> unit
        /// <summary>Execute a step of block dragging, based on the given event.  Update the
        /// display accordingly.</summary>
        /// <param name="e">The most recent move event.</param>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel units.</param>
        abstract dragBlock: e: Event * currentDragDeltaXY: Blockly.Utils.Coordinate -> unit
        /// <summary>Finish a block drag and put the block back on the workspace.</summary>
        /// <param name="e">The mouseup/touchend event.</param>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel units.</param>
        abstract endBlockDrag: e: Event * currentDragDeltaXY: Blockly.Utils.Coordinate -> unit
        /// Get a list of the insertion markers that currently exist.  Drags have 0, 1,
        /// or 2 insertion markers.
        abstract getInsertionMarkers: unit -> ResizeArray<Blockly.BlockSvg>

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockDragger__ClassStatic =
        /// <summary>Class for a block dragger.  It moves blocks around the workspace when they
        /// are being dragged by a mouse or touch.</summary>
        /// <param name="block">The block to drag.</param>
        /// <param name="workspace">The workspace to drag on.</param>
        [<Emit "new $0($1...)">] abstract Create: block: Blockly.BlockSvg * workspace: Blockly.WorkspaceSvg -> BlockDragger__Class

    module Events =

        type [<AllowNullLiteral>] IExports =
            abstract BlockBase: BlockBaseStatic
            abstract BlockBase__Class: BlockBase__ClassStatic
            abstract Change: ChangeStatic
            abstract Change__Class: Change__ClassStatic
            abstract BlockChange: BlockChangeStatic
            abstract BlockChange__Class: BlockChange__ClassStatic
            abstract Create: CreateStatic
            abstract Create__Class: Create__ClassStatic
            abstract BlockCreate: BlockCreateStatic
            abstract BlockCreate__Class: BlockCreate__ClassStatic
            abstract Delete: DeleteStatic
            abstract Delete__Class: Delete__ClassStatic
            abstract BlockDelete: BlockDeleteStatic
            abstract BlockDelete__Class: BlockDelete__ClassStatic
            abstract Move: MoveStatic
            abstract Move__Class: Move__ClassStatic
            abstract BlockMove: BlockMoveStatic
            abstract BlockMove__Class: BlockMove__ClassStatic
            abstract recordUndo: bool
            abstract CREATE: obj option
            abstract BLOCK_CREATE: obj option
            abstract DELETE: obj option
            abstract BLOCK_DELETE: obj option
            abstract CHANGE: obj option
            abstract BLOCK_CHANGE: obj option
            abstract MOVE: obj option
            abstract BLOCK_MOVE: obj option
            abstract VAR_CREATE: obj option
            abstract VAR_DELETE: obj option
            abstract VAR_RENAME: obj option
            abstract UI: obj option
            abstract COMMENT_CREATE: obj option
            abstract COMMENT_DELETE: obj option
            abstract COMMENT_CHANGE: obj option
            abstract COMMENT_MOVE: obj option
            abstract FINISHED_LOADING: obj option
            abstract BUMP_EVENTS: obj option
            /// <summary>Create a custom event and fire it.</summary>
            /// <param name="event">Custom data for event.</param>
            abstract fire: ``event``: Blockly.Events.Abstract -> unit
            /// <summary>Filter the queued events and merge duplicates.</summary>
            /// <param name="queueIn">Array of events.</param>
            /// <param name="forward">True if forward (redo), false if backward (undo).</param>
            abstract filter: queueIn: ResizeArray<Blockly.Events.Abstract> * forward: bool -> ResizeArray<Blockly.Events.Abstract>
            /// Modify pending undo events so that when they are fired they don't land
            /// in the undo stack.  Called by Blockly.Workspace.clearUndo.
            abstract clearPendingUndo: unit -> unit
            /// Stop sending events.  Every call to this function MUST also call enable.
            abstract disable: unit -> unit
            /// Start sending events.  Unless events were already disabled when the
            /// corresponding call to disable was made.
            abstract enable: unit -> unit
            /// Returns whether events may be fired or not.
            abstract isEnabled: unit -> bool
            /// Current group.
            abstract getGroup: unit -> string
            /// <summary>Start or stop a group.</summary>
            /// <param name="state">True to start new group, false to end group.
            /// String to set group explicitly.</param>
            abstract setGroup: state: U2<bool, string> -> unit
            /// <summary>Compute a list of the IDs of the specified block and all its descendants.</summary>
            /// <param name="block">The root block.</param>
            abstract getDescendantIds: block: Blockly.Block -> ResizeArray<string>
            /// <summary>Decode the JSON into an event.</summary>
            /// <param name="json">JSON representation.</param>
            /// <param name="workspace">Target workspace for event.</param>
            abstract fromJson: json: Object * workspace: Blockly.Workspace -> Blockly.Events.Abstract
            /// <summary>Enable/disable a block depending on whether it is properly connected.
            /// Use this on applications where all blocks should be connected to a top block.
            /// Recommend setting the 'disable' option to 'false' in the config so that
            /// users don't try to re-enable disabled orphan blocks.</summary>
            /// <param name="event">Custom data for event.</param>
            abstract disableOrphans: ``event``: Blockly.Events.Abstract -> unit
            abstract Abstract: AbstractStatic
            abstract Abstract__Class: Abstract__ClassStatic
            abstract Ui: UiStatic
            abstract Ui__Class: Ui__ClassStatic
            abstract VarBase: VarBaseStatic
            abstract VarBase__Class: VarBase__ClassStatic
            abstract VarCreate: VarCreateStatic
            abstract VarCreate__Class: VarCreate__ClassStatic
            abstract VarDelete: VarDeleteStatic
            abstract VarDelete__Class: VarDelete__ClassStatic
            abstract VarRename: VarRenameStatic
            abstract VarRename__Class: VarRename__ClassStatic
            abstract FinishedLoading: FinishedLoadingStatic
            abstract FinishedLoading__Class: FinishedLoading__ClassStatic
            abstract CommentBase: CommentBaseStatic
            abstract CommentBase__Class: CommentBase__ClassStatic
            abstract CommentChange: CommentChangeStatic
            abstract CommentChange__Class: CommentChange__ClassStatic
            abstract CommentCreate: CommentCreateStatic
            abstract CommentCreate__Class: CommentCreate__ClassStatic
            abstract CommentDelete: CommentDeleteStatic
            abstract CommentDelete__Class: CommentDelete__ClassStatic
            abstract CommentMove: CommentMoveStatic
            abstract CommentMove__Class: CommentMove__ClassStatic
            /// <summary>Helper function for Comment[Create|Delete]</summary>
            /// <param name="event">The event to run.</param>
            /// <param name="create">if True then Create, if False then Delete</param>
            abstract CommentCreateDeleteHelper: ``event``: U2<Blockly.Events.CommentCreate, Blockly.Events.CommentDelete> * create: bool -> unit

        type [<AllowNullLiteral>] BlockBase =
            inherit BlockBase__Class

        type [<AllowNullLiteral>] BlockBaseStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BlockBase

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockBase__Class =
            inherit Blockly.Events.Abstract__Class
            /// The block id for the block this event pertains to
            abstract blockId: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockBase__ClassStatic =
            /// <summary>Abstract class for a block event.</summary>
            /// <param name="block">The block this event corresponds to.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> BlockBase__Class

        type [<AllowNullLiteral>] Change =
            inherit Change__Class

        type [<AllowNullLiteral>] ChangeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Change
            /// <summary>Class for a block change event.</summary>
            /// <param name="block">The changed block.  Null for a blank event.</param>
            /// <param name="element">One of 'field', 'comment', 'disabled', etc.</param>
            /// <param name="name">Name of input or field affected, or null.</param>
            /// <param name="oldValue">Previous value of element.</param>
            /// <param name="newValue">New value of element.</param>
            /// AMO moved from below
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * name: string * oldValue: obj * newValue: obj -> Change
            // [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * name: string * oldValue: obj option * newValue: obj option -> Change

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Change__Class =
            inherit Blockly.Events.BlockBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// Does this event record any change of state?
            abstract isNull: unit -> bool
            /// <summary>Run a change event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Change__ClassStatic =
            /// <summary>Class for a block change event.</summary>
            /// <param name="block">The changed block.  Null for a blank event.</param>
            /// <param name="element">One of 'field', 'comment', 'disabled', etc.</param>
            /// <param name="name">Name of input or field affected, or null.</param>
            /// <param name="oldValue">Previous value of element.</param>
            /// <param name="newValue">New value of element.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * name: string * oldValue: obj option * newValue: obj option -> Change__Class

        type [<AllowNullLiteral>] BlockChange =
            inherit BlockChange__Class

        type [<AllowNullLiteral>] BlockChangeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BlockChange
            /// <summary>Class for a block change event.</summary>
            /// <param name="block">The changed block.  Null for a blank event.</param>
            /// <param name="element">One of 'field', 'comment', 'disabled', etc.</param>
            /// <param name="name">Name of input or field affected, or null.</param>
            /// <param name="oldValue">Previous value of element.</param>
            /// <param name="newValue">New value of element.</param>
            /// AMO moved from below
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * name: string * oldValue: obj * newValue: obj -> BlockChange


        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockChange__Class =
            inherit Blockly.Events.BlockBase__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockChange__ClassStatic =
            /// <summary>Class for a block change event.</summary>
            /// <param name="block">The changed block.  Null for a blank event.</param>
            /// <param name="element">One of 'field', 'comment', 'disabled', etc.</param>
            /// <param name="name">Name of input or field affected, or null.</param>
            /// <param name="oldValue">Previous value of element.</param>
            /// <param name="newValue">New value of element.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * name: string * oldValue: obj option * newValue: obj option -> BlockChange__Class

        type [<AllowNullLiteral>] Create =
            inherit Create__Class

        type [<AllowNullLiteral>] CreateStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Create

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Create__Class =
            inherit Blockly.Events.BlockBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a creation event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Create__ClassStatic =
            /// <summary>Class for a block creation event.</summary>
            /// <param name="block">The created block.  Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> Create__Class

        type [<AllowNullLiteral>] BlockCreate =
            inherit BlockCreate__Class

        type [<AllowNullLiteral>] BlockCreateStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BlockCreate

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockCreate__Class =
            inherit Blockly.Events.BlockBase__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockCreate__ClassStatic =
            /// <summary>Class for a block creation event.</summary>
            /// <param name="block">The created block. Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> BlockCreate__Class

        type [<AllowNullLiteral>] Delete =
            inherit Delete__Class

        type [<AllowNullLiteral>] DeleteStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Delete

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Delete__Class =
            inherit Blockly.Events.BlockBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a deletion event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Delete__ClassStatic =
            /// <summary>Class for a block deletion event.</summary>
            /// <param name="block">The deleted block.  Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> Delete__Class

        type [<AllowNullLiteral>] BlockDelete =
            inherit BlockDelete__Class

        type [<AllowNullLiteral>] BlockDeleteStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BlockDelete

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockDelete__Class =
            inherit Blockly.Events.BlockBase__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockDelete__ClassStatic =
            /// <summary>Class for a block deletion event.</summary>
            /// <param name="block">The deleted block.  Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> BlockDelete__Class

        type [<AllowNullLiteral>] Move =
            inherit Move__Class

        type [<AllowNullLiteral>] MoveStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Move

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Move__Class =
            inherit Blockly.Events.BlockBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// Record the block's new location.  Called after the move.
            abstract recordNew: unit -> unit
            /// Does this event record any change of state?
            abstract isNull: unit -> bool
            /// <summary>Run a move event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Move__ClassStatic =
            /// <summary>Class for a block move event.  Created before the move.</summary>
            /// <param name="block">The moved block.  Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> Move__Class

        type [<AllowNullLiteral>] BlockMove =
            inherit BlockMove__Class

        type [<AllowNullLiteral>] BlockMoveStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BlockMove

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockMove__Class =
            inherit Blockly.Events.BlockBase__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BlockMove__ClassStatic =
            /// <summary>Class for a block move event.  Created before the move.</summary>
            /// <param name="block">The moved block.  Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> BlockMove__Class

        type [<AllowNullLiteral>] Abstract =
            inherit Abstract__Class

        type [<AllowNullLiteral>] AbstractStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Abstract

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Abstract__Class =
            /// The workspace identifier for this event.
            abstract workspaceId: U2<string, obj option> with get, set
            /// The event group id for the group this event belongs to. Groups define
            /// events that should be treated as an single action from the user's
            /// perspective, and should be undone together.
            abstract group: string with get, set
            /// Sets whether the event should be added to the undo stack.
            abstract recordUndo: bool with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// Does this event record any change of state?
            abstract isNull: unit -> bool
            /// <summary>Run an event.</summary>
            /// <param name="_forward">True if run forward, false if run backward (undo).</param>
            abstract run: _forward: bool -> unit
            /// Get workspace the event belongs to.
            abstract getEventWorkspace_: unit -> Blockly.Workspace

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Abstract__ClassStatic =
            /// Abstract class for an event.
            [<Emit "new $0($1...)">] abstract Create: unit -> Abstract__Class

        type [<AllowNullLiteral>] Ui =
            inherit Ui__Class

        type [<AllowNullLiteral>] UiStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Ui

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Ui__Class =
            inherit Blockly.Events.Abstract__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Ui__ClassStatic =
            /// <summary>Class for a UI event.
            /// UI events are events that don't need to be sent over the wire for multi-user
            /// editing to work (e.g. scrolling the workspace, zooming, opening toolbox
            /// categories).
            /// UI events do not undo or redo.</summary>
            /// <param name="block">The affected block.</param>
            /// <param name="element">One of 'selected', 'comment', 'mutatorOpen', etc.</param>
            /// <param name="oldValue">Previous value of element.</param>
            /// <param name="newValue">New value of element.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block * element: string * oldValue: obj option * newValue: obj option -> Ui__Class

        type [<AllowNullLiteral>] VarBase =
            inherit VarBase__Class

        type [<AllowNullLiteral>] VarBaseStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> VarBase

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarBase__Class =
            inherit Blockly.Events.Abstract__Class
            /// The variable id for the variable this event pertains to.
            abstract varId: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarBase__ClassStatic =
            /// <summary>Abstract class for a variable event.</summary>
            /// <param name="variable">The variable this event corresponds
            /// to.</param>
            [<Emit "new $0($1...)">] abstract Create: variable: Blockly.VariableModel -> VarBase__Class

        type [<AllowNullLiteral>] VarCreate =
            inherit VarCreate__Class

        type [<AllowNullLiteral>] VarCreateStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> VarCreate

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarCreate__Class =
            inherit Blockly.Events.VarBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a variable creation event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarCreate__ClassStatic =
            /// <summary>Class for a variable creation event.</summary>
            /// <param name="variable">The created variable.
            /// Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: variable: Blockly.VariableModel -> VarCreate__Class

        type [<AllowNullLiteral>] VarDelete =
            inherit VarDelete__Class

        type [<AllowNullLiteral>] VarDeleteStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> VarDelete

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarDelete__Class =
            inherit Blockly.Events.VarBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a variable deletion event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarDelete__ClassStatic =
            /// <summary>Class for a variable deletion event.</summary>
            /// <param name="variable">The deleted variable.
            /// Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: variable: Blockly.VariableModel -> VarDelete__Class

        type [<AllowNullLiteral>] VarRename =
            inherit VarRename__Class

        type [<AllowNullLiteral>] VarRenameStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> VarRename

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarRename__Class =
            inherit Blockly.Events.VarBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a variable rename event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] VarRename__ClassStatic =
            /// <summary>Class for a variable rename event.</summary>
            /// <param name="variable">The renamed variable.
            /// Null for a blank event.</param>
            /// <param name="newName">The new name the variable will be changed to.</param>
            [<Emit "new $0($1...)">] abstract Create: variable: Blockly.VariableModel * newName: string -> VarRename__Class

        type [<AllowNullLiteral>] FinishedLoading =
            inherit FinishedLoading__Class

        type [<AllowNullLiteral>] FinishedLoadingStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> FinishedLoading

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] FinishedLoading__Class =
            inherit Blockly.Events.Abstract__Class
            /// The workspace identifier for this event.
            abstract workspaceId: string with get, set
            /// The event group id for the group this event belongs to. Groups define
            /// events that should be treated as an single action from the user's
            /// perspective, and should be undone together.
            abstract group: string with get, set
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] FinishedLoading__ClassStatic =
            /// <summary>Class for a finished loading event.
            /// Used to notify the developer when the workspace has finished loading (i.e
            /// domToWorkspace).
            /// Finished loading events do not record undo or redo.</summary>
            /// <param name="workspace">The workspace that has finished
            /// loading.</param>
            [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace -> FinishedLoading__Class

        type [<AllowNullLiteral>] CommentBase =
            inherit CommentBase__Class

        type [<AllowNullLiteral>] CommentBaseStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> CommentBase

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentBase__Class =
            inherit Blockly.Events.Abstract__Class
            /// The ID of the comment this event pertains to.
            abstract commentId: string with get, set
            /// The workspace identifier for this event.
            abstract workspaceId: string with get, set
            /// The event group id for the group this event belongs to. Groups define
            /// events that should be treated as an single action from the user's
            /// perspective, and should be undone together.
            abstract group: string with get, set
            /// Sets whether the event should be added to the undo stack.
            abstract recordUndo: bool with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentBase__ClassStatic =
            /// <summary>Abstract class for a comment event.</summary>
            /// <param name="comment">The comment this event corresponds
            /// to.</param>
            [<Emit "new $0($1...)">] abstract Create: comment: Blockly.WorkspaceComment -> CommentBase__Class

        type [<AllowNullLiteral>] CommentChange =
            inherit CommentChange__Class

        type [<AllowNullLiteral>] CommentChangeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> CommentChange

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentChange__Class =
            inherit Blockly.Events.CommentBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// Does this event record any change of state?
            abstract isNull: unit -> bool
            /// <summary>Run a change event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentChange__ClassStatic =
            /// <summary>Class for a comment change event.</summary>
            /// <param name="comment">The comment that is being changed.
            /// Null for a blank event.</param>
            /// <param name="oldContents">Previous contents of the comment.</param>
            /// <param name="newContents">New contents of the comment.</param>
            [<Emit "new $0($1...)">] abstract Create: comment: Blockly.WorkspaceComment * oldContents: string * newContents: string -> CommentChange__Class

        type [<AllowNullLiteral>] CommentCreate =
            inherit CommentCreate__Class

        type [<AllowNullLiteral>] CommentCreateStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> CommentCreate

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentCreate__Class =
            inherit Blockly.Events.CommentBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a creation event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentCreate__ClassStatic =
            /// <summary>Class for a comment creation event.</summary>
            /// <param name="comment">The created comment.
            /// Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: comment: Blockly.WorkspaceComment -> CommentCreate__Class

        type [<AllowNullLiteral>] CommentDelete =
            inherit CommentDelete__Class

        type [<AllowNullLiteral>] CommentDeleteStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> CommentDelete

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentDelete__Class =
            inherit Blockly.Events.CommentBase__Class
            /// Type of this event.
            abstract ``type``: string with get, set
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// <summary>Run a creation event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentDelete__ClassStatic =
            /// <summary>Class for a comment deletion event.</summary>
            /// <param name="comment">The deleted comment.
            /// Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: comment: Blockly.WorkspaceComment -> CommentDelete__Class

        type [<AllowNullLiteral>] CommentMove =
            inherit CommentMove__Class

        type [<AllowNullLiteral>] CommentMoveStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> CommentMove

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentMove__Class =
            inherit Blockly.Events.CommentBase__Class
            /// The comment that is being moved.  Will be cleared after recording the new
            /// location.
            abstract comment_: Blockly.WorkspaceComment with get, set
            /// The location before the move, in workspace coordinates.
            abstract oldCoordinate_: Blockly.Utils.Coordinate with get, set
            /// The location after the move, in workspace coordinates.
            abstract newCoordinate_: Blockly.Utils.Coordinate with get, set
            /// Record the comment's new location.  Called after the move.  Can only be
            /// called once.
            abstract recordNew: unit -> unit
            /// Type of this event.
            abstract ``type``: string with get, set
            /// <summary>Override the location before the move.  Use this if you don't create the
            /// event until the end of the move, but you know the original location.</summary>
            /// <param name="xy">The location before the move,
            /// in workspace coordinates.</param>
            abstract setOldCoordinate: xy: Blockly.Utils.Coordinate -> unit
            /// Encode the event as JSON.
            abstract toJson: unit -> Object
            /// <summary>Decode the JSON event.</summary>
            /// <param name="json">JSON representation.</param>
            abstract fromJson: json: Object -> unit
            /// Does this event record any change of state?
            abstract isNull: unit -> bool
            /// <summary>Run a move event.</summary>
            /// <param name="forward">True if run forward, false if run backward (undo).</param>
            abstract run: forward: bool -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] CommentMove__ClassStatic =
            /// <summary>Class for a comment move event.  Created before the move.</summary>
            /// <param name="comment">The comment that is being moved.
            /// Null for a blank event.</param>
            [<Emit "new $0($1...)">] abstract Create: comment: Blockly.WorkspaceComment -> CommentMove__Class

    type [<AllowNullLiteral>] BlockSvg =
        inherit BlockSvg__Class

    type [<AllowNullLiteral>] BlockSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> BlockSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockSvg__Class =
        inherit Blockly.Block__Class
        /// The renderer's path object.
        abstract pathObject: Blockly.BlockRendering.IPathObject with get, set
        abstract rendered: bool with get, set
        /// Height of this block, not including any statement blocks above or below.
        /// Height is in workspace units.
        abstract height: obj option with get, set
        /// Width of this block, including any connected value blocks.
        /// Width is in workspace units.
        abstract width: obj option with get, set
        /// An optional method called when a mutator dialog is first opened.
        /// This function must create and initialize a top-level block for the mutator
        /// dialog, and return it. This function should also populate this top-level
        /// block with any sub-blocks which are appropriate. This method must also be
        /// coupled with defining a `compose` method for the default mutation dialog
        /// button and UI to appear.
        abstract decompose: TypeLiteral_11 with get, set
        /// An optional method called when a mutator dialog saves its content.
        /// This function is called to modify the original block according to new
        /// settings. This method must also be coupled with defining a `decompose`
        /// method for the default mutation dialog button and UI to appear.
        abstract compose: TypeLiteral_12 with get, set
        /// Create and initialize the SVG representation of the block.
        /// May be called more than once.
        abstract initSvg: unit -> unit
        /// Select this block.  Highlight it visually.
        abstract select: unit -> unit
        /// Unselect this block.  Remove its highlighting.
        abstract unselect: unit -> unit
        /// Block's mutator icon (if any).
        abstract mutator: Blockly.Mutator with get, set
        /// Block's comment icon (if any).
        abstract comment: Blockly.Comment with get, set
        /// Block's warning icon (if any).
        abstract warning: Blockly.Warning with get, set
        /// Returns a list of mutator, comment, and warning icons.
        abstract getIcons: unit -> ResizeArray<obj option>
        /// Return the coordinates of the top-left corner of this block relative to the
        /// drawing surface's origin (0,0), in workspace units.
        /// If the block is on the workspace, (0, 0) is the origin of the workspace
        /// coordinate system.
        /// This does not change with workspace scale.
        abstract getRelativeToSurfaceXY: unit -> Blockly.Utils.Coordinate
        /// <summary>Move a block by a relative offset.</summary>
        /// <param name="dx">Horizontal offset in workspace units.</param>
        /// <param name="dy">Vertical offset in workspace units.</param>
        abstract moveBy: dx: float * dy: float -> unit
        /// <summary>Transforms a block by setting the translation on the transform attribute
        /// of the block's SVG.</summary>
        /// <param name="x">The x coordinate of the translation in workspace units.</param>
        /// <param name="y">The y coordinate of the translation in workspace units.</param>
        abstract translate: x: float * y: float -> unit
        /// <summary>Move a block to a position.</summary>
        /// <param name="xy">The position to move to in workspace units.</param>
        abstract moveTo: xy: Blockly.Utils.Coordinate -> unit
        /// <summary>Move this block during a drag, taking into account whether we are using a
        /// drag surface to translate blocks.
        /// This block must be a top-level block.</summary>
        /// <param name="newLoc">The location to translate to, in
        /// workspace coordinates.</param>
        abstract moveDuringDrag: newLoc: Blockly.Utils.Coordinate -> unit
        /// Snap this block to the nearest grid point.
        abstract snapToGrid: unit -> unit
        /// Returns the coordinates of a bounding box describing the dimensions of this
        /// block and any blocks stacked below it.
        /// Coordinate system: workspace coordinates.
        abstract getBoundingRectangle: unit -> Blockly.Utils.Rect
        /// Notify every input on this block to mark its fields as dirty.
        /// A dirty field is a field that needs to be re-rendererd.
        abstract markDirty: unit -> unit
        /// <summary>Set whether the block is collapsed or not.</summary>
        /// <param name="collapsed">True if collapsed.</param>
        abstract setCollapsed: collapsed: bool -> unit
        /// <summary>Open the next (or previous) FieldTextInput.</summary>
        /// <param name="start">Current location.</param>
        /// <param name="forward">If true go forward, otherwise backward.</param>
        abstract tab: start: U2<Blockly.Field, Blockly.Block> * forward: bool -> unit
        /// Generate the context menu for this block.
        abstract generateContextMenu: unit -> ResizeArray<Object>
        /// <summary>Recursively adds or removes the dragging class to this node and its children.</summary>
        /// <param name="adding">True if adding, false if removing.</param>
        abstract setDragging: adding: bool -> unit
        /// Add or remove the UI indicating if this block is movable or not.
        abstract updateMovable: unit -> unit
        /// <summary>Set whether this block is movable or not.</summary>
        /// <param name="movable">True if movable.</param>
        abstract setMovable: movable: bool -> unit
        /// <summary>Set whether this block is editable or not.</summary>
        /// <param name="editable">True if editable.</param>
        abstract setEditable: editable: bool -> unit
        /// <summary>Set whether this block is a shadow block or not.</summary>
        /// <param name="shadow">True if a shadow.</param>
        abstract setShadow: shadow: bool -> unit
        /// <summary>Set whether this block is an insertion marker block or not.
        /// Once set this cannot be unset.</summary>
        /// <param name="insertionMarker">True if an insertion marker.</param>
        abstract setInsertionMarker: insertionMarker: bool -> unit
        /// Return the root node of the SVG or null if none exists.
        abstract getSvgRoot: unit -> SVGElement
        /// <summary>Dispose of this block.</summary>
        /// <param name="healStack">If true, then try to heal any gap by connecting
        /// the next statement with the previous statement.  Otherwise, dispose of
        /// all children of this block.</param>
        /// <param name="animate">If true, show a disposal animation and sound.</param>
        abstract dispose: ?healStack: bool * ?animate: bool -> unit
        /// Change the colour of a block.
        abstract updateColour: unit -> unit
        /// Sets the colour of the border.
        /// Removes the light and dark paths if a border colour is defined.
        abstract setBorderColour_: unit -> unit
        /// Sets the colour of shadow blocks.
        abstract setShadowColour_: unit -> string
        /// Enable or disable a block.
        abstract updateDisabled: unit -> unit
        /// Get the comment icon attached to this block, or null if the block has no
        /// comment.
        abstract getCommentIcon: unit -> Blockly.Comment
        /// <summary>Set this block's comment text.</summary>
        /// <param name="text">The text, or null to delete.</param>
        abstract setCommentText: text: string -> unit
        /// <summary>Set this block's warning text.</summary>
        /// <param name="text">The text, or null to delete.</param>
        /// <param name="opt_id">An optional ID for the warning text to be able to
        /// maintain multiple warnings.</param>
        abstract setWarningText: text: string * ?opt_id: string -> unit
        /// <summary>Give this block a mutator dialog.</summary>
        /// <param name="mutator">A mutator dialog instance or null to remove.</param>
        abstract setMutator: mutator: Blockly.Mutator -> unit
        /// <summary>Set whether the block is disabled or not.</summary>
        /// <param name="disabled">True if disabled.</param>
        abstract setDisabled: disabled: bool -> unit
        /// <summary>Set whether the block is enabled or not.</summary>
        /// <param name="enabled">True if enabled.</param>
        abstract setEnabled: enabled: bool -> unit
        /// <summary>Set whether the block is highlighted or not.  Block highlighting is
        /// often used to visually mark blocks currently being executed.</summary>
        /// <param name="highlighted">True if highlighted.</param>
        abstract setHighlighted: highlighted: bool -> unit
        /// Select this block.  Highlight it visually.
        abstract addSelect: unit -> unit
        /// Unselect this block.  Remove its highlighting.
        abstract removeSelect: unit -> unit
        /// <summary>Update the cursor over this block by adding or removing a class.</summary>
        /// <param name="enable">True if the delete cursor should be shown, false
        /// otherwise.</param>
        abstract setDeleteStyle: enable: bool -> unit
        /// <summary>Change the colour of a block.</summary>
        /// <param name="colour">HSV hue value, or #RRGGBB string.</param>
        abstract setColour: colour: U2<float, string> -> unit
        /// Move this block to the front of the visible workspace.
        /// <g> tags do not respect z-index so SVG renders them in the
        /// order that they are in the DOM.  By placing this block first within the
        /// block group's <g>, it will render on top of any other blocks.
        abstract bringToFront: unit -> unit
        /// <summary>Set whether this block can chain onto the bottom of another block.</summary>
        /// <param name="newBoolean">True if there can be a previous statement.</param>
        /// <param name="opt_check">Statement type or
        /// list of statement types.  Null/undefined if any type could be connected.</param>
        abstract setPreviousStatement: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether another block can chain onto the bottom of this block.</summary>
        /// <param name="newBoolean">True if there can be a next statement.</param>
        /// <param name="opt_check">Statement type or
        /// list of statement types.  Null/undefined if any type could be connected.</param>
        abstract setNextStatement: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether this block returns a value.</summary>
        /// <param name="newBoolean">True if there is an output.</param>
        /// <param name="opt_check">Returned type or list
        /// of returned types.  Null or undefined if any type could be returned
        /// (e.g. variable get).</param>
        abstract setOutput: newBoolean: bool * ?opt_check: U3<string, ResizeArray<string>, obj option> -> unit
        /// <summary>Set whether value inputs are arranged horizontally or vertically.</summary>
        /// <param name="newBoolean">True if inputs are horizontal.</param>
        abstract setInputsInline: newBoolean: bool -> unit
        /// <summary>Remove an input from this block.</summary>
        /// <param name="name">The name of the input.</param>
        /// <param name="opt_quiet">True to prevent error if input is not present.</param>
        abstract removeInput: name: string * ?opt_quiet: bool -> unit
        /// <summary>Move a numbered input to a different location on this block.</summary>
        /// <param name="inputIndex">Index of the input to move.</param>
        /// <param name="refIndex">Index of input that should be after the moved input.</param>
        abstract moveNumberedInputBefore: inputIndex: float * refIndex: float -> unit
        /// <summary>Set whether the connections are hidden (not tracked in a database) or not.
        /// Recursively walk down all child blocks (except collapsed blocks).</summary>
        /// <param name="hidden">True if connections are hidden.</param>
        abstract setConnectionsHidden: hidden: bool -> unit
        /// <summary>Returns connections originating from this block.</summary>
        /// <param name="all">If true, return all connections even hidden ones.
        /// Otherwise, for a non-rendered block return an empty list, and for a
        /// collapsed block don't return inputs connections.</param>
        abstract getConnections_: all: bool -> ResizeArray<Blockly.Connection>
        /// Bump unconnected blocks out of alignment.  Two blocks which aren't actually
        /// connected should not coincidentally line up on screen.
        abstract bumpNeighbours: unit -> unit
        /// Schedule snapping to grid and bumping neighbours to occur after a brief
        /// delay.
        abstract scheduleSnapAndBump: unit -> unit
        /// <summary>Position a block so that it doesn't move the target block when connected.
        /// The block to position is usually either the first block in a dragged stack or
        /// an insertion marker.</summary>
        /// <param name="sourceConnection">The connection on the moving
        /// block's stack.</param>
        /// <param name="targetConnection">The connection that should stay
        /// stationary as this block is positioned.</param>
        abstract positionNearConnection: sourceConnection: Blockly.Connection * targetConnection: Blockly.Connection -> unit
        /// <summary>Render the block.
        /// Lays out and reflows a block based on its contents and settings.</summary>
        /// <param name="opt_bubble">If false, just render this block.
        /// If true, also render block's parent, grandparent, etc.  Defaults to true.</param>
        abstract render: ?opt_bubble: bool -> unit
        /// <summary>Add the cursor svg to this block's svg group.</summary>
        /// <param name="cursorSvg">The svg root of the cursor to be added to the
        /// block svg group.</param>
        abstract setCursorSvg: cursorSvg: SVGElement -> unit
        /// <summary>Add the marker svg to this block's svg group.</summary>
        /// <param name="markerSvg">The svg root of the marker to be added to the
        /// block svg group.</param>
        abstract setMarkerSvg: markerSvg: SVGElement -> unit
        /// Returns a bounding box describing the dimensions of this block
        /// and any blocks stacked below it.
        abstract getHeightWidth: unit -> BlockSvg__ClassGetHeightWidthReturn
        /// <summary>Position a new block correctly, so that it doesn't move the existing block
        /// when connected to it.</summary>
        /// <param name="newBlock">The block to position - either the first
        /// block in a dragged stack or an insertion marker.</param>
        /// <param name="newConnection">The connection on the new block's
        /// stack - either a connection on newBlock, or the last NEXT_STATEMENT
        /// connection on the stack if the stack's being dropped before another
        /// block.</param>
        /// <param name="existingConnection">The connection on the
        /// existing block, which newBlock should line up with.</param>
        abstract positionNewBlock: newBlock: Blockly.Block * newConnection: Blockly.Connection * existingConnection: Blockly.Connection -> unit
        /// <summary>Visual effect to show that if the dragging block is dropped, this block will
        /// be replaced.  If a shadow block, it will disappear.  Otherwise it will bump.</summary>
        /// <param name="add">True if highlighting should be added.</param>
        abstract highlightForReplacement: add: bool -> unit

    type [<AllowNullLiteral>] BlockSvg__ClassGetHeightWidthReturn =
        abstract height: float with get, set
        abstract width: float with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BlockSvg__ClassStatic =
        /// <summary>Class for a block's SVG representation.
        /// Not normally called directly, workspace.newBlock() is preferred.</summary>
        /// <param name="workspace">The block's workspace.</param>
        /// <param name="prototypeName">Name of the language object containing
        /// type-specific functions for this block.</param>
        /// <param name="opt_id">Optional ID.  Use this ID if provided, otherwise
        /// create a new ID.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg * prototypeName: string * ?opt_id: string -> BlockSvg__Class

    module BlockSvg =

        type [<AllowNullLiteral>] IExports =
            abstract INLINE: obj option
            abstract COLLAPSED_WARNING_ID: string
            abstract SEP_SPACE_Y: obj option
            abstract MIN_BLOCK_Y: obj option
            abstract TAB_WIDTH: obj option
            abstract START_HAT: obj option

    type [<AllowNullLiteral>] Bubble =
        inherit Bubble__Class

    type [<AllowNullLiteral>] BubbleStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Bubble

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Bubble__Class =
        /// Function to call on resize of bubble.
        abstract resizeCallback_: Function with get, set
        /// Return the root node of the bubble's SVG group.
        abstract getSvgRoot: unit -> SVGElement
        /// <summary>Expose the block's ID on the bubble's top-level SVG group.</summary>
        /// <param name="id">ID of block.</param>
        abstract setSvgId: id: string -> unit
        /// Get whether this bubble is deletable or not.
        abstract isDeletable: unit -> bool
        /// <summary>Register a function as a callback event for when the bubble is resized.</summary>
        /// <param name="callback">The function to call on resize.</param>
        abstract registerResizeEvent: callback: Function -> unit
        /// <summary>Notification that the anchor has moved.
        /// Update the arrow and bubble accordingly.</summary>
        /// <param name="xy">Absolute location.</param>
        abstract setAnchorLocation: xy: Blockly.Utils.Coordinate -> unit
        /// <summary>Move the bubble group to the specified location in workspace coordinates.</summary>
        /// <param name="x">The x position to move to.</param>
        /// <param name="y">The y position to move to.</param>
        abstract moveTo: x: float * y: float -> unit
        /// Get the dimensions of this bubble.
        abstract getBubbleSize: unit -> Blockly.Utils.Size
        /// <summary>Size this bubble.</summary>
        /// <param name="width">Width of the bubble.</param>
        /// <param name="height">Height of the bubble.</param>
        abstract setBubbleSize: width: float * height: float -> unit
        /// <summary>Change the colour of a bubble.</summary>
        /// <param name="hexColour">Hex code of colour.</param>
        abstract setColour: hexColour: string -> unit
        /// Dispose of this bubble.
        abstract dispose: unit -> unit
        /// <summary>Move this bubble during a drag, taking into account whether or not there is
        /// a drag surface.</summary>
        /// <param name="dragSurface">The surface that carries
        /// rendered items during a drag, or null if no drag surface is in use.</param>
        /// <param name="newLoc">The location to translate to, in
        /// workspace coordinates.</param>
        abstract moveDuringDrag: dragSurface: Blockly.BlockDragSurfaceSvg * newLoc: Blockly.Utils.Coordinate -> unit
        /// Return the coordinates of the top-left corner of this bubble's body relative
        /// to the drawing surface's origin (0,0), in workspace units.
        abstract getRelativeToSurfaceXY: unit -> Blockly.Utils.Coordinate
        /// <summary>Set whether auto-layout of this bubble is enabled.  The first time a bubble
        /// is shown it positions itself to not cover any blocks.  Once a user has
        /// dragged it to reposition, it renders where the user put it.</summary>
        /// <param name="enable">True if auto-layout should be enabled, false
        /// otherwise.</param>
        abstract setAutoLayout: enable: bool -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Bubble__ClassStatic =
        /// <summary>Class for UI bubble.</summary>
        /// <param name="workspace">The workspace on which to draw the
        /// bubble.</param>
        /// <param name="content">SVG content for the bubble.</param>
        /// <param name="shape">SVG element to avoid eclipsing.</param>
        /// <param name="anchorXY">Absolute position of bubble's
        /// anchor point.</param>
        /// <param name="bubbleWidth">Width of bubble, or null if not resizable.</param>
        /// <param name="bubbleHeight">Height of bubble, or null if not resizable.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg * content: Element * shape: Element * anchorXY: Blockly.Utils.Coordinate * bubbleWidth: float * bubbleHeight: float -> Bubble__Class

    module Bubble =

        type [<AllowNullLiteral>] IExports =
            abstract BORDER_WIDTH: obj option
            abstract ARROW_THICKNESS: obj option
            abstract ARROW_ANGLE: obj option
            abstract ARROW_BEND: obj option
            abstract ANCHOR_RADIUS: obj option

    type [<AllowNullLiteral>] BubbleDragger =
        inherit BubbleDragger__Class

    type [<AllowNullLiteral>] BubbleDraggerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> BubbleDragger

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BubbleDragger__Class =
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// Start dragging a bubble.  This includes moving it to the drag surface.
        abstract startBubbleDrag: unit -> unit
        /// <summary>Execute a step of bubble dragging, based on the given event.  Update the
        /// display accordingly.</summary>
        /// <param name="e">The most recent move event.</param>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel units.</param>
        abstract dragBubble: e: Event * currentDragDeltaXY: Blockly.Utils.Coordinate -> unit
        /// <summary>Finish a bubble drag and put the bubble back on the workspace.</summary>
        /// <param name="e">The mouseup/touchend event.</param>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel units.</param>
        abstract endBubbleDrag: e: Event * currentDragDeltaXY: Blockly.Utils.Coordinate -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] BubbleDragger__ClassStatic =
        /// <summary>Class for a bubble dragger.  It moves things on the bubble canvas around the
        /// workspace when they are being dragged by a mouse or touch.  These can be
        /// block comments, mutators, warnings, or workspace comments.</summary>
        /// <param name="bubble">The item on the
        /// bubble canvas to drag.</param>
        /// <param name="workspace">The workspace to drag on.</param>
        [<Emit "new $0($1...)">] abstract Create: bubble: U2<Blockly.Bubble, Blockly.WorkspaceCommentSvg> * workspace: Blockly.WorkspaceSvg -> BubbleDragger__Class

    type [<AllowNullLiteral>] Comment =
        inherit Comment__Class

    type [<AllowNullLiteral>] CommentStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Comment

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Comment__Class =
        inherit Blockly.Icon__Class
        /// <summary>Show or hide the comment bubble.</summary>
        /// <param name="visible">True if the bubble should be visible.</param>
        abstract setVisible: visible: bool -> unit
        /// Get the dimensions of this comment's bubble.
        abstract getBubbleSize: unit -> Blockly.Utils.Size
        /// <summary>Size this comment's bubble.</summary>
        /// <param name="width">Width of the bubble.</param>
        /// <param name="height">Height of the bubble.</param>
        abstract setBubbleSize: width: float * height: float -> unit
        /// Returns this comment's text.
        abstract getText: unit -> string
        /// <summary>Set this comment's text.
        /// 
        /// If you want to receive a comment change event, then this should not be called
        /// directly. Instead call block.setCommentText();</summary>
        /// <param name="text">Comment text.</param>
        abstract setText: text: string -> unit
        /// Update the comment's view to match the model.
        abstract updateText: unit -> unit
        /// Dispose of this comment.
        /// 
        /// If you want to receive a comment "delete" event (newValue: null), then this
        /// should not be called directly. Instead call block.setCommentText(null);
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Comment__ClassStatic =
        /// <summary>Class for a comment.</summary>
        /// <param name="block">The block associated with this comment.</param>
        [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> Comment__Class

    type [<AllowNullLiteral>] Connection =
        inherit Connection__Class

    type [<AllowNullLiteral>] ConnectionStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Connection

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Connection__Class =
        abstract sourceBlock_: Blockly.Block with get, set
        abstract ``type``: float with get, set
        /// Connection this connection connects to.  Null if not connected.
        abstract targetConnection: Blockly.Connection with get, set
        /// Has this connection been disposed of?
        abstract disposed: bool with get, set
        /// Horizontal location of this connection.
        abstract x_: float with get, set
        /// Vertical location of this connection.
        abstract y_: float with get, set
        /// <summary>Connect two connections together.  This is the connection on the superior
        /// block.</summary>
        /// <param name="childConnection">Connection on inferior block.</param>
        abstract connect_: childConnection: Blockly.Connection -> unit
        /// Dispose of this connection. Deal with connected blocks and remove this
        /// connection from the database.
        abstract dispose: unit -> unit
        /// Get the source block for this connection.
        abstract getSourceBlock: unit -> Blockly.Block
        /// Does the connection belong to a superior block (higher in the source stack)?
        abstract isSuperior: unit -> bool
        /// Is the connection connected?
        abstract isConnected: unit -> bool
        /// <summary>Check if the two connections can be dragged to connect to each other.</summary>
        /// <param name="candidate">A nearby connection to check.</param>
        abstract isConnectionAllowed: candidate: Blockly.Connection -> bool
        /// <summary>Behavior after a connection attempt fails.</summary>
        /// <param name="_otherConnection">Connection that this connection
        /// failed to connect to.</param>
        abstract onFailedConnect: _otherConnection: Blockly.Connection -> unit
        /// <summary>Connect this connection to another connection.</summary>
        /// <param name="otherConnection">Connection to connect to.</param>
        abstract connect: otherConnection: Blockly.Connection -> unit
        /// Disconnect this connection.
        abstract disconnect: unit -> unit
        /// <summary>Disconnect two blocks that are connected by this connection.</summary>
        /// <param name="parentBlock">The superior block.</param>
        /// <param name="childBlock">The inferior block.</param>
        abstract disconnectInternal_: parentBlock: Blockly.Block * childBlock: Blockly.Block -> unit
        /// Respawn the shadow block if there was one connected to the this connection.
        abstract respawnShadow_: unit -> unit
        /// Returns the block that this connection connects to.
        abstract targetBlock: unit -> Blockly.Block
        /// <summary>Is this connection compatible with another connection with respect to the
        /// value type system.  E.g. square_root("Hello") is not compatible.</summary>
        /// <param name="otherConnection">Connection to compare against.</param>
        abstract checkType_: otherConnection: Blockly.Connection -> bool
        /// <summary>Change a connection's compatibility.</summary>
        /// <param name="check">Compatible value type or list of value
        /// types. Null if all types are compatible.</param>
        abstract setCheck: check: U2<string, ResizeArray<string>> -> Blockly.Connection
        /// Get a connection's compatibility.
        abstract getCheck: unit -> ResizeArray<obj option>
        /// <summary>Change a connection's shadow block.</summary>
        /// <param name="shadow">DOM representation of a block or null.</param>
        abstract setShadowDom: shadow: Element -> unit
        /// Return a connection's shadow block.
        abstract getShadowDom: unit -> Element
        /// Get the parent input of a connection.
        abstract getParentInput: unit -> Blockly.Input
        /// This method returns a string describing this Connection in developer terms
        /// (English only). Intended to on be used in console logs and errors.
        abstract toString: unit -> string

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Connection__ClassStatic =
        /// <summary>Class for a connection between blocks.</summary>
        /// <param name="source">The block establishing this connection.</param>
        /// <param name="type">The type of the connection.</param>
        [<Emit "new $0($1...)">] abstract Create: source: Blockly.Block * ``type``: float -> Connection__Class

    module Connection =

        type [<AllowNullLiteral>] IExports =
            abstract CAN_CONNECT: obj option

    type [<AllowNullLiteral>] ConnectionDB =
        inherit ConnectionDB__Class

    type [<AllowNullLiteral>] ConnectionDBStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> ConnectionDB

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ConnectionDB__Class =
        /// <summary>Add a connection to the database.  Must not already exist in DB.</summary>
        /// <param name="connection">The connection to be added.</param>
        abstract addConnection: connection: Blockly.Connection -> unit
        /// <summary>Find the given connection.
        /// Starts by doing a binary search to find the approximate location, then
        ///      linearly searches nearby for the exact connection.</summary>
        /// <param name="conn">The connection to find.</param>
        abstract findConnection: conn: Blockly.Connection -> float
        /// <summary>Find all nearby connections to the given connection.
        /// Type checking does not apply, since this function is used for bumping.</summary>
        /// <param name="connection">The connection whose neighbours
        /// should be returned.</param>
        /// <param name="maxRadius">The maximum radius to another connection.</param>
        abstract getNeighbours: connection: Blockly.Connection * maxRadius: float -> ResizeArray<Blockly.Connection>
        /// <summary>Find the closest compatible connection to this connection.</summary>
        /// <param name="conn">The connection searching for a compatible
        /// mate.</param>
        /// <param name="maxRadius">The maximum radius to another connection.</param>
        /// <param name="dxy">Offset between this connection's
        /// location in the database and the current location (as a result of
        /// dragging).</param>
        abstract searchForClosest: conn: Blockly.Connection * maxRadius: float * dxy: Blockly.Utils.Coordinate -> ConnectionDB__ClassSearchForClosestReturn

    type [<AllowNullLiteral>] ConnectionDB__ClassSearchForClosestReturn =
        abstract connection: Blockly.Connection with get, set
        abstract radius: float with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ConnectionDB__ClassStatic =
        /// Database of connections.
        /// Connections are stored in order of their vertical component.  This way
        /// connections in an area may be looked up quickly using a binary search.
        [<Emit "new $0($1...)">] abstract Create: unit -> ConnectionDB__Class

    module ConnectionDB =

        type [<AllowNullLiteral>] IExports =
            /// Initialize a set of connection DBs for a workspace.
            abstract init: unit -> ResizeArray<Blockly.ConnectionDB>

    module ContextMenu =

        type [<AllowNullLiteral>] IExports =
            abstract currentBlock: Blockly.Block
            /// <summary>Construct the menu based on the list of options and show the menu.</summary>
            /// <param name="e">Mouse event.</param>
            /// <param name="options">Array of menu options.</param>
            /// <param name="rtl">True if RTL, false if LTR.</param>
            abstract show: e: Event * options: ResizeArray<Object> * rtl: bool -> unit
            /// Hide the context menu.
            abstract hide: unit -> unit
            /// <summary>Create a callback function that creates and configures a block,
            ///    then places the new block next to the original.</summary>
            /// <param name="block">Original block.</param>
            /// <param name="xml">XML representation of new block.</param>
            abstract callbackFactory: block: Blockly.Block * xml: Element -> Function
            /// <summary>Make a context menu option for deleting the current block.</summary>
            /// <param name="block">The block where the right-click originated.</param>
            abstract blockDeleteOption: block: Blockly.BlockSvg -> Object
            /// <summary>Make a context menu option for showing help for the current block.</summary>
            /// <param name="block">The block where the right-click originated.</param>
            abstract blockHelpOption: block: Blockly.BlockSvg -> Object
            /// <summary>Make a context menu option for duplicating the current block.</summary>
            /// <param name="block">The block where the right-click originated.</param>
            abstract blockDuplicateOption: block: Blockly.BlockSvg -> Object
            /// <summary>Make a context menu option for adding or removing comments on the current
            /// block.</summary>
            /// <param name="block">The block where the right-click originated.</param>
            abstract blockCommentOption: block: Blockly.BlockSvg -> Object
            /// <summary>Make a context menu option for deleting the current workspace comment.</summary>
            /// <param name="comment">The workspace comment where the
            /// right-click originated.</param>
            abstract commentDeleteOption: comment: Blockly.WorkspaceCommentSvg -> Object
            /// <summary>Make a context menu option for duplicating the current workspace comment.</summary>
            /// <param name="comment">The workspace comment where the
            /// right-click originated.</param>
            abstract commentDuplicateOption: comment: Blockly.WorkspaceCommentSvg -> Object
            /// <summary>Make a context menu option for adding a comment on the workspace.</summary>
            /// <param name="ws">The workspace where the right-click
            /// originated.</param>
            /// <param name="e">The right-click mouse event.</param>
            abstract workspaceCommentOption: ws: Blockly.WorkspaceSvg * e: Event -> Object

    module Css =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Add some CSS to the blob that will be injected later.  Allows optional
            /// components such as fields and the toolbox to store separate CSS.
            /// The provided array of CSS will be destroyed by this function.</summary>
            /// <param name="cssArray">Array of CSS strings.</param>
            abstract register: cssArray: ResizeArray<string> -> unit
            /// <summary>Inject the CSS into the DOM.  This is preferable over using a regular CSS
            /// file since:
            /// a) It loads synchronously and doesn't force a redraw later.
            /// b) It speeds up loading by not blocking on a separate HTTP transfer.
            /// c) The CSS content may be made dynamic depending on init options.</summary>
            /// <param name="hasCss">If false, don't inject CSS
            /// (providing CSS becomes the document's responsibility).</param>
            /// <param name="pathToMedia">Path from page to the Blockly media directory.</param>
            abstract inject: hasCss: bool * pathToMedia: string -> unit
            /// <summary>Set the cursor to be displayed when over something draggable.
            /// See See https://github.com/google/blockly/issues/981 for context.</summary>
            /// <param name="_cursor">Enum.</param>
            abstract setCursor: _cursor: obj option -> unit
            abstract CONTENT: obj option

    type [<AllowNullLiteral>] DropDownDiv =
        inherit DropDownDiv__Class

    type [<AllowNullLiteral>] DropDownDivStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> DropDownDiv

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] DropDownDiv__Class =
        interface end

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] DropDownDiv__ClassStatic =
        /// Class for drop-down div.
        [<Emit "new $0($1...)">] abstract Create: unit -> DropDownDiv__Class

    module DropDownDiv =

        type [<AllowNullLiteral>] IExports =
            abstract ARROW_SIZE: float
            abstract BORDER_SIZE: float
            abstract ARROW_HORIZONTAL_PADDING: float
            abstract PADDING_Y: float
            abstract ANIMATION_TIME: float
            abstract DEFAULT_DROPDOWN_BORDER_COLOR: string
            abstract DEFAULT_DROPDOWN_COLOR: string
            abstract animateOutTimer_: float
            abstract onHide_: Function
            /// Create and insert the DOM element for this div.
            abstract createDom: unit -> unit
            /// <summary>Set an element to maintain bounds within. Drop-downs will appear
            /// within the box of this element if possible.</summary>
            /// <param name="boundsElement">Element to bind drop-down to.</param>
            abstract setBoundsElement: boundsElement: Element -> unit
            /// Provide the div for inserting content into the drop-down.
            abstract getContentDiv: unit -> Element
            /// Clear the content of the drop-down.
            abstract clearContent: unit -> unit
            /// <summary>Set the colour for the drop-down.</summary>
            /// <param name="backgroundColour">Any CSS colour for the background.</param>
            /// <param name="borderColour">Any CSS colour for the border.</param>
            abstract setColour: backgroundColour: string * borderColour: string -> unit
            /// <summary>Set the category for the drop-down.</summary>
            /// <param name="category">The new category for the drop-down.</param>
            abstract setCategory: category: string -> unit
            /// <summary>Shortcut to show and place the drop-down with positioning determined
            /// by a particular block. The primary position will be below the block,
            /// and the secondary position above the block. Drop-down will be
            /// constrained to the block's workspace.</summary>
            /// <param name="field">The field showing the drop-down.</param>
            /// <param name="block">Block to position the drop-down around.</param>
            /// <param name="opt_onHide">Optional callback for when the drop-down is
            /// hidden.</param>
            /// <param name="opt_secondaryYOffset">Optional Y offset for above-block
            /// positioning.</param>
            abstract showPositionedByBlock: field: Blockly.Field * block: Blockly.Block * ?opt_onHide: Function * ?opt_secondaryYOffset: float -> bool
            /// <summary>Shortcut to show and place the drop-down with positioning determined
            /// by a particular field. The primary position will be below the field,
            /// and the secondary position above the field. Drop-down will be
            /// constrained to the block's workspace.</summary>
            /// <param name="owner">The object showing the drop-down.</param>
            /// <param name="opt_onHide">Optional callback for when the drop-down is
            /// hidden.</param>
            /// <param name="opt_secondaryYOffset">Optional Y offset for above-block
            /// positioning.</param>
            abstract showPositionedByField: owner: Object * ?opt_onHide: Function * ?opt_secondaryYOffset: float -> bool
            /// <summary>Show and place the drop-down.
            /// The drop-down is placed with an absolute "origin point" (x, y) - i.e.,
            /// the arrow will point at this origin and box will positioned below or above it.
            /// If we can maintain the container bounds at the primary point, the arrow will
            /// point there, and the container will be positioned below it.
            /// If we can't maintain the container bounds at the primary point, fall-back to the
            /// secondary point and position above.</summary>
            /// <param name="owner">The object showing the drop-down</param>
            /// <param name="primaryX">Desired origin point x, in absolute px</param>
            /// <param name="primaryY">Desired origin point y, in absolute px</param>
            /// <param name="secondaryX">Secondary/alternative origin point x, in absolute px</param>
            /// <param name="secondaryY">Secondary/alternative origin point y, in absolute px</param>
            /// <param name="opt_onHide">Optional callback for when the drop-down is hidden</param>
            abstract show: owner: Object * primaryX: float * primaryY: float * secondaryX: float * secondaryY: float * ?opt_onHide: Function -> bool
            /// <summary>Helper to position the drop-down and the arrow, maintaining bounds.
            /// See explanation of origin points in Blockly.DropDownDiv.show.</summary>
            /// <param name="primaryX">Desired origin point x, in absolute px.</param>
            /// <param name="primaryY">Desired origin point y, in absolute px.</param>
            /// <param name="secondaryX">Secondary/alternative origin point x,
            /// in absolute px.</param>
            /// <param name="secondaryY">Secondary/alternative origin point y,
            /// in absolute px.</param>
            abstract getPositionMetrics: primaryX: float * primaryY: float * secondaryX: float * secondaryY: float -> Object
            /// <summary>Get the metrics for positioning the div below the source.</summary>
            /// <param name="primaryX">Desired origin point x, in absolute px.</param>
            /// <param name="primaryY">Desired origin point y, in absolute px.</param>
            /// <param name="boundsInfo">An object containing size information about the
            /// bounding element (bounding box and width/height).</param>
            /// <param name="divSize">An object containing information about the size
            /// of the DropDownDiv (width & height).</param>
            abstract getPositionBelowMetrics: primaryX: float * primaryY: float * boundsInfo: Object * divSize: Object -> Object
            /// <summary>Get the metrics for positioning the div above the source.</summary>
            /// <param name="secondaryX">Secondary/alternative origin point x,
            /// in absolute px.</param>
            /// <param name="secondaryY">Secondary/alternative origin point y,
            /// in absolute px.</param>
            /// <param name="boundsInfo">An object containing size information about the
            /// bounding element (bounding box and width/height).</param>
            /// <param name="divSize">An object containing information about the size
            /// of the DropDownDiv (width & height).</param>
            abstract getPositionAboveMetrics: secondaryX: float * secondaryY: float * boundsInfo: Object * divSize: Object -> Object
            /// <summary>Get the metrics for positioning the div at the top of the page.</summary>
            /// <param name="sourceX">Desired origin point x, in absolute px.</param>
            /// <param name="boundsInfo">An object containing size information about the
            /// bounding element (bounding box and width/height).</param>
            /// <param name="divSize">An object containing information about the size
            /// of the DropDownDiv (width & height).</param>
            abstract getPositionTopOfPageMetrics: sourceX: float * boundsInfo: Object * divSize: Object -> Object
            /// <summary>Get the x positions for the left side of the DropDownDiv and the arrow,
            /// accounting for the bounds of the workspace.</summary>
            /// <param name="sourceX">Desired origin point x, in absolute px.</param>
            /// <param name="boundsLeft">The left edge of the bounding element, in
            /// absolute px.</param>
            /// <param name="boundsRight">The right edge of the bounding element, in
            /// absolute px.</param>
            /// <param name="divWidth">The width of the div in px.</param>
            abstract getPositionX: sourceX: float * boundsLeft: float * boundsRight: float * divWidth: float -> GetPositionXReturn
            /// Is the container visible?
            abstract isVisible: unit -> bool
            /// <summary>Hide the menu only if it is owned by the provided object.</summary>
            /// <param name="owner">Object which must be owning the drop-down to hide.</param>
            /// <param name="opt_withoutAnimation">True if we should hide the dropdown
            /// without animating.</param>
            abstract hideIfOwner: owner: Object * ?opt_withoutAnimation: bool -> bool
            /// Hide the menu, triggering animation.
            abstract hide: unit -> unit
            /// Hide the menu, without animation.
            abstract hideWithoutAnimation: unit -> unit
            /// Repositions the dropdownDiv on window resize. If it doesn't know how to
            /// calculate the new position, it will just hide it instead.
            abstract repositionForWindowResize: unit -> unit

        type [<AllowNullLiteral>] GetPositionXReturn =
            abstract divX: float with get, set
            abstract arrowX: float with get, set

    module Extensions =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Registers a new extension function. Extensions are functions that help
            /// initialize blocks, usually adding dynamic behavior such as onchange
            /// handlers and mutators. These are applied using Block.applyExtension(), or
            /// the JSON "extensions" array attribute.</summary>
            /// <param name="name">The name of this extension.</param>
            /// <param name="initFn">The function to initialize an extended block.</param>
            abstract register: name: string * initFn: Function -> unit
            /// <summary>Registers a new extension function that adds all key/value of mixinObj.</summary>
            /// <param name="name">The name of this extension.</param>
            /// <param name="mixinObj">The values to mix in.</param>
            abstract registerMixin: name: string * mixinObj: Object -> unit
            /// <summary>Registers a new extension function that adds a mutator to the block.
            /// At register time this performs some basic sanity checks on the mutator.
            /// The wrapper may also add a mutator dialog to the block, if both compose and
            /// decompose are defined on the mixin.</summary>
            /// <param name="name">The name of this mutator extension.</param>
            /// <param name="mixinObj">The values to mix in.</param>
            /// <param name="opt_helperFn">An optional function to apply after
            /// mixing in the object.</param>
            /// <param name="opt_blockList">A list of blocks to appear in the
            /// flyout of the mutator dialog.</param>
            abstract registerMutator: name: string * mixinObj: Object * ?opt_helperFn: RegisterMutatorOpt_helperFn * ?opt_blockList: ResizeArray<string> -> unit
            /// <summary>Unregisters the extension registered with the given name.</summary>
            /// <param name="name">The name of the extension to unregister.</param>
            abstract unregister: name: string -> unit
            /// <summary>Applies an extension method to a block. This should only be called during
            /// block construction.</summary>
            /// <param name="name">The name of the extension.</param>
            /// <param name="block">The block to apply the named extension to.</param>
            /// <param name="isMutator">True if this extension defines a mutator.</param>
            abstract apply: name: string * block: Blockly.Block * isMutator: bool -> unit
            /// <summary>Builds an extension function that will map a dropdown value to a tooltip
            /// string.
            /// 
            /// This method includes multiple checks to ensure tooltips, dropdown options,
            /// and message references are aligned. This aims to catch errors as early as
            /// possible, without requiring developers to manually test tooltips under each
            /// option. After the page is loaded, each tooltip text string will be checked
            /// for matching message keys in the internationalized string table. Deferring
            /// this until the page is loaded decouples loading dependencies. Later, upon
            /// loading the first block of any given type, the extension will validate every
            /// dropdown option has a matching tooltip in the lookupTable.  Errors are
            /// reported as warnings in the console, and are never fatal.</summary>
            /// <param name="dropdownName">The name of the field whose value is the key
            /// to the lookup table.</param>
            /// <param name="lookupTable">The table of field values to
            /// tooltip text.</param>
            abstract buildTooltipForDropdown: dropdownName: string * lookupTable: BuildTooltipForDropdownLookupTable -> Function
            /// <summary>Builds an extension function that will install a dynamic tooltip. The
            /// tooltip message should include the string '%1' and that string will be
            /// replaced with the text of the named field.</summary>
            /// <param name="msgTemplate">The template form to of the message text, with
            /// %1 placeholder.</param>
            /// <param name="fieldName">The field with the replacement text.</param>
            abstract buildTooltipWithFieldText: msgTemplate: string * fieldName: string -> Function

        type [<AllowNullLiteral>] RegisterMutatorOpt_helperFn =
            [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

        type [<AllowNullLiteral>] BuildTooltipForDropdownLookupTable =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

    type [<AllowNullLiteral>] Field =
        inherit Field__Class

    type [<AllowNullLiteral>] FieldStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Field

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Field__Class =
        /// A generic value possessed by the field.
        /// Should generally be non-null, only null when the field is created.
        abstract value_: obj option with get, set
        /// Validation function called when user edits an editable field.
        abstract validator_: Function with get, set
        /// The size of the area rendered by the field.
        abstract size_: Blockly.Utils.Size with get, set
        /// Name of field.  Unique within each block.
        /// Static labels are usually unnamed.
        abstract name: U2<string, obj option> with get, set
        /// Has this field been disposed of?
        abstract disposed: bool with get, set
        /// Maximum characters of text to display before adding an ellipsis.
        abstract maxDisplayLength: float with get, set
        /// Block this field is attached to.  Starts as null, then set in init.
        abstract sourceBlock_: Blockly.Block with get, set
        /// Does this block need to be re-rendered?
        abstract isDirty_: bool with get, set
        /// Is the field visible, or hidden due to the block being collapsed?
        abstract visible_: bool with get, set
        /// A developer hook to override the returned text of this field.
        /// Override if the text representation of the value of this field
        /// is not just a string cast of its value.
        abstract getText_: unit -> string
        /// Editable fields usually show some sort of UI indicating they are editable.
        /// They will also be saved by the XML renderer.
        abstract EDITABLE: bool with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable. This is not the
        /// case by default so that SERIALIZABLE is backwards compatible.
        abstract SERIALIZABLE: bool with get, set
        /// <summary>Process the configuration map passed to the field.</summary>
        /// <param name="config">A map of options used to configure the field. See
        /// the individual field's documentation for a list of properties this
        /// parameter supports.</param>
        abstract configure_: config: Object -> unit
        /// <summary>Attach this field to a block.</summary>
        /// <param name="block">The block containing this field.</param>
        abstract setSourceBlock: block: Blockly.Block -> unit
        /// Get the block this field is attached to.
        abstract getSourceBlock: unit -> Blockly.Block
        /// Initialize everything to render this field. Override
        /// methods initModel and initView rather than this method.
        abstract init: unit -> unit
        /// Create the block UI for this field.
        abstract initView: unit -> unit
        /// Initializes the model of the field after it has been installed on a block.
        /// No-op by default.
        abstract initModel: unit -> unit
        /// Create a field border rect element. Not to be overridden by subclasses.
        /// Instead modify the result of the function inside initView, or create a
        /// separate function to call.
        abstract createBorderRect_: unit -> unit
        /// Create a field text element. Not to be overridden by subclasses. Instead
        /// modify the result of the function inside initView, or create a separate
        /// function to call.
        abstract createTextElement_: unit -> unit
        /// Bind events to the field. Can be overridden by subclasses if they need to do
        /// custom input handling.
        abstract bindEvents_: unit -> unit
        /// <summary>Sets the field's value based on the given XML element. Should only be
        /// called by Blockly.Xml.</summary>
        /// <param name="fieldElement">The element containing info about the
        /// field's state.</param>
        abstract fromXml: fieldElement: Element -> unit
        /// <summary>Serializes this field's value to XML. Should only be called by Blockly.Xml.</summary>
        /// <param name="fieldElement">The element to populate with info about the
        /// field's state.</param>
        abstract toXml: fieldElement: Element -> Element
        /// Dispose of all DOM objects and events belonging to this editable field.
        abstract dispose: unit -> unit
        /// Add or remove the UI indicating if this field is editable or not.
        abstract updateEditable: unit -> unit
        /// Check whether this field defines the showEditor_ function.
        abstract isClickable: unit -> bool
        /// Check whether this field is currently editable.  Some fields are never
        /// EDITABLE (e.g. text labels). Other fields may be EDITABLE but may exist on
        /// non-editable blocks.
        abstract isCurrentlyEditable: unit -> bool
        /// Check whether this field should be serialized by the XML renderer.
        /// Handles the logic for backwards compatibility and incongruous states.
        abstract isSerializable: unit -> bool
        /// Gets whether this editable field is visible or not.
        abstract isVisible: unit -> bool
        /// <summary>Sets whether this editable field is visible or not. Should only be called
        /// by input.setVisible.</summary>
        /// <param name="visible">True if visible.</param>
        abstract setVisible: visible: bool -> unit
        /// <summary>Sets a new validation function for editable fields, or clears a previously
        /// set validator.
        /// 
        /// The validator function takes in the new field value, and returns
        /// validated value. The validated value could be the input value, a modified
        /// version of the input value, or null to abort the change.
        /// 
        /// If the function does not return anything (or returns undefined) the new
        /// value is accepted as valid. This is to allow for fields using the
        /// validated function as a field-level change event notification.</summary>
        /// <param name="handler">The validator function
        /// or null to clear a previous validator.</param>
        abstract setValidator: handler: Function -> unit
        /// Gets the validation function for editable fields, or null if not set.
        abstract getValidator: unit -> Function
        /// <summary>Validates a change.  Does nothing.  Subclasses may override this.</summary>
        /// <param name="text">The user's text.</param>
        abstract classValidator: text: string -> string
        /// <summary>Calls the validation function for this field, as well as all the validation
        /// function for the field's class and its parents.</summary>
        /// <param name="text">Proposed text.</param>
        abstract callValidator: text: string -> string
        /// Gets the group element for this editable field.
        /// Used for measuring the size and for positioning.
        abstract getSvgRoot: unit -> SVGElement
        /// Updates the field to match the colour/style of the block. Should only be
        /// called by BlockSvg.updateColour().
        abstract updateColour: unit -> unit
        /// Used by getSize() to move/resize any DOM elements, and get the new size.
        /// 
        /// All rendering that has an effect on the size/shape of the block should be
        /// done here, and should be triggered by getSize().
        abstract render_: unit -> unit
        /// Updates the width of the field. Redirects to updateSize_().
        abstract updateWidth: unit -> unit
        /// Updates the size of the field based on the text.
        abstract updateSize_: unit -> unit
        /// Returns the height and width of the field.
        /// 
        /// This should *in general* be the only place render_ gets called from.
        abstract getSize: unit -> Blockly.Utils.Size
        /// Returns the bounding box of the rendered field, accounting for workspace
        /// scaling.
        abstract getScaledBBox_: unit -> Object
        /// Get the text from this field to display on the block. May differ from
        /// ``getText`` due to ellipsis, and other formatting.
        abstract getDisplayText_: unit -> string
        /// Get the text from this field.
        abstract getText: unit -> string
        /// <summary>Set the text in this field.  Trigger a rerender of the source block.</summary>
        /// <param name="_newText">New text.</param>
        abstract setText: _newText: obj option -> unit
        /// Force a rerender of the block that this field is installed on, which will
        /// rerender this field and adjust for any sizing changes.
        /// Other fields on the same block will not rerender, because their sizes have
        /// already been recorded.
        abstract markDirty: unit -> unit
        /// Force a rerender of the block that this field is installed on, which will
        /// rerender this field and adjust for any sizing changes.
        /// Other fields on the same block will not rerender, because their sizes have
        /// already been recorded.
        abstract forceRerender: unit -> unit
        /// <summary>Used to change the value of the field. Handles validation and events.
        /// Subclasses should override doClassValidation_ and doValueUpdate_ rather
        /// than this method.</summary>
        /// <param name="newValue">New value.</param>
        abstract setValue: newValue: obj -> unit
        // abstract setValue: newValue: obj option -> unit
        /// Get the current value of the field.
        abstract getValue: unit -> obj option
        /// Used to validate a value. Returns input by default. Can be overridden by
        /// subclasses, see FieldDropdown.
        abstract doClassValidation_: ?opt_newValue: obj -> obj option
        /// <summary>Used to update the value of a field. Can be overridden by subclasses to do
        /// custom storage of values/updating of external things.</summary>
        /// <param name="newValue">The value to be saved.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// <summary>Used to notify the field an invalid value was input. Can be overidden by
        /// subclasses, see FieldTextInput.
        /// No-op by default.</summary>
        /// <param name="_invalidValue">The input value that was determined to be invalid.</param>
        abstract doValueInvalid_: _invalidValue: obj option -> unit
        /// <summary>Handle a mouse down event on a field.</summary>
        /// <param name="e">Mouse down event.</param>
        abstract onMouseDown_: e: Event -> unit
        /// <summary>Change the tooltip text for this field.</summary>
        /// <param name="newTip">Text for tooltip or a parent
        /// element to link to for its tooltip.</param>
        abstract setTooltip: newTip: U3<string, Function, SVGElement> -> unit
        /// Whether this field references any Blockly variables.  If true it may need to
        /// be handled differently during serialization and deserialization.  Subclasses
        /// may override this.
        abstract referencesVariables: unit -> bool
        /// Search through the list of inputs and their fields in order to find the
        /// parent input of a field.
        abstract getParentInput: unit -> Blockly.Input
        /// Returns whether or not we should flip the field in RTL.
        abstract getFlipRtl: unit -> bool
        /// Returns whether or not the field is tab navigable.
        abstract isTabNavigable: unit -> bool
        /// <summary>Handles the given action.
        /// This is only triggered when keyboard accessibility mode is enabled.</summary>
        /// <param name="_action">The action to be handled.</param>
        abstract onBlocklyAction: _action: Blockly.Action -> bool
        /// <summary>Add the cursor svg to this fields svg group.</summary>
        /// <param name="cursorSvg">The svg root of the cursor to be added to the
        /// field group.</param>
        abstract setCursorSvg: cursorSvg: SVGElement -> unit
        /// <summary>Add the marker svg to this fields svg group.</summary>
        /// <param name="markerSvg">The svg root of the marker to be added to the
        /// field group.</param>
        abstract setMarkerSvg: markerSvg: SVGElement -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Field__ClassStatic =
        /// <summary>Abstract class for an editable field.</summary>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a value & returns a validated
        /// value, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field. See
        /// the individual field's documentation for a list of properties this
        /// parameter supports.</param>
        [<Emit "new $0($1...)">] abstract Create: value: obj option * ?opt_validator: Function * ?opt_config: Object -> Field__Class

    module Field =

        type [<AllowNullLiteral>] IExports =
            abstract BORDER_RECT_DEFAULT_HEIGHT: float
            abstract TEXT_DEFAULT_HEIGHT: float
            abstract X_PADDING: float
            abstract Y_PADDING: float
            abstract DEFAULT_TEXT_OFFSET: float
            abstract NBSP: obj option

    type [<AllowNullLiteral>] FieldAngle =
        inherit FieldAngle__Class

    type [<AllowNullLiteral>] FieldAngleStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldAngle

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldAngle__Class =
        inherit Blockly.FieldTextInput__Class
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Create the block UI for this field.
        abstract initView: unit -> unit
        /// <summary>Set the angle to match the mouse's position.</summary>
        /// <param name="e">Mouse move event.</param>
        abstract onMouseMove: e: Event -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldAngle__ClassStatic =
        /// <summary>Class for an editable angle field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast
        /// to a number. Defaults to 0.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a number & returns a
        /// validated number, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: U2<string, float> * ?opt_validator: Function * ?opt_config: Object -> FieldAngle__Class

    module FieldAngle =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldAngle from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (angle).</param>
            abstract fromJson: options: Object -> Blockly.FieldAngle
            abstract ROUND: obj option
            abstract HALF: obj option
            abstract CLOCKWISE: obj option
            abstract OFFSET: obj option
            abstract WRAP: obj option
            abstract RADIUS: obj option

    type [<AllowNullLiteral>] FieldCheckbox =
        inherit FieldCheckbox__Class

    type [<AllowNullLiteral>] FieldCheckboxStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldCheckbox

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldCheckbox__Class =
        inherit Blockly.Field__Class
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Mouse cursor style when over the hotspot that initiates editability.
        abstract CURSOR: obj option with get, set
        /// Used to tell if the field needs to be rendered the next time the block is
        /// rendered. Checkbox fields are statically sized, and only need to be
        /// rendered at initialization.
        abstract isDirty_: bool with get, set
        /// Create the block UI for this checkbox.
        abstract initView: unit -> unit
        /// <summary>Set the character used for the check mark.</summary>
        /// <param name="character">The character to use for the check mark, or
        /// null to use the default.</param>
        abstract setCheckCharacter: character: string -> unit
        /// Toggle the state of the checkbox on click.
        abstract showEditor_: unit -> unit
        /// Ensure that the input value is valid ('TRUE' or 'FALSE').
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Update the value of the field, and update the checkElement.</summary>
        /// <param name="newValue">The value to be saved. The default validator guarantees
        /// that this is a either 'TRUE' or 'FALSE'.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// Get the value of this field, either 'TRUE' or 'FALSE'.
        abstract getValue: unit -> string
        /// Get the boolean value of this field.
        abstract getValueBoolean: unit -> bool
        /// Get the text of this field. Used when the block is collapsed.
        abstract getText: unit -> string

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldCheckbox__ClassStatic =
        /// <summary>Class for a checkbox field.</summary>
        /// <param name="opt_value">The initial value of the field. Should
        /// either be 'TRUE', 'FALSE' or a boolean. Defaults to 'FALSE'.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a value ('TRUE' or 'FALSE') &
        /// returns a validated value ('TRUE' or 'FALSE'), or null to abort the
        /// change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: U2<string, bool> * ?opt_validator: Function * ?opt_config: Object -> FieldCheckbox__Class

    module FieldCheckbox =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldCheckbox from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (checked).</param>
            abstract fromJson: options: Object -> Blockly.FieldCheckbox
            abstract WIDTH: float
            abstract CHECK_CHAR: string
            abstract CHECK_X_OFFSET: float
            abstract CHECK_Y_OFFSET: float

    type [<AllowNullLiteral>] FieldColour =
        inherit FieldColour__Class

    type [<AllowNullLiteral>] FieldColourStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldColour

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldColour__Class =
        inherit Blockly.Field__Class
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Mouse cursor style when over the hotspot that initiates the editor.
        abstract CURSOR: obj option with get, set
        /// Used to tell if the field needs to be rendered the next time the block is
        /// rendered. Colour fields are statically sized, and only need to be
        /// rendered at initialization.
        abstract isDirty_: bool with get, set
        /// Create the block UI for this colour field.
        abstract initView: unit -> unit
        /// Ensure that the input value is a valid colour.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Update the value of this colour field, and update the displayed colour.</summary>
        /// <param name="newValue">The value to be saved. The default validator guarantees
        /// that this is a colour in '#rrggbb' format.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// Get the text for this field.  Used when the block is collapsed.
        abstract getText: unit -> string
        /// <summary>Set a custom colour grid for this field.</summary>
        /// <param name="colours">Array of colours for this block,
        /// or null to use default (Blockly.FieldColour.COLOURS).</param>
        /// <param name="opt_titles">Optional array of colour tooltips,
        /// or null to use default (Blockly.FieldColour.TITLES).</param>
        abstract setColours: colours: ResizeArray<string> * ?opt_titles: ResizeArray<string> -> Blockly.FieldColour
        /// <summary>Set a custom grid size for this field.</summary>
        /// <param name="columns">Number of columns for this block,
        /// or 0 to use default (Blockly.FieldColour.COLUMNS).</param>
        abstract setColumns: columns: float -> Blockly.FieldColour
        /// <summary>Handles the given action.
        /// This is only triggered when keyboard accessibility mode is enabled.</summary>
        /// <param name="action">The action to be handled.</param>
        abstract onBlocklyAction: action: Blockly.Action -> bool

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldColour__ClassStatic =
        /// <summary>Class for a colour input field.</summary>
        /// <param name="opt_value">The initial value of the field. Should be in
        /// '#rrggbb' format. Defaults to the first value in the default colour array.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a colour string & returns a
        /// validated colour string ('#rrggbb' format), or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_validator: Function * ?opt_config: Object -> FieldColour__Class

    module FieldColour =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldColour from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (colour).</param>
            abstract fromJson: options: Object -> Blockly.FieldColour
            abstract COLOURS: ResizeArray<string>
            abstract TITLES: ResizeArray<string>
            abstract COLUMNS: obj option

    type [<AllowNullLiteral>] FieldDate =
        inherit FieldDate__Class

    type [<AllowNullLiteral>] FieldDateStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldDate

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldDate__Class =
        inherit Blockly.Field__Class
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Mouse cursor style when over the hotspot that initiates the editor.
        abstract CURSOR: obj option with get, set
        /// Ensure that the input value is a valid date.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// Render the field. If the picker is shown make sure it has the current
        /// date selected.
        abstract render_: unit -> unit
        /// Updates the field's colours to match those of the block.
        abstract updateColour: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldDate__ClassStatic =
        /// <summary>Class for a date input field.</summary>
        /// <param name="opt_value">The initial value of the field. Should be in
        /// 'YYYY-MM-DD' format. Defaults to the current date.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a date string & returns a
        /// validated date string ('YYYY-MM-DD' format), or null to abort the change.</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_validator: Function -> FieldDate__Class

    module FieldDate =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldDate from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (date).</param>
            abstract fromJson: options: Object -> Blockly.FieldDate

    type [<AllowNullLiteral>] FieldDropdown =
        inherit FieldDropdown__Class

    type [<AllowNullLiteral>] FieldDropdownStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldDropdown
        //AMO moved from below, modifying return type
        /// <summary>Class for an editable dropdown field.</summary>
        /// <param name="menuGenerator">A non-empty array of
        /// options for a dropdown list, or a function which generates these options.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a language-neutral dropdown
        /// option & returns a validated language-neutral dropdown option, or null to
        /// abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: menuGenerator: U2<string[][], Function> * ?opt_validator: Function * ?opt_config: Object -> FieldDropdown // FieldDropdown__Class
        // [<Emit "new $0($1...)">] abstract Create: menuGenerator: U2<ResizeArray<ResizeArray<obj option>>, Function> * ?opt_validator: Function * ?opt_config: Object -> FieldDropdown // FieldDropdown__Class


    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldDropdown__Class =
        inherit Blockly.Field__Class
        /// An array of options for a dropdown list,
        /// or a function which generates these options.
        abstract menuGenerator_: U2<ResizeArray<ResizeArray<obj option>>, TypeLiteral_13> with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Mouse cursor style when over the hotspot that initiates the editor.
        abstract CURSOR: obj option with get, set
        /// Create the block UI for this dropdown.
        abstract initView: unit -> unit
        /// <summary>Handle the selection of an item in the dropdown menu.</summary>
        /// <param name="menu">The Menu component clicked.</param>
        /// <param name="menuItem">The MenuItem selected within menu.</param>
        abstract onItemSelected: menu: Blockly.Menu * menuItem: Blockly.MenuItem -> unit
        abstract isOptionListDynamic: unit -> bool
        /// <summary>Return a list of the options for this dropdown.</summary>
        /// <param name="opt_useCache">For dynamic options, whether or not to use the
        /// cached options or to re-generate them.</param>
        abstract getOptions: ?opt_useCache: bool -> ResizeArray<ResizeArray<string>>
        // original below
        // abstract getOptions: ?opt_useCache: bool -> ResizeArray<ResizeArray<obj option>>
        /// Ensure that the input value is a valid language-neutral option.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Update the value of this dropdown field.</summary>
        /// <param name="newValue">The value to be saved. The default validator guarantees
        /// that this is one of the valid dropdown options.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// Updates the dropdown arrow to match the colour/style of the block.
        abstract updateColour: unit -> unit
        /// <summary>Handles the given action.
        /// This is only triggered when keyboard accessibility mode is enabled.</summary>
        /// <param name="action">The action to be handled.</param>
        abstract onBlocklyAction: action: Blockly.Action -> bool

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldDropdown__ClassStatic =
        /// <summary>Class for an editable dropdown field.</summary>
        /// <param name="menuGenerator">A non-empty array of
        /// options for a dropdown list, or a function which generates these options.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a language-neutral dropdown
        /// option & returns a validated language-neutral dropdown option, or null to
        /// abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: menuGenerator: U2<ResizeArray<ResizeArray<obj option>>, Function> * ?opt_validator: Function * ?opt_config: Object -> FieldDropdown__Class

    module FieldDropdown =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldDropdown from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (options).</param>
            abstract fromJson: options: Object -> Blockly.FieldDropdown
            abstract CHECKMARK_OVERHANG: obj option
            abstract MAX_MENU_HEIGHT_VH: obj option
            abstract ARROW_CHAR: obj option
            /// <summary>Use the calculated prefix and suffix lengths to trim all of the options in
            /// the given array.</summary>
            /// <param name="options">Array of option tuples:
            /// (human-readable text or image, language-neutral name).</param>
            /// <param name="prefixLength">The length of the common prefix.</param>
            /// <param name="suffixLength">The length of the common suffix</param>
            abstract applyTrim_: options: ResizeArray<ResizeArray<obj option>> * prefixLength: float * suffixLength: float -> ResizeArray<ResizeArray<obj option>>

        /// Dropdown image properties.
        type [<AllowNullLiteral>] ImageProperties =
            abstract src: string with get, set
            abstract alt: string with get, set
            abstract width: float with get, set
            abstract height: float with get, set

    type [<AllowNullLiteral>] FieldImage =
        inherit FieldImage__Class

    type [<AllowNullLiteral>] FieldImageStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldImage

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldImage__Class =
        inherit Blockly.Field__Class
        /// Editable fields usually show some sort of UI indicating they are
        /// editable. This field should not.
        abstract EDITABLE: bool with get, set
        /// Used to tell if the field needs to be rendered the next time the block is
        /// rendered. Image fields are statically sized, and only need to be
        /// rendered at initialization.
        abstract isDirty_: bool with get, set
        /// Create the block UI for this image.
        abstract initView: unit -> unit
        /// Ensure that the input value (the source URL) is a string.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Update the value of this image field, and update the displayed image.</summary>
        /// <param name="newValue">The value to be saved. The default validator guarantees
        /// that this is a string.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// <summary>Set the alt text of this image.</summary>
        /// <param name="alt">New alt text.</param>
        abstract setAlt: alt: string -> unit
        /// If field click is called, and click handler defined,
        /// call the handler.
        abstract showEditor_: unit -> unit
        /// <summary>Set the function that is called when this image  is clicked.</summary>
        /// <param name="func">The function that is called
        /// when the image is clicked, or null to remove.</param>
        abstract setOnClickHandler: func: FieldImage__ClassSetOnClickHandlerFunc -> unit

    type [<AllowNullLiteral>] FieldImage__ClassSetOnClickHandlerFunc =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.FieldImage -> obj option

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldImage__ClassStatic =
        /// <summary>Class for an image on a block.</summary>
        /// <param name="src">The URL of the image. Defaults to an empty string.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="opt_alt">Optional alt text for when block is collapsed.</param>
        /// <param name="opt_onClick">Optional function to be
        /// called when the image is clicked. If opt_onClick is defined, opt_alt must
        /// also be defined.</param>
        /// <param name="opt_flipRtl">Whether to flip the icon in RTL.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: src: string * width: U2<string, float> * height: U2<string, float> * ?opt_alt: string * ?opt_onClick: FieldImage__ClassStaticOpt_onClick * ?opt_flipRtl: bool * ?opt_config: Object -> FieldImage__Class

    type [<AllowNullLiteral>] FieldImage__ClassStaticOpt_onClick =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.FieldImage -> obj option

    module FieldImage =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldImage from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (src, width, height,
            /// alt, and flipRtl).</param>
            abstract fromJson: options: Object -> Blockly.FieldImage

    type [<AllowNullLiteral>] FieldLabel =
        inherit FieldLabel__Class

    type [<AllowNullLiteral>] FieldLabelStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldLabel

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldLabel__Class =
        inherit Blockly.Field__Class
        /// Editable fields usually show some sort of UI indicating they are
        /// editable. This field should not.
        abstract EDITABLE: bool with get, set
        /// Create block UI for this label.
        abstract initView: unit -> unit
        /// Ensure that the input value casts to a valid string.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Set the css class applied to the field's textElement_.</summary>
        /// <param name="cssClass">The new css class name, or null to remove.</param>
        abstract setClass: cssClass: string -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldLabel__ClassStatic =
        /// <summary>Class for a non-editable, non-serializable text field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast to a
        /// string. Defaults to an empty string if null or undefined.</param>
        /// <param name="opt_class">Optional CSS class for the field's text.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_class: string * ?opt_config: Object -> FieldLabel__Class

    module FieldLabel =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldLabel from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (text, and class).</param>
            abstract fromJson: options: Object -> Blockly.FieldLabel

    type [<AllowNullLiteral>] FieldLabelSerializable =
        inherit FieldLabelSerializable__Class

    type [<AllowNullLiteral>] FieldLabelSerializableStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldLabelSerializable

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldLabelSerializable__Class =
        inherit Blockly.FieldLabel__Class
        /// Editable fields usually show some sort of UI indicating they are
        /// editable. This field should not.
        abstract EDITABLE: bool with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not.  This field should be serialized, but only edited programmatically.
        abstract SERIALIZABLE: bool with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldLabelSerializable__ClassStatic =
        /// <summary>Class for a non-editable, serializable text field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast to a
        /// string. Defaults to an empty string if null or undefined.</param>
        /// <param name="opt_class">Optional CSS class for the field's text.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: opt_value: obj option * ?opt_class: string * ?opt_config: Object -> FieldLabelSerializable__Class

    module FieldLabelSerializable =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldLabelSerializable from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (text, and class).</param>
            abstract fromJson: options: Object -> Blockly.FieldLabelSerializable

    type [<AllowNullLiteral>] FieldMultilineInput =
        inherit FieldMultilineInput__Class

    type [<AllowNullLiteral>] FieldMultilineInputStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldMultilineInput

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldMultilineInput__Class =
        inherit Blockly.FieldTextInput__Class
        /// Create the block UI for this field.
        abstract initView: unit -> unit
        /// Updates the text of the textElement.
        abstract render_: unit -> unit
        /// Updates the size of the field based on the text.
        abstract updateSize_: unit -> unit
        /// Resize the editor to fit the text.
        abstract resizeEditor_: unit -> unit
        /// Create the text input editor widget.
        abstract widgetCreate_: unit -> HTMLTextAreaElement
        /// <summary>Handle key down to the editor. Override the text input definition of this
        /// so as to not close the editor when enter is typed in.</summary>
        /// <param name="e">Keyboard event.</param>
        abstract onHtmlInputKeyDown_: e: Event -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldMultilineInput__ClassStatic =
        /// <summary>Class for an editable text area field.</summary>
        /// <param name="opt_value">The initial content of the field. Should cast to a
        /// string. Defaults to an empty string if null or undefined.</param>
        /// <param name="opt_validator">An optional function that is called
        /// to validate any constraints on what the user entered.  Takes the new
        /// text as an argument and returns either the accepted text, a replacement
        /// text, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_validator: Function * ?opt_config: Object -> FieldMultilineInput__Class

    module FieldMultilineInput =

        type [<AllowNullLiteral>] IExports =
            abstract LINE_HEIGHT: float
            /// <summary>Construct a FieldMultilineInput from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (text, and spellcheck).</param>
            abstract fromJson: options: Object -> Blockly.FieldMultilineInput

    type [<AllowNullLiteral>] FieldNumber =
        inherit FieldNumber__Class

    type [<AllowNullLiteral>] FieldNumberStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldNumber

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldNumber__Class =
        inherit Blockly.FieldTextInput__Class
        /// The minimum value this number field can contain.
        abstract min_: float with get, set
        /// The maximum value this number field can contain.
        abstract max_: float with get, set
        /// The multiple to which this fields value is rounded.
        abstract precision_: float with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// <summary>Set the maximum, minimum and precision constraints on this field.
        /// Any of these properties may be undefined or NaN to be disabled.
        /// Setting precision (usually a power of 10) enforces a minimum step between
        /// values. That is, the user's value will rounded to the closest multiple of
        /// precision. The least significant digit place is inferred from the precision.
        /// Integers values can be enforces by choosing an integer precision.</summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="precision">Precision for value.</param>
        abstract setConstraints: min: U3<float, string, obj option> * max: U3<float, string, obj option> * precision: U3<float, string, obj option> -> unit
        /// <summary>Sets the minimum value this field can contain. Updates the value to reflect.</summary>
        /// <param name="min">Minimum value.</param>
        abstract setMin: min: U3<float, string, obj option> -> unit
        /// Returns the current minimum value this field can contain. Default is
        /// -Infinity.
        abstract getMin: unit -> float
        /// <summary>Sets the maximum value this field can contain. Updates the value to reflect.</summary>
        /// <param name="max">Maximum value.</param>
        abstract setMax: max: U3<float, string, obj option> -> unit
        /// Returns the current maximum value this field can contain. Default is
        /// Infinity.
        abstract getMax: unit -> float
        /// <summary>Sets the precision of this field's value, i.e. the number to which the
        /// value is rounded. Updates the field to reflect.</summary>
        /// <param name="precision">The number to which the
        /// field's value is rounded.</param>
        abstract setPrecision: precision: U3<float, string, obj option> -> unit
        /// Returns the current precision of this field. The precision being the
        /// number to which the field's value is rounded. A precision of 0 means that
        /// the value is not rounded.
        abstract getPrecision: unit -> float

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldNumber__ClassStatic =
        /// <summary>Class for an editable number field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast
        /// to a number. Defaults to 0.</param>
        /// <param name="opt_min">Minimum value.</param>
        /// <param name="opt_max">Maximum value.</param>
        /// <param name="opt_precision">Precision for value.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a number & returns a validated
        /// number, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: U2<string, float> * ?opt_min: U2<string, float> * ?opt_max: U2<string, float> * ?opt_precision: U2<string, float> * ?opt_validator: Function * ?opt_config: Object -> FieldNumber__Class

    module FieldNumber =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldNumber from a JSON arg object.</summary>
            /// <param name="options">A JSON object with options (value, min, max, and
            /// precision).</param>
            abstract fromJson: options: Object -> Blockly.FieldNumber

    module FieldRegistry =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Registers a field type.
            /// Blockly.fieldRegistry.fromJson uses this registry to
            /// find the appropriate field type.</summary>
            /// <param name="type">The field type name as used in the JSON definition.</param>
            /// <param name="fieldClass">The field class containing a
            /// fromJson function that can construct an instance of the field.</param>
            abstract register: ``type``: string * fieldClass: RegisterFieldClass -> unit
            /// <summary>Unregisters the field registered with the given type.</summary>
            /// <param name="type">The field type name as used in the JSON definition.</param>
            abstract unregister: ``type``: string -> unit
            /// <summary>Construct a Field from a JSON arg object.
            /// Finds the appropriate registered field by the type name as registered using
            /// Blockly.fieldRegistry.register.</summary>
            /// <param name="options">A JSON object with a type and options specific
            /// to the field type.</param>
            abstract fromJson: options: Object -> Blockly.Field

        type [<AllowNullLiteral>] RegisterFieldClass =
            abstract fromJson: Function with get, set

    type [<AllowNullLiteral>] FieldTextInput =
        inherit FieldTextInput__Class

    type [<AllowNullLiteral>] FieldTextInputStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldTextInput
        //AMO added overload
        /// <summary>Class for an editable text field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast to a
        /// string. Defaults to an empty string if null or undefined.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a string & returns a validated
        /// string, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_validator: Function * ?opt_config: Object -> FieldTextInput

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldTextInput__Class =
        inherit Blockly.Field__Class
        /// Allow browser to spellcheck this field.
        abstract spellcheck_: bool with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// Mouse cursor style when over the hotspot that initiates the editor.
        abstract CURSOR: obj option with get, set
        /// Ensure that the input value casts to a valid string.
        abstract doClassValidation_: ?opt_newValue: obj -> obj option
        /// <summary>Called by setValue if the text input is not valid. If the field is
        /// currently being edited it reverts value of the field to the previous
        /// value while allowing the display text to be handled by the htmlInput_.</summary>
        /// <param name="_invalidValue">The input value that was determined to be invalid.
        /// This is not used by the text input because its display value is stored on
        /// the htmlInput_.</param>
        abstract doValueInvalid_: _invalidValue: obj option -> unit
        /// <summary>Called by setValue if the text input is valid. Updates the value of the
        /// field, and updates the text of the field if it is not currently being
        /// edited (i.e. handled by the htmlInput_).</summary>
        /// <param name="newValue">The value to be saved. The default validator guarantees
        /// that this is a string.</param>
        abstract doValueUpdate_: newValue: obj option -> unit
        /// Updates the colour of the htmlInput given the current validity of the
        /// field's value.
        abstract render_: unit -> unit
        /// <summary>Set whether this field is spellchecked by the browser.</summary>
        /// <param name="check">True if checked.</param>
        abstract setSpellcheck: check: bool -> unit
        /// <summary>Show the inline free-text editor on top of the text.</summary>
        /// <param name="opt_quietInput">True if editor should be created without
        /// focus.  Defaults to false.</param>
        abstract showEditor_: ?opt_quietInput: bool -> unit
        /// Create the text input editor widget.
        abstract widgetCreate_: unit -> HTMLElement
        /// <summary>Bind handlers for user input on the text input field's editor.</summary>
        /// <param name="htmlInput">The htmlInput to which event
        /// handlers will be bound.</param>
        abstract bindInputEvents_: htmlInput: HTMLElement -> unit
        /// <summary>Handle key down to the editor.</summary>
        /// <param name="e">Keyboard event.</param>
        abstract onHtmlInputKeyDown_: e: Event -> unit
        /// <summary>Set the html input value and the field's internal value. The difference
        /// between this and ``setValue`` is that this also updates the html input
        /// value whilst editing.</summary>
        /// <param name="newValue">New value.</param>
        abstract setEditorValue_: newValue: obj option -> unit
        /// Resize the editor to fit the text.
        abstract resizeEditor_: unit -> unit
        /// <summary>Transform the provided value into a text to show in the html input.
        /// Override this method if the field's html input representation is different
        /// than the field's value. This should be coupled with an override of
        /// `getValueFromEditorText_`.</summary>
        /// <param name="value">The value stored in this field.</param>
        abstract getEditorText_: value: obj option -> string
        /// <summary>Transform the text received from the html input into a value to store
        /// in this field.
        /// Override this method if the field's html input representation is different
        /// than the field's value. This should be coupled with an override of
        /// `getEditorText_`.</summary>
        /// <param name="text">Text received from the html input.</param>
        abstract getValueFromEditorText_: text: string -> obj option

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldTextInput__ClassStatic =
        /// <summary>Class for an editable text field.</summary>
        /// <param name="opt_value">The initial value of the field. Should cast to a
        /// string. Defaults to an empty string if null or undefined.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a string & returns a validated
        /// string, or null to abort the change.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        /// AMO: copied this up to FieldTextInputStatic because it wasn't working here. 
        [<Emit "new $0($1...)">] abstract Create: ?opt_value: string * ?opt_validator: Function * ?opt_config: Object -> FieldTextInput__Class

    module FieldTextInput =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldTextInput from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (text, and spellcheck).</param>
            abstract fromJson: options: Object -> Blockly.FieldTextInput
            abstract FONTSIZE: obj option
            abstract BORDERRADIUS: obj option
            /// <summary>Ensure that only a number may be entered.</summary>
            /// <param name="text">The user's text.</param>
            abstract numberValidator: text: string -> string
            /// <summary>Ensure that only a non-negative integer may be entered.</summary>
            /// <param name="text">The user's text.</param>
            abstract nonnegativeIntegerValidator: text: string -> string

    type [<AllowNullLiteral>] FieldVariable =
        inherit FieldVariable__Class

    type [<AllowNullLiteral>] FieldVariableStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FieldVariable
        // AMO copied from below, modifying return type
        /// <summary>Class for a variable's dropdown field.</summary>
        /// <param name="varName">The default name for the variable.  If null,
        /// a unique variable name will be generated.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a variable ID  & returns a
        /// validated variable ID, or null to abort the change.</param>
        /// <param name="opt_variableTypes">A list of the types of variables
        /// to include in the dropdown.</param>
        /// <param name="opt_defaultType">The type of variable to create if this
        /// field's value is not explicitly set.  Defaults to ''.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        [<Emit "new $0($1...)">] abstract Create: varName: string * ?opt_validator: Function * ?opt_variableTypes: ResizeArray<string> * ?opt_defaultType: string * ?opt_config: Object -> FieldVariable


    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldVariable__Class =
        inherit Blockly.FieldDropdown__Class
        /// An array of options for a dropdown list,
        /// or a function which generates these options.
        abstract menuGenerator_: TypeLiteral_13 with get, set
        /// The initial variable name passed to this field's constructor, or an
        /// empty string if a name wasn't provided. Used to create the initial
        /// variable.
        abstract defaultVariableName: string with get, set
        /// Serializable fields are saved by the XML renderer, non-serializable fields
        /// are not. Editable fields should also be serializable.
        abstract SERIALIZABLE: bool with get, set
        /// <summary>Configure the field based on the given map of options.</summary>
        /// <param name="config">A map of options to configure the field based on.</param>
        abstract configure_: config: Object -> unit
        /// Initialize the model for this field if it has not already been initialized.
        /// If the value has not been set to a variable by the first render, we make up a
        /// variable rather than let the value be invalid.
        abstract initModel: unit -> unit
        /// <summary>Initialize this field based on the given XML.</summary>
        /// <param name="fieldElement">The element containing information about the
        /// variable field's state.</param>
        abstract fromXml: fieldElement: Element -> unit
        /// <summary>Serialize this field to XML.</summary>
        /// <param name="fieldElement">The element to populate with info about the
        /// field's state.</param>
        abstract toXml: fieldElement: Element -> Element
        /// <summary>Attach this field to a block.</summary>
        /// <param name="block">The block containing this field.</param>
        abstract setSourceBlock: block: Blockly.Block -> unit
        /// Get the variable's ID.
        abstract getValue: unit -> string
        /// Get the text from this field, which is the selected variable's name.
        abstract getText: unit -> string
        /// Get the variable model for the selected variable.
        /// Not guaranteed to be in the variable map on the workspace (e.g. if accessed
        /// after the variable has been deleted).
        abstract getVariable: unit -> Blockly.VariableModel
        /// Gets the validation function for this field, or null if not set.
        /// Returns null if the variable is not set, because validators should not
        /// run on the initial setValue call, because the field won't be attached to
        /// a block and workspace at that point.
        abstract getValidator: unit -> Function
        /// Ensure that the id belongs to a valid variable of an allowed type.
        abstract doClassValidation_: ?opt_newValue: obj -> string
        /// <summary>Update the value of this variable field, as well as its variable and text.
        /// 
        /// The variable ID should be valid at this point, but if a variable field
        /// validator returns a bad ID, this could break.</summary>
        /// <param name="newId">The value to be saved.</param>
        abstract doValueUpdate_: newId: obj option -> unit
        /// Refreshes the name of the variable by grabbing the name of the model.
        /// Used when a variable gets renamed, but the ID stays the same. Should only
        /// be called by the block.
        abstract refreshVariableName: unit -> unit
        /// <summary>Handle the selection of an item in the variable dropdown menu.
        /// Special case the 'Rename variable...' and 'Delete variable...' options.
        /// In the rename case, prompt the user for a new name.</summary>
        /// <param name="menu">The Menu component clicked.</param>
        /// <param name="menuItem">The MenuItem selected within menu.</param>
        abstract onItemSelected: menu: Blockly.Menu * menuItem: Blockly.MenuItem -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FieldVariable__ClassStatic =
        /// <summary>Class for a variable's dropdown field.</summary>
        /// <param name="varName">The default name for the variable.  If null,
        /// a unique variable name will be generated.</param>
        /// <param name="opt_validator">A function that is called to validate
        /// changes to the field's value. Takes in a variable ID  & returns a
        /// validated variable ID, or null to abort the change.</param>
        /// <param name="opt_variableTypes">A list of the types of variables
        /// to include in the dropdown.</param>
        /// <param name="opt_defaultType">The type of variable to create if this
        /// field's value is not explicitly set.  Defaults to ''.</param>
        /// <param name="opt_config">A map of options used to configure the field.
        /// See the [field creation documentation]{</param>
        /// AMO copied up to FieldVariableStatic because it wasn't working here
        [<Emit "new $0($1...)">] abstract Create: varName: string * ?opt_validator: Function * ?opt_variableTypes: ResizeArray<string> * ?opt_defaultType: string * ?opt_config: Object -> FieldVariable__Class

    module FieldVariable =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct a FieldVariable from a JSON arg object,
            /// dereferencing any string table references.</summary>
            /// <param name="options">A JSON object with options (variable,
            /// variableTypes, and defaultType).</param>
            abstract fromJson: options: Object -> Blockly.FieldVariable
            /// Return a sorted list of variable names for variable dropdown menus.
            /// Include a special option at the end for creating a new variable name.
            abstract dropdownCreate: unit -> ResizeArray<ResizeArray<obj option>>

    type [<AllowNullLiteral>] Flyout =
        inherit Flyout__Class

    type [<AllowNullLiteral>] FlyoutStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Flyout

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Flyout__Class =
        abstract workspace_: Blockly.WorkspaceSvg with get, set
        /// Is RTL vs LTR.
        abstract RTL: bool with get, set
        /// Position of the toolbox and flyout relative to the workspace.
        abstract toolboxPosition_: float with get, set
        /// List of visible buttons.
        abstract buttons_: ResizeArray<Blockly.FlyoutButton> with get, set
        /// Width of output tab.
        abstract tabWidth_: float with get, set
        /// Does the flyout automatically close when a block is created?
        abstract autoClose: bool with get, set
        /// Corner radius of the flyout background.
        abstract CORNER_RADIUS: float with get, set
        /// Margin around the edges of the blocks in the flyout.
        abstract MARGIN: float with get, set
        /// Gap between items in horizontal flyouts. Can be overridden with the "sep"
        /// element.
        abstract GAP_X: obj option with get, set
        /// Gap between items in vertical flyouts. Can be overridden with the "sep"
        /// element.
        abstract GAP_Y: obj option with get, set
        /// Top/bottom padding between scrollbar and edge of flyout background.
        abstract SCROLLBAR_PADDING: float with get, set
        /// Width of flyout.
        abstract width_: float with get, set
        /// Height of flyout.
        abstract height_: float with get, set
        /// Range of a drag angle from a flyout considered "dragging toward workspace".
        /// Drags that are within the bounds of this many degrees from the orthogonal
        /// line to the flyout edge are considered to be "drags toward the workspace".
        /// Example:
        /// Flyout                                                  Edge   Workspace
        /// [block] /  <-within this angle, drags "toward workspace" |
        /// [block] ---- orthogonal to flyout boundary ----          |
        /// [block] \                                                |
        /// The angle is given in degrees from the orthogonal.
        /// 
        /// This is used to know when to create a new block and when to scroll the
        /// flyout. Setting it to 360 means that all drags create a new block.
        abstract dragAngleRange_: float with get, set
        /// <summary>Creates the flyout's DOM.  Only needs to be called once.  The flyout can
        /// either exist as its own svg element or be a g element nested inside a
        /// separate svg element.</summary>
        /// <param name="tagName">The type of tag to put the flyout in. This
        /// should be <svg> or <g>.</param>
        abstract createDom: tagName: string -> SVGElement
        /// <summary>Initializes the flyout.</summary>
        /// <param name="targetWorkspace">The workspace in which to create
        /// new blocks.</param>
        abstract init: targetWorkspace: Blockly.Workspace -> unit
        /// Dispose of this flyout.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// Get the width of the flyout.
        abstract getWidth: unit -> float
        /// Get the height of the flyout.
        abstract getHeight: unit -> float
        /// Get the workspace inside the flyout.
        abstract getWorkspace: unit -> Blockly.WorkspaceSvg
        /// Is the flyout visible?
        abstract isVisible: unit -> bool
        /// <summary>Set whether the flyout is visible. A value of true does not necessarily mean
        /// that the flyout is shown. It could be hidden because its container is hidden.</summary>
        /// <param name="visible">True if visible.</param>
        abstract setVisible: visible: bool -> unit
        /// <summary>Set whether this flyout's container is visible.</summary>
        /// <param name="visible">Whether the container is visible.</param>
        abstract setContainerVisible: visible: bool -> unit
        /// <summary>Update the view based on coordinates calculated in position().</summary>
        /// <param name="width">The computed width of the flyout's SVG group</param>
        /// <param name="height">The computed height of the flyout's SVG group.</param>
        /// <param name="x">The computed x origin of the flyout's SVG group.</param>
        /// <param name="y">The computed y origin of the flyout's SVG group.</param>
        abstract positionAt_: width: float * height: float * x: float * y: float -> unit
        /// Hide and empty the flyout.
        abstract hide: unit -> unit
        /// <summary>Show and populate the flyout.</summary>
        /// <param name="xmlList">List of blocks to show.
        /// Variables and procedures have a custom set of blocks.</param>
        abstract show: xmlList: U2<ResizeArray<obj option>, string> -> unit
        /// <summary>Add listeners to a block that has been added to the flyout.</summary>
        /// <param name="root">The root node of the SVG group the block is in.</param>
        /// <param name="block">The block to add listeners for.</param>
        /// <param name="rect">The invisible rectangle under the block that acts
        /// as a mat for that block.</param>
        abstract addBlockListeners_: root: SVGElement * block: Blockly.Block * rect: SVGElement -> unit
        /// <summary>Does this flyout allow you to create a new instance of the given block?
        /// Used for deciding if a block can be "dragged out of" the flyout.</summary>
        /// <param name="block">The block to copy from the flyout.</param>
        abstract isBlockCreatable_: block: Blockly.BlockSvg -> bool
        /// <summary>Create a copy of this block on the workspace.</summary>
        /// <param name="originalBlock">The block to copy from the flyout.</param>
        abstract createBlock: originalBlock: Blockly.BlockSvg -> Blockly.BlockSvg
        /// <summary>Initialize the given button: move it to the correct location,
        /// add listeners, etc.</summary>
        /// <param name="button">The button to initialize and place.</param>
        /// <param name="x">The x position of the cursor during this layout pass.</param>
        /// <param name="y">The y position of the cursor during this layout pass.</param>
        abstract initFlyoutButton_: button: Blockly.FlyoutButton * x: float * y: float -> unit
        /// <summary>Create and place a rectangle corresponding to the given block.</summary>
        /// <param name="block">The block to associate the rect to.</param>
        /// <param name="x">The x position of the cursor during this layout pass.</param>
        /// <param name="y">The y position of the cursor during this layout pass.</param>
        /// <param name="blockHW">The height and width of the
        /// block.</param>
        /// <param name="index">The index into the mats list where this rect should be
        /// placed.</param>
        abstract createRect_: block: Blockly.Block * x: float * y: float * blockHW: Flyout__ClassCreateRect_BlockHW * index: float -> SVGElement
        /// <summary>Move a rectangle to sit exactly behind a block, taking into account tabs,
        /// hats, and any other protrusions we invent.</summary>
        /// <param name="rect">The rectangle to move directly behind the block.</param>
        /// <param name="block">The block the rectangle should be behind.</param>
        abstract moveRectToBlock_: rect: SVGElement * block: Blockly.BlockSvg -> unit
        /// Reflow blocks and their mats.
        abstract reflow: unit -> unit
        abstract isScrollable: unit -> bool

    type [<AllowNullLiteral>] Flyout__ClassCreateRect_BlockHW =
        abstract height: float with get, set
        abstract width: float with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Flyout__ClassStatic =
        /// <summary>Class for a flyout.</summary>
        /// <param name="workspaceOptions">Dictionary of options for the workspace.</param>
        [<Emit "new $0($1...)">] abstract Create: workspaceOptions: Object -> Flyout__Class

    type [<AllowNullLiteral>] FlyoutButton =
        inherit FlyoutButton__Class

    type [<AllowNullLiteral>] FlyoutButtonStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FlyoutButton

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutButton__Class =
        /// The width of the button's rect.
        abstract width: float with get, set
        /// The height of the button's rect.
        abstract height: float with get, set
        /// Create the button elements.
        abstract createDom: unit -> SVGElement
        /// Correctly position the flyout button and make it visible.
        abstract show: unit -> unit
        /// <summary>Move the button to the given x, y coordinates.</summary>
        /// <param name="x">The new x coordinate.</param>
        /// <param name="y">The new y coordinate.</param>
        abstract moveTo: x: float * y: float -> unit
        /// Location of the button.
        abstract getPosition: unit -> Blockly.Utils.Coordinate
        /// Get the button's target workspace.
        abstract getTargetWorkspace: unit -> Blockly.WorkspaceSvg
        /// Dispose of this button.
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutButton__ClassStatic =
        /// <summary>Class for a button in the flyout.</summary>
        /// <param name="workspace">The workspace in which to place this
        /// button.</param>
        /// <param name="targetWorkspace">The flyout's target workspace.</param>
        /// <param name="xml">The XML specifying the label/button.</param>
        /// <param name="isLabel">Whether this button should be styled as a label.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg * targetWorkspace: Blockly.WorkspaceSvg * xml: Element * isLabel: bool -> FlyoutButton__Class

    module FlyoutButton =

        type [<AllowNullLiteral>] IExports =
            abstract MARGIN: obj option

    type [<AllowNullLiteral>] FlyoutDragger =
        inherit FlyoutDragger__Class

    type [<AllowNullLiteral>] FlyoutDraggerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FlyoutDragger

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutDragger__Class =
        inherit Blockly.WorkspaceDragger__Class
        /// <summary>Move the flyout based on the most recent mouse movements.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel coordinates.</param>
        abstract drag: currentDragDeltaXY: Blockly.Utils.Coordinate -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutDragger__ClassStatic =
        /// <summary>Class for a flyout dragger.  It moves a flyout workspace around when it is
        /// being dragged by a mouse or touch.
        /// Note that the workspace itself manages whether or not it has a drag surface
        /// and how to do translations based on that.  This simply passes the right
        /// commands based on events.</summary>
        /// <param name="flyout">The flyout to drag.</param>
        [<Emit "new $0($1...)">] abstract Create: flyout: Blockly.Flyout -> FlyoutDragger__Class

    type [<AllowNullLiteral>] HorizontalFlyout =
        inherit HorizontalFlyout__Class

    type [<AllowNullLiteral>] HorizontalFlyoutStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> HorizontalFlyout

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] HorizontalFlyout__Class =
        inherit Blockly.Flyout__Class
        /// Move the flyout to the edge of the workspace.
        abstract position: unit -> unit
        /// Scroll the flyout to the top.
        abstract scrollToStart: unit -> unit
        /// <summary>Determine if a drag delta is toward the workspace, based on the position
        /// and orientation of the flyout. This is used in determineDragIntention_ to
        /// determine if a new block should be created or if the flyout should scroll.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at mouse down, in pixel units.</param>
        abstract isDragTowardWorkspace: currentDragDeltaXY: Blockly.Utils.Coordinate -> bool
        /// Return the deletion rectangle for this flyout in viewport coordinates.
        abstract getClientRect: unit -> Blockly.Utils.Rect

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] HorizontalFlyout__ClassStatic =
        /// <summary>Class for a flyout.</summary>
        /// <param name="workspaceOptions">Dictionary of options for the workspace.</param>
        [<Emit "new $0($1...)">] abstract Create: workspaceOptions: Object -> HorizontalFlyout__Class

    type [<AllowNullLiteral>] VerticalFlyout =
        inherit VerticalFlyout__Class

    type [<AllowNullLiteral>] VerticalFlyoutStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> VerticalFlyout

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VerticalFlyout__Class =
        inherit Blockly.Flyout__Class
        /// Move the flyout to the edge of the workspace.
        abstract position: unit -> unit
        /// Scroll the flyout to the top.
        abstract scrollToStart: unit -> unit
        /// <summary>Determine if a drag delta is toward the workspace, based on the position
        /// and orientation of the flyout. This is used in determineDragIntention_ to
        /// determine if a new block should be created or if the flyout should scroll.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at mouse down, in pixel units.</param>
        abstract isDragTowardWorkspace: currentDragDeltaXY: Blockly.Utils.Coordinate -> bool
        /// Return the deletion rectangle for this flyout in viewport coordinates.
        abstract getClientRect: unit -> Blockly.Utils.Rect

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VerticalFlyout__ClassStatic =
        /// <summary>Class for a flyout.</summary>
        /// <param name="workspaceOptions">Dictionary of options for the workspace.</param>
        [<Emit "new $0($1...)">] abstract Create: workspaceOptions: Object -> VerticalFlyout__Class

    type [<AllowNullLiteral>] Generator =
        inherit Generator__Class

    type [<AllowNullLiteral>] GeneratorStatic =
        [<Emit "new $0($1...)">] abstract Create: name: string  -> Generator
        [<Emit "new $0($1...)">] abstract Create: unit  -> Generator

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Generator__Class =
        /// Arbitrary code to inject into locations that risk causing infinite loops.
        /// Any instances of '%1' will be replaced by the block ID that failed.
        /// E.g. '  checkTimeout(%1);\n'
        abstract INFINITE_LOOP_TRAP: string with get, set
        /// Arbitrary code to inject before every statement.
        /// Any instances of '%1' will be replaced by the block ID of the statement.
        /// E.g. 'highlight(%1);\n'
        abstract STATEMENT_PREFIX: string with get, set
        /// Arbitrary code to inject after every statement.
        /// Any instances of '%1' will be replaced by the block ID of the statement.
        /// E.g. 'highlight(%1);\n'
        abstract STATEMENT_SUFFIX: string with get, set
        /// The method of indenting.  Defaults to two spaces, but language generators
        /// may override this to increase indent or change to tabs.
        abstract INDENT: string with get, set
        /// Maximum length for a comment before wrapping.  Does not account for
        /// indenting level.
        abstract COMMENT_WRAP: float with get, set
        /// List of outer-inner pairings that do NOT require parentheses.
        abstract ORDER_OVERRIDES: ResizeArray<ResizeArray<float>> with get, set
        /// <summary>Generate code for all blocks in the workspace to the specified language.</summary>
        /// <param name="workspace">Workspace to generate code from.</param>
        abstract workspaceToCode: workspace: Blockly.Workspace -> string
        /// <summary>Prepend a common prefix onto each line of code.
        /// Intended for indenting code or adding comment markers.</summary>
        /// <param name="text">The lines of code.</param>
        /// <param name="prefix">The common prefix.</param>
        abstract prefixLines: text: string * prefix: string -> string
        /// <summary>Recursively spider a tree of blocks, returning all their comments.</summary>
        /// <param name="block">The block from which to start spidering.</param>
        abstract allNestedComments: block: Blockly.Block -> string
        /// <summary>Generate code for the specified block (and attached blocks).</summary>
        /// <param name="block">The block to generate code for.</param>
        /// <param name="opt_thisOnly">True to generate code for only this statement.</param>
        abstract blockToCode: block: Blockly.Block * ?opt_thisOnly: bool -> U2<string, ResizeArray<obj option>>
        /// <summary>Generate code representing the specified value input.</summary>
        /// <param name="block">The block containing the input.</param>
        /// <param name="name">The name of the input.</param>
        /// <param name="outerOrder">The maximum binding strength (minimum order value)
        /// of any operators adjacent to "block".</param>
        abstract valueToCode: block: Blockly.Block * name: string * outerOrder: float -> string
        /// <summary>Generate a code string representing the blocks attached to the named
        /// statement input. Indent the code.
        /// This is mainly used in generators. When trying to generate code to evaluate
        /// look at using workspaceToCode or blockToCode.</summary>
        /// <param name="block">The block containing the input.</param>
        /// <param name="name">The name of the input.</param>
        abstract statementToCode: block: Blockly.Block * name: string -> string
        /// <summary>Add an infinite loop trap to the contents of a loop.
        /// Add statement suffix at the start of the loop block (right after the loop
        /// statement executes), and a statement prefix to the end of the loop block
        /// (right before the loop statement executes).</summary>
        /// <param name="branch">Code for loop contents.</param>
        /// <param name="block">Enclosing block.</param>
        abstract addLoopTrap: branch: string * block: Blockly.Block -> string
        /// <summary>Inject a block ID into a message to replace '%1'.
        /// Used for STATEMENT_PREFIX, STATEMENT_SUFFIX, and INFINITE_LOOP_TRAP.</summary>
        /// <param name="msg">Code snippet with '%1'.</param>
        /// <param name="block">Block which has an ID.</param>
        abstract injectId: msg: string * block: Blockly.Block -> string
        /// <summary>Add one or more words to the list of reserved words for this language.</summary>
        /// <param name="words">Comma-separated list of words to add to the list.
        /// No spaces.  Duplicates are ok.</param>
        abstract addReservedWords: words: string -> unit
        /// <summary>Hook for code to run before code generation starts.
        /// Subclasses may override this, e.g. to initialise the database of variable
        /// names.</summary>
        /// <param name="_workspace">Workspace to generate code from.</param>
        abstract init: _workspace: Blockly.Workspace -> unit
        /// <summary>Hook for code to run at end of code generation.
        /// Subclasses may override this, e.g. to prepend the generated code with the
        /// variable definitions.</summary>
        /// <param name="code">Generated code.</param>
        abstract finish: code: string -> string
        /// <summary>Naked values are top-level blocks with outputs that aren't plugged into
        /// anything.
        /// Subclasses may override this, e.g. if their language does not allow
        /// naked values.</summary>
        /// <param name="line">Line of generated code.</param>
        abstract scrubNakedValue: line: string -> string

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Generator__ClassStatic =
        /// <summary>Class for a code generator that translates the blocks into a language.</summary>
        /// <param name="name">Language name of this generator.</param>
        [<Emit "new $0($1...)">] abstract Create: name: string -> Generator__Class

    module Generator =

        type [<AllowNullLiteral>] IExports =
            abstract NAME_TYPE: obj option

    type [<AllowNullLiteral>] Gesture =
        inherit Gesture__Class

    type [<AllowNullLiteral>] GestureStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Gesture

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Gesture__Class =
        /// The position of the mouse when the gesture started.  Units are CSS pixels,
        /// with (0, 0) at the top left of the browser window (mouseEvent clientX/Y).
        abstract mouseDownXY_: Blockly.Utils.Coordinate with get, set
        /// The workspace that the gesture started on.  There may be multiple
        /// workspaces on a page; this is more accurate than using
        /// Blockly.getMainWorkspace().
        abstract startWorkspace_: Blockly.WorkspaceSvg with get, set
        /// A handle to use to unbind a mouse move listener at the end of a drag.
        /// Opaque data returned from Blockly.bindEventWithChecks_.
        abstract onMoveWrapper_: ResizeArray<ResizeArray<obj option>> with get, set
        /// A handle to use to unbind a mouse up listener at the end of a drag.
        /// Opaque data returned from Blockly.bindEventWithChecks_.
        abstract onUpWrapper_: ResizeArray<ResizeArray<obj option>> with get, set
        /// Boolean used internally to break a cycle in disposal.
        abstract isEnding_: bool with get, set
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// <summary>Start a gesture: update the workspace to indicate that a gesture is in
        /// progress and bind mousemove and mouseup handlers.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        abstract doStart: e: Event -> unit
        /// <summary>Bind gesture events.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        abstract bindMouseEvents: e: Event -> unit
        /// <summary>Handle a mouse move or touch move event.</summary>
        /// <param name="e">A mouse move or touch move event.</param>
        abstract handleMove: e: Event -> unit
        /// <summary>Handle a mouse up or touch end event.</summary>
        /// <param name="e">A mouse up or touch end event.</param>
        abstract handleUp: e: Event -> unit
        /// Cancel an in-progress gesture.  If a workspace or block drag is in progress,
        /// end the drag at the most recent location.
        abstract cancel: unit -> unit
        /// <summary>Handle a real or faked right-click event by showing a context menu.</summary>
        /// <param name="e">A mouse move or touch move event.</param>
        abstract handleRightClick: e: Event -> unit
        /// <summary>Handle a mousedown/touchstart event on a workspace.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        /// <param name="ws">The workspace the event hit.</param>
        abstract handleWsStart: e: Event * ws: Blockly.Workspace -> unit
        /// <summary>Handle a mousedown/touchstart event on a flyout.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        /// <param name="flyout">The flyout the event hit.</param>
        abstract handleFlyoutStart: e: Event * flyout: Blockly.Flyout -> unit
        /// <summary>Handle a mousedown/touchstart event on a block.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        /// <param name="block">The block the event hit.</param>
        abstract handleBlockStart: e: Event * block: Blockly.BlockSvg -> unit
        /// <summary>Handle a mousedown/touchstart event on a bubble.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        /// <param name="bubble">The bubble the event hit.</param>
        abstract handleBubbleStart: e: Event * bubble: Blockly.Bubble -> unit
        /// <summary>Record the field that a gesture started on.</summary>
        /// <param name="field">The field the gesture started on.</param>
        abstract setStartField: field: Blockly.Field -> unit
        /// <summary>Record the bubble that a gesture started on</summary>
        /// <param name="bubble">The bubble the gesture started on.</param>
        abstract setStartBubble: bubble: Blockly.Bubble -> unit
        /// <summary>Record the block that a gesture started on, and set the target block
        /// appropriately.</summary>
        /// <param name="block">The block the gesture started on.</param>
        abstract setStartBlock: block: Blockly.BlockSvg -> unit
        /// Whether this gesture is a drag of either a workspace or block.
        /// This function is called externally to block actions that cannot be taken
        /// mid-drag (e.g. using the keyboard to delete the selected blocks).
        abstract isDragging: unit -> bool
        /// Whether this gesture has already been started.  In theory every mouse down
        /// has a corresponding mouse up, but in reality it is possible to lose a
        /// mouse up, leaving an in-process gesture hanging.
        abstract hasStarted: unit -> bool
        /// Get a list of the insertion markers that currently exist.  Block drags have
        /// 0, 1, or 2 insertion markers.
        abstract getInsertionMarkers: unit -> ResizeArray<Blockly.BlockSvg>

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Gesture__ClassStatic =
        /// <summary>Class for one gesture.</summary>
        /// <param name="e">The event that kicked off this gesture.</param>
        /// <param name="creatorWorkspace">The workspace that created
        /// this gesture and has a reference to it.</param>
        [<Emit "new $0($1...)">] abstract Create: e: Event * creatorWorkspace: Blockly.WorkspaceSvg -> Gesture__Class

    module Gesture =

        type [<AllowNullLiteral>] IExports =
            /// Is a drag or other gesture currently in progress on any workspace?
            abstract inProgress: unit -> bool

    type [<AllowNullLiteral>] Grid =
        inherit Grid__Class

    type [<AllowNullLiteral>] GridStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Grid

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Grid__Class =
        /// Dispose of this grid and unlink from the DOM.
        abstract dispose: unit -> unit
        /// Whether blocks should snap to the grid, based on the initial configuration.
        abstract shouldSnap: unit -> bool
        /// Get the spacing of the grid points (in px).
        abstract getSpacing: unit -> float
        /// Get the id of the pattern element, which should be randomized to avoid
        /// conflicts with other Blockly instances on the page.
        abstract getPatternId: unit -> string
        /// <summary>Update the grid with a new scale.</summary>
        /// <param name="scale">The new workspace scale.</param>
        abstract update: scale: float -> unit
        /// <summary>Move the grid to a new x and y position, and make sure that change is
        /// visible.</summary>
        /// <param name="x">The new x position of the grid (in px).</param>
        /// <param name="y">The new y position ofthe grid (in px).</param>
        abstract moveTo: x: float * y: float -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Grid__ClassStatic =
        /// <summary>Class for a workspace's grid.</summary>
        /// <param name="pattern">The grid's SVG pattern, created during
        /// injection.</param>
        /// <param name="options">A dictionary of normalized options for the grid.
        /// See grid documentation:
        /// https://developers.google.com/blockly/guides/configure/web/grid</param>
        [<Emit "new $0($1...)">] abstract Create: pattern: SVGElement * options: Object -> Grid__Class

    module Grid =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Create the DOM for the grid described by options.</summary>
            /// <param name="rnd">A random ID to append to the pattern's ID.</param>
            /// <param name="gridOptions">The object containing grid configuration.</param>
            /// <param name="defs">The root SVG element for this workspace's defs.</param>
            abstract createDom: rnd: string * gridOptions: Object * defs: SVGElement -> SVGElement

    type [<AllowNullLiteral>] Icon =
        inherit Icon__Class

    type [<AllowNullLiteral>] IconStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Icon

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Icon__Class =
        /// The block this icon is attached to.
        abstract block_: Blockly.BlockSvg with get, set
        /// Does this icon get hidden when the block is collapsed.
        abstract collapseHidden: obj option with get, set
        /// Height and width of icons.
        abstract SIZE: obj option with get, set
        /// Bubble UI (if visible).
        abstract bubble_: Blockly.Bubble with get, set
        /// Absolute coordinate of icon's center.
        abstract iconXY_: Blockly.Utils.Coordinate with get, set
        /// Create the icon on the block.
        abstract createIcon: unit -> unit
        /// Dispose of this icon.
        abstract dispose: unit -> unit
        /// Add or remove the UI indicating if this icon may be clicked or not.
        abstract updateEditable: unit -> unit
        /// Is the associated bubble visible?
        abstract isVisible: unit -> bool
        /// <summary>Clicking on the icon toggles if the bubble is visible.</summary>
        /// <param name="e">Mouse click event.</param>
        abstract iconClick_: e: Event -> unit
        /// Change the colour of the associated bubble to match its block.
        abstract updateColour: unit -> unit
        /// <summary>Notification that the icon has moved.  Update the arrow accordingly.</summary>
        /// <param name="xy">Absolute location in workspace coordinates.</param>
        abstract setIconLocation: xy: Blockly.Utils.Coordinate -> unit
        /// Notification that the icon has moved, but we don't really know where.
        /// Recompute the icon's location from scratch.
        abstract computeIconLocation: unit -> unit
        /// Returns the center of the block's icon relative to the surface.
        abstract getIconLocation: unit -> Blockly.Utils.Coordinate
        /// Get the size of the icon as used for rendering.
        /// This differs from the actual size of the icon, because it bulges slightly
        /// out of its row rather than increasing the height of its row.
        /// TODO (#2562): Remove getCorrectedSize.
        abstract getCorrectedSize: unit -> Blockly.Utils.Size

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Icon__ClassStatic =
        /// <summary>Class for an icon.</summary>
        /// <param name="block">The block associated with this icon.</param>
        [<Emit "new $0($1...)">] abstract Create: block: Blockly.BlockSvg -> Icon__Class

    type [<AllowNullLiteral>] Input =
        inherit Input__Class

    type [<AllowNullLiteral>] InputStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Input

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Input__Class =
        abstract ``type``: float with get, set
        abstract name: string with get, set
        abstract connection: Blockly.Connection with get, set
        abstract fieldRow: ResizeArray<Blockly.Field> with get, set
        /// Alignment of input's fields (left, right or centre).
        abstract align: float with get, set
        /// Get the source block for this input.
        abstract getSourceBlock: unit -> Blockly.Block
        /// <summary>Add a field (or label from string), and all prefix and suffix fields, to the
        /// end of the input's field row.</summary>
        /// <param name="field">Something to add as a field.</param>
        /// <param name="opt_name">Language-neutral identifier which may used to find
        /// this field again.  Should be unique to the host block.</param>
        abstract appendField: field: U2<string, Blockly.Field> * ?opt_name: string -> Blockly.Input
        /// <summary>Inserts a field (or label from string), and all prefix and suffix fields, at
        /// the location of the input's field row.</summary>
        /// <param name="index">The index at which to insert field.</param>
        /// <param name="field">Something to add as a field.</param>
        /// <param name="opt_name">Language-neutral identifier which may used to find
        /// this field again.  Should be unique to the host block.</param>
        abstract insertFieldAt: index: float * field: U2<string, Blockly.Field> * ?opt_name: string -> float
        /// <summary>Remove a field from this input.</summary>
        /// <param name="name">The name of the field.</param>
        abstract removeField: name: string -> unit
        /// Gets whether this input is visible or not.
        abstract isVisible: unit -> bool
        /// <summary>Sets whether this input is visible or not.
        /// Should only be used to collapse/uncollapse a block.</summary>
        /// <param name="visible">True if visible.</param>
        abstract setVisible: visible: bool -> ResizeArray<Blockly.Block>
        /// Mark all fields on this input as dirty.
        abstract markDirty: unit -> unit
        /// <summary>Change a connection's compatibility.</summary>
        /// <param name="check">Compatible value type or
        /// list of value types.  Null if all types are compatible.</param>
        abstract setCheck: check: U3<string, ResizeArray<string>, obj option> -> Blockly.Input
        /// <summary>Change the alignment of the connection's field(s).</summary>
        /// <param name="align">One of Blockly.ALIGN_LEFT, ALIGN_CENTRE, ALIGN_RIGHT.
        /// In RTL mode directions are reversed, and ALIGN_RIGHT aligns to the left.</param>
        abstract setAlign: align: float -> Blockly.Input
        /// Initialize the fields on this input.
        abstract init: unit -> unit
        /// Sever all links to this input.
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Input__ClassStatic =
        /// <summary>Class for an input with an optional field.</summary>
        /// <param name="type">The type of the input.</param>
        /// <param name="name">Language-neutral identifier which may used to find this
        /// input again.</param>
        /// <param name="block">The block containing this input.</param>
        /// <param name="connection">Optional connection for this input.</param>
        [<Emit "new $0($1...)">] abstract Create: ``type``: float * name: string * block: Blockly.Block * connection: Blockly.Connection -> Input__Class

    type [<AllowNullLiteral>] InsertionMarkerManager =
        inherit InsertionMarkerManager__Class

    type [<AllowNullLiteral>] InsertionMarkerManagerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> InsertionMarkerManager

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] InsertionMarkerManager__Class =
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// Return whether the block would be deleted if dropped immediately, based on
        /// information from the most recent move event.
        abstract wouldDeleteBlock: unit -> bool
        /// Return whether the block would be connected if dropped immediately, based on
        /// information from the most recent move event.
        abstract wouldConnectBlock: unit -> bool
        /// Connect to the closest connection and render the results.
        /// This should be called at the end of a drag.
        abstract applyConnections: unit -> unit
        /// <summary>Update highlighted connections based on the most recent move location.</summary>
        /// <param name="dxy">Position relative to drag start,
        /// in workspace units.</param>
        /// <param name="deleteArea">One of {</param>
        abstract update: dxy: Blockly.Utils.Coordinate * deleteArea: float -> unit
        /// <summary>Find the nearest valid connection, which may be the same as the current
        /// closest connection.</summary>
        /// <param name="dxy">Position relative to drag start,
        /// in workspace units.</param>
        abstract getCandidate_: dxy: Blockly.Utils.Coordinate -> Object
        /// Add highlighting showing which block will be replaced.
        abstract highlightBlock_: unit -> unit
        /// Get rid of the highlighting marking the block that will be replaced.
        abstract unhighlightBlock_: unit -> unit
        /// Get a list of the insertion markers that currently exist.  Drags have 0, 1,
        /// or 2 insertion markers.
        abstract getInsertionMarkers: unit -> ResizeArray<Blockly.BlockSvg>

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] InsertionMarkerManager__ClassStatic =
        /// <summary>Class that controls updates to connections during drags.  It is primarily
        /// responsible for finding the closest eligible connection and highlighting or
        /// unhiglighting it as needed during a drag.</summary>
        /// <param name="block">The top block in the stack being dragged.</param>
        [<Emit "new $0($1...)">] abstract Create: block: Blockly.BlockSvg -> InsertionMarkerManager__Class

    type [<AllowNullLiteral>] Mutator =
        inherit Mutator__Class

    type [<AllowNullLiteral>] MutatorStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Mutator

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Mutator__Class =
        inherit Blockly.Icon__Class
        /// Add or remove the UI indicating if this icon may be clicked or not.
        abstract updateEditable: unit -> unit
        /// <summary>Show or hide the mutator bubble.</summary>
        /// <param name="visible">True if the bubble should be visible.</param>
        abstract setVisible: visible: bool -> unit
        /// Dispose of this mutator.
        abstract dispose: unit -> unit
        /// Update the styles on all blocks in the mutator.
        abstract updateBlockStyle: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Mutator__ClassStatic =
        /// <summary>Class for a mutator dialog.</summary>
        /// <param name="quarkNames">List of names of sub-blocks for flyout.</param>
        [<Emit "new $0($1...)">] abstract Create: quarkNames: ResizeArray<string> -> Mutator__Class

    module Mutator =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Reconnect an block to a mutated input.</summary>
            /// <param name="connectionChild">Connection on child block.</param>
            /// <param name="block">Parent block.</param>
            /// <param name="inputName">Name of input on parent block.</param>
            abstract reconnect: connectionChild: Blockly.Connection * block: Blockly.Block * inputName: string -> bool
            /// <summary>Get the parent workspace of a workspace that is inside a mutator, taking into
            /// account whether it is a flyout.</summary>
            /// <param name="workspace">The workspace that is inside a mutator.</param>
            abstract findParentWs: workspace: Blockly.Workspace -> Blockly.Workspace

    type [<AllowNullLiteral>] Names =
        inherit Names__Class

    type [<AllowNullLiteral>] NamesStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Names

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Names__Class =
        /// Empty the database and start from scratch.  The reserved words are kept.
        abstract reset: unit -> unit
        /// <summary>Set the variable map that maps from variable name to variable object.</summary>
        /// <param name="map">The map to track.</param>
        abstract setVariableMap: map: Blockly.VariableMap -> unit
        /// <summary>Convert a Blockly entity name to a legal exportable entity name.</summary>
        /// <param name="name">The Blockly entity name (no constraints).</param>
        /// <param name="type">The type of entity in Blockly
        /// ('VARIABLE', 'PROCEDURE', 'BUILTIN', etc...).</param>
        abstract getName: name: string * ``type``: string -> string
        /// <summary>Convert a Blockly entity name to a legal exportable entity name.
        /// Ensure that this is a new name not overlapping any previously defined name.
        /// Also check against list of reserved words for the current language and
        /// ensure name doesn't collide.</summary>
        /// <param name="name">The Blockly entity name (no constraints).</param>
        /// <param name="type">The type of entity in Blockly
        /// ('VARIABLE', 'PROCEDURE', 'BUILTIN', etc...).</param>
        abstract getDistinctName: name: string * ``type``: string -> string

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Names__ClassStatic =
        /// <summary>Class for a database of entity names (variables, functions, etc).</summary>
        /// <param name="reservedWords">A comma-separated string of words that are
        /// illegal for use as names in a language (e.g. 'new,if,this,...').</param>
        /// <param name="opt_variablePrefix">Some languages need a '$' or a namespace
        /// before all variable names.</param>
        [<Emit "new $0($1...)">] abstract Create: reservedWords: string * ?opt_variablePrefix: string -> Names__Class

    module Names =

        type [<AllowNullLiteral>] IExports =
            abstract DEVELOPER_VARIABLE_TYPE: obj option
            /// <summary>Do the given two entity names refer to the same entity?
            /// Blockly names are case-insensitive.</summary>
            /// <param name="name1">First name.</param>
            /// <param name="name2">Second name.</param>
            abstract equals: name1: string * name2: string -> bool

    type [<AllowNullLiteral>] Options =
        inherit Options__Class

    type [<AllowNullLiteral>] OptionsStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Options

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Options__Class =
        abstract hasScrollbars: obj option with get, set
        /// The parent of the current workspace, or null if there is no parent workspace.
        abstract parentWorkspace: Blockly.Workspace with get, set
        /// If set, sets the translation of the workspace to match the scrollbars.
        abstract setMetrics: obj option with get, set
        /// Return an object with the metrics required to size the workspace.
        abstract getMetrics: unit -> Object

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Options__ClassStatic =
        /// <summary>Parse the user-specified options, using reasonable defaults where behaviour
        /// is unspecified.</summary>
        /// <param name="options">Dictionary of options.  Specification:
        /// https://developers.google.com/blockly/guides/get-started/web#configuration</param>
        [<Emit "new $0($1...)">] abstract Create: options: Object -> Options__Class

    module Options =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Parse the provided toolbox tree into a consistent DOM format.</summary>
            /// <param name="tree">DOM tree of blocks, or text representation of same.</param>
            abstract parseToolboxTree: tree: U2<Node, string> -> Node

    module Procedures =

        type [<AllowNullLiteral>] IExports =
            abstract NAME_TYPE: obj option
            /// <summary>Find all user-created procedure definitions in a workspace.</summary>
            /// <param name="root">Root workspace.</param>
            abstract allProcedures: root: Blockly.Workspace -> ResizeArray<ResizeArray<ResizeArray<obj option>>>
            /// <summary>Ensure two identically-named procedures don't exist.
            /// Take the proposed procedure name, and return a legal name i.e. one that
            /// is not empty and doesn't collide with other procedures.</summary>
            /// <param name="name">Proposed procedure name.</param>
            /// <param name="block">Block to disambiguate.</param>
            abstract findLegalName: name: string * block: Blockly.Block -> string
            /// <summary>Return if the given name is already a procedure name.</summary>
            /// <param name="name">The questionable name.</param>
            /// <param name="workspace">The workspace to scan for collisions.</param>
            /// <param name="opt_exclude">Optional block to exclude from
            /// comparisons (one doesn't want to collide with oneself).</param>
            abstract isNameUsed: name: string * workspace: Blockly.Workspace * ?opt_exclude: Blockly.Block -> bool
            /// <summary>Rename a procedure.  Called by the editable field.</summary>
            /// <param name="name">The proposed new name.</param>
            abstract rename: name: string -> string
            /// <summary>Construct the blocks required by the flyout for the procedure category.</summary>
            /// <param name="workspace">The workspace containing procedures.</param>
            abstract flyoutCategory: workspace: Blockly.Workspace -> ResizeArray<Element>
            /// <summary>Find all the callers of a named procedure.</summary>
            /// <param name="name">Name of procedure.</param>
            /// <param name="workspace">The workspace to find callers in.</param>
            abstract getCallers: name: string * workspace: Blockly.Workspace -> ResizeArray<Blockly.Block>
            /// <summary>When a procedure definition changes its parameters, find and edit all its
            /// callers.</summary>
            /// <param name="defBlock">Procedure definition block.</param>
            abstract mutateCallers: defBlock: Blockly.Block -> unit
            /// <summary>Find the definition block for the named procedure.</summary>
            /// <param name="name">Name of procedure.</param>
            /// <param name="workspace">The workspace to search.</param>
            abstract getDefinition: name: string * workspace: Blockly.Workspace -> Blockly.Block

    type [<AllowNullLiteral>] RenderedConnection =
        inherit RenderedConnection__Class

    type [<AllowNullLiteral>] RenderedConnectionStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> RenderedConnection

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] RenderedConnection__Class =
        inherit Blockly.Connection__Class
        /// <summary>Returns the distance between this connection and another connection in
        /// workspace units.</summary>
        /// <param name="otherConnection">The other connection to measure
        /// the distance to.</param>
        abstract distanceFrom: otherConnection: Blockly.Connection -> float
        /// <summary>Change the connection's coordinates.</summary>
        /// <param name="x">New absolute x coordinate, in workspace coordinates.</param>
        /// <param name="y">New absolute y coordinate, in workspace coordinates.</param>
        abstract moveTo: x: float * y: float -> unit
        /// <summary>Change the connection's coordinates.</summary>
        /// <param name="dx">Change to x coordinate, in workspace units.</param>
        /// <param name="dy">Change to y coordinate, in workspace units.</param>
        abstract moveBy: dx: float * dy: float -> unit
        /// <summary>Move this connection to the location given by its offset within the block and
        /// the location of the block's top left corner.</summary>
        /// <param name="blockTL">The location of the top left corner
        /// of the block, in workspace coordinates.</param>
        abstract moveToOffset: blockTL: Blockly.Utils.Coordinate -> unit
        /// <summary>Set the offset of this connection relative to the top left of its block.</summary>
        /// <param name="x">The new relative x, in workspace units.</param>
        /// <param name="y">The new relative y, in workspace units.</param>
        abstract setOffsetInBlock: x: float * y: float -> unit
        /// Get the offset of this connection relative to the top left of its block.
        abstract getOffsetInBlock: unit -> Blockly.Utils.Coordinate
        /// <summary>Find the closest compatible connection to this connection.
        /// All parameters are in workspace units.</summary>
        /// <param name="maxLimit">The maximum radius to another connection.</param>
        /// <param name="dxy">Offset between this connection's location
        /// in the database and the current location (as a result of dragging).</param>
        abstract closest: maxLimit: float * dxy: Blockly.Utils.Coordinate -> RenderedConnection__ClassClosestReturn
        /// Add highlighting around this connection.
        abstract highlight: unit -> unit
        /// Unhide this connection, as well as all down-stream connections on any block
        /// attached to this connection.  This happens when a block is expanded.
        /// Also unhides down-stream comments.
        abstract unhideAll: unit -> ResizeArray<Blockly.Block>
        /// Remove the highlighting around this connection.
        abstract unhighlight: unit -> unit
        /// <summary>Set whether this connections is hidden (not tracked in a database) or not.</summary>
        /// <param name="hidden">True if connection is hidden.</param>
        abstract setHidden: hidden: bool -> unit
        /// Hide this connection, as well as all down-stream connections on any block
        /// attached to this connection.  This happens when a block is collapsed.
        /// Also hides down-stream comments.
        abstract hideAll: unit -> unit
        /// <summary>Check if the two connections can be dragged to connect to each other.</summary>
        /// <param name="candidate">A nearby connection to check.</param>
        /// <param name="maxRadius">The maximum radius allowed for connections, in
        /// workspace units.</param>
        abstract isConnectionAllowed: candidate: Blockly.Connection * ?maxRadius: float -> bool
        /// <summary>Behavior after a connection attempt fails.</summary>
        /// <param name="otherConnection">Connection that this connection
        /// failed to connect to.</param>
        abstract onFailedConnect: otherConnection: Blockly.Connection -> unit

    type [<AllowNullLiteral>] RenderedConnection__ClassClosestReturn =
        abstract connection: Blockly.Connection with get, set
        abstract radius: float with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] RenderedConnection__ClassStatic =
        /// <summary>Class for a connection between blocks that may be rendered on screen.</summary>
        /// <param name="source">The block establishing this connection.</param>
        /// <param name="type">The type of the connection.</param>
        [<Emit "new $0($1...)">] abstract Create: source: Blockly.BlockSvg * ``type``: float -> RenderedConnection__Class

    type [<AllowNullLiteral>] ScrollbarPair =
        inherit ScrollbarPair__Class

    type [<AllowNullLiteral>] ScrollbarPairStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> ScrollbarPair

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ScrollbarPair__Class =
        /// Dispose of this pair of scrollbars.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// Recalculate both of the scrollbars' locations and lengths.
        /// Also reposition the corner rectangle.
        abstract resize: unit -> unit
        /// <summary>Set the handles of both scrollbars to be at a certain position in CSS pixels
        /// relative to their parents.</summary>
        /// <param name="x">Horizontal scroll value.</param>
        /// <param name="y">Vertical scroll value.</param>
        abstract set: x: float * y: float -> unit
        /// <summary>Set whether this scrollbar's container is visible.</summary>
        /// <param name="visible">Whether the container is visible.</param>
        abstract setContainerVisible: visible: bool -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ScrollbarPair__ClassStatic =
        /// <summary>Class for a pair of scrollbars.  Horizontal and vertical.</summary>
        /// <param name="workspace">Workspace to bind the scrollbars to.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace -> ScrollbarPair__Class

    type [<AllowNullLiteral>] Scrollbar =
        inherit Scrollbar__Class

    type [<AllowNullLiteral>] ScrollbarStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Scrollbar

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Scrollbar__Class =
        /// The position of the mouse along this scrollbar's major axis at the start of
        /// the most recent drag.
        /// Units are CSS pixels, with (0, 0) at the top left of the browser window.
        /// For a horizontal scrollbar this is the x coordinate of the mouse down event;
        /// for a vertical scrollbar it's the y coordinate of the mouse down event.
        abstract startDragMouse_: Blockly.Utils.Coordinate with get, set
        /// Dispose of this scrollbar.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// <summary>Set the length of the scrollbar's handle and change the SVG attribute
        /// accordingly.</summary>
        /// <param name="newLength">The new scrollbar handle length in CSS pixels.</param>
        abstract setHandleLength_: newLength: float -> unit
        /// <summary>Set the offset of the scrollbar's handle from the scrollbar's position, and
        /// change the SVG attribute accordingly.</summary>
        /// <param name="newPosition">The new scrollbar handle offset in CSS pixels.</param>
        abstract setHandlePosition: newPosition: float -> unit
        /// <summary>Recalculate the scrollbar's location and its length.</summary>
        /// <param name="opt_metrics">A data structure of from the describing all the
        /// required dimensions.  If not provided, it will be fetched from the host
        /// object.</param>
        abstract resize: ?opt_metrics: Object -> unit
        /// <summary>Recalculate a horizontal scrollbar's location on the screen and path length.
        /// This should be called when the layout or size of the window has changed.</summary>
        /// <param name="hostMetrics">A data structure describing all the
        /// required dimensions, possibly fetched from the host object.</param>
        abstract resizeViewHorizontal: hostMetrics: Object -> unit
        /// <summary>Recalculate a horizontal scrollbar's location within its path and length.
        /// This should be called when the contents of the workspace have changed.</summary>
        /// <param name="hostMetrics">A data structure describing all the
        /// required dimensions, possibly fetched from the host object.</param>
        abstract resizeContentHorizontal: hostMetrics: Object -> unit
        /// <summary>Recalculate a vertical scrollbar's location on the screen and path length.
        /// This should be called when the layout or size of the window has changed.</summary>
        /// <param name="hostMetrics">A data structure describing all the
        /// required dimensions, possibly fetched from the host object.</param>
        abstract resizeViewVertical: hostMetrics: Object -> unit
        /// <summary>Recalculate a vertical scrollbar's location within its path and length.
        /// This should be called when the contents of the workspace have changed.</summary>
        /// <param name="hostMetrics">A data structure describing all the
        /// required dimensions, possibly fetched from the host object.</param>
        abstract resizeContentVertical: hostMetrics: Object -> unit
        /// Is the scrollbar visible.  Non-paired scrollbars disappear when they aren't
        /// needed.
        abstract isVisible: unit -> bool
        /// <summary>Set whether the scrollbar's container is visible and update
        /// display accordingly if visibility has changed.</summary>
        /// <param name="visible">Whether the container is visible</param>
        abstract setContainerVisible: visible: bool -> unit
        /// <summary>Set whether the scrollbar is visible.
        /// Only applies to non-paired scrollbars.</summary>
        /// <param name="visible">True if visible.</param>
        abstract setVisible: visible: bool -> unit
        /// Update visibility of scrollbar based on whether it thinks it should
        /// be visible and whether its containing workspace is visible.
        /// We cannot rely on the containing workspace being hidden to hide us
        /// because it is not necessarily our parent in the DOM.
        abstract updateDisplay_: unit -> unit
        /// <summary>Set the scrollbar handle's position.</summary>
        /// <param name="value">The distance from the top/left end of the bar, in CSS
        /// pixels.  It may be larger than the maximum allowable position of the
        /// scrollbar handle.</param>
        abstract set: value: float -> unit
        /// <summary>Record the origin of the workspace that the scrollbar is in, in pixels
        /// relative to the injection div origin. This is for times when the scrollbar is
        /// used in an object whose origin isn't the same as the main workspace
        /// (e.g. in a flyout.)</summary>
        /// <param name="x">The x coordinate of the scrollbar's origin, in CSS pixels.</param>
        /// <param name="y">The y coordinate of the scrollbar's origin, in CSS pixels.</param>
        abstract setOrigin: x: float * y: float -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Scrollbar__ClassStatic =
        /// <summary>Class for a pure SVG scrollbar.
        /// This technique offers a scrollbar that is guaranteed to work, but may not
        /// look or behave like the system's scrollbars.</summary>
        /// <param name="workspace">Workspace to bind the scrollbar to.</param>
        /// <param name="horizontal">True if horizontal, false if vertical.</param>
        /// <param name="opt_pair">True if scrollbar is part of a horiz/vert pair.</param>
        /// <param name="opt_class">A class to be applied to this scrollbar.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace * horizontal: bool * ?opt_pair: bool * ?opt_class: string -> Scrollbar__Class

    module Scrollbar =

        type [<AllowNullLiteral>] IExports =
            abstract scrollbarThickness: obj option

    type [<AllowNullLiteral>] Theme =
        inherit Theme__Class

    type [<AllowNullLiteral>] ThemeStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Theme

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Theme__Class =
        /// The block styles map.
        abstract blockStyles_: TypeLiteral_14 with get, set
        /// The category styles map.
        abstract categoryStyles_: TypeLiteral_15 with get, set
        /// The UI components styles map.
        abstract componentStyles_: TypeLiteral_16 with get, set
        /// <summary>Overrides or adds all values from blockStyles to blockStyles_</summary>
        /// <param name="blockStyles">Map of
        /// block styles.</param>
        abstract setAllBlockStyles: blockStyles: Theme__ClassSetAllBlockStylesBlockStyles -> unit
        /// Gets a map of all the block style names.
        abstract getAllBlockStyles: unit -> Theme__ClassGetAllBlockStylesReturn
        /// <summary>Gets the BlockStyle for the given block style name.</summary>
        /// <param name="blockStyleName">The name of the block style.</param>
        abstract getBlockStyle: blockStyleName: string -> U2<Blockly.Theme.BlockStyle, obj option>
        /// <summary>Overrides or adds a style to the blockStyles map.</summary>
        /// <param name="blockStyleName">The name of the block style.</param>
        /// <param name="blockStyle">The block style.</param>
        abstract setBlockStyle: blockStyleName: string * blockStyle: Blockly.Theme.BlockStyle -> unit
        /// <summary>Gets the CategoryStyle for the given category style name.</summary>
        /// <param name="categoryStyleName">The name of the category style.</param>
        abstract getCategoryStyle: categoryStyleName: string -> U2<Blockly.Theme.CategoryStyle, obj option>
        /// <summary>Overrides or adds a style to the categoryStyles map.</summary>
        /// <param name="categoryStyleName">The name of the category style.</param>
        /// <param name="categoryStyle">The category style.</param>
        abstract setCategoryStyle: categoryStyleName: string * categoryStyle: Blockly.Theme.CategoryStyle -> unit
        /// <summary>Gets the style for a given Blockly UI component.  If the style value is a
        /// string, we attempt to find the value of any named references.</summary>
        /// <param name="componentName">The name of the component.</param>
        abstract getComponentStyle: componentName: string -> string
        /// <summary>Configure a specific Blockly UI component with a style value.</summary>
        /// <param name="componentName">The name of the component.</param>
        /// <param name="styleValue">The style value.</param>
        abstract setComponentStyle: componentName: string * styleValue: obj option -> unit

    type [<AllowNullLiteral>] Theme__ClassSetAllBlockStylesBlockStyles =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.BlockStyle with get, set

    type [<AllowNullLiteral>] Theme__ClassGetAllBlockStylesReturn =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.BlockStyle with get, set

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Theme__ClassStatic =
        /// <summary>Class for a theme.</summary>
        /// <param name="blockStyles">A map from
        /// style names (strings) to objects with style attributes for blocks.</param>
        /// <param name="categoryStyles">A map
        /// from style names (strings) to objects with style attributes for
        /// categories.</param>
        /// <param name="opt_componentStyles">A map of Blockly component
        /// names to style value.</param>
        [<Emit "new $0($1...)">] abstract Create: blockStyles: Theme__ClassStaticBlockStyles * categoryStyles: Theme__ClassStaticCategoryStyles * ?opt_componentStyles: Theme__ClassStaticOpt_componentStyles -> Theme__Class

    type [<AllowNullLiteral>] Theme__ClassStaticBlockStyles =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.BlockStyle with get, set

    type [<AllowNullLiteral>] Theme__ClassStaticCategoryStyles =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.CategoryStyle with get, set

    type [<AllowNullLiteral>] Theme__ClassStaticOpt_componentStyles =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

    module Theme =

        /// A block style.
        type [<AllowNullLiteral>] BlockStyle =
            abstract colourPrimary: string with get, set
            abstract colourSecondary: string with get, set
            abstract colourTertiary: string with get, set
            abstract hat: string with get, set

        /// A category style.
        type [<AllowNullLiteral>] CategoryStyle =
            abstract colour: string with get, set

    type [<AllowNullLiteral>] ThemeManager =
        inherit ThemeManager__Class

    type [<AllowNullLiteral>] ThemeManagerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> ThemeManager

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ThemeManager__Class =
        /// Get the workspace theme.
        abstract getTheme: unit -> Blockly.Theme
        /// <summary>Set the workspace theme, and refresh the workspace and all components.</summary>
        /// <param name="theme">The workspace theme.</param>
        abstract setTheme: theme: Blockly.Theme -> unit
        /// <summary>Subscribe a workspace to changes to the selected theme.  If a new theme is
        /// set, the workspace is called to refresh its blocks.</summary>
        /// <param name="workspace">The workspace to subscribe.</param>
        abstract subscribeWorkspace: workspace: Blockly.Workspace -> unit
        /// <summary>Unsubscribe a workspace to changes to the selected theme.</summary>
        /// <param name="workspace">The workspace to unsubscribe.</param>
        abstract unsubscribeWorkspace: workspace: Blockly.Workspace -> unit
        /// <summary>Subscribe an element to changes to the selected theme.  If a new theme is
        /// selected, the element's style is refreshed with the new theme's style.</summary>
        /// <param name="element">The element to subscribe.</param>
        /// <param name="componentName">The name used to identify the component. This
        /// must be the same name used to configure the style in the Theme object.</param>
        /// <param name="propertyName">The inline style property name to update.</param>
        abstract subscribe: element: Element * componentName: string * propertyName: string -> unit
        /// <summary>Unsubscribe an element to changes to the selected theme.</summary>
        /// <param name="element">The element to unsubscribe.</param>
        abstract unsubscribe: element: Element -> unit
        /// Dispose of this theme manager.
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ThemeManager__ClassStatic =
        /// <summary>Class for storing and updating a workspace's theme and UI components.</summary>
        /// <param name="theme">The workspace theme.</param>
        [<Emit "new $0($1...)">] abstract Create: theme: Blockly.Theme -> ThemeManager__Class

    module ThemeManager =

        /// A Blockly UI component type.
        type [<AllowNullLiteral>] Component =
            abstract element: Element with get, set
            abstract propertyName: string with get, set

    type [<AllowNullLiteral>] Toolbox =
        inherit Toolbox__Class

    type [<AllowNullLiteral>] ToolboxStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Toolbox

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Toolbox__Class =
        /// Is RTL vs LTR.
        abstract RTL: bool with get, set
        /// Position of the toolbox and flyout relative to the workspace.
        abstract toolboxPosition: float with get, set
        /// Width of the toolbox, which changes only in vertical layout.
        abstract width: float with get, set
        /// Height of the toolbox, which changes only in horizontal layout.
        abstract height: float with get, set
        /// Initializes the toolbox.
        abstract init: unit -> unit
        /// HTML container for the Toolbox menu.
        abstract HtmlDiv: Element with get, set
        /// <summary>Fill the toolbox with categories and blocks.</summary>
        /// <param name="languageTree">DOM tree of blocks.</param>
        abstract renderTree: languageTree: Node -> unit
        /// <summary>Handles the given Blockly action on a toolbox.
        /// This is only triggered when keyboard accessibility mode is enabled.</summary>
        /// <param name="action">The action to be handled.</param>
        abstract onBlocklyAction: action: Blockly.Action -> bool
        /// Dispose of this toolbox.
        abstract dispose: unit -> unit
        /// Get the width of the toolbox.
        abstract getWidth: unit -> float
        /// Get the height of the toolbox.
        abstract getHeight: unit -> float
        /// Move the toolbox to the edge.
        abstract position: unit -> unit
        /// <summary>Retrieves and sets the colour for the category using the style name.
        /// The category colour is set from the colour style attribute.</summary>
        /// <param name="styleName">Name of the style.</param>
        /// <param name="childOut">The child to set the hexColour on.</param>
        /// <param name="categoryName">Name of the toolbox category.</param>
        abstract setColourFromStyle_: styleName: string * childOut: Blockly.Tree.TreeNode * categoryName: string -> unit
        /// Updates the category colours and background colour of selected categories.
        abstract updateColourFromTheme: unit -> unit
        /// Unhighlight any previously specified option.
        abstract clearSelection: unit -> unit
        /// <summary>Adds a style on the toolbox. Usually used to change the cursor.</summary>
        /// <param name="style">The name of the class to add.</param>
        abstract addStyle: style: string -> unit
        /// <summary>Removes a style from the toolbox. Usually used to change the cursor.</summary>
        /// <param name="style">The name of the class to remove.</param>
        abstract removeStyle: style: string -> unit
        /// Return the deletion rectangle for this toolbox.
        abstract getClientRect: unit -> Blockly.Utils.Rect
        /// Update the flyout's contents without closing it.  Should be used in response
        /// to a change in one of the dynamic categories, such as variables or
        /// procedures.
        abstract refreshSelection: unit -> unit
        /// Select the first toolbox category if no category is selected.
        abstract selectFirstCategory: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Toolbox__ClassStatic =
        /// <summary>Class for a Toolbox.
        /// Creates the toolbox's DOM.</summary>
        /// <param name="workspace">The workspace in which to create new
        /// blocks.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg -> Toolbox__Class

    module Toolbox =

        type [<AllowNullLiteral>] IExports =
            abstract TreeSeparator: TreeSeparatorStatic
            abstract TreeSeparator__Class: TreeSeparator__ClassStatic

        type [<AllowNullLiteral>] TreeSeparator =
            inherit TreeSeparator__Class

        type [<AllowNullLiteral>] TreeSeparatorStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> TreeSeparator

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeSeparator__Class =
            inherit Blockly.Tree.TreeNode__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeSeparator__ClassStatic =
            /// <summary>A blank separator node in the tree.</summary>
            /// <param name="config">The configuration for the tree.</param>
            [<Emit "new $0($1...)">] abstract Create: config: Blockly.Tree.BaseNode.Config -> TreeSeparator__Class

    module Tooltip =

        type [<AllowNullLiteral>] IExports =
            abstract visible: obj option
            abstract LIMIT: obj option
            abstract OFFSET_X: obj option
            abstract OFFSET_Y: obj option
            abstract RADIUS_OK: obj option
            abstract HOVER_MS: obj option
            abstract MARGINS: obj option
            abstract DIV: Element
            /// Create the tooltip div and inject it onto the page.
            abstract createDom: unit -> unit
            /// <summary>Binds the required mouse events onto an SVG element.</summary>
            /// <param name="element">SVG element onto which tooltip is to be bound.</param>
            abstract bindMouseEvents: element: Element -> unit
            /// Hide the tooltip.
            abstract hide: unit -> unit
            /// Hide any in-progress tooltips and block showing new tooltips until the next
            /// call to unblock().
            abstract block: unit -> unit
            /// Unblock tooltips: allow them to be scheduled and shown according to their own
            /// logic.
            abstract unblock: unit -> unit

    module Touch =

        type [<AllowNullLiteral>] IExports =
            abstract TOUCH_ENABLED: obj option
            abstract TOUCH_MAP: Object
            /// Clear the touch identifier that tracks which touch stream to pay attention
            /// to.  This ends the current drag/gesture and allows other pointers to be
            /// captured.
            abstract clearTouchIdentifier: unit -> unit
            /// <summary>Decide whether Blockly should handle or ignore this event.
            /// Mouse and touch events require special checks because we only want to deal
            /// with one touch stream at a time.  All other events should always be handled.</summary>
            /// <param name="e">The event to check.</param>
            abstract shouldHandleEvent: e: Event -> bool
            /// <summary>Get the touch identifier from the given event.  If it was a mouse event, the
            /// identifier is the string 'mouse'.</summary>
            /// <param name="e">Mouse event or touch event.</param>
            abstract getTouchIdentifierFromEvent: e: Event -> string
            /// <summary>Check whether the touch identifier on the event matches the current saved
            /// identifier.  If there is no identifier, that means it's a mouse event and
            /// we'll use the identifier "mouse".  This means we won't deal well with
            /// multiple mice being used at the same time.  That seems okay.
            /// If the current identifier was unset, save the identifier from the
            /// event.  This starts a drag/gesture, during which touch events with other
            /// identifiers will be silently ignored.</summary>
            /// <param name="e">Mouse event or touch event.</param>
            abstract checkTouchIdentifier: e: Event -> bool
            /// <summary>Set an event's clientX and clientY from its first changed touch.  Use this to
            /// make a touch event work in a mouse event handler.</summary>
            /// <param name="e">A touch event.</param>
            abstract setClientFromTouch: e: Event -> unit
            /// <summary>Check whether a given event is a mouse or touch event.</summary>
            /// <param name="e">An event.</param>
            abstract isMouseOrTouchEvent: e: Event -> bool
            /// <summary>Check whether a given event is a touch event or a pointer event.</summary>
            /// <param name="e">An event.</param>
            abstract isTouchEvent: e: Event -> bool
            /// <summary>Split an event into an array of events, one per changed touch or mouse
            /// point.</summary>
            /// <param name="e">A mouse event or a touch event with one or more changed
            /// touches.</param>
            abstract splitEventByTouches: e: Event -> ResizeArray<Event>

    type [<AllowNullLiteral>] TouchGesture =
        inherit TouchGesture__Class

    type [<AllowNullLiteral>] TouchGestureStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> TouchGesture

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] TouchGesture__Class =
        inherit Blockly.Gesture__Class
        /// <summary>Start a gesture: update the workspace to indicate that a gesture is in
        /// progress and bind mousemove and mouseup handlers.</summary>
        /// <param name="e">A mouse down, touch start or pointer down event.</param>
        abstract doStart: e: Event -> unit
        /// <summary>Bind gesture events.
        /// Overriding the gesture definition of this function, binding the same
        /// functions for onMoveWrapper_ and onUpWrapper_ but passing
        /// opt_noCaptureIdentifier.
        /// In addition, binding a second mouse down event to detect multi-touch events.</summary>
        /// <param name="e">A mouse down or touch start event.</param>
        abstract bindMouseEvents: e: Event -> unit
        /// <summary>Handle a mouse down, touch start, or pointer down event.</summary>
        /// <param name="e">A mouse down, touch start, or pointer down event.</param>
        abstract handleStart: e: Event -> unit
        /// <summary>Handle a mouse move, touch move, or pointer move event.</summary>
        /// <param name="e">A mouse move, touch move, or pointer move event.</param>
        abstract handleMove: e: Event -> unit
        /// <summary>Handle a mouse up, touch end, or pointer up event.</summary>
        /// <param name="e">A mouse up, touch end, or pointer up event.</param>
        abstract handleUp: e: Event -> unit
        /// Whether this gesture is part of a multi-touch gesture.
        abstract isMultiTouch: unit -> bool
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// <summary>Handle a touch start or pointer down event and keep track of current
        /// pointers.</summary>
        /// <param name="e">A touch start, or pointer down event.</param>
        abstract handleTouchStart: e: Event -> unit
        /// <summary>Handle a touch move or pointer move event and zoom in/out if two pointers
        /// are on the screen.</summary>
        /// <param name="e">A touch move, or pointer move event.</param>
        abstract handleTouchMove: e: Event -> unit
        /// <summary>Handle a touch end or pointer end event and end the gesture.</summary>
        /// <param name="e">A touch end, or pointer end event.</param>
        abstract handleTouchEnd: e: Event -> unit
        /// <summary>Helper function returning the current touch point coordinate.</summary>
        /// <param name="e">A touch or pointer event.</param>
        abstract getTouchPoint: e: Event -> Blockly.Utils.Coordinate

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] TouchGesture__ClassStatic =
        /// <summary>Class for one gesture.</summary>
        /// <param name="e">The event that kicked off this gesture.</param>
        /// <param name="creatorWorkspace">The workspace that created
        /// this gesture and has a reference to it.</param>
        [<Emit "new $0($1...)">] abstract Create: e: Event * creatorWorkspace: Blockly.WorkspaceSvg -> TouchGesture__Class

    module TouchGesture =

        type [<AllowNullLiteral>] IExports =
            abstract ZOOM_IN_MULTIPLIER: obj option
            abstract ZOOM_OUT_MULTIPLIER: obj option

    type [<AllowNullLiteral>] Trashcan =
        inherit Trashcan__Class

    type [<AllowNullLiteral>] TrashcanStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Trashcan

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Trashcan__Class =
        /// Current open/close state of the lid.
        abstract isOpen: bool with get, set
        /// Create the trash can elements.
        abstract createDom: unit -> SVGElement
        /// <summary>Initialize the trash can.</summary>
        /// <param name="verticalSpacing">Vertical distance from workspace edge to the
        /// same edge of the trashcan.</param>
        abstract init: verticalSpacing: float -> float
        /// Dispose of this trash can.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// Position the trashcan.
        /// It is positioned in the opposite corner to the corner the
        /// categories/toolbox starts at.
        abstract position: unit -> unit
        /// Return the deletion rectangle for this trash can.
        abstract getClientRect: unit -> Blockly.Utils.Rect
        /// Flip the lid shut.
        /// Called externally after a drag.
        abstract close: unit -> unit
        /// Inspect the contents of the trash.
        abstract click: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Trashcan__ClassStatic =
        /// <summary>Class for a trash can.</summary>
        /// <param name="workspace">The workspace to sit in.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace -> Trashcan__Class

    module Utils =
        //TODO: need dotted imports for all of these? See 'utils.xml' below.
        let [<Import("_string","blockly")>] _string: _string.IExports = jsNative
        let [<Import("aria","blockly")>] aria: Aria.IExports = jsNative
        let [<Import("colour","blockly")>] colour: Colour.IExports = jsNative
        let [<Import("Coordinate","blockly")>] coordinate: Coordinate.IExports = jsNative
        let [<Import("dom","blockly")>] dom: Dom.IExports = jsNative
        let [<Import("IdGenerator","blockly")>] idGenerator: IdGenerator.IExports = jsNative
        let [<Import("math","blockly")>] math: Math.IExports = jsNative
        let [<Import("object","blockly")>] ``object``: Object.IExports = jsNative
        let [<Import("Size","blockly")>] size: Size.IExports = jsNative
        let [<Import("style","blockly")>] style: Style.IExports = jsNative
        let [<Import("svgPaths","blockly")>] svgPaths: SvgPaths.IExports = jsNative
        let [<Import("uiMenu","blockly")>] uiMenu: UiMenu.IExports = jsNative
        let [<Import("utils.xml","blockly")>] xml: Xml.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            /// <summary>Don't do anything for this event, just halt propagation.</summary>
            /// <param name="e">An event.</param>
            abstract noEvent: e: Event -> unit
            /// <summary>Is this event targeting a text input widget?</summary>
            /// <param name="e">An event.</param>
            abstract isTargetInput: e: Event -> bool
            /// <summary>Return the coordinates of the top-left corner of this element relative to
            /// its parent.  Only for SVG elements and children (e.g. rect, g, path).</summary>
            /// <param name="element">SVG element to find the coordinates of.</param>
            abstract getRelativeXY: element: Element -> Blockly.Utils.Coordinate
            /// <summary>Return the coordinates of the top-left corner of this element relative to
            /// the div Blockly was injected into.</summary>
            /// <param name="element">SVG element to find the coordinates of. If this is
            /// not a child of the div Blockly was injected into, the behaviour is
            /// undefined.</param>
            abstract getInjectionDivXY_: element: Element -> Blockly.Utils.Coordinate
            /// <summary>Is this event a right-click?</summary>
            /// <param name="e">Mouse event.</param>
            abstract isRightButton: e: Event -> bool
            /// <summary>Return the converted coordinates of the given mouse event.
            /// The origin (0,0) is the top-left corner of the Blockly SVG.</summary>
            /// <param name="e">Mouse event.</param>
            /// <param name="svg">SVG element.</param>
            /// <param name="matrix">Inverted screen CTM to use.</param>
            abstract mouseToSvg: e: Event * svg: Element * matrix: SVGMatrix -> SVGPoint
            /// <summary>Get the scroll delta of a mouse event in pixel units.</summary>
            /// <param name="e">Mouse event.</param>
            abstract getScrollDeltaPixels: e: Event -> GetScrollDeltaPixelsReturn
            /// <summary>Parse a string with any number of interpolation tokens (%1, %2, ...).
            /// It will also replace string table references (e.g., %{bky_my_msg} and
            /// %{BKY_MY_MSG} will both be replaced with the value in
            /// Blockly.Msg['MY_MSG']). Percentage sign characters '%' may be self-escaped
            /// (e.g., '%%').</summary>
            /// <param name="message">Text which might contain string table references and
            /// interpolation tokens.</param>
            abstract tokenizeInterpolation: message: string -> U2<string, ResizeArray<float>>
            /// <summary>Replaces string table references in a message, if the message is a string.
            /// For example, "%{bky_my_msg}" and "%{BKY_MY_MSG}" will both be replaced with
            /// the value in Blockly.Msg['MY_MSG'].</summary>
            /// <param name="message">Message, which may be a string that contains
            /// string table references.</param>
            abstract replaceMessageReferences: message: U2<string, obj option> -> string
            /// <summary>Validates that any %{MSG_KEY} references in the message refer to keys of
            /// the Blockly.Msg string table.</summary>
            /// <param name="message">Text which might contain string table references.</param>
            abstract checkMessageReferences: message: string -> bool
            /// Generate a unique ID.  This should be globally unique.
            /// 87 characters ^ 20 length > 128 bits (better than a UUID).
            abstract genUid: unit -> string
            /// Check if 3D transforms are supported by adding an element
            /// and attempting to set the property.
            abstract is3dSupported: unit -> bool
            /// <summary>Calls a function after the page has loaded, possibly immediately.</summary>
            /// <param name="fn">Function to run.</param>
            abstract runAfterPageLoad: fn: RunAfterPageLoadFn -> unit
            /// Get the position of the current viewport in window coordinates.  This takes
            /// scroll into account.
            abstract getViewportBBox: unit -> Object
            /// <summary>Removes the first occurrence of a particular value from an array.</summary>
            /// <param name="arr">Array from which to remove
            /// value.</param>
            /// <param name="obj">Object to remove.</param>
            abstract arrayRemove: arr: ResizeArray<obj option> * obj: obj option -> bool
            /// Gets the document scroll distance as a coordinate object.
            /// Copied from Closure's goog.dom.getDocumentScroll.
            abstract getDocumentScroll: unit -> Blockly.Utils.Coordinate
            /// <summary>Get a map of all the block's descendants mapping their type to the number of
            ///     children with that type.</summary>
            /// <param name="block">The block to map.</param>
            /// <param name="opt_stripFollowing">Optionally ignore all following
            /// statements (blocks that are not inside a value or statement input
            /// of the block).</param>
            abstract getBlockTypeCounts: block: Blockly.Block * ?opt_stripFollowing: bool -> Object
            /// <summary>Converts screen coordinates to workspace coordinates.</summary>
            /// <param name="ws">The workspace to find the coordinates on.</param>
            /// <param name="screenCoordinates">The screen coordinates to
            /// be converted to workspace coordintaes</param>
            abstract screenToWsCoordinates: ws: Blockly.WorkspaceSvg * screenCoordinates: Blockly.Utils.Coordinate -> Blockly.Utils.Coordinate
            abstract Coordinate: CoordinateStatic
            abstract Coordinate__Class: Coordinate__ClassStatic
            abstract ``global``: obj option
            abstract Rect: RectStatic
            abstract Rect__Class: Rect__ClassStatic
            abstract Size: SizeStatic
            abstract Size__Class: Size__ClassStatic

        type [<AllowNullLiteral>] GetScrollDeltaPixelsReturn =
            abstract x: float with get, set
            abstract y: float with get, set

        type [<AllowNullLiteral>] RunAfterPageLoadFn =
            [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

        module UiMenu =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Get the size of a rendered goog.ui.Menu.</summary>
                /// <param name="menu">The menu to measure.</param>
                abstract getSize: menu: Blockly.Menu -> Blockly.Utils.Size
                /// <summary>Adjust the bounding boxes used to position the widget div to deal with RTL
                /// goog.ui.Menu positioning.  In RTL mode the menu renders down and to the left
                /// of its start point, instead of down and to the right.  Adjusting all of the
                /// bounding boxes accordingly allows us to use the same code for all widgets.
                /// This function in-place modifies the provided bounding boxes.</summary>
                /// <param name="viewportBBox">The bounding rectangle of the current viewport,
                /// in window coordinates.</param>
                /// <param name="anchorBBox">The bounding rectangle of the anchor, in window
                /// coordinates.</param>
                /// <param name="menuSize">The size of the menu that is inside the
                /// widget div, in window coordinates.</param>
                abstract adjustBBoxesForRTL: viewportBBox: Object * anchorBBox: Object * menuSize: Blockly.Utils.Size -> unit

        module Aria =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Sets the role of an element. If the roleName is
                /// empty string or null, the role for the element is removed.
                /// We encourage clients to call the goog.a11y.aria.removeRole
                /// method instead of setting null and empty string values.
                /// Special handling for this case is added to ensure
                /// backword compatibility with existing code.
                /// 
                /// Similar to Closure's goog.a11y.aria</summary>
                /// <param name="element">DOM node to set role of.</param>
                /// <param name="roleName">role name(s).</param>
                abstract setRole: element: Element * roleName: U2<Blockly.Utils.Aria.Role, string> -> unit
                /// <summary>Gets role of an element.
                /// Copied from Closure's goog.a11y.aria</summary>
                /// <param name="element">DOM element to get role of.</param>
                abstract getRole: element: Element -> Blockly.Utils.Aria.Role
                /// <summary>Removes role of an element.
                /// Copied from Closure's goog.a11y.aria</summary>
                /// <param name="element">DOM element to remove the role from.</param>
                abstract removeRole: element: Element -> unit
                /// <summary>Sets the state or property of an element.
                /// Copied from Closure's goog.a11y.aria</summary>
                /// <param name="element">DOM node where we set state.</param>
                /// <param name="stateName">State attribute being set.
                /// Automatically adds prefix 'aria-' to the state name if the attribute is
                /// not an extra attribute.</param>
                /// <param name="value">Value
                /// for the state attribute.</param>
                abstract setState: element: Element * stateName: U2<Blockly.Utils.Aria.State, string> * value: U4<string, bool, float, ResizeArray<string>> -> unit

            type Role =
                obj

            type State =
                obj

        module Colour =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Parses a colour from a string.
                /// .parse('red') -> '#ff0000'
                /// .parse('#f00') -> '#ff0000'
                /// .parse('#ff0000') -> '#ff0000'
                /// .parse('rgb(255, 0, 0)') -> '#ff0000'</summary>
                /// <param name="str">Colour in some CSS format.</param>
                abstract parse: str: string -> U2<string, obj option>
                /// <summary>Converts a colour from RGB to hex representation.</summary>
                /// <param name="r">Amount of red, int between 0 and 255.</param>
                /// <param name="g">Amount of green, int between 0 and 255.</param>
                /// <param name="b">Amount of blue, int between 0 and 255.</param>
                abstract rgbToHex: r: float * g: float * b: float -> string
                /// <summary>Converts a hex representation of a colour to RGB.</summary>
                /// <param name="hexColor">Colour in '#ff0000' format.</param>
                abstract hexToRgb: hexColor: string -> ResizeArray<float>
                /// <summary>Converts an HSV triplet to hex representation.</summary>
                /// <param name="h">Hue value in [0, 360].</param>
                /// <param name="s">Saturation value in [0, 1].</param>
                /// <param name="v">Brightness in [0, 255].</param>
                abstract hsvToHex: h: float * s: float * v: float -> string
                /// <summary>Blend two colours together, using the specified factor to indicate the
                /// weight given to the first colour.</summary>
                /// <param name="colour1">First colour.</param>
                /// <param name="colour2">Second colour.</param>
                /// <param name="factor">The weight to be given to colour1 over colour2.
                /// Values should be in the range [0, 1].</param>
                abstract blend: colour1: string * colour2: string * factor: float -> string
                abstract names: TypeLiteral_01

            type [<AllowNullLiteral>] TypeLiteral_01 =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

        type [<AllowNullLiteral>] Coordinate =
            inherit Coordinate__Class

        type [<AllowNullLiteral>] CoordinateStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Coordinate

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Coordinate__Class =
            /// X-value
            abstract x: float with get, set
            /// Y-value
            abstract y: float with get, set
            /// <summary>Scales this coordinate by the given scale factor.</summary>
            /// <param name="s">The scale factor to use for both x and y dimensions.</param>
            abstract scale: s: float -> Blockly.Utils.Coordinate
            /// <summary>Translates this coordinate by the given offsets.
            /// respectively.</summary>
            /// <param name="tx">The value to translate x by.</param>
            /// <param name="ty">The value to translate y by.</param>
            abstract translate: tx: float * ty: float -> Blockly.Utils.Coordinate

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Coordinate__ClassStatic =
            /// <summary>Class for representing coordinates and positions.</summary>
            /// <param name="x">Left.</param>
            /// <param name="y">Top.</param>
            [<Emit "new $0($1...)">] abstract Create: x: float * y: float -> Coordinate__Class

        module Coordinate =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Compares coordinates for equality.</summary>
                /// <param name="a">A Coordinate.</param>
                /// <param name="b">A Coordinate.</param>
                abstract equals: a: Blockly.Utils.Coordinate * b: Blockly.Utils.Coordinate -> bool
                /// <summary>Returns the distance between two coordinates.</summary>
                /// <param name="a">A Coordinate.</param>
                /// <param name="b">A Coordinate.</param>
                abstract distance: a: Blockly.Utils.Coordinate * b: Blockly.Utils.Coordinate -> float
                /// <summary>Returns the magnitude of a coordinate.</summary>
                /// <param name="a">A Coordinate.</param>
                abstract magnitude: a: Blockly.Utils.Coordinate -> float
                /// <summary>Returns the difference between two coordinates as a new
                /// Blockly.utils.Coordinate.</summary>
                /// <param name="a">An x/y coordinate.</param>
                /// <param name="b">An x/y coordinate.</param>
                abstract difference: a: U2<Blockly.Utils.Coordinate, SVGPoint> * b: U2<Blockly.Utils.Coordinate, SVGPoint> -> Blockly.Utils.Coordinate
                /// <summary>Returns the sum of two coordinates as a new Blockly.utils.Coordinate.</summary>
                /// <param name="a">An x/y coordinate.</param>
                /// <param name="b">An x/y coordinate.</param>
                abstract sum: a: U2<Blockly.Utils.Coordinate, SVGPoint> * b: U2<Blockly.Utils.Coordinate, SVGPoint> -> Blockly.Utils.Coordinate

        module Dom =

            type [<AllowNullLiteral>] IExports =
                abstract SVG_NS: obj option
                abstract HTML_NS: obj option
                abstract XLINK_NS: obj option
                /// <summary>Helper method for creating SVG elements.</summary>
                /// <param name="name">Element's tag name.</param>
                /// <param name="attrs">Dictionary of attribute names and values.</param>
                /// <param name="parent">Optional parent on which to append the element.</param>
                abstract createSvgElement: name: string * attrs: Object * parent: Element -> SVGElement
                /// <summary>Add a CSS class to a element.
                /// Similar to Closure's goog.dom.classes.add, except it handles SVG elements.</summary>
                /// <param name="element">DOM element to add class to.</param>
                /// <param name="className">Name of class to add.</param>
                abstract addClass: element: Element * className: string -> bool
                /// <summary>Remove a CSS class from a element.
                /// Similar to Closure's goog.dom.classes.remove, except it handles SVG elements.</summary>
                /// <param name="element">DOM element to remove class from.</param>
                /// <param name="className">Name of class to remove.</param>
                abstract removeClass: element: Element * className: string -> bool
                /// <summary>Checks if an element has the specified CSS class.
                /// Similar to Closure's goog.dom.classes.has, except it handles SVG elements.</summary>
                /// <param name="element">DOM element to check.</param>
                /// <param name="className">Name of class to check.</param>
                abstract hasClass: element: Element * className: string -> bool
                /// <summary>Removes a node from its parent. No-op if not attached to a parent.</summary>
                /// <param name="node">The node to remove.</param>
                abstract removeNode: node: Node -> Node
                /// <summary>Insert a node after a reference node.
                /// Contrast with node.insertBefore function.</summary>
                /// <param name="newNode">New element to insert.</param>
                /// <param name="refNode">Existing element to precede new node.</param>
                abstract insertAfter: newNode: Element * refNode: Element -> unit
                /// <summary>Whether a node contains another node.</summary>
                /// <param name="parent">The node that should contain the other node.</param>
                /// <param name="descendant">The node to test presence of.</param>
                abstract containsNode: parent: Node * descendant: Node -> bool
                /// <summary>Sets the CSS transform property on an element. This function sets the
                /// non-vendor-prefixed and vendor-prefixed versions for backwards compatibility
                /// with older browsers. See https://caniuse.com/#feat=transforms2d</summary>
                /// <param name="element">Element to which the CSS transform will be applied.</param>
                /// <param name="transform">The value of the CSS `transform` property.</param>
                abstract setCssTransform: element: Element * transform: string -> unit
                /// Start caching text widths. Every call to this function MUST also call
                /// stopTextWidthCache. Caches must not survive between execution threads.
                abstract startTextWidthCache: unit -> unit
                /// Stop caching field widths. Unless caching was already on when the
                /// corresponding call to startTextWidthCache was made.
                abstract stopTextWidthCache: unit -> unit
                /// <summary>Gets the width of a text element, caching it in the process.</summary>
                /// <param name="textElement">An SVG 'text' element.</param>
                abstract getTextWidth: textElement: Element -> float

            type Node =
                obj

        module IdGenerator =

            type [<AllowNullLiteral>] IExports =
                /// Gets the next unique ID.
                /// IDs are compatible with the HTML4 id attribute restrictions:
                /// Use only ASCII letters, digits, '_', '-' and '.'
                abstract getNextUniqueId: unit -> string

        type KeyCodes =
            obj

        module Math =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Converts degrees to radians.
                /// Copied from Closure's goog.math.toRadians.</summary>
                /// <param name="angleDegrees">Angle in degrees.</param>
                abstract toRadians: angleDegrees: float -> float
                /// <summary>Converts radians to degrees.
                /// Copied from Closure's goog.math.toDegrees.</summary>
                /// <param name="angleRadians">Angle in radians.</param>
                abstract toDegrees: angleRadians: float -> float
                /// <summary>Clamp the provided number between the lower bound and the upper bound.</summary>
                /// <param name="lowerBound">The desired lower bound.</param>
                /// <param name="number">The number to clamp.</param>
                /// <param name="upperBound">The desired upper bound.</param>
                abstract clamp: lowerBound: float * number: float * upperBound: float -> float

        module Object =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Inherit the prototype methods from one constructor into another.</summary>
                /// <param name="childCtor">Child class.</param>
                /// <param name="parentCtor">Parent class.</param>
                abstract inherits: childCtor: Function * parentCtor: Function -> unit
                /// <summary>Copies all the members of a source object to a target object.</summary>
                /// <param name="target">Target.</param>
                /// <param name="source">Source.</param>
                abstract ``mixin``: target: Object * source: Object -> unit
                /// <summary>Returns an array of a given object's own enumerable property values.</summary>
                /// <param name="obj">Object containing values.</param>
                abstract values: obj: Object -> ResizeArray<obj option>

        type [<AllowNullLiteral>] Rect =
            inherit Rect__Class

        type [<AllowNullLiteral>] RectStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Rect

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Rect__Class =
            abstract top: float with get, set
            abstract bottom: float with get, set
            abstract left: float with get, set
            abstract right: float with get, set
            /// <summary>Tests whether this rectangle contains a x/y coordinate.</summary>
            /// <param name="x">The x coordinate to test for containment.</param>
            /// <param name="y">The y coordinate to test for containment.</param>
            abstract contains: x: float * y: float -> bool

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Rect__ClassStatic =
            /// <summary>Class for representing rectangular regions.</summary>
            /// <param name="top">Top.</param>
            /// <param name="bottom">Bottom.</param>
            /// <param name="left">Left.</param>
            /// <param name="right">Right.</param>
            [<Emit "new $0($1...)">] abstract Create: top: float * bottom: float * left: float * right: float -> Rect__Class

        type [<AllowNullLiteral>] Size =
            inherit Size__Class

        type [<AllowNullLiteral>] SizeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Size

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Size__Class =
            /// Width
            abstract width: float with get, set
            /// Height
            abstract height: float with get, set

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Size__ClassStatic =
            /// <summary>Class for representing sizes consisting of a width and height.</summary>
            /// <param name="width">Width.</param>
            /// <param name="height">Height.</param>
            [<Emit "new $0($1...)">] abstract Create: width: float * height: float -> Size__Class

        module Size =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Compares sizes for equality.</summary>
                /// <param name="a">A Size.</param>
                /// <param name="b">A Size.</param>
                abstract equals: a: Blockly.Utils.Size * b: Blockly.Utils.Size -> bool

        module _string =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Fast prefix-checker.
                /// Copied from Closure's goog.string.startsWith.</summary>
                /// <param name="str">The string to check.</param>
                /// <param name="prefix">A string to look for at the start of `str`.</param>
                abstract startsWith: str: string * prefix: string -> bool
                /// <summary>Given an array of strings, return the length of the shortest one.</summary>
                /// <param name="array">Array of strings.</param>
                abstract shortestStringLength: array: ResizeArray<string> -> float
                /// <summary>Given an array of strings, return the length of the common prefix.
                /// Words may not be split.  Any space after a word is included in the length.</summary>
                /// <param name="array">Array of strings.</param>
                /// <param name="opt_shortest">Length of shortest string.</param>
                abstract commonWordPrefix: array: ResizeArray<string> * ?opt_shortest: float -> float
                /// <summary>Given an array of strings, return the length of the common suffix.
                /// Words may not be split.  Any space after a word is included in the length.</summary>
                /// <param name="array">Array of strings.</param>
                /// <param name="opt_shortest">Length of shortest string.</param>
                abstract commonWordSuffix: array: ResizeArray<string> * ?opt_shortest: float -> float
                /// <summary>Wrap text to the specified width.</summary>
                /// <param name="text">Text to wrap.</param>
                /// <param name="limit">Width to wrap each line.</param>
                abstract wrap: text: string * limit: float -> string

        module Style =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Gets the height and width of an element.
                /// Similar to Closure's goog.style.getSize</summary>
                /// <param name="element">Element to get size of.</param>
                abstract getSize: element: Element -> Blockly.Utils.Size
                /// <summary>Retrieves a computed style value of a node. It returns empty string if the
                /// value cannot be computed (which will be the case in Internet Explorer) or
                /// "none" if the property requested is an SVG one and it has not been
                /// explicitly set (firefox and webkit).
                /// 
                /// Copied from Closure's goog.style.getComputedStyle</summary>
                /// <param name="element">Element to get style of.</param>
                /// <param name="property">Property to get (camel-case).</param>
                abstract getComputedStyle: element: Element * property: string -> string
                /// <summary>Gets the cascaded style value of a node, or null if the value cannot be
                /// computed (only Internet Explorer can do this).
                /// 
                /// Copied from Closure's goog.style.getCascadedStyle</summary>
                /// <param name="element">Element to get style of.</param>
                /// <param name="style">Property to get (camel-case).</param>
                abstract getCascadedStyle: element: Element * style: string -> string
                /// <summary>Returns a Coordinate object relative to the top-left of the HTML document.
                /// Similar to Closure's goog.style.getPageOffset</summary>
                /// <param name="el">Element to get the page offset for.</param>
                abstract getPageOffset: el: Element -> Blockly.Utils.Coordinate
                /// Calculates the viewport coordinates relative to the document.
                /// Similar to Closure's goog.style.getViewportPageOffset
                abstract getViewportPageOffset: unit -> Blockly.Utils.Coordinate
                /// <summary>Shows or hides an element from the page. Hiding the element is done by
                /// setting the display property to "none", removing the element from the
                /// rendering hierarchy so it takes up no space. To show the element, the default
                /// inherited display property is restored (defined either in stylesheets or by
                /// the browser's default style rules).
                /// Copied from Closure's goog.style.getViewportPageOffset</summary>
                /// <param name="el">Element to show or hide.</param>
                /// <param name="isShown">True to render the element in its default style,
                /// false to disable rendering the element.</param>
                abstract setElementShown: el: Element * isShown: obj option -> unit
                /// <summary>Returns true if the element is using right to left (RTL) direction.
                /// Copied from Closure's goog.style.isRightToLeft</summary>
                /// <param name="el">The element to test.</param>
                abstract isRightToLeft: el: Element -> bool
                /// <summary>Gets the computed border widths (on all sides) in pixels
                /// Copied from Closure's goog.style.getBorderBox</summary>
                /// <param name="element">The element to get the border widths for.</param>
                abstract getBorderBox: element: Element -> Object
                /// <summary>Changes the scroll position of `container` with the minimum amount so
                /// that the content and the borders of the given `element` become visible.
                /// If the element is bigger than the container, its top left corner will be
                /// aligned as close to the container's top left corner as possible.
                /// Copied from Closure's goog.style.scrollIntoContainerView</summary>
                /// <param name="element">The element to make visible.</param>
                /// <param name="container">The container to scroll. If not set, then the
                /// document scroll element will be used.</param>
                /// <param name="opt_center">Whether to center the element in the container.
                /// Defaults to false.</param>
                abstract scrollIntoContainerView: element: Element * container: Element * ?opt_center: bool -> unit
                /// <summary>Calculate the scroll position of `container` with the minimum amount so
                /// that the content and the borders of the given `element` become visible.
                /// If the element is bigger than the container, its top left corner will be
                /// aligned as close to the container's top left corner as possible.
                /// Copied from Closure's goog.style.getContainerOffsetToScrollInto</summary>
                /// <param name="element">The element to make visible.</param>
                /// <param name="container">The container to scroll. If not set, then the
                /// document scroll element will be used.</param>
                /// <param name="opt_center">Whether to center the element in the container.
                /// Defaults to false.</param>
                abstract getContainerOffsetToScrollInto: element: Element * container: Element * ?opt_center: bool -> Blockly.Utils.Coordinate

        module SvgPaths =

            type [<AllowNullLiteral>] IExports =
                /// <summary>Create a string representing the given x, y pair.  It does not matter whether
                /// the coordinate is relative or absolute.  The result has leading
                /// and trailing spaces, and separates the x and y coordinates with a comma but
                /// no space.</summary>
                /// <param name="x">The x coordinate.</param>
                /// <param name="y">The y coordinate.</param>
                abstract point: x: float * y: float -> string
                /// <summary>Draw a curbic or quadratic curve.  See
                /// developer.mozilla.org/en-US/docs/Web/SVG/Attribute/d#Cubic_B%C3%A9zier_Curve
                /// These coordinates are unitless and hence in the user coordinate system.</summary>
                /// <param name="command">The command to use.
                /// Should be one of: c, C, s, S, q, Q.</param>
                /// <param name="points">An array containing all of the points to pass to the
                /// curve command, in order.  The points are represented as strings of the
                /// format ' x, y '.</param>
                abstract curve: command: string * points: ResizeArray<string> -> string
                /// <summary>Move the cursor to the given position without drawing a line.
                /// The coordinates are absolute.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths#Line_commands</summary>
                /// <param name="x">The absolute x coordinate.</param>
                /// <param name="y">The absolute y coordinate.</param>
                abstract moveTo: x: float * y: float -> string
                /// <summary>Move the cursor to the given position without drawing a line.
                /// Coordinates are relative.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths#Line_commands</summary>
                /// <param name="dx">The relative x coordinate.</param>
                /// <param name="dy">The relative y coordinate.</param>
                abstract moveBy: dx: float * dy: float -> string
                /// <summary>Draw a line from the current point to the end point, which is the current
                /// point shifted by dx along the x-axis and dy along the y-axis.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths#Line_commands</summary>
                /// <param name="dx">The relative x coordinate.</param>
                /// <param name="dy">The relative y coordinate.</param>
                abstract lineTo: dx: float * dy: float -> string
                /// <summary>Draw multiple lines connecting all of the given points in order.  This is
                /// equivalent to a series of 'l' commands.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths#Line_commands</summary>
                /// <param name="points">An array containing all of the points to
                /// draw lines to, in order.  The points are represented as strings of the
                /// format ' dx,dy '.</param>
                abstract line: points: ResizeArray<string> -> string
                /// <summary>Draw a horizontal or vertical line.
                /// The first argument specifies the direction and whether the given position is
                /// relative or absolute.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Attribute/d#LineTo_path_commands</summary>
                /// <param name="command">The command to prepend to the coordinate.  This
                /// should be one of: V, v, H, h.</param>
                /// <param name="val">The coordinate to pass to the command.  It may be
                /// absolute or relative.</param>
                abstract lineOnAxis: command: string * ``val``: float -> string
                /// <summary>Draw an elliptical arc curve.
                /// These coordinates are unitless and hence in the user coordinate system.
                /// See developer.mozilla.org/en-US/docs/Web/SVG/Attribute/d#Elliptical_Arc_Curve</summary>
                /// <param name="command">The command string.  Either 'a' or 'A'.</param>
                /// <param name="flags">The flag string.  See the MDN documentation for a
                /// description and examples.</param>
                /// <param name="radius">The radius of the arc to draw.</param>
                /// <param name="point">The point to move the cursor to after drawing the arc,
                /// specified either in absolute or relative coordinates depending on the
                /// command.</param>
                abstract arc: command: string * flags: string * radius: float * point: string -> string

        module Xml =

            type [<AllowNullLiteral>] IExports =
                abstract NAME_SPACE: obj option
                /// Get the document object.  This method is overridden in the Node.js build of
                /// Blockly. See gulpfile.js, package-blockly-node task.
                abstract document: unit -> Document
                /// <summary>Create DOM element for XML.</summary>
                /// <param name="tagName">Name of DOM element.</param>
                abstract createElement: tagName: string -> Element
                /// <summary>Create text element for XML.</summary>
                /// <param name="text">Text content.</param>
                abstract createTextNode: text: string -> Node
                /// <summary>Converts an XML string into a DOM tree.</summary>
                /// <param name="text">XML string.</param>
                abstract textToDomDocument: text: string -> Document
                /// <summary>Converts a DOM structure into plain text.
                /// Currently the text format is fairly ugly: all one line with no whitespace.</summary>
                /// <param name="dom">A tree of XML elements.</param>
                abstract domToText: dom: Element -> string

    type [<AllowNullLiteral>] VariableMap =
        inherit VariableMap__Class

    type [<AllowNullLiteral>] VariableMapStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> VariableMap

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VariableMap__Class =
        /// The workspace this map belongs to.
        abstract workspace: Blockly.Workspace with get, set
        /// Clear the variable map.
        abstract clear: unit -> unit
        /// <summary>Rename the given variable by updating its name in the variable map.</summary>
        /// <param name="variable">Variable to rename.</param>
        /// <param name="newName">New variable name.</param>
        abstract renameVariable: variable: Blockly.VariableModel * newName: string -> unit
        /// <summary>Rename a variable by updating its name in the variable map. Identify the
        /// variable to rename with the given ID.</summary>
        /// <param name="id">ID of the variable to rename.</param>
        /// <param name="newName">New variable name.</param>
        abstract renameVariableById: id: string * newName: string -> unit
        /// <summary>Create a variable with a given name, optional type, and optional ID.</summary>
        /// <param name="name">The name of the variable. This must be unique across
        /// variables and procedures.</param>
        /// <param name="opt_type">The type of the variable like 'int' or 'string'.
        /// Does not need to be unique. Field_variable can filter variables based on
        /// their type. This will default to '' which is a specific type.</param>
        /// <param name="opt_id">The unique ID of the variable. This will default to
        /// a UUID.</param>
        abstract createVariable: name: string * ?opt_type: string * ?opt_id: string -> Blockly.VariableModel
        /// <summary>Delete a variable.</summary>
        /// <param name="variable">Variable to delete.</param>
        abstract deleteVariable: variable: Blockly.VariableModel -> unit
        /// <summary>Delete a variables by the passed in ID and all of its uses from this
        /// workspace. May prompt the user for confirmation.</summary>
        /// <param name="id">ID of variable to delete.</param>
        abstract deleteVariableById: id: string -> unit
        /// <summary>Find the variable by the given name and type and return it.  Return null if
        ///      it is not found.</summary>
        /// <param name="name">The name to check for.</param>
        /// <param name="opt_type">The type of the variable.  If not provided it
        /// defaults to the empty string, which is a specific type.</param>
        abstract getVariable: name: string * ?opt_type: string -> Blockly.VariableModel
        /// <summary>Find the variable by the given ID and return it. Return null if it is not
        ///      found.</summary>
        /// <param name="id">The ID to check for.</param>
        abstract getVariableById: id: string -> Blockly.VariableModel
        /// <summary>Get a list containing all of the variables of a specified type. If type is
        ///      null, return list of variables with empty string type.</summary>
        /// <param name="type">Type of the variables to find.</param>
        abstract getVariablesOfType: ``type``: string -> ResizeArray<Blockly.VariableModel>
        /// <summary>Return all variable and potential variable types.  This list always contains
        /// the empty string.</summary>
        /// <param name="ws">The workspace used to look for potential
        /// variables. This can be different than the workspace stored on this object
        /// if the passed in ws is a flyout workspace.</param>
        abstract getVariableTypes: ws: Blockly.Workspace -> ResizeArray<string>
        /// Return all variables of all types.
        abstract getAllVariables: unit -> ResizeArray<Blockly.VariableModel>
        /// <summary>Find all the uses of a named variable.</summary>
        /// <param name="id">ID of the variable to find.</param>
        abstract getVariableUsesById: id: string -> ResizeArray<Blockly.Block>

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VariableMap__ClassStatic =
        /// <summary>Class for a variable map.  This contains a dictionary data structure with
        /// variable types as keys and lists of variables as values.  The list of
        /// variables are the type indicated by the key.</summary>
        /// <param name="workspace">The workspace this map belongs to.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace -> VariableMap__Class

    type [<AllowNullLiteral>] VariableModel =
        inherit VariableModel__Class

    type [<AllowNullLiteral>] VariableModelStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> VariableModel

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VariableModel__Class =
        /// The workspace the variable is in.
        abstract workspace: Blockly.Workspace with get, set
        /// The name of the variable, typically defined by the user. It must be
        /// unique across all names used for procedures and variables. It may be
        /// changed by the user.
        abstract name: string with get, set
        /// The type of the variable, such as 'int' or 'sound_effect'. This may be
        /// used to build a list of variables of a specific type. By default this is
        /// the empty string '', which is a specific type.
        abstract ``type``: string with get, set
        abstract getId: unit -> string

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] VariableModel__ClassStatic =
        /// <summary>Class for a variable model.
        /// Holds information for the variable including name, ID, and type.</summary>
        /// <param name="workspace">The variable's workspace.</param>
        /// <param name="name">The name of the variable. This must be unique across
        /// variables and procedures.</param>
        /// <param name="opt_type">The type of the variable like 'int' or 'string'.
        /// Does not need to be unique. Field_variable can filter variables based on
        /// their type. This will default to '' which is a specific type.</param>
        /// <param name="opt_id">The unique ID of the variable. This will default to
        /// a UUID.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace * name: string * ?opt_type: string * ?opt_id: string -> VariableModel__Class

    module VariableModel =

        type [<AllowNullLiteral>] IExports =
            /// <summary>A custom compare function for the VariableModel objects.</summary>
            /// <param name="var1">First variable to compare.</param>
            /// <param name="var2">Second variable to compare.</param>
            abstract compareByName: var1: Blockly.VariableModel * var2: Blockly.VariableModel -> float

    module Variables =

        type [<AllowNullLiteral>] IExports =
            abstract NAME_TYPE: obj option
            /// <summary>Find all user-created variables that are in use in the workspace.
            /// For use by generators.
            /// To get a list of all variables on a workspace, including unused variables,
            /// call Workspace.getAllVariables.</summary>
            /// <param name="ws">The workspace to search for variables.</param>
            abstract allUsedVarModels: ws: Blockly.Workspace -> ResizeArray<Blockly.VariableModel>
            /// Find all user-created variables that are in use in the workspace and return
            /// only their names.
            /// For use by generators.
            /// To get a list of all variables on a workspace, including unused variables,
            /// call Workspace.getAllVariables.
            abstract allUsedVariables: unit -> unit
            /// <summary>Find all developer variables used by blocks in the workspace.
            /// Developer variables are never shown to the user, but are declared as global
            /// variables in the generated code.
            /// To declare developer variables, define the getDeveloperVariables function on
            /// your block and return a list of variable names.
            /// For use by generators.</summary>
            /// <param name="workspace">The workspace to search.</param>
            abstract allDeveloperVariables: workspace: Blockly.Workspace -> ResizeArray<string>
            /// <summary>Construct the elements (blocks and button) required by the flyout for the
            /// variable category.</summary>
            /// <param name="workspace">The workspace containing variables.</param>
            abstract flyoutCategory: workspace: Blockly.Workspace -> ResizeArray<Element>
            /// <summary>Construct the blocks required by the flyout for the variable category.</summary>
            /// <param name="workspace">The workspace containing variables.</param>
            abstract flyoutCategoryBlocks: workspace: Blockly.Workspace -> ResizeArray<Element>
            /// <summary>Return a new variable name that is not yet being used. This will try to
            /// generate single letter variable names in the range 'i' to 'z' to start with.
            /// If no unique name is located it will try 'i' to 'z', 'a' to 'h',
            /// then 'i2' to 'z2' etc.  Skip 'l'.</summary>
            /// <param name="workspace">The workspace to be unique in.</param>
            abstract generateUniqueName: workspace: Blockly.Workspace -> string
            /// <summary>Handles "Create Variable" button in the default variables toolbox category.
            /// It will prompt the user for a variable name, including re-prompts if a name
            /// is already in use among the workspace's variables.
            /// 
            /// Custom button handlers can delegate to this function, allowing variables
            /// types and after-creation processing. More complex customization (e.g.,
            /// prompting for variable type) is beyond the scope of this function.</summary>
            /// <param name="workspace">The workspace on which to create the
            /// variable.</param>
            /// <param name="opt_callback">A callback. It will be passed an
            /// acceptable new variable name, or null if change is to be aborted (cancel
            /// button), or undefined if an existing variable was chosen.</param>
            /// <param name="opt_type">The type of the variable like 'int', 'string', or
            /// ''. This will default to '', which is a specific type.</param>
            abstract createVariableButtonHandler: workspace: Blockly.Workspace * ?opt_callback: CreateVariableButtonHandlerOpt_callback * ?opt_type: string -> unit
            /// <summary>Original name of Blockly.Variables.createVariableButtonHandler(..).</summary>
            /// <param name="workspace">The workspace on which to create the
            /// variable.</param>
            /// <param name="opt_callback">A callback. It will be passed an
            /// acceptable new variable name, or null if change is to be aborted (cancel
            /// button), or undefined if an existing variable was chosen.</param>
            /// <param name="opt_type">The type of the variable like 'int', 'string', or
            /// ''. This will default to '', which is a specific type.</param>
            abstract createVariable: workspace: Blockly.Workspace * ?opt_callback: CreateVariableOpt_callback * ?opt_type: string -> unit
            /// <summary>Rename a variable with the given workspace, variableType, and oldName.</summary>
            /// <param name="workspace">The workspace on which to rename the
            /// variable.</param>
            /// <param name="variable">Variable to rename.</param>
            /// <param name="opt_callback">A callback. It will
            /// be passed an acceptable new variable name, or null if change is to be
            /// aborted (cancel button), or undefined if an existing variable was chosen.</param>
            abstract renameVariable: workspace: Blockly.Workspace * variable: Blockly.VariableModel * ?opt_callback: RenameVariableOpt_callback -> unit
            /// <summary>Prompt the user for a new variable name.</summary>
            /// <param name="promptText">The string of the prompt.</param>
            /// <param name="defaultText">The default value to show in the prompt's field.</param>
            /// <param name="callback">A callback. It will return the new
            /// variable name, or null if the user picked something illegal.</param>
            abstract promptName: promptText: string * defaultText: string * callback: PromptNameCallback -> unit
            /// <summary>Generate DOM objects representing a variable field.</summary>
            /// <param name="variableModel">The variable model to
            /// represent.</param>
            abstract generateVariableFieldDom: variableModel: Blockly.VariableModel -> Element
            /// <summary>Helper function to look up or create a variable on the given workspace.
            /// If no variable exists, creates and returns it.</summary>
            /// <param name="workspace">The workspace to search for the
            /// variable.  It may be a flyout workspace or main workspace.</param>
            /// <param name="id">The ID to use to look up or create the variable, or null.</param>
            /// <param name="opt_name">The string to use to look up or create the
            /// variable.</param>
            /// <param name="opt_type">The type to use to look up or create the variable.</param>
            abstract getOrCreateVariablePackage: workspace: Blockly.Workspace * id: string * ?opt_name: string * ?opt_type: string -> Blockly.VariableModel
            /// <summary>Look up  a variable on the given workspace.
            /// Always looks in the main workspace before looking in the flyout workspace.
            /// Always prefers lookup by ID to lookup by name + type.</summary>
            /// <param name="workspace">The workspace to search for the
            /// variable.  It may be a flyout workspace or main workspace.</param>
            /// <param name="id">The ID to use to look up the variable, or null.</param>
            /// <param name="opt_name">The string to use to look up the variable.
            /// Only used if lookup by ID fails.</param>
            /// <param name="opt_type">The type to use to look up the variable.
            /// Only used if lookup by ID fails.</param>
            abstract getVariable: workspace: Blockly.Workspace * id: string * ?opt_name: string * ?opt_type: string -> Blockly.VariableModel
            /// <summary>Helper function to get the list of variables that have been added to the
            /// workspace after adding a new block, using the given list of variables that
            /// were in the workspace before the new block was added.</summary>
            /// <param name="workspace">The workspace to inspect.</param>
            /// <param name="originalVariables">The array of
            /// variables that existed in the workspace before adding the new block.</param>
            abstract getAddedVariables: workspace: Blockly.Workspace * originalVariables: ResizeArray<Blockly.VariableModel> -> ResizeArray<Blockly.VariableModel>

        type [<AllowNullLiteral>] CreateVariableButtonHandlerOpt_callback =
            [<Emit "$0($1...)">] abstract Invoke: _0: string -> obj option

        type [<AllowNullLiteral>] CreateVariableOpt_callback =
            [<Emit "$0($1...)">] abstract Invoke: _0: string -> obj option

        type [<AllowNullLiteral>] RenameVariableOpt_callback =
            [<Emit "$0($1...)">] abstract Invoke: _0: string -> obj option

        type [<AllowNullLiteral>] PromptNameCallback =
            [<Emit "$0($1...)">] abstract Invoke: _0: string -> obj option

    module VariablesDynamic =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Construct the elements (blocks and button) required by the flyout for the
            /// variable category.</summary>
            /// <param name="workspace">The workspace containing variables.</param>
            abstract flyoutCategory: workspace: Blockly.Workspace -> ResizeArray<Element>
            /// <summary>Construct the blocks required by the flyout for the variable category.</summary>
            /// <param name="workspace">The workspace containing variables.</param>
            abstract flyoutCategoryBlocks: workspace: Blockly.Workspace -> ResizeArray<Element>

    type [<AllowNullLiteral>] Warning =
        inherit Warning__Class

    type [<AllowNullLiteral>] WarningStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Warning

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Warning__Class =
        inherit Blockly.Icon__Class
        /// Does this icon get hidden when the block is collapsed.
        abstract collapseHidden: obj option with get, set
        /// <summary>Show or hide the warning bubble.</summary>
        /// <param name="visible">True if the bubble should be visible.</param>
        abstract setVisible: visible: bool -> unit
        /// Show the bubble.
        abstract createBubble: unit -> unit
        /// Dispose of the bubble and references to it.
        abstract disposeBubble: unit -> unit
        /// <summary>Set this warning's text.</summary>
        /// <param name="text">Warning text (or '' to delete). This supports
        /// linebreaks.</param>
        /// <param name="id">An ID for this text entry to be able to maintain
        /// multiple warnings.</param>
        abstract setText: text: string * id: string -> unit
        /// Get this warning's texts.
        abstract getText: unit -> string
        /// Dispose of this warning.
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Warning__ClassStatic =
        /// <summary>Class for a warning.</summary>
        /// <param name="block">The block associated with this warning.</param>
        [<Emit "new $0($1...)">] abstract Create: block: Blockly.Block -> Warning__Class

    module WidgetDiv =

        type [<AllowNullLiteral>] IExports =
            abstract DIV: Element
            /// Create the widget div and inject it onto the page.
            abstract createDom: unit -> unit
            /// <summary>Initialize and display the widget div.  Close the old one if needed.</summary>
            /// <param name="newOwner">The object that will be using this container.</param>
            /// <param name="rtl">Right-to-left (true) or left-to-right (false).</param>
            /// <param name="dispose">Optional cleanup function to be run when the
            /// widget is closed.</param>
            abstract show: newOwner: Object * rtl: bool * dispose: Function -> unit
            /// Destroy the widget and hide the div.
            abstract hide: unit -> unit
            /// Is the container visible?
            abstract isVisible: unit -> bool
            /// <summary>Destroy the widget and hide the div if it is being used by the specified
            /// object.</summary>
            /// <param name="oldOwner">The object that was using this container.</param>
            abstract hideIfOwner: oldOwner: Object -> unit
            /// <summary>Position the widget div based on an anchor rectangle.
            /// The widget should be placed adjacent to but not overlapping the anchor
            /// rectangle.  The preferred position is directly below and aligned to the left
            /// (LTR) or right (RTL) side of the anchor.</summary>
            /// <param name="viewportBBox">The bounding rectangle of the current viewport,
            /// in window coordinates.</param>
            /// <param name="anchorBBox">The bounding rectangle of the anchor, in window
            /// coordinates.</param>
            /// <param name="widgetSize">The size of the widget that is inside the
            /// widget div, in window coordinates.</param>
            /// <param name="rtl">Whether the workspace is in RTL mode.  This determines
            /// horizontal alignment.</param>
            abstract positionWithAnchor: viewportBBox: Object * anchorBBox: Object * widgetSize: Blockly.Utils.Size * rtl: bool -> unit

    type [<AllowNullLiteral>] Workspace =
        inherit Workspace__Class

    type [<AllowNullLiteral>] WorkspaceStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Workspace

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Workspace__Class =
        abstract id: string with get, set
        abstract options: Blockly.Options with get, set
        abstract RTL: bool with get, set
        abstract horizontalLayout: bool with get, set
        abstract toolboxPosition: float with get, set
        abstract undoStack_: ResizeArray<Blockly.Events.Abstract> with get, set
        abstract redoStack_: ResizeArray<Blockly.Events.Abstract> with get, set
        /// The cursor used to navigate around the AST for keyboard navigation.
        abstract cursor_: Blockly.Cursor with get, set
        /// The marker used to mark a location for keyboard navigation.
        abstract marker_: Blockly.MarkerCursor with get, set
        /// Object in charge of storing and updating the workspace theme.
        abstract themeManager_: Blockly.ThemeManager with get, set
        /// Returns `true` if the workspace is visible and `false` if it's headless.
        abstract rendered: bool with get, set
        /// Returns `true` if the workspace is currently in the process of a bulk clear.
        abstract isClearing: bool with get, set
        /// Maximum number of undo events in stack. `0` turns off undo, `Infinity` sets
        /// it to unlimited.
        abstract MAX_UNDO: float with get, set
        /// Set of databases for rapid lookup of connection locations.
        abstract connectionDBList: ResizeArray<Blockly.ConnectionDB> with get, set
        /// <summary>Sets the cursor for keyboard navigation.</summary>
        /// <param name="cursor">The cursor used to navigate around the Blockly
        /// AST for keyboard navigation.</param>
        abstract setCursor: cursor: Blockly.Cursor -> unit
        /// <summary>Sets the marker for keyboard navigation.</summary>
        /// <param name="marker">The marker used to mark a location for
        /// keyboard navigation.</param>
        abstract setMarker: marker: Blockly.MarkerCursor -> unit
        /// Get the cursor used to navigate around the AST for keyboard navigation.
        abstract getCursor: unit -> Blockly.Cursor
        /// Get the marker used to mark a location for keyboard navigation.
        abstract getMarker: unit -> Blockly.MarkerCursor
        /// Get the workspace theme object.
        abstract getTheme: unit -> Blockly.Theme
        /// <summary>Set the workspace theme object.
        /// If no theme is passed, default to the `Blockly.Themes.Classic` theme.</summary>
        /// <param name="theme">The workspace theme object.</param>
        abstract setTheme: theme: Blockly.Theme -> unit
        /// Refresh all blocks on the workspace after a theme update.
        abstract refreshTheme: unit -> unit
        /// Dispose of this workspace.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// <summary>Add a block to the list of top blocks.</summary>
        /// <param name="block">Block to add.</param>
        abstract addTopBlock: block: Blockly.Block -> unit
        /// <summary>Remove a block from the list of top blocks.</summary>
        /// <param name="block">Block to remove.</param>
        abstract removeTopBlock: block: Blockly.Block -> unit
        /// <summary>Finds the top-level blocks and returns them.  Blocks are optionally sorted
        /// by position; top to bottom (with slight LTR or RTL bias).</summary>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getTopBlocks: ordered: bool -> ResizeArray<Blockly.Block>
        /// <summary>Add a block to the list of blocks keyed by type.</summary>
        /// <param name="block">Block to add.</param>
        abstract addTypedBlock: block: Blockly.Block -> unit
        /// <summary>Remove a block from the list of blocks keyed by type.</summary>
        /// <param name="block">Block to remove.</param>
        abstract removeTypedBlock: block: Blockly.Block -> unit
        /// <summary>Finds the blocks with the associated type and returns them. Blocks are
        /// optionally sorted by position; top to bottom (with slight LTR or RTL bias).</summary>
        /// <param name="type">The type of block to search for.</param>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getBlocksByType: ``type``: string * ordered: bool -> ResizeArray<Blockly.Block>
        /// <summary>Add a comment to the list of top comments.</summary>
        /// <param name="comment">comment to add.</param>
        abstract addTopComment: comment: Blockly.WorkspaceComment -> unit
        /// <summary>Remove a comment from the list of top comments.</summary>
        /// <param name="comment">comment to remove.</param>
        abstract removeTopComment: comment: Blockly.WorkspaceComment -> unit
        /// <summary>Finds the top-level comments and returns them.  Comments are optionally
        /// sorted by position; top to bottom (with slight LTR or RTL bias).</summary>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getTopComments: ordered: bool -> ResizeArray<Blockly.WorkspaceComment>
        /// <summary>Find all blocks in workspace.  Blocks are optionally sorted
        /// by position; top to bottom (with slight LTR or RTL bias).</summary>
        /// <param name="ordered">Sort the list if true.</param>
        abstract getAllBlocks: ordered: bool -> ResizeArray<Blockly.Block>
        /// Dispose of all blocks and comments in workspace.
        abstract clear: unit -> unit
        /// <summary>Rename a variable by updating its name in the variable map. Identify the
        /// variable to rename with the given ID.</summary>
        /// <param name="id">ID of the variable to rename.</param>
        /// <param name="newName">New variable name.</param>
        abstract renameVariableById: id: string * newName: string -> unit
        /// <summary>Create a variable with a given name, optional type, and optional ID.</summary>
        /// <param name="name">The name of the variable. This must be unique across
        /// variables and procedures.</param>
        /// <param name="opt_type">The type of the variable like 'int' or 'string'.
        /// Does not need to be unique. Field_variable can filter variables based on
        /// their type. This will default to '' which is a specific type.</param>
        /// <param name="opt_id">The unique ID of the variable. This will default to
        /// a UUID.</param>
        abstract createVariable: name: string * ?opt_type: string * ?opt_id: string -> Blockly.VariableModel
        /// <summary>Find all the uses of the given variable, which is identified by ID.</summary>
        /// <param name="id">ID of the variable to find.</param>
        abstract getVariableUsesById: id: string -> ResizeArray<Blockly.Block>
        /// <summary>Delete a variables by the passed in ID and all of its uses from this
        /// workspace. May prompt the user for confirmation.</summary>
        /// <param name="id">ID of variable to delete.</param>
        abstract deleteVariableById: id: string -> unit
        /// <summary>Check whether a variable exists with the given name.  The check is
        /// case-insensitive.</summary>
        /// <param name="_name">The name to check for.</param>
        abstract variableIndexOf: _name: string -> float
        /// <summary>Find the variable by the given name and return it. Return null if it is not
        ///      found.</summary>
        /// <param name="name">The name to check for.</param>
        /// <param name="opt_type">The type of the variable.  If not provided it
        /// defaults to the empty string, which is a specific type.</param>
        abstract getVariable: name: string * ?opt_type: string -> Blockly.VariableModel
        /// <summary>Find the variable by the given ID and return it. Return null if it is not
        ///      found.</summary>
        /// <param name="id">The ID to check for.</param>
        abstract getVariableById: id: string -> Blockly.VariableModel
        /// <summary>Find the variable with the specified type. If type is null, return list of
        ///      variables with empty string type.</summary>
        /// <param name="type">Type of the variables to find.</param>
        abstract getVariablesOfType: ``type``: string -> ResizeArray<Blockly.VariableModel>
        /// Return all variable types.
        abstract getVariableTypes: unit -> ResizeArray<string>
        /// Return all variables of all types.
        abstract getAllVariables: unit -> ResizeArray<Blockly.VariableModel>
        /// Returns the horizontal offset of the workspace.
        /// Intended for LTR/RTL compatibility in XML.
        /// Not relevant for a headless workspace.
        abstract getWidth: unit -> float
        /// <summary>Obtain a newly created block.</summary>
        /// <param name="prototypeName">Name of the language object containing
        /// type-specific functions for this block.</param>
        /// <param name="opt_id">Optional ID.  Use this ID if provided, otherwise
        /// create a new ID.</param>
        abstract newBlock: prototypeName: string * ?opt_id: string -> Blockly.Block
        /// The number of blocks that may be added to the workspace before reaching
        ///      the maxBlocks.
        abstract remainingCapacity: unit -> float
        /// <summary>The number of blocks of the given type that may be added to the workspace
        ///     before reaching the maxInstances allowed for that type.</summary>
        /// <param name="type">Type of block to return capacity for.</param>
        abstract remainingCapacityOfType: ``type``: string -> float
        /// <summary>Check if there is remaining capacity for blocks of the given counts to be
        ///     created. If the total number of blocks represented by the map is more than
        ///     the total remaining capacity, it returns false. If a type count is more
        ///     than the remaining capacity for that type, it returns false.</summary>
        /// <param name="typeCountsMap">A map of types to counts (usually representing
        /// blocks to be created).</param>
        abstract isCapacityAvailable: typeCountsMap: Object -> bool
        /// Checks if the workspace has any limits on the maximum number of blocks,
        ///     or the maximum number of blocks of specific types.
        abstract hasBlockLimits: unit -> bool
        /// <summary>Undo or redo the previous action.</summary>
        /// <param name="redo">False if undo, true if redo.</param>
        abstract undo: redo: bool -> unit
        /// Clear the undo/redo stacks.
        abstract clearUndo: unit -> unit
        /// <summary>When something in this workspace changes, call a function.
        /// Note that there may be a few recent events already on the stack.  Thus the
        /// new change listener might be called with events that occurred a few
        /// milliseconds before the change listener was added.</summary>
        /// <param name="func">Function to call.</param>
        abstract addChangeListener: func: Func<Blockly.Events.Abstract__Class,unit> -> unit //Function -> Function
        /// <summary>Stop listening for this workspace's changes.</summary>
        /// <param name="func">Function to stop calling.</param>
        abstract removeChangeListener: func: Func<Blockly.Events.Abstract__Class,unit> -> unit //Function -> unit
        /// <summary>Fire a change event.</summary>
        /// <param name="event">Event to fire.</param>
        abstract fireChangeListener: ``event``: Blockly.Events.Abstract -> unit
        /// <summary>Find the block on this workspace with the specified ID.</summary>
        /// <param name="id">ID of block to find.</param>
        abstract getBlockById: id: string -> Blockly.Block
        /// <summary>Find the comment on this workspace with the specified ID.</summary>
        /// <param name="id">ID of comment to find.</param>
        abstract getCommentById: id: string -> Blockly.WorkspaceComment
        /// <summary>Checks whether all value and statement inputs in the workspace are filled
        /// with blocks.</summary>
        /// <param name="opt_shadowBlocksAreFilled">An optional argument controlling
        /// whether shadow blocks are counted as filled. Defaults to true.</param>
        abstract allInputsFilled: ?opt_shadowBlocksAreFilled: bool -> bool
        /// Return the variable map that contains "potential" variables.
        /// These exist in the flyout but not in the workspace.
        abstract getPotentialVariableMap: unit -> Blockly.VariableMap
        /// Create and store the potential variable map for this workspace.
        abstract createPotentialVariableMap: unit -> unit
        /// Return the map of all variables on the workspace.
        abstract getVariableMap: unit -> Blockly.VariableMap
        /// Get the theme manager for this workspace.
        abstract getThemeManager: unit -> Blockly.ThemeManager

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Workspace__ClassStatic =
        /// <summary>Class for a workspace.  This is a data structure that contains blocks.
        /// There is no UI, and can be created headlessly.</summary>
        /// <param name="opt_options">Dictionary of options.</param>
        [<Emit "new $0($1...)">] abstract Create: ?opt_options: Blockly.Options -> Workspace__Class

    module Workspace =

        type [<AllowNullLiteral>] IExports =
            abstract SCAN_ANGLE: obj option
            /// <summary>Find the workspace with the specified ID.</summary>
            /// <param name="id">ID of workspace to find.</param>
            abstract getById: id: string -> Blockly.Workspace
            /// Find all workspaces.
            abstract getAll: unit -> ResizeArray<Blockly.Workspace>

    type [<AllowNullLiteral>] WorkspaceAudio =
        inherit WorkspaceAudio__Class

    type [<AllowNullLiteral>] WorkspaceAudioStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceAudio

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceAudio__Class =
        /// Dispose of this audio manager.
        abstract dispose: unit -> unit
        /// <summary>Load an audio file.  Cache it, ready for instantaneous playing.</summary>
        /// <param name="filenames">List of file types in decreasing order of
        /// preference (i.e. increasing size).  E.g. ['media/go.mp3', 'media/go.wav']
        /// Filenames include path from Blockly's root.  File extensions matter.</param>
        /// <param name="name">Name of sound.</param>
        abstract load: filenames: ResizeArray<string> * name: string -> unit
        /// Preload all the audio files so that they play quickly when asked for.
        abstract preload: unit -> unit
        /// <summary>Play a named sound at specified volume.  If volume is not specified,
        /// use full volume (1).</summary>
        /// <param name="name">Name of sound.</param>
        /// <param name="opt_volume">Volume of sound (0-1).</param>
        abstract play: name: string * ?opt_volume: float -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceAudio__ClassStatic =
        /// <summary>Class for loading, storing, and playing audio for a workspace.</summary>
        /// <param name="parentWorkspace">The parent of the workspace
        /// this audio object belongs to, or null.</param>
        [<Emit "new $0($1...)">] abstract Create: parentWorkspace: Blockly.WorkspaceSvg -> WorkspaceAudio__Class

    type [<AllowNullLiteral>] WorkspaceComment =
        inherit WorkspaceComment__Class

    type [<AllowNullLiteral>] WorkspaceCommentStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceComment

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceComment__Class =
        abstract id: string with get, set
        /// The comment's position in workspace units.  (0, 0) is at the workspace's
        /// origin; scale does not change this value.
        abstract xy_: Blockly.Utils.Coordinate with get, set
        abstract workspace: Blockly.Workspace with get, set
        abstract RTL: bool with get, set
        abstract content_: string with get, set
        abstract isComment: bool with get, set
        /// Dispose of this comment.
        abstract dispose: unit -> unit
        /// Get comment height.
        abstract getHeight: unit -> float
        /// <summary>Set comment height.</summary>
        /// <param name="height">Comment height.</param>
        abstract setHeight: height: float -> unit
        /// Get comment width.
        abstract getWidth: unit -> float
        /// <summary>Set comment width.</summary>
        /// <param name="width">comment width.</param>
        abstract setWidth: width: float -> unit
        /// Get stored location.
        abstract getXY: unit -> Blockly.Utils.Coordinate
        /// <summary>Move a comment by a relative offset.</summary>
        /// <param name="dx">Horizontal offset, in workspace units.</param>
        /// <param name="dy">Vertical offset, in workspace units.</param>
        abstract moveBy: dx: float * dy: float -> unit
        /// Get whether this comment is deletable or not.
        abstract isDeletable: unit -> bool
        /// <summary>Set whether this comment is deletable or not.</summary>
        /// <param name="deletable">True if deletable.</param>
        abstract setDeletable: deletable: bool -> unit
        /// Get whether this comment is movable or not.
        abstract isMovable: unit -> bool
        /// <summary>Set whether this comment is movable or not.</summary>
        /// <param name="movable">True if movable.</param>
        abstract setMovable: movable: bool -> unit
        /// Returns this comment's text.
        abstract getContent: unit -> string
        /// <summary>Set this comment's content.</summary>
        /// <param name="content">Comment content.</param>
        abstract setContent: content: string -> unit
        /// <summary>Encode a comment subtree as XML with XY coordinates.</summary>
        /// <param name="opt_noId">True if the encoder should skip the comment ID.</param>
        abstract toXmlWithXY: ?opt_noId: bool -> Element
        /// <summary>Encode a comment subtree as XML, but don't serialize the XY coordinates.
        /// This method avoids some expensive metrics-related calls that are made in
        /// toXmlWithXY().</summary>
        /// <param name="opt_noId">True if the encoder should skip the comment ID.</param>
        abstract toXml: ?opt_noId: bool -> Element

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceComment__ClassStatic =
        /// <summary>Class for a workspace comment.</summary>
        /// <param name="workspace">The block's workspace.</param>
        /// <param name="content">The content of this workspace comment.</param>
        /// <param name="height">Height of the comment.</param>
        /// <param name="width">Width of the comment.</param>
        /// <param name="opt_id">Optional ID.  Use this ID if provided, otherwise
        /// create a new ID.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace * content: string * height: float * width: float * ?opt_id: string -> WorkspaceComment__Class

    module WorkspaceComment =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Fire a create event for the given workspace comment, if comments are enabled.</summary>
            /// <param name="comment">The comment that was just created.</param>
            abstract fireCreateEvent: comment: Blockly.WorkspaceComment -> unit
            /// <summary>Decode an XML comment tag and create a comment on the workspace.</summary>
            /// <param name="xmlComment">XML comment element.</param>
            /// <param name="workspace">The workspace.</param>
            abstract fromXml: xmlComment: Element * workspace: Blockly.Workspace -> Blockly.WorkspaceComment
            /// <summary>Decode an XML comment tag and return the results in an object.</summary>
            /// <param name="xml">XML comment element.</param>
            abstract parseAttributes: xml: Element -> ParseAttributesReturn

        type [<AllowNullLiteral>] ParseAttributesReturn =
            abstract w: float with get, set
            abstract h: float with get, set
            abstract x: float with get, set
            abstract y: float with get, set
            abstract content: string with get, set

    type [<AllowNullLiteral>] WorkspaceCommentSvg =
        inherit WorkspaceCommentSvg__Class

    type [<AllowNullLiteral>] WorkspaceCommentSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceCommentSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceCommentSvg__Class =
        inherit Blockly.WorkspaceComment__Class
        /// Dispose of this comment.
        abstract dispose: unit -> unit
        /// Create and initialize the SVG representation of a workspace comment.
        /// May be called more than once.
        abstract initSvg: unit -> unit
        /// Select this comment.  Highlight it visually.
        abstract select: unit -> unit
        /// Unselect this comment.  Remove its highlighting.
        abstract unselect: unit -> unit
        /// Select this comment.  Highlight it visually.
        abstract addSelect: unit -> unit
        /// Unselect this comment.  Remove its highlighting.
        abstract removeSelect: unit -> unit
        /// Focus this comment.  Highlight it visually.
        abstract addFocus: unit -> unit
        /// Unfocus this comment.  Remove its highlighting.
        abstract removeFocus: unit -> unit
        /// Return the coordinates of the top-left corner of this comment relative to
        /// the drawing surface's origin (0,0), in workspace units.
        /// If the comment is on the workspace, (0, 0) is the origin of the workspace
        /// coordinate system.
        /// This does not change with workspace scale.
        abstract getRelativeToSurfaceXY: unit -> Blockly.Utils.Coordinate
        /// <summary>Move a comment by a relative offset.</summary>
        /// <param name="dx">Horizontal offset, in workspace units.</param>
        /// <param name="dy">Vertical offset, in workspace units.</param>
        abstract moveBy: dx: float * dy: float -> unit
        /// <summary>Transforms a comment by setting the translation on the transform attribute
        /// of the block's SVG.</summary>
        /// <param name="x">The x coordinate of the translation in workspace units.</param>
        /// <param name="y">The y coordinate of the translation in workspace units.</param>
        abstract translate: x: float * y: float -> unit
        /// <summary>Move this comment during a drag, taking into account whether we are using a
        /// drag surface to translate blocks.</summary>
        /// <param name="dragSurface">The surface that carries
        /// rendered items during a drag, or null if no drag surface is in use.</param>
        /// <param name="newLoc">The location to translate to, in
        /// workspace coordinates.</param>
        abstract moveDuringDrag: dragSurface: Blockly.BlockDragSurfaceSvg * newLoc: Blockly.Utils.Coordinate -> unit
        /// <summary>Move the bubble group to the specified location in workspace coordinates.</summary>
        /// <param name="x">The x position to move to.</param>
        /// <param name="y">The y position to move to.</param>
        abstract moveTo: x: float * y: float -> unit
        /// Returns the coordinates of a bounding box describing the dimensions of this
        /// comment.
        /// Coordinate system: workspace coordinates.
        abstract getBoundingRectangle: unit -> Blockly.Utils.Rect
        /// Add or remove the UI indicating if this comment is movable or not.
        abstract updateMovable: unit -> unit
        /// <summary>Set whether this comment is movable or not.</summary>
        /// <param name="movable">True if movable.</param>
        abstract setMovable: movable: bool -> unit
        /// <summary>Recursively adds or removes the dragging class to this node and its children.</summary>
        /// <param name="adding">True if adding, false if removing.</param>
        abstract setDragging: adding: bool -> unit
        /// Return the root node of the SVG or null if none exists.
        abstract getSvgRoot: unit -> SVGElement
        /// Returns this comment's text.
        abstract getContent: unit -> string
        /// <summary>Set this comment's content.</summary>
        /// <param name="content">Comment content.</param>
        abstract setContent: content: string -> unit
        /// <summary>Update the cursor over this comment by adding or removing a class.</summary>
        /// <param name="enable">True if the delete cursor should be shown, false
        /// otherwise.</param>
        abstract setDeleteStyle: enable: bool -> unit
        /// <summary>Encode a comment subtree as XML with XY coordinates.</summary>
        /// <param name="opt_noId">True if the encoder should skip the comment ID.</param>
        abstract toXmlWithXY: ?opt_noId: bool -> Element

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceCommentSvg__ClassStatic =
        /// <summary>Class for a workspace comment's SVG representation.</summary>
        /// <param name="workspace">The block's workspace.</param>
        /// <param name="content">The content of this workspace comment.</param>
        /// <param name="height">Height of the comment.</param>
        /// <param name="width">Width of the comment.</param>
        /// <param name="opt_id">Optional ID.  Use this ID if provided, otherwise
        /// create a new ID.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.Workspace * content: string * height: float * width: float * ?opt_id: string -> WorkspaceCommentSvg__Class

    module WorkspaceCommentSvg =

        type [<AllowNullLiteral>] IExports =
            abstract DEFAULT_SIZE: float
            /// <summary>Decode an XML comment tag and create a rendered comment on the workspace.</summary>
            /// <param name="xmlComment">XML comment element.</param>
            /// <param name="workspace">The workspace.</param>
            /// <param name="opt_wsWidth">The width of the workspace, which is used to
            /// position comments correctly in RTL.</param>
            abstract fromXml: xmlComment: Element * workspace: Blockly.Workspace * ?opt_wsWidth: float -> Blockly.WorkspaceCommentSvg

    type [<AllowNullLiteral>] WorkspaceDragSurfaceSvg =
        inherit WorkspaceDragSurfaceSvg__Class

    type [<AllowNullLiteral>] WorkspaceDragSurfaceSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceDragSurfaceSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceDragSurfaceSvg__Class =
        /// Dom structure when the workspace is being dragged. If there is no drag in
        /// progress, the SVG is empty and display: none.
        /// <svg class="blocklyWsDragSurface" style=transform:translate3d(...)>
        ///    <g class="blocklyBlockCanvas"></g>
        ///    <g class="blocklyBubbleCanvas">/g>
        /// </svg>
        abstract SVG_: obj option with get, set
        /// Create the drag surface and inject it into the container.
        abstract createDom: unit -> unit
        /// <summary>Translate the entire drag surface during a drag.
        /// We translate the drag surface instead of the blocks inside the surface
        /// so that the browser avoids repainting the SVG.
        /// Because of this, the drag coordinates must be adjusted by scale.</summary>
        /// <param name="x">X translation for the entire surface</param>
        /// <param name="y">Y translation for the entire surface</param>
        abstract translateSurface: x: float * y: float -> unit
        /// Reports the surface translation in scaled workspace coordinates.
        /// Use this when finishing a drag to return blocks to the correct position.
        abstract getSurfaceTranslation: unit -> Blockly.Utils.Coordinate
        /// <summary>Move the blockCanvas and bubbleCanvas out of the surface SVG and on to
        /// newSurface.</summary>
        /// <param name="newSurface">The element to put the drag surface contents
        /// into.</param>
        abstract clearAndHide: newSurface: SVGElement -> unit
        /// <summary>Set the SVG to have the block canvas and bubble canvas in it and then
        /// show the surface.</summary>
        /// <param name="blockCanvas">The block canvas <g> element from the
        /// workspace.</param>
        /// <param name="bubbleCanvas">The <g> element that contains the bubbles.</param>
        /// <param name="previousSibling">The element to insert the block canvas and
        /// bubble canvas after when it goes back in the DOM at the end of a drag.</param>
        /// <param name="width">The width of the workspace SVG element.</param>
        /// <param name="height">The height of the workspace SVG element.</param>
        /// <param name="scale">The scale of the workspace being dragged.</param>
        abstract setContentsAndShow: blockCanvas: SVGElement * bubbleCanvas: SVGElement * previousSibling: Element * width: float * height: float * scale: float -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceDragSurfaceSvg__ClassStatic =
        /// <summary>Blocks are moved into this SVG during a drag, improving performance.
        /// The entire SVG is translated using CSS transforms instead of SVG so the
        /// blocks are never repainted during drag improving performance.</summary>
        /// <param name="container">Containing element.</param>
        [<Emit "new $0($1...)">] abstract Create: container: Element -> WorkspaceDragSurfaceSvg__Class

    type [<AllowNullLiteral>] WorkspaceDragger =
        inherit WorkspaceDragger__Class

    type [<AllowNullLiteral>] WorkspaceDraggerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceDragger

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceDragger__Class =
        /// Sever all links from this object.
        abstract dispose: unit -> unit
        /// Start dragging the workspace.
        abstract startDrag: unit -> unit
        /// <summary>Finish dragging the workspace and put everything back where it belongs.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel coordinates.</param>
        abstract endDrag: currentDragDeltaXY: Blockly.Utils.Coordinate -> unit
        /// <summary>Move the workspace based on the most recent mouse movements.</summary>
        /// <param name="currentDragDeltaXY">How far the pointer has
        /// moved from the position at the start of the drag, in pixel coordinates.</param>
        abstract drag: currentDragDeltaXY: Blockly.Utils.Coordinate -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceDragger__ClassStatic =
        /// <summary>Class for a workspace dragger.  It moves the workspace around when it is
        /// being dragged by a mouse or touch.
        /// Note that the workspace itself manages whether or not it has a drag surface
        /// and how to do translations based on that.  This simply passes the right
        /// commands based on events.</summary>
        /// <param name="workspace">The workspace to drag.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg -> WorkspaceDragger__Class

    type [<AllowNullLiteral>] WorkspaceSvg =
        inherit WorkspaceSvg__Class

    type [<AllowNullLiteral>] WorkspaceSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceSvg__Class =
        inherit Blockly.Workspace__Class
        /// A wrapper function called when a resize event occurs.
        /// You can pass the result to `unbindEvent_`.
        abstract resizeHandlerWrapper_: ResizeArray<ResizeArray<obj option>> with get, set
        /// The render status of an SVG workspace.
        /// Returns `false` for headless workspaces and true for instances of
        /// `Blockly.WorkspaceSvg`.
        abstract rendered: bool with get, set
        /// Is this workspace the surface for a flyout?
        abstract isFlyout: bool with get, set
        /// Is this workspace the surface for a mutator?
        abstract isMutator: bool with get, set
        /// Current horizontal scrolling offset in pixel units, relative to the
        /// workspace origin.
        /// 
        /// It is useful to think about a view, and a canvas moving beneath that
        /// view. As the canvas moves right, this value becomes more positive, and
        /// the view is now "seeing" the left side of the canvas. As the canvas moves
        /// left, this value becomes more negative, and the view is now "seeing" the
        /// right side of the canvas.
        /// 
        /// The confusing thing about this value is that it does not, and must not
        /// include the absoluteLeft offset. This is because it is used to calculate
        /// the viewLeft value.
        /// 
        /// The viewLeft is relative to the workspace origin (although in pixel
        /// units). The workspace origin is the top-left corner of the workspace (at
        /// least when it is enabled). It is shifted from the top-left of the blocklyDiv
        /// so as not to be beneath the toolbox.
        /// 
        /// When the workspace is enabled the viewLeft and workspace origin are at
        /// the same X location. As the canvas slides towards the right beneath the view
        /// this value (scrollX) becomes more positive, and the viewLeft becomes more
        /// negative relative to the workspace origin (imagine the workspace origin
        /// as a dot on the canvas sliding to the right as the canvas moves).
        /// 
        /// So if the scrollX were to include the absoluteLeft this would in a way
        /// "unshift" the workspace origin. This means that the viewLeft would be
        /// representing the left edge of the blocklyDiv, rather than the left edge
        /// of the workspace.
        abstract scrollX: float with get, set
        /// Current vertical scrolling offset in pixel units, relative to the
        /// workspace origin.
        /// 
        /// It is useful to think about a view, and a canvas moving beneath that
        /// view. As the canvas moves down, this value becomes more positive, and the
        /// view is now "seeing" the upper part of the canvas. As the canvas moves
        /// up, this value becomes more negative, and the view is "seeing" the lower
        /// part of the canvas.
        /// 
        /// This confusing thing about this value is that it does not, and must not
        /// include the absoluteTop offset. This is because it is used to calculate
        /// the viewTop value.
        /// 
        /// The viewTop is relative to the workspace origin (although in pixel
        /// units). The workspace origin is the top-left corner of the workspace (at
        /// least when it is enabled). It is shifted from the top-left of the
        /// blocklyDiv so as not to be beneath the toolbox.
        /// 
        /// When the workspace is enabled the viewTop and workspace origin are at the
        /// same Y location. As the canvas slides towards the bottom this value
        /// (scrollY) becomes more positive, and the viewTop becomes more negative
        /// relative to the workspace origin (image in the workspace origin as a dot
        /// on the canvas sliding downwards as the canvas moves).
        /// 
        /// So if the scrollY were to include the absoluteTop this would in a way
        /// "unshift" the workspace origin. This means that the viewTop would be
        /// representing the top edge of the blocklyDiv, rather than the top edge of
        /// the workspace.
        abstract scrollY: float with get, set
        /// Horizontal scroll value when scrolling started in pixel units.
        abstract startScrollX: float with get, set
        /// Vertical scroll value when scrolling started in pixel units.
        abstract startScrollY: float with get, set
        /// Current scale.
        abstract scale: float with get, set
        abstract trashcan: Blockly.Trashcan with get, set
        /// This workspace's scrollbars, if they exist.
        abstract scrollbar: Blockly.ScrollbarPair with get, set
        /// <summary>Developers may define this function to add custom menu options to the
        /// workspace's context menu or edit the workspace-created set of menu options.</summary>
        /// <param name="options">List of menu options to add to.</param>
        abstract configureContextMenu: options: ResizeArray<Object> -> unit
        /// In a flyout, the target workspace where blocks should be placed after a drag.
        /// Otherwise null.
        abstract targetWorkspace: Blockly.WorkspaceSvg with get, set
        /// Get the block renderer attached to this workspace.
        abstract getRenderer: unit -> Blockly.BlockRendering.Renderer
        /// <summary>Add the cursor svg to this workspaces svg group.</summary>
        /// <param name="cursorSvg">The svg root of the cursor to be added to the
        /// workspace svg group.</param>
        abstract setCursorSvg: cursorSvg: SVGElement -> unit
        /// <summary>Add the marker svg to this workspaces svg group.</summary>
        /// <param name="markerSvg">The svg root of the marker to be added to the
        /// workspace svg group.</param>
        abstract setMarkerSvg: markerSvg: SVGElement -> unit
        /// Getter for the inverted screen CTM.
        abstract getInverseScreenCTM: unit -> SVGMatrix
        /// Mark the inverse screen CTM as dirty.
        abstract updateInverseScreenCTM: unit -> unit
        /// Getter for isVisible
        abstract isVisible: unit -> bool
        /// Return the position of the workspace origin relative to the injection div
        /// origin in pixels.
        /// The workspace origin is where a block would render at position (0, 0).
        /// It is not the upper left corner of the workspace SVG.
        abstract getOriginOffsetInPixels: unit -> Blockly.Utils.Coordinate
        /// Return the injection div that is a parent of this workspace.
        /// Walks the DOM the first time it's called, then returns a cached value.
        abstract getInjectionDiv: unit -> Element
        /// <summary>Save resize handler data so we can delete it later in dispose.</summary>
        /// <param name="handler">Data that can be passed to unbindEvent_.</param>
        abstract setResizeHandlerWrapper: handler: ResizeArray<ResizeArray<obj option>> -> unit
        /// <summary>Create the workspace DOM elements.</summary>
        /// <param name="opt_backgroundClass">Either 'blocklyMainBackground' or
        /// 'blocklyMutatorBackground'.</param>
        abstract createDom: ?opt_backgroundClass: string -> Element
        /// <g class="blocklyWorkspace">
        ///    <rect class="blocklyMainBackground" height="100%" width="100%"></rect>
        ///    [Trashcan and/or flyout may go here]
        ///    <g class="blocklyBlockCanvas"></g>
        ///    <g class="blocklyBubbleCanvas"></g>
        /// </g>
        abstract svgGroup_: SVGElement with get, set
        abstract svgBackground_: SVGElement with get, set
        abstract svgBlockCanvas_: SVGElement with get, set
        abstract svgBubbleCanvas_: SVGElement with get, set
        /// Dispose of this workspace.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// Add a trashcan.
        abstract addTrashcan: unit -> unit
        /// Add zoom controls.
        abstract addZoomControls: unit -> unit
        abstract zoomControls_: Blockly.ZoomControls with get, set
        /// Getter for the flyout associated with this workspace.  This flyout may be
        /// owned by either the toolbox or the workspace, depending on toolbox
        /// configuration.  It will be null if there is no flyout.
        abstract getFlyout: unit -> Blockly.Flyout
        /// Getter for the toolbox associated with this workspace, if one exists.
        abstract getToolbox: unit -> Blockly.Toolbox
        /// If enabled, resize the parts of the workspace that change when the workspace
        /// contents (e.g. block positions) change.  This will also scroll the
        /// workspace contents if needed.
        abstract resizeContents: unit -> unit
        /// Resize and reposition all of the workspace chrome (toolbox,
        /// trash, scrollbars etc.)
        /// This should be called when something changes that
        /// requires recalculating dimensions and positions of the
        /// trash, zoom, toolbox, etc. (e.g. window resize).
        abstract resize: unit -> unit
        /// Resizes and repositions workspace chrome if the page has a new
        /// scroll position.
        abstract updateScreenCalculationsIfScrolled: unit -> unit
        /// Get the SVG element that forms the drawing surface.
        abstract getCanvas: unit -> SVGElement
        /// Get the SVG element that forms the bubble surface.
        abstract getBubbleCanvas: unit -> SVGGElement
        /// Get the SVG element that contains this workspace.
        abstract getParentSvg: unit -> SVGElement
        /// <summary>Translate this workspace to new coordinates.</summary>
        /// <param name="x">Horizontal translation, in pixel units relative to the
        /// top left of the Blockly div.</param>
        /// <param name="y">Vertical translation, in pixel units relative to the
        /// top left of the Blockly div.</param>
        abstract translate: x: float * y: float -> unit
        /// Called at the end of a workspace drag to take the contents
        /// out of the drag surface and put them back into the workspace SVG.
        /// Does nothing if the workspace drag surface is not enabled.
        abstract resetDragSurface: unit -> unit
        /// Called at the beginning of a workspace drag to move contents of
        /// the workspace to the drag surface.
        /// Does nothing if the drag surface is not enabled.
        abstract setupDragSurface: unit -> unit
        abstract getBlockDragSurface: unit -> Blockly.BlockDragSurfaceSvg
        /// Returns the horizontal offset of the workspace.
        /// Intended for LTR/RTL compatibility in XML.
        abstract getWidth: unit -> float
        /// <summary>Toggles the visibility of the workspace.
        /// Currently only intended for main workspace.</summary>
        /// <param name="isVisible">True if workspace should be visible.</param>
        abstract setVisible: isVisible: bool -> unit
        /// Render all blocks in workspace.
        abstract render: unit -> unit
        /// Was used back when block highlighting (for execution) and block selection
        /// (for editing) were the same thing.
        /// Any calls of this function can be deleted.
        abstract traceOn: unit -> unit
        /// <summary>Highlight or unhighlight a block in the workspace.  Block highlighting is
        /// often used to visually mark blocks currently being executed.</summary>
        /// <param name="id">ID of block to highlight/unhighlight,
        /// or null for no block (used to unhighlight all blocks).</param>
        /// <param name="opt_state">If undefined, highlight specified block and
        /// automatically unhighlight all others.  If true or false, manually
        /// highlight/unhighlight the specified block.</param>
        abstract highlightBlock: id: string * ?opt_state: bool -> unit
        /// <summary>Paste the provided block onto the workspace.</summary>
        /// <param name="xmlBlock">XML block element.</param>
        abstract paste: xmlBlock: Element -> unit
        /// <summary>Paste the provided block onto the workspace.</summary>
        /// <param name="xmlBlock">XML block element.</param>
        abstract pasteBlock_: xmlBlock: Element -> unit
        /// Refresh the toolbox unless there's a drag in progress.
        abstract refreshToolboxSelection: unit -> unit
        /// <summary>Rename a variable by updating its name in the variable map.  Update the
        ///      flyout to show the renamed variable immediately.</summary>
        /// <param name="id">ID of the variable to rename.</param>
        /// <param name="newName">New variable name.</param>
        abstract renameVariableById: id: string * newName: string -> unit
        /// <summary>Delete a variable by the passed in ID.   Update the flyout to show
        ///      immediately that the variable is deleted.</summary>
        /// <param name="id">ID of variable to delete.</param>
        abstract deleteVariableById: id: string -> unit
        /// <summary>Create a new variable with the given name.  Update the flyout to show the
        ///      new variable immediately.</summary>
        /// <param name="name">The new variable's name.</param>
        /// <param name="opt_type">The type of the variable like 'int' or 'string'.
        /// Does not need to be unique. Field_variable can filter variables based on
        /// their type. This will default to '' which is a specific type.</param>
        /// <param name="opt_id">The unique ID of the variable. This will default to
        /// a UUID.</param>
        abstract createVariable: name: string * ?opt_type: string * ?opt_id: string -> Blockly.VariableModel
        /// Make a list of all the delete areas for this workspace.
        abstract recordDeleteAreas: unit -> unit
        /// <summary>Is the mouse event over a delete area (toolbox or non-closing flyout)?</summary>
        /// <param name="e">Mouse move event.</param>
        abstract isDeleteArea: e: Event -> float
        /// <summary>Start tracking a drag of an object on this workspace.</summary>
        /// <param name="e">Mouse down event.</param>
        /// <param name="xy">Starting location of object.</param>
        abstract startDrag: e: Event * xy: Blockly.Utils.Coordinate -> unit
        /// <summary>Track a drag of an object on this workspace.</summary>
        /// <param name="e">Mouse move event.</param>
        abstract moveDrag: e: Event -> Blockly.Utils.Coordinate
        /// Is the user currently dragging a block or scrolling the flyout/workspace?
        abstract isDragging: unit -> bool
        /// Is this workspace draggable?
        abstract isDraggable: unit -> bool
        /// Should the workspace have bounded content? Used to tell if the
        /// workspace's content should be sized so that it can move (bounded) or not
        /// (exact sizing).
        abstract isContentBounded: unit -> bool
        /// Is this workspace movable?
        /// 
        /// This means the user can reposition the X Y coordinates of the workspace
        /// through input. This can be through scrollbars, scroll wheel, dragging, or
        /// through zooming with the scroll wheel (since the zoom is centered on the
        /// mouse position). This does not include zooming with the zoom controls
        /// since the X Y coordinates are decided programmatically.
        abstract isMovable: unit -> bool
        /// Calculate the bounding box for the blocks on the workspace.
        /// Coordinate system: workspace coordinates.
        abstract getBlocksBoundingBox: unit -> Blockly.Utils.Rect
        /// Clean up the workspace by ordering all the blocks in a column.
        abstract cleanUp: unit -> unit
        /// <summary>Modify the block tree on the existing toolbox.</summary>
        /// <param name="tree">DOM tree of blocks, or text representation of same.</param>
        abstract updateToolbox: tree: U2<Node, string> -> unit
        /// Mark this workspace as the currently focused main workspace.
        abstract markFocused: unit -> unit
        /// <summary>Zooms the workspace in or out relative to/centered on the given (x, y)
        /// coordinate.</summary>
        /// <param name="x">X coordinate of center, in pixel units relative to the
        /// top-left corner of the parentSVG.</param>
        /// <param name="y">Y coordinate of center, in pixel units relative to the
        /// top-left corner of the parentSVG.</param>
        /// <param name="amount">Amount of zooming. The formula for the new scale
        /// is newScale = currentScale * (scaleSpeed^amount). scaleSpeed is set in
        /// the workspace options. Negative amount values zoom out, and positive
        /// amount values zoom in.</param>
        abstract zoom: x: float * y: float * amount: float -> unit
        /// <summary>Zooming the blocks centered in the center of view with zooming in or out.</summary>
        /// <param name="type">Type of zooming (-1 zooming out and 1 zooming in).</param>
        abstract zoomCenter: ``type``: float -> unit
        /// Zoom the blocks to fit in the workspace if possible.
        abstract zoomToFit: unit -> unit
        /// Add a transition class to the block and bubble canvas, to animate any
        /// transform changes.
        abstract beginCanvasTransition: unit -> unit
        /// Remove transition class from the block and bubble canvas.
        abstract endCanvasTransition: unit -> unit
        /// Center the workspace.
        abstract scrollCenter: unit -> unit
        /// <summary>Scroll the workspace to center on the given block.</summary>
        /// <param name="id">ID of block center on.</param>
        abstract centerOnBlock: id: string -> unit
        /// <summary>Set the workspace's zoom factor.</summary>
        /// <param name="newScale">Zoom factor. Units: (pixels / workspaceUnit).</param>
        abstract setScale: newScale: float -> unit
        /// <summary>Scroll the workspace to a specified offset (in pixels), keeping in the
        /// workspace bounds. See comment on workspaceSvg.scrollX for more detail on
        /// the meaning of these values.</summary>
        /// <param name="x">Target X to scroll to.</param>
        /// <param name="y">Target Y to scroll to.</param>
        abstract scroll: x: float * y: float -> unit
        /// <summary>Update whether this workspace has resizes enabled.
        /// If enabled, workspace will resize when appropriate.
        /// If disabled, workspace will not resize until re-enabled.
        /// Use to avoid resizing during a batch operation, for performance.</summary>
        /// <param name="enabled">Whether resizes should be enabled.</param>
        abstract setResizesEnabled: enabled: bool -> unit
        /// Dispose of all blocks in workspace, with an optimization to prevent resizes.
        abstract clear: unit -> unit
        /// <summary>Register a callback function associated with a given key, for clicks on
        /// buttons and labels in the flyout.
        /// For instance, a button specified by the XML
        /// <button text="create variable" callbackKey="CREATE_VARIABLE"></button>
        /// should be matched by a call to
        /// registerButtonCallback("CREATE_VARIABLE", yourCallbackFunction).</summary>
        /// <param name="key">The name to use to look up this function.</param>
        /// <param name="func">The function to call when the
        /// given button is clicked.</param>
        abstract registerButtonCallback: key: string * func: WorkspaceSvg__ClassRegisterButtonCallbackFunc -> unit
        /// <summary>Get the callback function associated with a given key, for clicks on buttons
        /// and labels in the flyout.</summary>
        /// <param name="key">The name to use to look up the function.</param>
        abstract getButtonCallback: key: string -> WorkspaceSvg__ClassGetButtonCallbackReturn
        /// <summary>Remove a callback for a click on a button in the flyout.</summary>
        /// <param name="key">The name associated with the callback function.</param>
        abstract removeButtonCallback: key: string -> unit
        /// <summary>Register a callback function associated with a given key, for populating
        /// custom toolbox categories in this workspace.  See the variable and procedure
        /// categories as an example.</summary>
        /// <param name="key">The name to use to look up this function.</param>
        /// <param name="func">The function to
        /// call when the given toolbox category is opened.</param>
        abstract registerToolboxCategoryCallback: key: string * func: WorkspaceSvg__ClassRegisterToolboxCategoryCallbackFunc -> unit
        /// <summary>Get the callback function associated with a given key, for populating
        /// custom toolbox categories in this workspace.</summary>
        /// <param name="key">The name to use to look up the function.</param>
        abstract getToolboxCategoryCallback: key: string -> WorkspaceSvg__ClassGetToolboxCategoryCallbackReturn
        /// <summary>Remove a callback for a click on a custom category's name in the toolbox.</summary>
        /// <param name="key">The name associated with the callback function.</param>
        abstract removeToolboxCategoryCallback: key: string -> unit
        /// <summary>Look up the gesture that is tracking this touch stream on this workspace.
        /// May create a new gesture.</summary>
        /// <param name="e">Mouse event or touch event.</param>
        abstract getGesture: e: Event -> Blockly.TouchGesture
        /// Clear the reference to the current gesture.
        abstract clearGesture: unit -> unit
        /// Cancel the current gesture, if one exists.
        abstract cancelCurrentGesture: unit -> unit
        /// Get the audio manager for this workspace.
        abstract getAudioManager: unit -> Blockly.WorkspaceAudio
        /// Get the grid object for this workspace, or null if there is none.
        abstract getGrid: unit -> Blockly.Grid

    type [<AllowNullLiteral>] WorkspaceSvg__ClassRegisterButtonCallbackFunc =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.FlyoutButton -> obj option

    type [<AllowNullLiteral>] WorkspaceSvg__ClassGetButtonCallbackReturn =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.FlyoutButton -> obj option

    type [<AllowNullLiteral>] WorkspaceSvg__ClassRegisterToolboxCategoryCallbackFunc =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.Workspace -> ResizeArray<Element>

    type [<AllowNullLiteral>] WorkspaceSvg__ClassGetToolboxCategoryCallbackReturn =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.Workspace -> ResizeArray<Element>

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] WorkspaceSvg__ClassStatic =
        /// <summary>Class for a workspace.  This is an onscreen area with optional trashcan,
        /// scrollbars, bubbles, and dragging.</summary>
        /// <param name="options">Dictionary of options.</param>
        /// <param name="opt_blockDragSurface">Drag surface for
        /// blocks.</param>
        /// <param name="opt_wsDragSurface">Drag surface for
        /// the workspace.</param>
        [<Emit "new $0($1...)">] abstract Create: options: Blockly.Options * ?opt_blockDragSurface: Blockly.BlockDragSurfaceSvg * ?opt_wsDragSurface: Blockly.WorkspaceDragSurfaceSvg -> WorkspaceSvg__Class

    module Xml =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Encode a block tree as XML.</summary>
            /// <param name="workspace">The workspace containing blocks.</param>
            /// <param name="opt_noId">True if the encoder should skip the block IDs.</param>
            abstract workspaceToDom: workspace: Blockly.Workspace * ?opt_noId: bool -> Element
            /// <summary>Encode a list of variables as XML.</summary>
            /// <param name="variableList">List of all variable
            /// models.</param>
            abstract variablesToDom: variableList: ResizeArray<Blockly.VariableModel> -> Element
            /// <summary>Encode a block subtree as XML with XY coordinates.</summary>
            /// <param name="block">The root block to encode.</param>
            /// <param name="opt_noId">True if the encoder should skip the block ID.</param>
            abstract blockToDomWithXY: block: Blockly.Block * ?opt_noId: bool -> Element
            /// <summary>Encode a block subtree as XML.</summary>
            /// <param name="block">The root block to encode.</param>
            /// <param name="opt_noId">True if the encoder should skip the block ID.</param>
            abstract blockToDom: block: Blockly.Block * ?opt_noId: bool -> Element
            /// <summary>Converts a DOM structure into plain text.
            /// Currently the text format is fairly ugly: all one line with no whitespace,
            /// unless the DOM itself has whitespace built-in.</summary>
            /// <param name="dom">A tree of XML elements.</param>
            abstract domToText: dom: Element -> string
            /// <summary>Converts a DOM structure into properly indented text.</summary>
            /// <param name="dom">A tree of XML elements.</param>
            abstract domToPrettyText: dom: Element -> string
            /// <summary>Converts an XML string into a DOM structure.</summary>
            /// <param name="text">An XML string.</param>
            abstract textToDom: text: string -> Element
            /// <summary>Clear the given workspace then decode an XML DOM and
            /// create blocks on the workspace.</summary>
            /// <param name="xml">XML DOM.</param>
            /// <param name="workspace">The workspace.</param>
            abstract clearWorkspaceAndLoadFromXml: xml: Element * workspace: Blockly.Workspace -> ResizeArray<string>
            /// <summary>Decode an XML DOM and create blocks on the workspace.</summary>
            /// <param name="xml">XML DOM.</param>
            /// <param name="workspace">The workspace.</param>
            abstract domToWorkspace: xml: Element * workspace: Blockly.Workspace -> ResizeArray<string>
            /// <summary>Decode an XML DOM and create blocks on the workspace. Position the new
            /// blocks immediately below prior blocks, aligned by their starting edge.</summary>
            /// <param name="xml">The XML DOM.</param>
            /// <param name="workspace">The workspace to add to.</param>
            abstract appendDomToWorkspace: xml: Element * workspace: Blockly.Workspace -> ResizeArray<string>
            /// <summary>Decode an XML block tag and create a block (and possibly sub blocks) on the
            /// workspace.</summary>
            /// <param name="xmlBlock">XML block element.</param>
            /// <param name="workspace">The workspace.</param>
            abstract domToBlock: xmlBlock: Element * workspace: Blockly.Workspace -> Blockly.Block
            /// <summary>Decode an XML list of variables and add the variables to the workspace.</summary>
            /// <param name="xmlVariables">List of XML variable elements.</param>
            /// <param name="workspace">The workspace to which the variable
            /// should be added.</param>
            abstract domToVariables: xmlVariables: Element * workspace: Blockly.Workspace -> unit
            /// <summary>Remove any 'next' block (statements in a stack).</summary>
            /// <param name="xmlBlock">XML block element.</param>
            abstract deleteNext: xmlBlock: Element -> unit

    type [<AllowNullLiteral>] ZoomControls =
        inherit ZoomControls__Class

    type [<AllowNullLiteral>] ZoomControlsStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> ZoomControls

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ZoomControls__Class =
        /// Create the zoom controls.
        abstract createDom: unit -> SVGElement
        /// <summary>Initialize the zoom controls.</summary>
        /// <param name="verticalSpacing">Vertical distances from workspace edge to the
        /// same edge of the controls.</param>
        abstract init: verticalSpacing: float -> float
        /// Dispose of this zoom controls.
        /// Unlink from all DOM elements to prevent memory leaks.
        abstract dispose: unit -> unit
        /// Position the zoom controls.
        /// It is positioned in the opposite corner to the corner the
        /// categories/toolbox starts at.
        abstract position: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ZoomControls__ClassStatic =
        /// <summary>Class for a zoom controls.</summary>
        /// <param name="workspace">The workspace to sit in.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg -> ZoomControls__Class

    type [<AllowNullLiteral>] Component =
        inherit Component__Class

    type [<AllowNullLiteral>] ComponentStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Component

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Component__Class =
        /// Gets the unique ID for the instance of this component.  If the instance
        /// doesn't already have an ID, generates one on the fly.
        abstract getId: unit -> string
        /// Gets the component's element.
        abstract getElement: unit -> Element
        /// <summary>Sets the component's root element to the given element.  Considered
        /// protected and final.
        /// 
        /// This should generally only be called during createDom. Setting the element
        /// does not actually change which element is rendered, only the element that is
        /// associated with this UI component.
        /// 
        /// This should only be used by subclasses and its associated renderers.</summary>
        /// <param name="element">Root element for the component.</param>
        abstract setElementInternal: element: Element -> unit
        /// <summary>Sets the parent of this component to use for event bubbling.  Throws an error
        /// if the component already has a parent or if an attempt is made to add a
        /// component to itself as a child.</summary>
        /// <param name="parent">The parent component.</param>
        abstract setParent: parent: Blockly.Component -> unit
        /// Returns the component's parent, if any.
        abstract getParent: unit -> Blockly.Component
        /// Determines whether the component has been added to the document.
        abstract isInDocument: unit -> bool
        /// Creates the initial DOM representation for the component.  The default
        /// implementation is to set this.element_ = div.
        abstract createDom: unit -> unit
        /// <summary>Renders the component.  If a parent element is supplied, the component's
        /// element will be appended to it.  If there is no optional parent element and
        /// the element doesn't have a parentNode then it will be appended to the
        /// document body.
        /// 
        /// If this component has a parent component, and the parent component is
        /// not in the document already, then this will not call `enterDocument`
        /// on this component.
        /// 
        /// Throws an Error if the component is already rendered.</summary>
        /// <param name="opt_parentElement">Optional parent element to render the
        /// component into.</param>
        abstract render: ?opt_parentElement: Element -> unit
        /// <summary>Renders the component before another element. The other element should be in
        /// the document already.
        /// 
        /// Throws an Error if the component is already rendered.</summary>
        /// <param name="sibling">Node to render the component before.</param>
        abstract renderBefore: sibling: Node -> unit
        /// Called when the component's element is known to be in the document. Anything
        /// using document.getElementById etc. should be done at this stage.
        /// 
        /// If the component contains child components, this call is propagated to its
        /// children.
        abstract enterDocument: unit -> unit
        /// Called by dispose to clean up the elements and listeners created by a
        /// component, or by a parent component/application who has removed the
        /// component from the document but wants to reuse it later.
        /// 
        /// If the component contains child components, this call is propagated to its
        /// children.
        /// 
        /// It should be possible for the component to be rendered again once this method
        /// has been called.
        abstract exitDocument: unit -> unit
        /// Disposes of the object. If the object hasn't already been disposed of, calls
        /// {@link #disposeInternal}.
        abstract dispose: unit -> unit
        /// Disposes of the component.  Calls `exitDocument`, which is expected to
        /// remove event handlers and clean up the component.  Propagates the call to
        /// the component's children, if any. Removes the component's DOM from the
        /// document.
        abstract disposeInternal: unit -> unit
        /// <summary>Adds the specified component as the last child of this component.  See
        /// {@link Blockly.Component#addChildAt} for detailed semantics.</summary>
        /// <param name="child">The new child component.</param>
        /// <param name="opt_render">If true, the child component will be rendered
        /// into the parent.</param>
        abstract addChild: child: Blockly.Component * ?opt_render: bool -> unit
        /// <summary>Adds the specified component as a child of this component at the given
        /// 0-based index.
        /// 
        /// Both `addChild` and `addChildAt` assume the following contract
        /// between parent and child components:
        ///   <ul>
        ///     <li>the child component's element must be a descendant of the parent
        ///         component's element, and
        ///     <li>the DOM state of the child component must be consistent with the DOM
        ///         state of the parent component (see `isInDocument`) in the
        ///         steady state -- the exception is to addChildAt(child, i, false) and
        ///         then immediately decorate/render the child.
        ///   </ul>
        /// 
        /// In particular, `parent.addChild(child)` will throw an error if the
        /// child component is already in the document, but the parent isn't.
        /// 
        /// Clients of this API may call `addChild` and `addChildAt` with
        /// `opt_render` set to true.  If `opt_render` is true, calling these
        /// methods will automatically render the child component's element into the
        /// parent component's element. If the parent does not yet have an element, then
        /// `createDom` will automatically be invoked on the parent before
        /// rendering the child.
        /// 
        /// Invoking {@code parent.addChild(child, true)} will throw an error if the
        /// child component is already in the document, regardless of the parent's DOM
        /// state.
        /// 
        /// If `opt_render` is true and the parent component is not already
        /// in the document, `enterDocument` will not be called on this component
        /// at this point.
        /// 
        /// Finally, this method also throws an error if the new child already has a
        /// different parent, or the given index is out of bounds.</summary>
        /// <param name="child">The new child component.</param>
        /// <param name="index">0-based index at which the new child component is to be
        /// added; must be between 0 and the current child count (inclusive).</param>
        /// <param name="opt_render">If true, the child component will be rendered
        /// into the parent.</param>
        abstract addChildAt: child: Blockly.Component * index: float * ?opt_render: bool -> unit
        /// Returns the DOM element into which child components are to be rendered,
        /// or null if the component itself hasn't been rendered yet.  This default
        /// implementation returns the component's root element.  Subclasses with
        /// complex DOM structures must override this method.
        abstract getContentElement: unit -> Element
        /// Returns true if the component is rendered right-to-left, false otherwise.
        /// The first time this function is invoked, the right-to-left rendering property
        /// is set if it has not been already.
        abstract isRightToLeft: unit -> bool
        /// <summary>Set is right-to-left. This function should be used if the component needs
        /// to know the rendering direction during DOM creation (i.e. before
        /// {@link #enterDocument} is called and is right-to-left is set).</summary>
        /// <param name="rightToLeft">Whether the component is rendered
        /// right-to-left.</param>
        abstract setRightToLeft: rightToLeft: bool -> unit
        /// Returns true if the component has children.
        abstract hasChildren: unit -> bool
        /// Returns the number of children of this component.
        abstract getChildCount: unit -> float
        /// <summary>Returns the child with the given ID, or null if no such child exists.</summary>
        /// <param name="id">Child component ID.</param>
        abstract getChild: id: string -> Blockly.Component
        /// <summary>Returns the child at the given index, or null if the index is out of bounds.</summary>
        /// <param name="index">0-based index.</param>
        abstract getChildAt: index: float -> Blockly.Component
        /// <summary>Calls the given function on each of this component's children in order.  If
        /// `opt_obj` is provided, it will be used as the 'this' object in the
        /// function when called.  The function should take two arguments:  the child
        /// component and its 0-based index.  The return value is ignored.</summary>
        /// <param name="f">The function to call for every
        /// child component; should take 2 arguments (the child and its index).</param>
        /// <param name="opt_obj">Used as the 'this' object in f when called.</param>
        abstract forEachChild: f: Component__ClassForEachChildF * ?opt_obj: 'T -> unit
        /// <summary>Returns the 0-based index of the given child component, or -1 if no such
        /// child is found.</summary>
        /// <param name="child">The child component.</param>
        abstract indexOfChild: child: Blockly.Component -> float

    type [<AllowNullLiteral>] Component__ClassForEachChildF =
        [<Emit "$0($1...)">] abstract Invoke: _0: obj option * _1: float -> obj option

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Component__ClassStatic =
        /// Default implementation of a UI component.
        /// Similar to Closure's goog.ui.Component.
        [<Emit "new $0($1...)">] abstract Create: unit -> Component__Class

    module Component =

        type [<AllowNullLiteral>] IExports =
            abstract defaultRightToLeft: bool

        type Error =
            obj

    type [<AllowNullLiteral>] Action =
        inherit Action__Class

    type [<AllowNullLiteral>] ActionStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Action

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Action__Class =
        interface end

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Action__ClassStatic =
        /// <summary>Class for a single action.
        /// An action describes user intent. (ex go to next or go to previous)</summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="desc">The description of the action.</param>
        [<Emit "new $0($1...)">] abstract Create: name: string * desc: string -> Action__Class

    type [<AllowNullLiteral>] ASTNode =
        inherit ASTNode__Class

    type [<AllowNullLiteral>] ASTNodeStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> ASTNode

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ASTNode__Class =
        /// Gets the value pointed to by this node.
        /// It is the callers responsibility to check the node type to figure out what
        /// type of object they get back from this.
        abstract getLocation: unit -> U4<Blockly.Field, Blockly.Connection, Blockly.Block, Blockly.Workspace>
        /// The type of the current location.
        /// One of Blockly.ASTNode.types
        abstract getType: unit -> string
        /// The coordinate on the workspace.
        abstract getWsCoordinate: unit -> Blockly.Utils.Coordinate
        /// Whether the node points to a connection.
        abstract isConnection: unit -> bool
        /// Find the element to the right of the current element in the AST.
        abstract next: unit -> Blockly.ASTNode
        /// Find the element one level below and all the way to the left of the current
        /// location.
        abstract ``in``: unit -> Blockly.ASTNode
        /// Find the element to the left of the current element in the AST.
        abstract prev: unit -> Blockly.ASTNode
        /// Find the next element that is one position above and all the way to the left
        /// of the current location.
        abstract out: unit -> Blockly.ASTNode

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] ASTNode__ClassStatic =
        /// <summary>Class for an AST node.
        /// It is recommended that you use one of the createNode methods instead of
        /// creating a node directly.</summary>
        /// <param name="type">The type of the location.
        /// Must be in Bockly.ASTNode.types.</param>
        /// <param name="opt_params">Optional dictionary of options.</param>
        [<Emit "new $0($1...)">] abstract Create: ``type``: string * location: U4<Blockly.Block, Blockly.Connection, Blockly.Field, Blockly.Workspace> * ?opt_params: Object -> ASTNode__Class

    module ASTNode =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Create an AST node pointing to a field.</summary>
            /// <param name="field">The location of the AST node.</param>
            abstract createFieldNode: field: Blockly.Field -> Blockly.ASTNode
            /// <summary>Creates an AST node pointing to a connection. If the connection has a parent
            /// input then create an AST node of type input that will hold the connection.</summary>
            /// <param name="connection">This is the connection the node will
            /// point to.</param>
            abstract createConnectionNode: connection: Blockly.Connection -> Blockly.ASTNode
            /// <summary>Creates an AST node pointing to an input. Stores the input connection as the
            ///      location.</summary>
            /// <param name="input">The input used to create an AST node.</param>
            abstract createInputNode: input: Blockly.Input -> Blockly.ASTNode
            /// <summary>Creates an AST node pointing to a block.</summary>
            /// <param name="block">The block used to create an AST node.</param>
            abstract createBlockNode: block: Blockly.Block -> Blockly.ASTNode
            /// <summary>Create an AST node of type stack. A stack, represented by its top block, is
            ///      the set of all blocks connected to a top block, including the top block.</summary>
            /// <param name="topBlock">A top block has no parent and can be found
            /// in the list returned by workspace.getTopBlocks().</param>
            abstract createStackNode: topBlock: Blockly.Block -> Blockly.ASTNode
            /// <summary>Creates an AST node pointing to a workspace.</summary>
            /// <param name="workspace">The workspace that we are on.</param>
            /// <param name="wsCoordinate">The position on the workspace
            /// for this node.</param>
            abstract createWorkspaceNode: workspace: Blockly.Workspace * wsCoordinate: Blockly.Utils.Coordinate -> Blockly.ASTNode

        type types =
            obj

    type [<AllowNullLiteral>] Cursor =
        inherit Cursor__Class

    type [<AllowNullLiteral>] CursorStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Cursor

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Cursor__Class =
        /// <summary>Sets the object in charge of drawing the cursor.</summary>
        /// <param name="drawer">The object in charge of drawing the cursor.</param>
        abstract setDrawer: drawer: Blockly.CursorSvg -> unit
        /// Get the current drawer for the cursor.
        abstract getDrawer: unit -> Blockly.CursorSvg
        /// Gets the current location of the cursor.
        abstract getCurNode: unit -> Blockly.ASTNode
        /// <summary>Set the location of the cursor and call the update method.
        /// Setting isStack to true will only work if the newLocation is the top most
        /// output or previous connection on a stack.</summary>
        /// <param name="newNode">The new location of the cursor.</param>
        abstract setCurNode: newNode: Blockly.ASTNode -> unit
        /// Hide the cursor SVG.
        abstract hide: unit -> unit
        /// Find the next connection, field, or block.
        abstract next: unit -> Blockly.ASTNode
        /// Find the in connection or field.
        abstract ``in``: unit -> Blockly.ASTNode
        /// Find the previous connection, field, or block.
        abstract prev: unit -> Blockly.ASTNode
        /// Find the out connection, field, or block.
        abstract out: unit -> Blockly.ASTNode

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Cursor__ClassStatic =
        /// Class for a cursor.
        /// A cursor controls how a user navigates the Blockly AST.
        [<Emit "new $0($1...)">] abstract Create: unit -> Cursor__Class

    type [<AllowNullLiteral>] CursorSvg =
        inherit CursorSvg__Class

    type [<AllowNullLiteral>] CursorSvgStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> CursorSvg

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] CursorSvg__Class =
        /// The current SVG element for the cursor.
        abstract currentCursorSvg: Element with get, set
        /// Return the root node of the SVG or null if none exists.
        abstract getSvgRoot: unit -> SVGElement
        /// Create the DOM element for the cursor.
        abstract createDom: unit -> SVGElement
        /// <summary>Position the cursor for a block.
        /// Displays an outline of the top half of a rectangle around a block.</summary>
        /// <param name="width">The width of the block.</param>
        /// <param name="cursorOffset">The extra padding for around the block.</param>
        /// <param name="cursorHeight">The height of the cursor.</param>
        abstract positionBlock_: width: float * cursorOffset: float * cursorHeight: float -> unit
        /// Hide the cursor.
        abstract hide: unit -> unit
        /// <summary>Update the cursor.</summary>
        /// <param name="curNode">The node that we want to draw the cursor for.</param>
        abstract draw: curNode: Blockly.ASTNode -> unit
        /// Dispose of this cursor.
        abstract dispose: unit -> unit

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] CursorSvg__ClassStatic =
        /// <summary>Class for a cursor.</summary>
        /// <param name="workspace">The workspace the cursor belongs to.</param>
        /// <param name="opt_marker">True if the cursor is a marker. A marker is used
        /// to save a location and is an immovable cursor. False or undefined if the
        /// cursor is not a marker.</param>
        [<Emit "new $0($1...)">] abstract Create: workspace: Blockly.WorkspaceSvg * ?opt_marker: bool -> CursorSvg__Class

    module CursorSvg =

        type [<AllowNullLiteral>] IExports =
            abstract CURSOR_HEIGHT: float
            abstract CURSOR_WIDTH: float
            abstract NOTCH_START_LENGTH: float
            abstract VERTICAL_PADDING: float
            abstract STACK_PADDING: float
            abstract BLOCK_PADDING: float
            abstract HEIGHT_MULTIPLIER: float
            abstract CURSOR_COLOR: string
            abstract MARKER_COLOR: string
            abstract CURSOR_CLASS: obj option
            abstract MARKER_CLASS: obj option

    type [<AllowNullLiteral>] FlyoutCursor =
        inherit FlyoutCursor__Class

    type [<AllowNullLiteral>] FlyoutCursorStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> FlyoutCursor

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutCursor__Class =
        inherit Blockly.Cursor__Class

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] FlyoutCursor__ClassStatic =
        /// Class for a flyout cursor.
        /// This controls how a user navigates blocks in the flyout.
        [<Emit "new $0($1...)">] abstract Create: unit -> FlyoutCursor__Class

    module User =
        let [<Import("keyMap","blockly")>] keyMap: KeyMap.IExports = jsNative

        module KeyMap =

            type [<AllowNullLiteral>] IExports =
                abstract map_: TypeLiteral_02
                /// <summary>Update the key map to contain the new action.</summary>
                /// <param name="keyCode">The key code serialized by the serializeKeyEvent.</param>
                /// <param name="action">The action to be executed when the keys
                /// corresponding to the serialized key code is pressed.</param>
                abstract setActionForKey: keyCode: string * action: Blockly.Action -> unit
                /// <summary>Creates a new key map.</summary>
                /// <param name="keyMap">The object holding the key
                /// to action mapping.</param>
                abstract setKeyMap: keyMap: SetKeyMapKeyMap -> unit
                /// Gets the current key map.
                abstract getKeyMap: unit -> GetKeyMapReturn
                /// <summary>Get the action by the serialized key code.</summary>
                /// <param name="keyCode">The serialized key code.</param>
                abstract getActionByKeyCode: keyCode: string -> U2<Blockly.Action, obj option>
                /// <summary>Get the serialized key that corresponds to the action.</summary>
                /// <param name="action">The action for which we want to get
                /// the key.</param>
                abstract getKeyByAction: action: Blockly.Action -> string
                /// <summary>Serialize the key event.</summary>
                /// <param name="e">A key up event holding the key code.</param>
                abstract serializeKeyEvent: e: Event -> string
                /// <summary>Create the serialized key code that will be used in the key map.</summary>
                /// <param name="keyCode">Number code representing the key.</param>
                /// <param name="modifiers">List of modifiers to be used with the key.
                /// All valid modifiers can be found in the Blockly.user.keyMap.modifierKeys.</param>
                abstract createSerializedKey: keyCode: float * modifiers: ResizeArray<string> -> string
                /// Creates the default key map.
                abstract createDefaultKeyMap: unit -> CreateDefaultKeyMapReturn

            type [<AllowNullLiteral>] SetKeyMapKeyMap =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Action with get, set

            type [<AllowNullLiteral>] GetKeyMapReturn =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Action with get, set

            type [<AllowNullLiteral>] CreateDefaultKeyMapReturn =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Action with get, set

            type modifierKeys =
                obj

            type [<AllowNullLiteral>] TypeLiteral_02 =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Action with get, set

    type [<AllowNullLiteral>] MarkerCursor =
        inherit MarkerCursor__Class

    type [<AllowNullLiteral>] MarkerCursorStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> MarkerCursor

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] MarkerCursor__Class =
        inherit Blockly.Cursor__Class

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] MarkerCursor__ClassStatic =
        /// Class for a marker.
        /// This is used in keyboard navigation to save a location in the Blockly AST.
        [<Emit "new $0($1...)">] abstract Create: unit -> MarkerCursor__Class

    module Navigation =

        type [<AllowNullLiteral>] IExports =
            abstract loggingCallback: TypeLiteral_03
            abstract STATE_FLYOUT: float
            abstract STATE_WS: float
            abstract STATE_TOOLBOX: float
            /// If there is a marked connection try connecting the block from the flyout to
            /// that connection. If no connection has been marked then inserting will place
            /// it on the workspace.
            abstract insertFromFlyout: unit -> unit
            /// <summary>Tries to connect the given block to the destination connection, making an
            /// intelligent guess about which connection to use to on the moving block.</summary>
            /// <param name="block">The block to move.</param>
            /// <param name="destConnection">The connection to connect to.</param>
            abstract insertBlock: block: Blockly.Block * destConnection: Blockly.Connection -> bool
            /// <summary>Set the current navigation state.</summary>
            /// <param name="newState">The new navigation state.</param>
            abstract setState: newState: float -> unit
            /// <summary>Gets the top node on a block.
            /// This is either the previous connection, output connection or the block.</summary>
            /// <param name="block">The block to find the top most AST node on.</param>
            abstract getTopNode: block: Blockly.Block -> Blockly.ASTNode
            /// <summary>Before a block is deleted move the cursor to the appropriate position.</summary>
            /// <param name="deletedBlock">The block that is being deleted.</param>
            abstract moveCursorOnBlockDelete: deletedBlock: Blockly.Block -> unit
            /// <summary>When a block that the cursor is on is mutated move the cursor to the block
            /// level.</summary>
            /// <param name="mutatedBlock">The block that is being mutated.</param>
            abstract moveCursorOnBlockMutation: mutatedBlock: Blockly.Block -> unit
            /// Enable accessibility mode.
            abstract enableKeyboardAccessibility: unit -> unit
            /// Disable accessibility mode.
            abstract disableKeyboardAccessibility: unit -> unit
            /// <summary>Handler for all the keyboard navigation events.</summary>
            /// <param name="e">The keyboard event.</param>
            abstract onKeyPress: e: Event -> bool
            /// <summary>Execute any actions on the flyout, workspace, or toolbox that correspond to
            /// the given action.</summary>
            /// <param name="action">The current action.</param>
            abstract onBlocklyAction: action: Blockly.Action -> bool
            abstract ACTION_PREVIOUS: Blockly.Action
            abstract ACTION_OUT: Blockly.Action
            abstract ACTION_NEXT: Blockly.Action
            abstract ACTION_IN: Blockly.Action
            abstract ACTION_INSERT: Blockly.Action
            abstract ACTION_MARK: Blockly.Action
            abstract ACTION_DISCONNECT: Blockly.Action
            abstract ACTION_TOOLBOX: Blockly.Action
            abstract ACTION_EXIT: Blockly.Action
            abstract ACTION_TOGGLE_KEYBOARD_NAV: Blockly.Action
            abstract READONLY_ACTION_LIST: ResizeArray<Blockly.Action>

        type actionNames =
            obj

        type [<AllowNullLiteral>] TypeLiteral_03 =
            [<Emit "$0($1...)">] abstract Invoke: _0: string * _1: string -> obj option

    type [<AllowNullLiteral>] Menu =
        inherit Menu__Class

    type [<AllowNullLiteral>] MenuStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Menu

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Menu__Class =
        inherit Blockly.Component__Class
        /// Focus the menu element.
        abstract focus: unit -> unit
        /// Blur the menu element.
        abstract blur: unit -> unit
        /// <summary>Set the menu accessibility role.</summary>
        /// <param name="roleName">role name.</param>
        abstract setRole: roleName: U2<Blockly.Utils.Aria.Role, string> -> unit
        /// <summary>Returns the child menuitem that owns the given DOM node, or null if no such
        /// menuitem is found.</summary>
        /// <param name="node">DOM node whose owner is to be returned.</param>
        abstract getMenuItem: node: Node -> Blockly.MenuItem
        /// Unhighlight the current highlighted item.
        abstract unhighlightCurrent: unit -> unit
        /// Clears the currently highlighted item.
        abstract clearHighlighted: unit -> unit
        /// Returns the currently highlighted item (if any).
        abstract getHighlighted: unit -> Blockly.Component
        /// <summary>Highlights the item at the given 0-based index (if any). If another item
        /// was previously highlighted, it is un-highlighted.</summary>
        /// <param name="index">Index of item to highlight (-1 removes the current
        /// highlight).</param>
        abstract setHighlightedIndex: index: float -> unit
        /// <summary>Highlights the given item if it exists and is a child of the container;
        /// otherwise un-highlights the currently highlighted item.</summary>
        /// <param name="item">Item to highlight.</param>
        abstract setHighlighted: item: Blockly.MenuItem -> unit
        /// Highlights the next highlightable item (or the first if nothing is currently
        /// highlighted).
        abstract highlightNext: unit -> unit
        /// Highlights the previous highlightable item (or the last if nothing is
        /// currently highlighted).
        abstract highlightPrevious: unit -> unit
        /// <summary>Helper function that manages the details of moving the highlight among
        /// child menuitems in response to keyboard events.</summary>
        /// <param name="fn">Function that accepts the current and maximum indices, and returns the
        /// next index to check.</param>
        /// <param name="startIndex">Start index.</param>
        abstract highlightHelper: fn: Menu__ClassHighlightHelperFn * startIndex: float -> bool
        /// <summary>Returns whether the given item can be highlighted.</summary>
        /// <param name="item">The item to check.</param>
        abstract canHighlightItem: item: Blockly.MenuItem -> bool
        /// <summary>Attempts to handle a keyboard event, if the menuitem is enabled, by calling
        /// {@link handleKeyEventInternal}.  Considered protected; should only be used
        /// within this package and by subclasses.</summary>
        /// <param name="e">Key event to handle.</param>
        abstract handleKeyEvent: e: Event -> bool
        /// <summary>Attempts to handle a keyboard event; returns true if the event was handled,
        /// false otherwise.  If the container is enabled, and a child is highlighted,
        /// calls the child menuitem's `handleKeyEvent` method to give the menuitem
        /// a chance to handle the event first.</summary>
        /// <param name="e">Key event to handle.</param>
        abstract handleKeyEventInternal: e: Event -> bool

    type [<AllowNullLiteral>] Menu__ClassHighlightHelperFn =
        [<Emit "$0($1...)">] abstract Invoke: _0: float * _1: float -> float

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] Menu__ClassStatic =
        /// A basic menu class.
        [<Emit "new $0($1...)">] abstract Create: unit -> Menu__Class

    type [<AllowNullLiteral>] MenuItem =
        inherit MenuItem__Class

    type [<AllowNullLiteral>] MenuItemStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> MenuItem

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] MenuItem__Class =
        inherit Blockly.Component__Class
        abstract getCheckboxDom: unit -> Element
        abstract getContentDom: unit -> Element
        abstract getContentWrapperDom: unit -> Element
        /// <summary>Sets the content associated with the menu item.</summary>
        /// <param name="content">Text caption to set as the
        /// menuitem's contents.</param>
        abstract setContentInternal: content: string -> unit
        /// <summary>Sets the value associated with the menu item.</summary>
        /// <param name="value">Value to be associated with the menu item.</param>
        abstract setValue: value: obj option -> unit
        /// Gets the value associated with the menu item.
        abstract getValue: unit -> obj option
        /// <summary>Set the menu accessibility role.</summary>
        /// <param name="roleName">role name.</param>
        abstract setRole: roleName: U2<Blockly.Utils.Aria.Role, string> -> unit
        /// <summary>Sets the menu item to be checkable or not. Set to true for menu items
        /// that represent checkable options.</summary>
        /// <param name="checkable">Whether the menu item is checkable.</param>
        abstract setCheckable: checkable: bool -> unit
        /// <summary>Checks or unchecks the component.</summary>
        /// <param name="checked">Whether to check or uncheck the component.</param>
        abstract setChecked: ``checked``: bool -> unit
        /// <summary>Highlights or unhighlights the component.</summary>
        /// <param name="highlight">Whether to highlight or unhighlight the component.</param>
        abstract setHighlighted: highlight: bool -> unit
        /// Returns true if the menu item is enabled, false otherwise.
        abstract isEnabled: unit -> bool
        /// <summary>Enables or disables the menu item.</summary>
        /// <param name="enabled">Whether to enable or disable the menu item.</param>
        abstract setEnabled: enabled: bool -> unit
        /// <summary>Handles click events. If the component is enabled, trigger
        /// the action associated with this menu item.</summary>
        /// <param name="_e">Mouse event to handle.</param>
        abstract handleClick: _e: Event -> unit
        /// Performs the appropriate action when the menu item is activated
        /// by the user.
        abstract performActionInternal: unit -> unit
        /// <summary>Set the handler that's triggered when the menu item is activated
        /// by the user. If `opt_obj` is provided, it will be used as the
        /// 'this' object in the function when called.</summary>
        /// <param name="fn">The handler.</param>
        /// <param name="opt_obj">Used as the 'this' object in f when called.</param>
        abstract onAction: fn: MenuItem__ClassOnActionFn * ?opt_obj: 'T -> unit

    type [<AllowNullLiteral>] MenuItem__ClassOnActionFn =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.MenuItem -> obj option

    /// Fake class which should be extended to avoid inheriting static properties 
    type [<AllowNullLiteral>] MenuItem__ClassStatic =
        /// <summary>Class representing an item in a menu.</summary>
        /// <param name="content">Text caption to display as the content of
        /// the item.</param>
        /// <param name="opt_value">Data/model associated with the menu item.</param>
        [<Emit "new $0($1...)">] abstract Create: content: string * ?opt_value: string -> MenuItem__Class

    module Tree =
        let [<Import("BaseNode","blockly")>] baseNode: BaseNode.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            abstract BaseNode: BaseNodeStatic
            abstract BaseNode__Class: BaseNode__ClassStatic
            abstract TreeControl: TreeControlStatic
            abstract TreeControl__Class: TreeControl__ClassStatic
            abstract TreeNode: TreeNodeStatic
            abstract TreeNode__Class: TreeNode__ClassStatic

        type [<AllowNullLiteral>] BaseNode =
            inherit BaseNode__Class

        type [<AllowNullLiteral>] BaseNodeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BaseNode

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BaseNode__Class =
            inherit Blockly.Component__Class
            abstract tree: obj option with get, set
            /// Adds roles and states.
            abstract initAccessibility: unit -> unit
            /// <summary>Appends a node as a child to the current node.</summary>
            /// <param name="child">The child to add.</param>
            abstract add: child: Blockly.Tree.BaseNode -> unit
            /// Returns the tree.
            abstract getTree: unit -> Blockly.Tree.TreeControl
            /// Returns the depth of the node in the tree. Should not be overridden.
            abstract getDepth: unit -> float
            /// <summary>Returns true if the node is a descendant of this node</summary>
            /// <param name="node">The node to check.</param>
            abstract contains: node: Blockly.Tree.BaseNode -> bool
            /// <summary>This is re-defined here to indicate to the closure compiler the correct
            /// child return type.</summary>
            /// <param name="index">0-based index.</param>
            abstract getChildAt: index: float -> Blockly.Tree.BaseNode
            /// Returns the children of this node.
            abstract getChildren: unit -> ResizeArray<Blockly.Tree.BaseNode>
            abstract getFirstChild: unit -> Blockly.Tree.BaseNode
            abstract getLastChild: unit -> Blockly.Tree.BaseNode
            abstract getPreviousSibling: unit -> Blockly.Tree.BaseNode
            abstract getNextSibling: unit -> Blockly.Tree.BaseNode
            abstract isLastSibling: unit -> bool
            abstract isSelected: unit -> bool
            /// Selects the node.
            abstract select: unit -> unit
            /// Selects the first node.
            abstract selectFirst: unit -> unit
            /// <summary>Called from the tree to instruct the node change its selection state.</summary>
            /// <param name="selected">The new selection state.</param>
            abstract setSelectedInternal: selected: bool -> unit
            abstract getExpanded: unit -> bool
            /// <summary>Sets the node to be expanded internally, without state change events.</summary>
            /// <param name="expanded">Whether to expand or close the node.</param>
            abstract setExpandedInternal: expanded: bool -> unit
            /// <summary>Sets the node to be expanded.</summary>
            /// <param name="expanded">Whether to expand or close the node.</param>
            abstract setExpanded: expanded: bool -> unit
            /// Used to notify a node of that we have expanded it.
            /// Can be overidden by subclasses, see Blockly.tree.TreeNode.
            abstract doNodeExpanded: unit -> unit
            /// Used to notify a node that we have collapsed it.
            /// Can be overidden by subclasses, see Blockly.tree.TreeNode.
            abstract doNodeCollapsed: unit -> unit
            /// Toggles the expanded state of the node.
            abstract toggle: unit -> unit
            abstract isUserCollapsible: unit -> bool
            /// Creates HTML Element for the node.
            abstract toDom: unit -> Element
            abstract getRowDom: unit -> Element
            abstract getRowClassName: unit -> string
            abstract getLabelDom: unit -> Element
            abstract getIconDom: unit -> Element
            /// Gets the calculated icon class.
            abstract getCalculatedIconClass: unit -> unit
            abstract getBackgroundPosition: unit -> string
            abstract getRowElement: unit -> Element
            abstract getIconElement: unit -> Element
            abstract getLabelElement: unit -> Element
            abstract getChildrenElement: unit -> Element
            /// Gets the icon class for the node.
            abstract getIconClass: unit -> string
            /// Gets the icon class for when the node is expanded.
            abstract getExpandedIconClass: unit -> string
            /// <summary>Sets the text of the label.</summary>
            /// <param name="s">The plain text of the label.</param>
            abstract setText: s: string -> unit
            /// Returns the text of the label. If the text was originally set as HTML, the
            /// return value is unspecified.
            abstract getText: unit -> string
            /// Updates the row styles.
            abstract updateRow: unit -> unit
            /// Updates the expand icon of the node.
            abstract updateExpandIcon: unit -> unit
            /// <summary>Handles mouse down event.</summary>
            /// <param name="e">The browser event.</param>
            abstract onMouseDown: e: Event -> unit
            /// <summary>Handles a click event.</summary>
            /// <param name="e">The browser event.</param>
            abstract onClick_: e: Event -> unit
            /// <summary>Handles a key down event.</summary>
            /// <param name="e">The browser event.</param>
            abstract onKeyDown: e: Event -> bool
            /// Select the next node.
            abstract selectNext: unit -> bool
            /// Select the previous node.
            abstract selectPrevious: unit -> bool
            /// Select the parent node or collapse the current node.
            abstract selectParent: unit -> bool
            /// Expand the current node if it's not already expanded, or select the
            /// child node.
            abstract selectChild: unit -> bool
            abstract getLastShownDescendant: unit -> Blockly.Tree.BaseNode
            abstract getNextShownNode: unit -> Blockly.Tree.BaseNode
            abstract getPreviousShownNode: unit -> Blockly.Tree.BaseNode
            abstract getConfig: unit -> Blockly.Tree.BaseNode.Config
            /// <summary>Internal method that is used to set the tree control on the node.</summary>
            /// <param name="tree">The tree control.</param>
            abstract setTreeInternal: tree: Blockly.Tree.TreeControl -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BaseNode__ClassStatic =
            /// <summary>An abstract base class for a node in the tree.
            /// Similar to goog.ui.tree.BaseNode</summary>
            /// <param name="content">The content of the node label treated as
            /// plain-text and will be HTML escaped.</param>
            /// <param name="config">The configuration for the tree.</param>
            [<Emit "new $0($1...)">] abstract Create: content: string * config: Blockly.Tree.BaseNode.Config -> BaseNode__Class

        module BaseNode =

            type [<AllowNullLiteral>] IExports =
                abstract allNodes: Object

            /// The config type for the tree.
            type [<AllowNullLiteral>] Config =
                abstract indentWidth: float with get, set
                abstract cssRoot: string with get, set
                abstract cssHideRoot: string with get, set
                abstract cssTreeRow: string with get, set
                abstract cssItemLabel: string with get, set
                abstract cssTreeIcon: string with get, set
                abstract cssExpandedFolderIcon: string with get, set
                abstract cssCollapsedFolderIcon: string with get, set
                abstract cssFileIcon: string with get, set
                abstract cssSelectedRow: string with get, set

        type [<AllowNullLiteral>] TreeControl =
            inherit TreeControl__Class

        type [<AllowNullLiteral>] TreeControlStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> TreeControl

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeControl__Class =
            inherit Blockly.Tree.BaseNode__Class
            /// Returns the associated toolbox.
            abstract getToolbox: unit -> Blockly.Toolbox
            /// Get whether this tree has focus or not.
            abstract hasFocus: unit -> bool
            /// <summary>Sets the selected item.</summary>
            /// <param name="node">The item to select.</param>
            abstract setSelectedItem: node: Blockly.Tree.BaseNode -> unit
            /// <summary>Set the handler that's triggered before a node is selected.</summary>
            /// <param name="fn">The handler</param>
            abstract onBeforeSelected: fn: TreeControl__ClassOnBeforeSelectedFn -> unit
            /// <summary>Set the handler that's triggered after a node is selected.</summary>
            /// <param name="fn">The handler</param>
            abstract onAfterSelected: fn: TreeControl__ClassOnAfterSelectedFn -> unit
            /// Returns the selected item.
            abstract getSelectedItem: unit -> Blockly.Tree.BaseNode
            /// <summary>Creates a new tree node using the same config as the root.</summary>
            /// <param name="opt_content">The content of the node label.</param>
            abstract createNode: ?opt_content: string -> Blockly.Tree.TreeNode

        type [<AllowNullLiteral>] TreeControl__ClassOnBeforeSelectedFn =
            [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.Tree.BaseNode -> bool

        type [<AllowNullLiteral>] TreeControl__ClassOnAfterSelectedFn =
            [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.Tree.BaseNode * _1: Blockly.Tree.BaseNode -> obj option

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeControl__ClassStatic =
            /// <summary>An extension of the TreeControl object in closure that provides
            /// a way to view a hierarchical set of data.
            /// Similar to Closure's goog.ui.tree.TreeControl</summary>
            /// <param name="toolbox">The parent toolbox for this tree.</param>
            /// <param name="config">The configuration for the tree.</param>
            [<Emit "new $0($1...)">] abstract Create: toolbox: Blockly.Toolbox * config: Blockly.Tree.BaseNode.Config -> TreeControl__Class

        type [<AllowNullLiteral>] TreeNode =
            inherit TreeNode__Class

        type [<AllowNullLiteral>] TreeNodeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> TreeNode

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeNode__Class =
            inherit Blockly.Tree.BaseNode__Class
            /// <summary>Set the handler that's triggered when the size of node has changed.</summary>
            /// <param name="fn">The handler</param>
            abstract onSizeChanged: fn: TreeNode__ClassOnSizeChangedFn -> unit

        type [<AllowNullLiteral>] TreeNode__ClassOnSizeChangedFn =
            [<Emit "$0($1...)">] abstract Invoke: unit -> obj option

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TreeNode__ClassStatic =
            /// <summary>A single node in the tree, customized for Blockly's UI.
            /// Similar to Closure's goog.ui.tree.TreeNode</summary>
            /// <param name="toolbox">The parent toolbox for this tree.</param>
            /// <param name="content">The content of the node label treated as
            /// plain-text and will be HTML escaped.</param>
            /// <param name="config">The configuration for the tree.</param>
            [<Emit "new $0($1...)">] abstract Create: toolbox: Blockly.Toolbox * content: string * config: Blockly.Tree.BaseNode.Config -> TreeNode__Class

    module BlockRendering =
        let [<Import("Debug","blockly")>] debug: Debug.IExports = jsNative
        let [<Import("Types","blockly")>] types: Types = jsNative

        type [<AllowNullLiteral>] IExports =
            abstract useDebugger: bool
            /// <summary>Registers a new renderer.</summary>
            /// <param name="name">The name of the renderer.</param>
            /// <param name="rendererClass">The new renderer class
            /// to register.</param>
            abstract register: name: string * rendererClass: Function -> unit
            /// <summary>Unregisters the renderer registered with the given name.</summary>
            /// <param name="name">The name of the renderer.</param>
            abstract unregister: name: string -> unit
            /// Turn on the blocks debugger.
            abstract startDebugger: unit -> unit
            /// Turn off the blocks debugger.
            abstract stopDebugger: unit -> unit
            /// <summary>Initialize anything needed for rendering (constants, etc).</summary>
            /// <param name="name">Name of the renderer to initialize.</param>
            abstract init: name: string -> Blockly.BlockRendering.Renderer
            abstract ConstantProvider: ConstantProviderStatic
            abstract ConstantProvider__Class: ConstantProvider__ClassStatic
            abstract Debug: DebugStatic
            abstract Debug__Class: Debug__ClassStatic
            abstract Drawer: DrawerStatic
            abstract Drawer__Class: Drawer__ClassStatic
            abstract RenderInfo: RenderInfoStatic
            abstract RenderInfo__Class: RenderInfo__ClassStatic
            abstract PathObject: PathObjectStatic
            abstract PathObject__Class: PathObject__ClassStatic
            abstract Renderer: RendererStatic
            abstract Renderer__Class: Renderer__ClassStatic
            abstract Measurable: MeasurableStatic
            abstract Measurable__Class: Measurable__ClassStatic
            abstract Connection: ConnectionStatic
            abstract Connection__Class: Connection__ClassStatic
            abstract OutputConnection: OutputConnectionStatic
            abstract OutputConnection__Class: OutputConnection__ClassStatic
            abstract PreviousConnection: PreviousConnectionStatic
            abstract PreviousConnection__Class: PreviousConnection__ClassStatic
            abstract NextConnection: NextConnectionStatic
            abstract NextConnection__Class: NextConnection__ClassStatic
            abstract InputConnection: InputConnectionStatic
            abstract InputConnection__Class: InputConnection__ClassStatic
            abstract InlineInput: InlineInputStatic
            abstract InlineInput__Class: InlineInput__ClassStatic
            abstract StatementInput: StatementInputStatic
            abstract StatementInput__Class: StatementInput__ClassStatic
            abstract ExternalValueInput: ExternalValueInputStatic
            abstract ExternalValueInput__Class: ExternalValueInput__ClassStatic
            abstract Icon: IconStatic
            abstract Icon__Class: Icon__ClassStatic
            abstract JaggedEdge: JaggedEdgeStatic
            abstract JaggedEdge__Class: JaggedEdge__ClassStatic
            abstract Field: FieldStatic
            abstract Field__Class: Field__ClassStatic
            abstract Hat: HatStatic
            abstract Hat__Class: Hat__ClassStatic
            abstract SquareCorner: SquareCornerStatic
            abstract SquareCorner__Class: SquareCorner__ClassStatic
            abstract RoundCorner: RoundCornerStatic
            abstract RoundCorner__Class: RoundCorner__ClassStatic
            abstract InRowSpacer: InRowSpacerStatic
            abstract InRowSpacer__Class: InRowSpacer__ClassStatic
            abstract Row: RowStatic
            abstract Row__Class: Row__ClassStatic
            abstract TopRow: TopRowStatic
            abstract TopRow__Class: TopRow__ClassStatic
            abstract BottomRow: BottomRowStatic
            abstract BottomRow__Class: BottomRow__ClassStatic
            abstract SpacerRow: SpacerRowStatic
            abstract SpacerRow__Class: SpacerRow__ClassStatic
            abstract InputRow: InputRowStatic
            abstract InputRow__Class: InputRow__ClassStatic

        type [<AllowNullLiteral>] ConstantProvider =
            inherit ConstantProvider__Class

        type [<AllowNullLiteral>] ConstantProviderStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ConstantProvider

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] ConstantProvider__Class =
            /// Rounded corner radius.
            abstract CORNER_RADIUS: float with get, set
            /// The height of an empty statement input.  Note that in the old rendering this
            /// varies slightly depending on whether the block has external or inline inputs.
            /// In the new rendering this is consistent.  It seems unlikely that the old
            /// behaviour was intentional.
            abstract EMPTY_STATEMENT_INPUT_HEIGHT: float with get, set
            /// Height of SVG path for jagged teeth at the end of collapsed blocks.
            abstract JAGGED_TEETH_HEIGHT: obj option with get, set
            /// Width of SVG path for jagged teeth at the end of collapsed blocks.
            abstract JAGGED_TEETH_WIDTH: obj option with get, set
            /// Initialize shape objects based on the constants set in the constructor.
            abstract init: unit -> unit
            /// An object containing sizing and path information about collapsed block
            /// indicators.
            abstract JAGGED_TEETH: Object with get, set
            /// An object containing sizing and path information about notches.
            abstract NOTCH: Object with get, set
            /// An object containing sizing and path information about start hats
            abstract START_HAT: Object with get, set
            /// An object containing sizing and path information about puzzle tabs.
            abstract PUZZLE_TAB: Object with get, set
            /// An object containing sizing and path information about inside corners
            abstract INSIDE_CORNERS: Object with get, set
            /// An object containing sizing and path information about outside corners.
            abstract OUTSIDE_CORNERS: Object with get, set
            abstract makeJaggedTeeth: unit -> Object
            abstract makeStartHat: unit -> Object
            abstract makePuzzleTab: unit -> Object
            abstract makeNotch: unit -> Object
            abstract makeInsideCorners: unit -> Object
            abstract makeOutsideCorners: unit -> Object
            /// <summary>Get an object with connection shape and sizing information based on the type
            /// of the connection.</summary>
            /// <param name="connection">The connection to find a
            /// shape object for</param>
            abstract shapeFor: connection: Blockly.RenderedConnection -> Object

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] ConstantProvider__ClassStatic =
            /// An object that provides constants for rendering blocks.
            [<Emit "new $0($1...)">] abstract Create: unit -> ConstantProvider__Class

        type [<AllowNullLiteral>] Debug =
            inherit Debug__Class

        type [<AllowNullLiteral>] DebugStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Debug

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Debug__Class =
            /// An array of SVG elements that have been created by this object.
            abstract debugElements_: ResizeArray<SVGElement> with get, set
            /// The SVG root of the block that is being rendered.  Debug elements will
            /// be attached to this root.
            abstract svgRoot_: SVGElement with get, set
            /// Remove all elements the this object created on the last pass.
            abstract clearElems: unit -> unit
            /// <summary>Draw a debug rectangle for a spacer (empty) row.</summary>
            /// <param name="row">The row to render.</param>
            /// <param name="cursorY">The y position of the top of the row.</param>
            /// <param name="isRtl">Whether the block is rendered RTL.</param>
            abstract drawSpacerRow: row: Blockly.BlockRendering.Row * cursorY: float * isRtl: bool -> unit
            /// <summary>Draw a debug rectangle for a horizontal spacer.</summary>
            /// <param name="elem">The spacer to render.</param>
            /// <param name="rowHeight">The height of the container row.</param>
            /// <param name="isRtl">Whether the block is rendered RTL.</param>
            abstract drawSpacerElem: elem: Blockly.BlockRendering.InRowSpacer * rowHeight: float * isRtl: bool -> unit
            /// <summary>Draw a debug rectangle for an in-row element.</summary>
            /// <param name="elem">The element to render.</param>
            /// <param name="isRtl">Whether the block is rendered RTL.</param>
            abstract drawRenderedElem: elem: Blockly.BlockRendering.Measurable * isRtl: bool -> unit
            /// <summary>Draw a circle at the location of the given connection.  Inputs and outputs
            /// share the same colors, as do previous and next.  When positioned correctly
            /// a connected pair will look like a bullseye.</summary>
            /// <param name="conn">The connection to circle.</param>
            abstract drawConnection: conn: Blockly.RenderedConnection -> unit
            /// <summary>Draw a debug rectangle for a non-empty row.</summary>
            /// <param name="row">The non-empty row to render.</param>
            /// <param name="cursorY">The y position of the top of the row.</param>
            /// <param name="isRtl">Whether the block is rendered RTL.</param>
            abstract drawRenderedRow: row: Blockly.BlockRendering.Row * cursorY: float * isRtl: bool -> unit
            /// <summary>Draw debug rectangles for a non-empty row and all of its subcomponents.</summary>
            /// <param name="row">The non-empty row to render.</param>
            /// <param name="cursorY">The y position of the top of the row.</param>
            /// <param name="isRtl">Whether the block is rendered RTL.</param>
            abstract drawRowWithElements: row: Blockly.BlockRendering.Row * cursorY: float * isRtl: bool -> unit
            /// <summary>Draw a debug rectangle around the entire block.</summary>
            /// <param name="info">Rendering information about
            /// the block to debug.</param>
            abstract drawBoundingBox: info: Blockly.BlockRendering.RenderInfo -> unit
            /// <summary>Do all of the work to draw debug information for the whole block.</summary>
            /// <param name="block">The block to draw debug information for.</param>
            /// <param name="info">Rendering information about
            /// the block to debug.</param>
            abstract drawDebug: block: Blockly.BlockSvg * info: Blockly.BlockRendering.RenderInfo -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Debug__ClassStatic =
            /// An object that renders rectangles and dots for debugging rendering code.
            [<Emit "new $0($1...)">] abstract Create: unit -> Debug__Class

        module Debug =

            type [<AllowNullLiteral>] IExports =
                abstract config: TypeLiteral_04

            type [<AllowNullLiteral>] TypeLiteral_04 =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> bool with get, set

        type [<AllowNullLiteral>] Drawer =
            inherit Drawer__Class

        type [<AllowNullLiteral>] DrawerStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Drawer

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Drawer__Class =
            /// The renderer's constant provider.
            abstract constants_: Blockly.BlockRendering.ConstantProvider with get, set
            /// Draw the block to the workspace. Here "drawing" means setting SVG path
            /// elements and moving fields, icons, and connections on the screen.
            /// 
            /// The pieces of the paths are pushed into arrays of "steps", which are then
            /// joined with spaces and set directly on the block.  This guarantees that
            /// the steps are separated by spaces for improved readability, but isn't
            /// required.
            abstract draw: unit -> unit
            /// Save sizing information back to the block
            /// Most of the rendering information can be thrown away at the end of the
            /// render. Anything that needs to be kept around should be set in this function.
            abstract recordSizeOnBlock_: unit -> unit
            /// Hide icons that were marked as hidden.
            abstract hideHiddenIcons_: unit -> unit
            /// Create the outline of the block.  This is a single continuous path.
            abstract drawOutline_: unit -> unit
            /// Add steps for the top corner of the block, taking into account
            /// details such as hats and rounded corners.
            abstract drawTop_: unit -> unit
            /// <summary>Add steps for the jagged edge of a row on a collapsed block.</summary>
            /// <param name="row">The row to draw the side of.</param>
            abstract drawJaggedEdge_: row: Blockly.BlockRendering.Row -> unit
            /// <summary>Add steps for an external value input, rendered as a notch in the side
            /// of the block.</summary>
            /// <param name="row">The row that this input
            /// belongs to.</param>
            abstract drawValueInput_: row: Blockly.BlockRendering.Row -> unit
            /// <summary>Add steps for a statement input.</summary>
            /// <param name="row">The row that this input
            /// belongs to.</param>
            abstract drawStatementInput_: row: Blockly.BlockRendering.Row -> unit
            /// <summary>Add steps for the right side of a row that does not have value or
            /// statement input connections.</summary>
            /// <param name="row">The row to draw the
            /// side of.</param>
            abstract drawRightSideRow_: row: Blockly.BlockRendering.Row -> unit
            /// Add steps for the bottom edge of a block, possibly including a notch
            /// for the next connection
            abstract drawBottom_: unit -> unit
            /// Add steps for the left side of the block, which may include an output
            /// connection
            abstract drawLeft_: unit -> unit
            /// Draw the internals of the block: inline inputs, fields, and icons.  These do
            /// not depend on the outer path for placement.
            abstract drawInternals_: unit -> unit
            /// <summary>Push a field or icon's new position to its SVG root.</summary>
            /// <param name="fieldInfo">The rendering information for the field or icon.</param>
            abstract layoutField_: fieldInfo: U2<Blockly.BlockRendering.Icon, Blockly.BlockRendering.Field> -> unit
            /// <summary>Add steps for an inline input.</summary>
            /// <param name="input">The information about the
            /// input to render.</param>
            abstract drawInlineInput_: input: Blockly.BlockRendering.InlineInput -> unit
            /// <summary>Position the connection on an inline value input, taking into account
            /// RTL and the small gap between the parent block and child block which lets the
            /// parent block's dark path show through.</summary>
            /// <param name="input">The information about
            /// the input that the connection is on.</param>
            abstract positionInlineInputConnection_: input: Blockly.BlockRendering.InlineInput -> unit
            /// <summary>Position the connection on a statement input, taking into account
            /// RTL and the small gap between the parent block and child block which lets the
            /// parent block's dark path show through.</summary>
            /// <param name="row">The row that the connection is on.</param>
            abstract positionStatementInputConnection_: row: Blockly.BlockRendering.Row -> unit
            /// <summary>Position the connection on an external value input, taking into account
            /// RTL and the small gap between the parent block and child block which lets the
            /// parent block's dark path show through.</summary>
            /// <param name="row">The row that the connection is on.</param>
            abstract positionExternalValueConnection_: row: Blockly.BlockRendering.Row -> unit
            /// Position the previous connection on a block.
            abstract positionPreviousConnection_: unit -> unit
            /// Position the next connection on a block.
            abstract positionNextConnection_: unit -> unit
            /// Position the output connection on a block.
            abstract positionOutputConnection_: unit -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Drawer__ClassStatic =
            /// <summary>An object that draws a block based on the given rendering information.</summary>
            /// <param name="block">The block to render.</param>
            /// <param name="info">An object containing all
            /// information needed to render this block.</param>
            [<Emit "new $0($1...)">] abstract Create: block: Blockly.BlockSvg * info: Blockly.BlockRendering.RenderInfo -> Drawer__Class

        type [<AllowNullLiteral>] RenderInfo =
            inherit RenderInfo__Class

        type [<AllowNullLiteral>] RenderInfoStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> RenderInfo

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] RenderInfo__Class =
            /// The block renderer in use.
            abstract renderer_: Blockly.BlockRendering.Renderer with get, set
            /// The renderer's constant provider.
            abstract constants_: Blockly.BlockRendering.ConstantProvider with get, set
            /// A measurable representing the output connection if the block has one.
            /// Otherwise null.
            abstract outputConnection: Blockly.BlockRendering.OutputConnection with get, set
            /// Whether the block should be rendered as a single line, either because it's
            /// inline or because it has been collapsed.
            abstract isInline: bool with get, set
            /// Whether the block is collapsed.
            abstract isCollapsed: bool with get, set
            /// Whether the block is an insertion marker.  Insertion markers are the same
            /// shape as normal blocks, but don't show fields.
            abstract isInsertionMarker: bool with get, set
            /// True if the block should be rendered right-to-left.
            abstract RTL: bool with get, set
            /// The height of the rendered block, including child blocks.
            abstract height: float with get, set
            /// The width of the rendered block, including child blocks.
            abstract widthWithChildren: float with get, set
            /// The width of the rendered block, excluding child blocks.  This is the right
            /// edge of the block when rendered LTR.
            abstract width: float with get, set
            abstract statementEdge: float with get, set
            /// An array of Row objects containing sizing information.
            abstract rows: ResizeArray<Blockly.BlockRendering.Row> with get, set
            /// An array of measurable objects containing hidden icons.
            abstract hiddenIcons: ResizeArray<Blockly.BlockRendering.Icon> with get, set
            /// An object with rendering information about the top row of the block.
            abstract topRow: Blockly.BlockRendering.TopRow with get, set
            /// An object with rendering information about the bottom row of the block.
            abstract bottomRow: Blockly.BlockRendering.BottomRow with get, set
            /// Get the block renderer in use.
            abstract getRenderer: unit -> Blockly.BlockRendering.Renderer
            /// Populate and return an object containing all sizing information needed to
            /// draw this block.
            /// 
            /// This measure pass does not propagate changes to the block (although fields
            /// may choose to rerender when getSize() is called).  However, calling it
            /// repeatedly may be expensive.
            abstract ``measure``: unit -> unit
            /// Create rows of Measurable objects representing all renderable parts of the
            /// block.
            abstract createRows_: unit -> unit
            /// Create all non-spacer elements that belong on the top row.
            abstract populateTopRow_: unit -> unit
            /// Create all non-spacer elements that belong on the bottom row.
            abstract populateBottomRow_: unit -> unit
            /// <summary>Add an input element to the active row, if needed, and record the type of the
            /// input on the row.</summary>
            /// <param name="input">The input to record information about.</param>
            /// <param name="activeRow">The row that is currently being
            /// populated.</param>
            abstract addInput_: input: Blockly.Input * activeRow: Blockly.BlockRendering.Row -> unit
            /// <summary>Decide whether to start a new row between the two Blockly.Inputs.</summary>
            /// <param name="input">The first input to consider</param>
            /// <param name="lastInput">The input that follows.</param>
            abstract shouldStartNewRow_: input: Blockly.Input * lastInput: Blockly.Input -> bool
            /// Add horizontal spacing between and around elements within each row.
            abstract addElemSpacing_: unit -> unit
            /// <summary>Calculate the width of a spacer element in a row based on the previous and
            /// next elements in that row.  For instance, extra padding is added between two
            /// editable fields.</summary>
            /// <param name="prev">The element before the
            /// spacer.</param>
            /// <param name="next">The element after the spacer.</param>
            abstract getInRowSpacing_: prev: Blockly.BlockRendering.Measurable * next: Blockly.BlockRendering.Measurable -> float
            /// Figure out where the right edge of the block and right edge of statement inputs
            /// should be placed.
            /// TODO: More cleanup.
            abstract computeBounds_: unit -> unit
            /// Extra spacing may be necessary to make sure that the right sides of all
            /// rows line up.  This can only be calculated after a first pass to calculate
            /// the sizes of all rows.
            abstract alignRowElements_: unit -> unit
            /// <summary>Modify the given row to add the given amount of padding around its fields.
            /// The exact location of the padding is based on the alignment property of the
            /// last input in the field.</summary>
            /// <param name="row">The row to add padding to.</param>
            /// <param name="missingSpace">How much padding to add.</param>
            abstract addAlignmentPadding_: row: Blockly.BlockRendering.Row * missingSpace: float -> unit
            /// <summary>Align the elements of a statement row based on computed bounds.
            /// Unlike other types of rows, statement rows add space in multiple places.</summary>
            /// <param name="row">The statement row to resize.</param>
            abstract alignStatementRow_: row: Blockly.BlockRendering.InputRow -> unit
            /// Add spacers between rows and set their sizes.
            abstract addRowSpacing_: unit -> unit
            /// <summary>Create a spacer row to go between prev and next, and set its size.</summary>
            /// <param name="prev">The previous row.</param>
            /// <param name="next">The next row.</param>
            abstract makeSpacerRow_: prev: Blockly.BlockRendering.Row * next: Blockly.BlockRendering.Row -> Blockly.BlockRendering.SpacerRow
            /// <summary>Calculate the width of a spacer row.</summary>
            /// <param name="_prev">The row before the spacer.</param>
            /// <param name="_next">The row after the spacer.</param>
            abstract getSpacerRowWidth_: _prev: Blockly.BlockRendering.Row * _next: Blockly.BlockRendering.Row -> float
            /// <summary>Calculate the height of a spacer row.</summary>
            /// <param name="_prev">The row before the spacer.</param>
            /// <param name="_next">The row after the spacer.</param>
            abstract getSpacerRowHeight_: _prev: Blockly.BlockRendering.Row * _next: Blockly.BlockRendering.Row -> float
            /// <summary>Calculate the centerline of an element in a rendered row.
            /// This base implementation puts the centerline at the middle of the row
            /// vertically, with no special cases.  You will likely need extra logic to
            /// handle (at minimum) top and bottom rows.</summary>
            /// <param name="row">The row containing the element.</param>
            /// <param name="elem">The element to place.</param>
            abstract getElemCenterline_: row: Blockly.BlockRendering.Row * elem: Blockly.BlockRendering.Measurable -> float
            /// Make any final changes to the rendering information object.  In particular,
            /// store the y position of each row, and record the height of the full block.
            abstract finalize_: unit -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] RenderInfo__ClassStatic =
            /// <summary>An object containing all sizing information needed to draw this block.
            /// 
            /// This measure pass does not propagate changes to the block (although fields
            /// may choose to rerender when getSize() is called).  However, calling it
            /// repeatedly may be expensive.</summary>
            /// <param name="renderer">The renderer in use.</param>
            /// <param name="block">The block to measure.</param>
            [<Emit "new $0($1...)">] abstract Create: renderer: Blockly.BlockRendering.Renderer * block: Blockly.BlockSvg -> RenderInfo__Class

        type [<AllowNullLiteral>] IPathObject =
            interface end

        type [<AllowNullLiteral>] PathObject =
            inherit PathObject__Class

        type [<AllowNullLiteral>] PathObjectStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> PathObject

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] PathObject__Class =
            inherit Blockly.BlockRendering.IPathObject
            /// The primary path of the block.
            abstract svgPath: SVGElement with get, set
            /// The light path of the block.
            abstract svgPathLight: SVGElement with get, set
            /// The dark path of the block.
            abstract svgPathDark: SVGElement with get, set
            /// <summary>Set the path generated by the renderer onto the respective SVG element.</summary>
            /// <param name="pathString">The path.</param>
            abstract setPaths: pathString: string -> unit
            /// Flip the SVG paths in RTL.
            abstract flipRTL: unit -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] PathObject__ClassStatic =
            /// <summary>An object that handles creating and setting each of the SVG elements
            /// used by the renderer.</summary>
            /// <param name="root">The root SVG element.</param>
            [<Emit "new $0($1...)">] abstract Create: root: SVGElement -> PathObject__Class

        type [<AllowNullLiteral>] Renderer =
            inherit Renderer__Class

        type [<AllowNullLiteral>] RendererStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Renderer

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Renderer__Class =
            /// Initialize the renderer.
            abstract init: unit -> unit
            /// Create a new instance of the renderer's constant provider.
            abstract makeConstants_: unit -> Blockly.BlockRendering.ConstantProvider
            /// <summary>Create a new instance of the renderer's render info object.</summary>
            /// <param name="block">The block to measure.</param>
            abstract makeRenderInfo_: block: Blockly.BlockSvg -> Blockly.BlockRendering.RenderInfo
            /// <summary>Create a new instance of the renderer's drawer.</summary>
            /// <param name="block">The block to render.</param>
            /// <param name="info">An object containing all
            /// information needed to render this block.</param>
            abstract makeDrawer_: block: Blockly.BlockSvg * info: Blockly.BlockRendering.RenderInfo -> Blockly.BlockRendering.Drawer
            /// Create a new instance of the renderer's debugger.
            abstract makeDebugger_: unit -> Blockly.BlockRendering.Debug
            /// <summary>Create a new instance of the renderer's cursor drawer.</summary>
            /// <param name="workspace">The workspace the cursor belongs to.</param>
            /// <param name="opt_marker">True if the cursor is a marker. A marker is used
            /// to save a location and is an immovable cursor. False or undefined if the
            /// cursor is not a marker.</param>
            abstract makeCursorDrawer: workspace: Blockly.WorkspaceSvg * ?opt_marker: bool -> Blockly.CursorSvg
            /// <summary>Create a new instance of a renderer path object.</summary>
            /// <param name="root">The root SVG element.</param>
            abstract makePathObject: root: SVGElement -> Blockly.BlockRendering.IPathObject
            /// Get the current renderer's constant provider.  We assume that when this is
            /// called, the renderer has already been initialized.
            abstract getConstants: unit -> Blockly.BlockRendering.ConstantProvider
            /// <summary>Render the block.</summary>
            /// <param name="block">The block to render.</param>
            abstract render: block: Blockly.BlockSvg -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Renderer__ClassStatic =
            /// The base class for a block renderer.
            [<Emit "new $0($1...)">] abstract Create: unit -> Renderer__Class

        type [<AllowNullLiteral>] Measurable =
            inherit Measurable__Class

        type [<AllowNullLiteral>] MeasurableStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Measurable

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Measurable__Class =
            /// The renderer's constant provider.
            abstract constants_: Blockly.BlockRendering.ConstantProvider with get, set

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Measurable__ClassStatic =
            /// <summary>The base class to represent a part of a block that takes up space during
            /// rendering.  The constructor for each non-spacer Measurable records the size
            /// of the block element (e.g. field, statement input).</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> Measurable__Class

        type [<AllowNullLiteral>] Connection =
            inherit Connection__Class

        type [<AllowNullLiteral>] ConnectionStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Connection

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Connection__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Connection__ClassStatic =
            /// <summary>The base class to represent a connection and the space that it takes up on
            /// the block.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="connectionModel">The connection object on
            /// the block that this represents.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * connectionModel: Blockly.RenderedConnection -> Connection__Class

        type [<AllowNullLiteral>] OutputConnection =
            inherit OutputConnection__Class

        type [<AllowNullLiteral>] OutputConnectionStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> OutputConnection

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] OutputConnection__Class =
            inherit Blockly.BlockRendering.Connection__Class
            /// Whether or not the connection shape is dynamic. Dynamic shapes get their
            /// height from the block.
            abstract isDynamic: unit -> bool

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] OutputConnection__ClassStatic =
            /// <summary>An object containing information about the space an output connection takes
            /// up during rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="connectionModel">The connection object on
            /// the block that this represents.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * connectionModel: Blockly.RenderedConnection -> OutputConnection__Class

        type [<AllowNullLiteral>] PreviousConnection =
            inherit PreviousConnection__Class

        type [<AllowNullLiteral>] PreviousConnectionStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> PreviousConnection

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] PreviousConnection__Class =
            inherit Blockly.BlockRendering.Connection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] PreviousConnection__ClassStatic =
            /// <summary>An object containing information about the space a previous connection takes
            /// up during rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="connectionModel">The connection object on
            /// the block that this represents.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * connectionModel: Blockly.RenderedConnection -> PreviousConnection__Class

        type [<AllowNullLiteral>] NextConnection =
            inherit NextConnection__Class

        type [<AllowNullLiteral>] NextConnectionStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> NextConnection

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] NextConnection__Class =
            inherit Blockly.BlockRendering.Connection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] NextConnection__ClassStatic =
            /// <summary>An object containing information about the space a next connection takes
            /// up during rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="connectionModel">The connection object on
            /// the block that this represents.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * connectionModel: Blockly.RenderedConnection -> NextConnection__Class

        type [<AllowNullLiteral>] InputConnection =
            inherit InputConnection__Class

        type [<AllowNullLiteral>] InputConnectionStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> InputConnection

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InputConnection__Class =
            inherit Blockly.BlockRendering.Connection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InputConnection__ClassStatic =
            /// <summary>The base class to represent an input that takes up space on a block
            /// during rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="input">The input to measure and store information for.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * input: Blockly.Input -> InputConnection__Class

        type [<AllowNullLiteral>] InlineInput =
            inherit InlineInput__Class

        type [<AllowNullLiteral>] InlineInputStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> InlineInput

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InlineInput__Class =
            inherit Blockly.BlockRendering.InputConnection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InlineInput__ClassStatic =
            /// <summary>An object containing information about the space an inline input takes up
            /// during rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="input">The inline input to measure and store
            /// information for.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * input: Blockly.Input -> InlineInput__Class

        type [<AllowNullLiteral>] StatementInput =
            inherit StatementInput__Class

        type [<AllowNullLiteral>] StatementInputStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> StatementInput

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] StatementInput__Class =
            inherit Blockly.BlockRendering.InputConnection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] StatementInput__ClassStatic =
            /// <summary>An object containing information about the space a statement input takes up
            /// during rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="input">The statement input to measure and store
            /// information for.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * input: Blockly.Input -> StatementInput__Class

        type [<AllowNullLiteral>] ExternalValueInput =
            inherit ExternalValueInput__Class

        type [<AllowNullLiteral>] ExternalValueInputStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ExternalValueInput

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] ExternalValueInput__Class =
            inherit Blockly.BlockRendering.InputConnection__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] ExternalValueInput__ClassStatic =
            /// <summary>An object containing information about the space an external value input
            /// takes up during rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="input">The external value input to measure and store
            /// information for.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * input: Blockly.Input -> ExternalValueInput__Class

        type [<AllowNullLiteral>] Icon =
            inherit Icon__Class

        type [<AllowNullLiteral>] IconStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Icon

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Icon__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Icon__ClassStatic =
            /// <summary>An object containing information about the space an icon takes up during
            /// rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="icon">The icon to measure and store information for.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * icon: Blockly.Icon -> Icon__Class

        type [<AllowNullLiteral>] JaggedEdge =
            inherit JaggedEdge__Class

        type [<AllowNullLiteral>] JaggedEdgeStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> JaggedEdge

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] JaggedEdge__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] JaggedEdge__ClassStatic =
            /// <summary>An object containing information about the jagged edge of a collapsed block
            /// takes up during rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> JaggedEdge__Class

        type [<AllowNullLiteral>] Field =
            inherit Field__Class

        type [<AllowNullLiteral>] FieldStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Field

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Field__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Field__ClassStatic =
            /// <summary>An object containing information about the space a field takes up during
            /// rendering</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="field">The field to measure and store information for.</param>
            /// <param name="parentInput">The parent input for the field.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * field: Blockly.Field * parentInput: Blockly.Input -> Field__Class

        type [<AllowNullLiteral>] Hat =
            inherit Hat__Class

        type [<AllowNullLiteral>] HatStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Hat

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Hat__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Hat__ClassStatic =
            /// <summary>An object containing information about the space a hat takes up during
            /// rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> Hat__Class

        type [<AllowNullLiteral>] SquareCorner =
            inherit SquareCorner__Class

        type [<AllowNullLiteral>] SquareCornerStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> SquareCorner

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] SquareCorner__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] SquareCorner__ClassStatic =
            /// <summary>An object containing information about the space a square corner takes up
            /// during rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="opt_position">The position of this corner.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * ?opt_position: string -> SquareCorner__Class

        type [<AllowNullLiteral>] RoundCorner =
            inherit RoundCorner__Class

        type [<AllowNullLiteral>] RoundCornerStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> RoundCorner

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] RoundCorner__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] RoundCorner__ClassStatic =
            /// <summary>An object containing information about the space a rounded corner takes up
            /// during rendering.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="opt_position">The position of this corner.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * ?opt_position: string -> RoundCorner__Class

        type [<AllowNullLiteral>] InRowSpacer =
            inherit InRowSpacer__Class

        type [<AllowNullLiteral>] InRowSpacerStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> InRowSpacer

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InRowSpacer__Class =
            inherit Blockly.BlockRendering.Measurable__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InRowSpacer__ClassStatic =
            /// <summary>An object containing information about a spacer between two elements on a
            /// row.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="width">The width of the spacer.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * width: float -> InRowSpacer__Class

        type [<AllowNullLiteral>] Row =
            inherit Row__Class

        type [<AllowNullLiteral>] RowStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Row

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Row__Class =
            /// The type of this rendering object.
            abstract ``type``: float with get, set
            /// An array of elements contained in this row.
            abstract elements: ResizeArray<Blockly.BlockRendering.Measurable> with get, set
            /// The height of the row.
            abstract height: float with get, set
            /// The width of the row, from the left edge of the block to the right.
            /// Does not include child blocks unless they are inline.
            abstract width: float with get, set
            /// The minimum height of the row.
            abstract minHeight: float with get, set
            /// The minimum width of the row, from the left edge of the block to the right.
            /// Does not include child blocks unless they are inline.
            abstract minWidth: float with get, set
            /// The width of the row, from the left edge of the block to the edge of the
            /// block or any connected child blocks.
            abstract widthWithConnectedBlocks: float with get, set
            /// The Y position of the row relative to the origin of the block's svg group.
            abstract yPos: float with get, set
            /// The X position of the row relative to the origin of the block's svg group.
            abstract xPos: float with get, set
            /// Whether the row has any external inputs.
            abstract hasExternalInput: bool with get, set
            /// Whether the row has any statement inputs.
            abstract hasStatement: bool with get, set
            /// Whether the row has any inline inputs.
            abstract hasInlineInput: bool with get, set
            /// Whether the row has any dummy inputs.
            abstract hasDummyInput: bool with get, set
            /// Whether the row has a jagged edge.
            abstract hasJaggedEdge: bool with get, set
            /// The renderer's constant provider.
            abstract constants_: Blockly.BlockRendering.ConstantProvider with get, set
            /// Inspect all subcomponents and populate all size properties on the row.
            abstract ``measure``: unit -> unit
            /// Get the last input on this row, if it has one.
            /// TODO: Consider moving this to InputRow, if possible.
            abstract getLastInput: unit -> Blockly.BlockRendering.InputConnection
            /// Determines whether this row should start with an element spacer.
            abstract startsWithElemSpacer: unit -> bool
            /// Determines whether this row should end with an element spacer.
            abstract endsWithElemSpacer: unit -> bool
            /// Convenience method to get the first spacer element on this row.
            abstract getFirstSpacer: unit -> Blockly.BlockRendering.InRowSpacer
            /// Convenience method to get the last spacer element on this row.
            abstract getLastSpacer: unit -> Blockly.BlockRendering.InRowSpacer

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] Row__ClassStatic =
            /// <summary>An object representing a single row on a rendered block and all of its
            /// subcomponents.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> Row__Class

        type [<AllowNullLiteral>] TopRow =
            inherit TopRow__Class

        type [<AllowNullLiteral>] TopRowStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> TopRow

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TopRow__Class =
            inherit Blockly.BlockRendering.Row__Class
            /// The starting point for drawing the row, in the y direction.
            /// This allows us to draw hats and similar shapes that don't start at the
            /// origin. Must be non-negative (see #2820).
            abstract capline: float with get, set
            /// How much the row extends up above its capline.
            abstract ascenderHeight: float with get, set
            /// Whether the block has a previous connection.
            abstract hasPreviousConnection: bool with get, set
            /// The previous connection on the block, if any.
            abstract connection: Blockly.BlockRendering.PreviousConnection with get, set
            /// <summary>Returns whether or not the top row has a left square corner.</summary>
            /// <param name="block">The block whose top row this represents.</param>
            abstract hasLeftSquareCorner: block: Blockly.BlockSvg -> bool

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] TopRow__ClassStatic =
            /// <summary>An object containing information about what elements are in the top row of a
            /// block as well as sizing information for the top row.
            /// Elements in a top row can consist of corners, hats, spacers, and previous
            /// connections.
            /// After this constructor is called, the row will contain all non-spacer
            /// elements it needs.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> TopRow__Class

        type [<AllowNullLiteral>] BottomRow =
            inherit BottomRow__Class

        type [<AllowNullLiteral>] BottomRowStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> BottomRow

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BottomRow__Class =
            inherit Blockly.BlockRendering.Row__Class
            /// Whether this row has a next connection.
            abstract hasNextConnection: bool with get, set
            /// The next connection on the row, if any.
            abstract connection: Blockly.BlockRendering.NextConnection with get, set
            /// The amount that the bottom of the block extends below the horizontal edge,
            /// e.g. because of a next connection.  Must be non-negative (see #2820).
            abstract descenderHeight: float with get, set
            /// The Y position of the bottom edge of the block, relative to the origin
            /// of the block rendering.
            abstract baseline: float with get, set
            /// <summary>Returns whether or not the bottom row has a left square corner.</summary>
            /// <param name="block">The block whose bottom row this represents.</param>
            abstract hasLeftSquareCorner: block: Blockly.BlockSvg -> bool

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] BottomRow__ClassStatic =
            /// <summary>An object containing information about what elements are in the bottom row of
            /// a block as well as spacing information for the top row.
            /// Elements in a bottom row can consist of corners, spacers and next
            /// connections.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> BottomRow__Class

        type [<AllowNullLiteral>] SpacerRow =
            inherit SpacerRow__Class

        type [<AllowNullLiteral>] SpacerRowStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> SpacerRow

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] SpacerRow__Class =
            inherit Blockly.BlockRendering.Row__Class

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] SpacerRow__ClassStatic =
            /// <summary>An object containing information about a spacer between two rows.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            /// <param name="height">The height of the spacer.</param>
            /// <param name="width">The width of the spacer.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider * height: float * width: float -> SpacerRow__Class

        type [<AllowNullLiteral>] InputRow =
            inherit InputRow__Class

        type [<AllowNullLiteral>] InputRowStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> InputRow

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InputRow__Class =
            inherit Blockly.BlockRendering.Row__Class
            /// The total width of all blocks connected to this row.
            abstract connectedBlockWidths: float with get, set
            /// Inspect all subcomponents and populate all size properties on the row.
            abstract ``measure``: unit -> unit

        /// Fake class which should be extended to avoid inheriting static properties 
        type [<AllowNullLiteral>] InputRow__ClassStatic =
            /// <summary>An object containing information about a row that holds one or more inputs.</summary>
            /// <param name="constants">The rendering
            /// constants provider.</param>
            [<Emit "new $0($1...)">] abstract Create: constants: Blockly.BlockRendering.ConstantProvider -> InputRow__Class

        type Types =
            obj

        module Types =

            type [<AllowNullLiteral>] IExports =
                abstract LEFT_CORNER: float
                abstract RIGHT_CORNER: float
                /// <summary>Get the enum flag value of an existing type or register a new type.</summary>
                /// <param name="type">The name of the type.</param>
                abstract getType: ``type``: string -> float
                /// <summary>Whether a measurable stores information about a field.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isField: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a hat.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isHat: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about an icon.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isIcon: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a spacer.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isSpacer: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about an in-row spacer.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isInRowSpacer: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about an input.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isInput: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about an external input.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isExternalInput: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about an inline input.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isInlineInput: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a statement input.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isStatementInput: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a previous connection.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isPreviousConnection: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a next connection.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isNextConnection: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a previous or next connection.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isPreviousOrNextConnection: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a left round corner.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isLeftRoundedCorner: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a right round corner.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isRightRoundedCorner: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a left square corner.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isLeftSquareCorner: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a right square corner.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isRightSquareCorner: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a corner.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isCorner: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a jagged edge.</summary>
                /// <param name="elem">The element to check.</param>
                abstract isJaggedEdge: elem: Blockly.BlockRendering.Measurable -> float
                /// <summary>Whether a measurable stores information about a row.</summary>
                /// <param name="row">The row to check.</param>
                abstract isRow: row: Blockly.BlockRendering.Row -> float
                /// <summary>Whether a measurable stores information about a between-row spacer.</summary>
                /// <param name="row">The row to check.</param>
                abstract isBetweenRowSpacer: row: Blockly.BlockRendering.Row -> float
                /// <summary>Whether a measurable stores information about a top row.</summary>
                /// <param name="row">The row to check.</param>
                abstract isTopRow: row: Blockly.BlockRendering.Row -> float
                /// <summary>Whether a measurable stores information about a bottom row.</summary>
                /// <param name="row">The row to check.</param>
                abstract isBottomRow: row: Blockly.BlockRendering.Row -> float
                /// <summary>Whether a measurable stores information about a top or bottom row.</summary>
                /// <param name="row">The row to check.</param>
                abstract isTopOrBottomRow: row: Blockly.BlockRendering.Row -> float
                /// <summary>Whether a measurable stores information about an input row.</summary>
                /// <param name="row">The row to check.</param>
                abstract isInputRow: row: Blockly.BlockRendering.Row -> float

    module Msg =

        type [<AllowNullLiteral>] IExports =
            abstract LOGIC_HUE: string
            abstract LOOPS_HUE: string
            abstract MATH_HUE: string
            abstract TEXTS_HUE: string
            abstract LISTS_HUE: string
            abstract COLOUR_HUE: string
            abstract VARIABLES_HUE: string
            abstract VARIABLES_DYNAMIC_HUE: string
            abstract PROCEDURES_HUE: string
            abstract VARIABLES_DEFAULT_NAME: string
            abstract UNNAMED_KEY: string
            abstract TODAY: string
            abstract DUPLICATE_BLOCK: string
            abstract ADD_COMMENT: string
            abstract REMOVE_COMMENT: string
            abstract DUPLICATE_COMMENT: string
            abstract EXTERNAL_INPUTS: string
            abstract INLINE_INPUTS: string
            abstract DELETE_BLOCK: string
            abstract DELETE_X_BLOCKS: string
            abstract DELETE_ALL_BLOCKS: string
            abstract CLEAN_UP: string
            abstract COLLAPSE_BLOCK: string
            abstract COLLAPSE_ALL: string
            abstract EXPAND_BLOCK: string
            abstract EXPAND_ALL: string
            abstract DISABLE_BLOCK: string
            abstract ENABLE_BLOCK: string
            abstract HELP: string
            abstract UNDO: string
            abstract REDO: string
            abstract CHANGE_VALUE_TITLE: string
            abstract RENAME_VARIABLE: string
            abstract RENAME_VARIABLE_TITLE: string
            abstract NEW_VARIABLE: string
            abstract NEW_STRING_VARIABLE: string
            abstract NEW_NUMBER_VARIABLE: string
            abstract NEW_COLOUR_VARIABLE: string
            abstract NEW_VARIABLE_TYPE_TITLE: string
            abstract NEW_VARIABLE_TITLE: string
            abstract VARIABLE_ALREADY_EXISTS: string
            abstract VARIABLE_ALREADY_EXISTS_FOR_ANOTHER_TYPE: string
            abstract DELETE_VARIABLE_CONFIRMATION: string
            abstract CANNOT_DELETE_VARIABLE_PROCEDURE: string
            abstract DELETE_VARIABLE: string
            abstract COLOUR_PICKER_HELPURL: string
            abstract COLOUR_PICKER_TOOLTIP: string
            abstract COLOUR_RANDOM_HELPURL: string
            abstract COLOUR_RANDOM_TITLE: string
            abstract COLOUR_RANDOM_TOOLTIP: string
            abstract COLOUR_RGB_HELPURL: string
            abstract COLOUR_RGB_TITLE: string
            abstract COLOUR_RGB_RED: string
            abstract COLOUR_RGB_GREEN: string
            abstract COLOUR_RGB_BLUE: string
            abstract COLOUR_RGB_TOOLTIP: string
            abstract COLOUR_BLEND_HELPURL: string
            abstract COLOUR_BLEND_TITLE: string
            abstract COLOUR_BLEND_COLOUR1: string
            abstract COLOUR_BLEND_COLOUR2: string
            abstract COLOUR_BLEND_RATIO: string
            abstract COLOUR_BLEND_TOOLTIP: string
            abstract CONTROLS_REPEAT_HELPURL: string
            abstract CONTROLS_REPEAT_TITLE: string
            abstract CONTROLS_REPEAT_INPUT_DO: string
            abstract CONTROLS_REPEAT_TOOLTIP: string
            abstract CONTROLS_WHILEUNTIL_HELPURL: string
            abstract CONTROLS_WHILEUNTIL_INPUT_DO: string
            abstract CONTROLS_WHILEUNTIL_OPERATOR_WHILE: string
            abstract CONTROLS_WHILEUNTIL_OPERATOR_UNTIL: string
            abstract CONTROLS_WHILEUNTIL_TOOLTIP_WHILE: string
            abstract CONTROLS_WHILEUNTIL_TOOLTIP_UNTIL: string
            abstract CONTROLS_FOR_HELPURL: string
            abstract CONTROLS_FOR_TOOLTIP: string
            abstract CONTROLS_FOR_TITLE: string
            abstract CONTROLS_FOR_INPUT_DO: string
            abstract CONTROLS_FOREACH_HELPURL: string
            abstract CONTROLS_FOREACH_TITLE: string
            abstract CONTROLS_FOREACH_INPUT_DO: string
            abstract CONTROLS_FOREACH_TOOLTIP: string
            abstract CONTROLS_FLOW_STATEMENTS_HELPURL: string
            abstract CONTROLS_FLOW_STATEMENTS_OPERATOR_BREAK: string
            abstract CONTROLS_FLOW_STATEMENTS_OPERATOR_CONTINUE: string
            abstract CONTROLS_FLOW_STATEMENTS_TOOLTIP_BREAK: string
            abstract CONTROLS_FLOW_STATEMENTS_TOOLTIP_CONTINUE: string
            abstract CONTROLS_FLOW_STATEMENTS_WARNING: string
            abstract CONTROLS_IF_HELPURL: string
            abstract CONTROLS_IF_TOOLTIP_1: string
            abstract CONTROLS_IF_TOOLTIP_2: string
            abstract CONTROLS_IF_TOOLTIP_3: string
            abstract CONTROLS_IF_TOOLTIP_4: string
            abstract CONTROLS_IF_MSG_IF: string
            abstract CONTROLS_IF_MSG_ELSEIF: string
            abstract CONTROLS_IF_MSG_ELSE: string
            abstract CONTROLS_IF_MSG_THEN: string
            abstract CONTROLS_IF_IF_TITLE_IF: string
            abstract CONTROLS_IF_IF_TOOLTIP: string
            abstract CONTROLS_IF_ELSEIF_TITLE_ELSEIF: string
            abstract CONTROLS_IF_ELSEIF_TOOLTIP: string
            abstract CONTROLS_IF_ELSE_TITLE_ELSE: string
            abstract CONTROLS_IF_ELSE_TOOLTIP: string
            abstract IOS_OK: string
            abstract IOS_CANCEL: string
            abstract IOS_ERROR: string
            abstract IOS_PROCEDURES_INPUTS: string
            abstract IOS_PROCEDURES_ADD_INPUT: string
            abstract IOS_PROCEDURES_ALLOW_STATEMENTS: string
            abstract IOS_PROCEDURES_DUPLICATE_INPUTS_ERROR: string
            abstract IOS_VARIABLES_ADD_VARIABLE: string
            abstract IOS_VARIABLES_ADD_BUTTON: string
            abstract IOS_VARIABLES_RENAME_BUTTON: string
            abstract IOS_VARIABLES_DELETE_BUTTON: string
            abstract IOS_VARIABLES_VARIABLE_NAME: string
            abstract IOS_VARIABLES_EMPTY_NAME_ERROR: string
            abstract LOGIC_COMPARE_HELPURL: string
            abstract LOGIC_COMPARE_TOOLTIP_EQ: string
            abstract LOGIC_COMPARE_TOOLTIP_NEQ: string
            abstract LOGIC_COMPARE_TOOLTIP_LT: string
            abstract LOGIC_COMPARE_TOOLTIP_LTE: string
            abstract LOGIC_COMPARE_TOOLTIP_GT: string
            abstract LOGIC_COMPARE_TOOLTIP_GTE: string
            abstract LOGIC_OPERATION_HELPURL: string
            abstract LOGIC_OPERATION_TOOLTIP_AND: string
            abstract LOGIC_OPERATION_AND: string
            abstract LOGIC_OPERATION_TOOLTIP_OR: string
            abstract LOGIC_OPERATION_OR: string
            abstract LOGIC_NEGATE_HELPURL: string
            abstract LOGIC_NEGATE_TITLE: string
            abstract LOGIC_NEGATE_TOOLTIP: string
            abstract LOGIC_BOOLEAN_HELPURL: string
            abstract LOGIC_BOOLEAN_TRUE: string
            abstract LOGIC_BOOLEAN_FALSE: string
            abstract LOGIC_BOOLEAN_TOOLTIP: string
            abstract LOGIC_NULL_HELPURL: string
            abstract LOGIC_NULL: string
            abstract LOGIC_NULL_TOOLTIP: string
            abstract LOGIC_TERNARY_HELPURL: string
            abstract LOGIC_TERNARY_CONDITION: string
            abstract LOGIC_TERNARY_IF_TRUE: string
            abstract LOGIC_TERNARY_IF_FALSE: string
            abstract LOGIC_TERNARY_TOOLTIP: string
            abstract MATH_NUMBER_HELPURL: string
            abstract MATH_NUMBER_TOOLTIP: string
            abstract MATH_ADDITION_SYMBOL: string
            abstract MATH_SUBTRACTION_SYMBOL: string
            abstract MATH_DIVISION_SYMBOL: string
            abstract MATH_MULTIPLICATION_SYMBOL: string
            abstract MATH_POWER_SYMBOL: string
            abstract MATH_TRIG_SIN: string
            abstract MATH_TRIG_COS: string
            abstract MATH_TRIG_TAN: string
            abstract MATH_TRIG_ASIN: string
            abstract MATH_TRIG_ACOS: string
            abstract MATH_TRIG_ATAN: string
            abstract MATH_ARITHMETIC_HELPURL: string
            abstract MATH_ARITHMETIC_TOOLTIP_ADD: string
            abstract MATH_ARITHMETIC_TOOLTIP_MINUS: string
            abstract MATH_ARITHMETIC_TOOLTIP_MULTIPLY: string
            abstract MATH_ARITHMETIC_TOOLTIP_DIVIDE: string
            abstract MATH_ARITHMETIC_TOOLTIP_POWER: string
            abstract MATH_SINGLE_HELPURL: string
            abstract MATH_SINGLE_OP_ROOT: string
            abstract MATH_SINGLE_TOOLTIP_ROOT: string
            abstract MATH_SINGLE_OP_ABSOLUTE: string
            abstract MATH_SINGLE_TOOLTIP_ABS: string
            abstract MATH_SINGLE_TOOLTIP_NEG: string
            abstract MATH_SINGLE_TOOLTIP_LN: string
            abstract MATH_SINGLE_TOOLTIP_LOG10: string
            abstract MATH_SINGLE_TOOLTIP_EXP: string
            abstract MATH_SINGLE_TOOLTIP_POW10: string
            abstract MATH_TRIG_HELPURL: string
            abstract MATH_TRIG_TOOLTIP_SIN: string
            abstract MATH_TRIG_TOOLTIP_COS: string
            abstract MATH_TRIG_TOOLTIP_TAN: string
            abstract MATH_TRIG_TOOLTIP_ASIN: string
            abstract MATH_TRIG_TOOLTIP_ACOS: string
            abstract MATH_TRIG_TOOLTIP_ATAN: string
            abstract MATH_CONSTANT_HELPURL: string
            abstract MATH_CONSTANT_TOOLTIP: string
            abstract MATH_IS_EVEN: string
            abstract MATH_IS_ODD: string
            abstract MATH_IS_PRIME: string
            abstract MATH_IS_WHOLE: string
            abstract MATH_IS_POSITIVE: string
            abstract MATH_IS_NEGATIVE: string
            abstract MATH_IS_DIVISIBLE_BY: string
            abstract MATH_IS_TOOLTIP: string
            abstract MATH_CHANGE_HELPURL: string
            abstract MATH_CHANGE_TITLE: string
            abstract MATH_CHANGE_TITLE_ITEM: string
            abstract MATH_CHANGE_TOOLTIP: string
            abstract MATH_ROUND_HELPURL: string
            abstract MATH_ROUND_TOOLTIP: string
            abstract MATH_ROUND_OPERATOR_ROUND: string
            abstract MATH_ROUND_OPERATOR_ROUNDUP: string
            abstract MATH_ROUND_OPERATOR_ROUNDDOWN: string
            abstract MATH_ONLIST_HELPURL: string
            abstract MATH_ONLIST_OPERATOR_SUM: string
            abstract MATH_ONLIST_TOOLTIP_SUM: string
            abstract MATH_ONLIST_OPERATOR_MIN: string
            abstract MATH_ONLIST_TOOLTIP_MIN: string
            abstract MATH_ONLIST_OPERATOR_MAX: string
            abstract MATH_ONLIST_TOOLTIP_MAX: string
            abstract MATH_ONLIST_OPERATOR_AVERAGE: string
            abstract MATH_ONLIST_TOOLTIP_AVERAGE: string
            abstract MATH_ONLIST_OPERATOR_MEDIAN: string
            abstract MATH_ONLIST_TOOLTIP_MEDIAN: string
            abstract MATH_ONLIST_OPERATOR_MODE: string
            abstract MATH_ONLIST_TOOLTIP_MODE: string
            abstract MATH_ONLIST_OPERATOR_STD_DEV: string
            abstract MATH_ONLIST_TOOLTIP_STD_DEV: string
            abstract MATH_ONLIST_OPERATOR_RANDOM: string
            abstract MATH_ONLIST_TOOLTIP_RANDOM: string
            abstract MATH_MODULO_HELPURL: string
            abstract MATH_MODULO_TITLE: string
            abstract MATH_MODULO_TOOLTIP: string
            abstract MATH_CONSTRAIN_HELPURL: string
            abstract MATH_CONSTRAIN_TITLE: string
            abstract MATH_CONSTRAIN_TOOLTIP: string
            abstract MATH_RANDOM_INT_HELPURL: string
            abstract MATH_RANDOM_INT_TITLE: string
            abstract MATH_RANDOM_INT_TOOLTIP: string
            abstract MATH_RANDOM_FLOAT_HELPURL: string
            abstract MATH_RANDOM_FLOAT_TITLE_RANDOM: string
            abstract MATH_RANDOM_FLOAT_TOOLTIP: string
            abstract MATH_ATAN2_HELPURL: string
            abstract MATH_ATAN2_TITLE: string
            abstract MATH_ATAN2_TOOLTIP: string
            abstract TEXT_TEXT_HELPURL: string
            abstract TEXT_TEXT_TOOLTIP: string
            abstract TEXT_JOIN_HELPURL: string
            abstract TEXT_JOIN_TITLE_CREATEWITH: string
            abstract TEXT_JOIN_TOOLTIP: string
            abstract TEXT_CREATE_JOIN_TITLE_JOIN: string
            abstract TEXT_CREATE_JOIN_TOOLTIP: string
            abstract TEXT_CREATE_JOIN_ITEM_TITLE_ITEM: string
            abstract TEXT_CREATE_JOIN_ITEM_TOOLTIP: string
            abstract TEXT_APPEND_HELPURL: string
            abstract TEXT_APPEND_TITLE: string
            abstract TEXT_APPEND_VARIABLE: string
            abstract TEXT_APPEND_TOOLTIP: string
            abstract TEXT_LENGTH_HELPURL: string
            abstract TEXT_LENGTH_TITLE: string
            abstract TEXT_LENGTH_TOOLTIP: string
            abstract TEXT_ISEMPTY_HELPURL: string
            abstract TEXT_ISEMPTY_TITLE: string
            abstract TEXT_ISEMPTY_TOOLTIP: string
            abstract TEXT_INDEXOF_HELPURL: string
            abstract TEXT_INDEXOF_TOOLTIP: string
            abstract TEXT_INDEXOF_TITLE: string
            abstract TEXT_INDEXOF_OPERATOR_FIRST: string
            abstract TEXT_INDEXOF_OPERATOR_LAST: string
            abstract TEXT_CHARAT_HELPURL: string
            abstract TEXT_CHARAT_TITLE: string
            abstract TEXT_CHARAT_FROM_START: string
            abstract TEXT_CHARAT_FROM_END: string
            abstract TEXT_CHARAT_FIRST: string
            abstract TEXT_CHARAT_LAST: string
            abstract TEXT_CHARAT_RANDOM: string
            abstract TEXT_CHARAT_TAIL: string
            abstract TEXT_CHARAT_TOOLTIP: string
            abstract TEXT_GET_SUBSTRING_TOOLTIP: string
            abstract TEXT_GET_SUBSTRING_HELPURL: string
            abstract TEXT_GET_SUBSTRING_INPUT_IN_TEXT: string
            abstract TEXT_GET_SUBSTRING_START_FROM_START: string
            abstract TEXT_GET_SUBSTRING_START_FROM_END: string
            abstract TEXT_GET_SUBSTRING_START_FIRST: string
            abstract TEXT_GET_SUBSTRING_END_FROM_START: string
            abstract TEXT_GET_SUBSTRING_END_FROM_END: string
            abstract TEXT_GET_SUBSTRING_END_LAST: string
            abstract TEXT_GET_SUBSTRING_TAIL: string
            abstract TEXT_CHANGECASE_HELPURL: string
            abstract TEXT_CHANGECASE_TOOLTIP: string
            abstract TEXT_CHANGECASE_OPERATOR_UPPERCASE: string
            abstract TEXT_CHANGECASE_OPERATOR_LOWERCASE: string
            abstract TEXT_CHANGECASE_OPERATOR_TITLECASE: string
            abstract TEXT_TRIM_HELPURL: string
            abstract TEXT_TRIM_TOOLTIP: string
            abstract TEXT_TRIM_OPERATOR_BOTH: string
            abstract TEXT_TRIM_OPERATOR_LEFT: string
            abstract TEXT_TRIM_OPERATOR_RIGHT: string
            abstract TEXT_PRINT_HELPURL: string
            abstract TEXT_PRINT_TITLE: string
            abstract TEXT_PRINT_TOOLTIP: string
            abstract TEXT_PROMPT_HELPURL: string
            abstract TEXT_PROMPT_TYPE_TEXT: string
            abstract TEXT_PROMPT_TYPE_NUMBER: string
            abstract TEXT_PROMPT_TOOLTIP_NUMBER: string
            abstract TEXT_PROMPT_TOOLTIP_TEXT: string
            abstract TEXT_COUNT_MESSAGE0: string
            abstract TEXT_COUNT_HELPURL: string
            abstract TEXT_COUNT_TOOLTIP: string
            abstract TEXT_REPLACE_MESSAGE0: string
            abstract TEXT_REPLACE_HELPURL: string
            abstract TEXT_REPLACE_TOOLTIP: string
            abstract TEXT_REVERSE_MESSAGE0: string
            abstract TEXT_REVERSE_HELPURL: string
            abstract TEXT_REVERSE_TOOLTIP: string
            abstract LISTS_CREATE_EMPTY_HELPURL: string
            abstract LISTS_CREATE_EMPTY_TITLE: string
            abstract LISTS_CREATE_EMPTY_TOOLTIP: string
            abstract LISTS_CREATE_WITH_HELPURL: string
            abstract LISTS_CREATE_WITH_TOOLTIP: string
            abstract LISTS_CREATE_WITH_INPUT_WITH: string
            abstract LISTS_CREATE_WITH_CONTAINER_TITLE_ADD: string
            abstract LISTS_CREATE_WITH_CONTAINER_TOOLTIP: string
            abstract LISTS_CREATE_WITH_ITEM_TITLE: string
            abstract LISTS_CREATE_WITH_ITEM_TOOLTIP: string
            abstract LISTS_REPEAT_HELPURL: string
            abstract LISTS_REPEAT_TOOLTIP: string
            abstract LISTS_REPEAT_TITLE: string
            abstract LISTS_LENGTH_HELPURL: string
            abstract LISTS_LENGTH_TITLE: string
            abstract LISTS_LENGTH_TOOLTIP: string
            abstract LISTS_ISEMPTY_HELPURL: string
            abstract LISTS_ISEMPTY_TITLE: string
            abstract LISTS_ISEMPTY_TOOLTIP: string
            abstract LISTS_INLIST: string
            abstract LISTS_INDEX_OF_HELPURL: string
            abstract LISTS_INDEX_OF_INPUT_IN_LIST: string
            abstract LISTS_INDEX_OF_FIRST: string
            abstract LISTS_INDEX_OF_LAST: string
            abstract LISTS_INDEX_OF_TOOLTIP: string
            abstract LISTS_GET_INDEX_HELPURL: string
            abstract LISTS_GET_INDEX_GET: string
            abstract LISTS_GET_INDEX_GET_REMOVE: string
            abstract LISTS_GET_INDEX_REMOVE: string
            abstract LISTS_GET_INDEX_FROM_START: string
            abstract LISTS_GET_INDEX_FROM_END: string
            abstract LISTS_GET_INDEX_FIRST: string
            abstract LISTS_GET_INDEX_LAST: string
            abstract LISTS_GET_INDEX_RANDOM: string
            abstract LISTS_GET_INDEX_TAIL: string
            abstract LISTS_GET_INDEX_INPUT_IN_LIST: string
            abstract LISTS_INDEX_FROM_START_TOOLTIP: string
            abstract LISTS_INDEX_FROM_END_TOOLTIP: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_FROM: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_FIRST: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_LAST: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_RANDOM: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_REMOVE_FROM: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_REMOVE_FIRST: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_REMOVE_LAST: string
            abstract LISTS_GET_INDEX_TOOLTIP_GET_REMOVE_RANDOM: string
            abstract LISTS_GET_INDEX_TOOLTIP_REMOVE_FROM: string
            abstract LISTS_GET_INDEX_TOOLTIP_REMOVE_FIRST: string
            abstract LISTS_GET_INDEX_TOOLTIP_REMOVE_LAST: string
            abstract LISTS_GET_INDEX_TOOLTIP_REMOVE_RANDOM: string
            abstract LISTS_SET_INDEX_HELPURL: string
            abstract LISTS_SET_INDEX_INPUT_IN_LIST: string
            abstract LISTS_SET_INDEX_SET: string
            abstract LISTS_SET_INDEX_INSERT: string
            abstract LISTS_SET_INDEX_INPUT_TO: string
            abstract LISTS_SET_INDEX_TOOLTIP_SET_FROM: string
            abstract LISTS_SET_INDEX_TOOLTIP_SET_FIRST: string
            abstract LISTS_SET_INDEX_TOOLTIP_SET_LAST: string
            abstract LISTS_SET_INDEX_TOOLTIP_SET_RANDOM: string
            abstract LISTS_SET_INDEX_TOOLTIP_INSERT_FROM: string
            abstract LISTS_SET_INDEX_TOOLTIP_INSERT_FIRST: string
            abstract LISTS_SET_INDEX_TOOLTIP_INSERT_LAST: string
            abstract LISTS_SET_INDEX_TOOLTIP_INSERT_RANDOM: string
            abstract LISTS_GET_SUBLIST_HELPURL: string
            abstract LISTS_GET_SUBLIST_INPUT_IN_LIST: string
            abstract LISTS_GET_SUBLIST_START_FROM_START: string
            abstract LISTS_GET_SUBLIST_START_FROM_END: string
            abstract LISTS_GET_SUBLIST_START_FIRST: string
            abstract LISTS_GET_SUBLIST_END_FROM_START: string
            abstract LISTS_GET_SUBLIST_END_FROM_END: string
            abstract LISTS_GET_SUBLIST_END_LAST: string
            abstract LISTS_GET_SUBLIST_TAIL: string
            abstract LISTS_GET_SUBLIST_TOOLTIP: string
            abstract LISTS_SORT_HELPURL: string
            abstract LISTS_SORT_TITLE: string
            abstract LISTS_SORT_TOOLTIP: string
            abstract LISTS_SORT_ORDER_ASCENDING: string
            abstract LISTS_SORT_ORDER_DESCENDING: string
            abstract LISTS_SORT_TYPE_NUMERIC: string
            abstract LISTS_SORT_TYPE_TEXT: string
            abstract LISTS_SORT_TYPE_IGNORECASE: string
            abstract LISTS_SPLIT_HELPURL: string
            abstract LISTS_SPLIT_LIST_FROM_TEXT: string
            abstract LISTS_SPLIT_TEXT_FROM_LIST: string
            abstract LISTS_SPLIT_WITH_DELIMITER: string
            abstract LISTS_SPLIT_TOOLTIP_SPLIT: string
            abstract LISTS_SPLIT_TOOLTIP_JOIN: string
            abstract LISTS_REVERSE_HELPURL: string
            abstract LISTS_REVERSE_MESSAGE0: string
            abstract LISTS_REVERSE_TOOLTIP: string
            abstract ORDINAL_NUMBER_SUFFIX: string
            abstract VARIABLES_GET_HELPURL: string
            abstract VARIABLES_GET_TOOLTIP: string
            abstract VARIABLES_GET_CREATE_SET: string
            abstract VARIABLES_SET_HELPURL: string
            abstract VARIABLES_SET: string
            abstract VARIABLES_SET_TOOLTIP: string
            abstract VARIABLES_SET_CREATE_GET: string
            abstract PROCEDURES_DEFNORETURN_HELPURL: string
            abstract PROCEDURES_DEFNORETURN_TITLE: string
            abstract PROCEDURES_DEFNORETURN_PROCEDURE: string
            abstract PROCEDURES_BEFORE_PARAMS: string
            abstract PROCEDURES_CALL_BEFORE_PARAMS: string
            abstract PROCEDURES_DEFNORETURN_DO: string
            abstract PROCEDURES_DEFNORETURN_TOOLTIP: string
            abstract PROCEDURES_DEFNORETURN_COMMENT: string
            abstract PROCEDURES_DEFRETURN_HELPURL: string
            abstract PROCEDURES_DEFRETURN_TITLE: string
            abstract PROCEDURES_DEFRETURN_PROCEDURE: string
            abstract PROCEDURES_DEFRETURN_DO: string
            abstract PROCEDURES_DEFRETURN_COMMENT: string
            abstract PROCEDURES_DEFRETURN_RETURN: string
            abstract PROCEDURES_DEFRETURN_TOOLTIP: string
            abstract PROCEDURES_ALLOW_STATEMENTS: string
            abstract PROCEDURES_DEF_DUPLICATE_WARNING: string
            abstract PROCEDURES_CALLNORETURN_HELPURL: string
            abstract PROCEDURES_CALLNORETURN_TOOLTIP: string
            abstract PROCEDURES_CALLRETURN_HELPURL: string
            abstract PROCEDURES_CALLRETURN_TOOLTIP: string
            abstract PROCEDURES_MUTATORCONTAINER_TITLE: string
            abstract PROCEDURES_MUTATORCONTAINER_TOOLTIP: string
            abstract PROCEDURES_MUTATORARG_TITLE: string
            abstract PROCEDURES_MUTATORARG_TOOLTIP: string
            abstract PROCEDURES_HIGHLIGHT_DEF: string
            abstract PROCEDURES_CREATE_DO: string
            abstract PROCEDURES_IFRETURN_TOOLTIP: string
            abstract PROCEDURES_IFRETURN_HELPURL: string
            abstract PROCEDURES_IFRETURN_WARNING: string
            abstract WORKSPACE_COMMENT_DEFAULT_TEXT: string
            abstract COLLAPSED_WARNINGS_WARNING: string

    type [<AllowNullLiteral>] TypeLiteral_07 =
        abstract controls: bool option with get, set
        abstract wheel: bool option with get, set
        abstract startScale: float option with get, set
        abstract maxScale: float option with get, set
        abstract minScale: float option with get, set
        abstract scaleSpeed: float option with get, set

    type [<AllowNullLiteral>] TypeLiteral_05 =
        abstract scrollbars: bool option with get, set
        abstract drag: bool option with get, set
        abstract wheel: bool option with get, set

    type [<AllowNullLiteral>] TypeLiteral_06 =
        abstract spacing: float option with get, set
        abstract colour: string option with get, set
        abstract length: float option with get, set
        abstract snap: bool option with get, set

    type [<AllowNullLiteral>] TypeLiteral_08 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: blocks: string -> Blockly.Theme.BlockStyle with get, set

    type [<AllowNullLiteral>] TypeLiteral_09 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: category: string -> Blockly.Theme.CategoryStyle with get, set

    type [<AllowNullLiteral>] TypeLiteral_14 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.BlockStyle with get, set

    type [<AllowNullLiteral>] TypeLiteral_15 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> Blockly.Theme.CategoryStyle with get, set

    type [<AllowNullLiteral>] TypeLiteral_16 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

    type [<AllowNullLiteral>] TypeLiteral_13 =
        [<Emit "$0($1...)">] abstract Invoke: unit -> ResizeArray<ResizeArray<obj option>>

    type [<AllowNullLiteral>] TypeLiteral_12 =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.BlockSvg -> obj option

    type [<AllowNullLiteral>] TypeLiteral_11 =
        [<Emit "$0($1...)">] abstract Invoke: _0: Blockly.WorkspaceSvg -> Blockly.BlockSvg

    type [<AllowNullLiteral>] TypeLiteral_10 =
        [<Emit "$0($1...)">] abstract Invoke: _0: Element -> obj option

module Goog =
    let [<Import("getMsg","blockly/goog")>] getMsg: GetMsg.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        abstract getMsgOrig: Function
        /// <summary>Gets a localized message.
        /// Overrides the default Closure function to check for a Blockly.Msg first.
        /// Used infrequently, only known case is TODAY button in date picker.</summary>
        /// <param name="str">Translatable string, places holders in the form {$foo}.</param>
        /// <param name="opt_values">Maps place holder name to value.</param>
        abstract getMsg: str: string * ?opt_values: GetMsgOpt_values -> string

    type [<AllowNullLiteral>] GetMsgOpt_values =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

    module GetMsg =

        type [<AllowNullLiteral>] IExports =
            abstract blocklyMsgMap: obj option
