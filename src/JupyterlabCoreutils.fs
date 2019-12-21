// ts2fable 0.0.0
module rec JupyterlabCoreutils
open System
// open FSharp.Core
open Fable.Core
open Fable.Core.JS
// open Fable.Core.JsInterop
// open Fable.Import

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo: end typescript hacks

module Activitymonitor =
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal

    type [<AllowNullLiteral>] IExports =
        abstract ActivityMonitor: ActivityMonitorStatic

    /// A class that monitors activity on a signal.
    /// The namespace for `ActivityMonitor` statics.
    type [<AllowNullLiteral>] ActivityMonitor<'Sender, 'Args> =
        inherit IDisposable
        /// A signal emitted when activity has ceased.
        abstract activityStopped: ISignal<ActivityMonitor<'Sender, 'Args>, ActivityMonitor.IArguments<'Sender, 'Args>>
        /// The timeout associated with the monitor, in milliseconds.
        abstract timeout: float with get, set
        /// Test whether the monitor has been disposed.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract isDisposed: bool
        /// Dispose of the resources used by the activity monitor.
        abstract dispose: unit -> unit

    /// A class that monitors activity on a signal.
    /// The namespace for `ActivityMonitor` statics.
    type [<AllowNullLiteral>] ActivityMonitorStatic =
        /// Construct a new activity monitor.
        [<Emit "new $0($1...)">] abstract Create: options: ActivityMonitor.IOptions<'Sender, 'Args> -> ActivityMonitor<'Sender, 'Args>

    module ActivityMonitor =

        /// The options used to construct a new `ActivityMonitor`.
        type [<AllowNullLiteral>] IOptions<'Sender, 'Args> =
            /// The signal to monitor.
            abstract signal: ISignal<'Sender, 'Args> with get, set
            /// The activity timeout in milliseconds.
            /// 
            /// The default is 1 second.
            abstract timeout: float option with get, set

        /// The argument object for an activity timeout.
        type [<AllowNullLiteral>] IArguments<'Sender, 'Args> =
            /// The most recent sender object.
            abstract sender: 'Sender with get, set
            /// The most recent argument object.
            abstract args: 'Args with get, set

module Dataconnector =
    type IDataConnector<'T, 'U, 'V> = Interfaces.IDataConnector<'T, 'U, 'V>

    type [<AllowNullLiteral>] IExports =
        abstract DataConnector: DataConnectorStatic

    type DataConnector<'U, 'V> =
        DataConnector<obj, 'U, 'V>

    type DataConnector<'V> =
        DataConnector<obj, obj, 'V>

    /// An abstract class that adheres to the data connector interface.
    type [<AllowNullLiteral>] DataConnector<'T, 'U, 'V> =
        inherit IDataConnector<'T, 'U, 'V>
        /// <summary>Retrieve an item from the data connector.</summary>
        /// <param name="id">- The identifier used to retrieve an item.</param>
        abstract fetch: id: 'V -> Promise<'T option>
        /// <summary>Retrieve the list of items available from the data connector.</summary>
        /// <param name="query">- The optional query filter to apply to the connector request.</param>
        abstract list: ?query: obj -> Promise<TypeLiteral_01<'T, 'V>>
        /// <summary>Remove a value using the data connector.</summary>
        /// <param name="id">- The identifier for the data being removed.</param>
        abstract remove: id: 'V -> Promise<obj option>
        /// <summary>Save a value using the data connector.</summary>
        /// <param name="id">- The identifier for the data being saved.</param>
        /// <param name="value">- The data being saved.</param>
        abstract save: id: 'V * value: 'U -> Promise<obj option>

    /// An abstract class that adheres to the data connector interface.
    type [<AllowNullLiteral>] DataConnectorStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> DataConnector<'T, 'U, 'V>

    type [<AllowNullLiteral>] TypeLiteral_01<'T, 'V> =
        abstract ids: ResizeArray<'V> with get, set
        abstract values: ResizeArray<'T> with get, set

