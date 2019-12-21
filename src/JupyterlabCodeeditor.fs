// ts2fable 0.0.0
module rec JupyterlabCodeeditor
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//end typecript

module Editor =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IChangedArgs<'T> = JupyterlabCoreutils.Interfaces.IChangedArgs<'T> // __@jupyterlab_coreutils.IChangedArgs
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    type IObservableMap<'T> = JupyterlabObservables.Observablemap.IObservableMap<'T> // __@jupyterlab_observables.IObservableMap
    type IObservableString = JupyterlabObservables.Observablestring.IObservableString // __@jupyterlab_observables.IObservableString

    module CodeEditor =

        type [<AllowNullLiteral>] IExports =
            abstract defaultSelectionStyle: ISelectionStyle
            abstract Model: ModelStatic
            abstract defaultConfig: IConfig

        /// A zero-based position in the editor.
        type [<AllowNullLiteral>] IPosition =
            inherit JSONObject
            /// The cursor line number.
            abstract line: float
            /// The cursor column number.
            abstract column: float

        /// The dimension of an element.
        type [<AllowNullLiteral>] IDimension =
            /// The width of an element in pixels.
            abstract width: float
            /// The height of an element in pixels.
            abstract height: float

        /// An interface describing editor state coordinates.
        type [<AllowNullLiteral>] ICoordinate =
            inherit JSONObject
            inherit ClientRect

        /// A range.
        type [<AllowNullLiteral>] IRange =
            inherit JSONObject
            /// The position of the first character in the current range.
            /// 
            /// #### Notes
            /// If this position is greater than [end] then the range is considered
            /// to be backward.
            abstract start: IPosition
            /// The position of the last character in the current range.
            /// 
            /// #### Notes
            /// If this position is less than [start] then the range is considered
            /// to be backward.
            abstract ``end``: IPosition

        /// A selection style.
        type [<AllowNullLiteral>] ISelectionStyle =
            inherit JSONObject
            /// A class name added to a selection.
            abstract className: string with get, set
            /// A display name added to a selection.
            abstract displayName: string with get, set
            /// A color for UI elements.
            abstract color: string with get, set

        /// A text selection.
        type [<AllowNullLiteral>] ITextSelection =
            inherit IRange
            /// The uuid of the text selection owner.
            abstract uuid: string
            /// The style of this selection.
            abstract style: ISelectionStyle

        /// An interface for a text token, such as a word, keyword, or variable.
        type [<AllowNullLiteral>] IToken =
            /// The value of the token.
            abstract value: string with get, set
            /// The offset of the token in the code editor.
            abstract offset: float with get, set
            /// An optional type for the token.
            abstract ``type``: string option with get, set

        /// An interface to manage selections by selection owners.
        /// 
        /// #### Definitions
        /// - a user code that has an associated uuid is called a selection owner, see `CodeEditor.ISelectionOwner`
        /// - a selection belongs to a selection owner only if it is associated with the owner by an uuid, see `CodeEditor.ITextSelection`
        /// 
        /// #### Read access
        /// - any user code can observe any selection
        /// 
        /// #### Write access
        /// - if a user code is a selection owner then:
        ///    - it can change selections belonging to it
        ///    - but it must not change selections belonging to other selection owners
        /// - otherwise it must not change any selection
        /// An editor model.
        type [<AllowNullLiteral>] IModel =
            inherit IDisposable
            /// A signal emitted when a property changes.
            abstract mimeTypeChanged: ISignal<IModel, IChangedArgs<string>> with get, set
            /// The text stored in the model.
            abstract value: IObservableString
            /// A mime type of the model.
            /// 
            /// #### Notes
            /// It is never `null`, the default mime type is `text/plain`.
            abstract mimeType: string with get, set
            /// The currently selected code.
            abstract selections: IObservableMap<ResizeArray<ITextSelection>>
            /// The underlying `IModelDB` instance in which model
            /// data is stored.
            abstract modelDB: IModelDB

        /// The default implementation of the editor model.
        type [<AllowNullLiteral>] Model =
            inherit IModel
            /// The underlying `IModelDB` instance in which model
            /// data is stored.
            abstract modelDB: IModelDB
            /// A signal emitted when a mimetype changes.
            abstract mimeTypeChanged: ISignal<Model, IChangedArgs<string>>
            /// Get the value of the model.
            abstract value: IObservableString
            /// Get the selections for the model.
            abstract selections: IObservableMap<ResizeArray<ITextSelection>>
            /// A mime type of the model.
            abstract mimeType: string with get, set
            /// Whether the model is disposed.
            abstract isDisposed: bool
            /// Dispose of the resources used by the model.
            abstract dispose: unit -> unit

        /// The default implementation of the editor model.
        type [<AllowNullLiteral>] ModelStatic =
            /// Construct a new Model.
            [<Emit "new $0($1...)">] abstract Create: ?options: Model.IOptions -> Model

        /// A selection owner.
        type [<AllowNullLiteral>] ISelectionOwner =
            /// The uuid of this selection owner.
            abstract uuid: string with get, set
            /// Returns the primary position of the cursor, never `null`.
            abstract getCursorPosition: unit -> IPosition
            /// <summary>Set the primary position of the cursor.</summary>
            /// <param name="position">- The new primary position.
            /// 
            /// #### Notes
            /// This will remove any secondary cursors.</param>
            abstract setCursorPosition: position: IPosition -> unit
            /// Returns the primary selection, never `null`.
            abstract getSelection: unit -> IRange
            /// <summary>Set the primary selection.</summary>
            /// <param name="selection">- The desired selection range.
            /// 
            /// #### Notes
            /// This will remove any secondary cursors.</param>
            abstract setSelection: selection: IRange -> unit
            /// Gets the selections for all the cursors, never `null` or empty.
            abstract getSelections: unit -> ResizeArray<IRange>
            /// <summary>Sets the selections for all the cursors.</summary>
            /// <param name="selections">- The new selections.
            /// 
            /// #### Notes
            /// Cursors will be removed or added, as necessary.
            /// Passing an empty array resets a cursor position to the start of a
            /// document.</param>
            abstract setSelections: selections: ResizeArray<IRange> -> unit

        type [<AllowNullLiteral>] KeydownHandler =
            [<Emit "$0($1...)">] abstract Invoke: instance: IEditor * ``event``: KeyboardEvent -> bool

        type [<StringEnum>] [<RequireQualifiedAccess>] EdgeLocation =
            | Top
            | TopLine
            | Bottom

        /// A widget that provides a code editor.
        type [<AllowNullLiteral>] IEditor =
            inherit ISelectionOwner
            inherit IDisposable
            /// A signal emitted when either the top or bottom edge is requested.
            abstract edgeRequested: ISignal<IEditor, EdgeLocation>
            /// The default selection style for the editor.
            abstract selectionStyle: CodeEditor.ISelectionStyle with get, set
            /// The DOM node that hosts the editor.
            abstract host: HTMLElement
            /// The model used by the editor.
            abstract model: IModel
            /// The height of a line in the editor in pixels.
            abstract lineHeight: float
            /// The widget of a character in the editor in pixels.
            abstract charWidth: float
            /// Get the number of lines in the eidtor.
            abstract lineCount: float
            /// Get a config option for the editor.
            abstract getOption: option: 'K -> IConfig
            /// Set a config option for the editor.
            abstract setOption: option: 'K * value: IConfig -> unit
            /// <summary>Returns the content for the given line number.</summary>
            /// <param name="line">- The line of interest.</param>
            abstract getLine: line: float -> string option
            /// <summary>Find an offset for the given position.</summary>
            /// <param name="position">- The position of interest.</param>
            abstract getOffsetAt: position: IPosition -> float
            /// <summary>Find a position for the given offset.</summary>
            /// <param name="offset">- The offset of interest.</param>
            abstract getPositionAt: offset: float -> IPosition option
            /// Undo one edit (if any undo events are stored).
            abstract undo: unit -> unit
            /// Redo one undone edit.
            abstract redo: unit -> unit
            /// Clear the undo history.
            abstract clearHistory: unit -> unit
            /// Brings browser focus to this editor text.
            abstract focus: unit -> unit
            /// Test whether the editor has keyboard focus.
            abstract hasFocus: unit -> bool
            /// Explicitly blur the editor.
            abstract blur: unit -> unit
            /// Repaint the editor.
            /// 
            /// #### Notes
            /// A repainted editor should fit to its host node.
            abstract refresh: unit -> unit
            /// Resize the editor to fit its host node.
            abstract resizeToFit: unit -> unit
            /// <summary>Add a keydown handler to the editor.</summary>
            /// <param name="handler">- A keydown handler.</param>
            abstract addKeydownHandler: handler: KeydownHandler -> IDisposable
            /// <summary>Set the size of the editor.</summary>
            /// <param name="size">- The desired size.
            /// 
            /// #### Notes
            /// Use `null` if the size is unknown.</param>
            abstract setSize: size: IDimension option -> unit
            /// <summary>Reveals the given position in the editor.</summary>
            /// <param name="position">- The desired position to reveal.</param>
            abstract revealPosition: position: IPosition -> unit
            /// Reveals the given selection in the editor.
            abstract revealSelection: selection: IRange -> unit
            /// <summary>Get the window coordinates given a cursor position.</summary>
            /// <param name="position">- The desired position.</param>
            abstract getCoordinateForPosition: position: IPosition -> ICoordinate
            /// <summary>Get the cursor position given window coordinates.</summary>
            /// <param name="coordinate">- The desired coordinate.</param>
            abstract getPositionForCoordinate: coordinate: ICoordinate -> IPosition option
            /// Inserts a new line at the cursor position and indents it.
            abstract newIndentedLine: unit -> unit
            /// Gets the token at a given position.
            abstract getTokenForPosition: position: IPosition -> IToken
            /// Gets the list of tokens for the editor model.
            abstract getTokens: unit -> ResizeArray<IToken>

        type [<AllowNullLiteral>] Factory =
            [<Emit "$0($1...)">] abstract Invoke: options: IOptions -> CodeEditor.IEditor

        /// The configuration options for an editor.
        type [<AllowNullLiteral>] IConfig =
            /// User preferred font family for text editors.
            abstract fontFamily: string option with get, set
            /// User preferred size in pixel of the font used in text editors.
            abstract fontSize: float option with get, set
            /// User preferred text line height, as a multiplier of font size.
            abstract lineHeight: float option with get, set
            /// Whether line numbers should be displayed.
            abstract lineNumbers: bool with get, set
            /// Control the line wrapping of the editor. Possible values are:
            /// - "off", lines will never wrap.
            /// - "on", lines will wrap at the viewport border.
            /// - "wordWrapColumn", lines will wrap at `wordWrapColumn`.
            /// - "bounded", lines will wrap at minimum between viewport width and wordWrapColumn.
            abstract lineWrap: U4<string, string, string, string> with get, set
            /// Whether the editor is read-only.
            abstract readOnly: bool with get, set
            /// The number of spaces a tab is equal to.
            abstract tabSize: float with get, set
            /// Whether to insert spaces when pressing Tab.
            abstract insertSpaces: bool with get, set
            /// Whether to highlight matching brackets when one of them is selected.
            abstract matchBrackets: bool with get, set
            /// Whether to automatically close brackets after opening them.
            abstract autoClosingBrackets: bool with get, set
            /// The column where to break text line.
            abstract wordWrapColumn: float with get, set
            /// Column index at which rulers should be added.
            abstract rulers: Array<float> with get, set
            /// Wheter to allow code folding
            abstract codeFolding: bool with get, set

        /// The options used to initialize an editor.
        type [<AllowNullLiteral>] IOptions =
            /// The host widget used by the editor.
            abstract host: HTMLElement with get, set
            /// The model used by the editor.
            abstract model: IModel with get, set
            /// The desired uuid for the editor.
            abstract uuid: string option with get, set
            /// The default selection style for the editor.
            abstract selectionStyle: obj option with get, set
            /// The configuration options for the editor.
            abstract config: obj option with get, set

        module Model =

            type [<AllowNullLiteral>] IOptions =
                /// The initial value of the model.
                abstract value: string option with get, set
                /// The mimetype of the model.
                abstract mimeType: string option with get, set
                /// An optional modelDB for storing model state.
                abstract modelDB: IModelDB option with get, set

