// ts2fable 0.0.0
module rec JupyterlabServices
open System
open Fable.Core
open Fable.Core.JS
open Fetch.Types
// open Node
// open Browser.Types

//amo typescript 
type Error = Node.Base.Error 
type TypeError = obj 
type PromiseLike<'T> = Promise<'T>
//amo: end typescript

module Manager =
    //amo: some of these aliases are for modules. just giving full path to avoid issues
    type Poll = JupyterlabCoreutils.Poll.Poll //__@jupyterlab_coreutils.Poll
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    // type Builder = __builder_index.Builder //  Builder.Builder
    type BuildManager = __builder_index.BuildManager // Builder.BuildManager
    // type NbConvert = __nbconvert_index.NbConvert //Nbconvert.NbConvert
    type NbConvertManager = __nbconvert_index.NbConvertManager //Nbconvert.NbConvertManager
    // type Contents = __contents_index.Contents // Contents.Contents
    type ContentsManager = __contents_index.ContentsManager // Contents.ContentsManager
    // type Kernel = __kernel_kernel.Kernel // __kernel_kernel.Kernel.Kernel
    // type Session = __session_session.Session //Session.Session
    // type SessionManager = __session_session.SessionManager //Session.SessionManager
    // type Setting = __setting_index.Setting //Setting.Setting
    type SettingManager = __setting_index.SettingManager //Setting.SettingManager
    // type TerminalSession = __terminal_terminal.TerminalSession // Terminal.TerminalSession
    // type TerminalManager = __terminal_terminal.TerminalManager // Terminal.TerminalManager
    // type ServerConnection = Serverconnection.ServerConnection
    // type Workspace = __workspace_index.Workspace // Workspace.Workspace
    type WorkspaceManager = __workspace_index.WorkspaceManager // Workspace.WorkspaceManager

    type [<AllowNullLiteral>] IExports =
        abstract ServiceManager: ServiceManagerStatic

    /// A Jupyter services manager.
    /// The namespace for `ServiceManager` statics.
    type [<AllowNullLiteral>] ServiceManager =
        inherit ServiceManager.IManager
        /// A signal emitted when the kernel specs change.
        abstract specsChanged: ISignal<ServiceManager, __kernel_kernel.Kernel.ISpecModels>
        /// A signal emitted when there is a connection failure with the kernel.
        abstract connectionFailure: ISignal<ServiceManager, Error>
        /// Test whether the service manager is disposed.
        abstract isDisposed: bool
        /// Dispose of the resources used by the manager.
        abstract dispose: unit -> unit
        /// The kernel spec models.
        abstract specs: __kernel_kernel.Kernel.ISpecModels option
        /// The server settings of the manager.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Get the session manager instance.
        abstract sessions: __session_manager.SessionManager
        /// Get the setting manager instance.
        abstract settings: SettingManager
        /// The builder for the manager.
        abstract builder: BuildManager
        /// Get the contents manager instance.
        abstract contents: ContentsManager
        /// Get the terminal manager instance.
        abstract terminals: __terminal_manager.TerminalManager
        /// Get the workspace manager instance.
        abstract workspaces: WorkspaceManager
        /// Get the nbconvert manager instance.
        abstract nbconvert: NbConvertManager
        /// Test whether the manager is ready.
        abstract isReady: bool
        /// A promise that fulfills when the manager is ready.
        abstract ready: Promise<unit>

    /// A Jupyter services manager.
    /// The namespace for `ServiceManager` statics.
    type [<AllowNullLiteral>] ServiceManagerStatic =
        /// Construct a new services provider.
        [<Emit "new $0($1...)">] abstract Create: ?options: ServiceManager.IOptions -> ServiceManager

    module ServiceManager =

        /// A service manager interface.
        type [<AllowNullLiteral>] IManager =
            inherit IDisposable
            /// The builder for the manager.
            abstract builder: __builder_index.Builder.IManager
            /// The contents manager for the manager.
            abstract contents: __contents_index.Contents.IManager
            /// Test whether the manager is ready.
            abstract isReady: bool
            /// A promise that fulfills when the manager is initially ready.
            abstract ready: Promise<unit>
            /// The server settings of the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// The session manager for the manager.
            abstract sessions: __session_session.Session.IManager
            /// The setting manager for the manager.
            abstract settings: __setting_index.Setting.IManager
            /// The kernel spec models.
            abstract specs: __kernel_kernel.Kernel.ISpecModels option
            /// A signal emitted when the kernel specs change.
            abstract specsChanged: ISignal<IManager, __kernel_kernel.Kernel.ISpecModels>
            /// The terminals manager for the manager.
            abstract terminals: __terminal_terminal.TerminalSession.IManager
            /// The workspace manager for the manager.
            abstract workspaces: __workspace_index.Workspace.IManager
            /// The nbconvert manager for the manager.
            abstract nbconvert: __nbconvert_index.NbConvert.IManager
            /// A signal emitted when there is a connection failure with the server.
            abstract connectionFailure: ISignal<IManager, Error>

        /// The options used to create a service manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings of the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option
            /// The default drive for the contents manager.
            abstract defaultDrive: __contents_index.Contents.IDrive option
            /// When the manager stops polling the API. Defaults to `when-hidden`.
            abstract standby: JupyterlabCoreutils.Poll.Poll.Standby option with get, set

module Serverconnection =

    //amo typescript
    type RequestInit = obj
    type RequestInfo = obj

    module ServerConnection =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Create a settings object given a subset of options.</summary>
            /// <param name="options">- An optional partial set of options.</param>
            abstract makeSettings: ?options: obj -> ISettings
            /// <summary>Make an request to the notebook server.</summary>
            /// <param name="url">- The url for the request.</param>
            /// <param name="init">- The initialization options for the request.</param>
            /// <param name="settings">- The server settings to apply to the request.</param>
            abstract makeRequest: url: string * init: RequestInit * settings: ISettings -> Promise<Response>
            abstract ResponseError: ResponseErrorStatic
            abstract NetworkError: NetworkErrorStatic
            abstract defaultSettings: Serverconnection.ServerConnection.ISettings

        /// A Jupyter server settings object.
        /// Note that all of the settings are optional when passed to
        /// [[makeSettings]].  The default settings are given in [[defaultSettings]].
        type [<AllowNullLiteral>] ISettings =
            /// The base url of the server.
            abstract baseUrl: string
            /// The app url of the JupyterLab application.
            abstract appUrl: string
            /// The base ws url of the server.
            abstract wsUrl: string
            /// The default request init options.
            abstract init: RequestInit
            /// The authentication token for requests.  Use an empty string to disable.
            abstract token: string
            /// The `fetch` method to use.
            abstract fetch: (RequestInfo -> RequestInit -> Promise<Response>)
            /// The `Request` object constructor.
            abstract Request: obj
            /// The `Headers` object constructor.
            abstract Headers: obj
            /// The `WebSocket` object constructor.
            abstract WebSocket: obj

        /// A wrapped error for a fetch response.
        type [<AllowNullLiteral>] ResponseError =
            inherit Error
            /// The response associated with the error.
            abstract response: Response with get, set

        /// A wrapped error for a fetch response.
        type [<AllowNullLiteral>] ResponseErrorStatic =
            /// Create a new response error.
            [<Emit "new $0($1...)">] abstract Create: response: Response * ?message: string -> ResponseError

        /// A wrapped error for a network error.
        // type [<AllowNullLiteral>] NetworkError = 
        //     inherit TypeError //can't find in fable namespace?
        type NetworkError = obj

        /// A wrapped error for a network error.
        type [<AllowNullLiteral>] NetworkErrorStatic =
            /// Create a new network error.
            [<Emit "new $0($1...)">] abstract Create: original: TypeError -> NetworkError

module __builder_index =
    //type ServerConnection = Serverconnection.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract BuildManager: BuildManagerStatic

    /// The build API service manager.
    /// A namespace for `BuildManager` statics.
    type [<AllowNullLiteral>] BuildManager =
        /// The server settings used to make API requests.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the build service is available.
        abstract isAvailable: bool
        /// Test whether to check build status automatically.
        abstract shouldCheck: bool
        /// Get whether the application should be built.
        abstract getStatus: unit -> Promise<BuildManager.IStatus>
        /// Build the application.
        abstract build: unit -> Promise<unit>
        /// Cancel an active build.
        abstract cancel: unit -> Promise<unit>

    /// The build API service manager.
    /// A namespace for `BuildManager` statics.
    type [<AllowNullLiteral>] BuildManagerStatic =
        /// Create a new setting manager.
        [<Emit "new $0($1...)">] abstract Create: ?options: BuildManager.IOptions -> BuildManager

    module BuildManager =

        /// The instantiation options for a setting manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings used to make API requests.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

        /// The build status response from the server.
        type [<AllowNullLiteral>] IStatus =
            /// Whether a build is needed.
            abstract status: U3<string, string, string>
            /// The message associated with the build status.
            abstract message: string

    module Builder =

        /// The interface for the build manager.
        type [<AllowNullLiteral>] IManager =
            inherit BuildManager

module __config_index =
    type JSONObject = PhosphorCoreutils.JSONObject // __config_@phosphor_coreutils.JSONObject
    type JSONValue = PhosphorCoreutils.JSONValue // __config_@phosphor_coreutils.JSONValue
    // type ServerConnection = __.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract ConfigWithDefaults: ConfigWithDefaultsStatic

    /// A Configurable data section.
    type [<AllowNullLiteral>] IConfigSection =
        /// The data for this section.
        abstract data: JSONObject
        /// Modify the stored config values.
        /// 
        /// #### Notes
        /// Updates the local data immediately, sends the change to the server,
        /// and updates the local data with the response, and fulfils the promise
        /// with that data.
        abstract update: newdata: JSONObject -> Promise<JSONObject>
        /// The server settings for the section.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings

    module ConfigSection =

        type [<AllowNullLiteral>] IExports =
            /// Create a config section.
            abstract create: options: ConfigSection.IOptions -> Promise<IConfigSection>

        /// The options used to create a config section.
        type [<AllowNullLiteral>] IOptions =
            /// The section name.
            abstract name: string with get, set
            /// The optional server settings.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

    /// Configurable object with defaults.
    /// A namespace for ConfigWithDefaults statics.
    type [<AllowNullLiteral>] ConfigWithDefaults =
        /// Get data from the config section or fall back to defaults.
        abstract get: key: string -> JSONValue
        /// Set a config value.
        /// 
        /// #### Notes
        /// Uses the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/config).
        /// 
        /// The promise is fulfilled on a valid response and rejected otherwise.
        /// 
        /// Sends the update to the server, and changes our local copy of the data
        /// immediately.
        abstract set: key: string * value: JSONValue -> Promise<JSONValue>

    /// Configurable object with defaults.
    /// A namespace for ConfigWithDefaults statics.
    type [<AllowNullLiteral>] ConfigWithDefaultsStatic =
        /// Create a new config with defaults.
        [<Emit "new $0($1...)">] abstract Create: options: ConfigWithDefaults.IOptions -> ConfigWithDefaults

    module ConfigWithDefaults =

        /// The options used to initialize a ConfigWithDefaults object.
        type [<AllowNullLiteral>] IOptions =
            /// The configuration section.
            abstract section: IConfigSection with get, set
            /// The default values.
            abstract defaults: JSONObject option with get, set
            /// The optional classname namespace.
            abstract className: string option with get, set

