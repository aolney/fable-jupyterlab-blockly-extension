// ts2fable 0.0.0
module rec JupyterlabObservables
open System
open Fable.Core
open Fable.Core.JS


module Modeldb =
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type IObservableJSON = Observablejson.IObservableJSON
    type IObservableString = Observablestring.IObservableString
    type IObservableUndoableList<'T> = Undoablelist.IObservableUndoableList<'T>
    type IObservableMap<'T> = Observablemap.IObservableMap<'T>

    type [<AllowNullLiteral>] IExports =
        abstract ObservableValue: ObservableValueStatic
        abstract ModelDB: ModelDBStatic

    type [<StringEnum>] [<RequireQualifiedAccess>] ObservableType =
        | [<CompiledName "Map">] Map
        | [<CompiledName "List">] List
        | [<CompiledName "String">] String
        | [<CompiledName "Value">] Value

    /// Base interface for Observable objects.
    type [<AllowNullLiteral>] IObservable =
        inherit IDisposable
        /// The type of this object.
        abstract ``type``: ObservableType

    /// Interface for an Observable object that represents
    /// an opaque JSON value.
    type [<AllowNullLiteral>] IObservableValue =
        inherit IObservable
        /// The type of this object.
        abstract ``type``: string
        /// The changed signal.
        abstract changed: ISignal<IObservableValue, ObservableValue.IChangedArgs>
        /// Get the current value, or `undefined` if it has not been set.
        abstract get: unit -> JSONValue option
        /// Set the value.
        abstract set: value: JSONValue -> unit

    /// Interface for an object representing a single collaborator
    /// on a realtime model.
    type [<AllowNullLiteral>] ICollaborator =
        inherit JSONObject
        /// A user id for the collaborator.
        /// This might not be unique, if the user has more than
        /// one editing session at a time.
        abstract userId: string
        /// A session id, which should be unique to a
        /// particular view on a collaborative model.
        abstract sessionId: string
        /// A human-readable display name for a collaborator.
        abstract displayName: string
        /// A color to be used to identify the collaborator in
        /// UI elements.
        abstract color: string
        /// A human-readable short name for a collaborator, for
        /// use in places where the full `displayName` would take
        /// too much space.
        abstract shortName: string

    /// Interface for an IObservableMap that tracks collaborators.
    type [<AllowNullLiteral>] ICollaboratorMap =
        inherit IObservableMap<ICollaborator>
        /// The local collaborator on a model.
        abstract localCollaborator: ICollaborator

    /// An interface for a path based database for
    /// creating and storing values, which is agnostic
    /// to the particular type of store in the backend.
    type [<AllowNullLiteral>] IModelDB =
        inherit IDisposable
        /// The base path for the `IModelDB`. This is prepended
        /// to all the paths that are passed in to the member
        /// functions of the object.
        abstract basePath: string
        /// Whether the database has been disposed.
        abstract isDisposed: bool
        /// Whether the database has been populated
        /// with model values prior to connection.
        abstract isPrepopulated: bool
        /// Whether the database is collaborative.
        abstract isCollaborative: bool
        /// A promise that resolves when the database
        /// has connected to its backend, if any.
        abstract connected: Promise<unit>
        /// A map of the currently active collaborators
        /// for the database, including the local user.
        abstract collaborators: ICollaboratorMap option
        /// <summary>Get a value for a path.</summary>
        /// <param name="path">: the path for the object.</param>
        abstract get: path: string -> IObservable option
        /// <summary>Whether the `IModelDB` has an object at this path.</summary>
        /// <param name="path">: the path for the object.</param>
        abstract has: path: string -> bool
        /// <summary>Create a string and insert it in the database.</summary>
        /// <param name="path">: the path for the string.</param>
        abstract createString: path: string -> IObservableString
        /// <summary>Create an undoable list and insert it in the database.</summary>
        /// <param name="path">: the path for the list.</param>
        abstract createList: path: string -> IObservableUndoableList<'T>
        /// <summary>Create a map and insert it in the database.</summary>
        /// <param name="path">: the path for the map.</param>
        abstract createMap: path: string -> IObservableJSON
        /// <summary>Create an opaque value and insert it in the database.</summary>
        /// <param name="path">: the path for the value.</param>
        abstract createValue: path: string -> IObservableValue
        /// <summary>Get a value at a path, or `undefined if it has not been set
        /// That value must already have been created using `createValue`.</summary>
        /// <param name="path">: the path for the value.</param>
        abstract getValue: path: string -> JSONValue option
        /// <summary>Set a value at a path. That value must already have
        /// been created using `createValue`.</summary>
        /// <param name="path">: the path for the value.</param>
        /// <param name="value">: the new value.</param>
        abstract setValue: path: string * value: JSONValue -> unit
        /// <summary>Create a view onto a subtree of the model database.</summary>
        /// <param name="basePath">: the path for the root of the subtree.</param>
        abstract view: basePath: string -> IModelDB
        /// Dispose of the resources held by the database.
        abstract dispose: unit -> unit

    /// A concrete implementation of an `IObservableValue`.
    /// The namespace for the `ObservableValue` class statics.
    type [<AllowNullLiteral>] ObservableValue =
        inherit IObservableValue
        /// The observable type.
        abstract ``type``: string
        /// Whether the value has been disposed.
        abstract isDisposed: bool
        /// The changed signal.
        abstract changed: ISignal<ObservableValue, ObservableValue.IChangedArgs>
        /// Get the current value, or `undefined` if it has not been set.
        abstract get: unit -> JSONValue
        /// Set the current value.
        abstract set: value: JSONValue -> unit
        /// Dispose of the resources held by the value.
        abstract dispose: unit -> unit

    /// A concrete implementation of an `IObservableValue`.
    /// The namespace for the `ObservableValue` class statics.
    type [<AllowNullLiteral>] ObservableValueStatic =
        /// <summary>Constructor for the value.</summary>
        /// <param name="initialValue">: the starting value for the `ObservableValue`.</param>
        [<Emit "new $0($1...)">] abstract Create: ?initialValue: JSONValue -> ObservableValue

    module ObservableValue =

        type [<AllowNullLiteral>] IExports =
            abstract IChangedArgs: IChangedArgsStatic

        /// The changed args object emitted by the `IObservableValue`.
        type [<AllowNullLiteral>] IChangedArgs =
            /// The old value.
            abstract oldValue: JSONValue option with get, set
            /// The new value.
            abstract newValue: JSONValue option with get, set

        /// The changed args object emitted by the `IObservableValue`.
        type [<AllowNullLiteral>] IChangedArgsStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> IChangedArgs

    /// A concrete implementation of an `IModelDB`.
    /// A namespace for the `ModelDB` class statics.
    type [<AllowNullLiteral>] ModelDB =
        inherit IModelDB
        /// The base path for the `ModelDB`. This is prepended
        /// to all the paths that are passed in to the member
        /// functions of the object.
        abstract basePath: string
        /// Whether the database is disposed.
        abstract isDisposed: bool
        /// Whether the model has been populated with
        /// any model values.
        abstract isPrepopulated: bool
        /// Whether the model is collaborative.
        abstract isCollaborative: bool
        /// A promise resolved when the model is connected
        /// to its backend. For the in-memory ModelDB it
        /// is immediately resolved.
        abstract connected: Promise<unit>
        /// <summary>Get a value for a path.</summary>
        /// <param name="path">: the path for the object.</param>
        abstract get: path: string -> IObservable option
        /// <summary>Whether the `IModelDB` has an object at this path.</summary>
        /// <param name="path">: the path for the object.</param>
        abstract has: path: string -> bool
        /// <summary>Create a string and insert it in the database.</summary>
        /// <param name="path">: the path for the string.</param>
        abstract createString: path: string -> IObservableString
        /// <summary>Create an undoable list and insert it in the database.</summary>
        /// <param name="path">: the path for the list.</param>
        abstract createList: path: string -> IObservableUndoableList<'T>
        /// <summary>Create a map and insert it in the database.</summary>
        /// <param name="path">: the path for the map.</param>
        abstract createMap: path: string -> IObservableJSON
        /// <summary>Create an opaque value and insert it in the database.</summary>
        /// <param name="path">: the path for the value.</param>
        abstract createValue: path: string -> IObservableValue
        /// <summary>Get a value at a path, or `undefined if it has not been set
        /// That value must already have been created using `createValue`.</summary>
        /// <param name="path">: the path for the value.</param>
        abstract getValue: path: string -> JSONValue option
        /// <summary>Set a value at a path. That value must already have
        /// been created using `createValue`.</summary>
        /// <param name="path">: the path for the value.</param>
        /// <param name="value">: the new value.</param>
        abstract setValue: path: string * value: JSONValue -> unit
        /// <summary>Create a view onto a subtree of the model database.</summary>
        /// <param name="basePath">: the path for the root of the subtree.</param>
        abstract view: basePath: string -> ModelDB
        /// <summary>Set a value at a path. Not intended to
        /// be called by user code, instead use the
        /// `create*` factory methods.</summary>
        /// <param name="path">: the path to set the value at.</param>
        /// <param name="value">: the value to set at the path.</param>
        abstract set: path: string * value: IObservable -> unit
        /// Dispose of the resources held by the database.
        abstract dispose: unit -> unit

    /// A concrete implementation of an `IModelDB`.
    /// A namespace for the `ModelDB` class statics.
    type [<AllowNullLiteral>] ModelDBStatic =
        /// Constructor for the `ModelDB`.
        [<Emit "new $0($1...)">] abstract Create: ?options: ModelDB.ICreateOptions -> ModelDB

    module ModelDB =

        /// Options for creating a `ModelDB` object.
        type [<AllowNullLiteral>] ICreateOptions =
            /// The base path to prepend to all the path arguments.
            abstract basePath: string option with get, set
            /// A ModelDB to use as the store for this
            /// ModelDB. If none is given, it uses its own store.
            abstract baseDB: ModelDB option with get, set

        /// A factory interface for creating `IModelDB` objects.
        type [<AllowNullLiteral>] IFactory =
            /// Create a new `IModelDB` instance.
            abstract createNew: path: string -> IModelDB

