# 🎄 Advent of Code 2015 - Day 01: Not Quite Lisp

## 📜 Puzzle Overview

Santa is trying to navigate an apartment building using a set of instructions.

- `(` means go **up one floor**
- `)` means go **down one floor**

He starts on floor `0`.

---

## 🧩 Part 1

Determine **which floor Santa ends up on** after following all instructions.

### 💡 Approach

This is essentially a running total:

- Increment for `(`
- Decrement for `)`

Final result = total ups - total downs

---

## 🧩 Part 2

Find the **position of the first character** that causes Santa to enter the basement (`floor -1`).

- Positions are **1-based**

### 💡 Approach

Iterate through the input:

1. Track current floor
2. As soon as floor < 0 → return position

---

## 🧠 Code Breakdown

### Floor Tracking

The core logic maintains a running floor total:

- `(` increments the floor
- `)` decrements the floor

This is done in a single pass over the input string.

### Part 1 Logic

- Process the entire input
- Return the final floor value after all instructions

### Part 2 Logic

- Iterate character by character
- Update the floor each step
- As soon as the floor drops below `0`, return the current position (1-based)

### Early Exit

Part 2 avoids unnecessary work by exiting immediately once the basement is reached.

---

## 🛠 Implementation Notes

- Input is treated as a simple character stream
- No complex parsing required
- Uses a single-pass approach for efficiency

---

## 🧪 Examples

| Input    | Result |
|----------|--------|
| `(())`   | 0      |
| `(((`    | 3      |
| `())`    | -1     |
| `)()`    | -1     |

---

## 🚀 Key Takeaways

- Great warm-up problem
- Introduces:
  - Iteration
  - State tracking
  - Early exit conditions (Part 2)

---

## 🔗 References

- https://adventofcode.com/2015/day/1