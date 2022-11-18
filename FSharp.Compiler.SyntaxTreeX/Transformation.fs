module rec FSharp.Compiler.SyntaxTreeX.Transformation

open FSharp.Compiler.Syntax
open FSharp.Compiler.SyntaxTrivia
open FSharp.Compiler.Xml
open FSharp.Compiler.Text
open FSharp.Compiler

let getIdent (ident:SynIdent) =
    match ident with SynIdent (ident,_) ->
    ident.idText

let getLongIdent (longId:LongIdent) =
    longId
    |> List.map(fun i -> i.idText)

let getLongIdentFromSyn (longId:SynLongIdent) =
    match longId with
    | SynLongIdent(i,_,_) -> getLongIdent i

let getAccess (accessibility:SynAccess) =
    match accessibility with
    | SynAccess.Public   _ -> XAccess.Public  
    | SynAccess.Internal _ -> XAccess.Internal
    | SynAccess.Private  _ -> XAccess.Private 

let yieldAccess (accessibility:SynAccess option) =
    seq {
        match accessibility with
        | Some (SynAccess.Public   _) -> "public"
        | Some (SynAccess.Internal _) -> "internal"
        | Some (SynAccess.Private  _) -> "private"
        | None -> ()
    }

let getModuleOrNamespaceKind (kind:SynModuleOrNamespaceKind) =
    match kind with
    | SynModuleOrNamespaceKind.AnonModule ->
        XModuleOrNamespaceKind.AnonModule
    | SynModuleOrNamespaceKind.NamedModule ->
        XModuleOrNamespaceKind.NamedModule
    | SynModuleOrNamespaceKind.DeclaredNamespace ->
        XModuleOrNamespaceKind.DeclaredNamespace
    | SynModuleOrNamespaceKind.GlobalNamespace ->
        XModuleOrNamespaceKind.GlobalNamespace

let getModuleOrNamespace (m:SynModuleOrNamespace) =
    match m with
    | SynModuleOrNamespace(        
        longId,
        isRecursive,
        kind,
        decls,
        xmlDoc,
        attribs,
        accessibility,
        range,
        triva
        ) -> 
        let modifiers = [
            if isRecursive then "rec"
            yield! yieldAccess accessibility
        ]
        XModuleOrNamespace(
            getLongIdent longId,
            modifiers,
            getModuleOrNamespaceKind kind,
            List.map getModuleDecl decls,
            getAttributes attribs
            )

let getModuleDecl (d:SynModuleDecl) =
    match d with
    | SynModuleDecl.ModuleAbbrev(ident: Ident, longId: LongIdent, range: range) ->
        XModuleDecl.ModuleAbbrev(
            ident.idText,
            getLongIdent longId
            )
    | SynModuleDecl.NestedModule (
        moduleInfo: SynComponentInfo,
        isRecursive: bool,
        decls: SynModuleDecl list,
        isContinuing: bool,
        range: range,
        trivia: SynModuleDeclNestedModuleTrivia
        ) ->
        XModuleDecl.NestedModule(
            getComponentInfo moduleInfo,
            isRecursive,
            List.map getModuleDecl decls,
            isContinuing
        )
    | SynModuleDecl.Let(
        isRecursive: bool, bindings: SynBinding list, range: range
        ) ->
        XModuleDecl.Let(
            [if isRecursive then "rec"], 
            bindings |> List.map getBinding
        )
    | SynModuleDecl.Expr(expr: SynExpr, range: range) ->
        XModuleDecl.Expr(getExpr expr)
    | SynModuleDecl.Types(typeDefns: SynTypeDefn list, range: range) ->
        XModuleDecl.Types(List.map getTypeDefn typeDefns)
    | SynModuleDecl.Exception(exnDefn: SynExceptionDefn, range: range) ->
        XModuleDecl.Exception(getExceptionDefn exnDefn)
    | SynModuleDecl.Open(target: SynOpenDeclTarget, range: range) ->
        XModuleDecl.Open(getOpenDeclTarget target)
    | SynModuleDecl.Attributes(attributes: SynAttributes, range: range) -> 
        XModuleDecl.Attributes(getAttributes attributes)
    | SynModuleDecl.HashDirective(hashDirective: ParsedHashDirective, range: range)->
        XModuleDecl.HashDirective // (getParsedHashDirective hashDirective)
    | SynModuleDecl.NamespaceFragment(fragment: SynModuleOrNamespace) ->
        XModuleDecl.NamespaceFragment(getModuleOrNamespace fragment)

let getAttributes (attributes: SynAttributes) =
    attributes
    |> List.map(getAttributeList)

let getAttributeList(attributeList: SynAttributeList) =
    {
        Attributes =
            attributeList.Attributes
            |> List.map getAttribute
    }:XAttributeList

let getAttribute(attribute: SynAttribute) =
    {
        TypeName = getLongIdent attribute.TypeName.LongIdent

        ArgExpr = getExpr attribute.ArgExpr

        Target = attribute.Target |> Option.map (fun(i:Ident)->i.idText)

        AppliesToGetterAndSetter = attribute.AppliesToGetterAndSetter

    }:XAttribute