module Observablejson =
    type JSONObject = PhosphorCoreutils.JSONObject // __@phosphor_coreutils.JSONObject
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type IObservableMap<'T> = Observablemap.IObservableMap<'T>
    type ObservableMap<'T> = Observablemap.ObservableMap<'T>

    type [<AllowNullLiteral>] IExports =
        abstract ObservableJSON: ObservableJSONStatic

    /// An observable JSON value.
    /// The namespace for IObservableJSON related interfaces.
    type [<AllowNullLiteral>] IObservableJSON =
        inherit IObservableMap<JSONValue>
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JSONObject

    module IObservableJSON =

        type IChangedArgs =
            Observablemap.IObservableMap.IChangedArgs<JSONValue>
            //IObservableMap.IChangedArgs<JSONValue>

    /// A concrete Observable map for JSON data.
    /// The namespace for ObservableJSON static data.
    type [<AllowNullLiteral>] ObservableJSON =
        inherit ObservableMap<JSONValue>
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JSONObject

    /// A concrete Observable map for JSON data.
    /// The namespace for ObservableJSON static data.
    type [<AllowNullLiteral>] ObservableJSONStatic =
        /// Construct a new observable JSON object.
        [<Emit "new $0($1...)">] abstract Create: ?options: ObservableJSON.IOptions -> ObservableJSON

    module ObservableJSON =

        type [<AllowNullLiteral>] IExports =
            abstract ChangeMessage: ChangeMessageStatic

        /// The options use to initialize an observable JSON object.
        type [<AllowNullLiteral>] IOptions =
            /// The optional initial value for the object.
            abstract values: JSONObject option with get, set

        /// An observable JSON change message.
        type [<AllowNullLiteral>] ChangeMessage =
            inherit Message
            /// The arguments of the change.
            abstract args: IObservableJSON.IChangedArgs

        /// An observable JSON change message.
        type [<AllowNullLiteral>] ChangeMessageStatic =
            /// Create a new metadata changed message.
            [<Emit "new $0($1...)">] abstract Create: ``type``: string * args: IObservableJSON.IChangedArgs -> ChangeMessage

