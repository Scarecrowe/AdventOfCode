# 🎄 Advent of Code 2015 - Day 23: Opening the Turing Lock

## 📜 Puzzle Overview

The program for this puzzle is a tiny assembly-like language that works with two registers:

- `a`
- `b`

The instruction set includes:

- `hlf r` - halve register `r`
- `tpl r` - triple register `r`
- `inc r` - increment register `r`
- `jmp offset` - jump to a new instruction by offset
- `jie r, offset` - jump if register `r` is even
- `jio r, offset` - jump if register `r` is exactly one

The program starts at the first instruction and executes until the instruction pointer moves outside the instruction list.

Part 1 asks for the value in register `b` when the program finishes with both registers starting at `0`.

Part 2 uses the same program, but starts with register `a = 1`.

---

## 🧩 Part 1

Determine the final value of register `b` after running the full program with:

- `a = 0`
- `b = 0`

### 💡 Approach

- Parse each instruction into a structured form
- Track register values in a dictionary
- Execute instructions one by one using an instruction pointer
- Apply jumps by modifying the pointer directly
- Stop when the pointer moves beyond the program
- Return the final value of register `b`

---

## 🧩 Part 2

Repeat the same execution, but start with:

- `a = 1`
- `b = 0`

### 💡 Approach

- Reuse the same instruction parser and execution logic
- Only change the initial value of register `a`
- Run the program again
- Return the new value of register `b`

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new OpeningTheTuringLock(this.Input)`
- Calls `Execute()`
- Returns `RegisterB()`

For Part 2:

- Creates `new OpeningTheTuringLock(this.Input, 1)`
- Calls `Execute()`
- Returns `RegisterB()`

The only difference between the two parts is the initial value of register `a`.

---

### `InstructionType.cs`

This enum defines the supported instruction set:

- `Half`
- `Triple`
- `Increment`
- `Jump`
- `JumpIfEven`
- `JumpIfOne`

This gives the parser and executor a strongly typed way to identify each operation.

---

### `Instruction.cs`

This class stores one parsed instruction.

Each instruction contains:

- `Type`
- `Register`
- `Value`

Depending on the instruction:

- register-based instructions use `Register`
- jump instructions use `Value`
- conditional jumps use both

This keeps execution simple because all parsed instructions share the same basic structure.

---

### `OpeningTheTuringLock.cs`

This class contains the full parsing and execution logic.

The constructor:

- creates the register dictionary
- sets:
  - `a` to the supplied starting value
  - `b` to `0`
- parses the input into an indexed instruction map

It stores:

- `Registers`
- `Instructions`

The main public methods are:

- `Execute()`
- `RegisterB()`

---

### Register Storage

The registers are stored in a dictionary keyed by register name.

At startup the values are:

- `a = 0` for Part 1
- `a = 1` for Part 2
- `b = 0`

This makes register access straightforward during instruction execution.

---

### Parsing Instructions

`ParseInsturctions()` converts the raw input lines into `Instruction` objects.

Each line is split into parts and matched by opcode.

Examples:

- `hlf a`
- `tpl a`
- `inc b`
- `jmp +2`
- `jie a, +2`
- `jio b, -1`

Parsing handles:

- the instruction type
- the target register if present
- signed jump offsets

The resulting instructions are stored in a dictionary indexed by instruction position.

---

### Execution Flow

`Execute()` runs through the instruction list using a pointer:

- start at instruction `0`
- execute the current instruction
- move to the next instruction unless a jump changes the pointer
- stop when the pointer moves outside the program

At a high level, the loop behaves like this:

    for pointer = 0 while pointer < instruction count
        execute instruction at pointer
        adjust pointer when jumps occur

Because jump instructions replace the normal flow, the pointer is updated by:

- adding the jump offset
- then subtracting `1` to account for the loop increment

This keeps the jump logic consistent with the `for` loop structure.

---

### Register Operations

The direct register instructions work as follows:

- `hlf` divides the register value by `2`
- `tpl` multiplies the register value by `3`
- `inc` adds `1`

These operations modify the targeted register in place.

Register values are stored as unsigned integers.

---

### Jump Operations

The jump instructions control program flow:

- `jmp` always jumps by the given offset
- `jie` jumps only if the register value is even
- `jio` jumps only if the register value is exactly `1`

If a conditional jump does not match, execution simply continues to the next instruction.

This is what makes the puzzle behave like a very small interpreter.

---

### Final Result

Once execution finishes, `RegisterB()` returns:

- the final value of register `b`

That value is the answer for both parts.

---

## 🛠 Implementation Notes

- Input is parsed once into structured instruction objects
- Registers are stored in a dictionary using the names `a` and `b`
- The instruction pointer is managed directly during execution
- Conditional and unconditional jumps share the same pointer-adjustment pattern
- Part 1 and Part 2 reuse the exact same execution logic
- The only difference between the two parts is the starting value of register `a`

---

## 🧪 Examples

Given the sample program:

    inc a
    jio a, +2
    tpl a
    inc a

Execution works like this:

1. `inc a` sets `a` to `1`
2. `jio a, +2` jumps because `a` is exactly `1`
3. `tpl a` is skipped
4. `inc a` sets `a` to `2`

Final result:

- `a = 2`

This shows how the conditional jump changes the instruction flow.

---

## 🚀 Key Takeaways

- Good example of building a small instruction interpreter
- Parsing and execution are separated cleanly
- Strongly typed instruction kinds keep the switch-based executor simple
- Part 2 is solved by changing only the initial register state
- Jump handling is compact and easy to follow once the pointer offset pattern is understood

---

## 🔗 References

- https://adventofcode.com/2015/day/23