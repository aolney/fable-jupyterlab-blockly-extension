// ts2fable 0.6.1
module rec JupyterlabApplication
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

//amo typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
type KeyboardEvent = Browser.Types.KeyboardEvent
type Error = Node.Base.Error 
type TypeError = obj 
type PromiseLike<'T> = Promise<'T>
//end typescript

type CommandRegistry = PhosphorCommands.CommandRegistry // ``@phosphor_commands``.CommandRegistry //AMO bacticked variables starting with @ throughout
// type ServerConnection = JupyterlabServices.Serverconnection.ServerConnection // ``@jupyterlab_services``.ServerConnection
type ServiceManager = JupyterlabServices.Manager.ServiceManager // ``@jupyterlab_services``.ServiceManager
type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // ``@phosphor_coreutils``.ReadonlyJSONObject
type Token<'T> = PhosphorCoreutils.Token<'T> // ``@phosphor_coreutils``.Token
type IDisposable = PhosphorDisposable.IDisposable // ``@phosphor_disposable``.IDisposable
type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // ``@phosphor_signaling``.ISignal
let [<Import("*","@jupyterlab/application/lib/tokens")>] IConnectionLost: Token<IConnectionLost> = jsNative
let [<Import("*","@jupyterlab/application/lib/tokens")>] IRouter: Token<IRouter> = jsNative

type [<AllowNullLiteral>] IConnectionLost =
    [<Emit "$0($1...)">] abstract Invoke: manager: JupyterlabServices.Manager.ServiceManager.IManager * err: JupyterlabServices.Serverconnection.ServerConnection.NetworkError -> Promise<unit>

/// The URL Router token.
/// A static class that routes URLs within the application.
/// A namespace for the `IRouter` specification.
type [<AllowNullLiteral>] IRouter =
    /// The base URL for the router.
    abstract ``base``: string
    /// The command registry used by the router.
    abstract commands: CommandRegistry
    /// The parsed current URL of the application.
    abstract current: IRouter.ILocation
    /// A signal emitted when the router routes a route.
    abstract routed: ISignal<IRouter, IRouter.ILocation>
    /// If a matching rule's command resolves with the `stop` token during routing,
    /// no further matches will execute.
    abstract stop: Token<unit>
    /// <summary>Navigate to a new path within the application.</summary>
    /// <param name="path">- The new path or empty string if redirecting to root.</param>
    /// <param name="options">- The navigation options.</param>
    abstract navigate: path: string * ?options: IRouter.INavOptions -> unit
    /// <summary>Register a rule that maps a path pattern to a command.</summary>
    /// <param name="options">- The route registration options.</param>
    abstract register: options: IRouter.IRegisterOptions -> IDisposable
    /// Cause a hard reload of the document.
    abstract reload: unit -> unit
    /// <summary>Route a specific path to an action.</summary>
    /// <param name="url">- The URL string that will be routed.
    /// 
    /// #### Notes
    /// If a pattern is matched, its command will be invoked with arguments that
    /// match the `IRouter.ILocation` interface.</param>
    abstract route: url: string -> unit

module IRouter =
    // amo
    type RegExp = System.Text.RegularExpressions.Regex

    /// The parsed location currently being routed.
    type [<AllowNullLiteral>] ILocation =
        inherit ReadonlyJSONObject
        /// The location hash.
        abstract hash: string with get, set
        /// The path that matched a routing pattern.
        abstract path: string with get, set
        /// The request being routed with the router `base` omitted.
        /// 
        /// #### Notes
        /// This field includes the query string and hash, if they exist.
        abstract request: string with get, set
        /// The search element, including leading question mark (`'?'`), if any,
        /// of the path.
        abstract search: string with get, set

    /// The options passed into a navigation request.
    type [<AllowNullLiteral>] INavOptions =
        /// Whether the navigation should be hard URL change instead of an HTML
        /// history API change.
        abstract hard: bool option with get, set

    /// The specification for registering a route with the router.
    type [<AllowNullLiteral>] IRegisterOptions =
        /// The command string that will be invoked upon matching.
        abstract command: string with get, set
        /// The regular expression that will be matched against URLs.
        abstract pattern: RegExp with get, set
        /// The rank order of the registered rule. A lower rank denotes a higher
        /// priority. The default rank is `100`.
        abstract rank: float option with get, set
type CommandLinker = JupyterlabApputils.CommandLinker //``@jupyterlab_apputils``.CommandLinker
type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry
// type ServiceManager = ``@jupyterlab_services``.ServiceManager //AMO duplicate type
type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T>
type Application<'T> = PhosphorApplication.Application<'T>
type IPlugin<'T,'U> = PhosphorApplication.IPlugin<'T,'U>
// type Token = ``@phosphor_coreutils``.Token //AMO duplicate type
type Widget = PhosphorWidgets.Widget