module Observablelist =
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __@phosphor_algorithm.IIterator
    type IterableOrArrayLike<'T> = PhosphorAlgorithm.Iter.IterableOrArrayLike<'T> // __@phosphor_algorithm.IterableOrArrayLike
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal

    type [<AllowNullLiteral>] IExports =
        abstract ObservableList: ObservableListStatic

    /// A list which can be observed for changes.
    /// The namespace for IObservableList related interfaces.
    type [<AllowNullLiteral>] IObservableList<'T> =
        inherit IDisposable
        /// A signal emitted when the list has changed.
        abstract changed: ISignal<IObservableList<'T>, IObservableList.IChangedArgs<'T>>
        /// The type of this object.
        abstract ``type``: string
        /// The length of the list.
        /// 
        /// #### Notes
        /// This is a read-only property.
        abstract length: float with get, set
        /// Create an iterator over the values in the list.
        abstract iter: unit -> IIterator<'T>
        /// Remove all values from the list.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// All current iterators are invalidated.
        abstract clear: unit -> unit
        /// <summary>Get the value at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        abstract get: index: float -> 'T option
        /// <summary>Insert a value into the list at a specific index.</summary>
        /// <param name="index">- The index at which to insert the value.</param>
        /// <param name="value">- The value to set at the specified index.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Notes
        /// The `index` will be clamped to the bounds of the list.
        /// 
        /// #### Undefined Behavior
        /// An `index` which is non-integral.</param>
        abstract insert: index: float * value: 'T -> unit
        /// <summary>Insert a set of items into the list at the specified index.</summary>
        /// <param name="index">- The index at which to insert the values.</param>
        /// <param name="values">- The values to insert at the specified index.
        /// 
        /// #### Complexity.
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Notes
        /// The `index` will be clamped to the bounds of the list.
        /// 
        /// #### Undefined Behavior.
        /// An `index` which is non-integral.</param>
        abstract insertAll: index: float * values: IterableOrArrayLike<'T> -> unit
        /// <summary>Move a value from one index to another.</summary>
        /// <param name="toIndex">- The index to move the element to.
        /// 
        /// #### Complexity
        /// Constant.
        /// 
        /// #### Iterator Validity
        /// Iterators pointing at the lesser of the `fromIndex` and the `toIndex`
        /// and beyond are invalidated.
        /// 
        /// #### Undefined Behavior
        /// A `fromIndex` or a `toIndex` which is non-integral.</param>
        abstract move: fromIndex: float * toIndex: float -> unit
        /// <summary>Add a value to the back of the list.</summary>
        /// <param name="value">- The value to add to the back of the list.</param>
        abstract push: value: 'T -> float
        /// <summary>Push a set of values to the back of the list.</summary>
        /// <param name="values">- An iterable or array-like set of values to add.</param>
        abstract pushAll: values: IterableOrArrayLike<'T> -> float
        /// <summary>Remove and return the value at a specific index.</summary>
        /// <param name="index">- The index of the value of interest.</param>
        abstract remove: index: float -> 'T option
        /// <summary>Remove a range of items from the list.</summary>
        /// <param name="startIndex">- The start index of the range to remove (inclusive).</param>
        /// <param name="endIndex">- The end index of the range to remove (exclusive).</param>
        abstract removeRange: startIndex: float * endIndex: float -> float
        /// <summary>Remove the first occurrence of a value from the list.</summary>
        /// <param name="value">- The value of interest.</param>
        abstract removeValue: value: 'T -> float
        /// <summary>Set the value at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        /// <param name="value">- The value to set at the specified index.
        /// 
        /// #### Complexity
        /// Constant.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Undefined Behavior
        /// An `index` which is non-integral or out of range.</param>
        abstract set: index: float * value: 'T -> unit

    module IObservableList =

        type [<StringEnum>] [<RequireQualifiedAccess>] ChangeType =
            | Add
            | Move
            | Remove
            | Set

        /// The changed args object which is emitted by an observable list.
        type [<AllowNullLiteral>] IChangedArgs<'T> =
            /// The type of change undergone by the vector.
            abstract ``type``: ChangeType with get, set
            /// The new index associated with the change.
            abstract newIndex: float with get, set
            /// The new values associated with the change.
            /// 
            /// #### Notes
            /// The values will be contiguous starting at the `newIndex`.
            abstract newValues: ResizeArray<'T> with get, set
            /// The old index associated with the change.
            abstract oldIndex: float with get, set
            /// The old values associated with the change.
            /// 
            /// #### Notes
            /// The values will be contiguous starting at the `oldIndex`.
            abstract oldValues: ResizeArray<'T> with get, set

    /// A concrete implementation of [[IObservableList]].
    /// The namespace for `ObservableList` class statics.
    type [<AllowNullLiteral>] ObservableList<'T> =
        inherit IObservableList<'T>
        /// The type of this object.
        abstract ``type``: string
        /// A signal emitted when the list has changed.
        abstract changed: ISignal<ObservableList<'T>, IObservableList.IChangedArgs<'T>>
        /// The length of the list.
        abstract length: float
        /// Test whether the list has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the list.
        abstract dispose: unit -> unit
        /// Create an iterator over the values in the list.
        abstract iter: unit -> IIterator<'T>
        /// <summary>Get the value at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        abstract get: index: float -> 'T option
        /// <summary>Set the value at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        /// <param name="value">- The value to set at the specified index.
        /// 
        /// #### Complexity
        /// Constant.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Undefined Behavior
        /// An `index` which is non-integral or out of range.</param>
        abstract set: index: float * value: 'T -> unit
        /// <summary>Add a value to the end of the list.</summary>
        /// <param name="value">- The value to add to the end of the list.</param>
        abstract push: value: 'T -> float
        /// <summary>Insert a value into the list at a specific index.</summary>
        /// <param name="index">- The index at which to insert the value.</param>
        /// <param name="value">- The value to set at the specified index.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Notes
        /// The `index` will be clamped to the bounds of the list.
        /// 
        /// #### Undefined Behavior
        /// An `index` which is non-integral.</param>
        abstract insert: index: float * value: 'T -> unit
        /// <summary>Remove the first occurrence of a value from the list.</summary>
        /// <param name="value">- The value of interest.</param>
        abstract removeValue: value: 'T -> float
        /// <summary>Remove and return the value at a specific index.</summary>
        /// <param name="index">- The index of the value of interest.</param>
        abstract remove: index: float -> 'T option
        /// Remove all values from the list.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// All current iterators are invalidated.
        abstract clear: unit -> unit
        /// <summary>Move a value from one index to another.</summary>
        /// <param name="toIndex">- The index to move the element to.
        /// 
        /// #### Complexity
        /// Constant.
        /// 
        /// #### Iterator Validity
        /// Iterators pointing at the lesser of the `fromIndex` and the `toIndex`
        /// and beyond are invalidated.
        /// 
        /// #### Undefined Behavior
        /// A `fromIndex` or a `toIndex` which is non-integral.</param>
        abstract move: fromIndex: float * toIndex: float -> unit
        /// <summary>Push a set of values to the back of the list.</summary>
        /// <param name="values">- An iterable or array-like set of values to add.</param>
        abstract pushAll: values: IterableOrArrayLike<'T> -> float
        /// <summary>Insert a set of items into the list at the specified index.</summary>
        /// <param name="index">- The index at which to insert the values.</param>
        /// <param name="values">- The values to insert at the specified index.
        /// 
        /// #### Complexity.
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Notes
        /// The `index` will be clamped to the bounds of the list.
        /// 
        /// #### Undefined Behavior.
        /// An `index` which is non-integral.</param>
        abstract insertAll: index: float * values: IterableOrArrayLike<'T> -> unit
        /// <summary>Remove a range of items from the list.</summary>
        /// <param name="startIndex">- The start index of the range to remove (inclusive).</param>
        /// <param name="endIndex">- The end index of the range to remove (exclusive).</param>
        abstract removeRange: startIndex: float * endIndex: float -> float

    /// A concrete implementation of [[IObservableList]].
    /// The namespace for `ObservableList` class statics.
    type [<AllowNullLiteral>] ObservableListStatic =
        /// Construct a new observable map.
        [<Emit "new $0($1...)">] abstract Create: ?options: ObservableList.IOptions<'T> -> ObservableList<'T>

    module ObservableList =

        /// The options used to initialize an observable map.
        type [<AllowNullLiteral>] IOptions<'T> =
            /// An optional initial set of values.
            abstract values: IterableOrArrayLike<'T> option with get, set
            /// The item comparison function for change detection on `set`.
            /// 
            /// If not given, strict `===` equality will be used.
            abstract itemCmp: ('T -> 'T -> bool) option with get, set

