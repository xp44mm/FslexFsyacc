## A.2 Syntactic Grammar

In general, this syntax summary describes full syntax. By default, however, `.fs`, `.fsi`, `.fsx`, and `.fsscript` files support lightweight syntax, in which indentation replaces `begin/end` and `done` tokens. This appendix uses `begin_opt`, `end_(opt)`, and `done_opt` to indicate that these tokens are omitted in lightweight syntax. Complete rules for lightweight syntax appear in §15.1.

To disable lightweight syntax:

```fsharp
#indent "off"
```

When lightweight syntax is disabled, whitespace can include tab characters:

```fsharp
*whitespace* : [ ' ' '\t' ]+
```

### A.2.1 Program Format

#### A.2.1.1 Namespaces and Modules

```fsharp
implementationFile :
    | fileNamespaceImpls EOF {}
fileNamespaceImpls :
    | fileModuleImpl {}
    | fileModuleImpl fileNamespaceImplList {}
fileModuleImpl :
    | attributes? access? moduleIntro moduleDefnsOrExprPossiblyEmptyOrBlock {}
    | moduleDefnsOrExprPossiblyEmptyOrBlock {}
attributes? :
    | attributes {}
    | (*empty*) {}
access? :
    | (*empty*) {}
    | access {}
access :
    | private {}
    | public {}
    | internal {}
moduleIntro :
    | module attributes? access? rec? path {}
rec? :
    | rec {}
    | (*empty*) {}
path :
    | global {}
    | IDENT {}
    | path "." IDENT {}
moduleDefnsOrExprPossiblyEmptyOrBlock :
    | OBLOCKBEGIN moduleDefnsOrExprPossiblyEmpty OBLOCKEND OBLOCKSEP? {}
    | moduleDefnsOrExprPossiblyEmpty {}
moduleDefnsOrExprPossiblyEmpty :
    | moduleDefnsOrExpr {}
    | (*empty*) {}
moduleDefnsOrExpr :
    | attributes? access? declExpr topSeparator+ moduleDefnsOrExpr {}
    | attributes? access? declExpr topSeparator+ {}
    | attributes? access? declExpr {}
    | moduleDefns {}
declExpr :
    | defnBindings in typedSequentialExpr {}
    | hardwhiteLetBindings typedSequentialExprBlock {}
    | hardwhiteLetBindings OBLOCKSEP typedSequentialExprBlock {}
    | hardwhiteDoBinding {}
    | anonMatchingExpr {}
    | anonLambdaExpr {}
    | match typedSequentialExpr withClauses {}
    | "match!" typedSequentialExpr withClauses {}
    | try typedSequentialExprBlockR withClauses {}
    | try typedSequentialExprBlockR finally typedSequentialExprBlock {}
    | if declExpr ifExprCases {}
    | lazy declExpr {}
    | assert declExpr {}
    | assert {}
    | OLAZY declExprBlock {}
    | OASSERT declExprBlock {}
    | OASSERT {}
    | while declExpr doToken typedSequentialExprBlock doneDeclEnd {}
    | for forLoopBinder doToken typedSequentialExprBlock doneDeclEnd {}
    | for forLoopBinder OBLOCKSEP? arrowThenExprR {}
    | for forLoopRange doToken typedSequentialExprBlock doneDeclEnd {}
    | yield declExpr {}
    | "yield!" declExpr {}
    | BINDER headBindingPattern "=" typedSequentialExprBlock in OBLOCKSEP? moreBinders typedSequentialExprBlock {}
    | OBINDER headBindingPattern "=" typedSequentialExprBlock ODECLEND OBLOCKSEP? moreBinders typedSequentialExprBlock {}
    | "do!" typedSequentialExpr in OBLOCKSEP? typedSequentialExprBlock {}
    | ODO_BANG typedSequentialExprBlock ODECLEND {}
    | fixed declExpr {}
    | "->" typedSequentialExprBlockR {}
    | declExpr ":?" typ {}
    | declExpr ":>" typ {}
    | declExpr ":?>" typ {}
    | declExpr ":=" declExpr {}
    | minusExpr "<-" declExprBlock {}
    | tupleExpr {}
    | declExpr JOIN_IN declExpr {}
    | declExpr "||" declExpr {}
    | declExpr ["|"] declExpr {}
    | declExpr or declExpr {}
    | declExpr "&" declExpr {}
    | declExpr "&&" declExpr {}
    | declExpr ["&"] declExpr {}
    | declExpr "=" declExpr {}
    | declExpr INFIX_COMPARE_OP declExpr {}
    | declExpr "$" declExpr {}
    | declExpr "<" declExpr {}
    | declExpr ">" declExpr {}
    | declExpr ["@""^"] declExpr {}
    | declExpr ["%" "%%"] declExpr {}
    | declExpr "::" declExpr {}
    | declExpr ["-" "+"] declExpr {}
    | declExpr "-" declExpr {}
    | declExpr "*" declExpr {}
    | declExpr ["*" "/" "%"] declExpr {}
    | declExpr ["**"] declExpr {}
    | declExpr ".." declExpr {}
    | declExpr ".." {}
    | ".." declExpr {}
    | "*" {}
    | minusExpr {}
defnBindings :
    | let rec? localBindings {}
    | cPrototype {}
typedSequentialExpr :
    | sequentialExpr ":" typeWithTypeConstraints {}
    | sequentialExpr {}
hardwhiteLetBindings :
    | OLET rec? localBindings ODECLEND {}
typedSequentialExprBlock :
    | OBLOCKBEGIN typedSequentialExpr OBLOCKEND {}
    | typedSequentialExpr {}
hardwhiteDoBinding :
    | ODO typedSequentialExprBlock ODECLEND {}
anonMatchingExpr :
    | function withPatternClauses {}
    | OFUNCTION withPatternClauses OEND {}
anonLambdaExpr :
    | fun atomicPatterns "->" typedSequentialExprBlock {}
    | OFUN atomicPatterns "->" typedSequentialExprBlockR OEND {}
    | OFUN atomicPatterns "->" ORIGHT_BLOCK_END OEND {}
typedSequentialExprBlockR :
    | typedSequentialExpr ORIGHT_BLOCK_END {}
    | typedSequentialExpr {}
declExprBlock :
    | OBLOCKBEGIN typedSequentialExpr OBLOCKEND {}
    | declExpr {}
doToken :
    | do {}
    | ODO {}
doneDeclEnd :
    | done {}
    | ODECLEND {}
forLoopBinder :
    | parenPattern in declExpr {}
OBLOCKSEP? :
    | OBLOCKSEP {}
    | (*empty*) {}
arrowThenExprR :
    | "->" typedSequentialExprBlockR {}
forLoopRange :
    | parenPattern "=" declExpr forLoopDirection declExpr {}
forLoopDirection :
    | to {}
    | downto {}
moreBinders :
    | "and!" headBindingPattern "=" typedSequentialExprBlock in moreBinders {}
    | OAND_BANG headBindingPattern "=" typedSequentialExprBlock ODECLEND OBLOCKSEP? moreBinders {}
    | (*empty*) {}
minusExpr :
    | ["@" "^"] minusExpr {}
    | "-" minusExpr {}
    | ["-" "+"] minusExpr {}
    | ADJACENT_PREFIX_OP minusExpr {}
    | ["%" "%%"] minusExpr {}
    | "&" minusExpr {}
    | "&&" minusExpr {}
    | new atomTypeNonAtomicDeprecated HIGH_PRECEDENCE_APP? atomicExprAfterType "." atomicExprQualification {}
    | new atomTypeNonAtomicDeprecated HIGH_PRECEDENCE_APP? atomicExprAfterType {}
    | upcast minusExpr {}
    | downcast minusExpr {}
    | appExpr {}
HIGH_PRECEDENCE_APP? :
    | HIGH_PRECEDENCE_BRACK_APP {}
    | HIGH_PRECEDENCE_PAREN_APP {}
    | (*empty*) {}
atomicExprQualification :
    | identOrOp {}
    | global {}
    | (*empty*) {}
    | "(" "::" ")" "." INT32 {}
    | "(" typedSequentialExpr ")" {}
    | "[" typedSequentialExpr "]" {}
appExpr :
    | appExpr argExpr {}
    | atomicExpr {}
argExpr :
    | ADJACENT_PREFIX_OP atomicExpr {}
    | atomicExpr {}
topSeparator+ :
    | topSeparator {}
    | topSeparator topSeparator+ {}
topSeparator :
    | ";" {}
    | ";;" {}
    | OBLOCKSEP {}
moduleDefns :
    | moduleDefnOrDirective moduleDefns {}
    | moduleDefnOrDirective topSeparator+ moduleDefnsOrExpr {}
    | moduleDefnOrDirective {}
    | moduleDefnOrDirective topSeparator+ {}
moduleDefnOrDirective :
    | moduleDefn {}
    | hashDirective {}
moduleDefn :
    | attributes? access? defnBindings {}
    | attributes? access? hardwhiteLetBindings {}
    | attributes? access? doBinding {}
    | attributes? access? type tyconDefn tyconDefnList {}
    | attributes? access? exconDefn {}
    | attributes? access? moduleIntro "=" namedModuleDefnBlock {}
    | openDecl {}
doBinding :
    | do typedSequentialExprBlock {}
tyconDefnList :
    | and tyconDefn tyconDefnList {}
    | (*empty*) {}
exconDefn :
    | exconCore classDefn? {}
exconCore :
    | exception attributes? access? exconIntro exconRepr {}
exconIntro :
    | IDENT {}
    | IDENT of unionCaseRepr {}
exconRepr :
    | (*empty*) {}
    | "=" path {}
classDefn? :
    | with classDefnBlock declEnd {}
    | (*empty*) {}
classDefnBlock :
    | OBLOCKBEGIN classDefnMembers OBLOCKEND {}
    | classDefnMembers {}
classDefnMembers :
    | classDefnMembersAtLeastOne {}
    | (*empty*) {}
classDefnMembersAtLeastOne :
    | classDefnMember seps? classDefnMembers {}
seps? :
    | seps {}
    | (*empty*) {}
seps :
    | OBLOCKSEP {}
    | ";" {}
    | OBLOCKSEP ";" {}
    | ";" OBLOCKSEP {}
declEnd :
    | ODECLEND {}
    | OEND {}
    | end {}
namedModuleDefnBlock :
    | OBLOCKBEGIN wrappedNamedModuleDefn OBLOCKEND {}
    | OBLOCKBEGIN moduleDefnsOrExpr OBLOCKEND {}
    | wrappedNamedModuleDefn {}
    | path {}
wrappedNamedModuleDefn :
    | structOrBegin moduleDefnsOrExprPossiblyEmpty end {}
structOrBegin :
    | struct {}
    | begin {}
openDecl :
    | open path {}
    | open type appType {}
fileNamespaceImplList :
    | fileNamespaceImpl fileNamespaceImplList {}
    | fileNamespaceImpl {}
fileNamespaceImpl :
    | namespaceIntro "="? fileModuleImpl {}
namespaceIntro :
    | namespace rec? path {}
"="? :
    | "=" {}
    | (*empty*) {}
```

