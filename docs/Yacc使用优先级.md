# Yacc使用优先级

本示例是龙书4.9.2的示例，见图4-59。

和前一章一样，新建xUnit项目，用F#语言。起个名`C4F59`安装NuGet包：

```powershell
Install-Package FSharpCompiler.Yacc
Install-Package FSharpCompiler.Parsing
Install-Package FSharp.xUnit
Install-Package FSharp.Literals
Install-Package FSharp.Idioms
```

编写语法输入文件`C4F59.yacc`:

```F#
lines : lines expr "\n"
      | lines "\n"
      | /* empty */
      ;
expr : expr "+" expr
     | expr "-" expr
     | expr "*" expr
     | expr "/" expr
     | "(" expr ")"
     | "-" expr %prec UMINUS
     | NUMBER
     ;

%%

%left "+" "-"
%left "*" "/"
%right UMINUS
```

当需要指定运算符号的优先级时，文法输入文件的结构为：

```
rule list
%%
precedence list
```

多行注释同C语言语法`/* .*? */`，不可以嵌套。将被忽略，不放入结果序列。

`%prec UMINUS` 用于为规则命名，这个名称被优先级定义引用。

优先级规则同龙书中yacc的规则相同。这里暂且不深入分解。

文法的写作并非一蹴而就，需要一些手段技巧编写。此处，暂且直接输入书中现成的文法，如何从零开始写文法文件，将下一章详细介绍。

输入文件完成，我们可以解析输入文件，得到结构化的数据。我们首先新建一个测试文件。然后将下面代码放到一个测试中：

```F#
    let path = Path.Combine(__SOURCE_DIRECTORY__, @"C4F59.yacc")
    let text = File.ReadAllText(path)
    let yaccFile = YaccFile.parse text
```

我们得到YaccFile的结构化数据，就是`yaccFile`变量中。

```F#
        let y = {
            mainRules=[
                ["lines";"lines";"expr";"\n"];
                ["lines";"lines";"\n"];
                ["lines"];
                ["expr";"expr";"+";"expr"];
                ["expr";"expr";"-";"expr"];
                ["expr";"expr";"*";"expr"];
                ["expr";"expr";"/";"expr"];
                ["expr";"(";"expr";")"];
                ["expr";"-";"expr"];
                ["expr";"NUMBER"]
                ];
            precedences=[
                LeftAssoc,[TerminalKey "+";TerminalKey "-"];
                LeftAssoc,[TerminalKey "*";TerminalKey "/"];
                RightAssoc,[ProductionKey ["expr";"-";"expr"]]
                ]
            }
```

这个结构化数据，排除了注释和多余的空白，整理后，放入一个记录中。注意输入中的`%prec`已经被消除，整理成等价的形式。F#是值相等，所以，尽管引用不相等，值相等的产生式仍然被当作一个数据。

此时我们可以打印解析表数据。

```F#
    let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

    [<Fact>]
    member this.``generate parse table``() =
        let result =
            [
                "let rules = " + Render.stringify yacc.rules
                "let kernelSymbols = " + Render.stringify yacc.kernelSymbols
                "let parsingTable = " + Render.stringify yacc.parsingTable
            ] |> String.concat System.Environment.NewLine
        output.WriteLine(result)
```

创建一个新模块，将打印的三个值，复制到模块中。代码类似：

```F#
module C4F59ParseTable

let rules = set [["";"lines"];["expr";"(";"expr";")"];....]
let kernelSymbols = Map.ofList [1,"lines";2,"(";3,"expr";....]
let parsingTable = set [0,"",-8;0,"\n",-8;0,"(",-8;....]
```

F#的文件是有依赖顺序的，这个模块应该在测试类之前添加。为了保证生成数据的完整性，添加一个验证Fact：

```F#
    [<Fact>]
    member this.``validate parse table``() =
        Should.equal yacc.rules         C4F59ParseTable.rules
        Should.equal yacc.kernelSymbols C4F59ParseTable.kernelSymbols
        Should.equal yacc.parsingTable  C4F59ParseTable.parsingTable
```

`FSharpCompiler.Yacc`的主要任务生成语法解析表，到这里就完成了，下面介绍解析器的编写。

定义文法的输入类型：

首先，用如下方法，看看文法中用到了哪些个词法符记：

```F#
    [<Fact>]
    member this.``terminals``() =
        let grammar = Grammar.from yaccFile.mainRules
        let terminals = grammar.symbols - grammar.nonterminals
        let result = Render.stringify terminals
        output.WriteLine(result)
```

输出一个字符串集合：

```F#
set ["\n";"(";")";"*";"+";"-";"/";"NUMBER"]
```

很好，现在我们一对一，定义文法的输入类型，词法符记：

