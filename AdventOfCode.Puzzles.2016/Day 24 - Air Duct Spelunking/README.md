# 🎄 Advent of Code 2016 - Day 24: Air Duct Spelunking

## 📜 Puzzle Overview

This puzzle navigates a maze containing numbered locations.

The map consists of:

- walls `#`
- open spaces `.`
- numbered points `0, 1, 2, ...`

You start at location `0` and must visit all other numbered locations.

Movement is allowed:

- up
- down
- left
- right

The goal is to find the shortest path that visits all points.

Part 1 requires visiting all points once.  
Part 2 requires returning to the starting point after visiting all points.

---

## 🧩 Part 1

Determine the minimum number of steps required to visit all numbered locations.

### 💡 Approach

- Parse the grid and locate all numbered points
- Compute shortest paths between every pair of points
- Evaluate all possible visit orders
- Select the path with the minimum total distance

---

## 🧩 Part 2

Determine the minimum number of steps required to visit all locations and return to the start.

### 💡 Approach

- Use the same distances computed in Part 1
- Evaluate all permutations of visit orders
- Add the distance from the final point back to `0`
- Select the shortest total route

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Air Duct Spelunking`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Computes shortest route visiting all points

For Part 2:

- Computes route including return to start

---

### Parsing the Grid

Each cell is:

- wall → not traversable
- open space → traversable
- number → point of interest

Store:

- grid layout
- coordinates of each numbered point

---

### Distance Calculation

- Use Breadth-First Search from each point
- Compute shortest distance to all other points
- Store results in a lookup table

This avoids repeated pathfinding during permutation checks

---

### Graph Reduction

Instead of working on the full grid:

- reduce the problem to a graph of numbered points
- edges represent shortest distances between points

---

### Permutation Search

- Generate all possible orders of visiting points
- Always start at `0`
- Sum distances between consecutive points
- Track the minimum total

---

### Part 1 Logic

- Compute pairwise distances
- Evaluate permutations of remaining points
- Return shortest total path

---

### Part 2 Logic

- Same as Part 1
- Add final step returning to `0`
- Return shortest complete cycle

---

## 🛠 Implementation Notes

- BFS is used for shortest path in grid
- Number of points is small, making permutations feasible
- Precomputing distances is critical for performance
- Problem is similar to travelling salesman but with small input size
- Avoid recomputing paths inside permutation loop

---

## 🧪 Behaviour Summary

Given a grid with numbered locations:

- Movement occurs on open tiles only
- Distances between points are precomputed
- All visit orders are evaluated
- Part 1 finds shortest route visiting all points
- Part 2 finds shortest route returning to start

---

## 🚀 Key Takeaways

- Grid pathfinding combined with permutation search
- BFS used for distance precomputation
- Problem reduces to small graph optimisation
- Similar to travelling salesman problem with manageable size

---

## 🔗 References

- https://adventofcode.com/2016/day/24