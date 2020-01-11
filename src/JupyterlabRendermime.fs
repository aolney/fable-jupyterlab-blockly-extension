// ts2fable 0.0.0
module rec JupyterlabRendermime
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

//amo: typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo: end typescript

module Attachmentmodel =
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject //  __@phosphor_coreutils.ReadonlyJSONObject
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U>  // __@phosphor_signaling.ISignal

    type [<AllowNullLiteral>] IExports =
        abstract AttachmentModel: AttachmentModelStatic

    /// The interface for an attachment model.
    /// The namespace for IAttachmentModel sub-interfaces.
    type [<AllowNullLiteral>] IAttachmentModel =
        inherit JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel
        /// A signal emitted when the attachment model changes.
        abstract changed: ISignal<IAttachmentModel, unit>
        /// Dispose of the resources used by the attachment model.
        abstract dispose: unit -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle

    module IAttachmentModel =

        /// The options used to create a notebook attachment model.
        type [<AllowNullLiteral>] IOptions =
            /// The raw attachment value.
            abstract value: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle with get, set

    /// The default implementation of a notebook attachment model.
    /// The namespace for AttachmentModel statics.
    type [<AllowNullLiteral>] AttachmentModel =
        inherit IAttachmentModel
        /// A signal emitted when the attachment model changes.
        abstract changed: ISignal<AttachmentModel, unit>
        /// Dispose of the resources used by the attachment model.
        abstract dispose: unit -> unit
        /// The data associated with the model.
        abstract data: ReadonlyJSONObject
        /// The metadata associated with the model.
        abstract metadata: ReadonlyJSONObject
        /// Set the data associated with the model.
        /// 
        /// #### Notes
        /// Depending on the implementation of the mime model,
        /// this call may or may not have deferred effects,
        abstract setData: options: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel.ISetDataOptions -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle
        abstract trusted: bool

    /// The default implementation of a notebook attachment model.
    /// The namespace for AttachmentModel statics.
    type [<AllowNullLiteral>] AttachmentModelStatic =
        /// Construct a new attachment model.
        [<Emit "new $0($1...)">] abstract Create: options: IAttachmentModel.IOptions -> AttachmentModel

    module AttachmentModel =

        type [<AllowNullLiteral>] IExports =
            /// Get the data for an attachment.
            abstract getData: bundle: JupyterlabCoreutils.Nbformat.Nbformat.IMimeBundle -> JSONObject

module Factories =
    // type IRenderMime = __@jupyterlab_rendermime_interfaces.IRenderMime

    type [<AllowNullLiteral>] IExports =
        abstract htmlRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract imageRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract latexRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract markdownRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract svgRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract textRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract javaScriptRendererFactory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory
        abstract standardRendererFactories: ReadonlyArray<JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory>

module Latex =

    type [<AllowNullLiteral>] IExports =
        /// Break up the text into its component parts and search
        ///    through them for math delimiters, braces, linebreaks, etc.
        /// Math delimiters must match and braces must balance.
        /// Don't allow math to pass through a double linebreak
        ///    (which will be a paragraph).
        abstract removeMath: text: string -> RemoveMathReturn
        /// Put back the math strings that were saved,
        /// and clear the math array (no need to keep it around).
        abstract replaceMath: text: string * math: ResizeArray<string> -> string

    type [<AllowNullLiteral>] RemoveMathReturn =
        abstract text: string with get, set
        abstract math: ResizeArray<string> with get, set