#### A.2.1.2 Namespace and Module Signatures

```fsharp
*namespace-decl-group-signature* : 
**namespace** *long-ident* *module-signature-elements*

*module-signature* : 
**module** *ident* = **begin_opt** *module-signature-body* **end_opt**

*module-signature-element* :
**val** **mutable***_opt* *curried-sig*
**val** *value-defn*
**type** *type-signatures*
**exception** *exception-signature*
*module-signature*
*module-abbrev*
*import-decl*

*module-signature-elements* :
**begin_opt** *module-signature-element* ... *module-signature-element* **end_opt**

*module-signature-body* : **begin** *module-signature-elements* **end**

*type-signature* :
*abbrev-type-signature*
*record-type-signature*
*union-type-signature*
*anon-type-signature*
*class-type-signature*
*struct-type-signature*
*interface-type-signature*
*enum-type-signature*
*delegate-type-signature*
*type-extension-signature*

*type-signatures* : *type-signature* ... **and** ... *type-signature*

*type-signature-element* :
*attributes_opt* **access***_opt* **new** : *uncurried-sig*
*attributes_opt* **member** **access***_opt* *member-sig*
*attributes_opt* **abstract** **access***_opt* *member-sig*
*attributes_opt* **override** *member-sig*
*attributes_opt* **default** *member-sig*
*attributes_opt* **static** **member** **access***_opt* *member-sig*
**interface** *type*

*abbrev-type-signature* : *type-name* '=' *type*

*union-type-signature* : 
*type-name* '=' *union-type-cases type-extension-elements-signature_opt*

*record-type-signature* :
*type-name* '=' '{' *record-fields* '}' *type-extension-elements-signature_opt*

*anon-type-signature* : *type-name* '=' **begin** *type-elements-signature* **end**

*class-type-signature* : *type-name* '=' **class** *type-elements-signature* **end**

*struct-type-signature* : *type-name* '=' **struct** *type-elements-signature* **end**

*interface-type-signature* : 
*type-name* '=' **interface** *type-elements-signature* **end**

*enum-type-signature* : *type-name '*=' *enum-type-cases*

*delegate-type-signature* : *type-name '*=' *delegate-sig*

*type-extension-signature* : *type-name type-extension-elements-signature*

*type-extension-elements-signature* : 
**with** *type-elements-signature* **end**
```

