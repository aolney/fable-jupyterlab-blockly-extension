// ts2fable 0.0.0
module rec JupyterlabDocregistry
open System
open Fable.Core
open Fable.Core.JS
open Fetch

//amo typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
type KeyboardEvent = Browser.Types.KeyboardEvent
//end typescript

module Context =
    // type Contents = JupyterlabServices.__contents_index.Contents. // __@jupyterlab_services.Contents
    type ServiceManager = JupyterlabServices.Manager.ServiceManager // __@jupyterlab_services.ServiceManager
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type ClientSession = JupyterlabApputils.ClientSession // __@jupyterlab_apputils.ClientSession
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    type ModelDB = JupyterlabObservables.Modeldb.ModelDB // __@jupyterlab_observables.ModelDB
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime
    type DocumentRegistry = Registry.DocumentRegistry

    type [<AllowNullLiteral>] IExports =
        abstract Context: ContextStatic

    /// An implementation of a document context.
    /// 
    /// This class is typically instantiated by the document manager.
    /// A namespace for `Context` statics.
    type [<AllowNullLiteral>] Context<'T> =
        inherit Registry.DocumentRegistry.IContext<'T>
        /// A signal emitted when the path changes.
        abstract pathChanged: ISignal<Context<'T>, string>
        /// A signal emitted when the model is saved or reverted.
        abstract fileChanged: ISignal<Context<'T>, JupyterlabServices.__contents_index.Contents.IModel>
        /// A signal emitted on the start and end of a saving operation.
        abstract saveState: ISignal<Context<'T>, Registry.DocumentRegistry.SaveState>
        /// A signal emitted when the context is disposed.
        abstract disposed: ISignal<Context<'T>, unit>
        /// Get the model associated with the document.
        abstract model: 'T
        /// The client session object associated with the context.
        abstract session: ClientSession
        /// The current path associated with the document.
        abstract path: string
        /// The current local path associated with the document.
        /// If the document is in the default notebook file browser,
        /// this is the same as the path.
        abstract localPath: string
        /// The current contents model associated with the document.
        /// 
        /// #### Notes
        /// The contents model will be null until the context is populated.
        /// It will have an  empty `contents` field.
        abstract contentsModel: JupyterlabServices.__contents_index.Contents.IModel option
        /// Get the model factory name.
        /// 
        /// #### Notes
        /// This is not part of the `IContext` API.
        abstract factoryName: string
        /// Test whether the context is disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the context.
        abstract dispose: unit -> unit
        /// Whether the context is ready.
        abstract isReady: bool
        /// A promise that is fulfilled when the context is ready.
        abstract ready: Promise<unit>
        /// The url resolver for the context.
        abstract urlResolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver
        /// <summary>Initialize the context.</summary>
        /// <param name="isNew">- Whether it is a new file.</param>
        abstract initialize: isNew: bool -> Promise<unit>
        /// Save the document contents to disk.
        abstract save: unit -> Promise<unit>
        /// Save the document to a different path chosen by the user.
        abstract saveAs: unit -> Promise<unit>
        /// Revert the document contents to disk contents.
        abstract revert: unit -> Promise<unit>
        /// Create a checkpoint for the file.
        abstract createCheckpoint: unit -> Promise<JupyterlabServices.__contents_index.Contents.ICheckpointModel>
        /// Delete a checkpoint for the file.
        abstract deleteCheckpoint: checkpointId: string -> Promise<unit>
        /// Restore the file to a known checkpoint state.
        abstract restoreCheckpoint: ?checkpointId: string -> Promise<unit>
        /// List available checkpoints for a file.
        abstract listCheckpoints: unit -> Promise<ResizeArray<JupyterlabServices.__contents_index.Contents.ICheckpointModel>>
        /// <summary>Add a sibling widget to the document manager.</summary>
        /// <param name="widget">- The widget to add to the document manager.</param>
        /// <param name="options">- The desired options for adding the sibling.</param>
        abstract addSibling: widget: Widget * ?options: Registry.DocumentRegistry.IOpenOptions -> IDisposable

    /// An implementation of a document context.
    /// 
    /// This class is typically instantiated by the document manager.
    /// A namespace for `Context` statics.
    type [<AllowNullLiteral>] ContextStatic =
        /// Construct a new document context.
        [<Emit "new $0($1...)">] abstract Create: options: Context.IOptions<'T> -> Context<'T>

    module Context =

        /// The options used to initialize a context.
        type [<AllowNullLiteral>] IOptions<'T> =
            /// A service manager instance.
            abstract manager: JupyterlabServices.Manager.ServiceManager.IManager with get, set
            /// The model factory used to create the model.
            abstract factory: Registry.DocumentRegistry.IModelFactory<'T> with get, set
            /// The initial path of the file.
            abstract path: string with get, set
            /// The kernel preference associated with the context.
            abstract kernelPreference: JupyterlabApputils.IClientSession.IKernelPreference option with get, set
            /// An IModelDB factory method which may be used for the document.
            abstract modelDBFactory: JupyterlabObservables.Modeldb.ModelDB.IFactory option with get, set
            /// An optional callback for opening sibling widgets.
            abstract opener: (Widget -> unit) option with get, set
            /// A function to call when the kernel is busy.
            abstract setBusy: (unit -> IDisposable) option with get, set