```F#
type C4F59Token =
    | EOL
    | LPAREN
    | RPAREN
    | STAR
    | DIV
    | PLUS
    | MINUS
    | NUMBER of int

    member this.getTag() =
        match this with
        | EOL -> "\n"
        | LPAREN -> "("
        | RPAREN -> ")"
        | STAR -> "*"
        | DIV -> "/"
        | PLUS -> "+"
        | MINUS -> "-"
        | NUMBER _ -> "NUMBER"
```

可区分联合的`getTag`成员，是文法输入终结符号，字符串类型，与语义数据的获取桥梁。

我们先不单元测试，我们先继续完成词法分析器，下面是将输入变成词法符记的程序：

```F#
open FSharp.Literals.StringUtils
open System

type ....

    static member from (text:string) =
        let rec loop (inp:string) =
            seq {
                match inp with
                | "" -> ()

                | Prefix @"[\s-[\n]]+" (_,rest) // 空白
                    -> yield! loop rest

                | Prefix @"\n" (_,rest) -> //换行
                    yield EOL
                    yield! loop rest

                | PrefixChar '(' rest ->
                    yield LPAREN
                    yield! loop rest

                | PrefixChar ')' rest ->
                    yield RPAREN
                    yield! loop rest

                | PrefixChar '*' rest ->
                    yield STAR
                    yield! loop rest

                | PrefixChar '/' rest ->
                    yield DIV
                    yield! loop rest

                | PrefixChar '+' rest ->
                    yield PLUS
                    yield! loop rest

                | PrefixChar '-' rest ->
                    yield MINUS
                    yield! loop rest

                | Prefix @"\d+" (n,rest) ->
                    yield  NUMBER(Int32.Parse n)
                    yield! loop rest

                | never -> failwith never
            }
        loop text

```

词法分析器利用两个活动模式`Prefix`，和`PrefixChar` 。`Prefix`检测输入的头部是否匹配给定的正则表达式，如果匹配，将字符串分为两部分，头部的匹配的子字符串，和剩余部分的字符串。如：

```F#
| Prefix @"\d+" (n,rest) ->
```

会成功匹配字符串`"123xyz..."`。并返回元组为`"123"`，`"xyz..."`前者赋值给`n`，后者赋值给`rest`。

`PrefixChar`检测输入的第一个字符是否是给定的字符，如果是，将返回除去头部字符的剩余部分的字符串。如：

```F#
| PrefixChar '-' rest ->
```

会成功匹配字符串`"-123"`。并返回给参数`rest`为`"123"`。

测试词法分析器：

```F#
    [<Fact>]
    member this.``tokenize``() =
        let inp = "-1/2+3*(4-5)" + System.Environment.NewLine
        let tokens = C4F59Token.from inp
        let result = Render.stringify (List.ofSeq tokens)
        output.WriteLine(result)
```

得到结果：

```F#
[MINUS;NUMBER 1;DIV;NUMBER 2;PLUS;NUMBER 3;STAR;LPAREN;NUMBER 4;MINUS;NUMBER 5;RPAREN;EOL]
```

有了解析表数据，我们编写解析器代码：

```F#
module C4F59.C4F59Parser

open FSharpCompiler.Parsing

let parser =
    SyntacticParser(
        C4F59ParseTable.rules,
        C4F59ParseTable.kernelSymbols,
        C4F59ParseTable.parsingTable
        )

let parseTokens tokens =
    parser.parse(tokens,fun (tok:C4F59Token) -> tok.getTag())
```

我们首先打开名字空间`FSharpCompiler.Parsing`，同名NuGet包，利用`SyntacticParser`类型构造解析器，解析器是单例的，只需要初始化构造一次即可。解析方法的第一个参数是词法符记的序列，第二个参数是一个函数，用来告诉解析方法如何获得语义数据类型的标签字符串，作为文法的终结符号。

我们测试这个方法：

```F#
    [<Fact>]
    member this.``parse tokens``() =
        let tokens = [MINUS;NUMBER 1;DIV;NUMBER 2;PLUS;NUMBER 3;STAR;LPAREN;NUMBER 4;MINUS;NUMBER 5;RPAREN;EOL]
        let tree = C4F59Parser. parseTokens tokens
        let result = Render.stringify tree
        output.WriteLine(result)
```

输出结果如下：

```F#
        let y = Interior("lines",[
            Interior("lines",[]);
            Interior("expr",[
                Interior("expr",[
                    Interior("expr",[Terminal MINUS;Interior("expr",[Terminal(NUMBER 1)])]);
                    Terminal DIV;
                    Interior("expr",[Terminal(NUMBER 2)])]);
                Terminal PLUS;
                Interior("expr",[
                    Interior("expr",[Terminal(NUMBER 3)]);
                    Terminal STAR;
                    Interior("expr",[
                        Terminal LPAREN;
                        Interior("expr",[
                            Interior("expr",[Terminal(NUMBER 4)]);
                            Terminal MINUS;
                            Interior("expr",[Terminal(NUMBER 5)])]);
                        Terminal RPAREN])])]);
            Terminal EOL])
```

