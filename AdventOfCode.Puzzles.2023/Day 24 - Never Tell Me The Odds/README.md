# 🎄 Advent of Code 2023 - Day 24: Never Tell Me The Odds

## 📜 Puzzle Overview

This puzzle works with a list of hailstones moving through 3D space.

Each input line contains:

- a starting position
- a velocity

A line looks like this:

    x, y, z @ vx, vy, vz

The solver parses every line into a `Hailstone` object containing:

- `Point`
- `Velocity`

Part 1 checks how many pairs of hailstones intersect within a large 2D test area when projected onto the XY plane.

Part 2 searches for a single rock throw that can intersect multiple hailstones by adjusting relative velocity and matching a shared collision point.

---

## 🧩 Part 1

Determine how many hailstone pairs intersect within the test area on the XY plane.

### 💡 Approach

- Parse every input line into hailstones
- Compare every unique hailstone pair
- For each pair:
  - calculate whether their XY paths intersect
  - reject intersections that happen in the past
  - reject intersections outside the test area bounds
- Count the valid intersections

The default test area used by the solver is:

- minimum: `200000000000000`
- maximum: `400000000000000`

---

## 🧩 Part 2

Determine the result of a single rock throw that can collide with the hailstones.

### 💡 Approach

- Reuse the parsed hailstones, but use the finite intersection logic
- Select four hailstones:
  - the first two
  - and the last two
- Search through a bounded velocity offset range for:
  - `x`
  - `y`
- For each candidate XY velocity offset:
  - test whether the chosen hailstones intersect at the same XY point
- Once a matching XY solution is found:
  - search through a bounded `z` velocity offset
  - interpolate the Z value at the collision times
- When all three Z values match:
  - return `x + y + z`

This implementation solves the gold part with a bounded search over velocity adjustments rather than using symbolic algebra.

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Never Tell Me The Odds`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new NeverTellMeTheOdds(this.Input)`
- Calls `TestAreaIntersections()`

For Part 2:

- Creates `new NeverTellMeTheOdds(this.Input, false)`
- Calls `SingleThrow()`

The second constructor call passes `false` so the hailstones use the non-infinite intersection mode for the gold solution.

---

### `NeverTellMeTheOdds.cs`

This class contains the main puzzle logic.

It stores:

- `Hailstones`

The constructor parses the input with:

- `ParseHailstones(input, infinity)`

So each puzzle mode is controlled by whether hailstones should use:

- infinite XY line intersections
- or direct finite trajectory intersections

---

### Parsing the Input

`ParseHailstones(string[] input, bool infinity)` processes each line by splitting on:

    " @ "

Then it splits each side on:

    ", "

The left side becomes the starting point:

- `X`
- `Y`
- `Z`

The right side becomes the velocity:

- `X`
- `Y`
- `Z`

Each parsed line becomes:

- `new Hailstone((pointX, pointY, pointZ), (velocityX, velocityY, velocityZ), infinity)`

So the parsed result is:

- `Hailstone[]`

---

### `Hailstone.cs`

This class models one moving hailstone.

It stores:

- `Point`
- `Velocity`
- `Infinity`

`Infinity` controls which XY intersection method is used.

So the same hailstone structure supports both:

- Part 1 infinite-line style comparisons
- Part 2 finite-path collision checks

---

### Part 1 Pair Counting

`TestAreaIntersections(...)` calls:

- `CountIntersectionsXY(this.Hailstones, min, max)`

`CountIntersectionsXY(...)` generates every unique pair of hailstones using:

- `SelectMany(...)`
- `Skip(i + 1)`

For each pair it checks:

- `p.h1.IntersectsXY(p.h2, min, max)`

The final result is the count of pairs whose XY intersection lies within the allowed bounds.

---

### XY Intersection Checks

`IntersectsXY(Hailstone hailstone, long min, long max)` first calculates the raw XY intersection, then checks that:

- `x >= min`
- `x <= max`
- `y >= min`
- `y <= max`

So the test area filter is applied only after a valid future intersection has already been found.

---

### Infinite vs Finite Intersection Modes

The private intersection dispatcher is:

    IntersectsXY(Hailstone hailstone)

It chooses between:

- `IntersectsInfinity(...)`
- `Intersects(...)`

depending on the `Infinity` flag.

This means:

- Part 1 uses `IntersectsInfinity(...)`
- Part 2 uses `Intersects(...)`

---

### Infinite XY Intersection Logic

