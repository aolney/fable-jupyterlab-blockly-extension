// ts2fable 0.0.0
module rec PhosphorVirtualdom
open System
open Fable.Core
// open Fable.Core.JS
open Browser.Types 
// open Browser.Event

//amo: typescript arraylike
type [<AllowNullLiteral>] ArrayLike<'T> =
    abstract length : int
    abstract Item : int -> 'T with get, set
type Array<'T> = ArrayLike<'T>
type ReadonlyArray<'T> = Array<'T>
//amo: end typescript hacks

type [<AllowNullLiteral>] IExports =
    abstract VirtualText: VirtualTextStatic
    abstract VirtualElement: VirtualElementStatic
    /// <summary>Create a new virtual element node.</summary>
    /// <param name="tag">- The tag name for the element.</param>
    /// <param name="children">- The children for the element, if any.</param>
    abstract h: tag: string * [<ParamArray>] children: ResizeArray<H.Child> -> VirtualElement
    abstract h: tag: string * attrs: ElementAttrs * [<ParamArray>] children: ResizeArray<H.Child> -> VirtualElement

type [<StringEnum>] [<RequireQualifiedAccess>] ElementAttrNames =
    | Abbr
    | Accept
    | [<CompiledName "accept-charset">] AcceptCharset
    | Accesskey
    | Action
    | Allowfullscreen
    | Alt
    | Autocomplete
    | Autofocus
    | Autoplay
    | Autosave
    | Checked
    | Cite
    | Cols
    | Colspan
    | Contenteditable
    | Controls
    | Coords
    | Crossorigin
    | Data
    | Datetime
    | Default
    | Dir
    | Dirname
    | Disabled
    | Download
    | Draggable
    | Dropzone
    | Enctype
    | Form
    | Formaction
    | Formenctype
    | Formmethod
    | Formnovalidate
    | Formtarget
    | Headers
    | Height
    | Hidden
    | High
    | Href
    | Hreflang
    | Id
    | Inputmode
    | Integrity
    | Ismap
    | Kind
    | Label
    | Lang
    | List
    | Loop
    | Low
    | Max
    | Maxlength
    | Media
    | Mediagroup
    | Method
    | Min
    | Minlength
    | Multiple
    | Muted
    | Name
    | Novalidate
    | Optimum
    | Pattern
    | Placeholder
    | Poster
    | Preload
    | Readonly
    | Rel
    | Required
    | Reversed
    | Rows
    | Rowspan
    | Sandbox
    | Scope
    | Selected
    | Shape
    | Size
    | Sizes
    | Span
    | Spellcheck
    | Src
    | Srcdoc
    | Srclang
    | Srcset
    | Start
    | Step
    | Tabindex
    | Target
    | Title
    | Type
    | Typemustmatch
    | Usemap
    | Value
    | Width
    | Wrap

