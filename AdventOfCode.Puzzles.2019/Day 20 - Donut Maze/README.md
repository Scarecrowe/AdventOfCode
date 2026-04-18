# 🎄 Advent of Code 2019 - Day 20: Donut Maze

## 📜 Puzzle Overview

This puzzle explores a maze made of:

- open paths
- walls
- named portals

The maze contains pairs of two-letter portal labels such as:

    BC
    DE
    FG

Each portal is linked to exactly one matching portal elsewhere in the maze, except:

- `AA` = start
- `ZZ` = end

The solver parses the maze into a grid, identifies all portal entrances, and then performs a breadth-first search through the maze.

Part 1 searches the normal portal maze. Part 2 treats the maze as recursive, where inner portals move deeper and outer portals move back outward through recursion levels.

---

## 🧩 Part 1

Determine the shortest number of steps from `AA` to `ZZ`.

### 💡 Approach

- Parse the maze into a 2D map
- Detect all letter pairs that form portal names
- Find the adjacent `.` tile that acts as the portal entrance
- Mark each portal as either:
  - inner
  - outer
- Start from portal `AA`
- Use breadth-first search across open tiles
- When a portal is reached:
  - jump to the matching portal with the same name
- Stop when `ZZ` is reached
- Return the shortest distance

---

## 🧩 Part 2

Determine the shortest path through the recursive version of the maze.

### 💡 Approach

- Reuse the same portal detection logic
- Treat each recursion depth as a separate maze state
- Inner portals move one level deeper
- Outer portals move one level outward
- Prevent invalid moves:
  - cannot go above level `0`
  - `AA` and `ZZ` only work on the outermost level
- Run breadth-first search using:
  - position
  - distance
  - recursion level
- Return the distance when `ZZ` is reached at the valid level

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Donut Maze`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new DonutMaze(this.Input)`
- Calls `Search()`

For Part 2:

- Creates `new DonutMaze(this.Input)`
- Calls `Search(true)`

So the same solver handles both puzzle modes with a recursion flag.

---

### `Portal.cs`

This class models one portal entrance in the maze.

It stores:

- `Point`
- `Name`
- `Travelled`
- `Inner`

The constructor records:

- the walkable portal entrance coordinate
- the portal label
- whether the portal is inner or outer

It also includes a copy constructor so portal state can be duplicated for recursive maze levels.

---

### `ProcessedCell.cs`

This helper class extends a grid cell with a processed flag.

It stores:

- `Processed`

This is used while scanning the maze letters so each portal label pair is only handled once.

---

### `DonutMaze.cs`

This class contains the full maze parsing and search logic.

It stores:

- `Map`
- `Portals`
- `Start`

The constructor:

- builds the maze grid from the input
- extracts all portals with `GetPortals()`
- finds the start portal by selecting the one named `AA`

So the maze is prepared once up front before any search begins.

---

### Maze Representation

The map is stored as a `VectorArray`.

That means each maze coordinate can be inspected by position, including:

- open path `'.'`
- letters used for portal names
- other maze characters

The solver uses cardinal adjacency when walking the maze.

---

### Parsing Portals

`GetPortals()` scans all letter cells in the maze and wraps them as `ProcessedCell` entries.

For each unprocessed letter cell:

- get its adjacent cells
- check whether it touches another letter
- determine whether one of the two letters has an adjacent `.` tile
- build the portal name from the two letters
- record the walkable entrance point

So a portal is identified by:

- two adjacent letters
- one nearby open path tile

That open path tile is the actual position used during traversal.

---

### Inner vs Outer Portals

When creating a portal, the solver determines whether it is inner or outer by checking the portal entrance coordinate against the maze border region.

A portal is treated as outer when its entrance lies near the outside edge of the maze.

Otherwise it is treated as inner.

This matters only for Part 2, where:

- inner portals increase recursion depth
- outer portals decrease recursion depth

---

### Main Search Loop

`Search(bool recursive = false)` performs a breadth-first search.

It creates:

- a list of per-level search states
- each state holds:
  - a portal list
  - a visited grid

It also creates a queue of:

- current point
- current distance
- current recursion step

The search starts from:

- `AA`
- distance `0`
- level `0`

Then it repeatedly dequeues states and explores adjacent cardinal cells.

---

### Walking Open Tiles

When an adjacent cell contains:

    .

the solver simply enqueues that coordinate with:

- distance + 1
- same recursion level

So standard corridor movement behaves like an ordinary shortest-path maze search.

---

### Travelling Through Portals

When an adjacent cell is a letter, the solver treats the current location as a portal entrance.

It then:

- finds the portal at the current point
- skips it if already marked as travelled in that state
- checks special rules for recursion
- if it is not `ZZ`, finds the matching portal with the same name
- enqueues the matching portal entrance point
- increases distance by `1`

For recursive mode, the next level changes by:

- `+1` for inner portals
- `-1` for outer portals

So portal travel acts like a teleport with level adjustment.

---

### Recursive State Handling

For Part 2, each recursion level gets its own copy of:

- portal state
- visited grid

When the search first needs to enter a deeper level through an inner portal, the solver creates a new level state by cloning all base portals.

This allows:

- visited tracking per level
- independent `Travelled` flags per level

So the recursive maze is handled as multiple layered BFS states rather than one giant flattened graph.

---

### Special Rules for `AA` and `ZZ`

In recursive mode:

- `AA` and `ZZ` cannot be used as normal transport portals on deeper levels
- outer portals cannot be used at level `0`
- reaching `ZZ` returns the final distance

In non-recursive mode:

- reaching `ZZ` immediately returns the current shortest path distance

---

### Part 1 Return Value

When `recursive` is `false`, `Search()` returns:

- the shortest distance from `AA` to `ZZ`

using normal portal teleportation.

---

### Part 2 Return Value

When `recursive` is `true`, `Search()` returns:

- the shortest distance from `AA` to `ZZ`

while respecting recursion depth rules for inner and outer portals.

---

## 🛠 Implementation Notes

- The maze is stored in a `VectorArray`
- Portals are extracted by pairing adjacent letter cells
- Each portal stores its entrance point and whether it is inner or outer
- Search is implemented with breadth-first traversal
- Recursive mode tracks separate visited grids and portal state per level
- Inner portals move deeper into recursion
- Outer portals move back outward
- `AA` is the start and `ZZ` is the finish

---

## 🧪 Behaviour Summary

Given a donut-shaped maze with labelled portals:

- the solver parses the maze into a 2D grid
- identifies all portal entrances
- starts at `AA`
- walks corridors one step at a time
- teleports through matching portals when encountered
- Part 1 searches the flat maze
- Part 2 searches a recursive layered version of the maze
- the final answer is the shortest path length to `ZZ`

---

## 🚀 Key Takeaways

- Nice example of combining grid parsing with graph-style traversal
- Portal detection is built directly from neighbouring letter cells
- Breadth-first search guarantees the shortest route
- Recursive mode is handled cleanly with separate per-level state
- The `Inner` flag is the key to controlling recursive portal behaviour
- The same search method supports both puzzle parts with one boolean switch

---

## 🔗 References

- https://adventofcode.com/2019/day/20