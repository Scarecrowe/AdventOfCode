# 🎄 Advent of Code 2018 - Day 18: Settlers of The North Pole

## 📜 Puzzle Overview

This puzzle simulates a cellular automaton over a 2D grid representing a lumber collection area.

Each cell in the grid can be:

- `.` → Open ground
- `|` → Trees
- `#` → Lumberyard

The entire grid evolves simultaneously each minute based on adjacent cells.

Example input:

    .#.#...|#.
    .....#|##|
    .|..|...#.
    ..|#.....#
    #.#|||#|#|
    ...#.||...
    .|....|...
    ||...#|.#|
    |.||||..|.
    ...#.|..|.

Each minute, every cell updates based on its 8 neighbours.

---

## 🧩 Part 1

Determine the resource value after 10 minutes.

### 💡 Approach

- Parse input into a 2D grid of characters
- Simulate 10 iterations of the grid
- For each cell:
  - count adjacent trees (`|`)
  - count adjacent lumberyards (`#`)
  - count open ground (`.`)
- Apply transformation rules simultaneously:
  - `.` → `|` if 3+ adjacent trees
  - `|` → `#` if 3+ adjacent lumberyards
  - `#` → `#` if at least 1 tree AND 1 lumberyard adjacent, else `.`
- After 10 steps:
  - count total trees and lumberyards
  - multiply them for resource value

---

## 🧩 Part 2

Determine the resource value after 1,000,000,000 minutes.

### 💡 Approach

- Run the same simulation as Part 1
- Store previous grid states (hashing the full grid)
- Detect when a previous state repeats
- Once a cycle is found:
  - determine cycle length
  - skip ahead using modulo arithmetic
- Resume simulation only for remaining steps
- Compute final resource value

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Settlers of The North Pole`
- Loads the input grid
- Calls simulation for Part 1 (10 steps)
- Calls simulation for Part 2 (1 billion steps with cycle detection)

---

### Grid Representation

The area is stored as a:

- 2D array or list of strings

Each cell represents one acre.

The grid is updated using a **double-buffer approach**:

- current state
- next state

This ensures all updates happen simultaneously.

---

### Neighbour Counting

For each cell:

- iterate over the 8 surrounding positions
- count:
  - trees (`|`)
  - lumberyards (`#`)
  - open ground (`.`)

Edges are handled by bounds checking (out-of-range ignored).

---

### Transition Rules

Each cell transitions based on its current state:

#### Open ground (`.`)

Becomes trees if:

- 3 or more adjacent trees

Otherwise remains open.

---

#### Trees (`|`)

Becomes lumberyard if:

- 3 or more adjacent lumberyards

Otherwise remains trees.

---

#### Lumberyard (`#`)

Remains a lumberyard only if:

- at least 1 adjacent lumberyard AND
- at least 1 adjacent tree

Otherwise becomes open ground.

---

### Simulation Loop

Each minute:

- build a new grid from current grid
- apply rules to every cell
- replace old grid with new grid

This repeats until the target time is reached.

---

### Resource Value Calculation

After simulation completes:

- count all `|`
- count all `#`
- compute:

    trees × lumberyards

This produces the final answer for both parts.

---

### Cycle Detection (Part 2 Optimisation)

For large iterations (1 billion minutes):

- store each grid state as a string hash
- track when each state first appears
- detect repetition

Once a cycle is found:

- compute cycle length
- skip forward using:

    remainingSteps % cycleLength

This avoids simulating all 1,000,000,000 steps.

---

## 🛠 Implementation Notes

- Classic 2D cellular automaton simulation
- Uses 8-direction neighbour checks
- Requires double-buffering for correct simultaneous updates
- Cycle detection is critical for Part 2 performance
- Grid hashing enables fast state comparison
- Final result is a simple resource multiplication

---

## 🧪 Behaviour Summary

Given a forested grid:

- the solver parses the input into a 2D map
- simulates minute-by-minute evolution of terrain
- each cell updates based on adjacent cells
- Part 1 runs for a fixed 10 minutes
- Part 2 runs for 1 billion minutes using cycle detection
- final answer is computed as tree count × lumberyard count

---

## 🚀 Key Takeaways

- Strong example of a 2D cellular automaton
- Requires careful simultaneous update handling
- Edge handling is important for correctness
- Cycle detection turns a huge simulation into a fast lookup problem
- Hashing full grid states enables efficient repetition detection
- Classic “simulate until pattern emerges” optimisation problem

---

## 🔗 References

- https://adventofcode.com/2018/day/18