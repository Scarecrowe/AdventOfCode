# 🎄 Advent of Code 2019 - Day 03: Crossed Wires

## 📜 Puzzle Overview

This puzzle works with two wires laid out on a grid.

Each wire is defined by a sequence of movement instructions such as:

    R8,U5,L5,D3

Each instruction consists of:

- a direction (`R`, `L`, `U`, `D`)
- a distance (number of steps)

The wires both start at the origin `(0,0)` and trace paths across the grid.

The challenge is to find where the wires cross and calculate specific metrics based on those intersections.

---

## 🧩 Part 1

Find the intersection point closest to the origin using Manhattan distance.

### 💡 Approach

- Parse both wire instruction sets
- Walk each wire step-by-step across the grid
- Record every coordinate visited by the first wire
- Walk the second wire and detect intersections with the first
- Ignore the origin `(0,0)`
- Calculate Manhattan distance for each intersection:

      distance = |x| + |y|

- Return the smallest distance

---

## 🧩 Part 2

Find the intersection with the fewest combined steps taken by both wires.

### 💡 Approach

- Track not just positions, but the number of steps taken to reach each position
- For the first wire:
  - store each coordinate with the step count when first reached
- For the second wire:
  - track steps as it moves
  - when hitting an intersection, combine:
    - steps from wire 1
    - steps from wire 2
- Return the smallest combined step count

---

## 🧠 Code Breakdown

### `Day03.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Crossed Wires`
- Loads input (two lines, one per wire)
- Splits each line into instruction sequences
- Calls the silver and gold solutions

---

### Wire Representation

Each wire is not stored as a full structure upfront but is processed step-by-step.

Instructions are parsed into:

- direction (`char`)
- distance (`int`)

Example:

    R8 → move right 8 steps

---

### Grid Movement

Movement is handled one step at a time, not in large jumps.

This is important because:

- intersections can occur anywhere along a segment
- step counts must be tracked precisely

Directions map to coordinate changes:

- `R` → `(x + 1, y)`
- `L` → `(x - 1, y)`
- `U` → `(x, y + 1)`
- `D` → `(x, y - 1)`

---

### Tracking Positions

For Part 1:

- Use a collection such as:

      HashSet<(int x, int y)>

- Store every visited coordinate from wire 1

For Part 2:

- Use:

      Dictionary<(int x, int y), int>

- Store the first step count at which each position is reached

---

### Detecting Intersections

While walking the second wire:

- Check if the current coordinate exists in wire 1's data
- If it does:
  - it's an intersection
  - process it depending on the part

Important:

- Skip `(0,0)` since both wires start there

---

### Manhattan Distance

Distance is calculated as:

    |x| + |y|

This is used for Part 1 to determine the closest intersection.

---

### Step Counting

Each move increments a step counter.

Example:

- First move → step 1
- Second move → step 2

For Part 2:

- Only record the first time a coordinate is reached
- This ensures the minimal step path is used

---

### Part 1 Return Value

- Compute Manhattan distance for all intersections
- Return the smallest value

---

### Part 2 Return Value

- For each intersection:
  - combine steps from both wires
- Return the minimum combined total

---

## 🛠 Implementation Notes

- Movement is processed step-by-step for accuracy
- Coordinates are typically stored as tuples `(x, y)`
- First visits to a coordinate are preserved for correct step counting
- The origin `(0,0)` is excluded from results
- Part 2 builds directly on Part 1 logic with added step tracking

---

## 🧪 Behaviour Summary

Given two wire paths:

- both start at `(0,0)`
- each instruction expands into multiple single-step moves
- wire 1 lays down a full path map
- wire 2 walks and checks for overlaps
- intersections are collected and evaluated

Results:

- Part 1 → closest intersection by Manhattan distance
- Part 2 → intersection reached in the fewest combined steps

---

## 🚀 Key Takeaways

- Step-by-step simulation is essential for correctness
- Using a `HashSet` or `Dictionary` makes intersection detection efficient
- Separating position tracking from step counting simplifies logic
- Part 2 is a natural extension of Part 1 with additional state
- Grid-based problems benefit from simple coordinate systems

---

## 🔗 References

- https://adventofcode.com/2019/day/3