type [<AllowNullLiteral>] IExports =
    abstract JupyterFrontEnd: JupyterFrontEndStatic
    abstract LabStatus: LabStatusStatic
    abstract JupyterLab: JupyterLabStatic //duplicate below
    abstract LayoutRestorer: LayoutRestorerStatic
    /// Create rendermime plugins for rendermime extension modules.
    abstract createRendermimePlugins: extensions: ResizeArray<JupyterlabRendermimeInterfaces.IRenderMime.IExtensionModule> -> ResizeArray<JupyterFrontEndPlugin<U2<unit, IMimeDocumentTracker>>>
    /// Create rendermime plugins for rendermime extension modules.
    abstract createRendermimePlugin: tracker: JupyterlabApputils.WidgetTracker<MimeDocument> * item: JupyterlabRendermimeInterfaces.IRenderMime.IExtension -> JupyterFrontEndPlugin<unit>
    abstract Router: RouterStatic

type JupyterFrontEndPlugin<'T> =
    IPlugin<JupyterFrontEnd, 'T>

type JupyterFrontEnd =
    JupyterFrontEnd<obj>

/// The base Jupyter front-end application class.
/// The namespace for `JupyterFrontEnd` class statics.
type [<AllowNullLiteral>] JupyterFrontEnd<'T> =
    inherit Application<'T>
    /// The name of this Jupyter front-end application.
    abstract name: string
    /// A namespace/prefix plugins may use to denote their provenance.
    abstract ``namespace``: string
    /// The version of this Jupyter front-end application.
    abstract version: string
    /// The command linker used by the application.
    abstract commandLinker: CommandLinker
    /// The document registry instance used by the application.
    abstract docRegistry: DocumentRegistry
    /// Promise that resolves when state is first restored.
    abstract restored: Promise<unit>
    /// The service manager used by the application.
    abstract serviceManager: ServiceManager
    /// <summary>Walks up the DOM hierarchy of the target of the active `contextmenu`
    /// event, testing each HTMLElement ancestor for a user-supplied funcion. This can
    /// be used to find an HTMLElement on which to operate, given a context menu click.</summary>
    /// <param name="test">- a function that takes an `HTMLElement` and returns a
    /// boolean for whether it is the element the requester is seeking.</param>
    abstract contextMenuHitTest: test: (HTMLElement -> bool) -> HTMLElement option
    /// A method invoked on a document `'contextmenu'` event.
    abstract evtContextMenu: ``event``: MouseEvent -> unit
    abstract _contextMenuEvent: obj with get, set

/// The base Jupyter front-end application class.
/// The namespace for `JupyterFrontEnd` class statics.
type [<AllowNullLiteral>] JupyterFrontEndStatic =
    /// Construct a new JupyterFrontEnd object.
    [<Emit "new $0($1...)">] abstract Create: options: JupyterFrontEnd.IOptions<'T> -> JupyterFrontEnd<'T>

module JupyterFrontEnd =

    // type Application<'T> = PhosphorApplication.Application<'T>
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry

    type [<AllowNullLiteral>] IExports =
        abstract IPaths: Token<IPaths>
        abstract LabShell: LabShellStatic

    type IOptions<'U> =
        IOptions<obj, 'U>

    type IOptions =
        IOptions<obj, obj>

    /// The options used to initialize a JupyterFrontEnd.
    type [<AllowNullLiteral>] IOptions<'T, 'U> =
        inherit PhosphorApplication.Application.IOptions<'T>
        /// The document registry instance used by the application.
        abstract docRegistry: DocumentRegistry option with get, set
        /// The command linker used by the application.
        abstract commandLinker: CommandLinker option with get, set
        /// The service manager used by the application.
        abstract serviceManager: ServiceManager option with get, set
        /// Promise that resolves when state is first restored, returning layout
        /// description.
        abstract restored: Promise<'U> option with get, set

    /// A minimal shell type for Jupyter front-end applications.
    type [<AllowNullLiteral>] IShell =
        inherit Widget
        /// <summary>Activates a widget inside the application shell.</summary>
        /// <param name="id">- The ID of the widget being activated.</param>
        abstract activateById: id: string -> unit
        /// <summary>Add a widget to the application shell.</summary>
        /// <param name="widget">- The widget being added.</param>
        /// <param name="area">- Optional region in the shell into which the widget should
        /// be added.</param>
        /// <param name="options">- Optional flags the shell might use when opening the
        /// widget, as defined in the `DocumentRegistry`.</param>
        abstract add: widget: Widget * ?area: string * ?options: JupyterlabDocregistry.Registry.DocumentRegistry.IOpenOptions -> unit
        /// The focused widget in the application shell.
        /// 
        /// #### Notes
        /// Different shell implementations have latitude to decide what "current"
        /// or "focused" mean, depending on their user interface characteristics.
        abstract currentWidget: Widget
        /// <summary>Returns an iterator for the widgets inside the application shell.</summary>
        /// <param name="area">- Optional regions in the shell whose widgets are iterated.</param>
        abstract widgets: ?area: string -> IIterator<Widget>

    /// The application paths dictionary token.
    /// An interface for URL and directory paths used by a Jupyter front-end.
    type [<AllowNullLiteral>] IPaths =
        /// The urls used by the application.
        abstract urls: obj
        /// The server directories used by the application, for user information
        /// only.
        /// 
        /// #### Notes
        /// These are for user information and user interface hints only and should
        /// not be relied on in code. A server may set these to empty strings if it
        /// does not want to expose this information.
        /// 
        /// Examples of appropriate use include displaying a help dialog for a user
        /// listing the paths, or a tooltip in a filebrowser displaying the server
        /// root. Examples of inapproriate use include using one of these paths in a
        /// terminal command, generating code using these paths, or using one of
        /// these paths in a request to the server (it would be better to write a
        /// server extension to handle these cases).
        abstract directories: obj
