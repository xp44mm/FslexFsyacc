# Yacc入门示例

本文向大家推荐一个yacc语法自动构建器，`FSharpCompiler.Yacc`和`FSharpCompiler.Parsing`前者是解析器生成工具，后者是解析器的依赖项。顾名思义，这个编译器是专门为F#语言使用的。这个文件位于[GitHub](https://github.com/xp44mm/FSharpCompiler.Docs/tree/master/FSharpCompiler.Docs)大家有问题，可以去提交issue，将会及时得到更正或补充。

龙书示例4.69，文法如下：

```
E -> E + T | T
T -> T * F | F
F -> ( E ) | digit
```

新建一个文本文件，输入以下内容：

```
line   : expr "\n"
       ;
expr   : expr "+" term
       | term
       ;
term   : term "*" factor
       | factor
       ;
factor : "(" expr ")"
       | DIGIT
       ;
```

文法文件与文法几乎相同，一一对应。这个文件可以任何的扩展名，建议使用`*.yacc`作为扩展名。表示这个文件是yacc语法规范文件。

说明：

语法符号说明：

语法符号字面量，为双引号包围的字符串字面量。转义规则同JSON语法。

语法符号不可以为空字符串`""`，空字符串被yacc内部用作特殊符号。

当语法符号匹配正则表达式`\w+`时，可以省略包围的引号，用标识符表示法。比如,`"EXP"`可以简写为`EXP`.

空白仅用于分隔语法符号。



翻译规则说明：

```
<head> : <body>_1
       | <body>_2
         ...
       | <body>_n
       ;
```

第一个产生式的左侧符号是开始符号。

空白仅用于分隔语法符号。

无需输入语义动作，语义动作已经内置为构造语法树。



新建一个FSharp xUnit测试项目：

安装NuGet包：

```
Install-Package FSharpCompiler.Yacc
Install-Package FSharpCompiler.Parsing
Install-Package FSharp.xUnit
Install-Package FSharp.Literals
Install-Package FSharp.Idioms
```

前两个包是编译器的主包，其余的是各种常用功能的函数库。本文为节约篇幅，直接使用库函数，而不展开代码。

把上面输入的规则文件添加到项目中，为了方便。

将文件中的内容读取到变量中：

```F#
let path = Path.Combine(__SOURCE_DIRECTORY__, @"E69.yacc")
let text = File.ReadAllText(path)
```

解析输入文本：

```F#
let yaccFile = YaccFile.parse text
let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)
```

生成数据:

```F#
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

把输出的结果，复制到一个单独的模块文件中比如：

```F#
module E69ParseTable

let rules = set [....]
let kernelSymbols = Map.ofList [....]
let parsingTable = set [....]
```

数字太长太枯燥，我们用点号略过。我们可能经常升级改动输入文件，这是验证输出文件和输入文件是一致的方法：

```F#
    [<Fact>]
    member this.``validate parse table``() =
        Should.equal yacc.rules E69ParseTable.rules
        Should.equal yacc.kernelSymbols E69ParseTable.kernelSymbols
        Should.equal yacc.parsingTable E69ParseTable.parsingTable
```

这里需要NuGet程序包`FSharp.xUnit`，包里面只有一个方法`Should.equal x y`。这个方法好处是可以类型推导，省去输入类型参数的麻烦。

这就是yacc的基本用法。下一步，我们将用生成的结果，构造一个解析器。

首先定义解析器的输入，输入为一个F#可区分联合的符记序列，我们需要知道，语法分析其中那些符号是终结符号

```F#
    [<Fact>]
    member this.``terminals``() =
        let grammar = Grammar.from yaccFile.mainRules
        let terminals = grammar.symbols - grammar.nonterminals
        let result = Render.stringify terminals
        output.WriteLine(result)
```

说明`Grammar`是yacc包中带的工具类型，可以获取文法的常用性质，这里是获知文法中所有的终结符号集合。`Render.stringify`位于`FSharp.Literals`中，用于打印F#数据字面量，可以理解为数据源代码。

打印的终结符号集合结果如下：

```F#
set ["\n";"(";")";"*";"+";"DIGIT"]
```

当然，有熟练的人可以自己计算文法中的那些符号是终结符号。接下来，我们根据此结果编写Token类型：

```F#
type ExpToken =
    | EOL
    | LPAREN
    | RPAREN
    | STAR
    | PLUS
    | DIGIT of int
