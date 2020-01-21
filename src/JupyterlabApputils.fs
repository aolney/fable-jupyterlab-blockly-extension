// ts2fable 0.6.1
module rec JupyterlabApputils
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types
open Browser.Css
open Fable.React

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>


type HTMLCollectionOf<'T> =
    inherit HTMLCollection
    abstract Item : int -> 'T with get, set
    abstract namedItem: name: string -> 'T;
//amo: end typescript hacks

// type Kernel = JupyterlabServices.__kernel_kernel.Kernel // ``@jupyterlab_services``.Kernel
// type KernelMessage = JupyerlabServices.KernelMessage // ``@jupyterlab_services``.KernelMessage
// type Session = JupyterlabServices.Session //``@jupyterlab_services``.Session
type IterableOrArrayLike<'T> = PhosphorAlgorithm.Iter.IterableOrArrayLike<'T> //PhosphorAlgorithm.IterableOrArrayLike
type IDisposable = PhosphorDisposable.IDisposable //PhosphorDisposable.IDisposable
type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> //<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // ``@phosphor_signaling``.ISignal

//amo: moved all top level exports here to avoid collisions
type [<AllowNullLiteral>] IExports =
    abstract ClientSession: ClientSessionStatic
    abstract Collapse: CollapseStatic
    abstract CommandLinker: CommandLinkerStatic
    abstract WidgetTracker: WidgetTrackerStatic
    /// <summary>Create and show a dialog.</summary>
    /// <param name="options">- The dialog setup options.</param>
    abstract showDialog: ?options: obj -> Promise<Dialog.IResult<'T>>
    /// <summary>Show an error message dialog.</summary>
    /// <param name="title">- The title of the dialog box.</param>
    /// <param name="error">- the error to show in the dialog body (either a string
    /// or an object with a string `message` property).</param>
    abstract showErrorMessage: title: string * error: obj option * ?buttons: ResizeArray<Dialog.IButton> -> Promise<unit>
    abstract Dialog: DialogStatic
    abstract IFrame: IFrameStatic
    abstract ReactWidget: ReactWidgetStatic
    abstract VDomRenderer: VDomRendererStatic
    abstract UseSignal: UseSignalStatic
    abstract VDomModel: VDomModelStatic
    abstract Toolbar: ToolbarStatic
    /// <summary>React component for a toolbar button.</summary>
    /// <param name="props">- The props for ToolbarButtonComponent.</param>
    abstract ToolbarButtonComponent: props: ToolbarButtonComponent.IProps -> Element //JSX.Element
    /// <summary>Adds the toolbar button class to the toolbar widget.</summary>
    /// <param name="w">Toolbar button widget.</param>
    abstract addToolbarButtonClass: w: Widget -> Widget
    abstract ToolbarButton: ToolbarButtonStatic
    /// React component for a toolbar button that wraps a command.
    /// 
    /// This wraps the ToolbarButtonComponent and watches the command registry
    /// for changes to the command.
    abstract CommandToolbarButtonComponent: props: CommandToolbarButtonComponent.IProps -> Element //JSX.Element
    abstract addCommandToolbarButtonClass: w: Widget -> Widget
    abstract CommandToolbarButton: CommandToolbarButtonStatic
    abstract MainAreaWidget: MainAreaWidgetStatic
    abstract Spinner: SpinnerStatic
    abstract ThemeManager: ThemeManagerStatic
    abstract WindowResolver: WindowResolverStatic

[<Import("*","@jupyterlab/apputils")>]
let Types:IExports = jsNative

/// The interface of client session object.
/// 
/// The client session represents the link between
/// a path and its kernel for the duration of the lifetime
/// of the session object.  The session can have no current
/// kernel, and can start a new kernel at any time.
/// The namespace for Client Session related interfaces.
type [<AllowNullLiteral>] IClientSession =
    inherit IDisposable
    /// A signal emitted when the session is shut down.
    abstract terminated: ISignal<IClientSession, unit>
    /// A signal emitted when the kernel changes.
    abstract kernelChanged: ISignal<IClientSession, JupyterlabServices.__session_session.Session.IKernelChangedArgs>
    /// A signal emitted when the kernel status changes.
    abstract statusChanged: ISignal<IClientSession, JupyterlabServices.__kernel_kernel.Kernel.Status>
    /// A signal emitted for a kernel messages.
    abstract iopubMessage: ISignal<IClientSession, JupyterlabServices.__kernel_messages.KernelMessage.IMessage>
    /// A signal emitted for an unhandled kernel message.
    abstract unhandledMessage: ISignal<IClientSession, JupyterlabServices.__kernel_messages.KernelMessage.IMessage>
    /// A signal emitted when a session property changes.
    abstract propertyChanged: ISignal<IClientSession, U3<string, string, string>>
    /// The current kernel associated with the document.
    abstract kernel: JupyterlabServices.__kernel_kernel.Kernel.IKernelConnection option
    /// The current path associated with the client JupyterlabServices.__session_session.Session.
    abstract path: string
    /// The current name associated with the client session.
    abstract name: string
    /// The type of the client session.
    abstract ``type``: string
    /// The current status of the client session.
    abstract status: JupyterlabServices.__kernel_kernel.Kernel.Status
    /// Whether the session is ready.
    abstract isReady: bool
    /// A promise that is fulfilled when the session is ready.
    abstract ready: Promise<unit>
    /// The kernel preference.
    abstract kernelPreference: IClientSession.IKernelPreference with get, set
    /// The display name of the kernel.
    abstract kernelDisplayName: string
    /// Change the current kernel associated with the document.
    abstract changeKernel: options: obj -> Promise<JupyterlabServices.__kernel_kernel.Kernel.IKernelConnection>
    /// Kill the kernel and shutdown the session.
    abstract shutdown: unit -> Promise<unit>
    /// Select a kernel for the session.
    abstract selectKernel: unit -> Promise<unit>
    /// Restart the session.
    abstract restart: unit -> Promise<bool>
    /// <summary>Change the session path.</summary>
    /// <param name="path">- The new session path.</param>
    abstract setPath: path: string -> Promise<unit>
    /// Change the session name.
    abstract setName: name: string -> Promise<unit>
    /// Change the session type.
    abstract setType: ``type``: string -> Promise<unit>

module IClientSession =

    /// A kernel preference.
    type [<AllowNullLiteral>] IKernelPreference =
        /// The name of the kernel.
        abstract name: string option
        /// The preferred kernel language.
        abstract language: string option
        /// The id of an existing kernel.
        abstract id: string option
        /// Whether to prefer starting a kernel.
        abstract shouldStart: bool option
        /// Whether a kernel can be started.
        abstract canStart: bool option
        /// Whether a kernel needs to be close with the associated session
        abstract shutdownOnClose: bool option
        /// Whether to auto-start the default kernel if no matching kernel is found.
        abstract autoStartDefault: bool option

/// The default implementation of client session object.
/// A namespace for `ClientSession` statics.
type [<AllowNullLiteral>] ClientSession =
    inherit IClientSession
    /// A signal emitted when the session is shut down.
    abstract terminated: ISignal<ClientSession, unit>
    /// A signal emitted when the kernel changes.
    abstract kernelChanged: ISignal<ClientSession, JupyterlabServices.__session_session.Session.IKernelChangedArgs>
    /// A signal emitted when the status changes.
    abstract statusChanged: ISignal<ClientSession, JupyterlabServices.__kernel_kernel.Kernel.Status>
    /// A signal emitted for iopub kernel messages.
    abstract iopubMessage: ISignal<ClientSession, JupyterlabServices.__kernel_messages.KernelMessage.IIOPubMessage>
    /// A signal emitted for an unhandled kernel message.
    abstract unhandledMessage: ISignal<ClientSession, JupyterlabServices.__kernel_messages.KernelMessage.IMessage>
    /// A signal emitted when a session property changes.
    abstract propertyChanged: ISignal<ClientSession, U3<string, string, string>>
    /// The current kernel of the session.
    abstract kernel: JupyterlabServices.__kernel_kernel.Kernel.IKernelConnection option
    /// The current path of the session.
    abstract path: string
    /// The current name of the session.
    abstract name: string
    /// The type of the client session.
    abstract ``type``: string
    /// The kernel preference of the session.
    abstract kernelPreference: IClientSession.IKernelPreference with get, set
    /// The session manager used by the session.
    abstract manager: JupyterlabServices.__session_session.Session.IManager
    /// The current status of the session.
    abstract status: JupyterlabServices.__kernel_kernel.Kernel.Status
    /// Whether the session is ready.
    abstract isReady: bool
    /// A promise that is fulfilled when the session is ready.
    abstract ready: Promise<unit>
    /// The display name of the current kernel.
    abstract kernelDisplayName: string
    /// Test whether the context is disposed.
    abstract isDisposed: bool
    /// Dispose of the resources held by the context.
    abstract dispose: unit -> unit
    /// Change the current kernel associated with the document.
    abstract changeKernel: options: obj -> Promise<JupyterlabServices.__kernel_kernel.Kernel.IKernelConnection>
    /// Select a kernel for the session.
    abstract selectKernel: unit -> Promise<unit>
    /// Kill the kernel and shutdown the session.
    abstract shutdown: unit -> Promise<unit>
    /// Restart the session.
    abstract restart: unit -> Promise<bool>
    /// <summary>Change the session path.</summary>
    /// <param name="path">- The new session path.</param>
    abstract setPath: path: string -> Promise<unit>
    /// Change the session name.
    abstract setName: name: string -> Promise<unit>
    /// Change the session type.
    abstract setType: ``type``: string -> Promise<unit>
    /// Initialize the session.
    /// 
    /// #### Notes
    /// If a server session exists on the current path, we will connect to it.
    /// If preferences include disabling `canStart` or `shouldStart`, no
    /// server session will be started.
    /// If a kernel id is given, we attempt to start a session with that id.
    /// If a default kernel is available, we connect to it.
    /// Otherwise we ask the user to select a kernel.
    abstract initialize: unit -> Promise<unit>
    /// Start the session if necessary.
    abstract _startIfNecessary: obj with get, set
    /// Change the kernel.
    abstract _changeKernel: obj with get, set
    /// Select a kernel.
    abstract _selectKernel: obj with get, set
    /// Start a session and set up its signals.
    abstract _startSession: obj with get, set
    /// Handle a new session object.
    abstract _handleNewSession: obj with get, set
    /// Handle an error in session startup.
    abstract _handleSessionError: obj with get, set
    /// Handle a session termination.
    abstract _onTerminated: obj with get, set
    /// Handle a change to a session property.
    abstract _onPropertyChanged: obj with get, set
    /// Handle a change to the kernel.
    abstract _onKernelChanged: obj with get, set
    /// Handle a change to the session status.
    abstract _onStatusChanged: obj with get, set
    /// Handle an iopub message.
    abstract _onIopubMessage: obj with get, set
    /// Handle an unhandled message.
    abstract _onUnhandledMessage: obj with get, set
    abstract _path: obj with get, set
    abstract _name: obj with get, set
    abstract _type: obj with get, set
    abstract _prevKernelName: obj with get, set
    abstract _kernelPreference: obj with get, set
    abstract _isDisposed: obj with get, set
    abstract _session: obj with get, set
    abstract _ready: obj with get, set
    abstract _initializing: obj with get, set
    abstract _isReady: obj with get, set
    abstract _terminated: obj with get, set
    abstract _kernelChanged: obj with get, set
    abstract _statusChanged: obj with get, set
    abstract _iopubMessage: obj with get, set
    abstract _unhandledMessage: obj with get, set
    abstract _propertyChanged: obj with get, set
    abstract _dialog: obj with get, set
    abstract _setBusy: obj with get, set
    abstract _busyDisposable: obj with get, set

