namespace rec FSharp.Compiler.SyntaxTreeX

type Ident = string
type LongIdent = string list

[<RequireQualifiedAccess>]
type TyparStaticReq =
    | None
    | HeadType

type XTypar =
    | XTypar of ident: Ident * staticReq: TyparStaticReq * isCompGen: bool

[<RequireQualifiedAccess>]
type XStringKind =
    | Regular
    | Verbatim
    | TripleQuote

[<RequireQualifiedAccess>]
type XByteStringKind =
    | Regular
    | Verbatim

[<RequireQualifiedAccess>]
type XAccess =
    | Public
    | Internal
    | Private

[<RequireQualifiedAccess>]
type XConst =
    | Unit
    | Bool of bool
    | SByte of sbyte
    | Byte of byte
    | Int16 of int16
    | UInt16 of uint16
    | Int32 of int32
    | UInt32 of uint32
    | Int64 of int64
    | UInt64 of uint64
    | IntPtr of int64
    | UIntPtr of uint64
    | Single of single
    | Double of double
    | Char of char
    | Decimal of System.Decimal
    | UserNum of value: string * suffix: string
    | String of text: string * XStringKind: XStringKind
    | Bytes of bytes: byte[] * XByteStringKind: XByteStringKind
    | UInt16s of uint16[]
    | Measure of constant: XConst * XMeasure
    | SourceIdentifier of constant: string * value: string

[<RequireQualifiedAccess>]
type XMeasure =
    | Named of longId: LongIdent
    | Product of measure1: XMeasure * measure2: XMeasure
    | Seq of measures: XMeasure list
    | Divide of measure1: XMeasure * measure2: XMeasure
    | Power of measure: XMeasure * power: XRationalConst
    | One
    | Anon
    | Var of typar: XTypar
    | Paren of measure: XMeasure

[<RequireQualifiedAccess>]
type XRationalConst =
    | Integer of value: int32
    | Rational of numerator: int32 * denominator: int32
    | Negate of XRationalConst

type SeqExprOnly = SeqExprOnly of bool

type RecordFieldName = LongIdent * bool

type ExprAtomicFlag =
    | Atomic = 0
    | NonAtomic = 1

[<RequireQualifiedAccess>]
type XBindingKind =
    | StandaloneExpression
    | Normal
    | Do

type XTyparDecl = XTyparDecl of attributes: XAttributes * XTypar

[<RequireQualifiedAccess>]
type XTypeConstraint =
    | WhereTyparIsValueType of typar: XTypar
    | WhereTyparIsReferenceType of typar: XTypar
    | WhereTyparIsUnmanaged of typar: XTypar
    | WhereTyparSupportsNull of typar: XTypar
    | WhereTyparIsComparable of typar: XTypar
    | WhereTyparIsEquatable of typar: XTypar
    | WhereTyparDefaultsToType of typar: XTypar * typeName: XType
    | WhereTyparSubtypeOfType of typar: XTypar * typeName: XType
    | WhereTyparSupportsMember of typars: XType list * memberSig: XMemberSig
    | WhereTyparIsEnum of typar: XTypar * typeArgs: XType list
    | WhereTyparIsDelegate of typar: XTypar * typeArgs: XType list

[<RequireQualifiedAccess>]
type XTyparDecls =
    | PostfixList of decls: XTyparDecl list * constraints: XTypeConstraint list
    | PrefixList of decls: XTyparDecl list
    | SinglePrefix of decl: XTyparDecl

[<RequireQualifiedAccess>]
type XTupleTypeSegment =
    | Type of typeName: XType
    | Star // of range: range
    | Slash // of range: range

[<RequireQualifiedAccess>]
type XType =
    | LongIdent of longDotId: LongIdent
    | App of
        typeName: XType *
        typeArgs: XType list *
        isPostfix: bool
    | LongIdentApp of
        typeName: XType *
        longDotId: LongIdent *
        typeArgs: XType list
    | Tuple of isStruct: bool * path: XTupleTypeSegment list
    | AnonRecd of isStruct: bool * fields: (Ident * XType) list
    | Array of rank: int * elementType: XType
    | Fun of argType: XType * returnType: XType
    | Var of typar: XTypar
    | Anon
    | WithGlobalConstraints of typeName: XType * constraints: XTypeConstraint list
    | HashConstraint of innerType: XType
    | MeasureDivide of dividend: XType * divisor: XType
    | MeasurePower of baseMeasure: XType * exponent: XRationalConst
    | StaticConstant of constant: XConst
    | StaticConstantExpr of expr: XExpr
    | StaticConstantNamed of ident: XType * value: XType
    | Paren of innerType: XType

