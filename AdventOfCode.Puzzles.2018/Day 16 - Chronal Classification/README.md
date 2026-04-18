# 🎄 Advent of Code 2018 - Day 16: Chronal Classification

## 📜 Puzzle Overview

This puzzle is about reverse-engineering a CPU instruction set.

You are given two inputs:

1. **Samples** showing how instructions behave  
   - A register state *before*
   - An instruction
   - A register state *after*

2. A **program** made of raw instructions to execute once opcode mappings are known

The device has:

- 4 registers (`0`–`3`)
- 16 unknown opcodes
- Instructions of the form:

    opcode A B C

Where `C` is always the output register.

Example sample:

    Before: [3, 2, 1, 1]
    9 2 1 2
    After:  [3, 2, 2, 1]

The goal is to determine how many samples behave like 3 or more possible opcodes, then deduce the full opcode mapping.

---

## 🧩 Part 1

Count how many samples behave like **three or more opcodes**.

### 💡 Approach

- Parse the input into:
  - sample blocks (Before / instruction / After)
  - program instructions (second section, ignored in Part 1)
- For each sample:
  - simulate all 16 possible opcode behaviours
  - count how many produce the observed "After" state
- If 3 or more opcodes match:
  - increment result counter
- Return total count

---

## 🧩 Part 2

Determine the actual opcode mapping and execute the program.

### 💡 Approach

- Start with all 16 opcodes as possible matches for each opcode number
- For each sample:
  - test all opcode implementations
  - eliminate invalid mappings
- Repeatedly reduce possibilities until each opcode number maps to exactly one operation

Once mapping is known:

- execute the program instructions sequentially
- update registers using resolved opcode functions
- return value in register `0`

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets title to `Chronal Classification`
- Loads input
- Splits input into:
  - samples section
  - program section

For Part 1:

- calls analysis over samples
- counts ambiguous opcode matches (≥3 behaviors)

For Part 2:

- builds opcode mapping
- executes program using resolved instructions

---

### Input Structure

The input has two distinct parts:

#### 1. Samples

Each sample consists of:

- Before state
- Instruction line
- After state

Example:

    Before: [3, 2, 1, 1]
    9 2 1 2
    After:  [3, 2, 2, 1]

These are used to deduce opcode behavior.

---

#### 2. Program

After samples, raw instructions appear:

    9 2 1 2
    3 0 0 1
    ...

These are executed only after opcode mapping is known.

---

### Opcode Simulation Engine

The solver implements 16 operations, each taking:

- registers
- A, B inputs
- output register C

Each operation follows rules like:

- register vs immediate mode
- arithmetic (add, multiply)
- bitwise AND/OR
- comparisons
- assignment

Each opcode is tested against samples during deduction.

---

### Part 1 Logic (Sample Analysis)

For each sample:

- clone register state
- run all 16 opcode functions
- compare result to expected "After"

If match count ≥ 3:

    increment result

This identifies ambiguous instruction samples.

---

### Opcode Deduction (Part 2 Setup)

The solver builds a mapping structure:

- opcode number → possible operations

Initially:

- each opcode number can map to any of the 16 functions

Then for each sample:

- test instruction against all operations
- remove operations that do not match

Over time:

- constraints narrow possibilities
- eventually each opcode maps to exactly one operation

This is effectively constraint elimination.

---

### Program Execution

Once mapping is resolved:

- initialise registers to `[0, 0, 0, 0]`
- iterate program instructions:

    opcode A B C

- resolve opcode number → operation
- execute operation
- write result to register `C`

Final answer:

    register[0]

---

## 🛠 Implementation Notes

- Core challenge is constraint resolution, not execution
- Each opcode is a pure function over registers
- Samples act as training data for mapping inference
- Mapping reduction is iterative elimination
- Program execution is straightforward once mapping is known
- Register state is always mutable and passed by reference

---

## 🧪 Behaviour Summary

Given:

- sample executions of unknown CPU instructions
- a program written in opcode numbers

The solver:

- identifies ambiguous opcode behaviours (Part 1)
- deduces full opcode mapping via elimination (Part 2)
- executes final program using resolved instruction set
- returns final register state

---

## 🚀 Key Takeaways

- Classic “reverse engineering a virtual machine” puzzle
- Part 1 is classification (how many opcodes fit a sample)
- Part 2 is constraint solving (deduce full opcode mapping)
- Strong use of functional opcode simulation
- Demonstrates elimination-based deduction similar to Sudoku logic
- Clean separation between analysis phase and execution phase

---

## 🔗 References

- https://adventofcode.com/2018/day/16