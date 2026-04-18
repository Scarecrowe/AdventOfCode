# 🎄 Advent of Code 2019 - Day 24: Planet of Discord

## 📜 Puzzle Overview

This puzzle simulates the spread of bugs on a 5x5 grid.

The input is a small map made of:

- `#` for bug
- `.` for empty space

Each minute, every tile updates based on the number of adjacent bugs in the four cardinal directions.

The rules are:

- A bug dies unless it has exactly one adjacent bug
- An empty tile becomes infested if it has exactly one or two adjacent bugs

Part 1 keeps evolving the grid until a layout appears for the second time, then calculates its biodiversity rating.

Part 2 treats the grid as recursive layers and simulates bug growth across multiple depth levels for 200 minutes.

---

## 🧩 Part 1

Find the biodiversity rating of the first repeated layout.

### 💡 Approach

- Parse the input into a 2D map
- Store each full layout in a cache
- Repeatedly evolve the grid one minute at a time
- After each step, flatten the map into a string key
- Stop when a layout is seen twice
- Calculate the biodiversity rating by summing powers of two for every bug tile position
- Return that total

---

## 🧩 Part 2

Simulate the recursive bug system for 200 minutes and count the total number of bugs.

### 💡 Approach

- Convert the input map into a boolean grid
- Treat that as recursion level `0`
- Simulate all levels together for 200 iterations
- Add new outer or inner levels when bugs reach the required edges
- Skip the centre tile because it links to child levels
- Apply the same bug survival rules across recursive neighbours
- Count every bug across every level after the simulation finishes
- Return the total

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Planet of Discord`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PlanetOfDiscord(this.Input)`
- Calls `BiodiversityRating()`

For Part 2:

- Creates `new PlanetOfDiscord(this.Input)`
- Calls `BugCount()`

---

### `PlanetOfDiscord.cs`

This class contains both puzzle solutions.

It stores:

- `Map`
- `Cache`
- `Visited`
- `Input`

The constructor:

- stores the raw input
- builds a `VectorArray` map from the characters
- creates an empty cache
- initialises the visited grid

So the same class handles both the repeated-layout logic and the recursive-level simulation.

---

### Map Representation

For Part 1, the map is stored as a `VectorArray` of characters.

Each tile is either:

- `#`
- `.`

The map is always processed using cardinal adjacency only:

- up
- down
- left
- right

Diagonal neighbours are never used.

---

### Part 1 Evolution

`BiodiversityRating()` starts by caching the initial layout:

    this.Cache.Add(this.Map.Flatten().Join());

It then repeatedly creates a fresh map and applies the update rules to every cell.

For each tile:

- if it currently contains a bug, it survives only when exactly one adjacent tile is also a bug
- if it is empty, it becomes a bug when the adjacent bug count is one or two

This is done by counting:

    this.Map.AdjacentCardinal(cell.Point).Count(c => c.Value == '#')

Each minute produces a completely new map, which then replaces the old one.

---

### Detecting the First Repeated Layout

After each evolution step, the solver flattens the new map into a string key:

    string key = string.Join(string.Empty, map.Flatten());

If that key already exists in `Cache`, the layout has repeated and the biodiversity rating is calculated.

Otherwise, the layout is added to the cache and the simulation continues.

So Part 1 stops at the first repeated full-map state, not after a fixed number of minutes.

---

### Biodiversity Rating

When a repeated layout is found, the solver scans each tile in reading order and adds powers of two for every bug tile.

At a high level it does:

    for each tile index i
        if tile is '#'
            result += 2^i

So the top-left tile contributes:

    2^0

the next tile contributes:

    2^1

and so on across the full flattened 5x5 grid.

The final total is returned as the silver answer.

---

### Part 2 Recursive Levels

`BugCount()` uses a different representation.

It converts the input into:

- `bool[,]`

where:

- `true` means bug
- `false` means empty

It stores the starting map in a dictionary of levels:

    { 0, this.Visited }

So recursion level `0` is the original puzzle input.

The solver then runs:

    200

iterations of recursive evolution by calling:

    this.EvolveWithLevels(levels);

After those 200 minutes, it counts all `true` tiles across every level.

---

### Adding New Levels

Before evolving the recursive system, the solver may create a new outer or inner level.

It checks:

- `HasInnerEdgeBugs(...)`
- `HasOuterEdgeBugs(...)`

If bugs exist on the relevant edges, it adds:

- one level below the current minimum
- one level above the current maximum

Each new level starts as an empty 5x5 boolean grid.

This allows recursion to expand outward as the simulation progresses.

---

### Centre Tile Handling

In the recursive version, the centre tile at:

    (2, 2)

is skipped entirely:

    if (x == 2 && y == 2) { continue; }

That tile is not treated as a normal cell because it represents the gateway into the next inner level.

So Part 2 never places or evolves a bug directly on the middle tile.

---

### Recursive Neighbour Counting

`EvolveWithLevels(...)` counts neighbours from three places:

- the current level
- the parent level
- the child level

For the current level, it starts with ordinary adjacent cells.

Then it adjusts the count for recursion:

- tiles on the left, right, top, or bottom edge can see into the parent level
- tiles immediately next to the centre can see into a whole row or column of the child level

For example:

- a tile at the left edge checks the parent level tile at `(1, 2)`
- a tile just above the centre checks the entire top row of the child level
- a tile just left of the centre checks the entire left column of the child level

This is how the implementation models the nested recursive grid structure.

---

### Part 2 Update Rules

Once the recursive neighbour count is known, the same survival rules are applied as in Part 1.

For each tile:

- a bug survives only with exactly one bug around it
- an empty tile becomes infested when the count is one or two

The result is written into a new map for that level.

After every level has been processed, the solver replaces the old level maps with the new ones.

---

### Final Bug Count

After 200 recursive evolution steps, the solver counts bugs across every level.

It loops through:

- every stored recursion level
- every `y`
- every `x`

and increments the total whenever the tile is `true`.

That total is returned as the gold answer.

---

## 🛠 Implementation Notes

- Part 1 uses a character-based `VectorArray`
- Part 2 uses `bool[,]` grids stored in a level dictionary
- Layout repetition in Part 1 is tracked with a flattened string cache
- Biodiversity is calculated with powers of two by tile index
- Part 2 always runs for exactly 200 minutes
- The recursive simulation skips the centre tile
- New levels are added dynamically when bugs reach the necessary edges
- Both parts use the same bug survival and infestation rules

---

## 🧪 Behaviour Summary

Given a 5x5 bug map:

- the solver evolves the grid using cardinal adjacency rules
- Part 1 stops when a layout repeats and computes its biodiversity score
- Part 2 treats the map as a recursive stack of 5x5 levels
- the centre tile acts as a link to inner levels
- edge tiles can connect to parent levels
- after 200 recursive minutes, the solver counts all remaining bugs

---

## 🚀 Key Takeaways

- Good example of reusing the same cellular automaton rules in two different representations
- Part 1 focuses on repeated-state detection with a cache
- Part 2 extends the puzzle into recursive spatial layers
- The centre tile becomes a structural link rather than a normal cell
- The implementation cleanly separates flat-grid logic from recursive-level logic

---

## 🔗 References

- https://adventofcode.com/2019/day/24