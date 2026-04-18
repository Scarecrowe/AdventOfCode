# 🎄 Advent of Code 2025 - Day 04: Printing Department

## 📜 Puzzle Overview

This puzzle works with a grid-based map made up of printable cells and roll markers.

The solver looks for cells marked with:

- `@`

A roll is considered accessible when it does **not** have four neighbouring `@` cells in the intercardinal directions provided by the map helper.

Part 1 counts how many rolls are currently accessible.

Part 2 repeatedly removes all currently accessible rolls, then counts how many rolls were removed in total once the process is complete.

---

## 🧩 Part 1

Determine how many rolls are immediately accessible in the current map.

### 💡 Approach

- Load the input into a grid structure
- Scan every cell in the map
- Ignore any cell that is not `@`
- Check the neighbouring intercardinal cells
- Count the roll if fewer than four neighbouring cells are also `@`

---

## 🧩 Part 2

Determine how many rolls can be removed in total if accessible rolls are removed in waves until none remain.

### 💡 Approach

- Reuse the same grid
- Find every accessible roll in the current state
- Remove all of them at once
- Repeat until no accessible rolls remain
- Sum the number of removed rolls across all rounds

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Printing Department`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PrintingDepartment(this.Input)`
- Calls `AccessableRolls()`

For Part 2:

- Creates `new PrintingDepartment(this.Input)`
- Calls `AllAccessableRolls()`

The two parts share the same solver and differ only in whether the grid is evaluated once or repeatedly reduced.

---

### `PrintingDepartment.cs`

This class contains the full grid-processing logic.

It stores:

- `Map`

The map is created as a `VectorArray` from the raw input using:

    new(input, (c) => c)

This means every input character is stored directly in the grid without conversion.

The main methods are:

- `AccessableRolls()`
- `AllAccessableRolls()`

---

### Map Scanning

Both methods iterate across the grid using:

    this.Map.AxisEnumerator()

This gives access to:

- the current cell position
- the current cell value

Only cells containing `@` are considered for further checks.

All other cells are skipped immediately.

---

### Neighbour Checking

Accessibility is determined using:

    this.Map.AdjacentInterCardinal(cell.Point)

The solver then counts how many of those neighbouring cells also contain `@`.

A roll is considered accessible when:

- the neighbour count is less than `4`

At a high level, the rule behaves like this:

- fully surrounded rolls are not accessible
- anything with at least one missing `@` neighbour is accessible

---

### Part 1 Logic

`AccessableRolls()` performs a single scan of the map.

For each `@` cell:

- count neighbouring `@` cells
- if the count is less than `4`, increment the result

The method returns the number of rolls that are accessible in the original grid state.

---

### Part 2 Logic

`AllAccessableRolls()` repeatedly removes accessible rolls until no more remain.

For each round:

- clear the working list of roll positions
- scan the full map
- collect every `@` position with fewer than `4` neighbouring `@` cells
- stop if no positions were collected
- change each collected position to `.`

After each round:

- add the number of removed rolls to the running result

This continues until the grid no longer contains any accessible rolls.

---

### Wave-Based Removal

Part 2 removes rolls in batches rather than one at a time.

That means all currently accessible rolls are collected first, then removed together.

This is important because:

- removing one roll early in the loop could otherwise affect whether another roll appears accessible in the same pass

By removing them in waves, each round is based entirely on the grid state at the start of that round.

---

## 🛠 Implementation Notes

- Input is stored directly as a character grid
- Only `@` cells are treated as rolls
- Accessibility is based on the count of neighbouring `@` cells from `AdjacentInterCardinal`
- Part 1 performs a single evaluation pass
- Part 2 repeatedly removes accessible rolls until the map stabilises
- Batch removal keeps each round consistent

---

## 🧪 Behaviour Summary

Given a map of roll markers:

- any `@` cell with fewer than four neighbouring `@` cells is accessible
- Part 1 counts those rolls once
- Part 2 removes them and repeats the process
- the final answer for Part 2 is the total number removed across all waves

This makes Part 2 behave like a layered peeling process over the grid.

---

## 🚀 Key Takeaways

- Good example of solving two related parts with one shared grid model
- Part 1 is a direct accessibility count
- Part 2 builds on the same rule by repeatedly stripping accessible layers
- Batch collection before removal keeps the simulation predictable

---

## 🔗 References

- https://adventofcode.com/2025/day/4