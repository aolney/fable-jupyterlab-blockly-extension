// ts2fable 0.0.0
module rec JupyterlabNotebook
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types
open Fable.React

//amo typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    [<Emit("$0[$1]{{=$2}}")>] 
    abstract Item: index: int -> 'T with get, set
    //abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
///Note this does not enforce readonly static typechecking
type ReadonlyArray<'T> = Array<'T>
type PromiseLike<'T> = Promise<'T>
//end typescript


module Actions =
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    type Cell = JupyterlabCells.Widget.Cell // __@jupyterlab_cells.Cell
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Notebook = Widget.Notebook

    type [<AllowNullLiteral>] IExports =
        abstract NotebookActions: NotebookActionsStatic

    /// A collection of actions that run against notebooks.
    /// 
    /// #### Notes
    /// All of the actions are a no-op if there is no model on the notebook.
    /// The actions set the widget `mode` to `'command'` unless otherwise specified.
    /// The actions will preserve the selection on the notebook widget unless
    /// otherwise specified.
    /// A namespace for `NotebookActions` static methods.
    type [<AllowNullLiteral>] NotebookActions =
        interface end

    /// A collection of actions that run against notebooks.
    /// 
    /// #### Notes
    /// All of the actions are a no-op if there is no model on the notebook.
    /// The actions set the widget `mode` to `'command'` unless otherwise specified.
    /// The actions will preserve the selection on the notebook widget unless
    /// otherwise specified.
    /// A namespace for `NotebookActions` static methods.
    type [<AllowNullLiteral>] NotebookActionsStatic =
        /// A signal that emits whenever a cell is run.
        abstract executed: ISignal<obj option, TypeLiteral_01>

    module NotebookActions =

        type [<AllowNullLiteral>] IExports =
            /// Split the active cell into two or more cells.
            abstract splitCell: notebook: Notebook -> unit
            /// <summary>Merge the selected cells.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget mode will be preserved.
            /// If only one cell is selected, the next cell will be selected.
            /// If the active cell is a code cell, its outputs will be cleared.
            /// This action can be undone.
            /// The final cell will have the same type as the active cell.
            /// If the active cell is a markdown cell, it will be unrendered.</param>
            abstract mergeCells: notebook: Notebook -> unit
            /// <summary>Delete the selected cells.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The cell after the last selected cell will be activated.
            /// It will add a code cell if all cells are deleted.
            /// This action can be undone.</param>
            abstract deleteCells: notebook: Notebook -> unit
            /// <summary>Insert a new code cell above the active cell.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget mode will be preserved.
            /// This action can be undone.
            /// The existing selection will be cleared.
            /// The new cell will the active cell.</param>
            abstract insertAbove: notebook: Notebook -> unit
            /// <summary>Insert a new code cell below the active cell.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget mode will be preserved.
            /// This action can be undone.
            /// The existing selection will be cleared.
            /// The new cell will be the active cell.</param>
            abstract insertBelow: notebook: Notebook -> unit
            /// <summary>Move the selected cell(s) down.</summary>
            /// <param name="notebook">= The target notebook widget.</param>
            abstract moveDown: notebook: Notebook -> unit
            /// Move the selected cell(s) up.
            abstract moveUp: notebook: Notebook -> unit
            /// <summary>Change the selected cell type(s).</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="value">- The target cell type.
            /// 
            /// #### Notes
            /// It should preserve the widget mode.
            /// This action can be undone.
            /// The existing selection will be cleared.
            /// Any cells converted to markdown will be unrendered.</param>
            abstract changeCellType: notebook: Notebook * value: JupyterlabCoreutils.Nbformat.Nbformat.CellType -> unit
            /// <summary>Run the selected cell(s).</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// The last selected cell will be activated, but not scrolled into view.
            /// The existing selection will be cleared.
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.</param>
            abstract run: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Run the selected cell(s) and advance to the next cell.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// The existing selection will be cleared.
            /// The cell after the last selected cell will be activated and scrolled into view.
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.
            /// If the last selected cell is the last cell, a new code cell
            /// will be created in `'edit'` mode.  The new cell creation can be undone.</param>
            abstract runAndAdvance: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Run the selected cell(s) and insert a new code cell.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.
            /// The widget mode will be set to `'edit'` after running.
            /// The existing selection will be cleared.
            /// The cell insert can be undone.
            /// The new cell will be scrolled into view.</param>
            abstract runAndInsert: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Run all of the cells in the notebook.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// The existing selection will be cleared.
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.
            /// The last cell in the notebook will be activated and scrolled into view.</param>
            abstract runAll: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            abstract renderAllMarkdown: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Run all of the cells before the currently active cell (exclusive).</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// The existing selection will be cleared.
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.
            /// The currently active cell will remain selected.</param>
            abstract runAllAbove: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Run all of the cells after the currently active cell (inclusive).</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="session">- The optional client session object.
            /// 
            /// #### Notes
            /// The existing selection will be cleared.
            /// An execution error will prevent the remaining code cells from executing.
            /// All markdown cells will be rendered.
            /// The last cell in the notebook will be activated and scrolled into view.</param>
            abstract runAllBelow: notebook: Notebook * ?session: IClientSession -> Promise<bool>
            /// <summary>Select the above the active cell.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget mode will be preserved.
            /// This is a no-op if the first cell is the active cell.
            /// This will skip any collapsed cells.
            /// The existing selection will be cleared.</param>
            abstract selectAbove: notebook: Notebook -> unit
            /// <summary>Select the cell below the active cell.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget mode will be preserved.
            /// This is a no-op if the last cell is the active cell.
            /// This will skip any collapsed cells.
            /// The existing selection will be cleared.</param>
            abstract selectBelow: notebook: Notebook -> unit
            /// <summary>Extend the selection to the cell above.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="toTop">- If true, denotes selection to extend to the top.
            /// 
            /// #### Notes
            /// This is a no-op if the first cell is the active cell.
            /// The new cell will be activated.</param>
            abstract extendSelectionAbove: notebook: Notebook * ?toTop: bool -> unit
            /// <summary>Extend the selection to the cell below.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="toBottom">- If true, denotes selection to extend to the bottom.
            /// 
            /// #### Notes
            /// This is a no-op if the last cell is the active cell.
            /// The new cell will be activated.</param>
            abstract extendSelectionBelow: notebook: Notebook * ?toBottom: bool -> unit
            /// <summary>Select all of the cells of the notebook.</summary>
            /// <param name="notebook">- the target notebook widget.</param>
            abstract selectAll: notebook: Notebook -> unit
            /// <summary>Deselect all of the cells of the notebook.</summary>
            /// <param name="notebook">- the targe notebook widget.</param>
            abstract deselectAll: notebook: Notebook -> unit
            /// <summary>Copy the selected cell data to a clipboard.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract copy: notebook: Notebook -> unit
            /// <summary>Cut the selected cell data to a clipboard.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// This action can be undone.
            /// A new code cell is added if all cells are cut.</param>
            abstract cut: notebook: Notebook -> unit
            /// <summary>Paste cells from the application clipboard.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="mode">- the mode of the paste operation: 'below' pastes cells
            /// below the active cell, 'above' pastes cells above the active cell,
            /// and 'replace' removes the currently selected cells and pastes cells
            /// in their place.
            /// 
            /// #### Notes
            /// The last pasted cell becomes the active cell.
            /// This is a no-op if there is no cell data on the clipboard.
            /// This action can be undone.</param>
            abstract paste: notebook: Notebook * ?mode: U3<string, string, string> -> unit
            /// <summary>Undo a cell action.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// This is a no-op if if there are no cell actions to undo.</param>
            abstract undo: notebook: Notebook -> unit
            /// <summary>Redo a cell action.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// This is a no-op if there are no cell actions to redo.</param>
            abstract redo: notebook: Notebook -> unit
            /// <summary>Toggle the line number of all cells.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The original state is based on the state of the active cell.
            /// The `mode` of the widget will be preserved.</param>
            abstract toggleAllLineNumbers: notebook: Notebook -> unit
            /// <summary>Toggle whether to record cell timing execution.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract toggleRecordTiming: notebook: Notebook -> unit
            /// <summary>Clear the code outputs of the selected cells.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget `mode` will be preserved.</param>
            abstract clearOutputs: notebook: Notebook -> unit
            /// <summary>Clear all the code outputs on the widget.</summary>
            /// <param name="notebook">- The target notebook widget.
            /// 
            /// #### Notes
            /// The widget `mode` will be preserved.</param>
            abstract clearAllOutputs: notebook: Notebook -> unit
            /// <summary>Hide the code on selected code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract hideCode: notebook: Notebook -> unit
            /// <summary>Show the code on selected code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract showCode: notebook: Notebook -> unit
            /// <summary>Hide the code on all code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract hideAllCode: notebook: Notebook -> unit
            /// Show the code on all code cells.
            abstract showAllCode: notebook: Notebook -> unit
            /// <summary>Hide the output on selected code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract hideOutput: notebook: Notebook -> unit
            /// <summary>Show the output on selected code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract showOutput: notebook: Notebook -> unit
            /// <summary>Hide the output on all code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract hideAllOutputs: notebook: Notebook -> unit
            /// <summary>Show the output on all code cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract showAllOutputs: notebook: Notebook -> unit
            /// <summary>Enable output scrolling for all selected cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract enableOutputScrolling: notebook: Notebook -> unit
            /// <summary>Disable output scrolling for all selected cells.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract disableOutputScrolling: notebook: Notebook -> unit
            /// <summary>Set the markdown header level.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            /// <param name="level">- The header level.
            /// 
            /// #### Notes
            /// All selected cells will be switched to markdown.
            /// The level will be clamped between 1 and 6.
            /// If there is an existing header, it will be replaced.
            /// There will always be one blank space after the header.
            /// The cells will be unrendered.</param>
            abstract setMarkdownHeader: notebook: Notebook * level: float -> unit
            /// <summary>Trust the notebook after prompting the user.</summary>
            /// <param name="notebook">- The target notebook widget.</param>
            abstract trust: notebook: Notebook -> Promise<unit>

    type [<AllowNullLiteral>] TypeLiteral_01 =
        abstract notebook: Notebook with get, set
        abstract cell: Cell with get, set

