# 🎄 Advent of Code 2024 - Day 20: Race Condition

## 📜 Puzzle Overview

This puzzle revolves around navigating a race track grid and identifying how shortcuts (or "cheats") can reduce travel time.

The input represents a map containing:

- walls (`#`)
- open paths (`.`)
- a start position (`S`)
- an end position (`E`)

The goal is to:

- determine the shortest valid path from start to end
- analyse potential "cheats" where jumping across the map can save time

Part 1 looks at small shortcut opportunities, while Part 2 expands this idea significantly.

---

## 🧩 Part 1

Determine how many valid cheats save at least 100 picoseconds using a small cheat range.

### 💡 Approach

- Parse the grid and locate:
  - start (`S`)
  - end (`E`)
- Perform a shortest path search (BFS) to compute:
  - distance from start to every reachable tile
- Walk the discovered path and evaluate possible shortcuts:
  - for each position, look at nearby tiles within a small Manhattan distance (typically 2)
- For each potential cheat:
  - calculate the time saved:
  
        saved = normal_distance - shortcut_distance

- Count how many cheats save at least `100`

---

## 🧩 Part 2

Repeat the same logic but allow much larger cheat distances.

### 💡 Approach

- Reuse the same parsed grid and distance map
- Increase the allowed cheat radius significantly (e.g. up to 20 tiles)
- For every pair of points within this range:
  - compute Manhattan distance between them
  - compare it with the actual path distance
- Count all cheats where:

        saved >= 100

This dramatically increases the search space, so efficiency matters.

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Race Condition`
- Loads the puzzle input
- Executes both parts

For Part 1:

- builds the solver with input
- runs the cheat detection with a small radius

For Part 2:

- runs the same logic with an expanded cheat radius

---

### Grid Parsing

The input is treated as a 2D grid.

The parser:

- reads each line
- maps characters into coordinates
- identifies:
  - start position (`S`)
  - end position (`E`)
- stores walkable tiles (`.`)

---

### Pathfinding

A breadth-first search (BFS) is used to compute shortest distances.

At a high level:

- start from `S`
- explore all valid neighbouring tiles
- store distance to each tile

This produces:

- a map of shortest distances from the start to every reachable point

---

### Distance Map

The BFS produces a structure similar to:

- `Dictionary<Point, int> distances`

Where:

- key = coordinate
- value = shortest distance from start

This is critical for evaluating cheats efficiently.

---

### Cheat Detection

The core idea:

- compare the real path distance vs a direct shortcut

For two points `A` and `B`:

- real cost = `dist[B] - dist[A]`
- shortcut cost = Manhattan distance between A and B

If:

    real_cost - shortcut_cost >= 100

Then it's a valid cheat.

---

### Manhattan Distance

Distance between two points is computed as:

    abs(x1 - x2) + abs(y1 - y2)

This represents the cost of a direct "jump".

---

### Cheat Radius

Part 1:

- only checks nearby points (small radius)

Part 2:

- checks a much larger radius (e.g. 20)

This is implemented by:

- iterating over all offsets within the allowed range
- filtering by Manhattan distance

---

### Optimisation Considerations

- Only evaluate tiles that exist in the distance map
- Avoid recomputing distances
- Use precomputed BFS results for all comparisons

---

## 🧪 Behaviour Summary

Given a race track grid:

- compute shortest paths from start to all points
- evaluate potential shortcuts between positions
- compare real path vs direct movement
- count how many shortcuts save at least 100 time units

Part 1:

- small cheat radius
- fewer valid shortcuts

Part 2:

- large cheat radius
- significantly more combinations

---

## 🛠 Implementation Notes

- BFS is used for shortest path calculation
- Manhattan distance is used for shortcut evaluation
- A dictionary stores computed distances
- Cheat detection compares real vs direct path cost
- Part 2 expands the search radius dramatically

---

## 🚀 Key Takeaways

- Efficient pathfinding (BFS) enables reuse of computed distances
- Comparing two distance metrics reveals optimisation opportunities
- Manhattan distance is a powerful heuristic for grid problems
- Expanding search constraints can drastically increase complexity
- Precomputation is key to keeping the solution performant

---

## 🔗 References

- https://adventofcode.com/2024/day/20