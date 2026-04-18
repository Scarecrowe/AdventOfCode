# 🎄 Advent of Code 2021 - Day 05: Hydrothermal Venture

## 📜 Puzzle Overview

This puzzle works with a set of hydrothermal vent lines plotted onto a 2D grid.

Each input line describes a vent line segment in this format:

    x1,y1 -> x2,y2

Example input:

    0,9 -> 5,9
    8,0 -> 0,8
    9,4 -> 3,4
    2,2 -> 2,1

Part 1 considers only horizontal and vertical vent lines.

Part 2 includes diagonal vent lines as well.

The solver parses all vent definitions, maps them onto a grid, and then counts how many grid cells are crossed by at least two vents.

---

## 🧩 Part 1

Count how many points are overlapped by at least two horizontal or vertical vent lines.

### 💡 Approach

- Parse every vent line into an origin and destination point
- Build a grid large enough to contain every coordinate in the input
- For each vent:
  - map it horizontally when the X coordinate stays the same
  - map it vertically when the Y coordinate stays the same
  - ignore diagonal vents
- Increment the grid cell for every point touched by a vent
- Count how many cells have a value of `2` or more
- Return that total

---

## 🧩 Part 2

Count how many points are overlapped by at least two vents when diagonal lines are also included.

### 💡 Approach

- Reuse the same parsing and grid construction
- Map horizontal and vertical vents exactly as in Part 1
- Also include diagonal vents
- Determine the diagonal direction from the start and end points
- Walk one step at a time across the diagonal, incrementing each visited cell
- Count how many cells have a value of `2` or more
- Return that total

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Hydrothermal Venture`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new HydrothermalVenture(this.Input, false)`
- calls `TotalCrossOverVents()`

For Part 2:

- creates `new HydrothermalVenture(this.Input, true)`
- calls `TotalCrossOverVents()`

---

### `HydrothermalVent.cs`

This class models a single vent line.

It stores:

- `Origin`
- `Destination`

It also exposes helper properties:

- `IsHorizontal`
- `IsVertical`
- `IsDiagional`

A vent is considered diagonal when it is neither horizontal nor vertical.

---

### Vent Orientation

The implementation determines line type using coordinate equality.

- `IsHorizontal` checks whether `Origin.X == Destination.X`
- `IsVertical` checks whether `Origin.Y == Destination.Y`
- `IsDiagional` is true when neither of those conditions holds

This means the solver decides how to map each vent entirely from its endpoint coordinates.

---

### Diagonal Direction

The vent class also exposes a `Direction` property.

This determines which of the four diagonal directions the line travels in:

- `NorthEast`
- `SouthEast`
- `SouthWest`
- `NorthWest`

The direction is inferred by comparing the origin coordinates against the destination coordinates.

That direction is then used to select the correct diagonal stepping routine.

---

### `HydrothermalVenture.cs`

This class contains the full mapping and overlap-counting logic.

It:

- parses the raw input
- creates the 2D map
- maps every vent onto the grid
- counts all cells where at least two vents overlap

It stores:

- `Map`

which is a `VectorArray<int, int>` representing the vent coverage grid.

---

### Parsing the Input

`Parse(string[] input)` converts each line into a `HydrothermalVent`.

At a high level it does:

- split each line on `" -> "`
- split each endpoint on `","`
- convert those coordinate parts to integers
- create `new HydrothermalVent(...)`
- track the maximum `X` and `Y` values seen

The maximum coordinate values are used to size the map.

The grid is created as:

    new(point.X + 1, point.Y + 1)

So the map is always large enough to include every parsed vent endpoint.

---

### Counting Overlaps

`TotalCrossOverVents()` returns the final answer by scanning the whole map.

It counts every cell where the stored value is at least `2`.

Logically this is:

    count all cells where value >= 2

So the silver and gold answers both come from the same final counting rule. The only difference is whether diagonal vents were mapped in the first place.

---

### Mapping Horizontal Vents

`MapHorizontally(HydrothermalVent vent)` walks from the smaller Y coordinate to the larger Y coordinate and increments:

    this.Map[y, vent.Origin.X]++

So every point on that straight line is marked on the grid.

---

### Mapping Vertical Vents

`MapVertically(HydrothermalVent vent)` walks from the smaller X coordinate to the larger X coordinate and increments:

    this.Map[vent.Origin.Y, x]++

This marks every point covered by a straight horizontal run across the map.

---

### Mapping Diagonal Vents

`MapDiagionally(HydrothermalVent vent)` starts at the vent origin:

- copies `Origin.X` into `x`
- copies `Origin.Y` into `y`
- increments the starting grid cell
- checks `vent.Direction`
- dispatches to one of four movement helpers

Those helpers are:

- `MoveNorthEast`
- `MoveSouthEast`
- `MoveSouthWest`
- `MoveNorthWest`

Each helper loops for the diagonal length and updates both coordinates by one step on each iteration.

So the diagonal mapping is handled as an explicit step-by-step grid walk.

---

### Main Mapping Flow

`MapVents(List<HydrothermalVent> vents, bool includeDiagional)` processes every vent in turn.

For each vent:

- if it is horizontal, call `MapHorizontally`
- else if it is vertical, call `MapVertically`
- else if diagonals are disabled, skip it
- otherwise, if it is diagonal, call `MapDiagionally`

This means both puzzle parts share the same mapping pipeline, with only the `includeDiagional` flag changing the behaviour.

---

## 🛠 Implementation Notes

- The constructor takes `bool includeDiagional`
- The word `Diagional` is misspelled in the implementation and appears in method and property names
- `Day5.cs` passes `false` for silver and `true` for gold
- The parser sizes the map from the largest coordinates found in the input
- The final answer is based on grid cells with values greater than or equal to `2`
- Diagonal movement is handled through explicit directional helper methods rather than a generic step vector

---

## 🧪 Behaviour Summary

Given a list of vent line segments:

- the solver parses each line into origin and destination points
- it builds a grid large enough for all coordinates
- it maps horizontal and vertical vents directly onto that grid
- Part 1 ignores diagonal vents
- Part 2 includes diagonal vents and walks them step by step
- every visited point increments its grid cell count
- the final result is the number of cells touched by at least two vents

---

## 🚀 Key Takeaways

- Good example of converting line-segment input into a discrete grid map
- The solver cleanly separates parsing, mapping, and overlap counting
- Part 1 and Part 2 reuse the same main structure with a mode flag
- Diagonal support is added through directional stepping helpers
- The final overlap count is simple once the map has been populated

---

## 🔗 References

- https://adventofcode.com/2021/day/5