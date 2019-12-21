// ts2fable 0.0.0
module rec PhosphorAlgorithm
open System
open Fable.Core
open Fable.Core.JS

//amo: typescript
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo: end typescript

module Array =
    
    module ArrayExt =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Find the index of the first occurrence of a value in an array.</summary>
            /// <param name="array">- The array-like object to search.</param>
            /// <param name="value">- The value to locate in the array. Values are
            /// compared using strict `===` equality.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract firstIndexOf: array: ArrayLike<'T> * value: 'T * ?start: float * ?stop: float -> float
            /// <summary>Find the index of the last occurrence of a value in an array.</summary>
            /// <param name="array">- The array-like object to search.</param>
            /// <param name="value">- The value to locate in the array. Values are
            /// compared using strict `===` equality.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract lastIndexOf: array: ArrayLike<'T> * value: 'T * ?start: float * ?stop: float -> float
            /// <summary>Find the index of the first value which matches a predicate.</summary>
            /// <param name="array">- The array-like object to search.</param>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract findFirstIndex: array: ArrayLike<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> float
            /// <summary>Find the index of the last value which matches a predicate.</summary>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract findLastIndex: array: ArrayLike<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> float
            /// <summary>Find the first value which matches a predicate.</summary>
            /// <param name="array">- The array-like object to search.</param>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract findFirstValue: array: ArrayLike<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> 'T option
            /// <summary>Find the last value which matches a predicate.</summary>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract findLastValue: array: ArrayLike<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> 'T option
            /// <summary>Find the index of the first element which compares `>=` to a value.</summary>
            /// <param name="array">- The sorted array-like object to search.</param>
            /// <param name="value">- The value to locate in the array.</param>
            /// <param name="fn">- The 3-way comparison function to apply to the values.
            /// It should return `< 0` if an element is less than a value, `0` if
            /// an element is equal to a value, or `> 0` if an element is greater
            /// than a value.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract lowerBound: array: ArrayLike<'T> * value: 'U * fn: ('T -> 'U -> float) * ?start: float * ?stop: float -> float
            /// <summary>Find the index of the first element which compares `>` than a value.</summary>
            /// <param name="array">- The sorted array-like object to search.</param>
            /// <param name="value">- The value to locate in the array.</param>
            /// <param name="fn">- The 3-way comparison function to apply to the values.
            /// It should return `< 0` if an element is less than a value, `0` if
            /// an element is equal to a value, or `> 0` if an element is greater
            /// than a value.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract upperBound: array: ArrayLike<'T> * value: 'U * fn: ('T -> 'U -> float) * ?start: float * ?stop: float -> float
            /// <summary>Test whether two arrays are shallowly equal.</summary>
            /// <param name="a">- The first array-like object to compare.</param>
            /// <param name="b">- The second array-like object to compare.</param>
            /// <param name="fn">- The comparison function to apply to the elements. It
            /// should return `true` if the elements are "equal". The default
            /// compares elements using strict `===` equality.</param>
            abstract shallowEqual: a: ArrayLike<'T> * b: ArrayLike<'T> * ?fn: ('T -> 'T -> bool) -> bool
            /// <summary>Create a slice of an array subject to an optional step.</summary>
            /// <param name="array">- The array-like object of interest.</param>
            /// <param name="options">- The options for configuring the slice.</param>
            abstract slice: array: ArrayLike<'T> * ?options: Slice.IOptions -> ResizeArray<'T>
            /// <summary>Move an element in an array from one index to another.</summary>
            /// <param name="array">- The mutable array-like object of interest.</param>
            /// <param name="fromIndex">- The index of the element to move. Negative
            /// values are taken as an offset from the end of the array.</param>
            /// <param name="toIndex">- The target index of the element. Negative
            /// values are taken as an offset from the end of the array.
            /// 
            /// #### Complexity
            /// Linear.
            /// 
            /// #### Undefined Behavior
            /// A `fromIndex` or `toIndex` which is non-integral.
            /// 
            /// #### Example
            /// ```typescript
            /// import { ArrayExt } from from '</param>
            abstract move: array: MutableArrayLike<'T> * fromIndex: float * toIndex: float -> unit
            /// <summary>Reverse an array in-place.</summary>
            /// <param name="array">- The mutable array-like object of interest.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// reversed, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// reversed, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.
            /// 
            /// #### Complexity
            /// Linear.
            /// 
            /// #### Undefined Behavior
            /// A `start` or  `stop` index which is non-integral.
            /// 
            /// #### Example
            /// ```typescript
            /// import { ArrayExt } from '</param>
            abstract reverse: array: MutableArrayLike<'T> * ?start: float * ?stop: float -> unit
            /// <summary>Rotate the elements of an array in-place.</summary>
            /// <param name="array">- The mutable array-like object of interest.</param>
            /// <param name="delta">- The amount of rotation to apply to the elements. A
            /// positive value will rotate the elements to the left. A negative
            /// value will rotate the elements to the right.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// rotated, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// rotated, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.
            /// 
            /// #### Complexity
            /// Linear.
            /// 
            /// #### Undefined Behavior
            /// A `delta`, `start`, or `stop` which is non-integral.
            /// 
            /// #### Example
            /// ```typescript
            /// import { ArrayExt } from '</param>
            abstract rotate: array: MutableArrayLike<'T> * delta: float * ?start: float * ?stop: float -> unit
            /// <summary>Fill an array with a static value.</summary>
            /// <param name="array">- The mutable array-like object to fill.</param>
            /// <param name="value">- The static value to use to fill the array.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// filled, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// filled, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.
            /// 
            /// #### Notes
            /// If `stop < start` the fill will wrap at the end of the array.
            /// 
            /// #### Complexity
            /// Linear.
            /// 
            /// #### Undefined Behavior
            /// A `start` or `stop` which is non-integral.
            /// 
            /// #### Example
            /// ```typescript
            /// import { ArrayExt } from '</param>
            abstract fill: array: MutableArrayLike<'T> * value: 'T * ?start: float * ?stop: float -> unit
            /// <summary>Insert a value into an array at a specific index.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="index">- The index at which to insert the value. Negative
            /// values are taken as an offset from the end of the array.</param>
            /// <param name="value">- The value to set at the specified index.
            /// 
            /// #### Complexity
            /// Linear.
            /// 
            /// #### Undefined Behavior
            /// An `index` which is non-integral.
            /// 
            /// #### Example
            /// ```typescript
            /// import { ArrayExt } from '</param>
            abstract insert: array: Array<'T> * index: float * value: 'T -> unit
            /// <summary>Remove and return a value at a specific index in an array.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="index">- The index of the value to remove. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeAt: array: Array<'T> * index: float -> 'T option
            /// <summary>Remove the first occurrence of a value from an array.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="value">- The value to remove from the array. Values are
            /// compared using strict `===` equality.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeFirstOf: array: Array<'T> * value: 'T * ?start: float * ?stop: float -> float
            /// <summary>Remove the last occurrence of a value from an array.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="value">- The value to remove from the array. Values are
            /// compared using strict `===` equality.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeLastOf: array: Array<'T> * value: 'T * ?start: float * ?stop: float -> float
            /// <summary>Remove all occurrences of a value from an array.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="value">- The value to remove from the array. Values are
            /// compared using strict `===` equality.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeAllOf: array: Array<'T> * value: 'T * ?start: float * ?stop: float -> float
            /// <summary>Remove the first occurrence of a value which matches a predicate.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeFirstWhere: array: Array<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> RemoveFirstWhereReturn
            /// <summary>Remove the last occurrence of a value which matches a predicate.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeLastWhere: array: Array<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> RemoveLastWhereReturn
            /// <summary>Remove all occurrences of values which match a predicate.</summary>
            /// <param name="array">- The array of interest.</param>
            /// <param name="fn">- The predicate function to apply to the values.</param>
            /// <param name="start">- The index of the first element in the range to be
            /// searched, inclusive. The default value is `0`. Negative values
            /// are taken as an offset from the end of the array.</param>
            /// <param name="stop">- The index of the last element in the range to be
            /// searched, inclusive. The default value is `-1`. Negative values
            /// are taken as an offset from the end of the array.</param>
            abstract removeAllWhere: array: Array<'T> * fn: ('T -> float -> bool) * ?start: float * ?stop: float -> float

        type [<AllowNullLiteral>] RemoveFirstWhereReturn =
            abstract index: float with get, set
            abstract value: 'T option with get, set

        type [<AllowNullLiteral>] RemoveLastWhereReturn =
            abstract index: float with get, set
            abstract value: 'T option with get, set

        module Slice =

            /// The options for the `slice` function.
            type [<AllowNullLiteral>] IOptions =
                /// The starting index of the slice, inclusive.
                /// 
                /// Negative values are taken as an offset from the end
                /// of the array.
                /// 
                /// The default is `0` if `step > 0` else `n - 1`.
                abstract start: float option with get, set
                /// The stopping index of the slice, exclusive.
                /// 
                /// Negative values are taken as an offset from the end
                /// of the array.
                /// 
                /// The default is `n` if `step > 0` else `-n - 1`.
                abstract stop: float option with get, set
                /// The step value for the slice.
                /// 
                /// This must not be `0`.
                /// 
                /// The default is `1`.
                abstract step: float option with get, set

        type [<AllowNullLiteral>] MutableArrayLike<'T> =
            abstract length: float
            [<Emit "$0[$1]{{=$2}}">] abstract Item: index: float -> 'T with get, set