/// The default implementation of client session object.
/// A namespace for `ClientSession` statics.
type [<AllowNullLiteral>] ClientSessionStatic =
    /// Construct a new client session.
    [<Emit "new $0($1...)">] abstract Create: options: ClientSession.IOptions -> ClientSession

module ClientSession =

    type [<AllowNullLiteral>] IExports =
        /// Restart a kernel if the user accepts the risk.
        /// 
        /// Returns a promise resolving with whether the kernel was restarted.
        abstract restartKernel: kernel: JupyterlabServices.__kernel_kernel.Kernel.IKernelConnection -> Promise<bool>
        /// Get the default kernel name given select options.
        abstract getDefaultKernel: options: IKernelSearch -> string option
        /// <summary>Populate a kernel dropdown list.</summary>
        /// <param name="node">- The node to populate.</param>
        /// <param name="options">- The options used to populate the kernels.
        /// 
        /// #### Notes
        /// Populates the list with separated sections:
        /// - Kernels matching the preferred language (display names).
        /// - "None" signifying no kernel.
        /// - The remaining kernels.
        /// - Sessions matching the preferred language (file names).
        /// - The remaining sessions.
        /// If no preferred language is given or no kernels are found using
        /// the preferred language, the default kernel is used in the first
        /// section.  Kernels are sorted by display name.  Sessions display the
        /// base name of the file with an ellipsis overflow and a tooltip with
        /// the explicit session information.</param>
        abstract populateKernelSelect: node: HTMLSelectElement * options: IKernelSearch -> unit

    /// The options used to initialize a context.
    type [<AllowNullLiteral>] IOptions =
        /// A session manager instance.
        abstract manager: JupyterlabServices.__session_session.Session.IManager with get, set
        /// The initial path of the file.
        abstract path: string option with get, set
        /// The name of the session.
        abstract name: string option with get, set
        /// The type of the session.
        abstract ``type``: string option with get, set
        /// A kernel preference.
        abstract kernelPreference: IClientSession.IKernelPreference option with get, set
        /// A function to call when the session becomes busy.
        abstract setBusy: (unit -> IDisposable) option with get, set

    /// An interface for populating a kernel selector.
    type [<AllowNullLiteral>] IKernelSearch =
        /// The Kernel specs.
        abstract specs: JupyterlabServices.__kernel_kernel.Kernel.ISpecModels option with get, set
        /// The kernel preference.
        abstract preference: IClientSession.IKernelPreference with get, set
        /// The current running sessions.
        abstract sessions: IterableOrArrayLike<JupyterlabServices.__session_session.Session.IModel> option with get, set
type MimeData = PhosphorCoreutils.MimeData // PhosphorCoreutils.MimeData

type ClipboardData =
    U2<string, MimeData>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ClipboardData =
    let ofString v: ClipboardData = v |> U2.Case1
    let isString (v: ClipboardData) = match v with U2.Case1 _ -> true | _ -> false
    let asString (v: ClipboardData) = match v with U2.Case1 o -> Some o | _ -> None
    let ofMimeData v: ClipboardData = v |> U2.Case2
    let isMimeData (v: ClipboardData) = match v with U2.Case2 _ -> true | _ -> false
    let asMimeData (v: ClipboardData) = match v with U2.Case2 o -> Some o | _ -> None

module Clipboard =

    type [<AllowNullLiteral>] IExports =
        /// Get the application clipboard instance.
        abstract getInstance: unit -> MimeData
        /// Set the application clipboard instance.
        abstract setInstance: value: MimeData -> unit
        /// Copy text to the system clipboard.
        /// 
        /// #### Notes
        /// This can only be called in response to a user input event.
        abstract copyToSystem: clipboardData: ClipboardData -> unit
        /// <summary>Generate a clipboard event on a node.</summary>
        /// <param name="node">- The element on which to generate the event.</param>
        /// <param name="type">- The type of event to generate.
        /// `'paste'` events cannot be programmatically generated.
        /// 
        /// #### Notes
        /// This can only be called in response to a user input event.</param>
        abstract generateEvent: node: HTMLElement * ?``type``: U2<string, string> -> unit
type Message = PhosphorMessaging.Message // PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> //<'T,'U> = PhosphorSignaling.ISignal<'T,'U> //  ``@phosphor_signaling``.ISignal
type Widget = PhosphorWidgets.Widget // PhosphorWidgets.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract Collapse: CollapseStatic

type Collapse =
    Collapse<obj>

/// A panel that supports a collapsible header made from the widget's title.
/// Clicking on the title expands or contracts the widget.
type [<AllowNullLiteral>] Collapse<'T> =
    inherit Widget
    /// The widget inside the collapse panel.
    abstract widget: 'T with get, set
    /// The collapsed state of the panel.
    abstract collapsed: bool with get, set
    /// A signal for when the widget collapse state changes.
    abstract collapseChanged: ISignal<Collapse, unit>
    /// Toggle the collapse state of the panel.
    abstract toggle: unit -> unit
    /// Dispose the widget.
    abstract dispose: unit -> unit
    /// <summary>Handle the DOM events for the Collapse widget.</summary>
    /// <param name="event">- The DOM event sent to the panel.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the panel's DOM node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    abstract onAfterAttach: msg: Message -> unit
    abstract onBeforeDetach: msg: Message -> unit
    abstract _collapse: obj with get, set
    abstract _uncollapse: obj with get, set
    abstract _evtClick: obj with get, set
    /// Handle the `changed` signal of a title object.
    abstract _onTitleChanged: obj with get, set
    abstract _collapseChanged: obj with get, set
    abstract _collapsed: obj with get, set
    abstract _content: obj with get, set
    abstract _header: obj with get, set
    abstract _widget: obj with get, set

/// A panel that supports a collapsible header made from the widget's title.
/// Clicking on the title expands or contracts the widget.
type [<AllowNullLiteral>] CollapseStatic =
    [<Emit "new $0($1...)">] abstract Create: options: Collapse.IOptions<'T> -> Collapse<'T>

module Collapse =

    type IOptions =
        IOptions<obj>

    type [<AllowNullLiteral>] IOptions<'T> =
        inherit PhosphorWidgets.Widget.IOptions
        abstract widget: 'T with get, set
        abstract collapsed: bool option with get, set
type JSONObject = PhosphorCoreutils.JSONObject //PhosphorCoreutils.JSONObject
// type IDisposable = PhosphorDisposable.IDisposable
type CommandRegistry = PhosphorCommands.CommandRegistry
type ElementDataset = PhosphorVirtualdom.ElementDataset

// type [<AllowNullLiteral>] IExports =
//     abstract CommandLinker: CommandLinkerStatic

/// A static class that provides helper methods to generate clickable nodes that
/// execute registered commands with pre-populated arguments.
/// A namespace for command linker statics.
type [<AllowNullLiteral>] CommandLinker =
    inherit IDisposable
    /// Test whether the linker is disposed.
    abstract isDisposed: bool
    /// Dispose of the resources held by the linker.
    abstract dispose: unit -> unit
    /// <summary>Connect a command/argument pair to a given node so that when it is clicked,
    /// the command will execute.</summary>
    /// <param name="node">- The node being connected.</param>
    /// <param name="command">- The command ID to execute upon click.</param>
    /// <param name="args">- The arguments with which to invoke the command.</param>
    abstract connectNode: node: HTMLElement * command: string * ?args: JSONObject -> HTMLElement
    /// <summary>Disconnect a node that has been connected to execute a command on click.</summary>
    /// <param name="node">- The node being disconnected.</param>
    abstract disconnectNode: node: HTMLElement -> HTMLElement
    /// <summary>Handle the DOM events for the command linker helper class.</summary>
    /// <param name="event">- The DOM event sent to the class.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the panel's DOM node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// <summary>Populate the `dataset` attribute within the collection of attributes used
    /// to instantiate a virtual DOM node with the values necessary for its
    /// rendered DOM node to respond to clicks by executing a command/argument
    /// pair.</summary>
    /// <param name="command">- The command ID to execute upon click.</param>
    /// <param name="args">- The arguments with which to invoke the command.</param>
    abstract populateVNodeDataset: command: string * ?args: JSONObject -> ElementDataset
    /// The global click handler that deploys commands/argument pairs that are
    /// attached to the node being clicked.
    abstract _evtClick: obj with get, set
    abstract _commands: obj with get, set
    abstract _isDisposed: obj with get, set

