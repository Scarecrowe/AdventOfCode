# 🎄 Advent of Code 2020 - Day 24: Lobby Layout

## 📜 Puzzle Overview

This puzzle works on a hex tile floor.

Each input line is a path made from hex directions such as:

- `e`
- `se`
- `sw`
- `w`
- `nw`
- `ne`

Starting from the origin tile, each path leads to a destination tile.  
Whenever a tile is reached, it is flipped:

- white becomes black
- black becomes white

Part 1 counts how many tiles are black after all flips.  
Part 2 simulates 100 days of tile changes using neighbour-based rules.

---

## 🧩 Part 1

Determine how many tiles are black after following every path and flipping the destination tile each time.

### 💡 Approach

- Start with a tile dictionary containing the origin
- For each input line:
  - walk the path from the origin
  - resolve the final hex coordinate
  - flip that tile
- Count how many tiles are black at the end

---

## 🧩 Part 2

Simulate 100 days of tile changes and count how many black tiles remain.

### 💡 Approach

- Reuse the same initial flipping logic from Part 1
- Expand the tracked grid so neighbouring tiles always exist
- Repeat for 100 iterations:
  - count black neighbours for each tile
  - apply the daily flip rules
  - fill in adjacent white tiles for future checks
- Count the black tiles after day 100

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Lobby Layout`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `LobbyLayout.BlackSideUp(this.Input)`

For Part 2:

- Calls `LobbyLayout.BlackCount(this.Input)`

---

### `LobbyLayout.cs`

This class contains the full tile-flipping logic.

It exposes:

- `BlackSideUp(string[] input)`
- `BlackCount(string[] input)`

It also defines a `Directions` map for the six hex moves using cube-coordinate vectors.

Those directions are:

- East
- SouthEast
- SouthWest
- West
- NorthWest
- NorthEast

Each direction maps to a 3D vector such as:

    East      -> ( 1,  0, -1)
    SouthEast -> ( 0,  1, -1)
    SouthWest -> (-1,  1,  0)
    West      -> (-1,  0,  1)
    NorthWest -> ( 0, -1,  1)
    NorthEast -> ( 1, -1,  0)

---

### Hex Grid Representation

The solver uses cube coordinates to represent hex tiles.

Each tile position is stored as a `Vector`, and the tile collection is stored in a dictionary keyed by that vector, with a boolean value representing tile colour:

- `false` = white
- `true` = black

The dictionary is initialised with the origin tile:

    (0, 0, 0) -> false

So the floor starts with the origin explicitly tracked as white.

---

### `BlackSideUp(string[] input)`

This method solves Part 1.

It:

- creates a tile dictionary with the origin
- processes every path line with `Move(line, points)`
- gets the destination tile
- flips that tile:
  - adds it as black if unseen
  - otherwise toggles its current boolean value

At the end it returns:

- the count of dictionary values that are `true`

So the silver answer is simply the number of black tiles after all destination flips have been applied.

---

### Path Walking

`Move(string path, VectorDictionary tiles)` parses one input path and returns the destination tile.

It starts from:

    (0, 0, 0)

Then it converts the path into a stack and consumes characters one at a time.

Parsing works like this:

- if the next character is `e` or `w`, that is a complete direction
- if the next character is `n` or `s`, it combines it with the following character to form:
  - `ne`
  - `nw`
  - `se`
  - `sw`

Each parsed direction is converted through `CompassToCardinalMap`, then the corresponding cube vector is added to the current tile position.

After each move, the solver also calls `Fill(hex, tiles)` so adjacent tiles exist in the dictionary.

The method returns the final tile coordinate after the full path has been consumed.

---

### Counting Black Neighbours

`Black(Vector point, VectorDictionary points)` counts how many adjacent tiles are black.

It does this by:

- iterating over the six direction vectors
- adding each one to the current tile
- checking whether that neighbour exists and is black
- counting the matches

So this method provides the neighbour count needed for the Part 2 daily simulation.

---

### `Fill(Vector point, VectorDictionary points)`

This helper ensures the grid is expanded around a tile.

For every adjacent hex direction it:

- computes the neighbouring coordinate
- adds it to the dictionary as white if it does not already exist

This matters because Part 2 needs white tiles around black regions to be present so they can potentially flip on a future day.

---

### `BlackCount(string[] input)`

This method solves Part 2.

It begins by repeating the same flip setup used in Part 1:

- create the origin tile
- walk each input path
- flip the destination tile

Then it runs:

    100

iterations of:

    points = Iterate(points);

Finally it returns the number of black tiles remaining.

So the gold answer is the black tile count after 100 days of evolution.

---

### Daily Simulation Rules

`Iterate(VectorDictionary tiles)` applies one full day of updates.

For every tile currently in the dictionary it:

- counts adjacent black tiles
- applies these rules:
  - if the tile is black, it stays black only when it has 1 or 2 black neighbours
  - if the tile is white, it becomes black only when it has exactly 2 black neighbours

This is implemented as:

- black tile -> `adjacent == 1 || adjacent == 2`
- white tile -> `adjacent == 2`

It writes all results into a new dictionary called `next`.

After that, it expands `next` by calling `Fill(...)` on every tile so that surrounding white tiles are present before the next iteration.

Then it returns the new dictionary.

---

## 🛠 Implementation Notes

- The solver uses cube coordinates for hex movement
- Tile colours are stored as booleans in a vector-keyed dictionary
- The origin starts as white at `(0, 0, 0)`
- Each input path flips exactly one destination tile
- Adjacent tiles are proactively added through `Fill(...)`
- Part 2 reuses the Part 1 setup before running 100 daily updates
- Daily rules are based on the number of black neighbouring tiles

---

## 🧪 Behaviour Summary

Given a list of hex-direction paths:

- the solver starts from the origin for each path
- walks through the hex grid using cube-coordinate vectors
- flips the destination tile after each path
- Part 1 counts black tiles after all flips
- Part 2 simulates 100 days of neighbour-based updates
- the final result is either:
  - the initial black tile count, or
  - the black tile count after 100 iterations

---

## 🚀 Key Takeaways

- Clean use of cube coordinates for hex-grid navigation
- Part 1 is a straightforward destination-tile flip tracker
- Part 2 is a cellular automaton on a hex grid
- `Fill(...)` is important because white border tiles must exist to be evaluated
- The same vector direction map supports both path walking and neighbour checks

---

## 🔗 References

- https://adventofcode.com/2020/day/24