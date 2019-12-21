// ts2fable 0.6.1
module rec PhosphorWidgets
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

//amo moved exports here to avoid collisions
type [<AllowNullLiteral>] IExports =
    abstract BoxSizer: BoxSizerStatic
    abstract Title: TitleStatic
    abstract Widget: WidgetStatic
    abstract Layout: LayoutStatic
    abstract LayoutItem: LayoutItemStatic
    abstract PanelLayout: PanelLayoutStatic
    abstract BoxLayout: BoxLayoutStatic
    abstract Panel: PanelStatic
    abstract BoxPanel: BoxPanelStatic
    abstract CommandPalette: CommandPaletteStatic
    abstract Menu: MenuStatic
    abstract ContextMenu: ContextMenuStatic
    abstract TabBar: TabBarStatic
    abstract DockLayout: DockLayoutStatic
    abstract DockPanel: DockPanelStatic
    abstract FocusTracker: FocusTrackerStatic
    abstract GridLayout: GridLayoutStatic
    abstract MenuBar: MenuBarStatic
    abstract ScrollBar: ScrollBarStatic
    abstract SingletonLayout: SingletonLayoutStatic
    abstract SplitLayout: SplitLayoutStatic
    abstract SplitPanel: SplitPanelStatic
    abstract StackedLayout: StackedLayoutStatic
    abstract StackedPanel: StackedPanelStatic
    abstract TabPanel: TabPanelStatic

[<Import("*","@phosphor/widgets")>]
let Types:IExports = jsNative

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo: end typescript hacks


/// A sizer object for use with the box engine layout functions.
/// 
/// #### Notes
/// A box sizer holds the geometry information for an object along an
/// arbitrary layout orientation.
/// 
/// For best performance, this class should be treated as a raw data
/// struct. It should not typically be subclassed.
type [<AllowNullLiteral>] BoxSizer =
    /// The preferred size for the sizer.
    /// 
    /// #### Notes
    /// The sizer will be given this initial size subject to its size
    /// bounds. The sizer will not deviate from this size unless such
    /// deviation is required to fit into the available layout space.
    /// 
    /// There is no limit to this value, but it will be clamped to the
    /// bounds defined by [[minSize]] and [[maxSize]].
    /// 
    /// The default value is `0`.
    abstract sizeHint: float with get, set
    /// The minimum size of the sizer.
    /// 
    /// #### Notes
    /// The sizer will never be sized less than this value, even if
    /// it means the sizer will overflow the available layout space.
    /// 
    /// It is assumed that this value lies in the range `[0, Infinity)`
    /// and that it is `<=` to [[maxSize]]. Failure to adhere to this
    /// constraint will yield undefined results.
    /// 
    /// The default value is `0`.
    abstract minSize: float with get, set
    /// The maximum size of the sizer.
    /// 
    /// #### Notes
    /// The sizer will never be sized greater than this value, even if
    /// it means the sizer will underflow the available layout space.
    /// 
    /// It is assumed that this value lies in the range `[0, Infinity]`
    /// and that it is `>=` to [[minSize]]. Failure to adhere to this
    /// constraint will yield undefined results.
    /// 
    /// The default value is `Infinity`.
    abstract maxSize: float with get, set
    /// The stretch factor for the sizer.
    /// 
    /// #### Notes
    /// This controls how much the sizer stretches relative to its sibling
    /// sizers when layout space is distributed. A stretch factor of zero
    /// is special and will cause the sizer to only be resized after all
    /// other sizers with a stretch factor greater than zero have been
    /// resized to their limits.
    /// 
    /// It is assumed that this value is an integer that lies in the range
    /// `[0, Infinity)`. Failure to adhere to this constraint will yield
    /// undefined results.
    /// 
    /// The default value is `1`.
    abstract stretch: float with get, set
    /// The computed size of the sizer.
    /// 
    /// #### Notes
    /// This value is the output of a call to [[boxCalc]]. It represents
    /// the computed size for the object along the layout orientation,
    /// and will always lie in the range `[minSize, maxSize]`.
    /// 
    /// This value is output only.
    /// 
    /// Changing this value will have no effect.
    abstract size: float with get, set
    /// An internal storage property for the layout algorithm.
    /// 
    /// #### Notes
    /// This value is used as temporary storage by the layout algorithm.
    /// 
    /// Changing this value will have no effect.
    abstract ``done``: bool with get, set

/// A sizer object for use with the box engine layout functions.
/// 
/// #### Notes
/// A box sizer holds the geometry information for an object along an
/// arbitrary layout orientation.
/// 
/// For best performance, this class should be treated as a raw data
/// struct. It should not typically be subclassed.
type [<AllowNullLiteral>] BoxSizerStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> BoxSizer

module BoxEngine =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Calculate the optimal layout sizes for a sequence of box sizers.
        /// 
        /// This distributes the available layout space among the box sizers
        /// according to the following algorithm:
        /// 
        /// 1. Initialize the sizers's size to its size hint and compute the
        ///     sums for each of size hint, min size, and max size.
        /// 
        /// 2. If the total size hint equals the available space, return.
        /// 
        /// 3. If the available space is less than the total min size, set all
        ///     sizers to their min size and return.
        /// 
        /// 4. If the available space is greater than the total max size, set
        ///     all sizers to their max size and return.
        /// 
        /// 5. If the layout space is less than the total size hint, distribute
        ///     the negative delta as follows:
        /// 
        ///     a. Shrink each sizer with a stretch factor greater than zero by
        ///        an amount proportional to the negative space and the sum of
        ///        stretch factors. If the sizer reaches its min size, remove
        ///        it and its stretch factor from the computation.
        /// 
        ///     b. If after adjusting all stretch sizers there remains negative
        ///        space, distribute the space equally among the sizers with a
        ///        stretch factor of zero. If a sizer reaches its min size,
        ///        remove it from the computation.
        /// 
        /// 6. If the layout space is greater than the total size hint,
        ///     distribute the positive delta as follows:
        /// 
        ///     a. Expand each sizer with a stretch factor greater than zero by
        ///        an amount proportional to the postive space and the sum of
        ///        stretch factors. If the sizer reaches its max size, remove
        ///        it and its stretch factor from the computation.
        /// 
        ///     b. If after adjusting all stretch sizers there remains positive
        ///        space, distribute the space equally among the sizers with a
        ///        stretch factor of zero. If a sizer reaches its max size,
        ///        remove it from the computation.
        /// 
        /// 7. return</summary>
        /// <param name="sizers">- The sizers for a particular layout line.</param>
        /// <param name="space">- The available layout space for the sizers.</param>
        abstract calc: sizers: ArrayLike<BoxSizer> * space: float -> float
        /// <summary>Adjust a sizer by a delta and update its neighbors accordingly.</summary>
        /// <param name="sizers">- The sizers which should be adjusted.</param>
        /// <param name="index">- The index of the sizer to grow.</param>
        /// <param name="delta">- The amount to adjust the sizer, positive or negative.
        /// 
        /// #### Notes
        /// This will adjust the indicated sizer by the specified amount, along
        /// with the sizes of the appropriate neighbors, subject to the limits
        /// specified by each of the sizers.
        /// 
        /// This is useful when implementing box layouts where the boundaries
        /// between the sizers are interactively adjustable by the user.</param>
        abstract adjust: sizers: ArrayLike<BoxSizer> * index: float * delta: float -> unit
type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal

// type [<AllowNullLiteral>] IExports =
//     abstract Title: TitleStatic

/// An object which holds data related to an object's title.
/// 
/// #### Notes
/// A title object is intended to hold the data necessary to display a
/// header for a particular object. A common example is the `TabPanel`,
/// which uses the widget title to populate the tab for a child widget.
/// The namespace for the `Title` class statics.
type [<AllowNullLiteral>] Title<'T> =
    /// A signal emitted when the state of the title changes.
    abstract changed: ISignal<Title<'T>, unit>
    /// The object which owns the title.
    abstract owner: 'T
    /// Get the label for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the label for the title.
    abstract label: string with get, set
    /// Get the mnemonic index for the title.
    /// 
    /// #### Notes
    /// The default value is `-1`.
    /// Set the mnemonic index for the title.
    abstract mnemonic: float with get, set
    abstract icon: string with get, set
    /// Get the icon class name for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the icon class name for the title.
    /// 
    /// #### Notes
    /// Multiple class names can be separated with whitespace.
    abstract iconClass: string with get, set
    /// Get the icon label for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the icon label for the title.
    /// 
    /// #### Notes
    /// Multiple class names can be separated with whitespace.
    abstract iconLabel: string with get, set
    /// Get the caption for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the caption for the title.
    abstract caption: string with get, set
    /// Get the extra class name for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the extra class name for the title.
    /// 
    /// #### Notes
    /// Multiple class names can be separated with whitespace.
    abstract className: string with get, set
    /// Get the closable state for the title.
    /// 
    /// #### Notes
    /// The default value is `false`.
    /// Set the closable state for the title.
    /// 
    /// #### Notes
    /// This controls the presence of a close icon when applicable.
    abstract closable: bool with get, set
    /// Get the dataset for the title.
    /// 
    /// #### Notes
    /// The default value is an empty dataset.
    /// Set the dataset for the title.
    /// 
    /// #### Notes
    /// This controls the data attributes when applicable.
    abstract dataset: Title.Dataset with get, set
    abstract _label: obj with get, set
    abstract _caption: obj with get, set
    abstract _mnemonic: obj with get, set
    abstract _iconClass: obj with get, set
    abstract _iconLabel: obj with get, set
    abstract _className: obj with get, set
    abstract _closable: obj with get, set
    abstract _dataset: obj with get, set
    abstract _changed: obj with get, set

/// An object which holds data related to an object's title.
/// 
/// #### Notes
/// A title object is intended to hold the data necessary to display a
/// header for a particular object. A common example is the `TabPanel`,
/// which uses the widget title to populate the tab for a child widget.
/// The namespace for the `Title` class statics.
type [<AllowNullLiteral>] TitleStatic =
    /// <summary>Construct a new title.</summary>
    /// <param name="options">- The options for initializing the title.</param>
    [<Emit "new $0($1...)">] abstract Create: options: Title.IOptions<'T> -> Title<'T>

module Title =

    type [<AllowNullLiteral>] Dataset =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string

    /// An options object for initializing a title.
    type [<AllowNullLiteral>] IOptions<'T> =
        /// The object which owns the title.
        abstract owner: 'T with get, set
        /// The label for the title.
        abstract label: string option with get, set
        /// The mnemonic index for the title.
        abstract mnemonic: float option with get, set
        abstract icon: string option with get, set
        /// The icon class name for the title.
        abstract iconClass: string option with get, set
        /// The icon label for the title.
        abstract iconLabel: string option with get, set
        /// The caption for the title.
        abstract caption: string option with get, set
        /// The extra class name for the title.
        abstract className: string option with get, set
        /// The closable state for the title.
        abstract closable: bool option with get, set
        /// The dataset for the title.
        abstract dataset: Dataset option with get, set
type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T>
type IObservableDisposable = PhosphorDisposable.IObservableDisposable
type ConflatableMessage = PhosphorMessaging.ConflatableMessage
type IMessageHandler = PhosphorMessaging.IMessageHandler
type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type Layout = __layout.Layout
// type Title = __title.Title

// type [<AllowNullLiteral>] IExports =
//     abstract Widget: WidgetStatic

/// The base class of the Phosphor widget hierarchy.
/// 
/// #### Notes
/// This class will typically be subclassed in order to create a useful
/// widget. However, it can be used directly to host externally created
/// content.
/// The namespace for the `Widget` class statics.
type [<AllowNullLiteral>] Widget =
    inherit IMessageHandler
    inherit IObservableDisposable
    /// Dispose of the widget and its descendant widgets.
    /// 
    /// #### Notes
    /// It is unsafe to use the widget after it has been disposed.
    /// 
    /// All calls made to this method after the first are a no-op.
    abstract dispose: unit -> unit
    /// A signal emitted when the widget is disposed.
    abstract disposed: ISignal<Widget, unit>
    /// Get the DOM node owned by the widget.
    abstract node: HTMLElement
    /// Test whether the widget has been disposed.
    abstract isDisposed: bool
    /// Test whether the widget's node is attached to the DOM.
    abstract isAttached: bool
    /// Test whether the widget is explicitly hidden.
    abstract isHidden: bool
    /// Test whether the widget is visible.
    /// 
    /// #### Notes
    /// A widget is visible when it is attached to the DOM, is not
    /// explicitly hidden, and has no explicitly hidden ancestors.
    abstract isVisible: bool
    /// The title object for the widget.
    /// 
    /// #### Notes
    /// The title object is used by some container widgets when displaying
    /// the widget alongside some title, such as a tab panel or side bar.
    /// 
    /// Since not all widgets will use the title, it is created on demand.
    /// 
    /// The `owner` property of the title is set to this widget.
    abstract title: Title<Widget>
    /// Get the id of the widget's DOM node.
    /// Set the id of the widget's DOM node.
    abstract id: string with get, set
    /// The dataset for the widget's DOM node.
    abstract dataset: DOMStringMap
    /// Get the parent of the widget.
    /// Set the parent of the widget.
    /// 
    /// #### Notes
    /// Children are typically added to a widget by using a layout, which
    /// means user code will not normally set the parent widget directly.
    /// 
    /// The widget will be automatically removed from its old parent.
    /// 
    /// This is a no-op if there is no effective parent change.
    abstract parent: Widget option with get, set
    /// Get the layout for the widget.
    /// Set the layout for the widget.
    /// 
    /// #### Notes
    /// The layout is single-use only. It cannot be changed after the
    /// first assignment.
    /// 
    /// The layout is disposed automatically when the widget is disposed.
    abstract layout: Layout option with get, set
    /// Create an iterator over the widget's children.
    abstract children: unit -> IIterator<Widget>
    /// <summary>Test whether a widget is a descendant of this widget.</summary>
    /// <param name="widget">- The descendant widget of interest.</param>
    abstract contains: widget: Widget -> bool
    /// <summary>Test whether the widget's DOM node has the given class name.</summary>
    /// <param name="name">- The class name of interest.</param>
    abstract hasClass: name: string -> bool
    /// <summary>Add a class name to the widget's DOM node.</summary>
    /// <param name="name">- The class name to add to the node.
    /// 
    /// #### Notes
    /// If the class name is already added to the node, this is a no-op.
    /// 
    /// The class name must not contain whitespace.</param>
    abstract addClass: name: string -> unit
    /// <summary>Remove a class name from the widget's DOM node.</summary>
    /// <param name="name">- The class name to remove from the node.
    /// 
    /// #### Notes
    /// If the class name is not yet added to the node, this is a no-op.
    /// 
    /// The class name must not contain whitespace.</param>
    abstract removeClass: name: string -> unit
    /// <summary>Toggle a class name on the widget's DOM node.</summary>
    /// <param name="name">- The class name to toggle on the node.</param>
    /// <param name="force">- Whether to force add the class (`true`) or force
    /// remove the class (`false`). If not provided, the presence of
    /// the class will be toggled from its current state.</param>
    abstract toggleClass: name: string * ?force: bool -> bool
    /// Post an `'update-request'` message to the widget.
    /// 
    /// #### Notes
    /// This is a simple convenience method for posting the message.
    abstract update: unit -> unit
    /// Post a `'fit-request'` message to the widget.
    /// 
    /// #### Notes
    /// This is a simple convenience method for posting the message.
    abstract fit: unit -> unit
    /// Post an `'activate-request'` message to the widget.
    /// 
    /// #### Notes
    /// This is a simple convenience method for posting the message.
    abstract activate: unit -> unit
    /// Send a `'close-request'` message to the widget.
    /// 
    /// #### Notes
    /// This is a simple convenience method for sending the message.
    abstract close: unit -> unit
    /// Show the widget and make it visible to its parent widget.
    /// 
    /// #### Notes
    /// This causes the [[isHidden]] property to be `false`.
    /// 
    /// If the widget is not explicitly hidden, this is a no-op.
    abstract show: unit -> unit
    /// Hide the widget and make it hidden to its parent widget.
    /// 
    /// #### Notes
    /// This causes the [[isHidden]] property to be `true`.
    /// 
    /// If the widget is explicitly hidden, this is a no-op.
    abstract hide: unit -> unit
    /// <summary>Show or hide the widget according to a boolean value.</summary>
    /// <param name="hidden">- `true` to hide the widget, or `false` to show it.
    /// 
    /// #### Notes
    /// This is a convenience method for `hide()` and `show()`.</param>
    abstract setHidden: hidden: bool -> unit
    /// Test whether the given widget flag is set.
    /// 
    /// #### Notes
    /// This will not typically be called directly by user code.
    abstract testFlag: flag: Widget.Flag -> bool
    /// Set the given widget flag.
    /// 
    /// #### Notes
    /// This will not typically be called directly by user code.
    abstract setFlag: flag: Widget.Flag -> unit
    /// Clear the given widget flag.
    /// 
    /// #### Notes
    /// This will not typically be called directly by user code.
    abstract clearFlag: flag: Widget.Flag -> unit
    /// <summary>Process a message sent to the widget.</summary>
    /// <param name="msg">- The message sent to the widget.
    /// 
    /// #### Notes
    /// Subclasses may reimplement this method as needed.</param>
    abstract processMessage: msg: Message -> unit
    /// <summary>Invoke the message processing routine of the widget's layout.</summary>
    /// <param name="msg">- The message to dispatch to the layout.
    /// 
    /// #### Notes
    /// This is a no-op if the widget does not have a layout.
    /// 
    /// This will not typically be called directly by user code.</param>
    abstract notifyLayout: msg: Message -> unit
    /// A message handler invoked on a `'close-request'` message.
    /// 
    /// #### Notes
    /// The default implementation unparents or detaches the widget.
    abstract onCloseRequest: msg: Message -> unit
    /// A message handler invoked on a `'resize'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onFitRequest: msg: Message -> unit
    /// A message handler invoked on an `'activate-request'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onActivateRequest: msg: Message -> unit
    /// A message handler invoked on a `'before-show'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on an `'after-show'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onAfterShow: msg: Message -> unit
    /// A message handler invoked on a `'before-hide'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onBeforeHide: msg: Message -> unit
    /// A message handler invoked on an `'after-hide'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onAfterHide: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-attach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onAfterAttach: msg: Message -> unit
    /// A message handler invoked on a `'before-detach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onBeforeDetach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on a `'child-added'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onChildAdded: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-removed'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit
    abstract _flags: obj with get, set
    abstract _layout: obj with get, set
    abstract _parent: obj with get, set
    abstract _disposed: obj with get, set