module __contents_index =
    // type ModelDB = JupyterlabObservables.Modeldb.ModelDB // __contents_@jupyterlab_observables.ModelDB
    type IDisposable = PhosphorDisposable.IDisposable // __contents_@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __contents_@phosphor_signaling.ISignal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract ContentsManager: ContentsManagerStatic
        abstract Drive: DriveStatic

    module Contents =

        type [<AllowNullLiteral>] IExports =
            /// Validates an IModel, thowing an error if it does not pass.
            abstract validateContentsModel: contents: IModel -> unit
            /// Validates an ICheckpointModel, thowing an error if it does not pass.
            abstract validateCheckpointModel: checkpoint: ICheckpointModel -> unit

        /// A contents model.
        type [<AllowNullLiteral>] IModel =
            /// Name of the contents file.
            /// 
            /// #### Notes
            ///   Equivalent to the last part of the `path` field.
            abstract name: string
            /// The full file path.
            /// 
            /// #### Notes
            /// It will *not* start with `/`, and it will be `/`-delimited.
            abstract path: string
            /// The type of file.
            abstract ``type``: ContentType
            /// Whether the requester has permission to edit the file.
            abstract writable: bool
            /// File creation timestamp.
            abstract created: string
            /// Last modified timestamp.
            abstract last_modified: string
            /// Specify the mime-type of file contents.
            /// 
            /// #### Notes
            /// Only non-`null` when `content` is present and `type` is `"file"`.
            abstract mimetype: string
            /// The optional file content.
            abstract content: obj option
            /// The chunk of the file upload.
            abstract chunk: float option
            /// The format of the file `content`.
            /// 
            /// #### Notes
            /// Only relevant for type: 'file'
            abstract format: FileFormat

        type [<StringEnum>] [<RequireQualifiedAccess>] ContentType =
            | Notebook
            | File
            | Directory

        type [<StringEnum>] [<RequireQualifiedAccess>] FileFormat =
            | Json
            | Text
            | Base64

        /// The options used to fetch a file.
        type [<AllowNullLiteral>] IFetchOptions =
            /// The override file type for the request.
            abstract ``type``: ContentType option with get, set
            /// The override file format for the request.
            abstract format: FileFormat option with get, set
            /// Whether to include the file content.
            /// 
            /// The default is `true`.
            abstract content: bool option with get, set

        /// The options used to create a file.
        type [<AllowNullLiteral>] ICreateOptions =
            /// The directory in which to create the file.
            abstract path: string option with get, set
            /// The optional file extension for the new file (e.g. `".txt"`).
            /// 
            /// #### Notes
            /// This ignored if `type` is `'notebook'`.
            abstract ext: string option with get, set
            /// The file type.
            abstract ``type``: ContentType option with get, set

        /// Checkpoint model.
        type [<AllowNullLiteral>] ICheckpointModel =
            /// The unique identifier for the checkpoint.
            abstract id: string
            /// Last modified timestamp.
            abstract last_modified: string

        /// The change args for a file change.
        type [<AllowNullLiteral>] IChangedArgs =
            /// The type of change.
            abstract ``type``: U4<string, string, string, string> with get, set
            /// The new contents.
            abstract oldValue: obj option with get, set
            /// The old contents.
            abstract newValue: obj option with get, set

        /// The interface for a contents manager.
        type [<AllowNullLiteral>] IManager =
            inherit IDisposable
            /// A signal emitted when a file operation takes place.
            abstract fileChanged: ISignal<IManager, IChangedArgs>
            /// The server settings associated with the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// Add an `IDrive` to the manager.
            abstract addDrive: drive: IDrive -> unit
            /// <summary>Given a path of the form `drive:local/portion/of/it.txt`
            /// get the local part of it.</summary>
            /// <param name="path">: the path.</param>
            abstract localPath: path: string -> string
            /// <summary>Normalize a global path. Reduces '..' and '.' parts, and removes
            /// leading slashes from the local part of the path, while retaining
            /// the drive name if it exists.</summary>
            /// <param name="path">: the path.</param>
            abstract normalize: path: string -> string
            /// <summary>Given a path of the form `drive:local/portion/of/it.txt`
            /// get the name of the drive. If the path is missing
            /// a drive portion, returns an empty string.</summary>
            /// <param name="path">: the path.</param>
            abstract driveName: path: string -> string
            /// Given a path, get a ModelDB.IFactory from the
            /// relevant backend. Returns `null` if the backend
            /// does not provide one.
            abstract getModelDBFactory: path: string -> JupyterlabObservables.Modeldb.ModelDB.IFactory option
            /// <summary>Get a file or directory.</summary>
            /// <param name="path">: The path to the file.</param>
            /// <param name="options">: The options used to fetch the file.</param>
            abstract get: path: string * ?options: IFetchOptions -> Promise<IModel>
            /// Get an encoded download url given a file path.
            abstract getDownloadUrl: path: string -> Promise<string>
            /// <summary>Create a new untitled file or directory in the specified directory path.</summary>
            /// <param name="options">: The options used to create the file.</param>
            abstract newUntitled: ?options: ICreateOptions -> Promise<IModel>
            /// <summary>Delete a file.</summary>
            /// <param name="path">- The path to the file.</param>
            abstract delete: path: string -> Promise<unit>
            /// <summary>Rename a file or directory.</summary>
            /// <param name="path">- The original file path.</param>
            /// <param name="newPath">- The new file path.</param>
            abstract rename: path: string * newPath: string -> Promise<IModel>
            /// <summary>Save a file.</summary>
            /// <param name="path">- The desired file path.</param>
            /// <param name="options">- Optional overrides to the model.</param>
            abstract save: path: string * ?options: obj -> Promise<IModel>
            /// <summary>Copy a file into a given directory.</summary>
            /// <param name="path">- The original file path.</param>
            /// <param name="toDir">- The destination directory path.</param>
            abstract copy: path: string * toDir: string -> Promise<IModel>
            /// <summary>Create a checkpoint for a file.</summary>
            /// <param name="path">- The path of the file.</param>
            abstract createCheckpoint: path: string -> Promise<ICheckpointModel>
            /// <summary>List available checkpoints for a file.</summary>
            /// <param name="path">- The path of the file.</param>
            abstract listCheckpoints: path: string -> Promise<ResizeArray<ICheckpointModel>>
            /// <summary>Restore a file to a known checkpoint state.</summary>
            /// <param name="path">- The path of the file.</param>
            /// <param name="checkpointID">- The id of the checkpoint to restore.</param>
            abstract restoreCheckpoint: path: string * checkpointID: string -> Promise<unit>
            /// <summary>Delete a checkpoint for a file.</summary>
            /// <param name="path">- The path of the file.</param>
            /// <param name="checkpointID">- The id of the checkpoint to delete.</param>
            abstract deleteCheckpoint: path: string * checkpointID: string -> Promise<unit>

        /// The interface for a network drive that can be mounted
        /// in the contents manager.
        type [<AllowNullLiteral>] IDrive =
            inherit IDisposable
            /// The name of the drive, which is used at the leading
            /// component of file paths.
            abstract name: string
            /// The server settings of the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// An optional ModelDB.IFactory instance for the
            /// drive.
            abstract modelDBFactory: JupyterlabObservables.Modeldb.ModelDB.IFactory option
            /// A signal emitted when a file operation takes place.
            abstract fileChanged: ISignal<IDrive, IChangedArgs> with get, set
            /// <summary>Get a file or directory.</summary>
            /// <param name="localPath">: The path to the file.</param>
            /// <param name="options">: The options used to fetch the file.</param>
            abstract get: localPath: string * ?options: IFetchOptions -> Promise<IModel>
            /// Get an encoded download url given a file path.
            abstract getDownloadUrl: localPath: string -> Promise<string>
            /// <summary>Create a new untitled file or directory in the specified directory path.</summary>
            /// <param name="options">: The options used to create the file.</param>
            abstract newUntitled: ?options: ICreateOptions -> Promise<IModel>
            /// <summary>Delete a file.</summary>
            /// <param name="localPath">- The path to the file.</param>
            abstract delete: localPath: string -> Promise<unit>
            /// <summary>Rename a file or directory.</summary>
            /// <param name="oldLocalPath">- The original file path.</param>
            /// <param name="newLocalPath">- The new file path.</param>
            abstract rename: oldLocalPath: string * newLocalPath: string -> Promise<IModel>
            /// <summary>Save a file.</summary>
            /// <param name="localPath">- The desired file path.</param>
            /// <param name="options">- Optional overrides to the model.</param>
            abstract save: localPath: string * ?options: obj -> Promise<IModel>
            /// <summary>Copy a file into a given directory.</summary>
            /// <param name="localPath">- The original file path.</param>
            /// <param name="toLocalDir">- The destination directory path.</param>
            abstract copy: localPath: string * toLocalDir: string -> Promise<IModel>
            /// <summary>Create a checkpoint for a file.</summary>
            /// <param name="localPath">- The path of the file.</param>
            abstract createCheckpoint: localPath: string -> Promise<ICheckpointModel>
            /// <summary>List available checkpoints for a file.</summary>
            /// <param name="localPath">- The path of the file.</param>
            abstract listCheckpoints: localPath: string -> Promise<ResizeArray<ICheckpointModel>>
            /// <summary>Restore a file to a known checkpoint state.</summary>
            /// <param name="localPath">- The path of the file.</param>
            /// <param name="checkpointID">- The id of the checkpoint to restore.</param>
            abstract restoreCheckpoint: localPath: string * checkpointID: string -> Promise<unit>
            /// <summary>Delete a checkpoint for a file.</summary>
            /// <param name="localPath">- The path of the file.</param>
            /// <param name="checkpointID">- The id of the checkpoint to delete.</param>
            abstract deleteCheckpoint: localPath: string * checkpointID: string -> Promise<unit>

    /// A contents manager that passes file operations to the server.
    /// Multiple servers implementing the `IDrive` interface can be
    /// attached to the contents manager, so that the same session can
    /// perform file operations on multiple backends.
    /// 
    /// This includes checkpointing with the normal file operations.
    /// A namespace for ContentsManager statics.
    type [<AllowNullLiteral>] ContentsManager =
        inherit Contents.IManager
        /// The server settings associated with the manager.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// A signal emitted when a file operation takes place.
        abstract fileChanged: ISignal<ContentsManager, Contents.IChangedArgs>
        /// Test whether the manager has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the manager.
        abstract dispose: unit -> unit
        /// Add an `IDrive` to the manager.
        abstract addDrive: drive: Contents.IDrive -> unit
        /// Given a path, get a ModelDB.IFactory from the
        /// relevant backend. Returns `null` if the backend
        /// does not provide one.
        abstract getModelDBFactory: path: string -> JupyterlabObservables.Modeldb.ModelDB.IFactory option
        /// <summary>Given a path of the form `drive:local/portion/of/it.txt`
        /// get the local part of it.</summary>
        /// <param name="path">: the path.</param>
        abstract localPath: path: string -> string
        /// <summary>Normalize a global path. Reduces '..' and '.' parts, and removes
        /// leading slashes from the local part of the path, while retaining
        /// the drive name if it exists.</summary>
        /// <param name="path">: the path.</param>
        abstract normalize: path: string -> string
        /// <summary>Given a path of the form `drive:local/portion/of/it.txt`
        /// get the name of the drive. If the path is missing
        /// a drive portion, returns an empty string.</summary>
        /// <param name="path">: the path.</param>
        abstract driveName: path: string -> string
        /// <summary>Get a file or directory.</summary>
        /// <param name="path">: The path to the file.</param>
        /// <param name="options">: The options used to fetch the file.</param>
        abstract get: path: string * ?options: Contents.IFetchOptions -> Promise<Contents.IModel>
        /// <summary>Get an encoded download url given a file path.</summary>
        /// <param name="path">- An absolute POSIX file path on the server.
        /// 
        /// #### Notes
        /// It is expected that the path contains no relative paths.
        /// 
        /// The returned URL may include a query parameter.</param>
        abstract getDownloadUrl: path: string -> Promise<string>
        /// <summary>Create a new untitled file or directory in the specified directory path.</summary>
        /// <param name="options">: The options used to create the file.</param>
        abstract newUntitled: ?options: Contents.ICreateOptions -> Promise<Contents.IModel>
        /// <summary>Delete a file.</summary>
        /// <param name="path">- The path to the file.</param>
        abstract delete: path: string -> Promise<unit>
        /// <summary>Rename a file or directory.</summary>
        /// <param name="path">- The original file path.</param>
        /// <param name="newPath">- The new file path.</param>
        abstract rename: path: string * newPath: string -> Promise<Contents.IModel>
        /// <summary>Save a file.</summary>
        /// <param name="path">- The desired file path.</param>
        /// <param name="options">- Optional overrides to the model.</param>
        abstract save: path: string * ?options: obj -> Promise<Contents.IModel>
        /// <summary>Copy a file into a given directory.</summary>
        /// <param name="toDir">- The destination directory path.</param>
        abstract copy: fromFile: string * toDir: string -> Promise<Contents.IModel>
        /// <summary>Create a checkpoint for a file.</summary>
        /// <param name="path">- The path of the file.</param>
        abstract createCheckpoint: path: string -> Promise<Contents.ICheckpointModel>
        /// <summary>List available checkpoints for a file.</summary>
        /// <param name="path">- The path of the file.</param>
        abstract listCheckpoints: path: string -> Promise<ResizeArray<Contents.ICheckpointModel>>
        /// <summary>Restore a file to a known checkpoint state.</summary>
        /// <param name="path">- The path of the file.</param>
        /// <param name="checkpointID">- The id of the checkpoint to restore.</param>
        abstract restoreCheckpoint: path: string * checkpointID: string -> Promise<unit>
        /// <summary>Delete a checkpoint for a file.</summary>
        /// <param name="path">- The path of the file.</param>
        /// <param name="checkpointID">- The id of the checkpoint to delete.</param>
        abstract deleteCheckpoint: path: string * checkpointID: string -> Promise<unit>

    /// A contents manager that passes file operations to the server.
    /// Multiple servers implementing the `IDrive` interface can be
    /// attached to the contents manager, so that the same session can
    /// perform file operations on multiple backends.
    /// 
    /// This includes checkpointing with the normal file operations.
    /// A namespace for ContentsManager statics.
    type [<AllowNullLiteral>] ContentsManagerStatic =
        /// <summary>Construct a new contents manager object.</summary>
        /// <param name="options">- The options used to initialize the object.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: ContentsManager.IOptions -> ContentsManager

    /// A default implementation for an `IDrive`, talking to the
    /// server using the Jupyter REST API.
    /// A namespace for Drive statics.
    type [<AllowNullLiteral>] Drive =
        inherit Contents.IDrive
        /// The name of the drive, which is used at the leading
        /// component of file paths.
        abstract name: string
        /// A signal emitted when a file operation takes place.
        abstract fileChanged: ISignal<Drive, Contents.IChangedArgs>
        /// The server settings of the drive.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the manager has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the manager.
        abstract dispose: unit -> unit
        /// <summary>Get a file or directory.</summary>
        /// <param name="localPath">: The path to the file.</param>
        /// <param name="options">: The options used to fetch the file.</param>
        abstract get: localPath: string * ?options: Contents.IFetchOptions -> Promise<Contents.IModel>
        /// <summary>Get an encoded download url given a file path.</summary>
        /// <param name="localPath">- An absolute POSIX file path on the server.
        /// 
        /// #### Notes
        /// It is expected that the path contains no relative paths.
        /// 
        /// The returned URL may include a query parameter.</param>
        abstract getDownloadUrl: localPath: string -> Promise<string>
        /// <summary>Create a new untitled file or directory in the specified directory path.</summary>
        /// <param name="options">: The options used to create the file.</param>
        abstract newUntitled: ?options: Contents.ICreateOptions -> Promise<Contents.IModel>
        /// <summary>Delete a file.</summary>
        /// <param name="localPath">- The path to the file.</param>
        abstract delete: localPath: string -> Promise<unit>
        /// <summary>Rename a file or directory.</summary>
        /// <param name="oldLocalPath">- The original file path.</param>
        /// <param name="newLocalPath">- The new file path.</param>
        abstract rename: oldLocalPath: string * newLocalPath: string -> Promise<Contents.IModel>
        /// <summary>Save a file.</summary>
        /// <param name="localPath">- The desired file path.</param>
        /// <param name="options">- Optional overrides to the model.</param>
        abstract save: localPath: string * ?options: obj -> Promise<Contents.IModel>
        /// <summary>Copy a file into a given directory.</summary>
        /// <param name="toDir">- The destination directory path.</param>
        abstract copy: fromFile: string * toDir: string -> Promise<Contents.IModel>
        /// <summary>Create a checkpoint for a file.</summary>
        /// <param name="localPath">- The path of the file.</param>
        abstract createCheckpoint: localPath: string -> Promise<Contents.ICheckpointModel>
        /// <summary>List available checkpoints for a file.</summary>
        /// <param name="localPath">- The path of the file.</param>
        abstract listCheckpoints: localPath: string -> Promise<ResizeArray<Contents.ICheckpointModel>>
        /// <summary>Restore a file to a known checkpoint state.</summary>
        /// <param name="localPath">- The path of the file.</param>
        /// <param name="checkpointID">- The id of the checkpoint to restore.</param>
        abstract restoreCheckpoint: localPath: string * checkpointID: string -> Promise<unit>
        /// <summary>Delete a checkpoint for a file.</summary>
        /// <param name="localPath">- The path of the file.</param>
        /// <param name="checkpointID">- The id of the checkpoint to delete.</param>
        abstract deleteCheckpoint: localPath: string * checkpointID: string -> Promise<unit>

    /// A default implementation for an `IDrive`, talking to the
    /// server using the Jupyter REST API.
    /// A namespace for Drive statics.
    type [<AllowNullLiteral>] DriveStatic =
        /// <summary>Construct a new contents manager object.</summary>
        /// <param name="options">- The options used to initialize the object.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: Drive.IOptions -> Drive

    module ContentsManager =

        /// The options used to initialize a contents manager.
        type [<AllowNullLiteral>] IOptions =
            /// The default drive backend for the contents manager.
            abstract defaultDrive: Contents.IDrive option with get, set
            /// The server settings associated with the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

    module Drive =

        /// The options used to initialize a `Drive`.
        type [<AllowNullLiteral>] IOptions =
            /// The name for the `Drive`, which is used in file
            /// paths to disambiguate it from other drives.
            abstract name: string option with get, set
            /// The server settings for the server.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// A REST endpoint for drive requests.
            /// If not given, defaults to the Jupyter
            /// REST API given by [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/contents).
            abstract apiEndpoint: string option with get, set

module __contents_validate =
    // type Contents = __contents_index.Contents

    type [<AllowNullLiteral>] IExports =
        /// Validate an `Contents.IModel` object.
        abstract validateContentsModel: model: __contents_index.Contents.IModel -> unit
        /// Validate an `Contents.ICheckpointModel` object.
        abstract validateCheckpointModel: model: __contents_index.Contents.ICheckpointModel -> unit

module __kernel_comm =
    type JSONObject = PhosphorCoreutils.JSONObject //__kernel_@phosphor_coreutils.JSONObject
    type DisposableDelegate = PhosphorDisposable.DisposableDelegate //__kernel_@phosphor_disposable.DisposableDelegate
    // type Kernel =  __kernel_kernel.Kernel
    // type KernelMessage = __kernel_messages.KernelMessage

    type [<AllowNullLiteral>] IExports =
        abstract CommHandler: CommHandlerStatic

    /// Comm channel handler.
    type [<AllowNullLiteral>] CommHandler =
        inherit DisposableDelegate
        inherit __kernel_kernel.Kernel.IComm
        /// The unique id for the comm channel.
        abstract commId: string
        /// The target name for the comm channel.
        abstract targetName: string
        /// Get the callback for a comm close event.
        /// 
        /// #### Notes
        /// This is called when the comm is closed from either the server or client.
        /// 
        /// **See also:** [[ICommClose]], [[close]]
        /// Set the callback for a comm close event.
        /// 
        /// #### Notes
        /// This is called when the comm is closed from either the server or client. If
        /// the function returns a promise, and the kernel was closed from the server,
        /// kernel message processing will pause until the returned promise is
        /// fulfilled.
        /// 
        /// **See also:** [[close]]
        abstract onClose: (__kernel_messages.KernelMessage.ICommCloseMsg -> U2<unit, PromiseLike<unit>>) with get, set
        /// Get the callback for a comm message received event.
        /// Set the callback for a comm message received event.
        /// 
        /// #### Notes
        /// This is called when a comm message is received. If the function returns a
        /// promise, kernel message processing will pause until it is fulfilled.
        abstract onMsg: (__kernel_messages.KernelMessage.ICommMsgMsg -> U2<unit, PromiseLike<unit>>) with get, set
        /// Open a comm with optional data and metadata.
        /// 
        /// #### Notes
        /// This sends a `comm_open` message to the server.
        /// 
        /// **See also:** [[ICommOpen]]
        abstract ``open``: ?data: JSONObject * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> -> __kernel_kernel.Kernel.IShellFuture
        /// Send a `comm_msg` message to the kernel.
        /// 
        /// #### Notes
        /// This is a no-op if the comm has been closed.
        /// 
        /// **See also:** [[ICommMsg]]
        abstract send: data: JSONObject * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IShellFuture
        /// Close the comm.
        /// 
        /// #### Notes
        /// This will send a `comm_close` message to the kernel, and call the
        /// `onClose` callback if set.
        /// 
        /// This is a no-op if the comm is already closed.
        /// 
        /// **See also:** [[ICommClose]], [[onClose]]
        abstract close: ?data: JSONObject * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> -> __kernel_kernel.Kernel.IShellFuture

    /// Comm channel handler.
    type [<AllowNullLiteral>] CommHandlerStatic =
        /// Construct a new comm channel.
        [<Emit "new $0($1...)">] abstract Create: target: string * id: string * kernel: __kernel_kernel.Kernel.IKernel * disposeCb: (unit -> unit) -> CommHandler

