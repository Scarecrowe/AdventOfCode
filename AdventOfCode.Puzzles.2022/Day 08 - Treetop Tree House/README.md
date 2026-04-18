# 🎄 Advent of Code 2022 - Day 08: Treetop Tree House

## 📜 Puzzle Overview

This puzzle works with a grid of tree heights.

Each input line is a row of digits, where each digit represents the height of one tree.

The solver parses the input into a 2D numeric map and solves two related problems:

- Part 1 counts how many trees are visible from outside the grid
- Part 2 finds the best scenic score for any tree

The implementation treats the edge trees as automatically visible, then scans from each direction to discover additional visible interior trees.

---

## 🧩 Part 1

Count how many trees are visible from outside the grid.

A tree is visible if, looking from one of the four edges, it is taller than every tree seen before it in that direction.

### 💡 Approach

- Parse the input into a 2D height map
- Add every edge tree to a `Visible` set immediately
- Scan inward from:
  - north
  - south
  - west
  - east
- For each scan line:
  - track the tallest tree seen so far
  - when a tree is taller than that value:
    - mark it as visible
    - update the tallest height
- Return the total number of visible trees

This means edge trees are counted first, and only newly discovered interior trees are added during the directional scans.

---

## 🧩 Part 2

Find the highest scenic score for any tree.

The scenic score is the product of the viewing distances in all four directions:

- north
- south
- west
- east

### 💡 Approach

- Visit every point in the map
- For each tree:
  - count how many trees can be seen looking north
  - count how many trees can be seen looking south
  - count how many trees can be seen looking west
  - count how many trees can be seen looking east
- Multiply those four values together
- Return the maximum score found

A viewing direction stops when:

- the edge of the map is reached
- or a tree of equal or greater height blocks the view

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Treetop Tree House`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TreetopTreeHouse(this.Input)`
- Calls `VisibleTrees()`

For Part 2:

- Creates `new TreetopTreeHouse(this.Input)`
- Calls `BestTreeHouseScore()`

---

### `TreetopTreeHouse.cs`

This class contains all puzzle logic.

It stores:

- `Map`
- `Visible`

Where:

- `Map` is the parsed 2D tree-height grid
- `Visible` is a set of visible tree coordinates

The constructor does two things:

- parses the grid with `Parse(input)`
- seeds `Visible` with all edge points from `this.Map.EdgeEnumerator()`

So the edge trees are already counted before any directional visibility scans begin.

---

### Parsing the Grid

`Parse(string[] input)` builds the map with:

    new(input, (c) => c.ToInt())

So each character digit in the input is converted directly into an integer tree height and stored in a `VectorArray`.

---

### Part 1 Entry Point

`VisibleTrees()` returns:

    this.Visible.Count + this.ViewNorth() + this.ViewSouth() + this.ViewWest() + this.ViewEast()

So the result is:

- all edge trees
- plus any newly discovered interior visible trees from the four directional scans

Because visibility is stored in a set, a tree is only counted once even if it is visible from multiple directions.

---

### Shared Visibility Scan Logic

The four directional methods all call one helper:

    View(...)

This helper scans one line at a time across the map.

For each scan line it:

- starts with the edge tree as the current tallest
- moves inward through the line
- checks whether each next tree is taller than the current tallest
- if it is:
  - update the tallest
  - add the point to `Visible` if it is not already present
  - increment the result count only for newly added trees

So the directional methods differ only in how they choose:

- which rows or columns to scan
- where to start
- whether the scan moves forward or backward

---

### Directional Visibility Methods

The implementation provides:

- `ViewNorth()`
- `ViewSouth()`
- `ViewWest()`
- `ViewEast()`

Each one configures the shared `View(...)` helper with different coordinate selectors.

That means the same visibility logic is reused for all four directions without duplicating the core comparison behaviour.

---

### Scenic Score Calculation

`BestTreeHouseScore()` iterates over all map points with:

    this.Map.AxisEnumerator()
        .Select(cell => cell.Point)
        .Max(point => this.MoveNorth(point) * this.MoveSouth(point) * this.MoveWest(point) * this.MoveEast(point))

So for every tree, it calculates the four directional viewing distances and multiplies them together.

The highest such product becomes the gold answer.

---

### Shared Movement Logic

The four movement methods all call:

    Move(...)

This helper counts how far a tree can see in one direction.

It walks cell by cell away from the starting point and:

- increments the distance while trees are shorter than the current tree
- if it finds a tree of equal or greater height:
  - increments once more
  - stops immediately

So the blocking tree is included in the viewing distance, which matches the puzzle rules.

---

### Directional Scenic Methods

The scenic helper methods are:

- `MoveNorth(Vector point)`
- `MoveSouth(Vector point)`
- `MoveWest(Vector point)`
- `MoveEast(Vector point)`

Each method passes the appropriate:

- starting index
- loop condition
- increment/decrement logic
- coordinate selector

to the shared `Move(...)` helper.

This keeps the directional scenic-score logic compact and consistent.

---

## 🛠 Implementation Notes

- The grid is parsed into a numeric `VectorArray`
- Edge trees are preloaded into a `HashSet` of visible points
- Part 1 uses four directional scans to discover additional visible interior trees
- Duplicate visibility is prevented by the set
- Part 2 computes scenic score as the product of four directional viewing distances
- A blocking tree still counts as the final visible step in a direction
- Both puzzle parts reuse shared helper methods for directional logic

---

## 🧪 Behaviour Summary

Given a grid of tree heights:

- the solver parses the digits into a 2D map
- all edge trees are treated as visible immediately
- Part 1 scans inward from each edge and adds taller newly visible trees
- Part 2 measures viewing distance from each tree in all four directions
- the scenic score is the product of those four directional distances
- the final answers are:
  - total visible trees
  - best scenic score

---

## 🚀 Key Takeaways

- Nice example of reusing generic directional helpers for both parts
- Edge trees are handled up front, which simplifies the visibility scans
- A `HashSet` ensures visible trees are not double-counted
- Part 1 is based on tallest-so-far comparisons
- Part 2 is based on directional distance multiplication
- The implementation keeps north/south/east/west behaviour symmetrical and compact

---

## 🔗 References

- https://adventofcode.com/2022/day/8