// type DocumentRegistry = ``@jupyterlab_docregistry``.DocumentRegistry //AMO duplicate type
// type IIterator = ``@phosphor_algorithm``.IIterator //AMO duplicate type
// type Token = ``@phosphor_coreutils``.Token //AMO duplicate type
type Message = PhosphorMessaging.Message
// type ISignal = ``@phosphor_signaling``.ISignal  //AMO duplicate type
type DockLayout = PhosphorWidgets.DockLayout
type DockPanel = PhosphorWidgets.DockPanel
type FocusTracker<'T> = PhosphorWidgets.FocusTracker<'T>
// type Widget = ``@phosphor_widgets``.Widget //AMO duplicate type
// type JupyterFrontEnd = __frontend.JupyterFrontEnd //AMO duplicate type
let [<Import("*","@jupyterlab/application/lib/shell")>] ILabShell: Token<ILabShell> = jsNative

// type [<AllowNullLiteral>] IExports =
//     abstract LabShell: LabShellStatic

/// The JupyterLab application shell token.
/// The JupyterLab application shell interface.
/// The namespace for `ILabShell` type information.
type [<AllowNullLiteral>] ILabShell =
    inherit LabShell

module ILabShell =

    type [<StringEnum>] [<RequireQualifiedAccess>] Area =
        | Main
        | Header
        | Top
        | Left
        | Right
        | Bottom

    type AreaConfig =
        PhosphorWidgets.DockLayout.AreaConfig

    type IChangedArgs =
        PhosphorWidgets.FocusTracker.IChangedArgs<Widget>

    /// A description of the application's user interface layout.
    type [<AllowNullLiteral>] ILayout =
        /// Indicates whether fetched session restore data was actually retrieved
        /// from the state database or whether it is a fresh blank slate.
        /// 
        /// #### Notes
        /// This attribute is only relevant when the layout data is retrieved via a
        /// `fetch` call. If it is set when being passed into `save`, it will be
        /// ignored.
        abstract fresh: bool option
        /// The main area of the user interface.
        abstract mainArea: IMainArea option
        /// The left area of the user interface.
        abstract leftArea: ISideArea option
        /// The right area of the user interface.
        abstract rightArea: ISideArea option

    /// The restorable description of the main application area.
    type [<AllowNullLiteral>] IMainArea =
        /// The current widget that has application focus.
        abstract currentWidget: Widget option
        /// The contents of the main application dock panel.
        abstract dock: PhosphorWidgets.DockLayout.ILayoutConfig option
        /// The document mode (i.e., multiple/single) of the main dock panel.
        abstract mode: PhosphorWidgets.DockPanel.Mode option

    /// The restorable description of a sidebar in the user interface.
    type [<AllowNullLiteral>] ISideArea =
        /// A flag denoting whether the sidebar has been collapsed.
        abstract collapsed: bool
        /// The current widget that has side area focus.
        abstract currentWidget: Widget option
        /// The collection of widgets held by the sidebar.
        abstract widgets: Array<Widget> option