[<RequireQualifiedAccess>]
type XExpr =
    | Paren of expr: XExpr
    | Quote of operator: XExpr * isRaw: bool * quotedExpr: XExpr * isFromQueryExpression: bool
    | Const of constant: XConst
    | Typed of expr: XExpr * targetType: XType
    | Tuple of
        exprs: XExpr list
    | AnonRecd of
        recordFields: (Ident * XExpr) list
    | ArrayOrList of isArray: bool * exprs: XExpr list
    | Record of
        recordFields: XExprRecordField list
    | New of 
        targetType: XType * 
        expr: XExpr
    | ObjExpr of
        objType: XType *
        argOptions: (XExpr * Ident option) option *
        bindings: XBinding list *
        members: XMemberDefn list *
        extraImpls: XInterfaceImpl list
    | While of  whileExpr: XExpr * doExpr: XExpr
    | For of
        ident: Ident *
        identBody: XExpr *
        direction: bool *
        toBody: XExpr *
        doBody: XExpr
    | ForEach of
        seqExprOnly: SeqExprOnly *
        isFromSource: bool *
        pat: XPat *
        enumExpr: XExpr *
        bodyExpr: XExpr
    | ArrayOrListComputed of isArray: bool * expr: XExpr
    | IndexRange of expr1: XExpr option  * expr2: XExpr option
    | IndexFromEnd of expr: XExpr
    | ComputationExpr of hasSeqBuilder: bool * expr: XExpr
    | Lambda of
        //fromMethod: bool *
        //inLambdaSeq: bool *
        args: XSimplePats *
        body: XExpr //*
        //parsedData: (XPat list * XExpr) option
    | MatchLambda of
        isExnMatch: bool *
        matchClauses: XMatchClause list
    | Match of
        expr: XExpr *
        clauses: XMatchClause list
    | Do of expr: XExpr
    | Assert of expr: XExpr
    | App of 
        //flag: ExprAtomicFlag * 
        //isInfix: bool * 
        funcExpr: XExpr * 
        argExpr: XExpr
    | TypeApp of
        expr: XExpr *
        typeArgs: XType list
    | LetOrUse of 
        isRecursive: bool * 
        isUse: bool * 
        bindings: XBinding list * 
        body: XExpr
    | TryWith of
        tryExpr: XExpr *
        withCases: XMatchClause list
    | TryFinally of
        tryExpr: XExpr *
        finallyExpr: XExpr
    | Lazy of expr: XExpr
    | Sequential of  
        isTrueSeq: bool * 
        expr1: XExpr * 
        expr2: XExpr
    | IfThenElse of
        ifExpr: XExpr *
        thenExpr: XExpr *
        elseExpr: XExpr option *
        isFromErrorRecovery: bool
    | Ident of ident: Ident
    | LongIdent of isOptional: bool * longDotId: LongIdent * altNameRefCell: XSimplePatAlternativeIdInfo option
    | LongIdentSet of longDotId: LongIdent * expr: XExpr
    | DotGet of expr: XExpr  * longDotId: LongIdent
    | DotSet of targetExpr: XExpr * longDotId: LongIdent * rhsExpr: XExpr
    | Set of targetExpr: XExpr * rhsExpr: XExpr
    | DotIndexedGet of objectExpr: XExpr * indexArgs: XExpr
    | DotIndexedSet of
        objectExpr: XExpr *
        indexArgs: XExpr *
        valueExpr: XExpr
    | NamedIndexedPropertySet of longDotId: LongIdent * expr1: XExpr * expr2: XExpr
    | DotNamedIndexedPropertySet of targetExpr: XExpr * longDotId: LongIdent * argExpr: XExpr * rhsExpr: XExpr
    | TypeTest of expr: XExpr * targetType: XType
    | Upcast of expr: XExpr * targetType: XType
    | Downcast of expr: XExpr * targetType: XType
    | InferredUpcast of expr: XExpr
    | InferredDowncast of expr: XExpr
    | Null
    | AddressOf of isByref: bool * expr: XExpr
    | TraitCall of supportTys: XType list * traitSig: XMemberSig * argExpr: XExpr
    | JoinIn of lhsExpr: XExpr  * rhsExpr: XExpr
    | ImplicitZero
    | SequentialOrImplicitYield of  expr1: XExpr * expr2: XExpr * ifNotStmt: XExpr
    | YieldOrReturn of flags: (bool * bool) * expr: XExpr
    | YieldOrReturnFrom of flags: (bool * bool) * expr: XExpr
    | LetOrUseBang of
        isUse: bool *
        isFromSource: bool *
        pat: XPat *
        rhs: XExpr *
        andBangs: XExprAndBang list *
        body: XExpr
    | MatchBang of
        expr: XExpr *
        clauses: XMatchClause list
    | DoBang of expr: XExpr
    | LibraryOnlyILAssembly
    | LibraryOnlyStaticOptimization
    | LibraryOnlyUnionCaseFieldGet
    | LibraryOnlyUnionCaseFieldSet
    | ArbitraryAfterError
    | FromParseError
    | DiscardAfterMissingQualificationAfterDot
    | Fixed of expr: XExpr
    | InterpolatedString of contents: XInterpolatedStringPart list * XStringKind: XStringKind
    | DebugPoint
    | Dynamic of funcExpr: XExpr  * argExpr: XExpr