module Observablemap =
    type IDisposable = PhosphorDisposable.IDisposable //__@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IObservable = Modeldb.IObservable

    type [<AllowNullLiteral>] IExports =
        abstract ObservableMap: ObservableMapStatic

    /// A map which can be observed for changes.
    /// The interfaces associated with an IObservableMap.
    type [<AllowNullLiteral>] IObservableMap<'T> =
        inherit IDisposable
        inherit IObservable
        /// The type of the Observable.
        abstract ``type``: string with get, set
        /// A signal emitted when the map has changed.
        abstract changed: ISignal<IObservableMap<'T>, IObservableMap.IChangedArgs<'T>>
        /// The number of key-value pairs in the map.
        abstract size: float
        /// <summary>Set a key-value pair in the map</summary>
        /// <param name="key">- The key to set.</param>
        /// <param name="value">- The value for the key.</param>
        abstract set: key: string * value: 'T -> 'T option
        /// <summary>Get a value for a given key.</summary>
        /// <param name="key">- the key.</param>
        abstract get: key: string -> 'T option
        /// <summary>Check whether the map has a key.</summary>
        /// <param name="key">- the key to check.</param>
        abstract has: key: string -> bool
        /// Get a list of the keys in the map.
        abstract keys: unit -> ResizeArray<string>
        /// Get a list of the values in the map.
        abstract values: unit -> ResizeArray<'T>
        /// <summary>Remove a key from the map</summary>
        /// <param name="key">- the key to remove.</param>
        abstract delete: key: string -> 'T option
        /// Set the ObservableMap to an empty map.
        abstract clear: unit -> unit
        /// Dispose of the resources held by the map.
        abstract dispose: unit -> unit

    module IObservableMap =

        type [<StringEnum>] [<RequireQualifiedAccess>] ChangeType =
            | Add
            | Remove
            | Change

        /// The changed args object which is emitted by an observable map.
        type [<AllowNullLiteral>] IChangedArgs<'T> =
            /// The type of change undergone by the map.
            abstract ``type``: ChangeType with get, set
            /// The key of the change.
            abstract key: string with get, set
            /// The old value of the change.
            abstract oldValue: 'T option with get, set
            /// The new value of the change.
            abstract newValue: 'T option with get, set

    /// A concrete implementation of IObservbleMap<T>.
    /// The namespace for `ObservableMap` class statics.
    type [<AllowNullLiteral>] ObservableMap<'T> =
        inherit IObservableMap<'T>
        /// The type of the Observable.
        abstract ``type``: string
        /// A signal emitted when the map has changed.
        abstract changed: ISignal<ObservableMap<'T>, IObservableMap.IChangedArgs<'T>>
        /// Whether this map has been disposed.
        abstract isDisposed: bool
        /// The number of key-value pairs in the map.
        abstract size: float
        /// <summary>Set a key-value pair in the map</summary>
        /// <param name="key">- The key to set.</param>
        /// <param name="value">- The value for the key.</param>
        abstract set: key: string * value: 'T -> 'T option
        /// <summary>Get a value for a given key.</summary>
        /// <param name="key">- the key.</param>
        abstract get: key: string -> 'T option
        /// <summary>Check whether the map has a key.</summary>
        /// <param name="key">- the key to check.</param>
        abstract has: key: string -> bool
        /// Get a list of the keys in the map.
        abstract keys: unit -> ResizeArray<string>
        /// Get a list of the values in the map.
        abstract values: unit -> ResizeArray<'T>
        /// <summary>Remove a key from the map</summary>
        /// <param name="key">- the key to remove.</param>
        abstract delete: key: string -> 'T option
        /// Set the ObservableMap to an empty map.
        abstract clear: unit -> unit
        /// Dispose of the resources held by the map.
        abstract dispose: unit -> unit

    /// A concrete implementation of IObservbleMap<T>.
    /// The namespace for `ObservableMap` class statics.
    type [<AllowNullLiteral>] ObservableMapStatic =
        /// Construct a new observable map.
        [<Emit "new $0($1...)">] abstract Create: ?options: ObservableMap.IOptions<'T> -> ObservableMap<'T>

    module ObservableMap =

        /// The options used to initialize an observable map.
        type [<AllowNullLiteral>] IOptions<'T> =
            /// An optional initial set of values.
            abstract values: TypeLiteral_01<'T> option with get, set
            /// The item comparison function for change detection on `set`.
            /// 
            /// If not given, strict `===` equality will be used.
            abstract itemCmp: ('T -> 'T -> bool) option with get, set

        type [<AllowNullLiteral>] TypeLiteral_01<'T> =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T with get, set

