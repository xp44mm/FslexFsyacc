# Chapter 6 Virtual Machines

A *computer* constructed from actual physical devices is termed an *actual computer* or *hardware computer*. From the programming point of view, it is the instruction set of the hardware that defines a machine. An operating system is built on top of a machine to manage access to the machine and to provide additional services. The services provided by the operating system constitute another machine, a *virtual machine*.

A programming language provides a set of operations. Thus, for example, it is possible to speak of a Pascal computer or a Scheme computer. For the programmer, the programming language is the computer; the programming language defines a virtual computer. The virtual machine for Simple consists of a data area which contains the association between variables and values and the program which manipulates the data area.

Between the programmer’s view of the program and the virtual machine provided by the operating system is another virtual machine. It consists of the data structures and algorithms necessary to support the execution of the program.

This virtual machine is the run time system of the language. Its complexity may range in size from virtually nothing, as in the case of FORTRAN, to an extremely sophisticated system supporting memory management and inter process communication as in the case of a concurrent programming language like SR. The run time system for Simple as includes the processing unit capable of executing the code and a data area in which the values assigned to variables are accessed through an offset into the data area.

User programs constitute another class of virtual machines.

## A Stack Machine

The S-machine [^1] is a stack machine organized to simplify the implementation of block structured languages. It provides dynamic storage allocation through a stack of activation records. The activation records are linked to provide support for static scoping and they contain the context information to support procedures.

[^ 1]: This is an adaptation of: Niklaus Wirth, Algorithms + Data Structures = Programs Prentice—Hall, Englewood Cliffs, N.J., 1976.

### Machine Organization: 

The S-machine consists of two stores, a program store, C(organized as an array and is read only), and a data store, S(organized as a stack). There are four registers, an instruction register, IR, which contains the instruction which is being interpreted, the stack top register, T, which contains the address of the top element of the stack, the program address register, PC, which contains the address of the next instruction to be fetched for interpretation, and the current activation record register, AR, which contains the base address of the activation record of the procedure which is being interpreted. Each location of C is capable of holding an instruction. Each location of S is capable of holding an address or an integer. Each instruction consists of three fields, an operation code and two parameters.

### Instruction Set: 

S-codes are the machine language of the S-machine. S-codes occupy four bytes each. The first byte is the operation code (opcode). There are nine basic S-code instructions, each with a different opcode. The second byte of the S-code instruction contains either 0 or a lexical level offset, or a condition code for the conditional jump instruction. The last two bytes taken as a 16-bit integer form an operand which is a literal value, or a variable offset from a base in the stack, or a S-code instruction location, or an operation number, or a special routine number, depending on the opcode.

The action of each instruction is described using a mixture of English language description and mathematical formalism. The mathematical formalism is used to note changes in values that occur to the registers and the stack of the S-machine. Data access and storage instructions require an offset within the activation record and the level difference between the referencing level and the definition level. Procedure calls require a code address and the level difference between the referencing level and the definition level.



```
Instruction Operands Comments
READ 0,N Input integer in to location N: S(N) := Input
WRITE Output top of stack: Output := S(T); T:= T-1
OPR Arithmetic and logical operations
0,0 process and function, return operation
T:= AR-1; AR:= S(T+2); P:= S(T+3)
ADD ADD: S(T-1) := S(T-1) + S(T); T := T-1
SUB SUBTRACT: S(T-1) := S(T-1) - S(T); T := T-1
MULT MULTIPLY: S(T-1) := S(T-1) * S(T); T := T-1
DIV DIVIDE: S(T-1) := S(T-1) / S(T); T := T-1
MOD MOD: S(T-1) := S(T-1) mod S(T); T := T-1
EQ TEST EQUAL:
S(T-1) := if S(T-1) = S(T) then 1 else 0; T:= T-1
LT TEST LESS THAN:
S(T-1) := if S(T-1) < S(T) then 1 else 0; T:= T-1
GT TEST GREATER THAN:
S(T-1) := if S(T-1) > S(T) then 1 else 0; T:= T-1
LD NUMBER 0,N LOAD literal value onto stack: T:= T+1; S(T):= N
LD VAR L,N LOAD value of variable at level offset L, base
offset N in stack onto top of stack
T:= T + 1; S(T):= S(f(L,AR)+N)+3
STORE L,N store value on top of stack into variable location
at level offset L, base offset N in stack
S(f(ld,AR)+os+3):= S(T); T:= T - 1
CAL L,N call PROC or FUNC at S-code location N declared
at level offset L
S(T+1):= f(ld,AR);{Static Link}
S(T+2):= AR; {Dynamic Link}
S(T+3):= P; {Return Address}
AR:= T+1; {Activation Record}
P:= cos; {Program Counter}
T:= T+3 {Stack Top}
CAL 255,0 call procedure address in memory: POP address, PUSH return
address, JUMP to address
DATA 0,NN ADD NN to stack pointer T := T+NN
GOTO 0,N JUMP: P := N
JMP FALSE C,N JUMP: if S(T) = C then P:= N; T:= T-1
```