### A.2.2 Types and Type Constraints

```fsharp
typ :
    | tupleType "->" typ {}
    | tupleType {}
tupleType :
    | appType "*" tupleOrQuotTypeElements {}
    | ["*" "/" "%"] tupleOrQuotTypeElements {}
    | appType ["*" "/" "%"] tupleOrQuotTypeElements {}
    | appType {}
appType :
    | appType arrayTypeSuffix {}
    | appType HIGH_PRECEDENCE_BRACK_APP arrayTypeSuffix {}
    | appType appTypeConPower {}
    | "(" appTypePrefixArguments ")" appTypeConPower {}
    | powerType {}
    | typar ":>" typ {}
    | "_" ":>" typ {}
arrayTypeSuffix :
    | "[" "]" {}
    | "[" "," "]" {}
    | "[" "," "," "]" {}
    | "[" "," "," "," "]" {}
appTypeConPower :
    | appTypeCon ["@" "^"] atomicRationalConstant {}
    | appTypeCon {}
appTypeCon :
    | path {}
    | typar {}
path :
    | global {}
    | IDENT {}
    | path "." IDENT {}
typar :
    | "'" IDENT {}
    | ["@" "^"] IDENT {}
atomicRationalConstant :
    | atomicUnsignedRationalConstant {}
    | "-" atomicUnsignedRationalConstant {}
atomicUnsignedRationalConstant :
    | INT32 {}
    | "(" rationalConstant ")" {}
rationalConstant :
    | INT32 ["*" "/" "%"] INT32 {}
    | "-" INT32 ["*" "/" "%"] INT32 {}
    | INT32 {}
    | "-" INT32 {}
appTypePrefixArguments :
    | typeArgActual "," typeArgActual typeArgListElements {}
typeArgActual :
    | typ {}
    | typ "=" typ {}
    | typ "=" {}
typeArgListElements :
    | typeArgListElements "," typeArgActual {}
    | typeArgListElements "," {}
    | (*empty*) {}
powerType :
    | atomTypeOrAnonRecdType {}
    | atomTypeOrAnonRecdType ["@" "^"] atomicRationalConstant {}
atomTypeOrAnonRecdType :
    | atomType {}
    | anonRecdType {}
atomType :
    | "#" atomType {}
    | appTypeConPower {}
    | "_" {}
    | "(" typ ")" {}
    | struct "(" appType "*" tupleOrQuotTypeElements ")" {}
    | rawConstant {}
    | null {}
    | const atomicExpr {}
    | FALSE {}
    | TRUE {}
    | appTypeCon typeArgsNoHpaDeprecated {}
    | atomType "." path {}
    | atomType "." path typeArgsNoHpaDeprecated {}
tupleOrQuotTypeElements :
    | appType "*" tupleOrQuotTypeElements {}
    | appType ["*" "/" "%"] tupleOrQuotTypeElements {}
    | appType {}
rawConstant :
    | INT8 {}
    | UINT8 {}
    | INT16 {}
    | UINT16 {}
    | INT32 {}
    | UINT32 {}
    | INT64 {}
    | UINT64 {}
    | NATIVEINT {}
    | UNATIVEINT {}
    | IEEE32 {}
    | IEEE64 {}
    | CHAR {}
    | DECIMAL {}
    | BIGNUM {}
    | STRING {}
    | KEYWORD_STRING {}
    | BYTEARRAY {}
typeArgsNoHpaDeprecated :
    | typeArgsActual {}
    | HIGH_PRECEDENCE_TYAPP typeArgsActual {}
typeArgsActual :
    | "<" typeArgActualOrDummyIfEmpty "," typeArgActualOrDummyIfEmpty typeArgListElements ">" {}
    | "<" typeArgActual ">" {}
    | "<" ">" {}
typeArgActualOrDummyIfEmpty :
    | typeArgActual {}
    | (*empty*) {}
anonRecdType :
    | struct braceBarFieldDeclListCore {}
    | braceBarFieldDeclListCore {}
braceBarFieldDeclListCore :
    | "{|" recdFieldDeclList "|}" {}
recdFieldDeclList :
    | recdFieldDecl seps recdFieldDeclList {}
    | recdFieldDecl seps? {}
recdFieldDecl :
    | attributes? fieldDecl {}
attributes? :
    | attributes {}
    | (*empty*) {}
fieldDecl :
    | mutable? access? IDENT ":" typ {}
mutable? :
    | mutable {}
    | (*empty*) {}
access? :
    | (*empty*) {}
    | access {}
access :
    | private {}
    | public {}
    | internal {}
seps :
    | OBLOCKSEP {}
    | ";" {}
    | OBLOCKSEP ";" {}
    | ";" OBLOCKSEP {}
seps? :
    | seps {}
    | (*empty*) {}
```