/// A static class that provides helper methods to generate clickable nodes that
/// execute registered commands with pre-populated arguments.
/// A namespace for command linker statics.
type [<AllowNullLiteral>] CommandLinkerStatic =
    /// Instantiate a new command linker.
    [<Emit "new $0($1...)">] abstract Create: options: CommandLinker.IOptions -> CommandLinker

module CommandLinker =

    /// The instantiation options for a command linker.
    type [<AllowNullLiteral>] IOptions =
        /// The command registry instance that all linked commands will use.
        abstract commands: CommandRegistry with get, set
type Token<'T> = PhosphorCoreutils.Token<'T>
// type IDisposable = PhosphorDisposable.IDisposable
type CommandPalette = PhosphorWidgets.CommandPalette
// let [<Import("*","@jupyterlab/apputils/lib/commandpalette")>] ICommandPalette: Token<ICommandPalette> = jsNative
let [<ImportMember("@jupyterlab/apputils")>] ICommandPalette: Token<ICommandPalette> = jsNative

/// The options for creating a command palette item.
type [<AllowNullLiteral>] IPaletteItem =
    inherit PhosphorWidgets.CommandPalette.IItemOptions

/// The command palette token.
/// The interface for a Jupyter Lab command palette.
type [<AllowNullLiteral>] ICommandPalette =
    /// The placeholder text of the command palette's search input.
    abstract placeholder: string with get, set
    /// Activate the command palette for user input.
    abstract activate: unit -> unit
    /// <summary>Add a command item to the command palette.</summary>
    /// <param name="options">- The options for creating the command item.</param>
    abstract addItem: options: IPaletteItem -> IDisposable
type IRestorable<'T> = JupyterlabCoreutils.Interfaces.IRestorable<'T>
// type IDisposable = PhosphorDisposable.IDisposable
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type Widget = PhosphorWidgets.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract WidgetTracker: WidgetTrackerStatic

type IWidgetTracker =
    IWidgetTracker<obj>

