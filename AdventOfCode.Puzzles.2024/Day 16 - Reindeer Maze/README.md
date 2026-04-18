# 🎄 Advent of Code 2024 - Day 16: Reindeer Maze

## 📜 Puzzle Overview

This puzzle navigates a reindeer through a maze from:

- `S` = start
- `E` = end
- `#` = wall
- `.` = open space

The reindeer begins facing East.

Movement has two different costs:

- moving forward one tile costs `1`
- turning 90 degrees costs `1000`

The solver treats this as a weighted pathfinding problem where both position and facing direction matter.

---

## 🧩 Part 1

Determine the lowest possible score to reach the end tile.

### 💡 Approach

- Parse the maze into a 2D map
- Find the `S` start position
- Find the `E` finish position
- Use a priority queue to explore states in increasing score order
- Treat each state as:
  - current position
  - current facing direction
  - accumulated path score
- From each state, inspect the adjacent cardinal cells
- If moving in the same direction:
  - cost increases by `1`
- If turning left or right and then moving:
  - cost increases by `1001`
- Track visited states using both:
  - position
  - facing direction
- Return the smallest score that reaches the end

---

## 🧩 Part 2

Count the tiles that are part of the best paths.

### 💡 Approach

- Reuse the same weighted-search structure from Part 1
- Track the points visited by candidate paths
- Attempt to identify all tiles that belong to optimal routes
- Return the number of collected seat tiles

### ⚠ Current Implementation Note

In the committed code, `Seats()` creates:

- `var shortestPath = this.ShortestPath()`
- `HashSet<Vector<int>> seats = []`

It then performs a second priority-queue traversal and prints the maze with any seat tiles marked as `O`.

However, the shown implementation never adds any points into `seats`, so as committed it would return:

    seats.Count

with no populated result set.

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Reindeer Maze`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ReindeerMaze(this.Input)`
- Calls `BestScore()`

For Part 2:

- Creates `new ReindeerMaze(this.Input)`
- Calls `Seats()`

---

### `ReindeerMaze.cs`

This class contains the maze parsing and pathfinding logic.

It stores:

- `Map`
- `Reindeer`
- `Finish`

The constructor:

- builds the map using `VectorArray<int, char>`
- scans every cell in the maze
- records the start point when it finds `S`
- records the finish point when it finds `E`

---

### Maze Parsing

The maze is parsed directly from the input text grid.

At a high level it does:

- read each line into a 2D map
- enumerate all coordinates
- locate:
  - the start tile
  - the end tile

This gives the solver the full maze layout plus the two important anchor points.

---

### Why Direction Matters

This puzzle does not just ask for the shortest number of steps.

Because turning is extremely expensive, the solver must consider:

- where the reindeer is
- which way it is facing

That means the state key is not just:

    position

but instead:

    (position, direction)

This is why visited tracking uses tuples of point and facing direction.

---

### Priority Queue Search

Both `BestScore()` and `Seats()` use a priority queue of states shaped like:

- `Point`
- `Direction`
- `Path`

The queue is ordered by total score so that cheaper states are explored first.

The initial queue is seeded with three starting options:

- facing East with score `1`
- facing clockwise from East with score `1000`
- facing anticlockwise from East with score `1000`

This mirrors the implementation exactly, even though the intended puzzle model is "start facing East".

---

### Expanding a State

For each dequeued state, the solver checks adjacent cardinal cells.

If the next cell is open (`.`), it clones the current path and applies one of three scoring rules:

- same direction:
  - add `1`
- clockwise turn then move:
  - add `1001`
- anticlockwise turn then move:
  - add `1001`

Each valid move is then pushed back into the priority queue.

This means the solver combines turning and stepping into one transition when changing direction.

---

### Reaching the Finish

When an adjacent cell is `E`, the solver updates the path score by `1`.

In `ShortestPath()`:

- the first finish score found becomes the current best
- later finishes replace it only if their score is lower

At the end, the method returns the `ReindeerPath` with the best score found.

---

### `ReindeerPath.cs`

This class stores the state of a candidate path.

It contains:

- `Points`
- `Visited`
- `Score`

`Points` stores:

- a point
- the score at which that point was reached

The constructor initialises empty collections and stores the starting score.

`Clone()` creates a new `ReindeerPath` with the same score and copies the recorded points.

---

### Part 1 Return Value

`BestScore()` simply returns:

- `this.ShortestPath().Score`

So the silver answer is the minimum weighted route score through the maze.

---

### Part 2 Current Behaviour

`Seats()` appears intended to identify all tiles involved in the best-scoring solutions.

It does:

- call `ShortestPath()`
- perform another queue-driven traversal
- build a printable map overlay using `O` for seat tiles
- return `seats.Count`

But in the current committed code:

- `shortestPath` is retrieved and not used afterward
- `seats` is checked during printing
- `seats` is never populated

So the gold solution structure is present, but the seat-collection logic is incomplete in the version currently in the repository.

---

## 🛠 Implementation Notes

- The maze is stored in a `VectorArray<int, char>`
- Start and finish are located by scanning the map
- Search state includes both position and facing direction
- The solver uses a priority queue for weighted pathfinding
- Forward moves cost `1`
- Turning and moving costs `1001`
- Paths are cloned as the search branches
- `Seats()` currently appears incomplete in the committed implementation

---

## 🧪 Behaviour Summary

Given a maze with walls, a start tile, and an end tile:

- the solver parses the map
- tracks the reindeer's position and facing
- explores possible moves using weighted search
- prefers low-score states first
- Part 1 returns the best route score
- Part 2 appears intended to count best-path tiles, but the committed implementation does not yet populate that result set

---

## 🚀 Key Takeaways

- Good example of weighted pathfinding where orientation matters
- Position alone is not enough for visited-state tracking
- Priority queues work well for turn-cost navigation problems
- Path cloning allows score and route history to move together
- Part 2's structure is present, but the seat-aggregation logic still looks unfinished

---

## 🔗 References

- https://adventofcode.com/2024/day/16