```fsharp
typeConstraints :
    | typeConstraints and typeConstraint {}
    | typeConstraint {}
typeConstraint :
    | default typar ":" typ {}
    | typar ":>" typ {}
    | typar ":" struct {}
    | typar ":" IDENT struct {}
    | typar ":" null {}
    | typar ":" "(" classMemberSpfn ")" {}
    | "(" typeAlts ")" ":" "(" classMemberSpfn ")" {}
    | typar ":" delegate typeArgsNoHpaDeprecated {}
    | typar ":" IDENT typeArgsNoHpaDeprecated {}
    | typar ":" IDENT {}
    | appType {}
typeAlts :
    | typeAlts or appType {}
    | appType {}
```

#### A.2.2.1 Equality and Comparison Constraints

```fsharp
*typar* : **equality**
*typar* : **comparison**
```

#### A.2.2.2 Type Providers

```fsharp
*static-parameter* =

*static-parameter-value*

*id* = *static-parameter-value*

*static-parameter-value* =
*const*
const *expr*
```

### A.2.3 Expressions

```fsharp
*expr* :
*const*
( *expr* )
**begin** *expr* **end**
*long-ident-or-op*
*expr* '.' *long-ident-or-op*
*expr expr*
*expr*( *expr* )
*expr*<*types*>
*expr* *infix-op* *expr*
*prefix-op* *expr*
*expr*.[*expr*]
*expr*.[*slice-ranges*]
*expr* <- *expr*
*expr* , ... , *expr*
**new** *type* *expr*
{ **new** *base-call* *object-members* *interface-impls* }
{ *field-initializers* }
{ *expr* **with** *field-initializers* }
[ *expr* ; ... ; *expr* ]
[| *expr* ; ... ; *expr* |]
*expr* { *comp-or-range-expr* }
[ *comp-or-range-expr*]
[| *comp-or-range-expr* |]
**lazy** *expr*
**null**
*expr* : *type*
*expr* :> *type*
*expr* :? *type*
*expr* :?> *type*
**upcast** *expr*
**downcast** *expr*
```