/// The base class of the Phosphor widget hierarchy.
/// 
/// #### Notes
/// This class will typically be subclassed in order to create a useful
/// widget. However, it can be used directly to host externally created
/// content.
/// The namespace for the `Widget` class statics.
type [<AllowNullLiteral>] WidgetStatic =
    /// <summary>Construct a new widget.</summary>
    /// <param name="options">- The options for initializing the widget.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: Widget.IOptions -> Widget

module Widget =
    let [<Import("Msg","@phosphor/widgets/lib/widget/Widget")>] msg: Msg.IExports = jsNative
    let [<Import("ResizeMessage","@phosphor/widgets/lib/widget/Widget")>] resizeMessage: ResizeMessage.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        abstract ChildMessage: ChildMessageStatic
        abstract ResizeMessage: ResizeMessageStatic
        /// <summary>Attach a widget to a host DOM node.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="host">- The DOM node to use as the widget's host.</param>
        /// <param name="ref">- The child of `host` to use as the reference element.
        /// If this is provided, the widget will be inserted before this
        /// node in the host. The default is `null`, which will cause the
        /// widget to be added as the last child of the host.
        /// 
        /// #### Notes
        /// This will throw an error if the widget is not a root widget, if
        /// the widget is already attached, or if the host is not attached
        /// to the DOM.</param>
        abstract attach: widget: Widget * host: HTMLElement * ?ref: HTMLElement option -> unit
        /// <summary>Detach the widget from its host DOM node.</summary>
        /// <param name="widget">- The widget of interest.
        /// 
        /// #### Notes
        /// This will throw an error if the widget is not a root widget,
        /// or if the widget is not attached to the DOM.</param>
        abstract detach: widget: Widget -> unit

    /// An options object for initializing a widget.
    type [<AllowNullLiteral>] IOptions =
        /// The optional node to use for the widget.
        /// 
        /// If a node is provided, the widget will assume full ownership
        /// and control of the node, as if it had created the node itself.
        /// 
        /// The default is a new `<div>`.
        abstract node: HTMLElement option with get, set

    type [<RequireQualifiedAccess>] Flag =
        | IsDisposed = 1
        | IsAttached = 2
        | IsHidden = 4
        | IsVisible = 8
        | DisallowLayout = 16

    module Msg =

        type [<AllowNullLiteral>] IExports =
            abstract BeforeShow: Message
            abstract AfterShow: Message
            abstract BeforeHide: Message
            abstract AfterHide: Message
            abstract BeforeAttach: Message
            abstract AfterAttach: Message
            abstract BeforeDetach: Message
            abstract AfterDetach: Message
            abstract ParentChanged: Message
            abstract UpdateRequest: ConflatableMessage
            abstract FitRequest: ConflatableMessage
            abstract ActivateRequest: ConflatableMessage
            abstract CloseRequest: ConflatableMessage

    /// A message class for child related messages.
    type [<AllowNullLiteral>] ChildMessage =
        inherit Message
        /// The child widget for the message.
        abstract child: Widget

    /// A message class for child related messages.
    type [<AllowNullLiteral>] ChildMessageStatic =
        /// <summary>Construct a new child message.</summary>
        /// <param name="type">- The message type.</param>
        /// <param name="child">- The child widget for the message.</param>
        [<Emit "new $0($1...)">] abstract Create: ``type``: string * child: Widget -> ChildMessage

    /// A message class for `'resize'` messages.
    /// The namespace for the `ResizeMessage` class statics.
    type [<AllowNullLiteral>] ResizeMessage =
        inherit Message
        /// The offset width of the widget.
        /// 
        /// #### Notes
        /// This will be `-1` if the width is unknown.
        abstract width: float
        /// The offset height of the widget.
        /// 
        /// #### Notes
        /// This will be `-1` if the height is unknown.
        abstract height: float

    /// A message class for `'resize'` messages.
    /// The namespace for the `ResizeMessage` class statics.
    type [<AllowNullLiteral>] ResizeMessageStatic =
        /// <summary>Construct a new resize message.</summary>
        /// <param name="width">- The **offset width** of the widget, or `-1` if
        /// the width is not known.</param>
        /// <param name="height">- The **offset height** of the widget, or `-1` if
        /// the height is not known.</param>
        [<Emit "new $0($1...)">] abstract Create: width: float * height: float -> ResizeMessage

    module ResizeMessage =

        type [<AllowNullLiteral>] IExports =
            abstract UnknownSize: ResizeMessage
type IIterable<'T> = PhosphorAlgorithm.Iter.IIterable<'T>
// type IIterator = PhosphorAlgorithm.IIterator
type IDisposable = PhosphorDisposable.IDisposable
// type Message = PhosphorMessaging.Message
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract Layout: LayoutStatic
//     abstract LayoutItem: LayoutItemStatic

/// An abstract base class for creating Phosphor layouts.
/// 
/// #### Notes
/// A layout is used to add widgets to a parent and to arrange those
/// widgets within the parent's DOM node.
/// 
/// This class implements the base functionality which is required of
/// nearly all layouts. It must be subclassed in order to be useful.
/// 
/// Notably, this class does not define a uniform interface for adding
/// widgets to the layout. A subclass should define that API in a way
/// which is meaningful for its intended use.
/// The namespace for the `Layout` class statics.
type [<AllowNullLiteral>] Layout =
    inherit IIterable<Widget>
    inherit IDisposable
    /// Dispose of the resources held by the layout.
    /// 
    /// #### Notes
    /// This should be reimplemented to clear and dispose of the widgets.
    /// 
    /// All reimplementations should call the superclass method.
    /// 
    /// This method is called automatically when the parent is disposed.
    abstract dispose: unit -> unit
    /// Test whether the layout is disposed.
    abstract isDisposed: bool
    /// Get the parent widget of the layout.
    /// Set the parent widget of the layout.
    /// 
    /// #### Notes
    /// This is set automatically when installing the layout on the parent
    /// widget. The parent widget should not be set directly by user code.
    abstract parent: Widget option with get, set
    /// Get the fit policy for the layout.
    /// 
    /// #### Notes
    /// The fit policy controls the computed size constraints which are
    /// applied to the parent widget by the layout.
    /// 
    /// Some layout implementations may ignore the fit policy.
    /// Set the fit policy for the layout.
    /// 
    /// #### Notes
    /// The fit policy controls the computed size constraints which are
    /// applied to the parent widget by the layout.
    /// 
    /// Some layout implementations may ignore the fit policy.
    /// 
    /// Changing the fit policy will clear the current size constraint
    /// for the parent widget and then re-fit the parent.
    abstract fitPolicy: Layout.FitPolicy with get, set
    /// Create an iterator over the widgets in the layout.
    abstract iter: unit -> IIterator<Widget>
    /// <summary>Remove a widget from the layout.</summary>
    /// <param name="widget">- The widget to remove from the layout.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method should *not* modify the widget's `parent`.</param>
    abstract removeWidget: widget: Widget -> unit
    /// <summary>Process a message sent to the parent widget.</summary>
    /// <param name="msg">- The message sent to the parent widget.
    /// 
    /// #### Notes
    /// This method is called by the parent widget to process a message.
    /// 
    /// Subclasses may reimplement this method as needed.</param>
    abstract processParentMessage: msg: Message -> unit
    /// Perform layout initialization which requires the parent widget.
    /// 
    /// #### Notes
    /// This method is invoked immediately after the layout is installed
    /// on the parent widget.
    /// 
    /// The default implementation reparents all of the widgets to the
    /// layout parent widget.
    /// 
    /// Subclasses should reimplement this method and attach the child
    /// widget nodes to the parent widget's node.
    abstract init: unit -> unit
    /// A message handler invoked on a `'resize'` message.
    /// 
    /// #### Notes
    /// The layout should ensure that its widgets are resized according
    /// to the specified layout space, and that they are sent a `'resize'`
    /// message if appropriate.
    /// 
    /// The default implementation of this method sends an `UnknownSize`
    /// resize message to all widgets.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    /// 
    /// #### Notes
    /// The layout should ensure that its widgets are resized according
    /// to the available layout space, and that they are sent a `'resize'`
    /// message if appropriate.
    /// 
    /// The default implementation of this method sends an `UnknownSize`
    /// resize message to all widgets.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message
    /// to all widgets. It assumes all widget nodes are attached to the
    /// parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-attach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message
    /// to all widgets. It assumes all widget nodes are attached to the
    /// parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onAfterAttach: msg: Message -> unit
    /// A message handler invoked on a `'before-detach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message
    /// to all widgets. It assumes all widget nodes are attached to the
    /// parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onBeforeDetach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message
    /// to all widgets. It assumes all widget nodes are attached to the
    /// parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on a `'before-show'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message to
    /// all non-hidden widgets. It assumes all widget nodes are attached
    /// to the parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on an `'after-show'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message to
    /// all non-hidden widgets. It assumes all widget nodes are attached
    /// to the parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onAfterShow: msg: Message -> unit
    /// A message handler invoked on a `'before-hide'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message to
    /// all non-hidden widgets. It assumes all widget nodes are attached
    /// to the parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onBeforeHide: msg: Message -> unit
    /// A message handler invoked on an `'after-hide'` message.
    /// 
    /// #### Notes
    /// The default implementation of this method forwards the message to
    /// all non-hidden widgets. It assumes all widget nodes are attached
    /// to the parent widget node.
    /// 
    /// This may be reimplemented by subclasses as needed.
    abstract onAfterHide: msg: Message -> unit
    /// A message handler invoked on a `'child-removed'` message.
    /// 
    /// #### Notes
    /// This will remove the child widget from the layout.
    /// 
    /// Subclasses should **not** typically reimplement this method.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'fit-request'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onFitRequest: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    /// 
    /// #### Notes
    /// The default implementation of this handler is a no-op.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    abstract _disposed: obj with get, set
    abstract _fitPolicy: obj with get, set
    abstract _parent: obj with get, set

/// An abstract base class for creating Phosphor layouts.
/// 
/// #### Notes
/// A layout is used to add widgets to a parent and to arrange those
/// widgets within the parent's DOM node.
/// 
/// This class implements the base functionality which is required of
/// nearly all layouts. It must be subclassed in order to be useful.
/// 
/// Notably, this class does not define a uniform interface for adding
/// widgets to the layout. A subclass should define that API in a way
/// which is meaningful for its intended use.
/// The namespace for the `Layout` class statics.
type [<AllowNullLiteral>] LayoutStatic =
    /// <summary>Construct a new layout.</summary>
    /// <param name="options">- The options for initializing the layout.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: Layout.IOptions -> Layout

module Layout =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Get the horizontal alignment for a widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getHorizontalAlignment: widget: Widget -> HorizontalAlignment
        /// <summary>Set the horizontal alignment for a widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the horizontal alignment.
        /// 
        /// #### Notes
        /// If the layout width allocated to a widget is larger than its max
        /// width, the horizontal alignment controls how the widget is placed
        /// within the extra horizontal space.
        /// 
        /// If the allocated width is less than the widget's max width, the
        /// horizontal alignment has no effect.
        /// 
        /// Some layout implementations may ignore horizontal alignment.
        /// 
        /// Changing the horizontal alignment will post an `update-request`
        /// message to widget's parent, provided the parent has a layout
        /// installed.</param>
        abstract setHorizontalAlignment: widget: Widget * value: HorizontalAlignment -> unit
        /// <summary>Get the vertical alignment for a widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getVerticalAlignment: widget: Widget -> VerticalAlignment
        /// <summary>Set the vertical alignment for a widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the vertical alignment.
        /// 
        /// #### Notes
        /// If the layout height allocated to a widget is larger than its max
        /// height, the vertical alignment controls how the widget is placed
        /// within the extra vertical space.
        /// 
        /// If the allocated height is less than the widget's max height, the
        /// vertical alignment has no effect.
        /// 
        /// Some layout implementations may ignore vertical alignment.
        /// 
        /// Changing the horizontal alignment will post an `update-request`
        /// message to widget's parent, provided the parent has a layout
        /// installed.</param>
        abstract setVerticalAlignment: widget: Widget * value: VerticalAlignment -> unit

    type [<StringEnum>] [<RequireQualifiedAccess>] FitPolicy =
        | [<CompiledName "set-no-constraint">] SetNoConstraint
        | [<CompiledName "set-min-size">] SetMinSize

    /// An options object for initializing a layout.
    type [<AllowNullLiteral>] IOptions =
        /// The fit policy for the layout.
        /// 
        /// The default is `'set-min-size'`.
        abstract fitPolicy: FitPolicy option with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] HorizontalAlignment =
        | Left
        | Center
        | Right

    type [<StringEnum>] [<RequireQualifiedAccess>] VerticalAlignment =
        | Top
        | Center
        | Bottom

/// An object which assists in the absolute layout of widgets.
/// 
/// #### Notes
/// This class is useful when implementing a layout which arranges its
/// widgets using absolute positioning.
/// 
/// This class is used by nearly all of the built-in Phosphor layouts.
type [<AllowNullLiteral>] LayoutItem =
    inherit IDisposable
    /// Dispose of the the layout item.
    /// 
    /// #### Notes
    /// This will reset the positioning of the widget.
    abstract dispose: unit -> unit
    /// The widget managed by the layout item.
    abstract widget: Widget
    /// The computed minimum width of the widget.
    /// 
    /// #### Notes
    /// This value can be updated by calling the `fit` method.
    abstract minWidth: float
    /// The computed minimum height of the widget.
    /// 
    /// #### Notes
    /// This value can be updated by calling the `fit` method.
    abstract minHeight: float
    /// The computed maximum width of the widget.
    /// 
    /// #### Notes
    /// This value can be updated by calling the `fit` method.
    abstract maxWidth: float
    /// The computed maximum height of the widget.
    /// 
    /// #### Notes
    /// This value can be updated by calling the `fit` method.
    abstract maxHeight: float
    /// Whether the layout item is disposed.
    abstract isDisposed: bool
    /// Whether the managed widget is hidden.
    abstract isHidden: bool
    /// Whether the managed widget is visible.
    abstract isVisible: bool
    /// Whether the managed widget is attached.
    abstract isAttached: bool
    /// Update the computed size limits of the managed widget.
    abstract fit: unit -> unit
    /// <summary>Update the position and size of the managed widget.</summary>
    /// <param name="left">- The left edge position of the layout box.</param>
    /// <param name="top">- The top edge position of the layout box.</param>
    /// <param name="width">- The width of the layout box.</param>
    /// <param name="height">- The height of the layout box.</param>
    abstract update: left: float * top: float * width: float * height: float -> unit
    abstract _top: obj with get, set
    abstract _left: obj with get, set
    abstract _width: obj with get, set
    abstract _height: obj with get, set
    abstract _minWidth: obj with get, set
    abstract _minHeight: obj with get, set
    abstract _maxWidth: obj with get, set
    abstract _maxHeight: obj with get, set
    abstract _disposed: obj with get, set

/// An object which assists in the absolute layout of widgets.
/// 
/// #### Notes
/// This class is useful when implementing a layout which arranges its
/// widgets using absolute positioning.
/// 
/// This class is used by nearly all of the built-in Phosphor layouts.
type [<AllowNullLiteral>] LayoutItemStatic =
    /// <summary>Construct a new layout item.</summary>
    /// <param name="widget">- The widget to be managed by the item.
    /// 
    /// #### Notes
    /// The widget will be set to absolute positioning.</param>
    [<Emit "new $0($1...)">] abstract Create: widget: Widget -> LayoutItem
// type IIterator = PhosphorAlgorithm.IIterator
// type Layout = __layout.Layout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract PanelLayout: PanelLayoutStatic

/// A concrete layout implementation suitable for many use cases.
/// 
/// #### Notes
/// This class is suitable as a base class for implementing a variety of
/// layouts, but can also be used directly with standard CSS to layout a
/// collection of widgets.
type [<AllowNullLiteral>] PanelLayout =
    inherit Layout
    /// Dispose of the resources held by the layout.
    /// 
    /// #### Notes
    /// This will clear and dispose all widgets in the layout.
    /// 
    /// All reimplementations should call the superclass method.
    /// 
    /// This method is called automatically when the parent is disposed.
    abstract dispose: unit -> unit
    /// A read-only array of the widgets in the layout.
    abstract widgets: ReadonlyArray<Widget>
    /// Create an iterator over the widgets in the layout.
    abstract iter: unit -> IIterator<Widget>
    /// <summary>Add a widget to the end of the layout.</summary>
    /// <param name="widget">- The widget to add to the layout.
    /// 
    /// #### Notes
    /// If the widget is already contained in the layout, it will be moved.</param>
    abstract addWidget: widget: Widget -> unit
    /// <summary>Insert a widget into the layout at the specified index.</summary>
    /// <param name="index">- The index at which to insert the widget.</param>
    /// <param name="widget">- The widget to insert into the layout.
    /// 
    /// #### Notes
    /// The index will be clamped to the bounds of the widgets.
    /// 
    /// If the widget is already added to the layout, it will be moved.
    /// 
    /// #### Undefined Behavior
    /// An `index` which is non-integral.</param>
    abstract insertWidget: index: float * widget: Widget -> unit
    /// <summary>Remove a widget from the layout.</summary>
    /// <param name="widget">- The widget to remove from the layout.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method does *not* modify the widget's `parent`.</param>
    abstract removeWidget: widget: Widget -> unit
    /// <summary>Remove the widget at a given index from the layout.</summary>
    /// <param name="index">- The index of the widget to remove.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method does *not* modify the widget's `parent`.
    /// 
    /// #### Undefined Behavior
    /// An `index` which is non-integral.</param>
    abstract removeWidgetAt: index: float -> unit
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="index">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This method is called automatically by the panel layout at the
    /// appropriate time. It should not be called directly by user code.
    /// 
    /// The default implementation adds the widgets's node to the parent's
    /// node at the proper location, and sends the appropriate attach
    /// messages to the widget if the parent is attached to the DOM.
    /// 
    /// Subclasses may reimplement this method to control how the widget's
    /// node is added to the parent's node.</param>
    abstract attachWidget: index: float * widget: Widget -> unit
    /// <summary>Move a widget in the parent's DOM node.</summary>
    /// <param name="fromIndex">- The previous index of the widget in the layout.</param>
    /// <param name="toIndex">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to move in the parent.
    /// 
    /// #### Notes
    /// This method is called automatically by the panel layout at the
    /// appropriate time. It should not be called directly by user code.
    /// 
    /// The default implementation moves the widget's node to the proper
    /// location in the parent's node and sends the appropriate attach and
    /// detach messages to the widget if the parent is attached to the DOM.
    /// 
    /// Subclasses may reimplement this method to control how the widget's
    /// node is moved in the parent's node.</param>
    abstract moveWidget: fromIndex: float * toIndex: float * widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="index">- The previous index of the widget in the layout.</param>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This method is called automatically by the panel layout at the
    /// appropriate time. It should not be called directly by user code.
    /// 
    /// The default implementation removes the widget's node from the
    /// parent's node, and sends the appropriate detach messages to the
    /// widget if the parent is attached to the DOM.
    /// 
    /// Subclasses may reimplement this method to control how the widget's
    /// node is removed from the parent's node.</param>
    abstract detachWidget: index: float * widget: Widget -> unit
    abstract _widgets: obj with get, set

