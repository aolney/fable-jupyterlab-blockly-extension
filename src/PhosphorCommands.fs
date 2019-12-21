// ts2fable 0.0.0
module rec PhosphorCommands
open System
open Fable.Core
open Fable.Core.JS
open Browser

//amo typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
type KeyboardEvent = Browser.Types.KeyboardEvent
//end typescript

type ReadonlyJSONObject = PhosphorCoreutils.ReadonlyJSONObject // @phosphor_coreutils.ReadonlyJSONObject
type IDisposable = PhosphorDisposable.IDisposable // @phosphor_disposable.IDisposable
type ISignal<'T,'U> = PhosphorSignaling.ISignal<'T,'U> // @phosphor_signaling.ISignal

type [<AllowNullLiteral>] IExports =
    abstract CommandRegistry: CommandRegistryStatic

/// An object which manages a collection of commands.
/// 
/// #### Notes
/// A command registry can be used to populate a variety of action-based
/// widgets, such as command palettes, menus, and toolbars.
/// The namespace for the `CommandRegistry` class statics.
type [<AllowNullLiteral>] CommandRegistry =
    /// A signal emitted when a command has changed.
    /// 
    /// #### Notes
    /// This signal is useful for visual representations of commands which
    /// need to refresh when the state of a relevant command has changed.
    abstract commandChanged: ISignal<CommandRegistry, CommandRegistry.ICommandChangedArgs>
    /// A signal emitted when a command has executed.
    /// 
    /// #### Notes
    /// Care should be taken when consuming this signal. It is intended to
    /// be used largely for debugging and logging purposes. It should not
    /// be (ab)used for general purpose spying on command execution.
    abstract commandExecuted: ISignal<CommandRegistry, CommandRegistry.ICommandExecutedArgs>
    /// A signal emitted when a key binding is changed.
    abstract keyBindingChanged: ISignal<CommandRegistry, CommandRegistry.IKeyBindingChangedArgs>
    /// A read-only array of the key bindings in the registry.
    abstract keyBindings: ReadonlyArray<CommandRegistry.IKeyBinding>
    /// List the ids of the registered commands.
    abstract listCommands: unit -> ResizeArray<string>
    /// <summary>Test whether a specific command is registered.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    abstract hasCommand: id: string -> bool
    /// <summary>Add a command to the registry.</summary>
    /// <param name="id">- The unique id of the command.</param>
    /// <param name="options">- The options for the command.</param>
    abstract addCommand: id: string * options: CommandRegistry.ICommandOptions -> IDisposable
    /// <summary>Notify listeners that the state of a command has changed.</summary>
    /// <param name="id">- The id of the command which has changed. If more than
    /// one command has changed, this argument should be omitted.</param>
    abstract notifyCommandChanged: ?id: string -> unit
    /// <summary>Get the display label for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract label: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the mnemonic index for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract mnemonic: id: string * ?args: ReadonlyJSONObject -> float
    abstract icon: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the icon class for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract iconClass: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the icon label for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract iconLabel: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the short form caption for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract caption: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the usage help text for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract usage: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the extra class name for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract className: id: string * ?args: ReadonlyJSONObject -> string
    /// <summary>Get the dataset for a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract dataset: id: string * ?args: ReadonlyJSONObject -> CommandRegistry.Dataset
    /// <summary>Test whether a specific command is enabled.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract isEnabled: id: string * ?args: ReadonlyJSONObject -> bool
    /// <summary>Test whether a specific command is toggled.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract isToggled: id: string * ?args: ReadonlyJSONObject -> bool
    /// <summary>Test whether a specific command is visible.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract isVisible: id: string * ?args: ReadonlyJSONObject -> bool
    /// <summary>Execute a specific command.</summary>
    /// <param name="id">- The id of the command of interest.</param>
    /// <param name="args">- The arguments for the command.</param>
    abstract execute: id: string * ?args: ReadonlyJSONObject -> Promise<obj option>
    /// <summary>Add a key binding to the registry.</summary>
    /// <param name="options">- The options for creating the key binding.</param>
    abstract addKeyBinding: options: CommandRegistry.IKeyBindingOptions -> IDisposable
    /// <summary>Process a `'keydown'` event and invoke a matching key binding.</summary>
    /// <param name="event">- The event object for a `'keydown'` event.
    /// 
    /// #### Notes
    /// This should be called in response to a `'keydown'` event in order
    /// to invoke the command for the best matching key binding.
    /// 
    /// The registry **does not** install its own listener for `'keydown'`
    /// events. This allows the application full control over the nodes
    /// and phase for which the registry processes `'keydown'` events.</param>
    abstract processKeydownEvent: ``event``: KeyboardEvent -> unit

/// An object which manages a collection of commands.
/// 
/// #### Notes
/// A command registry can be used to populate a variety of action-based
/// widgets, such as command palettes, menus, and toolbars.
/// The namespace for the `CommandRegistry` class statics.
type [<AllowNullLiteral>] CommandRegistryStatic =
    /// Construct a new command registry.
    [<Emit "new $0($1...)">] abstract Create: unit -> CommandRegistry

