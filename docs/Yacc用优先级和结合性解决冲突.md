# Yacc用优先级和结合性解决冲突

上一章，我们演示了一个已经写好的文法，带有优先级和结合性的。本章内容为如何利用Yacc写优先级和结合性。

和上一章一样新建一个F#语言的xUnit测试项目，并安装依赖的NuGet包。

然后，我们先输入设计好的文法，此时我们还不知道是否需要解决优先级：

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
     | "-" expr
     | NUMBER
     ;
```

如前所述，我们可以获取yacc输入文件的结构化表示：

```F#
    let path = Path.Combine(__SOURCE_DIRECTORY__, @"C4F59.yacc")
    let text = File.ReadAllText(path)
    let yaccFile = YaccFile.parse text
```

上面代码最后的`yaccFile`的`mainRules`，我们打印出来看：

```F#

```

我们生成歧义表：

```F#
let tbl = AmbiguousTable.create yaccFile.mainRules
```

歧义表测试，首先，文法设计不能有产生式冲突，否则需要修改产生式：

```F#
    [<Fact>]
    member this.``there are not production conflicts``() =
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        Assert.True(pconflicts.IsEmpty)
```

其次，文法设计尽量不要过度使用同一个符号，这只是警告，如果文法通过，可以无视：

```F#
    [<Fact>]
    member this.``show warning``() =
        let warning = ConflictFactory.overloadsWarning tbl
        Assert.True(warning.IsEmpty)
```

当文法产生式完成，不再修改后，可以查看移动与归约的冲突：

```F#
    [<Fact>]
    member this.``resolve shift reduce conflicts``() =
        //优先级冲突
        let srconflicts = ConflictFactory.shiftReduceConflict tbl
        //show srconflicts
        let y = set [
            set [["expr";"-";"expr"];["expr";"expr";"*";"expr"]];
            set [["expr";"-";"expr"];["expr";"expr";"+";"expr"]];
            set [["expr";"-";"expr"];["expr";"expr";"-";"expr"]];
            set [["expr";"-";"expr"];["expr";"expr";"/";"expr"]];
            set [["expr";"expr";"*";"expr"]];
            set [["expr";"expr";"*";"expr"];["expr";"expr";"+";"expr"]];
            set [["expr";"expr";"*";"expr"];["expr";"expr";"-";"expr"]];
            set [["expr";"expr";"*";"expr"];["expr";"expr";"/";"expr"]];
            set [["expr";"expr";"+";"expr"]];
            set [["expr";"expr";"+";"expr"];["expr";"expr";"-";"expr"]];
            set [["expr";"expr";"+";"expr"];["expr";"expr";"/";"expr"]];
            set [["expr";"expr";"-";"expr"]];
            set [["expr";"expr";"-";"expr"];["expr";"expr";"/";"expr"]];
            set [["expr";"expr";"/";"expr"]]]

        Should.equal y srconflicts
```

两个产生式的集合是第一个和第二个产生式冲突。一个产生式的集合，是自己和自己冲突。我们根据打印的冲突产生式很容易知道那些产生式需要指定优先级和结合性。注意连字号可以指向两个产生式。负号，和减号。需要在第一部分给产生式命名。使用`%prec`，别忘了文法产生式与优先级列表之间需要`%%`分隔。

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