/// A concrete layout implementation suitable for many use cases.
/// 
/// #### Notes
/// This class is suitable as a base class for implementing a variety of
/// layouts, but can also be used directly with standard CSS to layout a
/// collection of widgets.
type [<AllowNullLiteral>] PanelLayoutStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> PanelLayout
// type Message = PhosphorMessaging.Message
// type PanelLayout = __panellayout.PanelLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract BoxLayout: BoxLayoutStatic

/// A layout which arranges its widgets in a single row or column.
/// The namespace for the `BoxLayout` class statics.
type [<AllowNullLiteral>] BoxLayout =
    inherit PanelLayout
    /// Dispose of the resources held by the layout.
    abstract dispose: unit -> unit
    /// Get the layout direction for the box layout.
    /// Set the layout direction for the box layout.
    abstract direction: BoxLayout.Direction with get, set
    /// Get the content alignment for the box layout.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire box layout.
    /// Set the content alignment for the box layout.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire box layout.
    abstract alignment: BoxLayout.Alignment with get, set
    /// Get the inter-element spacing for the box layout.
    /// Set the inter-element spacing for the box layout.
    abstract spacing: float with get, set
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="index">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract attachWidget: index: float * widget: Widget -> unit
    /// <summary>Move a widget in the parent's DOM node.</summary>
    /// <param name="fromIndex">- The previous index of the widget in the layout.</param>
    /// <param name="toIndex">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to move in the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract moveWidget: fromIndex: float * toIndex: float * widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="index">- The previous index of the widget in the layout.</param>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract detachWidget: index: float * widget: Widget -> unit
    /// A message handler invoked on a `'before-show'` message.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'resize'` message.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    abstract onFitRequest: msg: Message -> unit
    /// Fit the layout to the total size required by the widgets.
    abstract _fit: obj with get, set
    /// Update the layout position and size of the widgets.
    /// 
    /// The parent offset dimensions should be `-1` if unknown.
    abstract _update: obj with get, set
    abstract _fixed: obj with get, set
    abstract _spacing: obj with get, set
    abstract _dirty: obj with get, set
    abstract _sizers: obj with get, set
    abstract _items: obj with get, set
    abstract _box: obj with get, set
    abstract _alignment: obj with get, set
    abstract _direction: obj with get, set

/// A layout which arranges its widgets in a single row or column.
/// The namespace for the `BoxLayout` class statics.
type [<AllowNullLiteral>] BoxLayoutStatic =
    /// <summary>Construct a new box layout.</summary>
    /// <param name="options">- The options for initializing the layout.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: BoxLayout.IOptions -> BoxLayout

module BoxLayout =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Get the box layout stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getStretch: widget: Widget -> float
        /// <summary>Set the box layout stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the stretch factor.</param>
        abstract setStretch: widget: Widget * value: float -> unit
        /// <summary>Get the box layout size basis for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getSizeBasis: widget: Widget -> float
        /// <summary>Set the box layout size basis for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the size basis.</param>
        abstract setSizeBasis: widget: Widget * value: float -> unit

    type [<StringEnum>] [<RequireQualifiedAccess>] Direction =
        | [<CompiledName "left-to-right">] LeftToRight
        | [<CompiledName "right-to-left">] RightToLeft
        | [<CompiledName "top-to-bottom">] TopToBottom
        | [<CompiledName "bottom-to-top">] BottomToTop

    type [<StringEnum>] [<RequireQualifiedAccess>] Alignment =
        | Start
        | Center
        | End
        | Justify

    /// An options object for initializing a box layout.
    type [<AllowNullLiteral>] IOptions =
        /// The direction of the layout.
        /// 
        /// The default is `'top-to-bottom'`.
        abstract direction: Direction option with get, set
        /// The content alignment of the layout.
        /// 
        /// The default is `'start'`.
        abstract alignment: Alignment option with get, set
        /// The spacing between items in the layout.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set
// type PanelLayout = __panellayout.PanelLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract Panel: PanelStatic

/// A simple and convenient panel widget class.
/// 
/// #### Notes
/// This class is suitable as a base class for implementing a variety of
/// convenience panel widgets, but can also be used directly with CSS to
/// arrange a collection of widgets.
/// 
/// This class provides a convenience wrapper around a [[PanelLayout]].
/// The namespace for the `Panel` class statics.
type [<AllowNullLiteral>] Panel =
    inherit Widget
    /// A read-only array of the widgets in the panel.
    abstract widgets: ReadonlyArray<Widget>
    /// <summary>Add a widget to the end of the panel.</summary>
    /// <param name="widget">- The widget to add to the panel.
    /// 
    /// #### Notes
    /// If the widget is already contained in the panel, it will be moved.</param>
    abstract addWidget: widget: Widget -> unit
    /// <summary>Insert a widget at the specified index.</summary>
    /// <param name="index">- The index at which to insert the widget.</param>
    /// <param name="widget">- The widget to insert into to the panel.
    /// 
    /// #### Notes
    /// If the widget is already contained in the panel, it will be moved.</param>
    abstract insertWidget: index: float * widget: Widget -> unit

/// A simple and convenient panel widget class.
/// 
/// #### Notes
/// This class is suitable as a base class for implementing a variety of
/// convenience panel widgets, but can also be used directly with CSS to
/// arrange a collection of widgets.
/// 
/// This class provides a convenience wrapper around a [[PanelLayout]].
/// The namespace for the `Panel` class statics.
type [<AllowNullLiteral>] PanelStatic =
    /// <summary>Construct a new panel.</summary>
    /// <param name="options">- The options for initializing the panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: Panel.IOptions -> Panel

module Panel =

    /// An options object for creating a panel.
    type [<AllowNullLiteral>] IOptions =
        /// The panel layout to use for the panel.
        /// 
        /// The default is a new `PanelLayout`.
        abstract layout: PanelLayout option with get, set
// type BoxLayout = __boxlayout.BoxLayout
// type Panel = __panel.Panel
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract BoxPanel: BoxPanelStatic

/// A panel which arranges its widgets in a single row or column.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[BoxLayout]].
/// The namespace for the `BoxPanel` class statics.
type [<AllowNullLiteral>] BoxPanel =
    inherit Panel
    /// Get the layout direction for the box panel.
    /// Set the layout direction for the box panel.
    abstract direction: BoxPanel.Direction with get, set
    /// Get the content alignment for the box panel.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire box layout.
    /// Set the content alignment for the box panel.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire box layout.
    abstract alignment: BoxPanel.Alignment with get, set
    /// Get the inter-element spacing for the box panel.
    /// Set the inter-element spacing for the box panel.
    abstract spacing: float with get, set
    /// A message handler invoked on a `'child-added'` message.
    abstract onChildAdded: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-removed'` message.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit

/// A panel which arranges its widgets in a single row or column.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[BoxLayout]].
/// The namespace for the `BoxPanel` class statics.
type [<AllowNullLiteral>] BoxPanelStatic =
    /// <summary>Construct a new box panel.</summary>
    /// <param name="options">- The options for initializing the box panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: BoxPanel.IOptions -> BoxPanel

module BoxPanel =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Get the box panel stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getStretch: widget: Widget -> float
        /// <summary>Set the box panel stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the stretch factor.</param>
        abstract setStretch: widget: Widget * value: float -> unit
        /// <summary>Get the box panel size basis for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getSizeBasis: widget: Widget -> float
        /// <summary>Set the box panel size basis for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the size basis.</param>
        abstract setSizeBasis: widget: Widget * value: float -> unit

    type Direction =
        BoxLayout.Direction

    type Alignment =
        BoxLayout.Alignment

    /// An options object for initializing a box panel.
    type [<AllowNullLiteral>] IOptions =
        /// The layout direction of the panel.
        /// 
        /// The default is `'top-to-bottom'`.
        abstract direction: Direction option with get, set
        /// The content alignment of the panel.
        /// 
        /// The default is `'start'`.
        abstract alignment: Alignment option with get, set
        /// The spacing between items in the panel.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set
        /// The box layout to use for the box panel.
        /// 
        /// If this is provided, the other options are ignored.
        /// 
        /// The default is a new `BoxLayout`.
        abstract layout: BoxLayout option with get, set
type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject
type CommandRegistry = PhosphorCommands.CommandRegistry
// type Message = PhosphorMessaging.Message
type ElementDataset = PhosphorVirtualdom.ElementDataset //``@phosphor_virtualdom``.ElementDataset
type VirtualElement = PhosphorVirtualdom.VirtualElement //``@phosphor_virtualdom``.VirtualElement
// type h = PhosphorVirtualdom.H // ``@phosphor_virtualdom``.h
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract CommandPalette: CommandPaletteStatic

/// A widget which displays command items as a searchable palette.
/// The namespace for the `CommandPalette` class statics.
type [<AllowNullLiteral>] CommandPalette =
    inherit Widget
    /// Dispose of the resources held by the widget.
    abstract dispose: unit -> unit
    /// The command registry used by the command palette.
    abstract commands: CommandRegistry
    /// The renderer used by the command palette.
    abstract renderer: CommandPalette.IRenderer
    /// The command palette search node.
    /// 
    /// #### Notes
    /// This is the node which contains the search-related elements.
    abstract searchNode: HTMLDivElement
    /// The command palette input node.
    /// 
    /// #### Notes
    /// This is the actual input node for the search area.
    abstract inputNode: HTMLInputElement
    /// The command palette content node.
    /// 
    /// #### Notes
    /// This is the node which holds the command item nodes.
    /// 
    /// Modifying this node directly can lead to undefined behavior.
    abstract contentNode: HTMLUListElement
    /// A read-only array of the command items in the palette.
    abstract items: ReadonlyArray<CommandPalette.IItem>
    /// <summary>Add a command item to the command palette.</summary>
    /// <param name="options">- The options for creating the command item.</param>
    abstract addItem: options: CommandPalette.IItemOptions -> CommandPalette.IItem
    /// <summary>Remove an item from the command palette.</summary>
    /// <param name="item">- The item to remove from the palette.
    /// 
    /// #### Notes
    /// This is a no-op if the item is not in the palette.</param>
    abstract removeItem: item: CommandPalette.IItem -> unit
    /// <summary>Remove the item at a given index from the command palette.</summary>
    /// <param name="index">- The index of the item to remove.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract removeItemAt: index: float -> unit
    /// Remove all items from the command palette.
    abstract clearItems: unit -> unit
    /// Clear the search results and schedule an update.
    /// 
    /// #### Notes
    /// This should be called whenever the search results of the palette
    /// should be updated.
    /// 
    /// This is typically called automatically by the palette as needed,
    /// but can be called manually if the input text is programatically
    /// changed.
    /// 
    /// The rendered results are updated asynchronously.
    abstract refresh: unit -> unit
    /// <summary>Handle the DOM events for the command palette.</summary>
    /// <param name="event">- The DOM event sent to the command palette.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the command palette's DOM node.
    /// It should not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on an `'activate-request'` message.
    abstract onActivateRequest: msg: Message -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// Handle the `'click'` event for the command palette.
    abstract _evtClick: obj with get, set
    /// Handle the `'keydown'` event for the command palette.
    abstract _evtKeyDown: obj with get, set
    /// Activate the next enabled command item.
    abstract _activateNextItem: obj with get, set
    /// Activate the previous enabled command item.
    abstract _activatePreviousItem: obj with get, set
    /// Execute the command item at the given index, if possible.
    abstract _execute: obj with get, set
    /// Toggle the focused modifier based on the input node focus state.
    abstract _toggleFocused: obj with get, set
    /// A signal handler for generic command changes.
    abstract _onGenericChange: obj with get, set
    abstract _activeIndex: obj with get, set
    abstract _items: obj with get, set
    abstract _results: obj with get, set

/// A widget which displays command items as a searchable palette.
/// The namespace for the `CommandPalette` class statics.
type [<AllowNullLiteral>] CommandPaletteStatic =
    /// <summary>Construct a new command palette.</summary>
    /// <param name="options">- The options for initializing the palette.</param>
    [<Emit "new $0($1...)">] abstract Create: options: CommandPalette.IOptions -> CommandPalette

module CommandPalette =

    type [<AllowNullLiteral>] IExports =
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer

    /// An options object for creating a command palette.
    type [<AllowNullLiteral>] IOptions =
        /// The command registry for use with the command palette.
        abstract commands: CommandRegistry with get, set
        /// A custom renderer for use with the command palette.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer option with get, set

    /// An options object for creating a command item.
    type [<AllowNullLiteral>] IItemOptions =
        /// The category for the item.
        abstract category: string with get, set
        /// The command to execute when the item is triggered.
        abstract command: string with get, set
        /// The arguments for the command.
        /// 
        /// The default value is an empty object.
        abstract args: ReadonlyJSONObject option with get, set
        /// The rank for the command item.
        /// 
        /// The rank is used as a tie-breaker when ordering command items
        /// for display. Items are sorted in the following order:
        ///    1. Text match (lower is better)
        ///    2. Category (locale order)
        ///    3. Rank (lower is better)
        ///    4. Label (locale order)
        /// 
        /// The default rank is `Infinity`.
        abstract rank: float option with get, set

    /// An object which represents an item in a command palette.
    /// 
    /// #### Notes
    /// Item objects are created automatically by a command palette.
    type [<AllowNullLiteral>] IItem =
        /// The command to execute when the item is triggered.
        abstract command: string
        /// The arguments for the command.
        abstract args: ReadonlyJSONObject
        /// The category for the command item.
        abstract category: string
        /// The rank for the command item.
        abstract rank: float
        /// The display label for the command item.
        abstract label: string
        /// The display caption for the command item.
        abstract caption: string
        /// The icon class for the command item.
        abstract iconClass: string
        /// The icon label for the command item.
        abstract iconLabel: string
        /// The extra class name for the command item.
        abstract className: string
        /// The dataset for the command item.
        abstract dataset: PhosphorCommands.CommandRegistry.Dataset
        /// Whether the command item is enabled.
        abstract isEnabled: bool
        /// Whether the command item is toggled.
        abstract isToggled: bool
        /// Whether the command item is visible.
        abstract isVisible: bool
        /// The key binding for the command item.
        abstract keyBinding: PhosphorCommands.CommandRegistry.IKeyBinding option

    /// The render data for a command palette header.
    type [<AllowNullLiteral>] IHeaderRenderData =
        /// The category of the header.
        abstract category: string
        /// The indices of the matched characters in the category.
        abstract indices: ReadonlyArray<float> option

    /// The render data for a command palette item.
    type [<AllowNullLiteral>] IItemRenderData =
        /// The command palette item to render.
        abstract item: IItem
        /// The indices of the matched characters in the label.
        abstract indices: ReadonlyArray<float> option
        /// Whether the item is the active item.
        abstract active: bool

    /// The render data for a command palette empty message.
    type [<AllowNullLiteral>] IEmptyMessageRenderData =
        /// The query which failed to match any commands.
        abstract query: string with get, set

    /// A renderer for use with a command palette.
    type [<AllowNullLiteral>] IRenderer =
        /// <summary>Render the virtual element for a command palette header.</summary>
        /// <param name="data">- The data to use for rendering the header.</param>
        abstract renderHeader: data: IHeaderRenderData -> VirtualElement
        /// <summary>Render the virtual element for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IItemRenderData -> VirtualElement
        /// <summary>Render the empty results message for a command palette.</summary>
        /// <param name="data">- The data to use for rendering the message.</param>
        abstract renderEmptyMessage: data: IEmptyMessageRenderData -> VirtualElement

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer
        /// <summary>Render the virtual element for a command palette header.</summary>
        /// <param name="data">- The data to use for rendering the header.</param>
        abstract renderHeader: data: IHeaderRenderData -> VirtualElement
        /// <summary>Render the virtual element for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IItemRenderData -> VirtualElement
        /// <summary>Render the empty results message for a command palette.</summary>
        /// <param name="data">- The data to use for rendering the message.</param>
        abstract renderEmptyMessage: data: IEmptyMessageRenderData -> VirtualElement
        /// <summary>Render the icon for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the icon.</param>
        abstract renderItemIcon: data: IItemRenderData -> VirtualElement
        /// <summary>Render the content for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the content.</param>
        abstract renderItemContent: data: IItemRenderData -> VirtualElement
        /// <summary>Render the label for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the label.</param>
        abstract renderItemLabel: data: IItemRenderData -> VirtualElement
        /// <summary>Render the caption for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the caption.</param>
        abstract renderItemCaption: data: IItemRenderData -> VirtualElement
        /// <summary>Render the shortcut for a command palette item.</summary>
        /// <param name="data">- The data to use for rendering the shortcut.</param>
        abstract renderItemShortcut: data: IItemRenderData -> VirtualElement
        /// <summary>Create the class name for the command palette item.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createItemClass: data: IItemRenderData -> string
        /// <summary>Create the dataset for the command palette item.</summary>
        /// <param name="data">- The data to use for creating the dataset.</param>
        abstract createItemDataset: data: IItemRenderData -> ElementDataset
        /// <summary>Create the class name for the command item icon.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createIconClass: data: IItemRenderData -> string
        /// <summary>Create the render content for the header node.</summary>
        /// <param name="data">- The data to use for the header content.</param>
        abstract formatHeader: data: IHeaderRenderData -> PhosphorVirtualdom.H.Child
        /// <summary>Create the render content for the empty message node.</summary>
        /// <param name="data">- The data to use for the empty message content.</param>
        abstract formatEmptyMessage: data: IEmptyMessageRenderData -> PhosphorVirtualdom.H.Child
        /// <summary>Create the render content for the item shortcut node.</summary>
        /// <param name="data">- The data to use for the shortcut content.</param>
        abstract formatItemShortcut: data: IItemRenderData -> PhosphorVirtualdom.H.Child
        /// <summary>Create the render content for the item label node.</summary>
        /// <param name="data">- The data to use for the label content.</param>
        abstract formatItemLabel: data: IItemRenderData -> PhosphorVirtualdom.H.Child
        /// <summary>Create the render content for the item caption node.</summary>
        /// <param name="data">- The data to use for the caption content.</param>
        abstract formatItemCaption: data: IItemRenderData -> PhosphorVirtualdom.H.Child

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] RendererStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type CommandRegistry = PhosphorCommands.CommandRegistry
// type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject
// type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type ElementDataset = ``@phosphor_virtualdom``.ElementDataset
// type VirtualElement = ``@phosphor_virtualdom``.VirtualElement
// type h = ``@phosphor_virtualdom``.h
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract Menu: MenuStatic

