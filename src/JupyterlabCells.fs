// ts2fable 0.0.0
module rec JupyterlabCells
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types
open Fable.React

//amo typescript shims. Fable.React simplifies a lot of the typescript complexity 
type BaseSyntheticEvent<'E,'C,'T> =
    abstract nativeEvent : 'E with get,set
    abstract currentTarget: 'C with get,set
    abstract target: 'T with get,set

type SyntheticEvent<'T,'E> =
    inherit BaseSyntheticEvent<'E, 'T, EventTarget>

type ReactElement<'T> =
    abstract ``type`` : 'T with get,set

type MouseEvent<'T> =
    inherit SyntheticEvent<MouseEvent, 'T>
// amo end typescript

module Celldragutils =
    type IterableOrArrayLike<'T> = PhosphorAlgorithm.Iter.IterableOrArrayLike<'T> // __@phosphor_algorithm.IterableOrArrayLike
    type Cell = Widget.Cell
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat

    module CellDragUtils =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Find the cell index containing the target html element.
            /// This function traces up the DOM hierarchy to find the root cell
            /// node. Then find the corresponding child and select it.</summary>
            /// <param name="node">- the cell node or a child of the cell node.</param>
            /// <param name="cells">- an iterable of Cells</param>
            /// <param name="isCellNode">- a function that takes in a node and checks if
            /// it is a cell node.</param>
            abstract findCell: node: HTMLElement * cells: IterableOrArrayLike<Cell> * isCellNode: (HTMLElement -> bool) -> float
            /// <summary>Detect which part of the cell triggered the MouseEvent</summary>
            /// <param name="cell">- The cell which contains the MouseEvent's target</param>
            /// <param name="target">- The DOM node which triggered the MouseEvent</param>
            abstract detectTargetArea: cell: Cell * target: HTMLElement -> ICellTargetArea
            /// <summary>Detect if a drag event should be started. This is down if the
            /// mouse is moved beyond a certain distance (DRAG_THRESHOLD).</summary>
            /// <param name="prevX">- X Coordinate of the mouse pointer during the mousedown event</param>
            /// <param name="prevY">- Y Coordinate of the mouse pointer during the mousedown event</param>
            /// <param name="nextX">- Current X Coordinate of the mouse pointer</param>
            /// <param name="nextY">- Current Y Coordinate of the mouse pointer</param>
            abstract shouldStartDrag: prevX: float * prevY: float * nextX: float * nextY: float -> bool
            /// <summary>Create an image for the cell(s) to be dragged</summary>
            /// <param name="activeCell">- The cell from where the drag event is triggered</param>
            /// <param name="selectedCells">- The cells to be dragged</param>
            abstract createCellDragImage: activeCell: Cell * selectedCells: ResizeArray<JupyterlabCoreutils.Nbformat.Nbformat.ICell> -> HTMLElement

        type [<StringEnum>] [<RequireQualifiedAccess>] ICellTargetArea =
            | Input
            | Prompt
            | Cell
            | Unknown

module Collapser =
    type ReactWidget = JupyterlabApputils.ReactWidget // __@jupyterlab_apputils.ReactWidget

    //AMO: global export
    type [<AllowNullLiteral>] IExports =
        abstract Collapser: CollapserStatic
        abstract InputCollapser: InputCollapserStatic
        abstract OutputCollapser: OutputCollapserStatic

    /// Abstract collapser base class.
    /// 
    /// ### Notes
    /// A collapser is a visible div to the left of a cell's
    /// input/output that a user can click on to collapse the
    /// input/output.
    type [<AllowNullLiteral>] Collapser =
        inherit ReactWidget
        /// Is the input/output of the parent collapsed.
        abstract collapsed: bool
        // /// Render the collapser with the virtual DOM.
        // abstract render: unit -> React.ReactElement<obj option>
        // /// Handle the click event.
        // abstract handleClick: e: React.MouseEvent<HTMLDivElement> -> unit
        /// Render the collapser with the virtual DOM.
        abstract render: unit -> ReactElement<obj option>
        /// Handle the click event.
        abstract handleClick: e: MouseEvent<HTMLDivElement> -> unit

    /// Abstract collapser base class.
    /// 
    /// ### Notes
    /// A collapser is a visible div to the left of a cell's
    /// input/output that a user can click on to collapse the
    /// input/output.
    type [<AllowNullLiteral>] CollapserStatic =
        /// Construct a new collapser.
        [<Emit "new $0($1...)">] abstract Create: unit -> Collapser

    /// A collapser subclass to collapse a cell's input area.
    type [<AllowNullLiteral>] InputCollapser =
        inherit Collapser
        /// Is the cell's input collapsed?
        abstract collapsed: bool
        /// Handle a click event for the user to collapse the cell's input.
        // abstract handleClick: e: React.MouseEvent<HTMLDivElement> -> unit
        abstract handleClick: e: MouseEvent<HTMLDivElement> -> unit

    /// A collapser subclass to collapse a cell's input area.
    type [<AllowNullLiteral>] InputCollapserStatic =
        /// Construct a new input collapser.
        [<Emit "new $0($1...)">] abstract Create: unit -> InputCollapser

    /// A collapser subclass to collapse a cell's output area.
    type [<AllowNullLiteral>] OutputCollapser =
        inherit Collapser
        /// Is the cell's output collapsed?
        abstract collapsed: bool
        /// Handle a click event for the user to collapse the cell's output.
        // abstract handleClick: e: React.MouseEvent<HTMLDivElement> -> unit
        abstract handleClick: e: MouseEvent<HTMLDivElement> -> unit

    /// A collapser subclass to collapse a cell's output area.
    type [<AllowNullLiteral>] OutputCollapserStatic =
        /// Construct a new output collapser.
        [<Emit "new $0($1...)">] abstract Create: unit -> OutputCollapser

