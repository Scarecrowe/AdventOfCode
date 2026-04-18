# 🎄 Advent of Code 2018 - Day 06: Chronal Coordinates

## 📜 Puzzle Overview

This puzzle works with a set of 2D coordinates on a grid.

Each input line defines a single point:

    1, 1

The solver parses all coordinates and analyses the surrounding grid using **Manhattan distance**.

Part 1 finds the size of the largest finite area owned by a coordinate.

Part 2 finds the size of the region containing all locations whose total distance to every coordinate is below a fixed threshold.

---

## 🧩 Part 1

Determine the size of the largest area that is not infinite.

### 💡 Approach

- Parse each input line into an `(x, y)` coordinate
- Determine the bounding box that contains all coordinates
- For every point inside that box:
  - calculate its Manhattan distance to every coordinate
  - find the nearest coordinate if there is no tie
- Count how many grid points belong to each coordinate
- Exclude coordinates whose areas reach the bounding box edge
- Return the largest remaining area

---

## 🧩 Part 2

Determine the size of the safe region.

### 💡 Approach

- Reuse the same grid bounds
- For every point in the search area:
  - calculate the Manhattan distance to every input coordinate
  - sum all distances
- Count how many points have a total distance below the puzzle threshold
- Return that count

---

## 🧠 Code Breakdown

### `Day06.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Chronal Coordinates`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- parses the coordinate list
- calculates the largest finite area

For Part 2:

- reuses the parsed coordinates
- calculates the size of the safe region

---

### Coordinate Parsing

Each input row contains an X and Y value separated by a comma.

For example:

    1, 1

This becomes a coordinate object or tuple representing:

- `X = 1`
- `Y = 1`

The full input becomes a list of coordinates used by both puzzle parts.

---

### Manhattan Distance

Distance is measured using Manhattan distance.

For two points:

    (x1, y1)
    (x2, y2)

the distance is logically:

    abs(x1 - x2) + abs(y1 - y2)

This distance metric is used throughout both parts.

---

### Bounding Box

Before scanning the grid, the solver determines the smallest rectangle that contains all input coordinates.

This means finding:

- minimum X
- maximum X
- minimum Y
- maximum Y

That bounding area is then used as the main search space for the puzzle.

---

### Assigning Grid Points to Coordinates

For Part 1, every point in the bounding box is checked against every coordinate.

At each grid position the solver:

- calculates the Manhattan distance to all coordinates
- finds the smallest distance
- checks whether only one coordinate owns that minimum

If a point has a unique nearest coordinate:

- that coordinate's area count is increased

If two or more coordinates tie:

- that point is ignored

---

### Detecting Infinite Areas

Any coordinate that owns points on the outer boundary of the scanned box must have an infinite area.

So during or after the scan, the solver identifies coordinates that appear on:

- the left edge
- the right edge
- the top edge
- the bottom edge

Those coordinates are excluded from the final Part 1 area comparison.

---

### Part 1 Return Value

After removing infinite areas, the solver returns:

- the largest remaining area count

So the silver answer is the biggest finite region owned by a single coordinate.

---

### Safe Region Calculation

For Part 2, the solver does not care which coordinate is closest.

Instead, for each point in the search area it:

- calculates the distance to every coordinate
- sums all of those distances
- checks whether the total is below the required threshold

If it is below the threshold:

- that point is part of the safe region

---

### Part 2 Return Value

The gold result is:

- the number of grid points whose total distance to all coordinates is below the limit

For the real puzzle input, that threshold is:

    10000

So Part 2 returns the size of the region where the combined Manhattan distance stays under that value.

---

## 🛠 Implementation Notes

- Input is a list of 2D coordinates
- Manhattan distance drives both puzzle parts
- A bounding box limits the main search space
- Tied nearest coordinates do not count toward any area
- Edge-owned regions are treated as infinite
- Part 2 sums distances instead of assigning ownership
- Both parts operate over the same coordinate set

---

## 🧪 Behaviour Summary

Given a set of coordinate points:

- the solver parses all coordinates from input
- scans the surrounding grid
- Part 1 assigns each point to its nearest coordinate when the result is unique
- coordinates touching the outer boundary are excluded as infinite
- the largest finite area is returned
- Part 2 sums distances from each grid point to all coordinates
- points below the distance threshold form the safe region
- the size of that region is returned

---

## 🚀 Key Takeaways

- Strong example of using Manhattan distance on a 2D grid
- Bounding boxes help reduce an effectively infinite search space
- Infinite regions can be detected from boundary ownership
- Tie handling is essential for correct Part 1 results
- Part 2 reuses the same grid concept with a different scoring rule

---

## 🔗 References

- https://adventofcode.com/2018/day/6