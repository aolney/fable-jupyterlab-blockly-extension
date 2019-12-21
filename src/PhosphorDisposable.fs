// ts2fable 0.0.0
module rec PhosphorDisposable
open System
open Fable.Core
open Fable.Core.JS

type IterableOrArrayLike<'T> = PhosphorAlgorithm.Iter.IterableOrArrayLike<'T> // @phosphor_algorithm.IterableOrArrayLike
type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> //@phosphor_signaling.ISignal

type [<AllowNullLiteral>] IExports =
    abstract DisposableDelegate: DisposableDelegateStatic
    abstract ObservableDisposableDelegate: ObservableDisposableDelegateStatic
    abstract DisposableSet: DisposableSetStatic
    abstract ObservableDisposableSet: ObservableDisposableSetStatic

/// An object which implements the disposable pattern.
type [<AllowNullLiteral>] IDisposable =
    /// Test whether the object has been disposed.
    /// 
    /// #### Notes
    /// This property is always safe to access.
    abstract isDisposed: bool
    /// Dispose of the resources held by the object.
    /// 
    /// #### Notes
    /// If the object's `dispose` method is called more than once, all
    /// calls made after the first will be a no-op.
    /// 
    /// #### Undefined Behavior
    /// It is undefined behavior to use any functionality of the object
    /// after it has been disposed unless otherwise explicitly noted.
    abstract dispose: unit -> unit

/// A disposable object with an observable `disposed` signal.
type [<AllowNullLiteral>] IObservableDisposable =
    inherit IDisposable
    /// A signal emitted when the object is disposed.
    abstract disposed: ISignal<IObservableDisposable, unit>

/// A disposable object which delegates to a callback function.
type [<AllowNullLiteral>] DisposableDelegate =
    inherit IDisposable
    /// Test whether the delegate has been disposed.
    abstract isDisposed: bool
    /// Dispose of the delegate and invoke the callback function.
    abstract dispose: unit -> unit

/// A disposable object which delegates to a callback function.
type [<AllowNullLiteral>] DisposableDelegateStatic =
    /// <summary>Construct a new disposable delegate.</summary>
    /// <param name="fn">- The callback function to invoke on dispose.</param>
    [<Emit "new $0($1...)">] abstract Create: fn: (unit -> unit) -> DisposableDelegate

/// An observable disposable object which delegates to a callback function.
type [<AllowNullLiteral>] ObservableDisposableDelegate =
    inherit DisposableDelegate
    inherit IObservableDisposable
    /// A signal emitted when the delegate is disposed.
    abstract disposed: ISignal<ObservableDisposableDelegate, unit>
    /// Dispose of the delegate and invoke the callback function.
    abstract dispose: unit -> unit

/// An observable disposable object which delegates to a callback function.
type [<AllowNullLiteral>] ObservableDisposableDelegateStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ObservableDisposableDelegate

/// An object which manages a collection of disposable items.
/// The namespace for the `DisposableSet` class statics.
type [<AllowNullLiteral>] DisposableSet =
    inherit IDisposable
    /// Test whether the set has been disposed.
    abstract isDisposed: bool
    /// Dispose of the set and the items it contains.
    /// 
    /// #### Notes
    /// Items are disposed in the order they are added to the set.
    abstract dispose: unit -> unit
    /// <summary>Test whether the set contains a specific item.</summary>
    /// <param name="item">- The item of interest.</param>
    abstract contains: item: IDisposable -> bool
    /// <summary>Add a disposable item to the set.</summary>
    /// <param name="item">- The item to add to the set.
    /// 
    /// #### Notes
    /// If the item is already contained in the set, this is a no-op.</param>
    abstract add: item: IDisposable -> unit
    /// <summary>Remove a disposable item from the set.</summary>
    /// <param name="item">- The item to remove from the set.
    /// 
    /// #### Notes
    /// If the item is not contained in the set, this is a no-op.</param>
    abstract remove: item: IDisposable -> unit
    /// Remove all items from the set.
    abstract clear: unit -> unit

/// An object which manages a collection of disposable items.
/// The namespace for the `DisposableSet` class statics.
type [<AllowNullLiteral>] DisposableSetStatic =
    /// Construct a new disposable set.
    [<Emit "new $0($1...)">] abstract Create: unit -> DisposableSet

module DisposableSet =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create a disposable set from an iterable of items.</summary>
        /// <param name="items">- The iterable or array-like object of interest.</param>
        abstract from: items: IterableOrArrayLike<IDisposable> -> DisposableSet

/// An observable object which manages a collection of disposable items.
/// The namespace for the `ObservableDisposableSet` class statics.
type [<AllowNullLiteral>] ObservableDisposableSet =
    inherit DisposableSet
    inherit IObservableDisposable
    /// A signal emitted when the set is disposed.
    abstract disposed: ISignal<ObservableDisposableSet, unit>
    /// Dispose of the set and the items it contains.
    /// 
    /// #### Notes
    /// Items are disposed in the order they are added to the set.
    abstract dispose: unit -> unit

/// An observable object which manages a collection of disposable items.
/// The namespace for the `ObservableDisposableSet` class statics.
type [<AllowNullLiteral>] ObservableDisposableSetStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ObservableDisposableSet

module ObservableDisposableSet =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create an observable disposable set from an iterable of items.</summary>
        /// <param name="items">- The iterable or array-like object of interest.</param>
        abstract from: items: IterableOrArrayLike<IDisposable> -> ObservableDisposableSet
