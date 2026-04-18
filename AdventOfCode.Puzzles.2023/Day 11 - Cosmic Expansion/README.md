# 🎄 Advent of Code 2023 - Day 11: Cosmic Expansion

## 📜 Puzzle Overview

This puzzle works with an image of space represented as a 2D grid.

The grid contains:

- galaxies marked with `#`
- empty space marked with `.`

Some rows and columns contain no galaxies at all. These empty rows and columns are considered expanded, which increases the distance between galaxies when measuring travel paths.

Part 1 uses a small expansion amount.

Part 2 uses a much larger expansion amount.

The solver does not physically rebuild the grid. Instead, it:

- records galaxy positions
- identifies empty rows and columns
- adjusts pairwise distances based on how many expanded gaps sit between two galaxies

---

## 🧩 Part 1

Sum the shortest path distances between every pair of galaxies using an expansion amount of `2`.

### 💡 Approach

- Parse the input grid and collect all galaxy coordinates
- Find every row that contains no galaxies
- Find every column that contains no galaxies
- Compare every pair of galaxies
- For each pair:
  - calculate the normal Manhattan distance
  - count how many empty rows lie between them
  - count how many empty columns lie between them
  - add extra distance for those expanded gaps
- Sum the adjusted distances for all galaxy pairs

---

## 🧩 Part 2

Repeat the same calculation, but expand empty rows and columns by `1000000`.

### 💡 Approach

- Reuse the same galaxy positions
- Reuse the same empty row and column lists
- Compare all galaxy pairs again
- Add a much larger expansion penalty for each empty row and column crossed
- Sum all adjusted distances

The logic is identical to Part 1. Only the expansion amount changes.

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Cosmic Expansion`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new CosmicExpansion(this.Input)`
- calls `SumOfShortestPath(2)`

For Part 2:

- creates `new CosmicExpansion(this.Input)`
- calls `SumOfShortestPath(1000000)`

So both parts use the same solver and differ only by the expansion amount passed in.

---

### `CosmicExpansion.cs`

This class contains the full parsing and distance logic.

It stores:

- `Galaxies`
- `Cols`
- `Rows`

`Galaxies` contains the coordinates of every galaxy found in the map.

`Rows` stores the indices of rows with no galaxies.

`Cols` stores the indices of columns with no galaxies.

These three collections are enough to solve both parts without rebuilding an expanded version of the grid.

---

### Parsing the Grid

The constructor reads the input into a `VectorDictionary` and then enumerates every cell.

It collects galaxy positions with logic equivalent to:

- keep every point where the character is `#`

So the constructor builds:

- a list of all galaxy coordinates

It then determines empty rows with:

- every row index from `0` to `input.Length - 1`
- where no galaxy has that `Y` value

And empty columns with:

- every column index from `0` to `input[0].Length - 1`
- where no galaxy has that `X` value

This means expansion is represented indirectly through row and column index lists, not by inserting new rows or columns into the map.

---

### `Galaxies`

`Galaxies` is stored as:

- `List<Vector<int>>`

Each entry is the coordinate of one galaxy in the original unexpanded map.

So if the input contains several `#` symbols, each one becomes one vector point in this list.

These coordinates are then used when comparing galaxy pairs.

---

### `Rows` and `Cols`

The solver precomputes all empty rows and columns.

`Rows` contains:

- row indices with no galaxies at all

`Cols` contains:

- column indices with no galaxies at all

These are the exact rows and columns that should count as expanded gaps when measuring distances.

This precomputation makes the later pair-distance logic much simpler.

---

### `SumOfShortestPath(long amount)`

This method solves both parts.

It:

- loops through every pair of galaxies
- avoids counting the same pair twice
- calculates the distance between the two points
- adds extra distance for each empty row and column between them
- returns the total sum

The method starts with:

- `long result = 0`
- `HashSet<(Vector<int>, Vector<int>)> visited = new();`

The `visited` set is used to make sure `(A, B)` and `(B, A)` are not both counted.

---

### Pair Enumeration

Galaxy pairs are generated using:

- `this.Galaxies.PairEnumerator()`

For each pair:

- `pointA`
- `pointB`

the solver checks whether that pair has already been visited in either order.

If not:

- compute the adjusted shortest path
- add it to the running total
- store the pair in `visited`

This ensures each unique galaxy pair contributes exactly once.

---

### Base Distance

The starting distance between two galaxies is:

- `pointA.Distance(pointB)`

This is the standard Manhattan distance between the two coordinates.

So before any expansion adjustments, the solver already knows how far apart the galaxies are in the original grid.

---

### Expansion Adjustment

After calculating the base distance, the solver adds extra cost for every empty row and empty column crossed.

For columns, it counts how many empty column indices lie strictly between the two galaxy `X` values.

For rows, it counts how many empty row indices lie strictly between the two galaxy `Y` values.

Each such gap contributes:

- `amount - 1`

extra distance

That means the total added distance is:

- `(amount - 1) * number_of_empty_columns_between`
- plus `(amount - 1) * number_of_empty_rows_between`

So the full logic is effectively:

- base Manhattan distance
- plus expansion penalty for empty columns crossed
- plus expansion penalty for empty rows crossed

This works because the original grid already counts each row and column once, so only the extra expansion beyond the original size needs to be added.

---

### Why `amount - 1` Is Used

If an empty row or column expands by:

- `2`

then it becomes one extra gap beyond the original single row or column.

If it expands by:

- `1000000`

then it contributes `999999` extra spaces beyond the original one already included in the Manhattan distance.

That is why the implementation adds:

- `amount - 1`

instead of the full `amount`.

---

## 🛠 Implementation Notes

- The puzzle title is `Cosmic Expansion`
- The solver stores galaxy positions as `List<Vector<int>>`
- Empty rows and columns are precomputed during construction
- The grid is not physically expanded
- Pair distances are calculated directly from original coordinates
- A `visited` set prevents double-counting reversed galaxy pairs
- Part 1 uses `SumOfShortestPath(2)`
- Part 2 uses `SumOfShortestPath(1000000)`
- Distance adjustments are based on empty row and column counts strictly between two galaxies
- The final result is returned as a `long`

---

## 🧪 Behaviour Summary

Given a galaxy map:

- the solver finds every galaxy position
- identifies which rows and columns are completely empty
- compares every unique pair of galaxies
- computes their Manhattan distance
- adds extra distance for every expanded empty row and column crossed
- sums all adjusted pair distances

So the final result is either:

- the total path sum with expansion amount `2`
- or the total path sum with expansion amount `1000000`

---

## 🚀 Key Takeaways

- The implementation avoids rebuilding the expanded map entirely
- Empty rows and columns are tracked as index lists
- Pairwise galaxy comparison is enough to solve the puzzle
- Expansion is applied as an adjustment on top of Manhattan distance
- Using `amount - 1` correctly accounts for the original row or column already included in the base distance
- The same method solves both parts by changing only the expansion factor

---

## 🔗 References

- https://adventofcode.com/2023/day/11