let getExpr (expr:SynExpr) =
    match expr with
    
    | SynExpr.Paren( expr: SynExpr, leftParenRange: range, rightParenRange: range option, range: range) ->
        XExpr.Paren(
            getExpr expr
        )

    | SynExpr.Quote(operator: SynExpr, isRaw: bool, quotedExpr: SynExpr, isFromQueryExpression: bool, range: range) ->
        XExpr.Quote(getExpr operator, isRaw, getExpr quotedExpr, isFromQueryExpression)
    | SynExpr.Const (constant: SynConst, range: range)->
        XExpr.Const (getConst constant)
    | SynExpr.Typed (expr: SynExpr, targetType: SynType, range: range)->
        XExpr.Typed (getExpr expr, getType targetType)
    | SynExpr.Tuple (
        isStruct: bool ,
        exprs: SynExpr list ,
        commaRanges: range list,
        range: range) ->
        XExpr.Tuple (
            List.map getExpr exprs)

    | SynExpr.AnonRecd (
        isStruct: bool ,
        copyInfo: (SynExpr * BlockSeparator) option ,
        recordFields: (Ident * range option * SynExpr) list ,
        range: range) ->
        XExpr.AnonRecd (
            recordFields|> List.map (fun(i,_,e)->i.idText, getExpr e) 
            )

    | SynExpr.ArrayOrList(isArray: bool, exprs: SynExpr list, range: range)->
        XExpr.ArrayOrList(isArray, List.map getExpr exprs)

    | SynExpr.Record(
        baseInfo: (SynType * SynExpr * range * BlockSeparator option * range) option,
        copyInfo: (SynExpr * BlockSeparator) option,
        recordFields: SynExprRecordField list,
        range: range
        ) ->
        XExpr.Record(
            List.map getExprRecordField recordFields
        )
    | SynExpr.New (isProtected: bool, targetType: SynType, expr: SynExpr, range: range)->
        XExpr.New(
            //isProtected, 
            getType targetType, 
            getExpr expr
            )
    | SynExpr.ObjExpr (
        objType: SynType,
        argOptions: (SynExpr * Ident option) option,
        withKeyword: range option,
        bindings: SynBinding list,
        members: SynMemberDefn list,
        extraImpls: SynInterfaceImpl list,
        newExprRange: range,
        range: range
        ) ->
        XExpr.ObjExpr (
            getType objType,
            argOptions|>Option.map (fun(e,i)-> getExpr e,i|>Option.map (fun i -> i.idText) ) ,
            List.map getBinding bindings,
            List.map getMemberDefn members,
            List.map getInterfaceImpl extraImpls
        )

    | SynExpr.While(whileDebugPoint: DebugPointAtWhile, whileExpr: SynExpr, doExpr: SynExpr, range: range) ->
        XExpr.While(getExpr whileExpr,getExpr doExpr)
    | SynExpr.For (
        forDebugPoint: DebugPointAtFor,
        toDebugPoint: DebugPointAtInOrTo,
        ident: Ident,
        equalsRange: range option,
        identBody: SynExpr,
        direction: bool,
        toBody: SynExpr,
        doBody: SynExpr,
        range: range
        ) ->
        XExpr.For (
            ident.idText,
            getExpr identBody,
            direction,
            getExpr toBody,
            getExpr doBody
        )
    | SynExpr.ForEach(
        forDebugPoint: DebugPointAtFor,
        inDebugPoint: DebugPointAtInOrTo,
        seqExprOnly: SeqExprOnly,
        isFromSource: bool,
        pat: SynPat,
        enumExpr: SynExpr,
        bodyExpr: SynExpr,
        range: range
        ) ->
        XExpr.ForEach(
            getSeqExprOnly seqExprOnly,
            isFromSource,
            getPat pat,
            getExpr enumExpr,
            getExpr bodyExpr
        ) 
    | SynExpr.ArrayOrListComputed( isArray: bool , expr: SynExpr , range: range)->
        XExpr.ArrayOrListComputed( isArray ,getExpr expr)
    | SynExpr.IndexRange(expr1: SynExpr option , opm: range , expr2: SynExpr option , range1: range , range2: range , range: range) ->
        XExpr.IndexRange(Option.map getExpr expr1,Option.map getExpr expr2)
    | SynExpr.IndexFromEnd( expr: SynExpr , range: range) ->
        XExpr.IndexFromEnd(getExpr expr)
    | SynExpr.ComputationExpr(hasSeqBuilder: bool , expr: SynExpr , range: range) ->
        XExpr.ComputationExpr(hasSeqBuilder,getExpr expr)
    | SynExpr.Lambda (
        fromMethod: bool ,
        inLambdaSeq: bool ,
        args: SynSimplePats ,
        body: SynExpr ,
        parsedData: (SynPat list * SynExpr) option ,
        range: range ,
        trivia: SynExprLambdaTrivia) ->
        XExpr.Lambda (
            //fromMethod ,
            //inLambdaSeq ,
            getSimplePats args,
            getExpr body 
            //Option.map(fun(ls,ex)->List.map getPat ls,getExpr ex) parsedData
            )
    | SynExpr.MatchLambda (
        isExnMatch: bool ,
        keywordRange: range ,
        matchClauses: SynMatchClause list ,
        matchDebugPoint: DebugPointAtBinding ,
        range: range)->
        XExpr.MatchLambda (
            isExnMatch ,
            List.map getMatchClause matchClauses
            )
    | SynExpr.Match (
        matchDebugPoint: DebugPointAtBinding,
        expr: SynExpr,
        clauses: SynMatchClause list,
        range: range,
        trivia)->
        XExpr.Match (
            getExpr expr,
            List.map getMatchClause clauses
            )

    | SynExpr.Do ( expr: SynExpr , range: range)->
        XExpr.Do (getExpr expr)
    | SynExpr.Assert ( expr: SynExpr , range: range)->
        XExpr.Assert (getExpr expr)
    | SynExpr.App ( flag: ExprAtomicFlag , isInfix: bool , funcExpr: SynExpr , argExpr: SynExpr , range: range)->
        XExpr.App (
            //getExprAtomicFlag flag , 
            //isInfix , 
            getExpr funcExpr , 
            getExpr argExpr)
    | SynExpr.TypeApp (
        expr: SynExpr ,
        lessRange: range ,
        typeArgs: SynType list ,
        commaRanges: range list ,
        greaterRange: range option ,
        typeArgsRange: range ,
        range: range
        )->
        XExpr.TypeApp (
            getExpr expr ,
            List.map getType typeArgs
        )
    | SynExpr.LetOrUse ( isRecursive: bool , isUse: bool , bindings: SynBinding list , body: SynExpr , range: range , trivia: SynExprLetOrUseTrivia)->
        XExpr.LetOrUse ( isRecursive , isUse ,List.map getBinding bindings, getExpr body)
    | SynExpr.TryWith (
        tryExpr: SynExpr ,
        withCases: SynMatchClause list ,
        range: range ,
        tryDebugPoint: DebugPointAtTry ,
        withDebugPoint: DebugPointAtWith ,
        trivia: SynExprTryWithTrivia
        )->
        XExpr.TryWith (
            getExpr tryExpr ,
            List.map getMatchClause withCases
        )
    | SynExpr.TryFinally (
        tryExpr: SynExpr ,
        finallyExpr: SynExpr ,
        range: range ,
        tryDebugPoint: DebugPointAtTry ,
        finallyDebugPoint: DebugPointAtFinally ,
        trivia: SynExprTryFinallyTrivia
        )->
        XExpr.TryFinally (
            getExpr tryExpr ,
            getExpr finallyExpr
        )

    | SynExpr.Lazy ( expr: SynExpr , range: range)->
        XExpr.Lazy (getExpr expr)
    | SynExpr.Sequential ( debugPoint: DebugPointAtSequential , isTrueSeq: bool , expr1: SynExpr , expr2: SynExpr , range: range)->
        XExpr.Sequential ( isTrueSeq , getExpr expr1 ,getExpr expr2)
    | SynExpr.IfThenElse (
        ifExpr: SynExpr ,
        thenExpr: SynExpr ,
        elseExpr: SynExpr option ,
        spIfToThen: DebugPointAtBinding ,
        isFromErrorRecovery: bool ,
        range: range ,
        trivia: SynExprIfThenElseTrivia
        )->
        XExpr.IfThenElse (
            getExpr ifExpr,
            getExpr thenExpr,
            Option.map getExpr elseExpr ,
            isFromErrorRecovery
        )
    | SynExpr.Ident ( ident: Ident)->
        XExpr.Ident ( ident.idText)
    | SynExpr.LongIdent ( isOptional: bool , longDotId: SynLongIdent , 
        altNameRefCell: SynSimplePatAlternativeIdInfo ref option , range: range)->
        XExpr.LongIdent ( isOptional ,getLongIdent longDotId.LongIdent , 
            Option.map getSimplePatAlternativeIdInfo altNameRefCell)
    | SynExpr.LongIdentSet ( longDotId: SynLongIdent , expr: SynExpr , range: range)->
        XExpr.LongIdentSet ( getLongIdent longDotId.LongIdent ,getExpr expr)
    | SynExpr.DotGet ( expr: SynExpr , rangeOfDot: range , longDotId: SynLongIdent , range: range)->
        XExpr.DotGet (getExpr expr ,getLongIdent longDotId.LongIdent)
    | SynExpr.DotSet ( targetExpr: SynExpr , longDotId: SynLongIdent , rhsExpr: SynExpr , range: range)->
        XExpr.DotSet (getExpr targetExpr ,getLongIdent longDotId.LongIdent ,getExpr rhsExpr)
    | SynExpr.Set ( targetExpr: SynExpr , rhsExpr: SynExpr , range: range)->
        XExpr.Set (getExpr targetExpr ,getExpr rhsExpr)
    | SynExpr.DotIndexedGet ( objectExpr: SynExpr , indexArgs: SynExpr , dotRange: range , range: range)->
        XExpr.DotIndexedGet (getExpr objectExpr ,getExpr indexArgs)
    | SynExpr.DotIndexedSet (
        objectExpr: SynExpr ,
        indexArgs: SynExpr ,
        valueExpr: SynExpr ,
        leftOfSetRange: range ,
        dotRange: range ,
        range: range)->
        XExpr.DotIndexedSet (
            getExpr objectExpr ,
            getExpr indexArgs ,
            getExpr valueExpr
            )
    | SynExpr.NamedIndexedPropertySet ( longDotId: SynLongIdent, expr1: SynExpr , expr2: SynExpr , range: range)->
        XExpr.NamedIndexedPropertySet (getLongIdent longDotId.LongIdent ,getExpr expr1 ,getExpr expr2)
    | SynExpr.DotNamedIndexedPropertySet ( targetExpr: SynExpr , longDotId: SynLongIdent, argExpr: SynExpr , rhsExpr: SynExpr , range: range)->
        XExpr.DotNamedIndexedPropertySet (getExpr targetExpr ,getLongIdent longDotId.LongIdent ,getExpr argExpr , getExpr rhsExpr)
    | SynExpr.TypeTest ( expr: SynExpr , targetType: SynType , range: range)->
        XExpr.TypeTest (getExpr expr ,getType targetType)
    | SynExpr.Upcast ( expr: SynExpr , targetType: SynType , range: range)->
        XExpr.Upcast (getExpr expr ,getType targetType)
    | SynExpr.Downcast ( expr: SynExpr , targetType: SynType , range: range)->
        XExpr.Downcast (getExpr expr ,getType targetType)
    | SynExpr.InferredUpcast ( expr: SynExpr , range: range)->
        XExpr.InferredUpcast (getExpr expr)
    | SynExpr.InferredDowncast ( expr: SynExpr , range: range)->
        XExpr.InferredDowncast (getExpr expr)
    | SynExpr.Null ( range: range)->
        XExpr.Null
    | SynExpr.AddressOf ( isByref: bool , expr: SynExpr , opRange: range , range: range)->
        XExpr.AddressOf ( isByref , getExpr expr)
    | SynExpr.TraitCall ( supportTys: SynType list , traitSig: SynMemberSig , argExpr: SynExpr , range: range)->
        XExpr.TraitCall (List.map getType supportTys ,getMemberSig traitSig,getExpr argExpr)
    | SynExpr.JoinIn ( lhsExpr: SynExpr , lhsRange: range , rhsExpr: SynExpr , range: range)->
        XExpr.JoinIn (getExpr lhsExpr,getExpr rhsExpr)
    | SynExpr.ImplicitZero ( range: range)->
        XExpr.ImplicitZero
    | SynExpr.SequentialOrImplicitYield ( debugPoint: DebugPointAtSequential , expr1: SynExpr , expr2: SynExpr , ifNotStmt: SynExpr , range: range)->
        XExpr.SequentialOrImplicitYield (getExpr expr1 , getExpr expr2 , getExpr ifNotStmt)
    | SynExpr.YieldOrReturn ( flags: (bool * bool) , expr: SynExpr , range: range)->
        XExpr.YieldOrReturn ( flags,getExpr expr)
    | SynExpr.YieldOrReturnFrom ( flags: (bool * bool) , expr: SynExpr , range: range)->
        XExpr.YieldOrReturnFrom ( flags ,getExpr expr)
    | SynExpr.LetOrUseBang (
        bindDebugPoint: DebugPointAtBinding ,
        isUse: bool ,
        isFromSource: bool ,
        pat: SynPat ,
        rhs: SynExpr ,
        andBangs: SynExprAndBang list ,
        body: SynExpr ,
        range: range ,
        trivia: SynExprLetOrUseBangTrivia
        ) ->
        XExpr.LetOrUseBang (
            isUse ,
            isFromSource ,
            getPat pat ,
            getExpr rhs ,
            List.map getExprAndBang andBangs,
            getExpr body
            )
    | SynExpr.MatchBang (
        //matchKeyword,
        matchDebugPoint: DebugPointAtBinding ,
        expr: SynExpr ,
        //withKeyword,
        clauses: SynMatchClause list,
        range: range,
        trivia
        )->
        XExpr.MatchBang (
            getExpr expr ,
            List.map getMatchClause clauses
        )
    | SynExpr.DoBang ( expr: SynExpr , range: range)->
        XExpr.DoBang (getExpr expr)
    | SynExpr.LibraryOnlyILAssembly (
        ilCode: obj , 
        typeArgs: SynType list ,
        args: SynExpr list ,
        retTy: SynType list ,
        range: range
        ) ->
        XExpr.LibraryOnlyILAssembly
    | SynExpr.LibraryOnlyStaticOptimization (
        constraints: SynStaticOptimizationConstraint list ,
        expr: SynExpr ,
        optimizedExpr: SynExpr ,
        range: range
        )->
        XExpr.LibraryOnlyStaticOptimization
    | SynExpr.LibraryOnlyUnionCaseFieldGet ( expr: SynExpr , longId: LongIdent , fieldNum: int , range: range)->
        XExpr.LibraryOnlyUnionCaseFieldGet
    | SynExpr.LibraryOnlyUnionCaseFieldSet ( expr: SynExpr , longId: LongIdent , fieldNum: int , rhsExpr: SynExpr , range: range)->
        XExpr.LibraryOnlyUnionCaseFieldSet
    | SynExpr.ArbitraryAfterError ( debugStr: string , range: range)->
        XExpr.ArbitraryAfterError
    | SynExpr.FromParseError ( expr: SynExpr , range: range)->
        XExpr.FromParseError
    | SynExpr.DiscardAfterMissingQualificationAfterDot ( expr: SynExpr , range: range)->
        XExpr.DiscardAfterMissingQualificationAfterDot
    | SynExpr.Fixed ( expr: SynExpr , range: range)->
        XExpr.Fixed (getExpr expr)
    | SynExpr.InterpolatedString ( contents: SynInterpolatedStringPart list , synStringKind: SynStringKind , range: range)->
        XExpr.InterpolatedString (List.map getInterpolatedStringPart contents, getStringKind synStringKind)
    | SynExpr.DebugPoint ( debugPoint: DebugPointAtLeafExpr , isControlFlow: bool , innerExpr: SynExpr)->
        XExpr.DebugPoint
    | SynExpr.Dynamic ( funcExpr: SynExpr , qmark: range , argExpr: SynExpr , range: range) ->
        XExpr.Dynamic (getExpr funcExpr ,getExpr argExpr)
    | SynExpr.Typar _ -> failwith ""