module Chain =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Chain together several iterables.</summary>
        /// <param name="objects">- The iterable or array-like objects of interest.</param>
        abstract chain: [<ParamArray>] objects: ResizeArray<IterableOrArrayLike<'T>> -> IIterator<'T>
        abstract ChainIterator: ChainIteratorStatic

    /// An iterator which chains together several iterators.
    type [<AllowNullLiteral>] ChainIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which chains together several iterators.
    type [<AllowNullLiteral>] ChainIteratorStatic =
        /// <summary>Construct a new chain iterator.</summary>
        /// <param name="source">- The iterator of iterators of interest.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<IIterator<'T>> -> ChainIterator<'T>

module Empty =
    type IIterator<'T> = Iter.IIterator<'T>

    type [<AllowNullLiteral>] IExports =
        /// Create an empty iterator.
        abstract empty: unit -> IIterator<'T>
        abstract EmptyIterator: EmptyIteratorStatic

    /// An iterator which is always empty.
    type [<AllowNullLiteral>] EmptyIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which is always empty.
    type [<AllowNullLiteral>] EmptyIteratorStatic =
        /// Construct a new empty iterator.
        [<Emit "new $0($1...)">] abstract Create: unit -> EmptyIterator<'T>

module Enumerate =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Enumerate an iterable object.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="start">- The starting enum value. The default is `0`.</param>
        abstract enumerate: ``object``: IterableOrArrayLike<'T> * ?start: float -> IIterator<float * 'T>
        abstract EnumerateIterator: EnumerateIteratorStatic

    /// An iterator which enumerates the source values.
    type [<AllowNullLiteral>] EnumerateIterator<'T> =
        inherit IIterator<float * 'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<float * 'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<float * 'T>
        /// Get the next value from the iterator.
        abstract next: unit -> float * 'T option

    /// An iterator which enumerates the source values.
    type [<AllowNullLiteral>] EnumerateIteratorStatic =
        /// <summary>Construct a new enumerate iterator.</summary>
        /// <param name="source">- The iterator of values of interest.</param>
        /// <param name="start">- The starting enum value.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<'T> * start: float -> EnumerateIterator<'T>

