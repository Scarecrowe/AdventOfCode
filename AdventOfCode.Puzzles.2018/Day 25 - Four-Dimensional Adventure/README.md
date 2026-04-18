# 🎄 Advent of Code 2018 - Day 25: Four-Dimensional Adventure

## 📜 Puzzle Overview

This puzzle is about grouping points in **4-dimensional space** into “constellations”.

Each input line is a point in 4D:

    x, y, z, t

Example:

    0,0,0,0
    3,0,0,0
    0,3,0,0
    0,0,3,0

Two points belong to the same constellation if:

- their Manhattan distance is **≤ 3**, OR
- they are connected through a chain of intermediate points, each within distance 3

So this becomes a **graph connectivity problem**, where edges exist between “close enough” points.

---

## 🧩 Part 1

Determine how many constellations exist in the input set.

### 💡 Approach

- Parse each line into a `Point4D` structure:
  - `X, Y, Z, T`
- Treat each point as a node in a graph
- Connect two nodes if their Manhattan distance ≤ 3
- Use a graph traversal approach:
  - DFS or BFS
  - or Union-Find (Disjoint Set Union)
- Count how many disconnected groups remain

Return:

    number of constellations

---

## 🧩 Part 2

There is no separate Part 2 for this day.

Instead:

- The puzzle is a single-part completion
- Once constellation count is found, the challenge is complete

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets title to `Four-Dimensional Adventure`
- Loads input as a list of 4D coordinates
- Parses each line into a point object
- Builds connectivity graph or union-find structure

Returns:

- total number of constellations

---

### Point Representation

Each point is stored as:

- `X`
- `Y`
- `Z`
- `T`

Distance between two points is computed using:

    |x1 - x2| + |y1 - y2| + |z1 - z2| + |t1 - t2|

If this value is ≤ 3, the points are directly connected.

---

### Graph Construction

The solver conceptually builds edges:

- For each pair of points:
  - compute Manhattan distance
  - if ≤ 3 → connect them

This forms an undirected graph where:

- nodes = points
- edges = “same constellation” relationships

---

### Finding Constellations

Once the graph exists, the solver finds connected components:

### Option 1: DFS/BFS

- Maintain a `visited` set
- For each unvisited node:
  - run DFS/BFS
  - mark all reachable nodes
  - increment constellation count

### Option 2: Union-Find (typical optimisation)

- Initially each point is its own set
- For every connected pair:
  - union their sets
- Final answer = number of unique roots

---

## 🛠 Implementation Notes

- Distance checks dominate performance (O(n²))
- Union-Find is typically faster for large inputs
- Graph is implicitly complete but sparsely connected due to distance constraint
- No time dimension despite 4D coordinates (T is just another axis)
- Problem reduces to clustering in metric space

---

## 🧪 Behaviour Summary

Given a list of 4D points:

- each point is compared to every other point
- connections are formed if Manhattan distance ≤ 3
- clusters merge transitively through chains of points
- final output is the number of disconnected clusters

---

## 🚀 Key Takeaways

- Classic clustering / connected components problem
- 4D space increases complexity but not conceptual difficulty
- Manhattan distance defines adjacency
- Union-Find or DFS both work effectively
- Transitive connectivity is the key idea (chains matter, not just direct links)
- Elegant example of spatial grouping via graph theory

---

## 🔗 References

- https://adventofcode.com/2018/day/25