module Observablestring =
    type IDisposable = PhosphorDisposable.IDisposable // __@phosphor_disposable.IDisposable
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IObservable = Modeldb.IObservable

    type [<AllowNullLiteral>] IExports =
        abstract ObservableString: ObservableStringStatic

    /// A string which can be observed for changes.
    /// The namespace for `IObservableString` associate interfaces.
    type [<AllowNullLiteral>] IObservableString =
        inherit IDisposable
        inherit Modeldb.IObservable //IObservable
        /// The type of the Observable.
        abstract ``type``: string with get, set
        /// A signal emitted when the string has changed.
        abstract changed: ISignal<IObservableString, IObservableString.IChangedArgs>
        /// The value of the string.
        abstract text: string with get, set
        /// <summary>Insert a substring.</summary>
        /// <param name="index">- The starting index.</param>
        /// <param name="text">- The substring to insert.</param>
        abstract insert: index: float * text: string -> unit
        /// <summary>Remove a substring.</summary>
        /// <param name="start">- The starting index.</param>
        /// <param name="end">- The ending index.</param>
        abstract remove: start: float * ``end``: float -> unit
        /// Set the ObservableString to an empty string.
        abstract clear: unit -> unit
        /// Dispose of the resources held by the string.
        abstract dispose: unit -> unit

    module IObservableString =

        type [<StringEnum>] [<RequireQualifiedAccess>] ChangeType =
            | Insert
            | Remove
            | Set

        /// The changed args object which is emitted by an observable string.
        type [<AllowNullLiteral>] IChangedArgs =
            /// The type of change undergone by the list.
            abstract ``type``: ChangeType with get, set
            /// The starting index of the change.
            abstract start: float with get, set
            /// The end index of the change.
            abstract ``end``: float with get, set
            /// The value of the change.
            /// 
            /// ### Notes
            /// If `ChangeType` is `set`, then
            /// this is the new value of the string.
            /// 
            /// If `ChangeType` is `insert` this is
            /// the value of the inserted string.
            /// 
            /// If `ChangeType` is remove this is the
            /// value of the removed substring.
            abstract value: string with get, set

    /// A concrete implementation of [[IObservableString]]
    type [<AllowNullLiteral>] ObservableString =
        inherit IObservableString
        /// The type of the Observable.
        abstract ``type``: string
        /// A signal emitted when the string has changed.
        abstract changed: ISignal<ObservableString, IObservableString.IChangedArgs>
        /// Set the value of the string.
        /// Get the value of the string.
        abstract text: string with get, set
        /// <summary>Insert a substring.</summary>
        /// <param name="index">- The starting index.</param>
        /// <param name="text">- The substring to insert.</param>
        abstract insert: index: float * text: string -> unit
        /// <summary>Remove a substring.</summary>
        /// <param name="start">- The starting index.</param>
        /// <param name="end">- The ending index.</param>
        abstract remove: start: float * ``end``: float -> unit
        /// Set the ObservableString to an empty string.
        abstract clear: unit -> unit
        /// Test whether the string has been disposed.
        abstract isDisposed: bool
        /// Dispose of the resources held by the string.
        abstract dispose: unit -> unit

    /// A concrete implementation of [[IObservableString]]
    type [<AllowNullLiteral>] ObservableStringStatic =
        /// Construct a new observable string.
        [<Emit "new $0($1...)">] abstract Create: ?initialText: string -> ObservableString