/// A tracker that tracks widgets.
type [<AllowNullLiteral>] IWidgetTracker<'T> =
    inherit IDisposable
    /// A signal emitted when a widget is added.
    abstract widgetAdded: ISignal<IWidgetTracker<'T>, 'T>
    /// The current widget is the most recently focused or added widget.
    /// 
    /// #### Notes
    /// It is the most recently focused widget, or the most recently added
    /// widget if no widget has taken focus.
    abstract currentWidget: 'T option
    /// A signal emitted when the current instance changes.
    /// 
    /// #### Notes
    /// If the last instance being tracked is disposed, `null` will be emitted.
    abstract currentChanged: ISignal<IWidgetTracker<'T>, 'T option>
    /// The number of instances held by the tracker.
    abstract size: float
    /// A promise that is resolved when the widget tracker has been
    /// restored from a serialized state.
    /// 
    /// #### Notes
    /// Most client code will not need to use this, since they can wait
    /// for the whole application to restore. However, if an extension
    /// wants to perform actions during the application restoration, but
    /// after the restoration of another widget tracker, they can use
    /// this promise.
    abstract restored: Promise<unit>
    /// A signal emitted when a widget is updated.
    abstract widgetUpdated: ISignal<IWidgetTracker<'T>, 'T>
    /// Find the first instance in the tracker that satisfies a filter function.
    abstract find: fn: ('T -> bool) -> 'T option
    /// <summary>Iterate through each instance in the tracker.</summary>
    /// <param name="fn">- The function to call on each instance.</param>
    abstract forEach: fn: ('T -> unit) -> unit
    /// <summary>Filter the instances in the tracker based on a predicate.</summary>
    /// <param name="fn">- The function by which to filter.</param>
    abstract filter: fn: ('T -> bool) -> ResizeArray<'T>
    /// <summary>Check if this tracker has the specified instance.</summary>
    /// <param name="obj">- The object whose existence is being checked.</param>
    abstract has: obj: Widget -> bool
    /// <summary>Inject an instance into the widget tracker without the tracker handling
    /// its restoration lifecycle.</summary>
    /// <param name="obj">- The instance to inject into the tracker.</param>
    abstract inject: obj: 'T -> unit

type WidgetTracker =
    WidgetTracker<obj>

/// A class that keeps track of widget instances on an Application shell.
/// A namespace for `WidgetTracker` statics.
type [<AllowNullLiteral>] WidgetTracker<'T> =
    inherit IWidgetTracker<'T>
    inherit IRestorable<'T>
    /// A namespace for all tracked widgets, (e.g., `notebook`).
    abstract ``namespace``: string
    /// A signal emitted when the current widget changes.
    abstract currentChanged: ISignal<WidgetTracker<'T>, 'T option>
    /// The current widget is the most recently focused or added widget.
    /// 
    /// #### Notes
    /// It is the most recently focused widget, or the most recently added
    /// widget if no widget has taken focus.
    abstract currentWidget: 'T option
    /// A promise resolved when the tracker has been restored.
    abstract restored: Promise<unit>
    /// The number of widgets held by the tracker.
    abstract size: float
    /// A signal emitted when a widget is added.
    /// 
    /// #### Notes
    /// This signal will only fire when a widget is added to the tracker. It will
    /// not fire if a widget is injected into the tracker.
    abstract widgetAdded: ISignal<WidgetTracker<'T>, 'T>
    /// A signal emitted when a widget is updated.
    abstract widgetUpdated: ISignal<WidgetTracker<'T>, 'T>
    /// <summary>Add a new widget to the tracker.</summary>
    /// <param name="widget">- The widget being added.
    /// 
    /// #### Notes
    /// The widget passed into the tracker is added synchronously; its existence in
    /// the tracker can be checked with the `has()` method. The promise this method
    /// returns resolves after the widget has been added and saved to an underlying
    /// restoration connector, if one is available.</param>
    abstract add: widget: 'T -> Promise<unit>
    /// Test whether the tracker is disposed.
    abstract isDisposed: bool
    /// Dispose of the resources held by the tracker.
    abstract dispose: unit -> unit
    /// Find the first widget in the tracker that satisfies a filter function.
    abstract find: fn: ('T -> bool) -> 'T option
    /// <summary>Iterate through each widget in the tracker.</summary>
    /// <param name="fn">- The function to call on each widget.</param>
    abstract forEach: fn: ('T -> unit) -> unit
    /// <summary>Filter the widgets in the tracker based on a predicate.</summary>
    /// <param name="fn">- The function by which to filter.</param>
    abstract filter: fn: ('T -> bool) -> ResizeArray<'T>
    /// <summary>Inject a foreign widget into the widget tracker.</summary>
    /// <param name="widget">- The widget to inject into the tracker.
    /// 
    /// #### Notes
    /// Injected widgets will not have their state saved by the tracker.
    /// 
    /// The primary use case for widget injection is for a plugin that offers a
    /// sub-class of an extant plugin to have its instances share the same commands
    /// as the parent plugin (since most relevant commands will use the
    /// `currentWidget` of the parent plugin's widget tracker). In this situation,
    /// the sub-class plugin may well have its own widget tracker for layout and
    /// state restoration in addition to injecting its widgets into the parent
    /// plugin's widget tracker.</param>
    abstract inject: widget: 'T -> Promise<unit>
    /// <summary>Check if this tracker has the specified widget.</summary>
    /// <param name="widget">- The widget whose existence is being checked.</param>
    abstract has: widget: Widget -> bool
    /// <summary>Restore the widgets in this tracker's namespace.</summary>
    /// <param name="options">- The configuration options that describe restoration.</param>
    abstract restore: options: JupyterlabCoreutils.Interfaces.IRestorable.IOptions<'T> -> Promise<obj option>
    /// <summary>Save the restore data for a given widget.</summary>
    /// <param name="widget">- The widget being saved.</param>
    abstract save: widget: 'T -> Promise<unit>
    /// Handle the current change event.
    /// 
    /// #### Notes
    /// The default implementation is a no-op.
    abstract onCurrentChanged: value: 'T option -> unit
    abstract _currentChanged: obj with get, set
    abstract _focusTracker: obj with get, set
    abstract _pool: obj with get, set
    abstract _isDisposed: obj with get, set
    abstract _widgetAdded: obj with get, set
    abstract _widgetUpdated: obj with get, set

/// A class that keeps track of widget instances on an Application shell.
/// A namespace for `WidgetTracker` statics.
type [<AllowNullLiteral>] WidgetTrackerStatic =
    /// <summary>Create a new widget tracker.</summary>
    /// <param name="options">- The instantiation options for a widget tracker.</param>
    [<Emit "new $0($1...)">] abstract Create: options: WidgetTracker.IOptions -> WidgetTracker<'T>

module WidgetTracker =

    /// The instantiation options for a widget tracker.
    type [<AllowNullLiteral>] IOptions =
        /// A namespace for all tracked widgets, (e.g., `notebook`).
        abstract ``namespace``: string with get, set
// type Message = PhosphorMessaging.Message
// type Widget = PhosphorWidgets.Widget
// type WidgetTracker = __widgettracker.WidgetTracker

// type [<AllowNullLiteral>] IExports =
//     /// <summary>Create and show a dialog.</summary>
//     /// <param name="options">- The dialog setup options.</param>
//     abstract showDialog: ?options: obj -> Promise<Dialog.IResult<'T>>
//     /// <summary>Show an error message dialog.</summary>
//     /// <param name="title">- The title of the dialog box.</param>
//     /// <param name="error">- the error to show in the dialog body (either a string
//     /// or an object with a string `message` property).</param>
//     abstract showErrorMessage: title: string * error: obj option * ?buttons: ResizeArray<Dialog.IButton> -> Promise<unit>
//     abstract Dialog: DialogStatic

/// A modal dialog widget.
/// The namespace for Dialog class statics.
type [<AllowNullLiteral>] Dialog<'T> =
    inherit Widget
    /// Dispose of the resources used by the dialog.
    abstract dispose: unit -> unit
    /// Launch the dialog as a modal window.
    abstract launch: unit -> Promise<Dialog.IResult<'T>>
    /// <summary>Resolve the current dialog.</summary>
    /// <param name="index">- An optional index to the button to resolve.
    /// 
    /// #### Notes
    /// Will default to the defaultIndex.
    /// Will resolve the current `show()` with the button value.
    /// Will be a no-op if the dialog is not shown.</param>
    abstract resolve: ?index: float -> unit
    /// Reject the current dialog with a default reject value.
    /// 
    /// #### Notes
    /// Will be a no-op if the dialog is not shown.
    abstract reject: unit -> unit
    /// <summary>Handle the DOM events for the directory listing.</summary>
    /// <param name="event">- The DOM event sent to the widget.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the panel's DOM node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on an `'after-attach'` message.
    abstract onAfterAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on a `'close-request'` message.
    abstract onCloseRequest: msg: Message -> unit
    /// <summary>Handle the `'click'` event for a dialog button.</summary>
    /// <param name="event">- The DOM event sent to the widget</param>
    abstract _evtClick: ``event``: MouseEvent -> unit
    /// <summary>Handle the `'keydown'` event for the widget.</summary>
    /// <param name="event">- The DOM event sent to the widget</param>
    abstract _evtKeydown: ``event``: KeyboardEvent -> unit
    /// <summary>Handle the `'focus'` event for the widget.</summary>
    /// <param name="event">- The DOM event sent to the widget</param>
    abstract _evtFocus: ``event``: FocusEvent -> unit
    /// Resolve a button item.
    abstract _resolve: obj with get, set
    abstract _buttonNodes: obj with get, set
    abstract _buttons: obj with get, set
    abstract _original: obj with get, set
    abstract _first: obj with get, set
    abstract _primary: obj with get, set
    abstract _promise: obj with get, set
    abstract _defaultButton: obj with get, set
    abstract _host: obj with get, set
    abstract _body: obj with get, set
    abstract _focusNodeSelector: obj with get, set

/// A modal dialog widget.
/// The namespace for Dialog class statics.
type [<AllowNullLiteral>] DialogStatic =
    /// <summary>Create a dialog panel instance.</summary>
    /// <param name="options">- The dialog setup options.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: obj -> Dialog<'T>

module Dialog =

    type [<AllowNullLiteral>] IExports =
        /// Create a button item.
        abstract createButton: value: obj -> obj
        /// Create a reject button.
        abstract cancelButton: ?options: obj -> obj
        /// Create an accept button.
        abstract okButton: ?options: obj -> obj
        /// Create a warn button.
        abstract warnButton: ?options: obj -> obj
        /// Disposes all dialog instances.
        /// 
        /// #### Notes
        /// This function should only be used in tests or cases where application state
        /// may be discarded.
        abstract flush: unit -> unit
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer
        abstract tracker: WidgetTracker<Dialog<obj option>>

    type Body<'T> = 
        U3<IBodyWidget<'T>, ReactElementTypeWrapper<obj option>, string>
        // U3<IBodyWidget<'T>, React.ReactElement<obj option>, string>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Body =
        let ofIBodyWidget v: Body<'T> = v |> U3.Case1
        let isIBodyWidget (v: Body<'T>) = match v with U3.Case1 _ -> true | _ -> false
        let asIBodyWidget (v: Body<'T>) = match v with U3.Case1 o -> Some o | _ -> None
        let ``ofReact.ReactElement`` v: Body<'T> = v |> U3.Case2
        let ``isReact.ReactElement`` (v: Body<'T>) = match v with U3.Case2 _ -> true | _ -> false
        let ``asReact.ReactElement`` (v: Body<'T>) = match v with U3.Case2 o -> Some o | _ -> None
        let ofString v: Body<'T> = v |> U3.Case3
        let isString (v: Body<'T>) = match v with U3.Case3 _ -> true | _ -> false
        let asString (v: Body<'T>) = match v with U3.Case3 o -> Some o | _ -> None

    type Header =
        U2<Fable.React.ReactElementTypeWrapper<obj option>, string>
        // U2<React.ReactElement<obj option>, string>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Header =
        let ``ofReact.ReactElement`` v: Header = v |> U2.Case1
        let ``isReact.ReactElement`` (v: Header) = match v with U2.Case1 _ -> true | _ -> false
        let ``asReact.ReactElement`` (v: Header) = match v with U2.Case1 o -> Some o | _ -> None
        let ofString v: Header = v |> U2.Case2
        let isString (v: Header) = match v with U2.Case2 _ -> true | _ -> false
        let asString (v: Header) = match v with U2.Case2 o -> Some o | _ -> None

    type IBodyWidget =
        IBodyWidget<obj>

    /// A widget used as a dialog body.
    type [<AllowNullLiteral>] IBodyWidget<'T> =
        inherit Widget
        /// Get the serialized value of the widget.
        abstract getValue: unit -> 'T

    /// The options used to make a button item.
    type [<AllowNullLiteral>] IButton =
        /// The label for the button.
        abstract label: string with get, set
        /// The icon class for the button.
        abstract iconClass: string with get, set
        /// The icon label for the button.
        abstract iconLabel: string with get, set
        /// The caption for the button.
        abstract caption: string with get, set
        /// The extra class name for the button.
        abstract className: string with get, set
        /// The dialog action to perform when the button is clicked.
        abstract accept: bool with get, set
        /// The button display type.
        abstract displayType: U2<string, string> with get, set

    /// The options used to create a dialog.
    type [<AllowNullLiteral>] IOptions<'T> =
        /// The top level text for the dialog.  Defaults to an empty string.
        abstract title: Header with get, set
        /// The main body element for the dialog or a message to display.
        /// Defaults to an empty string.
        /// 
        /// #### Notes
        /// If a widget is given as the body, it will be disposed after the
        /// dialog is resolved.  If the widget has a `getValue()` method,
        /// the method will be called prior to disposal and the value
        /// will be provided as part of the dialog result.
        /// A string argument will be used as raw `textContent`.
        /// All `input` and `select` nodes will be wrapped and styled.
        abstract body: Body<'T> with get, set
        /// The host element for the dialog. Defaults to `document.body`.
        abstract host: HTMLElement with get, set
        /// The buttons to display. Defaults to cancel and accept buttons.
        abstract buttons: ReadonlyArray<IButton> with get, set
        /// The index of the default button.  Defaults to the last button.
        abstract defaultButton: float with get, set
        /// A selector for the primary element that should take focus in the dialog.
        /// Defaults to an empty string, causing the [[defaultButton]] to take
        /// focus.
        abstract focusNodeSelector: string with get, set
        /// An optional renderer for dialog items.  Defaults to a shared
        /// default renderer.
        abstract renderer: IRenderer with get, set

    /// A dialog renderer.
    type [<AllowNullLiteral>] IRenderer =
        /// <summary>Create the header of the dialog.</summary>
        /// <param name="title">- The title of the dialog.</param>
        abstract createHeader: title: Header -> Widget
        /// Create the body of the dialog.
        abstract createBody: body: Body<obj option> -> Widget
        /// <summary>Create the footer of the dialog.</summary>
        /// <param name="buttons">- The button nodes to add to the footer.</param>
        abstract createFooter: buttons: ResizeArray<HTMLElement> -> Widget
        /// <summary>Create a button node for the dialog.</summary>
        /// <param name="button">- The button data.</param>
        abstract createButtonNode: button: IButton -> HTMLElement

    /// The result of a dialog.
    type [<AllowNullLiteral>] IResult<'T> =
        /// The button that was pressed.
        abstract button: IButton with get, set
        /// The value retrieved from `.getValue()` if given on the widget.
        abstract value: 'T option with get, set

    /// The default implementation of a dialog renderer.
    type [<AllowNullLiteral>] Renderer =
        /// <summary>Create the header of the dialog.</summary>
        /// <param name="title">- The title of the dialog.</param>
        abstract createHeader: title: Header -> Widget
        /// <summary>Create the body of the dialog.</summary>
        /// <param name="value">- The input value for the body.</param>
        abstract createBody: value: Body<obj option> -> Widget
        /// Create the footer of the dialog.
        abstract createFooter: buttons: ResizeArray<HTMLElement> -> Widget
        /// <summary>Create a button node for the dialog.</summary>
        /// <param name="button">- The button data.</param>
        abstract createButtonNode: button: IButton -> HTMLElement
        /// <summary>Create the class name for the button.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createItemClass: data: IButton -> string
        /// <summary>Render an icon element for a dialog item.</summary>
        /// <param name="data">- The data to use for rendering the icon.</param>
        abstract renderIcon: data: IButton -> HTMLElement
        /// <summary>Create the class name for the button icon.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createIconClass: data: IButton -> string
        /// <summary>Render the label element for a button.</summary>
        /// <param name="data">- The data to use for rendering the label.</param>
        abstract renderLabel: data: IButton -> HTMLElement

    /// The default implementation of a dialog renderer.
    type [<AllowNullLiteral>] RendererStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer

module DOMUtils =

    type [<AllowNullLiteral>] IExports =
        /// Get the index of the node at a client position, or `-1`.
        abstract hitTestNodes: nodes: U2<ResizeArray<HTMLElement>, HTMLCollection> * x: float * y: float -> float
        /// Find the first element matching a class name.
        abstract findElement: parent: HTMLElement * className: string -> HTMLElement
        /// Find the first element matching a class name.
        abstract findElements: parent: HTMLElement * className: string -> HTMLCollectionOf<HTMLElement>
        /// Create a DOM id with prefix "id-" to solve bug for UUIDs beginning with numbers.
        abstract createDomID: unit -> string

module HoverBox =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Set the visible dimensions of a hovering box anchored to an editor cursor.</summary>
        /// <param name="options">- The hover box geometry calculation options.</param>
        abstract setGeometry: options: IOptions -> unit

    /// Options for setting the geometry of a hovering node and its anchor node.
    type [<AllowNullLiteral>] IOptions =
        /// The referent anchor rectangle to which the hover box is bound.
        /// 
        /// #### Notes
        /// In an editor context, this value will typically be the cursor's
        /// coordinate position, which can be retrieved via calling the
        /// `getCoordinateForPosition` method.
        abstract anchor: ClientRect with get, set
        /// The node that hosts the anchor.
        /// 
        /// #### Notes
        /// The visibility of the anchor rectangle within this host node is the
        /// heuristic that determines whether the hover box ought to be visible.
        abstract host: HTMLElement with get, set
        /// The maximum height of a hover box.
        /// 
        /// #### Notes
        /// This value is only used if a CSS max-height attribute is not set for the
        /// hover box. It is a fallback value.
        abstract maxHeight: float with get, set
        /// The minimum height of a hover box.
        abstract minHeight: float with get, set
        /// The hover box node.
        abstract node: HTMLElement with get, set
        /// Optional pixel offset values added to where the hover box should render.
        /// 
        /// #### Notes
        /// This option is useful for passing in values that may pertain to CSS
        /// borders or padding in cases where the text inside the hover box may need
        /// to align with the text of the referent editor.
        /// 
        /// Because the hover box calculation may render a box either above or below
        /// the cursor, the `vertical` offset accepts `above` and `below` values for
        /// the different render modes.
        abstract offset: obj option with get, set
        /// If space is available both above and below the anchor, denote which
        /// location is privileged. Use forceBelow and forceAbove to mandate where
        /// hover box should render relative to anchor.
        /// 
        /// #### Notes
        /// The default value is `'below'`.
        abstract privilege: U4<string, string, string, string> option with get, set
        /// If the style of the node has already been computed, it can be passed into
        /// the hover box for geometry calculation.
        abstract style: CSSStyleDeclaration option with get, set
// type Widget = PhosphorWidgets.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract IFrame: IFrameStatic

/// A phosphor widget which wraps an IFrame.
/// A namespace for IFrame widget statics.
type [<AllowNullLiteral>] IFrame =
    inherit Widget
    /// Referrer policy for the iframe.
    /// 
    /// #### Notes
    /// By default, `no-referrer` is chosen.
    /// 
    /// For more information, see
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLIFrameElement/referrerPolicy
    abstract referrerPolicy: IFrame.ReferrerPolicy with get, set
    /// Exceptions to the sandboxing.
    /// 
    /// #### Notes
    /// By default, all sandboxing security policies are enabled.
    /// This setting allows the user to selectively disable these
    /// policies. This should be done with care, as it can
    /// introduce security risks, and possibly allow malicious
    /// sites to execute code in a JupyterLab session.
    /// 
    /// For more information, see
    /// https://developer.mozilla.org/en-US/docs/Web/HTML/Element/iframe
    abstract sandbox: ResizeArray<IFrame.SandboxExceptions> with get, set
    /// The url of the IFrame.
    abstract url: string with get, set
    abstract _sandbox: obj with get, set
    abstract _referrerPolicy: obj with get, set

/// A phosphor widget which wraps an IFrame.
/// A namespace for IFrame widget statics.
type [<AllowNullLiteral>] IFrameStatic =
    /// Create a new IFrame widget.
    [<Emit "new $0($1...)">] abstract Create: ?options: IFrame.IOptions -> IFrame

module IFrame =

    type [<StringEnum>] [<RequireQualifiedAccess>] ReferrerPolicy =
        | [<CompiledName "no-referrer">] NoReferrer
        | [<CompiledName "no-referrer-when-downgrade">] NoReferrerWhenDowngrade
        | Origin
        | [<CompiledName "origin-when-cross-origin">] OriginWhenCrossOrigin
        | [<CompiledName "same-origin">] SameOrigin
        | [<CompiledName "strict-origin">] StrictOrigin
        | [<CompiledName "strict-origin-when-cross-origin">] StrictOriginWhenCrossOrigin
        | [<CompiledName "unsafe-url">] UnsafeUrl

    type [<StringEnum>] [<RequireQualifiedAccess>] SandboxExceptions =
        | [<CompiledName "allow-forms">] AllowForms
        | [<CompiledName "allow-modals">] AllowModals
        | [<CompiledName "allow-orientation-lock">] AllowOrientationLock
        | [<CompiledName "allow-pointer-lock">] AllowPointerLock
        | [<CompiledName "allow-popups">] AllowPopups
        | [<CompiledName "popups-to-escape-sandbox">] PopupsToEscapeSandbox
        | [<CompiledName "allow-presentation">] AllowPresentation
        | [<CompiledName "allow-same-origin">] AllowSameOrigin
        | [<CompiledName "allow-scripts">] AllowScripts
        | [<CompiledName "allow-storage-access-by-user-activation">] AllowStorageAccessByUserActivation
        | [<CompiledName "allow-top-navigation">] AllowTopNavigation
        | [<CompiledName "allow-top-navigation-by-user-activation">] AllowTopNavigationByUserActivation

    /// Options for creating a new IFrame widget.
    type [<AllowNullLiteral>] IOptions =
        /// Exceptions for the iframe sandbox.
        abstract sandbox: ResizeArray<SandboxExceptions> option with get, set
        /// Referrer policy for the iframe.
        abstract referrerPolicy: ReferrerPolicy option with get, set
// type Dialog = __dialog.Dialog

module InputDialog =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create and show a input dialog for a boolean.</summary>
        /// <param name="options">- The dialog setup options.</param>
        abstract getBoolean: options: IBooleanOptions -> Promise<Dialog.IResult<bool>>
        /// <summary>Create and show a input dialog for a number.</summary>
        /// <param name="options">- The dialog setup options.</param>
        abstract getNumber: options: INumberOptions -> Promise<Dialog.IResult<float>>
        /// <summary>Create and show a input dialog for a choice.</summary>
        /// <param name="options">- The dialog setup options.</param>
        abstract getItem: options: IItemOptions -> Promise<Dialog.IResult<string>>
        /// <summary>Create and show a input dialog for a text.</summary>
        /// <param name="options">- The dialog setup options.</param>
        abstract getText: options: ITextOptions -> Promise<Dialog.IResult<string>>

    /// Common constructor options for input dialogs
    type [<AllowNullLiteral>] IOptions =
        /// The top level text for the dialog.  Defaults to an empty string.
        abstract title: Dialog.Header with get, set
        /// The host element for the dialog. Defaults to `document.body`.
        abstract host: HTMLElement option with get, set
        /// Label of the requested input
        abstract label: string option with get, set
        /// An optional renderer for dialog items.  Defaults to a shared
        /// default renderer.
        abstract renderer: Dialog.IRenderer option with get, set
        /// Label for ok button.
        abstract okLabel: string option with get, set
        /// Label for cancel button.
        abstract cancelLabel: string option with get, set

    /// Constructor options for boolean input dialogs
    type [<AllowNullLiteral>] IBooleanOptions =
        inherit IOptions
        /// Default value
        abstract value: bool option with get, set

    /// Constructor options for number input dialogs
    type [<AllowNullLiteral>] INumberOptions =
        inherit IOptions
        /// Default value
        abstract value: float option with get, set

    /// Constructor options for item selection input dialogs
    type [<AllowNullLiteral>] IItemOptions =
        inherit IOptions
        /// List of choices
        abstract items: Array<string> with get, set
        /// Default choice
        /// 
        /// If the list is editable a string with a default value can be provided
        /// otherwise the index of the default choice should be given.
        abstract current: U2<float, string> option with get, set
        /// Is the item editable?
        abstract editable: bool option with get, set
        /// Placeholder text for editable input
        abstract placeholder: string option with get, set

    /// Constructor options for text input dialogs
    type [<AllowNullLiteral>] ITextOptions =
        inherit IOptions
        /// Default input text
        abstract text: string option with get, set
        /// Placeholder text
        abstract placeholder: string option with get, set
// type IDisposable = PhosphorDisposable.IDisposable
// type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
type Signal<'T,'U> = PhosphorSignaling.Signal<'T,'U> // ``@phosphor_signaling``.Signal
// type Widget = PhosphorWidgets.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract ReactWidget: ReactWidgetStatic
//     abstract VDomRenderer: VDomRendererStatic
//     abstract UseSignal: UseSignalStatic
//     abstract VDomModel: VDomModelStatic

type ReactRenderElement =
    U2<Array<Fable.React.ReactElementTypeWrapper<obj option>>, Fable.React.ReactElementTypeWrapper<obj option>>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ReactRenderElement =
    let ofArray (v: ReactRenderElement) = v |> U2.Case1
    let isArray (v: ReactRenderElement) = match v with U2.Case1 _ -> true | _ -> false
    let asArray (v: ReactRenderElement) = match v with U2.Case1 o -> Some o | _ -> None
    let ``ofReact.ReactElement`` (v: ReactRenderElement)  = v |> U2.Case2
    let ``isReact.ReactElement`` (v: ReactRenderElement) = match v with U2.Case2 _ -> true | _ -> false
    let ``asReact.ReactElement`` (v: ReactRenderElement) = match v with U2.Case2 o -> Some o | _ -> None

/// An abstract class for a Phosphor widget which renders a React component.
type [<AllowNullLiteral>] ReactWidget =
    inherit Widget
    /// Render the content of this widget using the virtual DOM.
    /// 
    /// This method will be called anytime the widget needs to be rendered, which
    /// includes layout triggered rendering.
    /// 
    /// Subclasses should define this method and return the root React nodes here.
    abstract render: unit -> ReactRenderElement
    /// Called to update the state of the widget.
    /// 
    /// The default implementation of this method triggers
    /// VDOM based rendering by calling the `renderDOM` method.
    abstract onUpdateRequest: msg: Message -> unit
    /// Called after the widget is attached to the DOM
    abstract onAfterAttach: msg: Message -> unit
    /// Called before the widget is detached from the DOM.
    abstract onBeforeDetach: msg: Message -> unit
    /// Render the React nodes to the DOM.
    abstract renderDOM: obj with get, set
    abstract renderPromise: Promise<unit> option with get, set

/// An abstract class for a Phosphor widget which renders a React component.
type [<AllowNullLiteral>] ReactWidgetStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ReactWidget
    /// <summary>Creates a new `ReactWidget` that renders a constant element.</summary>
    /// <param name="element">React element to render.</param>
    abstract create: element: ReactRenderElement -> ReactWidget

/// An abstract ReactWidget with a model.
/// The namespace for VDomRenderer statics.
type [<AllowNullLiteral>] VDomRenderer<'T> =
    inherit ReactWidget
    /// A signal emitted when the model changes.
    abstract modelChanged: ISignal<VDomRenderer<'T>, unit>
    /// Set the model and fire changed signals.
    /// Get the current model.
    abstract model: 'T option with get, set
    /// Dispose this widget.
    abstract dispose: unit -> unit
    abstract _model: obj with get, set
    abstract _modelChanged: obj with get, set

/// An abstract ReactWidget with a model.
/// The namespace for VDomRenderer statics.
type [<AllowNullLiteral>] VDomRendererStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> VDomRenderer<'T>

/// Props for the UseSignal component
type [<AllowNullLiteral>] IUseSignalProps<'SENDER, 'ARGS> =
    /// Phosphor signal to connect to.
    abstract signal: ISignal<'SENDER, 'ARGS> with get, set
    /// Initial value to use for the sender, used before the signal emits a value.
    /// If not provided, initial sender will be undefined
    abstract initialSender: 'SENDER option with get, set
    /// Initial value to use for the args, used before the signal emits a value.
    /// If not provided, initial args will be undefined.
    abstract initialArgs: 'ARGS option with get, set
    /// Function mapping the last signal value or inital values to an element to render.
    /// 
    /// Note: returns `React.ReactNode` as per
    /// https://github.com/sw-yx/react-typescript-cheatsheet#higher-order-componentsrender-props
    // abstract children: ('SENDER -> 'ARGS -> React.ReactNode) with get, set
    abstract children: ('SENDER -> 'ARGS -> ReactElement) with get, set
    /// Given the last signal value, should return whether to update the state or not.
    /// 
    /// The default unconditionally returns `true`, so you only have to override if you want
    /// to skip some updates.
    abstract shouldUpdate: ('SENDER -> 'ARGS -> bool) option with get, set

/// State for the UseSignal component
type [<AllowNullLiteral>] IUseSignalState<'SENDER, 'ARGS> =
    abstract value: obj * obj with get, set

/// UseSignal provides a way to hook up a Phosphor signal to a React element,
/// so that the element is re-rendered every time the signal fires.
/// 
/// It is implemented through the "render props" technique, using the `children`
/// prop as a function to render, so that it can be used either as a prop or as a child
/// of this element
/// https://reactjs.org/docs/render-props.html
/// 
/// 
/// Example as child:
/// 
/// ```
/// function LiveButton(isActiveSignal: ISignal<any, boolean>) {
///   return (
///     <UseSignal signal={isActiveSignal} initialArgs={True}>
///      {(_, isActive) => <Button isActive={isActive}>}
///     </UseSignal>
///   )
/// }
/// ```
/// 
/// Example as prop:
/// 
/// ```
/// function LiveButton(isActiveSignal: ISignal<any, boolean>) {
///   return (
///     <UseSignal
///       signal={isActiveSignal}
///       initialArgs={True}
///       children={(_, isActive) => <Button isActive={isActive}>}
///     />
///   )
/// }
// type [<AllowNullLiteral>] UseSignal<'SENDER, 'ARGS> =
//     inherit Component<IUseSignalProps<'SENDER, 'ARGS>, IUseSignalState<'SENDER, 'ARGS>>

type [<AbstractClassAttribute>] UseSignal<'SENDER, 'ARGS> (props)=
    inherit Component<IUseSignalProps<'SENDER, 'ARGS>, IUseSignalState<'SENDER, 'ARGS>>(props)
    // abstract componentDidMount: unit -> unit
    // abstract componentWillUnmount: unit -> unit
    abstract slot: obj with get, set
    // abstract render: unit -> ReactElement // React.ReactNode

/// UseSignal provides a way to hook up a Phosphor signal to a React element,
/// so that the element is re-rendered every time the signal fires.
/// 
/// It is implemented through the "render props" technique, using the `children`
/// prop as a function to render, so that it can be used either as a prop or as a child
/// of this element
/// https://reactjs.org/docs/render-props.html
/// 
/// 
/// Example as child:
/// 
/// ```
/// function LiveButton(isActiveSignal: ISignal<any, boolean>) {
///   return (
///     <UseSignal signal={isActiveSignal} initialArgs={True}>
///      {(_, isActive) => <Button isActive={isActive}>}
///     </UseSignal>
///   )
/// }
/// ```
/// 
/// Example as prop:
/// 
/// ```
/// function LiveButton(isActiveSignal: ISignal<any, boolean>) {
///   return (
///     <UseSignal
///       signal={isActiveSignal}
///       initialArgs={True}
///       children={(_, isActive) => <Button isActive={isActive}>}
///     />
///   )
/// }
type [<AllowNullLiteral>] UseSignalStatic =
    [<Emit "new $0($1...)">] abstract Create: props: IUseSignalProps<'SENDER, 'ARGS> -> UseSignal<'SENDER, 'ARGS>

module VDomRenderer =

    /// An interface for a model to be used with vdom rendering.
    type [<AllowNullLiteral>] IModel =
        inherit IDisposable
        /// A signal emitted when any model state changes.
        abstract stateChanged: ISignal<IModel, unit>

/// Concrete implementation of VDomRenderer model.
type [<AllowNullLiteral>] VDomModel =
    inherit VDomRenderer.IModel
    /// A signal emitted when any model state changes.
    abstract stateChanged: Signal<VDomModel, unit>
    /// Test whether the model is disposed.
    abstract isDisposed: bool
    /// Dispose the model.
    abstract dispose: unit -> unit
    abstract _isDisposed: obj with get, set

/// Concrete implementation of VDomRenderer model.
type [<AllowNullLiteral>] VDomModelStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> VDomModel
// type ReactWidget = __vdom.ReactWidget
type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T>
// type CommandRegistry = PhosphorCommands.CommandRegistry
// type Message = PhosphorMessaging.Message
// type Widget = PhosphorWidgets.Widget
// type IClientSession = __clientsession.IClientSession
type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject

// type [<AllowNullLiteral>] IExports =
//     abstract Toolbar: ToolbarStatic
//     /// <summary>React component for a toolbar button.</summary>
//     /// <param name="props">- The props for ToolbarButtonComponent.</param>
//     abstract ToolbarButtonComponent: props: ToolbarButtonComponent.IProps -> JSX.Element
//     /// <summary>Adds the toolbar button class to the toolbar widget.</summary>
//     /// <param name="w">Toolbar button widget.</param>
//     abstract addToolbarButtonClass: w: Widget -> Widget
//     abstract ToolbarButton: ToolbarButtonStatic
//     /// React component for a toolbar button that wraps a command.
//     /// 
//     /// This wraps the ToolbarButtonComponent and watches the command registry
//     /// for changes to the command.
//     abstract CommandToolbarButtonComponent: props: CommandToolbarButtonComponent.IProps -> JSX.Element
//     abstract addCommandToolbarButtonClass: w: Widget -> Widget
//     abstract CommandToolbarButton: CommandToolbarButtonStatic

type Toolbar =
    Toolbar<obj>

/// A class which provides a toolbar widget.
/// The namespace for Toolbar class statics.
type [<AllowNullLiteral>] Toolbar<'T> =
    inherit Widget
    /// Get an iterator over the ordered toolbar item names.
    abstract names: unit -> IIterator<string>
    /// <summary>Add an item to the end of the toolbar.</summary>
    /// <param name="name">- The name of the widget to add to the toolbar.</param>
    /// <param name="widget">- The widget to add to the toolbar.</param>
    abstract addItem: name: string * widget: 'T -> bool
    /// <summary>Insert an item into the toolbar at the specified index.</summary>
    /// <param name="index">- The index at which to insert the item.</param>
    /// <param name="name">- The name of the item.</param>
    /// <param name="widget">- The widget to add.</param>
    abstract insertItem: index: float * name: string * widget: 'T -> bool
    /// <summary>Insert an item into the toolbar at the after a target item.</summary>
    /// <param name="at">- The target item to insert after.</param>
    /// <param name="name">- The name of the item.</param>
    /// <param name="widget">- The widget to add.</param>
    abstract insertAfter: at: string * name: string * widget: 'T -> bool
    /// <summary>Insert an item into the toolbar at the before a target item.</summary>
    /// <param name="at">- The target item to insert before.</param>
    /// <param name="name">- The name of the item.</param>
    /// <param name="widget">- The widget to add.</param>
    abstract insertBefore: at: string * name: string * widget: 'T -> bool
    abstract _insertRelative: obj with get, set
    /// <summary>Handle the DOM events for the widget.</summary>
    /// <param name="event">- The DOM event sent to the widget.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the dock panel's node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// Handle a DOM click event.
    abstract handleClick: ``event``: Event -> unit
    /// Handle `after-attach` messages for the widget.
    abstract onAfterAttach: msg: Message -> unit
    /// Handle `before-detach` messages for the widget.
    abstract onBeforeDetach: msg: Message -> unit

/// A class which provides a toolbar widget.
/// The namespace for Toolbar class statics.
type [<AllowNullLiteral>] ToolbarStatic =
    /// Construct a new toolbar widget.
    [<Emit "new $0($1...)">] abstract Create: unit -> Toolbar<'T>

module Toolbar =

    type [<AllowNullLiteral>] IExports =
        /// Create an interrupt toolbar item.
        abstract createInterruptButton: session: IClientSession -> Widget
        /// Create a restart toolbar item.
        abstract createRestartButton: session: IClientSession -> Widget
        /// Create a toolbar spacer item.
        /// 
        /// #### Notes
        /// It is a flex spacer that separates the left toolbar items
        /// from the right toolbar items.
        abstract createSpacerItem: unit -> Widget
        /// Create a kernel name indicator item.
        /// 
        /// #### Notes
        /// It will display the `'display_name`' of the current kernel,
        /// or `'No Kernel!'` if there is no kernel.
        /// It can handle a change in context or kernel.
        abstract createKernelNameItem: session: IClientSession -> Widget
        /// Create a kernel status indicator item.
        /// 
        /// #### Notes
        /// It will show a busy status if the kernel status is busy.
        /// It will show the current status in the node title.
        /// It can handle a change to the context or the kernel.
        abstract createKernelStatusItem: session: IClientSession -> Widget

module ToolbarButtonComponent =

    /// Interface for ToolbarButttonComponent props.
    type [<AllowNullLiteral>] IProps =
        abstract className: string option with get, set
        abstract label: string option with get, set
        abstract iconClassName: string option with get, set
        abstract iconLabel: string option with get, set
        abstract tooltip: string option with get, set
        abstract onClick: (unit -> unit) option with get, set
        abstract enabled: bool option with get, set

/// Phosphor Widget version of static ToolbarButtonComponent.
type [<AllowNullLiteral>] ToolbarButton =
    inherit ReactWidget
    abstract props: obj with get, set
    abstract render: unit -> obj // JSX.Element

/// Phosphor Widget version of static ToolbarButtonComponent.
type [<AllowNullLiteral>] ToolbarButtonStatic =
    /// <summary>Creates a toolbar button</summary>
    /// <param name="props">props for underlying `ToolbarButton` componenent</param>
    [<Emit "new $0($1...)">] abstract Create: ?props: ToolbarButtonComponent.IProps -> ToolbarButton

module CommandToolbarButtonComponent =

    /// Interface for CommandToolbarButtonComponent props.
    type [<AllowNullLiteral>] IProps =
        abstract commands: CommandRegistry with get, set
        abstract id: string with get, set
        abstract args: ReadonlyJSONObject option with get, set

/// Phosphor Widget version of CommandToolbarButtonComponent.
type [<AllowNullLiteral>] CommandToolbarButton =
    inherit ReactWidget
    abstract props: obj with get, set
    abstract render: unit -> obj //JSX.Element

/// Phosphor Widget version of CommandToolbarButtonComponent.
type [<AllowNullLiteral>] CommandToolbarButtonStatic =
    /// <summary>Creates a command toolbar button</summary>
    /// <param name="props">props for underlying `CommandToolbarButtonComponent` componenent</param>
    [<Emit "new $0($1...)">] abstract Create: props: CommandToolbarButtonComponent.IProps -> CommandToolbarButton
// type Widget = PhosphorWidgets.Widget

module Printing =

    type [<AllowNullLiteral>] IExports =
        abstract symbol: obj // unique
        /// Returns whether an object implements a print method.
        // abstract isPrintable: a: unknown -> bool
        abstract isPrintable: a: obj -> bool
        /// Returns the print function for an object, or null if it does not provide a handler.
        // abstract getPrintFunction: ``val``: unknown -> OptionalAsyncThunk
        abstract getPrintFunction: ``val``: obj -> OptionalAsyncThunk
        /// Prints a widget by copying it's DOM node
        /// to a hidden iframe and printing that iframe.
        abstract printWidget: widget: Widget -> Promise<unit>
        /// <summary>Prints a URL by loading it into an iframe.</summary>
        /// <param name="url">URL to load into an iframe.</param>
        abstract printURL: url: string -> Promise<unit>

    type [<AllowNullLiteral>] OptionalAsyncThunk =
        [<Emit "$0($1...)">] abstract Invoke: unit -> Promise<unit> option

    /// Objects who provide a custom way of printing themselves
    /// should implement this interface.
    type [<AllowNullLiteral>] IPrintable =
        abstract ``[symbol]``: (unit -> OptionalAsyncThunk) with get, set
// type Message = PhosphorMessaging.Message
// type Widget = PhosphorWidgets.Widget
// type Toolbar = __toolbar.Toolbar
// type Printing = __printing.Printing

// type [<AllowNullLiteral>] IExports =
//     abstract MainAreaWidget: MainAreaWidgetStatic

type MainAreaWidget =
    MainAreaWidget<obj>

/// A widget meant to be contained in the JupyterLab main area.
/// 
/// #### Notes
/// Mirrors all of the `title` attributes of the content.
/// This widget is `closable` by default.
/// This widget is automatically disposed when closed.
/// This widget ensures its own focus when activated.
/// The namespace for the `MainAreaWidget` class statics.
type [<AllowNullLiteral>] MainAreaWidget<'T> =
    inherit Widget
    inherit Printing.IPrintable
    /// Print method. Defered to content.
    abstract ``[Printing.symbol]``: unit -> Printing.OptionalAsyncThunk
    /// The content hosted by the widget.
    abstract content: 'T
    /// The toolbar hosted by the widget.
    abstract toolbar: Toolbar
    /// Whether the content widget or an error is revealed.
    abstract isRevealed: bool
    /// A promise that resolves when the widget is revealed.
    abstract revealed: Promise<unit>
    /// Handle `'activate-request'` messages.
    abstract onActivateRequest: msg: Message -> unit
    /// Handle `'close-request'` messages.
    abstract onCloseRequest: msg: Message -> unit
    /// Handle `'update-request'` messages by forwarding them to the content.
    abstract onUpdateRequest: msg: Message -> unit
    /// Update the title based on the attributes of the child widget.
    abstract _updateTitle: obj with get, set
    /// Update the content title based on attributes of the main widget.
    abstract _updateContentTitle: obj with get, set
    /// Give focus to the content.
    abstract _focusContent: obj with get, set
    abstract _content: obj with get, set
    abstract _toolbar: obj with get, set
    abstract _changeGuard: obj with get, set
    abstract _spinner: obj with get, set
    abstract _isRevealed: obj with get, set
    abstract _revealed: obj with get, set

/// A widget meant to be contained in the JupyterLab main area.
/// 
/// #### Notes
/// Mirrors all of the `title` attributes of the content.
/// This widget is `closable` by default.
/// This widget is automatically disposed when closed.
/// This widget ensures its own focus when activated.
/// The namespace for the `MainAreaWidget` class statics.
type [<AllowNullLiteral>] MainAreaWidgetStatic =
    /// <summary>Construct a new main area widget.</summary>
    /// <param name="options">- The options for initializing the widget.</param>
    [<Emit "new $0($1...)">] abstract Create: options: MainAreaWidget.IOptions<'T> -> MainAreaWidget<'T>
    [<Emit "new $0($1...)">] abstract Create: o: obj -> MainAreaWidget<'T>

module MainAreaWidget =

    type IOptions =
        IOptions<obj>

    /// An options object for creating a main area widget.
    type [<AllowNullLiteral>] IOptions<'T> =
        inherit PhosphorWidgets.Widget.IOptions
        /// The child widget to wrap.
        abstract content: 'T with get, set
        /// The toolbar to use for the widget.  Defaults to an empty toolbar.
        abstract toolbar: Toolbar option with get, set
        /// An optional promise for when the content is ready to be revealed.
        abstract reveal: Promise<obj option> option with get, set

    type IOptionsOptionalContent =
        IOptionsOptionalContent<obj>

    /// An options object for main area widget subclasses providing their own
    /// default content.
    /// 
    /// #### Notes
    /// This makes it easier to have a subclass that provides its own default
    /// content. This can go away once we upgrade to TypeScript 2.8 and have an
    /// easy way to make a single property optional, ala
    /// https://stackoverflow.com/a/46941824
    type [<AllowNullLiteral>] IOptionsOptionalContent<'T> =
        inherit PhosphorWidgets.Widget.IOptions
        /// The child widget to wrap.
        abstract content: 'T option with get, set
        /// The toolbar to use for the widget.  Defaults to an empty toolbar.
        abstract toolbar: Toolbar option with get, set
        /// An optional promise for when the content is ready to be revealed.
        abstract reveal: Promise<obj option> option with get, set
let [<Import("defaultSanitizer","@jupyterlab/apputils/lib/sanitizer")>] defaultSanitizer: ISanitizer = jsNative

/// The namespace for `ISanitizer` related interfaces.
type [<AllowNullLiteral>] ISanitizer =
    /// <summary>Sanitize an HTML string.</summary>
    /// <param name="dirty">- The dirty text.</param>
    /// <param name="options">- The optional sanitization options.</param>
    abstract sanitize: dirty: string * ?options: ISanitizer.IOptions -> string

module ISanitizer =

    /// The options used to sanitize.
    type [<AllowNullLiteral>] IOptions =
        /// The allowed tags.
        abstract allowedTags: ResizeArray<string> option with get, set
        /// The allowed attributes for a given tag.
        abstract allowedAttributes: obj option with get, set
        /// The allowed style values for a given tag.
        abstract allowedStyles: obj option with get, set
// type Message = PhosphorMessaging.Message
// type Widget = PhosphorWidgets.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract Spinner: SpinnerStatic

/// The spinner class.
type [<AllowNullLiteral>] Spinner =
    inherit Widget
    /// Handle `'activate-request'` messages.
    abstract onActivateRequest: msg: Message -> unit

/// The spinner class.
type [<AllowNullLiteral>] SpinnerStatic =
    /// Construct a spinner widget.
    [<Emit "new $0($1...)">] abstract Create: unit -> Spinner
// type IDisposable = PhosphorDisposable.IDisposable
// type Token = PhosphorCoreutils.Token
let [<Import("*","@jupyterlab/apputils/lib/splash")>] ISplashScreen: Token<ISplashScreen> = jsNative

/// The main menu token.
/// The interface for an application splash screen.
type [<AllowNullLiteral>] ISplashScreen =
    /// <summary>Show the application splash screen.</summary>
    /// <param name="light">- Whether to show the light splash screen or the dark one.</param>
    abstract show: ?light: bool -> IDisposable

module Styling =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Style a node and its child elements with the default tag names.</summary>
        /// <param name="node">- The base node.</param>
        /// <param name="className">- The optional CSS class to add to styled nodes.</param>
        abstract styleNode: node: HTMLElement * ?className: string -> unit
        /// <summary>Style a node and its elements that have a given tag name.</summary>
        /// <param name="node">- The base node.</param>
        /// <param name="tagName">- The html tag name to style.</param>
        /// <param name="className">- The optional CSS class to add to styled nodes.</param>
        abstract styleNodeByTag: node: HTMLElement * tagName: string * ?className: string -> unit
        /// Wrap a select node.
        abstract wrapSelect: node: HTMLSelectElement -> HTMLElement
type IChangedArgs<'T> = JupyterlabCoreutils.Interfaces.IChangedArgs<'T>
// type Token = PhosphorCoreutils.Token
// type IDisposable = PhosphorDisposable.IDisposable
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
let [<Import("*","@jupyterlab/apputils/lib/tokens")>] IThemeManager: Token<IThemeManager> = jsNative

/// The theme manager token.
/// An interface for a theme manager.
/// A namespace for the `IThemeManager` sub-types.
type [<AllowNullLiteral>] IThemeManager =
    /// Get the name of the current theme.
    abstract theme: string option
    /// The names of the registered themes.
    abstract themes: ReadonlyArray<string>
    /// A signal fired when the application theme changes.
    abstract themeChanged: ISignal<IThemeManager, IChangedArgs<string>>
    /// <summary>Load a theme CSS file by path.</summary>
    /// <param name="path">- The path of the file to load.</param>
    abstract loadCSS: path: string -> Promise<unit>
    /// <summary>Register a theme with the theme manager.</summary>
    /// <param name="theme">- The theme to register.</param>
    abstract register: theme: IThemeManager.ITheme -> IDisposable
    /// Set the current theme.
    abstract setTheme: name: string -> Promise<unit>
    /// Test whether a given theme is light.
    abstract isLight: name: string -> bool
    /// Test whether a given theme styles scrollbars,
    /// and if the user has scrollbar styling enabled.
    abstract themeScrollbars: name: string -> bool

module IThemeManager =

    /// An interface for a theme.
    type [<AllowNullLiteral>] ITheme =
        /// The display name of the theme.
        abstract name: string with get, set
        /// Whether the theme is light or dark. Downstream authors
        /// of extensions can use this information to customize their
        /// UI depending upon the current theme.
        abstract isLight: bool with get, set
        /// Whether the theme includes styling for the scrollbar.
        /// If set to false, this theme will leave the native scrollbar untouched.
        abstract themeScrollbars: bool option with get, set
        /// Load the theme.
        abstract load: unit -> Promise<unit>
        /// Unload the theme.
        abstract unload: unit -> Promise<unit>
// type IChangedArgs = ``JupyterlabCoreutils``.IChangedArgs
type ISettingRegistry = JupyterlabCoreutils.Tokens.ISettingRegistry
// type IDisposable = PhosphorDisposable.IDisposable
// type Widget = PhosphorWidgets.Widget
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type ISplashScreen = __splash.ISplashScreen
// type IThemeManager = __tokens.IThemeManager

// type [<AllowNullLiteral>] IExports =
//     abstract ThemeManager: ThemeManagerStatic

/// A class that provides theme management.
type [<AllowNullLiteral>] ThemeManager =
    inherit IThemeManager
    /// Get the name of the current theme.
    abstract theme: string option
    /// The names of the registered themes.
    abstract themes: ReadonlyArray<string>
    /// A signal fired when the application theme changes.
    abstract themeChanged: ISignal<ThemeManager, IChangedArgs<string>>
    /// <summary>Get the value of a CSS variable from its key.</summary>
    /// <param name="key">- A Jupyterlab CSS variable, without the leading '--jp-'.</param>
    abstract getCSS: key: string -> string
    /// <summary>Load a theme CSS file by path.</summary>
    /// <param name="path">- The path of the file to load.</param>
    abstract loadCSS: path: string -> Promise<unit>
    /// Loads all current CSS overrides from settings. If an override has been
    /// removed or is invalid, this function unloads it instead.
    abstract loadCSSOverrides: unit -> unit
    /// <summary>Validate a CSS value w.r.t. a key</summary>
    /// <param name="key">- A Jupyterlab CSS variable, without the leading '--jp-'.</param>
    /// <param name="val">- A candidate CSS value</param>
    abstract validateCSS: key: string * ``val``: string -> bool
    /// <summary>Register a theme with the theme manager.</summary>
    /// <param name="theme">- The theme to register.</param>
    abstract register: theme: IThemeManager.ITheme -> IDisposable
    /// Add a CSS override to the settings.
    abstract setCSSOverride: key: string * value: string -> Promise<unit>
    /// Set the current theme.
    abstract setTheme: name: string -> Promise<unit>
    /// Test whether a given theme is light.
    abstract isLight: name: string -> bool
    /// <summary>Increase a font size w.r.t. its current setting or its value in the
    /// current theme.</summary>
    /// <param name="key">- A Jupyterlab font size CSS variable, without the leading '--jp-'.</param>
    abstract incrFontSize: key: string -> Promise<unit>
    /// <summary>Decrease a font size w.r.t. its current setting or its value in the
    /// current theme.</summary>
    /// <param name="key">- A Jupyterlab font size CSS variable, without the leading '--jp-'.</param>
    abstract decrFontSize: key: string -> Promise<unit>
    /// Test whether a given theme styles scrollbars,
    /// and if the user has scrollbar styling enabled.
    abstract themeScrollbars: name: string -> bool
    /// Test if the user has scrollbar styling enabled.
    abstract isToggledThemeScrollbars: unit -> bool
    /// Toggle the `theme-scrollbbars` setting.
    abstract toggleThemeScrollbars: unit -> Promise<unit>
    /// Change a font size by a positive or negative increment.
    abstract _incrFontSize: obj with get, set
    /// Initialize the key -> property dict for the overrides
    abstract _initOverrideProps: obj with get, set
    /// Handle the current settings.
    abstract _loadSettings: obj with get, set
    /// Load the theme.
    /// 
    /// #### Notes
    /// This method assumes that the `theme` exists.
    abstract _loadTheme: obj with get, set
    /// Handle a theme error.
    abstract _onError: obj with get, set
    abstract _base: obj with get, set
    abstract _current: obj with get, set
    abstract _host: obj with get, set
    abstract _links: obj with get, set
    abstract _overrides: obj with get, set
    abstract _overrideProps: obj with get, set
    abstract _outstanding: obj with get, set
    abstract _pending: obj with get, set
    abstract _requests: obj with get, set
    abstract _settings: obj with get, set
    abstract _splash: obj with get, set
    abstract _themes: obj with get, set
    abstract _themeChanged: obj with get, set

/// A class that provides theme management.
type [<AllowNullLiteral>] ThemeManagerStatic =
    /// Construct a new theme manager.
    [<Emit "new $0($1...)">] abstract Create: options: ThemeManager.IOptions -> ThemeManager

module ThemeManager =

    /// The options used to create a theme manager.
    type [<AllowNullLiteral>] IOptions =
        /// The host widget for the theme manager.
        abstract host: Widget with get, set
        /// The setting registry key that holds theme setting data.
        abstract key: string with get, set
        /// The settings registry.
        abstract settings: ISettingRegistry with get, set
        /// The splash screen to show when loading themes.
        abstract splash: ISplashScreen option with get, set
        /// The url for local theme loading.
        abstract url: string with get, set
// type Token = PhosphorCoreutils.Token
let [<Import("*","@jupyterlab/apputils/lib/windowresolver")>] IWindowResolver: Token<IWindowResolver> = jsNative

// type [<AllowNullLiteral>] IExports =
//     abstract WindowResolver: WindowResolverStatic

/// The default window resolver token.
/// The description of a window name resolver.
type [<AllowNullLiteral>] IWindowResolver =
    /// A window name to use as a handle among shared resources.
    abstract name: string

/// A concrete implementation of a window name resolver.
type [<AllowNullLiteral>] WindowResolver =
    inherit IWindowResolver
    /// The resolved window name.
    abstract name: string
    /// <summary>Resolve a window name to use as a handle among shared resources.</summary>
    /// <param name="candidate">- The potential window name being resolved.
    /// 
    /// #### Notes
    /// Typically, the name candidate should be a JupyterLab workspace name or
    /// an empty string if there is no workspace.
    /// 
    /// If the returned promise rejects, a window name cannot be resolved without
    /// user intervention, which typically means navigation to a new URL.</param>
    abstract resolve: candidate: string -> Promise<unit>
    abstract _name: obj with get, set

/// A concrete implementation of a window name resolver.
type [<AllowNullLiteral>] WindowResolverStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WindowResolver