In the following four expression forms, the `in` token is optional if `expr` appears on a subsequent line and is aligned with the `let` token.

```fsharp
let *function-defn* in *expr*

**let** *value-defn* in *expr*

**let** **rec** *function-or-value-defns* **in** *expr*

**use** *ident* = *expr* **in** *expr*

**fun** *argument-pats* -> *expr*

**function** *rules*

**match** *expr* **with** *rules*

**try** *expr* **with** *rules*

**try** *expr* **finally** *expr*

**if** *expr* **then** *expr* *elif-branches_opt* *else-branch*_opt

**while** *expr* **do** *expr* done_opt

**for** *ident* = *expr* **to** *expr* **do** *expr* done_opt

**for** *pat* **in** *expr*-*or-range-expr* **do** *expr* done_opt

**assert** *expr*

<@ *expr* @>

<@@ *expr* @@>

%*expr*

%%*expr*

( *static-typars* : ( *member-sig* ) *expr* )

*expr* $app *expr* // equivalent to "*expr*(*expr*)"

*expr* $sep *expr* // equivalent to "*expr*; *expr*"

*expr* $tyapp < *types >* // equivalent to "*expr*<*types*>"

*expr*< >

*exprs* : *expr* ',' ... ',' *expr*

*expr-or-range-expr* :

*expr*

*range-expr*

*elif-branches* : *elif-branch* ... *elif-branch*

*elif-branch* : **elif** *expr* **then** *expr*

*else-branch* : **else** *expr*

*function-or-value-defn :*

*function-defn*

*value-defn*

*function-defn* :  
**inline*_opt*** *access_opt* *ident-or-op typar-defns_opt* *argument-pats* *return-type_opt* = *expr*

*value-defn* :

**mutable*_opt*** *access_opt* *pat* *typar-defns_opt* *return-type_opt* = *expr*

*return-type* :

**:** *type*

*function-or-value-defns* :

*function-or-value-defn* **and** ... **and** *function-or-value-defn*

*argument-pats*: *atomic-pat* ... *atomic-pat*

*field-initializer* : *long-ident* = *expr*

*field- initializer s* : *field-*

*initializer* ; ... ; *field-initializer*

*object-construction* :

*type expr*

*type*

*base-call* :

*object-construction*

*object-construction* **as** *ident*

*interface-impls* : *interface-impl* ... *interface-impl*

*interface-impl* : **interface** *type* *object-members_opt*

*object-members* : **with** *member-defns* **end**

*member-defns* : *member-defn* ... *member-defn*
```

#### A.2.3.1 Computation and Range Expressions

