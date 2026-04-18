# 🎄 Advent of Code 2017 - Day 08: I Heard You Like Registers

## 📜 Puzzle Overview

This puzzle simulates a set of registers modified by conditional instructions.

Each instruction consists of:

- a target register to modify
- an operation (`inc` or `dec`)
- a value
- a condition that must be true for the operation to occur

Registers are created on demand and default to `0`.

Part 1 finds the largest value in any register after execution.  
Part 2 tracks the highest value ever held during execution.

---

## 🧩 Part 1

Determine the largest value in any register after all instructions have been processed.

### 💡 Approach

- Parse each instruction into:
  - target register
  - operation and value
  - condition (register, operator, comparison value)
- Maintain a dictionary of registers
- For each instruction:
  - evaluate the condition
  - if true:
    - apply the operation to the target register
- After all instructions:
  - return the maximum value in any register

---

## 🧩 Part 2

Determine the highest value held in any register during execution.

### 💡 Approach

- Use the same execution process as Part 1
- Track the maximum value seen at any point:
  - after each modification
- Return the highest value observed during execution

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `I Heard You Like Registers`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Executes all instructions
- Returns final maximum register value

For Part 2:

- Tracks peak value during execution

---

### Instruction Format

Each line follows this structure:

```
b inc 5 if a > 1
```

Meaning:

- increase `b` by 5
- only if `a > 1`

---

### Register Handling

- Registers are stored in a dictionary
- Any missing register defaults to `0`
- Values are updated dynamically

---

### Condition Evaluation

Conditions use standard comparison operators:

```
>
<
>=
<=
==
!=
```

Each condition compares:

- a register value
- against a constant

---

### Operation Execution

If condition is true:

- `inc` increases value
- `dec` decreases value

Example:

```
b inc 5 → b += 5
b dec 3 → b -= 3
```

---

### Part 1 Logic

- Execute all instructions
- Track register values
- Return maximum value at end

---

### Part 2 Logic

- Execute instructions as normal
- After each update:
  - check current maximum
- Track highest value seen during execution

---

## 🛠 Implementation Notes

- Dictionary lookup should handle missing keys
- Parsing instructions into structured objects simplifies execution
- Condition evaluation can be implemented using switch or function mapping
- Part 2 requires tracking intermediate state
- Input size is small, so performance is straightforward

---

## 🧪 Behaviour Summary

Given a list of instructions:

- Registers are modified conditionally
- Values change over time
- Part 1 returns final maximum value
- Part 2 returns highest value seen during execution

---

## 🚀 Key Takeaways

- Conditional execution pattern
- Dynamic register creation
- State tracking over time
- Same execution reused with different metrics

---

## 🔗 References

- https://adventofcode.com/2017/day/8