module __kernel_default =
    type JSONObject = PhosphorCoreutils.JSONObject //  __kernel_@phosphor_coreutils.JSONObject
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> //__kernel_@phosphor_signaling.ISignal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection
    // type Kernel = __kernel_kernel.Kernel
    // type KernelMessage = __kernel_messages.KernelMessage

    type [<AllowNullLiteral>] IExports =
        abstract DefaultKernel: DefaultKernelStatic

    /// Implementation of the Kernel object.
    /// 
    /// #### Notes
    /// Messages from the server are handled in the order they were received and
    /// asynchronously. Any message handler can return a promise, and message
    /// handling will pause until the promise is fulfilled.
    /// The namespace for `DefaultKernel` statics.
    type [<AllowNullLiteral>] DefaultKernel =
        inherit __kernel_kernel.Kernel.IKernel
        /// A signal emitted when the kernel is shut down.
        abstract terminated: ISignal<DefaultKernel, unit>
        /// The server settings for the kernel.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Handle comm messages
        /// 
        /// #### Notes
        /// The comm message protocol currently has implicit assumptions that only
        /// one kernel connection is handling comm messages. This option allows a
        /// kernel connection to opt out of handling comms.
        /// 
        /// See https://github.com/jupyter/jupyter_client/issues/263
        abstract handleComms: bool
        /// A signal emitted when the kernel status changes.
        abstract statusChanged: ISignal<DefaultKernel, __kernel_kernel.Kernel.Status>
        /// A signal emitted for iopub kernel messages.
        /// 
        /// #### Notes
        /// This signal is emitted after the iopub message is handled asynchronously.
        abstract iopubMessage: ISignal<DefaultKernel, __kernel_messages.KernelMessage.IIOPubMessage>
        /// A signal emitted for unhandled kernel message.
        /// 
        /// #### Notes
        /// This signal is emitted for a message that was not handled. It is emitted
        /// during the asynchronous message handling code.
        abstract unhandledMessage: ISignal<DefaultKernel, __kernel_messages.KernelMessage.IMessage>
        /// A signal emitted for any kernel message.
        /// 
        /// #### Notes
        /// This signal is emitted when a message is received, before it is handled
        /// asynchronously.
        /// 
        /// The behavior is undefined if the message is modified during message
        /// handling. As such, the message should be treated as read-only.
        abstract anyMessage: ISignal<DefaultKernel, __kernel_kernel.Kernel.IAnyMessageArgs>
        /// The id of the server-side kernel.
        abstract id: string
        /// The name of the server-side kernel.
        abstract name: string
        /// Get the model associated with the kernel.
        abstract model: __kernel_kernel.Kernel.IModel
        /// The client username.
        abstract username: string
        /// The client unique id.
        abstract clientId: string
        /// The current status of the kernel.
        abstract status: __kernel_kernel.Kernel.Status
        /// Test whether the kernel has been disposed.
        abstract isDisposed: bool
        /// The cached kernel info.
        /// 
        /// #### Notes
        /// This value will be null until the kernel is ready.
        abstract info: __kernel_messages.KernelMessage.IInfoReply option
        /// Test whether the kernel is ready.
        abstract isReady: bool
        /// A promise that is fulfilled when the kernel is ready.
        abstract ready: Promise<unit>
        /// Get the kernel spec.
        abstract getSpec: unit -> Promise<__kernel_kernel.Kernel.ISpecModel>
        /// Clone the current kernel with a new clientId.
        abstract clone: ?options: __kernel_kernel.Kernel.IOptions -> __kernel_kernel.Kernel.IKernel
        /// Dispose of the resources held by the kernel.
        abstract dispose: unit -> unit
        /// Send a shell message to the kernel.
        /// 
        /// #### Notes
        /// Send a message to the kernel's shell channel, yielding a future object
        /// for accepting replies.
        /// 
        /// If `expectReply` is given and `true`, the future is disposed when both a
        /// shell reply and an idle status message are received. If `expectReply`
        /// is not given or is `false`, the future is resolved when an idle status
        /// message is received.
        /// If `disposeOnDone` is not given or is `true`, the Future is disposed at this point.
        /// If `disposeOnDone` is given and `false`, it is up to the caller to dispose of the Future.
        /// 
        /// All replies are validated as valid kernel messages.
        /// 
        /// If the kernel status is `dead`, this will throw an error.
        abstract sendShellMessage: msg: __kernel_messages.KernelMessage.IShellMessage<'T> * ?expectReply: bool * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IShellFuture<__kernel_messages.KernelMessage.IShellMessage<'T>>
        /// Send a control message to the kernel.
        /// 
        /// #### Notes
        /// Send a message to the kernel's control channel, yielding a future object
        /// for accepting replies.
        /// 
        /// If `expectReply` is given and `true`, the future is disposed when both a
        /// control reply and an idle status message are received. If `expectReply`
        /// is not given or is `false`, the future is resolved when an idle status
        /// message is received.
        /// If `disposeOnDone` is not given or is `true`, the Future is disposed at this point.
        /// If `disposeOnDone` is given and `false`, it is up to the caller to dispose of the Future.
        /// 
        /// All replies are validated as valid kernel messages.
        /// 
        /// If the kernel status is `dead`, this will throw an error.
        abstract sendControlMessage: msg: __kernel_messages.KernelMessage.IControlMessage<'T> * ?expectReply: bool * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IControlFuture<__kernel_messages.KernelMessage.IControlMessage<'T>>
        /// Interrupt a kernel.
        /// 
        /// #### Notes
        /// Uses the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/kernels).
        /// 
        /// The promise is fulfilled on a valid response and rejected otherwise.
        /// 
        /// It is assumed that the API call does not mutate the kernel id or name.
        /// 
        /// The promise will be rejected if the kernel status is `Dead` or if the
        /// request fails or the response is invalid.
        abstract interrupt: unit -> Promise<unit>
        /// Restart a kernel.
        /// 
        /// #### Notes
        /// Uses the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/kernels) and validates the response model.
        /// 
        /// Any existing Future or Comm objects are cleared.
        /// 
        /// The promise is fulfilled on a valid response and rejected otherwise.
        /// 
        /// It is assumed that the API call does not mutate the kernel id or name.
        /// 
        /// The promise will be rejected if the request fails or the response is
        /// invalid.
        abstract restart: unit -> Promise<unit>
        /// Handle a restart on the kernel.  This is not part of the `IKernel`
        /// interface.
        abstract handleRestart: unit -> Promise<unit>
        /// Reconnect to a disconnected kernel.
        /// 
        /// #### Notes
        /// Used when the websocket connection to the kernel is lost.
        abstract reconnect: unit -> Promise<unit>
        /// Shutdown a kernel.
        /// 
        /// #### Notes
        /// Uses the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/kernels).
        /// 
        /// The promise is fulfilled on a valid response and rejected otherwise.
        /// 
        /// On a valid response, closes the websocket and disposes of the kernel
        /// object, and fulfills the promise.
        /// 
        /// If the kernel is already `dead`, it closes the websocket and returns
        /// without a server request.
        abstract shutdown: unit -> Promise<unit>
        /// Send a `kernel_info_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-info).
        /// 
        /// Fulfills with the `kernel_info_response` content when the shell reply is
        /// received and validated.
        abstract requestKernelInfo: unit -> Promise<__kernel_messages.KernelMessage.IInfoReplyMsg>
        /// Send a `complete_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#completion).
        /// 
        /// Fulfills with the `complete_reply` content when the shell reply is
        /// received and validated.
        abstract requestComplete: content: __kernel_messages.KernelMessage.ICompleteRequestMsg -> Promise<__kernel_messages.KernelMessage.ICompleteReplyMsg>
        /// Send an `inspect_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#introspection).
        /// 
        /// Fulfills with the `inspect_reply` content when the shell reply is
        /// received and validated.
        abstract requestInspect: content: __kernel_messages.KernelMessage.IInspectRequestMsg -> Promise<__kernel_messages.KernelMessage.IInspectReplyMsg>
        /// Send a `history_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// Fulfills with the `history_reply` content when the shell reply is
        /// received and validated.
        abstract requestHistory: content: __kernel_messages.KernelMessage.IHistoryRequestMsg -> Promise<__kernel_messages.KernelMessage.IHistoryReplyMsg>
        /// Send an `execute_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#execute).
        /// 
        /// Future `onReply` is called with the `execute_reply` content when the
        /// shell reply is received and validated. The future will resolve when
        /// this message is received and the `idle` iopub status is received.
        /// The future will also be disposed at this point unless `disposeOnDone`
        /// is specified and `false`, in which case it is up to the caller to dispose
        /// of the future.
        /// 
        /// **See also:** [[IExecuteReply]]
        abstract requestExecute: content: __kernel_messages.KernelMessage.IExecuteRequestMsg * ?disposeOnDone: bool * ?metadata: JSONObject -> __kernel_kernel.Kernel.IShellFuture<__kernel_messages.KernelMessage.IExecuteRequestMsg, __kernel_messages.KernelMessage.IExecuteReplyMsg>
        /// Send an experimental `debug_request` message.
        abstract requestDebug: content: __kernel_messages.KernelMessage.IDebugRequestMsg * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IControlFuture<__kernel_messages.KernelMessage.IDebugRequestMsg, __kernel_messages.KernelMessage.IDebugReplyMsg>
        /// Send an `is_complete_request` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#code-completeness).
        /// 
        /// Fulfills with the `is_complete_response` content when the shell reply is
        /// received and validated.
        abstract requestIsComplete: content: __kernel_messages.KernelMessage.IIsCompleteRequestMsg -> Promise<__kernel_messages.KernelMessage.IIsCompleteReplyMsg>
        /// Send a `comm_info_request` message.
        /// 
        /// #### Notes
        /// Fulfills with the `comm_info_reply` content when the shell reply is
        /// received and validated.
        abstract requestCommInfo: content: __kernel_messages.KernelMessage.ICommInfoRequestMsg -> Promise<__kernel_messages.KernelMessage.ICommInfoReplyMsg>
        /// Send an `input_reply` message.
        /// 
        /// #### Notes
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#messages-on-the-stdin-router-dealer-sockets).
        abstract sendInputReply: content: __kernel_messages.KernelMessage.IInputReplyMsg -> unit
        /// Connect to a comm, or create a new one.
        /// 
        /// #### Notes
        /// If a client-side comm already exists with the given commId, it is returned.
        /// An error is thrown if the kernel does not handle comms.
        abstract connectToComm: targetName: string * ?commId: string -> __kernel_kernel.Kernel.IComm
        /// <summary>Register a comm target handler.</summary>
        /// <param name="targetName">- The name of the comm target.</param>
        /// <param name="callback">- The callback invoked for a comm open message.</param>
        abstract registerCommTarget: targetName: string * callback: (__kernel_kernel.Kernel.IComm -> __kernel_messages.KernelMessage.ICommOpenMsg -> U2<unit, PromiseLike<unit>>) -> unit
        /// <summary>Remove a comm target handler.</summary>
        /// <param name="targetName">- The name of the comm target to remove.</param>
        /// <param name="callback">- The callback to remove.
        /// 
        /// #### Notes
        /// The comm target is only removed if the callback argument matches.</param>
        abstract removeCommTarget: targetName: string * callback: (__kernel_kernel.Kernel.IComm -> __kernel_messages.KernelMessage.ICommOpenMsg -> U2<unit, PromiseLike<unit>>) -> unit
        /// <summary>Register an IOPub message hook.</summary>
        /// <param name="hook">- The callback invoked for the message.
        /// 
        /// #### Notes
        /// The IOPub hook system allows you to preempt the handlers for IOPub
        /// messages that are responses to a given message id.
        /// 
        /// The most recently registered hook is run first. A hook can return a
        /// boolean or a promise to a boolean, in which case all kernel message
        /// processing pauses until the promise is fulfilled. If a hook return value
        /// resolves to false, any later hooks will not run and the function will
        /// return a promise resolving to false. If a hook throws an error, the error
        /// is logged to the console and the next hook is run. If a hook is
        /// registered during the hook processing, it will not run until the next
        /// message. If a hook is removed during the hook processing, it will be
        /// deactivated immediately.
        /// 
        /// See also [[IFuture.registerMessageHook]].</param>
        abstract registerMessageHook: msgId: string * hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
        /// <summary>Remove an IOPub message hook.</summary>
        /// <param name="hook">- The callback invoked for the message.</param>
        abstract removeMessageHook: msgId: string * hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit

    /// Implementation of the Kernel object.
    /// 
    /// #### Notes
    /// Messages from the server are handled in the order they were received and
    /// asynchronously. Any message handler can return a promise, and message
    /// handling will pause until the promise is fulfilled.
    /// The namespace for `DefaultKernel` statics.
    type [<AllowNullLiteral>] DefaultKernelStatic =
        /// Construct a kernel object.
        [<Emit "new $0($1...)">] abstract Create: options: __kernel_kernel.Kernel.IOptions * id: string -> DefaultKernel

    module DefaultKernel =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Find a kernel by id.</summary>
            /// <param name="id">- The id of the kernel of interest.</param>
            /// <param name="settings">- The optional server settings.</param>
            abstract findById: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<__kernel_kernel.Kernel.IModel>
            /// <summary>Fetch all of the kernel specs.</summary>
            /// <param name="settings">- The optional server settings.</param>
            abstract getSpecs: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<__kernel_kernel.Kernel.ISpecModels>
            /// <summary>Fetch the running kernels.</summary>
            /// <param name="settings">- The optional server settings.</param>
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<__kernel_kernel.Kernel.IModel>>
            /// <summary>Start a new kernel.</summary>
            /// <param name="options">- The options used to create the kernel.</param>
            abstract startNew: options: __kernel_kernel.Kernel.IOptions -> Promise<__kernel_kernel.Kernel.IKernel>
            /// <summary>Connect to a running kernel.</summary>
            /// <param name="model">- The model of the running kernel.</param>
            /// <param name="settings">- The server settings for the request.</param>
            abstract connectTo: model: __kernel_kernel.Kernel.IModel * ?settings: Serverconnection.ServerConnection.ISettings -> __kernel_kernel.Kernel.IKernel
            /// <summary>Shut down a kernel by id.</summary>
            /// <param name="id">- The id of the running kernel.</param>
            /// <param name="settings">- The server settings for the request.</param>
            abstract shutdown: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// <summary>Shut down all kernels.</summary>
            /// <param name="settings">- The server settings to use.</param>
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

module __kernel_future =
    type DisposableDelegate = PhosphorDisposable.DisposableDelegate // __kernel_@phosphor_disposable.DisposableDelegate
    // type Kernel = __kernel_kernel.Kernel
    // type KernelMessage = __kernel_messages.KernelMessage

    type [<AllowNullLiteral>] IExports =
        abstract KernelFutureHandler: KernelFutureHandlerStatic
        abstract KernelControlFutureHandler: KernelControlFutureHandlerStatic
        abstract KernelShellFutureHandler: KernelShellFutureHandlerStatic

    /// Implementation of a kernel future.
    /// 
    /// If a reply is expected, the Future is considered done when both a `reply`
    /// message and an `idle` iopub status message have been received.  Otherwise, it
    /// is considered done when the `idle` status is received.
    type [<AllowNullLiteral>] KernelFutureHandler<'REQUEST, 'REPLY> =
        inherit DisposableDelegate
        inherit __kernel_kernel.Kernel.IFuture<'REQUEST, 'REPLY>
        /// Get the original outgoing message.
        abstract msg: 'REQUEST
        /// A promise that resolves when the future is done.
        abstract ``done``: Promise<'REPLY>
        /// Get the reply handler.
        /// Set the reply handler.
        abstract onReply: ('REPLY -> U2<unit, PromiseLike<unit>>) with get, set
        /// Get the iopub handler.
        /// Set the iopub handler.
        abstract onIOPub: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<unit, PromiseLike<unit>>) with get, set
        /// Get the stdin handler.
        /// Set the stdin handler.
        abstract onStdin: (__kernel_messages.KernelMessage.IStdinMessage -> U2<unit, PromiseLike<unit>>) with get, set
        /// <summary>Register hook for IOPub messages.</summary>
        /// <param name="hook">- The callback invoked for an IOPub message.
        /// 
        /// #### Notes
        /// The IOPub hook system allows you to preempt the handlers for IOPub
        /// messages handled by the future.
        /// 
        /// The most recently registered hook is run first. A hook can return a
        /// boolean or a promise to a boolean, in which case all kernel message
        /// processing pauses until the promise is fulfilled. If a hook return value
        /// resolves to false, any later hooks will not run and the function will
        /// return a promise resolving to false. If a hook throws an error, the error
        /// is logged to the console and the next hook is run. If a hook is
        /// registered during the hook processing, it will not run until the next
        /// message. If a hook is removed during the hook processing, it will be
        /// deactivated immediately.</param>
        abstract registerMessageHook: hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
        /// <summary>Remove a hook for IOPub messages.</summary>
        /// <param name="hook">- The hook to remove.
        /// 
        /// #### Notes
        /// If a hook is removed during the hook processing, it will be deactivated immediately.</param>
        abstract removeMessageHook: hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
        /// Send an `input_reply` message.
        abstract sendInputReply: content: __kernel_messages.KernelMessage.IInputReplyMsg -> unit
        /// Dispose and unregister the future.
        abstract dispose: unit -> unit
        /// Handle an incoming kernel message.
        abstract handleMsg: msg: __kernel_messages.KernelMessage.IMessage -> Promise<unit>

    /// Implementation of a kernel future.
    /// 
    /// If a reply is expected, the Future is considered done when both a `reply`
    /// message and an `idle` iopub status message have been received.  Otherwise, it
    /// is considered done when the `idle` status is received.
    type [<AllowNullLiteral>] KernelFutureHandlerStatic =
        /// Construct a new KernelFutureHandler.
        [<Emit "new $0($1...)">] abstract Create: cb: (unit -> unit) * msg: 'REQUEST * expectReply: bool * disposeOnDone: bool * kernel: __kernel_kernel.Kernel.IKernel -> KernelFutureHandler<'REQUEST, 'REPLY>

    type KernelControlFutureHandler<'REPLY> =
        KernelControlFutureHandler<obj, 'REPLY>

    type KernelControlFutureHandler =
        KernelControlFutureHandler<obj, obj>

    type [<AllowNullLiteral>] KernelControlFutureHandler<'REQUEST, 'REPLY> =
        inherit KernelFutureHandler<'REQUEST, 'REPLY>
        inherit __kernel_kernel.Kernel.IControlFuture<'REQUEST, 'REPLY>

    type [<AllowNullLiteral>] KernelControlFutureHandlerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> KernelControlFutureHandler<'REQUEST, 'REPLY>

    type KernelShellFutureHandler<'REPLY> =
        KernelShellFutureHandler<obj, 'REPLY>

    type KernelShellFutureHandler =
        KernelShellFutureHandler<obj, obj>

    type [<AllowNullLiteral>] KernelShellFutureHandler<'REQUEST, 'REPLY> =
        inherit KernelFutureHandler<'REQUEST, 'REPLY>
        inherit __kernel_kernel.Kernel.IShellFuture<'REQUEST, 'REPLY>

    type [<AllowNullLiteral>] KernelShellFutureHandlerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> KernelShellFutureHandler<'REQUEST, 'REPLY>

