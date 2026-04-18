# 🎄 Advent of Code 2016 - Day 23: Safe Cracking

## 📜 Puzzle Overview

This puzzle extends the assembunny interpreter from earlier challenges.

You are given a program written in a small assembly-like language operating on registers:

- `a`
- `b`
- `c`
- `d`

Part 1 introduces a new instruction that allows the program to modify itself during execution.  
Part 2 requires recognising patterns in the program to optimise performance.

---

## 🧩 Part 1

Determine the final value in register `a` after executing the program.

### 💡 Approach

- Reuse the interpreter from Day 12
- Add support for a new instruction:

```
tgl x
```

- Execute instructions sequentially until completion
- Return the final value of register `a`

---

## 🧩 Part 2

Run the same program with a different initial value and determine the result.

### 💡 Approach

- Initialise register `a` with a larger value
- Execute the same program
- Due to performance issues:
  - analyse the instruction patterns
  - replace repeated loops with equivalent optimised operations

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Safe Cracking`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Executes the assembunny program with toggle support

For Part 2:

- Applies optimisation or pattern recognition
- Computes result efficiently

---

### Instruction Set

Includes previous instructions:

```
cpy x y
inc x
dec x
jnz x y
```

Plus the new instruction:

```
tgl x
```

---

### Toggle Instruction

```
tgl x
```

- Finds the instruction at position:

```
currentIndex + value(x)
```

- Modifies that instruction:

Rules:

- One-argument instructions:
  - `inc` → `dec`
  - others → `inc`
- Two-argument instructions:
  - `jnz` → `cpy`
  - others → `jnz`

- If target is out of bounds:
  - do nothing
- If resulting instruction is invalid:
  - skip it during execution

The program becomes **self-modifying**.

---

### Execution Model

- Maintain:
  - registers
  - instruction pointer
  - instruction list (mutable)
- Process instructions sequentially
- Apply changes dynamically when `tgl` is executed

---

### Part 1 Logic

- Initialise registers (typically `a = 7`)
- Execute full program
- Return final value in register `a`

---

### Part 2 Logic

- Initialise with larger value (typically `a = 12`)
- Direct execution becomes very slow
- Identify patterns in instructions:
  - repeated addition loops
  - nested loops representing multiplication
- Replace loops with equivalent operations

Example pattern:

```
inc a
dec b
jnz b -2
```

Equivalent to:

```
a += b
b = 0
```

Nested loops simulate multiplication.

---

### Optimisation Strategy

- Detect instruction patterns during runtime or preprocess
- Replace with:
  - addition
  - multiplication
- Skip large loop executions

This dramatically improves performance.

---

## 🛠 Implementation Notes

- Instruction list must be mutable due to `tgl`
- Invalid instructions should be safely ignored
- Pattern recognition is key for Part 2
- Pure interpretation is too slow for large inputs
- Hybrid approach (interpreter + optimisation) works best

---

## 🧪 Behaviour Summary

Given an assembunny program:

- Instructions modify registers and control flow
- `tgl` dynamically alters program behaviour
- Part 1 executes normally
- Part 2 requires optimisation due to heavy looping

---

## 🚀 Key Takeaways

- Self-modifying code simulation
- Interpreter design with mutable instruction set
- Pattern recognition replaces brute-force execution
- Real-world analogy to compiler optimisation

---

## 🔗 References

- https://adventofcode.com/2016/day/23