/// The application shell for JupyterLab.
type [<AllowNullLiteral>] LabShell =
    inherit Widget
    inherit JupyterFrontEnd.IShell
    /// A signal emitted when main area's active focus changes.
    abstract activeChanged: ISignal<LabShell, ILabShell.IChangedArgs>
    /// The active widget in the shell's main area.
    abstract activeWidget: Widget option
    /// A signal emitted when main area's current focus changes.
    abstract currentChanged: ISignal<LabShell, ILabShell.IChangedArgs>
    /// The current widget in the shell's main area.
    abstract currentWidget: Widget option
    /// A signal emitted when the main area's layout is modified.
    abstract layoutModified: ISignal<LabShell, unit>
    /// Whether the left area is collapsed.
    abstract leftCollapsed: bool
    /// Whether the left area is collapsed.
    abstract rightCollapsed: bool
    /// Whether JupyterLab is in presentation mode with the
    /// `jp-mod-presentationMode` CSS class.
    /// Enable/disable presentation mode (`jp-mod-presentationMode` CSS class) with
    /// a boolean.
    abstract presentationMode: bool with get, set
    /// The main dock area's user interface mode.
    abstract mode: PhosphorWidgets.DockPanel.Mode with get, set
    /// Promise that resolves when state is first restored, returning layout
    /// description.
    abstract restored: Promise<ILabShell.ILayout>
    /// Activate a widget in its area.
    abstract activateById: id: string -> unit
    abstract activateNextTab: unit -> unit
    abstract activatePreviousTab: unit -> unit
    abstract add: widget: Widget * ?area: ILabShell.Area * ?options: JupyterlabDocregistry.Registry.DocumentRegistry.IOpenOptions -> unit
    /// Collapse the left area.
    abstract collapseLeft: unit -> unit
    /// Collapse the right area.
    abstract collapseRight: unit -> unit
    /// Dispose the shell.
    abstract dispose: unit -> unit
    /// Expand the left area.
    /// 
    /// #### Notes
    /// This will open the most recently used tab,
    /// or the first tab if there is no most recently used.
    abstract expandLeft: unit -> unit
    /// Expand the right area.
    /// 
    /// #### Notes
    /// This will open the most recently used tab,
    /// or the first tab if there is no most recently used.
    abstract expandRight: unit -> unit
    /// Close all widgets in the main area.
    abstract closeAll: unit -> unit
    /// True if the given area is empty.
    abstract isEmpty: area: ILabShell.Area -> bool
    /// Restore the layout state for the application shell.
    abstract restoreLayout: layout: ILabShell.ILayout -> unit
    /// Save the dehydrated state of the application shell.
    abstract saveLayout: unit -> ILabShell.ILayout
    /// Returns the widgets for an application area.
    abstract widgets: ?area: ILabShell.Area -> IIterator<Widget>
    /// Handle `after-attach` messages for the application shell.
    abstract onAfterAttach: msg: Message -> unit
    /// Add a widget to the left content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    abstract _addToLeftArea: obj with get, set
    /// Add a widget to the main content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    /// All widgets added to the main area should be disposed after removal
    /// (disposal before removal will remove the widget automatically).
    /// 
    /// In the options, `ref` defaults to `null`, `mode` defaults to `'tab-after'`,
    /// and `activate` defaults to `true`.
    abstract _addToMainArea: obj with get, set
    /// Add a widget to the right content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    abstract _addToRightArea: obj with get, set
    /// Add a widget to the top content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    abstract _addToTopArea: obj with get, set
    /// Add a widget to the header content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    abstract _addToHeaderArea: obj with get, set
    /// Add a widget to the bottom content area.
    /// 
    /// #### Notes
    /// Widgets must have a unique `id` property, which will be used as the DOM id.
    abstract _addToBottomArea: obj with get, set
    abstract _adjacentBar: obj with get, set
    abstract _currentTabBar: obj with get, set
    /// Handle a change to the dock area active widget.
    abstract _onActiveChanged: obj with get, set
    /// Handle a change to the dock area current widget.
    abstract _onCurrentChanged: obj with get, set
    /// Handle a change to the layout.
    abstract _onLayoutModified: obj with get, set
    /// A message hook for child add/remove messages on the main area dock panel.
    abstract _dockChildHook: obj with get, set
    abstract _activeChanged: obj with get, set
    abstract _cachedLayout: obj with get, set
    abstract _currentChanged: obj with get, set
    abstract _dockPanel: obj with get, set
    abstract _isRestored: obj with get, set
    abstract _layoutModified: obj with get, set
    abstract _layoutDebouncer: obj with get, set
    abstract _leftHandler: obj with get, set
    abstract _restored: obj with get, set
    abstract _rightHandler: obj with get, set
    abstract _tracker: obj with get, set
    abstract _headerPanel: obj with get, set
    abstract _topPanel: obj with get, set
    abstract _bottomPanel: obj with get, set
    abstract _mainOptionsCache: obj with get, set
    abstract _sideOptionsCache: obj with get, set

/// The application shell for JupyterLab.
type [<AllowNullLiteral>] LabShellStatic =
    /// Construct a new application shell.
    [<Emit "new $0($1...)">] abstract Create: unit -> LabShell
