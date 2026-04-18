# 🎄 Advent of Code 2023 - Day 18: Lavaduct Lagoon

## 📜 Puzzle Overview

This puzzle follows a digging plan that traces out the boundary of a lagoon.

Each instruction tells the digger to move in a direction for a number of steps.

Directions are:

- `R` → right
- `L` → left
- `U` → up
- `D` → down

Part 1 uses the instructions exactly as written.

Part 2 decodes the real instructions from the hexadecimal colour data at the end of each line.

The goal in both parts is to determine how many tiles are contained in the final dug lagoon, including its trench boundary.

---

## 🧩 Part 1

Determine the lagoon size using the normal instruction format.

### 💡 Approach

- Parse each input line into:
  - a direction
  - a distance
- Walk the trench one step at a time
- Record every trench coordinate in a set
- Work out the bounding box around the trench
- Flood fill from outside that box to discover all exterior positions
- Count all positions inside the trench that are not reachable from outside
- Add the trench tiles themselves to get the final lagoon volume

---

## 🧩 Part 2

Determine the lagoon size using the hexadecimal instruction encoding.

### 💡 Approach

- Decode each line's colour code into:
  - distance from the first 5 hex digits
  - direction from the final hex digit
- Instead of storing every single filled tile, track row intervals
- For horizontal movement:
  - add a continuous interval on the current row
- For vertical movement:
  - add a single-point interval on each traversed row
- Merge overlapping and touching intervals
- Sum the sizes of the merged intervals across all rows

This avoids the expensive flood-fill style approach used in Part 1 and scales much better for the huge distances in Part 2.

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Lavaduct Lagoon`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new LavaductLagoon(this.Input)`
- Calls `Small()`

For Part 2:

- Creates `new LavaductLagoon(this.Input)`
- Calls `Large()`

---

### `InstructionParser.cs`

This file handles converting the raw input into instruction data.

It defines:

- `Direction`
  - `R`
  - `D`
  - `L`
  - `U`

It also provides two parsing methods:

- `ParseNormalInstructions(string[] input)`
- `ParseHexInstructions(string[] input)`

---

### Normal Instruction Parsing

`ParseNormalInstructions(...)` reads each line by splitting on spaces and parentheses.

From each line it extracts:

- the plain-text direction
- the numeric distance

So a line like:

    R 6 (#70c710)

produces:

- direction `R`
- distance `6`

The parsed results are stored as:

- `List<(Direction Dir, long Distance)>`

---

### Hex Instruction Parsing

`ParseHexInstructions(...)` reads the encoded value inside the parentheses.

It:

- finds the substring between `(` and `)`
- removes the leading `#`
- uses the first 5 hex digits as the distance
- uses the final hex digit as the direction code

Direction decoding is:

- `0` → `R`
- `1` → `D`
- `2` → `L`
- `3` → `U`

This produces the hidden instruction set used for Part 2.

---

### `LavaductLagoon.cs`

This class contains the lagoon-building and area-counting logic.

It stores:

- `smallTrench`
- `rowIntervals`
- `instructions`
- `hexInstructions`

`instructions` is parsed from the normal text instructions.

`hexInstructions` is parsed from the hexadecimal form.

---

### Small Digging Mode

`DigSmall(...)` builds the trench by walking every instruction one step at a time.

It starts at:

    (0, 0)

Then for each instruction it updates the current coordinate step-by-step and adds each new point into:

- `smallTrench`

Direction handling is:

- `R` → `x++`
- `L` → `x--`
- `U` → `y++`
- `D` → `y--`

Because every traversed point is recorded individually, this gives an exact trench outline for the smaller Part 1 grid.

---

### Large Digging Mode

`DigLarge(...)` uses a more compact representation.

Instead of storing every tile in a giant set, it stores dug spans as intervals per row in:

- `rowIntervals`

It begins by adding the starting point as an interval.

Then:

- horizontal movement adds a whole interval on the current row
- vertical movement adds a single-point interval on each row crossed

For example:

- moving right adds `AddInterval(y, x + 1, x + distance)`
- moving left adds `AddInterval(y, x - distance, x - 1)`

Vertical movement adds a point interval for each row passed through.

This makes the large-scale solution practical.

---

### Interval Storage

`AddInterval(long y, long start, long end)` stores dug row segments.

It:

- creates a list for the row if needed
- normalises the interval so `start <= end`
- appends the interval to that row's list

So each row can accumulate many separate dug segments before they are merged.

---

### Merging Intervals

`MergeIntervals(...)` combines overlapping or directly adjacent intervals.

It works by:

- sorting intervals by start position
- walking through them in order
- merging whenever the next interval starts at or before:

    current.end + 1

This means touching intervals are treated as one continuous dug span.

The result is a clean set of non-overlapping row ranges.

---

### Part 1 Volume Calculation

`ComputeSmallVolume()` uses flood fill to identify the outside of the lagoon.

It first computes a bounding box around the trench and expands it by 1 in every direction.

Then it:

- starts from the outer corner of that expanded box
- runs a queue-based flood fill
- marks all reachable non-trench positions as outside

After that, it scans the interior bounding area and counts every position that is:

- not part of `smallTrench`
- not reachable from outside

That gives the filled interior area.

Finally it returns:

- interior area
- plus `smallTrench.Count`

So the final Part 1 result includes both the dug trench and the enclosed lagoon tiles.

---

### Part 2 Volume Calculation

`ComputeLargeVolume()` sums the merged intervals for every row.

For each row:

- merge all row intervals
- for each merged interval add:

    end - start + 1

The total across all rows is returned as the final large lagoon volume.

So Part 2 avoids flood filling entirely and instead computes the filled lagoon directly from row coverage.

---

## 🛠 Implementation Notes

- Part 1 and Part 2 deliberately use different strategies
- Part 1 stores every trench point explicitly
- Part 1 uses flood fill from outside the trench boundary
- Part 2 parses directions from hexadecimal metadata
- Part 2 stores dug coverage as row intervals instead of individual tiles
- Adjacent intervals are merged as one continuous span
- Both parts start digging from coordinate `(0, 0)`

---

## 🧪 Behaviour Summary

Given a digging plan:

- the solver parses the instructions
- Part 1 follows the visible directions and distances
- the trench boundary is recorded point-by-point
- a flood fill identifies the outside region
- enclosed tiles plus trench tiles are counted

For the gold solution:

- the hidden hex instructions are decoded
- trench coverage is stored as row intervals
- overlapping intervals are merged
- all dug coverage is summed directly

---

## 🚀 Key Takeaways

- Nice example of using two completely different solving strategies for two puzzle scales
- Part 1 uses explicit simulation plus flood fill
- Part 2 switches to a compressed interval-based representation
- Hex parsing is used to reveal the real digging plan
- Interval merging makes huge coordinate ranges manageable

---

## 🔗 References

- https://adventofcode.com/2023/day/18