// ts2fable 0.0.0
module rec PhosphorSignaling
open System
open Fable.Core

// open Fable.Core.JS

type Error =  Node.Base.Error 

type [<AllowNullLiteral>] IExports =
    abstract Signal: SignalStatic

//AMO changed
type Slot<'T, 'U> = Func<'T,'U,bool>
type Slot<'T> = Func<'T,bool>
// type [<AllowNullLiteral>] Slot<'T, 'U> =
//     [<Emit "$0($1...)">] abstract Invoke: sender: 'T * args: 'U -> unit

/// An object used for type-safe inter-object communication.
/// 
/// #### Notes
/// Signals provide a type-safe implementation of the publish-subscribe
/// pattern. An object (publisher) declares which signals it will emit,
/// and consumers connect callbacks (subscribers) to those signals. The
/// subscribers are invoked whenever the publisher emits the signal.
type [<AllowNullLiteral>] ISignal<'T, 'U> =
    /// <summary>Connect a slot to the signal.</summary>
    /// <param name="slot">- The slot to invoke when the signal is emitted.</param>
    /// <param name="thisArg">- The `this` context for the slot. If provided,
    /// this must be a non-primitive object.</param>
    abstract connect: slot: Slot<'T, 'U> * ?thisArg: obj -> bool
    /// <summary>Disconnect a slot from the signal.</summary>
    /// <param name="slot">- The slot to disconnect from the signal.</param>
    /// <param name="thisArg">- The `this` context for the slot. If provided,
    /// this must be a non-primitive object.</param>
    abstract disconnect: slot: Slot<'T, 'U> * ?thisArg: obj -> bool

/// A concrete implementation of `ISignal`.
/// 
/// #### Example
/// ```typescript
/// import { ISignal, Signal } from '@phosphor/signaling';
/// 
/// class SomeClass {
/// 
///    constructor(name: string) {
///      this.name = name;
///    }
/// 
///    readonly name: string;
/// 
///    get valueChanged: ISignal<this, number> {
///      return this._valueChanged;
///    }
/// 
///    get value(): number {
///      return this._value;
///    }
/// 
///    set value(value: number) {
///      if (value === this._value) {
///        return;
///      }
///      this._value = value;
///      this._valueChanged.emit(value);
///    }
/// 
///    private _value = 0;
///    private _valueChanged = new Signal<this, number>(this);
/// }
/// 
/// function logger(sender: SomeClass, value: number): void {
///    console.log(sender.name, value);
/// }
/// 
/// let m1 = new SomeClass('foo');
/// let m2 = new SomeClass('bar');
/// 
/// m1.valueChanged.connect(logger);
/// m2.valueChanged.connect(logger);
/// 
/// m1.value = 42;  // logs: foo 42
/// m2.value = 17;  // logs: bar 17
/// ```
/// The namespace for the `Signal` class statics.
type [<AllowNullLiteral>] Signal<'T, 'U> =
    inherit ISignal<'T, 'U>
    /// The sender which owns the signal.
    abstract sender: 'T
    /// <summary>Connect a slot to the signal.</summary>
    /// <param name="slot">- The slot to invoke when the signal is emitted.</param>
    /// <param name="thisArg">- The `this` context for the slot. If provided,
    /// this must be a non-primitive object.</param>
    abstract connect: slot: Slot<'T, 'U> * ?thisArg: obj -> bool
    /// <summary>Disconnect a slot from the signal.</summary>
    /// <param name="slot">- The slot to disconnect from the signal.</param>
    /// <param name="thisArg">- The `this` context for the slot. If provided,
    /// this must be a non-primitive object.</param>
    abstract disconnect: slot: Slot<'T, 'U> * ?thisArg: obj -> bool
    /// <summary>Emit the signal and invoke the connected slots.</summary>
    /// <param name="args">- The args to pass to the connected slots.
    /// 
    /// #### Notes
    /// Slots are invoked synchronously in connection order.
    /// 
    /// Exceptions thrown by connected slots will be caught and logged.</param>
    abstract emit: args: 'U -> unit

