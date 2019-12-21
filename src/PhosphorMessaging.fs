// ts2fable 0.6.1
module rec PhosphorMessaging
open System
open Fable.Core
open Fable.Core.JS

type Error = Node.Base.Error 

type [<AllowNullLiteral>] IExports =
    abstract Message: MessageStatic
    abstract ConflatableMessage: ConflatableMessageStatic

/// A message which can be delivered to a message handler.
/// 
/// #### Notes
/// This class may be subclassed to create complex message types.
type [<AllowNullLiteral>] Message =
    /// The type of the message.
    /// 
    /// #### Notes
    /// The `type` of a message should be related directly to its actual
    /// runtime type. This means that `type` can and will be used to cast
    /// the message to the relevant derived `Message` subtype.
    abstract ``type``: string
    /// Test whether the message is conflatable.
    /// 
    /// #### Notes
    /// Message conflation is an advanced topic. Most message types will
    /// not make use of this feature.
    /// 
    /// If a conflatable message is posted to a handler while another
    /// conflatable message of the same `type` has already been posted
    /// to the handler, the `conflate()` method of the existing message
    /// will be invoked. If that method returns `true`, the new message
    /// will not be enqueued. This allows messages to be compressed, so
    /// that only a single instance of the message type is processed per
    /// cycle, no matter how many times messages of that type are posted.
    /// 
    /// Custom message types may reimplement this property.
    /// 
    /// The default implementation is always `false`.
    abstract isConflatable: bool
    /// <summary>Conflate this message with another message of the same `type`.</summary>
    /// <param name="other">- A conflatable message of the same `type`.</param>
    abstract conflate: other: Message -> bool

/// A message which can be delivered to a message handler.
/// 
/// #### Notes
/// This class may be subclassed to create complex message types.
type [<AllowNullLiteral>] MessageStatic =
    /// <summary>Construct a new message.</summary>
    /// <param name="type">- The type of the message.</param>
    [<Emit "new $0($1...)">] abstract Create: ``type``: string -> Message

/// A convenience message class which conflates automatically.
/// 
/// #### Notes
/// Message conflation is an advanced topic. Most user code will not
/// make use of this class.
/// 
/// This message class is useful for creating message instances which
/// should be conflated, but which have no state other than `type`.
/// 
/// If conflation of stateful messages is required, a custom `Message`
/// subclass should be created.
type [<AllowNullLiteral>] ConflatableMessage =
    inherit Message
    /// Test whether the message is conflatable.
    /// 
    /// #### Notes
    /// This property is always `true`.
    abstract isConflatable: bool
    /// Conflate this message with another message of the same `type`.
    /// 
    /// #### Notes
    /// This method always returns `true`.
    abstract conflate: other: ConflatableMessage -> bool

/// A convenience message class which conflates automatically.
/// 
/// #### Notes
/// Message conflation is an advanced topic. Most user code will not
/// make use of this class.
/// 
/// This message class is useful for creating message instances which
/// should be conflated, but which have no state other than `type`.
/// 
/// If conflation of stateful messages is required, a custom `Message`
/// subclass should be created.
type [<AllowNullLiteral>] ConflatableMessageStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ConflatableMessage

/// An object which handles messages.
/// 
/// #### Notes
/// A message handler is a simple way of defining a type which can act
/// upon on a large variety of external input without requiring a large
/// abstract API surface. This is particularly useful in the context of
/// widget frameworks where the number of distinct message types can be
/// unbounded.
type [<AllowNullLiteral>] IMessageHandler =
    /// <summary>Process a message sent to the handler.</summary>
    /// <param name="msg">- The message to be processed.</param>
    abstract processMessage: msg: Message -> unit

/// An object which intercepts messages sent to a message handler.
/// 
/// #### Notes
/// A message hook is useful for intercepting or spying on messages
/// sent to message handlers which were either not created by the
/// consumer, or when subclassing the handler is not feasible.
/// 
/// If `messageHook` returns `false`, no other message hooks will be
/// invoked and the message will not be delivered to the handler.
/// 
/// If all installed message hooks return `true`, the message will
/// be delivered to the handler for processing.
/// 
/// **See also:** [[installMessageHook]] and [[removeMessageHook]]
type [<AllowNullLiteral>] IMessageHook =
    /// <summary>Intercept a message sent to a message handler.</summary>
    /// <param name="handler">- The target handler of the message.</param>
    /// <param name="msg">- The message to be sent to the handler.</param>
    abstract messageHook: handler: IMessageHandler * msg: Message -> bool