```

F#可区分联合的标签名首字母必须大写，且是合法的标识，所以无法做到Token的标签和终结符号完全相同。还需要定义一个成员，用于把Token数据映射到终结符号：

```F#
    member this.getTag() =
        match this with
        | EOL -> "\n"
        | LPAREN -> "("
        | RPAREN -> ")"
        | STAR -> "*"
        | PLUS -> "+"
        | DIGIT _ -> "DIGIT"
```

定义了语法的输入类型Token，这时就可以写解析器了。

```F#
let parser =
    SyntacticParser(
        E69ParseTable.rules,
        E69ParseTable.kernelSymbols,
        E69ParseTable.parsingTable)

let parseTokens tokens =
    parser.parse(tokens,fun (tok:ExpToken) -> tok.getTag())
```

`SyntacticParser` 位于`FSharpCompiler.Parsing`中的类型，其输入正是前面我们使用Yacc得到的数据，对于一个文法，我们只需要一个实例，应该把它的构造代码放在解析器外面。`parser.parse`是生成抽象语法树，第一个参数是词法符记的序列，第二个参数用来向解析器提供终结符号，`getTag`去除了词法符记的语义数据，只保留解析器所需要的文本数据，当树节点解析成功时，根据终结符号在序列中的位置取出词法符记的语义数据。绑定到抽象语法树相应的节点上。

此时，我们可以测试这个函数：

```F#
    [<Fact>]
    member this.``parse tokens``() =
        let tokens = [DIGIT 1;PLUS;DIGIT 2;STAR;LPAREN;DIGIT 4;PLUS;DIGIT 3;RPAREN;EOL]
        let tree = E69.Parser.parseTokens tokens
        let result = Render.stringify tree
        output.WriteLine(result)
```

可以得到语法树的结果：

```F#
        let y = Interior("line",[
            Interior("expr",[
                Interior("expr",[
                    Interior("term",[
                        Interior("factor",[
                            Terminal(DIGIT 1)])])]);
                Terminal PLUS;
                Interior("term",[
                    Interior("term",[
                        Interior("factor",[
                            Terminal(DIGIT 2)])]);
                    Terminal STAR;
                    Interior("factor",[
                        Terminal LPAREN;
                        Interior("expr",[
                            Interior("expr",[
                                Interior("term",[Interior("factor",[Terminal(DIGIT 4)])])]);
                            Terminal PLUS;
                            Interior("term",[
                                Interior("factor",[Terminal(DIGIT 3)])])]);
                        Terminal RPAREN])])]);
            Terminal EOL])
```

这是一个抽象语法树对象，这个树用可区分联合递归表达，此对象在`FSharpCompiler.Parsing`包中定义：

```F#
type ParseTree<'tok> =
    | Interior of symbol:string * children: list<ParseTree<'tok>>
    | Terminal of 'tok
```

这个数据结构是自解释的，Interior是内部节点，也就是龙书中的非终结符号，Terminal是终结符号。

如何从普通文本得到词法符记序列，利用.net库中的字符串和正则表达式方法，很容易就可以写一个词法分析器，把它作为词法符记类型的静态构造函数是很自然的：

```F#
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

                | PrefixChar '+' rest ->
                    yield PLUS
                    yield! loop rest

                | Prefix @"\d+" (lexeme,rest) ->
                    yield  DIGIT(Int32.Parse lexeme)
                    yield! loop rest

                | never -> failwith never
            }
        loop text
```

这里用到了活动模式，位于`FSharp.Literals.StringUtils`模块中，需要安装`FSharp.Literals`包。活动模式`Prefix`后面跟着一个正则表达式，匹配输入字符串的最开头的部分，则成功。输入的正则表达式不需要使用`^`进行定位。活动模式`PrefixChar` 带有一个字符参数，如果输入字符串的首字符匹配，则成功。

测试这个函数：

```F#
    [<Fact>]
    member this.``tokenize``() =
        let inp = "1+2*(4+3)" + System.Environment.NewLine
        let tokens = E69.ExpToken.from inp

        let result = Render.stringify (List.ofSeq tokens)
        output.WriteLine(result)
```

如上所属最后两行是打印输出结果的功能。第一行是输入，第二行得到函数的输出：

```F#
[DIGIT 1;PLUS;DIGIT 2;STAR;LPAREN;DIGIT 4;PLUS;DIGIT 3;RPAREN;EOL]
```

这个实例的源代码在[GitHub](https://github.com/xp44mm/FSharpCompiler.Docs)上。