module Mimemodel =
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    // type IRenderMime = __@jupyterlab_rendermime_interfaces.IRenderMime

    type [<AllowNullLiteral>] IExports =
        abstract MimeModel: MimeModelStatic

    [<Import("*","@jupyterlab/rendermime")>]
    let Types:IExports = jsNative


    /// The default mime model implementation.
    /// The namespace for MimeModel class statics.
    type [<AllowNullLiteral>] MimeModel =
        inherit JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel
        /// Whether the model is trusted.
        abstract trusted: bool
        /// The data associated with the model.
        abstract data: ReadonlyJSONObject
        /// The metadata associated with the model.
        abstract metadata: ReadonlyJSONObject
        /// Set the data associated with the model.
        /// 
        /// #### Notes
        /// Depending on the implementation of the mime model,
        /// this call may or may not have deferred effects,
        abstract setData: options: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel.ISetDataOptions -> unit

    /// The default mime model implementation.
    /// The namespace for MimeModel class statics.
    type [<AllowNullLiteral>] MimeModelStatic =
        /// Construct a new mime model.
        [<Emit "new $0($1...)">] abstract Create: ?options: Mimemodel.MimeModel.IOptions -> MimeModel

    module MimeModel =

        /// The options used to create a mime model.
        type [<AllowNullLiteral>] IOptions =
            /// Whether the model is trusted.  Defaults to `false`.
            abstract trusted: bool option with get, set
            /// A callback function for when the data changes.
            abstract callback: (JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel.ISetDataOptions -> unit) option with get, set
            /// The initial mime data.
            abstract data: ReadonlyJSONObject option with get, set
            /// The initial mime metadata.
            abstract metadata: ReadonlyJSONObject option with get, set

module Outputmodel =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    // type nbformat =  __@jupyterlab_coreutils.nbformat
    // type IRenderMime = __@jupyterlab_rendermime_interfaces.IRenderMime

    type [<AllowNullLiteral>] IExports =
        abstract OutputModel: OutputModelStatic

    /// The interface for an output model.
    /// The namespace for IOutputModel sub-interfaces.
    type [<AllowNullLiteral>] IOutputModel =
        inherit JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel
        /// A signal emitted when the output model changes.
        abstract changed: ISignal<IOutputModel, unit>
        /// The output type.
        abstract ``type``: string
        /// The execution count of the model.
        abstract executionCount: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount
        /// Whether the output is trusted.
        abstract trusted: bool with get, set
        /// Dispose of the resources used by the output model.
        abstract dispose: unit -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IOutput

    module IOutputModel =

        /// The options used to create a notebook output model.
        type [<AllowNullLiteral>] IOptions =
            /// The raw output value.
            abstract value: JupyterlabCoreutils.Nbformat.Nbformat.IOutput with get, set
            /// Whether the output is trusted.  The default is false.
            abstract trusted: bool option with get, set

    /// The default implementation of a notebook output model.
    /// The namespace for OutputModel statics.
    type [<AllowNullLiteral>] OutputModel =
        inherit IOutputModel
        /// A signal emitted when the output model changes.
        abstract changed: ISignal<OutputModel, unit>
        /// The output type.
        abstract ``type``: string
        /// The execution count.
        abstract executionCount: JupyterlabCoreutils.Nbformat.Nbformat.ExecutionCount
        /// Whether the model is trusted.
        abstract trusted: bool
        /// Dispose of the resources used by the output model.
        abstract dispose: unit -> unit
        /// The data associated with the model.
        abstract data: ReadonlyJSONObject
        /// The metadata associated with the model.
        abstract metadata: ReadonlyJSONObject
        /// Set the data associated with the model.
        /// 
        /// #### Notes
        /// Depending on the implementation of the mime model,
        /// this call may or may not have deferred effects,
        abstract setData: options: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel.ISetDataOptions -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.IOutput

    /// The default implementation of a notebook output model.
    /// The namespace for OutputModel statics.
    type [<AllowNullLiteral>] OutputModelStatic =
        /// Construct a new output model.
        [<Emit "new $0($1...)">] abstract Create: options: IOutputModel.IOptions -> OutputModel

    module OutputModel =

        type [<AllowNullLiteral>] IExports =
            /// Get the data for an output.
            abstract getData: output: JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> JSONObject
            /// Get the metadata from an output message.
            abstract getMetadata: output: JupyterlabCoreutils.Nbformat.Nbformat.IOutput -> JSONObject

