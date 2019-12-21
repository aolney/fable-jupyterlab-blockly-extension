// ts2fable 0.0.0
module rec PhosphorApplication
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types 

type CommandRegistry = PhosphorCommands.CommandRegistry // @phosphor_commands.CommandRegistry
type Token<'T> = PhosphorCoreutils.Token<'T> // @phosphor_coreutils.Token
type ContextMenu = PhosphorWidgets.ContextMenu //@phosphor_widgets.ContextMenu
type Menu = PhosphorWidgets.Menu //@phosphor_widgets.Menu
type Widget = PhosphorWidgets.Widget //@phosphor_widgets.Widget

type [<AllowNullLiteral>] IExports =
    abstract Application: ApplicationStatic

/// A user-defined application plugin.
/// 
/// #### Notes
/// Plugins are the foundation for building an extensible application.
/// 
/// Plugins consume and provide "services", which are nothing more than
/// concrete implementations of interfaces and/or abstract types.
/// 
/// Unlike regular imports and exports, which tie the service consumer
/// to a particular implementation of the service, plugins decouple the
/// service producer from the service consumer, allowing an application
/// to be easily customized by third parties in a type-safe fashion.
type [<AllowNullLiteral>] IPlugin<'T, 'U> =
    /// The human readable id of the plugin.
    /// 
    /// #### Notes
    /// This must be unique within an application.
    abstract id: string with get, set
    /// Whether the plugin should be activated on application start.
    /// 
    /// #### Notes
    /// The default is `false`.
    abstract autoStart: bool option with get, set
    /// The types of required services for the plugin, if any.
    /// 
    /// #### Notes
    /// These tokens correspond to the services that are required by
    /// the plugin for correct operation.
    /// 
    /// When the plugin is activated, a concrete instance of each type
    /// will be passed to the `activate()` function, in the order they
    /// are specified in the `requires` array.
    abstract requires: ResizeArray<Token<obj option>> option with get, set
    /// The types of optional services for the plugin, if any.
    /// 
    /// #### Notes
    /// These tokens correspond to the services that can be used by the
    /// plugin if available, but are not necessarily required.
    /// 
    /// The optional services will be passed to the `activate()` function
    /// following all required services. If an optional service cannot be
    /// resolved, `null` will be passed in its place.
    abstract optional: ResizeArray<Token<obj option>> option with get, set
    /// The type of service provided by the plugin, if any.
    /// 
    /// #### Notes
    /// This token corresponds to the service exported by the plugin.
    /// 
    /// When the plugin is activated, the return value of `activate()`
    /// is used as the concrete instance of the type.
    abstract provides: Token<'U> option with get, set
    /// A function invoked to activate the plugin.
    abstract activate: ('T -> ResizeArray<obj option> -> U2<'U, Promise<'U>>) with get, set

/// A class for creating pluggable applications.
/// 
/// #### Notes
/// The `Application` class is useful when creating large, complex
/// UI applications with the ability to be safely extended by third
/// party code via plugins.
/// The namespace for the `Application` class statics.
type [<AllowNullLiteral>] Application<'T> =
    /// The application command registry.
    abstract commands: CommandRegistry
    /// The application context menu.
    abstract contextMenu: ContextMenu
    /// The application shell widget.
    /// 
    /// #### Notes
    /// The shell widget is the root "container" widget for the entire
    /// application. It will typically expose an API which allows the
    /// application plugins to insert content in a variety of places.
    abstract shell: 'T
    /// A promise which resolves after the application has started.
    /// 
    /// #### Notes
    /// This promise will resolve after the `start()` method is called,
    /// when all the bootstrapping and shell mounting work is complete.
    abstract started: Promise<unit>
    /// <summary>Test whether a plugin is registered with the application.</summary>
    /// <param name="id">- The id of the plugin of interest.</param>
    abstract hasPlugin: id: string -> bool
    /// List the IDs of the plugins registered with the application.
    abstract listPlugins: unit -> ResizeArray<string>
    /// <summary>Register a plugin with the application.</summary>
    /// <param name="plugin">- The plugin to register.
    /// 
    /// #### Notes
    /// An error will be thrown if a plugin with the same id is already
    /// registered, or if the plugin has a circular dependency.
    /// 
    /// If the plugin provides a service which has already been provided
    /// by another plugin, the new service will override the old service.</param>
    abstract registerPlugin: plugin: IPlugin<Application<'T>, obj option> -> unit
    /// <summary>Register multiple plugins with the application.</summary>
    /// <param name="plugins">- The plugins to register.
    /// 
    /// #### Notes
    /// This calls `registerPlugin()` for each of the given plugins.</param>
    abstract registerPlugins: plugins: ResizeArray<IPlugin<Application<'T>, obj option>> -> unit
    /// <summary>Activate the plugin with the given id.</summary>
    /// <param name="id">- The ID of the plugin of interest.</param>
    abstract activatePlugin: id: string -> Promise<unit>
    /// <summary>Resolve a required service of a given type.</summary>
    /// <param name="token">- The token for the service type of interest.</param>
    abstract resolveRequiredService: token: Token<'U> -> Promise<'U>
    /// <summary>Resolve an optional service of a given type.</summary>
    /// <param name="token">- The token for the service type of interest.</param>
    abstract resolveOptionalService: token: Token<'U> -> Promise<'U option>
    /// <summary>Start the application.</summary>
    /// <param name="options">- The options for starting the application.</param>
    abstract start: ?options: Application.IStartOptions -> Promise<unit>
    /// <summary>Handle the DOM events for the application.</summary>
    /// <param name="event">- The DOM event sent to the application.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events registered for the application. It
    /// should not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// <summary>Attach the application shell to the DOM.</summary>
    /// <param name="id">- The id of the host node for the shell, or `''`.
    /// 
    /// #### Notes
    /// If the id is not provided, the document body will be the host.
    /// 
    /// A subclass may reimplement this method as needed.</param>
    abstract attachShell: id: string -> unit
    /// Add the application event listeners.
    /// 
    /// #### Notes
    /// The default implementation of this method adds listeners for
    /// `'keydown'` and `'resize'` events.
    /// 
    /// A subclass may reimplement this method as needed.
    abstract addEventListeners: unit -> unit
    /// A method invoked on a document `'keydown'` event.
    /// 
    /// #### Notes
    /// The default implementation of this method invokes the key down
    /// processing method of the application command registry.
    /// 
    /// A subclass may reimplement this method as needed.
    abstract evtKeydown: ``event``: KeyboardEvent -> unit
    /// A method invoked on a document `'contextmenu'` event.
    /// 
    /// #### Notes
    /// The default implementation of this method opens the application
    /// `contextMenu` at the current mouse position.
    /// 
    /// If the application context menu has no matching content *or* if
    /// the shift key is pressed, the default browser context menu will
    /// be opened instead.
    /// 
    /// A subclass may reimplement this method as needed.
    abstract evtContextMenu: ``event``: MouseEvent -> unit
    /// A method invoked on a window `'resize'` event.
    /// 
    /// #### Notes
    /// The default implementation of this method updates the shell.
    /// 
    /// A subclass may reimplement this method as needed.
    abstract evtResize: ``event``: Event -> unit

/// A class for creating pluggable applications.
/// 
/// #### Notes
/// The `Application` class is useful when creating large, complex
/// UI applications with the ability to be safely extended by third
/// party code via plugins.
/// The namespace for the `Application` class statics.
type [<AllowNullLiteral>] ApplicationStatic =
    /// <summary>Construct a new application.</summary>
    /// <param name="options">- The options for creating the application.</param>
    [<Emit "new $0($1...)">] abstract Create: options: Application.IOptions<'T> -> Application<'T>

module Application =

    /// An options object for creating an application.
    type [<AllowNullLiteral>] IOptions<'T> =
        /// The shell widget to use for the application.
        /// 
        /// This should be a newly created and initialized widget.
        /// 
        /// The application will attach the widget to the DOM.
        abstract shell: 'T with get, set
        /// A custom renderer for the context menu.
        abstract contextMenuRenderer: PhosphorWidgets.Menu.IRenderer option with get, set

    /// An options object for application startup.
    type [<AllowNullLiteral>] IStartOptions =
        /// The ID of the DOM node to host the application shell.
        /// 
        /// #### Notes
        /// If this is not provided, the document body will be the host.
        abstract hostID: string option with get, set
        /// The plugins to activate on startup.
        /// 
        /// #### Notes
        /// These will be *in addition* to any `autoStart` plugins.
        abstract startPlugins: ResizeArray<string> option with get, set
        /// The plugins to **not** activate on startup.
        /// 
        /// #### Notes
        /// This will override `startPlugins` and any `autoStart` plugins.
        abstract ignorePlugins: ResizeArray<string> option with get, set
