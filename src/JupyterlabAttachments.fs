// ts2fable 0.0.0
module rec JupyterlabAttachments
open System
open Fable.Core
open Fable.Core.JS

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo end

module Model =
    // type nbformat = __@jupyterlab_coreutils.nbformat
    type IObservableMap<'T> = JupyterlabObservables.Observablemap.IObservableMap<'T> // __@jupyterlab_observables.IObservableMap
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    type IAttachmentModel = JupyterlabRendermime.Attachmentmodel.IAttachmentModel // __@jupyterlab_rendermime.IAttachmentModel
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal

    type [<AllowNullLiteral>] IExports =
        abstract AttachmentsModel: AttachmentsModelStatic
        abstract AttachmentsResolver: AttachmentsResolverStatic

    /// The model for attachments.
    /// The namespace for IAttachmentsModel interfaces.
    type [<AllowNullLiteral>] IAttachmentsModel =
        inherit IDisposable
        /// A signal emitted when the model state changes.
        abstract stateChanged: ISignal<IAttachmentsModel, unit>
        /// A signal emitted when the model changes.
        abstract changed: ISignal<IAttachmentsModel, IAttachmentsModel.ChangedArgs>
        /// The length of the items in the model.
        abstract length: float
        /// The keys of the attachments in the model.
        abstract keys: ReadonlyArray<string>
        /// The attachment content factory used by the model.
        abstract contentFactory: IAttachmentsModel.IContentFactory
        /// Whether the specified key is set.
        abstract has: key: string -> bool
        /// Get an item for the specified key.
        abstract get: key: string -> IAttachmentModel
        /// Set the value of the specified key.
        abstract set: key: string * attachment: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle -> unit
        /// Remove the attachment whose name is the specified key.
        /// Note that this is optional only until Jupyterlab 2.0 release.
        abstract remove: (string -> unit) option with get, set
        /// Clear all of the attachments.
        abstract clear: unit -> unit
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// This will clear any existing data.
        abstract fromJSON: values: JupyterlabCoreutils.Nbformat.Nbformat.IAttachments -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IAttachments

    module IAttachmentsModel =

        /// The options used to create a attachments model.
        type [<AllowNullLiteral>] IOptions =
            /// The initial values for the model.
            abstract values: JupyterlabCoreutils.Nbformat.Nbformat.IAttachments option with get, set
            /// The attachment content factory used by the model.
            /// 
            /// If not given, a default factory will be used.
            abstract contentFactory: IContentFactory option with get, set
            /// An optional IModelDB to store the attachments model.
            abstract modelDB: IModelDB option with get, set

        type ChangedArgs =
            JupyterlabObservables.Observablemap.IObservableMap.IChangedArgs<IAttachmentModel>

        /// The interface for an attachment content factory.
        type [<AllowNullLiteral>] IContentFactory =
            /// Create an attachment model.
            abstract createAttachmentModel: options: JupyterlabRendermime.Attachmentmodel.IAttachmentModel.IOptions -> IAttachmentModel

    /// The default implementation of the IAttachmentsModel.
    /// The namespace for AttachmentsModel class statics.
    type [<AllowNullLiteral>] AttachmentsModel =
        inherit IAttachmentsModel
        /// A signal emitted when the model state changes.
        abstract stateChanged: ISignal<IAttachmentsModel, unit>
        /// A signal emitted when the model changes.
        abstract changed: ISignal<AttachmentsModel, IAttachmentsModel.ChangedArgs>
        /// The keys of the attachments in the model.
        abstract keys: ReadonlyArray<string>
        /// Get the length of the items in the model.
        abstract length: float
        /// The attachment content factory used by the model.
        abstract contentFactory: IAttachmentsModel.IContentFactory
        /// Test whether the model is disposed.
        abstract isDisposed: bool
        /// Dispose of the resources used by the model.
        abstract dispose: unit -> unit
        /// Whether the specified key is set.
        abstract has: key: string -> bool
        /// Get an item at the specified key.
        abstract get: key: string -> IAttachmentModel
        /// Set the value at the specified key.
        abstract set: key: string * value: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle -> unit
        /// Remove the attachment whose name is the specified key
        abstract remove: key: string -> unit
        /// Clear all of the attachments.
        abstract clear: unit -> unit
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// This will clear any existing data.
        abstract fromJSON: values: JupyterlabCoreutils.Nbformat.Nbformat.IAttachments -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IAttachments

    /// The default implementation of the IAttachmentsModel.
    /// The namespace for AttachmentsModel class statics.
    type [<AllowNullLiteral>] AttachmentsModelStatic =
        /// Construct a new observable outputs instance.
        [<Emit "new $0($1...)">] abstract Create: ?options: IAttachmentsModel.IOptions -> AttachmentsModel

    module AttachmentsModel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// The default implementation of a `IAttachemntsModel.IContentFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit IAttachmentsModel.IContentFactory
            /// Create an attachment model.
            abstract createAttachmentModel: options: JupyterlabRendermime.Attachmentmodel.IAttachmentModel.IOptions -> IAttachmentModel

        /// The default implementation of a `IAttachemntsModel.IContentFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

    /// A resolver for cell attachments 'attchment:filename'.
    /// 
    /// Will resolve to a data: url.
    /// The namespace for `AttachmentsResolver` class statics.
    type [<AllowNullLiteral>] AttachmentsResolver =
        inherit JupyterlabRendermimeInterfaces.IRenderMime.IResolver
        /// Resolve a relative url to a correct server path.
        abstract resolveUrl: url: string -> Promise<string>
        /// Get the download url of a given absolute server path.
        /// 
        /// #### Notes
        /// The returned URL may include a query parameter.
        abstract getDownloadUrl: path: string -> Promise<string>
        /// Whether the URL should be handled by the resolver
        /// or not.
        abstract isLocal: url: string -> bool

    /// A resolver for cell attachments 'attchment:filename'.
    /// 
    /// Will resolve to a data: url.
    /// The namespace for `AttachmentsResolver` class statics.
    type [<AllowNullLiteral>] AttachmentsResolverStatic =
        /// Create an attachments resolver object.
        [<Emit "new $0($1...)">] abstract Create: options: AttachmentsResolver.IOptions -> AttachmentsResolver

    module AttachmentsResolver =

        /// The options used to create an AttachmentsResolver.
        type [<AllowNullLiteral>] IOptions =
            /// The attachments model to resolve against.
            abstract model: IAttachmentsModel with get, set
            /// A parent resolver to use if the URL/path is not for an attachment.
            abstract parent: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option with get, set