module Registry =
    // type Contents = JupyterlabServices.__contents_index.Contents // __@jupyterlab_services.Contents
    // type Session = JupyterlabServices.__session_session.Session // __@jupyterlab_services.Session
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime
    type IClientSession = JupyterlabApputils.IClientSession // = __@jupyterlab_apputils.IClientSession
    type ISanitizer = JupyterlabApputils.ISanitizer  // __@jupyterlab_apputils.ISanitizer
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject //__@phosphor_coreutils.ReadonlyJSONObject
    type MimeModel = Mimemodel.MimeModel
    type IRenderMimeRegistry = Tokens.IRenderMimeRegistry

    type [<AllowNullLiteral>] IExports =
        abstract RenderMimeRegistry: RenderMimeRegistryStatic

    /// An object which manages mime renderer factories.
    /// 
    /// This object is used to render mime models using registered mime
    /// renderers, selecting the preferred mime renderer to render the
    /// model into a widget.
    /// 
    /// #### Notes
    /// This class is not intended to be subclassed.
    /// The namespace for `RenderMimeRegistry` class statics.
    type [<AllowNullLiteral>] RenderMimeRegistry =
        inherit IRenderMimeRegistry
        /// The sanitizer used by the rendermime instance.
        abstract sanitizer: ISanitizer
        /// The object used to resolve relative urls for the rendermime instance.
        abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option
        /// The object used to handle path opening links.
        abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option
        /// The LaTeX typesetter for the rendermime.
        abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option
        /// The ordered list of mimeTypes.
        abstract mimeTypes: ReadonlyArray<string>
        /// <summary>Find the preferred mime type for a mime bundle.</summary>
        /// <param name="bundle">- The bundle of mime data.</param>
        /// <param name="safe">- How to consider safe/unsafe factories. If 'ensure',
        /// it will only consider safe factories. If 'any', any factory will be
        /// considered. If 'prefer', unsafe factories will be considered, but
        /// only after the safe options have been exhausted.</param>
        abstract preferredMimeType: bundle: ReadonlyJSONObject * ?safe: U3<string, string, string> -> string option
        /// <summary>Create a renderer for a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract createRenderer: mimeType: string -> JupyterlabRendermimeInterfaces.IRenderMime.IRenderer
        /// Create a new mime model.  This is a convenience method.
        abstract createModel: ?options: Mimemodel.MimeModel.IOptions -> MimeModel
        /// <summary>Create a clone of this rendermime instance.</summary>
        /// <param name="options">- The options for configuring the clone.</param>
        abstract clone: ?options: Tokens.IRenderMimeRegistry.ICloneOptions -> RenderMimeRegistry
        /// <summary>Get the renderer factory registered for a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract getFactory: mimeType: string -> JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory option
        /// <summary>Add a renderer factory to the rendermime.</summary>
        /// <param name="factory">- The renderer factory of interest.</param>
        /// <param name="rank">- The rank of the renderer. A lower rank indicates
        /// a higher priority for rendering. If not given, the rank will
        /// defer to the `defaultRank` of the factory.  If no `defaultRank`
        /// is given, it will default to 100.
        /// 
        /// #### Notes
        /// The renderer will replace an existing renderer for the given
        /// mimeType.</param>
        abstract addFactory: factory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory * ?rank: float -> unit
        /// <summary>Remove a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract removeMimeType: mimeType: string -> unit
        /// <summary>Get the rank for a given mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract getRank: mimeType: string -> float option
        /// <summary>Set the rank of a given mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        /// <param name="rank">- The new rank to assign.
        /// 
        /// #### Notes
        /// This is a no-op if the mime type is not registered.</param>
        abstract setRank: mimeType: string * rank: float -> unit

    /// An object which manages mime renderer factories.
    /// 
    /// This object is used to render mime models using registered mime
    /// renderers, selecting the preferred mime renderer to render the
    /// model into a widget.
    /// 
    /// #### Notes
    /// This class is not intended to be subclassed.
    /// The namespace for `RenderMimeRegistry` class statics.
    type [<AllowNullLiteral>] RenderMimeRegistryStatic =
        /// <summary>Construct a new rendermime.</summary>
        /// <param name="options">- The options for initializing the instance.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: RenderMimeRegistry.IOptions -> RenderMimeRegistry

    module RenderMimeRegistry =

        type [<AllowNullLiteral>] IExports =
            abstract UrlResolver: UrlResolverStatic

        /// The options used to initialize a rendermime instance.
        type [<AllowNullLiteral>] IOptions =
            /// Initial factories to add to the rendermime instance.
            abstract initialFactories: ReadonlyArray<JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory> option with get, set
            /// The sanitizer used to sanitize untrusted html inputs.
            /// 
            /// If not given, a default sanitizer will be used.
            abstract sanitizer: JupyterlabRendermimeInterfaces.IRenderMime.ISanitizer option with get, set
            /// The initial resolver object.
            /// 
            /// The default is `null`.
            abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option with get, set
            /// An optional path handler.
            abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option with get, set
            /// An optional LaTeX typesetter.
            abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option with get, set

        /// A default resolver that uses a session and a contents manager.
        type [<AllowNullLiteral>] UrlResolver =
            inherit JupyterlabRendermimeInterfaces.IRenderMime.IResolver
            /// Resolve a relative url to an absolute url path.
            abstract resolveUrl: url: string -> Promise<string>
            /// Get the download url of a given absolute url path.
            /// 
            /// #### Notes
            /// This URL may include a query parameter.
            abstract getDownloadUrl: url: string -> Promise<string>
            /// Whether the URL should be handled by the resolver
            /// or not.
            /// 
            /// #### Notes
            /// This is similar to the `isLocal` check in `URLExt`,
            /// but it also checks whether the path points to any
            /// of the `IDrive`s that may be registered with the contents
            /// manager.
            abstract isLocal: url: string -> bool

        /// A default resolver that uses a session and a contents manager.
        type [<AllowNullLiteral>] UrlResolverStatic =
            /// Create a new url resolver for a console.
            [<Emit "new $0($1...)">] abstract Create: options: IUrlResolverOptions -> UrlResolver

        /// The options used to create a UrlResolver.
        type [<AllowNullLiteral>] IUrlResolverOptions =
            /// The session used by the resolver.
            abstract session: U2<JupyterlabServices.__session_session.Session.ISession, IClientSession> with get, set
            /// The contents manager used by the resolver.
            abstract contents: JupyterlabServices.__contents_index.Contents.IManager with get, set