type [<StringEnum>] [<RequireQualifiedAccess>] CSSPropertyNames =
    | AlignContent
    | AlignItems
    | AlignSelf
    | AlignmentBaseline
    | Animation
    | AnimationDelay
    | AnimationDirection
    | AnimationDuration
    | AnimationFillMode
    | AnimationIterationCount
    | AnimationName
    | AnimationPlayState
    | AnimationTimingFunction
    | BackfaceVisibility
    | Background
    | BackgroundAttachment
    | BackgroundClip
    | BackgroundColor
    | BackgroundImage
    | BackgroundOrigin
    | BackgroundPosition
    | BackgroundPositionX
    | BackgroundPositionY
    | BackgroundRepeat
    | BackgroundSize
    | BaselineShift
    | Border
    | BorderBottom
    | BorderBottomColor
    | BorderBottomLeftRadius
    | BorderBottomRightRadius
    | BorderBottomStyle
    | BorderBottomWidth
    | BorderCollapse
    | BorderColor
    | BorderImage
    | BorderImageOutset
    | BorderImageRepeat
    | BorderImageSlice
    | BorderImageSource
    | BorderImageWidth
    | BorderLeft
    | BorderLeftColor
    | BorderLeftStyle
    | BorderLeftWidth
    | BorderRadius
    | BorderRight
    | BorderRightColor
    | BorderRightStyle
    | BorderRightWidth
    | BorderSpacing
    | BorderStyle
    | BorderTop
    | BorderTopColor
    | BorderTopLeftRadius
    | BorderTopRightRadius
    | BorderTopStyle
    | BorderTopWidth
    | BorderWidth
    | Bottom
    | BoxShadow
    | BoxSizing
    | BreakAfter
    | BreakBefore
    | BreakInside
    | CaptionSide
    | Clear
    | Clip
    | ClipPath
    | ClipRule
    | Color
    | ColorInterpolationFilters
    | ColumnCount
    | ColumnFill
    | ColumnGap
    | ColumnRule
    | ColumnRuleColor
    | ColumnRuleStyle
    | ColumnRuleWidth
    | ColumnSpan
    | ColumnWidth
    | Columns
    | Content
    | CounterIncrement
    | CounterReset
    | CssFloat
    | CssText
    | Cursor
    | Direction
    | Display
    | DominantBaseline
    | EmptyCells
    | EnableBackground
    | Fill
    | FillOpacity
    | FillRule
    | Filter
    | Flex
    | FlexBasis
    | FlexDirection
    | FlexFlow
    | FlexGrow
    | FlexShrink
    | FlexWrap
    | FloodColor
    | FloodOpacity
    | Font
    | FontFamily
    | FontFeatureSettings
    | FontSize
    | FontSizeAdjust
    | FontStretch
    | FontStyle
    | FontVariant
    | FontWeight
    | GlyphOrientationHorizontal
    | GlyphOrientationVertical
    | Height
    | ImeMode
    | JustifyContent
    | Kerning
    | Left
    | LetterSpacing
    | LightingColor
    | LineHeight
    | ListStyle
    | ListStyleImage
    | ListStylePosition
    | ListStyleType
    | Margin
    | MarginBottom
    | MarginLeft
    | MarginRight
    | MarginTop
    | Marker
    | MarkerEnd
    | MarkerMid
    | MarkerStart
    | Mask
    | MaxHeight
    | MaxWidth
    | MinHeight
    | MinWidth
    | MsContentZoomChaining
    | MsContentZoomLimit
    | MsContentZoomLimitMax
    | MsContentZoomLimitMin
    | MsContentZoomSnap
    | MsContentZoomSnapPoints
    | MsContentZoomSnapType
    | MsContentZooming
    | MsFlowFrom
    | MsFlowInto
    | MsFontFeatureSettings
    | MsGridColumn
    | MsGridColumnAlign
    | MsGridColumnSpan
    | MsGridColumns
    | MsGridRow
    | MsGridRowAlign
    | MsGridRowSpan
    | MsGridRows
    | MsHighContrastAdjust
    | MsHyphenateLimitChars
    | MsHyphenateLimitLines
    | MsHyphenateLimitZone
    | MsHyphens
    | MsImeAlign
    | MsOverflowStyle
    | MsScrollChaining
    | MsScrollLimit
    | MsScrollLimitXMax
    | MsScrollLimitXMin
    | MsScrollLimitYMax
    | MsScrollLimitYMin
    | MsScrollRails
    | MsScrollSnapPointsX
    | MsScrollSnapPointsY
    | MsScrollSnapType
    | MsScrollSnapX
    | MsScrollSnapY
    | MsScrollTranslation
    | MsTextCombineHorizontal
    | MsTextSizeAdjust
    | MsTouchAction
    | MsTouchSelect
    | MsUserSelect
    | MsWrapFlow
    | MsWrapMargin
    | MsWrapThrough
    | Opacity
    | Order
    | Orphans
    | Outline
    | OutlineColor
    | OutlineStyle
    | OutlineWidth
    | Overflow
    | OverflowX
    | OverflowY
    | Padding
    | PaddingBottom
    | PaddingLeft
    | PaddingRight
    | PaddingTop
    | PageBreakAfter
    | PageBreakBefore
    | PageBreakInside
    | Perspective
    | PerspectiveOrigin
    | PointerEvents
    | Position
    | Quotes
    | Resize
    | Right
    | RubyAlign
    | RubyOverhang
    | RubyPosition
    | StopColor
    | StopOpacity
    | Stroke
    | StrokeDasharray
    | StrokeDashoffset
    | StrokeLinecap
    | StrokeLinejoin
    | StrokeMiterlimit
    | StrokeOpacity
    | StrokeWidth
    | TableLayout
    | TextAlign
    | TextAlignLast
    | TextAnchor
    | TextDecoration
    | TextIndent
    | TextJustify
    | TextKashida
    | TextKashidaSpace
    | TextOverflow
    | TextShadow
    | TextTransform
    | TextUnderlinePosition
    | Top
    | TouchAction
    | Transform
    | TransformOrigin
    | TransformStyle
    | Transition
    | TransitionDelay
    | TransitionDuration
    | TransitionProperty
    | TransitionTimingFunction
    | UnicodeBidi
    | VerticalAlign
    | Visibility
    | WebkitAlignContent
    | WebkitAlignItems
    | WebkitAlignSelf
    | WebkitAnimation
    | WebkitAnimationDelay
    | WebkitAnimationDirection
    | WebkitAnimationDuration
    | WebkitAnimationFillMode
    | WebkitAnimationIterationCount
    | WebkitAnimationName
    | WebkitAnimationPlayState
    | WebkitAnimationTimingFunction
    | WebkitAppearance
    | WebkitBackfaceVisibility
    | WebkitBackgroundClip
    | WebkitBackgroundOrigin
    | WebkitBackgroundSize
    | WebkitBorderBottomLeftRadius
    | WebkitBorderBottomRightRadius
    | WebkitBorderImage
    | WebkitBorderRadius
    | WebkitBorderTopLeftRadius
    | WebkitBorderTopRightRadius
    | WebkitBoxAlign
    | WebkitBoxDirection
    | WebkitBoxFlex
    | WebkitBoxOrdinalGroup
    | WebkitBoxOrient
    | WebkitBoxPack
    | WebkitBoxSizing
    | WebkitColumnBreakAfter
    | WebkitColumnBreakBefore
    | WebkitColumnBreakInside
    | WebkitColumnCount
    | WebkitColumnGap
    | WebkitColumnRule
    | WebkitColumnRuleColor
    | WebkitColumnRuleStyle
    | WebkitColumnRuleWidth
    | WebkitColumnSpan
    | WebkitColumnWidth
    | WebkitColumns
    | WebkitFilter
    | WebkitFlex
    | WebkitFlexBasis
    | WebkitFlexDirection
    | WebkitFlexFlow
    | WebkitFlexGrow
    | WebkitFlexShrink
    | WebkitFlexWrap
    | WebkitJustifyContent
    | WebkitOrder
    | WebkitPerspective
    | WebkitPerspectiveOrigin
    | WebkitTapHighlightColor
    | WebkitTextFillColor
    | WebkitTextSizeAdjust
    | WebkitTransform
    | WebkitTransformOrigin
    | WebkitTransformStyle
    | WebkitTransition
    | WebkitTransitionDelay
    | WebkitTransitionDuration
    | WebkitTransitionProperty
    | WebkitTransitionTimingFunction
    | WebkitUserModify
    | WebkitUserSelect
    | WebkitWritingMode
    | WhiteSpace
    | Widows
    | Width
    | WordBreak
    | WordSpacing
    | WordWrap
    | WritingMode
    | ZIndex
    | Zoom