// type Token = ``@phosphor_coreutils``.Token //AMO duplicate type
// type IDisposable = ``@phosphor_disposable``.IDisposable //AMO duplicate type
// type ISignal = ``@phosphor_signaling``.ISignal //AMO duplicate type
// type JupyterFrontEnd = __frontend.JupyterFrontEnd //AMO duplicate type
let [<Import("*","@jupyterlab/application/lib/status")>] ILabStatus: Token<ILabStatus> = jsNative

// type [<AllowNullLiteral>] IExports = 
//     abstract LabStatus: LabStatusStatic

/// The application status token.
/// An interface for JupyterLab-like application status functionality.
type [<AllowNullLiteral>] ILabStatus =
    /// A signal for when application changes its busy status.
    abstract busySignal: ISignal<JupyterFrontEnd, bool>
    /// A signal for when application changes its dirty status.
    abstract dirtySignal: ISignal<JupyterFrontEnd, bool>
    /// Whether the application is busy.
    abstract isBusy: bool
    /// Whether the application is dirty.
    abstract isDirty: bool
    /// Set the application state to busy.
    abstract setBusy: unit -> IDisposable
    /// Set the application state to dirty.
    abstract setDirty: unit -> IDisposable

/// The application status signals and flags class.
type [<AllowNullLiteral>] LabStatus =
    inherit ILabStatus
    /// Returns a signal for when application changes its busy status.
    abstract busySignal: ISignal<JupyterFrontEnd, bool>
    /// Returns a signal for when application changes its dirty status.
    abstract dirtySignal: ISignal<JupyterFrontEnd, bool>
    /// Whether the application is busy.
    abstract isBusy: bool
    /// Whether the application is dirty.
    abstract isDirty: bool
    /// Set the application state to dirty.
    abstract setDirty: unit -> IDisposable
    /// Set the application state to busy.
    abstract setBusy: unit -> IDisposable
    abstract _busyCount: obj with get, set
    abstract _busySignal: obj with get, set
    abstract _dirtyCount: obj with get, set
    abstract _dirtySignal: obj with get, set

/// The application status signals and flags class.
type [<AllowNullLiteral>] LabStatusStatic =
    /// Construct a new  status object.
    [<Emit "new $0($1...)">] abstract Create: app: JupyterFrontEnd -> LabStatus
// type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // ``@jupyterlab_rendermime_interfaces``.IRenderMime
// type Token = ``@phosphor_coreutils``.Token
// type JupyterFrontEnd = __frontend.JupyterFrontEnd
// type JupyterFrontEndPlugin = __frontend.JupyterFrontEndPlugin
// type ILabShell = __shell.ILabShell
// type LabShell = __shell.LabShell
// type LabStatus = __status.LabStatus

// type [<AllowNullLiteral>] IExports =
//     abstract JupyterLab: JupyterLabStatic

/// JupyterLab is the main application class. It is instantiated once and shared.
/// The namespace for `JupyterLab` class statics.
type [<AllowNullLiteral>] JupyterLab =
    inherit JupyterFrontEnd<ILabShell>
    /// The name of the JupyterLab application.
    abstract name: string
    /// A namespace/prefix plugins may use to denote their provenance.
    abstract ``namespace``: string
    /// A list of all errors encountered when registering plugins.
    abstract registerPluginErrors: Array<Error>
    /// Promise that resolves when state is first restored, returning layout
    /// description.
    abstract restored: Promise<unit>
    /// The application busy and dirty status signals and flags.
    abstract status: LabStatus
    /// The version of the JupyterLab application.
    abstract version: string
    /// The JupyterLab application information dictionary.
    abstract info: JupyterLab.IInfo
    /// The JupyterLab application paths dictionary.
    abstract paths: JupyterFrontEnd.IPaths
    /// <summary>Register plugins from a plugin module.</summary>
    /// <param name="mod">- The plugin module to register.</param>
    abstract registerPluginModule: ``mod``: JupyterLab.IPluginModule -> unit
    /// <summary>Register the plugins from multiple plugin modules.</summary>
    /// <param name="mods">- The plugin modules to register.</param>
    abstract registerPluginModules: mods: ResizeArray<JupyterLab.IPluginModule> -> unit
    abstract _info: obj with get, set
    abstract _paths: obj with get, set

/// JupyterLab is the main application class. It is instantiated once and shared.
/// The namespace for `JupyterLab` class statics.
type [<AllowNullLiteral>] JupyterLabStatic =
    /// Construct a new JupyterLab object.
    [<Emit "new $0($1...)">] abstract Create: ?options: JupyterLab.IOptions -> JupyterLab

