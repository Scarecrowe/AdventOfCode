# 🎄 Advent of Code 2017 - Day 10: Knot Hash

## 📜 Puzzle Overview

This puzzle implements a custom hashing algorithm.

You begin with a circular list of numbers:

```
0 to 255
```

The algorithm processes a sequence of lengths to repeatedly reverse sections of the list.

Part 1 performs a single round of transformations.  
Part 2 expands this into a full hashing algorithm producing a hexadecimal string.

---

## 🧩 Part 1

Determine the result of multiplying the first two numbers after processing the input.

### 💡 Approach

- Initialise a list from `0` to `255`
- Maintain:
  - current position
  - skip size (starting at 0)
- For each length:
  - reverse that many elements starting from current position
  - move current position forward by:

```
length + skip size
```

  - increase skip size by 1
- After processing all lengths:
  - multiply the first two numbers in the list

---

## 🧩 Part 2

Compute the full Knot Hash as a hexadecimal string.

### 💡 Approach

- Convert input string into ASCII values
- Append the suffix:

```
17, 31, 73, 47, 23
```

- Perform **64 rounds** of the Part 1 algorithm
  - do not reset position or skip size between rounds

---

### Dense Hash

- After 64 rounds, the list contains 256 numbers
- Reduce into 16 blocks of 16 numbers each
- For each block:
  - compute XOR of all values:

```
blockHash = a ^ b ^ c ^ ...
```

- This produces 16 numbers

---

### Hex Conversion

- Convert each block value to a two-digit hexadecimal string
- Concatenate all values to produce final hash

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Knot Hash`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Performs single round transformation
- Returns product of first two numbers

For Part 2:

- Performs full hash algorithm
- Returns hexadecimal string

---

### Circular List Handling

- The list is treated as circular
- Indexing wraps using modulo:

```
index = (index + offset) % length
```

---

### Segment Reversal

- Reverse a section of the list starting at current position
- Wrap around if segment exceeds list bounds

---

### Position and Skip Size

- After each operation:

```
currentPosition += length + skipSize
skipSize += 1
```

---

### Part 1 Logic

- Parse input as comma-separated integers
- Perform one round of operations
- Multiply first two values

---

### Part 2 Logic

- Convert input to ASCII values
- Add fixed suffix
- Perform 64 rounds
- Compute dense hash
- Convert to hexadecimal string

---

## 🛠 Implementation Notes

- Efficient circular indexing is important
- Reversal logic must handle wrapping correctly
- XOR reduction simplifies dense hash calculation
- String formatting required for hexadecimal output
- Same core algorithm reused across both parts

---

## 🧪 Behaviour Summary

Given an input sequence:

- A circular list is transformed through reversals
- Part 1 performs a single transformation pass
- Part 2 builds a full hash from repeated transformations
- Output is either numeric result or hexadecimal string

---

## 🚀 Key Takeaways

- Circular list manipulation
- Stateful iteration with position and skip size
- XOR reduction for hash compression
- Multi-stage transformation pipeline

---

## 🔗 References

- https://adventofcode.com/2017/day/10