/// A widget which displays items as a canonical menu.
/// The namespace for the `Menu` class statics.
type [<AllowNullLiteral>] Menu =
    inherit Widget
    /// Dispose of the resources held by the menu.
    abstract dispose: unit -> unit
    /// A signal emitted just before the menu is closed.
    /// 
    /// #### Notes
    /// This signal is emitted when the menu receives a `'close-request'`
    /// message, just before it removes itself from the DOM.
    /// 
    /// This signal is not emitted if the menu is already detached from
    /// the DOM when it receives the `'close-request'` message.
    abstract aboutToClose: ISignal<Menu, unit>
    /// A signal emitted when a new menu is requested by the user.
    /// 
    /// #### Notes
    /// This signal is emitted whenever the user presses the right or left
    /// arrow keys, and a submenu cannot be opened or closed in response.
    /// 
    /// This signal is useful when implementing menu bars in order to open
    /// the next or previous menu in response to a user key press.
    /// 
    /// This signal is only emitted for the root menu in a hierarchy.
    abstract menuRequested: ISignal<Menu, U2<string, string>>
    /// The command registry used by the menu.
    abstract commands: CommandRegistry
    /// The renderer used by the menu.
    abstract renderer: Menu.IRenderer
    /// The parent menu of the menu.
    /// 
    /// #### Notes
    /// This is `null` unless the menu is an open submenu.
    abstract parentMenu: Menu option
    /// The child menu of the menu.
    /// 
    /// #### Notes
    /// This is `null` unless the menu has an open submenu.
    abstract childMenu: Menu option
    /// The root menu of the menu hierarchy.
    abstract rootMenu: Menu
    /// The leaf menu of the menu hierarchy.
    abstract leafMenu: Menu
    /// The menu content node.
    /// 
    /// #### Notes
    /// This is the node which holds the menu item nodes.
    /// 
    /// Modifying this node directly can lead to undefined behavior.
    abstract contentNode: HTMLUListElement
    /// Get the currently active menu item.
    /// Set the currently active menu item.
    /// 
    /// #### Notes
    /// If the item cannot be activated, the item will be set to `null`.
    abstract activeItem: Menu.IItem option with get, set
    /// Get the index of the currently active menu item.
    /// 
    /// #### Notes
    /// This will be `-1` if no menu item is active.
    /// Set the index of the currently active menu item.
    /// 
    /// #### Notes
    /// If the item cannot be activated, the index will be set to `-1`.
    abstract activeIndex: float with get, set
    /// A read-only array of the menu items in the menu.
    abstract items: ReadonlyArray<Menu.IItem>
    /// Activate the next selectable item in the menu.
    /// 
    /// #### Notes
    /// If no item is selectable, the index will be set to `-1`.
    abstract activateNextItem: unit -> unit
    /// Activate the previous selectable item in the menu.
    /// 
    /// #### Notes
    /// If no item is selectable, the index will be set to `-1`.
    abstract activatePreviousItem: unit -> unit
    /// Trigger the active menu item.
    /// 
    /// #### Notes
    /// If the active item is a submenu, it will be opened and the first
    /// item will be activated.
    /// 
    /// If the active item is a command, the command will be executed.
    /// 
    /// If the menu is not attached, this is a no-op.
    /// 
    /// If there is no active item, this is a no-op.
    abstract triggerActiveItem: unit -> unit
    /// <summary>Add a menu item to the end of the menu.</summary>
    /// <param name="options">- The options for creating the menu item.</param>
    abstract addItem: options: Menu.IItemOptions -> Menu.IItem
    /// <summary>Insert a menu item into the menu at the specified index.</summary>
    /// <param name="index">- The index at which to insert the item.</param>
    /// <param name="options">- The options for creating the menu item.</param>
    abstract insertItem: index: float * options: Menu.IItemOptions -> Menu.IItem
    /// <summary>Remove an item from the menu.</summary>
    /// <param name="item">- The item to remove from the menu.
    /// 
    /// #### Notes
    /// This is a no-op if the item is not in the menu.</param>
    abstract removeItem: item: Menu.IItem -> unit
    /// <summary>Remove the item at a given index from the menu.</summary>
    /// <param name="index">- The index of the item to remove.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract removeItemAt: index: float -> unit
    /// Remove all menu items from the menu.
    abstract clearItems: unit -> unit
    /// <summary>Open the menu at the specified location.</summary>
    /// <param name="x">- The client X coordinate of the menu location.</param>
    /// <param name="y">- The client Y coordinate of the menu location.</param>
    /// <param name="options">- The additional options for opening the menu.
    /// 
    /// #### Notes
    /// The menu will be opened at the given location unless it will not
    /// fully fit on the screen. If it will not fit, it will be adjusted
    /// to fit naturally on the screen.
    /// 
    /// This is a no-op if the menu is already attached to the DOM.</param>
    abstract ``open``: x: float * y: float * ?options: Menu.IOpenOptions -> unit
    /// <summary>Handle the DOM events for the menu.</summary>
    /// <param name="event">- The DOM event sent to the menu.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the menu's DOM nodes. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on an `'activate-request'` message.
    abstract onActivateRequest: msg: Message -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'close-request'` message.
    abstract onCloseRequest: msg: Message -> unit
    /// Handle the `'keydown'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the menu node.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mouseup'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the menu node.
    abstract _evtMouseUp: obj with get, set
    /// Handle the `'mousemove'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the menu node.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseenter'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the menu node.
    abstract _evtMouseEnter: obj with get, set
    /// Handle the `'mouseleave'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the menu node.
    abstract _evtMouseLeave: obj with get, set
    /// Handle the `'mousedown'` event for the menu.
    /// 
    /// #### Notes
    /// This listener is attached to the document node.
    abstract _evtMouseDown: obj with get, set
    /// Open the child menu at the active index immediately.
    /// 
    /// If a different child menu is already open, it will be closed,
    /// even if the active item is not a valid submenu.
    abstract _openChildMenu: obj with get, set
    /// Close the child menu immediately.
    /// 
    /// This is a no-op if a child menu is not open.
    abstract _closeChildMenu: obj with get, set
    /// Start the open timer, unless it is already pending.
    abstract _startOpenTimer: obj with get, set
    /// Start the close timer, unless it is already pending.
    abstract _startCloseTimer: obj with get, set
    /// Cancel the open timer, if the timer is pending.
    abstract _cancelOpenTimer: obj with get, set
    /// Cancel the close timer, if the timer is pending.
    abstract _cancelCloseTimer: obj with get, set
    abstract _childIndex: obj with get, set
    abstract _activeIndex: obj with get, set
    abstract _openTimerID: obj with get, set
    abstract _closeTimerID: obj with get, set
    abstract _items: obj with get, set
    abstract _childMenu: obj with get, set
    abstract _parentMenu: obj with get, set
    abstract _aboutToClose: obj with get, set
    abstract _menuRequested: obj with get, set

/// A widget which displays items as a canonical menu.
/// The namespace for the `Menu` class statics.
type [<AllowNullLiteral>] MenuStatic =
    /// <summary>Construct a new menu.</summary>
    /// <param name="options">- The options for initializing the menu.</param>
    [<Emit "new $0($1...)">] abstract Create: options: Menu.IOptions -> Menu

module Menu =

    type [<AllowNullLiteral>] IExports =
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer

    /// An options object for creating a menu.
    type [<AllowNullLiteral>] IOptions =
        /// The command registry for use with the menu.
        abstract commands: CommandRegistry with get, set
        /// A custom renderer for use with the menu.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer option with get, set

    /// An options object for the `open` method on a menu.
    type [<AllowNullLiteral>] IOpenOptions =
        /// Whether to force the X position of the menu.
        /// 
        /// Setting to `true` will disable the logic which repositions the
        /// X coordinate of the menu if it will not fit entirely on screen.
        /// 
        /// The default is `false`.
        abstract forceX: bool option with get, set
        /// Whether to force the Y position of the menu.
        /// 
        /// Setting to `true` will disable the logic which repositions the
        /// Y coordinate of the menu if it will not fit entirely on screen.
        /// 
        /// The default is `false`.
        abstract forceY: bool option with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] ItemType =
        | Command
        | Submenu
        | Separator

    /// An options object for creating a menu item.
    type [<AllowNullLiteral>] IItemOptions =
        /// The type of the menu item.
        /// 
        /// The default value is `'command'`.
        abstract ``type``: ItemType option with get, set
        /// The command to execute when the item is triggered.
        /// 
        /// The default value is an empty string.
        abstract command: string option with get, set
        /// The arguments for the command.
        /// 
        /// The default value is an empty object.
        abstract args: ReadonlyJSONObject option with get, set
        /// The submenu for a `'submenu'` type item.
        /// 
        /// The default value is `null`.
        abstract submenu: Menu option with get, set

    /// An object which represents a menu item.
    /// 
    /// #### Notes
    /// Item objects are created automatically by a menu.
    type [<AllowNullLiteral>] IItem =
        /// The type of the menu item.
        abstract ``type``: ItemType
        /// The command to execute when the item is triggered.
        abstract command: string
        /// The arguments for the command.
        abstract args: ReadonlyJSONObject
        /// The submenu for a `'submenu'` type item.
        abstract submenu: Menu option
        /// The display label for the menu item.
        abstract label: string
        /// The mnemonic index for the menu item.
        abstract mnemonic: float
        abstract icon: string
        /// The icon class for the menu item.
        abstract iconClass: string
        /// The icon label for the menu item.
        abstract iconLabel: string
        /// The display caption for the menu item.
        abstract caption: string
        /// The extra class name for the menu item.
        abstract className: string
        /// The dataset for the menu item.
        abstract dataset: PhosphorCommands.CommandRegistry.Dataset
        /// Whether the menu item is enabled.
        abstract isEnabled: bool
        /// Whether the menu item is toggled.
        abstract isToggled: bool
        /// Whether the menu item is visible.
        abstract isVisible: bool
        /// The key binding for the menu item.
        abstract keyBinding: PhosphorCommands.CommandRegistry.IKeyBinding option

    /// An object which holds the data to render a menu item.
    type [<AllowNullLiteral>] IRenderData =
        /// The item to be rendered.
        abstract item: IItem
        /// Whether the item is the active item.
        abstract active: bool
        /// Whether the item should be collapsed.
        abstract collapsed: bool

    /// A renderer for use with a menu.
    type [<AllowNullLiteral>] IRenderer =
        /// <summary>Render the virtual element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IRenderData -> VirtualElement

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer
        /// <summary>Render the virtual element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IRenderData -> VirtualElement
        /// <summary>Render the icon element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the icon.</param>
        abstract renderIcon: data: IRenderData -> VirtualElement
        /// <summary>Render the label element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the label.</param>
        abstract renderLabel: data: IRenderData -> VirtualElement
        /// <summary>Render the shortcut element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the shortcut.</param>
        abstract renderShortcut: data: IRenderData -> VirtualElement
        /// <summary>Render the submenu icon element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the submenu icon.</param>
        abstract renderSubmenu: data: IRenderData -> VirtualElement
        /// <summary>Create the class name for the menu item.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createItemClass: data: IRenderData -> string
        /// <summary>Create the dataset for the menu item.</summary>
        /// <param name="data">- The data to use for creating the dataset.</param>
        abstract createItemDataset: data: IRenderData -> ElementDataset
        /// <summary>Create the class name for the menu item icon.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createIconClass: data: IRenderData -> string
        /// <summary>Create the render content for the label node.</summary>
        /// <param name="data">- The data to use for the label content.</param>
        abstract formatLabel: data: IRenderData -> PhosphorVirtualdom.H.Child
        /// <summary>Create the render content for the shortcut node.</summary>
        /// <param name="data">- The data to use for the shortcut content.</param>
        abstract formatShortcut: data: IRenderData -> PhosphorVirtualdom.H.Child

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] RendererStatic =
        /// Construct a new renderer.
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type CommandRegistry = PhosphorCommands.CommandRegistry
// type IDisposable = PhosphorDisposable.IDisposable
// type Menu = __menu.Menu

// type [<AllowNullLiteral>] IExports =
//     abstract ContextMenu: ContextMenuStatic

/// An object which implements a universal context menu.
/// 
/// #### Notes
/// The items shown in the context menu are determined by CSS selector
/// matching against the DOM hierarchy at the site of the mouse click.
/// This is similar in concept to how keyboard shortcuts are matched
/// in the command registry.
/// The namespace for the `ContextMenu` class statics.
type [<AllowNullLiteral>] ContextMenu =
    /// The menu widget which displays the matched context items.
    abstract menu: Menu
    /// <summary>Add an item to the context menu.</summary>
    /// <param name="options">- The options for creating the item.</param>
    abstract addItem: options: ContextMenu.IItemOptions -> IDisposable
    /// <summary>Open the context menu in response to a `'contextmenu'` event.</summary>
    /// <param name="event">- The `'contextmenu'` event of interest.</param>
    abstract ``open``: ``event``: MouseEvent -> bool
    abstract _idTick: obj with get, set
    abstract _items: obj with get, set

/// An object which implements a universal context menu.
/// 
/// #### Notes
/// The items shown in the context menu are determined by CSS selector
/// matching against the DOM hierarchy at the site of the mouse click.
/// This is similar in concept to how keyboard shortcuts are matched
/// in the command registry.
/// The namespace for the `ContextMenu` class statics.
type [<AllowNullLiteral>] ContextMenuStatic =
    /// <summary>Construct a new context menu.</summary>
    /// <param name="options">- The options for initializing the menu.</param>
    [<Emit "new $0($1...)">] abstract Create: options: ContextMenu.IOptions -> ContextMenu

module ContextMenu =

    /// An options object for initializing a context menu.
    type [<AllowNullLiteral>] IOptions =
        /// The command registry to use with the context menu.
        abstract commands: CommandRegistry with get, set
        /// A custom renderer for use with the context menu.
        abstract renderer: Menu.IRenderer option with get, set

    /// An options object for creating a context menu item.
    type [<AllowNullLiteral>] IItemOptions =
        inherit Menu.IItemOptions
        /// The CSS selector for the context menu item.
        /// 
        /// The context menu item will only be displayed in the context menu
        /// when the selector matches a node on the propagation path of the
        /// contextmenu event. This allows the menu item to be restricted to
        /// user-defined contexts.
        /// 
        /// The selector must not contain commas.
        abstract selector: string with get, set
        /// The rank for the item.
        /// 
        /// The rank is used as a tie-breaker when ordering context menu
        /// items for display. Items are sorted in the following order:
        ///    1. Depth in the DOM tree (deeper is better)
        ///    2. Selector specificity (higher is better)
        ///    3. Rank (lower is better)
        ///    4. Insertion order
        /// 
        /// The default rank is `Infinity`.
        abstract rank: float option with get, set
// type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type ElementDataset = ``@phosphor_virtualdom``.ElementDataset
type ElementInlineStyle = PhosphorVirtualdom.ElementInlineStyle //``@phosphor_virtualdom``.ElementInlineStyle
// type VirtualElement = ``@phosphor_virtualdom``.VirtualElement
// type Title = __title.Title
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract TabBar: TabBarStatic