module CommandRegistry =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Parse a keystroke into its constituent components.</summary>
        /// <param name="keystroke">- The keystroke of interest.</param>
        abstract parseKeystroke: keystroke: string -> IKeystrokeParts
        /// <summary>Normalize a keystroke into a canonical representation.</summary>
        /// <param name="keystroke">- The keystroke of interest.</param>
        abstract normalizeKeystroke: keystroke: string -> string
        /// Format a keystroke for display on the local system.
        abstract formatKeystroke: keystroke: string -> string
        /// <summary>Create a normalized keystroke for a `'keydown'` event.</summary>
        /// <param name="event">- The event object for a `'keydown'` event.</param>
        abstract keystrokeForKeydownEvent: ``event``: KeyboardEvent -> string

    type [<AllowNullLiteral>] CommandFunc<'T> =
        [<Emit "$0($1...)">] abstract Invoke: args: ReadonlyJSONObject -> 'T

    type [<AllowNullLiteral>] Dataset =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string

    /// An options object for creating a command.
    /// 
    /// #### Notes
    /// A command is an abstract representation of code to be executed along
    /// with metadata for describing how the command should be displayed in
    /// a visual representation.
    /// 
    /// A command is a collection of functions, *not* methods. The command
    /// registry will always invoke the command functions with a `thisArg`
    /// which is `undefined`.
    type [<AllowNullLiteral>] ICommandOptions =
        /// The function to invoke when the command is executed.
        /// 
        /// #### Notes
        /// This should return the result of the command (if applicable) or
        /// a promise which yields the result. The result is resolved as a
        /// promise and that promise is returned to the code which executed
        /// the command.
        /// 
        /// This may be invoked even when `isEnabled` returns `false`.
        abstract execute: CommandFunc<U2<obj option, Promise<obj option>>> with get, set
        /// The label for the command.
        /// 
        /// #### Notes
        /// This can be a string literal, or a function which returns the
        /// label based on the provided command arguments.
        /// 
        /// The label is often used as the primary text for the command.
        /// 
        /// The default value is an empty string.
        abstract label: U2<string, CommandFunc<string>> option with get, set
        /// The index of the mnemonic character in the command's label.
        /// 
        /// #### Notes
        /// This can be an index literal, or a function which returns the
        /// mnemonic index based on the provided command arguments.
        /// 
        /// The mnemonic character is often used by menus to provide easy
        /// single-key keyboard access for triggering a menu item. It is
        /// typically rendered as an underlined character in the label.
        /// 
        /// The default value is `-1`.
        abstract mnemonic: U2<float, CommandFunc<float>> option with get, set
        abstract icon: U2<string, CommandFunc<string>> option with get, set
        /// The icon class for the command.
        /// 
        /// #### Notes
        /// This class name will be added to the icon node for the visual
        /// representation of the command.
        /// 
        /// Multiple class names can be separated with white space.
        /// 
        /// This can be a string literal, or a function which returns the
        /// icon based on the provided command arguments.
        /// 
        /// The default value is an empty string.
        abstract iconClass: U2<string, CommandFunc<string>> option with get, set
        /// The icon label for the command.
        /// 
        /// #### Notes
        /// This label will be added as text to the icon node for the visual
        /// representation of the command.
        /// 
        /// This can be a string literal, or a function which returns the
        /// label based on the provided command arguments.
        /// 
        /// The default value is an empty string.
        abstract iconLabel: U2<string, CommandFunc<string>> option with get, set
        /// The caption for the command.
        /// 
        /// #### Notes
        /// This should be a simple one line description of the command. It
        /// is used by some visual representations to show quick info about
        /// the command.
        /// 
        /// This can be a string literal, or a function which returns the
        /// caption based on the provided command arguments.
        /// 
        /// The default value is an empty string.
        abstract caption: U2<string, CommandFunc<string>> option with get, set
        /// The usage text for the command.
        /// 
        /// #### Notes
        /// This should be a full description of the command, which includes
        /// information about the structure of the arguments and the type of
        /// the return value. It is used by some visual representations when
        /// displaying complete help info about the command.
        /// 
        /// This can be a string literal, or a function which returns the
        /// usage text based on the provided command arguments.
        /// 
        /// The default value is an empty string.
        abstract usage: U2<string, CommandFunc<string>> option with get, set
        /// The general class name for the command.
        /// 
        /// #### Notes
        /// This class name will be added to the primary node for the visual
        /// representation of the command.
        /// 
        /// Multiple class names can be separated with white space.
        /// 
        /// This can be a string literal, or a function which returns the
        /// class name based on the provided command arguments.
        /// 
        /// The default value is an empty string.
        abstract className: U2<string, CommandFunc<string>> option with get, set
        /// The dataset for the command.
        /// 
        /// #### Notes
        /// The dataset values will be added to the primary node for the
        /// visual representation of the command.
        /// 
        /// This can be a dataset object, or a function which returns the
        /// dataset object based on the provided command arguments.
        /// 
        /// The default value is an empty dataset.
        abstract dataset: U2<Dataset, CommandFunc<Dataset>> option with get, set
        /// A function which indicates whether the command is enabled.
        /// 
        /// #### Notes
        /// Visual representations may use this value to display a disabled
        /// command as grayed-out or in some other non-interactive fashion.
        /// 
        /// The default value is `() => true`.
        abstract isEnabled: CommandFunc<bool> option with get, set
        /// A function which indicates whether the command is toggled.
        /// 
        /// #### Notes
        /// Visual representations may use this value to display a toggled
        /// command in a different form, such as a check mark icon for a
        /// menu item or a depressed state for a toggle button.
        /// 
        /// The default value is `() => false`.
        abstract isToggled: CommandFunc<bool> option with get, set
        /// A function which indicates whether the command is visible.
        /// 
        /// #### Notes
        /// Visual representations may use this value to hide or otherwise
        /// not display a non-visible command.
        /// 
        /// The default value is `() => true`.
        abstract isVisible: CommandFunc<bool> option with get, set

    /// An arguments object for the `commandChanged` signal.
    type [<AllowNullLiteral>] ICommandChangedArgs =
        /// The id of the associated command.
        /// 
        /// This will be `undefined` when the type is `'many-changed'`.
        abstract id: string option
        /// Whether the command was added, removed, or changed.
        abstract ``type``: U4<string, string, string, string>

    /// An arguments object for the `commandExecuted` signal.
    type [<AllowNullLiteral>] ICommandExecutedArgs =
        /// The id of the associated command.
        abstract id: string
        /// The arguments object passed to the command.
        abstract args: ReadonlyJSONObject
        /// The promise which resolves with the result of the command.
        abstract result: Promise<obj option>

    /// An options object for creating a key binding.
    type [<AllowNullLiteral>] IKeyBindingOptions =
        /// The default key sequence for the key binding.
        /// 
        /// A key sequence is composed of one or more keystrokes, where each
        /// keystroke is a combination of modifiers and a primary key.
        /// 
        /// Most key sequences will contain a single keystroke. Key sequences
        /// with multiple keystrokes are called "chords", and are useful for
        /// implementing modal input (ala Vim).
        /// 
        /// Each keystroke in the sequence should be of the form:
        ///    `[<modifier 1> [<modifier 2> [<modifier N> ]]]<primary key>`
        /// 
        /// The supported modifiers are: `Accel`, `Alt`, `Cmd`, `Ctrl`, and
        /// `Shift`. The `Accel` modifier is translated to `Cmd` on Mac and
        /// `Ctrl` on all other platforms. The `Cmd` modifier is ignored on
        /// non-Mac platforms.
        /// 
        /// Keystrokes are case sensitive.
        /// 
        /// **Examples:** `['Accel C']`, `['Shift F11']`, `['D', 'D']`
        abstract keys: ResizeArray<string> with get, set
        /// The CSS selector for the key binding.
        /// 
        /// The key binding will only be invoked when the selector matches a
        /// node on the propagation path of the keydown event. This allows
        /// the key binding to be restricted to user-defined contexts.
        /// 
        /// The selector must not contain commas.
        abstract selector: string with get, set
        /// The id of the command to execute when the binding is matched.
        abstract command: string with get, set
        /// The arguments for the command, if necessary.
        /// 
        /// The default value is an empty object.
        abstract args: ReadonlyJSONObject option with get, set
        /// The key sequence to use when running on Windows.
        /// 
        /// If provided, this will override `keys` on Windows platforms.
        abstract winKeys: ResizeArray<string> option with get, set
        /// The key sequence to use when running on Mac.
        /// 
        /// If provided, this will override `keys` on Mac platforms.
        abstract macKeys: ResizeArray<string> option with get, set
        /// The key sequence to use when running on Linux.
        /// 
        /// If provided, this will override `keys` on Linux platforms.
        abstract linuxKeys: ResizeArray<string> option with get, set

    /// An object which represents a key binding.
    /// 
    /// #### Notes
    /// A key binding is an immutable object created by a registry.
    type [<AllowNullLiteral>] IKeyBinding =
        /// The key sequence for the binding.
        abstract keys: ReadonlyArray<string>
        /// The CSS selector for the binding.
        abstract selector: string
        /// The command executed when the binding is matched.
        abstract command: string
        /// The arguments for the command.
        abstract args: ReadonlyJSONObject

    /// An arguments object for the `keyBindingChanged` signal.
    type [<AllowNullLiteral>] IKeyBindingChangedArgs =
        /// The key binding which was changed.
        abstract binding: IKeyBinding
        /// Whether the key binding was added or removed.
        abstract ``type``: U2<string, string>

    /// An object which holds the results of parsing a keystroke.
    type [<AllowNullLiteral>] IKeystrokeParts =
        /// Whether `'Cmd'` appears in the keystroke.
        abstract cmd: bool with get, set
        /// Whether `'Ctrl'` appears in the keystroke.
        abstract ctrl: bool with get, set
        /// Whether `'Alt'` appears in the keystroke.
        abstract alt: bool with get, set
        /// Whether `'Shift'` appears in the keystroke.
        abstract shift: bool with get, set
        /// The primary key for the keystroke.
        abstract key: string with get, set