module Filter =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Filter an iterable for values which pass a test.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The predicate function to invoke for each value.</param>
        abstract filter: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> bool) -> IIterator<'T>
        abstract FilterIterator: FilterIteratorStatic

    /// An iterator which yields values which pass a test.
    type [<AllowNullLiteral>] FilterIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which yields values which pass a test.
    type [<AllowNullLiteral>] FilterIteratorStatic =
        /// <summary>Construct a new filter iterator.</summary>
        /// <param name="source">- The iterator of values of interest.</param>
        /// <param name="fn">- The predicate function to invoke for each value.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<'T> * fn: ('T -> float -> bool) -> FilterIterator<'T>

module Find =
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Find the first value in an iterable which matches a predicate.</summary>
        /// <param name="object">- The iterable or array-like object to search.</param>
        /// <param name="fn">- The predicate function to apply to the values.</param>
        abstract find: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> bool) -> 'T option
        /// <summary>Find the index of the first value which matches a predicate.</summary>
        /// <param name="object">- The iterable or array-like object to search.</param>
        /// <param name="fn">- The predicate function to apply to the values.</param>
        abstract findIndex: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> bool) -> float
        /// <summary>Find the minimum value in an iterable.</summary>
        /// <param name="object">- The iterable or array-like object to search.</param>
        /// <param name="fn">- The 3-way comparison function to apply to the values.
        /// It should return `< 0` if the first value is less than the second.
        /// `0` if the values are equivalent, or `> 0` if the first value is
        /// greater than the second.</param>
        abstract min: ``object``: IterableOrArrayLike<'T> * fn: ('T -> 'T -> float) -> 'T option
        /// <summary>Find the maximum value in an iterable.</summary>
        /// <param name="object">- The iterable or array-like object to search.</param>
        /// <param name="fn">- The 3-way comparison function to apply to the values.
        /// It should return `< 0` if the first value is less than the second.
        /// `0` if the values are equivalent, or `> 0` if the first value is
        /// greater than the second.</param>
        abstract max: ``object``: IterableOrArrayLike<'T> * fn: ('T -> 'T -> float) -> 'T option
        /// <summary>Find the minimum and maximum values in an iterable.</summary>
        /// <param name="object">- The iterable or array-like object to search.</param>
        /// <param name="fn">- The 3-way comparison function to apply to the values.
        /// It should return `< 0` if the first value is less than the second.
        /// `0` if the values are equivalent, or `> 0` if the first value is
        /// greater than the second.</param>
        abstract minmax: ``object``: IterableOrArrayLike<'T> * fn: ('T -> 'T -> float) -> 'T * 'T option

