# Chapter 8 Peephole Optimization

Following code generation there are further optimizations that are possible. The code is scanned a few instructions at a time (the peephole) looking for combinations of instructions that may be replaced by more efficient combinations. Typical optimizations performed by a peephole optimizer include copy propagation across register loads and stores, strength reduction in arithmetic operators and memory access, and branch chaining.

We do not illustrate a peephole optimizer for `Simple`.

```C
x := x + 1   ld x        ld x
             inc         inc
             store x     dup
y := x + 3   ld x
             ld 3        ld 3
             add         add
             store y     store y
x := x + z   ld x
             ld z        ld z
             add         add
             store x     store x
```