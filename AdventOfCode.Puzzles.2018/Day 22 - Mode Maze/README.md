# 🎄 Advent of Code 2018 - Day 22: Mode Maze

## 📜 Puzzle Overview

This puzzle involves navigating and analysing a dynamically generated cave system.

The cave is defined by:

- a **depth value**
- a **target coordinate (x, y)**

From these, every region in the grid can be calculated using:

- geologic index
- erosion level
- region type

Each coordinate becomes one of three terrain types:

- Rocky (`.`)
- Wet (`=`)
- Narrow (`|`)

The cave starts at `(0,0)` and extends until at least the target location.

Example behaviour is defined by deterministic formulas for generating terrain based on position and previous values.

---

## 🧩 Part 1

Determine the **total risk level** of the smallest rectangle from `(0,0)` to the target.

### 💡 Approach

- Parse input:
  - cave depth
  - target coordinate
- Generate region types using:
  - geologic index rules
  - erosion level calculation
  - modulo-based terrain classification
- For each coordinate `(x, y)` in the rectangle:
  - compute region type
  - map to risk level:
    - rocky = 0
    - wet = 1
    - narrow = 2
- Sum all risk values in the bounding box

Return the total risk level.

---

## 🧩 Part 2

Find the shortest time to reach the target using tools.

### 💡 Approach

This part turns the cave into a **pathfinding problem with constraints**:

- You can carry one of three tools:
  - Torch
  - Climbing gear
  - Neither (not always allowed)

Each region type restricts allowed tools.

Movement rules:

- moving to adjacent region costs 1 minute
- switching tools costs 7 minutes
- you must always have a valid tool for the region you are in

---

### Pathfinding Strategy

- Treat each state as:
  - position `(x, y)`
  - equipped tool
- Use a shortest-path algorithm (typically Dijkstra or A*)
- Generate neighbours by:
  - moving up/down/left/right if valid
  - switching tools if needed for region compatibility
- Use a priority queue keyed by time

The goal state is:

    (targetX, targetY, torch)

Return the minimum time to reach it.

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets title to `Mode Maze`
- Parses input:
  - depth
  - target coordinates
- Builds cave model (usually memoised region generator)

For Part 1:

- iterates grid from `(0,0)` → `(targetX, targetY)`
- computes risk level sum

For Part 2:

- runs shortest-path search over state space:
  - `(x, y, tool)`
- returns minimum travel time

---

### Cave Generation Logic

Each region is computed on demand:

1. **Geologic index rules**
   - `(0,0)` → 0
   - target → 0
   - x == 0 → `y * 48271`
   - y == 0 → `x * 16807`
   - otherwise:
     - product of left and above erosion levels

2. **Erosion level**

    erosion = (geologicIndex + depth) % 20183

3. **Region type**

    erosion % 3:
    - 0 → rocky
    - 1 → wet
    - 2 → narrow

This allows lazy computation of an infinite grid.

---

### State Representation (Part 2)

Each node in the search is:

- `(x, y, equippedTool, timeCost)`

The solver explores:

- movement transitions (cost 1)
- tool switches (cost 7)

Each region restricts valid tools:

- rocky → torch, climbing gear
- wet → climbing gear, neither
- narrow → torch, neither

So invalid states are never expanded.

---

### Pathfinding Loop

The solver typically uses:

- priority queue (min-heap)

Loop:

- pop lowest-cost state
- if target reached with torch → return cost
- generate valid neighbours
- push updated states

This ensures optimal shortest path discovery.

---

## 🛠 Implementation Notes

- Cave is computed lazily (memoised erosion/region lookup)
- Grid is effectively infinite but only explored as needed
- Part 1 is deterministic grid aggregation
- Part 2 is constrained graph search
- State space includes tool as a dimension
- Tool switching is sometimes required even without movement

---

## 🧪 Behaviour Summary

Given a depth and target:

- the cave is algorithmically generated cell-by-cell
- each region type is derived from erosion rules
- Part 1 computes total risk in bounding rectangle
- Part 2 finds shortest valid route with tool constraints
- movement cost + tool switching cost defines optimal path
- final result is minimum time to reach target with torch equipped

---

## 🚀 Key Takeaways

- Classic procedural terrain generation puzzle
- Strong use of memoisation to avoid recomputation
- Part 1 is straightforward aggregation
- Part 2 becomes a constrained shortest-path problem
- Tool system introduces an extra state dimension
- Efficient search requires treating `(position + tool)` as graph nodes
- Excellent example of combining simulation + graph theory

---

## 🔗 References

- https://adventofcode.com/2018/day/22