module Celllist =
    type IIterator<'T> = PhosphorAlgorithm.Iter.IIterator<'T> // __@phosphor_algorithm.IIterator
    type IterableOrArrayLike<'T> = PhosphorAlgorithm.Iter.IterableOrArrayLike<'T> // __@phosphor_algorithm.IterableOrArrayLike
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U>  //__@phosphor_signaling.ISignal
    type ICellModel = JupyterlabCells.Widget.ICellModel // __@jupyterlab_cells.ICellModel
    type IObservableList<'T> = JupyterlabObservables.Observablelist.IObservableList<'T> // __@jupyterlab_observables.IObservableList
    type IObservableUndoableList<'T> =  JupyterlabObservables.Undoablelist.IObservableUndoableList<'T>  //__@jupyterlab_observables.IObservableUndoableList
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    type NotebookModel = Model.NotebookModel

    type [<AllowNullLiteral>] IExports =
        abstract CellList: CellListStatic

    /// A cell list object that supports undo/redo.
    type [<AllowNullLiteral>] CellList =
        inherit IObservableUndoableList<ICellModel>
        abstract ``type``: string with get, set
        /// A signal emitted when the cell list has changed.
        abstract changed: ISignal<CellList, JupyterlabObservables.Observablelist.IObservableList.IChangedArgs<ICellModel>>
        /// Test whether the cell list has been disposed.
        abstract isDisposed: bool
        /// Test whether the list is empty.
        abstract isEmpty: bool
        /// Get the length of the cell list.
        abstract length: float
        /// Create an iterator over the cells in the cell list.
        abstract iter: unit -> IIterator<ICellModel>
        /// Dispose of the resources held by the cell list.
        abstract dispose: unit -> unit
        /// <summary>Get the cell at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        abstract get: index: float -> ICellModel
        /// <summary>Set the cell at the specified index.</summary>
        /// <param name="index">- The positive integer index of interest.</param>
        /// <param name="cell">- The cell to set at the specified index.
        /// 
        /// #### Complexity
        /// Constant.
        /// 
        /// #### Iterator Validity
        /// No changes.
        /// 
        /// #### Undefined Behavior
        /// An `index` which is non-integral or out of range.
        /// 
        /// #### Notes
        /// This should be considered to transfer ownership of the
        /// cell to the `CellList`. As such, `cell.dispose()` should
        /// not be called by other actors.</param>
        abstract set: index: float * cell: ICellModel -> unit
        /// <summary>Add a cell to the back of the cell list.</summary>
        /// <param name="cell">- The cell to add to the back of the cell list.</param>
        abstract push: cell: ICellModel -> float
        /// <summary>Insert a cell into the cell list at a specific index.</summary>
        /// <param name="index">- The index at which to insert the cell.</param>
        /// <param name="cell">- The cell to set at the specified index.</param>
        abstract insert: index: float * cell: ICellModel -> unit
        /// <summary>Remove the first occurrence of a cell from the cell list.</summary>
        /// <param name="cell">- The cell of interest.</param>
        abstract removeValue: cell: ICellModel -> float
        /// <summary>Remove and return the cell at a specific index.</summary>
        /// <param name="index">- The index of the cell of interest.</param>
        abstract remove: index: float -> ICellModel
        /// Remove all cells from the cell list.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Iterator Validity
        /// All current iterators are invalidated.
        abstract clear: unit -> unit
        /// <summary>Move a cell from one index to another.</summary>
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
        /// <summary>Push a set of cells to the back of the cell list.</summary>
        /// <param name="cells">- An iterable or array-like set of cells to add.</param>
        abstract pushAll: cells: IterableOrArrayLike<ICellModel> -> float
        /// <summary>Insert a set of items into the cell list at the specified index.</summary>
        /// <param name="index">- The index at which to insert the cells.</param>
        /// <param name="cells">- The cells to insert at the specified index.</param>
        abstract insertAll: index: float * cells: IterableOrArrayLike<ICellModel> -> float
        /// <summary>Remove a range of items from the cell list.</summary>
        /// <param name="startIndex">- The start index of the range to remove (inclusive).</param>
        /// <param name="endIndex">- The end index of the range to remove (exclusive).</param>
        abstract removeRange: startIndex: float * endIndex: float -> float
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

    /// A cell list object that supports undo/redo.
    type [<AllowNullLiteral>] CellListStatic =
        /// Construct the cell list.
        [<Emit "new $0($1...)">] abstract Create: modelDB: IModelDB * factory: Model.NotebookModel.IContentFactory -> CellList

module Default_toolbar =
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry // __@jupyterlab_docregistry.DocumentRegistry
    type Widget = PhosphorWidgets.Widget //__@phosphor_widgets.Widget
    type ReactWidget = JupyterlabApputils.ReactWidget // __@jupyterlab_apputils.ReactWidget
    type NotebookPanel = Panel.NotebookPanel
    type Notebook = Widget.Notebook
    //typescript hack
    type ChangeEvent = Event

    type [<AllowNullLiteral>] IExports =
        abstract CellTypeSwitcher: CellTypeSwitcherStatic

    module ToolbarItems =

        type [<AllowNullLiteral>] IExports =
            /// Create save button toolbar item.
            abstract createSaveButton: panel: NotebookPanel -> Widget
            /// Create an insert toolbar item.
            abstract createInsertButton: panel: NotebookPanel -> Widget
            /// Create a cut toolbar item.
            abstract createCutButton: panel: NotebookPanel -> Widget
            /// Create a copy toolbar item.
            abstract createCopyButton: panel: NotebookPanel -> Widget
            /// Create a paste toolbar item.
            abstract createPasteButton: panel: NotebookPanel -> Widget
            /// Create a run toolbar item.
            abstract createRunButton: panel: NotebookPanel -> Widget
            /// Create a cell type switcher item.
            /// 
            /// #### Notes
            /// It will display the type of the current active cell.
            /// If more than one cell is selected but are of different types,
            /// it will display `'-'`.
            /// When the user changes the cell type, it will change the
            /// cell types of the selected cells.
            /// It can handle a change to the context.
            abstract createCellTypeItem: panel: NotebookPanel -> Widget
            /// Get the default toolbar items for panel
            abstract getDefaultItems: panel: NotebookPanel -> ResizeArray<JupyterlabDocregistry.Registry.DocumentRegistry.IToolbarItem>

    /// A toolbar widget that switches cell types.
    type [<AllowNullLiteral>] CellTypeSwitcher =
        inherit ReactWidget
        /// Handle `change` events for the HTMLSelect component.
        // abstract handleChange: (React.ChangeEvent<HTMLSelectElement> -> unit) with get, set
        abstract handleChange: (ChangeEvent -> unit) with get, set
        /// Handle `keydown` events for the HTMLSelect component.
        // abstract handleKeyDown: (React.KeyboardEvent<Element> -> unit) with get, set
        abstract handleKeyDown: (KeyboardEvent -> unit) with get, set
        abstract render: unit -> obj // JSX.Element

    /// A toolbar widget that switches cell types.
    type [<AllowNullLiteral>] CellTypeSwitcherStatic =
        /// Construct a new cell type switcher.
        [<Emit "new $0($1...)">] abstract Create: widget: Notebook -> CellTypeSwitcher