`IntersectsInfinity(...)` treats each hailstone as an infinite line in the XY plane.

It:

- computes the slope for each hailstone
- computes the line intercept
- rejects parallel lines
- solves for the intersection `x`
- calculates time values `t1` and `t2`
- rejects intersections where either time is negative

If valid, it returns:

- `intersects`
- `x`
- `y`
- `t`

with `x` and `y` rounded to 3 decimal places.

---

### Finite XY Intersection Logic

`Intersects(...)` handles the non-infinite mode.

It explicitly deals with:

- both lines vertical
- current line vertical
- other line vertical
- neither vertical

For non-vertical cases it calculates:

- slope
- intercept
- intersection point
- time for each hailstone

Then it rejects the result if either time is negative.

So Part 2 uses a more direct trajectory-based XY intersection routine.

---

### Velocity Offsets

`VOffsetXY(long dvy, long dvx)` creates a new hailstone with modified XY velocity:

- `Velocity.X + dvx`
- `Velocity.Y + dvy`

This is used by:

- `IsIntersectionXY(Hailstone hailstone, long dvy, long dvx)`

That method offsets both hailstones by the same candidate rock velocity adjustment and then tests for an intersection.

So the gold solution works by searching for a velocity offset that makes multiple hailstones line up at the same collision point.

---

### Z Interpolation

`InterpolateZ(double t, long dvz)` calculates:

    Point.Z + t * (Velocity.Z + dvz)

and rounds the result to 3 decimal places.

This lets the solver test whether different hailstones would share the same Z position at their computed XY collision times once a candidate Z velocity offset is applied.

---

### Part 2 Search Logic

`SingleThrow()` calls:

- `SingleThrowPosition(this.Hailstones)`

`SingleThrowPosition(...)` uses four hailstones:

- `hs0`
- `hs1`
- `hs2`
- `hs3`

specifically chosen as:

- first
- second
- second-last
- last

It then searches a fixed range generated by:

    Enumerable.Range(-300, 600)

So candidate offsets are checked for:

- `y`
- `x`
- then `z`

---

### Matching the XY Collision Point

For each candidate `(y, x)` offset the solver computes:

- `hs0.IsIntersectionXY(hs1, y, x)`
- `hs0.IsIntersectionXY(hs2, y, x)`
- `hs0.IsIntersectionXY(hs3, y, x)`

It skips the candidate unless:

- all three intersections exist
- all three share the same `(y, x)` point

Only then does it continue into the Z search.

So the gold solution first finds a shared XY collision position before validating Z.

---

### Matching the Z Value

For each candidate `z` offset the solver computes:

- `hs1.InterpolateZ(i1.t, z)`
- `hs2.InterpolateZ(i2.t, z)`
- `hs3.InterpolateZ(i3.t, z)`

If all three Z values are equal, it returns:

    i1.x + i1.y + z1

cast to `long`.

So the final gold answer is the sum of the discovered collision coordinates.

If no solution is found, the solver throws:

- `new Exception("No intersection found")`

---

## 🛠 Implementation Notes

- Input is parsed into `Hailstone[]`
- Each hailstone stores position, velocity, and an `Infinity` mode flag
- Part 1 counts pairwise XY intersections inside a fixed test area
- Part 1 rejects parallel paths and past-only intersections
- Part 2 uses `new NeverTellMeTheOdds(this.Input, false)`
- Part 2 searches candidate velocity offsets from `-300` to `299`
- XY and Z values are rounded to 3 decimal places in the helper methods
- The gold solution uses a brute-force search over bounded offsets rather than a symbolic solver

---

## 🧪 Behaviour Summary

Given a list of hailstones in 3D space:

- the solver parses each position and velocity
- Part 1 projects motion onto the XY plane
- every unique hailstone pair is tested for a future intersection
- only intersections inside the large test area are counted

For Part 2:

- the solver searches for a shared rock velocity offset
- it finds an XY point where multiple hailstones align
- it then checks for a matching Z value at the corresponding times
- the final result is the sum of the discovered collision coordinates

---

## 🚀 Key Takeaways

- Good example of separating parsing, geometry, and search logic
- Part 1 is a pairwise XY line-intersection counting problem
- The implementation supports both infinite-line and finite-path intersection modes
- Part 2 uses relative velocity offsets to reduce the collision search problem
- Z is solved only after a consistent XY collision point has been found
- The gold solution is an explicit bounded search rather than a formula-based solver

---

## 🔗 References

- https://adventofcode.com/2023/day/24