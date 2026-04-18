# 🎄 Advent of Code 2024 - Day 8: Resonant Collinearity

## 📜 Puzzle Overview

This puzzle works with a 2D map containing antennas identified by frequency characters.

The grid contains:

- empty cells `.`
- antenna markers such as `A`, `0`, and other non-dot characters

The solver:

- loads the input into a `Map`
- groups antenna coordinates by character
- processes every pair of antennas with the same frequency
- generates antinode positions from those pairs

Part 1 finds the immediate antinode locations for each matching antenna pair.

Part 2 uses a resonance mode that continues projecting antinodes repeatedly along the same pattern until positions leave the map.

---

## 🧩 Part 1

Determine how many unique antinode positions exist within the map.

### 💡 Approach

- Parse the input into a 2D map structure
- Collect all antennas into groups by frequency character
- For each antenna group:
  - compare every antenna with every other antenna in that same group
  - generate antinode positions from the pair
- Keep only positions that stay inside the map
- Store results in a set so duplicates are only counted once
- Return the final unique antinode count

---

## 🧩 Part 2

Determine how many unique antinode positions exist when resonance is enabled.

### 💡 Approach

- Reuse the same grouped antenna data
- For each same-frequency antenna pair:
  - calculate the path between the antennas
  - derive movement deltas from that path
  - keep stepping outward using those deltas
- Continue adding positions while they remain inside the map
- Use a set to avoid duplicate counting

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Resonant Collinearity`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ResonantCollinearity(this.Input)`
- Calls `AntiNodes()`
- Returns `.Count`

For Part 2:

- Creates `new ResonantCollinearity(this.Input)`
- Calls `AntiNodes(true)`
- Returns `.Count`

---

### `ResonantCollinearity.cs`

This class contains the full map parsing and antinode generation logic.

It stores:

- `Map`
- `Antennas`

`Map` is created as:

    new(input, c => c)

So the raw puzzle input is loaded directly into a `VectorArray<int, char>` grid.

`Antennas` is then built by calling:

- `GetAntennas()`

---

### Antenna Collection

`GetAntennas()` scans the entire map and groups all non-dot cells by their character.

It builds:

- `Dictionary<char, HashSet<Vector<int>>>`

At a high level it does:

- iterate through every map cell
- skip cells whose value is `.`
- create a set for the frequency character if needed
- add that cell position to the frequency's set

So all antennas of the same frequency are stored together for later pair comparisons.

---

### Path Between Two Antennas

`PathToAntenna(Vector<int> start, Vector<int> end)` creates a list of points from one antenna to another.

It:

- starts from the first antenna position
- moves X one step at a time until the X coordinate matches
- then moves Y one step at a time until the Y coordinate matches
- adds each intermediate point into a list

So this method produces a step-by-step path from one antenna position to the other.

---

### Deriving Node Positions

`PathToNode(List<Vector<int>> path)` takes that path and converts it into a new projected path.

For each adjacent pair of points it:

- computes `delta = start - end`
- adds that delta onto a running `current` position
- records the new position

The solver then uses the last position from that generated sequence as the antinode candidate for that path direction.

---

### Part 1 Antinode Logic

`UniqueNodes(...)` handles the non-resonant mode used in Part 1.

For a pair of antenna positions it:

- builds a path from `pointA` to `pointB`
- computes the projected node path
- takes the last projected point as a candidate antinode
- checks that the point is inside the map
- adds it to the result set if it is valid

Then it reverses the path and repeats the same process so the opposite side is also tested.

This means each antenna pair can contribute up to two antinode positions, one in each direction, provided they remain within bounds.

---

### Resonant Mode

`Resonate(...)` handles Part 2.

It:

- builds the path between two antennas
- converts that path into step deltas using `PathDeltas(...)`
- starts from `pointB`
- repeatedly applies every delta in sequence
- adds each resulting position while it stays inside the map
- stops when the position moves outside the map

This extends the antinode generation beyond a single projected position and keeps going until the pattern leaves the grid.

---

### Processing All Antenna Pairs

`AntiNodes(bool resonate = false)` drives the whole solution.

It creates:

- `HashSet<Vector<int>> result = new();`

Then for every antenna frequency group it:

- loops over every `pointA`
- loops over every `pointB`
- skips when `pointA == pointB`
- calls either:
  - `UniqueNodes(...)` for Part 1
  - `Resonate(...)` for Part 2

Because the result is a `HashSet`, duplicate antinode coordinates are automatically collapsed into a single unique position.

---

### Part 1 Return Value

When called as:

    AntiNodes()

the method returns:

- a `HashSet<Vector<int>>` containing all unique in-range antinode positions generated by same-frequency antenna pairs using the one-step projection logic

`Day8.cs` then returns:

    AntiNodes().Count

as the silver answer.

---

### Part 2 Return Value

When called as:

    AntiNodes(true)

the method returns:

- a `HashSet<Vector<int>>` containing all unique in-range resonant antinode positions generated by repeatedly extending the pair pattern outward

`Day8.cs` then returns:

    AntiNodes(true).Count

as the gold answer. 

---

## 🛠 Implementation Notes

- The map is stored as a `VectorArray<int, char>`
- Antennas are grouped by frequency character in a dictionary
- Only non-dot cells are treated as antennas
- Every ordered pair of antennas in the same frequency group is processed
- Part 1 uses `UniqueNodes(...)`
- Part 2 uses `Resonate(...)`
- Results are stored in a `HashSet<Vector<int>>` to guarantee uniqueness
- Bounds checking is done with `this.Map.IsVectorInRange(...)`

---

## 🧪 Behaviour Summary

Given a grid of antennas:

- the solver loads the input into a 2D map
- scans all non-empty cells to group antennas by frequency
- compares every antenna with every other antenna of the same type
- Part 1 projects immediate antinode positions in both directions
- Part 2 repeatedly extends the pattern outward until leaving the map
- the final answer is the number of unique antinode coordinates collected

---

## 🚀 Key Takeaways

- Good example of grouping coordinates by symbol before pairwise processing
- The implementation uses reusable helper methods to build paths and derive projections
- Part 1 and Part 2 share the same outer pair-iteration logic
- Resonance mode is implemented as repeated delta application until out of bounds
- A `HashSet` keeps the final counting logic simple by removing duplicates automatically

---

## 🔗 References

- https://adventofcode.com/2024/day/8
