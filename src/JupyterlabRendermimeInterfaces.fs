// ts2fable 0.0.0
module rec JupyterlabRendermimeInterfaces
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

type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // @phosphor_coreutils.ReadonlyJSONObject
type Widget = PhosphorWidgets.Widget // @phosphor_widgets.Widget

module IRenderMime =

    /// A model for mime data.
    /// The namespace for IMimeModel associated interfaces.
    type [<AllowNullLiteral>] IMimeModel =
        /// Whether the data in the model is trusted.
        abstract trusted: bool
        /// The data associated with the model.
        abstract data: ReadonlyJSONObject
        /// The metadata associated with the model.
        /// 
        /// Among others, it can include an attribute named `fragment`
        /// that stores a URI fragment identifier for the MIME resource.
        abstract metadata: ReadonlyJSONObject
        /// Set the data associated with the model.
        /// 
        /// #### Notes
        /// Calling this function may trigger an asynchronous operation
        /// that could cause the renderer to be rendered with a new model
        /// containing the new data.
        abstract setData: options: IMimeModel.ISetDataOptions -> unit

    module IMimeModel =

        /// The options used to update a mime model.
        type [<AllowNullLiteral>] ISetDataOptions =
            /// The new data object.
            abstract data: ReadonlyJSONObject option with get, set
            /// The new metadata object.
            abstract metadata: ReadonlyJSONObject option with get, set

    /// A toolbar item.
    type [<AllowNullLiteral>] IToolbarItem =
        abstract name: string with get, set
        abstract widget: Widget with get, set

    /// The options used to initialize a document widget factory.
    /// 
    /// This interface is intended to be used by mime renderer extensions
    /// to define a document opener that uses its renderer factory.
    type [<AllowNullLiteral>] IDocumentWidgetFactoryOptions =
        /// The name of the widget to display in dialogs.
        abstract name: string
        /// The name of the document model type.
        abstract modelName: string option
        /// The primary file type of the widget.
        abstract primaryFileType: string
        /// The file types the widget can view.
        abstract fileTypes: ReadonlyArray<string>
        /// The file types for which the factory should be the default.
        abstract defaultFor: ReadonlyArray<string> option
        /// The file types for which the factory should be the default for rendering,
        /// if that is different than the default factory (which may be for editing)
        /// If undefined, then it will fall back on the default file type.
        abstract defaultRendered: ReadonlyArray<string> option
        /// A function returning a list of toolbar items to add to the toolbar.
        abstract toolbarFactory: (IRenderer -> ResizeArray<IToolbarItem>) option

    /// A file type to associate with the renderer.
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
        /// The file format for the file type ('text', 'base64', or 'json').
        abstract fileFormat: string option

    /// An interface for using a RenderMime.IRenderer for output and read-only documents.
    type [<AllowNullLiteral>] IExtension =
        /// The ID of the extension.
        /// 
        /// #### Notes
        /// The convention for extension IDs in JupyterLab is the full NPM package
        /// name followed by a colon and a unique string token, e.g.
        /// `'@jupyterlab/apputils-extension:settings'` or `'foo-extension:bar'`.
        abstract id: string
        /// A renderer factory to be registered to render the MIME type.
        abstract rendererFactory: IRendererFactory
        /// The rank passed to `RenderMime.addFactory`.  If not given,
        /// defaults to the `defaultRank` of the factory.
        abstract rank: float option
        /// The timeout after user activity to re-render the data.
        abstract renderTimeout: float option
        /// Preferred data type from the model.  Defaults to `string`.
        abstract dataType: U2<string, string> option
        /// The options used to open a document with the renderer factory.
        abstract documentWidgetFactoryOptions: U2<IDocumentWidgetFactoryOptions, ReadonlyArray<IDocumentWidgetFactoryOptions>> option
        /// The optional file type associated with the extension.
        abstract fileTypes: ReadonlyArray<IFileType> option

    /// The interface for a module that exports an extension or extensions as
    /// the default value.
    type [<AllowNullLiteral>] IExtensionModule =
        /// The default export.
        abstract ``default``: U2<IExtension, ReadonlyArray<IExtension>>

    /// A widget which displays the contents of a mime model.
    type [<AllowNullLiteral>] IRenderer =
        inherit Widget
        /// <summary>Render a mime model.</summary>
        /// <param name="model">- The mime model to render.</param>
        abstract renderModel: model: IMimeModel -> Promise<unit>

    /// The interface for a renderer factory.
    type [<AllowNullLiteral>] IRendererFactory =
        /// Whether the factory is a "safe" factory.
        /// 
        /// #### Notes
        /// A "safe" factory produces renderer widgets which can render
        /// untrusted model data in a usable way. *All* renderers must
        /// handle untrusted data safely, but some may simply failover
        /// with a "Run cell to view output" message. A "safe" renderer
        /// is an indication that its sanitized output will be useful.
        abstract safe: bool
        /// The mime types handled by this factory.
        abstract mimeTypes: ReadonlyArray<string>
        /// The default rank of the factory.  If not given, defaults to 100.
        abstract defaultRank: float option
        /// <summary>Create a renderer which displays the mime data.</summary>
        /// <param name="options">- The options used to render the data.</param>
        abstract createRenderer: options: IRendererOptions -> IRenderer

    /// The options used to create a renderer.
    type [<AllowNullLiteral>] IRendererOptions =
        /// The preferred mimeType to render.
        abstract mimeType: string with get, set
        /// The html sanitizer.
        abstract sanitizer: ISanitizer with get, set
        /// An optional url resolver.
        abstract resolver: IResolver option with get, set
        /// An optional link handler.
        abstract linkHandler: ILinkHandler option with get, set
        /// The LaTeX typesetter.
        abstract latexTypesetter: ILatexTypesetter option with get, set

    /// An object that handles html sanitization.
    type [<AllowNullLiteral>] ISanitizer =
        /// Sanitize an HTML string.
        abstract sanitize: dirty: string -> string

    /// An object that handles links on a node.
    type [<AllowNullLiteral>] ILinkHandler =
        /// <summary>Add the link handler to the node.</summary>
        /// <param name="node">: the anchor node for which to handle the link.</param>
        /// <param name="path">: the path to open when the link is clicked.</param>
        /// <param name="id">: an optional element id to scroll to when the path is opened.</param>
        abstract handleLink: node: HTMLElement * path: string * ?id: string -> unit

    /// An object that resolves relative URLs.
    type [<AllowNullLiteral>] IResolver =
        /// Resolve a relative url to an absolute url path.
        abstract resolveUrl: url: string -> Promise<string>
        /// Get the download url for a given absolute url path.
        /// 
        /// #### Notes
        /// This URL may include a query parameter.
        abstract getDownloadUrl: url: string -> Promise<string>
        /// Whether the URL should be handled by the resolver
        /// or not.
        /// 
        /// #### Notes
        /// This is similar to the `isLocal` check in `URLExt`,
        /// but can also perform additional checks on whether the
        /// resolver should handle a given URL.
        abstract isLocal: (string -> bool) option with get, set

    /// The interface for a LaTeX typesetter.
    type [<AllowNullLiteral>] ILatexTypesetter =
        /// <summary>Typeset a DOM element.</summary>
        /// <param name="element">- the DOM element to typeset. The typesetting may
        /// happen synchronously or asynchronously.
        /// 
        /// #### Notes
        /// The application-wide rendermime object has a settable
        /// `latexTypesetter` property which is used wherever LaTeX
        /// typesetting is required. Extensions wishing to provide their
        /// own typesetter may replace that on the global `lab.rendermime`.</param>
        abstract typeset: element: HTMLElement -> unit
