# 🎄 Advent of Code 2024 - Day 6: Guard Gallivant

## 📜 Puzzle Overview

This puzzle simulates a guard moving around a grid-based map.

The map consists of:

- open tiles `.`
- obstacles `#`
- a guard starting position with a facing direction (`^`, `v`, `<`, `>`)

The guard follows a strict movement pattern:

1. Move forward in the current direction
2. If the next tile is an obstacle, turn right instead
3. Repeat until leaving the map (Part 1) or detecting a loop (Part 2)

---

## 🧩 Part 1

Determine how many unique positions the guard visits before leaving the map.

### 💡 Approach

- Parse the grid into a 2D structure
- Locate the guard's starting position and direction
- Track visited positions using a set
- Simulate movement:
  - Attempt to move forward
  - If blocked, rotate right and try again
- Continue until the guard exits the grid
- Return the number of unique visited positions

---

## 🧩 Part 2

Determine how many positions could cause the guard to get stuck in a loop if an obstacle were placed there.

### 💡 Approach

- For every possible empty tile:
  - Temporarily place an obstacle
  - Run the same simulation as Part 1
  - Detect if the guard enters a loop
- Count how many placements result in a loop

Loop detection is typically done by tracking:

- position
- direction

If the same combination is encountered again, a loop exists.

---

## 🧠 Code Breakdown

### `Day06.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Guard Gallivant`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses the grid
- Runs the guard simulation
- Returns the number of visited tiles

For Part 2:

- Iterates over candidate obstacle placements
- Runs loop detection for each
- Counts valid loop-causing positions

---

### Grid Representation

The grid is typically stored as:

- `char[,]` or similar structure

It allows:

- checking boundaries
- reading tile contents
- updating temporary obstacles

---

### Guard State

The guard is defined by:

- `Position` (x, y)
- `Direction` (enum or vector)

Directions map to movement vectors, for example:

- up: `(0, -1)`
- right: `(1, 0)`
- down: `(0, 1)`
- left: `(-1, 0)`

---

### Movement Logic

Each step follows:

1. Calculate next position based on current direction
2. If next tile is `#`:
   - rotate right
3. Else:
   - move forward

Rotation follows a fixed order:

- up → right → down → left → up

---

### Tracking Visited Positions

For Part 1:

- Use a `HashSet<(int x, int y)>`
- Add position after each successful move

This ensures:

- only unique tiles are counted

---

### Loop Detection

For Part 2:

Track states using:

    (position, direction)

Use a structure like:

    HashSet<(int x, int y, direction)>

If a state repeats:

- the guard is in a loop
- simulation stops early

---

### Obstacle Testing

To test each candidate tile:

- skip:
  - existing obstacles
  - the guard's starting position

For each valid tile:

1. Place a temporary `#`
2. Run the simulation
3. Check for loop detection
4. Restore the tile

---

### Part 1 Return Value

- Total number of unique tiles visited before exiting the grid

---

### Part 2 Return Value

- Number of obstacle placements that cause a loop

---

## 🛠 Implementation Notes

- Direction handling is critical for correctness
- Loop detection must include direction, not just position
- Simulation should terminate early on loop detection
- Grid mutations in Part 2 must be safely reverted
- Performance can be improved by avoiding unnecessary allocations

---

## 🧪 Behaviour Summary

Given a grid with a guard:

- the solver simulates movement step-by-step
- the guard turns right when blocked
- Part 1 tracks all visited tiles until exit
- Part 2 tests obstacle placements to detect infinite loops
- loop detection relies on repeated state recognition

---

## 🚀 Key Takeaways

- Classic grid traversal with deterministic movement rules
- Direction-aware state tracking is essential
- Loop detection transforms the problem from simulation to state analysis
- Reusing the same simulation logic across both parts keeps the solution clean
- Small rule changes (adding obstacles) can drastically change system behaviour

---

## 🔗 References

- https://adventofcode.com/2024/day/6