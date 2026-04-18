# 🎄 Advent of Code 2018 - Day 11: Chronal Charge

## 📜 Puzzle Overview

This puzzle involves working with a **300x300 grid** where each cell has a computed power level.

Each fuel cell at coordinate `(x, y)` has a value derived from a formula using:

- its position
- a grid serial number (input)

The goal is to find the square region with the highest total power.

---

## 🧩 Part 1

Find the **3x3 square** with the largest total power and return its top-left coordinate.

### 💡 Approach

- Generate power levels for all 300×300 cells
- Evaluate every possible 3×3 square
- Sum all values in each square
- Track the maximum total found
- Return the top-left coordinate of that square

---

## 🧩 Part 2

Find the square (any size from 1×1 to 300×300) with the largest total power.

### 💡 Approach

- Reuse the precomputed grid
- Try every square size from 1 to 300
- For each size:
  - slide the square across the grid
  - compute total power
- Track the best (x, y, size) combination

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Chronal Charge`
- Reads the grid serial number input
- Executes both parts using the solver

For Part 1:

- Calls solver with fixed square size of `3`

For Part 2:

- Calls solver with variable square sizes up to `300`

---

### Grid Power Calculation

Each cell power level is calculated using the standard AoC formula:

- Compute rack ID from `x + 10`
- Multiply rack ID by `y`
- Add serial number
- Multiply again by rack ID
- Extract hundreds digit
- Subtract 5

This produces positive and negative values across the grid.

---

### Grid Construction

The implementation builds a **300×300 integer grid**:

- Each coordinate is precomputed once
- Stored for fast reuse during square summation

This avoids recalculating power levels repeatedly.

---

### Square Summation Logic

For any square size `s`:

- Iterate all valid top-left positions `(x, y)`
- Sum all values in the `s × s` region
- Track the highest total

Conceptually:

    total = sum(grid[x..x+s-1, y..y+s-1])

This is repeated for every position and size.

---

### Part 1 Solver

For fixed size `3`:

- Slide a 3×3 window over the grid
- Compute each sum
- Track maximum result

Return:

    x,y

---

### Part 2 Solver

For variable sizes:

- Loop `size` from 1 → 300
- For each size:
  - scan full grid
  - compute all square totals
- Track best result:

    x,y,size

Return the best overall coordinate-size combination.

---

### Solver Structure

The core solver method returns:

- best X coordinate
- best Y coordinate
- best square size

It is shared between both parts:

- Part 1 fixes `size = 3`
- Part 2 explores all sizes

---

### Optimisation Notes

The implementation is intentionally brute-force but structured:

- Grid is precomputed once
- No repeated power formula calculations
- Reuses same summation logic for both parts

This keeps the code simple while remaining performant enough for 300×300 constraints.

---

## 🛠 Implementation Notes

- Input is a single integer (grid serial number)
- Grid is always 300×300
- Part 1 uses fixed-size scanning (3×3)
- Part 2 extends to all square sizes
- Same solver function is reused for both parts
- Output includes coordinates (and size for Part 2)

---

## 🧪 Behaviour Summary

Given a grid serial number:

- Compute a full 300×300 power grid
- Evaluate all possible square regions
- Part 1 returns best 3×3 square
- Part 2 returns best square of any size
- Both rely on exhaustive search over the grid

---

## 🚀 Key Takeaways

- Classic grid-search optimisation problem
- Demonstrates brute-force scanning over a fixed domain
- Shows how precomputation simplifies repeated queries
- Same core solver reused for both fixed and variable window sizes
- Good candidate for prefix-sum optimisation (summed-area table)
- Highlights tradeoff between simplicity and performance

---

## 🔗 References

- https://adventofcode.com/2018/day/11