module Factory =
    // type CodeEditor = Editor.CodeEditor

    /// The editor factory service interface.
    type [<AllowNullLiteral>] IEditorFactoryService =
        /// Create a new editor for inline code.
        abstract newInlineEditor: options: Editor.CodeEditor.IOptions -> Editor.CodeEditor.IEditor
        /// Create a new editor for a full document.
        abstract newDocumentEditor: options: Editor.CodeEditor.IOptions -> Editor.CodeEditor.IEditor

module Jsoneditor =
    type IObservableJSON = JupyterlabObservables.Observablejson.IObservableJSON // __@jupyterlab_observables.IObservableJSON
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    // type CodeEditor = Editor.CodeEditor

    type [<AllowNullLiteral>] IExports =
        abstract JSONEditor: JSONEditorStatic

    /// A widget for editing observable JSON.
    /// The static namespace JSONEditor class statics.
    type [<AllowNullLiteral>] JSONEditor =
        inherit Widget
        /// The code editor used by the editor.
        abstract editor: Editor.CodeEditor.IEditor
        /// The code editor model used by the editor.
        abstract model: Editor.CodeEditor.IModel
        /// The editor host node used by the JSON editor.
        abstract headerNode: HTMLDivElement
        /// The editor host node used by the JSON editor.
        abstract editorHostNode: HTMLDivElement
        /// The revert button used by the JSON editor.
        abstract revertButtonNode: HTMLSpanElement
        /// The commit button used by the JSON editor.
        abstract commitButtonNode: HTMLSpanElement
        /// The observable source.
        abstract source: IObservableJSON option with get, set
        /// Get whether the editor is dirty.
        abstract isDirty: bool
        /// <summary>Handle the DOM events for the widget.</summary>
        /// <param name="event">- The DOM event sent to the widget.
        /// 
        /// #### Notes
        /// This method implements the DOM `EventListener` interface and is
        /// called in response to events on the notebook panel's node. It should
        /// not be called directly by user code.</param>
        abstract handleEvent: ``event``: Event -> unit
        /// Handle `after-attach` messages for the widget.
        abstract onAfterAttach: msg: Message -> unit
        /// Handle `after-show` messages for the widget.
        abstract onAfterShow: msg: Message -> unit
        /// Handle `update-request` messages for the widget.
        abstract onUpdateRequest: msg: Message -> unit
        /// Handle `before-detach` messages for the widget.
        abstract onBeforeDetach: msg: Message -> unit

    /// A widget for editing observable JSON.
    /// The static namespace JSONEditor class statics.
    type [<AllowNullLiteral>] JSONEditorStatic =
        /// Construct a new JSON editor.
        [<Emit "new $0($1...)">] abstract Create: options: JSONEditor.IOptions -> JSONEditor

    module JSONEditor =

        /// The options used to initialize a json editor.
        type [<AllowNullLiteral>] IOptions =
            /// The editor factory used by the editor.
            abstract editorFactory: Editor.CodeEditor.Factory with get, set