type [<AllowNullLiteral>] ElementEventMap =
    abstract onabort: UIEvent with get, set
    abstract onauxclick: MouseEvent with get, set
    abstract onblur: FocusEvent with get, set
    abstract oncanplay: Event with get, set
    abstract oncanplaythrough: Event with get, set
    abstract onchange: Event with get, set
    abstract onclick: MouseEvent with get, set
    abstract oncontextmenu: PointerEvent with get, set
    abstract oncopy: ClipboardEvent with get, set
    abstract oncuechange: Event with get, set
    abstract oncut: ClipboardEvent with get, set
    abstract ondblclick: MouseEvent with get, set
    abstract ondrag: DragEvent with get, set
    abstract ondragend: DragEvent with get, set
    abstract ondragenter: DragEvent with get, set
    abstract ondragexit: DragEvent with get, set
    abstract ondragleave: DragEvent with get, set
    abstract ondragover: DragEvent with get, set
    abstract ondragstart: DragEvent with get, set
    abstract ondrop: DragEvent with get, set
    abstract ondurationchange: Event with get, set
    abstract onemptied: Event with get, set
    abstract onended: Event with get, set
    // abstract onended: MediaStreamErrorEvent with get, set
    abstract onerror: ErrorEvent with get, set
    abstract onfocus: FocusEvent with get, set
    abstract oninput: Event with get, set
    abstract oninvalid: Event with get, set
    abstract onkeydown: KeyboardEvent with get, set
    abstract onkeypress: KeyboardEvent with get, set
    abstract onkeyup: KeyboardEvent with get, set
    abstract onload: Event with get, set
    abstract onloadeddata: Event with get, set
    abstract onloadedmetadata: Event with get, set
    abstract onloadend: Event with get, set
    abstract onloadstart: Event with get, set
    abstract onmousedown: MouseEvent with get, set
    abstract onmouseenter: MouseEvent with get, set
    abstract onmouseleave: MouseEvent with get, set
    abstract onmousemove: MouseEvent with get, set
    abstract onmouseout: MouseEvent with get, set
    abstract onmouseover: MouseEvent with get, set
    abstract onmouseup: MouseEvent with get, set
    abstract onmousewheel: WheelEvent with get, set
    abstract onpaste: ClipboardEvent with get, set
    abstract onpause: Event with get, set
    abstract onplay: Event with get, set
    abstract onplaying: Event with get, set
    abstract onpointercancel: PointerEvent with get, set
    abstract onpointerdown: PointerEvent with get, set
    abstract onpointerenter: PointerEvent with get, set
    abstract onpointerleave: PointerEvent with get, set
    abstract onpointermove: PointerEvent with get, set
    abstract onpointerout: PointerEvent with get, set
    abstract onpointerover: PointerEvent with get, set
    abstract onpointerup: PointerEvent with get, set
    abstract onprogress: ProgressEvent with get, set
    abstract onratechange: Event with get, set
    abstract onreset: Event with get, set
    abstract onscroll: UIEvent with get, set
    abstract onseeked: Event with get, set
    abstract onseeking: Event with get, set
    abstract onselect: UIEvent with get, set
    abstract onselectstart: Event with get, set
    abstract onstalled: Event with get, set
    abstract onsubmit: Event with get, set
    abstract onsuspend: Event with get, set
    abstract ontimeupdate: Event with get, set
    abstract onvolumechange: Event with get, set
    abstract onwaiting: Event with get, set

