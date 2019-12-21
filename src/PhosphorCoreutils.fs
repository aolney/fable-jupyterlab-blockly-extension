// ts2fable 0.6.1
module rec PhosphorCoreutils
open System
open Fable.Core
open Fable.Core.JS

//amo typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
type PromiseLike<'T> = Promise<'T>
//end typescript

type JSONPrimitive =
    U3<bool, float, string> option

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module JSONPrimitive =
    let ofBoolOption v: JSONPrimitive = v |> Option.map U3.Case1
    let ofBool v: JSONPrimitive = v |> U3.Case1 |> Some
    let isBool (v: JSONPrimitive) = match v with None -> false | Some o -> match o with U3.Case1 _ -> true | _ -> false
    let asBool (v: JSONPrimitive) = match v with None -> None | Some o -> match o with U3.Case1 o -> Some o | _ -> None
    let ofFloatOption v: JSONPrimitive = v |> Option.map U3.Case2
    let ofFloat v: JSONPrimitive = v |> U3.Case2 |> Some
    let isFloat (v: JSONPrimitive) = match v with None -> false | Some o -> match o with U3.Case2 _ -> true | _ -> false
    let asFloat (v: JSONPrimitive) = match v with None -> None | Some o -> match o with U3.Case2 o -> Some o | _ -> None
    let ofStringOption v: JSONPrimitive = v |> Option.map U3.Case3
    let ofString v: JSONPrimitive = v |> U3.Case3 |> Some
    let isString (v: JSONPrimitive) = match v with None -> false | Some o -> match o with U3.Case3 _ -> true | _ -> false
    let asString (v: JSONPrimitive) = match v with None -> None | Some o -> match o with U3.Case3 o -> Some o | _ -> None

type JSONValue =
    U3<JSONPrimitive, JSONObject, JSONArray>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module JSONValue =
    let ofJSONPrimitive v: JSONValue = v |> U3.Case1
    let isJSONPrimitive (v: JSONValue) = match v with U3.Case1 _ -> true | _ -> false
    let asJSONPrimitive (v: JSONValue) = match v with U3.Case1 o -> Some o | _ -> None
    let ofJSONObject v: JSONValue = v |> U3.Case2
    let isJSONObject (v: JSONValue) = match v with U3.Case2 _ -> true | _ -> false
    let asJSONObject (v: JSONValue) = match v with U3.Case2 o -> Some o | _ -> None
    let ofJSONArray v: JSONValue = v |> U3.Case3
    let isJSONArray (v: JSONValue) = match v with U3.Case3 _ -> true | _ -> false
    let asJSONArray (v: JSONValue) = match v with U3.Case3 o -> Some o | _ -> None

/// A type definition for a JSON object.
type [<AllowNullLiteral>] JSONObject =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> JSONValue with get, set

/// A type definition for a JSON array.
type [<AllowNullLiteral>] JSONArray =
    inherit Array<JSONValue>
// type JSONArray = JSONValue[]

/// A type definition for a readonly JSON object.
type [<AllowNullLiteral>] ReadonlyJSONObject =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> ReadonlyJSONValue

/// A type definition for a readonly JSON array.
type [<AllowNullLiteral>] ReadonlyJSONArray =
    inherit ReadonlyArray<ReadonlyJSONValue>

type ReadonlyJSONValue =
    U3<JSONPrimitive, ReadonlyJSONObject, ReadonlyJSONArray>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ReadonlyJSONValue =
    let ofJSONPrimitive v: ReadonlyJSONValue = v |> U3.Case1
    let isJSONPrimitive (v: ReadonlyJSONValue) = match v with U3.Case1 _ -> true | _ -> false
    let asJSONPrimitive (v: ReadonlyJSONValue) = match v with U3.Case1 o -> Some o | _ -> None
    let ofReadonlyJSONObject v: ReadonlyJSONValue = v |> U3.Case2
    let isReadonlyJSONObject (v: ReadonlyJSONValue) = match v with U3.Case2 _ -> true | _ -> false
    let asReadonlyJSONObject (v: ReadonlyJSONValue) = match v with U3.Case2 o -> Some o | _ -> None
    let ofReadonlyJSONArray v: ReadonlyJSONValue = v |> U3.Case3
    let isReadonlyJSONArray (v: ReadonlyJSONValue) = match v with U3.Case3 _ -> true | _ -> false
    let asReadonlyJSONArray (v: ReadonlyJSONValue) = match v with U3.Case3 o -> Some o | _ -> None

