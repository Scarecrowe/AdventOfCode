# 🎄 Advent of Code 2023 - Day 21: Step Counter

## 📜 Puzzle Overview

This puzzle is about walking across a garden grid in cardinal directions.

The map contains:

- open garden plots
- rocks
- a single starting position marked with `S`

From the start, you can move:

- up
- down
- left
- right

You cannot move into rocks.

The goal is to count how many positions can be reached after a fixed number of steps.

In Part 1 this is done on the original bounded map.

In Part 2 the map is treated as repeating infinitely in every direction, and the solver uses a pattern-based extrapolation to avoid simulating all `26501365` steps directly.

---

## 🧩 Part 1

Count how many garden plots can be reached in exactly `64` steps.

### 💡 Approach

- Parse the input into a grid
- Locate the starting tile `S`
- Track the current frontier of reachable positions
- For each step:
  - expand to the four cardinal neighbours
  - discard moves that leave the map
  - discard moves that hit rocks
- After `64` iterations, return the number of positions in the frontier

This means the solver keeps only the positions reachable at the current exact step count.

---

## 🧩 Part 2

Count how many positions can be reached in exactly `26501365` steps on an infinitely repeating version of the map.

### 💡 Approach

- Treat the original grid as a repeating tile pattern
- Allow movement beyond the original map bounds
- When checking whether a position is blocked:
  - wrap its coordinates back into the original map using modulo
- Count reachable positions for three carefully chosen step values
- Use those three results to fit a quadratic
- Evaluate that quadratic for the full target step count

This avoids simulating the full enormous walk directly.

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Step Counter`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new StepCounter(this.Input)`
- Calls `ShortWalk()`

For Part 2:

- Creates `new StepCounter(this.Input)`
- Calls `LongWalk()`

---

### `StepCounter.cs`

This class contains all of the puzzle logic.

It stores:

- `Map`
- `Rows`
- `Columns`
- `Start`

Where:

- `Map` is the input grid
- `Rows` is the number of input rows
- `Columns` is the width of a row
- `Start` is the position of `S`

The constructor:

- stores the input grid
- records the grid dimensions
- finds the starting coordinate using `FindStart('S', input)`

---

### Finding the Start Position

`FindStart(char target, string[] grid)` scans each row until it finds the target character.

At a high level it does:

- loop through each row
- call `IndexOf(target)`
- if found, return:
  - row index
  - column index

If the character is never found, it throws an exception.

So the starting point is discovered once during construction and reused by both puzzle parts.

---

### Part 1 Walk Logic

`ShortWalk()` performs a fixed-step frontier expansion.

It sets:

- `steps = 64`

It also creates:

- `HashSet<(int r, int c)> frontier`

initialised with the start coordinate.

Direction arrays are used for the four cardinal moves:

- `dr = { -1, 1, 0, 0 }`
- `dc = { 0, 0, -1, 1 }`

Then for each step:

- create a new empty `next` set
- expand every current frontier position
- test all four neighbour coordinates
- keep only neighbours that:
  - remain inside the map
  - are not `#`

After processing one layer, the frontier becomes `next`.

At the end, the method returns:

- `frontier.Count`

So the silver answer is the number of distinct positions reachable in exactly 64 steps.

---

### Why a `HashSet` is Used

The frontier is stored as a `HashSet<(int r, int c)>`.

That means:

- duplicate positions are automatically ignored
- a position reached by multiple paths is only counted once
- each step layer contains only unique reachable coordinates

This keeps the search simple and ensures the final count is based on distinct locations, not number of paths.

---

### Part 2 Infinite Walk Logic

`LongWalk()` solves the huge-step version.

It starts by computing:

- `baseOffset = this.Rows / 2`
- `N = 26501365`
- `k = (N - baseOffset) / this.Rows`

It then chooses three sample step counts:

- `s0 = baseOffset`
- `s1 = baseOffset + this.Rows`
- `s2 = baseOffset + 2 * this.Rows`

For each of those values, it calls:

- `CountReach(...)`

to get:

- `f0`
- `f1`
- `f2`

These three results are then used to build a quadratic of the form:

    a * k * k + b * k + c

Finally, the method returns that quadratic evaluated at `k`.

---

### Why the Quadratic Works

The implementation assumes that once the repeating-map growth settles into its pattern, the number of reachable cells at these aligned step intervals follows a quadratic progression.

So instead of calculating the huge target directly, it calculates three aligned sample values and derives:

- `a`
- `b`
- `c`

using:

    a = (f2 - 2 * f1 + f0) / 2
    b = f1 - f0 - a
    c = f0

Then the final result is:

    a * k * k + b * k + c

This is the key optimisation that makes Part 2 practical.

---

### `CountReach(...)`

This helper performs the actual step expansion for the repeating infinite map.

It takes:

- the grid
- height and width
- start row and column
- number of steps

It begins with:

- `current = { (sr, sc) }`

Then for each step:

- create a new empty `next` set
- expand in four cardinal directions
- allow coordinates to move beyond the original grid bounds
- wrap each candidate coordinate back into the base tile using modulo

The wrapped coordinates are calculated as:

    rr = ((nr % H) + H) % H
    cc = ((nc % W) + W) % W

This lets the solver test whether the repeated-map tile at that position is a rock.

If the wrapped map location is not `#`, the unbounded coordinate is kept.

At the end, the method returns:

- `current.Count`

So Part 2 tracks positions in infinite space while using wrapped coordinates only for terrain lookup.

---

### Important Difference Between Part 1 and Part 2

Part 1 uses bounded coordinates:

- positions outside the map are rejected immediately

Part 2 uses unbounded coordinates:

- positions may extend infinitely in any direction
- the original map is treated as a repeating pattern
- modulo is used only to determine whether the repeated tile is blocked

This is why Part 1 can use a simple bounded walk, while Part 2 needs special repeated-map logic.

---

## 🛠 Implementation Notes

- The map is stored directly as `string[]`
- The start position is found once in the constructor
- Part 1 uses exact-step frontier expansion for `64` steps
- Part 1 rejects out-of-bounds positions
- Part 2 treats the map as infinitely tiled
- Part 2 uses modulo wrapping to check repeated terrain
- Reachable positions are always stored in hash sets to preserve uniqueness
- Part 2 samples three aligned step counts and extrapolates with a quadratic
- The final gold answer is returned as `long`

---

## 🧪 Behaviour Summary

Given a garden map:

- the solver finds the start tile `S`
- each step expands to the four cardinal neighbours
- rocks are never entered
- Part 1 stays within the original map bounds
- Part 2 allows movement across an infinite repeating grid
- the repeated map is checked by wrapping coordinates back into the original tile
- silver returns the number of reachable positions after 64 steps
- gold returns the number of reachable positions after 26501365 steps using sampled counts plus quadratic extrapolation

---

## 🚀 Key Takeaways

- Good example of layered frontier expansion using sets
- Distinct reachable positions matter more than number of paths
- Part 1 is a straightforward bounded breadth-style expansion
- Part 2 reuses the same movement idea on an infinite tiled map
- Modulo wrapping is used cleanly to map infinite coordinates back to the original grid
- Quadratic extrapolation avoids an impossible full simulation for the huge target step count

---

## 🔗 References

- https://adventofcode.com/2023/day/21