module Headerfooter =
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget

    //AMO: global export
    type [<AllowNullLiteral>] IExports =
        abstract CellHeader: CellHeaderStatic
        abstract CellFooter: CellFooterStatic

    /// The interface for a cell header.
    type [<AllowNullLiteral>] ICellHeader =
        inherit Widget

    /// Default implementation of a cell header.
    type [<AllowNullLiteral>] CellHeader =
        inherit Widget
        inherit ICellHeader

    /// Default implementation of a cell header.
    type [<AllowNullLiteral>] CellHeaderStatic =
        /// Construct a new cell header.
        [<Emit "new $0($1...)">] abstract Create: unit -> CellHeader

    /// The interface for a cell footer.
    type [<AllowNullLiteral>] ICellFooter =
        inherit Widget

    /// Default implementation of a cell footer.
    type [<AllowNullLiteral>] CellFooter =
        inherit Widget
        inherit ICellFooter

    /// Default implementation of a cell footer.
    type [<AllowNullLiteral>] CellFooterStatic =
        /// Construct a new cell footer.
        [<Emit "new $0($1...)">] abstract Create: unit -> CellFooter

module Inputarea =
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    type CodeEditorWrapper = JupyterlabCodeeditor.Widget.CodeEditorWrapper //  __@jupyterlab_codeeditor.CodeEditorWrapper
    type ICellModel = Model.ICellModel
    //AMO: global export
    type [<AllowNullLiteral>] IExports =
        abstract InputArea: InputAreaStatic
        abstract InputPrompt: InputPromptStatic

    /// An input area widget, which hosts a prompt and an editor widget.
    /// A namespace for `InputArea` statics.
    type [<AllowNullLiteral>] InputArea =
        inherit Widget
        /// The model used by the widget.
        abstract model: ICellModel
        /// The content factory used by the widget.
        abstract contentFactory: InputArea.IContentFactory
        /// Get the CodeEditorWrapper used by the cell.
        abstract editorWidget: CodeEditorWrapper
        /// Get the CodeEditor used by the cell.
        abstract editor: JupyterlabCodeeditor.Editor.CodeEditor.IEditor
        /// Get the prompt node used by the cell.
        abstract promptNode: HTMLElement
        /// Render an input instead of the text editor.
        abstract renderInput: widget: Widget -> unit
        /// Show the text editor.
        abstract showEditor: unit -> unit
        /// Set the prompt of the input area.
        abstract setPrompt: value: string -> unit
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit

    /// An input area widget, which hosts a prompt and an editor widget.
    /// A namespace for `InputArea` statics.
    type [<AllowNullLiteral>] InputAreaStatic =
        /// Construct an input area widget.
        [<Emit "new $0($1...)">] abstract Create: options: InputArea.IOptions -> InputArea

    module InputArea =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultEditorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory
            abstract defaultContentFactory: ContentFactory

        /// The options used to create an `InputArea`.
        type [<AllowNullLiteral>] IOptions =
            /// The model used by the widget.
            abstract model: ICellModel with get, set
            /// The content factory used by the widget to create children.
            /// 
            /// Defaults to one that uses CodeMirror.
            abstract contentFactory: IContentFactory option with get, set
            /// Whether to send an update request to the editor when it is shown.
            abstract updateOnShow: bool option with get, set

        /// An input area widget content factory.
        /// 
        /// The content factory is used to create children in a way
        /// that can be customized.
        type [<AllowNullLiteral>] IContentFactory =
            /// The editor factory we need to include in `CodeEditorWratter.IOptions`.
            /// 
            /// This is a separate readonly attribute rather than a factory method as we need
            /// to pass it around.
            abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory
            /// Create an input prompt.
            abstract createInputPrompt: unit -> IInputPrompt

        /// Default implementation of `IContentFactory`.
        /// 
        /// This defaults to using an `editorFactory` based on CodeMirror.
        /// A namespace for the input area content factory.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IContentFactory
            /// Return the `CodeEditor.Factory` being used.
            abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory
            /// Create an input prompt.
            abstract createInputPrompt: unit -> IInputPrompt

        /// Default implementation of `IContentFactory`.
        /// 
        /// This defaults to using an `editorFactory` based on CodeMirror.
        /// A namespace for the input area content factory.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            /// Construct a `ContentFactory`.
            [<Emit "new $0($1...)">] abstract Create: ?options: ContentFactory.IOptions -> ContentFactory

        module ContentFactory =

            /// Options for the content factory.
            type [<AllowNullLiteral>] IOptions =
                /// The editor factory used by the content factory.
                /// 
                /// If this is not passed, a default CodeMirror editor factory
                /// will be used.
                abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory option with get, set

    /// The interface for the input prompt.
    type [<AllowNullLiteral>] IInputPrompt =
        inherit Widget
        /// The execution count of the prompt.
        abstract executionCount: string with get, set

    /// The default input prompt implementation.
    type [<AllowNullLiteral>] InputPrompt =
        inherit Widget
        inherit IInputPrompt
        /// The execution count for the prompt.
        abstract executionCount: string with get, set

    /// The default input prompt implementation.
    type [<AllowNullLiteral>] InputPromptStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> InputPrompt