module Mimetype =
    // type nbformat = JupyterlabCoreutils.Nbformat // __@jupyterlab_coreutils.nbformat

    /// The mime type service of a code editor.
    /// A namespace for `IEditorMimeTypeService`.
    type [<AllowNullLiteral>] IEditorMimeTypeService =
        /// <summary>Get a mime type for the given language info.</summary>
        /// <param name="info">- The language information.</param>
        abstract getMimeTypeByLanguage: info: JupyterlabCoreutils.Nbformat.Nbformat.ILanguageInfoMetadata -> string
        /// <summary>Get a mime type for the given file path.</summary>
        /// <param name="filePath">- The full path to the file.</param>
        abstract getMimeTypeByFilePath: filePath: string -> string

    module IEditorMimeTypeService =

        type [<AllowNullLiteral>] IExports =
            abstract defaultMimeType: string

module Tokens =
    type Token<'T> = PhosphorCoreutils.Token<'T> // __@phosphor_coreutils.Token
    type IEditorFactoryService = Factory.IEditorFactoryService
    type IEditorMimeTypeService = Mimetype.IEditorMimeTypeService

    type [<AllowNullLiteral>] IExports =
        abstract IEditorServices: Token<IEditorServices>

    /// Code editor services token.
    /// Code editor services.
    type [<AllowNullLiteral>] IEditorServices =
        /// The code editor factory.
        abstract factoryService: IEditorFactoryService
        /// The editor mime type service.
        abstract mimeTypeService: IEditorMimeTypeService

