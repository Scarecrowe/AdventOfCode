# 🎄 Advent of Code 2016 - Day 22: Grid Computing

## 📜 Puzzle Overview

This puzzle models a grid of storage nodes.

Each node has:

- position `(x, y)`
- total size
- used space
- available space

Data can be moved between nodes under certain conditions.

Part 1 focuses on identifying viable data transfers.  
Part 2 turns the problem into a movement puzzle to retrieve specific data.

---

## 🧩 Part 1

Determine the number of viable pairs of nodes.

### 💡 Approach

A pair of nodes `(A, B)` is viable if:

- `A` is not empty (`used > 0`)
- `A` and `B` are not the same node
- `A.used <= B.available`

Steps:

- Parse all nodes from input
- Compare every pair of nodes
- Count how many satisfy the conditions

---

## 🧩 Part 2

Determine the minimum number of steps required to move the goal data to `(0, 0)`.

### 💡 Approach

- Identify key node types:
  - empty node (used = 0)
  - very large nodes (effectively immovable)
- Treat the grid as a movement puzzle:
  - large nodes act as walls
  - empty node acts as the movable space

Strategy:

1. Move the empty node next to the goal data
2. Repeatedly shift the goal data toward `(0, 0)`:
   - swap goal data with adjacent empty node
   - reposition empty node around it

This resembles a sliding puzzle.

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Grid Computing`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Counts viable node pairs

For Part 2:

- Models grid movement
- Computes minimum steps

---

### Parsing Input

Each line contains:

```
/dev/grid/node-x0-y0     size  used  avail
```

Extract:

- `x`, `y`
- `size`
- `used`
- `available`

---

### Node Representation

Each node includes:

- coordinates
- total size
- used space
- available space

---

### Part 1 Logic

- Iterate through all node pairs
- Apply viability conditions
- Count valid pairs

---

### Grid Interpretation

For Part 2:

- Empty node → movable space
- Large nodes → walls
- Normal nodes → traversable

Visualising the grid helps identify structure.

---

### Movement Strategy

- Move empty node to position adjacent to goal data
- Then repeat a cycle:
  - move goal data left by one
  - reposition empty node around it

Each shift requires a fixed number of moves.

---

### Part 2 Logic

- Locate:
  - empty node
  - goal data (top-right corner)
- Use pathfinding (BFS) to move empty node
- Then simulate repeated shifts toward `(0, 0)`
- Count total moves

---

## 🛠 Implementation Notes

- Part 1 is brute-force pair comparison
- Part 2 is a pathfinding + pattern problem
- Large nodes act as obstacles
- Visualising grid simplifies reasoning
- Movement becomes predictable after setup

---

## 🧪 Behaviour Summary

Given a grid of nodes:

- Part 1 counts viable transfers
- Part 2 rearranges data using movement rules
- Empty node acts as the key to movement
- Goal is to shift target data to `(0, 0)`

---

## 🚀 Key Takeaways

- Pairwise validation problem (Part 1)
- Grid transformation into sliding puzzle (Part 2)
- Visualisation reveals hidden structure
- Combination of BFS and pattern-based optimisation

---

## 🔗 References

- https://adventofcode.com/2016/day/22