module Model =
    type DocumentModel = JupyterlabDocregistry.Default.DocumentModel // __@jupyterlab_docregistry.DocumentModel
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry //__@jupyterlab_docregistry.DocumentRegistry
    type ICellModel = JupyterlabCells.Widget.ICellModel // __@jupyterlab_cells.ICellModel
    type ICodeCellModel = JupyterlabCells.Model.ICodeCellModel // __@jupyterlab_cells.ICodeCellModel
    type IRawCellModel = JupyterlabCells.Model.IRawCellModel // __@jupyterlab_cells.IRawCellModel
    type IMarkdownCellModel = JupyterlabCells.Model.IMarkdownCellModel //__@jupyterlab_cells.IMarkdownCellModel
    type CodeCellModel = JupyterlabCells.Model.CodeCellModel //__@jupyterlab_cells.CodeCellModel
    type CellModel = JupyterlabCells.Model.CellModel // __@jupyterlab_cells.CellModel
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    type IObservableJSON = JupyterlabObservables.Observablejson.IObservableJSON // __@jupyterlab_observables.IObservableJSON
    type IObservableUndoableList<'T> = JupyterlabObservables.Undoablelist.IObservableUndoableList<'T> // __@jupyterlab_observables.IObservableUndoableList
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB

    type [<AllowNullLiteral>] IExports =
        abstract NotebookModel: NotebookModelStatic

    /// The definition of a model object for a notebook widget.
    type [<AllowNullLiteral>] INotebookModel =
        inherit JupyterlabDocregistry.Registry.DocumentRegistry.IModel
        /// The list of cells in the notebook.
        abstract cells: IObservableUndoableList<ICellModel>
        /// The cell model factory for the notebook.
        abstract contentFactory: NotebookModel.IContentFactory
        /// The major version number of the nbformat.
        abstract nbformat: float
        /// The minor version number of the nbformat.
        abstract nbformatMinor: float
        /// The metadata associated with the notebook.
        abstract metadata: IObservableJSON
        /// The array of deleted cells since the notebook was last run.
        abstract deletedCells: ResizeArray<string>

    /// An implementation of a notebook Model.
    /// The namespace for the `NotebookModel` class statics.
    type [<AllowNullLiteral>] NotebookModel =
        inherit DocumentModel
        inherit INotebookModel
        /// The cell model factory for the notebook.
        abstract contentFactory: NotebookModel.IContentFactory
        /// The metadata associated with the notebook.
        abstract metadata: IObservableJSON
        /// Get the observable list of notebook cells.
        abstract cells: IObservableUndoableList<ICellModel>
        /// The major version number of the nbformat.
        abstract nbformat: float
        /// The minor version number of the nbformat.
        abstract nbformatMinor: float
        /// The default kernel name of the document.
        abstract defaultKernelName: string
        /// A list of deleted cells for the notebook..
        abstract deletedCells: ResizeArray<string>
        /// The default kernel language of the document.
        abstract defaultKernelLanguage: string
        /// Dispose of the resources held by the model.
        abstract dispose: unit -> unit
        /// Serialize the model to a string.
        abstract toString: unit -> string
        /// Deserialize the model from a string.
        /// 
        /// #### Notes
        /// Should emit a [contentChanged] signal.
        abstract fromString: value: string -> unit
        /// Serialize the model to JSON.
        abstract toJSON: unit -> JupyterlabCoreutils.Nbformat.Nbformat.INotebookContent
        /// Deserialize the model from JSON.
        /// 
        /// #### Notes
        /// Should emit a [contentChanged] signal.
        abstract fromJSON: value: JupyterlabCoreutils.Nbformat.Nbformat.INotebookContent -> unit
        /// Initialize the model with its current state.
        abstract initialize: unit -> unit

    /// An implementation of a notebook Model.
    /// The namespace for the `NotebookModel` class statics.
    type [<AllowNullLiteral>] NotebookModelStatic =
        /// Construct a new notebook model.
        [<Emit "new $0($1...)">] abstract Create: ?options: NotebookModel.IOptions -> NotebookModel

    module NotebookModel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory

        /// An options object for initializing a notebook model.
        type [<AllowNullLiteral>] IOptions =
            /// The language preference for the model.
            abstract languagePreference: string option with get, set
            /// A factory for creating cell models.
            /// 
            /// The default is a shared factory instance.
            abstract contentFactory: IContentFactory option with get, set
            /// A modelDB for storing notebook data.
            abstract modelDB: IModelDB option with get, set

        /// A factory for creating notebook model content.
        type [<AllowNullLiteral>] IContentFactory =
            /// The factory for output area models.
            abstract codeCellContentFactory: JupyterlabCells.Model.CodeCellModel.IContentFactory
            /// The IModelDB in which to put data for the notebook model.
            abstract modelDB: IModelDB with get, set
            /// <summary>Create a new cell by cell type.</summary>
            /// <param name="type">:  the type of the cell to create.</param>
            abstract createCell: ``type``: JupyterlabCoreutils.Nbformat.Nbformat.CellType * opts: JupyterlabCells.Model.CellModel.IOptions -> ICellModel
            /// <summary>Create a new code cell.</summary>
            /// <param name="options">- The options used to create the cell.</param>
            abstract createCodeCell: options: JupyterlabCells.Model.CodeCellModel.IOptions -> ICodeCellModel
            /// <summary>Create a new markdown cell.</summary>
            /// <param name="options">- The options used to create the cell.</param>
            abstract createMarkdownCell: options: JupyterlabCells.Model.CellModel.IOptions -> IMarkdownCellModel
            /// <summary>Create a new raw cell.</summary>
            /// <param name="options">- The options used to create the cell.</param>
            abstract createRawCell: options: JupyterlabCells.Model.CellModel.IOptions -> IRawCellModel
            /// Clone the content factory with a new IModelDB.
            abstract clone: modelDB: IModelDB -> IContentFactory

        /// The default implementation of an `IContentFactory`.
        /// A namespace for the notebook model content factory.
        type [<AllowNullLiteral>] ContentFactory =
            /// The factory for code cell content.
            abstract codeCellContentFactory: JupyterlabCells.Model.CodeCellModel.IContentFactory
            /// The IModelDB in which to put the notebook data.
            abstract modelDB: IModelDB option
            /// <summary>Create a new cell by cell type.</summary>
            /// <param name="type">:  the type of the cell to create.</param>
            abstract createCell: ``type``: JupyterlabCoreutils.Nbformat.Nbformat.CellType * opts: JupyterlabCells.Model.CellModel.IOptions -> ICellModel
            /// Create a new code cell.
            abstract createCodeCell: options: JupyterlabCells.Model.CodeCellModel.IOptions -> ICodeCellModel
            /// Create a new markdown cell.
            abstract createMarkdownCell: options: JupyterlabCells.Model.CellModel.IOptions -> IMarkdownCellModel
            /// Create a new raw cell.
            abstract createRawCell: options: JupyterlabCells.Model.CellModel.IOptions -> IRawCellModel
            /// Clone the content factory with a new IModelDB.
            abstract clone: modelDB: IModelDB -> ContentFactory

        /// The default implementation of an `IContentFactory`.
        /// A namespace for the notebook model content factory.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            /// Create a new cell model factory.
            [<Emit "new $0($1...)">] abstract Create: options: ContentFactory.IOptions -> ContentFactory

        module ContentFactory =

            /// The options used to initialize a `ContentFactory`.
            type [<AllowNullLiteral>] IOptions =
                /// The factory for code cell model content.
                abstract codeCellContentFactory: JupyterlabCells.Model.CodeCellModel.IContentFactory option with get, set
                /// The modelDB in which to place new content.
                abstract modelDB: IModelDB option with get, set

