# 🎄 Advent of Code 2022 - Day 12: Hill Climbing Algorithm

## 📜 Puzzle Overview

This puzzle works on a 2D height map where each position is represented by a letter:

- `a` is the lowest elevation
- `z` is the highest elevation
- `S` marks the start
- `E` marks the end

The goal is to find the fewest number of steps needed to reach the end while only moving up by at most one elevation level at a time.

The solver parses the input into a `VectorArray` map, tracks one or more starting positions, and then uses breadth-first search to find the shortest valid route to the finish.

Part 1 starts only from `S`. Part 2 starts from every valid lowest-elevation position as well.

---

## 🧩 Part 1

Determine the fewest number of steps required to reach the end starting from `S`.

### 💡 Approach

- Parse the map input into a grid
- Locate:
  - the start position `S`
  - the end position `E`
- Convert:
  - `S` to elevation `a`
  - `E` to elevation `z`
- Use breadth-first search from the start
- Only move to adjacent cells when the target elevation is at most:

      current elevation + 1

- Return the shortest path length to the finish

---

## 🧩 Part 2

Determine the fewest number of steps required to reach the end starting from any lowest-elevation position.

### 💡 Approach

- Reuse the same map parsing logic
- Collect all valid starting positions with elevation `a`
- Run the same breadth-first search from each start
- Record every successful route length
- Return the minimum number of moves found

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Hill Climbing Algorithm`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new HillClimbingAlgorithm(this.Input)`
- Calls `Fewest()`

For Part 2:

- Creates `new HillClimbingAlgorithm(this.Input, false)`
- Calls `Fewest()`

The second parameter changes whether only `S` is used as a start or whether all `a` positions are included as well.

---

### `HillClimbingAlgorithm.cs`

This class contains the full pathfinding logic.

It stores:

- `Map`
- `Positions`
- `Finish`
- `Visited`
- `Moves`

The constructor:

- initialises the collections
- sets a default finish position
- parses the map
- creates a visited grid sized to match the map

---

### Map Parsing

`Parse(string[] input, bool isStartOnly)` converts the input into a `VectorArray`.

For each character in the input:

- if the value is `S`
  - add that coordinate to `Positions`
  - store it as `a`
- if the value is `E`
  - store that coordinate as `Finish`
  - store it as `z`
- if Part 2 mode is enabled and the value is `a`
  - add that coordinate to `Positions` as an additional possible start

So the parser both builds the elevation map and identifies all valid starting positions.

---

### Starting Positions

The solver supports two modes:

For Part 1:

- only the original `S` position is used

For Part 2:

- every `a` position is also added as a starting point

This means Part 2 effectively tries the route from every lowest-elevation tile and keeps the best result.

---

### Breadth-First Search

`Fewest()` performs the search.

For each starting position in `Positions`:

- reset the `Visited` map
- create a queue of:

      (Vector Point, int Move)

- enqueue the starting position with move count `0`

Then while the queue has items:

- dequeue the next position
- read its current elevation
- attempt to move:
  - up
  - down
  - left
  - right

Each valid move is handled through `Enqueue(...)`.

---

### Movement Rules

The solver checks four neighbouring cells.

A move is only allowed when:

- the target position has not been visited
- the target elevation is less than or equal to:

      current elevation + 1

This matches the puzzle rule that you may climb up by at most one level at a time, while descending any amount is allowed.

---

### `Enqueue(...)`

`Enqueue(Queue<(Vector Point, int Move)> queue, Vector point, int move, int elevation)` applies the move rules.

If the target cell is valid:

- mark it as visited
- if the target is the finish:
  - store `move + 1` in `Moves`
- otherwise:
  - enqueue that position with `move + 1`

This means successful route lengths are collected as soon as the finish is reached.

---

### Visited Tracking

`Visited` is a `VectorArray` matching the size of the map.

For each new starting position:

- the visited grid is recreated fresh

This ensures each breadth-first search run is independent and does not carry over state from previous starting points.

---

### Result Calculation

After all searches complete, the method returns:

    this.Moves.Min()

So the solver keeps every successful route length and returns the smallest one.

That gives:

- the shortest route from `S` in Part 1
- the shortest route from any `a` start in Part 2

---

## 🛠 Implementation Notes

- The map is parsed into a `VectorArray`
- `S` is normalised to `a`
- `E` is normalised to `z`
- Part 1 uses only the original start position
- Part 2 includes all `a` positions as possible starts
- Breadth-first search is run separately for each start
- The solver stores all successful move counts and returns the minimum

---

## 🧪 Behaviour Summary

Given a height map:

- the solver parses the grid into elevations
- it records the finish position
- it collects one or more valid starting positions
- it performs breadth-first search from each start
- moves are only allowed when climbing by at most one elevation level
- every successful route length is recorded
- the final result is the fewest number of moves needed to reach the end

---

## 🚀 Key Takeaways

- Good example of shortest-path solving with breadth-first search
- Cleanly separates parsing and movement validation
- Reuses the same search logic for both puzzle parts
- Part 2 extends naturally by broadening the set of starting points
- Visited tracking is reset per search to keep each path attempt independent

---

## 🔗 References

- https://adventofcode.com/2022/day/12