module JupyterLab =

    type [<AllowNullLiteral>] IExports =
        abstract IInfo: Token<IInfo>
        abstract defaultInfo: IInfo
        abstract defaultPaths: JupyterFrontEnd.IPaths

    /// The options used to initialize a JupyterLab object.
    type [<AllowNullLiteral>] IOptions =
        inherit JupyterFrontEnd.IOptions<LabShell>
        // inherit obj
        abstract paths: obj option with get, set

    /// The layout restorer token.
    /// The information about a JupyterLab application.
    type [<AllowNullLiteral>] IInfo =
        /// Whether the application is in dev mode.
        abstract devMode: bool
        /// The collection of deferred extension patterns and matched extensions.
        abstract deferred: obj
        /// The collection of disabled extension patterns and matched extensions.
        abstract disabled: obj
        /// The mime renderer extensions.
        abstract mimeExtensions: ResizeArray<JupyterlabRendermimeInterfaces.IRenderMime.IExtensionModule>
        /// Whether files are cached on the server.
        abstract filesCached: bool

    /// The interface for a module that exports a plugin or plugins as
    /// the default value.
    type [<AllowNullLiteral>] IPluginModule =
        /// The default export.
        abstract ``default``: U2<JupyterFrontEndPlugin<obj option>, ResizeArray<JupyterFrontEndPlugin<obj option>>> with get, set
type WidgetTracker = JupyterlabApputils.WidgetTracker
type IDataConnector<'T> = JupyterlabCoreutils.Interfaces.IDataConnector<'T>
type IRestorer = JupyterlabCoreutils.Interfaces.IRestorer
// type CommandRegistry = ``@phosphor_commands``.CommandRegistry
type ReadonlyJSONValue = PhosphorCoreutils.ReadonlyJSONValue
// type Token = ``@phosphor_coreutils``.Token
// type Widget = ``@phosphor_widgets``.Widget
// type ILabShell = __shell.ILabShell
// let [<Import("*","@jupyterlab/application/lib/layoutrestorer")>] ILayoutRestorer: Token<ILayoutRestorer> = jsNative
let [<ImportMember("@jupyterlab/application")>] ILayoutRestorer: Token<ILayoutRestorer> = jsNative
// type [<AllowNullLiteral>] IExports =
//     abstract LayoutRestorer: LayoutRestorerStatic

/// The layout restorer token.
/// A static class that restores the widgets of the application when it reloads.
type [<AllowNullLiteral>] ILayoutRestorer =
    inherit IRestorer
    /// A promise resolved when the layout restorer is ready to receive signals.
    abstract restored: Promise<unit> with get, set
    /// Add a widget to be tracked by the layout restorer.
    abstract add: widget: Widget * name: string -> unit
    /// <summary>Restore the widgets of a particular widget tracker.</summary>
    /// <param name="tracker">- The widget tracker whose widgets will be restored.</param>
    /// <param name="options">- The restoration options.</param>
    abstract restore: tracker: JupyterlabApputils.WidgetTracker<'T> * options:  JupyterlabCoreutils.Interfaces.IRestorer.IOptions<'T> -> Promise<obj option>