type XExprAndBang =
    | XExprAndBang of
        isUse: bool *
        isFromSource: bool *
        pat: XPat *
        body: XExpr

type XExprRecordField =
    | XExprRecordField of
        fieldName: RecordFieldName *
        expr: XExpr option

[<RequireQualifiedAccess>]
type XInterpolatedStringPart =
    | String of value: string
    | FillExpr of fillExpr: XExpr * qualifiers: Ident option

[<RequireQualifiedAccess>]
type XSimplePat =
    | Id of
        ident: Ident *
        altNameRefCell: XSimplePatAlternativeIdInfo option *
        isCompilerGenerated: bool *
        isThisVal: bool *
        isOptional: bool
    | Typed of pat: XSimplePat * targetType: XType
    | Attrib of pat: XSimplePat * attributes: XAttributes

[<RequireQualifiedAccess>]
type XSimplePatAlternativeIdInfo =
    | Undecided of Ident
    | Decided of Ident

[<RequireQualifiedAccess>]
type XStaticOptimizationConstraint =
    | WhenTyparTyconEqualsTycon of typar: XTypar * rhsType: XType
    | WhenTyparIsStruct of typar: XTypar

[<RequireQualifiedAccess>]
type XSimplePats =
    | SimplePats of pats: XSimplePat list
    | Typed of pats: XSimplePats * targetType: XType

[<RequireQualifiedAccess>]
type XArgPats =
    | Pats of pats: XPat list
    | NamePatPairs of pats: (Ident * XPat) list

[<RequireQualifiedAccess>]
type XPat =
    | Const of constant: XConst
    | Wild
    | Named of ident: Ident * isThisVal: bool * accessibility: XAccess option
    | Typed of pat: XPat * targetType: XType
    | Attrib of pat: XPat * attributes: XAttributes
    | Or of lhsPat: XPat * rhsPat: XPat
    | Ands of pats: XPat list
    | As of lhsPat: XPat * rhsPat: XPat
    | LongIdent of
        longDotId: LongIdent *
        extraId: Ident option *
        typarDecls: XValTyparDecls option *
        argPats: XArgPats *
        accessibility: XAccess option
    | Tuple of isStruct: bool * elementPats: XPat list
    | Paren of pat: XPat
    | ArrayOrList of isArray: bool * elementPats: XPat list
    | Record of fieldPats: ((LongIdent * Ident) * XPat) list
    | Null
    | OptionalVal of ident: Ident
    | IsInst of pat: XType
    | QuoteExpr of expr: XExpr
    | DeprecatedCharRange of startChar: char * endChar: char
    | InstanceMember of
        thisId: Ident *
        memberId: Ident *
        toolingId: Ident option *
        accessibility: XAccess option
    | FromParseError of pat: XPat

