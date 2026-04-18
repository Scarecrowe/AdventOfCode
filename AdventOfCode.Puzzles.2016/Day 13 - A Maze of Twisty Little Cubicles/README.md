# 🎄 Advent of Code 2016 - Day 13: A Maze of Twisty Little Cubicles

## 📜 Puzzle Overview

This puzzle generates a maze using a mathematical formula.

Each coordinate `(x, y)` is either:

- an open space
- or a wall

This is determined by:

```
x*x + 3*x + 2*x*y + y + y*y + favouriteNumber
```

- Convert the result to binary
- Count the number of `1` bits
- If the count is:
  - even → open space
  - odd → wall

You start at position `(1, 1)`.

Part 1 finds the shortest path to a target location.  
Part 2 counts how many locations can be reached within a limited number of steps.

---

## 🧩 Part 1

Determine the minimum number of steps required to reach the target coordinate.

### 💡 Approach

- Use a pathfinding algorithm (BFS)
- Start from `(1, 1)`
- At each step:
  - explore neighbouring positions:
    - up, down, left, right
- Only move to:
  - positions with `x >= 0`, `y >= 0`
  - open spaces (not walls)
- Track visited positions to avoid repetition
- Stop when the target is reached

---

## 🧩 Part 2

Determine how many unique positions can be reached in at most 50 steps.

### 💡 Approach

- Use BFS starting from `(1, 1)`
- Track number of steps taken to reach each position
- Only explore positions where:
  - steps ≤ 50
- Count all unique positions visited within this limit

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `A Maze of Twisty Little Cubicles`
- Loads the puzzle input (favourite number)
- Calls the silver and gold solutions

For Part 1:

- Performs BFS to find shortest path

For Part 2:

- Performs BFS with step limit
- Counts reachable positions

---

### Determining Walls

To evaluate a coordinate:

```
value = x*x + 3*x + 2*x*y + y + y*y + favouriteNumber
```

- Convert `value` to binary
- Count set bits
- Even → open space
- Odd → wall

---

### Movement Rules

From a position `(x, y)`:

- Possible moves:
  - `(x + 1, y)`
  - `(x - 1, y)`
  - `(x, y + 1)`
  - `(x, y - 1)`
- Only valid if:
  - coordinates are non-negative
  - position is not a wall

---

### Breadth-First Search

- Use a queue to explore positions
- Track visited coordinates
- Store distance (number of steps)

---

### Part 1 Logic

- Start BFS from `(1, 1)`
- Expand outward until target is reached
- Return number of steps

---

### Part 2 Logic

- Start BFS from `(1, 1)`
- Limit exploration to 50 steps
- Count all unique visited positions

---

## 🛠 Implementation Notes

- BFS guarantees shortest path in unweighted grid
- Bit counting can be optimised using built-in methods
- Avoid revisiting positions
- Step tracking is required for Part 2
- Grid is effectively infinite but bounded by exploration

---

## 🧪 Behaviour Summary

Given a favourite number:

- Maze layout is generated dynamically
- BFS explores reachable positions
- Part 1 finds shortest route to target
- Part 2 counts reachable locations within a step limit

---

## 🚀 Key Takeaways

- Procedural maze generation
- BFS for shortest path and reachability
- Bitwise operations for classification
- Same traversal logic reused with different stopping conditions

---

## 🔗 References

- https://adventofcode.com/2016/day/13