module Model =
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Signal<'T,'U>  = PhosphorSignaling.Signal<'T,'U>  // __@phosphor_signaling.Signal
    type IAttachmentsModel = JupyterlabAttachments.Model.IAttachmentsModel // __@jupyterlab_attachments.IAttachmentsModel
    // type CodeEditor =   __@jupyterlab_codeeditor.CodeEditor
    // type IChangedArgs = JupyterlabCoreutils.Interfaces.IChangedArgs // __@jupyterlab_coreutils.IChangedArgs
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    type IObservableJSON = JupyterlabObservables.Observablejson.IObservableJSON // __@jupyterlab_observables.IObservableJSON
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB //  __@jupyterlab_observables.IModelDB
    type IObservableValue = JupyterlabObservables.Modeldb.IObservableValue // __@jupyterlab_observables.IObservableValue
    type ObservableValue = JupyterlabObservables.Modeldb.ObservableValue // __@jupyterlab_observables.ObservableValue
    type IOutputAreaModel = JupyterlabOutputarea.Widget.IOutputAreaModel // __@jupyterlab_outputarea.IOutputAreaModel

    type [<AllowNullLiteral>] IExports =
        abstract isCodeCellJupyterlabObservablesModel: model: ICellModel -> bool
        abstract isMarkdownCellModel: model: ICellModel -> bool
        abstract isRawCellModel: model: ICellModel -> bool
        abstract CellModel: CellModelStatic
        abstract AttachmentsCellModel: AttachmentsCellModelStatic
        abstract RawCellModel: RawCellModelStatic
        abstract MarkdownCellModel: MarkdownCellModelStatic
        abstract CodeCellModel: CodeCellModelStatic

    //TODO better name
    [<Import("*","@jupyterlab/cells")>]
    let Types:IExports = jsNative

    /// The definition of a model object for a cell.
    type [<AllowNullLiteral>] ICellModel =
        inherit JupyterlabCodeeditor.Editor.CodeEditor.IModel
        /// The type of the cell.
        abstract ``type``: JupyterlabCoreutils.Nbformat.Nbformat.CellType
        /// A unique identifier for the cell.
        abstract id: string
        /// A signal emitted when the content of the model changes.
        abstract contentChanged: ISignal<ICellModel, unit>
        /// A signal emitted when a model state changes.
        abstract stateChanged: ISignal<ICellModel,  JupyterlabCoreutils.Interfaces.IChangedArgs<obj option>>
        /// Whether the cell is trusted.
        abstract trusted: bool with get, set
        /// The metadata associated with the cell.
        abstract metadata: IObservableJSON
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.ICell

    /// The definition of a model cell object for a cell with attachments.
    type [<AllowNullLiteral>] IAttachmentsCellModel =
        inherit ICellModel
        /// The cell attachments
        abstract attachments: IAttachmentsModel

    /// The definition of a code cell.
    type [<AllowNullLiteral>] ICodeCellModel =
        inherit ICellModel
        /// The type of the cell.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract ``type``: string
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.ICodeCell
        /// The code cell's prompt number. Will be null if the cell has not been run.
        abstract executionCount: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set
        /// The cell outputs.
        abstract outputs: IOutputAreaModel

    /// The definition of a markdown cell.
    type [<AllowNullLiteral>] IMarkdownCellModel =
        inherit IAttachmentsCellModel
        /// The type of the cell.
        abstract ``type``: string
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IMarkdownCell

    /// The definition of a raw cell.
    type [<AllowNullLiteral>] IRawCellModel =
        inherit IAttachmentsCellModel
        /// The type of the cell.
        abstract ``type``: string
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IRawCell

    /// An implementation of the cell model.
    /// The namespace for `CellModel` statics.
    type [<AllowNullLiteral>] CellModel =
        inherit JupyterlabCodeeditor.Editor.CodeEditor.Model
        inherit ICellModel
        /// The type of cell.
        abstract ``type``: JupyterlabCoreutils.Nbformat.Nbformat.CellType
        /// A signal emitted when the state of the model changes.
        abstract contentChanged: Signal<CellModel, unit>
        /// A signal emitted when a model state changes.
        abstract stateChanged: Signal<CellModel,  JupyterlabCoreutils.Interfaces.IChangedArgs<obj option, string>>
        /// The id for the cell.
        abstract id: string
        /// The metadata associated with the cell.
        abstract metadata: IObservableJSON
        /// Get the trusted state of the model.
        /// Set the trusted state of the model.
        abstract trusted: bool with get, set
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.ICell
        /// Handle a change to the trusted state.
        /// 
        /// The default implementation is a no-op.
        abstract onTrustedChanged: trusted: IObservableValue * args: JupyterlabObservables.Modeldb.ObservableValue.IChangedArgs -> unit
        /// Handle a change to the observable value.
        abstract onGenericChange: unit -> unit

    /// An implementation of the cell model.
    /// The namespace for `CellModel` statics.
    type [<AllowNullLiteral>] CellModelStatic =
        /// Construct a cell model from optional cell content.
        [<Emit "new $0($1...)">] abstract Create: options: CellModel.IOptions -> CellModel

    module CellModel =

        /// The options used to initialize a `CellModel`.
        type [<AllowNullLiteral>] IOptions =
            /// The source cell data.
            abstract cell: JupyterlabCoreutils.Nbformat.Nbformat.IBaseCell option with get, set
            /// An IModelDB in which to store cell data.
            abstract modelDB: IModelDB option with get, set
            /// A unique identifier for this cell.
            abstract id: string option with get, set

    /// A base implementation for cell models with attachments.
    /// The namespace for `AttachmentsCellModel` statics.
    type [<AllowNullLiteral>] AttachmentsCellModel =
        inherit CellModel
        /// Get the attachments of the model.
        abstract attachments: IAttachmentsModel
        /// Serialize the model to JSON.
        abstract toJSON: unit -> U2<JupyterlabCoreutils.Nbformat.Nbformat.IRawCell, JupyterlabCoreutils.Nbformat.Nbformat.IMarkdownCell>

    /// A base implementation for cell models with attachments.
    /// The namespace for `AttachmentsCellModel` statics.
    type [<AllowNullLiteral>] AttachmentsCellModelStatic =
        /// Construct a new cell with optional attachments.
        [<Emit "new $0($1...)">] abstract Create: options: AttachmentsCellModel.IOptions -> AttachmentsCellModel

    module AttachmentsCellModel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// The options used to initialize a `AttachmentsCellModel`.
        type [<AllowNullLiteral>] IOptions =
            inherit CellModel.IOptions
            /// The factory for attachment model creation.
            abstract contentFactory: IContentFactory option with get, set

        /// A factory for creating code cell model content.
        type [<AllowNullLiteral>] IContentFactory =
            /// Create an output area.
            abstract createAttachmentsModel: options: JupyterlabAttachments.Model.IAttachmentsModel.IOptions -> IAttachmentsModel

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IContentFactory
            /// Create an attachments model.
            abstract createAttachmentsModel: options: JupyterlabAttachments.Model.IAttachmentsModel.IOptions -> IAttachmentsModel

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

    /// An implementation of a raw cell model.
    type [<AllowNullLiteral>] RawCellModel =
        inherit AttachmentsCellModel
        /// The type of the cell.
        abstract ``type``: string
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IRawCell

    /// An implementation of a raw cell model.
    type [<AllowNullLiteral>] RawCellModelStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> RawCellModel

    /// An implementation of a markdown cell model.
    type [<AllowNullLiteral>] MarkdownCellModel =
        inherit AttachmentsCellModel
        /// The type of the cell.
        abstract ``type``: string
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IMarkdownCell

    /// An implementation of a markdown cell model.
    type [<AllowNullLiteral>] MarkdownCellModelStatic =
        /// Construct a markdown cell model from optional cell content.
        [<Emit "new $0($1...)">] abstract Create: options: CellModel.IOptions -> MarkdownCellModel

    /// An implementation of a code cell Model.
    /// The namespace for `CodeCellModel` statics.
    type [<AllowNullLiteral>] CodeCellModel =
        inherit CellModel
        inherit ICodeCellModel
        /// The type of the cell.
        abstract ``type``: string
        /// The execution count of the cell.
        abstract executionCount: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set
        /// The cell outputs.
        abstract outputs: IOutputAreaModel
        /// Dispose of the resources held by the model.
        abstract dispose: unit -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.ICodeCell
        /// Handle a change to the trusted state.
        abstract onTrustedChanged: trusted: IObservableValue * args: JupyterlabObservables.Modeldb.ObservableValue.IChangedArgs -> unit

    /// An implementation of a code cell Model.
    /// The namespace for `CodeCellModel` statics.
    type [<AllowNullLiteral>] CodeCellModelStatic =
        /// Construct a new code cell with optional original cell content.
        [<Emit "new $0($1...)">] abstract Create: options: CodeCellModel.IOptions -> CodeCellModel

    module CodeCellModel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// The options used to initialize a `CodeCellModel`.
        type [<AllowNullLiteral>] IOptions =
            inherit CellModel.IOptions
            /// The factory for output area model creation.
            abstract contentFactory: IContentFactory option with get, set

        /// A factory for creating code cell model content.
        type [<AllowNullLiteral>] IContentFactory =
            /// Create an output area.
            abstract createOutputArea: options: JupyterlabOutputarea.Model.IOutputAreaModel.IOptions -> IOutputAreaModel

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IContentFactory
            /// Create an output area.
            abstract createOutputArea: options: JupyterlabOutputarea.Model.IOutputAreaModel.IOptions -> IOutputAreaModel

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