module Default =
    // type Contents = JupyterlabServices.Contents // __@jupyterlab_services.Contents
    type JSONValue = PhosphorCoreutils.JSONValue //  __@phosphor_coreutils.JSONValue
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Widget = PhosphorWidgets.Widget //__@phosphor_widgets.Widget
    type MainAreaWidget = JupyterlabApputils.MainAreaWidget // __@jupyterlab_apputils.MainAreaWidget
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    // type IChangedArgs = JupyterlabCoreutils.Interfaces.IChangedArgs // __@jupyterlab_coreutils.IChangedArgs
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    type DocumentRegistry = Registry.DocumentRegistry
    type IDocumentWidget = Registry.IDocumentWidget

    type [<AllowNullLiteral>] IExports =
        abstract DocumentModel: DocumentModelStatic
        abstract TextModelFactory: TextModelFactoryStatic
        abstract Base64ModelFactory: Base64ModelFactoryStatic
        abstract ABCWidgetFactory: ABCWidgetFactoryStatic
        abstract DocumentWidget: DocumentWidgetStatic

    /// The default implementation of a document model.
    type [<AllowNullLiteral>] DocumentModel =
        inherit  JupyterlabCodeeditor.Editor.CodeEditor.Model
        inherit Registry.DocumentRegistry.ICodeModel
        /// A signal emitted when the document content changes.
        abstract contentChanged: ISignal<DocumentModel, unit>
        /// A signal emitted when the document state changes.
        abstract stateChanged: ISignal<DocumentModel, JupyterlabCoreutils.Interfaces.IChangedArgs<obj option>>
        /// The dirty state of the document.
        abstract dirty: bool with get, set
        /// The read only state of the document.
        abstract readOnly: bool with get, set
        /// The default kernel name of the document.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract defaultKernelName: string
        /// The default kernel language of the document.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract defaultKernelLanguage: string
        /// Serialize the model to a string.
        abstract toString: unit -> string
        /// Deserialize the model from a string.
        /// 
        /// #### Notes
        /// Should emit a [contentChanged] signal.
        abstract fromString: value: string -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JSONValue
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// Should emit a [contentChanged] signal.
        abstract fromJSON: value: JSONValue -> unit
        /// Initialize the model with its current state.
        abstract initialize: unit -> unit
        /// Trigger a state change signal.
        abstract triggerStateChange: args: JupyterlabCoreutils.Interfaces.IChangedArgs<obj option> -> unit
        /// Trigger a content changed signal.
        abstract triggerContentChange: unit -> unit

    /// The default implementation of a document model.
    type [<AllowNullLiteral>] DocumentModelStatic =
        /// Construct a new document model.
        [<Emit "new $0($1...)">] abstract Create: ?languagePreference: string * ?modelDB: IModelDB -> DocumentModel

    /// An implementation of a model factory for text files.
    type [<AllowNullLiteral>] TextModelFactory =
        inherit Registry.DocumentRegistry.CodeModelFactory
        /// The name of the model type.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract name: string
        /// The type of the file.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract contentType: JupyterlabServices.__contents_index.Contents.ContentType
        /// The format of the file.
        /// 
        /// This is a read-only property.
        abstract fileFormat: JupyterlabServices.__contents_index.Contents.FileFormat
        /// Get whether the model factory has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the model factory.
        abstract dispose: unit -> unit
        /// <summary>Create a new model.</summary>
        /// <param name="languagePreference">- An optional kernel language preference.</param>
        abstract createNew: ?languagePreference: string * ?modelDB: IModelDB -> Registry.DocumentRegistry.ICodeModel
        /// Get the preferred kernel language given a file path.
        abstract preferredLanguage: path: string -> string

    /// An implementation of a model factory for text files.
    type [<AllowNullLiteral>] TextModelFactoryStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> TextModelFactory

    /// An implementation of a model factory for base64 files.
    type [<AllowNullLiteral>] Base64ModelFactory =
        inherit TextModelFactory
        /// The name of the model type.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract name: string
        /// The type of the file.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract contentType: JupyterlabServices.__contents_index.Contents.ContentType
        /// The format of the file.
        /// 
        /// This is a read-only property.
        abstract fileFormat: JupyterlabServices.__contents_index.Contents.FileFormat

    /// An implementation of a model factory for base64 files.
    type [<AllowNullLiteral>] Base64ModelFactoryStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Base64ModelFactory

    type ABCWidgetFactory<'U> =
        ABCWidgetFactory<obj, 'U>

    /// The default implementation of a widget factory.
    type [<AllowNullLiteral>] ABCWidgetFactory<'T, 'U> =
        inherit Registry.DocumentRegistry.IWidgetFactory<'T, 'U>
        /// A signal emitted when a widget is created.
        abstract widgetCreated: ISignal<Registry.DocumentRegistry.IWidgetFactory<'T, 'U>, 'T>
        /// Get whether the model factory has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the document manager.
        abstract dispose: unit -> unit
        /// Whether the widget factory is read only.
        abstract readOnly: bool
        /// The name of the widget to display in dialogs.
        abstract name: string
        /// The file types the widget can view.
        abstract fileTypes: ResizeArray<string>
        /// The registered name of the model type used to create the widgets.
        abstract modelName: string
        /// The file types for which the factory should be the default.
        abstract defaultFor: ResizeArray<string>
        /// The file types for which the factory should be the default for
        /// rendering a document model, if different from editing.
        abstract defaultRendered: ResizeArray<string>
        /// Whether the widgets prefer having a kernel started.
        abstract preferKernel: bool
        /// Whether the widgets can start a kernel when opened.
        abstract canStartKernel: bool
        /// Whether the kernel should be shutdown when the widget is closed.
        abstract shutdownOnClose: bool with get, set
        /// Create a new widget given a document model and a context.
        /// 
        /// #### Notes
        /// It should emit the [widgetCreated] signal with the new widget.
        abstract createNew: context: Registry.DocumentRegistry.IContext<'U> * ?source: 'T -> 'T
        /// Create a widget for a context.
        abstract createNewWidget: context: Registry.DocumentRegistry.IContext<'U> * ?source: 'T -> 'T
        /// Default factory for toolbar items to be added after the widget is created.
        abstract defaultToolbarFactory: widget: 'T -> ResizeArray<Registry.DocumentRegistry.IToolbarItem>

    /// The default implementation of a widget factory.
    type [<AllowNullLiteral>] ABCWidgetFactoryStatic =
        /// Construct a new `ABCWidgetFactory`.
        [<Emit "new $0($1...)">] abstract Create: options: Registry.DocumentRegistry.IWidgetFactoryOptions<'T> -> ABCWidgetFactory<'T, 'U>

    type DocumentWidget<'U> =
        DocumentWidget<obj, 'U>

    type DocumentWidget =
        DocumentWidget<obj, obj>

    /// A document widget implementation.
    type [<AllowNullLiteral>] DocumentWidget<'T, 'U> =
        inherit JupyterlabApputils.MainAreaWidget<'T>
        inherit Registry.IDocumentWidget<'T, 'U>
        /// Set URI fragment identifier.
        abstract setFragment: fragment: string -> unit
        abstract context: Registry.DocumentRegistry.IContext<'U>

    /// A document widget implementation.
    type [<AllowNullLiteral>] DocumentWidgetStatic =
        [<Emit "new $0($1...)">] abstract Create: options: DocumentWidget.IOptions<'T, 'U> -> DocumentWidget<'T, 'U>

    module DocumentWidget =

        type IOptions<'U> =
            IOptions<obj, 'U>

        type IOptions =
            IOptions<obj, obj>

        type [<AllowNullLiteral>] IOptions<'T, 'U> =
            inherit JupyterlabApputils.MainAreaWidget.IOptions<'T>
            abstract context: Registry.DocumentRegistry.IContext<'U> with get, set

        type IOptionsOptionalContent<'U> =
            IOptionsOptionalContent<obj, 'U>

        type IOptionsOptionalContent =
            IOptionsOptionalContent<obj, obj>

        type [<AllowNullLiteral>] IOptionsOptionalContent<'T, 'U> =
            inherit JupyterlabApputils.MainAreaWidget.IOptionsOptionalContent<'T>
            abstract context: Registry.DocumentRegistry.IContext<'U> with get, set