```fsharp
*comp-or-range-expr* :
   
*comp-expr*
   
*short-comp-expr*
   
*range-expr*
   
*comp-expr* :
   
**let!** *pat* = *expr* **in** *comp-expr*
   
**let** *pat* = *expr* **in** *comp-expr*
   
**do!** *expr* **in** *comp-expr*
   
**do** *expr* **in** *comp-expr*
   
**use!** *pat* = expr **in** *comp-expr*
   
**use** *pat* = expr **in** *comp-expr*
   
**yield!** *expr*
   
**yield** *expr*
   
**return!** *expr*
   
**return** *expr*
   
**if** *expr* **then** *comp-expr*
   
**if** *expr* **then** *comp-expr* **else** *comp-expr*
   
**match** *expr* **with** *comp-rules*
   
**try** *comp-expr* **with** *comp-rules*
   
**try** *comp-expr* **finally** *expr*
   
**while** *expr* **do** *expr* done_opt
   
**for** *ident* = *expr* **to** *expr* **do** *comp-expr* done_opt
   
**for** *pat* **in** *expr*-*or-range-expr* **do** *comp-expr* done_opt
   
*comp-expr*; *comp-expr*
   
*expr*
   
*comp-rule* : *pat* *pattern-guard_opt* -> *comp-expr*
   
*comp-rules* : '|'*_opt* *comp*-*rule* '|' ... '|' *comp*-*rule*
   
*short-comp-expr* : **for** *pat* **in** *expr-or-range-expr* -> *expr*
   
*range-expr* :
   
*expr* .. *expr*
   
*expr* .. *expr* .. *expr*
   
*slice-ranges* : *slice-range* , *…* , *slice-range*
   
*slice-range* :
   
*expr*
   
*expr*..
   
..*expr*
   
*expr*..*expr*
   
*'*'*
```

#### A.2.3.2 Computation Expressions

```fsharp
*expr* { **for** ... }
   
*expr* { **let** ... }
   
*expr* { **let!** ... }
   
*expr* { **use** ... }
   
*expr* { **while** ... }
   
*expr* { **yield** ... }
   
*expr* { **yield!** ... }
   
*expr* { **try** ... }
   
*expr* { **return** ... }
   
*expr* { **return!** ... }
```

#### A.2.3.3 Sequence Expressions

```fsharp
seq { *comp-expr* }
seq { *short-comp-expr* }
```

#### A.2.3.4 Range Expressions

```fsharp
**seq** { *e1* .. *e2* }
**seq** { *e1* .. *e2 .. e3* }
```

#### A.2.3.5 Copy and Update Record Expression

```fsharp
{ *expr* **with** *field-label₁* = *expr₁* ; … ; *field-label_(n)* = *expr_(n)* }
```

#### A.2.3.6 Dynamic Operator Expressions

```fsharp
*expr* ? *ident* → (?) *expr* "*ident*"
   
*expr1* ? ( *expr2*) → (?) *expr1 expr2*
   
*expr1* ? *ident* <- *expr2* → (?<-) *expr1* "*ident*" *expr2*
   
*expr1* ? ( *expr2*) <- *expr3* → (?<-) *expr1 expr2 expr3*
```

"*ident*" is a string literal that contains the text of *ident*.

#### A.2.3.7 AddressOf Operators

```fsharp
&*expr*
&&*expr*
```

#### A.2.3.8 Lookup Expressions

```fsharp
*e1*.[*eargs*] → *e1*.get_Item( *eargs* )
*e1*.[*eargs*] <- *e3* → *e1*.set_Item( *eargs*, *e3* )
```

#### A.2.3.9 Slice Expressions

```fsharp
*e1.[sliceArg1, ,,, sliceArgN] → e1.GetSlice( args1,…,argsN)*
*e1.[sliceArg1, ,,, sliceArgN] <- expr → e1.SetSlice( args1,…,argsN, expr)*
```

where each `sliceArgN` is a *slice-range* and translated to `argsN` (giving one or two args) as follows:

```fsharp
**     → None, None  
e1..   → Some e1, None  
..e2   → None, Some e2  
e1..e2 → Some e1, Some e2  
idx    → idx*
```

#### A.2.3.10 Shortcut Operator Expressions

```fsharp
*expr1* && *expr2* → if *expr1* then *expr2* else false
*expr1* || *expr2* → if *expr1* then true else *expr2*
```

#### A.2.3.11 Deterministic Disposal Expressions

```fsharp
**use** *ident* = *expr1* **in** *expr2*
```

### A.2.4 Patterns

