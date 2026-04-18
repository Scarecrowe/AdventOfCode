# 🎄 Advent of Code 2017 - Day 23: Coprocessor Conflagration

## 📜 Puzzle Overview

This puzzle simulates a simple assembly-like program running on a coprocessor.

The input is a list of instructions, each operating on registers.

There are eight registers:

    a through h

All registers start at:

    0

Each instruction follows this format:

    instruction X Y

The supported instructions are:

- `set X Y` → set register X to value Y
- `sub X Y` → subtract Y from register X
- `mul X Y` → multiply register X by Y
- `jnz X Y` → jump Y steps if X is not zero

The program executes sequentially, modifying registers and jumping as needed.

---

## 🧩 Part 1

Count how many times the `mul` instruction is invoked.

### 💡 Approach

- Parse all instructions into executable operations
- Maintain a dictionary of registers (`a` to `h`)
- Track a program counter (`pc`)
- Execute instructions until `pc` exits the instruction list
- Increment a counter every time `mul` is executed
- Return the total count

---

## 🧩 Part 2

Determine the final value in register `h` after modifying the initial state.

### 💡 Approach

- Set register `a` to `1` before execution
- Observe that the program becomes extremely slow if fully simulated
- Analyse the instruction logic instead of brute forcing

The program effectively:

- iterates through a range of numbers
- checks whether each number is composite
- increments `h` for each non-prime value

So Part 2 reduces to:

- counting how many numbers in a given range are **not prime**

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Coprocessor Conflagration`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- runs the instruction interpreter
- counts `mul` executions

For Part 2:

- either runs an optimised version
- or replaces the program with equivalent high-level logic

---

### Instruction Parsing

Each line is parsed into:

- an operation (`set`, `sub`, `mul`, `jnz`)
- operands `X` and `Y`

Operands can be:

- register names
- or literal integers

A helper function typically resolves:

    value = register OR integer

---

### Register Storage

Registers are stored in:

- `Dictionary<string, long>`

All registers default to:

    0

This allows dynamic access without pre-initialising all registers.

---

### Instruction Execution

Each instruction behaves as follows:

#### `set X Y`

    X = Y

#### `sub X Y`

    X -= Y

#### `mul X Y`

    X *= Y

Also increments the Part 1 counter.

#### `jnz X Y`

    if X != 0:
        jump by Y

Otherwise:

    move to next instruction

---

### Program Counter

The program counter (`pc`):

- starts at `0`
- increments after each instruction
- may jump forward or backward with `jnz`

Execution stops when:

    pc < 0 OR pc >= instruction count

---

### Simulation Loop

Core loop structure:

    while pc is within bounds:
        execute instruction
        update pc

This is effectively a simple virtual machine.

---

### Part 1 Logic

- Count every execution of `mul`
- Return the total after the program halts

---

### Part 2 Optimisation

Running the program directly is too slow.

Instead, analysing the instructions reveals:

- a loop over a numeric range
- a nested check for factors
- a condition that increments `h` when a number is not prime

So the program is equivalent to:

    count composite numbers in a range with a fixed step

This allows replacing the interpreter with:

- a mathematical solution
- or an optimised loop

---

## 🛠 Implementation Notes

- Very similar structure to Day 18 (assembly interpreter reuse)
- Operand resolution must handle both registers and literals
- `jnz` introduces non-linear control flow
- Part 2 is intentionally designed to require reverse engineering
- Performance is critical if attempting brute force

---

## 🧪 Behaviour Summary

Given a list of assembly-like instructions:

- registers are initialised to zero
- instructions are executed sequentially
- control flow can jump using `jnz`
- Part 1 counts how often multiplication occurs
- Part 2 transforms the program into a mathematical problem
- the final result is either the `mul` count or the computed value of `h`

---

## 🚀 Key Takeaways

- Classic interpreter / virtual machine implementation
- Strong reuse from earlier puzzle (Day 18)
- Highlights inefficiency of naive simulation
- Encourages analysing low-level code into high-level logic
- Demonstrates how assembly-like programs can hide simple maths

---

## 🔗 References

- https://adventofcode.com/2017/day/23