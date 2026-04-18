# 🎄 Advent of Code 2018 - Day 21: Chronal Conversion

## 📜 Puzzle Overview

This puzzle involves executing a small assembly-like program (ElfCode) with a set of registers and instructions.

The input is:

- an instruction pointer binding
- a list of instructions

Example format:

    #ip 2
    seti 123 0 5
    bani 5 456 5
    eqri 5 72 5
    addr 5 2 2
    ...

The program behaves like a virtual machine with 6 registers:

    r0, r1, r2, r3, r4, r5

One register is bound as the instruction pointer.

The goal is not just to run the program, but to analyse its behaviour at a specific point.

---

## 🧩 Part 1

Determine the first value that causes the program to halt immediately.

### 💡 Approach

- Parse the instruction pointer binding
- Parse all instructions into an opcode model
- Simulate execution step-by-step
- Monitor execution when the instruction pointer reaches a critical check (early comparison point)
- The first value compared against register 0 at the halt condition is the answer

In practice:

- the program is not run to completion naively
- execution is intercepted at a specific instruction
- the first observed comparison value is returned

---

## 🧩 Part 2

Determine the last unique value before the program starts repeating itself.

### 💡 Approach

- Run the same simulation as Part 1
- Track every value observed at the key comparison step
- Store seen values in a set
- Detect repetition:
  - when a value repeats, a cycle has formed
- Return the last unique value before repetition begins

This avoids infinite execution by identifying loop behaviour in the register state.

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets title to `Chronal Conversion`
- Loads instruction input
- Executes ElfCode simulation

For Part 1:

- runs until first halt comparison
- returns first observed register value

For Part 2:

- tracks all observed values
- returns last unique value before repetition

---

### ElfCode Execution Model

The VM operates with:

- 6 integer registers
- an instruction pointer bound to a register
- a set of opcode operations (from Day 19 / 16 style VM)

Each cycle:

1. write instruction pointer into bound register
2. execute instruction
3. update instruction pointer from bound register
4. increment instruction pointer

---

### Key Insight (Critical for Efficiency)

The program behaves in a way where:

- only a specific instruction (early in execution) matters
- the rest of the program builds a long-running comparison loop

Instead of full simulation:

- execution is intercepted at the comparison step
- register 5 (or equivalent depending on binding) drives behaviour
- values are generated in a deterministic sequence

---

### Value Tracking Logic

During execution at the key check:

- capture the value being compared to register 0
- store it in a set
- if a value repeats:
  - a cycle has been found
  - stop simulation

This transforms the problem into:

- cycle detection in a generated sequence

---

### Cycle Detection Strategy

Part 2 relies on detecting repetition:

- maintain:
  - `HashSet<long> seen`
  - `lastUniqueValue`

Each time a value appears:

- if not seen:
  - store it
  - update last unique value
- if already seen:
  - break and return lastUniqueValue

---

## 🛠 Implementation Notes

- Full VM execution is based on ElfCode instruction set
- Instruction pointer is explicitly bound to a register
- Execution is intercepted early for performance
- Part 1 captures first meaningful comparison
- Part 2 detects repeating sequence of values
- This puzzle is effectively a cycle detection problem disguised as VM execution

---

## 🧪 Behaviour Summary

Given a list of ElfCode instructions:

- the solver simulates a register-based virtual machine
- execution is controlled via instruction pointer binding
- at a specific instruction, a key register value is produced
- Part 1 returns the first observed halting comparison value
- Part 2 tracks values until repetition and returns last unique value
- the program effectively generates a deterministic sequence with a loop

---

## 🚀 Key Takeaways

- Classic reverse-engineering of a virtual machine
- Heavy reliance on cycle detection instead of full execution
- Instruction pointer binding is critical to behaviour
- Only a small portion of execution is actually relevant
- Part 2 transforms execution trace into a sequence analysis problem
- Demonstrates when “simulate everything” is the wrong approach

---

## 🔗 References

- https://adventofcode.com/2018/day/21