```fsharp
*rule* : *pat* *pattern-guard_opt* -> *expr*

*pattern-guard* : **when** *expr*

*pat* :

*const*

*long-ident* *pat-param_opt* *pat_opt*

_

*pat* **as** *ident*

*pat* '|' *pat*

*pat* '&' *pat*

*pat* :: *pat*

*pat* : *type*

*pat*,...,*pat*

( *pat* )

*list-pat*

*array-pat*

*record-pat*

:? *atomic-type*

:? *atomic-type* **as** *ident*

**null**

*attributes* *pat*

*list-pat* :

[ ]

[ *pat* ; ... ; *pat* ]

*array-pat* :

[| |]

[| *pat* ; ... ; *pat* |]

*record-pat* : { *field-pat* ; ... ; *field-pat* }

*atomic-pat :*

*pat* one of

*const* *long-ident* *list-pat* *record-pat* *array-pat* (*pat*)

:? *atomic-type*

**null** _ _

*field-pat* : *long-ident* = *pat*

*pat-param* :

*const*

*long-ident*

[ *pat-param* ; ... ; *pat-param* ]

( *pat-param*, ..., *pat-param* )

*long-ident* *pat-param*

*pat-param* : *type*

<@ *expr* @>

<@@ *expr* @@>

**null**

*pats* : *pat* , ... , *pat*

*field-pats* : *field-pat* ; ... ; *field-pat*

*rules* : '|'*_opt* *rule* '|' ... '|' *rule*
```

### A.2.5 Type Definitions

```fsharp
*type-defn* :

*abbrev-type-defn*

*record-type-defn*

*union-type-defn*

*anon-type-defn*

*class-type-defn*

*struct-type-defn*

*interface-type-defn*

*enum-type-defn*

*delegate-type-defn*

*type-extension*

*type-name* : *attributes_opt access_opt ident* *typar-defns*_opt

*abbrev-type-defn* : *type-name* = *type*

*union-type-defn* : *type-name* '=' *union-type-cases type-extension-elements_opt*

*union-type-cases* : '|'*opt* *union-type-case* '|' ... '|' *union-type-case*

*union-type-case* : *attributes_opt union-type-case-data*

*union-type-case-data* :

*ident* -- nullary union case

*ident* **of** *union-type-field* * ... * *union-type-field* -- n-ary union case

*ident* : *uncurried-sig* -- n-ary union case

*union-type-field* :

*type* -- unnamed union type field

*ident* : *type* -- named union type field

*anon-type-defn* :

*type-name* *primary-constr-args_opt* *object-val_opt* '=' **begin** *class-type-body* **end**

*record-type-defn* : *type-name* = '{' *record-fields* '}' *type-extension-elements_opt*

*record-fields* : *record-field* ; ... ; *record-field* ;*_opt*

*record-field* : *attributes_opt* **mutable***_opt* *access_opt* *ident* : *type*

*class-type-defn* :

*type-name* *primary-constr-args_opt* *object-val_opt* '=' **class** *class-type-body* **end**

*as-defn* : **as** *ident*

*class-type-body* :

**begin_opt** *class-inherits-decl_opt class-function-or-value-defns_opt type-defn-elements_opt***end_opt**

*class-inherits-decl* : **inherit** *type* *expr_opt*

*class-function-or-value-defn* :

*attributes_opt* **static***_opt* **let** **rec***_opt* *function-or-value-defns*

*attributes_opt* **static***_opt* **do** *expr*

*struct-type-defn* :

*type-name primary-constr-args_opt* *as-defn_opt* '=' **struct** *struct-type-body* **end**

*struct-type-body* : *type-defn-elements*

*interface-type-defn* : *type-name* '=' **interface** *interface-type-body* **end**

*interface-type-body* : *type-defn-elements*

*exception-defn* :

*attributes_opt* **exception** *union-type-case-data*

*attributes_opt* **exception** *ident* = *long-ident*

*enum-type-defn* : *type-name '*=' *enum-type-cases*

*enum-type-cases* : '|'*_opt* *enum-type-case* '|' ... '|' *enum-type-case*

*enum-type-case* : *ident* '=' *const*

*delegate-type-defn* : *type-name '*=' *delegate-sig*

*delegate-sig* : delegate **of** *uncurried-sig*

*type-extension* : *type-name type-extension-elements*

*type-extension-elements* : **with** *type-defn-elements* **end**

*type-defn-element* :

*member-defn*

*interface-impl*

*interface-signature*

*type-defn-elements :* *type-defn-element ... type-defn-element*

*primary-constr-args* : *attributes_opt* *access_opt* (*simple-pat, ... , simplepat*)

*simple-pat* :

| *ident*

| *simple-pat* : *type*

*additional-constr-defn* :

*attributes_opt* *access_opt* **new** *pat* *as-defn* = *additional-constr-expr*

*additional-constr-expr* :

*stmt* ';' *additional-constr-expr*

*additional-constr-expr* **then** *expr*

**if** *expr* **then** *additional-constr-expr* **else** *additional-constr-expr*

**let** val-decls **in** *additional-constr-expr*

*additional-constr-init-expr*

*additional-constr-init-expr* :

'{' *class-inherits-decl* *field-initializers* '}'

**new** *type* *expr*

*member-defn* :

*attributes_opt* **static***_opt* **member** *access_opt* *method-or-prop-defn*

*attributes_opt* **abstract** **member***_opt* *access_opt* *member-sig*

*attributes_opt* **override** *access_opt* *method-or-prop-defn*

*attributes_opt* **default** *access_opt* *method-or-prop-defn*

*attributes_opt* **static***_opt* **val** **mutable***_opt* *access_opt* *ident* *: type*

*additional-constr-defn*

*method-or-prop-defn* :

*ident_opt* *function-defn*

*ident_opt value-defn*

*ident_opt ident* **with** *function-or-value-defns*

**member** *ident* = *exp*

**member** *ident* = *exp* **with get**

**member** *ident* = *exp* **with set**

**member** *ident* = *exp* **with get,set**

**member** *ident* = *exp* **with set,get**

*member-sig* :

*ident typar-defns_opt* : *curried-sig*

*ident typar-defns_opt* : *curried-sig* **with get**

*ident typar-defns_opt* : *curried-sig* **with set**

*ident typar-defns_opt* : *curried-sig* **with get,set**

*ident typar-defns_opt* : *curried-sig* **with set,get**

*curried-sig* : *args-spec* -> *...* -> *args-spec* -> *type*

*uncurried-sig* : *args-spec* -> *type*

*args-spec* : *arg-spec * ... * arg-spec*

*arg-spec* : *attributes_opt* *arg-name-spec_opt* *type*

*arg-name-spec* : ?*_opt* *ident* :

*interface-spec* : **interface** *type*
```