/// A widget which displays titles as a single row or column of tabs.
/// 
/// #### Notes
/// If CSS transforms are used to rotate nodes for vertically oriented
/// text, then tab dragging will not work correctly. The `tabsMovable`
/// property should be set to `false` when rotating nodes from CSS.
/// The namespace for the `TabBar` class statics.
type [<AllowNullLiteral>] TabBar<'T> =
    inherit Widget
    /// Dispose of the resources held by the widget.
    abstract dispose: unit -> unit
    /// A signal emitted when the current tab is changed.
    /// 
    /// #### Notes
    /// This signal is emitted when the currently selected tab is changed
    /// either through user or programmatic interaction.
    /// 
    /// Notably, this signal is not emitted when the index of the current
    /// tab changes due to tabs being inserted, removed, or moved. It is
    /// only emitted when the actual current tab node is changed.
    abstract currentChanged: ISignal<TabBar<'T>, TabBar.ICurrentChangedArgs<'T>>
    /// A signal emitted when a tab is moved by the user.
    /// 
    /// #### Notes
    /// This signal is emitted when a tab is moved by user interaction.
    /// 
    /// This signal is not emitted when a tab is moved programmatically.
    abstract tabMoved: ISignal<TabBar<'T>, TabBar.ITabMovedArgs<'T>>
    /// A signal emitted when a tab is clicked by the user.
    /// 
    /// #### Notes
    /// If the clicked tab is not the current tab, the clicked tab will be
    /// made current and the `currentChanged` signal will be emitted first.
    /// 
    /// This signal is emitted even if the clicked tab is the current tab.
    abstract tabActivateRequested: ISignal<TabBar<'T>, TabBar.ITabActivateRequestedArgs<'T>>
    /// A signal emitted when a tab close icon is clicked.
    /// 
    /// #### Notes
    /// This signal is not emitted unless the tab title is `closable`.
    abstract tabCloseRequested: ISignal<TabBar<'T>, TabBar.ITabCloseRequestedArgs<'T>>
    /// A signal emitted when a tab is dragged beyond the detach threshold.
    /// 
    /// #### Notes
    /// This signal is emitted when the user drags a tab with the mouse,
    /// and mouse is dragged beyond the detach threshold.
    /// 
    /// The consumer of the signal should call `releaseMouse` and remove
    /// the tab in order to complete the detach.
    /// 
    /// This signal is only emitted once per drag cycle.
    abstract tabDetachRequested: ISignal<TabBar<'T>, TabBar.ITabDetachRequestedArgs<'T>>
    /// The renderer used by the tab bar.
    abstract renderer: TabBar.IRenderer<'T>
    /// Whether the tabs are movable by the user.
    /// 
    /// #### Notes
    /// Tabs can always be moved programmatically.
    abstract tabsMovable: bool with get, set
    /// Whether a tab can be deselected by the user.
    /// 
    /// #### Notes
    /// Tabs can be always be deselected programmatically.
    abstract allowDeselect: bool with get, set
    /// The selection behavior when inserting a tab.
    abstract insertBehavior: TabBar.InsertBehavior with get, set
    /// The selection behavior when removing a tab.
    abstract removeBehavior: TabBar.RemoveBehavior with get, set
    /// Get the currently selected title.
    /// 
    /// #### Notes
    /// This will be `null` if no tab is selected.
    /// Set the currently selected title.
    /// 
    /// #### Notes
    /// If the title does not exist, the title will be set to `null`.
    abstract currentTitle: Title<'T> option with get, set
    /// Get the index of the currently selected tab.
    /// 
    /// #### Notes
    /// This will be `-1` if no tab is selected.
    /// Set the index of the currently selected tab.
    /// 
    /// #### Notes
    /// If the value is out of range, the index will be set to `-1`.
    abstract currentIndex: float with get, set
    /// Get the orientation of the tab bar.
    /// 
    /// #### Notes
    /// This controls whether the tabs are arranged in a row or column.
    /// Set the orientation of the tab bar.
    /// 
    /// #### Notes
    /// This controls whether the tabs are arranged in a row or column.
    abstract orientation: TabBar.Orientation with get, set
    /// A read-only array of the titles in the tab bar.
    abstract titles: ReadonlyArray<Title<'T>>
    /// The tab bar content node.
    /// 
    /// #### Notes
    /// This is the node which holds the tab nodes.
    /// 
    /// Modifying this node directly can lead to undefined behavior.
    abstract contentNode: HTMLUListElement
    /// <summary>Add a tab to the end of the tab bar.</summary>
    /// <param name="value">- The title which holds the data for the tab,
    /// or an options object to convert to a title.</param>
    abstract addTab: value: U2<Title<'T>, Title.IOptions<'T>> -> Title<'T>
    /// <summary>Insert a tab into the tab bar at the specified index.</summary>
    /// <param name="index">- The index at which to insert the tab.</param>
    /// <param name="value">- The title which holds the data for the tab,
    /// or an options object to convert to a title.</param>
    abstract insertTab: index: float * value: U2<Title<'T>, Title.IOptions<'T>> -> Title<'T>
    /// <summary>Remove a tab from the tab bar.</summary>
    /// <param name="title">- The title for the tab to remove.
    /// 
    /// #### Notes
    /// This is a no-op if the title is not in the tab bar.</param>
    abstract removeTab: title: Title<'T> -> unit
    /// <summary>Remove the tab at a given index from the tab bar.</summary>
    /// <param name="index">- The index of the tab to remove.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract removeTabAt: index: float -> unit
    /// Remove all tabs from the tab bar.
    abstract clearTabs: unit -> unit
    /// Release the mouse and restore the non-dragged tab positions.
    /// 
    /// #### Notes
    /// This will cause the tab bar to stop handling mouse events and to
    /// restore the tabs to their non-dragged positions.
    abstract releaseMouse: unit -> unit
    /// <summary>Handle the DOM events for the tab bar.</summary>
    /// <param name="event">- The DOM event sent to the tab bar.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the tab bar's DOM node.
    /// 
    /// This should not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// Handle the `'keydown'` event for the tab bar.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mousedown'` event for the tab bar.
    abstract _evtMouseDown: obj with get, set
    /// Handle the `'mousemove'` event for the tab bar.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseup'` event for the document.
    abstract _evtMouseUp: obj with get, set
    /// Release the mouse and restore the non-dragged tab positions.
    abstract _releaseMouse: obj with get, set
    /// Adjust the current index for a tab insert operation.
    /// 
    /// This method accounts for the tab bar's insertion behavior when
    /// adjusting the current index and emitting the changed signal.
    abstract _adjustCurrentForInsert: obj with get, set
    /// Adjust the current index for a tab move operation.
    /// 
    /// This method will not cause the actual current tab to change.
    /// It silently adjusts the index to account for the given move.
    abstract _adjustCurrentForMove: obj with get, set
    /// Adjust the current index for a tab remove operation.
    /// 
    /// This method accounts for the tab bar's remove behavior when
    /// adjusting the current index and emitting the changed signal.
    abstract _adjustCurrentForRemove: obj with get, set
    /// Handle the `changed` signal of a title object.
    abstract _onTitleChanged: obj with get, set
    abstract _currentIndex: obj with get, set
    abstract _titles: obj with get, set
    abstract _orientation: obj with get, set
    abstract _previousTitle: obj with get, set
    abstract _dragData: obj with get, set
    abstract _tabMoved: obj with get, set
    abstract _currentChanged: obj with get, set
    abstract _tabCloseRequested: obj with get, set
    abstract _tabDetachRequested: obj with get, set
    abstract _tabActivateRequested: obj with get, set

/// A widget which displays titles as a single row or column of tabs.
/// 
/// #### Notes
/// If CSS transforms are used to rotate nodes for vertically oriented
/// text, then tab dragging will not work correctly. The `tabsMovable`
/// property should be set to `false` when rotating nodes from CSS.
/// The namespace for the `TabBar` class statics.
type [<AllowNullLiteral>] TabBarStatic =
    /// <summary>Construct a new tab bar.</summary>
    /// <param name="options">- The options for initializing the tab bar.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: TabBar.IOptions<'T> -> TabBar<'T>

module TabBar =

    type [<AllowNullLiteral>] IExports =
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer

    type [<StringEnum>] [<RequireQualifiedAccess>] Orientation =
        | Horizontal
        | Vertical

    type [<StringEnum>] [<RequireQualifiedAccess>] InsertBehavior =
        | None
        | [<CompiledName "select-tab">] SelectTab
        | [<CompiledName "select-tab-if-needed">] SelectTabIfNeeded

    type [<StringEnum>] [<RequireQualifiedAccess>] RemoveBehavior =
        | None
        | [<CompiledName "select-tab-after">] SelectTabAfter
        | [<CompiledName "select-tab-before">] SelectTabBefore
        | [<CompiledName "select-previous-tab">] SelectPreviousTab

    /// An options object for creating a tab bar.
    type [<AllowNullLiteral>] IOptions<'T> =
        /// The layout orientation of the tab bar.
        /// 
        /// The default is `horizontal`.
        abstract orientation: TabBar.Orientation option with get, set
        /// Whether the tabs are movable by the user.
        /// 
        /// The default is `false`.
        abstract tabsMovable: bool option with get, set
        /// Whether a tab can be deselected by the user.
        /// 
        /// The default is `false`.
        abstract allowDeselect: bool option with get, set
        /// The selection behavior when inserting a tab.
        /// 
        /// The default is `'select-tab-if-needed'`.
        abstract insertBehavior: TabBar.InsertBehavior option with get, set
        /// The selection behavior when removing a tab.
        /// 
        /// The default is `'select-tab-after'`.
        abstract removeBehavior: TabBar.RemoveBehavior option with get, set
        /// A renderer to use with the tab bar.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer<'T> option with get, set

    /// The arguments object for the `currentChanged` signal.
    type [<AllowNullLiteral>] ICurrentChangedArgs<'T> =
        /// The previously selected index.
        abstract previousIndex: float
        /// The previously selected title.
        abstract previousTitle: Title<'T> option
        /// The currently selected index.
        abstract currentIndex: float
        /// The currently selected title.
        abstract currentTitle: Title<'T> option

    /// The arguments object for the `tabMoved` signal.
    type [<AllowNullLiteral>] ITabMovedArgs<'T> =
        /// The previous index of the tab.
        abstract fromIndex: float
        /// The current index of the tab.
        abstract toIndex: float
        /// The title for the tab.
        abstract title: Title<'T>

    /// The arguments object for the `tabActivateRequested` signal.
    type [<AllowNullLiteral>] ITabActivateRequestedArgs<'T> =
        /// The index of the tab to activate.
        abstract index: float
        /// The title for the tab.
        abstract title: Title<'T>

    /// The arguments object for the `tabCloseRequested` signal.
    type [<AllowNullLiteral>] ITabCloseRequestedArgs<'T> =
        /// The index of the tab to close.
        abstract index: float
        /// The title for the tab.
        abstract title: Title<'T>

    /// The arguments object for the `tabDetachRequested` signal.
    type [<AllowNullLiteral>] ITabDetachRequestedArgs<'T> =
        /// The index of the tab to detach.
        abstract index: float
        /// The title for the tab.
        abstract title: Title<'T>
        /// The node representing the tab.
        abstract tab: HTMLElement
        /// The current client X position of the mouse.
        abstract clientX: float
        /// The current client Y position of the mouse.
        abstract clientY: float

    /// An object which holds the data to render a tab.
    type [<AllowNullLiteral>] IRenderData<'T> =
        /// The title associated with the tab.
        abstract title: Title<'T>
        /// Whether the tab is the current tab.
        abstract current: bool
        /// The z-index for the tab.
        abstract zIndex: float

    /// A renderer for use with a tab bar.
    type [<AllowNullLiteral>] IRenderer<'T> =
        /// A selector which matches the close icon node in a tab.
        abstract closeIconSelector: string
        /// <summary>Render the virtual element for a tab.</summary>
        /// <param name="data">- The data to use for rendering the tab.</param>
        abstract renderTab: data: IRenderData<'T> -> VirtualElement

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer<obj option>
        /// A selector which matches the close icon node in a tab.
        abstract closeIconSelector: string
        /// <summary>Render the virtual element for a tab.</summary>
        /// <param name="data">- The data to use for rendering the tab.</param>
        abstract renderTab: data: IRenderData<obj option> -> VirtualElement
        /// <summary>Render the icon element for a tab.</summary>
        /// <param name="data">- The data to use for rendering the tab.</param>
        abstract renderIcon: data: IRenderData<obj option> -> VirtualElement
        /// <summary>Render the label element for a tab.</summary>
        /// <param name="data">- The data to use for rendering the tab.</param>
        abstract renderLabel: data: IRenderData<obj option> -> VirtualElement
        /// <summary>Render the close icon element for a tab.</summary>
        /// <param name="data">- The data to use for rendering the tab.</param>
        abstract renderCloseIcon: data: IRenderData<obj option> -> VirtualElement
        /// <summary>Create a unique render key for the tab.</summary>
        /// <param name="data">- The data to use for the tab.</param>
        abstract createTabKey: data: IRenderData<obj option> -> string
        /// <summary>Create the inline style object for a tab.</summary>
        /// <param name="data">- The data to use for the tab.</param>
        abstract createTabStyle: data: IRenderData<obj option> -> ElementInlineStyle
        /// <summary>Create the class name for the tab.</summary>
        /// <param name="data">- The data to use for the tab.</param>
        abstract createTabClass: data: IRenderData<obj option> -> string
        /// <summary>Create the dataset for a tab.</summary>
        /// <param name="data">- The data to use for the tab.</param>
        abstract createTabDataset: data: IRenderData<obj option> -> ElementDataset
        /// <summary>Create the class name for the tab icon.</summary>
        /// <param name="data">- The data to use for the tab.</param>
        abstract createIconClass: data: IRenderData<obj option> -> string
        abstract _tabID: obj with get, set
        abstract _tabKeys: obj with get, set

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] RendererStatic =
        /// Construct a new renderer.
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type IIterator = PhosphorAlgorithm.IIterator
// type Message = PhosphorMessaging.Message
// type Layout = __layout.Layout
// type TabBar = __tabbar.TabBar
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract DockLayout: DockLayoutStatic

/// A layout which provides a flexible docking arrangement.
/// 
/// #### Notes
/// The consumer of this layout is responsible for handling all signals
/// from the generated tab bars and managing the visibility of widgets
/// and tab bars as needed.
/// The namespace for the `DockLayout` class statics.
type [<AllowNullLiteral>] DockLayout =
    inherit Layout
    /// Dispose of the resources held by the layout.
    /// 
    /// #### Notes
    /// This will clear and dispose all widgets in the layout.
    abstract dispose: unit -> unit
    /// The renderer used by the dock layout.
    abstract renderer: DockLayout.IRenderer
    /// Get the inter-element spacing for the dock layout.
    /// Set the inter-element spacing for the dock layout.
    abstract spacing: float with get, set
    /// Whether the dock layout is empty.
    abstract isEmpty: bool
    /// Create an iterator over all widgets in the layout.
    abstract iter: unit -> IIterator<Widget>
    /// Create an iterator over the user widgets in the layout.
    abstract widgets: unit -> IIterator<Widget>
    /// Create an iterator over the selected widgets in the layout.
    abstract selectedWidgets: unit -> IIterator<Widget>
    /// Create an iterator over the tab bars in the layout.
    abstract tabBars: unit -> IIterator<TabBar<Widget>>
    /// Create an iterator over the handles in the layout.
    abstract handles: unit -> IIterator<HTMLDivElement>
    /// <summary>Move a handle to the given offset position.</summary>
    /// <param name="handle">- The handle to move.</param>
    /// <param name="offsetX">- The desired offset X position of the handle.</param>
    /// <param name="offsetY">- The desired offset Y position of the handle.
    /// 
    /// #### Notes
    /// If the given handle is not contained in the layout, this is no-op.
    /// 
    /// The handle will be moved as close as possible to the desired
    /// position without violating any of the layout constraints.
    /// 
    /// Only one of the coordinates is used depending on the orientation
    /// of the handle. This method accepts both coordinates to make it
    /// easy to invoke from a mouse move event without needing to know
    /// the handle orientation.</param>
    abstract moveHandle: handle: HTMLDivElement * offsetX: float * offsetY: float -> unit
    /// Save the current configuration of the dock layout.
    abstract saveLayout: unit -> DockLayout.ILayoutConfig
    /// <summary>Restore the layout to a previously saved configuration.</summary>
    /// <param name="config">- The layout configuration to restore.
    /// 
    /// #### Notes
    /// Widgets which currently belong to the layout but which are not
    /// contained in the config will be unparented.</param>
    abstract restoreLayout: config: DockLayout.ILayoutConfig -> unit
    /// <summary>Add a widget to the dock layout.</summary>
    /// <param name="widget">- The widget to add to the dock layout.</param>
    /// <param name="options">- The additional options for adding the widget.
    /// 
    /// #### Notes
    /// The widget will be moved if it is already contained in the layout.
    /// 
    /// An error will be thrown if the reference widget is invalid.</param>
    abstract addWidget: widget: Widget * ?options: DockLayout.IAddOptions -> unit
    /// <summary>Remove a widget from the layout.</summary>
    /// <param name="widget">- The widget to remove from the layout.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method does *not* modify the widget's `parent`.</param>
    abstract removeWidget: widget: Widget -> unit
    /// <summary>Find the tab area which contains the given client position.</summary>
    /// <param name="clientX">- The client X position of interest.</param>
    /// <param name="clientY">- The client Y position of interest.</param>
    abstract hitTestTabAreas: clientX: float * clientY: float -> DockLayout.ITabAreaGeometry option
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach the widget to the layout parent widget.</summary>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This is a no-op if the widget is already attached.</param>
    abstract attachWidget: widget: Widget -> unit
    /// <summary>Detach the widget from the layout parent widget.</summary>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This is a no-op if the widget is not attached.</param>
    abstract detachWidget: widget: Widget -> unit
    /// A message handler invoked on a `'before-show'` message.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'resize'` message.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    abstract onFitRequest: msg: Message -> unit
    /// Remove the specified widget from the layout structure.
    /// 
    /// #### Notes
    /// This is a no-op if the widget is not in the layout tree.
    /// 
    /// This does not detach the widget from the parent node.
    abstract _removeWidget: obj with get, set
    /// Insert a widget next to an existing tab.
    /// 
    /// #### Notes
    /// This does not attach the widget to the parent widget.
    abstract _insertTab: obj with get, set
    /// Insert a widget as a new split area.
    /// 
    /// #### Notes
    /// This does not attach the widget to the parent widget.
    abstract _insertSplit: obj with get, set
    /// Ensure the root is a split node with the given orientation.
    abstract _splitRoot: obj with get, set
    /// Fit the layout to the total size required by the widgets.
    abstract _fit: obj with get, set
    /// Update the layout position and size of the widgets.
    /// 
    /// The parent offset dimensions should be `-1` if unknown.
    abstract _update: obj with get, set
    /// Create a new tab bar for use by the dock layout.
    /// 
    /// #### Notes
    /// The tab bar will be attached to the parent if it exists.
    abstract _createTabBar: obj with get, set
    /// Create a new handle for the dock layout.
    /// 
    /// #### Notes
    /// The handle will be attached to the parent if it exists.
    abstract _createHandle: obj with get, set
    abstract _spacing: obj with get, set
    abstract _dirty: obj with get, set
    abstract _root: obj with get, set
    abstract _box: obj with get, set
    abstract _items: obj with get, set

/// A layout which provides a flexible docking arrangement.
/// 
/// #### Notes
/// The consumer of this layout is responsible for handling all signals
/// from the generated tab bars and managing the visibility of widgets
/// and tab bars as needed.
/// The namespace for the `DockLayout` class statics.
type [<AllowNullLiteral>] DockLayoutStatic =
    /// <summary>Construct a new dock layout.</summary>
    /// <param name="options">- The options for initializing the layout.</param>
    [<Emit "new $0($1...)">] abstract Create: options: DockLayout.IOptions -> DockLayout