Where the static level difference between the current procedure and the called procedure is ld. os is the offset within the activation record, ld is the static level difference between the current activation record and the activation record in which the value is to be stored and

```C
f(ld,a) = if i=0 then a else f(i-1,S(a))
```

### Operation: 

The registers and the stack of the S-machine are initialized as follows:

```C
P = 0. {Program Counter}
AR = 0; {Activation Record}
T = 2; {Stack Top}
S[0] := 0; {Static Link}
S[1] := 0; {Static Dynamic Link}
S[2] := 0; {Return Address}
```

The machine repeatedly fetches the instruction at the address in the register P, increments the register P and executes the instruction until the register P contains a zero.

```C
execution-loop : 
    I:= C(P);
    P:= P+1;
    interpret(I);
    if { P ≤ 0 -> halt
       & P > 0 -> execution-loop }
```

## The Stack Machine Module

The implementation of the stack machine is straight forward.

The instruction set and the structure of an instruction are defined as follows:

```C
/* OPERATIONS: Internal Representation */
enum code_ops { 
    HALT, STORE, JMP_FALSE, GOTO,
    DATA, LD_NUM, LD_VAR,
    READ_NUM, WRITE_NUM,
    LT, EQ, GT, ADD, SUB, MULT, DIV, PWR };

/* OPERATIONS: External Representation */
char *op_name[] = {
    "halt", "store", "jmp_false", "goto",
    "data", "ld_int", "ld_var",
    "in_int", "out_int",
    "lt", "eq", "gt", "add", "sub", "mult", "div", "pwr" };
struct instruction
{
    enum code_ops op;
    int arg;
};
```

Memory is separated into two segments, a code segment and a runtime data and expression stack.

```C
struct instruction code[999];
int stack[999];
```

The definitions of the registers, the program counter `pc`, the instruction register `ir`, the activation record pointer `ar` (which points to be beginning of the current activation record), and the pointer to the top of the stack `top`, are straight forward.

```C
int pc = 0;
struct instruction ir;
int ar = 0;
int top = 0;
```

The fetch-execute cycle repeats until a halt instruction is encountered.

```C
void fetch_execute_cycle()
{ 
    do { /* Fetch */
         ir = code[pc++];
         /* Execute */
         switch (ir.op) {
case HALT : printf( "halt\n" ); break;
case READ_NUM : printf( "Input: " );
                scanf( "%ld", &stack[ar+ir.arg] ); break;
case WRITE_NUM : printf( "Output: %d\n", stack[top--] ); break;
case STORE : stack[ir.arg] = stack[top--]; break;
case JMP_FALSE : if ( stack[top--] == 0 )
                    pc = ir.arg;
                 break;
case GOTO : pc = ir.arg; break;
case DATA : top = top + ir.arg; break;
case LD_NUM : stack[++top] = ir.arg; break;
case LD_VAR : stack[++top] = stack[ar+ir.arg]; break;
case LT : if ( stack[top-1] < stack[top] )
             stack[--top] = 1;
          else stack[--top] = 0;
          break;
case EQ : if ( stack[top-1] == stack[top] )
             stack[--top] = 1;
          else stack[--top] = 0;
          break;
case GT : if ( stack[top-1] > stack[top] )
             stack[--top] = 1;
          else stack[--top] = 0;
          top--;
          break;
case ADD : stack[top-1] = stack[top-1] + stack[top];
           top--;
           break;
case SUB : stack[top-1] = stack[top-1] - stack[top];
           top--;
           break;
case MULT : stack[top-1] = stack[top-1] * stack[top];
            top--;
            break;
case DIV : stack[top-1] = stack[top-1] / stack[top];
           top--;
           break;
case PWR : stack[top-1] = stack[top-1] * stack[top];
           top--;
           break;
default : printf( "%sInternal Error: Memory Dump\n" );
          break;
}
}
while (ir.op != HALT);
}
```