[<RequireQualifiedAccess>]
type PropertyKeyword =
    | With
    | And

type XInterfaceImpl =
    | XInterfaceImpl of
        interfaceTy: XType *
        bindings: XBinding list *
        members: XMemberDefn list

type XMatchClause =
    | XMatchClause of
        pat: XPat *
        whenExpr: XExpr option *
        resultExpr: XExpr

[<RequireQualifiedAccess>]
type XAttribute =
    {
        TypeName: LongIdent
        ArgExpr: XExpr
        Target: Ident option
        AppliesToGetterAndSetter: bool
    }

[<RequireQualifiedAccess>]
type XAttributeList =
    {
        Attributes: XAttribute list
    }

type XAttributes = XAttributeList list


type XValData =
    | XValData of 
        memberFlags: XMemberFlags option * 
        valInfo: XValInfo * 
        thisIdOpt: Ident option

type XBinding =
    | XBinding of
        //accessibility: XAccess option *
        kind: XBindingKind *
        //isInline: bool *
        //isMutable: bool *
        modifiers:string list *
        attributes: XAttributes *
        //valData: XValData *
        headPat: XPat *
        returnInfo: XBindingReturnInfo option *
        expr: XExpr

type XBindingReturnInfo = 
    XBindingReturnInfo of 
        typeName: XType  * 
        attributes: XAttributes

type XMemberFlags =
    {
        IsInstance: bool
        IsDispatchSlot: bool
        IsOverrideOrExplicitImpl: bool
        IsFinal: bool
        MemberKind: XMemberKind
    }


[<RequireQualifiedAccess>]
type XMemberKind =
    | ClassConstructor
    | Constructor
    | Member
    | PropertyGet
    | PropertySet
    | PropertyGetSet

[<RequireQualifiedAccess>]
type XMemberSig =
    | Member of memberSig: XValSig * flags: XMemberFlags
    | Interface of interfaceType: XType
    | Inherit of inheritedType: XType
    | ValField of field: XField
    | NestedType of nestedType: XTypeDefnSig

[<RequireQualifiedAccess>]
type XTypeDefnKind =
    | Unspecified
    | Class
    | Interface
    | Struct
    | Record
    | Union
    | Abbrev
    | Opaque
    | Augmentation
    | IL
    | Delegate of signature: XType * signatureInfo: XValInfo

[<RequireQualifiedAccess>]
type XTypeDefnSimpleRepr =
    | Union of accessibility: XAccess option * unionCases: XUnionCase list
    | Enum of cases: XEnumCase list
    | Record of accessibility: XAccess option * recordFields: XField list
    | General of
        kind: XTypeDefnKind *
        inherits: (XType * Ident option) list *
        slotsigs: (XValSig * XMemberFlags) list *
        fields: XField list *
        isConcrete: bool *
        isIncrClass: bool *
        implicitCtorXPats: XSimplePats option
    | LibraryOnlyILAssembly
    | TypeAbbrev of 
        //detail: ParserDetail * 
        rhsType: XType
    | None
    | Exception of exnRepr: XExceptionDefnRepr

type XEnumCase =
    | XEnumCase of
        attributes: XAttributes *
        ident: Ident *
        value: XConst

type XUnionCase =
    | XUnionCase of
        attributes: XAttributes *
        ident: Ident *
        caseType: XUnionCaseKind *
        accessibility: XAccess option

[<RequireQualifiedAccess>]
type XUnionCaseKind =
    | Fields of cases: XField list
    | FullType of fullType: XType * fullTypeInfo: XValInfo

[<RequireQualifiedAccess>]
type XTypeDefnSigRepr =
    | ObjectModel of kind: XTypeDefnKind * memberSigs: XMemberSig list
    | Simple of repr: XTypeDefnSimpleRepr
    | Exception of repr: XExceptionDefnRepr

type XTypeDefnSig =
    | XTypeDefnSig of
        typeInfo: XComponentInfo *
        typeRepr: XTypeDefnSigRepr *
        members: XMemberSig list