module __kernel_kernel =
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __kernel_@phosphor_algorithm.IIterator
    type JSONObject = PhosphorCoreutils.JSONObject // __kernel_@phosphor_coreutils.JSONObject
    type JSONValue = PhosphorCoreutils.JSONValue //__kernel_@phosphor_coreutils.JSONValue
    type IDisposable = PhosphorDisposable.IDisposable //__kernel_@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __kernel_@phosphor_signaling.ISignal
    // type ServerConnection =  Serverconnection.ServerConnection //__.ServerConnection
    // type KernelMessage = __kernel_messages.KernelMessage

    module Kernel =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Find a kernel by id.</summary>
            /// <param name="id">- The id of the kernel of interest.</param>
            /// <param name="settings">- The optional server settings.</param>
            abstract findById: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<IModel>
            /// <summary>Fetch all of the kernel specs.</summary>
            /// <param name="settings">- The optional server settings.</param>
            abstract getSpecs: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<__kernel_kernel.Kernel.ISpecModels>
            /// <summary>Fetch the running kernels.</summary>
            /// <param name="settings">- The optional server settings.</param>
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<__kernel_kernel.Kernel.IModel>>
            /// <summary>Start a new kernel.</summary>
            /// <param name="options">- The options used to create the kernel.</param>
            abstract startNew: ?options: __kernel_kernel.Kernel.IOptions -> Promise<IKernel>
            /// <summary>Connect to a running kernel.</summary>
            /// <param name="model">- The model of the running kernel.</param>
            /// <param name="settings">- The server settings for the request.</param>
            abstract connectTo: model: __kernel_kernel.Kernel.IModel * ?settings: Serverconnection.ServerConnection.ISettings -> IKernel
            /// <summary>Shut down a kernel by id.</summary>
            /// <param name="id">- The id of the running kernel.</param>
            /// <param name="settings">- The server settings for the request.</param>
            abstract shutdown: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// Shut down all kernels.
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

        /// Interface of a Kernel connection that is managed by a session.
        /// 
        /// #### Notes
        /// The Kernel object is tied to the lifetime of the Kernel id, which is a
        /// unique id for the Kernel session on the server.  The Kernel object manages
        /// a websocket connection internally, and will auto-restart if the websocket
        /// temporarily loses connection.  Restarting creates a new Kernel process on
        /// the server, but preserves the Kernel id.
        type [<AllowNullLiteral>] IKernelConnection =
            inherit IDisposable
            /// The id of the server-side kernel.
            abstract id: string
            /// The name of the server-side kernel.
            abstract name: string
            /// The model associated with the kernel.
            abstract model: __kernel_kernel.Kernel.IModel
            /// The client username.
            abstract username: string
            /// The client unique id.
            /// 
            /// #### Notes
            /// This should be unique for a particular kernel connection object.
            abstract clientId: string
            /// The current status of the kernel.
            abstract status: __kernel_kernel.Kernel.Status
            /// The cached kernel info.
            /// 
            /// #### Notes
            /// This value will be null until the kernel is ready.
            abstract info: __kernel_messages.KernelMessage.IInfoReply option
            /// Test whether the kernel is ready.
            /// 
            /// #### Notes
            /// A kernel is ready when the communication channel is active and we have
            /// cached the kernel info.
            abstract isReady: bool
            /// A promise that resolves when the kernel is initially ready after a start
            /// or restart.
            /// 
            /// #### Notes
            /// A kernel is ready when the communication channel is active and we have
            /// cached the kernel info.
            abstract ready: Promise<unit>
            /// Whether the kernel connection handles comm messages.
            /// 
            /// #### Notes
            /// The comm message protocol currently has implicit assumptions that only
            /// one kernel connection is handling comm messages. This option allows a
            /// kernel connection to opt out of handling comms.
            /// 
            /// See https://github.com/jupyter/jupyter_client/issues/263
            abstract handleComms: bool with get, set
            /// Get the kernel spec.
            abstract getSpec: unit -> Promise<__kernel_kernel.Kernel.ISpecModel>
            /// <summary>Send a shell message to the kernel.</summary>
            /// <param name="msg">- The fully-formed shell message to send.</param>
            /// <param name="expectReply">- Whether to expect a shell reply message.</param>
            /// <param name="disposeOnDone">- Whether to dispose of the future when done.
            /// 
            /// #### Notes
            /// Send a message to the kernel's shell channel, yielding a future object
            /// for accepting replies.
            /// 
            /// If `expectReply` is given and `true`, the future is done when both a
            /// shell reply and an idle status message are received with the appropriate
            /// parent header, in which case the `.done` promise resolves to the reply.
            /// If `expectReply` is not given or is `false`, the future is done when an
            /// idle status message with the appropriate parent header is received, in
            /// which case the `.done` promise resolves to `undefined`.
            /// 
            /// If `disposeOnDone` is given and `false`, the future will not be disposed
            /// of when the future is done, instead relying on the caller to dispose of
            /// it. This allows for the handling of out-of-order output from ill-behaved
            /// kernels.
            /// 
            /// All replies are validated as valid kernel messages.
            /// 
            /// If the kernel status is `'dead'`, this will throw an error.</param>
            abstract sendShellMessage: msg: __kernel_messages.KernelMessage.IShellMessage<'T> * ?expectReply: bool * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IShellFuture<__kernel_messages.KernelMessage.IShellMessage<'T>>
            abstract sendControlMessage: msg: __kernel_messages.KernelMessage.IControlMessage<'T> * ?expectReply: bool * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IControlFuture<__kernel_messages.KernelMessage.IControlMessage<'T>>
            /// Reconnect to a disconnected kernel.
            abstract reconnect: unit -> Promise<unit>
            /// Interrupt a kernel.
            abstract interrupt: unit -> Promise<unit>
            /// Restart a kernel.
            abstract restart: unit -> Promise<unit>
            /// Send a `kernel_info_request` message.
            abstract requestKernelInfo: unit -> Promise<__kernel_messages.KernelMessage.IInfoReplyMsg>
            /// <summary>Send a `complete_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            abstract requestComplete: content: __kernel_messages.KernelMessage.TypeLiteral_14 -> Promise<__kernel_messages.KernelMessage.ICompleteReplyMsg>
            // THIS APPEARS WRONG, SEE ABOVE
            //abstract requestComplete: content: __kernel_messages.KernelMessage.ICompleteRequestMsg -> Promise<__kernel_messages.KernelMessage.ICompleteReplyMsg>
            
            /// <summary>Send an `inspect_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            abstract requestInspect: content: __kernel_messages.KernelMessage.TypeLiteral_15 -> Promise<__kernel_messages.KernelMessage.IInspectReplyMsg>
            // THIS APPEARS WRONG, SEE ABOVE
            // abstract requestInspect: content: __kernel_messages.KernelMessage.IInspectRequestMsg -> Promise<__kernel_messages.KernelMessage.IInspectReplyMsg>
            /// <summary>Send a `history_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            abstract requestHistory: content: __kernel_messages.KernelMessage.IHistoryRequestMsg -> Promise<__kernel_messages.KernelMessage.IHistoryReplyMsg>
            /// <summary>Send an `execute_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            /// <param name="disposeOnDone">- Whether to dispose of the future when done.</param>
            abstract requestExecute: content: __kernel_messages.KernelMessage.IExecuteRequestMsg * ?disposeOnDone: bool * ?metadata: JSONObject -> __kernel_kernel.Kernel.IShellFuture<__kernel_messages.KernelMessage.IExecuteRequestMsg, __kernel_messages.KernelMessage.IExecuteReplyMsg>
            /// <summary>Send an experimental `debug_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            /// <param name="disposeOnDone">- Whether to dispose of the future when done.</param>
            abstract requestDebug: content: __kernel_messages.KernelMessage.IDebugRequestMsg * ?disposeOnDone: bool -> __kernel_kernel.Kernel.IControlFuture<__kernel_messages.KernelMessage.IDebugRequestMsg, __kernel_messages.KernelMessage.IDebugReplyMsg>
            /// <summary>Send an `is_complete_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            abstract requestIsComplete: content: __kernel_messages.KernelMessage.IIsCompleteRequestMsg -> Promise<__kernel_messages.KernelMessage.IIsCompleteReplyMsg>
            /// <summary>Send a `comm_info_request` message.</summary>
            /// <param name="content">- The content of the request.</param>
            abstract requestCommInfo: content: __kernel_messages.KernelMessage.ICommInfoRequestMsg -> Promise<__kernel_messages.KernelMessage.ICommInfoReplyMsg>
            /// <summary>Send an `input_reply` message.</summary>
            /// <param name="content">- The content of the reply.
            /// 
            /// #### Notes
            /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#messages-on-the-stdin-router-dealer-sockets).</param>
            abstract sendInputReply: content: __kernel_messages.KernelMessage.IInputReplyMsg -> unit
            /// <summary>Connect to a comm, or create a new one.</summary>
            /// <param name="targetName">- The name of the comm target.</param>
            abstract connectToComm: targetName: string * ?commId: string -> __kernel_kernel.Kernel.IComm
            /// <summary>Register a comm target handler.</summary>
            /// <param name="targetName">- The name of the comm target.</param>
            /// <param name="callback">- The callback invoked for a comm open message.
            /// 
            /// #### Notes
            /// Only one comm target can be registered to a target name at a time, an
            /// existing callback for the same target name will be overridden.  A registered
            /// comm target handler will take precedence over a comm which specifies a
            /// `target_module`.
            /// 
            /// If the callback returns a promise, kernel message processing will pause
            /// until the returned promise is fulfilled.</param>
            abstract registerCommTarget: targetName: string * callback: (__kernel_kernel.Kernel.IComm -> __kernel_messages.KernelMessage.ICommOpenMsg -> U2<unit, PromiseLike<unit>>) -> unit
            /// <summary>Remove a comm target handler.</summary>
            /// <param name="targetName">- The name of the comm target to remove.</param>
            /// <param name="callback">- The callback to remove.
            /// 
            /// #### Notes
            /// The comm target is only removed if it matches the callback argument.</param>
            abstract removeCommTarget: targetName: string * callback: (__kernel_kernel.Kernel.IComm -> __kernel_messages.KernelMessage.ICommOpenMsg -> U2<unit, PromiseLike<unit>>) -> unit
            /// <summary>Register an IOPub message hook.</summary>
            /// <param name="hook">- The callback invoked for the message.
            /// 
            /// #### Notes
            /// The IOPub hook system allows you to preempt the handlers for IOPub
            /// messages with a given parent_header message id. The most recently
            /// registered hook is run first. If a hook return value resolves to false,
            /// any later hooks and the future's onIOPub handler will not run. If a hook
            /// throws an error, the error is logged to the console and the next hook is
            /// run. If a hook is registered during the hook processing, it will not run
            /// until the next message. If a hook is disposed during the hook processing,
            /// it will be deactivated immediately.
            /// 
            /// See also [[IFuture.registerMessageHook]].</param>
            abstract registerMessageHook: msgId: string * hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
            /// <summary>Remove an IOPub message hook.</summary>
            /// <param name="hook">- The callback invoked for the message.</param>
            abstract removeMessageHook: msgId: string * hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit

        /// The full interface of a kernel.
        type [<AllowNullLiteral>] IKernel =
            inherit IKernelConnection
            /// A signal emitted when the kernel is shut down.
            abstract terminated: ISignal<IKernel, unit> with get, set
            /// A signal emitted when the kernel status changes.
            abstract statusChanged: ISignal<IKernel, __kernel_kernel.Kernel.Status> with get, set
            /// A signal emitted after an iopub kernel message is handled.
            abstract iopubMessage: ISignal<IKernel, __kernel_messages.KernelMessage.IIOPubMessage> with get, set
            /// A signal emitted for unhandled non-iopub kernel messages that claimed to
            /// be responses for messages we sent using this kernel object.
            abstract unhandledMessage: ISignal<IKernel, __kernel_messages.KernelMessage.IMessage> with get, set
            /// A signal emitted when any kernel message is sent or received.
            /// 
            /// #### Notes
            /// This signal is emitted before any message handling has happened. The
            /// message should be treated as read-only.
            abstract anyMessage: ISignal<IKernel, IAnyMessageArgs> with get, set
            /// The server settings for the kernel.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// Shutdown a kernel.
            abstract shutdown: unit -> Promise<unit>

        /// The options object used to initialize a kernel.
        type [<AllowNullLiteral>] IOptions =
            /// The kernel type (e.g. python3).
            abstract name: string option with get, set
            /// The server settings for the kernel.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// The username of the kernel client.
            abstract username: string option with get, set
            /// Whether the kernel connection should handle comm messages
            /// 
            /// #### Notes
            /// The comm message protocol currently has implicit assumptions that only
            /// one kernel connection is handling comm messages. This option allows a
            /// kernel connection to opt out of handling comms.
            /// 
            /// See https://github.com/jupyter/jupyter_client/issues/263
            abstract handleComms: bool option with get, set
            /// The unique identifier for the kernel client.
            abstract clientId: string option with get, set

        /// Object which manages kernel instances for a given base url.
        /// 
        /// #### Notes
        /// The manager is responsible for maintaining the state of running
        /// kernels and the initial fetch of kernel specs.
        type [<AllowNullLiteral>] IManager =
            inherit IDisposable
            /// A signal emitted when the kernel specs change.
            abstract specsChanged: ISignal<IManager, ISpecModels> with get, set
            /// A signal emitted when the running kernels change.
            abstract runningChanged: ISignal<IManager, ResizeArray<IModel>> with get, set
            /// A signal emitted when there is a connection failure.
            abstract connectionFailure: ISignal<IManager, Serverconnection.ServerConnection.NetworkError> with get, set
            /// The server settings for the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// The kernel spec models.
            /// 
            /// #### Notes
            /// The value will be null until the manager is ready.
            abstract specs: __kernel_kernel.Kernel.ISpecModels option
            /// Whether the manager is ready.
            abstract isReady: bool
            /// A promise that resolves when the manager is initially ready.
            abstract ready: Promise<unit>
            /// Create an iterator over the known running kernels.
            abstract running: unit -> IIterator<IModel>
            /// Force a refresh of the specs from the server.
            abstract refreshSpecs: unit -> Promise<unit>
            /// Force a refresh of the running kernels.
            abstract refreshRunning: unit -> Promise<unit>
            /// <summary>Start a new kernel.</summary>
            /// <param name="options">- The kernel options to use.</param>
            abstract startNew: ?options: IOptions -> Promise<IKernel>
            /// <summary>Find a kernel by id.</summary>
            /// <param name="id">- The id of the target kernel.</param>
            abstract findById: id: string -> Promise<IModel>
            /// <summary>Connect to an existing kernel.</summary>
            /// <param name="model">- The model of the target kernel.</param>
            abstract connectTo: model: __kernel_kernel.Kernel.IModel -> IKernel
            /// <summary>Shut down a kernel by id.</summary>
            /// <param name="id">- The id of the target kernel.</param>
            abstract shutdown: id: string -> Promise<unit>
            /// Shut down all kernels.
            abstract shutdownAll: unit -> Promise<unit>

        /// A Future interface for responses from the kernel.
        /// 
        /// When a message is sent to a kernel, a Future is created to handle any
        /// responses that may come from the kernel.
        type [<AllowNullLiteral>] IFuture<'REQUEST, 'REPLY> =
            inherit IDisposable
            /// The original outgoing message.
            abstract msg: 'REQUEST
            /// A promise that resolves when the future is done.
            /// 
            /// #### Notes
            /// The future is done when there are no more responses expected from the
            /// kernel.
            /// 
            /// The `done` promise resolves to the reply message if there is one,
            /// otherwise it resolves to `undefined`.
            abstract ``done``: Promise<'REPLY option>
            /// The reply handler for the kernel future.
            /// 
            /// #### Notes
            /// If the handler returns a promise, all kernel message processing pauses
            /// until the promise is resolved. If there is a reply message, the future
            /// `done` promise also resolves to the reply message after this handler has
            /// been called.
            abstract onReply: ('REPLY -> U2<unit, PromiseLike<unit>>) with get, set
            /// The iopub handler for the kernel future.
            /// 
            /// #### Notes
            /// If the handler returns a promise, all kernel message processing pauses
            /// until the promise is resolved.
            abstract onIOPub: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<unit, PromiseLike<unit>>) with get, set
            /// The stdin handler for the kernel future.
            /// 
            /// #### Notes
            /// If the handler returns a promise, all kernel message processing pauses
            /// until the promise is resolved.
            abstract onStdin: (__kernel_messages.KernelMessage.IStdinMessage -> U2<unit, PromiseLike<unit>>) with get, set
            /// <summary>Register hook for IOPub messages.</summary>
            /// <param name="hook">- The callback invoked for an IOPub message.
            /// 
            /// #### Notes
            /// The IOPub hook system allows you to preempt the handlers for IOPub
            /// messages handled by the future.
            /// 
            /// The most recently registered hook is run first. A hook can return a
            /// boolean or a promise to a boolean, in which case all kernel message
            /// processing pauses until the promise is fulfilled. If a hook return value
            /// resolves to false, any later hooks will not run and the function will
            /// return a promise resolving to false. If a hook throws an error, the error
            /// is logged to the console and the next hook is run. If a hook is
            /// registered during the hook processing, it will not run until the next
            /// message. If a hook is removed during the hook processing, it will be
            /// deactivated immediately.</param>
            abstract registerMessageHook: hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
            /// <summary>Remove a hook for IOPub messages.</summary>
            /// <param name="hook">- The hook to remove.
            /// 
            /// #### Notes
            /// If a hook is removed during the hook processing, it will be deactivated immediately.</param>
            abstract removeMessageHook: hook: (__kernel_messages.KernelMessage.IIOPubMessage -> U2<bool, PromiseLike<bool>>) -> unit
            /// Send an `input_reply` message.
            abstract sendInputReply: content: __kernel_messages.KernelMessage.IInputReplyMsg -> unit

        type IShellFuture<'REPLY> =
            IShellFuture<obj, 'REPLY>

        type IShellFuture =
            IShellFuture<obj, obj>

        type [<AllowNullLiteral>] IShellFuture<'REQUEST, 'REPLY> =
            inherit IFuture<'REQUEST, 'REPLY>

        type IControlFuture<'REPLY> =
            IControlFuture<obj, 'REPLY>

        type IControlFuture =
            IControlFuture<obj, obj>

        type [<AllowNullLiteral>] IControlFuture<'REQUEST, 'REPLY> =
            inherit IFuture<'REQUEST, 'REPLY>

        /// A client side Comm interface.
        type [<AllowNullLiteral>] IComm =
            inherit IDisposable
            /// The unique id for the comm channel.
            abstract commId: string
            /// The target name for the comm channel.
            abstract targetName: string
            /// Callback for a comm close event.
            /// 
            /// #### Notes
            /// This is called when the comm is closed from either the server or client.
            /// If this is called in response to a kernel message and the handler returns
            /// a promise, all kernel message processing pauses until the promise is
            /// resolved.
            abstract onClose: (__kernel_messages.KernelMessage.ICommCloseMsg -> U2<unit, PromiseLike<unit>>) with get, set
            /// Callback for a comm message received event.
            /// 
            /// #### Notes
            /// If the handler returns a promise, all kernel message processing pauses
            /// until the promise is resolved.
            abstract onMsg: (__kernel_messages.KernelMessage.ICommMsgMsg -> U2<unit, PromiseLike<unit>>) with get, set
            /// <summary>Open a comm with optional data and metadata.</summary>
            /// <param name="data">- The data to send to the server on opening.</param>
            /// <param name="metadata">- Additional metatada for the message.</param>
            abstract ``open``: ?data: JSONValue * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> -> IShellFuture
            /// <summary>Send a `comm_msg` message to the kernel.</summary>
            /// <param name="data">- The data to send to the server on opening.</param>
            /// <param name="metadata">- Additional metatada for the message.</param>
            /// <param name="buffers">- Optional buffer data.</param>
            /// <param name="disposeOnDone">- Whether to dispose of the future when done.</param>
            abstract send: data: JSONValue * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> * ?disposeOnDone: bool -> IShellFuture
            /// <summary>Close the comm.</summary>
            /// <param name="data">- The data to send to the server on opening.</param>
            /// <param name="metadata">- Additional metatada for the message.</param>
            abstract close: ?data: JSONValue * ?metadata: JSONObject * ?buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> -> IShellFuture

        type [<StringEnum>] [<RequireQualifiedAccess>] Status =
            | Unknown
            | Starting
            | Reconnecting
            | Idle
            | Busy
            | Restarting
            | Autorestarting
            | Dead
            | Connected

        /// The kernel model provided by the server.
        /// 
        /// #### Notes
        /// See the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/kernels).
        type [<AllowNullLiteral>] IModel =
            inherit JSONObject
            /// Unique identifier of the kernel server session.
            abstract id: string
            /// The name of the kernel.
            abstract name: string

        /// Kernel Spec interface.
        /// 
        /// #### Notes
        /// See [Kernel specs](https://jupyter-client.readthedocs.io/en/latest/kernels.html#kernelspecs).
        type [<AllowNullLiteral>] ISpecModel =
            inherit JSONObject
            /// The name of the kernel spec.
            abstract name: string
            /// The name of the language of the kernel.
            abstract language: string
            /// A list of command line arguments used to start the kernel.
            abstract argv: ResizeArray<string>
            /// The kernel’s name as it should be displayed in the UI.
            abstract display_name: string
            /// A dictionary of environment variables to set for the kernel.
            abstract env: JSONObject option
            /// A mapping of resource file name to download path.
            abstract resources: TypeLiteral_01
            /// A dictionary of additional attributes about this kernel; used by clients to aid in kernel selection.
            abstract metadata: JSONObject option

        /// The available kernelSpec models.
        /// 
        /// #### Notes
        /// See the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/kernelspecs).
        type [<AllowNullLiteral>] ISpecModels =
            inherit JSONObject
            /// The name of the default kernel spec.
            abstract ``default``: string with get, set
            /// A mapping of kernel spec name to spec.
            abstract kernelspecs: TypeLiteral_02

        /// Arguments interface for the anyMessage signal.
        type [<AllowNullLiteral>] IAnyMessageArgs =
            /// The message that is being signaled.
            abstract msg: obj with get, set
            /// The direction of the message.
            abstract direction: U2<string, string> with get, set

        type [<AllowNullLiteral>] TypeLiteral_02 =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> ISpecModel with get, set

        type [<AllowNullLiteral>] TypeLiteral_01 =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