let getTupleTypeSegment (x:SynTupleTypeSegment) =
    match x with
    | SynTupleTypeSegment.Type tname -> XTupleTypeSegment.Type (getType tname)
    | SynTupleTypeSegment.Star _ -> XTupleTypeSegment.Star
    | SynTupleTypeSegment.Slash _ -> XTupleTypeSegment.Slash

let getType(tp:SynType) =
    match tp with

    | SynType.LongIdent ( longDotId) ->
        XType.LongIdent (getLongIdent longDotId.LongIdent)

    | SynType.App (
        typeName: SynType ,
        lessRange: range option ,
        typeArgs: SynType list ,
        commaRanges: range list ,
        greaterRange: range option ,
        isPostfix: bool ,
        range: range
        ) ->
        XType.App (
            getType typeName ,
            List.map getType typeArgs ,
            isPostfix
            )
    | SynType.LongIdentApp (
        typeName: SynType ,
        longDotId: _,
        lessRange: range option ,
        typeArgs: SynType list ,
        commaRanges: range list ,
        greaterRange: range option ,
        range: range
        ) ->
        XType.LongIdentApp (
            getType typeName ,
            getLongIdent longDotId.LongIdent,
            List.map getType typeArgs
        )

    | SynType.Tuple (isStruct: bool, path:SynTupleTypeSegment list, range: range) ->
        XType.Tuple (isStruct , List.map getTupleTypeSegment path)
    | SynType.AnonRecd ( isStruct: bool , fields: (Ident * SynType) list , range: range)->
        XType.AnonRecd ( 
            isStruct ,
            fields |> List.map (fun(b,t)-> b.idText,getType t)
            )
    | SynType.Array ( rank: int , elementType: SynType , range: range) ->
        XType.Array ( rank , getType elementType)
    | SynType.Fun ( argType: SynType , returnType: SynType , range: range,trivia)->
        XType.Fun (getType argType ,getType returnType)
    | SynType.Var ( typar: SynTypar , range: range)->
        XType.Var (getTypar typar)
    | SynType.Anon ( range: range)->
        XType.Anon
    | SynType.WithGlobalConstraints ( typeName: SynType , constraints: SynTypeConstraint list ,
        range: range)->
        XType.WithGlobalConstraints (getType typeName ,List.map getTypeConstraint constraints)
    | SynType.HashConstraint ( innerType: SynType , range: range)->
        XType.HashConstraint (getType innerType)
    | SynType.MeasureDivide ( dividend: SynType , divisor: SynType , range: range)->
        XType.MeasureDivide (getType dividend,getType divisor)
    | SynType.MeasurePower ( baseMeasure: SynType , exponent: SynRationalConst , range: range)->
        XType.MeasurePower (getType baseMeasure ,getRationalConst exponent)
    | SynType.StaticConstant ( constant: SynConst , range: range)->
        XType.StaticConstant (getConst constant)
    | SynType.StaticConstantExpr ( expr: SynExpr , range: range) ->
        XType.StaticConstantExpr (getExpr expr)
    | SynType.StaticConstantNamed ( ident: SynType , value: SynType , range: range) ->
        XType.StaticConstantNamed (getType ident ,getType value)
    | SynType.Paren ( innerType: SynType , range: range) ->
        XType.Paren (getType innerType)
    | SynType.SignatureParameter _ -> failwith ""