module Placeholder =
    type ReactWidget = JupyterlabApputils.ReactWidget // __@jupyterlab_apputils.ReactWidget

    //AMO: Global export
    type [<AllowNullLiteral>] IExports =
        abstract Placeholder: PlaceholderStatic
        abstract InputPlaceholder: InputPlaceholderStatic
        abstract OutputPlaceholder: OutputPlaceholderStatic

    /// An abstract base class for placeholders
    /// 
    /// ### Notes
    /// A placeholder is the element that is shown when input/output
    /// is hidden.
    type [<AllowNullLiteral>] Placeholder =
        inherit ReactWidget
        /// Handle the click event.
        // abstract handleClick: e: React.MouseEvent<HTMLDivElement> -> unit
        abstract handleClick: e: MouseEvent<HTMLDivElement> -> unit

    /// An abstract base class for placeholders
    /// 
    /// ### Notes
    /// A placeholder is the element that is shown when input/output
    /// is hidden.
    type [<AllowNullLiteral>] PlaceholderStatic =
        /// Construct a new placeholder.
        // [<Emit "new $0($1...)">] abstract Create: callback: (React.MouseEvent<HTMLDivElement> -> unit) -> Placeholder
        [<Emit "new $0($1...)">] abstract Create: callback: (MouseEvent<HTMLDivElement> -> unit) -> Placeholder

    /// The input placeholder class.
    type [<AllowNullLiteral>] InputPlaceholder =
        inherit Placeholder
        /// Render the input placeholder using the virtual DOM.
        // abstract render: unit -> ResizeArray<React.ReactElement<obj option>>
        abstract render: unit -> ResizeArray<ReactElement<obj option>>

    /// The input placeholder class.
    type [<AllowNullLiteral>] InputPlaceholderStatic =
        /// Construct a new input placeholder.
        // [<Emit "new $0($1...)">] abstract Create: callback: (React.MouseEvent<HTMLDivElement> -> unit) -> InputPlaceholder
        [<Emit "new $0($1...)">] abstract Create: callback: (MouseEvent<HTMLDivElement> -> unit) -> InputPlaceholder

    /// The output placeholder class.
    type [<AllowNullLiteral>] OutputPlaceholder =
        inherit Placeholder
        /// Render the output placeholder using the virtual DOM.
        // abstract render: unit -> ResizeArray<React.ReactElement<obj option>>
        abstract render: unit -> ResizeArray<ReactElement<obj option>>

    /// The output placeholder class.
    type [<AllowNullLiteral>] OutputPlaceholderStatic =
        /// Construct a new output placeholder.
        // [<Emit "new $0($1...)">] abstract Create: callback: (React.MouseEvent<HTMLDivElement> -> unit) -> OutputPlaceholder
        [<Emit "new $0($1...)">] abstract Create: callback: (MouseEvent<HTMLDivElement> -> unit) -> OutputPlaceholder

