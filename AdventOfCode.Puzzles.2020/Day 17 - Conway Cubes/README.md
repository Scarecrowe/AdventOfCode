# 🎄 Advent of Code 2020 - Day 17: Conway Cubes

## 📜 Puzzle Overview

This puzzle simulates Conway-style cellular automata in higher dimensions.

The input is a 2D slice where:

- `#` means an active cube
- `.` means an inactive cube

The solver expands that starting slice into:

- 3D space for Part 1
- 4D space for Part 2

It stores cube states in dictionaries keyed by vector positions, then runs exactly 6 simulation cycles.

---

## 🧩 Part 1

Determine how many cubes are active after 6 cycles in 3D space.

### 💡 Approach

- Parse the input grid into 3D coordinates using `z = 0`
- Store each cube as active or inactive
- Repeat for 6 cycles:
  - examine every known cube
  - examine neighbouring positions around each cube
  - apply Conway cube rules
- Return the number of active cubes after the final cycle

---

## 🧩 Part 2

Determine how many cubes are active after 6 cycles in 4D space.

### 💡 Approach

- Parse the input grid into 4D coordinates using `z = 0` and `t = 0`
- Store each cube as active or inactive
- Repeat for 6 cycles:
  - inspect all known hypercubes
  - inspect neighbouring positions in 4D space
  - apply the same active/inactive rules
- Return the number of active cubes after 6 cycles

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Conway Cubes`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `ConwayCubes.Simple(this.Input)`

For Part 2:

- Calls `ConwayCubes.Advanced(this.Input)`

---

### `ConwayCubes.cs`

This class contains both puzzle solutions as static methods:

- `Simple(string[] input)`
- `Advanced(string[] input)`

It also contains the neighbour lookup helpers:

- `GetAdjacent3D(...)`
- `GetAdjacent4D(...)`

---

### Part 1 Grid Initialisation

`Simple(...)` creates a dictionary of cube states.

It uses:

- 3D vectors
- `bool` values for active or inactive state

For each input character:

- `#` becomes `true`
- `.` becomes `false`

Each starting point is stored at:

- `x`
- `y`
- `z = 0`

So the 2D input is treated as a flat slice in 3D space.

---

### Part 1 Simulation Loop

The 3D solver runs:

    6

cycles.

For each cycle it creates:

- `Dictionary<..., bool> next = new();`

It then loops through every known cube and:

- gets all adjacent 3D positions
- checks unknown neighbouring positions as possible births
- applies the active/inactive rules
- writes results into `next`

At the end of the cycle:

- `cubes = next.ToDictionary(x => x.Key, x => x.Value);`

So each round builds a fresh state map from the previous one.

---

### 3D Neighbour Rules

For an existing cube:

- if it is active and has 2 or 3 active neighbours, it stays active
- if it is inactive and has exactly 3 active neighbours, it becomes active
- otherwise it becomes inactive

This is implemented by counting:

- `allAdjacent.Count(x => x.Value)`

where the adjacent dictionary contains all 26 neighbouring positions around the current cube.

---

### Expanding into New 3D Positions

The solver does not only evaluate cubes already in the dictionary.

For every adjacent position around each cube:

- if that position is not already in `cubes`
- it gets that position's own neighbours
- counts how many are active
- activates it in `next` if exactly 3 neighbours are active

This allows new cubes outside the current known boundary to appear during a cycle.

---

### `GetAdjacent3D(...)`

This helper returns all neighbouring positions around a 3D target cube.

It loops through:

- `x - 1` to `x + 1`
- `y - 1` to `y + 1`
- `z - 1` to `z + 1`

It skips the target cube itself, so the result contains:

- 26 neighbouring positions

For each neighbour it checks whether the cube already exists in the dictionary, and stores the discovered state in the returned result.

If a neighbour is not found, it defaults to inactive.

---

### Part 2 Grid Initialisation

`Advanced(...)` works the same way as Part 1, but uses 4D vectors.

Each input character is stored at:

- `x`
- `y`
- `z = 0`
- `t = 0`

So the starting plane becomes a flat slice in 4D space.

---

### Part 2 Simulation Loop

The 4D solver also runs for:

    6

cycles.

For each known hypercube it:

- gets all adjacent 4D positions
- checks unknown neighbours for possible activation
- applies the same life rules as Part 1
- writes results into a fresh dictionary

The final answer is:

- the number of entries whose value is `true`

---

### `GetAdjacent4D(...)`

This helper extends the 3D neighbour logic into 4D.

It loops through:

- `x - 1` to `x + 1`
- `y - 1` to `y + 1`
- `z - 1` to `z + 1`
- `t - 1` to `t + 1`

It skips the target position itself, so the result contains:

- 80 neighbouring positions

As with the 3D version, each neighbour is returned with its current known state, defaulting to inactive if not present in the dictionary.

---

### Final Return Values

Both methods finish by returning:

- the count of cubes with value `true`

So:

- Part 1 returns the number of active 3D cubes after 6 cycles
- Part 2 returns the number of active 4D cubes after 6 cycles

---

## 🛠 Implementation Notes

- Part 1 uses 3D vectors and Part 2 uses 4D vectors
- Cube state is stored in dictionaries keyed by position
- Active state is represented with `bool`
- Both modes run exactly 6 cycles
- Each cycle builds a fresh dictionary rather than mutating in place
- Unknown neighbours are explicitly checked so new cubes can activate outside the current boundary
- The 3D helper returns 26 neighbours
- The 4D helper returns 80 neighbours

---

## 🧪 Behaviour Summary

Given a 2D starting pattern:

- the solver loads it into either 3D or 4D space
- each cycle checks neighbours around every known position
- active cubes survive with 2 or 3 active neighbours
- inactive cubes activate with exactly 3 active neighbours
- new outer positions are considered during each cycle
- after 6 cycles, the active cube count is returned

---

## 🚀 Key Takeaways

- Good example of extending Conway's Game of Life into higher dimensions
- Uses sparse dictionaries instead of large fixed-size grids
- Handles boundary growth naturally by probing unknown neighbours
- Reuses the same overall cycle logic for both 3D and 4D variants
- Keeps neighbour discovery isolated in dedicated helper methods

---

## 🔗 References

- https://adventofcode.com/2020/day/17