/// The default implementation of a layout restorer.
/// 
/// #### Notes
/// The lifecycle for state restoration is subtle. The sequence of events is:
/// 
/// 1. The layout restorer plugin is instantiated and makes a `fetch` call to
///     the data connector that stores the layout restoration data. The `fetch`
///     call returns a promise that resolves in step 6, below.
/// 
/// 2. Other plugins that care about state restoration require the layout
///     restorer as a dependency.
/// 
/// 3. As each load-time plugin initializes (which happens before the front-end
///     application has `started`), it instructs the layout restorer whether
///     the restorer ought to `restore` its widgets by passing in its widget
///     tracker.
///     Alternatively, a plugin that does not require its own widget tracker
///     (because perhaps it only creates a single widget, like a command palette),
///     can simply `add` its widget along with a persistent unique name to the
///     layout restorer so that its layout state can be restored when the lab
///     application restores.
/// 
/// 4. After all the load-time plugins have finished initializing, the front-end
///     application `started` promise will resolve. This is the `first`
///     promise that the layout restorer waits for. By this point, all of the
///     plugins that care about restoration will have instructed the layout
///     restorer to `restore` their widget trackers.
/// 
/// 5. The layout restorer will then instruct each plugin's widget tracker
///     to restore its state and reinstantiate whichever widgets it wants. The
///     tracker returns a promise to the layout restorer that resolves when it
///     has completed restoring the tracked widgets it cares about.
/// 
/// 6. As each widget tracker finishes restoring the widget instances it cares
///     about, it resolves the promise that was returned to the layout restorer
///     (in step 5). After all of the promises that the restorer is awaiting have
///     settled, the restorer then resolves the outstanding `fetch` promise
///     (from step 1) and hands off a layout state object to the application
///     shell's `restoreLayout` method for restoration.
/// 
/// 7. Once the application shell has finished restoring the layout, the
///     JupyterLab application's `restored` promise is resolved.
/// 
/// Of particular note are steps 5 and 6: since data restoration of plugins
/// is accomplished by executing commands, the command that is used to restore
/// the data of each plugin must return a promise that only resolves when the
/// widget has been created and added to the plugin's widget tracker.
/// A namespace for `LayoutRestorer` statics.
type [<AllowNullLiteral>] LayoutRestorer =
    inherit ILayoutRestorer
    /// A promise resolved when the layout restorer is ready to receive signals.
    abstract restored: Promise<unit>
    /// Add a widget to be tracked by the layout restorer.
    abstract add: widget: Widget * name: string -> unit
    /// Fetch the layout state for the application.
    /// 
    /// #### Notes
    /// Fetching the layout relies on all widget restoration to be complete, so
    /// calls to `fetch` are guaranteed to return after restoration is complete.
    abstract fetch: unit -> Promise<ILabShell.ILayout>
    /// <summary>Restore the widgets of a particular widget tracker.</summary>
    /// <param name="tracker">- The widget tracker whose widgets will be restored.</param>
    /// <param name="options">- The restoration options.</param>
    abstract restore: tracker: WidgetTracker * options:  JupyterlabCoreutils.Interfaces.IRestorer.IOptions<Widget> -> Promise<obj option>
    /// Save the layout state for the application.
    abstract save: data: ILabShell.ILayout -> Promise<unit>
    /// Dehydrate a main area description into a serializable object.
    abstract _dehydrateMainArea: obj with get, set
    /// Reydrate a serialized main area description object.
    /// 
    /// #### Notes
    /// This function consumes data that can become corrupted, so it uses type
    /// coercion to guarantee the dehydrated object is safely processed.
    abstract _rehydrateMainArea: obj with get, set
    /// Dehydrate a side area description into a serializable object.
    abstract _dehydrateSideArea: obj with get, set
    /// Reydrate a serialized side area description object.
    /// 
    /// #### Notes
    /// This function consumes data that can become corrupted, so it uses type
    /// coercion to guarantee the dehydrated object is safely processed.
    abstract _rehydrateSideArea: obj with get, set
    /// Handle a widget disposal.
    abstract _onWidgetDisposed: obj with get, set
    abstract _connector: obj with get, set
    abstract _first: obj with get, set
    abstract _firstDone: obj with get, set
    abstract _promisesDone: obj with get, set
    abstract _promises: obj with get, set
    abstract _restored: obj with get, set
    abstract _registry: obj with get, set
    abstract _trackers: obj with get, set
    abstract _widgets: obj with get, set

/// The default implementation of a layout restorer.
/// 
/// #### Notes
/// The lifecycle for state restoration is subtle. The sequence of events is:
/// 
/// 1. The layout restorer plugin is instantiated and makes a `fetch` call to
///     the data connector that stores the layout restoration data. The `fetch`
///     call returns a promise that resolves in step 6, below.
/// 
/// 2. Other plugins that care about state restoration require the layout
///     restorer as a dependency.
/// 
/// 3. As each load-time plugin initializes (which happens before the front-end
///     application has `started`), it instructs the layout restorer whether
///     the restorer ought to `restore` its widgets by passing in its widget
///     tracker.
///     Alternatively, a plugin that does not require its own widget tracker
///     (because perhaps it only creates a single widget, like a command palette),
///     can simply `add` its widget along with a persistent unique name to the
///     layout restorer so that its layout state can be restored when the lab
///     application restores.
/// 
/// 4. After all the load-time plugins have finished initializing, the front-end
///     application `started` promise will resolve. This is the `first`
///     promise that the layout restorer waits for. By this point, all of the
///     plugins that care about restoration will have instructed the layout
///     restorer to `restore` their widget trackers.
/// 
/// 5. The layout restorer will then instruct each plugin's widget tracker
///     to restore its state and reinstantiate whichever widgets it wants. The
///     tracker returns a promise to the layout restorer that resolves when it
///     has completed restoring the tracked widgets it cares about.
/// 
/// 6. As each widget tracker finishes restoring the widget instances it cares
///     about, it resolves the promise that was returned to the layout restorer
///     (in step 5). After all of the promises that the restorer is awaiting have
///     settled, the restorer then resolves the outstanding `fetch` promise
///     (from step 1) and hands off a layout state object to the application
///     shell's `restoreLayout` method for restoration.
/// 
/// 7. Once the application shell has finished restoring the layout, the
///     JupyterLab application's `restored` promise is resolved.
/// 
/// Of particular note are steps 5 and 6: since data restoration of plugins
/// is accomplished by executing commands, the command that is used to restore
/// the data of each plugin must return a promise that only resolves when the
/// widget has been created and added to the plugin's widget tracker.
/// A namespace for `LayoutRestorer` statics.
type [<AllowNullLiteral>] LayoutRestorerStatic =
    /// Create a layout restorer.
    [<Emit "new $0($1...)">] abstract Create: options: LayoutRestorer.IOptions -> LayoutRestorer

