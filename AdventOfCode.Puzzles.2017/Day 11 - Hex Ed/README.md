# 🎄 Advent of Code 2017 - Day 11: Hex Ed

## 📜 Puzzle Overview

This puzzle tracks movement across a hex grid.

The input is a comma-separated list of steps, where each step is one of:

- `n`
- `ne`
- `se`
- `s`
- `sw`
- `nw`

Unlike a square grid, movement on a hex grid follows six directions.

Part 1 finds the shortest distance from the starting point after all steps.  
Part 2 finds the maximum distance reached at any point during the walk.

---

## 🧩 Part 1

Determine the fewest number of steps required to reach the final position.

### 💡 Approach

- Start at the origin
- Process each step in sequence
- Update position using a hex-grid coordinate system
- After all moves:
  - compute the shortest hex distance from the origin to the final position

A common approach is to use cube coordinates or axial coordinates.

---

## 🧩 Part 2

Determine the furthest distance from the origin reached during the walk.

### 💡 Approach

- Track position as in Part 1
- After each step:
  - compute current distance from the origin
  - update the maximum distance seen so far
- Return the largest recorded distance

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Hex Ed`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Processes all hex steps
- Returns final distance

For Part 2:

- Tracks peak distance during movement

---

### Hex Grid Representation

A hex grid can be represented using:

- axial coordinates
- cube coordinates

Cube coordinates are especially useful because distance is simple to calculate.

Example cube axes:

- `x`
- `y`
- `z`

With constraint:

```
x + y + z = 0
```

---

### Movement Rules

Each direction updates coordinates in a fixed way.

Example using cube coordinates:

- `n`  → `(0, 1, -1)`
- `ne` → `(1, 0, -1)`
- `se` → `(1, -1, 0)`
- `s`  → `(0, -1, 1)`
- `sw` → `(-1, 0, 1)`
- `nw` → `(-1, 1, 0)`

Each step adds one of these offsets to the current position.

---

### Distance Calculation

For cube coordinates, hex distance from origin is:

```
distance = (abs(x) + abs(y) + abs(z)) / 2
```

This gives the minimum number of hex steps needed to return to the origin.

---

### Part 1 Logic

- Parse input into individual directions
- Apply all movements
- Compute distance from origin at the end

---

### Part 2 Logic

- Parse and apply steps one at a time
- After each move:
  - calculate current distance
  - compare against maximum seen
- Return the highest value reached

---

## 🛠 Implementation Notes

- Cube coordinates make distance calculation straightforward
- Axial coordinates also work, but distance usually needs conversion
- Input parsing is simple comma splitting
- Part 2 reuses the same movement logic as Part 1
- Performance is trivial due to small input size

---

## 🧪 Behaviour Summary

Given a list of hex-grid movements:

- Position changes step by step
- Part 1 measures final distance from the start
- Part 2 measures the furthest point reached at any time
- Both parts rely on the same coordinate system

---

## 🚀 Key Takeaways

- Hex grid movement using specialised coordinates
- Distance calculation differs from square grids
- Same traversal reused for final and peak distance
- Cube coordinates provide a clean implementation

---

## 🔗 References

- https://adventofcode.com/2017/day/11