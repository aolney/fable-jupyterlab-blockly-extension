// ts2fable 0.0.0
module rec JupyterlabOutputarea
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
//amo

module Model =
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> //__@phosphor_signaling.ISignal
    // type nbformat = __@jupyterlab_coreutils.nbformat
    // type IObservableList = JupyterlabObservables.Observablelist.IObservableList // __@jupyterlab_observables.IObservableList
    type IOutputModel = JupyterlabRendermime.Outputmodel.IOutputModel // __@jupyterlab_rendermime.IOutputModel

    type [<AllowNullLiteral>] IExports =
        abstract OutputAreaModel: OutputAreaModelStatic

    //TODO: move all exports to top level?
    [<Import("*","@jupyterlab/outputarea")>]
    let Types:IExports = jsNative

    /// The model for an output area.
    /// The namespace for IOutputAreaModel interfaces.
    type [<AllowNullLiteral>] IOutputAreaModel =
        inherit IDisposable
        /// A signal emitted when the model state changes.
        abstract stateChanged: ISignal<IOutputAreaModel, unit>
        /// A signal emitted when the model changes.
        abstract changed: ISignal<IOutputAreaModel, IOutputAreaModel.ChangedArgs>
        /// The length of the items in the model.
        abstract length: float
        /// Whether the output area is trusted.
        abstract trusted: bool with get, set
        /// The output content factory used by the model.
        abstract contentFactory: IOutputAreaModel.IContentFactory
        /// Get an item at the specified index.
        abstract get: index: float -> IOutputModel
        /// Add an output, which may be combined with previous output.
        abstract add: output:  JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> float
        /// Set the value at the specified index.
        abstract set: index: float * output:  JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> unit
        /// <summary>Clear all of the output.</summary>
        /// <param name="wait">- Delay clearing the output until the next message is added.</param>
        abstract clear: ?wait: bool -> unit
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// This will clear any existing data.
        abstract fromJSON: values: ResizeArray< JupyterlabCoreutils.Nbformat.Nbformat.IOutput> -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> ResizeArray< JupyterlabCoreutils.Nbformat.Nbformat.IOutput>

    module IOutputAreaModel =

        /// The options used to create a output area model.
        type [<AllowNullLiteral>] IOptions =
            /// The initial values for the model.
            abstract values: ResizeArray< JupyterlabCoreutils.Nbformat.Nbformat.IOutput> option with get, set
            /// Whether the output is trusted.  The default is false.
            abstract trusted: bool option with get, set
            /// The output content factory used by the model.
            /// 
            /// If not given, a default factory will be used.
            abstract contentFactory: IContentFactory option with get, set

        type ChangedArgs =
            JupyterlabObservables.Observablelist.IObservableList .IChangedArgs<IOutputModel>

        /// The interface for an output content factory.
        type [<AllowNullLiteral>] IContentFactory =
            /// Create an output model.
            abstract createOutputModel: options: JupyterlabRendermime.Outputmodel.IOutputModel.IOptions -> IOutputModel

    /// The default implementation of the IOutputAreaModel.
    /// The namespace for OutputAreaModel class statics.
    type [<AllowNullLiteral>] OutputAreaModel =
        inherit IOutputAreaModel
        /// A signal emitted when the model state changes.
        abstract stateChanged: ISignal<IOutputAreaModel, unit>
        /// A signal emitted when the model changes.
        abstract changed: ISignal<OutputAreaModel, IOutputAreaModel.ChangedArgs>
        /// Get the length of the items in the model.
        abstract length: float
        /// Get whether the model is trusted.
        /// Set whether the model is trusted.
        /// 
        /// #### Notes
        /// Changing the value will cause all of the models to re-set.
        abstract trusted: bool with get, set
        /// The output content factory used by the model.
        abstract contentFactory: IOutputAreaModel.IContentFactory
        /// Test whether the model is disposed.
        abstract isDisposed: bool
        /// Dispose of the resources used by the model.
        abstract dispose: unit -> unit
        /// Get an item at the specified index.
        abstract get: index: float -> IOutputModel
        /// Set the value at the specified index.
        abstract set: index: float * value:  JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> unit
        /// Add an output, which may be combined with previous output.
        abstract add: output:  JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> float
        /// <summary>Clear all of the output.</summary>
        /// <param name="wait">Delay clearing the output until the next message is added.</param>
        abstract clear: ?wait: bool -> unit
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// This will clear any existing data.
        abstract fromJSON: values: ResizeArray< JupyterlabCoreutils.Nbformat.Nbformat.IOutput> -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> ResizeArray< JupyterlabCoreutils.Nbformat.Nbformat.IOutput>
        /// Whether a new value should be consolidated with the previous output.
        /// 
        /// This will only be called if the minimal criteria of both being stream
        /// messages of the same type.
        abstract shouldCombine: options: OutputAreaModelShouldCombineOptions -> bool
        /// A flag that is set when we want to clear the output area
        /// *after* the next addition to it.
        abstract clearNext: bool with get, set
        /// An observable list containing the output models
        /// for this output area.
        abstract list: JupyterlabObservables.Observablelist.IObservableList<IOutputModel> with get, set

    type [<AllowNullLiteral>] OutputAreaModelShouldCombineOptions =
        abstract value:  JupyterlabCoreutils.Nbformat.Nbformat.IOutput with get, set
        abstract lastModel: IOutputModel with get, set

    /// The default implementation of the IOutputAreaModel.
    /// The namespace for OutputAreaModel class statics.
    type [<AllowNullLiteral>] OutputAreaModelStatic =
        /// Construct a new observable outputs instance.
        [<Emit "new $0($1...)">] abstract Create: ?options: IOutputAreaModel.IOptions -> OutputAreaModel

    module OutputAreaModel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// The default implementation of a `IModelOutputFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IOutputAreaModel.IContentFactory
            /// Create an output model.
            abstract createOutputModel: options: JupyterlabRendermime.Outputmodel.IOutputModel.IOptions -> IOutputModel

        /// The default implementation of a `IModelOutputFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