module LayoutRestorer =

    /// The configuration options for layout restorer instantiation.
    type [<AllowNullLiteral>] IOptions =
        /// The data connector used for layout saving and fetching.
        abstract connector: IDataConnector<ReadonlyJSONValue> with get, set
        /// The initial promise that has to be resolved before restoration.
        /// 
        /// #### Notes
        /// This promise should equal the JupyterLab application `started` notifier.
        abstract first: Promise<obj option> with get, set
        /// The application command registry.
        abstract registry: CommandRegistry with get, set
type IWidgetTracker = JupyterlabApputils.IWidgetTracker
// type WidgetTracker = ``@jupyterlab_apputils``.WidgetTracker
type MimeDocument = JupyterlabDocregistry.Mimedocument.MimeDocument
// type IRenderMime = ``@jupyterlab_rendermime_interfaces``.IRenderMime
// type Token = ``@phosphor_coreutils``.Token
// type JupyterFrontEndPlugin = __index.JupyterFrontEndPlugin
let [<Import("*","@jupyterlab/application/lib/mimerenderers")>] IMimeDocumentTracker: Token<IMimeDocumentTracker> = jsNative

// type [<AllowNullLiteral>] IExports =
//     /// Create rendermime plugins for rendermime extension modules.
//     abstract createRendermimePlugins: extensions: ResizeArray<IRenderMime.IExtensionModule> -> ResizeArray<JupyterFrontEndPlugin<U2<unit, IMimeDocumentTracker>>>
//     /// Create rendermime plugins for rendermime extension modules.
//     abstract createRendermimePlugin: tracker: WidgetTracker<MimeDocument> * item: IRenderMime.IExtension -> JupyterFrontEndPlugin<unit>

/// A class that tracks mime documents.
/// The mime document tracker token.
type [<AllowNullLiteral>] IMimeDocumentTracker =
    inherit JupyterlabApputils.IWidgetTracker<MimeDocument>
// type CommandRegistry = ``@phosphor_commands``.CommandRegistry
// type Token = ``@phosphor_coreutils``.Token
// type IDisposable = ``@phosphor_disposable``.IDisposable
// type ISignal = ``@phosphor_signaling``.ISignal
// type IRouter = __tokens.IRouter

// type [<AllowNullLiteral>] IExports =
//     abstract Router: RouterStatic

/// A static class that routes URLs within the application.
/// A namespace for `Router` class statics.
type [<AllowNullLiteral>] Router =
    inherit IRouter
    /// The base URL for the router.
    abstract ``base``: string
    /// The command registry used by the router.
    abstract commands: CommandRegistry
    /// Returns the parsed current URL of the application.
    abstract current: IRouter.ILocation
    /// A signal emitted when the router routes a route.
    abstract routed: ISignal<Router, IRouter.ILocation>
    /// If a matching rule's command resolves with the `stop` token during routing,
    /// no further matches will execute.
    abstract stop: Token<unit>
    /// <summary>Navigate to a new path within the application.</summary>
    /// <param name="path">- The new path or empty string if redirecting to root.</param>
    /// <param name="options">- The navigation options.</param>
    abstract navigate: path: string * ?options: IRouter.INavOptions -> unit
    /// <summary>Register to route a path pattern to a command.</summary>
    /// <param name="options">- The route registration options.</param>
    abstract register: options: IRouter.IRegisterOptions -> IDisposable
    /// Cause a hard reload of the document.
    abstract reload: unit -> unit
    /// Route a specific path to an action.
    /// 
    /// #### Notes
    /// If a pattern is matched, its command will be invoked with arguments that
    /// match the `IRouter.ILocation` interface.
    abstract route: unit -> Promise<unit>
    abstract _routed: obj with get, set
    abstract _rules: obj with get, set

/// A static class that routes URLs within the application.
/// A namespace for `Router` class statics.
type [<AllowNullLiteral>] RouterStatic =
    /// Create a URL router.
    [<Emit "new $0($1...)">] abstract Create: options: Router.IOptions -> Router

module Router =

    /// The options for instantiating a JupyterLab URL router.
    type [<AllowNullLiteral>] IOptions =
        /// The fully qualified base URL for the router.
        abstract ``base``: string with get, set
        /// The command registry used by the router.
        abstract commands: CommandRegistry with get, set