module Iter =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create an iterator for an iterable object.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        abstract iter: ``object``: IterableOrArrayLike<'T> -> IIterator<'T>
        /// <summary>Create an iterator for the keys in an object.</summary>
        /// <param name="object">- The object of interest.</param>
        abstract iterKeys: ``object``: IterKeysObject -> IIterator<string>
        /// <summary>Create an iterator for the values in an object.</summary>
        /// <param name="object">- The object of interest.</param>
        abstract iterValues: ``object``: IterValuesObject -> IIterator<'T>
        /// <summary>Create an iterator for the items in an object.</summary>
        /// <param name="object">- The object of interest.</param>
        abstract iterItems: ``object``: IterItemsObject -> IIterator<string * 'T>
        /// <summary>Create an iterator for an iterator-like function.</summary>
        /// <param name="fn">- A function which behaves like an iterator `next` method.</param>
        abstract iterFn: fn: (unit -> 'T option) -> IIterator<'T>
        /// <summary>Invoke a function for each value in an iterable.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The callback function to invoke for each value.
        /// 
        /// #### Notes
        /// Iteration can be terminated early by returning `false` from the
        /// callback function.
        /// 
        /// #### Complexity
        /// Linear.
        /// 
        /// #### Example
        /// ```typescript
        /// import { each } from '</param>
        abstract each: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> U2<bool, unit>) -> unit
        /// <summary>Test whether all values in an iterable satisfy a predicate.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The predicate function to invoke for each value.</param>
        abstract every: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> bool) -> bool
        /// <summary>Test whether any value in an iterable satisfies a predicate.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The predicate function to invoke for each value.</param>
        abstract some: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> bool) -> bool
        /// <summary>Create an array from an iterable of values.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        abstract toArray: ``object``: IterableOrArrayLike<'T> -> ResizeArray<'T>
        /// <summary>Create an object from an iterable of key/value pairs.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        abstract toObject: ``object``: IterableOrArrayLike<string * 'T> -> ToObjectReturn
        abstract ArrayIterator: ArrayIteratorStatic
        abstract KeyIterator: KeyIteratorStatic
        abstract ValueIterator: ValueIteratorStatic
        abstract ItemIterator: ItemIteratorStatic
        abstract FnIterator: FnIteratorStatic

    type [<AllowNullLiteral>] IterKeysObject =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T

    type [<AllowNullLiteral>] IterValuesObject =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T

    type [<AllowNullLiteral>] IterItemsObject =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T

    type [<AllowNullLiteral>] ToObjectReturn =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T with get, set

    /// An object which can produce an iterator over its values.
    type [<AllowNullLiteral>] IIterable<'T> =
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>

    /// An object which traverses a collection of values.
    /// 
    /// #### Notes
    /// An `IIterator` is itself an `IIterable`. Most implementations of
    /// `IIterator` should simply return `this` from the `iter()` method.
    type [<AllowNullLiteral>] IIterator<'T> =
        inherit IIterable<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    type IterableOrArrayLike<'T> =
        U2<IIterable<'T>, ArrayLike<'T>>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module IterableOrArrayLike =
        let ofIIterable v: IterableOrArrayLike<'T> = v |> U2.Case1
        let isIIterable (v: IterableOrArrayLike<'T>) = match v with U2.Case1 _ -> true | _ -> false
        let asIIterable (v: IterableOrArrayLike<'T>) = match v with U2.Case1 o -> Some o | _ -> None
        let ofArrayLike v: IterableOrArrayLike<'T> = v |> U2.Case2
        let isArrayLike (v: IterableOrArrayLike<'T>) = match v with U2.Case2 _ -> true | _ -> false
        let asArrayLike (v: IterableOrArrayLike<'T>) = match v with U2.Case2 o -> Some o | _ -> None

    /// An iterator for an array-like object.
    /// 
    /// #### Notes
    /// This iterator can be used for any builtin JS array-like object.
    type [<AllowNullLiteral>] ArrayIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator for an array-like object.
    /// 
    /// #### Notes
    /// This iterator can be used for any builtin JS array-like object.
    type [<AllowNullLiteral>] ArrayIteratorStatic =
        /// <summary>Construct a new array iterator.</summary>
        /// <param name="source">- The array-like object of interest.</param>
        [<Emit "new $0($1...)">] abstract Create: source: ArrayLike<'T> -> ArrayIterator<'T>

    /// An iterator for the keys in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] KeyIterator =
        inherit IIterator<string>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<string>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<string>
        /// Get the next value from the iterator.
        abstract next: unit -> string option

    /// An iterator for the keys in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] KeyIteratorStatic =
        /// <summary>Construct a new key iterator.</summary>
        /// <param name="source">- The object of interest.</param>
        /// <param name="keys">- The keys to iterate, if known.</param>
        [<Emit "new $0($1...)">] abstract Create: source: KeyIteratorStaticSource * ?keys: ResizeArray<string> -> KeyIterator

    type [<AllowNullLiteral>] KeyIteratorStaticSource =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option

    /// An iterator for the values in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] ValueIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator for the values in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] ValueIteratorStatic =
        /// <summary>Construct a new value iterator.</summary>
        /// <param name="source">- The object of interest.</param>
        /// <param name="keys">- The keys to iterate, if known.</param>
        [<Emit "new $0($1...)">] abstract Create: source: ValueIteratorStaticSource * ?keys: ResizeArray<string> -> ValueIterator<'T>

    type [<AllowNullLiteral>] ValueIteratorStaticSource =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T

    /// An iterator for the items in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] ItemIterator<'T> =
        inherit IIterator<string * 'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<string * 'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<string * 'T>
        /// Get the next value from the iterator.
        abstract next: unit -> string * 'T option

    /// An iterator for the items in an object.
    /// 
    /// #### Notes
    /// This iterator can be used for any JS object.
    type [<AllowNullLiteral>] ItemIteratorStatic =
        /// <summary>Construct a new item iterator.</summary>
        /// <param name="source">- The object of interest.</param>
        /// <param name="keys">- The keys to iterate, if known.</param>
        [<Emit "new $0($1...)">] abstract Create: source: ItemIteratorStaticSource * ?keys: ResizeArray<string> -> ItemIterator<'T>

    type [<AllowNullLiteral>] ItemIteratorStaticSource =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T

    /// An iterator for an iterator-like function.
    type [<AllowNullLiteral>] FnIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator for an iterator-like function.
    type [<AllowNullLiteral>] FnIteratorStatic =
        /// <summary>Construct a new function iterator.</summary>
        /// <param name="fn">- The iterator-like function of interest.</param>
        [<Emit "new $0($1...)">] abstract Create: fn: (unit -> 'T option) -> FnIterator<'T>

module Map =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Transform the values of an iterable with a mapping function.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The mapping function to invoke for each value.</param>
        abstract map: ``object``: IterableOrArrayLike<'T> * fn: ('T -> float -> 'U) -> IIterator<'U>
        abstract MapIterator: MapIteratorStatic

    /// An iterator which transforms values using a mapping function.
    type [<AllowNullLiteral>] MapIterator<'T, 'U> =
        inherit IIterator<'U>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'U>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'U>
        /// Get the next value from the iterator.
        abstract next: unit -> 'U option

    /// An iterator which transforms values using a mapping function.
    type [<AllowNullLiteral>] MapIteratorStatic =
        /// <summary>Construct a new map iterator.</summary>
        /// <param name="source">- The iterator of values of interest.</param>
        /// <param name="fn">- The mapping function to invoke for each value.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<'T> * fn: ('T -> float -> 'U) -> MapIterator<'T, 'U>

module Range =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create an iterator of evenly spaced values.</summary>
        /// <param name="start">- The starting value for the range, inclusive.</param>
        /// <param name="stop">- The stopping value for the range, exclusive.</param>
        /// <param name="step">- The distance between each value.</param>
        abstract range: start: float * ?stop: float * ?step: float -> IIterator<float>
        abstract RangeIterator: RangeIteratorStatic

    /// An iterator which produces a range of evenly spaced values.
    type [<AllowNullLiteral>] RangeIterator =
        inherit IIterator<float>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<float>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<float>
        /// Get the next value from the iterator.
        abstract next: unit -> float option

    /// An iterator which produces a range of evenly spaced values.
    type [<AllowNullLiteral>] RangeIteratorStatic =
        /// <summary>Construct a new range iterator.</summary>
        /// <param name="start">- The starting value for the range, inclusive.</param>
        /// <param name="stop">- The stopping value for the range, exclusive.</param>
        /// <param name="step">- The distance between each value.</param>
        [<Emit "new $0($1...)">] abstract Create: start: float * stop: float * step: float -> RangeIterator

module Reduce =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Summarize all values in an iterable using a reducer function.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="fn">- The reducer function to invoke for each value.</param>
        abstract reduce: ``object``: IterableOrArrayLike<'T> * fn: ('T -> 'T -> float -> 'T) -> 'T
        abstract reduce: ``object``: IterableOrArrayLike<'T> * fn: ('U -> 'T -> float -> 'U) * initial: 'U -> 'U

module Repeat =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create an iterator which repeats a value a number of times.</summary>
        /// <param name="value">- The value to repeat.</param>
        /// <param name="count">- The number of times to repeat the value.</param>
        abstract repeat: value: 'T * count: float -> IIterator<'T>
        /// <summary>Create an iterator which yields a value a single time.</summary>
        /// <param name="value">- The value to wrap in an iterator.</param>
        abstract once: value: 'T -> IIterator<'T>
        abstract RepeatIterator: RepeatIteratorStatic

    /// An iterator which repeats a value a specified number of times.
    type [<AllowNullLiteral>] RepeatIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which repeats a value a specified number of times.
    type [<AllowNullLiteral>] RepeatIteratorStatic =
        /// <summary>Construct a new repeat iterator.</summary>
        /// <param name="value">- The value to repeat.</param>
        /// <param name="count">- The number of times to repeat the value.</param>
        [<Emit "new $0($1...)">] abstract Create: value: 'T * count: float -> RepeatIterator<'T>

module Retro =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>


    type [<AllowNullLiteral>] IExports =
        /// <summary>Create an iterator for a retroable object.</summary>
        /// <param name="object">- The retroable or array-like object of interest.</param>
        abstract retro: ``object``: RetroableOrArrayLike<'T> -> IIterator<'T>
        abstract RetroArrayIterator: RetroArrayIteratorStatic

    /// An object which can produce a reverse iterator over its values.
    type [<AllowNullLiteral>] IRetroable<'T> =
        /// Get a reverse iterator over the object's values.
        abstract retro: unit -> IIterator<'T>

    type RetroableOrArrayLike<'T> =
        U2<IRetroable<'T>, ArrayLike<'T>>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module RetroableOrArrayLike =
        let ofIRetroable v: RetroableOrArrayLike<'T> = v |> U2.Case1
        let isIRetroable (v: RetroableOrArrayLike<'T>) = match v with U2.Case1 _ -> true | _ -> false
        let asIRetroable (v: RetroableOrArrayLike<'T>) = match v with U2.Case1 o -> Some o | _ -> None
        let ofArrayLike v: RetroableOrArrayLike<'T> = v |> U2.Case2
        let isArrayLike (v: RetroableOrArrayLike<'T>) = match v with U2.Case2 _ -> true | _ -> false
        let asArrayLike (v: RetroableOrArrayLike<'T>) = match v with U2.Case2 o -> Some o | _ -> None

    /// An iterator which traverses an array-like object in reverse.
    /// 
    /// #### Notes
    /// This iterator can be used for any builtin JS array-like object.
    type [<AllowNullLiteral>] RetroArrayIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which traverses an array-like object in reverse.
    /// 
    /// #### Notes
    /// This iterator can be used for any builtin JS array-like object.
    type [<AllowNullLiteral>] RetroArrayIteratorStatic =
        /// <summary>Construct a new retro iterator.</summary>
        /// <param name="source">- The array-like object of interest.</param>
        [<Emit "new $0($1...)">] abstract Create: source: ArrayLike<'T> -> RetroArrayIterator<'T>

module Sort =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Topologically sort an iterable of edges.</summary>
        /// <param name="edges">- The iterable or array-like object of edges to sort.
        /// An edge is represented as a 2-tuple of `[fromNode, toNode]`.</param>
        abstract topologicSort: edges: IterableOrArrayLike<'T * 'T> -> ResizeArray<'T>

module Stride =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Iterate over an iterable using a stepped increment.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="step">- The distance to step on each iteration. A value
        /// of less than `1` will behave the same as a value of `1`.</param>
        abstract stride: ``object``: IterableOrArrayLike<'T> * step: float -> IIterator<'T>
        abstract StrideIterator: StrideIteratorStatic

    /// An iterator which traverses a source iterator step-wise.
    type [<AllowNullLiteral>] StrideIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which traverses a source iterator step-wise.
    type [<AllowNullLiteral>] StrideIteratorStatic =
        /// <summary>Construct a new stride iterator.</summary>
        /// <param name="source">- The iterator of values of interest.</param>
        /// <param name="step">- The distance to step on each iteration. A value
        /// of less than `1` will behave the same as a value of `1`.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<'T> * step: float -> StrideIterator<'T>

module String =

    module StringExt =

        type [<AllowNullLiteral>] IExports =
            /// <summary>Find the indices of characters in a source text.</summary>
            /// <param name="source">- The source text which should be searched.</param>
            /// <param name="query">- The characters to locate in the source text.</param>
            /// <param name="start">- The index to start the search.</param>
            abstract findIndices: source: string * query: string * ?start: float -> ResizeArray<float> option
            /// <summary>A string matcher which uses a sum-of-squares algorithm.</summary>
            /// <param name="source">- The source text which should be searched.</param>
            /// <param name="query">- The characters to locate in the source text.</param>
            /// <param name="start">- The index to start the search.</param>
            abstract matchSumOfSquares: source: string * query: string * ?start: float -> IMatchResult option
            /// <summary>A string matcher which uses a sum-of-deltas algorithm.</summary>
            /// <param name="source">- The source text which should be searched.</param>
            /// <param name="query">- The characters to locate in the source text.</param>
            /// <param name="start">- The index to start the search.</param>
            abstract matchSumOfDeltas: source: string * query: string * ?start: float -> IMatchResult option
            /// <summary>Highlight the matched characters of a source text.</summary>
            /// <param name="source">- The text which should be highlighted.</param>
            /// <param name="indices">- The indices of the matched characters. They must
            /// appear in increasing order and must be in bounds of the source.</param>
            /// <param name="fn">- The function to apply to the matched chunks.</param>
            abstract highlight: source: string * indices: ResizeArray<float> * fn: (string -> 'T) -> Array<U2<string, 'T>>
            /// <summary>A 3-way string comparison function.</summary>
            /// <param name="a">- The first string of interest.</param>
            /// <param name="b">- The second string of interest.</param>
            abstract cmp: a: string * b: string -> float

        /// The result of a string match function.
        type [<AllowNullLiteral>] IMatchResult =
            /// A score which indicates the strength of the match.
            /// 
            /// The documentation of a given match function should specify
            /// whether a lower or higher score is a stronger match.
            abstract score: float with get, set
            /// The indices of the matched characters in the source text.
            /// 
            /// The indices will appear in increasing order.
            abstract indices: ResizeArray<float> with get, set

module Take =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Take a fixed number of items from an iterable.</summary>
        /// <param name="object">- The iterable or array-like object of interest.</param>
        /// <param name="count">- The number of items to take from the iterable.</param>
        abstract take: ``object``: IterableOrArrayLike<'T> * count: float -> IIterator<'T>
        abstract TakeIterator: TakeIteratorStatic

    /// An iterator which takes a fixed number of items from a source.
    type [<AllowNullLiteral>] TakeIterator<'T> =
        inherit IIterator<'T>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<'T>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<'T>
        /// Get the next value from the iterator.
        abstract next: unit -> 'T option

    /// An iterator which takes a fixed number of items from a source.
    type [<AllowNullLiteral>] TakeIteratorStatic =
        /// <summary>Construct a new take iterator.</summary>
        /// <param name="source">- The iterator of interest.</param>
        /// <param name="count">- The number of items to take from the source.</param>
        [<Emit "new $0($1...)">] abstract Create: source: IIterator<'T> * count: float -> TakeIterator<'T>

module Zip =
    type IIterator<'T> = Iter.IIterator<'T>
    type IterableOrArrayLike<'T> = Iter.IterableOrArrayLike<'T>

    type [<AllowNullLiteral>] IExports =
        /// <summary>Iterate several iterables in lockstep.</summary>
        /// <param name="objects">- The iterable or array-like objects of interest.</param>
        abstract zip: [<ParamArray>] objects: ResizeArray<IterableOrArrayLike<'T>> -> IIterator<ResizeArray<'T>>
        abstract ZipIterator: ZipIteratorStatic

    /// An iterator which iterates several sources in lockstep.
    type [<AllowNullLiteral>] ZipIterator<'T> =
        inherit IIterator<ResizeArray<'T>>
        /// Get an iterator over the object's values.
        abstract iter: unit -> IIterator<ResizeArray<'T>>
        /// Create an independent clone of the iterator.
        abstract clone: unit -> IIterator<ResizeArray<'T>>
        /// Get the next value from the iterator.
        abstract next: unit -> ResizeArray<'T> option

    /// An iterator which iterates several sources in lockstep.
    type [<AllowNullLiteral>] ZipIteratorStatic =
        /// <summary>Construct a new zip iterator.</summary>
        /// <param name="source">- The iterators of interest.</param>
        [<Emit "new $0($1...)">] abstract Create: source: ResizeArray<IIterator<'T>> -> ZipIterator<'T>
