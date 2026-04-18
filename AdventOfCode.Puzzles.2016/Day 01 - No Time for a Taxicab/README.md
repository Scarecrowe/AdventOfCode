# 🎄 Advent of Code 2016 - Day 01: No Time for a Taxicab

## 📜 Puzzle Overview

This puzzle models navigation through a city grid using a sequence of movement instructions.

Each instruction consists of:

- a turn direction:
  - `L` (left)
  - `R` (right)
- a number of steps forward

You begin:

- at position `(0, 0)`
- facing **North**

The movement follows a strict grid system (no diagonals), meaning all movement occurs along the X and Y axes.

The goal is to determine distances using **Manhattan distance** (sum of absolute X and Y coordinates).

---

## 🧩 Part 1

Determine how many blocks away the final destination is after following all instructions.

### 💡 Approach

- Parse the input into `(turn, steps)` instructions
- Maintain:
  - current position `(x, y)`
  - current direction (North, East, South, West)
- For each instruction:
  - update direction based on `L` or `R`
  - move forward by the given number of steps
- After all movements:
  - calculate Manhattan distance:

    abs(x) + abs(y)

This gives the shortest path distance back to the origin.

---

## 🧩 Part 2

Determine the distance to the **first location visited twice**.

### 💡 Approach

- Reuse the same instruction parsing
- Track every position visited using a set
- Instead of jumping directly:
  - move **one step at a time**
- After each step:
  - check if the position has already been visited
  - if so, stop immediately

Return:

    abs(x) + abs(y)

for the first repeated position

This requires finer-grained movement tracking compared to Part 1.

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `No Time for a Taxicab`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new navigation solver instance
- Calls the Part 1 method

For Part 2:

- Creates a new navigation solver instance
- Calls the Part 2 method

---

### Navigation Class

This class contains the full logic for:

- parsing instructions
- tracking direction and position
- solving both puzzle parts

It typically stores:

- current position `(x, y)`
- current direction index
- a set of visited positions (Part 2)

---

### Parsing the Input

The input is a single line like:

    R2, L3, R5, R2

Parsing involves:

- splitting on `, `
- extracting:
  - first character → turn direction
  - remaining substring → step count

Each instruction becomes:

- `(turn, steps)`

---

### Direction Handling

Directions are typically represented as:

- North
- East
- South
- West

A common approach is:

- store directions in an array
- rotate index:
  - `+1` for right
  - `-1` for left
- wrap using modulo

Each direction maps to a movement vector:

- North → `(0, +1)`
- East → `(+1, 0)`
- South → `(0, -1)`
- West → `(-1, 0)`

---

### Movement Logic

For each instruction:

- update direction
- move forward

Part 1:

- move in one step using multiplication

Part 2:

- move step-by-step
- track every intermediate coordinate

---

### Part 1 Logic

The Part 1 solver:

- processes all instructions sequentially
- updates position after each move

At the end:

- calculates Manhattan distance from `(0, 0)`

---

### Part 2 Logic

The Part 2 solver:

- tracks all visited positions in a set
- walks one step at a time

For each step:

- check if position already exists in the set
- if yes:
  - stop immediately
  - return its Manhattan distance

This ensures the **first repeated location** is found, not just the final one.

---

## 🛠 Implementation Notes

- Input is parsed into structured instructions
- Direction rotation is handled via modular arithmetic
- Manhattan distance is used for final calculations
- Part 2 requires step-level tracking instead of jumps
- A hash set enables efficient duplicate detection

---

## 🧪 Behaviour Summary

Given a sequence of instructions:

- direction changes determine movement orientation
- movement updates position on a grid

Part 1:

- follows all instructions
- computes final distance

Part 2:

- tracks every visited position
- stops at the first repeated location

---

## 🚀 Key Takeaways

- Classic **grid navigation problem**
- Introduces Manhattan distance calculation
- Demonstrates direction handling via rotation
- Highlights difference between:
  - endpoint tracking (Part 1)
  - path tracking (Part 2)
- Efficient duplicate detection via sets is key for Part 2

---

## 🔗 References

- https://adventofcode.com/2016/day/1