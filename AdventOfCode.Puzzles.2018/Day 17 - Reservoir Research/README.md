# 🎄 Advent of Code 2018 - Day 17: Reservoir Research

## 📜 Puzzle Overview

This puzzle simulates **water flowing through underground clay formations**.

The input describes clay veins as ranges in either `x` or `y`, for example:

    x=495, y=2..7
    y=7, x=495..501
    x=501, y=3..7

These lines define **blocked clay tiles** in a 2D grid.

Water starts from a single spring:

    x=500, y=0

and flows according to gravity and containment rules.

---

## 🧩 Part 1

Determine how many tiles are reached by water.

### 💡 Approach

- Parse clay veins into a 2D map/grid
- Simulate water flow from the spring downward
- Water can be:
  - flowing (`|`)
  - settled (`~`)
- Water spreads:
  - downwards first
  - then sideways if blocked
- Count all tiles that water can reach within bounds

Final answer:

- total number of tiles visited by water

---

## 🧩 Part 2

Count only the tiles where water is **settled (not flowing)**.

### 💡 Approach

- Use the same simulation as Part 1
- Track:
  - flowing water (`|`)
  - settled water (`~`)
- Only count tiles where water is fully contained
- Ignore water that eventually flows out of the system

Final answer:

- number of `~` tiles

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Reservoir Research`
- Loads input describing clay veins
- Builds simulation grid
- Runs both parts using the same simulation engine

For Part 1:

- Executes full water flow simulation
- Counts all water-reached tiles

For Part 2:

- Runs same simulation
- Filters only settled water tiles

---

### Grid Representation

The solution uses a **2D grid model** representing the ground.

Each cell can be:

- `.` → sand (empty)
- `#` → clay
- `|` → flowing water
- `~` → still water

The grid is typically stored as:

- a dictionary or array keyed by `(x, y)`
- or a bounded 2D array based on min/max coordinates

---

### Parsing Clay Veins

Each input line is parsed into coordinate ranges:

Example:

    x=495, y=2..7

becomes:

- all points `(495, 2)` through `(495, 7)`

Parsing logic:

- detect whether line starts with `x=` or `y=`
- expand range into individual blocked cells
- mark each as `#` in the grid

---

### Water Flow Rules

Water simulation follows deterministic rules:

1. Water flows **downwards if possible**
2. If blocked:
   - it spreads left and right
3. If both sides are bounded by clay:
   - water becomes **settled (`~`)**
4. If not bounded:
   - water remains **flowing (`|`)**

---

### Recursive Flow Behaviour

The core simulation typically uses a **recursive flood-fill style algorithm**:

- Start from spring `(500, 0)`
- Try to flow downward
- If blocked:
  - attempt left/right spread
- Each branch continues independently
- Stops when:
  - outside bounds
  - or already visited state

This naturally models branching water streams.

---

### Settling Logic

A row becomes settled when:

- both left and right ends hit clay walls
- and there is no downward escape path

Once confirmed:

- all `|` in that region are converted to `~`

This is often the trickiest part of the puzzle.

---

### Counting Results

After simulation completes:

Part 1 counts:

- all tiles containing:
  - `|`
  - `~`

Part 2 counts:

- only tiles containing:
  - `~`

---

## 🛠 Implementation Notes

- Input is parsed into clay coordinates first
- Grid is dynamically built or bounded from min/max coordinates
- Water simulation starts at `(500, 0)`
- Flow logic is recursive or stack-based DFS/BFS
- State tracking prevents infinite loops
- Settled water requires full horizontal containment
- Two result modes reuse the same simulation engine

---

## 🧪 Behaviour Summary

Given a map of clay veins:

- water is injected from a single source
- it flows downward, then sideways
- it may pool into reservoirs or escape
- Part 1 counts all water reachability
- Part 2 counts only stable reservoirs

---

## 🚀 Key Takeaways

- Classic physics-inspired grid simulation problem
- Requires careful handling of branching recursion
- Key complexity comes from detecting bounded regions
- Same engine supports both “reachability” and “stability” analysis
- Efficient state tracking is critical to avoid reprocessing cells
- Great example of turning physical intuition into discrete simulation logic

---

## 🔗 References

- https://adventofcode.com/2018/day/17