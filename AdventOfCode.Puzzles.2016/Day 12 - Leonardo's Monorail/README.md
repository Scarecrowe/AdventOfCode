# 🎄 Advent of Code 2016 - Day 12: Leonardo's Monorail

## 📜 Puzzle Overview

This puzzle implements a simple assembly-like interpreter.

You are given a list of instructions that operate on registers:

- `a`
- `b`
- `c`
- `d`

All registers start at `0` (with a variation in Part 2).

The goal is to execute the instruction set and determine the final value in register `a`.

---

## 🧩 Part 1

Determine the value in register `a` after executing all instructions.

### 💡 Approach

- Parse each line into an instruction
- Maintain:
  - a dictionary or array of registers
  - an instruction pointer
- Execute instructions sequentially
- Continue until the instruction pointer exits the program

---

## 🧩 Part 2

Run the same program, but initialise register `c` to `1`.

### 💡 Approach

- Use the same interpreter as Part 1
- Before execution:
  - set register `c = 1`
- Execute program normally
- Return final value of register `a`

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Leonardo's Monorail`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Executes program with default register values

For Part 2:

- Sets register `c` before execution
- Re-runs the same program

---

### Instruction Set

The program supports four instructions:

#### Copy

```
cpy x y
```

- Copies value `x` into register `y`
- `x` can be:
  - an integer
  - another register

---

#### Increment

```
inc x
```

- Increases register `x` by 1

---

#### Decrement

```
dec x
```

- Decreases register `x` by 1

---

#### Jump

```
jnz x y
```

- If `x` is not zero:
  - jump `y` steps (relative)
- Otherwise:
  - continue to next instruction

---

### Execution Loop

- Maintain an instruction pointer (`ip`)
- For each step:
  - execute instruction at `ip`
  - update registers as needed
  - adjust `ip` accordingly
- Stop when `ip` moves outside instruction range

---

### Value Resolution

For instructions like `cpy` and `jnz`:

- Determine if a value is:
  - a literal integer
  - a register reference
- Resolve to an integer before executing

---

### Part 1 Logic

- Initialise all registers to `0`
- Execute instructions
- Return value of register `a`

---

### Part 2 Logic

- Initialise registers:
  - `a = 0`
  - `b = 0`
  - `c = 1`
  - `d = 0`
- Execute instructions
- Return value of register `a`

---

## 🛠 Implementation Notes

- Instruction parsing can be string-based or precompiled into structures
- Register storage can use:
  - dictionary
  - array indexed by character
- Jump instructions modify control flow directly
- Performance is acceptable due to small program size

---

## 🧪 Behaviour Summary

Given a list of instructions:

- Registers are modified step-by-step
- Control flow is affected by conditional jumps
- Part 1 runs with default state
- Part 2 modifies initial conditions
- Final result is taken from register `a`

---

## 🚀 Key Takeaways

- Simple interpreter design
- Instruction parsing and execution loop
- Dynamic control flow via jumps
- Same program, different initial state

---

## 🔗 References

- https://adventofcode.com/2016/day/12