module DockLayout =

    /// An options object for creating a dock layout.
    type [<AllowNullLiteral>] IOptions =
        /// The renderer to use for the dock layout.
        abstract renderer: IRenderer with get, set
        /// The spacing between items in the layout.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set

    /// A renderer for use with a dock layout.
    type [<AllowNullLiteral>] IRenderer =
        /// Create a new tab bar for use with a dock layout.
        abstract createTabBar: unit -> TabBar<Widget>
        /// Create a new handle node for use with a dock layout.
        abstract createHandle: unit -> HTMLDivElement

    type [<StringEnum>] [<RequireQualifiedAccess>] InsertMode =
        | [<CompiledName "split-top">] SplitTop
        | [<CompiledName "split-left">] SplitLeft
        | [<CompiledName "split-right">] SplitRight
        | [<CompiledName "split-bottom">] SplitBottom
        | [<CompiledName "tab-before">] TabBefore
        | [<CompiledName "tab-after">] TabAfter

    /// An options object for adding a widget to the dock layout.
    type [<AllowNullLiteral>] IAddOptions =
        /// The insertion mode for adding the widget.
        /// 
        /// The default is `'tab-after'`.
        abstract mode: InsertMode option with get, set
        /// The reference widget for the insert location.
        /// 
        /// The default is `null`.
        abstract ref: Widget option with get, set

    /// A layout config object for a tab area.
    type [<AllowNullLiteral>] ITabAreaConfig =
        /// The discriminated type of the config object.
        abstract ``type``: string with get, set
        /// The widgets contained in the tab area.
        abstract widgets: ResizeArray<Widget> with get, set
        /// The index of the selected tab.
        abstract currentIndex: float with get, set

    /// A layout config object for a split area.
    type [<AllowNullLiteral>] ISplitAreaConfig =
        /// The discriminated type of the config object.
        abstract ``type``: string with get, set
        /// The orientation of the split area.
        abstract orientation: U2<string, string> with get, set
        /// The children in the split area.
        abstract children: ResizeArray<AreaConfig> with get, set
        /// The relative sizes of the children.
        abstract sizes: ResizeArray<float> with get, set

    type AreaConfig =
        U2<ITabAreaConfig, ISplitAreaConfig>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module AreaConfig =
        let ofITabAreaConfig v: AreaConfig = v |> U2.Case1
        let isITabAreaConfig (v: AreaConfig) = match v with U2.Case1 _ -> true | _ -> false
        let asITabAreaConfig (v: AreaConfig) = match v with U2.Case1 o -> Some o | _ -> None
        let ofISplitAreaConfig v: AreaConfig = v |> U2.Case2
        let isISplitAreaConfig (v: AreaConfig) = match v with U2.Case2 _ -> true | _ -> false
        let asISplitAreaConfig (v: AreaConfig) = match v with U2.Case2 o -> Some o | _ -> None

    /// A dock layout configuration object.
    type [<AllowNullLiteral>] ILayoutConfig =
        /// The layout config for the main dock area.
        abstract main: AreaConfig option with get, set

    /// An object which represents the geometry of a tab area.
    type [<AllowNullLiteral>] ITabAreaGeometry =
        /// The tab bar for the tab area.
        abstract tabBar: TabBar<Widget> with get, set
        /// The local X position of the hit test in the dock area.
        /// 
        /// #### Notes
        /// This is the distance from the left edge of the layout parent
        /// widget, to the local X coordinate of the hit test query.
        abstract x: float with get, set
        /// The local Y position of the hit test in the dock area.
        /// 
        /// #### Notes
        /// This is the distance from the top edge of the layout parent
        /// widget, to the local Y coordinate of the hit test query.
        abstract y: float with get, set
        /// The local coordinate of the top edge of the tab area.
        /// 
        /// #### Notes
        /// This is the distance from the top edge of the layout parent
        /// widget, to the top edge of the tab area.
        abstract top: float with get, set
        /// The local coordinate of the left edge of the tab area.
        /// 
        /// #### Notes
        /// This is the distance from the left edge of the layout parent
        /// widget, to the left edge of the tab area.
        abstract left: float with get, set
        /// The local coordinate of the right edge of the tab area.
        /// 
        /// #### Notes
        /// This is the distance from the right edge of the layout parent
        /// widget, to the right edge of the tab area.
        abstract right: float with get, set
        /// The local coordinate of the bottom edge of the tab area.
        /// 
        /// #### Notes
        /// This is the distance from the bottom edge of the layout parent
        /// widget, to the bottom edge of the tab area.
        abstract bottom: float with get, set
        /// The width of the tab area.
        /// 
        /// #### Notes
        /// This is total width allocated for the tab area.
        abstract width: float with get, set
        /// The height of the tab area.
        /// 
        /// #### Notes
        /// This is total height allocated for the tab area.
        abstract height: float with get, set
// type IIterator = PhosphorAlgorithm.IIterator
// type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type DockLayout = __docklayout.DockLayout
// type TabBar = __tabbar.TabBar
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract DockPanel: DockPanelStatic

/// A widget which provides a flexible docking area for widgets.
/// The namespace for the `DockPanel` class statics.
type [<AllowNullLiteral>] DockPanel =
    inherit Widget
    /// Dispose of the resources held by the panel.
    abstract dispose: unit -> unit
    /// A signal emitted when the layout configuration is modified.
    /// 
    /// #### Notes
    /// This signal is emitted whenever the current layout configuration
    /// may have changed.
    /// 
    /// This signal is emitted asynchronously in a collapsed fashion, so
    /// that multiple synchronous modifications results in only a single
    /// emit of the signal.
    abstract layoutModified: ISignal<DockPanel, unit>
    /// The overlay used by the dock panel.
    abstract overlay: DockPanel.IOverlay
    /// The renderer used by the dock panel.
    abstract renderer: DockPanel.IRenderer
    /// Get the spacing between the widgets.
    /// Set the spacing between the widgets.
    abstract spacing: float with get, set
    /// Get the mode for the dock panel.
    /// Set the mode for the dock panel.
    /// 
    /// #### Notes
    /// Changing the mode is a destructive operation with respect to the
    /// panel's layout configuration. If layout state must be preserved,
    /// save the current layout config before changing the mode.
    abstract mode: DockPanel.Mode with get, set
    /// Whether the dock panel is empty.
    abstract isEmpty: bool
    /// Create an iterator over the user widgets in the panel.
    abstract widgets: unit -> IIterator<Widget>
    /// Create an iterator over the selected widgets in the panel.
    abstract selectedWidgets: unit -> IIterator<Widget>
    /// Create an iterator over the tab bars in the panel.
    abstract tabBars: unit -> IIterator<TabBar<Widget>>
    /// Create an iterator over the handles in the panel.
    abstract handles: unit -> IIterator<HTMLDivElement>
    /// <summary>Select a specific widget in the dock panel.</summary>
    /// <param name="widget">- The widget of interest.
    /// 
    /// #### Notes
    /// This will make the widget the current widget in its tab area.</param>
    abstract selectWidget: widget: Widget -> unit
    /// <summary>Activate a specified widget in the dock panel.</summary>
    /// <param name="widget">- The widget of interest.
    /// 
    /// #### Notes
    /// This will select and activate the given widget.</param>
    abstract activateWidget: widget: Widget -> unit
    /// Save the current layout configuration of the dock panel.
    abstract saveLayout: unit -> DockPanel.ILayoutConfig
    /// <summary>Restore the layout to a previously saved configuration.</summary>
    /// <param name="config">- The layout configuration to restore.
    /// 
    /// #### Notes
    /// Widgets which currently belong to the layout but which are not
    /// contained in the config will be unparented.
    /// 
    /// The dock panel automatically reverts to `'multiple-document'`
    /// mode when a layout config is restored.</param>
    abstract restoreLayout: config: DockPanel.ILayoutConfig -> unit
    /// <summary>Add a widget to the dock panel.</summary>
    /// <param name="widget">- The widget to add to the dock panel.</param>
    /// <param name="options">- The additional options for adding the widget.
    /// 
    /// #### Notes
    /// If the panel is in single document mode, the options are ignored
    /// and the widget is always added as tab in the hidden tab bar.</param>
    abstract addWidget: widget: Widget * ?options: DockPanel.IAddOptions -> unit
    /// <summary>Process a message sent to the widget.</summary>
    /// <param name="msg">- The message sent to the widget.</param>
    abstract processMessage: msg: Message -> unit
    /// <summary>Handle the DOM events for the dock panel.</summary>
    /// <param name="event">- The DOM event sent to the panel.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the panel's DOM node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on a `'child-added'` message.
    abstract onChildAdded: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-removed'` message.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit
    /// Handle the `'p-dragenter'` event for the dock panel.
    abstract _evtDragEnter: obj with get, set
    /// Handle the `'p-dragleave'` event for the dock panel.
    abstract _evtDragLeave: obj with get, set
    /// Handle the `'p-dragover'` event for the dock panel.
    abstract _evtDragOver: obj with get, set
    /// Handle the `'p-drop'` event for the dock panel.
    abstract _evtDrop: obj with get, set
    /// Handle the `'keydown'` event for the dock panel.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mousedown'` event for the dock panel.
    abstract _evtMouseDown: obj with get, set
    /// Handle the `'mousemove'` event for the dock panel.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseup'` event for the dock panel.
    abstract _evtMouseUp: obj with get, set
    /// Release the mouse grab for the dock panel.
    abstract _releaseMouse: obj with get, set
    /// Show the overlay indicator at the given client position.
    /// 
    /// Returns the drop zone at the specified client position.
    /// 
    /// #### Notes
    /// If the position is not over a valid zone, the overlay is hidden.
    abstract _showOverlay: obj with get, set
    /// Create a new tab bar for use by the panel.
    abstract _createTabBar: obj with get, set
    /// Create a new handle for use by the panel.
    abstract _createHandle: obj with get, set
    /// Handle the `tabMoved` signal from a tab bar.
    abstract _onTabMoved: obj with get, set
    /// Handle the `currentChanged` signal from a tab bar.
    abstract _onCurrentChanged: obj with get, set
    /// Handle the `tabActivateRequested` signal from a tab bar.
    abstract _onTabActivateRequested: obj with get, set
    /// Handle the `tabCloseRequested` signal from a tab bar.
    abstract _onTabCloseRequested: obj with get, set
    /// Handle the `tabDetachRequested` signal from a tab bar.
    abstract _onTabDetachRequested: obj with get, set
    abstract _edges: obj with get, set
    abstract _mode: obj with get, set
    abstract _drag: obj with get, set
    abstract _renderer: obj with get, set
    abstract _pressData: obj with get, set
    abstract _layoutModified: obj with get, set

/// A widget which provides a flexible docking area for widgets.
/// The namespace for the `DockPanel` class statics.
type [<AllowNullLiteral>] DockPanelStatic =
    /// <summary>Construct a new dock panel.</summary>
    /// <param name="options">- The options for initializing the panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: DockPanel.IOptions -> DockPanel

module DockPanel =

    type [<AllowNullLiteral>] IExports =
        abstract Overlay: OverlayStatic
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer

    /// An options object for creating a dock panel.
    type [<AllowNullLiteral>] IOptions =
        /// The overlay to use with the dock panel.
        /// 
        /// The default is a new `Overlay` instance.
        abstract overlay: IOverlay option with get, set
        /// The renderer to use for the dock panel.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer option with get, set
        /// The spacing between the items in the panel.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set
        /// The mode for the dock panel.
        /// 
        /// The deafult is `'multiple-document'`.
        abstract mode: DockPanel.Mode option with get, set
        /// The sizes of the edge drop zones, in pixels.
        /// If not given, default values will be used.
        abstract edges: IEdges option with get, set

    /// The sizes of the edge drop zones, in pixels.
    type [<AllowNullLiteral>] IEdges =
        /// The size of the top edge drop zone.
        abstract top: float with get, set
        /// The size of the right edge drop zone.
        abstract right: float with get, set
        /// The size of the bottom edge drop zone.
        abstract bottom: float with get, set
        /// The size of the left edge drop zone.
        abstract left: float with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] Mode =
        | [<CompiledName "single-document">] SingleDocument
        | [<CompiledName "multiple-document">] MultipleDocument

    type ILayoutConfig =
        DockLayout.ILayoutConfig

    type InsertMode =
        DockLayout.InsertMode

    type IAddOptions =
        DockLayout.IAddOptions

    /// An object which holds the geometry for overlay positioning.
    type [<AllowNullLiteral>] IOverlayGeometry =
        /// The distance between the overlay and parent top edges.
        abstract top: float with get, set
        /// The distance between the overlay and parent left edges.
        abstract left: float with get, set
        /// The distance between the overlay and parent right edges.
        abstract right: float with get, set
        /// The distance between the overlay and parent bottom edges.
        abstract bottom: float with get, set

    /// An object which manages the overlay node for a dock panel.
    type [<AllowNullLiteral>] IOverlay =
        /// The DOM node for the overlay.
        abstract node: HTMLDivElement
        /// <summary>Show the overlay using the given overlay geometry.</summary>
        /// <param name="geo">- The desired geometry for the overlay.
        /// 
        /// #### Notes
        /// The given geometry values assume the node will use absolute
        /// positioning.
        /// 
        /// This is called on every mouse move event during a drag in order
        /// to update the position of the overlay. It should be efficient.</param>
        abstract show: geo: IOverlayGeometry -> unit
        /// <summary>Hide the overlay node.</summary>
        /// <param name="delay">- The delay (in ms) before hiding the overlay.
        /// A delay value <= 0 should hide the overlay immediately.
        /// 
        /// #### Notes
        /// This is called whenever the overlay node should been hidden.</param>
        abstract hide: delay: float -> unit

    /// A concrete implementation of `IOverlay`.
    /// 
    /// This is the default overlay implementation for a dock panel.
    type [<AllowNullLiteral>] Overlay =
        inherit IOverlay
        /// The DOM node for the overlay.
        abstract node: HTMLDivElement
        /// <summary>Show the overlay using the given overlay geometry.</summary>
        /// <param name="geo">- The desired geometry for the overlay.</param>
        abstract show: geo: IOverlayGeometry -> unit
        /// <summary>Hide the overlay node.</summary>
        /// <param name="delay">- The delay (in ms) before hiding the overlay.
        /// A delay value <= 0 will hide the overlay immediately.</param>
        abstract hide: delay: float -> unit
        abstract _timer: obj with get, set
        abstract _hidden: obj with get, set

    /// A concrete implementation of `IOverlay`.
    /// 
    /// This is the default overlay implementation for a dock panel.
    type [<AllowNullLiteral>] OverlayStatic =
        /// Construct a new overlay.
        [<Emit "new $0($1...)">] abstract Create: unit -> Overlay

    type IRenderer =
        DockLayout.IRenderer

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer
        /// Create a new tab bar for use with a dock panel.
        abstract createTabBar: unit -> TabBar<Widget>
        /// Create a new handle node for use with a dock panel.
        abstract createHandle: unit -> HTMLDivElement

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] RendererStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type IDisposable = PhosphorDisposable.IDisposable
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract FocusTracker: FocusTrackerStatic

/// A class which tracks focus among a set of widgets.
/// 
/// This class is useful when code needs to keep track of the most
/// recently focused widget(s) among a set of related widgets.
/// The namespace for the `FocusTracker` class statics.
type [<AllowNullLiteral>] FocusTracker<'T> =
    inherit IDisposable
    /// Dispose of the resources held by the tracker.
    abstract dispose: unit -> unit
    /// A signal emitted when the current widget has changed.
    abstract currentChanged: ISignal<FocusTracker<'T>, FocusTracker.IChangedArgs<'T>>
    /// A signal emitted when the active widget has changed.
    abstract activeChanged: ISignal<FocusTracker<'T>, FocusTracker.IChangedArgs<'T>>
    /// A flag indicating whether the tracker is disposed.
    abstract isDisposed: bool
    /// The current widget in the tracker.
    /// 
    /// #### Notes
    /// The current widget is the widget among the tracked widgets which
    /// has the *descendant node* which has most recently been focused.
    /// 
    /// The current widget will not be updated if the node loses focus. It
    /// will only be updated when a different tracked widget gains focus.
    /// 
    /// If the current widget is removed from the tracker, the previous
    /// current widget will be restored.
    /// 
    /// This behavior is intended to follow a user's conceptual model of
    /// a semantically "current" widget, where the "last thing of type X"
    /// to be interacted with is the "current instance of X", regardless
    /// of whether that instance still has focus.
    abstract currentWidget: 'T option
    /// The active widget in the tracker.
    /// 
    /// #### Notes
    /// The active widget is the widget among the tracked widgets which
    /// has the *descendant node* which is currently focused.
    abstract activeWidget: 'T option
    /// A read only array of the widgets being tracked.
    abstract widgets: ReadonlyArray<'T>
    /// <summary>Get the focus number for a particular widget in the tracker.</summary>
    /// <param name="widget">- The widget of interest.</param>
    abstract focusNumber: widget: 'T -> float
    /// <summary>Test whether the focus tracker contains a given widget.</summary>
    /// <param name="widget">- The widget of interest.</param>
    abstract has: widget: 'T -> bool
    /// <summary>Add a widget to the focus tracker.</summary>
    /// <param name="widget">- The widget of interest.
    /// 
    /// #### Notes
    /// A widget will be automatically removed from the tracker if it
    /// is disposed after being added.
    /// 
    /// If the widget is already tracked, this is a no-op.</param>
    abstract add: widget: 'T -> unit
    /// Remove a widget from the focus tracker.
    /// 
    /// #### Notes
    /// If the widget is the `currentWidget`, the previous current widget
    /// will become the new `currentWidget`.
    /// 
    /// A widget will be automatically removed from the tracker if it
    /// is disposed after being added.
    /// 
    /// If the widget is not tracked, this is a no-op.
    abstract remove: widget: 'T -> unit
    /// <summary>Handle the DOM events for the focus tracker.</summary>
    /// <param name="event">- The DOM event sent to the panel.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the tracked nodes. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// Set the current and active widgets for the tracker.
    abstract _setWidgets: obj with get, set
    /// Handle the `'focus'` event for a tracked widget.
    abstract _evtFocus: obj with get, set
    /// Handle the `'blur'` event for a tracked widget.
    abstract _evtBlur: obj with get, set
    /// Handle the `disposed` signal for a tracked widget.
    abstract _onWidgetDisposed: obj with get, set
    abstract _counter: obj with get, set
    abstract _widgets: obj with get, set
    abstract _activeWidget: obj with get, set
    abstract _currentWidget: obj with get, set
    abstract _numbers: obj with get, set
    abstract _nodes: obj with get, set
    abstract _activeChanged: obj with get, set
    abstract _currentChanged: obj with get, set

/// A class which tracks focus among a set of widgets.
/// 
/// This class is useful when code needs to keep track of the most
/// recently focused widget(s) among a set of related widgets.
/// The namespace for the `FocusTracker` class statics.
type [<AllowNullLiteral>] FocusTrackerStatic =
    /// Construct a new focus tracker.
    [<Emit "new $0($1...)">] abstract Create: unit -> FocusTracker<'T>

module FocusTracker =

    /// An arguments object for the changed signals.
    type [<AllowNullLiteral>] IChangedArgs<'T> =
        /// The old value for the widget.
        abstract oldValue: 'T option with get, set
        /// The new value for the widget.
        abstract newValue: 'T option with get, set
// type IIterator = PhosphorAlgorithm.IIterator
// type Message = PhosphorMessaging.Message
// type Layout = __layout.Layout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract GridLayout: GridLayoutStatic