let getConst (c: SynConst) =
    match c with
    | SynConst.Unit ->
        XConst.Unit

    | SynConst.Bool ( bool) ->
        XConst.Bool ( bool)
    | SynConst.SByte ( sbyte) ->
        XConst.SByte ( sbyte)
    | SynConst.Byte ( byte) ->
        XConst.Byte ( byte)
    | SynConst.Int16 ( int16) ->
        XConst.Int16 ( int16)
    | SynConst.UInt16 ( uint16) ->
        XConst.UInt16 ( uint16)
    | SynConst.Int32 ( int32) ->
        XConst.Int32 ( int32)
    | SynConst.UInt32 ( uint32) ->
        XConst.UInt32 ( uint32)
    | SynConst.Int64 ( int64) ->
        XConst.Int64 ( int64)
    | SynConst.UInt64 ( uint64) ->
        XConst.UInt64 ( uint64)
    | SynConst.IntPtr ( int64) ->
        XConst.IntPtr ( int64)
    | SynConst.UIntPtr ( uint64) ->
        XConst.UIntPtr ( uint64)
    | SynConst.Single ( single) ->
        XConst.Single ( single)
    | SynConst.Double ( double) ->
        XConst.Double ( double)
    | SynConst.Char ( char) ->
        XConst.Char ( char)
    | SynConst.Decimal (x: System.Decimal) ->
        XConst.Decimal (x)
    | SynConst.UserNum ( value: string , suffix: string) ->
        XConst.UserNum ( value, suffix)
    | SynConst.String ( text: string , synStringKind: SynStringKind , range: range) ->
        XConst.String ( text ,getStringKind synStringKind)
    | SynConst.Bytes ( bytes: byte[] , synByteStringKind: SynByteStringKind , range: range) ->
        XConst.Bytes ( bytes ,getByteStringKind synByteStringKind)
    | SynConst.UInt16s (x: uint16[]) ->
        XConst.UInt16s (x)
    | SynConst.Measure ( constant: SynConst , constantRange: range, ms: SynMeasure) ->
        XConst.Measure (getConst constant ,getMeasure ms)
    | SynConst.SourceIdentifier ( constant: string , value: string , range: range) ->
        XConst.SourceIdentifier ( constant , value)

let getStringKind(syn: SynStringKind )=
    match syn with
    | SynStringKind.Regular     -> XStringKind.Regular
    | SynStringKind.Verbatim    -> XStringKind.Verbatim
    | SynStringKind.TripleQuote -> XStringKind.TripleQuote

let getByteStringKind (syn:SynByteStringKind) =
    match syn with
    | SynByteStringKind.Regular  -> XByteStringKind.Regular
    | SynByteStringKind.Verbatim -> XByteStringKind.Verbatim

let getBinding (b:SynBinding) =
    match b with
    | SynBinding (
        accessibility: SynAccess option,
        kind: SynBindingKind,
        isInline: bool,
        isMutable: bool,
        attributes: SynAttributes ,
        xmlDoc: PreXmlDoc ,
        valData: SynValData ,
        headPat: SynPat ,
        returnInfo: SynBindingReturnInfo option ,
        expr: SynExpr ,
        range: range ,
        debugPoint: DebugPointAtBinding ,
        trivia: SynBindingTrivia
        ) ->
        let modifiers = [
            yield! yieldAccess accessibility
            if isInline then "inline"
            if isMutable then "mutable"

        ]
        XBinding (
            //Option.map getAccess accessibility,
            getBindingKind kind,
            modifiers,
            //isInline,
            //isMutable,
            getAttributes attributes,
            //getValData valData,
            getPat headPat ,
            Option.map getBindingReturnInfo returnInfo,
            getExpr expr 
        )

let getBindingKind (src:SynBindingKind) =
    match src with
    | SynBindingKind.StandaloneExpression -> XBindingKind.StandaloneExpression
    | SynBindingKind.Normal -> XBindingKind.Normal
    | SynBindingKind.Do -> XBindingKind.Do

let getBindingReturnInfo(src: SynBindingReturnInfo) = 
    match src with
    SynBindingReturnInfo ( typeName: SynType , range: range , attributes: SynAttributes) ->
        XBindingReturnInfo (getType typeName,getAttributes attributes)

let getPat(src: SynPat) =
    match src with
    | SynPat.Const( constant: SynConst , range: range) ->
        XPat.Const(getConst constant)
    | SynPat.Wild( range: range)->
        XPat.Wild
    | SynPat.Named( ident: SynIdent , isThisVal: bool , accessibility: SynAccess option , range: range) ->
        XPat.Named(getIdent ident, isThisVal,Option.map getAccess accessibility)
    | SynPat.Typed( pat: SynPat , targetType: SynType , range: range) ->
        XPat.Typed(getPat pat ,getType targetType)
    | SynPat.Attrib( pat: SynPat , attributes: SynAttributes , range: range) ->
        XPat.Attrib(getPat pat ,getAttributes attributes)
    | SynPat.Or( lhsPat: SynPat , rhsPat: SynPat , range: range , trivia: SynPatOrTrivia) ->
        XPat.Or(getPat lhsPat, getPat rhsPat)
    | SynPat.Ands( pats: SynPat list , range: range) ->
        XPat.Ands(List.map getPat pats)
    | SynPat.As( lhsPat: SynPat , rhsPat: SynPat , range: range) ->
        XPat.As(getPat lhsPat, getPat rhsPat)
    | SynPat.LongIdent(
        longDotId: _ ,
        extraId: Ident option ,
        typarDecls: SynValTyparDecls option ,
        argPats: SynArgPats ,
        accessibility: SynAccess option ,
        range: range) ->
        XPat.LongIdent(
            getLongIdent longDotId.LongIdent ,
            //Option.map getPropertyKeyword propertyKeyword,
            Option.map (fun(i:Ident)->i.idText) extraId,
            Option.map getValTyparDecls typarDecls,
            getArgPats argPats ,
            Option.map getAccess accessibility
            )
    | SynPat.Tuple( isStruct: bool , elementPats: SynPat list , range: range) ->
        XPat.Tuple( isStruct ,List.map getPat elementPats)
    | SynPat.Paren( pat: SynPat , range: range) ->
        XPat.Paren(getPat pat)
    | SynPat.ArrayOrList( isArray: bool , elementPats: SynPat list , range: range) ->
        XPat.ArrayOrList( isArray ,List.map getPat elementPats)
    | SynPat.Record( fieldPats: ((LongIdent * Ident) * range * SynPat) list , range: range) ->
        XPat.Record( 
            fieldPats
            |> List.map(fun((lid,id),_,pat)->
                (getLongIdent lid,id.idText),getPat pat
            )
        )
    | SynPat.Null( range: range) ->
        XPat.Null
    | SynPat.OptionalVal( ident: Ident , range: range) ->
        XPat.OptionalVal( ident.idText)
    | SynPat.IsInst( pat: SynType , range: range) ->
        XPat.IsInst(getType pat)
    | SynPat.QuoteExpr( expr: SynExpr , range: range) ->
        XPat.QuoteExpr(getExpr expr)
    | SynPat.DeprecatedCharRange( startChar: char , endChar: char , range: range) ->
        XPat.DeprecatedCharRange( startChar, endChar)
    | SynPat.InstanceMember(
        thisId: Ident ,
        memberId: Ident ,
        toolingId: Ident option ,
        accessibility: SynAccess option ,
        range: range
        ) ->
        XPat.InstanceMember(
            thisId.idText ,
            memberId.idText ,
            Option.map (fun(i:Ident)->i.idText) toolingId ,
            Option.map getAccess accessibility
        )

    | SynPat.FromParseError( pat: SynPat , range: range) ->
        XPat.FromParseError(getPat pat)

