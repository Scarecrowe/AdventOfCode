# 🎄 Advent of Code 2016 - Day 18: Like a Rogue

## 📜 Puzzle Overview

This puzzle simulates rows of tiles consisting of:

- safe tiles (`.`)
- traps (`^`)

You are given an initial row, and each subsequent row is generated based on the previous one.

A tile’s state depends on three tiles above it:

- left
- center
- right

Tiles outside the bounds are considered **safe**.

The goal is to count how many tiles are safe after generating a number of rows.

Part 1 uses 40 rows, while Part 2 scales up significantly.

---

## 🧩 Part 1

Determine how many safe tiles appear after generating 40 rows.

### 💡 Approach

- Start with the initial row
- For each new row:
  - determine each tile based on the previous row
- Count all safe tiles across all rows

A tile is a **trap** if one of the following is true:

```
^^.
.^^
^..
..^
```

Otherwise, the tile is safe.

---

## 🧩 Part 2

Determine how many safe tiles appear after generating 400,000 rows.

### 💡 Approach

- Use the same generation logic as Part 1
- Avoid storing all rows:
  - only keep the current row
- Track safe tile count incrementally
- Optimise for performance due to large number of rows

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Like a Rogue`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Generates rows up to 40
- Counts safe tiles

For Part 2:

- Generates rows up to 400,000
- Uses optimised counting

---

### Row Generation

Each new row is built by examining triplets:

```
(left, center, right)
```

Rules:

- A tile is a trap if it matches one of the four patterns:

```
^^.
.^^
^..
..^
```

- Otherwise, it is safe

---

### Handling Edges

- Leftmost tile:
  - left is treated as safe
- Rightmost tile:
  - right is treated as safe

This ensures all tiles can be evaluated consistently.

---

### Efficient Representation

- Represent rows as:
  - strings
  - boolean arrays
- For performance:
  - reuse buffers
  - avoid unnecessary allocations

---

### Part 1 Logic

- Generate each row sequentially
- Count safe tiles (`.`) per row
- Accumulate total

---

### Part 2 Logic

- Same as Part 1 but with large iteration count
- Only store current row
- Update safe count as rows are generated

---

## 🛠 Implementation Notes

- Only previous row is needed at any time
- Memory usage can be kept minimal
- Bitwise optimisation can significantly improve speed
- Counting safe tiles is more efficient than tracking traps
- Large iteration count requires efficient loops

---

## 🧪 Behaviour Summary

Given an initial row:

- Each new row is derived from the previous one
- Tile states depend only on neighbours
- Safe tiles are counted across all rows
- Part 1 uses small iteration count
- Part 2 scales to very large input

---

## 🚀 Key Takeaways

- Cellular automaton-style simulation
- Local rules produce global patterns
- Efficient state reuse is key
- Bitwise optimisation simplifies logic
- Problem scales dramatically with input size

---

## 🔗 References

- https://adventofcode.com/2016/day/18