module __kernel_manager =
    type Poll = JupyterlabCoreutils.Poll.Poll //__kernel_@jupyterlab_coreutils.Poll
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> //__kernel_@phosphor_algorithm.IIterator
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __kernel_@phosphor_signaling.ISignal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection
    // type Kernel = __kernel_kernel.Kernel

    type [<AllowNullLiteral>] IExports =
        abstract KernelManager: KernelManagerStatic

    /// An implementation of a kernel manager.
    /// The namespace for `KernelManager` class statics.
    type [<AllowNullLiteral>] KernelManager =
        inherit __kernel_kernel.Kernel.IManager
        /// The server settings for the manager.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the terminal manager is disposed.
        abstract isDisposed: bool
        /// Test whether the manager is ready.
        abstract isReady: bool
        /// A promise that fulfills when the manager is ready.
        abstract ready: Promise<unit>
        /// A signal emitted when the running kernels change.
        abstract runningChanged: ISignal<KernelManager, ResizeArray<__kernel_kernel.Kernel.IModel>>
        /// Get the most recently fetched kernel specs.
        abstract specs: __kernel_kernel.Kernel.ISpecModels option
        /// A signal emitted when the specs change.
        abstract specsChanged: ISignal<KernelManager, __kernel_kernel.Kernel.ISpecModels>
        /// A signal emitted when there is a connection failure.
        abstract connectionFailure: ISignal<KernelManager, Error>
        /// <summary>Connect to an existing kernel.</summary>
        /// <param name="model">- The model of the target kernel.</param>
        abstract connectTo: model: __kernel_kernel.Kernel.IModel -> __kernel_kernel.Kernel.IKernel
        /// Dispose of the resources used by the manager.
        abstract dispose: unit -> unit
        /// <summary>Find a kernel by id.</summary>
        /// <param name="id">- The id of the target kernel.</param>
        abstract findById: id: string -> Promise<__kernel_kernel.Kernel.IModel>
        /// Force a refresh of the running kernels.
        abstract refreshRunning: unit -> Promise<unit>
        /// Force a refresh of the specs from the server.
        abstract refreshSpecs: unit -> Promise<unit>
        /// Create an iterator over the most recent running kernels.
        abstract running: unit -> IIterator<__kernel_kernel.Kernel.IModel>
        /// <summary>Shut down a kernel by id.</summary>
        /// <param name="id">- The id of the target kernel.</param>
        abstract shutdown: id: string -> Promise<unit>
        /// Shut down all kernels.
        abstract shutdownAll: unit -> Promise<unit>
        /// <summary>Start a new kernel.</summary>
        /// <param name="options">- The kernel options to use.</param>
        abstract startNew: ?options: __kernel_kernel.Kernel.IOptions -> Promise<__kernel_kernel.Kernel.IKernel>
        /// Execute a request to the server to poll running kernels and update state.
        abstract requestRunning: unit -> Promise<unit>
        /// Execute a request to the server to poll specs and update state.
        abstract requestSpecs: unit -> Promise<unit>

    /// An implementation of a kernel manager.
    /// The namespace for `KernelManager` class statics.
    type [<AllowNullLiteral>] KernelManagerStatic =
        /// <summary>Construct a new kernel manager.</summary>
        /// <param name="options">- The default options for kernel.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: KernelManager.IOptions -> KernelManager

    module KernelManager =

        /// The options used to initialize a KernelManager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings for the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// When the manager stops polling the API. Defaults to `when-hidden`.
            abstract standby: JupyterlabCoreutils.Poll.Poll.Standby option with get, set