module Renderers =
    type ISanitizer = JupyterlabApputils.ISanitizer // __@jupyterlab_apputils.ISanitizer
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime

    type [<AllowNullLiteral>] IExports =
        /// Render HTML into a host node.
        abstract renderHTML: options: RenderHTML.IOptions -> Promise<unit>
        /// Render an image into a host node.
        abstract renderImage: options: RenderImage.IRenderOptions -> Promise<unit>
        /// Render LaTeX into a host node.
        abstract renderLatex: options: RenderLatex.IRenderOptions -> Promise<unit>
        /// Render Markdown into a host node.
        abstract renderMarkdown: options: RenderMarkdown.IRenderOptions -> Promise<unit>
        /// Render SVG into a host node.
        abstract renderSVG: options: RenderSVG.IRenderOptions -> Promise<unit>
        /// Render text into a host node.
        abstract renderText: options: RenderText.IRenderOptions -> Promise<unit>

    module RenderHTML =

        /// The options for the `renderHTML` function.
        type [<AllowNullLiteral>] IOptions =
            /// The host node for the rendered HTML.
            abstract host: HTMLElement with get, set
            /// The HTML source to render.
            abstract source: string with get, set
            /// Whether the source is trusted.
            abstract trusted: bool with get, set
            /// The html sanitizer for untrusted source.
            abstract sanitizer: ISanitizer with get, set
            /// An optional url resolver.
            abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option with get, set
            /// An optional link handler.
            abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option with get, set
            /// Whether the node should be typeset.
            abstract shouldTypeset: bool with get, set
            /// The LaTeX typesetter for the application.
            abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option with get, set

    module RenderImage =

        /// The options for the `renderImage` function.
        type [<AllowNullLiteral>] IRenderOptions =
            /// The image node to update with the content.
            abstract host: HTMLElement with get, set
            /// The mime type for the image.
            abstract mimeType: string with get, set
            /// The base64 encoded source for the image.
            abstract source: string with get, set
            /// The optional width for the image.
            abstract width: float option with get, set
            /// The optional height for the image.
            abstract height: float option with get, set
            /// Whether an image requires a background for legibility.
            abstract needsBackground: string option with get, set
            /// Whether the image should be unconfined.
            abstract unconfined: bool option with get, set

    module RenderLatex =

        /// The options for the `renderLatex` function.
        type [<AllowNullLiteral>] IRenderOptions =
            /// The host node for the rendered LaTeX.
            abstract host: HTMLElement with get, set
            /// The LaTeX source to render.
            abstract source: string with get, set
            /// Whether the node should be typeset.
            abstract shouldTypeset: bool with get, set
            /// The LaTeX typesetter for the application.
            abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option with get, set

    module RenderMarkdown =

        /// The options for the `renderMarkdown` function.
        type [<AllowNullLiteral>] IRenderOptions =
            /// The host node for the rendered Markdown.
            abstract host: HTMLElement with get, set
            /// The Markdown source to render.
            abstract source: string with get, set
            /// Whether the source is trusted.
            abstract trusted: bool with get, set
            /// The html sanitizer for untrusted source.
            abstract sanitizer: ISanitizer with get, set
            /// An optional url resolver.
            abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option with get, set
            /// An optional link handler.
            abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option with get, set
            /// Whether the node should be typeset.
            abstract shouldTypeset: bool with get, set
            /// The LaTeX typesetter for the application.
            abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option with get, set

    module RenderSVG =

        /// The options for the `renderSVG` function.
        type [<AllowNullLiteral>] IRenderOptions =
            /// The host node for the rendered SVG.
            abstract host: HTMLElement with get, set
            /// The SVG source.
            abstract source: string with get, set
            /// Whether the source is trusted.
            abstract trusted: bool with get, set
            /// Whether the svg should be unconfined.
            abstract unconfined: bool option with get, set

    module RenderText =

        /// The options for the `renderText` function.
        type [<AllowNullLiteral>] IRenderOptions =
            /// The host node for the text content.
            abstract host: HTMLElement with get, set
            /// The html sanitizer for untrusted source.
            abstract sanitizer: ISanitizer with get, set
            /// The source text to render.
            abstract source: string with get, set