module Widget =
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    type IChangedArgs<'T> = JupyterlabCoreutils.Interfaces.IChangedArgs<'T> // __@jupyterlab_coreutils.IChangedArgs
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    // type CodeEditorWrapper = JupyterlabCodeeditor.Widget.CodeEditorWrapper // __@jupyterlab_codeeditor.CodeEditorWrapper
    type IObservableMap<'T> = JupyterlabObservables.Observablemap.IObservableMap<'T> // __@jupyterlab_observables.IObservableMap
    type OutputArea = JupyterlabOutputarea.Widget.OutputArea // __@jupyterlab_outputarea.OutputArea
    type IOutputPrompt = JupyterlabOutputarea.Widget.IOutputAreaModel // __@jupyterlab_outputarea.IOutputPrompt
    type IStdin = JupyterlabOutputarea.Widget.IStdin // __@jupyterlab_outputarea.IStdin
    type Stdin = JupyterlabOutputarea.Widget.Stdin  // __@jupyterlab_outputarea.Stdin
    type IRenderMimeRegistry = JupyterlabRendermime.Registry.IRenderMimeRegistry // __@jupyterlab_rendermime.IRenderMimeRegistry
    // type KernelMessage = JupyterlabServices.__kernel_messages.KernelMessage // __@jupyterlab_services.KernelMessage
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type ICellHeader = Headerfooter.ICellHeader
    type ICellFooter = Headerfooter.ICellFooter
    type InputArea = Inputarea.InputArea
    type IInputPrompt = Inputarea.IInputPrompt
    type IAttachmentsCellModel = Model.IAttachmentsCellModel
    type ICellModel = Model.ICellModel
    type ICodeCellModel = Model.ICodeCellModel
    type IMarkdownCellModel = Model.IMarkdownCellModel
    type IRawCellModel = Model.IRawCellModel

    //AMO: global export
    type [<AllowNullLiteral>] IExports =
        abstract Cell: CellStatic
        abstract CodeCell: CodeCellStatic
        abstract AttachmentsCell: AttachmentsCellStatic
        abstract MarkdownCell: MarkdownCellStatic
        abstract RawCell: RawCellStatic

    /// A base cell widget.
    /// The namespace for the `Cell` class statics.
    type [<AllowNullLiteral>] Cell =
        inherit Widget
        /// Initialize view state from model.
        /// 
        /// #### Notes
        /// Should be called after construction. For convenience, returns this, so it
        /// can be chained in the construction, like `new Foo().initializeState();`
        abstract initializeState: unit -> Cell
        /// The content factory used by the widget.
        abstract contentFactory: Cell.IContentFactory
        /// Get the prompt node used by the cell.
        abstract promptNode: HTMLElement
        /// Get the CodeEditorWrapper used by the cell.
        abstract editorWidget: JupyterlabCodeeditor.Widget.CodeEditorWrapper
        /// Get the CodeEditor used by the cell.
        abstract editor: JupyterlabCodeeditor.Editor.CodeEditor.IEditor
        /// Get the model used by the cell.
        abstract model: ICellModel
        /// Get the input area for the cell.
        abstract inputArea: InputArea
        /// The read only state of the cell.
        abstract readOnly: bool with get, set
        /// Save view editable state to model
        abstract saveEditableState: unit -> unit
        /// Load view editable state from model.
        abstract loadEditableState: unit -> unit
        /// A promise that resolves when the widget renders for the first time.
        abstract ready: Promise<unit>
        /// Set the prompt for the widget.
        abstract setPrompt: value: string -> unit
        /// The view state of input being hidden.
        abstract inputHidden: bool with get, set
        /// Save view collapse state to model
        abstract saveCollapseState: unit -> unit
        /// Revert view collapse state from model.
        abstract loadCollapseState: unit -> unit
        /// Handle the input being hidden.
        /// 
        /// #### Notes
        /// This is called by the `inputHidden` setter so that subclasses
        /// can perform actions upon the input being hidden without accessing
        /// private state.
        abstract handleInputHidden: value: bool -> unit
        /// Whether to sync the collapse state to the cell model.
        abstract syncCollapse: bool with get, set
        /// Whether to sync the editable state to the cell model.
        abstract syncEditable: bool with get, set
        /// Clone the cell, using the same model.
        abstract clone: unit -> Cell
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit
        /// Handle `after-attach` messages.
        abstract onAfterAttach: msg: Message -> unit
        /// Handle `'activate-request'` messages.
        abstract onActivateRequest: msg: Message -> unit
        /// Handle `fit-request` messages.
        abstract onFitRequest: msg: Message -> unit
        /// Handle `update-request` messages.
        abstract onUpdateRequest: msg: Message -> unit
        /// Handle changes in the metadata.
        abstract onMetadataChanged: model: IObservableMap<JSONValue> * args: JupyterlabObservables.Observablemap.IObservableMap.IChangedArgs<JSONValue> -> unit

    /// A base cell widget.
    /// The namespace for the `Cell` class statics.
    type [<AllowNullLiteral>] CellStatic =
        /// Construct a new base cell widget.
        [<Emit "new $0($1...)">] abstract Create: options: Cell.IOptions -> Cell

    module Cell =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// An options object for initializing a cell widget.
        type [<AllowNullLiteral>] IOptions =
            /// The model used by the cell.
            abstract model: ICellModel with get, set
            /// The factory object for customizable cell children.
            abstract contentFactory: IContentFactory option with get, set
            /// The configuration options for the text editor widget.
            abstract editorConfig: obj option with get, set
            /// Whether to send an update request to the editor when it is shown.
            abstract updateEditorOnShow: bool option with get, set

        /// The factory object for customizable cell children.
        /// 
        /// This is used to allow users of cells to customize child content.
        /// 
        /// This inherits from `OutputArea.IContentFactory` to avoid needless nesting and
        /// provide a single factory object for all notebook/cell/outputarea related
        /// widgets.
        type [<AllowNullLiteral>] IContentFactory =
            inherit JupyterlabOutputarea.Widget.OutputArea.IContentFactory
            inherit Inputarea.InputArea.IContentFactory
            /// Create a new cell header for the parent widget.
            abstract createCellHeader: unit -> ICellHeader
            /// Create a new cell header for the parent widget.
            abstract createCellFooter: unit -> ICellFooter

        /// The default implementation of an `IContentFactory`.
        /// 
        /// This includes a CodeMirror editor factory to make it easy to use out of the box.
        /// A namespace for cell content factory.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IContentFactory
            /// The readonly editor factory that create code editors
            abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory
            /// Create a new cell header for the parent widget.
            abstract createCellHeader: unit -> ICellHeader
            /// Create a new cell header for the parent widget.
            abstract createCellFooter: unit -> ICellFooter
            /// Create an input prompt.
            abstract createInputPrompt: unit -> IInputPrompt
            /// Create the output prompt for the widget.
            abstract createOutputPrompt: unit -> IOutputPrompt
            /// Create an stdin widget.
            abstract createStdin: options:JupyterlabOutputarea.Widget.Stdin.IOptions -> IStdin

        /// The default implementation of an `IContentFactory`.
        /// 
        /// This includes a CodeMirror editor factory to make it easy to use out of the box.
        /// A namespace for cell content factory.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            /// Create a content factory for a cell.
            [<Emit "new $0($1...)">] abstract Create: ?options: ContentFactory.IOptions -> ContentFactory

        module ContentFactory =

            /// Options for the content factory.
            type [<AllowNullLiteral>] IOptions =
                /// The editor factory used by the content factory.
                /// 
                /// If this is not passed, a default CodeMirror editor factory
                /// will be used.
                abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory option with get, set

    /// A widget for a code cell.
    /// The namespace for the `CodeCell` class statics.
    type [<AllowNullLiteral>] CodeCell =
        inherit Cell
        /// The model used by the widget.
        abstract model: ICodeCellModel
        /// Initialize view state from model.
        /// 
        /// #### Notes
        /// Should be called after construction. For convenience, returns this, so it
        /// can be chained in the construction, like `new Foo().initializeState();`
        abstract initializeState: unit -> CodeCell
        /// Get the output area for the cell.
        abstract outputArea: OutputArea
        /// The view state of output being collapsed.
        abstract outputHidden: bool with get, set
        /// Save view collapse state to model
        abstract saveCollapseState: unit -> unit
        /// Revert view collapse state from model.
        /// 
        /// We consider the `collapsed` metadata key as the source of truth for outputs
        /// being hidden.
        abstract loadCollapseState: unit -> unit
        /// Whether the output is in a scrolled state?
        abstract outputsScrolled: bool with get, set
        /// Save view collapse state to model
        abstract saveScrolledState: unit -> unit
        /// Revert view collapse state from model.
        abstract loadScrolledState: unit -> unit
        /// Whether to sync the scrolled state to the cell model.
        abstract syncScrolled: bool with get, set
        /// Handle the input being hidden.
        /// 
        /// #### Notes
        /// This method is called by the case cell implementation and is
        /// subclasses here so the code cell can watch to see when input
        /// is hidden without accessing private state.
        abstract handleInputHidden: value: bool -> unit
        /// Clone the cell, using the same model.
        abstract clone: unit -> CodeCell
        /// Clone the OutputArea alone, returning a simplified output area, using the same model.
        abstract cloneOutputArea: unit -> OutputArea
        /// Dispose of the resources used by the widget.
        abstract dispose: unit -> unit
        /// Handle changes in the model.
        abstract onStateChanged: model: ICellModel * args:  JupyterlabCoreutils.Interfaces.IChangedArgs<obj option> -> unit
        /// Handle changes in the metadata.
        abstract onMetadataChanged: model: IObservableMap<JSONValue> * args: JupyterlabObservables.Observablemap.IObservableMap.IChangedArgs<JSONValue> -> unit

    /// A widget for a code cell.
    /// The namespace for the `CodeCell` class statics.
    type [<AllowNullLiteral>] CodeCellStatic =
        /// Construct a code cell widget.
        [<Emit "new $0($1...)">] abstract Create: options: CodeCell.IOptions -> CodeCell

    module CodeCell =

        type [<AllowNullLiteral>] IExports =
            /// Execute a cell given a client session.
            abstract execute: cell: CodeCell * session: IClientSession * ?metadata: JSONObject -> Promise<U2<JupyterlabServices.__kernel_messages.KernelMessage.IExecuteReplyMsg, unit>>

        /// An options object for initializing a base cell widget.
        type [<AllowNullLiteral>] IOptions =
            inherit Cell.IOptions
            /// The model used by the cell.
            abstract model: ICodeCellModel with get, set
            /// The mime renderer for the cell widget.
            abstract rendermime: IRenderMimeRegistry with get, set

    /// `AttachmentsCell` - A base class for a cell widget that allows
    ///   attachments to be drag/drop'd or pasted onto it
    type [<AllowNullLiteral>] AttachmentsCell =
        inherit Cell
        /// <summary>Handle the DOM events for the widget.</summary>
        /// <param name="event">- The DOM event sent to the widget.
        /// 
        /// #### Notes
        /// This method implements the DOM `EventListener` interface and is
        /// called in response to events on the notebook panel's node. It should
        /// not be called directly by user code.</param>
        abstract handleEvent: ``event``: Event -> unit
        /// Modify the cell source to include a reference to the attachment.
        abstract updateCellSourceWithAttachment: attachmentName: string -> unit
        /// Handle `after-attach` messages for the widget.
        abstract onAfterAttach: msg: Message -> unit
        /// A message handler invoked on a `'before-detach'`
        /// message
        abstract onBeforeDetach: msg: Message -> unit
        /// The model used by the widget.
        abstract model: IAttachmentsCellModel

    /// `AttachmentsCell` - A base class for a cell widget that allows
    ///   attachments to be drag/drop'd or pasted onto it
    type [<AllowNullLiteral>] AttachmentsCellStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> AttachmentsCell

    /// A widget for a Markdown cell.
    /// 
    /// #### Notes
    /// Things get complicated if we want the rendered text to update
    /// any time the text changes, the text editor model changes,
    /// or the input area model changes.  We don't support automatically
    /// updating the rendered text in all of these cases.
    /// The namespace for the `CodeCell` class statics.
    type [<AllowNullLiteral>] MarkdownCell =
        inherit AttachmentsCell
        /// The model used by the widget.
        abstract model: IMarkdownCellModel
        /// A promise that resolves when the widget renders for the first time.
        abstract ready: Promise<unit>
        /// Whether the cell is rendered.
        abstract rendered: bool with get, set
        /// Render an input instead of the text editor.
        abstract renderInput: widget: Widget -> unit
        /// Show the text editor instead of rendered input.
        abstract showEditor: unit -> unit
        abstract onUpdateRequest: msg: Message -> unit
        /// Modify the cell source to include a reference to the attachment.
        abstract updateCellSourceWithAttachment: attachmentName: string -> unit
        /// Clone the cell, using the same model.
        abstract clone: unit -> MarkdownCell

    /// A widget for a Markdown cell.
    /// 
    /// #### Notes
    /// Things get complicated if we want the rendered text to update
    /// any time the text changes, the text editor model changes,
    /// or the input area model changes.  We don't support automatically
    /// updating the rendered text in all of these cases.
    /// The namespace for the `CodeCell` class statics.
    type [<AllowNullLiteral>] MarkdownCellStatic =
        /// Construct a Markdown cell widget.
        [<Emit "new $0($1...)">] abstract Create: options: MarkdownCell.IOptions -> MarkdownCell

    module MarkdownCell =

        /// An options object for initializing a base cell widget.
        type [<AllowNullLiteral>] IOptions =
            inherit Cell.IOptions
            /// The model used by the cell.
            abstract model: IMarkdownCellModel with get, set
            /// The mime renderer for the cell widget.
            abstract rendermime: IRenderMimeRegistry with get, set

    /// A widget for a raw cell.
    /// The namespace for the `RawCell` class statics.
    type [<AllowNullLiteral>] RawCell =
        inherit Cell
        /// Clone the cell, using the same model.
        abstract clone: unit -> RawCell
        /// The model used by the widget.
        abstract model: IRawCellModel

    /// A widget for a raw cell.
    /// The namespace for the `RawCell` class statics.
    type [<AllowNullLiteral>] RawCellStatic =
        /// Construct a raw cell widget.
        [<Emit "new $0($1...)">] abstract Create: options: Cell.IOptions -> RawCell

    module RawCell =

        /// An options object for initializing a base cell widget.
        type [<AllowNullLiteral>] IOptions =
            inherit Cell.IOptions
            /// The model used by the cell.
            abstract model: IRawCellModel with get, set
