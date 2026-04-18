# 🎄 Advent of Code 2017 - Day 21: Fractal Art

## 📜 Puzzle Overview

This puzzle builds an evolving image using enhancement rules.

The image starts as a small grid:

    .#.
    ..#
    ###

Each iteration:

- splits the current grid into smaller squares (2x2 or 3x3)
- matches each square against a set of transformation rules
- replaces each square with a larger one (3x3 or 4x4)
- recombines everything into a new grid

Rules look like this:

    ../.# => ##./#../...

or

    .#./..#/### => #..#/..../..../#..#

The solver repeatedly applies these transformations and counts how many pixels are "on" (`#`) after a given number of iterations.

---

## 🧩 Part 1

Count the number of `#` pixels after 5 iterations.

### 💡 Approach

- Parse all transformation rules
- Store every possible variation (rotations and flips) of each input pattern
- Start from the initial 3x3 grid
- Repeat 5 times:
  - split grid into sub-squares
  - transform each square using the rules
  - merge squares back into a new grid
- Count all `#` pixels in the final grid

---

## 🧩 Part 2

Repeat the same process, but for 18 iterations.

### 💡 Approach

- Reuse the exact same enhancement logic
- Run the transformation loop for 18 iterations instead of 5
- Count the number of `#` pixels at the end

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Fractal Art`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Runs the enhancement process for 5 iterations

For Part 2:

- Runs the enhancement process for 18 iterations

---

### Rule Parsing

Each rule maps:

- a small input grid → a larger output grid

Example:

    ../.# => ##./#../...

The solver:

- splits on `" => "`
- parses both sides into grid structures
- generates all rotations and flips of the input pattern
- stores them in a lookup dictionary

This ensures any orientation of a pattern can be matched during enhancement.

---

### Grid Representation

The image is represented as a grid of characters:

- `.` for off
- `#` for on

Internally, this is typically handled as:

- string arrays, or
- 2D character arrays

This makes splitting and recombining easier.

---

### Splitting the Grid

Before each iteration, the grid size determines how it is split:

- if divisible by 2 → split into 2x2 squares
- if divisible by 3 → split into 3x3 squares

For example:

A 6x6 grid becomes:

- nine 2x2 squares

Each square is processed independently.

---

### Transforming Squares

Each sub-square is:

- converted into a string pattern
- looked up in the rule dictionary
- replaced with its corresponding output grid

Because all rotations/flips were precomputed:

- lookup is direct and fast

---

### Recombining the Grid

After transforming all squares:

- the output squares are stitched back together
- row by row

Example:

If 2x2 squares become 3x3:

- a 6x6 grid becomes a 9x9 grid

---

### Iteration Loop

The main loop:

- runs for a fixed number of iterations (5 or 18)
- performs:
  - split
  - transform
  - recombine

Conceptually:

    for each iteration:
        split grid
        enhance each square
        rebuild grid

---

### Counting Active Pixels

After all iterations complete:

- iterate over the grid
- count all `#` characters

This value is returned as the puzzle answer.

---

## 🛠 Implementation Notes

- Rules must include all rotated and flipped variations
- Grid size dynamically increases every iteration
- Splitting logic depends on divisibility (2 or 3)
- String-based pattern matching simplifies rule lookup
- Performance becomes important for 18 iterations due to grid growth

---

## 🧪 Behaviour Summary

Given a starting 3x3 grid:

- rules define how small patterns expand into larger ones
- each iteration increases the grid size
- the grid is repeatedly split and transformed
- Part 1 runs 5 iterations
- Part 2 runs 18 iterations
- the final result is the number of `#` pixels in the grid

---

## 🚀 Key Takeaways

- Strong example of grid transformation and pattern matching
- Precomputing rotations/flips avoids complex matching logic later
- Problem scales quickly due to exponential grid growth
- Clean separation of parsing, transforming, and recombining steps
- Same logic supports both parts with only iteration count changed

---

## 🔗 References

- https://adventofcode.com/2017/day/21