/// A layout which arranges its widgets in a grid.
/// The namespace for the `GridLayout` class statics.
type [<AllowNullLiteral>] GridLayout =
    inherit Layout
    /// Dispose of the resources held by the layout.
    abstract dispose: unit -> unit
    /// Get the number of rows in the layout.
    /// Set the number of rows in the layout.
    /// 
    /// #### Notes
    /// The minimum row count is `1`.
    abstract rowCount: float with get, set
    /// Get the number of columns in the layout.
    /// Set the number of columns in the layout.
    /// 
    /// #### Notes
    /// The minimum column count is `1`.
    abstract columnCount: float with get, set
    /// Get the row spacing for the layout.
    /// Set the row spacing for the layout.
    abstract rowSpacing: float with get, set
    /// Get the column spacing for the layout.
    /// Set the col spacing for the layout.
    abstract columnSpacing: float with get, set
    /// <summary>Get the stretch factor for a specific row.</summary>
    /// <param name="index">- The row index of interest.</param>
    abstract rowStretch: index: float -> float
    /// <summary>Set the stretch factor for a specific row.</summary>
    /// <param name="index">- The row index of interest.</param>
    /// <param name="value">- The stretch factor for the row.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract setRowStretch: index: float * value: float -> unit
    /// <summary>Get the stretch factor for a specific column.</summary>
    /// <param name="index">- The column index of interest.</param>
    abstract columnStretch: index: float -> float
    /// <summary>Set the stretch factor for a specific column.</summary>
    /// <param name="index">- The column index of interest.</param>
    /// <param name="value">- The stretch factor for the column.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract setColumnStretch: index: float * value: float -> unit
    /// Create an iterator over the widgets in the layout.
    abstract iter: unit -> IIterator<Widget>
    /// <summary>Add a widget to the grid layout.</summary>
    /// <param name="widget">- The widget to add to the layout.
    /// 
    /// #### Notes
    /// If the widget is already contained in the layout, this is no-op.</param>
    abstract addWidget: widget: Widget -> unit
    /// <summary>Remove a widget from the grid layout.</summary>
    /// <param name="widget">- The widget to remove from the layout.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method does *not* modify the widget's `parent`.</param>
    abstract removeWidget: widget: Widget -> unit
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="widget">- The widget to attach to the parent.</param>
    abstract attachWidget: widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="widget">- The widget to detach from the parent.</param>
    abstract detachWidget: widget: Widget -> unit
    /// A message handler invoked on a `'before-show'` message.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'resize'` message.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    abstract onFitRequest: msg: Message -> unit
    /// Fit the layout to the total size required by the widgets.
    abstract _fit: obj with get, set
    /// Update the layout position and size of the widgets.
    /// 
    /// The parent offset dimensions should be `-1` if unknown.
    abstract _update: obj with get, set
    abstract _dirty: obj with get, set
    abstract _rowSpacing: obj with get, set
    abstract _columnSpacing: obj with get, set
    abstract _items: obj with get, set
    abstract _rowStarts: obj with get, set
    abstract _columnStarts: obj with get, set
    abstract _rowSizers: obj with get, set
    abstract _columnSizers: obj with get, set
    abstract _box: obj with get, set

/// A layout which arranges its widgets in a grid.
/// The namespace for the `GridLayout` class statics.
type [<AllowNullLiteral>] GridLayoutStatic =
    /// <summary>Construct a new grid layout.</summary>
    /// <param name="options">- The options for initializing the layout.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: GridLayout.IOptions -> GridLayout

module GridLayout =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Get the cell config for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getCellConfig: widget: Widget -> ICellConfig
        /// <summary>Set the cell config for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the cell config.</param>
        abstract setCellConfig: widget: Widget * value: obj -> unit

    /// An options object for initializing a grid layout.
    type [<AllowNullLiteral>] IOptions =
        inherit Layout.IOptions
        /// The initial row count for the layout.
        /// 
        /// The default is `1`.
        abstract rowCount: float option with get, set
        /// The initial column count for the layout.
        /// 
        /// The default is `1`.
        abstract columnCount: float option with get, set
        /// The spacing between rows in the layout.
        /// 
        /// The default is `4`.
        abstract rowSpacing: float option with get, set
        /// The spacing between columns in the layout.
        /// 
        /// The default is `4`.
        abstract columnSpacing: float option with get, set

    /// An object which holds the cell configuration for a widget.
    type [<AllowNullLiteral>] ICellConfig =
        /// The row index for the widget.
        abstract row: float
        /// The column index for the widget.
        abstract column: float
        /// The row span for the widget.
        abstract rowSpan: float
        /// The column span for the widget.
        abstract columnSpan: float
// type Message = PhosphorMessaging.Message
// type ElementDataset = ``@phosphor_virtualdom``.ElementDataset
// type VirtualElement = ``@phosphor_virtualdom``.VirtualElement
// type h = ``@phosphor_virtualdom``.h
// type Menu = __menu.Menu
// type Title = __title.Title
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract MenuBar: MenuBarStatic

/// A widget which displays menus as a canonical menu bar.
/// The namespace for the `MenuBar` class statics.
type [<AllowNullLiteral>] MenuBar =
    inherit Widget
    /// Dispose of the resources held by the widget.
    abstract dispose: unit -> unit
    /// The renderer used by the menu bar.
    abstract renderer: MenuBar.IRenderer
    /// The child menu of the menu bar.
    /// 
    /// #### Notes
    /// This will be `null` if the menu bar does not have an open menu.
    abstract childMenu: Menu option
    /// Get the menu bar content node.
    /// 
    /// #### Notes
    /// This is the node which holds the menu title nodes.
    /// 
    /// Modifying this node directly can lead to undefined behavior.
    abstract contentNode: HTMLUListElement
    /// Get the currently active menu.
    /// Set the currently active menu.
    /// 
    /// #### Notes
    /// If the menu does not exist, the menu will be set to `null`.
    abstract activeMenu: Menu option with get, set
    /// Get the index of the currently active menu.
    /// 
    /// #### Notes
    /// This will be `-1` if no menu is active.
    /// Set the index of the currently active menu.
    /// 
    /// #### Notes
    /// If the menu cannot be activated, the index will be set to `-1`.
    abstract activeIndex: float with get, set
    /// A read-only array of the menus in the menu bar.
    abstract menus: ReadonlyArray<Menu>
    /// Open the active menu and activate its first menu item.
    /// 
    /// #### Notes
    /// If there is no active menu, this is a no-op.
    abstract openActiveMenu: unit -> unit
    /// <summary>Add a menu to the end of the menu bar.</summary>
    /// <param name="menu">- The menu to add to the menu bar.
    /// 
    /// #### Notes
    /// If the menu is already added to the menu bar, it will be moved.</param>
    abstract addMenu: menu: Menu -> unit
    /// <summary>Insert a menu into the menu bar at the specified index.</summary>
    /// <param name="index">- The index at which to insert the menu.</param>
    /// <param name="menu">- The menu to insert into the menu bar.
    /// 
    /// #### Notes
    /// The index will be clamped to the bounds of the menus.
    /// 
    /// If the menu is already added to the menu bar, it will be moved.</param>
    abstract insertMenu: index: float * menu: Menu -> unit
    /// <summary>Remove a menu from the menu bar.</summary>
    /// <param name="menu">- The menu to remove from the menu bar.
    /// 
    /// #### Notes
    /// This is a no-op if the menu is not in the menu bar.</param>
    abstract removeMenu: menu: Menu -> unit
    /// <summary>Remove the menu at a given index from the menu bar.</summary>
    /// <param name="index">- The index of the menu to remove.
    /// 
    /// #### Notes
    /// This is a no-op if the index is out of range.</param>
    abstract removeMenuAt: index: float -> unit
    /// Remove all menus from the menu bar.
    abstract clearMenus: unit -> unit
    /// <summary>Handle the DOM events for the menu bar.</summary>
    /// <param name="event">- The DOM event sent to the menu bar.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the menu bar's DOM nodes. It
    /// should not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on an `'activate-request'` message.
    abstract onActivateRequest: msg: Message -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// Handle the `'keydown'` event for the menu bar.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mousedown'` event for the menu bar.
    abstract _evtMouseDown: obj with get, set
    /// Handle the `'mousemove'` event for the menu bar.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseleave'` event for the menu bar.
    abstract _evtMouseLeave: obj with get, set
    /// Open the child menu at the active index immediately.
    /// 
    /// If a different child menu is already open, it will be closed,
    /// even if there is no active menu.
    abstract _openChildMenu: obj with get, set
    /// Close the child menu immediately.
    /// 
    /// This is a no-op if a child menu is not open.
    abstract _closeChildMenu: obj with get, set
    /// Handle the `aboutToClose` signal of a menu.
    abstract _onMenuAboutToClose: obj with get, set
    /// Handle the `menuRequested` signal of a child menu.
    abstract _onMenuMenuRequested: obj with get, set
    /// Handle the `changed` signal of a title object.
    abstract _onTitleChanged: obj with get, set
    abstract _activeIndex: obj with get, set
    abstract _menus: obj with get, set
    abstract _childMenu: obj with get, set

/// A widget which displays menus as a canonical menu bar.
/// The namespace for the `MenuBar` class statics.
type [<AllowNullLiteral>] MenuBarStatic =
    /// <summary>Construct a new menu bar.</summary>
    /// <param name="options">- The options for initializing the menu bar.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: MenuBar.IOptions -> MenuBar

module MenuBar =

    type [<AllowNullLiteral>] IExports =
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer

    /// An options object for creating a menu bar.
    type [<AllowNullLiteral>] IOptions =
        /// A custom renderer for creating menu bar content.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer option with get, set

    /// An object which holds the data to render a menu bar item.
    type [<AllowNullLiteral>] IRenderData =
        /// The title to be rendered.
        abstract title: Title<Widget>
        /// Whether the item is the active item.
        abstract active: bool

    /// A renderer for use with a menu bar.
    type [<AllowNullLiteral>] IRenderer =
        /// <summary>Render the virtual element for a menu bar item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IRenderData -> VirtualElement

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer
        /// <summary>Render the virtual element for a menu bar item.</summary>
        /// <param name="data">- The data to use for rendering the item.</param>
        abstract renderItem: data: IRenderData -> VirtualElement
        /// <summary>Render the icon element for a menu bar item.</summary>
        /// <param name="data">- The data to use for rendering the icon.</param>
        abstract renderIcon: data: IRenderData -> VirtualElement
        /// <summary>Render the label element for a menu item.</summary>
        /// <param name="data">- The data to use for rendering the label.</param>
        abstract renderLabel: data: IRenderData -> VirtualElement
        /// <summary>Create the class name for the menu bar item.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createItemClass: data: IRenderData -> string
        /// <summary>Create the dataset for a menu bar item.</summary>
        /// <param name="data">- The data to use for the item.</param>
        abstract createItemDataset: data: IRenderData -> ElementDataset
        /// <summary>Create the class name for the menu bar item icon.</summary>
        /// <param name="data">- The data to use for the class name.</param>
        abstract createIconClass: data: IRenderData -> string
        /// <summary>Create the render content for the label node.</summary>
        /// <param name="data">- The data to use for the label content.</param>
        abstract formatLabel: data: IRenderData -> PhosphorVirtualdom.H.Child

    /// The default implementation of `IRenderer`.
    /// 
    /// #### Notes
    /// Subclasses are free to reimplement rendering methods as needed.
    type [<AllowNullLiteral>] RendererStatic =
        /// Construct a new renderer.
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type Message = PhosphorMessaging.Message
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract ScrollBar: ScrollBarStatic

/// A widget which implements a canonical scroll bar.
/// The namespace for the `ScrollBar` class statics.
type [<AllowNullLiteral>] ScrollBar =
    inherit Widget
    /// A signal emitted when the user moves the scroll thumb.
    /// 
    /// #### Notes
    /// The payload is the current value of the scroll bar.
    abstract thumbMoved: ISignal<ScrollBar, float>
    /// A signal emitted when the user clicks a step button.
    /// 
    /// #### Notes
    /// The payload is whether a decrease or increase is requested.
    abstract stepRequested: ISignal<ScrollBar, U2<string, string>>
    /// A signal emitted when the user clicks the scroll track.
    /// 
    /// #### Notes
    /// The payload is whether a decrease or increase is requested.
    abstract pageRequested: ISignal<ScrollBar, U2<string, string>>
    /// Get the orientation of the scroll bar.
    /// Set the orientation of the scroll bar.
    abstract orientation: ScrollBar.Orientation with get, set
    /// Get the current value of the scroll bar.
    /// Set the current value of the scroll bar.
    /// 
    /// #### Notes
    /// The value will be clamped to the range `[0, maximum]`.
    abstract value: float with get, set
    /// Get the page size of the scroll bar.
    /// 
    /// #### Notes
    /// The page size is the amount of visible content in the scrolled
    /// region, expressed in data units. It determines the size of the
    /// scroll bar thumb.
    /// Set the page size of the scroll bar.
    /// 
    /// #### Notes
    /// The page size will be clamped to the range `[0, Infinity]`.
    abstract page: float with get, set
    /// Get the maximum value of the scroll bar.
    /// Set the maximum value of the scroll bar.
    /// 
    /// #### Notes
    /// The max size will be clamped to the range `[0, Infinity]`.
    abstract maximum: float with get, set
    /// The scroll bar decrement button node.
    /// 
    /// #### Notes
    /// Modifying this node directly can lead to undefined behavior.
    abstract decrementNode: HTMLDivElement
    /// The scroll bar increment button node.
    /// 
    /// #### Notes
    /// Modifying this node directly can lead to undefined behavior.
    abstract incrementNode: HTMLDivElement
    /// The scroll bar track node.
    /// 
    /// #### Notes
    /// Modifying this node directly can lead to undefined behavior.
    abstract trackNode: HTMLDivElement
    /// The scroll bar thumb node.
    /// 
    /// #### Notes
    /// Modifying this node directly can lead to undefined behavior.
    abstract thumbNode: HTMLDivElement
    /// <summary>Handle the DOM events for the scroll bar.</summary>
    /// <param name="event">- The DOM event sent to the scroll bar.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the scroll bar's DOM node.
    /// 
    /// This should not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A method invoked on a 'before-attach' message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A method invoked on an 'after-detach' message.
    abstract onAfterDetach: msg: Message -> unit
    /// A method invoked on an 'update-request' message.
    abstract onUpdateRequest: msg: Message -> unit
    /// Handle the `'keydown'` event for the scroll bar.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mousedown'` event for the scroll bar.
    abstract _evtMouseDown: obj with get, set
    /// Handle the `'mousemove'` event for the scroll bar.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseup'` event for the scroll bar.
    abstract _evtMouseUp: obj with get, set
    /// Release the mouse and restore the node states.
    abstract _releaseMouse: obj with get, set
    /// Move the thumb to the specified position.
    abstract _moveThumb: obj with get, set
    /// A timeout callback for repeating the mouse press.
    abstract _onRepeat: obj with get, set
    abstract _value: obj with get, set
    abstract _page: obj with get, set
    abstract _maximum: obj with get, set
    abstract _repeatTimer: obj with get, set
    abstract _orientation: obj with get, set
    abstract _pressData: obj with get, set
    abstract _thumbMoved: obj with get, set
    abstract _stepRequested: obj with get, set
    abstract _pageRequested: obj with get, set

/// A widget which implements a canonical scroll bar.
/// The namespace for the `ScrollBar` class statics.
type [<AllowNullLiteral>] ScrollBarStatic =
    /// <summary>Construct a new scroll bar.</summary>
    /// <param name="options">- The options for initializing the scroll bar.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: ScrollBar.IOptions -> ScrollBar

module ScrollBar =

    type [<StringEnum>] [<RequireQualifiedAccess>] Orientation =
        | Horizontal
        | Vertical

    /// An options object for creating a scroll bar.
    type [<AllowNullLiteral>] IOptions =
        /// The orientation of the scroll bar.
        /// 
        /// The default is `'vertical'`.
        abstract orientation: Orientation option with get, set
        /// The value for the scroll bar.
        /// 
        /// The default is `0`.
        abstract value: float option with get, set
        /// The page size for the scroll bar.
        /// 
        /// The default is `10`.
        abstract page: float option with get, set
        /// The maximum value for the scroll bar.
        /// 
        /// The default is `100`.
        abstract maximum: float option with get, set
// type IIterator = PhosphorAlgorithm.IIterator
// type Layout = __layout.Layout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract SingletonLayout: SingletonLayoutStatic

/// A concrete layout implementation which holds a single widget.
/// 
/// #### Notes
/// This class is useful for creating simple container widgets which
/// hold a single child. The child should be positioned with CSS.
type [<AllowNullLiteral>] SingletonLayout =
    inherit Layout
    /// Dispose of the resources held by the layout.
    abstract dispose: unit -> unit
    /// Get the child widget for the layout.
    /// Set the child widget for the layout.
    /// 
    /// #### Notes
    /// Setting the child widget will cause the old child widget to be
    /// automatically disposed. If that is not desired, set the parent
    /// of the old child to `null` before assigning a new child.
    abstract widget: Widget option with get, set
    /// Create an iterator over the widgets in the layout.
    abstract iter: unit -> IIterator<Widget>
    /// <summary>Remove a widget from the layout.</summary>
    /// <param name="widget">- The widget to remove from the layout.
    /// 
    /// #### Notes
    /// A widget is automatically removed from the layout when its `parent`
    /// is set to `null`. This method should only be invoked directly when
    /// removing a widget from a layout which has yet to be installed on a
    /// parent widget.
    /// 
    /// This method does *not* modify the widget's `parent`.</param>
    abstract removeWidget: widget: Widget -> unit
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This method is called automatically by the single layout at the
    /// appropriate time. It should not be called directly by user code.
    /// 
    /// The default implementation adds the widgets's node to the parent's
    /// node at the proper location, and sends the appropriate attach
    /// messages to the widget if the parent is attached to the DOM.
    /// 
    /// Subclasses may reimplement this method to control how the widget's
    /// node is added to the parent's node.</param>
    abstract attachWidget: widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This method is called automatically by the single layout at the
    /// appropriate time. It should not be called directly by user code.
    /// 
    /// The default implementation removes the widget's node from the
    /// parent's node, and sends the appropriate detach messages to the
    /// widget if the parent is attached to the DOM.
    /// 
    /// Subclasses may reimplement this method to control how the widget's
    /// node is removed from the parent's node.</param>
    abstract detachWidget: widget: Widget -> unit
    abstract _widget: obj with get, set

/// A concrete layout implementation which holds a single widget.
/// 
/// #### Notes
/// This class is useful for creating simple container widgets which
/// hold a single child. The child should be positioned with CSS.
type [<AllowNullLiteral>] SingletonLayoutStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> SingletonLayout
// type Message = PhosphorMessaging.Message
// type PanelLayout = __panellayout.PanelLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract SplitLayout: SplitLayoutStatic

