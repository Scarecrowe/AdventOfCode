# 🎄 Advent of Code 2024 - Day 04: Ceres Search

## 📜 Puzzle Overview

This puzzle searches a character grid for specific word patterns.

The input is a 2D map of letters.

Example:

    MMMSXXMASM
    MSAMXMSMSA
    AMXSXMAAMM

The solver searches the grid in multiple directions to find occurrences of target words.

For Part 1:

- find every occurrence of `XMAS`

For Part 2:

- find X-shaped patterns formed by `MAS` on both diagonals

---

## 🧩 Part 1

Count how many times `XMAS` appears in the grid.

### 💡 Approach

- Parse the input into a 2D character map
- Visit every coordinate in the grid
- From each point, search in every direction
- If the full term `XMAS` is matched, count it
- Return the total number of matches

---

## 🧩 Part 2

Count how many X-shaped `MAS` patterns appear in the grid.

### 💡 Approach

- Reuse the same map structure
- Search diagonally for:
  - `MAS`
  - `SAM`
- Store all matching diagonal paths
- Compare matches that share the same centre point
- Count only those that form a full X shape around the middle letter

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Ceres Search`
- Loads the puzzle input
- Builds the map with:

    new(this.Input, c => c)

For Part 1:

- creates a local `VectorArray<int, char> map`
- scans the entire grid
- checks every `Cardinal` direction for `XMAS`

For Part 2:

- iterates over every map cell
- searches diagonal `MAS` and `SAM` patterns
- filters the collected matches into valid X shapes

---

### Map Representation

The puzzle uses:

- `VectorArray<int, char>`

This provides:

- `Width`
- `Height`
- coordinate-based access
- adjacency lookups in directional form

Each input character becomes one cell in the grid.

---

### Silver Search Logic

`Silver()` loops over:

- every `y` from `0` to `map.Height`
- every `x` from `0` to `map.Width`

For each coordinate it checks:

- every direction in `Enum.GetValues<Cardinal>()`

It calls:

    FindTerm(map, new Vector<int>(x, y), cardinal, "XMAS")

If the returned point list has length `4`, the term was found and the result is incremented.

So Part 1 is a full-grid directional word search.

---

### `FindTerm(...)`

This helper performs directional matching.

Parameters:

- the map
- starting point
- direction
- target term

It works like this:

- if the starting cell is not the first letter, return immediately
- add the starting point to the result list
- for each remaining character:
  - get adjacent cells with:

    map.AdjacentInterCardinal(point)

  - look for the required direction and expected letter
  - move to that point if found
  - stop early if not found

If the whole word is matched, it returns the full list of points.

---

### Gold Search Logic

Part 2 stores the puzzle map in:

- `Map`

and matching paths in:

- `Locations`

For every cell in:

    this.Map.AxisEnumerator()

the solver calls:

    this.Search("MAS", diagonal directions, cell.Point)

and:

    this.Search("SAM", diagonal directions, cell.Point)

The diagonal directions are:

- `NorthEast`
- `NorthWest`
- `SouthEast`
- `SouthWest`

So gold collects every valid diagonal `MAS` or reversed `SAM` path starting from each point.

---

### `Search(...)`

This helper loops through the supplied directions and calls `FindTerm(...)`.

If the number of returned points matches the term length, it stores that path in:

- `this.Locations`

So `Locations` becomes a collection of all diagonal matches found across the map.

---

### Filtering X Shapes

`Filter(ref int result)` turns the diagonal matches into actual X-MAS patterns.

It compares paths in `Locations` and looks for pairs where:

- the middle point is the same

That shared point is:

    locationA[1]

When two different paths share the same centre, the solver builds a combined set of points and checks whether it contains all four diagonal neighbours around that centre:

- north-west
- north-east
- south-west
- south-east

If all four are present, it counts as a valid X shape.

A `processed` dictionary is used so the same centre is not counted twice.

---

### Pattern Shape in Part 2

The gold solution is not just looking for the word in straight lines.

It specifically verifies that two diagonal matches cross at the same middle cell and create this structure:

    M . S
    . A .
    M . S

or the reversed diagonal equivalents.

That is why both `MAS` and `SAM` are searched.

---

## 🛠 Implementation Notes

- The map is stored as `VectorArray<int, char>`
- Part 1 checks all directions using `Cardinal`
- Part 2 only checks diagonal directions
- `FindTerm()` returns the matched point path, not just a boolean
- Gold stores all diagonal matches first, then filters them into X patterns
- Duplicate X centres are prevented with a processed lookup

---

## 🧪 Behaviour Summary

Given a grid of letters:

- Part 1 scans every position in every direction for `XMAS`
- each successful four-letter directional match is counted
- Part 2 searches for diagonal `MAS` and `SAM` paths
- matching diagonals are paired by shared centre point
- only full X-shaped crossings are counted
- the final outputs are:
  - total `XMAS` occurrences
  - total X-MAS shapes

---

## 🚀 Key Takeaways

- Good example of directional grid searching
- Reuses the same matching helper for both parts
- Returning point lists makes later geometric filtering possible
- Part 2 is solved by collecting candidate diagonals first, then validating shape
- The vector-based map abstraction keeps the grid logic tidy

---

## 🔗 References

- https://adventofcode.com/2024/day/4