module Widget =
    type Message = PhosphorMessaging.Message //__@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget //__@phosphor_widgets.Widget
    // type CodeEditor = ____.CodeEditor

    type [<AllowNullLiteral>] IExports =
        abstract CodeEditorWrapper: CodeEditorWrapperStatic

    /// A widget which hosts a code editor.
    /// The namespace for the `CodeEditorWrapper` statics.
    type [<AllowNullLiteral>] CodeEditorWrapper =
        inherit Widget
        /// Get the editor wrapped by the widget.
        abstract editor: Editor.CodeEditor.IEditor
        /// Get the model used by the widget.
        abstract model: Editor.CodeEditor.IModel
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit
        /// <summary>Handle the DOM events for the widget.</summary>
        /// <param name="event">- The DOM event sent to the widget.
        /// 
        /// #### Notes
        /// This method implements the DOM `EventListener` interface and is
        /// called in response to events on the notebook panel's node. It should
        /// not be called directly by user code.</param>
        abstract handleEvent: ``event``: Event -> unit
        /// Handle `'activate-request'` messages.
        abstract onActivateRequest: msg: Message -> unit
        /// A message handler invoked on an `'after-attach'` message.
        abstract onAfterAttach: msg: Message -> unit
        /// Handle `before-detach` messages for the widget.
        abstract onBeforeDetach: msg: Message -> unit
        /// A message handler invoked on an `'after-show'` message.
        abstract onAfterShow: msg: Message -> unit
        /// A message handler invoked on a `'resize'` message.
        abstract onResize: msg: PhosphorWidgets.Widget.ResizeMessage -> unit
        /// A message handler invoked on an `'update-request'` message.
        abstract onUpdateRequest: msg: Message -> unit

    /// A widget which hosts a code editor.
    /// The namespace for the `CodeEditorWrapper` statics.
    type [<AllowNullLiteral>] CodeEditorWrapperStatic =
        /// Construct a new code editor widget.
        [<Emit "new $0($1...)">] abstract Create: options: CodeEditorWrapper.IOptions -> CodeEditorWrapper

    module CodeEditorWrapper =

        /// The options used to initialize a code editor widget.
        type [<AllowNullLiteral>] IOptions =
            /// A code editor factory.
            /// 
            /// #### Notes
            /// The widget needs a factory and a model instead of a `CodeEditor.IEditor`
            /// object because it needs to provide its own node as the host.
            abstract factory: Editor.CodeEditor.Factory with get, set
            /// The model used to initialize the code editor.
            abstract model: Editor.CodeEditor.IModel with get, set
            /// The desired uuid for the editor.
            abstract uuid: string option with get, set
            /// The configuration options for the editor.
            abstract config: obj option with get, set
            /// The default selection style for the editor.
            abstract selectionStyle: Editor.CodeEditor.ISelectionStyle option with get, set
            /// Whether to send an update request to the editor when it is shown.
            abstract updateOnShow: bool option with get, set
