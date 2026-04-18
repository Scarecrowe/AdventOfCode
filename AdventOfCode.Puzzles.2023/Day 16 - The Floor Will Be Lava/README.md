# 🎄 Advent of Code 2023 - Day 16: The Floor Will Be Lava

## 📜 Puzzle Overview

This puzzle simulates beams of light travelling through a grid of mirrors and splitters.

The map contains:

- `.` → empty space
- `/` → forward-slash mirror
- `\` → backslash mirror
- `|` → vertical splitter
- `-` → horizontal splitter

A beam starts at some edge position and moves in one of the four cardinal directions.

As it travels:

- empty space lets it continue straight
- mirrors reflect it
- splitters may either let it continue straight or split it into two beams

The goal is to count how many grid cells become energised by at least one beam.

Part 1 runs a single beam from the default starting point. Part 2 tries every valid beam entry from the outer edge and returns the maximum energised count.

---

## 🧩 Part 1

Determine how many tiles become energised from the initial beam run.

### 💡 Approach

- Parse the input into a 2D map
- Start a beam at the required entry point
- Traverse the beam through the grid
- Track both:
  - the current tile position
  - the direction of travel
- Prevent infinite loops by remembering already-visited `(position, direction)` states
- Count how many distinct positions were reached

---

## 🧩 Part 2

Determine the maximum number of energised tiles obtainable by firing a beam from any valid edge entry.

### 💡 Approach

- Enumerate every edge cell of the map
- Launch beams inward from each valid border direction
- Handle corners by trying both inward directions
- Run the same beam simulation used in Part 1
- Collect all energised tile counts
- Return the largest result

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Floor Will Be Lava`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TheFloorWillBeLava(this.Input)`
- Calls `Shine()`

For Part 2:

- Creates `new TheFloorWillBeLava(this.Input)`
- returns that object via string interpolation

In this implementation, Part 1 is explicitly driven through `Shine()`, while the gold path relies on the puzzle class' string conversion behaviour.

---

### `TheFloorWillBeLava.cs`

This class contains the beam simulation logic.

It stores:

- `Map`

The constructor builds the map with:

- `this.Map = new(input, (c) => c);`

So the puzzle input is loaded directly into a `VectorArray` of characters.

---

### Map Contents

The solver reacts to these tile types:

- `.` for empty space
- `/` for one mirror orientation
- `\` for the other mirror orientation
- `|` for a vertical splitter
- `-` for a horizontal splitter

Each tile changes the beam behaviour depending on the beam's incoming direction.

---

### `Shine()`

`Shine()` computes the best energised result by testing beam entry points around the outside of the map.

It creates:

- `List<int> results = new();`

Then loops through:

- `this.Map.EdgeEnumerator()`

For each edge cell it launches one or more beams inward.

Corner handling is special:

- top-left tries East and South
- top-right tries West and South
- bottom-left tries West and North
- bottom-right tries East and North

Non-corner edges only fire inward once.

After testing every edge start, it returns:

- `results.Max()`

So this method is effectively the exhaustive edge-search logic.

---

### Beam Traversal

`Beam(Vector start, Cardinal direction)` performs the core simulation.

It creates:

- `Queue<(Vector Point, Cardinal Direction)> queue = new();`
- `HashSet<(Vector, Cardinal)> visited = new();`

The queue allows the solver to process:

- a single continuing beam
- or multiple beams created by splitters

Each queued item stores both:

- the current position
- the current direction

This is important because revisiting the same tile from a different direction can lead to different future paths.

---

### Loop Prevention and Bounds Checks

At the start of each beam step, the solver skips work when:

- the `(position, direction)` state has already been visited
- the beam has moved outside the map bounds

This check prevents infinite cycling between mirrors and splitters.

---

### Empty Space

When the current tile is:

    .

the beam simply continues in the same direction by transforming the current point with that direction.

---

### Mirror Behaviour

When the current tile is:

    /

the beam is reflected as follows:

- North → East
- South → West
- East → North
- West → South

When the current tile is:

    \

the beam is reflected as follows:

- North → West
- South → East
- East → South
- West → North

So the implementation hard-codes the reflection rules for each mirror and incoming direction.

---

### Splitter Behaviour

When the current tile is:

    |

then:

- beams arriving from North or South continue straight
- beams arriving from East or West split into:
  - North
  - South

When the current tile is:

    -

then:

- beams arriving from East or West continue straight
- beams arriving from North or South split into:
  - East
  - West

This is why the queue is necessary: one incoming beam can create two outgoing beams.

---

### Counting Energised Tiles

At the end of `Beam(...)`, the solver returns:

- the count of distinct positions found in `visited`

It does that with:

    visited.Select(x => x.Item1).Distinct().Count()

So even if a tile is visited multiple times from different directions, it only counts once toward the energised total.

---

## 🛠 Implementation Notes

- The map is stored as a `VectorArray`
- Beam traversal uses a queue rather than recursion
- Visited states are tracked as `(Vector, Cardinal)`
- This avoids infinite loops in cyclic beam paths
- Mirrors redirect beams using explicit direction mappings
- Splitters may create two new beam paths
- The energised count is based on distinct positions only
- `Shine()` evaluates all inward edge starts and returns the best result

---

## 🧪 Behaviour Summary

Given a mirror grid:

- the solver launches a beam from an edge
- each step examines the current tile
- mirrors redirect the beam
- splitters may branch it into two beams
- repeated `(position, direction)` states are ignored
- Part 1 uses the beam logic directly
- Part 2 searches all valid edge-entry beams and keeps the maximum energised total

---

## 🚀 Key Takeaways

- Good example of grid traversal with directional state
- Tracking position alone is not enough; direction must also be tracked
- A queue makes splitter-based branching easy to manage
- Distinct energised tiles are counted separately from visited beam states
- Exhaustively testing all edge starts is a clean way to solve the maximum-search variant

---

## 🔗 References

- https://adventofcode.com/2023/day/16