/// A concrete implementation of `ISignal`.
/// 
/// #### Example
/// ```typescript
/// import { ISignal, Signal } from '@phosphor/signaling';
/// 
/// class SomeClass {
/// 
///    constructor(name: string) {
///      this.name = name;
///    }
/// 
///    readonly name: string;
/// 
///    get valueChanged: ISignal<this, number> {
///      return this._valueChanged;
///    }
/// 
///    get value(): number {
///      return this._value;
///    }
/// 
///    set value(value: number) {
///      if (value === this._value) {
///        return;
///      }
///      this._value = value;
///      this._valueChanged.emit(value);
///    }
/// 
///    private _value = 0;
///    private _valueChanged = new Signal<this, number>(this);
/// }
/// 
/// function logger(sender: SomeClass, value: number): void {
///    console.log(sender.name, value);
/// }
/// 
/// let m1 = new SomeClass('foo');
/// let m2 = new SomeClass('bar');
/// 
/// m1.valueChanged.connect(logger);
/// m2.valueChanged.connect(logger);
/// 
/// m1.value = 42;  // logs: foo 42
/// m2.value = 17;  // logs: bar 17
/// ```
/// The namespace for the `Signal` class statics.
type [<AllowNullLiteral>] SignalStatic =
    /// <summary>Construct a new signal.</summary>
    /// <param name="sender">- The sender which owns the signal.</param>
    [<Emit "new $0($1...)">] abstract Create: sender: 'T -> Signal<'T, 'U>

module Signal =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Remove all connections between a sender and receiver.</summary>
        /// <param name="sender">- The sender object of interest.</param>
        /// <param name="receiver">- The receiver object of interest.
        /// 
        /// #### Notes
        /// If a `thisArg` is provided when connecting a signal, that object
        /// is considered the receiver. Otherwise, the `slot` is considered
        /// the receiver.</param>
        abstract disconnectBetween: sender: obj option * receiver: obj option -> unit
        /// <summary>Remove all connections where the given object is the sender.</summary>
        /// <param name="sender">- The sender object of interest.</param>
        abstract disconnectSender: sender: obj option -> unit
        /// <summary>Remove all connections where the given object is the receiver.</summary>
        /// <param name="receiver">- The receiver object of interest.
        /// 
        /// #### Notes
        /// If a `thisArg` is provided when connecting a signal, that object
        /// is considered the receiver. Otherwise, the `slot` is considered
        /// the receiver.</param>
        abstract disconnectReceiver: receiver: obj option -> unit
        /// <summary>Remove all connections where an object is the sender or receiver.</summary>
        /// <param name="object">- The object of interest.
        /// 
        /// #### Notes
        /// If a `thisArg` is provided when connecting a signal, that object
        /// is considered the receiver. Otherwise, the `slot` is considered
        /// the receiver.</param>
        abstract disconnectAll: ``object``: obj option -> unit
        /// <summary>Clear all signal data associated with the given object.</summary>
        /// <param name="object">- The object for which the data should be cleared.
        /// 
        /// #### Notes
        /// This removes all signal connections and any other signal data
        /// associated with the object.</param>
        abstract clearData: ``object``: obj option -> unit
        /// Get the signal exception handler.
        abstract getExceptionHandler: unit -> ExceptionHandler
        /// <summary>Set the signal exception handler.</summary>
        /// <param name="handler">- The function to use as the exception handler.</param>
        abstract setExceptionHandler: handler: ExceptionHandler -> ExceptionHandler

    type [<AllowNullLiteral>] ExceptionHandler =
        [<Emit "$0($1...)">] abstract Invoke: err: Error -> unit
        // [<Emit "$0($1...)">] abstract Invoke: err: obj -> unit