type MessageHook =
    U2<IMessageHook, (IMessageHandler -> Message -> bool)>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MessageHook =
    let ofIMessageHook v: MessageHook = v |> U2.Case1
    let isIMessageHook (v: MessageHook) = match v with U2.Case1 _ -> true | _ -> false
    let asIMessageHook (v: MessageHook) = match v with U2.Case1 o -> Some o | _ -> None
    let ofCase2 v: MessageHook = v |> U2.Case2
    let isCase2 (v: MessageHook) = match v with U2.Case2 _ -> true | _ -> false
    let asCase2 (v: MessageHook) = match v with U2.Case2 o -> Some o | _ -> None

module MessageLoop =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Send a message to a message handler to process immediately.</summary>
        /// <param name="handler">- The handler which should process the message.</param>
        /// <param name="msg">- The message to deliver to the handler.
        /// 
        /// #### Notes
        /// The message will first be sent through any installed message hooks
        /// for the handler. If the message passes all hooks, it will then be
        /// delivered to the `processMessage` method of the handler.
        /// 
        /// The message will not be conflated with pending posted messages.
        /// 
        /// Exceptions in hooks and handlers will be caught and logged.</param>
        abstract sendMessage: handler: IMessageHandler * msg: Message -> unit
        /// <summary>Post a message to a message handler to process in the future.</summary>
        /// <param name="handler">- The handler which should process the message.</param>
        /// <param name="msg">- The message to post to the handler.
        /// 
        /// #### Notes
        /// The message will be conflated with the pending posted messages for
        /// the handler, if possible. If the message is not conflated, it will
        /// be queued for normal delivery on the next cycle of the event loop.
        /// 
        /// Exceptions in hooks and handlers will be caught and logged.</param>
        abstract postMessage: handler: IMessageHandler * msg: Message -> unit
        /// <summary>Install a message hook for a message handler.</summary>
        /// <param name="handler">- The message handler of interest.</param>
        /// <param name="hook">- The message hook to install.
        /// 
        /// #### Notes
        /// A message hook is invoked before a message is delivered to the
        /// handler. If the hook returns `false`, no other hooks will be
        /// invoked and the message will not be delivered to the handler.
        /// 
        /// The most recently installed message hook is executed first.
        /// 
        /// If the hook is already installed, this is a no-op.</param>
        abstract installMessageHook: handler: IMessageHandler * hook: MessageHook -> unit
        /// <summary>Remove an installed message hook for a message handler.</summary>
        /// <param name="handler">- The message handler of interest.</param>
        /// <param name="hook">- The message hook to remove.
        /// 
        /// #### Notes
        /// It is safe to call this function while the hook is executing.
        /// 
        /// If the hook is not installed, this is a no-op.</param>
        abstract removeMessageHook: handler: IMessageHandler * hook: MessageHook -> unit
        /// <summary>Clear all message data associated with a message handler.</summary>
        /// <param name="handler">- The message handler of interest.
        /// 
        /// #### Notes
        /// This will clear all posted messages and hooks for the handler.</param>
        abstract clearData: handler: IMessageHandler -> unit
        /// Process the pending posted messages in the queue immediately.
        /// 
        /// #### Notes
        /// This function is useful when posted messages must be processed
        /// immediately, instead of on the next animation frame.
        /// 
        /// This function should normally not be needed, but it may be
        /// required to work around certain browser idiosyncrasies.
        /// 
        /// Recursing into this function is a no-op.
        abstract flush: unit -> unit
        /// Get the message loop exception handler.
        abstract getExceptionHandler: unit -> ExceptionHandler
        /// <summary>Set the message loop exception handler.</summary>
        /// <param name="handler">- The function to use as the exception handler.</param>
        abstract setExceptionHandler: handler: ExceptionHandler -> ExceptionHandler

    type [<AllowNullLiteral>] ExceptionHandler =
        [<Emit "$0($1...)">] abstract Invoke: err: Error -> unit
        // [<Emit "$0($1...)">] abstract Invoke: err: obj -> unit

