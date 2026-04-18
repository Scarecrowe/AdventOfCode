# 🎄 Advent of Code 2025 - Day 07: Laboratories

## 📜 Puzzle Overview

This puzzle works with a character grid representing beam movement through a laboratory layout.

The map contains a starting point:

- `S`

It also uses two important path markers:

- `.` for a straight downward path
- `^` for a splitter that creates two side branches

Part 1 follows the beam flow from the start and counts how many splitters are encountered while traversing the map.

Part 2 treats the same layout as a branching timeline problem and counts how many distinct terminal timelines can be reached from the start.

---

## 🧩 Part 1

Determine how many beam split events occur while traversing the laboratory map.

### 💡 Approach

- Load the input into a grid structure
- Find the start position marked with `S`
- Use a queue to walk the map from that starting point
- Move downward whenever the cell below is `.`
- When the cell below is `^`, count a split and branch left and right
- Continue until all reachable paths have been processed

---

## 🧩 Part 2

Determine how many distinct terminal timelines are produced by the same layout.

### 💡 Approach

- Start again from `S`
- Track how many ways each reachable position can be entered
- Move downward through `.`
- When a `^` is reached below the current position, branch to left and right open cells
- Accumulate the number of ways each branch can be reached
- At the end, sum the counts for all terminal positions

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Laboratories`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Laboratories(this.Input)`
- Calls `SplitBeams()`

For Part 2:

- Creates `new Laboratories(this.Input)`
- Calls `Timelines()`

The two parts share the same solver and differ only in which traversal result is returned.

---

### `Laboratories.cs`

This class contains the full map-processing logic.

It stores:

- `Map`
- `Start`

The constructor:

- builds the grid using `VectorArray`
- scans the grid to find the first cell containing `S`
- stores that location as the starting point

This gives both puzzle parts a shared map and entry position.

---

### Map Setup

The map is created directly from the input characters:

    this.Map = new(input, (c) => c);

That means each grid cell keeps its original character value.

The start is then found by scanning every cell until `S` is located.

---

### Part 1 Logic

`SplitBeams()` performs a queue-based traversal starting from `Start`.

For each queued position:

- look at the south neighbour
- if there is no south neighbour, stop processing that path
- if the south cell is `.`, mark it as visited using `|` and enqueue it
- if the south cell is `^`, count one split and enqueue the west and east neighbours

This makes Part 1 behave like a beam simulation that always prefers downward progress until a splitter is found. Each splitter increases the result by one.

---

### Visited Path Marking

When Part 1 moves into a `.` cell, that cell is overwritten with:

- `|`

This acts as a visited marker so the traversal works against the updated map state instead of repeatedly treating that position as untouched open space.

---

### Split Handling

When the south cell is `^` in `SplitBeams()`:

- the result count is incremented
- the current adjacent cells are re-read
- the west neighbour is enqueued if it exists
- the east neighbour is enqueued if it exists

So a splitter does not continue downward from the `^` cell itself. Instead, it fans out into horizontal branches from the current position.

---

### Part 2 Logic

`Timelines()` changes the model from simple traversal to path counting.

It uses:

- a dictionary called `timelines` to store how many ways each point can be reached
- a queue to process reachable points

The start begins with a count of `1`.

For each dequeued point:

- read its accumulated count
- inspect the south neighbour
- if the south cell is `.`, pass the full count downward
- if the south cell is `^`, distribute the count into left and right open branches

This builds up the total number of distinct ways each reachable point can be visited.

---

### Timeline Accumulation

In Part 2, counts are accumulated rather than overwritten.

For example:

- if a point has already been reached before
- and another path reaches it again
- the new count is added to the existing value

This is handled through:

    timelines.GetValueOrDefault(point) + count

That makes the gold solution count total branching timelines rather than just unique positions.

---

### Terminal Positions

After the main traversal, `Timelines()` checks every recorded position to see whether it is terminal.

A point is treated as terminal when:

- there is no south neighbour
- or the south neighbour is neither `.`
- nor `^`

All timeline counts for those terminal points are added into the final result.

This means the gold answer is the total number of branch histories that end at valid stopping positions.

---

## 🛠 Implementation Notes

- The input is stored directly as a character grid
- The starting point is the first `S` found in the map
- Part 1 uses queue-based traversal and mutates the map by marking visited `.` cells with `|`
- Part 2 does not count splitters directly; it counts how many ways terminal points can be reached
- Downward movement is only allowed through `.`
- Split behaviour is triggered when the cell below the current point is `^`

---

## 🧪 Behaviour Summary

The solver treats the map as a directed flow system:

- `S` is the entry point
- `.` continues the path downward
- `^` causes a horizontal split
- a path ends when it can no longer continue downward into `.` or interact with a splitter below

Part 1 counts how many split events happen.

Part 2 counts how many distinct branch outcomes end at terminal positions.

---

## 🚀 Key Takeaways

- Good example of using the same grid model for two different interpretations
- Part 1 is a traversal-and-count problem
- Part 2 turns the same traversal into a path-counting problem
- Queue-based processing keeps both parts iterative and easy to follow
- The timeline dictionary in Part 2 makes branch accumulation explicit and compact

---

## 🔗 References

- https://adventofcode.com/2025/day/7