/// A layout which arranges its widgets into resizable sections.
/// The namespace for the `SplitLayout` class statics.
type [<AllowNullLiteral>] SplitLayout =
    inherit PanelLayout
    /// Dispose of the resources held by the layout.
    abstract dispose: unit -> unit
    /// The renderer used by the split layout.
    abstract renderer: SplitLayout.IRenderer
    /// Get the layout orientation for the split layout.
    /// Set the layout orientation for the split layout.
    abstract orientation: SplitLayout.Orientation with get, set
    /// Get the content alignment for the split layout.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand  to fill the
    /// entire split layout.
    /// Set the content alignment for the split layout.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand  to fill the
    /// entire split layout.
    abstract alignment: SplitLayout.Alignment with get, set
    /// Get the inter-element spacing for the split layout.
    /// Set the inter-element spacing for the split layout.
    abstract spacing: float with get, set
    /// A read-only array of the split handles in the layout.
    abstract handles: ReadonlyArray<HTMLDivElement>
    /// Get the relative sizes of the widgets in the layout.
    abstract relativeSizes: unit -> ResizeArray<float>
    /// <summary>Set the relative sizes for the widgets in the layout.</summary>
    /// <param name="sizes">- The relative sizes for the widgets in the panel.
    /// 
    /// #### Notes
    /// Extra values are ignored, too few will yield an undefined layout.
    /// 
    /// The actual geometry of the DOM nodes is updated asynchronously.</param>
    abstract setRelativeSizes: sizes: ResizeArray<float> -> unit
    /// <summary>Move the offset position of a split handle.</summary>
    /// <param name="index">- The index of the handle of the interest.</param>
    /// <param name="position">- The desired offset position of the handle.
    /// 
    /// #### Notes
    /// The position is relative to the offset parent.
    /// 
    /// This will move the handle as close as possible to the desired
    /// position. The sibling widgets will be adjusted as necessary.</param>
    abstract moveHandle: index: float * position: float -> unit
    /// Perform layout initialization which requires the parent widget.
    abstract init: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="index">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract attachWidget: index: float * widget: Widget -> unit
    /// <summary>Move a widget in the parent's DOM node.</summary>
    /// <param name="fromIndex">- The previous index of the widget in the layout.</param>
    /// <param name="toIndex">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to move in the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract moveWidget: fromIndex: float * toIndex: float * widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="index">- The previous index of the widget in the layout.</param>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract detachWidget: index: float * widget: Widget -> unit
    /// A message handler invoked on a `'before-show'` message.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'resize'` message.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    abstract onFitRequest: msg: Message -> unit
    /// Fit the layout to the total size required by the widgets.
    abstract _fit: obj with get, set
    /// Update the layout position and size of the widgets.
    /// 
    /// The parent offset dimensions should be `-1` if unknown.
    abstract _update: obj with get, set
    abstract _fixed: obj with get, set
    abstract _spacing: obj with get, set
    abstract _dirty: obj with get, set
    abstract _hasNormedSizes: obj with get, set
    abstract _sizers: obj with get, set
    abstract _items: obj with get, set
    abstract _handles: obj with get, set
    abstract _box: obj with get, set
    abstract _alignment: obj with get, set
    abstract _orientation: obj with get, set

/// A layout which arranges its widgets into resizable sections.
/// The namespace for the `SplitLayout` class statics.
type [<AllowNullLiteral>] SplitLayoutStatic =
    /// <summary>Construct a new split layout.</summary>
    /// <param name="options">- The options for initializing the layout.</param>
    [<Emit "new $0($1...)">] abstract Create: options: SplitLayout.IOptions -> SplitLayout

module SplitLayout =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Get the split layout stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getStretch: widget: Widget -> float
        /// <summary>Set the split layout stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the stretch factor.</param>
        abstract setStretch: widget: Widget * value: float -> unit

    type [<StringEnum>] [<RequireQualifiedAccess>] Orientation =
        | Horizontal
        | Vertical

    type [<StringEnum>] [<RequireQualifiedAccess>] Alignment =
        | Start
        | Center
        | End
        | Justify

    /// An options object for initializing a split layout.
    type [<AllowNullLiteral>] IOptions =
        /// The renderer to use for the split layout.
        abstract renderer: IRenderer with get, set
        /// The orientation of the layout.
        /// 
        /// The default is `'horizontal'`.
        abstract orientation: Orientation option with get, set
        /// The content alignment of the layout.
        /// 
        /// The default is `'start'`.
        abstract alignment: Alignment option with get, set
        /// The spacing between items in the layout.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set

    /// A renderer for use with a split layout.
    type [<AllowNullLiteral>] IRenderer =
        /// Create a new handle for use with a split layout.
        abstract createHandle: unit -> HTMLDivElement
// type Message = PhosphorMessaging.Message
// type Panel = __panel.Panel
// type SplitLayout = __splitlayout.SplitLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract SplitPanel: SplitPanelStatic

/// A panel which arranges its widgets into resizable sections.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[SplitLayout]].
/// The namespace for the `SplitPanel` class statics.
type [<AllowNullLiteral>] SplitPanel =
    inherit Panel
    /// Dispose of the resources held by the panel.
    abstract dispose: unit -> unit
    /// Get the layout orientation for the split panel.
    /// Set the layout orientation for the split panel.
    abstract orientation: SplitPanel.Orientation with get, set
    /// Get the content alignment for the split panel.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire split panel.
    /// Set the content alignment for the split panel.
    /// 
    /// #### Notes
    /// This is the alignment of the widgets in the layout direction.
    /// 
    /// The alignment has no effect if the widgets can expand to fill the
    /// entire split panel.
    abstract alignment: SplitPanel.Alignment with get, set
    /// Get the inter-element spacing for the split panel.
    /// Set the inter-element spacing for the split panel.
    abstract spacing: float with get, set
    /// The renderer used by the split panel.
    abstract renderer: SplitPanel.IRenderer
    /// A read-only array of the split handles in the panel.
    abstract handles: ReadonlyArray<HTMLDivElement>
    /// Get the relative sizes of the widgets in the panel.
    abstract relativeSizes: unit -> ResizeArray<float>
    /// <summary>Set the relative sizes for the widgets in the panel.</summary>
    /// <param name="sizes">- The relative sizes for the widgets in the panel.
    /// 
    /// #### Notes
    /// Extra values are ignored, too few will yield an undefined layout.
    /// 
    /// The actual geometry of the DOM nodes is updated asynchronously.</param>
    abstract setRelativeSizes: sizes: ResizeArray<float> -> unit
    /// <summary>Handle the DOM events for the split panel.</summary>
    /// <param name="event">- The DOM event sent to the panel.
    /// 
    /// #### Notes
    /// This method implements the DOM `EventListener` interface and is
    /// called in response to events on the panel's DOM node. It should
    /// not be called directly by user code.</param>
    abstract handleEvent: ``event``: Event -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on an `'after-detach'` message.
    abstract onAfterDetach: msg: Message -> unit
    /// A message handler invoked on a `'child-added'` message.
    abstract onChildAdded: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-removed'` message.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit
    /// Handle the `'keydown'` event for the split panel.
    abstract _evtKeyDown: obj with get, set
    /// Handle the `'mousedown'` event for the split panel.
    abstract _evtMouseDown: obj with get, set
    /// Handle the `'mousemove'` event for the split panel.
    abstract _evtMouseMove: obj with get, set
    /// Handle the `'mouseup'` event for the split panel.
    abstract _evtMouseUp: obj with get, set
    /// Release the mouse grab for the split panel.
    abstract _releaseMouse: obj with get, set
    abstract _pressData: obj with get, set

/// A panel which arranges its widgets into resizable sections.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[SplitLayout]].
/// The namespace for the `SplitPanel` class statics.
type [<AllowNullLiteral>] SplitPanelStatic =
    /// <summary>Construct a new split panel.</summary>
    /// <param name="options">- The options for initializing the split panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: SplitPanel.IOptions -> SplitPanel

module SplitPanel =

    type [<AllowNullLiteral>] IExports =
        abstract Renderer: RendererStatic
        abstract defaultRenderer: Renderer
        /// <summary>Get the split panel stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        abstract getStretch: widget: Widget -> float
        /// <summary>Set the split panel stretch factor for the given widget.</summary>
        /// <param name="widget">- The widget of interest.</param>
        /// <param name="value">- The value for the stretch factor.</param>
        abstract setStretch: widget: Widget * value: float -> unit

    type Orientation =
        SplitLayout.Orientation

    type Alignment =
        SplitLayout.Alignment

    type IRenderer =
        SplitLayout.IRenderer

    /// An options object for initializing a split panel.
    type [<AllowNullLiteral>] IOptions =
        /// The renderer to use for the split panel.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: IRenderer option with get, set
        /// The layout orientation of the panel.
        /// 
        /// The default is `'horizontal'`.
        abstract orientation: Orientation option with get, set
        /// The content alignment of the panel.
        /// 
        /// The default is `'start'`.
        abstract alignment: Alignment option with get, set
        /// The spacing between items in the panel.
        /// 
        /// The default is `4`.
        abstract spacing: float option with get, set
        /// The split layout to use for the split panel.
        /// 
        /// If this is provided, the other options are ignored.
        /// 
        /// The default is a new `SplitLayout`.
        abstract layout: SplitLayout option with get, set

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] Renderer =
        inherit IRenderer
        /// Create a new handle for use with a split panel.
        abstract createHandle: unit -> HTMLDivElement

    /// The default implementation of `IRenderer`.
    type [<AllowNullLiteral>] RendererStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Renderer
// type Message = PhosphorMessaging.Message
// type PanelLayout = __panellayout.PanelLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract StackedLayout: StackedLayoutStatic

/// A layout where visible widgets are stacked atop one another.
/// 
/// #### Notes
/// The Z-order of the visible widgets follows their layout order.
type [<AllowNullLiteral>] StackedLayout =
    inherit PanelLayout
    /// Dispose of the resources held by the layout.
    abstract dispose: unit -> unit
    /// <summary>Attach a widget to the parent's DOM node.</summary>
    /// <param name="index">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to attach to the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract attachWidget: index: float * widget: Widget -> unit
    /// <summary>Move a widget in the parent's DOM node.</summary>
    /// <param name="fromIndex">- The previous index of the widget in the layout.</param>
    /// <param name="toIndex">- The current index of the widget in the layout.</param>
    /// <param name="widget">- The widget to move in the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract moveWidget: fromIndex: float * toIndex: float * widget: Widget -> unit
    /// <summary>Detach a widget from the parent's DOM node.</summary>
    /// <param name="index">- The previous index of the widget in the layout.</param>
    /// <param name="widget">- The widget to detach from the parent.
    /// 
    /// #### Notes
    /// This is a reimplementation of the superclass method.</param>
    abstract detachWidget: index: float * widget: Widget -> unit
    /// A message handler invoked on a `'before-show'` message.
    abstract onBeforeShow: msg: Message -> unit
    /// A message handler invoked on a `'before-attach'` message.
    abstract onBeforeAttach: msg: Message -> unit
    /// A message handler invoked on a `'child-shown'` message.
    abstract onChildShown: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-hidden'` message.
    abstract onChildHidden: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'resize'` message.
    abstract onResize: msg: Widget.ResizeMessage -> unit
    /// A message handler invoked on an `'update-request'` message.
    abstract onUpdateRequest: msg: Message -> unit
    /// A message handler invoked on a `'fit-request'` message.
    abstract onFitRequest: msg: Message -> unit
    /// Fit the layout to the total size required by the widgets.
    abstract _fit: obj with get, set
    /// Update the layout position and size of the widgets.
    /// 
    /// The parent offset dimensions should be `-1` if unknown.
    abstract _update: obj with get, set
    abstract _dirty: obj with get, set
    abstract _items: obj with get, set
    abstract _box: obj with get, set

/// A layout where visible widgets are stacked atop one another.
/// 
/// #### Notes
/// The Z-order of the visible widgets follows their layout order.
type [<AllowNullLiteral>] StackedLayoutStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> StackedLayout
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type Panel = __panel.Panel
// type StackedLayout = __stackedlayout.StackedLayout
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract StackedPanel: StackedPanelStatic

/// A panel where visible widgets are stacked atop one another.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[StackedLayout]].
/// The namespace for the `StackedPanel` class statics.
type [<AllowNullLiteral>] StackedPanel =
    inherit Panel
    /// A signal emitted when a widget is removed from a stacked panel.
    abstract widgetRemoved: ISignal<StackedPanel, Widget>
    /// A message handler invoked on a `'child-added'` message.
    abstract onChildAdded: msg: Widget.ChildMessage -> unit
    /// A message handler invoked on a `'child-removed'` message.
    abstract onChildRemoved: msg: Widget.ChildMessage -> unit
    abstract _widgetRemoved: obj with get, set

/// A panel where visible widgets are stacked atop one another.
/// 
/// #### Notes
/// This class provides a convenience wrapper around a [[StackedLayout]].
/// The namespace for the `StackedPanel` class statics.
type [<AllowNullLiteral>] StackedPanelStatic =
    /// <summary>Construct a new stacked panel.</summary>
    /// <param name="options">- The options for initializing the panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: StackedPanel.IOptions -> StackedPanel

module StackedPanel =

    /// An options object for creating a stacked panel.
    type [<AllowNullLiteral>] IOptions =
        /// The stacked layout to use for the stacked panel.
        /// 
        /// The default is a new `StackedLayout`.
        abstract layout: StackedLayout option with get, set
// type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // = ``@phosphor_signaling``.ISignal
// type StackedPanel = __stackedpanel.StackedPanel
// type TabBar = __tabbar.TabBar
// type Widget = __widget.Widget

// type [<AllowNullLiteral>] IExports =
//     abstract TabPanel: TabPanelStatic

/// A widget which combines a `TabBar` and a `StackedPanel`.
/// 
/// #### Notes
/// This is a simple panel which handles the common case of a tab bar
/// placed next to a content area. The selected tab controls the widget
/// which is shown in the content area.
/// 
/// For use cases which require more control than is provided by this
/// panel, the `TabBar` widget may be used independently.
/// The namespace for the `TabPanel` class statics.
type [<AllowNullLiteral>] TabPanel =
    inherit Widget
    /// A signal emitted when the current tab is changed.
    /// 
    /// #### Notes
    /// This signal is emitted when the currently selected tab is changed
    /// either through user or programmatic interaction.
    /// 
    /// Notably, this signal is not emitted when the index of the current
    /// tab changes due to tabs being inserted, removed, or moved. It is
    /// only emitted when the actual current tab node is changed.
    abstract currentChanged: ISignal<TabPanel, TabPanel.ICurrentChangedArgs>
    /// Get the index of the currently selected tab.
    /// 
    /// #### Notes
    /// This will be `-1` if no tab is selected.
    /// Set the index of the currently selected tab.
    /// 
    /// #### Notes
    /// If the index is out of range, it will be set to `-1`.
    abstract currentIndex: float with get, set
    /// Get the currently selected widget.
    /// 
    /// #### Notes
    /// This will be `null` if there is no selected tab.
    /// Set the currently selected widget.
    /// 
    /// #### Notes
    /// If the widget is not in the panel, it will be set to `null`.
    abstract currentWidget: Widget option with get, set
    /// Get the whether the tabs are movable by the user.
    /// 
    /// #### Notes
    /// Tabs can always be moved programmatically.
    /// Set the whether the tabs are movable by the user.
    /// 
    /// #### Notes
    /// Tabs can always be moved programmatically.
    abstract tabsMovable: bool with get, set
    /// Get the tab placement for the tab panel.
    /// 
    /// #### Notes
    /// This controls the position of the tab bar relative to the content.
    /// Set the tab placement for the tab panel.
    /// 
    /// #### Notes
    /// This controls the position of the tab bar relative to the content.
    abstract tabPlacement: TabPanel.TabPlacement with get, set
    /// The tab bar used by the tab panel.
    /// 
    /// #### Notes
    /// Modifying the tab bar directly can lead to undefined behavior.
    abstract tabBar: TabBar<Widget>
    /// The stacked panel used by the tab panel.
    /// 
    /// #### Notes
    /// Modifying the panel directly can lead to undefined behavior.
    abstract stackedPanel: StackedPanel
    /// A read-only array of the widgets in the panel.
    abstract widgets: ReadonlyArray<Widget>
    /// <summary>Add a widget to the end of the tab panel.</summary>
    /// <param name="widget">- The widget to add to the tab panel.
    /// 
    /// #### Notes
    /// If the widget is already contained in the panel, it will be moved.
    /// 
    /// The widget's `title` is used to populate the tab.</param>
    abstract addWidget: widget: Widget -> unit
    /// <summary>Insert a widget into the tab panel at a specified index.</summary>
    /// <param name="index">- The index at which to insert the widget.</param>
    /// <param name="widget">- The widget to insert into to the tab panel.
    /// 
    /// #### Notes
    /// If the widget is already contained in the panel, it will be moved.
    /// 
    /// The widget's `title` is used to populate the tab.</param>
    abstract insertWidget: index: float * widget: Widget -> unit
    /// Handle the `currentChanged` signal from the tab bar.
    abstract _onCurrentChanged: obj with get, set
    /// Handle the `tabActivateRequested` signal from the tab bar.
    abstract _onTabActivateRequested: obj with get, set
    /// Handle the `tabCloseRequested` signal from the tab bar.
    abstract _onTabCloseRequested: obj with get, set
    /// Handle the `tabMoved` signal from the tab bar.
    abstract _onTabMoved: obj with get, set
    /// Handle the `widgetRemoved` signal from the stacked panel.
    abstract _onWidgetRemoved: obj with get, set
    abstract _tabPlacement: obj with get, set
    abstract _currentChanged: obj with get, set

/// A widget which combines a `TabBar` and a `StackedPanel`.
/// 
/// #### Notes
/// This is a simple panel which handles the common case of a tab bar
/// placed next to a content area. The selected tab controls the widget
/// which is shown in the content area.
/// 
/// For use cases which require more control than is provided by this
/// panel, the `TabBar` widget may be used independently.
/// The namespace for the `TabPanel` class statics.
type [<AllowNullLiteral>] TabPanelStatic =
    /// <summary>Construct a new tab panel.</summary>
    /// <param name="options">- The options for initializing the tab panel.</param>
    [<Emit "new $0($1...)">] abstract Create: ?options: TabPanel.IOptions -> TabPanel

module TabPanel =

    type [<StringEnum>] [<RequireQualifiedAccess>] TabPlacement =
        | Top
        | Left
        | Right
        | Bottom

    /// An options object for initializing a tab panel.
    type [<AllowNullLiteral>] IOptions =
        /// Whether the tabs are movable by the user.
        /// 
        /// The default is `false`.
        abstract tabsMovable: bool option with get, set
        /// The placement of the tab bar relative to the content.
        /// 
        /// The default is `'top'`.
        abstract tabPlacement: TabPlacement option with get, set
        /// The renderer for the panel's tab bar.
        /// 
        /// The default is a shared renderer instance.
        abstract renderer: TabBar.IRenderer<Widget> option with get, set

    /// The arguments object for the `currentChanged` signal.
    type [<AllowNullLiteral>] ICurrentChangedArgs =
        /// The previously selected index.
        abstract previousIndex: float with get, set
        /// The previously selected widget.
        abstract previousWidget: Widget option with get, set
        /// The currently selected index.
        abstract currentIndex: float with get, set
        /// The currently selected widget.
        abstract currentWidget: Widget option with get, set