module Mimedocument =
    // type Printing = JupyterlabApputils.Printing // __@jupyterlab_apputils.Printing
    // type IRenderMime = JupyterlabRendermime.IRenderMime // __@jupyterlab_rendermime.IRenderMime
    // type IRenderMimeRegistry = JupyterlabRendermime.IRenderMimeRegistry // @jupyterlab_rendermime.IRenderMimeRegistry
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type ABCWidgetFactory<'T> = Default.ABCWidgetFactory<'T>
    type DocumentWidget = Default.DocumentWidget
    type DocumentRegistry = Registry.DocumentRegistry

    type [<AllowNullLiteral>] IExports =
        abstract MimeContent: MimeContentStatic
        abstract MimeDocument: MimeDocumentStatic
        abstract MimeDocumentFactory: MimeDocumentFactoryStatic

    /// A content widget for a rendered mimetype document.
    /// The namespace for MimeDocument class statics.
    type [<AllowNullLiteral>] MimeContent =
        inherit Widget
        /// The mimetype for this rendered content.
        abstract mimeType: string
        /// Print method. Defered to the renderer.
        abstract ``[Printing.symbol]``: unit -> JupyterlabApputils.Printing.OptionalAsyncThunk
        /// A promise that resolves when the widget is ready.
        abstract ready: Promise<unit>
        /// Set URI fragment identifier.
        abstract setFragment: fragment: string -> unit
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit
        /// Handle an `update-request` message to the widget.
        abstract onUpdateRequest: msg: Message -> unit
        abstract renderer: JupyterlabRendermimeInterfaces.IRenderMime.IRenderer

    /// A content widget for a rendered mimetype document.
    /// The namespace for MimeDocument class statics.
    type [<AllowNullLiteral>] MimeContentStatic =
        /// Construct a new widget.
        [<Emit "new $0($1...)">] abstract Create: options: MimeContent.IOptions -> MimeContent

    module MimeContent =

        /// The options used to initialize a MimeDocument.
        type [<AllowNullLiteral>] IOptions =
            /// Context
            abstract context: Registry.DocumentRegistry.IContext<Registry.DocumentRegistry.IModel> with get, set
            /// The renderer instance.
            abstract renderer: JupyterlabRendermimeInterfaces.IRenderMime.IRenderer with get, set
            /// The mime type.
            abstract mimeType: string with get, set
            /// The render timeout.
            abstract renderTimeout: float with get, set
            /// Preferred data type from the model.
            abstract dataType: U2<string, string> option with get, set

    /// A document widget for mime content.
    type [<AllowNullLiteral>] MimeDocument =
        inherit Default.DocumentWidget<MimeContent>
        abstract setFragment: fragment: string -> unit

    /// A document widget for mime content.
    type [<AllowNullLiteral>] MimeDocumentStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> MimeDocument

    /// An implementation of a widget factory for a rendered mimetype document.
    /// The namespace for MimeDocumentFactory class statics.
    type [<AllowNullLiteral>] MimeDocumentFactory =
        inherit ABCWidgetFactory<MimeDocument>
        /// Create a new widget given a context.
        abstract createNewWidget: context: Registry.DocumentRegistry.Context -> MimeDocument

    /// An implementation of a widget factory for a rendered mimetype document.
    /// The namespace for MimeDocumentFactory class statics.
    type [<AllowNullLiteral>] MimeDocumentFactoryStatic =
        /// Construct a new mimetype widget factory.
        [<Emit "new $0($1...)">] abstract Create: options: MimeDocumentFactory.IOptions<MimeDocument> -> MimeDocumentFactory

    module MimeDocumentFactory =

        /// The options used to initialize a MimeDocumentFactory.
        type [<AllowNullLiteral>] IOptions<'T> =
            inherit Registry.DocumentRegistry.IWidgetFactoryOptions<'T>
            /// The primary file type associated with the document.
            abstract primaryFileType: Registry.DocumentRegistry.IFileType with get, set
            /// The rendermime instance.
            abstract rendermime: JupyterlabRendermime.Registry.IRenderMimeRegistry with get, set
            /// The render timeout.
            abstract renderTimeout: float option with get, set
            /// Preferred data type from the model.
            abstract dataType: U2<string, string> option with get, set