1. Property Members

```fsharp
static_opt member *ident*.*_opt* *ident* = *expr*
static_opt member *ident*.*_opt* *ident* **with get** *pat* = *expr*
static_opt member *ident*.*_opt* *ident* **with set** *pat_opt pat*= *expr*
static_opt member *ident*.*_opt* *ident* **with get** *pat* = *expr* **and set** *pat_opt pat* = *expr*
static_opt member *ident*.*_opt* *ident* **with set** *pat_opt pat* = *expr* **and get** *pat* = *expr*
```

2. Method Members

```fsharp
**static***_opt* **member** *ident*.*_opt* *ident* *pat1* ... *patn* = *expr*
```

3. Abstract Members

```fsharp
**abstract** *access_opt* *member-sig*

*member-sig* :

*ident typar-defns_opt : curried-sig*

*ident typar-defns_opt : curried-sig* **with get**

*ident typar-defns_opt : curried-sig* **with set**

*ident typar-defns_opt : curried-sig* **with get, set**

*ident typar-defns_opt : curried-sig* **with set, get**

*curried-sig :* *args-spec₁* -> *...* -> *args-spec_(n) -> type*
```

4. Implementation Members

```fsharp
**override** *ident*.*ident* *pat1* ... *patn* = *expr*

**default** *ident*.*ident* *pat1* ... *patn* = *expr*
```

### A.2.6 Units Of Measure

```fsharp
*measure-literal-atom* :

*long-ident*

( *measure-literal-simp* )

*measure-literal-power* :

*measure-literal-atom*

*measure-literal-atom* ^ *int32*

*measure-literal-seq* :

*measure-literal-power*

*measure-literal-power measure-literal-seq*

*measure-literal-simp* :

*measure-literal-seq*

*measure-literal-simp* * *measure-literal-simp*

*measure-literal-simp* / *measure-literal-simp*

/ *measure-literal-simp*

1

*measure-literal :*

_

*measure-literal-simp*

*const* :

...

*sbyte* < *measure-literal* >

*int16* < *measure-literal* >

*int32* < *measure-literal* >

*int64* < *measure-literal* >

*ieee32* < *measure-literal* >

*ieee64* < *measure-literal* >

*decimal* < *measure-literal* >

*measure-atom* :

*typar*

*long-ident*

( *measure-simp* )

*measure-power* :

*measure-atom*

*measure-atom* ^ *int32*

*measure-seq* :

*measure-power*

*measure-power measure-seq*

*measure-simp* :

*measure-seq*

*measure-simp* * *measure-simp*

*measure-simp* / *measure-simp*

/ *measure-simp*

1

*measure* :

_

*measure-simp*
```

### A.2.7 Custom Attributes and Reflection

```fsharp
*attribute* : *attribute-target*:*_opt* *object-construction*

*attribute-set* : [< *attribute* ; ... ; *attribute* >]

*attributes* : *attribute-set* ... *attribute-set*

*attribute-target* :

**assembly**

**module**

**return**

**field**

**property**

**param**

**type**

**constructor**

**event**
```

### A.2.8 Compiler Directives

Compiler directives in non-nested modules or namespace declaration groups:

```fsharp
#*id* *string* ... *string*
```