type [<AllowNullLiteral>] ElementDataset =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: name: string -> string

type [<AllowNullLiteral>] ElementInlineStyle =
    interface end

type [<AllowNullLiteral>] ElementBaseAttrs =
    interface end

type [<AllowNullLiteral>] ElementEventAttrs =
    interface end

type [<AllowNullLiteral>] ElementSpecialAttrs =
    /// The key id for the virtual element node.
    /// 
    /// If a node is given a key id, the generated DOM node will not be
    /// recreated during a rendering update if it only moves among its
    /// siblings in the render tree.
    /// 
    /// In general, reordering child nodes will cause the nodes to be
    /// completely re-rendered. Keys allow this to be optimized away.
    /// 
    /// If a key is provided, it must be unique among sibling nodes.
    abstract key: string option
    /// The JS-safe name for the HTML `class` attribute.
    abstract className: string option
    /// The JS-safe name for the HTML `for` attribute.
    abstract htmlFor: string option
    /// The dataset for the rendered DOM element.
    abstract dataset: ElementDataset option
    /// The inline style for the rendered DOM element.
    abstract style: ElementInlineStyle option

type [<AllowNullLiteral>] ElementAttrs =
    interface end

/// A virtual node which represents plain text content.
/// 
/// #### Notes
/// User code will not typically create a `VirtualText` node directly.
/// Instead, the `h()` function will be used to create an element tree.
type [<AllowNullLiteral>] VirtualText =
    /// The text content for the node.
    abstract content: string
    /// The type of the node.
    /// 
    /// This value can be used as a type guard for discriminating the
    /// `VirtualNode` union type.
    abstract ``type``: string