module Widget =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type Signal<'T,'U>  = PhosphorSignaling.Signal<'T,'U> // __@phosphor_signaling.Signal
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    type IOutputModel = JupyterlabRendermime.Outputmodel.IOutputModel // __@jupyterlab_rendermime.IOutputModel
    type IRenderMimeRegistry = JupyterlabRendermime.Registry.IRenderMimeRegistry // __@jupyterlab_rendermime.IRenderMimeRegistry
    // type Kernel = JupyterlabServices.__kernel_kernel.Kernel // __@jupyterlab_services.Kernel
    // type KernelMessage = JupyterlabServices.__kernel_messages.KernelMessage // __@jupyterlab_services.KernelMessage
    type IOutputAreaModel = Model.IOutputAreaModel

    type [<AllowNullLiteral>] IExports =
        abstract OutputArea: OutputAreaStatic
        abstract SimplifiedOutputArea: SimplifiedOutputAreaStatic
        abstract OutputPrompt: OutputPromptStatic
        abstract Stdin: StdinStatic

    /// An output area widget.
    /// 
    /// #### Notes
    /// The widget model must be set separately and can be changed
    /// at any time.  Consumers of the widget must account for a
    /// `null` model, and may want to listen to the `modelChanged`
    /// signal.
    /// A namespace for OutputArea statics.
    type [<AllowNullLiteral>] OutputArea =
        inherit Widget
        /// The model used by the widget.
        abstract model: IOutputAreaModel
        /// The content factory used by the widget.
        abstract contentFactory: OutputArea.IContentFactory
        /// The rendermime instance used by the widget.
        abstract rendermime: IRenderMimeRegistry
        /// A read-only sequence of the chidren widgets in the output area.
        abstract widgets: ReadonlyArray<Widget>
        /// A public signal used to indicate the number of outputs has changed.
        /// 
        /// #### Notes
        /// This is useful for parents who want to apply styling based on the number
        /// of outputs. Emits the current number of outputs.
        abstract outputLengthChanged: Signal<OutputArea, float>
        /// The kernel future associated with the output area.
        abstract future: JupyterlabServices.__kernel_kernel.Kernel.IShellFuture<JupyterlabServices.__kernel_messages.KernelMessage.IExecuteRequestMsg, JupyterlabServices.__kernel_messages.KernelMessage.IExecuteReplyMsg> option with get, set
        /// Dispose of the resources used by the output area.
        abstract dispose: unit -> unit
        /// Follow changes on the model state.
        abstract onModelChanged: sender: IOutputAreaModel * args: Model.IOutputAreaModel.ChangedArgs -> unit
        /// Follow changes on the output model state.
        abstract onStateChanged: sender: IOutputAreaModel -> unit
        /// Handle an input request from a kernel.
        abstract onInputRequest: msg: JupyterlabServices.__kernel_messages.KernelMessage.IInputRequestMsg * future: JupyterlabServices.__kernel_kernel.Kernel.IShellFuture -> unit
        /// Create an output item with a prompt and actual output
        abstract createOutputItem: model: IOutputModel -> Widget option
        /// Render a mimetype
        abstract createRenderedMimetype: model: IOutputModel -> Widget option

    /// An output area widget.
    /// 
    /// #### Notes
    /// The widget model must be set separately and can be changed
    /// at any time.  Consumers of the widget must account for a
    /// `null` model, and may want to listen to the `modelChanged`
    /// signal.
    /// A namespace for OutputArea statics.
    type [<AllowNullLiteral>] OutputAreaStatic =
        /// Construct an output area widget.
        [<Emit "new $0($1...)">] abstract Create: options: OutputArea.IOptions -> OutputArea

    type [<AllowNullLiteral>] SimplifiedOutputArea =
        inherit OutputArea
        /// Handle an input request from a kernel by doing nothing.
        abstract onInputRequest: msg: JupyterlabServices.__kernel_messages.KernelMessage.IInputRequestMsg * future: JupyterlabServices.__kernel_kernel.Kernel.IShellFuture -> unit
        /// Create an output item without a prompt, just the output widgets
        abstract createOutputItem: model: IOutputModel -> Widget option

    type [<AllowNullLiteral>] SimplifiedOutputAreaStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> SimplifiedOutputArea

    module OutputArea =

        type [<AllowNullLiteral>] IExports =
            /// Execute code on an output area.
            abstract execute: code: string * output: OutputArea * session: IClientSession * ?metadata: JSONObject -> Promise<JupyterlabServices.__kernel_messages.KernelMessage.IExecuteReplyMsg>
            abstract isIsolated: mimeType: string * metadata: ReadonlyJSONObject -> bool
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// The options to create an `OutputArea`.
        type [<AllowNullLiteral>] IOptions =
            /// The model used by the widget.
            abstract model: IOutputAreaModel with get, set
            /// The content factory used by the widget to create children.
            abstract contentFactory: IContentFactory option with get, set
            /// The rendermime instance used by the widget.
            abstract rendermime: IRenderMimeRegistry with get, set

        /// An output area widget content factory.
        /// 
        /// The content factory is used to create children in a way
        /// that can be customized.
        type [<AllowNullLiteral>] IContentFactory =
            /// Create an output prompt.
            abstract createOutputPrompt: unit -> IOutputPrompt
            /// Create an stdin widget.
            abstract createStdin: options: Stdin.IOptions -> IStdin

        /// The default implementation of `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IContentFactory
            /// Create the output prompt for the widget.
            abstract createOutputPrompt: unit -> IOutputPrompt
            /// Create an stdin widget.
            abstract createStdin: options: Stdin.IOptions -> IStdin

        /// The default implementation of `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

    /// The interface for an output prompt.
    type [<AllowNullLiteral>] IOutputPrompt =
        inherit Widget
        /// The execution count for the prompt.
        abstract executionCount:  JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set

    /// The default output prompt implementation
    type [<AllowNullLiteral>] OutputPrompt =
        inherit Widget
        inherit IOutputPrompt
        /// The execution count for the prompt.
        abstract executionCount:  JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set

    /// The default output prompt implementation
    type [<AllowNullLiteral>] OutputPromptStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> OutputPrompt

    /// The stdin interface
    type [<AllowNullLiteral>] IStdin =
        inherit Widget
        /// The stdin value.
        abstract value: Promise<string>

    /// The default stdin widget.
    type [<AllowNullLiteral>] Stdin =
        inherit Widget
        inherit IStdin
        /// The value of the widget.
        abstract value: Promise<string>
        /// <summary>Handle the DOM events for the widget.</summary>
        /// <param name="event">- The DOM event sent to the widget.
        /// 
        /// #### Notes
        /// This method implements the DOM `EventListener` interface and is
        /// called in response to events on the dock panel's node. It should
        /// not be called directly by user code.</param>
        abstract handleEvent: ``event``: Event -> unit
        /// Handle `after-attach` messages sent to the widget.
        abstract onAfterAttach: msg: Message -> unit
        /// Handle `update-request` messages sent to the widget.
        abstract onUpdateRequest: msg: Message -> unit
        /// Handle `before-detach` messages sent to the widget.
        abstract onBeforeDetach: msg: Message -> unit

    /// The default stdin widget.
    type [<AllowNullLiteral>] StdinStatic =
        /// Construct a new input widget.
        [<Emit "new $0($1...)">] abstract Create: options: Stdin.IOptions -> Stdin

    module Stdin =

        /// The options to create a stdin widget.
        type [<AllowNullLiteral>] IOptions =
            /// The prompt text.
            abstract prompt: string with get, set
            /// Whether the input is a password.
            abstract password: bool with get, set
            /// The kernel future associated with the request.
            abstract future: JupyterlabServices.__kernel_kernel.Kernel.IShellFuture with get, set
