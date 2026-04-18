# 🎄 Advent of Code 2018 - Day 19: Go With The Flow

## 📜 Puzzle Overview

This puzzle extends the device-style assembly language from earlier days by introducing **flow control via a bound instruction pointer**.

The input consists of:

1. A directive binding the instruction pointer to a register
2. A list of assembly-like instructions

Example input:

    #ip 0
    seti 5 0 1
    seti 6 0 2
    addi 0 1 0
    addr 1 2 3
    setr 1 0 0
    seti 8 0 4
    seti 9 0 5

The key concept:

- One register acts as the instruction pointer (`ip`)
- Before each instruction, `ip` is written into that register
- After execution, the register value is written back to `ip`
- Then `ip` is incremented

This effectively turns register operations into jump-like flow control.

---

## 🧩 Part 1

Simulate the program and determine the value in register `0` when it halts.

### 💡 Approach

- Parse the first line to determine which register is bound to the instruction pointer
- Parse the remaining lines into instruction objects
- Initialise all registers to `0`
- Execute instructions in a loop:
  - Write `ip` into the bound register
  - Execute the instruction
  - Read back updated `ip`
  - Increment `ip`
- Stop when `ip` moves outside the instruction range
- Return register `0`

---

## 🧩 Part 2

Determine the final value in register `0` for a modified initial condition.

### 💡 Approach

- Same execution model as Part 1
- However, naive simulation becomes too slow
- Recognise that the program is effectively computing:
  - a large number derivation based on divisor summation logic
- Optimise by reversing the intent of the instruction loop
- Replace full simulation with direct computation of the result

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets title to `Go With The Flow`
- Loads input
- Extracts:
  - instruction pointer binding (`#ip X`)
  - instruction list

For Part 1:

- Runs full instruction simulation
- Returns register `0`

For Part 2:

- Skips full VM execution
- Uses optimised mathematical shortcut derived from program behaviour

---

### Instruction Pointer Binding

First line example:

    #ip 0

This means:

- register `0` is used as the instruction pointer
- before each instruction:
  - `register[0] = ip`
- after execution:
  - `ip = register[0]`

Then:

    ip++

So execution flow is tightly coupled to register state.

---

### Instruction Model

Each instruction has:

- opcode name (e.g. `seti`, `addi`, `mulr`)
- inputs `A`, `B`
- output register `C`

Example:

    addi 0 1 0

Meaning:

- take register 0
- add 1
- store result in register 0

---

### Execution Loop

The core VM loop behaves like:

- while `ip` is valid:
  - write `ip` into bound register
  - decode instruction at `ip`
  - execute opcode logic
  - read updated `ip` from bound register
  - increment `ip`

This creates indirect jumps and loops without explicit jump instructions.

---

### Opcode Behaviour

The instruction set includes operations like:

- `addr`, `addi` → addition
- `mulr`, `muli` → multiplication
- `setr`, `seti` → assignment
- `gt*`, `eq*` → comparisons

Each operation writes into a target register `C`.

The VM is purely register-based with no stack or memory model.

---

## 🧮 Key Insight (Part 2 Optimisation)

Part 2 is intentionally not meant to be brute-forced.

The program effectively:

- sets up a large value in a register
- computes the **sum of all divisors** of that value

So instead of simulating instructions:

- compute the target number
- sum all integers that divide it evenly

This transforms a long-running VM into a direct mathematical solution.

---

## 🛠 Implementation Notes

- Instruction pointer is not separate state; it is stored in a register
- Execution order is affected by modifying the IP register mid-instruction
- Register mutation is central to control flow
- Part 1 is a full interpreter
- Part 2 is a reverse-engineered optimisation
- Most complexity comes from understanding control flow, not opcode logic

---

## 🧪 Behaviour Summary

Given an assembly-like program:

- the instruction pointer is bound to a register
- each instruction modifies registers and indirectly modifies control flow
- execution halts when `ip` goes out of bounds
- Part 1 returns register `0` after full simulation
- Part 2 computes the same result via mathematical interpretation instead of execution

---

## 🚀 Key Takeaways

- Registers double as control flow state via instruction pointer binding
- Program behaves like a minimal virtual machine
- Flow control emerges from arithmetic, not explicit jumps
- Part 2 requires reverse engineering the program’s intent
- Classic example of turning assembly into number theory
- Demonstrates when simulation should be replaced with direct computation

---

## 🔗 References

- https://adventofcode.com/2018/day/19