/// A virtual node which represents plain text content.
/// 
/// #### Notes
/// User code will not typically create a `VirtualText` node directly.
/// Instead, the `h()` function will be used to create an element tree.
type [<AllowNullLiteral>] VirtualTextStatic =
    /// <summary>Construct a new virtual text node.</summary>
    /// <param name="content">- The text content for the node.</param>
    [<Emit "new $0($1...)">] abstract Create: content: string -> VirtualText

/// A virtual node which represents an HTML element.
/// 
/// #### Notes
/// User code will not typically create a `VirtualElement` node directly.
/// Instead, the `h()` function will be used to create an element tree.
type [<AllowNullLiteral>] VirtualElement =
    /// The tag name for the element.
    abstract tag: string
    /// The attributes for the element.
    abstract attrs: ElementAttrs
    /// The children for the element.
    abstract children: ReadonlyArray<VirtualNode>
    /// The type of the node.
    /// 
    /// This value can be used as a type guard for discriminating the
    /// `VirtualNode` union type.
    abstract ``type``: string

/// A virtual node which represents an HTML element.
/// 
/// #### Notes
/// User code will not typically create a `VirtualElement` node directly.
/// Instead, the `h()` function will be used to create an element tree.
type [<AllowNullLiteral>] VirtualElementStatic =
    /// <summary>Construct a new virtual element node.</summary>
    /// <param name="tag">- The element tag name.</param>
    /// <param name="attrs">- The element attributes.</param>
    /// <param name="children">- The element children.</param>
    [<Emit "new $0($1...)">] abstract Create: tag: string * attrs: ElementAttrs * children: ResizeArray<VirtualNode> -> VirtualElement

type VirtualNode =
    U2<VirtualElement, VirtualText>

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module VirtualNode =
    let ofVirtualElement v: VirtualNode = v |> U2.Case1
    let isVirtualElement (v: VirtualNode) = match v with U2.Case1 _ -> true | _ -> false
    let asVirtualElement (v: VirtualNode) = match v with U2.Case1 o -> Some o | _ -> None
    let ofVirtualText v: VirtualNode = v |> U2.Case2
    let isVirtualText (v: VirtualNode) = match v with U2.Case2 _ -> true | _ -> false
    let asVirtualText (v: VirtualNode) = match v with U2.Case2 o -> Some o | _ -> None

