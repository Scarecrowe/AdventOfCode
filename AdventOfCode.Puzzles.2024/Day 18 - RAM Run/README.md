# 🎄 Advent of Code 2024 - Day 18: RAM Run

## 📜 Puzzle Overview

This puzzle revolves around navigating a grid while avoiding corrupted memory locations.

The input represents coordinates that become blocked over time, simulating failing RAM.

- You start at the top-left corner `(0,0)`
- You want to reach the bottom-right corner
- Bytes (coordinates) fall one by one and become blocked
- Movement is restricted to up, down, left, right

Part 1 asks for the shortest path after a fixed number of bytes have fallen.  
Part 2 asks for the first byte that makes the goal unreachable.

---

## 🧩 Part 1

Find the shortest path from the start to the goal after a set number of corrupted bytes.

### 💡 Approach

- Parse the input into a list of coordinates
- Apply the first **N bytes** as blocked positions
- Build a grid representing safe vs corrupted cells
- Use a pathfinding algorithm (typically BFS)
- Find the shortest path from `(0,0)` to `(maxX,maxY)`
- Return the number of steps in that path

---

## 🧩 Part 2

Determine which byte causes the grid to become unsolvable.

### 💡 Approach

- Start with an empty grid
- Add corrupted bytes one at a time
- After each addition:
  - Re-run pathfinding
  - Check if a path still exists
- The first byte that results in **no valid path** is the answer

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `RAM Run`
- Loads the input
- Calls both parts

For Part 1:

- Passes a fixed number of bytes into the solver
- Returns shortest path length

For Part 2:

- Iteratively increases the number of bytes
- Stops when no path can be found
- Returns the coordinate of the blocking byte

---

### Grid Representation

The grid is typically represented as:

- A 2D coordinate system
- A `HashSet` or similar structure for blocked cells

Each byte from the input marks a coordinate as corrupted.

---

### Parsing Input

Each line represents a coordinate:

    x,y

Parsing involves:

- Splitting on `,`
- Converting values into integers
- Storing as coordinate pairs

---

### Pathfinding Logic

The solver uses a breadth-first search (BFS) to find the shortest path.

At a high level:

- Start at `(0,0)`
- Explore all valid neighbouring positions
- Avoid:
  - Out-of-bounds positions
  - Corrupted cells
  - Already visited cells
- Continue until:
  - Goal is reached (success)
  - No more nodes to explore (failure)

---

### Movement Rules

From any position, you can move:

    up
    down
    left
    right

Each move:

- Costs 1 step
- Must remain within bounds
- Must not enter a corrupted cell

---

### Part 1 Execution

- Apply first `N` bytes as blocked
- Run BFS once
- Return number of steps to reach goal

---

### Part 2 Execution

- Start with zero blocked cells
- Loop through bytes:
  - Add next byte to blocked set
  - Run BFS
  - If BFS fails:
    - Return that byte as the answer

---

### Detecting Failure

Pathfinding fails when:

- The queue becomes empty
- The goal was never reached

This indicates the grid is fully blocked off.

---

## 🛠 Implementation Notes

- BFS guarantees the shortest path in an unweighted grid
- A `HashSet` is ideal for fast blocked-cell lookup
- Re-running BFS each iteration in Part 2 is brute-force but effective
- Coordinates are processed incrementally to simulate memory corruption
- Grid bounds are fixed based on puzzle constraints

---

## 🧪 Behaviour Summary

- The solver builds a grid of safe and corrupted cells
- Bytes progressively corrupt the grid
- Part 1 checks path viability after a fixed corruption state
- Part 2 identifies the tipping point where traversal becomes impossible
- BFS is used to determine reachability and shortest paths

---

## 🚀 Key Takeaways

- Classic grid traversal problem using BFS
- Demonstrates dynamic obstacle handling
- Part 2 highlights incremental failure detection
- Efficient use of sets for collision/block detection
- Clear separation between simulation and pathfinding logic

---

## 🔗 References

- https://adventofcode.com/2024/day/18