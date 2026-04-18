# 🎄 Advent of Code 2019 - Day 19: Tractor Beam

## 📜 Puzzle Overview

This puzzle uses the Intcode computer to probe a tractor beam.

For any coordinate `(x, y)`, the program returns whether that point is:

- stationary
- pulled by the beam

In Part 1, the solver scans a fixed area and counts how many points are affected.

In Part 2, it searches a much larger scanned region for the closest location where a `100 x 100` square fits entirely within the beam.

---

## 🧩 Part 1

Count how many points in the scanned area are affected by the tractor beam.

### 💡 Approach

- Run the Intcode program for every coordinate in the target scan range
- For each point:
  - reset the CPU
  - provide `x`
  - provide `y`
  - run the program
  - store whether the point is pulled or stationary
- After the map is built, count how many points are marked as pulled

---

## 🧩 Part 2

Find the closest point where a `100 x 100` square fits entirely inside the beam.

### 💡 Approach

- Build a much larger beam map first
- Iterate through each scanned row
- Only consider rows where at least 100 pulled points exist
- For each candidate starting `x` position in that row:
  - count how many pulled points exist vertically over 100 rows
  - if exactly 100 are found in that vertical slice
  - verify:
    - top-right corner is pulled
    - bottom-left corner is pulled
- Return:

    (x * 10000) + y

for the first matching square

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Tractor Beam`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TractorBeam(this.Input[0])`
- Calls `BuildMap(50, 0, 50)`
- Returns `TractorBeamArea`

For Part 2:

- Creates `new TractorBeam(this.Input[0])`
- Calls `BuildMap(1000, 700, 1200)`
- Calls `ClosestPoint()`

---

### `Entity.cs`

This file defines the beam state enum.

It contains:

- `Stationary = 0`
- `Pulled = 1`

These values match the Intcode output returned for each probed coordinate.

---

### `TractorBeam.cs`

This class contains the full tractor beam scanning logic.

It stores:

- `Cpu`
- `Map`

It also exposes:

- `TractorBeamArea`

The constructor:

- creates a new `IntcodeCpu`
- creates a new coordinate map

So the beam is represented as a dictionary of scanned points and their beam state.

---

### Program Setup

The constructor takes the Intcode program as a single string:

    new TractorBeam(this.Input[0])

It then initialises:

    this.Cpu = new(program);
    this.Map = new();

So the puzzle input is expected to be one comma-separated Intcode program line.

---

### Building the Beam Map

`BuildMap(int size, int min, int max)` scans a rectangular section of the grid.

It loops like this:

- `y` from `min` to `max - 1`
- `x` from `0` to `size - 1`

For every coordinate it:

- resets the CPU
- enqueues `x`
- enqueues `y`
- runs the Intcode program
- reads the output
- stores the result in the map

At a high level the logic is:

    for each y
        for each x
            reset cpu
            input x
            input y
            run cpu
            map[(x, y)] = output

This means every point is probed independently using a fresh Intcode execution.

---

### Beam Area Count

`TractorBeamArea` returns:

    this.Map.Count(c => c.Value == Entity.Pulled)

So the silver answer is simply the number of scanned coordinates that ended up inside the tractor beam.

---

### Map Storage

The scanned data is stored in:

- `VectorDictionary Map`

Each coordinate maps to one of:

- `Entity.Stationary`
- `Entity.Pulled`

This lets later methods query beam status directly by coordinate.

---

### Printing the Map

`PrintMap()` renders the scanned region to the console.

It:

- calculates the min and max map bounds
- maps entities to display characters
- prints each row from top to bottom

The display mapping is:

- stationary → `.`
- pulled → `#`

If a coordinate is not present in the map, it is also printed as `#`.

This appears to be a debugging or visualisation helper rather than part of the final answer logic.

---

### Searching for the Closest Square

`ClosestPoint()` performs the Part 2 search.

It begins by calculating the current map bounds from the scanned data.

Then for each row `y` it counts how many points in that row are:

- `Entity.Pulled`

Only rows with at least `100` pulled points are considered as candidates.

---

### Candidate Row Processing

For a candidate row, the solver finds:

- the first pulled `x`
- the last pulled `x`

It then checks every possible starting `x` between those bounds.

For each candidate `x`, it performs a vertical scan from:

- `y`
- to `y + 100`

and sums the beam values found at coordinate:

    (x, yy)

Because `Pulled = 1` and `Stationary = 0`, this count effectively measures how many of those 100 positions are inside the beam.

---

### Square Validation

If the vertical count is exactly:

    100

the solver then checks two critical corners:

- `this.Map[new(x + 99, y)]`
- `this.Map[new(x, y + 99)]`

If both are `Entity.Pulled`, the square is considered valid.

At that point it returns:

    (x * 10000) + y

So the gold answer is based on the top-left coordinate of the first valid `100 x 100` square found in the scanned map.

---

### Highest Count Tracking

Inside `ClosestPoint()` the code also tracks:

    int highest = 0;

This is updated whenever a larger vertical count is found.

However, that value is not used in the return logic, so it appears to be diagnostic or leftover tracking rather than part of the final puzzle solution.

---

## 🛠 Implementation Notes

- The puzzle input is stored as a single Intcode program string
- Each coordinate probe uses a full CPU reset before execution
- The map is built by scanning a rectangular region of `(x, y)` points
- Part 1 scans `x = 0..49` and `y = 0..49`
- Part 2 scans a much larger preselected area with:
  - width `1000`
  - rows `700..1199`
- Beam state is stored as an `Entity` enum
- Part 2 searches for a `100 x 100` fit by checking a vertical run and two square corners
- `highest` is tracked during Part 2 but is not required for the returned result

---

## 🧪 Behaviour Summary

Given one Intcode beam program:

- the solver probes coordinates one at a time
- each probe returns whether the point is stationary or pulled
- Part 1 counts how many scanned points lie inside the beam
- Part 2 searches a larger scanned region for where a `100 x 100` square fits
- the final result is either:
  - the number of pulled points in the small scan
  - or the encoded coordinate of the closest fitting square

---

## 🚀 Key Takeaways

- Good example of using the Intcode computer as a coordinate query engine
- Each point is evaluated independently by resetting and rerunning the CPU
- Part 1 is a straightforward scan-and-count problem
- Part 2 relies on building a large enough map first, then searching for a valid square
- The implementation uses direct coordinate checks rather than deriving the beam mathematically

---

## 🔗 References

- https://adventofcode.com/2019/day/19