数据已经整理成为树形，但是这个类型过于通用，我们可以遍历树，根据树上面的数据转换为更专用的数据。我们定义一个表达式数据类型。

```F#
type C4F59Expr =
    | Add      of C4F59Expr * C4F59Expr
    | Sub      of C4F59Expr * C4F59Expr
    | Mul      of C4F59Expr * C4F59Expr
    | Div      of C4F59Expr * C4F59Expr
    | Negative of C4F59Expr
    | Number   of int
```

下面是转换模块：

```F#
module C4F59.C4F59Translation

open FSharpCompiler.Parsing

/// 
let rec translateExpr = function
    | Interior("expr",[e1;Terminal PLUS;e2;]) ->
        C4F59Expr.Add(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal MINUS;e2;]) ->
        C4F59Expr.Sub(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal STAR;e2;]) ->
        C4F59Expr.Mul(translateExpr e1, translateExpr e2)
    | Interior("expr",[e1;Terminal DIV;e2;]) ->
        C4F59Expr.Div(translateExpr e1, translateExpr e2)
    | Interior("expr",[Terminal LPAREN;e;Terminal RPAREN;]) ->
        translateExpr e
    | Interior("expr",[Terminal MINUS;e;]) ->
        C4F59Expr.Negative(translateExpr e)
    | Interior("expr",[Terminal (NUMBER n);]) ->
        C4F59Expr.Number n
    | never -> failwithf "%A" never.firstLevel

/// 
let rec translateLines tree = 
    [
        match tree with
        | Interior("lines",[lines;expr;Terminal EOL]) ->
            yield! translateLines lines
            yield translateExpr expr
        | Interior("lines",[lines;Terminal EOL]) ->
            yield! (translateLines lines)
        | Interior("lines",[]) ->
            ()
        | _ -> failwithf "%A" tree.firstLevel
    ]
```

这里函数的输入参数类型是`ParseTree`类型，此类型位于`FSharpCompiler.Parsing`中，所以先打开名字空间。这个转译函数对应yacc输入文件的文法，每个函数对应一组产生式，依赖最少的非终结符号先定义。对于每个函数的每个匹配项对应一个产生式，匹配项一定是形如：

```F#
| Interior("left side",[symbol1;symbol2;....]) ->
```

文法的产生式一定对应节点`Interior`，节点的标签一定是产生式左侧的那个非终结符号，节点的子节点依次对应产生式右侧的元素。个数是相等的，空产生式对应树节点子节点列表也是空列表。如果子节点为终结符号则，如果字节的为非终结符号，定义一个值，以递归调用翻译函数。如果是终结符号，则显式列出，以匹配同一文法符号的不同产生式的特征。

```F#
        | Interior("lines",[lines;expr;Terminal EOL]) ->
            yield! (translateLines lines)
            yield expr
```

对应产生式：

```F#
lines : lines expr "\n"
```

产生式对应`Interior`；`"lines"`对应左侧的文法符号；子节点列表`,[lines;expr;Terminal EOL]`对应产生式`: lines expr "\n"`；其中`lines`对应`lines`；`expr`对应`expr`；`Terminal EOL`对应`"\n"`。

非终结符号对应的子树将会被递归翻译。如果确定无用，也可能被直接丢弃。终结符号对应的子树将会被提取有用的数据被转化成新的数据形式，或无用的数据被丢弃。

每组产生式的最后有一个默认的后备匹配项目，正确的程序永远用不到，如果用到只会用到输入树第一层的子树数据，即可以确定错误：

```F#
| _ -> failwithf "%A" tree.firstLevel
```

测试：

```F#
    [<Fact>]
    member this.``translate to expr``() =
        let tokens = [MINUS;NUMBER 1;DIV;NUMBER 2;PLUS;NUMBER 3;STAR;LPAREN;NUMBER 4;MINUS;NUMBER 5;RPAREN;EOL]
        let tree = C4F59Parser. parseTokens tokens
        let exprs = C4F59Translation.translateLines tree

        let result = Render.stringify exprs
        output.WriteLine(result)
```

输出结果：

```F#
[Add(Div(Negative(Number 1),Number 2),Mul(Number 3,Sub(Number 4,Number 5)))]
```

是不是清爽多了。

本章介绍了如何编译带优先级的文法，这个优先级已经是别人调试正确的文法。下一章将介绍如何从零写优先级。

