module JSONExt =

    type [<AllowNullLiteral>] IExports =
        abstract emptyObject: ReadonlyJSONObject
        abstract emptyArray: ReadonlyJSONArray
        /// <summary>Test whether a JSON value is a primitive.</summary>
        /// <param name="value">- The JSON value of interest.</param>
        abstract isPrimitive: value: ReadonlyJSONValue -> bool
        /// <summary>Test whether a JSON value is an array.</summary>
        /// <param name="value">- The JSON value of interest.</param>
        abstract isArray: value: JSONValue -> bool
        abstract isArray: value: ReadonlyJSONValue -> bool
        /// <summary>Test whether a JSON value is an object.</summary>
        /// <param name="value">- The JSON value of interest.</param>
        abstract isObject: value: JSONValue -> bool
        abstract isObject: value: ReadonlyJSONValue -> bool
        /// <summary>Compare two JSON values for deep equality.</summary>
        /// <param name="first">- The first JSON value of interest.</param>
        /// <param name="second">- The second JSON value of interest.</param>
        abstract deepEqual: first: ReadonlyJSONValue * second: ReadonlyJSONValue -> bool
        /// <summary>Create a deep copy of a JSON value.</summary>
        /// <param name="value">- The JSON value to copy.</param>
        abstract deepCopy: value: 'T -> 'T

type [<AllowNullLiteral>] IExports =
    abstract MimeData: MimeDataStatic
    abstract PromiseDelegate: PromiseDelegateStatic
    abstract Token: TokenStatic

/// An object which stores MIME data for general application use.
/// 
/// #### Notes
/// This class does not attempt to enforce "correctness" of MIME types
/// and their associated data. Since this class is designed to transfer
/// arbitrary data and objects within the same application, it assumes
/// that the user provides correct and accurate data.
type [<AllowNullLiteral>] MimeData =
    /// Get an array of the MIME types contained within the dataset.
    abstract types: unit -> ResizeArray<string>
    /// <summary>Test whether the dataset has an entry for the given type.</summary>
    /// <param name="mime">- The MIME type of interest.</param>
    abstract hasData: mime: string -> bool
    /// <summary>Get the data value for the given MIME type.</summary>
    /// <param name="mime">- The MIME type of interest.</param>
    abstract getData: mime: string -> obj option option
    /// <summary>Set the data value for the given MIME type.</summary>
    /// <param name="mime">- The MIME type of interest.</param>
    /// <param name="data">- The data value for the given MIME type.
    /// 
    /// #### Notes
    /// This will overwrite any previous entry for the MIME type.</param>
    abstract setData: mime: string * data: obj option -> unit
    /// <summary>Remove the data entry for the given MIME type.</summary>
    /// <param name="mime">- The MIME type of interest.
    /// 
    /// #### Notes
    /// This is a no-op if there is no entry for the given MIME type.</param>
    abstract clearData: mime: string -> unit
    /// Remove all data entries from the dataset.
    abstract clear: unit -> unit
    abstract _types: obj with get, set
    abstract _values: obj with get, set

/// An object which stores MIME data for general application use.
/// 
/// #### Notes
/// This class does not attempt to enforce "correctness" of MIME types
/// and their associated data. Since this class is designed to transfer
/// arbitrary data and objects within the same application, it assumes
/// that the user provides correct and accurate data.
type [<AllowNullLiteral>] MimeDataStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MimeData

// type [<AllowNullLiteral>] IExports =
//     abstract PromiseDelegate: PromiseDelegateStatic

/// A class which wraps a promise into a delegate object.
/// 
/// #### Notes
/// This class is useful when the logic to resolve or reject a promise
/// cannot be defined at the point where the promise is created.
type [<AllowNullLiteral>] PromiseDelegate<'T> =
    /// The promise wrapped by the delegate.
    abstract promise: Promise<'T>
    /// <summary>Resolve the wrapped promise with the given value.</summary>
    /// <param name="value">- The value to use for resolving the promise.</param>
    abstract resolve: value: U2<'T, PromiseLike<'T>> -> unit
    /// Reject the wrapped promise with the given value.
    abstract reject: reason: obj option -> unit
    abstract _resolve: obj with get, set
    abstract _reject: obj with get, set

/// A class which wraps a promise into a delegate object.
/// 
/// #### Notes
/// This class is useful when the logic to resolve or reject a promise
/// cannot be defined at the point where the promise is created.
type [<AllowNullLiteral>] PromiseDelegateStatic =
    /// Construct a new promise delegate.
    [<Emit "new $0($1...)">] abstract Create: unit -> PromiseDelegate<'T>

module Random =

    type [<AllowNullLiteral>] IExports =
        abstract getRandomValues: (Uint8Array -> unit)

// type [<AllowNullLiteral>] IExports =
//     abstract Token: TokenStatic

/// A runtime object which captures compile-time type information.
/// 
/// #### Notes
/// A token captures the compile-time type of an interface or class in
/// an object which can be used at runtime in a type-safe fashion.
type [<AllowNullLiteral>] Token<'T> =
    /// The human readable name for the token.
    /// 
    /// #### Notes
    /// This can be useful for debugging and logging.
    abstract name: string
    abstract _tokenStructuralPropertyT: obj with get, set

/// A runtime object which captures compile-time type information.
/// 
/// #### Notes
/// A token captures the compile-time type of an interface or class in
/// an object which can be used at runtime in a type-safe fashion.
type [<AllowNullLiteral>] TokenStatic =
    /// <summary>Construct a new token.</summary>
    /// <param name="name">- A human readable name for the token.</param>
    [<Emit "new $0($1...)">] abstract Create: name: string -> Token<'T>

module UUID =

    type [<AllowNullLiteral>] IExports =
        abstract uuid4: (unit -> string)