module Tokens =
    type Token<'T> = PhosphorCoreutils.Token<'T> // __@phosphor_coreutils.Token
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type ISanitizer = JupyterlabApputils.ISanitizer // __@jupyterlab_apputils.ISanitizer
    // type IRenderMime = JupyterlabRendermimeInterfaces.IRenderMime // __@jupyterlab_rendermime_interfaces.IRenderMime
    type MimeModel = Mimemodel.MimeModel

    type [<AllowNullLiteral>] IExports =
        abstract IRenderMimeRegistry: Token<IRenderMimeRegistry>
        abstract ILatexTypesetter: Token<JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter>

    /// The rendermime token.
    type [<AllowNullLiteral>] IRenderMimeRegistry =
        /// The sanitizer used by the rendermime instance.
        abstract sanitizer: ISanitizer
        /// The object used to resolve relative urls for the rendermime instance.
        abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option
        /// The object used to handle path opening links.
        abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option
        /// The LaTeX typesetter for the rendermime.
        abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option
        /// The ordered list of mimeTypes.
        abstract mimeTypes: ReadonlyArray<string>
        /// <summary>Find the preferred mime type for a mime bundle.</summary>
        /// <param name="bundle">- The bundle of mime data.</param>
        /// <param name="safe">- How to consider safe/unsafe factories. If 'ensure',
        /// it will only consider safe factories. If 'any', any factory will be
        /// considered. If 'prefer', unsafe factories will be considered, but
        /// only after the safe options have been exhausted.</param>
        abstract preferredMimeType: bundle: ReadonlyJSONObject * ?safe: U3<string, string, string> -> string option
        /// <summary>Create a renderer for a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract createRenderer: mimeType: string -> JupyterlabRendermimeInterfaces.IRenderMime.IRenderer
        /// Create a new mime model.  This is a convenience method.
        abstract createModel: ?options: Mimemodel.MimeModel.IOptions -> MimeModel
        /// <summary>Create a clone of this rendermime instance.</summary>
        /// <param name="options">- The options for configuring the clone.</param>
        abstract clone: ?options: Tokens.IRenderMimeRegistry.ICloneOptions -> IRenderMimeRegistry
        /// <summary>Get the renderer factory registered for a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract getFactory: mimeType: string -> JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory option
        /// <summary>Add a renderer factory to the rendermime.</summary>
        /// <param name="factory">- The renderer factory of interest.</param>
        /// <param name="rank">- The rank of the renderer. A lower rank indicates
        /// a higher priority for rendering. If not given, the rank will
        /// defer to the `defaultRank` of the factory.  If no `defaultRank`
        /// is given, it will default to 100.
        /// 
        /// #### Notes
        /// The renderer will replace an existing renderer for the given
        /// mimeType.</param>
        abstract addFactory: factory: JupyterlabRendermimeInterfaces.IRenderMime.IRendererFactory * ?rank: float -> unit
        /// <summary>Remove a mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract removeMimeType: mimeType: string -> unit
        /// <summary>Get the rank for a given mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        abstract getRank: mimeType: string -> float option
        /// <summary>Set the rank of a given mime type.</summary>
        /// <param name="mimeType">- The mime type of interest.</param>
        /// <param name="rank">- The new rank to assign.
        /// 
        /// #### Notes
        /// This is a no-op if the mime type is not registered.</param>
        abstract setRank: mimeType: string * rank: float -> unit

    module IRenderMimeRegistry =

        /// The options used to clone a rendermime instance.
        type [<AllowNullLiteral>] ICloneOptions =
            /// The new sanitizer used to sanitize untrusted html inputs.
            abstract sanitizer: JupyterlabRendermimeInterfaces.IRenderMime.ISanitizer option with get, set
            /// The new resolver object.
            abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option with get, set
            /// The new path handler.
            abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option with get, set
            /// The new LaTeX typesetter.
            abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter option with get, set

    /// The latex typesetter token.
    type [<AllowNullLiteral>] ILatexTypesetter =
        inherit JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter



module Widgets =
    // type IRenderMime = __@jupyterlab_rendermime_interfaces.IRenderMime
    type Message = PhosphorMessaging.Message //__@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget

    type [<AllowNullLiteral>] IExports =
        abstract RenderedCommon: RenderedCommonStatic
        abstract RenderedHTMLCommon: RenderedHTMLCommonStatic
        abstract RenderedHTML: RenderedHTMLStatic
        abstract RenderedLatex: RenderedLatexStatic
        abstract RenderedImage: RenderedImageStatic
        abstract RenderedMarkdown: RenderedMarkdownStatic
        abstract RenderedSVG: RenderedSVGStatic
        abstract RenderedText: RenderedTextStatic
        abstract RenderedJavaScript: RenderedJavaScriptStatic

    [<Import("*","@jupyterlab/rendermime")>]
    let Types:IExports = jsNative

    /// A common base class for mime renderers.
    type [<AllowNullLiteral>] RenderedCommon =
        inherit Widget
        inherit JupyterlabRendermimeInterfaces.IRenderMime.IRenderer
        /// The mimetype being rendered.
        abstract mimeType: string
        /// The sanitizer used to sanitize untrusted html inputs.
        abstract sanitizer: JupyterlabRendermimeInterfaces.IRenderMime.ISanitizer
        /// The resolver object.
        abstract resolver: JupyterlabRendermimeInterfaces.IRenderMime.IResolver option
        /// The link handler.
        abstract linkHandler: JupyterlabRendermimeInterfaces.IRenderMime.ILinkHandler option
        /// The latexTypesetter.
        abstract latexTypesetter: JupyterlabRendermimeInterfaces.IRenderMime.ILatexTypesetter
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract renderModel: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// <summary>Render the mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// <summary>Set the URI fragment identifier.</summary>
        /// <param name="fragment">- The URI fragment identifier.</param>
        abstract setFragment: fragment: string -> unit

    /// A common base class for mime renderers.
    type [<AllowNullLiteral>] RenderedCommonStatic =
        /// <summary>Construct a new rendered common widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedCommon

    /// A common base class for HTML mime renderers.
    type [<AllowNullLiteral>] RenderedHTMLCommon =
        inherit RenderedCommon
        abstract setFragment: fragment: string -> unit

    /// A common base class for HTML mime renderers.
    type [<AllowNullLiteral>] RenderedHTMLCommonStatic =
        /// <summary>Construct a new rendered HTML common widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedHTMLCommon

    /// A mime renderer for displaying HTML and math.
    type [<AllowNullLiteral>] RenderedHTML =
        inherit RenderedHTMLCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// A message handler invoked on an `'after-attach'` message.
        abstract onAfterAttach: msg: Message -> unit

    /// A mime renderer for displaying HTML and math.
    type [<AllowNullLiteral>] RenderedHTMLStatic =
        /// <summary>Construct a new rendered HTML widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedHTML

    /// A mime renderer for displaying LaTeX output.
    type [<AllowNullLiteral>] RenderedLatex =
        inherit RenderedCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// A message handler invoked on an `'after-attach'` message.
        abstract onAfterAttach: msg: Message -> unit

    /// A mime renderer for displaying LaTeX output.
    type [<AllowNullLiteral>] RenderedLatexStatic =
        /// <summary>Construct a new rendered LaTeX widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedLatex

    /// A mime renderer for displaying images.
    type [<AllowNullLiteral>] RenderedImage =
        inherit RenderedCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>

    /// A mime renderer for displaying images.
    type [<AllowNullLiteral>] RenderedImageStatic =
        /// <summary>Construct a new rendered image widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedImage

    /// A mime renderer for displaying Markdown with embedded latex.
    type [<AllowNullLiteral>] RenderedMarkdown =
        inherit RenderedHTMLCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// A message handler invoked on an `'after-attach'` message.
        abstract onAfterAttach: msg: Message -> unit

    /// A mime renderer for displaying Markdown with embedded latex.
    type [<AllowNullLiteral>] RenderedMarkdownStatic =
        /// <summary>Construct a new rendered markdown widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedMarkdown

    /// A widget for displaying SVG content.
    type [<AllowNullLiteral>] RenderedSVG =
        inherit RenderedCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>
        /// A message handler invoked on an `'after-attach'` message.
        abstract onAfterAttach: msg: Message -> unit

    /// A widget for displaying SVG content.
    type [<AllowNullLiteral>] RenderedSVGStatic =
        /// <summary>Construct a new rendered SVG widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedSVG

    /// A widget for displaying plain text and console text.
    type [<AllowNullLiteral>] RenderedText =
        inherit RenderedCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>

    /// A widget for displaying plain text and console text.
    type [<AllowNullLiteral>] RenderedTextStatic =
        /// <summary>Construct a new rendered text widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedText

    /// A widget for displaying deprecated JavaScript output.
    type [<AllowNullLiteral>] RenderedJavaScript =
        inherit RenderedCommon
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract render: model: JupyterlabRendermimeInterfaces.IRenderMime.IMimeModel -> Promise<unit>

    /// A widget for displaying deprecated JavaScript output.
    type [<AllowNullLiteral>] RenderedJavaScriptStatic =
        /// <summary>Construct a new rendered text widget.</summary>
        /// <param name="options">- The options for initializing the widget.</param>
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabRendermimeInterfaces.IRenderMime.IRendererOptions -> RenderedJavaScript
