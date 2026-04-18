# 🎄 Advent of Code 2021 - Day 09: Smoke Basin

## 📜 Puzzle Overview

This puzzle works with a height map where each input line is a row of single-digit heights.

Each character represents the height at one grid position.

Example input:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678

Part 1 finds every low point and sums their risk levels.

Part 2 finds the basins that grow out from those low points, takes the three largest basin sizes, and multiplies them together.

The solver parses the input into a 2D height map and then uses cardinal adjacency to inspect neighbouring cells.

---

## 🧩 Part 1

Find the sum of the risk levels of all low points.

### 💡 Approach

- Parse the input into a 2D map of integer heights
- Check every point in the map
- Compare that point against its up, down, left, and right neighbours
- A point is a low point only if every adjacent point is higher
- For each low point, add its height plus `1`
- Return the total risk level sum

---

## 🧩 Part 2

Find the three largest basins and multiply their sizes together.

### 💡 Approach

- Reuse the same height map
- Find all low points first
- For each low point, recursively expand outward
- Stop expanding when:
  - the point has already been visited, or
  - the height is `9`
- Collect every connected non-`9` point into that basin
- Sort the basin sizes in descending order
- Take the largest three
- Multiply their sizes together
- Return the result

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Smoke Basin`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new SmokeBasin(this.Input)`
- calls `SumOfRiskLevels()`

For Part 2:

- creates `new SmokeBasin(this.Input)`
- calls `SumOfBasin()`

---

### `SmokeBasin.cs`

This class contains the full solution logic.

It stores:

- `Map`

The constructor parses the input with:

    ParseInput(input)

and stores the result in:

    private VectorArray<int, int> Map { get; }

So the full puzzle is driven from a 2D integer height map.

---

### Parsing the Height Map

`ParseInput(string[] input)` creates the map directly from the raw text.

It does this with:

    new(input, (c) => $"{c}".ToInt())

That means:

- each input row becomes a row in the map
- each character is converted into an integer height
- the resulting structure can then be accessed by point or coordinate

---

### Finding Low Points

`SumOfRiskLevels()` works by calling:

    this.Lowest()

That method scans the full map and collects every point where `IsLowest(...)` returns `true`.

At a high level:

- loop over every `y`
- loop over every `x`
- test whether the current point is lower than all cardinal neighbours
- if so, add it to the low-point set

---

### Low Point Check

`IsLowest(Vector<int> point)` determines whether a point is lower than all adjacent cardinal cells.

It:

- reads the current height from `this.Map[point]`
- loops through `this.Map.AdjacentCardinal(point)`
- compares neighbour height against the current height

If any adjacent point has a value less than or equal to the current height, the point is not a low point.

Logically the rule is:

    if (adjacent <= current)
        not lowest

Only points strictly lower than all cardinal neighbours are accepted.

---

### Risk Level Sum

Once the low points have been found, `SumOfRiskLevels()` returns:

    this.Lowest().Sum(point => this.Map[point.Y, point.X] + 1)

So each low point contributes:

- its height
- plus `1` risk

The silver answer is the sum of all of those risk values.

---

### Basin Expansion

`Basin(Vector<int> lowest, HashSet<Vector<int>> basin)` recursively builds a basin starting from a low point.

It only continues expanding when:

- the point is not already in the basin
- the point height is not `9`

If both conditions pass:

- add the point to the basin
- inspect each cardinal neighbour
- recursively call `Basin(...)` for that neighbour
- merge any returned points into the same basin set

So the implementation performs a recursive flood-fill over all connected non-`9` cells.

---

### Building All Basins

`Basins()` generates every basin with:

    this.Lowest().Select(x => this.Basin(x, new())).ToHashSet()

That means:

- find every low point
- start a fresh empty set for each one
- grow a basin from that low point
- collect all resulting basins

Each basin is represented as:

- `HashSet<Vector<int>>`

So duplicate points within a basin are naturally prevented.

---

### Final Basin Product

`SumOfBasin()` orders the basins by size and takes the top three.

It loops through:

    this.Basins().OrderByDescending(x => x.Count).Take(3)

Then multiplies the basin sizes together.

The multiplication logic starts from `0` and uses:

    total = total == 0 ? basin.Count : total * basin.Count;

So the gold answer is the product of the sizes of the three largest basins.

---

## 🛠 Implementation Notes

- `Day9.cs` uses `SumOfRiskLevels()` for silver and `SumOfBasin()` for gold
- The height map is stored in `VectorArray<int, int>`
- Low points are found using only cardinal neighbours
- A point is low only when every adjacent height is strictly greater
- Basin expansion stops at height `9`
- Basins are built recursively with `Basin(...)`
- Basin points are stored in `HashSet<Vector<int>>`
- Part 2 sorts basin sizes descending and multiplies the top three

---

## 🧪 Behaviour Summary

Given a grid of height digits:

- the solver parses the input into a 2D map
- it scans every point to find low points
- Part 1 sums each low point's height plus one
- Part 2 starts a recursive basin search from each low point
- basin growth spreads through connected cardinal neighbours
- cells with height `9` act as basin boundaries
- the final gold result is the product of the three largest basin sizes

---

## 🚀 Key Takeaways

- Good example of grid-based neighbour analysis
- Part 1 is a clean low-point detection pass over the map
- Part 2 uses recursive flood-fill style expansion
- `HashSet` is used to avoid revisiting basin cells
- The same low-point logic drives both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2021/day/9