let getExprAtomicFlag (src:Syntax.ExprAtomicFlag) =
    match src with
    | ExprAtomicFlag.Atomic -> SyntaxTreeX.ExprAtomicFlag.Atomic
    | ExprAtomicFlag.NonAtomic -> SyntaxTreeX.ExprAtomicFlag.NonAtomic
    | _ -> failwith "never"
let getSeqExprOnly (src:Syntax.SeqExprOnly) =
    match src with 
    SeqExprOnly x -> SyntaxTreeX.SeqExprOnly x

let getInterpolatedStringPart(src: SynInterpolatedStringPart) =
    match src with
    | SynInterpolatedStringPart.String ( value: string,range: range ) ->  
        XInterpolatedStringPart.String value
    | SynInterpolatedStringPart.FillExpr ( fillExpr: SynExpr , qualifiers: Ident option) ->
        XInterpolatedStringPart.FillExpr(getExpr fillExpr, Option.map (fun(i:Ident)->i.idText) qualifiers)

let getOpenDeclTarget(src: SynOpenDeclTarget ) =
    match src with

    | SynOpenDeclTarget.ModuleOrNamespace ( longId: SynLongIdent , range: range) ->
        XOpenDeclTarget.ModuleOrNamespace (getLongIdentFromSyn longId)

    | SynOpenDeclTarget.Type ( typeName: SynType , range: range) ->
        XOpenDeclTarget.Type (getType typeName)

let getArgPats(src: SynArgPats )=
    match src with
    | SynArgPats.Pats ( pats: SynPat list)->
        XArgPats.Pats (List.map getPat pats)
    | SynArgPats.NamePatPairs ( pats: (Ident * range * SynPat) list , range: range) ->
        XArgPats.NamePatPairs (pats |> List.map (fun(i,_,p)->i.idText,getPat p)) 

let getComponentInfo(src: SynComponentInfo )=
    match src with
    SynComponentInfo ( attributes: SynAttributes , 
        typeParams: SynTyparDecls option ,
        constraints: SynTypeConstraint list , longId: LongIdent , 
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        preferPostfix: bool , accessibility: SynAccess option , 
        range: FSharp.Compiler.Text.range
        ) ->
        XComponentInfo ( 
            getAttributes attributes, 
            Option.map getTyparDecls typeParams ,
            List.map getTypeConstraint constraints , 
            getLongIdent longId, 
            preferPostfix , 
            Option.map getAccess accessibility
        )

let getTypeDefn(src: SynTypeDefn )=
    match src with
    SynTypeDefn ( 
        typeInfo: SynComponentInfo ,
        typeRepr: SynTypeDefnRepr ,
        members: SynMemberDefns ,
        implicitConstructor: SynMemberDefn option ,
        range: FSharp.Compiler.Text.range ,
        trivia: FSharp.Compiler.SyntaxTrivia.SynTypeDefnTrivia
        ) ->
        XTypeDefn ( 
            getComponentInfo typeInfo,
            getTypeDefnRepr typeRepr,
            getMemberDefns members,
            Option.map getMemberDefn implicitConstructor
        )


let getExceptionDefn (src:SynExceptionDefn) =
    match src with 
    SynExceptionDefn ( 
        exnRepr: SynExceptionDefnRepr , 
        withKeyword: FSharp.Compiler.Text.range option ,
        members: SynMemberDefns ,
        range: FSharp.Compiler.Text.range
        ) ->
        XExceptionDefn ( 
            getExceptionDefnRepr exnRepr , 
            getMemberDefns members
        )

let getExprRecordField (src:SynExprRecordField) =
    match src with
    SynExprRecordField (
        fieldName: RecordFieldName ,
        equalsRange: FSharp.Compiler.Text.range option ,
        expr: SynExpr option ,
        blockSeparator: BlockSeparator option
        ) ->
        XExprRecordField (
            getRecordFieldName fieldName,
            Option.map getExpr expr
        )