module __kernel_messages =
    // type nbformat = JupyterlabCoreutils.Nbformat // __kernel_@jupyterlab_coreutils.nbformat
    type JSONObject = PhosphorCoreutils.JSONObject // __kernel_@phosphor_coreutils.JSONObject
    // type Kernel = __kernel_kernel.Kernel

    module KernelMessage =

        type [<AllowNullLiteral>] IExports =
            abstract createMessage: options: IOptions<'T> -> 'T
            /// Test whether a kernel message is a `'stream'` message.
            abstract isStreamMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'display_data'` message.
            abstract isDisplayDataMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'update_display_data'` message.
            abstract isUpdateDisplayDataMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'execute_input'` message.
            abstract isExecuteInputMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'execute_result'` message.
            abstract isExecuteResultMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'error'` message.
            abstract isErrorMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'status'` message.
            abstract isStatusMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'clear_output'` message.
            abstract isClearOutputMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an experimental `'debug_event'` message.
            abstract isDebugEventMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'comm_open'` message.
            abstract isCommOpenMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'comm_close'` message.
            abstract isCommCloseMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'comm_msg'` message.
            abstract isCommMsgMsg: msg: IMessage -> bool
            /// Test whether a kernel message is a `'kernel_info_request'` message.
            abstract isInfoRequestMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'execute_reply'` message.
            abstract isExecuteReplyMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an experimental `'debug_request'` message.
            abstract isDebugRequestMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an experimental `'debug_reply'` message.
            abstract isDebugReplyMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'input_request'` message.
            abstract isInputRequestMsg: msg: IMessage -> bool
            /// Test whether a kernel message is an `'input_reply'` message.
            abstract isInputReplyMsg: msg: IMessage -> bool

        type [<AllowNullLiteral>] IOptions<'T> =
            abstract session: string with get, set
            abstract channel: 'T with get, set
            abstract msgType: 'T with get, set
            abstract content: 'T with get, set
            abstract buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> option with get, set
            abstract metadata: JSONObject option with get, set
            abstract msgId: string option with get, set
            abstract username: string option with get, set
            abstract parentHeader: 'T option with get, set

        type [<StringEnum>] [<RequireQualifiedAccess>] ShellMessageType =
            | Comm_close
            | Comm_info_reply
            | Comm_info_request
            | Comm_msg
            | Comm_open
            | Complete_reply
            | Complete_request
            | Execute_reply
            | Execute_request
            | History_reply
            | History_request
            | Inspect_reply
            | Inspect_request
            | Interrupt_reply
            | Interrupt_request
            | Is_complete_reply
            | Is_complete_request
            | Kernel_info_reply
            | Kernel_info_request
            | Shutdown_reply
            | Shutdown_request

        type [<StringEnum>] [<RequireQualifiedAccess>] ControlMessageType =
            | Debug_request
            | Debug_reply

        type [<StringEnum>] [<RequireQualifiedAccess>] IOPubMessageType =
            | Clear_output
            | Comm_close
            | Comm_msg
            | Comm_open
            | Display_data
            | Error
            | Execute_input
            | Execute_result
            | Status
            | Stream
            | Update_display_data
            | Debug_event

        type [<StringEnum>] [<RequireQualifiedAccess>] StdinMessageType =
            | Input_request
            | Input_reply

        type MessageType =
            U4<IOPubMessageType, ShellMessageType, ControlMessageType, StdinMessageType>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module MessageType =
            let ofIOPubMessageType v: MessageType = v |> U4.Case1
            let isIOPubMessageType (v: MessageType) = match v with U4.Case1 _ -> true | _ -> false
            let asIOPubMessageType (v: MessageType) = match v with U4.Case1 o -> Some o | _ -> None
            let ofShellMessageType v: MessageType = v |> U4.Case2
            let isShellMessageType (v: MessageType) = match v with U4.Case2 _ -> true | _ -> false
            let asShellMessageType (v: MessageType) = match v with U4.Case2 o -> Some o | _ -> None
            let ofControlMessageType v: MessageType = v |> U4.Case3
            let isControlMessageType (v: MessageType) = match v with U4.Case3 _ -> true | _ -> false
            let asControlMessageType (v: MessageType) = match v with U4.Case3 o -> Some o | _ -> None
            let ofStdinMessageType v: MessageType = v |> U4.Case4
            let isStdinMessageType (v: MessageType) = match v with U4.Case4 _ -> true | _ -> false
            let asStdinMessageType (v: MessageType) = match v with U4.Case4 o -> Some o | _ -> None

        type [<StringEnum>] [<RequireQualifiedAccess>] Channel =
            | Shell
            | Control
            | Iopub
            | Stdin

        type IHeader =
            IHeader<obj>

        /// Kernel message header content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#general-message-format).
        /// 
        /// **See also:** [[IMessage]]
        type [<AllowNullLiteral>] IHeader<'T> =
            /// ISO 8601 timestamp for when the message is created
            abstract date: string with get, set
            /// Message id, typically UUID, must be unique per message
            abstract msg_id: string with get, set
            /// Message type
            abstract msg_type: 'T with get, set
            /// Session id, typically UUID, should be unique per session.
            abstract session: string with get, set
            /// The user sending the message
            abstract username: string with get, set
            /// The message protocol version, should be 5.1, 5.2, 5.3, etc.
            abstract version: string with get, set

        type IMessage =
            IMessage<obj>

        /// Kernel message specification.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#general-message-format).
        type [<AllowNullLiteral>] IMessage<'MSGTYPE> =
            /// An optional list of binary buffers.
            abstract buffers: ResizeArray<U2<ArrayBuffer, ArrayBufferView>> option with get, set
            /// The channel on which the message is transmitted.
            abstract channel: Channel with get, set
            /// The content of the message.
            abstract content: Message with get, set
            /// The message header.
            abstract header: IHeader<'MSGTYPE> with get, set
            /// Metadata associated with the message.
            abstract metadata: JSONObject with get, set
            /// The parent message
            abstract parent_header: U2<IHeader, TypeLiteral_01> with get, set

        type IShellMessage =
            IShellMessage<obj>

        /// A kernel message on the `'shell'` channel.
        type [<AllowNullLiteral>] IShellMessage<'T> =
            inherit IMessage<'T>
            abstract channel: string with get, set

        type IControlMessage =
            IControlMessage<obj>

        /// A kernel message on the `'control'` channel.
        type [<AllowNullLiteral>] IControlMessage<'T> =
            inherit IMessage<'T>
            abstract channel: string with get, set

        type IShellControlMessage =
            U2<IShellMessage, IControlMessage>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module IShellControlMessage =
            let ofIShellMessage v: IShellControlMessage = v |> U2.Case1
            let isIShellMessage (v: IShellControlMessage) = match v with U2.Case1 _ -> true | _ -> false
            let asIShellMessage (v: IShellControlMessage) = match v with U2.Case1 o -> Some o | _ -> None
            let ofIControlMessage v: IShellControlMessage = v |> U2.Case2
            let isIControlMessage (v: IShellControlMessage) = match v with U2.Case2 _ -> true | _ -> false
            let asIControlMessage (v: IShellControlMessage) = match v with U2.Case2 o -> Some o | _ -> None

        type IIOPubMessage =
            IIOPubMessage<obj>

        /// A kernel message on the `'iopub'` channel.
        type [<AllowNullLiteral>] IIOPubMessage<'T> =
            inherit IMessage<'T>
            abstract channel: string with get, set

        type IStdinMessage =
            IStdinMessage<obj>

        /// A kernel message on the `'stdin'` channel.
        type [<AllowNullLiteral>] IStdinMessage<'T> =
            inherit IMessage<'T>
            abstract channel: string with get, set

        type Message =
            obj

        /// A `'stream'` message on the `'iopub'` channel.
        /// 
        /// See [Streams](https://jupyter-client.readthedocs.io/en/latest/messaging.html#streams-stdout-stderr-etc).
        type [<AllowNullLiteral>] IStreamMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_02 with get, set

        /// A `'display_data'` message on the `'iopub'` channel.
        /// 
        /// See [Display data](https://jupyter-client.readthedocs.io/en/latest/messaging.html#display-data).
        type [<AllowNullLiteral>] IDisplayDataMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_04 with get, set

        /// An `'update_display_data'` message on the `'iopub'` channel.
        /// 
        /// See [Update Display data](https://jupyter-client.readthedocs.io/en/latest/messaging.html#update-display-data).
        type [<AllowNullLiteral>] IUpdateDisplayDataMsg =
            inherit IIOPubMessage<string>
            abstract content: obj with get, set

        /// An `'execute_input'` message on the `'iopub'` channel.
        /// 
        /// See [Code inputs](https://jupyter-client.readthedocs.io/en/latest/messaging.html#code-inputs).
        type [<AllowNullLiteral>] IExecuteInputMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_05 with get, set

        /// An `'execute_result'` message on the `'iopub'` channel.
        /// 
        /// See [Execution results](https://jupyter-client.readthedocs.io/en/latest/messaging.html#id4).
        type [<AllowNullLiteral>] IExecuteResultMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_06 with get, set

        /// A `'error'` message on the `'iopub'` channel.
        /// 
        /// See [Execution errors](https://jupyter-client.readthedocs.io/en/latest/messaging.html#execution-errors).
        type [<AllowNullLiteral>] IErrorMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_07 with get, set

        /// A `'status'` message on the `'iopub'` channel.
        /// 
        /// See [Kernel status](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-status).
        type [<AllowNullLiteral>] IStatusMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_08 with get, set

        /// A `'clear_output'` message on the `'iopub'` channel.
        /// 
        /// See [Clear output](https://jupyter-client.readthedocs.io/en/latest/messaging.html#clear-output).
        type [<AllowNullLiteral>] IClearOutputMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_09 with get, set

        /// An experimental `'debug_event'` message on the `'iopub'` channel
        type [<AllowNullLiteral>] IDebugEventMsg =
            inherit IIOPubMessage<string>
            abstract content: TypeLiteral_10 with get, set

        type ICommOpenMsg =
            ICommOpenMsg<obj>

        /// A `'comm_open'` message on the `'iopub'` channel.
        /// 
        /// See [Comm open](https://jupyter-client.readthedocs.io/en/latest/messaging.html#opening-a-comm).
        type [<AllowNullLiteral>] ICommOpenMsg<'T> =
            inherit IMessage<string>
            abstract channel: 'T with get, set
            abstract content: TypeLiteral_11 with get, set

        type ICommCloseMsg =
            ICommCloseMsg<obj>

        /// A `'comm_close'` message on the `'iopub'` channel.
        /// 
        /// See [Comm close](https://jupyter-client.readthedocs.io/en/latest/messaging.html#opening-a-comm).
        type [<AllowNullLiteral>] ICommCloseMsg<'T> =
            inherit IMessage<string>
            abstract channel: 'T with get, set
            abstract content: TypeLiteral_12 with get, set

        type ICommMsgMsg =
            ICommMsgMsg<obj>

        /// A `'comm_msg'` message on the `'iopub'` channel.
        /// 
        /// See [Comm msg](https://jupyter-client.readthedocs.io/en/latest/messaging.html#opening-a-comm).
        type [<AllowNullLiteral>] ICommMsgMsg<'T> =
            inherit IMessage<string>
            abstract channel: 'T with get, set
            abstract content: TypeLiteral_12 with get, set

        /// Reply content indicating a sucessful request.
        type [<AllowNullLiteral>] IReplyOkContent =
            abstract status: string with get, set

        /// Reply content indicating an error.
        /// 
        /// See the [Message spec](https://jupyter-client.readthedocs.io/en/latest/messaging.html#request-reply) for details.
        type [<AllowNullLiteral>] IReplyErrorContent =
            abstract status: string with get, set
            /// Exception name
            abstract ename: string with get, set
            /// Exception value
            abstract evalue: string with get, set
            /// Traceback
            abstract traceback: ResizeArray<string> with get, set

        /// Reply content indicating an aborted request.
        /// 
        /// This is [deprecated](https://jupyter-client.readthedocs.io/en/latest/messaging.html#request-reply)
        /// in message spec 5.1. Kernels should send an 'error' reply instead.
        type [<AllowNullLiteral>] IReplyAbortContent =
            abstract status: string with get, set

        type ReplyContent<'T> =
            U3<'T, IReplyErrorContent, IReplyAbortContent>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module ReplyContent =
            let ofT v: ReplyContent<'T> = v |> U3.Case1
            let isT (v: ReplyContent<'T>) = match v with U3.Case1 _ -> true | _ -> false
            let asT (v: ReplyContent<'T>) = match v with U3.Case1 o -> Some o | _ -> None
            let ofIReplyErrorContent v: ReplyContent<'T> = v |> U3.Case2
            let isIReplyErrorContent (v: ReplyContent<'T>) = match v with U3.Case2 _ -> true | _ -> false
            let asIReplyErrorContent (v: ReplyContent<'T>) = match v with U3.Case2 o -> Some o | _ -> None
            let ofIReplyAbortContent v: ReplyContent<'T> = v |> U3.Case3
            let isIReplyAbortContent (v: ReplyContent<'T>) = match v with U3.Case3 _ -> true | _ -> false
            let asIReplyAbortContent (v: ReplyContent<'T>) = match v with U3.Case3 o -> Some o | _ -> None

        /// A `'kernel_info_request'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-info).
        type [<AllowNullLiteral>] IInfoRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_01 with get, set

        /// A `'kernel_info_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-info).
        type [<AllowNullLiteral>] IInfoReply =
            inherit IReplyOkContent
            abstract protocol_version: string with get, set
            abstract implementation: string with get, set
            abstract implementation_version: string with get, set
            abstract language_info: ILanguageInfo with get, set
            abstract banner: string with get, set
            abstract help_links: ResizeArray<TypeLiteral_13> with get, set

        /// The kernel language information specification.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-info).
        type [<AllowNullLiteral>] ILanguageInfo =
            inherit JupyterlabCoreutils.Nbformat.Nbformat.ILanguageInfoMetadata
            abstract version: string with get, set
            abstract nbconverter_exporter: string option with get, set

        /// A `'kernel_info_reply'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#kernel-info).
        type [<AllowNullLiteral>] IInfoReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<IInfoReply> with get, set

        /// A  `'complete_request'` message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#completion).
        /// 
        /// **See also:** [[ICompleteReplyMsg]], [[IKernel.complete]]
        type [<AllowNullLiteral>] ICompleteRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_14 with get, set

        /// A `'complete_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#completion).
        /// 
        /// **See also:** [[ICompleteRequest]], [[IKernel.complete]]
        type [<AllowNullLiteral>] ICompleteReply =
            inherit IReplyOkContent
            abstract matches: ResizeArray<string> with get, set
            abstract cursor_start: float with get, set
            abstract cursor_end: float with get, set
            abstract metadata: JSONObject with get, set

        /// A `'complete_reply'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#completion).
        /// 
        /// **See also:** [[ICompleteRequest]], [[IKernel.complete]]
        type [<AllowNullLiteral>] ICompleteReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<ICompleteReply> with get, set

        /// An `'inspect_request'` message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#introspection).
        /// 
        /// **See also:** [[IInspectReplyMsg]], [[[IKernel.inspect]]]
        type [<AllowNullLiteral>] IInspectRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_15 with get, set

        /// A `'inspect_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#introspection).
        /// 
        /// **See also:** [[IInspectRequest]], [[IKernel.inspect]]
        type [<AllowNullLiteral>] IInspectReply =
            inherit IReplyOkContent
            abstract found: bool with get, set
            abstract data: JSONObject with get, set
            abstract metadata: JSONObject with get, set

        /// A `'inspect_reply'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#introspection).
        /// 
        /// **See also:** [[IInspectRequest]], [[IKernel.inspect]]
        type [<AllowNullLiteral>] IInspectReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<IInspectReply> with get, set

        /// A `'history_request'` message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryReplyMsg]], [[[IKernel.history]]]
        type [<AllowNullLiteral>] IHistoryRequestMsg =
            inherit IShellMessage<string>
            abstract content: U3<IHistoryRequestRange, IHistoryRequestSearch, IHistoryRequestTail> with get, set

        /// The content of a `'history_request'` range message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryReply]], [[[IKernel.history]]]
        type [<AllowNullLiteral>] IHistoryRequestRange =
            abstract output: bool with get, set
            abstract raw: bool with get, set
            abstract hist_access_type: string with get, set
            abstract session: float with get, set
            abstract start: float with get, set
            abstract stop: float with get, set

        /// The content of a `'history_request'` search message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryReply]], [[[IKernel.history]]]
        type [<AllowNullLiteral>] IHistoryRequestSearch =
            abstract output: bool with get, set
            abstract raw: bool with get, set
            abstract hist_access_type: string with get, set
            abstract n: float with get, set
            abstract pattern: string with get, set
            abstract unique: bool with get, set

        /// The content of a `'history_request'` tail message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryReply]], [[[IKernel.history]]]
        type [<AllowNullLiteral>] IHistoryRequestTail =
            abstract output: bool with get, set
            abstract raw: bool with get, set
            abstract hist_access_type: string with get, set
            abstract n: float with get, set

        /// A `'history_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryRequest]], [[IKernel.history]]
        type [<AllowNullLiteral>] IHistoryReply =
            inherit IReplyOkContent
            abstract history: U2<ResizeArray<float * float * string>, ResizeArray<float * float * string * string>> with get, set

        /// A `'history_reply'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#history).
        /// 
        /// **See also:** [[IHistoryRequest]], [[IKernel.history]]
        type [<AllowNullLiteral>] IHistoryReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<IHistoryReply> with get, set

        /// An `'is_complete_request'` message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#code-completeness).
        /// 
        /// **See also:** [[IIsCompleteReplyMsg]], [[IKernel.isComplete]]
        type [<AllowNullLiteral>] IIsCompleteRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_16 with get, set

        /// An `'is_complete_reply'` message on the `'stream'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#code-completeness).
        /// 
        /// **See also:** [[IIsCompleteRequest]], [[IKernel.isComplete]]
        type [<AllowNullLiteral>] IIsCompleteReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<U2<IIsCompleteReplyIncomplete, IIsCompleteReplyOther>> with get, set

        /// An 'incomplete' completion reply
        type [<AllowNullLiteral>] IIsCompleteReplyIncomplete =
            abstract status: string with get, set
            abstract indent: string with get, set

        /// A completion reply for completion or invalid states.
        type [<AllowNullLiteral>] IIsCompleteReplyOther =
            abstract status: U3<string, string, string> with get, set

        /// An `execute_request` message on the `'shell'` channel.
        type [<AllowNullLiteral>] IExecuteRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_17 with get, set

        /// The content of an `execute-reply` message.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#execution-results).
        type [<AllowNullLiteral>] IExecuteCount =
            abstract execution_count: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set

        type [<AllowNullLiteral>] IExecuteReplyBase =
            interface end

        /// The `'execute_reply'` contents for an `'ok'` status.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#execution-results).
        type [<AllowNullLiteral>] IExecuteReply =
            inherit IExecuteReplyBase
            /// A list of payload objects.
            /// Payloads are considered deprecated.
            /// The only requirement of each payload object is that it have a 'source'
            /// key, which is a string classifying the payload (e.g. 'page').
            abstract payload: ResizeArray<JSONObject> option with get, set
            /// Results for the user_expressions.
            abstract user_expressions: JSONObject with get, set

        /// An `'execute_reply'` message on the `'stream'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#execution-results).
        /// 
        /// **See also:** [[IExecuteRequest]], [[IKernel.execute]]
        type [<AllowNullLiteral>] IExecuteReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: obj with get, set

        /// A `'comm_info_request'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#comm-info).
        /// 
        /// **See also:** [[ICommInfoReplyMsg]], [[IKernel.commInfo]]
        type [<AllowNullLiteral>] ICommInfoRequestMsg =
            inherit IShellMessage<string>
            abstract content: TypeLiteral_18 with get, set

        /// A `'comm_info_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#comm-info).
        /// 
        /// **See also:** [[ICommInfoRequest]], [[IKernel.commInfo]]
        type [<AllowNullLiteral>] ICommInfoReply =
            inherit IReplyOkContent
            /// Mapping of comm ids to target names.
            abstract comms: TypeLiteral_20 with get, set

        /// A `'comm_info_reply'` message on the `'shell'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#comm-info).
        /// 
        /// **See also:** [[ICommInfoRequestMsg]], [[IKernel.commInfo]]
        type [<AllowNullLiteral>] ICommInfoReplyMsg =
            inherit IShellMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<ICommInfoReply> with get, set

        /// An experimental `'debug_request'` messsage on the `'control'` channel.
        type [<AllowNullLiteral>] IDebugRequestMsg =
            inherit IControlMessage<string>
            abstract content: TypeLiteral_21 with get, set

        /// An experimental `'debug_reply'` messsage on the `'control'` channel.
        type [<AllowNullLiteral>] IDebugReplyMsg =
            inherit IControlMessage<string>
            abstract content: TypeLiteral_22 with get, set

        /// An `'input_request'` message on the `'stdin'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#messages-on-the-stdin-router-dealer-sockets).
        type [<AllowNullLiteral>] IInputRequestMsg =
            inherit IStdinMessage<string>
            abstract content: TypeLiteral_23 with get, set

        /// An `'input_reply'` message content.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#messages-on-the-stdin-router-dealer-sockets).
        type [<AllowNullLiteral>] IInputReply =
            inherit IReplyOkContent
            abstract value: string with get, set

        /// An `'input_reply'` message on the `'stdin'` channel.
        /// 
        /// See [Messaging in Jupyter](https://jupyter-client.readthedocs.io/en/latest/messaging.html#messages-on-the-stdin-router-dealer-sockets).
        type [<AllowNullLiteral>] IInputReplyMsg =
            inherit IStdinMessage<string>
            abstract parent_header: IHeader<string> with get, set
            abstract content: ReplyContent<IInputReply> with get, set

        type [<AllowNullLiteral>] TypeLiteral_01 =
            interface end

        type [<AllowNullLiteral>] TypeLiteral_16 =
            abstract code: string with get, set

        type [<AllowNullLiteral>] TypeLiteral_14 =
            abstract code: string with get, set
            abstract cursor_pos: int with get, set

        type [<AllowNullLiteral>] TypeLiteral_15 =
            abstract code: string with get, set
            abstract cursor_pos: int with get, set
            abstract detail_level: obj with get, set

        type [<AllowNullLiteral>] TypeLiteral_05 =
            abstract code: string with get, set
            abstract execution_count: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set

        type [<AllowNullLiteral>] TypeLiteral_12 =
            abstract comm_id: string with get, set
            abstract data: JSONObject with get, set

        type [<AllowNullLiteral>] TypeLiteral_11 =
            abstract comm_id: string with get, set
            abstract target_name: string with get, set
            abstract data: JSONObject with get, set
            abstract target_module: string option with get, set

        type [<AllowNullLiteral>] TypeLiteral_04 =
            abstract data: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle with get, set
            abstract metadata: JupyterlabCoreutils.Nbformat.Nbformat.OutputMetadata with get, set
            abstract transient: TypeLiteral_03 option with get, set

        type [<AllowNullLiteral>] TypeLiteral_03 =
            abstract display_id: string option with get, set

        type [<AllowNullLiteral>] TypeLiteral_07 =
            abstract ename: string with get, set
            abstract evalue: string with get, set
            abstract traceback: ResizeArray<string> with get, set

        type [<AllowNullLiteral>] TypeLiteral_06 =
            abstract execution_count: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount with get, set
            abstract data: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle with get, set
            abstract metadata: JupyterlabCoreutils.Nbformat.Nbformat.OutputMetadata with get, set
            abstract transient: TypeLiteral_03 option with get, set

        type [<AllowNullLiteral>] TypeLiteral_08 =
            abstract execution_state: __kernel_kernel.Kernel.Status with get, set

        type [<AllowNullLiteral>] TypeLiteral_02 =
            abstract name: U2<string, string> with get, set
            abstract text: string with get, set

        type [<AllowNullLiteral>] TypeLiteral_10 =
            abstract seq: float with get, set
            abstract ``type``: string with get, set
            abstract ``event``: string with get, set
            abstract body: obj option with get, set

        type [<AllowNullLiteral>] TypeLiteral_21 =
            abstract seq: float with get, set
            abstract ``type``: string with get, set
            abstract command: string with get, set
            abstract arguments: obj option with get, set

        type [<AllowNullLiteral>] TypeLiteral_22 =
            abstract seq: float with get, set
            abstract ``type``: string with get, set
            abstract request_seq: float with get, set
            abstract success: bool with get, set
            abstract command: string with get, set
            abstract message: string option with get, set
            abstract body: obj option with get, set

        type [<AllowNullLiteral>] TypeLiteral_19 =
            abstract target_name: string with get, set

        type [<AllowNullLiteral>] TypeLiteral_13 =
            abstract text: string with get, set
            abstract url: string with get, set

        type [<AllowNullLiteral>] TypeLiteral_09 =
            abstract wait: bool with get, set

        type [<AllowNullLiteral>] TypeLiteral_20 =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: commId: string -> TypeLiteral_19 with get, set

        type [<AllowNullLiteral>] TypeLiteral_17 =
            /// The code to execute.
            abstract code: string with get, set
            /// Whether to execute the code as quietly as possible.
            /// The default is `false`.
            abstract silent: bool option with get, set
            /// Whether to store history of the execution.
            /// The default `true` if silent is False.
            /// It is forced to  `false ` if silent is `true`.
            abstract store_history: bool option with get, set
            /// A mapping of names to expressions to be evaluated in the
            /// kernel's interactive namespace.
            abstract user_expressions: JSONObject option with get, set
            /// Whether to allow stdin requests.
            /// The default is `true`.
            abstract allow_stdin: bool option with get, set
            /// Whether to the abort execution queue on an error.
            /// The default is `false`.
            abstract stop_on_error: bool option with get, set

        type [<AllowNullLiteral>] TypeLiteral_18 =
            /// The comm target name to filter returned comms
            abstract target_name: string option with get, set
            /// Filter for returned comms
            abstract target: string option with get, set

        type [<AllowNullLiteral>] TypeLiteral_23 =
            /// The text to show at the prompt.
            abstract prompt: string with get, set
            /// Whether the request is for a password.
            /// If so, the frontend shouldn't echo input.
            abstract password: bool with get, set

module __kernel_serialize =
    // type KernelMessage = __kernel_messages.KernelMessage

    type [<AllowNullLiteral>] IExports =
        /// Deserialize and return the unpacked message.
        /// 
        /// #### Notes
        /// Handles JSON blob strings and binary messages.
        abstract deserialize: data: U2<ArrayBuffer, string> -> __kernel_messages.KernelMessage.IMessage
        /// Serialize a kernel message for transport.
        /// 
        /// #### Notes
        /// If there is binary content, an `ArrayBuffer` is returned,
        /// otherwise the message is converted to a JSON string.
        abstract serialize: msg: __kernel_messages.KernelMessage.IMessage -> U2<string, ArrayBuffer>

module __kernel_validate =
    // type Kernel = __kernel_kernel.Kernel
    // type KernelMessage = __kernel_messages.KernelMessage

    type [<AllowNullLiteral>] IExports =
        /// Validate a kernel message object.
        abstract validateMessage: msg: __kernel_messages.KernelMessage.IMessage -> unit
        /// Validate a `__kernel_kernel.Kernel.IModel` object.
        abstract validateModel: model: __kernel_kernel.Kernel.IModel -> unit
        /// Validate a server kernelspec model to a client side model.
        abstract validateSpecModel: data: obj option -> __kernel_kernel.Kernel.ISpecModel
        /// Validate a `__kernel_kernel.Kernel.ISpecModels` object.
        abstract validateSpecModels: data: obj option -> __kernel_kernel.Kernel.ISpecModels

module __nbconvert_index =
    // type ServerConnection = Serverconnection.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract NbConvertManager: NbConvertManagerStatic

    /// The nbconvert API service manager.
    /// A namespace for `BuildManager` statics.
    type [<AllowNullLiteral>] NbConvertManager =
        /// The server settings used to make API requests.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Get whether the application should be built.
        abstract getExportFormats: unit -> Promise<NbConvertManager.IExportFormats>

    /// The nbconvert API service manager.
    /// A namespace for `BuildManager` statics.
    type [<AllowNullLiteral>] NbConvertManagerStatic =
        /// Create a new nbconvert manager.
        [<Emit "new $0($1...)">] abstract Create: ?options: NbConvertManager.IOptions -> NbConvertManager

    module NbConvertManager =

        /// The instantiation options for a setting manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings used to make API requests.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

        /// A namespace for nbconvert API interfaces.
        type [<AllowNullLiteral>] IExportFormats =
            /// The list of supported export formats.
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> TypeLiteral_01 with get, set

        type [<AllowNullLiteral>] TypeLiteral_01 =
            abstract output_mimetype: string with get, set

    module NbConvert =

        /// The interface for the build manager.
        type [<AllowNullLiteral>] IManager =
            inherit NbConvertManager

module __session_default =
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __session_@phosphor_signaling.ISignal
    // type Kernel = __kernel_kernel.Kernel //__kernel_kernel.Kernel.Kernel
    // type KernelMessage = __kernel_kernel.KernelMessage //__kernel_kernel.Kernel.KernelMessage
    // type ServerConnection = Serverconnection.ServerConnection // __.ServerConnection
    // type Session = __session_session.Session

    type [<AllowNullLiteral>] IExports =
        abstract DefaultSession: DefaultSessionStatic

    /// Session object for accessing the session REST api. The session
    /// should be used to start kernels and then shut them down -- for
    /// all other operations, the kernel object should be used.
    /// The namespace for `DefaultSession` statics.
    type [<AllowNullLiteral>] DefaultSession =
        inherit __session_session.Session.ISession
        /// A signal emitted when the session is shut down.
        abstract terminated: ISignal<DefaultSession, unit>
        /// A signal emitted when the kernel changes.
        abstract kernelChanged: ISignal<DefaultSession, __session_session.Session.IKernelChangedArgs>
        /// A signal emitted when the kernel status changes.
        abstract statusChanged: ISignal<DefaultSession, __kernel_kernel.Kernel.Status>
        /// A signal emitted for a kernel messages.
        abstract iopubMessage: ISignal<DefaultSession, __kernel_messages.KernelMessage.IIOPubMessage>
        /// A signal emitted for an unhandled kernel message.
        abstract unhandledMessage: ISignal<DefaultSession, __kernel_messages.KernelMessage.IMessage>
        /// A signal emitted for any kernel message.
        /// 
        /// Note: The behavior is undefined if the message is modified
        /// during message handling. As such, it should be treated as read-only.
        abstract anyMessage: ISignal<DefaultSession, __kernel_kernel.Kernel.IAnyMessageArgs>
        /// A signal emitted when a session property changes.
        abstract propertyChanged: ISignal<DefaultSession, U3<string, string, string>>
        /// Get the session id.
        abstract id: string
        /// Get the session kernel object.
        /// 
        /// #### Notes
        /// This is a read-only property, and can be altered by [changeKernel].
        abstract kernel: __kernel_kernel.Kernel.IKernelConnection
        /// Get the session path.
        abstract path: string
        /// Get the session type.
        abstract ``type``: string
        /// Get the session name.
        abstract name: string
        /// Get the model associated with the session.
        abstract model: __session_session.Session.IModel
        /// The current status of the session.
        /// 
        /// #### Notes
        /// This is a delegate to the kernel status.
        abstract status: __kernel_kernel.Kernel.Status
        /// The server settings of the session.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the session has been disposed.
        abstract isDisposed: bool
        /// Clone the current session with a new clientId.
        abstract clone: unit -> __session_session.Session.ISession
        /// Update the session based on a session model from the server.
        abstract update: model: __session_session.Session.IModel -> unit
        /// Dispose of the resources held by the session.
        abstract dispose: unit -> unit
        /// <summary>Change the session path.</summary>
        /// <param name="path">- The new session path.</param>
        abstract setPath: path: string -> Promise<unit>
        /// Change the session name.
        abstract setName: name: string -> Promise<unit>
        /// Change the session type.
        abstract setType: ``type``: string -> Promise<unit>
        /// Change the kernel.
        abstract changeKernel: options: obj -> Promise<__kernel_kernel.Kernel.IKernelConnection>
        /// Kill the kernel and shutdown the session.
        abstract shutdown: unit -> Promise<unit>
        /// Create a new kernel connection and hook up to its events.
        /// 
        /// #### Notes
        /// This method is not meant to be subclassed.
        abstract setupKernel: model: __kernel_kernel.Kernel.IModel -> unit
        /// Handle to changes in the Kernel status.
        abstract onKernelStatus: sender: __kernel_kernel.Kernel.IKernel * state: __kernel_kernel.Kernel.Status -> unit
        /// Handle iopub kernel messages.
        abstract onIOPubMessage: sender: __kernel_kernel.Kernel.IKernel * msg: __kernel_messages.KernelMessage.IIOPubMessage -> unit
        /// Handle unhandled kernel messages.
        abstract onUnhandledMessage: sender: __kernel_kernel.Kernel.IKernel * msg: __kernel_messages.KernelMessage.IMessage -> unit
        /// Handle any kernel messages.
        abstract onAnyMessage: sender: __kernel_kernel.Kernel.IKernel * args: __kernel_kernel.Kernel.IAnyMessageArgs -> unit

    /// Session object for accessing the session REST api. The session
    /// should be used to start kernels and then shut them down -- for
    /// all other operations, the kernel object should be used.
    /// The namespace for `DefaultSession` statics.
    type [<AllowNullLiteral>] DefaultSessionStatic =
        /// Construct a new session.
        [<Emit "new $0($1...)">] abstract Create: options: __session_session.Session.IOptions * id: string * model: __kernel_kernel.Kernel.IModel -> DefaultSession

    module DefaultSession =

        type [<AllowNullLiteral>] IExports =
            /// List the running sessions.
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<__session_session.Session.IModel>>
            /// Start a new session.
            abstract startNew: options: __session_session.Session.IOptions -> Promise<__session_session.Session.ISession>
            /// Find a session by id.
            abstract findById: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<__session_session.Session.IModel>
            /// Find a session by path.
            abstract findByPath: path: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<__session_session.Session.IModel>
            /// Connect to a running session.
            abstract connectTo: model: __session_session.Session.IModel * ?settings: Serverconnection.ServerConnection.ISettings -> __session_session.Session.ISession
            /// Shut down a session by id.
            abstract shutdown: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// <summary>Shut down all sessions.</summary>
            /// <param name="settings">- The server settings to use.</param>
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

module __session_manager =
    type Poll =  JupyterlabCoreutils.Poll.Poll //__session_@jupyterlab_coreutils.Poll
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> //__session_@phosphor_algorithm.IIterator
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __session_@phosphor_signaling.ISignal
    // type Kernel = __kernel_kernel.Kernel //__kernel_kernel.Kernel.Kernel
    // type ServerConnection = Serverconnection.ServerConnection
    // type Session = __session_session.Session

    type [<AllowNullLiteral>] IExports =
        abstract SessionManager: SessionManagerStatic

    /// An implementation of a session manager.
    /// The namespace for `SessionManager` class statics.
    type [<AllowNullLiteral>] SessionManager =
        inherit __session_session.Session.IManager
        /// A signal emitted when the kernel specs change.
        abstract specsChanged: ISignal<SessionManager, __kernel_kernel.Kernel.ISpecModels>
        /// A signal emitted when the running sessions change.
        abstract runningChanged: ISignal<SessionManager, ResizeArray<__session_session.Session.IModel>>
        /// A signal emitted when there is a connection failure.
        abstract connectionFailure: ISignal<SessionManager, Error>
        /// Test whether the manager is disposed.
        abstract isDisposed: bool
        /// The server settings of the manager.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Get the most recently fetched kernel specs.
        abstract specs: __kernel_kernel.Kernel.ISpecModels option
        /// Test whether the manager is ready.
        abstract isReady: bool
        /// A promise that fulfills when the manager is ready.
        abstract ready: Promise<unit>
        /// Dispose of the resources used by the manager.
        abstract dispose: unit -> unit
        /// Create an iterator over the most recent running sessions.
        abstract running: unit -> IIterator<__session_session.Session.IModel>
        /// Force a refresh of the specs from the server.
        abstract refreshSpecs: unit -> Promise<unit>
        /// Force a refresh of the running sessions.
        abstract refreshRunning: unit -> Promise<unit>
        /// <summary>Start a new session.  See also [[startNewSession]].</summary>
        /// <param name="options">- Overrides for the default options, must include a `path`.</param>
        abstract startNew: options: __session_session.Session.IOptions -> Promise<__session_session.Session.ISession>
        /// <summary>Find a session associated with a path and stop it if it is the only session
        /// using that kernel.</summary>
        /// <param name="path">- The path in question.</param>
        abstract stopIfNeeded: path: string -> Promise<unit>
        /// Find a session by id.
        abstract findById: id: string -> Promise<__session_session.Session.IModel>
        /// Find a session by path.
        abstract findByPath: path: string -> Promise<__session_session.Session.IModel>
        abstract connectTo: model: __session_session.Session.IModel -> __session_session.Session.ISession
        /// Shut down a session by id.
        abstract shutdown: id: string -> Promise<unit>
        /// Shut down all sessions.
        abstract shutdownAll: unit -> Promise<unit>
        /// Execute a request to the server to poll running kernels and update state.
        abstract requestRunning: unit -> Promise<unit>
        /// Execute a request to the server to poll specs and update state.
        abstract requestSpecs: unit -> Promise<unit>

    /// An implementation of a session manager.
    /// The namespace for `SessionManager` class statics.
    type [<AllowNullLiteral>] SessionManagerStatic =
        /// <summary>Construct a new session manager.</summary>
        /// <param name="options">- The default options for each session.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: SessionManager.IOptions -> SessionManager

    module SessionManager =

        /// The options used to initialize a SessionManager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings for the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// When the manager stops polling the API. Defaults to `when-hidden`.
            abstract standby: JupyterlabCoreutils.Poll.Poll.Standby option with get, set

module __session_session =
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> //__session_@phosphor_algorithm.IIterator
    type JSONObject = PhosphorCoreutils.JSONObject //__session_@phosphor_coreutils.JSONObject
    type IDisposable = PhosphorDisposable.IDisposable //__session_@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __session_@phosphor_signaling.ISignal
    // type Kernel = __kernel_kernel.Kernel // __kernel_kernel.Kernel.Kernel
    // type KernelMessage = __kernel_kernel.KernelMessage //__kernel_kernel.Kernel.KernelMessage
    // type ServerConnection = Serverconnection.ServerConnection // __.ServerConnection

    module Session =

        type [<AllowNullLiteral>] IExports =
            /// <summary>List the running sessions.</summary>
            /// <param name="settings">- The server settings to use for the request.</param>
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<Session.IModel>>
            /// <summary>Start a new session.</summary>
            /// <param name="options">- The options used to start the session.</param>
            abstract startNew: options: Session.IOptions -> Promise<ISession>
            /// <summary>Find a session by id.</summary>
            /// <param name="id">- The id of the target session.</param>
            /// <param name="settings">- The server settings.</param>
            abstract findById: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<Session.IModel>
            /// <summary>Find a session by path.</summary>
            /// <param name="path">- The path of the target session.</param>
            /// <param name="settings">: The server settings.</param>
            abstract findByPath: path: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<Session.IModel>
            /// <summary>Connect to a running session.</summary>
            /// <param name="model">- The model of the target session.</param>
            abstract connectTo: model: Session.IModel * ?settings: Serverconnection.ServerConnection.ISettings -> ISession
            /// <summary>Shut down a session by id.</summary>
            /// <param name="id">- The id of the target session.</param>
            /// <param name="settings">- The server settings.</param>
            abstract shutdown: id: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// Shut down all sessions.
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

        /// Interface of a session object.
        type [<AllowNullLiteral>] ISession =
            inherit IDisposable //ISession
            /// A signal emitted when the session is shut down.
            abstract terminated: ISignal<ISession, unit> with get, set
            /// A signal emitted when the kernel changes.
            abstract kernelChanged: ISignal<ISession, IKernelChangedArgs> with get, set
            /// A signal emitted when the session status changes.
            abstract statusChanged: ISignal<ISession, __kernel_kernel.Kernel.Status> with get, set
            /// A signal emitted when a session property changes.
            abstract propertyChanged: ISignal<ISession, U3<string, string, string>>
            /// A signal emitted for iopub kernel messages.
            abstract iopubMessage: ISignal<ISession, __kernel_messages.KernelMessage.IIOPubMessage> with get, set
            /// A signal emitted for unhandled kernel message.
            abstract unhandledMessage: ISignal<ISession, __kernel_messages.KernelMessage.IMessage> with get, set
            /// A signal emitted for any kernel message.
            /// 
            /// Note: The behavior is undefined if the message is modified
            /// during message handling. As such, it should be treated as read-only.
            abstract anyMessage: ISignal<ISession, __kernel_kernel.Kernel.IAnyMessageArgs> with get, set
            /// Unique id of the session.
            abstract id: string
            /// The current path associated with the session.
            abstract path: string
            /// The current name associated with the session.
            abstract name: string
            /// The type of the session.
            abstract ``type``: string
            /// The server settings of the session.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// The model associated with the session.
            abstract model: Session.IModel
            /// The kernel.
            /// 
            /// #### Notes
            /// This is a read-only property, and can be altered by [changeKernel].
            abstract kernel: __kernel_kernel.Kernel.IKernelConnection
            /// The current status of the session.
            /// 
            /// #### Notes
            /// This is a delegate to the kernel status.
            abstract status: __kernel_kernel.Kernel.Status
            /// <summary>Change the session path.</summary>
            /// <param name="path">- The new session path.</param>
            abstract setPath: path: string -> Promise<unit>
            /// Change the session name.
            abstract setName: name: string -> Promise<unit>
            /// Change the session type.
            abstract setType: ``type``: string -> Promise<unit>
            /// <summary>Change the kernel.</summary>
            /// <param name="options">- The name or id of the new kernel.</param>
            abstract changeKernel: options: obj -> Promise<__kernel_kernel.Kernel.IKernelConnection>
            /// Kill the kernel and shutdown the session.
            abstract shutdown: unit -> Promise<unit>

        /// The session initialization options.
        type [<AllowNullLiteral>] IOptions =
            /// The path (not including name) to the session.
            abstract path: string with get, set
            /// The name of the session.
            abstract name: string option with get, set
            /// The type of the session.
            abstract ``type``: string option with get, set
            /// The type of kernel (e.g. python3).
            abstract kernelName: string option with get, set
            /// The id of an existing kernel.
            abstract kernelId: string option with get, set
            /// The server settings.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// The username of the session client.
            abstract username: string option with get, set
            /// The unique identifier for the session client.
            abstract clientId: string option with get, set

        /// An arguments object for the kernel changed signal.
        type [<AllowNullLiteral>] IKernelChangedArgs =
            /// The old kernel.
            abstract oldValue: __kernel_kernel.Kernel.IKernelConnection option with get, set
            /// The new kernel.
            abstract newValue: __kernel_kernel.Kernel.IKernelConnection option with get, set

        /// Object which manages session instances.
        /// 
        /// #### Notes
        /// The manager is responsible for maintaining the state of running
        /// sessions and the initial fetch of kernel specs.
        type [<AllowNullLiteral>] IManager =
            inherit IDisposable
            /// A signal emitted when the kernel specs change.
            abstract specsChanged: ISignal<IManager, __kernel_kernel.Kernel.ISpecModels> with get, set
            /// A signal emitted when the running sessions change.
            abstract runningChanged: ISignal<IManager, ResizeArray<IModel>> with get, set
            /// A signal emitted when there is a connection failure.
            abstract connectionFailure: ISignal<IManager, Serverconnection.ServerConnection.NetworkError> with get, set
            /// The server settings for the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// The cached kernel specs.
            /// 
            /// #### Notes
            /// This value will be null until the manager is ready.
            abstract specs: __kernel_kernel.Kernel.ISpecModels option
            /// Test whether the manager is ready.
            abstract isReady: bool
            /// A promise that is fulfilled when the manager is ready.
            abstract ready: Promise<unit>
            /// Create an iterator over the known running sessions.
            abstract running: unit -> IIterator<IModel>
            /// <summary>Start a new session.</summary>
            /// <param name="options">- The session options to use.</param>
            abstract startNew: options: IOptions -> Promise<ISession>
            /// <summary>Find a session by id.</summary>
            /// <param name="id">- The id of the target session.</param>
            abstract findById: id: string -> Promise<IModel>
            /// <summary>Find a session by path.</summary>
            /// <param name="path">- The path of the target session.</param>
            abstract findByPath: path: string -> Promise<IModel>
            /// <summary>Connect to a running session.</summary>
            /// <param name="model">- The model of the target session.</param>
            abstract connectTo: model: Session.IModel -> ISession
            /// <summary>Shut down a session by id.</summary>
            /// <param name="id">- The id of the target kernel.</param>
            abstract shutdown: id: string -> Promise<unit>
            /// Shut down all sessions.
            abstract shutdownAll: unit -> Promise<unit>
            /// Force a refresh of the specs from the server.
            abstract refreshSpecs: unit -> Promise<unit>
            /// Force a refresh of the running sessions.
            abstract refreshRunning: unit -> Promise<unit>
            /// <summary>Find a session associated with a path and stop it is the only session
            /// using that kernel.</summary>
            /// <param name="path">- The path in question.</param>
            abstract stopIfNeeded: path: string -> Promise<unit>

        /// The session model used by the server.
        /// 
        /// #### Notes
        /// See the [Jupyter Notebook API](http://petstore.swagger.io/?url=https://raw.githubusercontent.com/jupyter/notebook/master/notebook/services/api/api.yaml#!/sessions).
        type [<AllowNullLiteral>] IModel =
            inherit JSONObject
            /// The unique identifier for the session client.
            abstract id: string
            abstract name: string
            abstract path: string
            abstract ``type``: string
            abstract kernel: __kernel_kernel.Kernel.IModel

module __session_validate =
    // type Session = __session_session.Session

    type [<AllowNullLiteral>] IExports =
        /// Validate an `Session.IModel` object.
        abstract validateModel: data: obj option -> __session_session.Session.IModel

module __setting_index =
    // type DataConnector = JupyterlabCoreutils.Dataconnector // __setting_@jupyterlab_coreutils.DataConnector
    type ISettingRegistry =  JupyterlabCoreutils.Tokens.ISettingRegistry  //__setting_@jupyterlab_coreutils.ISettingRegistry
    // type ServerConnection = Serverconnection.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract SettingManager: SettingManagerStatic

    /// The settings API service manager.
    /// A namespace for `SettingManager` statics.
    type [<AllowNullLiteral>] SettingManager =
        inherit JupyterlabCoreutils.Dataconnector.DataConnector<JupyterlabCoreutils.Tokens.ISettingRegistry.IPlugin, string>
        /// The server settings used to make API requests.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// <summary>Fetch a plugin's settings.</summary>
        /// <param name="id">- The plugin's ID.</param>
        abstract fetch: id: string -> Promise<JupyterlabCoreutils.Tokens.ISettingRegistry.IPlugin>
        /// Fetch the list of all plugin setting bundles.
        abstract list: unit -> Promise<TypeLiteral_01>
        /// <summary>Save a plugin's settings.</summary>
        /// <param name="id">- The plugin's ID.</param>
        /// <param name="raw">- The user setting values as a raw string of JSON with comments.</param>
        abstract save: id: string * raw: string -> Promise<unit>

    /// The settings API service manager.
    /// A namespace for `SettingManager` statics.
    type [<AllowNullLiteral>] SettingManagerStatic =
        /// Create a new setting manager.
        [<Emit "new $0($1...)">] abstract Create: ?options: SettingManager.IOptions -> SettingManager

    module SettingManager =

        /// The instantiation options for a setting manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings used to make API requests.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

    module Setting =

        /// The interface for the setting system manager.
        type [<AllowNullLiteral>] IManager =
            inherit SettingManager

    type [<AllowNullLiteral>] TypeLiteral_01 =
        abstract ids: ResizeArray<string> with get, set
        abstract values: ResizeArray<JupyterlabCoreutils.Tokens.ISettingRegistry.IPlugin> with get, set

module __terminal_default =
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __terminal_@phosphor_signaling.ISignal
    type Signal<'T,'U>  = PhosphorSignaling.Signal<'T,'U>  // __terminal_@phosphor_signaling.Signal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection
    // type TerminalSession = __terminal_terminal.TerminalSession

    type [<AllowNullLiteral>] IExports =
        abstract DefaultTerminalSession: DefaultTerminalSessionStatic

    /// An implementation of a terminal interface.
    /// The static namespace for `DefaultTerminalSession`.
    type [<AllowNullLiteral>] DefaultTerminalSession =
        inherit __terminal_terminal.TerminalSession.ISession
        /// A signal emitted when the session is shut down.
        abstract terminated: Signal<DefaultTerminalSession, unit>
        /// A signal emitted when a message is received from the server.
        abstract messageReceived: ISignal<DefaultTerminalSession, __terminal_terminal.TerminalSession.IMessage>
        /// Get the name of the terminal session.
        abstract name: string
        /// Get the model for the terminal session.
        abstract model: __terminal_terminal.TerminalSession.IModel
        /// The server settings for the session.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the session is ready.
        abstract isReady: bool
        /// A promise that fulfills when the session is ready.
        abstract ready: Promise<unit>
        /// Test whether the session is disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the session.
        abstract dispose: unit -> unit
        /// Send a message to the terminal session.
        abstract send: message: __terminal_terminal.TerminalSession.IMessage -> unit
        /// Reconnect to the terminal.
        abstract reconnect: unit -> Promise<unit>
        /// Shut down the terminal session.
        abstract shutdown: unit -> Promise<unit>
        /// Clone the current session object.
        abstract clone: unit -> __terminal_terminal.TerminalSession.ISession

    /// An implementation of a terminal interface.
    /// The static namespace for `DefaultTerminalSession`.
    type [<AllowNullLiteral>] DefaultTerminalSessionStatic =
        /// Construct a new terminal session.
        [<Emit "new $0($1...)">] abstract Create: name: string * ?options: __terminal_terminal.TerminalSession.IOptions -> DefaultTerminalSession

    module DefaultTerminalSession =

        type [<AllowNullLiteral>] IExports =
            /// Whether the terminal service is available.
            abstract isAvailable: unit -> bool
            /// <summary>Start a new terminal session.</summary>
            /// <param name="options">- The session options to use.</param>
            abstract startNew: ?options: __terminal_terminal.TerminalSession.IOptions -> Promise<__terminal_terminal.TerminalSession.ISession>
            abstract connectTo: name: string * ?options: __terminal_terminal.TerminalSession.IOptions -> Promise<__terminal_terminal.TerminalSession.ISession>
            /// <summary>List the running terminal sessions.</summary>
            /// <param name="settings">- The server settings to use.</param>
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<__terminal_terminal.TerminalSession.IModel>>
            /// <summary>Shut down a terminal session by name.</summary>
            /// <param name="name">- The name of the target session.</param>
            /// <param name="settings">- The server settings to use.</param>
            abstract shutdown: name: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// <summary>Shut down all terminal sessions.</summary>
            /// <param name="settings">- The server settings to use.</param>
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

module __terminal_manager =
    type Poll = JupyterlabCoreutils.Poll.Poll // __terminal_@jupyterlab_coreutils.Poll
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __terminal_@phosphor_algorithm.IIterator
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __terminal_@phosphor_signaling.ISignal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection
    // type TerminalSession = __terminal_terminal.TerminalSession

    type [<AllowNullLiteral>] IExports =
        abstract TerminalManager: TerminalManagerStatic

    /// A terminal session manager.
    /// The namespace for TerminalManager statics.
    type [<AllowNullLiteral>] TerminalManager =
        inherit __terminal_terminal.TerminalSession.IManager
        /// A signal emitted when the running terminals change.
        abstract runningChanged: ISignal<TerminalManager, ResizeArray<__terminal_terminal.TerminalSession.IModel>>
        /// A signal emitted when there is a connection failure.
        abstract connectionFailure: ISignal<TerminalManager, Error>
        /// Test whether the terminal manager is disposed.
        abstract isDisposed: bool
        /// The server settings of the manager.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// Test whether the manager is ready.
        abstract isReady: bool
        /// Dispose of the resources used by the manager.
        abstract dispose: unit -> unit
        /// A promise that fulfills when the manager is ready.
        abstract ready: Promise<unit>
        /// Whether the terminal service is available.
        abstract isAvailable: unit -> bool
        /// Create an iterator over the most recent running terminals.
        abstract running: unit -> IIterator<__terminal_terminal.TerminalSession.IModel>
        /// <summary>Create a new terminal session.</summary>
        /// <param name="options">- The options used to connect to the session.</param>
        abstract startNew: ?options: __terminal_terminal.TerminalSession.IOptions -> Promise<__terminal_terminal.TerminalSession.ISession>
        abstract connectTo: name: string * ?options: __terminal_terminal.TerminalSession.IOptions -> Promise<__terminal_terminal.TerminalSession.ISession>
        /// Force a refresh of the running sessions.
        /// 
        /// #### Notes
        /// This is intended to be called only in response to a user action,
        /// since the manager maintains its internal state.
        abstract refreshRunning: unit -> Promise<unit>
        /// Shut down a terminal session by name.
        abstract shutdown: name: string -> Promise<unit>
        /// Shut down all terminal sessions.
        abstract shutdownAll: unit -> Promise<unit>
        /// Execute a request to the server to poll running terminals and update state.
        abstract requestRunning: unit -> Promise<unit>

    /// A terminal session manager.
    /// The namespace for TerminalManager statics.
    type [<AllowNullLiteral>] TerminalManagerStatic =
        /// Construct a new terminal manager.
        [<Emit "new $0($1...)">] abstract Create: ?options: TerminalManager.IOptions -> TerminalManager

    module TerminalManager =

        /// The options used to initialize a terminal manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings used by the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set
            /// When the manager stops polling the API. Defaults to `when-hidden`.
            abstract standby: JupyterlabCoreutils.Poll.Poll.Standby option with get, set

module __terminal_terminal =
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __terminal_@phosphor_algorithm.IIterator
    type JSONPrimitive = PhosphorCoreutils.JSONPrimitive // __terminal_@phosphor_coreutils.JSONPrimitive
    type JSONObject = PhosphorCoreutils.JSONObject //__terminal_@phosphor_coreutils.JSONObject
    type IDisposable = PhosphorDisposable.IDisposable //__terminal_@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> //__terminal_@phosphor_signaling.ISignal
    // type ServerConnection = Serverconnection.ServerConnection //__.ServerConnection

    module TerminalSession =

        type [<AllowNullLiteral>] IExports =
            /// Test whether the terminal service is available.
            abstract isAvailable: unit -> bool
            /// <summary>Start a new terminal session.</summary>
            /// <param name="options">- The session options to use.</param>
            abstract startNew: ?options: IOptions -> Promise<ISession>
            abstract connectTo: name: string * ?options: IOptions -> Promise<ISession>
            /// <summary>List the running terminal sessions.</summary>
            /// <param name="settings">- The server settings to use.</param>
            abstract listRunning: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<ResizeArray<IModel>>
            /// <summary>Shut down a terminal session by name.</summary>
            /// <param name="name">- The name of the target session.</param>
            /// <param name="settings">- The server settings to use.</param>
            abstract shutdown: name: string * ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>
            /// Shut down all terminal sessions.
            abstract shutdownAll: ?settings: Serverconnection.ServerConnection.ISettings -> Promise<unit>

        /// An interface for a terminal session.
        type [<AllowNullLiteral>] ISession =
            inherit IDisposable
            /// A signal emitted when the session is shut down.
            abstract terminated: ISignal<ISession, unit> with get, set
            /// A signal emitted when a message is received from the server.
            abstract messageReceived: ISignal<ISession, IMessage> with get, set
            /// Get the name of the terminal session.
            abstract name: string
            /// The model associated with the session.
            abstract model: IModel
            /// The server settings for the session.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// Test whether the session is ready.
            abstract isReady: bool
            /// A promise that fulfills when the session is initially ready.
            abstract ready: Promise<unit>
            /// Send a message to the terminal session.
            abstract send: message: IMessage -> unit
            /// Reconnect to the terminal.
            abstract reconnect: unit -> Promise<unit>
            /// Shut down the terminal session.
            abstract shutdown: unit -> Promise<unit>

        /// The options for initializing a terminal session object.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings for the session.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

        /// The server model for a terminal session.
        type [<AllowNullLiteral>] IModel =
            inherit JSONObject
            /// The name of the terminal session.
            abstract name: string

        /// A message from the terminal session.
        type [<AllowNullLiteral>] IMessage =
            /// The type of the message.
            abstract ``type``: MessageType
            /// The content of the message.
            abstract content: ResizeArray<JSONPrimitive> option

        type [<StringEnum>] [<RequireQualifiedAccess>] MessageType =
            | Stdout
            | Disconnect
            | Set_size
            | Stdin

        /// The interface for a terminal manager.
        /// 
        /// #### Notes
        /// The manager is responsible for maintaining the state of running
        /// terminal sessions.
        type [<AllowNullLiteral>] IManager =
            inherit IDisposable
            /// A signal emitted when the running terminals change.
            abstract runningChanged: ISignal<IManager, ResizeArray<IModel>> with get, set
            /// A signal emitted when there is a connection failure.
            abstract connectionFailure: ISignal<IManager, Serverconnection.ServerConnection.NetworkError> with get, set
            /// The server settings for the manager.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings
            /// Test whether the manager is ready.
            abstract isReady: bool
            /// A promise that fulfills when the manager is ready.
            abstract ready: Promise<unit>
            /// Whether the terminal service is available.
            abstract isAvailable: unit -> bool
            /// Create an iterator over the known running terminals.
            abstract running: unit -> IIterator<IModel>
            /// <summary>Create a new terminal session.</summary>
            /// <param name="options">- The options used to create the session.</param>
            abstract startNew: ?options: IOptions -> Promise<ISession>
            abstract connectTo: name: string -> Promise<ISession>
            /// <summary>Shut down a terminal session by name.</summary>
            /// <param name="name">- The name of the terminal session.</param>
            abstract shutdown: name: string -> Promise<unit>
            /// Shut down all terminal sessions.
            abstract shutdownAll: unit -> Promise<unit>
            /// Force a refresh of the running terminal sessions.
            abstract refreshRunning: unit -> Promise<unit>

module __workspace_index =
    type DataConnector<'T> = JupyterlabCoreutils.Dataconnector.DataConnector<'T> // __workspace_@jupyterlab_coreutils.DataConnector
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __workspace_@phosphor_coreutils.ReadonlyJSONObject
    // type ServerConnection = Serverconnection.ServerConnection

    type [<AllowNullLiteral>] IExports =
        abstract WorkspaceManager: WorkspaceManagerStatic

    /// The workspaces API service manager.
    /// A namespace for `WorkspaceManager` statics.
    type [<AllowNullLiteral>] WorkspaceManager =
        inherit DataConnector<Workspace.IWorkspace>
        /// The server settings used to make API requests.
        abstract serverSettings: Serverconnection.ServerConnection.ISettings
        /// <summary>Fetch a workspace.</summary>
        /// <param name="id">- The workspaces's ID.</param>
        abstract fetch: id: string -> Promise<Workspace.IWorkspace>
        /// Fetch the list of workspace IDs that exist on the server.
        abstract list: unit -> Promise<TypeLiteral_02>
        /// <summary>Remove a workspace from the server.</summary>
        /// <param name="id">- The workspaces's ID.</param>
        abstract remove: id: string -> Promise<unit>
        /// <summary>Save a workspace.</summary>
        /// <param name="id">- The workspace's ID.</param>
        /// <param name="workspace">- The workspace being saved.</param>
        abstract save: id: string * workspace: Workspace.IWorkspace -> Promise<unit>

    /// The workspaces API service manager.
    /// A namespace for `WorkspaceManager` statics.
    type [<AllowNullLiteral>] WorkspaceManagerStatic =
        /// Create a new workspace manager.
        [<Emit "new $0($1...)">] abstract Create: ?options: WorkspaceManager.IOptions -> WorkspaceManager

    module WorkspaceManager =

        /// The instantiation options for a workspace manager.
        type [<AllowNullLiteral>] IOptions =
            /// The server settings used to make API requests.
            abstract serverSettings: Serverconnection.ServerConnection.ISettings option with get, set

    module Workspace =

        /// The interface for the workspace API manager.
        type [<AllowNullLiteral>] IManager =
            inherit WorkspaceManager

        /// The interface describing a workspace API response.
        type [<AllowNullLiteral>] IWorkspace =
            /// The workspace data.
            abstract data: ReadonlyJSONObject with get, set
            /// The metadata for a workspace.
            abstract metadata: TypeLiteral_01 with get, set

        type [<AllowNullLiteral>] TypeLiteral_01 =
            /// The workspace ID.
            abstract id: string with get, set

    type [<AllowNullLiteral>] TypeLiteral_02 =
        abstract ids: ResizeArray<string> with get, set
        abstract values: ResizeArray<Workspace.IWorkspace> with get, set
