# 🎄 Advent of Code 2023 - Day 14: Parabolic Reflector Dish

## 📜 Puzzle Overview

This puzzle simulates a platform filled with rocks that can roll when the platform is tilted.

The grid consists of:

- `O` → rounded rocks (these roll)
- `#` → cube-shaped rocks (these block movement)
- `.` → empty space

The platform can be tilted in different directions, causing all rounded rocks to roll as far as possible until they hit either:

- another rock
- a cube
- the edge of the grid

Part 1 focuses on a single tilt, while Part 2 introduces repeated cycles of tilting in multiple directions.

---

## 🧩 Part 1

Calculate the total load on the north support beam after tilting the platform north.

### 💡 Approach

- Parse the input into a 2D grid
- Tilt the platform north:
  - iterate through each column
  - move each rounded rock (`O`) upward as far as possible
- After all rocks settle, calculate the load:
  - each rock contributes weight based on its distance from the south edge
- Sum all contributions to get the final load

---

## 🧩 Part 2

Simulate **1,000,000,000 cycles** of tilting the platform in this order:

1. North
2. West
3. South
4. East

Return the final load after all cycles complete.

### 💡 Approach

- Reuse the tilt logic from Part 1
- Perform tilts in all four directions per cycle
- Detect repeating grid states to avoid simulating all 1 billion cycles:
  - store previously seen grid configurations
  - when a cycle repeats, calculate the remaining cycles using modulo arithmetic
- Jump forward to the final state
- Compute the final load as in Part 1

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Parabolic Reflector Dish`
- Loads the puzzle input
- Executes both parts

For Part 1:

- Creates the platform
- Applies a north tilt
- Calculates the load

For Part 2:

- Creates the platform
- Runs repeated tilt cycles with cycle detection
- Calculates the final load

---

### Grid Representation

The platform is represented as a 2D structure (typically a `char[,]` or similar).

Each position contains:

- `O` → movable rock
- `#` → blocker
- `.` → empty

---

### Tilting Logic

Each tilt direction follows the same principle:

- iterate in the correct order depending on direction
- for each `O`, move it step-by-step until:
  - it hits a `#`
  - it hits another `O`
  - it reaches the boundary

#### Example (North tilt):

- process rows from top to bottom
- for each column:
  - track the next available "landing" position
  - move rocks upward into that position

Other directions (West, South, East) follow the same logic but adjust iteration order.

---

### Load Calculation

The load is calculated after rocks settle.

For a grid of height `H`:

- a rock at row `r` contributes:

    H - r

So rocks closer to the top contribute more weight.

The final result is the sum of all such values.

---

### Cycle Simulation

Part 2 introduces repeated tilting cycles.

Each cycle consists of:

- North tilt
- West tilt
- South tilt
- East tilt

After each full cycle:

- the grid state is serialized (e.g. as a string)
- stored in a dictionary or hash set

---

### Cycle Detection

To handle 1,000,000,000 iterations efficiently:

- store seen states with their iteration index
- when a repeated state is found:
  - determine cycle length
  - compute remaining iterations using:

    remaining = (target - current) % cycle_length

- skip ahead to the final required iteration

This avoids simulating every cycle explicitly.

---

### Final Result

After reaching the correct final state:

- compute the load exactly as in Part 1

---

## 🛠 Implementation Notes

- The grid is mutated in place during tilts
- Directional tilts reuse the same logic with different traversal order
- Cycle detection is essential for performance in Part 2
- State comparison is typically done via string serialization of the grid
- Movement ensures rocks never "jump" over blockers or other rocks

---

## 🧪 Behaviour Summary

Given a platform grid:

- rocks roll in the direction of tilt until blocked
- Part 1 performs a single north tilt and computes load
- Part 2 performs repeated 4-direction cycles
- cycle detection allows skipping large numbers of iterations
- final load is calculated from rock positions

---

## 🚀 Key Takeaways

- Great example of grid simulation with directional movement
- Reusable movement logic across multiple directions
- Cycle detection transforms an infeasible brute-force problem into a fast solution
- State serialization is key for detecting repetition
- Emphasises careful iteration order when mutating grids

---

## 🔗 References

- https://adventofcode.com/2023/day/14