module Modelfactory =
    type CodeCellModel = JupyterlabCells.Model.CodeCellModel // __@jupyterlab_cells.CodeCellModel
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry //  __@jupyterlab_docregistry.DocumentRegistry
    type IModelDB = JupyterlabObservables.Modeldb.IModelDB // __@jupyterlab_observables.IModelDB
    // type Contents = JupyterlabServices.__contents_index.Contents // __@jupyterlab_services.Contents
    type INotebookModel = Model.INotebookModel
    type NotebookModel = Model.NotebookModel

    type [<AllowNullLiteral>] IExports =
        abstract NotebookModelFactory: NotebookModelFactoryStatic

    /// A model factory for notebooks.
    /// The namespace for notebook model factory statics.
    type [<AllowNullLiteral>] NotebookModelFactory =
        inherit JupyterlabDocregistry.Registry.DocumentRegistry.IModelFactory<INotebookModel>
        /// The content model factory used by the NotebookModelFactory.
        abstract contentFactory: Model.NotebookModel.IContentFactory
        /// The name of the model.
        abstract name: string
        /// The content type of the file.
        abstract contentType: JupyterlabServices.__contents_index.Contents.ContentType
        /// The format of the file.
        abstract fileFormat: JupyterlabServices.__contents_index.Contents.FileFormat
        /// Get whether the model factory has been disposed.
        abstract isDisposed: bool
        /// Dispose of the model factory.
        abstract dispose: unit -> unit
        /// <summary>Create a new model for a given path.</summary>
        /// <param name="languagePreference">- An optional kernel language preference.</param>
        abstract createNew: ?languagePreference: string * ?modelDB: IModelDB -> INotebookModel
        /// Get the preferred kernel language given a path.
        abstract preferredLanguage: path: string -> string

    /// A model factory for notebooks.
    /// The namespace for notebook model factory statics.
    type [<AllowNullLiteral>] NotebookModelFactoryStatic =
        /// Construct a new notebook model factory.
        [<Emit "new $0($1...)">] abstract Create: options: NotebookModelFactory.IOptions -> NotebookModelFactory

    module NotebookModelFactory =

        /// The options used to initialize a NotebookModelFactory.
        type [<AllowNullLiteral>] IOptions =
            /// The factory for code cell content.
            abstract codeCellContentFactory: JupyterlabCells.Model.CodeCellModel.IContentFactory option with get, set
            /// The content factory used by the NotebookModelFactory.  If
            /// given, it will supersede the `codeCellContentFactory`.
            abstract contentFactory: Model.NotebookModel.IContentFactory option with get, set

module Modestatus =
    type VDomRenderer<'T> = JupyterlabApputils.VDomRenderer<'T> // __@jupyterlab_apputils.VDomRenderer
    type VDomModel = JupyterlabApputils.VDomModel // __@jupyterlab_apputils.VDomModel
    type Notebook = Widget.Notebook
    type NotebookMode = Widget.NotebookMode

    type [<AllowNullLiteral>] IExports =
        abstract CommandEditStatus: CommandEditStatusStatic

    /// StatusBar item to display which notebook mode user is in.
    /// A namespace for CommandEdit statics.
    type [<AllowNullLiteral>] CommandEditStatus =
        inherit VDomRenderer<CommandEditStatus.Model>
        /// Render the CommandEdit status item.
        abstract render: unit -> obj // JSX.Element

    /// StatusBar item to display which notebook mode user is in.
    /// A namespace for CommandEdit statics.
    type [<AllowNullLiteral>] CommandEditStatusStatic =
        /// Construct a new CommandEdit status item.
        [<Emit "new $0($1...)">] abstract Create: unit -> CommandEditStatus

    module CommandEditStatus =

        type [<AllowNullLiteral>] IExports =
            abstract Model: ModelStatic

        /// A VDomModle for the CommandEdit renderer.
        type [<AllowNullLiteral>] Model =
            inherit VDomModel
            /// The current mode of the current notebook.
            abstract notebookMode: NotebookMode
            /// Set the current notebook for the model.
            abstract notebook: Notebook option with get, set

        /// A VDomModle for the CommandEdit renderer.
        type [<AllowNullLiteral>] ModelStatic =
            /// Construct a new CommandEdit status item.
            [<Emit "new $0($1...)">] abstract Create: unit -> Model