type XField =
    | XField of
        attributes: XAttributes *
        isStatic: bool *
        idOpt: Ident option *
        fieldType: XType *
        isMutable: bool *
        accessibility: XAccess option

type XComponentInfo =
    | XComponentInfo of
        attributes: XAttributes *
        typeParams: XTyparDecls option *
        constraints: XTypeConstraint list *
        longId: LongIdent *
        preferPostfix: bool *
        accessibility: XAccess option

type XValSig =
    | XValSig of
        attributes: XAttributes *
        ident: Ident *
        explicitTypeParams: XValTyparDecls *
        XType: XType *
        arity: XValInfo *
        isInline: bool *
        isMutable: bool *
        accessibility: XAccess option *
        XExpr: XExpr option

type XValInfo =
    | XValInfo of curriedArgInfos: XArgInfo list list * returnInfo: XArgInfo

type XArgInfo =
    | XArgInfo of attributes: XAttributes * optional: bool * ident: Ident option

type XValTyparDecls = XValTyparDecls of typars: XTyparDecls option * canInfer: bool

type XReturnInfo = XReturnInfo of returnType: (XType * XArgInfo)


type XExceptionDefnRepr =
    | XExceptionDefnRepr of
        attributes: XAttributes *
        caseName: XUnionCase *
        longId: LongIdent option *
        accessibility: XAccess option

type XExceptionDefn =
    | XExceptionDefn of exnRepr: XExceptionDefnRepr  * members: XMemberDefns


[<RequireQualifiedAccess>]
type XTypeDefnRepr =
    | ObjectModel of kind: XTypeDefnKind * members: XMemberDefns
    | Simple of simpleRepr: XTypeDefnSimpleRepr
    | Exception of exnRepr: XExceptionDefnRepr

type XTypeDefn =
    | XTypeDefn of
        typeInfo: XComponentInfo *
        typeRepr: XTypeDefnRepr *
        members: XMemberDefns *
        implicitConstructor: XMemberDefn option

[<RequireQualifiedAccess>]
type XMemberDefn =
    | Open of target: XOpenDeclTarget
    | Member of memberDefn: XBinding
    | ImplicitCtor of
        accessibility: XAccess option *
        attributes: XAttributes *
        ctorArgs: XSimplePats *
        selfIdentifier: Ident option
    | ImplicitInherit of inheritType: XType * inheritArgs: XExpr * inheritAlias: Ident option
    | LetBindings of bindings: XBinding list * isStatic: bool * isRecursive: bool
    | AbstractSlot of slotSig: XValSig * flags: XMemberFlags
    | Interface of interfaceType: XType  * members: XMemberDefns option
    | Inherit of baseType: XType * asIdent: Ident option
    | ValField of fieldInfo: XField
    | NestedType of typeDefn: XTypeDefn * accessibility: XAccess option
    | AutoProperty of
        attributes: XAttributes *
        isStatic: bool *
        ident: Ident *
        typeOpt: XType option *
        propKind: XMemberKind *
        accessibility: XAccess option *
        XExpr: XExpr

type XMemberDefns = XMemberDefn list

[<RequireQualifiedAccess>]
type XModuleDecl =
    | ModuleAbbrev of ident: Ident * longId: LongIdent
    | NestedModule of
        moduleInfo: XComponentInfo *
        isRecursive: bool *
        decls: XModuleDecl list *
        isContinuing: bool
    | Let of 
        modifiers:string list *
        //isRecursive: bool * 
        bindings: XBinding list
    | Expr of expr: XExpr
    | Types of typeDefns: XTypeDefn list
    | Exception of exnDefn: XExceptionDefn
    | Open of target: XOpenDeclTarget
    | Attributes of attributes: XAttributes
    | HashDirective 
        //of hashDirective: ParsedHashDirective
    | NamespaceFragment of fragment: XModuleOrNamespace

[<RequireQualifiedAccess>]
type XOpenDeclTarget =
    | ModuleOrNamespace of longId: LongIdent
    | Type of typeName: XType

type XExceptionSig =
    | XExceptionSig of exnRepr: XExceptionDefnRepr  * members: XMemberSig list