module H =

    type [<AllowNullLiteral>] IExports =
        abstract a: IFactory
        abstract abbr: IFactory
        abstract address: IFactory
        abstract area: IFactory
        abstract article: IFactory
        abstract aside: IFactory
        abstract audio: IFactory
        abstract b: IFactory
        abstract bdi: IFactory
        abstract bdo: IFactory
        abstract blockquote: IFactory
        abstract br: IFactory
        abstract button: IFactory
        abstract canvas: IFactory
        abstract caption: IFactory
        abstract cite: IFactory
        abstract code: IFactory
        abstract col: IFactory
        abstract colgroup: IFactory
        abstract data: IFactory
        abstract datalist: IFactory
        abstract dd: IFactory
        abstract del: IFactory
        abstract dfn: IFactory
        abstract div: IFactory
        abstract dl: IFactory
        abstract dt: IFactory
        abstract em: IFactory
        abstract embed: IFactory
        abstract fieldset: IFactory
        abstract figcaption: IFactory
        abstract figure: IFactory
        abstract footer: IFactory
        abstract form: IFactory
        abstract h1: IFactory
        abstract h2: IFactory
        abstract h3: IFactory
        abstract h4: IFactory
        abstract h5: IFactory
        abstract h6: IFactory
        abstract header: IFactory
        abstract hr: IFactory
        abstract i: IFactory
        abstract iframe: IFactory
        abstract img: IFactory
        abstract input: IFactory
        abstract ins: IFactory
        abstract kbd: IFactory
        abstract label: IFactory
        abstract legend: IFactory
        abstract li: IFactory
        abstract main: IFactory
        abstract map: IFactory
        abstract mark: IFactory
        abstract meter: IFactory
        abstract nav: IFactory
        abstract noscript: IFactory
        abstract ``object``: IFactory
        abstract ol: IFactory
        abstract optgroup: IFactory
        abstract option: IFactory
        abstract output: IFactory
        abstract p: IFactory
        abstract param: IFactory
        abstract pre: IFactory
        abstract progress: IFactory
        abstract q: IFactory
        abstract rp: IFactory
        abstract rt: IFactory
        abstract ruby: IFactory
        abstract s: IFactory
        abstract samp: IFactory
        abstract section: IFactory
        abstract select: IFactory
        abstract small: IFactory
        abstract source: IFactory
        abstract span: IFactory
        abstract strong: IFactory
        abstract sub: IFactory
        abstract summary: IFactory
        abstract sup: IFactory
        abstract table: IFactory
        abstract tbody: IFactory
        abstract td: IFactory
        abstract textarea: IFactory
        abstract tfoot: IFactory
        abstract th: IFactory
        abstract thead: IFactory
        abstract time: IFactory
        abstract title: IFactory
        abstract tr: IFactory
        abstract track: IFactory
        abstract u: IFactory
        abstract ul: IFactory
        abstract var_: IFactory
        abstract video: IFactory
        abstract wbr: IFactory

    type Child =
        U2<U2<string, VirtualNode> option, Array<U2<string, VirtualNode> option>>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Child =
        let ofCase1 v: Child = v |> U2.Case1
        let isCase1 (v: Child) = match v with U2.Case1 _ -> true | _ -> false
        let asCase1 (v: Child) = match v with U2.Case1 o -> Some o | _ -> None
        let ofArray v: Child = v |> U2.Case2
        let isArray (v: Child) = match v with U2.Case2 _ -> true | _ -> false
        let asArray (v: Child) = match v with U2.Case2 o -> Some o | _ -> None

    /// A bound factory function for a specific `h()` tag.
    type [<AllowNullLiteral>] IFactory =
        [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] children: ResizeArray<Child> -> VirtualElement
        [<Emit "$0($1...)">] abstract Invoke: attrs: ElementAttrs * [<ParamArray>] children: ResizeArray<Child> -> VirtualElement

module VirtualDOM =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Create a real DOM element from a virtual element node.</summary>
        /// <param name="node">- The virtual element node to realize.</param>
        abstract realize: node: VirtualElement -> HTMLElement
        /// <summary>Render virtual DOM content into a host element.</summary>
        /// <param name="content">- The virtual DOM content to render.</param>
        /// <param name="host">- The host element for the rendered content.
        /// 
        /// #### Notes
        /// This renders the delta from the previous rendering. It assumes that
        /// the content of the host element is not manipulated by external code.
        /// 
        /// Providing `null` content will clear the rendering.
        /// 
        /// Externally modifying the provided content or the host element will
        /// result in undefined rendering behavior.</param>
        abstract render: content: U2<VirtualNode, ResizeArray<VirtualNode>> option * host: HTMLElement -> unit