module Notebooktools =
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type ConflatableMessage = PhosphorMessaging.ConflatableMessage //__@phosphor_messaging.ConflatableMessage
    type Message = PhosphorMessaging.Message // __@phosphor_messaging.Message
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type Cell = JupyterlabCells.Widget.Cell // __@jupyterlab_cells.Cell
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    type JSONEditor = JupyterlabCodeeditor.Jsoneditor.JSONEditor // __@jupyterlab_codeeditor.JSONEditor
    // type nbformat = JupyterlabCoreutils.Nbformat.Nbformat // __@jupyterlab_coreutils.nbformat
    type ObservableJSON = JupyterlabObservables.Observablejson.ObservableJSON // __@jupyterlab_observables.ObservableJSON
    type NotebookPanel = Panel.NotebookPanel
    type INotebookTools = Tokens.INotebookTools
    type INotebookTracker = Tokens.INotebookTracker

    type [<AllowNullLiteral>] IExports =
        abstract NotebookTools: NotebookToolsStatic

    /// A widget that provides metadata tools.
    /// The namespace for NotebookTools class statics.
    type [<AllowNullLiteral>] NotebookTools =
        inherit Widget
        inherit INotebookTools
        /// The active cell widget.
        abstract activeCell: Cell option
        /// The currently selected cells.
        abstract selectedCells: ResizeArray<Cell>
        /// The current notebook.
        abstract activeNotebookPanel: NotebookPanel option
        /// Add a cell tool item.
        abstract addItem: options: NotebookTools.IAddOptions -> unit

    /// A widget that provides metadata tools.
    /// The namespace for NotebookTools class statics.
    type [<AllowNullLiteral>] NotebookToolsStatic =
        /// Construct a new NotebookTools object.
        [<Emit "new $0($1...)">] abstract Create: options: NotebookTools.IOptions -> NotebookTools

    module NotebookTools =

        type [<AllowNullLiteral>] IExports =
            abstract ActiveNotebookPanelMessage: ConflatableMessage
            abstract ActiveCellMessage: ConflatableMessage
            abstract SelectionMessage: ConflatableMessage
            abstract Tool: ToolStatic
            abstract ActiveCellTool: ActiveCellToolStatic
            abstract MetadataEditorTool: MetadataEditorToolStatic
            abstract NotebookMetadataEditorTool: NotebookMetadataEditorToolStatic
            abstract CellMetadataEditorTool: CellMetadataEditorToolStatic
            abstract KeySelector: KeySelectorStatic
            /// Create a slideshow selector.
            abstract createSlideShowSelector: unit -> KeySelector
            /// Create an nbconvert selector.
            abstract createNBConvertSelector: optionsMap: CreateNBConvertSelectorOptionsMap -> KeySelector

        type [<AllowNullLiteral>] CreateNBConvertSelectorOptionsMap =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> JSONValue with get, set

        /// The options used to create a NotebookTools object.
        type [<AllowNullLiteral>] IOptions =
            /// The notebook tracker used by the notebook tools.
            abstract tracker: INotebookTracker with get, set

        /// The options used to add an item to the notebook tools.
        type [<AllowNullLiteral>] IAddOptions =
            /// The tool to add to the notebook tools area.
            abstract tool: Tool with get, set
            /// The section to which the tool should be added.
            abstract section: U2<string, string> option with get, set
            /// The rank order of the widget among its siblings.
            abstract rank: float option with get, set

        /// The base notebook tool, meant to be subclassed.
        type [<AllowNullLiteral>] Tool =
            inherit Widget
            inherit Tokens.INotebookTools.ITool
            /// The notebook tools object.
            abstract notebookTools: INotebookTools with get, set
            abstract dispose: unit -> unit
            /// <summary>Process a message sent to the widget.</summary>
            /// <param name="msg">- The message sent to the widget.</param>
            abstract processMessage: msg: Message -> unit
            /// Handle a change to the notebook panel.
            /// 
            /// #### Notes
            /// The default implementation is a no-op.
            abstract onActiveNotebookPanelChanged: msg: Message -> unit
            /// Handle a change to the active cell.
            /// 
            /// #### Notes
            /// The default implementation is a no-op.
            abstract onActiveCellChanged: msg: Message -> unit
            /// Handle a change to the selection.
            /// 
            /// #### Notes
            /// The default implementation is a no-op.
            abstract onSelectionChanged: msg: Message -> unit
            /// Handle a change to the metadata of the active cell.
            /// 
            /// #### Notes
            /// The default implementation is a no-op.
            abstract onActiveCellMetadataChanged: msg: JupyterlabObservables.Observablejson.ObservableJSON.ChangeMessage -> unit
            /// Handle a change to the metadata of the active cell.
            /// 
            /// #### Notes
            /// The default implementation is a no-op.
            abstract onActiveNotebookPanelMetadataChanged: msg: JupyterlabObservables.Observablejson.ObservableJSON.ChangeMessage -> unit

        /// The base notebook tool, meant to be subclassed.
        type [<AllowNullLiteral>] ToolStatic =
            /// Construct a new KeySelector.
            [<Emit "new $0($1...)">] abstract Create: options: KeySelector.IOptions -> Tool

        /// A cell tool displaying the active cell contents.
        type [<AllowNullLiteral>] ActiveCellTool =
            inherit Tool
            /// Dispose of the resources used by the tool.
            abstract dispose: unit -> unit
            /// Handle a change to the active cell.
            abstract onActiveCellChanged: unit -> unit

        /// A cell tool displaying the active cell contents.
        type [<AllowNullLiteral>] ActiveCellToolStatic =
            /// Construct a new active cell tool.
            [<Emit "new $0($1...)">] abstract Create: unit -> ActiveCellTool

        /// A raw metadata editor.
        /// The namespace for `MetadataEditorTool` static data.
        type [<AllowNullLiteral>] MetadataEditorTool =
            inherit Tool
            /// The editor used by the tool.
            abstract editor: JSONEditor

        /// A raw metadata editor.
        /// The namespace for `MetadataEditorTool` static data.
        type [<AllowNullLiteral>] MetadataEditorToolStatic =
            /// Construct a new raw metadata tool.
            [<Emit "new $0($1...)">] abstract Create: options: MetadataEditorTool.IOptions -> MetadataEditorTool

        module MetadataEditorTool =

            /// The options used to initialize a metadata editor tool.
            type [<AllowNullLiteral>] IOptions =
                /// The editor factory used by the tool.
                abstract editorFactory: JupyterlabCodeeditor.Editor.CodeEditor.Factory with get, set
                /// The label for the JSON editor
                abstract label: string option with get, set
                /// Initial collapse state, defaults to true.
                abstract collapsed: bool option with get, set

        /// A notebook metadata editor
        type [<AllowNullLiteral>] NotebookMetadataEditorTool =
            inherit MetadataEditorTool
            /// Handle a change to the notebook.
            abstract onActiveNotebookPanelChanged: msg: Message -> unit
            /// Handle a change to the notebook metadata.
            abstract onActiveNotebookPanelMetadataChanged: msg: Message -> unit

        /// A notebook metadata editor
        type [<AllowNullLiteral>] NotebookMetadataEditorToolStatic =
            [<Emit "new $0($1...)">] abstract Create: options: MetadataEditorTool.IOptions -> NotebookMetadataEditorTool

        /// A cell metadata editor
        type [<AllowNullLiteral>] CellMetadataEditorTool =
            inherit MetadataEditorTool
            /// Handle a change to the active cell.
            abstract onActiveCellChanged: msg: Message -> unit
            /// Handle a change to the active cell metadata.
            abstract onActiveCellMetadataChanged: msg: Message -> unit

        /// A cell metadata editor
        type [<AllowNullLiteral>] CellMetadataEditorToolStatic =
            [<Emit "new $0($1...)">] abstract Create: options: MetadataEditorTool.IOptions -> CellMetadataEditorTool

        /// A cell tool that provides a selection for a given metadata key.
        /// The namespace for `KeySelector` static data.
        type [<AllowNullLiteral>] KeySelector =
            inherit Tool
            /// The metadata key used by the selector.
            abstract key: string
            /// The select node for the widget.
            abstract selectNode: HTMLSelectElement
            /// <summary>Handle the DOM events for the widget.</summary>
            /// <param name="event">- The DOM event sent to the widget.
            /// 
            /// #### Notes
            /// This method implements the DOM `EventListener` interface and is
            /// called in response to events on the notebook panel's node. It should
            /// not be called directly by user code.</param>
            abstract handleEvent: ``event``: Event -> unit
            /// Handle `after-attach` messages for the widget.
            abstract onAfterAttach: msg: Message -> unit
            /// Handle `before-detach` messages for the widget.
            abstract onBeforeDetach: msg: Message -> unit
            /// Handle a change to the active cell.
            abstract onActiveCellChanged: msg: Message -> unit
            /// Handle a change to the metadata of the active cell.
            abstract onActiveCellMetadataChanged: msg: JupyterlabObservables.Observablejson.ObservableJSON.ChangeMessage -> unit
            /// Handle a change to the value.
            abstract onValueChanged: unit -> unit

        /// A cell tool that provides a selection for a given metadata key.
        /// The namespace for `KeySelector` static data.
        type [<AllowNullLiteral>] KeySelectorStatic =
            /// Construct a new KeySelector.
            [<Emit "new $0($1...)">] abstract Create: options: KeySelector.IOptions -> KeySelector

        module KeySelector =

            /// The options used to initialize a keyselector.
            type [<AllowNullLiteral>] IOptions =
                /// The metadata key of interest.
                abstract key: string with get, set
                /// The map of options to values.
                /// 
                /// #### Notes
                /// If a value equals the default, choosing it may erase the key from the
                /// metadata.
                abstract optionsMap: TypeLiteral_01 with get, set
                /// The optional title of the selector - defaults to capitalized `key`.
                abstract title: string option with get, set
                /// The optional valid cell types - defaults to all valid types.
                abstract validCellTypes: ResizeArray<JupyterlabCoreutils.Nbformat.Nbformat.CellType> option with get, set
                /// An optional value getter for the selector.
                abstract getter: (Cell -> JSONValue) option with get, set
                /// An optional value setter for the selector.
                abstract setter: (Cell -> JSONValue -> unit) option with get, set
                /// Default value for default setters and getters if value is not found.
                abstract ``default``: JSONValue option with get, set

            type [<AllowNullLiteral>] TypeLiteral_01 =
                [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> JSONValue with get, set

module Panel =
    type Token<'T> = PhosphorCoreutils.Token<'T> // __@phosphor_coreutils.Token
    type Message = PhosphorMessaging.Message // type Message = __@phosphor_messaging.Message
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type IClientSession = JupyterlabApputils.IClientSession // __@jupyterlab_apputils.IClientSession
    // type Printing = JupyterlabApputils.Printing // __@jupyterlab_apputils.Printing
    type DocumentWidget = JupyterlabDocregistry.Default.DocumentWidget // __@jupyterlab_docregistry.DocumentWidget
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry // __@jupyterlab_docregistry.DocumentRegistry
    type INotebookModel = Model.INotebookModel
    type Notebook = Widget.Notebook
    type StaticNotebook = Widget.StaticNotebook

    type [<AllowNullLiteral>] IExports =
        abstract NotebookPanel: NotebookPanelStatic

    /// A widget that hosts a notebook toolbar and content area.
    /// 
    /// #### Notes
    /// The widget keeps the document metadata in sync with the current
    /// kernel on the context.
    /// A namespace for `NotebookPanel` statics.
    type [<AllowNullLiteral>] NotebookPanel =
        inherit JupyterlabDocregistry.Default.DocumentWidget<Notebook, INotebookModel>
        abstract _onSave: sender: JupyterlabDocregistry.Registry.DocumentRegistry.Context * state: JupyterlabDocregistry.Registry.DocumentRegistry.SaveState -> unit
        /// A signal emitted when the panel has been activated.
        abstract activated: ISignal<NotebookPanel, unit>
        /// The client session used by the panel.
        abstract session: IClientSession
        /// The notebook used by the widget.
        abstract content: Notebook
        /// The model for the widget.
        abstract model: INotebookModel
        /// <summary>Update the options for the current notebook panel.</summary>
        /// <param name="config">new options to set</param>
        abstract setConfig: config: NotebookPanel.IConfig -> unit
        /// Set URI fragment identifier.
        abstract setFragment: fragment: string -> unit
        /// Dispose of the resources used by the widget.
        abstract dispose: unit -> unit
        /// Handle `'activate-request'` messages.
        abstract onActivateRequest: msg: Message -> unit
        /// Prints the notebook by converting to HTML with nbconvert.
        abstract ``[Printing.symbol]``: unit -> (unit -> Promise<unit>)

    /// A widget that hosts a notebook toolbar and content area.
    /// 
    /// #### Notes
    /// The widget keeps the document metadata in sync with the current
    /// kernel on the context.
    /// A namespace for `NotebookPanel` statics.
    type [<AllowNullLiteral>] NotebookPanelStatic =
        /// Construct a new notebook panel.
        [<Emit "new $0($1...)">] abstract Create: options: JupyterlabDocregistry.Default.DocumentWidget.IOptions<Notebook, INotebookModel> -> NotebookPanel

    module NotebookPanel =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: ContentFactory
            abstract IContentFactory: Token<IContentFactory>

        /// Notebook config interface for NotebookPanel
        type [<AllowNullLiteral>] IConfig =
            /// A config object for cell editors
            abstract editorConfig: Widget.StaticNotebook.IEditorConfig with get, set
            /// A config object for notebook widget
            abstract notebookConfig: Widget.StaticNotebook.INotebookConfig with get, set
            /// Whether to shut down the kernel when closing the panel or not
            abstract kernelShutdown: bool with get, set

        /// A content factory interface for NotebookPanel.
        /// The notebook renderer token.
        type [<AllowNullLiteral>] IContentFactory =
            inherit Widget.Notebook.IContentFactory
            /// Create a new content area for the panel.
            abstract createNotebook: options: Widget.Notebook.IOptions -> Notebook

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactory =
            inherit Widget.Notebook.ContentFactory
            inherit IContentFactory
            /// Create a new content area for the panel.
            abstract createNotebook: options: Widget.Notebook.IOptions -> Notebook

        /// The default implementation of an `IContentFactory`.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

module Tokens =
    type IWidgetTracker = JupyterlabApputils.IWidgetTracker // __@jupyterlab_apputils.IWidgetTracker
    type Cell = JupyterlabCells.Widget.Cell // __@jupyterlab_cells.Cell
    type Token<'T> = PhosphorCoreutils.Token<'T> // __@phosphor_coreutils.Token
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type NotebookPanel = Panel.NotebookPanel
    type NotebookTools = Notebooktools.NotebookTools

    type [<AllowNullLiteral>] IExports =
        abstract INotebookTools: Token<INotebookTools>
        abstract INotebookTracker: Token<INotebookTracker>

    [<Import("*","@jupyterlab/notebook")>]
    let Types : IExports = jsNative

    /// The notebook tools token.
    /// The interface for notebook metadata tools.
    /// The namespace for NotebookTools class statics.
    type [<AllowNullLiteral>] INotebookTools =
        inherit Widget
        abstract activeNotebookPanel: NotebookPanel option with get, set
        abstract activeCell: Cell option with get, set
        abstract selectedCells: ResizeArray<Cell> with get, set
        abstract addItem: options: Notebooktools.NotebookTools.IAddOptions -> unit

    module INotebookTools =

        /// The options used to add an item to the notebook tools.
        type [<AllowNullLiteral>] IAddOptions =
            /// The tool to add to the notebook tools area.
            abstract tool: ITool with get, set
            /// The section to which the tool should be added.
            abstract section: U2<string, string> option with get, set
            /// The rank order of the widget among its siblings.
            abstract rank: float option with get, set

        type [<AllowNullLiteral>] ITool =
            inherit Widget
            /// The notebook tools object.
            abstract notebookTools: INotebookTools with get, set

    /// The notebook tracker token.
    /// An object that tracks notebook widgets.
    type [<AllowNullLiteral>] INotebookTracker =
        inherit JupyterlabApputils.IWidgetTracker<NotebookPanel>
        /// The currently focused cell.
        /// 
        /// #### Notes
        /// If there is no cell with the focus, then this value is `null`.
        abstract activeCell: Cell
        /// A signal emitted when the current active cell changes.
        /// 
        /// #### Notes
        /// If there is no cell with the focus, then `null` will be emitted.
        abstract activeCellChanged: ISignal<INotebookTracker, Cell>
        /// A signal emitted when the selection state changes.
        abstract selectionChanged: ISignal<INotebookTracker, unit>

module Tracker =
    type WidgetTracker = JupyterlabApputils.WidgetTracker //__@jupyterlab_apputils.WidgetTracker
    type Cell = JupyterlabCells.Widget.Cell // __@jupyterlab_cells.Cell
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type INotebookTracker = Tokens.INotebookTracker
    type NotebookPanel = Panel.NotebookPanel

    type [<AllowNullLiteral>] IExports =
        abstract NotebookTracker: NotebookTrackerStatic

    type [<AllowNullLiteral>] NotebookTracker =
        inherit JupyterlabApputils.WidgetTracker<NotebookPanel>
        inherit INotebookTracker
        /// The currently focused cell.
        /// 
        /// #### Notes
        /// This is a read-only property. If there is no cell with the focus, then this
        /// value is `null`.
        abstract activeCell: Cell
        /// A signal emitted when the current active cell changes.
        /// 
        /// #### Notes
        /// If there is no cell with the focus, then `null` will be emitted.
        abstract activeCellChanged: ISignal<NotebookTracker, Cell>
        /// A signal emitted when the selection state changes.
        abstract selectionChanged: ISignal<NotebookTracker, unit>
        /// <summary>Add a new notebook panel to the tracker.</summary>
        /// <param name="panel">- The notebook panel being added.</param>
        abstract add: panel: NotebookPanel -> Promise<unit>
        /// Dispose of the resources held by the tracker.
        abstract dispose: unit -> unit
        /// Handle the current change event.
        abstract onCurrentChanged: widget: NotebookPanel -> unit

    type [<AllowNullLiteral>] NotebookTrackerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> NotebookTracker

module Truststatus =
    type VDomRenderer<'T> = JupyterlabApputils.VDomRenderer<'T> // __@jupyterlab_apputils.VDomRenderer
    type VDomModel = JupyterlabApputils.VDomModel //__@jupyterlab_apputils.VDomModel
    type Notebook = Widget.Notebook

    type [<AllowNullLiteral>] IExports =
        abstract NotebookTrustStatus: NotebookTrustStatusStatic

    /// The NotebookTrust status item.
    /// A namespace for NotebookTrust statics.
    type [<AllowNullLiteral>] NotebookTrustStatus =
        inherit VDomRenderer<NotebookTrustStatus.Model>
        /// Render the NotebookTrust status item.
        abstract render: unit -> obj // JSX.Element

    /// The NotebookTrust status item.
    /// A namespace for NotebookTrust statics.
    type [<AllowNullLiteral>] NotebookTrustStatusStatic =
        /// Construct a new status item.
        [<Emit "new $0($1...)">] abstract Create: unit -> NotebookTrustStatus

    module NotebookTrustStatus =

        type [<AllowNullLiteral>] IExports =
            abstract Model: ModelStatic

        /// A VDomModel for the NotebookTrust status item.
        type [<AllowNullLiteral>] Model =
            inherit VDomModel
            /// The number of trusted cells in the current notebook.
            abstract trustedCells: float
            /// The total number of cells in the current notebook.
            abstract totalCells: float
            /// Whether the active cell is trusted.
            abstract activeCellTrusted: bool
            /// The current notebook for the model.
            abstract notebook: Notebook option with get, set

        /// A VDomModel for the NotebookTrust status item.
        type [<AllowNullLiteral>] ModelStatic =
            /// Construct a new status item.
            [<Emit "new $0($1...)">] abstract Create: unit -> Model

module Widget =
    type JSONValue = PhosphorCoreutils.JSONValue // __@phosphor_coreutils.JSONValue
    type Message = PhosphorMessaging.Message // type Message = __@phosphor_messaging.Message
    type ISignal<'T,'U>  = PhosphorSignaling.ISignal<'T,'U> // __@phosphor_signaling.ISignal
    type Widget = PhosphorWidgets.Widget // __@phosphor_widgets.Widget
    type Cell = JupyterlabCells.Widget.Cell // __@jupyterlab_cells.Cell
    type CodeCell = JupyterlabCells.Widget.CodeCell // __@jupyterlab_cells.CodeCell
    type MarkdownCell = JupyterlabCells.Widget.MarkdownCell // __@jupyterlab_cells.MarkdownCell
    type RawCell = JupyterlabCells.Widget.RawCell // __@jupyterlab_cells.RawCell
    type IEditorMimeTypeService = JupyterlabCodeeditor.Mimetype.IEditorMimeTypeService // __@jupyterlab_codeeditor.IEditorMimeTypeService
    // type CodeEditor = JupyterlabCodeeditor.Editor.CodeEditor // __@jupyterlab_codeeditor.CodeEditor
    type IChangedArgs<'T> = JupyterlabCoreutils.Interfaces.IChangedArgs<'T> // __@jupyterlab_coreutils.IChangedArgs
    // type nbformat = __@jupyterlab_coreutils.nbformat
    type IObservableMap<'T> = JupyterlabObservables.Observablemap.IObservableMap<'T> // __@jupyterlab_observables.IObservableMap
    type IRenderMimeRegistry = JupyterlabRendermime.Registry.IRenderMimeRegistry // __@jupyterlab_rendermime.IRenderMimeRegistry
    type INotebookModel = Model.INotebookModel

    type [<AllowNullLiteral>] IExports =
        /// amo changed from Notebook to StaticNotebook to avoid collision
        abstract StaticNotebook: StaticNotebookStatic
        abstract Notebook: NotebookStatic

    type [<StringEnum>] [<RequireQualifiedAccess>] NotebookMode =
        | Command
        | Edit

    /// A widget which renders static non-interactive notebooks.
    /// 
    /// #### Notes
    /// The widget model must be set separately and can be changed
    /// at any time.  Consumers of the widget must account for a
    /// `null` model, and may want to listen to the `modelChanged`
    /// signal.
    /// The namespace for the `StaticNotebook` class statics.
    type [<AllowNullLiteral>] StaticNotebook =
        inherit Widget
        /// A signal emitted when the model of the notebook changes.
        abstract modelChanged: ISignal<StaticNotebook, unit>
        /// A signal emitted when the model content changes.
        /// 
        /// #### Notes
        /// This is a convenience signal that follows the current model.
        abstract modelContentChanged: ISignal<StaticNotebook, unit>
        /// The cell factory used by the widget.
        abstract contentFactory: StaticNotebook.IContentFactory
        /// The Rendermime instance used by the widget.
        abstract rendermime: IRenderMimeRegistry
        /// The model for the widget.
        abstract model: INotebookModel with get, set
        /// Get the mimetype for code cells.
        abstract codeMimetype: string
        /// A read-only sequence of the widgets in the notebook.
        abstract widgets: ReadonlyArray<Cell>
        /// A configuration object for cell editor settings.
        abstract editorConfig: StaticNotebook.IEditorConfig with get, set
        /// A configuration object for notebook settings.
        abstract notebookConfig: StaticNotebook.INotebookConfig with get, set
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit
        /// Handle a new model.
        /// 
        /// #### Notes
        /// This method is called after the model change has been handled
        /// internally and before the `modelChanged` signal is emitted.
        /// The default implementation is a no-op.
        abstract onModelChanged: oldValue: INotebookModel * newValue: INotebookModel -> unit
        /// Handle changes to the notebook model content.
        /// 
        /// #### Notes
        /// The default implementation emits the `modelContentChanged` signal.
        abstract onModelContentChanged: model: INotebookModel * args: unit -> unit
        /// Handle changes to the notebook model metadata.
        /// 
        /// #### Notes
        /// The default implementation updates the mimetypes of the code cells
        /// when the `language_info` metadata changes.
        abstract onMetadataChanged: sender: IObservableMap<JSONValue> * args: JupyterlabObservables.Observablemap.IObservableMap.IChangedArgs<JSONValue> -> unit
        /// Handle a cell being inserted.
        /// 
        /// The default implementation is a no-op
        abstract onCellInserted: index: float * cell: Cell -> unit
        /// Handle a cell being moved.
        /// 
        /// The default implementation is a no-op
        abstract onCellMoved: fromIndex: float * toIndex: float -> unit
        /// Handle a cell being removed.
        /// 
        /// The default implementation is a no-op
        abstract onCellRemoved: index: float * cell: Cell -> unit

    /// A widget which renders static non-interactive notebooks.
    /// 
    /// #### Notes
    /// The widget model must be set separately and can be changed
    /// at any time.  Consumers of the widget must account for a
    /// `null` model, and may want to listen to the `modelChanged`
    /// signal.
    /// The namespace for the `StaticNotebook` class statics.
    type [<AllowNullLiteral>] StaticNotebookStatic =
        /// Construct a notebook widget.
        [<Emit "new $0($1...)">] abstract Create: options: StaticNotebook.IOptions -> StaticNotebook

    module StaticNotebook =

        type [<AllowNullLiteral>] IExports =
            abstract defaultEditorConfig: IEditorConfig
            abstract defaultNotebookConfig: INotebookConfig
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: IContentFactory

        /// An options object for initializing a static notebook.
        type [<AllowNullLiteral>] IOptions =
            /// The rendermime instance used by the widget.
            abstract rendermime: IRenderMimeRegistry with get, set
            /// The language preference for the model.
            abstract languagePreference: string option with get, set
            /// A factory for creating content.
            abstract contentFactory: IContentFactory option with get, set
            /// A configuration object for the cell editor settings.
            abstract editorConfig: IEditorConfig option with get, set
            /// A configuration object for notebook settings.
            abstract notebookConfig: INotebookConfig option with get, set
            /// The service used to look up mime types.
            abstract mimeTypeService: IEditorMimeTypeService with get, set

        /// A factory for creating notebook content.
        /// 
        /// #### Notes
        /// This extends the content factory of the cell itself, which extends the content
        /// factory of the output area and input area. The result is that there is a single
        /// factory for creating all child content of a notebook.
        type [<AllowNullLiteral>] IContentFactory =
            inherit JupyterlabCells.Widget.Cell.IContentFactory
            /// Create a new code cell widget.
            abstract createCodeCell: options: JupyterlabCells.Widget.CodeCell.IOptions * parent: StaticNotebook -> CodeCell
            /// Create a new markdown cell widget.
            abstract createMarkdownCell: options: JupyterlabCells.Widget.MarkdownCell.IOptions * parent: StaticNotebook -> MarkdownCell
            /// Create a new raw cell widget.
            abstract createRawCell: options: JupyterlabCells.Widget.RawCell.IOptions * parent: StaticNotebook -> RawCell

        /// A config object for the cell editors.
        type [<AllowNullLiteral>] IEditorConfig =
            /// Config options for code cells.
            abstract code: obj
            /// Config options for markdown cells.
            abstract markdown: obj
            /// Config options for raw cells.
            abstract raw: obj

        /// A config object for the notebook widget
        type [<AllowNullLiteral>] INotebookConfig =
            /// Enable scrolling past the last cell
            abstract scrollPastEnd: bool with get, set
            /// The default type for new notebook cells.
            abstract defaultCell: JupyterlabCoreutils.Nbformat.Nbformat.CellType with get, set

        /// The default implementation of an `IContentFactory`.
        /// A namespace for the staic notebook content factory.
        type [<AllowNullLiteral>] ContentFactory =
            inherit JupyterlabCells.Widget.Cell.ContentFactory
            inherit IContentFactory
            /// Create a new code cell widget.
            /// 
            /// #### Notes
            /// If no cell content factory is passed in with the options, the one on the
            /// notebook content factory is used.
            abstract createCodeCell: options: JupyterlabCells.Widget.CodeCell.IOptions * parent: StaticNotebook -> CodeCell
            /// Create a new markdown cell widget.
            /// 
            /// #### Notes
            /// If no cell content factory is passed in with the options, the one on the
            /// notebook content factory is used.
            abstract createMarkdownCell: options: JupyterlabCells.Widget.MarkdownCell.IOptions * parent: StaticNotebook -> MarkdownCell
            /// Create a new raw cell widget.
            /// 
            /// #### Notes
            /// If no cell content factory is passed in with the options, the one on the
            /// notebook content factory is used.
            abstract createRawCell: options: JupyterlabCells.Widget.RawCell.IOptions * parent: StaticNotebook -> RawCell

        /// The default implementation of an `IContentFactory`.
        /// A namespace for the staic notebook content factory.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            /// Construct a notebook widget.
            [<Emit "new $0($1...)">] abstract Create: options: Notebook.IOptions -> ContentFactory

        module ContentFactory =

            /// Options for the content factory.
            type [<AllowNullLiteral>] IOptions =
                inherit JupyterlabCells.Widget.Cell.ContentFactory.IOptions

    /// A notebook widget that supports interactivity.
    /// The namespace for the `Notebook` class statics.
    type [<AllowNullLiteral>] Notebook =
        inherit StaticNotebook
        /// A signal emitted when the active cell changes.
        /// 
        /// #### Notes
        /// This can be due to the active index changing or the
        /// cell at the active index changing.
        abstract activeCellChanged: ISignal<Notebook, Cell>
        /// A signal emitted when the state of the notebook changes.
        abstract stateChanged: ISignal<Notebook, IChangedArgs<obj option>>
        /// A signal emitted when the selection state of the notebook changes.
        abstract selectionChanged: ISignal<Notebook, unit>
        /// The interactivity mode of the notebook.
        abstract mode: NotebookMode with get, set
        /// The active cell index of the notebook.
        /// 
        /// #### Notes
        /// The index will be clamped to the bounds of the notebook cells.
        abstract activeCellIndex: float with get, set
        /// Get the active cell widget.
        /// 
        /// #### Notes
        /// This is a cell or `null` if there is no active cell.
        abstract activeCell: Cell option
        /// Dispose of the resources held by the widget.
        abstract dispose: unit -> unit
        /// Select a cell widget.
        /// 
        /// #### Notes
        /// It is a no-op if the value does not change.
        /// It will emit the `selectionChanged` signal.
        abstract select: widget: Cell -> unit
        /// Deselect a cell widget.
        /// 
        /// #### Notes
        /// It is a no-op if the value does not change.
        /// It will emit the `selectionChanged` signal.
        abstract deselect: widget: Cell -> unit
        /// Whether a cell is selected.
        abstract isSelected: widget: Cell -> bool
        /// Whether a cell is selected or is the active cell.
        abstract isSelectedOrActive: widget: Cell -> bool
        /// Deselect all of the cells.
        abstract deselectAll: unit -> unit
        /// <summary>Move the head of an existing contiguous selection to extend the selection.</summary>
        /// <param name="index">- The new head of the existing selection.
        /// 
        /// #### Notes
        /// If there is no existing selection, the active cell is considered an
        /// existing one-cell selection.
        /// 
        /// If the new selection is a single cell, that cell becomes the active cell
        /// and all cells are deselected.
        /// 
        /// There is no change if there are no cells (i.e., activeCellIndex is -1).</param>
        abstract extendContiguousSelectionTo: index: float -> unit
        /// Get the head and anchor of a contiguous cell selection.
        /// 
        /// The head of a contiguous selection is always the active cell.
        /// 
        /// If there are no cells selected, `{head: null, anchor: null}` is returned.
        /// 
        /// Throws an error if the currently selected cells do not form a contiguous
        /// selection.
        abstract getContiguousSelection: unit -> NotebookGetContiguousSelectionReturn
        /// <summary>Scroll so that the given position is centered.</summary>
        /// <param name="position">- The vertical position in the notebook widget.</param>
        /// <param name="threshold">- An optional threshold for the scroll (0-50, defaults to
        /// 25).
        /// 
        /// #### Notes
        /// If the position is within the threshold percentage of the widget height,
        /// measured from the center of the widget, the scroll position will not be
        /// changed. A threshold of 0 means we will always scroll so the position is
        /// centered, and a threshold of 50 means scrolling only happens if position is
        /// outside the current window.</param>
        abstract scrollToPosition: position: float * ?threshold: float -> unit
        /// Set URI fragment identifier.
        abstract setFragment: fragment: string -> unit
        /// <summary>Handle the DOM events for the widget.</summary>
        /// <param name="event">- The DOM event sent to the widget.
        /// 
        /// #### Notes
        /// This method implements the DOM `EventListener` interface and is
        /// called in response to events on the notebook panel's node. It should
        /// not be called directly by user code.</param>
        abstract handleEvent: ``event``: Event -> unit
        /// Handle `after-attach` messages for the widget.
        abstract onAfterAttach: msg: Message -> unit
        /// Handle `before-detach` messages for the widget.
        abstract onBeforeDetach: msg: Message -> unit
        /// A message handler invoked on an `'after-show'` message.
        abstract onAfterShow: msg: Message -> unit
        /// A message handler invoked on a `'resize'` message.
        abstract onResize: msg: PhosphorWidgets.Widget.ResizeMessage -> unit
        /// A message handler invoked on an `'before-hide'` message.
        abstract onBeforeHide: msg: Message -> unit
        /// Handle `'activate-request'` messages.
        abstract onActivateRequest: msg: Message -> unit
        /// Handle `update-request` messages sent to the widget.
        abstract onUpdateRequest: msg: Message -> unit
        /// Handle a cell being inserted.
        abstract onCellInserted: index: float * cell: Cell -> unit
        /// Handle a cell being moved.
        abstract onCellMoved: fromIndex: float * toIndex: float -> unit
        /// Handle a cell being removed.
        abstract onCellRemoved: index: float * cell: Cell -> unit
        /// Handle a new model.
        abstract onModelChanged: oldValue: INotebookModel * newValue: INotebookModel -> unit

    type [<AllowNullLiteral>] NotebookGetContiguousSelectionReturn =
        abstract head: float option with get, set
        abstract anchor: float option with get, set

    /// A notebook widget that supports interactivity.
    /// The namespace for the `Notebook` class statics.
    type [<AllowNullLiteral>] NotebookStatic =
        /// Construct a notebook widget.
        [<Emit "new $0($1...)">] abstract Create: options: Notebook.IOptions -> Notebook

    module Notebook =

        type [<AllowNullLiteral>] IExports =
            abstract ContentFactory: ContentFactoryStatic
            abstract defaultContentFactory: IContentFactory

        /// An options object for initializing a notebook widget.
        type [<AllowNullLiteral>] IOptions =
            inherit StaticNotebook.IOptions

        /// The content factory for the notebook widget.
        type [<AllowNullLiteral>] IContentFactory =
            inherit StaticNotebook.IContentFactory

        /// The default implementation of a notebook content factory..
        /// 
        /// #### Notes
        /// Override methods on this class to customize the default notebook factory
        /// methods that create notebook content.
        /// A namespace for the notebook content factory.
        type [<AllowNullLiteral>] ContentFactory =
            inherit StaticNotebook.ContentFactory

        /// The default implementation of a notebook content factory..
        /// 
        /// #### Notes
        /// Override methods on this class to customize the default notebook factory
        /// methods that create notebook content.
        /// A namespace for the notebook content factory.
        type [<AllowNullLiteral>] ContentFactoryStatic =
            [<Emit "new $0($1...)">] abstract Create: unit -> ContentFactory

        module ContentFactory =

            /// An options object for initializing a notebook content factory.
            type [<AllowNullLiteral>] IOptions =
                inherit StaticNotebook.ContentFactory.IOptions

module Widgetfactory =
    type IEditorMimeTypeService = JupyterlabCodeeditor.Mimetype.IEditorMimeTypeService // __@jupyterlab_codeeditor.IEditorMimeTypeService
    type ABCWidgetFactory<'T> = JupyterlabDocregistry.Default.ABCWidgetFactory<'T> // __@jupyterlab_docregistry.ABCWidgetFactory
    type DocumentRegistry = JupyterlabDocregistry.Registry.DocumentRegistry // __@jupyterlab_docregistry.DocumentRegistry
    type IRenderMimeRegistry = JupyterlabRendermime.Registry.IRenderMimeRegistry // __@jupyterlab_rendermime.IRenderMimeRegistry
    type INotebookModel = Model.INotebookModel
    type NotebookPanel = Panel.NotebookPanel
    type StaticNotebook = Widget.StaticNotebook

    type [<AllowNullLiteral>] IExports =
        abstract NotebookWidgetFactory: NotebookWidgetFactoryStatic

    /// A widget factory for notebook panels.
    /// The namespace for `NotebookWidgetFactory` statics.
    type [<AllowNullLiteral>] NotebookWidgetFactory =
        inherit JupyterlabDocregistry.Default.ABCWidgetFactory<NotebookPanel, INotebookModel>
        abstract rendermime: IRenderMimeRegistry
        /// The content factory used by the widget factory.
        abstract contentFactory: Panel.NotebookPanel.IContentFactory
        /// The service used to look up mime types.
        abstract mimeTypeService: IEditorMimeTypeService
        /// A configuration object for cell editor settings.
        abstract editorConfig: Widget.StaticNotebook.IEditorConfig with get, set
        /// A configuration object for notebook settings.
        abstract notebookConfig: Widget.StaticNotebook.INotebookConfig with get, set
        /// Create a new widget.
        /// 
        /// #### Notes
        /// The factory will start the appropriate kernel.
        abstract createNewWidget: context: JupyterlabDocregistry.Registry.DocumentRegistry.IContext<INotebookModel> * ?source: NotebookPanel -> NotebookPanel
        /// Default factory for toolbar items to be added after the widget is created.
        abstract defaultToolbarFactory: widget: NotebookPanel -> ResizeArray<JupyterlabDocregistry.Registry.DocumentRegistry.IToolbarItem>

    /// A widget factory for notebook panels.
    /// The namespace for `NotebookWidgetFactory` statics.
    type [<AllowNullLiteral>] NotebookWidgetFactoryStatic =
        /// <summary>Construct a new notebook widget factory.</summary>
        /// <param name="options">- The options used to construct the factory.</param>
        [<Emit "new $0($1...)">] abstract Create: options: NotebookWidgetFactory.IOptions<NotebookPanel> -> NotebookWidgetFactory

    module NotebookWidgetFactory =

        /// The options used to construct a `NotebookWidgetFactory`.
        type [<AllowNullLiteral>] IOptions<'T> =
            inherit JupyterlabDocregistry.Registry.DocumentRegistry.IWidgetFactoryOptions<'T>
            abstract rendermime: IRenderMimeRegistry with get, set
            /// A notebook panel content factory.
            abstract contentFactory: Panel.NotebookPanel.IContentFactory with get, set
            /// The service used to look up mime types.
            abstract mimeTypeService: IEditorMimeTypeService with get, set
            /// The notebook cell editor configuration.
            abstract editorConfig: Widget.StaticNotebook.IEditorConfig option with get, set
            /// The notebook configuration.
            abstract notebookConfig: Widget.StaticNotebook.INotebookConfig option with get, set