[<RequireQualifiedAccess>]
type XModuleSigDecl =
    | ModuleAbbrev of ident: Ident * longId: LongIdent
    | NestedModule of
        moduleInfo: XComponentInfo *
        isRecursive: bool *
        moduleDecls: XModuleSigDecl list
    | Val of valSig: XValSig
    | Types of types: XTypeDefnSig list
    | Exception of exnSig: XExceptionSig
    | Open of target: XOpenDeclTarget
    | HashDirective 
        //of hashDirective: ParsedHashDirective
    | NamespaceFragment of XModuleOrNamespaceSig

[<RequireQualifiedAccess>]
type XModuleOrNamespaceKind =
    | NamedModule
    | AnonModule
    | DeclaredNamespace
    | GlobalNamespace

type XModuleOrNamespace =
    | XModuleOrNamespace of
        longId: LongIdent *
        modifiers: string list *
        //isRecursive: bool *
        kind: XModuleOrNamespaceKind *
        decls: XModuleDecl list *
        attribs: XAttributes
        //accessibility: XAccess option

type XModuleOrNamespaceSig =
    | XModuleOrNamespaceSig of
        longId: LongIdent *
        isRecursive: bool *
        kind: XModuleOrNamespaceKind *
        decls: XModuleSigDecl list *
        attribs: XAttributes *
        accessibility: XAccess option

//[<RequireQualifiedAccess>]
//type ParsedHashDirectiveArgument =
//    | String of value: string * stringKind: XStringKind
//    | SourceIdentifier of constant: string * value: string

//type ParsedHashDirective = ParsedHashDirective of ident: string * args: ParsedHashDirectiveArgument list

//[<RequireQualifiedAccess>]
//type ParsedImplFileFragment =
//    | AnonModule of decls: XModuleDecl list
//    | NamedModule of namedModule: XModuleOrNamespace
//    | NamespaceFragment of
//        longId: LongIdent *
//        isRecursive: bool *
//        kind: XModuleOrNamespaceKind *
//        decls: XModuleDecl list *
//        attributes: XAttributes

//[<RequireQualifiedAccess>]
//type ParsedSigFileFragment =
//    | AnonModule of decls: XModuleSigDecl list
//    | NamedModule of namedModule: XModuleOrNamespaceSig
//    | NamespaceFragment of
//        longId: LongIdent *
//        isRecursive: bool *
//        kind: XModuleOrNamespaceKind *
//        decls: XModuleSigDecl list *
//        attributes: XAttributes


//[<RequireQualifiedAccess>]
//type ParsedScriptInteraction =
//    | Definitions of defns: XModuleDecl list
//    | HashDirective of hashDirective: ParsedHashDirective

//type ParsedImplFile = ParsedImplFile of hashDirectives: ParsedHashDirective list * fragments: ParsedImplFileFragment list

//type ParsedSigFile = ParsedSigFile of hashDirectives: ParsedHashDirective list * fragments: ParsedSigFileFragment list

//[<RequireQualifiedAccess>]
//type ScopedPragma = WarningOff of  warningNumber: int

//type QualifiedNameOfFile =
//    | QualifiedNameOfFile of Ident

//type ParsedImplFileInput =
//    | ParsedImplFileInput of
//        fileName: string *
//        isScript: bool *
//        qualifiedNameOfFile: QualifiedNameOfFile *
//        scopedPragmas: ScopedPragma list *
//        hashDirectives: ParsedHashDirective list *
//        modules: XModuleOrNamespace list *
//        isLastCompiland: (bool * bool)

//type ParsedSigFileInput =
//    | ParsedSigFileInput of
//        fileName: string *
//        qualifiedNameOfFile: QualifiedNameOfFile *
//        scopedPragmas: ScopedPragma list *
//        hashDirectives: ParsedHashDirective list *
//        modules: XModuleOrNamespaceSig list

//[<RequireQualifiedAccess>]
//type ParsedInput =
//    | ImplFile of ParsedImplFileInput
//    | SigFile of ParsedSigFileInput

//[<RequireQualifiedAccess>]
//type ParserDetail =
//    | Ok
//    | ErrorRecovery

