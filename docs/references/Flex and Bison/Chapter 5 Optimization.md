# Chapter 5 Optimization

It may be possible to restructure the parse tree to reduce its size or to present a parse to the code generator from which the code generator is able to produce more efficient code. Some optimizations that can be applied to the parse tree are illustrated using source code rather than the parse tree.

## Constant folding:

```
I := 4 + J - 5; --> I := J - 1;
```

or

```
I := 3; J := I + 2; --> I := 3; J := 5
```



## Loop-Constant code motion:

From:

```
while (count < limit) do
INPUT SALES;
VALUE := SALES * ( MARK_UP + TAX );
OUTPUT := VALUE;
COUNT := COUNT + 1;
end; -->
```

to:

```
TEMP := MARK_UP + TAX;
while (COUNT < LIMIT) do
INPUT SALES;
VALUE := SALES * TEMP;
OUTPUT := VALUE;
COUNT := COUNT + 1;
end;
```



## Induction variable elimination: 

Most program time is spent in the body of loops so loop optimization can result in significant performance improvement. Often the induction variable of a for loop is used only within the loop. In this case, the induction variable may be stored in a register rather than in memory. And when the induction variable of a for loop is referenced only as an array subscript, it may be initialized to the initial address of the array and incremented by only used for address calculation. In such cases, its initial value may be set

From:

```
For I := 1 to 10 do
A[I] := A[I] + E
```

to:

```
For I := address of first element in A
to address of last element in A
increment by size of an element of A do
A[I] := A[I] + E
```

## Common subexpression elimination:

From:

```
A := 6 * (B+C);
D := 3 + 7 * (B+C);
E := A * (B+C);
```

to:

```
TEMP := B + C;
A := 6 * TEMP;
D := 3 * 7 * TEMP;
E := A * TEMP;
```



## Strength reduction:

```
2 * x --> x + x
2 * x --> shift left x
```



## Mathematical identities:

```
a*b + a*c --> a*(b+c)
a - b --> a + ( - b )
```

We do not illustrate an optimizer in the parser for Simple.