module Undoablelist =
    type JSONValue = PhosphorCoreutils.JSONValue //__@phosphor_coreutils.JSONValue
    type IObservableList<'T> = Observablelist.IObservableList<'T>
    type ObservableList<'T> = Observablelist.ObservableList<'T>

    type [<AllowNullLiteral>] IExports =
        abstract ObservableUndoableList: ObservableUndoableListStatic

    /// An object which knows how to serialize and
    /// deserialize the type T.
    type [<AllowNullLiteral>] ISerializer<'T> =
        /// Convert the object to JSON.
        abstract toJSON: value: 'T -> JSONValue
        /// Deserialize the object from JSON.
        abstract fromJSON: value: JSONValue -> 'T

    /// An observable list that supports undo/redo.
    type [<AllowNullLiteral>] IObservableUndoableList<'T> =
        inherit IObservableList<'T>
        /// Whether the object can redo changes.
        abstract canRedo: bool
        /// Whether the object can undo changes.
        abstract canUndo: bool
        /// <summary>Begin a compound operation.</summary>
        /// <param name="isUndoAble">- Whether the operation is undoable.
        /// The default is `false`.</param>
        abstract beginCompoundOperation: ?isUndoAble: bool -> unit
        /// End a compound operation.
        abstract endCompoundOperation: unit -> unit
        /// Undo an operation.
        abstract undo: unit -> unit
        /// Redo an operation.
        abstract redo: unit -> unit
        /// Clear the change stack.
        abstract clearUndo: unit -> unit

    /// A concrete implementation of an observable undoable list.
    /// Namespace for ObservableUndoableList utilities.
    type [<AllowNullLiteral>] ObservableUndoableList<'T> =
        inherit ObservableList<'T>
        inherit IObservableUndoableList<'T>
        /// Whether the object can redo changes.
        abstract canRedo: bool
        /// Whether the object can undo changes.
        abstract canUndo: bool
        /// <summary>Begin a compound operation.</summary>
        /// <param name="isUndoAble">- Whether the operation is undoable.
        /// The default is `true`.</param>
        abstract beginCompoundOperation: ?isUndoAble: bool -> unit
        /// End a compound operation.
        abstract endCompoundOperation: unit -> unit
        /// Undo an operation.
        abstract undo: unit -> unit
        /// Redo an operation.
        abstract redo: unit -> unit
        /// Clear the change stack.
        abstract clearUndo: unit -> unit

    /// A concrete implementation of an observable undoable list.
    /// Namespace for ObservableUndoableList utilities.
    type [<AllowNullLiteral>] ObservableUndoableListStatic =
        /// Construct a new undoable observable list.
        [<Emit "new $0($1...)">] abstract Create: serializer: ISerializer<'T> -> ObservableUndoableList<'T>

    module ObservableUndoableList =

        type [<AllowNullLiteral>] IExports =
            abstract IdentitySerializer: IdentitySerializerStatic

        /// A default, identity serializer.
        type [<AllowNullLiteral>] IdentitySerializer<'T> =
            inherit ISerializer<'T>
            /// Identity serialize.
            abstract toJSON: value: 'T -> JSONValue
            /// Identity deserialize.
            abstract fromJSON: value: JSONValue -> 'T

        /// A default, identity serializer.
        type [<AllowNullLiteral>] IdentitySerializerStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> IdentitySerializer<'T>