module Registry =
    // type Contents = JupyterlabServices.__contents_index.Contents // __@jupyterlab_services.Contents
    // type Kernel = __@jupyterlab_services.Kernel
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __@phosphor_algorithm.IIterator
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type DockLayout = PhosphorWidgets.DockLayout // __@phosphor_widgets.DockLayout
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    type Toolbar = JupyterlabApputils.Toolbar // __@jupyterlab_apputils.Toolbar
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    type IChangedArgsGeneric<'T> = JupyterlabCoreutils.Interfaces.IChangedArgs<'T> // __@jupyterlab_coreutils.IChangedArgs
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime

    type [<AllowNullLiteral>] IExports =
        abstract DocumentRegistry: DocumentRegistryStatic

    /// The document registry.
    /// The namespace for the `DocumentRegistry` class statics.
    type [<AllowNullLiteral>] DocumentRegistry =
        inherit IDisposable
        /// A signal emitted when the registry has changed.
        abstract changed: ISignal<DocumentRegistry, DocumentRegistry.IChangedArgs>
        /// Get whether the document registry has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the document registery.
        abstract dispose: unit -> unit
        /// <summary>Add a widget factory to the registry.</summary>
        /// <param name="factory">- The factory instance to register.</param>
        abstract addWidgetFactory: factory: DocumentRegistry.WidgetFactory -> IDisposable
        /// <summary>Add a model factory to the registry.</summary>
        /// <param name="factory">- The factory instance.</param>
        abstract addModelFactory: factory: DocumentRegistry.ModelFactory -> IDisposable
        /// <summary>Add a widget extension to the registry.</summary>
        /// <param name="widgetName">- The name of the widget factory.</param>
        /// <param name="extension">- A widget extension.</param>
        abstract addWidgetExtension: widgetName: string * extension: DocumentRegistry.WidgetExtension -> IDisposable
        /// Add a file type to the document registry.
        abstract addFileType: fileType: obj -> IDisposable
        /// <summary>Get a list of the preferred widget factories.</summary>
        /// <param name="path">- The file path to filter the results.</param>
        abstract preferredWidgetFactories: path: string -> ResizeArray<DocumentRegistry.WidgetFactory>
        /// <summary>Get the default rendered widget factory for a path.</summary>
        /// <param name="path">- The path to for which to find a widget factory.</param>
        abstract defaultRenderedWidgetFactory: path: string -> DocumentRegistry.WidgetFactory
        /// <summary>Get the default widget factory for a path.</summary>
        /// <param name="path">- An optional file path to filter the results.</param>
        abstract defaultWidgetFactory: ?path: string -> DocumentRegistry.WidgetFactory
        /// <summary>Set overrides for the default widget factory for a file type.
        /// 
        /// Normally, a widget factory informs the document registry which file types
        /// it should be the default for using the `defaultFor` option in the
        /// IWidgetFactoryOptions. This function can be used to override that after
        /// the fact.</summary>
        /// <param name="fileType">: The name of the file type.</param>
        /// <param name="factory">: The name of the factory.
        /// 
        /// #### Notes
        /// If `factory` is undefined, then any override will be unset, and the
        /// default factory will revert to the original value.
        /// 
        /// If `factory` or `fileType` are not known to the docregistry, or
        /// if `factory` cannot open files of type `fileType`, this will throw
        /// an error.</param>
        abstract setDefaultWidgetFactory: fileType: string * factory: string option -> unit
        /// Create an iterator over the widget factories that have been registered.
        abstract widgetFactories: unit -> IIterator<DocumentRegistry.WidgetFactory>
        /// Create an iterator over the model factories that have been registered.
        abstract modelFactories: unit -> IIterator<DocumentRegistry.ModelFactory>
        /// <summary>Create an iterator over the registered extensions for a given widget.</summary>
        /// <param name="widgetName">- The name of the widget factory.</param>
        abstract widgetExtensions: widgetName: string -> IIterator<DocumentRegistry.WidgetExtension>
        /// Create an iterator over the file types that have been registered.
        abstract fileTypes: unit -> IIterator<DocumentRegistry.IFileType>
        /// <summary>Get a widget factory by name.</summary>
        /// <param name="widgetName">- The name of the widget factory.</param>
        abstract getWidgetFactory: widgetName: string -> DocumentRegistry.WidgetFactory option
        /// <summary>Get a model factory by name.</summary>
        /// <param name="name">- The name of the model factory.</param>
        abstract getModelFactory: name: string -> DocumentRegistry.ModelFactory option
        /// Get a file type by name.
        abstract getFileType: name: string -> DocumentRegistry.IFileType option
        /// <summary>Get a kernel preference.</summary>
        /// <param name="path">- The file path.</param>
        /// <param name="widgetName">- The name of the widget factory.</param>
        /// <param name="kernel">- An optional existing kernel model.</param>
        abstract getKernelPreference: path: string * widgetName: string * ?kernel: obj -> JupyterlabApputils.IClientSession.IKernelPreference option
        /// <summary>Get the best file type given a contents model.</summary>
        /// <param name="model">- The contents model of interest.</param>
        abstract getFileTypeForModel: model: obj -> DocumentRegistry.IFileType
        /// <summary>Get the file types that match a file name.</summary>
        /// <param name="path">- The path of the file.</param>
        abstract getFileTypesForPath: path: string -> ResizeArray<DocumentRegistry.IFileType>

    /// The document registry.
    /// The namespace for the `DocumentRegistry` class statics.
    type [<AllowNullLiteral>] DocumentRegistryStatic =
        /// Construct a new document registry.
        [<Emit "new $0($1...)">] abstract Create: ?options: DocumentRegistry.IOptions -> DocumentRegistry

    module DocumentRegistry =

        type [<AllowNullLiteral>] IExports =
            abstract fileTypeDefaults: IFileType
            abstract defaultTextFileType: IFileType
            abstract defaultNotebookFileType: IFileType
            abstract defaultDirectoryFileType: IFileType
            abstract defaultFileTypes: ReadonlyArray<obj>

        /// The item to be added to document toolbar.
        type [<AllowNullLiteral>] IToolbarItem =
            abstract name: string with get, set
            abstract widget: Widget with get, set

        /// The options used to create a document registry.
        type [<AllowNullLiteral>] IOptions =
            /// The text model factory for the registry.  A default instance will
            /// be used if not given.
            abstract textModelFactory: ModelFactory option with get, set
            /// The initial file types for the registry.
            /// The [[DocumentRegistry.defaultFileTypes]] will be used if not given.
            abstract initialFileTypes: ResizeArray<DocumentRegistry.IFileType> option with get, set

        /// The interface for a document model.
        type [<AllowNullLiteral>] IModel =
            inherit IDisposable
            /// A signal emitted when the document content changes.
            abstract contentChanged: ISignal<IModel, unit> with get, set
            /// A signal emitted when the model state changes.
            abstract stateChanged: ISignal<IModel, IChangedArgsGeneric<obj option>> with get, set
            /// The dirty state of the model.
            /// 
            /// #### Notes
            /// This should be cleared when the document is loaded from
            /// or saved to disk.
            abstract dirty: bool with get, set
            /// The read-only state of the model.
            abstract readOnly: bool with get, set
            /// The default kernel name of the document.
            abstract defaultKernelName: string
            /// The default kernel language of the document.
            abstract defaultKernelLanguage: string
            /// The underlying `IModelDB` instance in which model
            /// data is stored.
            /// 
            /// ### Notes
            /// Making direct edits to the values stored in the`IModelDB`
            /// is not recommended, and may produce unpredictable results.
            abstract modelDB: IModelDB
            /// Serialize the model to a string.
            abstract toString: unit -> string
            /// Deserialize the model from a string.
            /// 
            /// #### Notes
            /// Should emit a [contentChanged] signal.
            abstract fromString: value: string -> unit
            /// Serialize the model to JSON.
            abstract toJSON: unit -> JSONValue
            /// Deserialize the model from JSON.
            /// 
            /// #### Notes
            /// Should emit a [contentChanged] signal.
            abstract fromJSON: value: obj option -> unit
            /// Initialize model state after initial data load.
            /// 
            /// #### Notes
            /// This function must be called after the initial data is loaded to set up
            /// initial model state, such as an initial undo stack, etc.
            abstract initialize: unit -> unit

        /// The interface for a document model that represents code.
        type [<AllowNullLiteral>] ICodeModel =
            inherit IModel
            inherit JupyterlabCodeeditor.Editor.CodeEditor.IModel

        /// The document context object.
        type [<AllowNullLiteral>] IContext<'T> =
            inherit IDisposable
            /// A signal emitted when the path changes.
            abstract pathChanged: ISignal<IContext<'T>, string> with get, set
            /// A signal emitted when the contentsModel changes.
            abstract fileChanged: ISignal<IContext<'T>, JupyterlabServices.__contents_index.Contents.IModel> with get, set
            /// A signal emitted on the start and end of a saving operation.
            abstract saveState: ISignal<IContext<'T>, SaveState> with get, set
            /// A signal emitted when the context is disposed.
            abstract disposed: ISignal<IContext<'T>, unit> with get, set
            /// The data model for the document.
            abstract model: 'T
            /// The client session object associated with the context.
            abstract session: IClientSession
            /// The current path associated with the document.
            abstract path: string
            /// The current local path associated with the document.
            /// If the document is in the default notebook file browser,
            /// this is the same as the path.
            abstract localPath: string
            /// The document metadata, stored as a services contents model.
            /// 
            /// #### Notes
            /// This will be null until the context is 'ready'. Since we only store
            /// metadata here, the `.contents` attribute will always be empty.
            abstract contentsModel: JupyterlabServices.__contents_index.Contents.IModel option
            /// The url resolver for the context.
            abstract urlResolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver
            /// Whether the context is ready.
            abstract isReady: bool
            /// A promise that is fulfilled when the context is ready.
            abstract ready: Promise<unit>
            /// Save the document contents to disk.
            abstract save: unit -> Promise<unit>
            /// Save the document to a different path chosen by the user.
            abstract saveAs: unit -> Promise<unit>
            /// Revert the document contents to disk contents.
            abstract revert: unit -> Promise<unit>
            /// Create a checkpoint for the file.
            abstract createCheckpoint: unit -> Promise<JupyterlabServices.__contents_index.Contents.ICheckpointModel>
            /// <summary>Delete a checkpoint for the file.</summary>
            /// <param name="checkpointID">- The id of the checkpoint to delete.</param>
            abstract deleteCheckpoint: checkpointID: string -> Promise<unit>
            /// <summary>Restore the file to a known checkpoint state.</summary>
            /// <param name="checkpointID">- The optional id of the checkpoint to restore,
            /// defaults to the most recent checkpoint.</param>
            abstract restoreCheckpoint: ?checkpointID: string -> Promise<unit>
            /// List available checkpoints for the file.
            abstract listCheckpoints: unit -> Promise<ResizeArray<JupyterlabServices.__contents_index.Contents.ICheckpointModel>>
            /// <summary>Add a sibling widget to the document manager.</summary>
            /// <param name="widget">- The widget to add to the document manager.</param>
            /// <param name="options">- The desired options for adding the sibling.</param>
            abstract addSibling: widget: Widget * ?options: IOpenOptions -> IDisposable

        type [<StringEnum>] [<RequireQualifiedAccess>] SaveState =
            | Started
            | Completed
            | Failed

        type Context =
            IContext<IModel>

        type CodeContext =
            IContext<ICodeModel>

        type IWidgetFactoryOptions =
            IWidgetFactoryOptions<obj>

        /// The options used to initialize a widget factory.
        type [<AllowNullLiteral>] IWidgetFactoryOptions<'T> =
            /// The name of the widget to display in dialogs.
            abstract name: string
            /// The file types the widget can view.
            abstract fileTypes: ReadonlyArray<string>
            /// The file types for which the factory should be the default.
            abstract defaultFor: ReadonlyArray<string> option
            /// The file types for which the factory should be the default for rendering,
            /// if that is different than the default factory (which may be for editing).
            /// If undefined, then it will fall back on the default file type.
            abstract defaultRendered: ReadonlyArray<string> option
            /// Whether the widget factory is read only.
            abstract readOnly: bool option
            /// The registered name of the model type used to create the widgets.
            abstract modelName: string option
            /// Whether the widgets prefer having a kernel started.
            abstract preferKernel: bool option
            /// Whether the widgets can start a kernel when opened.
            abstract canStartKernel: bool option
            /// Whether the kernel should be shutdown when the widget is closed.
            abstract shutdownOnClose: bool option
            /// A function producing toolbar widgets, overriding the default toolbar widgets.
            abstract toolbarFactory: ('T -> ResizeArray<DocumentRegistry.IToolbarItem>) option

        /// The options used to open a widget.
        type [<AllowNullLiteral>] IOpenOptions =
            /// The reference widget id for the insert location.
            /// 
            /// The default is `null`.
            abstract ref: string option with get, set
            /// The supported insertion modes.
            /// 
            /// An insert mode is used to specify how a widget should be added
            /// to the main area relative to a reference widget.
            abstract mode: PhosphorWidgets.DockLayout.InsertMode option with get, set
            /// Whether to activate the widget.  Defaults to `true`.
            abstract activate: bool option with get, set
            /// The rank order of the widget among its siblings.
            /// 
            /// #### Notes
            /// This field may be used or ignored depending on shell implementation.
            abstract rank: float option with get, set

        /// The interface for a widget factory.
        type [<AllowNullLiteral>] IWidgetFactory<'T, 'U> =
            inherit IDisposable
            inherit IWidgetFactoryOptions
            /// A signal emitted when a new widget is created.
            abstract widgetCreated: ISignal<IWidgetFactory<'T, 'U>, 'T> with get, set
            /// <summary>Create a new widget given a context.</summary>
            /// <param name="source">- A widget to clone
            /// 
            /// #### Notes
            /// It should emit the [widgetCreated] signal with the new widget.</param>
            abstract createNew: context: IContext<'U> * ?source: 'T -> 'T

        type WidgetFactory =
            IWidgetFactory<IDocumentWidget, IModel>

        /// An interface for a widget extension.
        type [<AllowNullLiteral>] IWidgetExtension<'T, 'U> =
            /// Create a new extension for a given widget.
            abstract createNew: widget: 'T * context: IContext<'U> -> IDisposable

        type WidgetExtension =
            IWidgetExtension<Widget, IModel>

        /// The interface for a model factory.
        type [<AllowNullLiteral>] IModelFactory<'T> =
            inherit IDisposable
            /// The name of the model.
            abstract name: string
            /// The content type of the file (defaults to `"file"`).
            abstract contentType: JupyterlabServices.__contents_index.Contents.ContentType
            /// The format of the file (defaults to `"text"`).
            abstract fileFormat: JupyterlabServices.__contents_index.Contents.FileFormat
            /// <summary>Create a new model for a given path.</summary>
            /// <param name="languagePreference">- An optional kernel language preference.</param>
            abstract createNew: ?languagePreference: string * ?modelDB: IModelDB -> 'T
            /// Get the preferred kernel language given a file path.
            abstract preferredLanguage: path: string -> string

        type ModelFactory =
            IModelFactory<IModel>

        type CodeModelFactory =
            IModelFactory<ICodeModel>

        /// An interface for a file type.
        type [<AllowNullLiteral>] IFileType =
            /// The name of the file type.
            abstract name: string
            /// The mime types associated the file type.
            abstract mimeTypes: ReadonlyArray<string>
            /// The extensions of the file type (e.g. `".txt"`).  Can be a compound
            /// extension (e.g. `".table.json`).
            abstract extensions: ReadonlyArray<string>
            /// An optional display name for the file type.
            abstract displayName: string option
            /// An optional pattern for a file name (e.g. `^Dockerfile$`).
            abstract pattern: string option
            /// The icon class name for the file type.
            abstract iconClass: string option
            /// The icon label for the file type.
            abstract iconLabel: string option
            /// The content type of the new file.
            abstract contentType: JupyterlabServices.__contents_index.Contents.ContentType
            /// The format of the new file.
            abstract fileFormat: JupyterlabServices.__contents_index.Contents.FileFormat

        /// An arguments object for the `changed` signal.
        type [<AllowNullLiteral>] IChangedArgs =
            /// The type of the changed item.
            abstract ``type``: U4<string, string, string, string>
            /// The name of the item or the widget factory being extended.
            abstract name: string
            /// Whether the item was added or removed.
            abstract change: U2<string, string>

    type IDocumentWidget<'U> =
        IDocumentWidget<obj, 'U>

    type IDocumentWidget =
        IDocumentWidget<obj, obj>

    /// An interface for a document widget.
    type [<AllowNullLiteral>] IDocumentWidget<'T, 'U> =
        inherit Widget
        /// The content widget.
        abstract content: 'T
        /// A promise resolving after the content widget is revealed.
        abstract revealed: Promise<unit>
        /// The context associated with the document.
        abstract context: DocumentRegistry.IContext<'U>
        /// The toolbar for the widget.
        abstract toolbar: JupyterlabApputils.Toolbar<Widget>
        /// Set URI fragment identifier.
        abstract setFragment: fragment: string -> unit