module Interfaces =
    type CommandRegistry = PhosphorCommands.CommandRegistry // __@phosphor_commands.CommandRegistry
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type ReadonlyJSONValue = PhosphorCoreutils.ReadonlyJSONValue // __@phosphor_coreutils.ReadonlyJSONValue
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type IObservableDisposable = PhosphorDisposable.IObservableDisposable // __@phosphor_disposable.IObservableDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal

    type IChangedArgs<'U> =
        IChangedArgs<obj, 'U>

    /// A generic interface for change emitter payloads.
    type [<AllowNullLiteral>] IChangedArgs<'T, 'U> =
        /// The name of the changed attribute.
        abstract name: 'U with get, set
        /// The old value of the changed attribute.
        abstract oldValue: 'T with get, set
        /// The new value of the changed attribute.
        abstract newValue: 'T with get, set

    type IDataConnector<'U, 'V> =
        IDataConnector<obj, 'U, 'V>

    type IDataConnector<'V> =
        IDataConnector<obj, obj, 'V>

    /// The description of a general purpose data connector.
    type [<AllowNullLiteral>] IDataConnector<'T, 'U, 'V> =
        /// <summary>Retrieve an item from the data connector.</summary>
        /// <param name="id">- The identifier used to retrieve an item.</param>
        abstract fetch: id: 'V -> Promise<'T option>
        /// <summary>Retrieve the list of items available from the data connector.</summary>
        /// <param name="query">- The optional query filter to apply to the connector request.</param>
        abstract list: ?query: obj -> Promise<TypeLiteral_01<'T, 'V>>
        /// <summary>Remove a value using the data connector.</summary>
        /// <param name="id">- The identifier for the data being removed.</param>
        abstract remove: id: 'V -> Promise<obj option>
        /// <summary>Save a value using the data connector.</summary>
        /// <param name="id">- The identifier for the data being saved.</param>
        /// <param name="value">- The data being saved.</param>
        abstract save: id: 'V * value: 'U -> Promise<obj option>

    /// A pool of objects whose disposable lifecycle is tracked.
    type [<AllowNullLiteral>] IObjectPool<'T> =
        inherit IDisposable
        /// A signal emitted when an object is added.
        /// 
        /// ####
        /// This signal does not emit if an object is added using `inject()`.
        abstract added: ISignal<IObjectPool<'T>, 'T>
        /// The current object.
        abstract current: 'T option
        /// A signal emitted when the current object changes.
        /// 
        /// #### Notes
        /// If the last object being tracked is disposed, `null` will be emitted.
        abstract currentChanged: ISignal<IObjectPool<'T>, 'T option>
        /// The number of objects held by the pool.
        abstract size: float
        /// A signal emitted when an object is updated.
        abstract updated: ISignal<IObjectPool<'T>, 'T>
        /// Find the first object in the pool that satisfies a filter function.
        abstract find: fn: ('T -> bool) -> 'T option
        /// <summary>Iterate through each object in the pool.</summary>
        /// <param name="fn">- The function to call on each object.</param>
        abstract forEach: fn: ('T -> unit) -> unit
        /// <summary>Filter the objects in the pool based on a predicate.</summary>
        /// <param name="fn">- The function by which to filter.</param>
        abstract filter: fn: ('T -> bool) -> ResizeArray<'T>
        /// <summary>Check if this pool has the specified object.</summary>
        /// <param name="obj">- The object whose existence is being checked.</param>
        abstract has: obj: 'T -> bool

    /// A readonly poll that calls an asynchronous function with each tick.
    /// A namespace for `IPoll` types.
    type [<AllowNullLiteral>] IPoll<'T, 'U, 'V> =
        /// A signal emitted when the poll is disposed.
        abstract disposed: ISignal<IPoll<'T, 'U, 'V>, unit>
        /// The polling frequency data.
        abstract frequency: IPoll.Frequency
        /// Whether the poll is disposed.
        abstract isDisposed: bool
        /// The name of the poll.
        abstract name: string
        /// The poll state, which is the content of the currently-scheduled poll tick.
        abstract state: IPoll.State<'T, 'U, 'V>
        /// A promise that resolves when the currently-scheduled tick completes.
        /// 
        /// #### Notes
        /// Usually this will resolve after `state.interval` milliseconds from
        /// `state.timestamp`. It can resolve earlier if the user starts or refreshes the
        /// poll, etc.
        abstract tick: Promise<IPoll<'T, 'U, 'V>>
        /// A signal emitted when the poll state changes, i.e., a new tick is scheduled.
        abstract ticked: ISignal<IPoll<'T, 'U, 'V>, IPoll.State<'T, 'U, 'V>>

    module IPoll =

        type [<AllowNullLiteral>] Frequency =
            /// Whether poll frequency backs off (boolean) or the backoff growth rate
            /// (float > 1).
            /// 
            /// #### Notes
            /// If `true`, the default backoff growth rate is `3`.
            abstract backoff: U2<bool, float>
            /// The basic polling interval in milliseconds (integer).
            abstract interval: float
            /// The maximum milliseconds (integer) between poll requests.
            abstract max: float

        type Phase<'T> = 'T //obj

        type [<AllowNullLiteral>] State<'T, 'U, 'V> =
            /// The number of milliseconds until the current tick resolves.
            abstract interval: float
            /// The payload of the last poll resolution or rejection.
            /// 
            /// #### Notes
            /// The payload is `null` unless the `phase` is `'reconnected`, `'resolved'`,
            /// or `'rejected'`. Its type is `T` for resolutions and `U` for rejections.
            abstract payload: U2<'T, 'U> option
            /// The current poll phase.
            abstract phase: Phase<'V>
            /// The timestamp for when this tick was scheduled.
            abstract timestamp: float

    type IRateLimiter<'U> =
        IRateLimiter<obj, 'U>

    type IRateLimiter =
        IRateLimiter<obj, obj>

    /// A function whose invocations are rate limited and can be stopped after
    /// invocation before it has fired.
    type [<AllowNullLiteral>] IRateLimiter<'T, 'U> =
        inherit IDisposable
        /// The rate limit in milliseconds.
        abstract limit: float
        /// Invoke the rate limited function.
        abstract invoke: unit -> Promise<'T>
        /// Stop the function if it is mid-flight.
        abstract stop: unit -> Promise<unit>

    type IRestorer<'U, 'V> =
        IRestorer<obj, 'U, 'V>

    type IRestorer<'V> =
        IRestorer<obj, obj, 'V>

    type IRestorer =
        IRestorer<obj, obj, obj>

    /// An interface for a state restorer.
    /// A namespace for `IRestorer` interface definitions.
    type [<AllowNullLiteral>] IRestorer<'T, 'U, 'V> =
        /// <summary>Restore the objects in a given restorable collection.</summary>
        /// <param name="restorable">- The restorable collection being restored.</param>
        /// <param name="options">- The configuration options that describe restoration.</param>
        abstract restore: restorable: 'T * options: IRestorable.IOptions<'U> -> Promise<'V>
        /// A promise that settles when the collection has been restored.
        abstract restored: Promise<'V>

    module IRestorer =

        /// The state restoration configuration options.
        type [<AllowNullLiteral>] IOptions<'T> =
            /// The command to execute when restoring instances.
            abstract command: string with get, set
            /// A function that returns the args needed to restore an instance.
            abstract args: ('T -> ReadonlyJSONObject) option with get, set
            /// A function that returns a unique persistent name for this instance.
            abstract name: ('T -> string) with get, set
            /// The point after which it is safe to restore state.
            abstract ``when``: U2<Promise<obj option>, Array<Promise<obj option>>> option with get, set

    type IRestorable<'U> =
        IRestorable<obj, 'U>

    /// An interface for objects that can be restored.
    /// A namespace for `IRestorable` interface definitions.
    type [<AllowNullLiteral>] IRestorable<'T, 'U> =
        /// <summary>Restore the objects in this restorable collection.</summary>
        /// <param name="options">- The configuration options that describe restoration.</param>
        abstract restore: options: IRestorable.IOptions<'T> -> Promise<'U>
        /// A promise that settles when the collection has been restored.
        abstract restored: Promise<'U>

    module IRestorable =

        /// The state restoration configuration options.
        type [<AllowNullLiteral>] IOptions<'T> =
            inherit IRestorer.IOptions<'T>
            /// The data connector to fetch restore data.
            abstract connector: IDataConnector<ReadonlyJSONValue> with get, set
            /// The command registry which holds the restore command.
            abstract registry: CommandRegistry with get, set

    type [<AllowNullLiteral>] TypeLiteral_01<'T, 'V> =
        abstract ids: ResizeArray<'V> with get, set
        abstract values: ResizeArray<'T> with get, set

module Markdowncodeblocks =

    module MarkdownCodeBlocks =

        type [<AllowNullLiteral>] IExports =
            abstract CODE_BLOCK_MARKER: obj
            abstract MarkdownCodeBlock: MarkdownCodeBlockStatic
            /// <summary>Check whether the given file extension is a markdown extension</summary>
            /// <param name="extension">- A file extension</param>
            abstract isMarkdown: extension: string -> bool
            /// <summary>Construct all code snippets from current text
            /// (this could be potentially optimized if we can cache and detect differences)</summary>
            /// <param name="text">- A string to parse codeblocks from</param>
            abstract findMarkdownCodeBlocks: text: string -> ResizeArray<MarkdownCodeBlock>

        type [<AllowNullLiteral>] MarkdownCodeBlock =
            abstract startLine: float with get, set
            abstract endLine: float with get, set
            abstract code: string with get, set

        type [<AllowNullLiteral>] MarkdownCodeBlockStatic =
            [<Emit "new $0($1...)">] abstract Create: startLine: float -> MarkdownCodeBlock

module Nbformat =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject

    module Nbformat =

        type [<AllowNullLiteral>] IExports =
            abstract MAJOR_VERSION: float
            abstract MINOR_VERSION: float
            /// <summary>Validate a mime type/value pair.</summary>
            /// <param name="type">- The mimetype name.</param>
            /// <param name="value">- The value associated with the type.</param>
            abstract validateMimeValue: ``type``: string * value: U2<MultilineString, JSONObject> -> bool
            /// Test whether a cell is a raw cell.
            abstract isRaw: cell: ICell -> bool
            /// Test whether a cell is a markdown cell.
            abstract isMarkdown: cell: ICell -> bool
            /// Test whether a cell is a code cell.
            abstract isCode: cell: ICell -> bool
            /// Test whether an output is an execute result.
            abstract isExecuteResult: output: IOutput -> bool
            /// Test whether an output is from display data.
            abstract isDisplayData: output: IOutput -> bool
            /// Test whether an output is from updated display data.
            abstract isDisplayUpdate: output: IOutput -> bool
            /// Test whether an output is from a stream.
            abstract isStream: output: IOutput -> bool
            /// Test whether an output is an error.
            abstract isError: output: IOutput -> bool

        /// The kernelspec metadata.
        type [<AllowNullLiteral>] IKernelspecMetadata =
            inherit JSONObject
            abstract name: string with get, set
            abstract display_name: string with get, set

        /// The language info metatda
        type [<AllowNullLiteral>] ILanguageInfoMetadata =
            inherit JSONObject
            abstract name: string with get, set
            abstract codemirror_mode: U2<string, JSONObject> option with get, set
            abstract file_extension: string option with get, set
            abstract mimetype: string option with get, set
            abstract pygments_lexer: string option with get, set

        /// The default metadata for the notebook.
        type [<AllowNullLiteral>] INotebookMetadata =
            inherit JSONObject
            abstract kernelspec: IKernelspecMetadata option with get, set
            abstract language_info: ILanguageInfoMetadata option with get, set
            abstract orig_nbformat: float with get, set

        /// The notebook content.
        type [<AllowNullLiteral>] INotebookContent =
            inherit JSONObject
            abstract metadata: INotebookMetadata with get, set
            abstract nbformat_minor: float with get, set
            abstract nbformat: float with get, set
            abstract cells: ResizeArray<ICell> with get, set

        type MultilineString =
            U2<string, ResizeArray<string>>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module MultilineString =
            let ofString v: MultilineString = v |> U2.Case1
            let isString (v: MultilineString) = match v with U2.Case1 _ -> true | _ -> false
            let asString (v: MultilineString) = match v with U2.Case1 o -> Some o | _ -> None
            let ofStringArray v: MultilineString = v |> U2.Case2
            let isStringArray (v: MultilineString) = match v with U2.Case2 _ -> true | _ -> false
            let asStringArray (v: MultilineString) = match v with U2.Case2 o -> Some o | _ -> None

        /// A mime-type keyed dictionary of data.
        type [<AllowNullLiteral>] IMimeBundle =
            inherit JSONObject
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> U2<MultilineString, JSONObject> with get, set

        /// Media attachments (e.g. inline images).
        type [<AllowNullLiteral>] IAttachments =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> IMimeBundle with get, set

        type ExecutionCount =
            float option

        type OutputMetadata =
            JSONObject

        type [<StringEnum>] [<RequireQualifiedAccess>] CellType =
            | Code
            | Markdown
            | Raw

        /// The Jupyter metadata namespace.
        type [<AllowNullLiteral>] IBaseCellJupyterMetadata =
            inherit JSONObject
            /// Whether the source is hidden.
            abstract source_hidden: bool with get, set

        /// Cell-level metadata.
        type [<AllowNullLiteral>] IBaseCellMetadata =
            inherit JSONObject
            /// Whether the cell is trusted.
            /// 
            /// #### Notes
            /// This is not strictly part of the nbformat spec, but it is added by
            /// the contents manager.
            /// 
            /// See https://jupyter-notebook.readthedocs.io/en/latest/security.html.
            abstract trusted: bool with get, set
            /// The cell's name. If present, must be a non-empty string.
            abstract name: string with get, set
            /// The Jupyter metadata namespace
            abstract jupyter: obj with get, set
            /// The cell's tags. Tags must be unique, and must not contain commas.
            abstract tags: ResizeArray<string> with get, set

        /// The base cell interface.
        type [<AllowNullLiteral>] IBaseCell =
            inherit JSONObject
            /// String identifying the type of cell.
            abstract cell_type: string with get, set
            /// Contents of the cell, represented as an array of lines.
            abstract source: MultilineString with get, set
            /// Cell-level metadata.
            abstract metadata: obj with get, set

        /// Metadata for the raw cell.
        type [<AllowNullLiteral>] IRawCellMetadata =
            inherit IBaseCellMetadata
            /// Raw cell metadata format for nbconvert.
            abstract format: string with get, set

        /// A raw cell.
        type [<AllowNullLiteral>] IRawCell =
            inherit IBaseCell
            /// String identifying the type of cell.
            abstract cell_type: string with get, set
            /// Cell-level metadata.
            abstract metadata: obj with get, set
            /// Cell attachments.
            abstract attachments: IAttachments option with get, set

        /// A markdown cell.
        type [<AllowNullLiteral>] IMarkdownCell =
            inherit IBaseCell
            /// String identifying the type of cell.
            abstract cell_type: string with get, set
            /// Cell attachments.
            abstract attachments: IAttachments option with get, set

        /// The Jupyter metadata namespace for code cells.
        type [<AllowNullLiteral>] ICodeCellJupyterMetadata =
            inherit IBaseCellJupyterMetadata
            /// Whether the outputs are hidden. See https://github.com/jupyter/nbformat/issues/137.
            abstract outputs_hidden: bool with get, set

        /// Metadata for a code cell.
        type [<AllowNullLiteral>] ICodeCellMetadata =
            inherit IBaseCellMetadata
            /// Whether the cell is collapsed/expanded.
            abstract collapsed: bool with get, set
            /// The Jupyter metadata namespace
            abstract jupyter: obj with get, set
            /// Whether the cell's output is scrolled, unscrolled, or autoscrolled.
            abstract scrolled: U2<bool, string> with get, set

        /// A code cell.
        type [<AllowNullLiteral>] ICodeCell =
            inherit IBaseCell
            /// String identifying the type of cell.
            abstract cell_type: string with get, set
            /// Cell-level metadata.
            abstract metadata: obj with get, set
            /// Execution, display, or stream outputs.
            abstract outputs: ResizeArray<IOutput> with get, set
            /// The code cell's prompt number. Will be null if the cell has not been run.
            abstract execution_count: ExecutionCount with get, set

        /// An unrecognized cell.
        type [<AllowNullLiteral>] IUnrecognizedCell =
            inherit IBaseCell

        type ICell =
            U4<IRawCell, IMarkdownCell, ICodeCell, IUnrecognizedCell>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module ICell =
            let ofIRawCell v: ICell = v |> U4.Case1
            let isIRawCell (v: ICell) = match v with U4.Case1 _ -> true | _ -> false
            let asIRawCell (v: ICell) = match v with U4.Case1 o -> Some o | _ -> None
            let ofIMarkdownCell v: ICell = v |> U4.Case2
            let isIMarkdownCell (v: ICell) = match v with U4.Case2 _ -> true | _ -> false
            let asIMarkdownCell (v: ICell) = match v with U4.Case2 o -> Some o | _ -> None
            let ofICodeCell v: ICell = v |> U4.Case3
            let isICodeCell (v: ICell) = match v with U4.Case3 _ -> true | _ -> false
            let asICodeCell (v: ICell) = match v with U4.Case3 o -> Some o | _ -> None
            let ofIUnrecognizedCell v: ICell = v |> U4.Case4
            let isIUnrecognizedCell (v: ICell) = match v with U4.Case4 _ -> true | _ -> false
            let asIUnrecognizedCell (v: ICell) = match v with U4.Case4 o -> Some o | _ -> None

        type ICellMetadata =
            U3<IBaseCellMetadata, IRawCellMetadata, ICodeCellMetadata>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module ICellMetadata =
            let ofIBaseCellMetadata v: ICellMetadata = v |> U3.Case1
            let isIBaseCellMetadata (v: ICellMetadata) = match v with U3.Case1 _ -> true | _ -> false
            let asIBaseCellMetadata (v: ICellMetadata) = match v with U3.Case1 o -> Some o | _ -> None
            let ofIRawCellMetadata v: ICellMetadata = v |> U3.Case2
            let isIRawCellMetadata (v: ICellMetadata) = match v with U3.Case2 _ -> true | _ -> false
            let asIRawCellMetadata (v: ICellMetadata) = match v with U3.Case2 o -> Some o | _ -> None
            let ofICodeCellMetadata v: ICellMetadata = v |> U3.Case3
            let isICodeCellMetadata (v: ICellMetadata) = match v with U3.Case3 _ -> true | _ -> false
            let asICodeCellMetadata (v: ICellMetadata) = match v with U3.Case3 o -> Some o | _ -> None

        type [<StringEnum>] [<RequireQualifiedAccess>] OutputType =
            | Execute_result
            | Display_data
            | Stream
            | Error
            | Update_display_data

        /// The base output type.
        type [<AllowNullLiteral>] IBaseOutput =
            inherit JSONObject
            /// Type of cell output.
            abstract output_type: string with get, set

        /// Result of executing a code cell.
        type [<AllowNullLiteral>] IExecuteResult =
            inherit IBaseOutput
            /// Type of cell output.
            abstract output_type: string with get, set
            /// A result's prompt number.
            abstract execution_count: ExecutionCount with get, set
            /// A mime-type keyed dictionary of data.
            abstract data: IMimeBundle with get, set
            /// Cell output metadata.
            abstract metadata: OutputMetadata with get, set

        /// Data displayed as a result of code cell execution.
        type [<AllowNullLiteral>] IDisplayData =
            inherit IBaseOutput
            /// Type of cell output.
            abstract output_type: string with get, set
            /// A mime-type keyed dictionary of data.
            abstract data: IMimeBundle with get, set
            /// Cell output metadata.
            abstract metadata: OutputMetadata with get, set

        /// Data displayed as an update to existing display data.
        type [<AllowNullLiteral>] IDisplayUpdate =
            inherit IBaseOutput
            /// Type of cell output.
            abstract output_type: string with get, set
            /// A mime-type keyed dictionary of data.
            abstract data: IMimeBundle with get, set
            /// Cell output metadata.
            abstract metadata: OutputMetadata with get, set

        /// Stream output from a code cell.
        type [<AllowNullLiteral>] IStream =
            inherit IBaseOutput
            /// Type of cell output.
            abstract output_type: string with get, set
            /// The name of the stream.
            abstract name: StreamType with get, set
            /// The stream's text output.
            abstract text: MultilineString with get, set

        type [<StringEnum>] [<RequireQualifiedAccess>] StreamType =
            | Stdout
            | Stderr

        /// Output of an error that occurred during code cell execution.
        type [<AllowNullLiteral>] IError =
            inherit IBaseOutput
            /// Type of cell output.
            abstract output_type: string with get, set
            /// The name of the error.
            abstract ename: string with get, set
            /// The value, or message, of the error.
            abstract evalue: string with get, set
            /// The error's traceback.
            abstract traceback: ResizeArray<string> with get, set

        /// Unrecognized output.
        type [<AllowNullLiteral>] IUnrecognizedOutput =
            inherit IBaseOutput

        type IOutput =
            U5<IUnrecognizedOutput, IExecuteResult, IDisplayData, IStream, IError>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module IOutput =
            let ofIUnrecognizedOutput v: IOutput = v |> U5.Case1
            let isIUnrecognizedOutput (v: IOutput) = match v with U5.Case1 _ -> true | _ -> false
            let asIUnrecognizedOutput (v: IOutput) = match v with U5.Case1 o -> Some o | _ -> None
            let ofIExecuteResult v: IOutput = v |> U5.Case2
            let isIExecuteResult (v: IOutput) = match v with U5.Case2 _ -> true | _ -> false
            let asIExecuteResult (v: IOutput) = match v with U5.Case2 o -> Some o | _ -> None
            let ofIDisplayData v: IOutput = v |> U5.Case3
            let isIDisplayData (v: IOutput) = match v with U5.Case3 _ -> true | _ -> false
            let asIDisplayData (v: IOutput) = match v with U5.Case3 o -> Some o | _ -> None
            let ofIStream v: IOutput = v |> U5.Case4
            let isIStream (v: IOutput) = match v with U5.Case4 _ -> true | _ -> false
            let asIStream (v: IOutput) = match v with U5.Case4 o -> Some o | _ -> None
            let ofIError v: IOutput = v |> U5.Case5
            let isIError (v: IOutput) = match v with U5.Case5 _ -> true | _ -> false
            let asIError (v: IOutput) = match v with U5.Case5 o -> Some o | _ -> None

module Pageconfig =

    //amo
    type RegExp = System.Text.RegularExpressions.Regex

    // [<Pojo>] //Pojo gone in 2 beta? https://fable.io/blog/Introducing-2-0-beta.html
    type PathFormatDownload =
        {
            path : string
            format : string
            download : string
        }
        
    module PageConfig =
        let [<Import("Extension","@jupyterlab/coreutils/lib/pageconfig/pageconfig/PageConfig")>] extension: Extension.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            /// <summary>Get global configuration data for the Jupyter application.</summary>
            /// <param name="name">- The name of the configuration option.</param>
            abstract getOption: name: string -> string
            /// <summary>Set global configuration data for the Jupyter application.</summary>
            /// <param name="name">- The name of the configuration option.</param>
            /// <param name="value">- The value to set the option to.</param>
            abstract setOption: name: string * value: string -> string
            /// Get the base url for a Jupyter application, or the base url of the page.
            abstract getBaseUrl: unit -> string
            /// Get the tree url for a JupyterLab application.
            abstract getTreeUrl: unit -> string
            /// Get the base websocket url for a Jupyter application, or an empty string.
            abstract getWsUrl: ?baseUrl: string -> string
            /// Returns the URL converting this notebook to a certain
            /// format with nbconvert.
            //abstract getNBConvertURL: { path, format, download }: GetNBConvertURL{ path, format, download } -> string
            //amo may be wrong
            abstract getNBConvertURL: pathFormatDownload: PathFormatDownload -> string
            /// Get the authorization token for a Jupyter application.
            abstract getToken: unit -> string
            /// Get the Notebook version info [major, minor, patch].
            abstract getNotebookVersion: unit -> float * float * float

        type [<AllowNullLiteral>] GetNBConvertURL = //{ path, format, download } =
            abstract path: string with get, set
            abstract format: string with get, set
            abstract download: bool with get, set

        module Extension =

            type [<AllowNullLiteral>] IExports =
                abstract deferred: ResizeArray<TypeLiteral_01>
                abstract disabled: ResizeArray<TypeLiteral_01>
                /// <summary>Returns whether a plugin is deferred.</summary>
                /// <param name="id">- The plugin ID.</param>
                abstract isDeferred: id: string -> bool
                /// <summary>Returns whether a plugin is disabled.</summary>
                /// <param name="id">- The plugin ID.</param>
                abstract isDisabled: id: string -> bool

            type [<AllowNullLiteral>] TypeLiteral_01 =
                abstract raw: string with get, set
                abstract rule: RegExp with get, set

module Path =

    module PathExt =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Join all arguments together and normalize the resulting path.
            /// Arguments must be strings. In v0.8, non-string arguments were silently ignored. In v0.10 and up, an exception is thrown.</summary>
            /// <param name="paths">- The string paths to join.</param>
            abstract join: [<ParamArray>] paths: ResizeArray<string> -> string
            /// <summary>Return the last portion of a path. Similar to the Unix basename command.
            /// Often used to extract the file name from a fully qualified path.</summary>
            /// <param name="path">- The path to evaluate.</param>
            /// <param name="ext">- An extension to remove from the result.</param>
            abstract basename: path: string * ?ext: string -> string
            /// <summary>Get the directory name of a path, similar to the Unix dirname command.
            /// When an empty path is given, returns the root path.</summary>
            /// <param name="path">- The file path.</param>
            abstract dirname: path: string -> string
            /// <summary>Get the extension of the path.</summary>
            /// <param name="path">- The file path.</param>
            abstract extname: path: string -> string
            /// <summary>Get the last portion of a path, without its extension (if any).</summary>
            /// <param name="path">- The file path.</param>
            abstract stem: path: string -> string
            /// <summary>Normalize a string path, reducing '..' and '.' parts.
            /// When multiple slashes are found, they're replaced by a single one; when the path contains a trailing slash, it is preserved. On Windows backslashes are used.
            /// When an empty path is given, returns the root path.</summary>
            /// <param name="path">- The string path to normalize.</param>
            abstract normalize: path: string -> string
            /// <summary>Resolve a sequence of paths or path segments into an absolute path.
            /// The root path in the application has no leading slash, so it is removed.</summary>
            /// <param name="parts">- The paths to join.
            /// 
            /// #### Notes
            /// The right-most parameter is considered {to}.  Other parameters are considered an array of {from}.
            /// 
            /// Starting from leftmost {from} parameter, resolves {to} to an absolute path.
            /// 
            /// If {to} isn't already absolute, {from} arguments are prepended in right to left order, until an absolute path is found. If after using all {from} paths still no absolute path is found, the current working directory is used as well. The resulting path is normalized, and trailing slashes are removed unless the path gets resolved to the root directory.</param>
            abstract resolve: [<ParamArray>] parts: ResizeArray<string> -> string
            /// <summary>Solve the relative path from {from} to {to}.</summary>
            /// <param name="from">- The source path.</param>
            /// <param name="to">- The target path.
            /// 
            /// #### Notes
            /// If from and to each resolve to the same path (after calling
            /// path.resolve() on each), a zero-length string is returned.
            /// If a zero-length string is passed as from or to, `/`
            /// will be used instead of the zero-length strings.</param>
            abstract relative: from: string * ``to``: string -> string
            /// <summary>Normalize a file extension to be of the type `'.foo'`.</summary>
            /// <param name="extension">- the file extension.
            /// 
            /// #### Notes
            /// Adds a leading dot if not present and converts to lower case.</param>
            abstract normalizeExtension: extension: string -> string
            /// <summary>Remove the leading slash from a path.</summary>
            /// <param name="path">: the path from which to remove a leading slash.</param>
            abstract removeSlash: path: string -> string

module Poll =
    type IObservableDisposable = PhosphorDisposable.IObservableDisposable // __@phosphor_disposable.IObservableDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IPoll<'T, 'U, 'V> = Interfaces.IPoll<'T, 'U, 'V>

    type [<AllowNullLiteral>] IExports =
        abstract Poll: PollStatic

    type Poll<'U, 'V> =
        Poll<obj, 'U, 'V>

    type Poll<'V> =
        Poll<obj, obj, 'V>

    type Poll =
        Poll<obj, obj, obj>

    /// A class that wraps an asynchronous function to poll at a regular interval
    /// with exponential increases to the interval length if the poll fails.
    /// A namespace for `Poll` types, interfaces, and statics.
    type [<AllowNullLiteral>] Poll<'T, 'U, 'V> =
        inherit IObservableDisposable
        inherit IPoll<'T, 'U, 'V>
        /// The name of the poll.
        abstract name: string
        /// A signal emitted when the poll is disposed.
        abstract disposed: ISignal<Poll<'T, 'U, 'V>, unit>
        /// The polling frequency parameters.
        abstract frequency: Interfaces.IPoll.Frequency with get, set
        /// Whether the poll is disposed.
        abstract isDisposed: bool
        /// Indicates when the poll switches to standby.
        abstract standby: U2<Poll.Standby, (unit -> U2<bool, Poll.Standby>)> with get, set
        /// The poll state, which is the content of the current poll tick.
        abstract state: Interfaces.IPoll.State<'T, 'U, 'V>
        /// A promise that resolves when the poll next ticks.
        abstract tick: Promise<Poll<'T, 'U, 'V>>
        /// A signal emitted when the poll ticks and fires off a new request.
        abstract ticked: ISignal<Poll<'T, 'U, 'V>, Interfaces.IPoll.State<'T, 'U, 'V>>
        /// Dispose the poll.
        abstract dispose: unit -> unit
        /// Refreshes the poll. Schedules `refreshed` tick if necessary.
        abstract refresh: unit -> Promise<unit>
        /// <summary>Schedule the next poll tick.</summary>
        /// <param name="next">- The next poll state data to schedule. Defaults to standby.</param>
        abstract schedule: ?next: obj -> Promise<unit>
        /// Starts the poll. Schedules `started` tick if necessary.
        abstract start: unit -> Promise<unit>
        /// Stops the poll. Schedules `stopped` tick if necessary.
        abstract stop: unit -> Promise<unit>

    /// A class that wraps an asynchronous function to poll at a regular interval
    /// with exponential increases to the interval length if the poll fails.
    /// A namespace for `Poll` types, interfaces, and statics.
    type [<AllowNullLiteral>] PollStatic =
        /// <summary>Instantiate a new poll with exponential backoff in case of failure.</summary>
        /// <param name="options">- The poll instantiation options.</param>
        [<Emit "new $0($1...)">] abstract Create: options: Poll.IOptions<'T, 'U, 'V> -> Poll<'T, 'U, 'V>

    module Poll =

        type [<AllowNullLiteral>] IExports =
            abstract IMMEDIATE: obj
            abstract MAX_INTERVAL: obj
            abstract NEVER: float

        type [<AllowNullLiteral>] Factory<'T, 'U, 'V> =
            [<Emit "$0($1...)">] abstract Invoke: state: Interfaces.IPoll.State<'T, 'U, 'V> -> Promise<'T>

        type [<StringEnum>] [<RequireQualifiedAccess>] Standby =
            | Never
            | [<CompiledName "when-hidden">] WhenHidden

        /// Instantiation options for polls.
        type [<AllowNullLiteral>] IOptions<'T, 'U, 'V> =
            /// Whether to begin polling automatically; defaults to `true`.
            abstract auto: bool option with get, set
            /// A factory function that is passed a poll tick and returns a poll promise.
            abstract factory: Factory<'T, 'U, 'V> with get, set
            /// The polling frequency parameters.
            abstract frequency: obj option with get, set
            /// The name of the poll.
            /// Defaults to `'unknown'`.
            abstract name: string option with get, set
            /// Indicates when the poll switches to standby or a function that returns
            /// a boolean or a `Poll.Standby` value to indicate whether to stand by.
            /// Defaults to `'when-hidden'`.
            /// 
            /// #### Notes
            /// If a function is passed in, for any given context, it should be
            /// idempotent and safe to call multiple times. It will be called before each
            /// tick execution, but may be called by clients as well.
            abstract standby: U2<Standby, (unit -> U2<bool, Standby>)> option with get, set

module Ratelimiter =
    type PromiseDelegate<'T> = PhosphorCoreutils.PromiseDelegate<'T> // __@phosphor_coreutils.PromiseDelegate
    type IRateLimiter = Interfaces.IRateLimiter
    type Poll = Poll.Poll

    type [<AllowNullLiteral>] IExports =
        abstract RateLimiter: RateLimiterStatic
        abstract Debouncer: DebouncerStatic
        abstract Throttler: ThrottlerStatic

    /// A base class to implement rate limiters with different invocation strategies.
    type [<AllowNullLiteral>] RateLimiter<'T, 'U> =
        inherit Interfaces.IRateLimiter<'T, 'U>
        /// Whether the rate limiter is disposed.
        abstract isDisposed: bool
        /// Disposes the rate limiter.
        abstract dispose: unit -> unit
        /// The rate limit in milliseconds.
        abstract limit: float
        /// Invoke the rate limited function.
        abstract invoke: unit -> Promise<'T>
        /// Stop the function if it is mid-flight.
        abstract stop: unit -> Promise<unit>
        /// A promise that resolves on each successful invocation.
        abstract payload: PromiseDelegate<'T> option with get, set
        /// The underlying poll instance used by the rate limiter.
        abstract poll: Poll.Poll<'T, 'U, string> with get, set

    /// A base class to implement rate limiters with different invocation strategies.
    type [<AllowNullLiteral>] RateLimiterStatic =
        /// <summary>Instantiate a rate limiter.</summary>
        /// <param name="fn">- The function to rate limit.</param>
        /// <param name="limit">- The rate limit; defaults to 500ms.</param>
        [<Emit "new $0($1...)">] abstract Create: fn: (unit -> U2<'T, Promise<'T>>) * ?limit: float -> RateLimiter<'T, 'U>

    type Debouncer<'U> =
        Debouncer<obj, 'U>

    type Debouncer =
        Debouncer<obj, obj>

    /// Wraps and debounces a function that can be called multiple times and only
    /// executes the underlying function one `interval` after the last invocation.
    type [<AllowNullLiteral>] Debouncer<'T, 'U> =
        inherit RateLimiter<'T, 'U>
        /// Invokes the function and only executes after rate limit has elapsed.
        /// Each invocation resets the timer.
        abstract invoke: unit -> Promise<'T>

    /// Wraps and debounces a function that can be called multiple times and only
    /// executes the underlying function one `interval` after the last invocation.
    type [<AllowNullLiteral>] DebouncerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Debouncer<'T, 'U>

    type Throttler<'U> =
        Throttler<obj, 'U>

    type Throttler =
        Throttler<obj, obj>

    /// Wraps and throttles a function that can be called multiple times and only
    /// executes the underlying function once per `interval`.
    type [<AllowNullLiteral>] Throttler<'T, 'U> =
        inherit RateLimiter<'T, 'U>
        /// Throttles function invocations if one is currently in flight.
        abstract invoke: unit -> Promise<'T>

    /// Wraps and throttles a function that can be called multiple times and only
    /// executes the underlying function once per `interval`.
    type [<AllowNullLiteral>] ThrottlerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Throttler<'T, 'U>

module Restorablepool =
    type IObservableDisposable = PhosphorDisposable.IObservableDisposable // __@phosphor_disposable.IObservableDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IObjectPool<'T> = Interfaces.IObjectPool<'T>
    type IRestorable<'T> = Interfaces.IRestorable<'T>

    type [<AllowNullLiteral>] IExports =
        abstract RestorablePool: RestorablePoolStatic

    type RestorablePool =
        RestorablePool<obj>

    /// An object pool that supports restoration.
    /// A namespace for `RestorablePool` statics.
    type [<AllowNullLiteral>] RestorablePool<'T> =
        inherit IObjectPool<'T>
        inherit IRestorable<'T>
        /// A namespace for all tracked objects.
        abstract ``namespace``: string
        /// A signal emitted when an object object is added.
        /// 
        /// #### Notes
        /// This signal will only fire when an object is added to the pool.
        /// It will not fire if an object injected into the pool.
        abstract added: ISignal<RestorablePool<'T>, 'T>
        /// The current object.
        /// 
        /// #### Notes
        /// The restorable pool does not set `current`. It is intended for client use.
        /// 
        /// If `current` is set to an object that does not exist in the pool, it is a
        /// no-op.
        abstract current: 'T option with get, set
        /// A signal emitted when the current widget changes.
        abstract currentChanged: ISignal<RestorablePool<'T>, 'T option>
        /// Test whether the pool is disposed.
        abstract isDisposed: bool
        /// A promise resolved when the restorable pool has been restored.
        abstract restored: Promise<unit>
        /// The number of objects held by the pool.
        abstract size: float
        /// A signal emitted when an object is updated.
        abstract updated: ISignal<RestorablePool<'T>, 'T>
        /// <summary>Add a new object to the pool.</summary>
        /// <param name="obj">- The object object being added.
        /// 
        /// #### Notes
        /// The object passed into the tracker is added synchronously; its existence in
        /// the tracker can be checked with the `has()` method. The promise this method
        /// returns resolves after the object has been added and saved to an underlying
        /// restoration connector, if one is available.</param>
        abstract add: obj: 'T -> Promise<unit>
        /// Dispose of the resources held by the pool.
        /// 
        /// #### Notes
        /// Disposing a pool does not affect the underlying data in the data connector,
        /// it simply disposes the client-side pool without making any connector calls.
        abstract dispose: unit -> unit
        /// Find the first object in the pool that satisfies a filter function.
        abstract find: fn: ('T -> bool) -> 'T option
        /// <summary>Iterate through each object in the pool.</summary>
        /// <param name="fn">- The function to call on each object.</param>
        abstract forEach: fn: ('T -> unit) -> unit
        /// <summary>Filter the objects in the pool based on a predicate.</summary>
        /// <param name="fn">- The function by which to filter.</param>
        abstract filter: fn: ('T -> bool) -> ResizeArray<'T>
        /// <summary>Inject an object into the restorable pool without the pool handling its
        /// restoration lifecycle.</summary>
        /// <param name="obj">- The object to inject into the pool.</param>
        abstract inject: obj: 'T -> Promise<unit>
        /// <summary>Check if this pool has the specified object.</summary>
        /// <param name="obj">- The object whose existence is being checked.</param>
        abstract has: obj: 'T -> bool
        /// <summary>Restore the objects in this pool's namespace.</summary>
        /// <param name="options">- The configuration options that describe restoration.</param>
        abstract restore: options: Interfaces.IRestorable.IOptions<'T> -> Promise<obj option>
        /// <summary>Save the restore data for a given object.</summary>
        /// <param name="obj">- The object being saved.</param>
        abstract save: obj: 'T -> Promise<unit>

    /// An object pool that supports restoration.
    /// A namespace for `RestorablePool` statics.
    type [<AllowNullLiteral>] RestorablePoolStatic =
        /// <summary>Create a new restorable pool.</summary>
        /// <param name="options">- The instantiation options for a restorable pool.</param>
        [<Emit "new $0($1...)">] abstract Create: options: RestorablePool.IOptions -> RestorablePool<'T>

    module RestorablePool =

        /// The instantiation options for the restorable pool.
        type [<AllowNullLiteral>] IOptions =
            /// A namespace designating objects from this pool.
            abstract ``namespace``: string with get, set

module Settingregistry =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type JSONValue = PhosphorCoreutils.JSONValue //__@phosphor_coreutils.JSONValue
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject //__@phosphor_coreutils.ReadonlyJSONObject
    type ReadonlyJSONValue = PhosphorCoreutils.ReadonlyJSONValue //__@phosphor_coreutils.ReadonlyJSONValue
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IDataConnector<'T,'U,'V> = Interfaces.IDataConnector<'T,'U,'V>
    type ISettingRegistry = Tokens.ISettingRegistry

    type [<AllowNullLiteral>] IExports =
        abstract DefaultSchemaValidator: DefaultSchemaValidatorStatic
        abstract SettingRegistry: SettingRegistryStatic
        abstract Settings: SettingsStatic

    /// An implementation of a schema validator.
    /// A namespace for schema validator interfaces.
    type [<AllowNullLiteral>] ISchemaValidator =
        /// <summary>Validate a plugin's schema and user data; populate the `composite` data.</summary>
        /// <param name="plugin">- The plugin being validated. Its `composite` data will be
        /// populated by reference.</param>
        /// <param name="populate">- Whether plugin data should be populated, defaults to
        /// `true`.</param>
        abstract validateData: plugin: Tokens.ISettingRegistry.IPlugin * ?populate: bool -> ResizeArray<ISchemaValidator.IError> option

    module ISchemaValidator =

        /// A schema validation error definition.
        type [<AllowNullLiteral>] IError =
            /// The path in the data where the error occurred.
            abstract dataPath: string with get, set
            /// The keyword whose validation failed.
            abstract keyword: string with get, set
            /// The error message.
            abstract message: string with get, set
            /// Optional parameter metadata that might be included in an error.
            abstract ``params``: ReadonlyJSONObject option with get, set
            /// The path in the schema where the error occurred.
            abstract schemaPath: string with get, set

    /// The default implementation of a schema validator.
    type [<AllowNullLiteral>] DefaultSchemaValidator =
        inherit ISchemaValidator
        /// <summary>Validate a plugin's schema and user data; populate the `composite` data.</summary>
        /// <param name="plugin">- The plugin being validated. Its `composite` data will be
        /// populated by reference.</param>
        /// <param name="populate">- Whether plugin data should be populated, defaults to
        /// `true`.</param>
        abstract validateData: plugin: Tokens.ISettingRegistry.IPlugin * ?populate: bool -> ResizeArray<ISchemaValidator.IError> option

    /// The default implementation of a schema validator.
    type [<AllowNullLiteral>] DefaultSchemaValidatorStatic =
        /// Instantiate a schema validator.
        [<Emit "new $0($1...)">] abstract Create: unit -> DefaultSchemaValidator

    /// The default concrete implementation of a setting registry.
    /// A namespace for `SettingRegistry` statics.
    type [<AllowNullLiteral>] SettingRegistry =
        inherit ISettingRegistry
        /// The data connector used by the setting registry.
        abstract connector: IDataConnector<Tokens.ISettingRegistry.IPlugin, string, string>
        /// The schema of the setting registry.
        abstract schema: Tokens.ISettingRegistry.ISchema
        /// The schema validator used by the setting registry.
        abstract validator: ISchemaValidator
        /// A signal that emits the name of a plugin when its settings change.
        abstract pluginChanged: ISignal<SettingRegistry, string>
        /// The collection of setting registry plugins.
        abstract plugins: TypeLiteral_01
        /// <summary>Get an individual setting.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being retrieved.</param>
        /// <param name="key">- The name of the setting being retrieved.</param>
        abstract get: plugin: string * key: string -> Promise<TypeLiteral_02>
        /// <summary>Load a plugin's settings into the setting registry.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being loaded.</param>
        abstract load: plugin: string -> Promise<Tokens.ISettingRegistry.ISettings>
        /// <summary>Reload a plugin's settings into the registry even if they already exist.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being reloaded.</param>
        abstract reload: plugin: string -> Promise<Tokens.ISettingRegistry.ISettings>
        /// <summary>Remove a single setting in the registry.</summary>
        /// <param name="plugin">- The name of the plugin whose setting is being removed.</param>
        /// <param name="key">- The name of the setting being removed.</param>
        abstract remove: plugin: string * key: string -> Promise<unit>
        /// <summary>Set a single setting in the registry.</summary>
        /// <param name="plugin">- The name of the plugin whose setting is being set.</param>
        /// <param name="key">- The name of the setting being set.</param>
        /// <param name="value">- The value of the setting being set.</param>
        abstract set: plugin: string * key: string * value: JSONValue -> Promise<unit>
        /// <summary>Register a plugin transform function to act on a specific plugin.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are transformed.</param>
        /// <param name="transforms">- The transform functions applied to the plugin.</param>
        abstract transform: plugin: string * transforms: obj -> IDisposable
        /// <summary>Upload a plugin's settings.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being set.</param>
        /// <param name="raw">- The raw plugin settings being uploaded.</param>
        abstract upload: plugin: string * raw: string -> Promise<unit>

    /// The default concrete implementation of a setting registry.
    /// A namespace for `SettingRegistry` statics.
    type [<AllowNullLiteral>] SettingRegistryStatic =
        /// Create a new setting registry.
        [<Emit "new $0($1...)">] abstract Create: options: SettingRegistry.IOptions -> SettingRegistry

    /// A manager for a specific plugin's settings.
    /// A namespace for `Settings` statics.
    type [<AllowNullLiteral>] Settings =
        inherit Tokens.ISettingRegistry.ISettings
        /// The plugin name.
        abstract id: string
        /// The setting registry instance used as a back-end for these settings.
        abstract registry: ISettingRegistry
        /// A signal that emits when the plugin's settings have changed.
        abstract changed: ISignal<Settings, unit>
        /// The composite of user settings and extension defaults.
        abstract composite: ReadonlyJSONObject
        /// Test whether the plugin settings manager disposed.
        abstract isDisposed: bool
        abstract plugin: Tokens.ISettingRegistry.IPlugin
        /// The plugin's schema.
        abstract schema: Tokens.ISettingRegistry.ISchema
        /// The plugin settings raw text value.
        abstract raw: string
        /// The user settings.
        abstract user: ReadonlyJSONObject
        /// The published version of the NPM package containing these settings.
        abstract version: string
        /// Return the defaults in a commented JSON format.
        abstract annotatedDefaults: unit -> string
        /// <summary>Calculate the default value of a setting by iterating through the schema.</summary>
        /// <param name="key">- The name of the setting whose default value is calculated.</param>
        abstract ``default``: key: string -> JSONValue option
        /// Dispose of the plugin settings resources.
        abstract dispose: unit -> unit
        /// <summary>Get an individual setting.</summary>
        /// <param name="key">- The name of the setting being retrieved.</param>
        abstract get: key: string -> SettingsGetReturn
        /// <summary>Remove a single setting.</summary>
        /// <param name="key">- The name of the setting being removed.</param>
        abstract remove: key: string -> Promise<unit>
        /// Save all of the plugin's user settings at once.
        abstract save: raw: string -> Promise<unit>
        /// <summary>Set a single setting.</summary>
        /// <param name="key">- The name of the setting being set.</param>
        /// <param name="value">- The value of the setting.</param>
        abstract set: key: string * value: JSONValue -> Promise<unit>
        /// <summary>Validates raw settings with comments.</summary>
        /// <param name="raw">- The JSON with comments string being validated.</param>
        abstract validate: raw: string -> ResizeArray<ISchemaValidator.IError> option

    type [<AllowNullLiteral>] SettingsGetReturn =
        abstract composite: ReadonlyJSONValue with get, set
        abstract user: ReadonlyJSONValue with get, set

    /// A manager for a specific plugin's settings.
    /// A namespace for `Settings` statics.
    type [<AllowNullLiteral>] SettingsStatic =
        /// Instantiate a new plugin settings manager.
        [<Emit "new $0($1...)">] abstract Create: options: Settings.IOptions -> Settings

    module SettingRegistry =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Reconcile default and user shortcuts and return the composite list.</summary>
            /// <param name="defaults">- The list of default shortcuts.</param>
            /// <param name="user">- The list of user shortcut overrides and additions.</param>
            abstract reconcileShortcuts: defaults: ResizeArray<Tokens.ISettingRegistry.IShortcut> * user: ResizeArray<Tokens.ISettingRegistry.IShortcut> -> ResizeArray<Tokens.ISettingRegistry.IShortcut>

        /// The instantiation options for a setting registry
        type [<AllowNullLiteral>] IOptions =
            /// The data connector used by the setting registry.
            abstract connector: Interfaces.IDataConnector<Tokens.ISettingRegistry.IPlugin, string> with get, set
            /// Preloaded plugin data to populate the setting registry.
            abstract plugins: ResizeArray<Tokens.ISettingRegistry.IPlugin> option with get, set
            /// The number of milliseconds before a `load()` call to the registry waits
            /// before timing out if it requires a transformation that has not been
            /// registered.
            /// 
            /// #### Notes
            /// The default value is 7000.
            abstract timeout: float option with get, set
            /// The validator used to enforce the settings JSON schema.
            abstract validator: ISchemaValidator option with get, set

    module Settings =

        /// The instantiation options for a `Settings` object.
        type [<AllowNullLiteral>] IOptions =
            /// The setting values for a plugin.
            abstract plugin: Tokens.ISettingRegistry.IPlugin with get, set
            /// The system registry instance used by the settings manager.
            abstract registry: ISettingRegistry with get, set

    module Private =

        type [<AllowNullLiteral>] IExports =
            /// Returns an annotated (JSON with comments) version of a schema's defaults.
            abstract annotatedDefaults: schema: Tokens.ISettingRegistry.ISchema * plugin: string -> string
            /// Returns an annotated (JSON with comments) version of a plugin's
            /// setting data.
            abstract annotatedPlugin: plugin: Tokens.ISettingRegistry.IPlugin * data: JSONObject -> string
            /// Create a fully extrapolated default value for a root key in a schema.
            abstract reifyDefault: schema: Tokens.ISettingRegistry.IProperty * ?root: string -> JSONValue option

    type [<AllowNullLiteral>] TypeLiteral_02 =
        abstract composite: JSONValue with get, set
        abstract user: JSONValue with get, set

    type [<AllowNullLiteral>] TypeLiteral_01 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> Tokens.ISettingRegistry.IPlugin with get, set

module Statedb =
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type ReadonlyJSONValue = PhosphorCoreutils.ReadonlyJSONValue // __@phosphor_coreutils.ReadonlyJSONValue
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IDataConnector<'T> = Interfaces.IDataConnector<'T>
    type IStateDB = Tokens.IStateDB

    type [<AllowNullLiteral>] IExports =
        abstract StateDB: StateDBStatic

    type StateDB =
        StateDB<obj>

    /// The default concrete implementation of a state database.
    /// A namespace for StateDB statics.
    type [<AllowNullLiteral>] StateDB<'T> =
        inherit Tokens.IStateDB<'T>
        /// A signal that emits the change type any time a value changes.
        abstract changed: ISignal<StateDB<'T>, StateDB.Change>
        /// Clear the entire database.
        abstract clear: unit -> Promise<unit>
        /// <summary>Retrieve a saved bundle from the database.</summary>
        /// <param name="id">- The identifier used to retrieve a data bundle.</param>
        abstract fetch: id: string -> Promise<'T>
        /// Retrieve all the saved bundles for a namespace.
        abstract list: ``namespace``: string -> Promise<TypeLiteral_02<'T>>
        /// <summary>Remove a value from the database.</summary>
        /// <param name="id">- The identifier for the data being removed.</param>
        abstract remove: id: string -> Promise<unit>
        /// <summary>Save a value in the database.</summary>
        /// <param name="id">- The identifier for the data being saved.</param>
        /// <param name="value">- The data being saved.</param>
        abstract save: id: string * value: 'T -> Promise<unit>
        /// Return a serialized copy of the state database's entire contents.
        abstract toJSON: unit -> Promise<TypeLiteral_03<'T>>

    /// The default concrete implementation of a state database.
    /// A namespace for StateDB statics.
    type [<AllowNullLiteral>] StateDBStatic =
        /// <summary>Create a new state database.</summary>
        /// <param name="options">- The instantiation options for a state database.</param>
        [<Emit "new $0($1...)">] abstract Create: ?options: StateDB.IOptions -> StateDB<'T>

    module StateDB =

        type [<AllowNullLiteral>] IExports =
            abstract Connector: ConnectorStatic

        type [<AllowNullLiteral>] Change =
            /// The key of the database item that was changed.
            /// 
            /// #### Notes
            /// This field is set to `null` for global changes (i.e. `clear`).
            abstract id: string option with get, set
            /// The type of change.
            abstract ``type``: U3<string, string, string> with get, set

        type [<AllowNullLiteral>] DataTransform =
            abstract ``type``: U4<string, string, string, string> with get, set
            /// The contents of the change operation.
            abstract contents: ReadonlyJSONObject option with get, set

        /// The instantiation options for a state database.
        type [<AllowNullLiteral>] IOptions =
            /// Optional string key/value connector. Defaults to in-memory connector.
            abstract connector: IDataConnector<string> option with get, set
            /// An optional promise that resolves with a data transformation that is
            /// applied to the database contents before the database begins resolving
            /// client requests.
            abstract transform: Promise<DataTransform> option with get, set

        /// An in-memory string key/value data connector.
        type [<AllowNullLiteral>] Connector =
            inherit IDataConnector<string>
            /// Retrieve an item from the data connector.
            abstract fetch: id: string -> Promise<string>
            /// Retrieve the list of items available from the data connector.
            abstract list: ?query: string -> Promise<TypeLiteral_01>
            /// Remove a value using the data connector.
            abstract remove: id: string -> Promise<unit>
            /// Save a value using the data connector.
            abstract save: id: string * value: string -> Promise<unit>

        /// An in-memory string key/value data connector.
        type [<AllowNullLiteral>] ConnectorStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> Connector

        type [<AllowNullLiteral>] TypeLiteral_01 =
            abstract ids: ResizeArray<string> with get, set
            abstract values: ResizeArray<string> with get, set

    type [<AllowNullLiteral>] TypeLiteral_02<'T> =
        abstract ids: ResizeArray<string> with get, set
        abstract values: ResizeArray<'T> with get, set

    type [<AllowNullLiteral>] TypeLiteral_03<'T> =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: id: string -> 'T

module Text =

    module Text =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Convert a javascript string index into a unicode character offset</summary>
            /// <param name="jsIdx">- The javascript string index (counting surrogate pairs)</param>
            /// <param name="text">- The text in which the offset is calculated</param>
            abstract jsIndexToCharIndex: jsIdx: float * text: string -> float
            /// <summary>Convert a unicode character offset to a javascript string index.</summary>
            /// <param name="charIdx">- The index in unicode characters</param>
            /// <param name="text">- The text in which the offset is calculated</param>
            abstract charIndexToJsIndex: charIdx: float * text: string -> float
            /// <summary>Given a 'snake-case', 'snake_case', or 'snake case' string,
            /// will return the camel case version: 'snakeCase'.</summary>
            /// <param name="str">: the snake-case input string.</param>
            /// <param name="upper">: default = false. If true, the first letter of the
            /// returned string will be capitalized.</param>
            abstract camelCase: str: string * ?upper: bool -> string
            /// <summary>Given a string, title case the words in the string.</summary>
            /// <param name="str">: the string to title case.</param>
            abstract titleCase: str: string -> string

module Time =

    module Time =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Convert a timestring to a human readable string (e.g. 'two minutes ago').</summary>
            /// <param name="value">- The date timestring or date object.</param>
            abstract formatHuman: value: U2<string, DateTime> -> string
            /// <summary>Convert a timestring to a date format.</summary>
            /// <param name="value">- The date timestring or date object.</param>
            /// <param name="format">- The format string.</param>
            abstract format: value: U2<string, DateTime> * ?format: string -> string

module Tokens =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // __@phosphor_coreutils.ReadonlyJSONObject
    type ReadonlyJSONValue = PhosphorCoreutils.ReadonlyJSONValue // __@phosphor_coreutils.ReadonlyJSONValue
    type Token<'T> = PhosphorCoreutils.Token<'T> // __@phosphor_coreutils.Token
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type ISchemaValidator = Settingregistry.ISchemaValidator
    type IDataConnector<'T,'U,'V> = Interfaces.IDataConnector<'T,'U,'V>

    type [<AllowNullLiteral>] IExports =
        abstract ISettingRegistry: Token<ISettingRegistry>
        abstract IStateDB: Token<IStateDB<ReadonlyJSONValue>>

    /// The setting registry token.
    /// The settings registry interface.
    /// A namespace for setting registry interfaces.
    type [<AllowNullLiteral>] ISettingRegistry =
        /// The data connector used by the setting registry.
        abstract connector: IDataConnector<ISettingRegistry.IPlugin, string, string>
        /// The schema of the setting registry.
        abstract schema: ISettingRegistry.ISchema
        /// The schema validator used by the setting registry.
        abstract validator: ISchemaValidator
        /// A signal that emits the name of a plugin when its settings change.
        abstract pluginChanged: ISignal<ISettingRegistry, string>
        /// The collection of setting registry plugins.
        abstract plugins: TypeLiteral_02
        /// <summary>Get an individual setting.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being retrieved.</param>
        /// <param name="key">- The name of the setting being retrieved.</param>
        abstract get: plugin: string * key: string -> Promise<TypeLiteral_03>
        /// <summary>Load a plugin's settings into the setting registry.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being loaded.</param>
        abstract load: plugin: string -> Promise<ISettingRegistry.ISettings>
        /// <summary>Reload a plugin's settings into the registry even if they already exist.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being reloaded.</param>
        abstract reload: plugin: string -> Promise<ISettingRegistry.ISettings>
        /// <summary>Remove a single setting in the registry.</summary>
        /// <param name="plugin">- The name of the plugin whose setting is being removed.</param>
        /// <param name="key">- The name of the setting being removed.</param>
        abstract remove: plugin: string * key: string -> Promise<unit>
        /// <summary>Set a single setting in the registry.</summary>
        /// <param name="plugin">- The name of the plugin whose setting is being set.</param>
        /// <param name="key">- The name of the setting being set.</param>
        /// <param name="value">- The value of the setting being set.</param>
        abstract set: plugin: string * key: string * value: JSONValue -> Promise<unit>
        /// <summary>Register a plugin transform function to act on a specific plugin.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are transformed.</param>
        /// <param name="transforms">- The transform functions applied to the plugin.</param>
        abstract transform: plugin: string * transforms: obj -> IDisposable
        /// <summary>Upload a plugin's settings.</summary>
        /// <param name="plugin">- The name of the plugin whose settings are being set.</param>
        /// <param name="raw">- The raw plugin settings being uploaded.</param>
        abstract upload: plugin: string * raw: string -> Promise<unit>

    module ISettingRegistry =

        type [<StringEnum>] [<RequireQualifiedAccess>] Primitive =
            | Array
            | Boolean
            | Null
            | Number
            | Object
            | String

        /// The settings for a specific plugin.
        /// A namespace for plugin functionality.
        type [<AllowNullLiteral>] IPlugin =
            inherit JSONObject
            /// The name of the plugin.
            abstract id: string with get, set
            /// The collection of values for a specified plugin.
            abstract data: ISettingBundle with get, set
            /// The raw user settings data as a string containing JSON with comments.
            abstract raw: string with get, set
            /// The JSON schema for the plugin.
            abstract schema: ISchema with get, set
            /// The published version of the NPM package containing the plugin.
            abstract version: string with get, set

        module IPlugin =

            type [<AllowNullLiteral>] Transform =
                [<Emit "$0($1...)">] abstract Invoke: plugin: IPlugin -> IPlugin

            type [<StringEnum>] [<RequireQualifiedAccess>] Phase =
                | Compose
                | Fetch

        /// A minimal subset of the formal JSON Schema that describes a property.
        type [<AllowNullLiteral>] IProperty =
            inherit JSONObject
            /// The default value, if any.
            abstract ``default``: obj option with get, set
            /// The schema description.
            abstract description: string option with get, set
            /// The schema's child properties.
            abstract properties: TypeLiteral_01 option with get, set
            /// The title of a property.
            abstract title: string option with get, set
            /// The type or types of the data.
            abstract ``type``: U2<Primitive, ResizeArray<Primitive>> option with get, set

        /// A schema type that is a minimal subset of the formal JSON Schema along with
        /// optional JupyterLab rendering hints.
        type [<AllowNullLiteral>] ISchema =
            inherit IProperty
            /// Whether the schema is deprecated.
            /// 
            /// #### Notes
            /// This flag can be used by functionality that loads this plugin's settings
            /// from the registry. For example, the setting editor does not display a
            /// plugin's settings if it is set to `true`.
            abstract ``jupyter.lab.setting-deprecated``: bool option with get, set
            /// The JupyterLab icon class hint.
            abstract ``jupyter.lab.setting-icon-class``: string option with get, set
            /// The JupyterLab icon label hint.
            abstract ``jupyter.lab.setting-icon-label``: string option with get, set
            /// A flag that indicates plugin should be transformed before being used by
            /// the setting registry.
            /// 
            /// #### Notes
            /// If this value is set to `true`, the setting registry will wait until a
            /// transformation has been registered (by calling the `transform()` method
            /// of the registry) for the plugin ID before resolving `load()` promises.
            /// This means that if the attribute is set to `true` but no transformation
            /// is registered in time, calls to `load()` a plugin will eventually time
            /// out and reject.
            abstract ``jupyter.lab.transform``: bool option with get, set
            /// The JupyterLab shortcuts that are creaed by a plugin's schema.
            abstract ``jupyter.lab.shortcuts``: ResizeArray<IShortcut> option with get, set
            /// The root schema is always an object.
            abstract ``type``: string with get, set

        /// The setting values for a plugin.
        type [<AllowNullLiteral>] ISettingBundle =
            inherit JSONObject
            /// A composite of the user setting values and the plugin schema defaults.
            /// 
            /// #### Notes
            /// The `composite` values will always be a superset of the `user` values.
            abstract composite: JSONObject with get, set
            /// The user setting values.
            abstract user: JSONObject with get, set

        /// An interface for manipulating the settings of a specific plugin.
        type [<AllowNullLiteral>] ISettings =
            inherit IDisposable
            /// A signal that emits when the plugin's settings have changed.
            abstract changed: ISignal<ISettings, unit>
            /// The composite of user settings and extension defaults.
            abstract composite: ReadonlyJSONObject
            /// The plugin's ID.
            abstract id: string
            abstract plugin: ISettingRegistry.IPlugin
            /// The plugin settings raw text value.
            abstract raw: string
            /// The plugin's schema.
            abstract schema: ISettingRegistry.ISchema
            /// The user settings.
            abstract user: ReadonlyJSONObject
            /// The published version of the NPM package containing these settings.
            abstract version: string
            /// Return the defaults in a commented JSON format.
            abstract annotatedDefaults: unit -> string
            /// <summary>Calculate the default value of a setting by iterating through the schema.</summary>
            /// <param name="key">- The name of the setting whose default value is calculated.</param>
            abstract ``default``: key: string -> JSONValue option
            /// <summary>Get an individual setting.</summary>
            /// <param name="key">- The name of the setting being retrieved.</param>
            abstract get: key: string -> ISettingsGetReturn
            /// <summary>Remove a single setting.</summary>
            /// <param name="key">- The name of the setting being removed.</param>
            abstract remove: key: string -> Promise<unit>
            /// Save all of the plugin's user settings at once.
            abstract save: raw: string -> Promise<unit>
            /// <summary>Set a single setting.</summary>
            /// <param name="key">- The name of the setting being set.</param>
            /// <param name="value">- The value of the setting.</param>
            abstract set: key: string * value: JSONValue -> Promise<unit>
            /// <summary>Validates raw settings with comments.</summary>
            /// <param name="raw">- The JSON with comments string being validated.</param>
            abstract validate: raw: string -> ResizeArray<Settingregistry.ISchemaValidator.IError> option

        type [<AllowNullLiteral>] ISettingsGetReturn =
            abstract composite: ReadonlyJSONValue with get, set
            abstract user: ReadonlyJSONValue with get, set

        /// An interface describing a JupyterLab keyboard shortcut.
        type [<AllowNullLiteral>] IShortcut =
            inherit JSONObject
            /// The optional arguments passed into the shortcut's command.
            abstract args: JSONObject option with get, set
            /// The command invoked by the shortcut.
            abstract command: string with get, set
            /// Whether a keyboard shortcut is disabled. `False` by default.
            abstract disabled: bool option with get, set
            /// The key combination of the shortcut.
            abstract keys: ResizeArray<string> with get, set
            /// The CSS selector applicable to the shortcut.
            abstract selector: string with get, set

        type [<AllowNullLiteral>] TypeLiteral_01 =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: property: string -> IProperty with get, set

    type IStateDB =
        IStateDB<obj>

    /// The default state database token.
    /// The description of a state database.
    type [<AllowNullLiteral>] IStateDB<'T> =
        inherit Interfaces.IDataConnector<'T>
        /// Return a serialized copy of the state database's entire contents.
        abstract toJSON: unit -> Promise<TypeLiteral_04<'T>>

    type [<AllowNullLiteral>] TypeLiteral_03 =
        abstract composite: JSONValue with get, set
        abstract user: JSONValue with get, set

    type [<AllowNullLiteral>] TypeLiteral_04<'T> =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: id: string -> 'T with get, set

    type [<AllowNullLiteral>] TypeLiteral_02 =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> ISettingRegistry.IPlugin with get, set

module Url =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject

    module URLExt =

        type [<AllowNullLiteral>] IExports =
            /// Parse a url into a URL object.
            abstract parse: url: string -> IUrl
            /// Normalize a url.
            abstract normalize: url: string -> string
            /// <summary>Join a sequence of url components and normalizes as in node `path.join`.</summary>
            /// <param name="parts">- The url components.</param>
            abstract join: [<ParamArray>] parts: ResizeArray<string> -> string
            /// <summary>Encode the components of a multi-segment url.</summary>
            /// <param name="url">- The url to encode.</param>
            abstract encodeParts: url: string -> string
            /// Return a serialized object string suitable for a query.
            abstract objectToQueryString: value: JSONObject -> string
            /// Return a parsed object that represents the values in a query string.
            abstract queryStringToObject: value: string -> QueryStringToObjectReturn
            /// Test whether the url is a local url.
            /// 
            /// #### Notes
            /// This function returns `false` for any fully qualified url, including
            /// `data:`, `file:`, and `//` protocol URLs.
            abstract isLocal: url: string -> bool

        type [<AllowNullLiteral>] QueryStringToObjectReturn =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

        /// The interface for a URL object
        type [<AllowNullLiteral>] IUrl =
            /// The full URL string that was parsed with both the protocol and host
            /// components converted to lower-case.
            abstract href: string option with get, set
            /// Identifies the URL's lower-cased protocol scheme.
            abstract protocol: string option with get, set
            /// The full lower-cased host portion of the URL, including the port if
            /// specified.
            abstract host: string option with get, set
            /// The lower-cased host name portion of the host component without the
            /// port included.
            abstract hostname: string option with get, set
            /// The numeric port portion of the host component.
            abstract port: string option with get, set
            /// The entire path section of the URL.
            abstract pathname: string option with get, set
            /// The "fragment" portion of the URL including the leading ASCII hash
            /// `(#)` character
            abstract hash: string option with get, set
            /// The search element, including leading question mark (`'?'`), if any,
            /// of the URL.
            abstract search: string option with get, set