let getMemberDefn (src:SynMemberDefn) =
    match src with  
    | SynMemberDefn.Open ( target: SynOpenDeclTarget , range: FSharp.Compiler.Text.range) ->
        XMemberDefn.Open (getOpenDeclTarget target)
    | SynMemberDefn.Member ( memberDefn: SynBinding , range: FSharp.Compiler.Text.range) ->
        XMemberDefn.Member (getBinding memberDefn)
    | SynMemberDefn.ImplicitCtor ( 
        accessibility: SynAccess option , 
        attributes: SynAttributes , 
        ctorArgs: SynSimplePats , 
        selfIdentifier: Ident option ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.ImplicitCtor ( 
            Option.map getAccess accessibility , 
            getAttributes attributes , 
            getSimplePats ctorArgs , 
            Option.map (fun(i:Ident)->i.idText) selfIdentifier
        )
    | SynMemberDefn.ImplicitInherit ( 
        inheritType: SynType ,
        inheritArgs: SynExpr ,
        inheritAlias: Ident option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.ImplicitInherit ( 
            getType inheritType ,
            getExpr inheritArgs ,
            Option.map (fun(i:Ident)->i.idText) inheritAlias
        )
    | SynMemberDefn.LetBindings ( 
        bindings: SynBinding list ,
        isStatic: bool ,
        isRecursive: bool ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.LetBindings ( 
            List.map getBinding bindings,
            isStatic ,
            isRecursive
        )
    | SynMemberDefn.AbstractSlot ( 
        slotSig: SynValSig ,
        flags: SynMemberFlags ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.AbstractSlot ( 
            getValSig slotSig ,
            getMemberFlags flags
        )
  
    | SynMemberDefn.Interface ( 
        interfaceType: SynType ,
        withKeyword: FSharp.Compiler.Text.range option ,
        members: SynMemberDefns option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.Interface ( 
            getType interfaceType ,
            Option.map getMemberDefns members
        )
    | SynMemberDefn.Inherit ( 
        baseType: SynType ,
        asIdent: Ident option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.Inherit ( 
            getType baseType ,
            Option.map (fun(i:Ident)->i.idText) asIdent
        )
    | SynMemberDefn.ValField ( 
        fieldInfo: SynField ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.ValField (getField fieldInfo)
  
    | SynMemberDefn.NestedType ( 
        typeDefn: SynTypeDefn ,
        accessibility: SynAccess option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberDefn.NestedType ( 
            getTypeDefn typeDefn ,
            Option.map getAccess accessibility
        )
    | SynMemberDefn.AutoProperty ( 
        attributes: SynAttributes,
        isStatic: bool,
        ident: Ident,
        typeOpt: SynType option,
        propKind: SynMemberKind,
        memberFlags: SynMemberFlags,
        memberFlagsForSet: SynMemberFlags,
        xmlDoc: PreXmlDoc,
        accessibility: SynAccess option,
        _,
        synExpr: SynExpr,
        _,
        _,
        _
        )  ->
        XMemberDefn.AutoProperty ( 
            getAttributes attributes ,
            isStatic ,
            ident.idText ,
            Option.map getType typeOpt ,
            getMemberKind propKind ,
            Option.map getAccess accessibility ,
            getExpr synExpr
        )
    | SynMemberDefn.GetSetMember _ -> failwith "unimpl"

let getInterfaceImpl (src:SynInterfaceImpl) =
    match src with
    | SynInterfaceImpl (
        interfaceTy: SynType ,
        withKeyword: FSharp.Compiler.Text.range option ,
        bindings: SynBinding list ,
        members: SynMemberDefn list ,
        range: FSharp.Compiler.Text.range
        ) ->
        XInterfaceImpl (
            getType interfaceTy ,
            List.map getBinding bindings ,
            List.map getMemberDefn members
        )

let getSimplePats(src:SynSimplePats)=
    match src with
    | SynSimplePats.SimplePats (
        pats: SynSimplePat list ,
        range: FSharp.Compiler.Text.range
        ) ->
        XSimplePats.SimplePats (
            List.map getSimplePat pats
        )
    | SynSimplePats.Typed (
        pats: SynSimplePats,
        targetType: SynType ,
        range: FSharp.Compiler.Text.range
        ) ->
        XSimplePats.Typed (
            getSimplePats pats,
            getType targetType
        )

let getMemberKind(src:SynMemberKind)=
    match src with
    | SynMemberKind.ClassConstructor -> XMemberKind.ClassConstructor
    | SynMemberKind.Constructor      -> XMemberKind.Constructor
    | SynMemberKind.Member           -> XMemberKind.Member
    | SynMemberKind.PropertyGet      -> XMemberKind.PropertyGet
    | SynMemberKind.PropertySet      -> XMemberKind.PropertySet
    | SynMemberKind.PropertyGetSet   -> XMemberKind.PropertyGetSet

let getMatchClause(src:SynMatchClause) =
    match src with
    | SynMatchClause (
        pat: SynPat ,
        whenExpr: SynExpr option ,
        resultExpr: SynExpr ,
        range: FSharp.Compiler.Text.range ,
        debugPoint: DebugPointAtTarget ,
        trivia: FSharp.Compiler.SyntaxTrivia.SynMatchClauseTrivia
        ) ->
        XMatchClause (
            getPat pat ,
            Option.map getExpr whenExpr ,
            getExpr resultExpr
        )

let getSimplePatAlternativeIdInfo(src:SynSimplePatAlternativeIdInfo ref)=
    match !src with
    | SynSimplePatAlternativeIdInfo.Undecided (i:Ident) ->
        XSimplePatAlternativeIdInfo.Undecided (i.idText)
    | SynSimplePatAlternativeIdInfo.Decided (i:Ident) ->
        XSimplePatAlternativeIdInfo.Decided (i.idText)

let getTypar (src: SynTypar) =
    match src with
    | SynTypar ( ident: Ident , staticReq: TyparStaticReq , isCompGen: bool) ->
        XTypar ( ident.idText, getTyparStaticReq staticReq, isCompGen)

let getTyparStaticReq(src:Syntax.TyparStaticReq) =
    match src with
    | TyparStaticReq.None     -> SyntaxTreeX.TyparStaticReq.None
    | TyparStaticReq.HeadType -> SyntaxTreeX.TyparStaticReq.HeadType

let getMemberSig(src:SynMemberSig) =
    match src with
    | SynMemberSig.Member ( 
        memberSig: SynValSig ,
        flags: SynMemberFlags ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberSig.Member ( 
           getValSig memberSig ,
           getMemberFlags flags
        )
    | SynMemberSig.Interface ( 
        interfaceType: SynType ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberSig.Interface ( 
           getType interfaceType
        )
    | SynMemberSig.Inherit ( 
        inheritedType: SynType ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberSig.Inherit ( 
            getType inheritedType
        )
    | SynMemberSig.ValField ( 
        field: SynField ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberSig.ValField ( 
            getField field
        )
    | SynMemberSig.NestedType ( 
        nestedType: SynTypeDefnSig ,
        range: FSharp.Compiler.Text.range
        ) ->
        XMemberSig.NestedType ( 
           getTypeDefnSig nestedType
        )
  
let getExprAndBang (src:SynExprAndBang) =
    match src with
    | SynExprAndBang (
        debugPoint: DebugPointAtBinding ,
        isUse: bool , 
        isFromSource: bool ,
        pat: SynPat ,
        body: SynExpr ,
        range: FSharp.Compiler.Text.range ,
        trivia: FSharp.Compiler.SyntaxTrivia.SynExprAndBangTrivia
        ) ->
        XExprAndBang (
            isUse , 
            isFromSource ,
            getPat pat ,
            getExpr body
        )

let getTypeConstraint(src:SynTypeConstraint) =
    match src with
  
    | SynTypeConstraint.WhereTyparIsValueType ( 
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> XTypeConstraint.WhereTyparIsValueType (getTypar typar)
    | SynTypeConstraint.WhereTyparIsReferenceType ( 
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparIsReferenceType (getTypar typar)

    | SynTypeConstraint.WhereTyparIsUnmanaged ( 
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparIsUnmanaged (getTypar typar)

    | SynTypeConstraint.WhereTyparSupportsNull ( 
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparSupportsNull (getTypar typar)

    | SynTypeConstraint.WhereTyparIsComparable (
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparIsComparable (getTypar typar)

    | SynTypeConstraint.WhereTyparIsEquatable (
        typar: SynTypar , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparIsEquatable (getTypar typar)

    | SynTypeConstraint.WhereTyparDefaultsToType ( 
        typar: SynTypar ,
        typeName: SynType , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparDefaultsToType (getTypar typar ,getType typeName)

    | SynTypeConstraint.WhereTyparSubtypeOfType (
        typar: SynTypar , 
        typeName: SynType , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparSubtypeOfType (getTypar typar ,getType typeName)

    | SynTypeConstraint.WhereTyparSupportsMember ( 
        typars: SynType list ,
        memberSig: SynMemberSig ,
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparSupportsMember (List.map getType typars ,getMemberSig memberSig)

    | SynTypeConstraint.WhereTyparIsEnum ( 
        typar: SynTypar ,
        typeArgs: SynType list , 
        range: FSharp.Compiler.Text.range
        ) -> 
        XTypeConstraint.WhereTyparIsEnum (getTypar typar ,List.map getType typeArgs)

    | SynTypeConstraint.WhereTyparIsDelegate (
        typar: SynTypar , 
        typeArgs: SynType list ,
        range: FSharp.Compiler.Text.range
        ) ->
        XTypeConstraint.WhereTyparIsDelegate (getTypar typar ,List.map getType typeArgs)
    | SynTypeConstraint.WhereSelfConstrained (_, _) -> failwith ""

let getRationalConst(src:SynRationalConst)=
    match src with
    | SynRationalConst.Integer ( value: int32) ->
        XRationalConst.Integer ( value)
    | SynRationalConst.Rational ( 
        numerator: int32 , 
        denominator: int32 , 
        range: FSharp.Compiler.Text.range
        ) ->
        XRationalConst.Rational (numerator,denominator)
    | SynRationalConst.Negate (x: SynRationalConst) ->
        XRationalConst.Negate (getRationalConst x)

let getMeasure(src:SynMeasure)=
    match src with
    | SynMeasure.Named ( longId: LongIdent , range: FSharp.Compiler.Text.range)->
        XMeasure.Named (getLongIdent longId)
    | SynMeasure.Product ( measure1: SynMeasure , measure2: SynMeasure , range: FSharp.Compiler.Text.range)->
        XMeasure.Product (getMeasure measure1  ,getMeasure measure2 )
    | SynMeasure.Seq ( measures: SynMeasure list , range: FSharp.Compiler.Text.range)->
        XMeasure.Seq (List.map getMeasure measures)
    | SynMeasure.Divide ( measure1: SynMeasure , measure2: SynMeasure , range: FSharp.Compiler.Text.range)->
        XMeasure.Divide (getMeasure measure1 ,getMeasure measure2)
    | SynMeasure.Power ( measure: SynMeasure , power: SynRationalConst , range: FSharp.Compiler.Text.range)->
        XMeasure.Power (getMeasure measure ,getRationalConst power)
    | SynMeasure.One ->
        XMeasure.One
    | SynMeasure.Anon ( range: FSharp.Compiler.Text.range)->
        XMeasure.Anon 
    | SynMeasure.Var ( typar: SynTypar , range: FSharp.Compiler.Text.range)->
        XMeasure.Var (getTypar typar )
    | SynMeasure.Paren(_,_) -> failwith "unimpl"
let getValData (src:SynValData)=
    match src with
    | SynValData (
        memberFlags: SynMemberFlags option ,
        valInfo: SynValInfo , 
        thisIdOpt: Ident option
        ) ->
        XValData (
            Option.map getMemberFlags memberFlags ,
            getValInfo valInfo , 
            thisIdOpt |> Option.map (fun i -> i.idText)
        )

//let getPropertyKeyword(src:FSharp.Compiler.Syntax.PropertyKeyword)=
//    match src with
//    | PropertyKeyword.With _ -> SyntaxTreeX.PropertyKeyword.With
//    | PropertyKeyword.And  _ -> SyntaxTreeX.PropertyKeyword.And

let getValTyparDecls(src:SynValTyparDecls)=
    match src with
    | SynValTyparDecls ( typars: SynTyparDecls option , canInfer: bool) ->
        XValTyparDecls (Option.map getTyparDecls typars , canInfer)

let getTyparDecls(src:SynTyparDecls)=
    match src with
    | SynTyparDecls.PostfixList ( decls: SynTyparDecl list , constraints: SynTypeConstraint list , range: FSharp.Compiler.Text.range) ->
        XTyparDecls.PostfixList(List.map getTyparDecl decls ,List.map getTypeConstraint constraints)
    | SynTyparDecls.PrefixList ( decls: SynTyparDecl list , range: FSharp.Compiler.Text.range)->
        XTyparDecls.PrefixList (List.map getTyparDecl decls)
    | SynTyparDecls.SinglePrefix ( decl: SynTyparDecl , range: FSharp.Compiler.Text.range)->
        XTyparDecls.SinglePrefix (getTyparDecl decl)

let getTypeDefnRepr(src:SynTypeDefnRepr)=
    match src with
    | SynTypeDefnRepr.ObjectModel ( kind: SynTypeDefnKind , members: SynMemberDefns , range: FSharp.Compiler.Text.range)->
        XTypeDefnRepr.ObjectModel (getTypeDefnKind kind ,getMemberDefns members)
    | SynTypeDefnRepr.Simple ( simpleRepr: SynTypeDefnSimpleRepr , range: FSharp.Compiler.Text.range)->
        XTypeDefnRepr.Simple (getTypeDefnSimpleRepr simpleRepr)
    | SynTypeDefnRepr.Exception ( exnRepr: SynExceptionDefnRepr) ->
        XTypeDefnRepr.Exception (getExceptionDefnRepr exnRepr)

let getMemberDefns(src:SynMemberDefns)=
    List.map getMemberDefn src
let getExceptionDefnRepr(src:SynExceptionDefnRepr)=
    match src with 
    SynExceptionDefnRepr ( 
        attributes: SynAttributes , 
        caseName: SynUnionCase , 
        longId: LongIdent option ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        accessibility: SynAccess option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XExceptionDefnRepr ( 
            getAttributes attributes , 
            getUnionCase caseName , 
            Option.map getLongIdent longId ,
            Option.map getAccess accessibility
        )

//let getParsedHashDirectiveArgument(src:FSharp.Compiler.Syntax.ParsedHashDirectiveArgument)=
//    match src with 
//    | FSharp.Compiler.Syntax.ParsedHashDirectiveArgument.String ( value: string , stringKind: SynStringKind , range: FSharp.Compiler.Text.range)->
//        SyntaxTreeX.ParsedHashDirectiveArgument.String ( 
//            value ,getStringKind stringKind)
//    | FSharp.Compiler.Syntax.ParsedHashDirectiveArgument.SourceIdentifier ( constant: string , value: string , range: FSharp.Compiler.Text.range) ->
//        SyntaxTreeX.ParsedHashDirectiveArgument.SourceIdentifier ( 
//            constant , value)

let getRecordFieldName(src:RecordFieldName) =
    match src with
    | (lid,b) -> getLongIdent lid.LongIdent, b


let getValSig(src:SynValSig) =
    match src with
    | SynValSig (
        attributes: SynAttributes ,
        ident: SynIdent ,
        explicitValDecls: SynValTyparDecls ,
        synType: SynType ,
        arity: SynValInfo ,
        isInline: bool ,
        isMutable: bool ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        accessibility: SynAccess option ,
        synExpr: SynExpr option ,
        //withKeyword: FSharp.Compiler.Text.range option ,
        range: FSharp.Compiler.Text.range,
        trivia) ->
        XValSig (
            getAttributes attributes ,
            getIdent ident ,
            getValTyparDecls explicitValDecls ,
            getType synType ,
            getValInfo arity ,
            isInline ,
            isMutable ,
            Option.map getAccess accessibility ,
            Option.map getExpr synExpr
        )

let getMemberFlags (src:SynMemberFlags) =
    {
        IsInstance = src.IsInstance
        IsDispatchSlot = src.IsDispatchSlot
        IsOverrideOrExplicitImpl = src.IsOverrideOrExplicitImpl
        IsFinal = src.IsFinal
        MemberKind = src.MemberKind |> getMemberKind
    }:XMemberFlags

let getField(src:SynField) =
    match src with
    | SynField (
        attributes: SynAttributes ,
        isStatic: bool ,
        idOpt: Ident option ,
        fieldType: SynType ,
        isMutable: bool ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        accessibility: SynAccess option ,
        range: FSharp.Compiler.Text.range
        ) ->
        XField (
            getAttributes attributes  ,
            isStatic ,
            idOpt|> Option.map (fun i -> i.idText),
            getType fieldType ,
            isMutable ,
            Option.map getAccess accessibility
        )

let getSimplePat(src:SynSimplePat) =
    match src with
    | SynSimplePat.Id ( 
        ident: Ident ,
        altNameRefCell: SynSimplePatAlternativeIdInfo ref option ,
        isCompilerGenerated: bool , isThisVal: bool , isOptional: bool ,
        range: FSharp.Compiler.Text.range
        ) ->
        XSimplePat.Id ( 
            ident.idText ,
            Option.map getSimplePatAlternativeIdInfo altNameRefCell ,
            isCompilerGenerated , isThisVal , isOptional 
        )
    | SynSimplePat.Typed ( pat: SynSimplePat , targetType: SynType , range: FSharp.Compiler.Text.range) ->
        XSimplePat.Typed (getSimplePat pat, getType targetType)
    | SynSimplePat.Attrib ( pat: SynSimplePat , attributes: SynAttributes , range: FSharp.Compiler.Text.range) ->
        XSimplePat.Attrib (getSimplePat pat ,getAttributes attributes)

let getTypeDefnSig(src:SynTypeDefnSig) =
    match src with
    | SynTypeDefnSig ( 
        typeInfo: SynComponentInfo ,
        typeRepr: SynTypeDefnSigRepr ,
        members: SynMemberSig list ,
        range: FSharp.Compiler.Text.range,
        _
        ) ->
        XTypeDefnSig ( 
            getComponentInfo typeInfo ,
            getTypeDefnSigRepr typeRepr ,
            List.map getMemberSig members
            )
let getValInfo(src:SynValInfo) =
    match src with
    | SynValInfo ( curriedArgInfos: SynArgInfo list list , returnInfo: SynArgInfo) ->
        XValInfo (List.map (List.map getArgInfo) curriedArgInfos , getArgInfo returnInfo)

let getTyparDecl(src:SynTyparDecl) =
    match src with
    | SynTyparDecl (attributes: SynAttributes,ty: SynTypar) ->
        XTyparDecl (getAttributes attributes , getTypar ty)

let getTypeDefnKind(src:SynTypeDefnKind) =
    match src with
    | SynTypeDefnKind.Unspecified                                             -> XTypeDefnKind.Unspecified
    | SynTypeDefnKind.Class                                                   -> XTypeDefnKind.Class
    | SynTypeDefnKind.Interface                                               -> XTypeDefnKind.Interface
    | SynTypeDefnKind.Struct                                                  -> XTypeDefnKind.Struct
    | SynTypeDefnKind.Record                                                  -> XTypeDefnKind.Record
    | SynTypeDefnKind.Union                                                   -> XTypeDefnKind.Union
    | SynTypeDefnKind.Abbrev                                                  -> XTypeDefnKind.Abbrev
    | SynTypeDefnKind.Opaque                                                  -> XTypeDefnKind.Opaque
    | SynTypeDefnKind.Augmentation(withKeyword: FSharp.Compiler.Text.range) -> XTypeDefnKind.Augmentation
    | SynTypeDefnKind.IL                                                      -> XTypeDefnKind.IL
    | SynTypeDefnKind.Delegate ( signature: SynType , signatureInfo: SynValInfo) ->
        XTypeDefnKind.Delegate (getType signature ,getValInfo signatureInfo)

let getTypeDefnSimpleRepr(src:SynTypeDefnSimpleRepr) =
    match src with
    | SynTypeDefnSimpleRepr.Union ( accessibility: SynAccess option , unionCases: SynUnionCase list , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.Union (
            Option.map getAccess accessibility, 
            List.map getUnionCase unionCases)
    | SynTypeDefnSimpleRepr.Enum ( cases: SynEnumCase list , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.Enum (List.map getEnumCase cases)
    | SynTypeDefnSimpleRepr.Record ( accessibility: SynAccess option , recordFields: SynField list , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.Record (
            Option.map getAccess accessibility, 
            List.map getField recordFields)
    | SynTypeDefnSimpleRepr.General ( kind: SynTypeDefnKind , inherits: (SynType * FSharp.Compiler.Text.range * Ident option) list , slotsigs: (SynValSig * SynMemberFlags) list , fields: SynField list , isConcrete: bool , isIncrClass: bool , implicitCtorSynPats: SynSimplePats option , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.General (
            getTypeDefnKind kind, 
            inherits |> List.map(fun(ty,_,i)-> getType ty,Option.map (fun(i:Ident)->i.idText) i) , 
            slotsigs |> List.map(fun(vs,mf)-> getValSig vs, getMemberFlags mf) , 
            List.map getField fields , 
            isConcrete , isIncrClass , 
            Option.map getSimplePats implicitCtorSynPats
            )
    | SynTypeDefnSimpleRepr.LibraryOnlyILAssembly ( ilType: obj , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.LibraryOnlyILAssembly
    | SynTypeDefnSimpleRepr.TypeAbbrev ( detail: ParserDetail , rhsType: SynType , range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.TypeAbbrev (getType rhsType)
    | SynTypeDefnSimpleRepr.None ( range: FSharp.Compiler.Text.range) ->
        XTypeDefnSimpleRepr.None
    | SynTypeDefnSimpleRepr.Exception ( exnRepr: SynExceptionDefnRepr) ->
        XTypeDefnSimpleRepr.Exception (getExceptionDefnRepr exnRepr )

let getUnionCase(src:SynUnionCase) =
    match src with
    | SynUnionCase(
        attributes: SynAttributes ,
        ident: SynIdent ,
        caseType: SynUnionCaseKind ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        accessibility: SynAccess option ,
        range: FSharp.Compiler.Text.range ,
        trivia: FSharp.Compiler.SyntaxTrivia.SynUnionCaseTrivia
        ) ->
        XUnionCase(
            getAttributes attributes,
            getIdent ident,
            getUnionCaseKind caseType,
            Option.map getAccess  accessibility
        )

let getTypeDefnSigRepr(src:SynTypeDefnSigRepr) =
    match src with
    | SynTypeDefnSigRepr.ObjectModel (
        kind: SynTypeDefnKind ,
        memberSigs: SynMemberSig list ,
        range: FSharp.Compiler.Text.range
        ) ->
        XTypeDefnSigRepr.ObjectModel (
            getTypeDefnKind kind ,
            List.map getMemberSig memberSigs
        )
    | SynTypeDefnSigRepr.Simple (
        repr: SynTypeDefnSimpleRepr ,
        range: FSharp.Compiler.Text.range
        ) ->
        XTypeDefnSigRepr.Simple (
            repr |> getTypeDefnSimpleRepr
        )
    | SynTypeDefnSigRepr.Exception ( repr: SynExceptionDefnRepr) ->
        XTypeDefnSigRepr.Exception ( repr|> getExceptionDefnRepr)
  
let getEnumCase(src:SynEnumCase) =
    match src with
    | SynEnumCase(
        attributes: SynAttributes ,
        ident: SynIdent ,
        value: SynConst ,
        valueRange: FSharp.Compiler.Text.range ,
        xmlDoc: FSharp.Compiler.Xml.PreXmlDoc ,
        range: FSharp.Compiler.Text.range ,
        trivia: FSharp.Compiler.SyntaxTrivia.SynEnumCaseTrivia
        ) ->
        XEnumCase(
            attributes |> getAttributes ,
            ident|> getIdent ,
            value|> getConst
        )
let getArgInfo(src:SynArgInfo) =
    match src with
    | SynArgInfo ( attributes: SynAttributes , optional: bool , ident: Ident option) ->
        XArgInfo ( attributes|>getAttributes , optional , ident|> Option.map (fun i -> i.idText)) 

let getUnionCaseKind(src:SynUnionCaseKind) =
    match src with
    | SynUnionCaseKind.Fields ( cases: SynField list) ->
        XUnionCaseKind.Fields ( cases|> List.map getField)
    | SynUnionCaseKind.FullType ( fullType: SynType , fullTypeInfo: SynValInfo) ->
        XUnionCaseKind.FullType ( fullType|>getType , fullTypeInfo|>getValInfo)

//let getParserDetail(src:FSharp.Compiler.Syntax. ParserDetail) =
//    match src with
//    | ParserDetail.Ok            -> SyntaxTreeX.ParserDetail.Ok
//    | ParserDetail.ErrorRecovery -> SyntaxTreeX.ParserDetail.ErrorRecovery

