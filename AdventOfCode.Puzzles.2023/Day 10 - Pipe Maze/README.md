# 🎄 Advent of Code 2023 - Day 10: Pipe Maze

## 📜 Puzzle Overview

This puzzle works with a 2D pipe map.

Each character in the input represents part of the maze, such as:

- `|`
- `-`
- `L`
- `J`
- `7`
- `F`
- `S`
- `.`

The solver loads the input into a grid and attempts to walk the pipe loop starting from `S`.

In the current implementation, both Part 1 and Part 2 call the same method:

    Move()

That method performs a breadth-first style traversal over connected pipe cells.

---

## 🧩 Part 1

Find the distance to the furthest point in the loop from the start.

### 💡 Approach

- Load the map into a 2D grid
- Find the starting tile `S`
- Look at the cardinally adjacent cells
- Enqueue any adjacent pipe that could validly connect to `S`
- Traverse through connected pipe tiles
- Track the largest step count reached during traversal

---

## 🧩 Part 2

The gold method currently calls the same traversal method as Part 1.

### 💡 Current Implementation

- Reuses exactly the same `Move()` method
- Does not yet contain separate enclosed-area logic
- Returns the same result path as the silver method would

At the moment, the code appears incomplete because `Move()` calculates a maximum distance in `result`, but actually returns `count`, which is always `0`.

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Pipe Maze`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PipeMaze(this.Input)`
- Calls `Move()`

For Part 2:

- Creates `new PipeMaze(this.Input)`
- Calls `Move()`

So both puzzle parts currently use the exact same solver path.

---

### `PipeMaze.cs`

This class contains the maze traversal logic.

The constructor:

- accepts the puzzle input
- builds `this.Map`

The map is created with:

    new(input, (value) => value)

So the input is stored as a character grid.

It keeps:

- `Map`

defined as:

    VectorArray<int, char>

---

### Map Traversal Setup

`Move()` starts by creating:

    Queue<(Vector<int> Point, Cardinal Direction, int Steps)> queue = new();

It then scans every map cell using:

    this.Map.AxisEnumerator()

When it finds the starting tile:

    S

it checks all cardinal neighbours with:

    this.Map.AdjacentCardinal(cell.Point)

Only neighbouring tiles that could connect properly to `S` are enqueued.

---

### Start Connection Rules

The method checks each adjacent tile and only accepts it if its shape is compatible with the direction from `S`.

Examples:

- `|` is accepted when the neighbour is north or south
- `-` is accepted when the neighbour is west or east
- `7` is accepted when the neighbour is west or south
- `L` is accepted when the neighbour is east or north
- `J` is accepted when the neighbour is west or north
- `F` is accepted when the neighbour is east or south

For each valid neighbour it enqueues:

    (cell.Point, adjacent.Direction, 0)

So traversal begins from the start position with step count `0`.

---

### Visited Tracking

After seeding the queue, the method creates:

    HashSet<Vector<int>> visited = new();

This prevents the same map location from being processed repeatedly.

It also creates:

    long result = 0;

which is used to track the highest step count seen during traversal.

---

### Main Traversal Loop

The solver then runs while the queue still contains items.

For each dequeued item it gets:

- `Point`
- `Direction`
- `Steps`

If the point has already been visited, it skips it.

Otherwise it:

- adds the point to `visited`
- compares `current.Steps` against `result`
- updates `result` when a larger distance is found

So this behaves like a breadth-first style search over the connected pipe system.

---

### Exploring Adjacent Pipes

For each current point, the solver again checks:

    this.Map.AdjacentCardinal(current.Point)

It skips any ground tile:

    .

Then it conditionally enqueues adjacent pipe tiles if the pipe shape is compatible with the movement direction.

The implemented checks include:

- `|`
- `-`
- `7`
- `L`
- `J`
- `F`
- `S`

Each accepted move is enqueued with:

    current.Steps + 1

So the solver is effectively trying to follow valid pipe connections through the map.

---

### Direction Compatibility Rules

The traversal logic uses direction-based checks for each pipe type.

Examples from the current logic:

- `|` allows north or south
- `-` allows west or east
- `7` allows east or north
- `L` allows west or south
- `J` allows east or south
- `F` and `S` are accepted when the adjacent direction is west or north

These checks are hard-coded directly inside `Move()` rather than abstracted into helper methods.

---

### Return Value

At the end of the method, the code does this:

    int count = 0;
    return count;

So even though it tracks the furthest step count in:

    result

that value is never returned.

This means the current implementation returns `0` for both silver and gold.

---

## 🛠 Implementation Notes

- `Day10.cs` calls `Move()` for both Part 1 and Part 2
- The map is stored as `VectorArray<int, char>`
- Traversal begins by locating `S`
- A queue is used to perform breadth-first style exploration
- A `HashSet` tracks visited positions
- Pipe connection rules are hard-coded with directional checks
- The method calculates a maximum step count in `result`
- The current code returns `count`, which is always `0`
- There is no separate enclosed-area logic for Part 2 yet

---

## 🧪 Behaviour Summary

Given a grid of pipe characters:

- the solver loads the grid into a map structure
- finds the start tile `S`
- identifies adjacent tiles that could connect to it
- walks the maze using a queue
- tracks visited cells
- records the largest step distance encountered

---

## 🚀 Key Takeaways

- Good start toward a breadth-first traversal of a pipe loop
- Uses a queue and visited set to explore the maze safely
- Pipe compatibility is encoded directly in directional checks
- Both puzzle parts currently share the same method

---

## 🔗 References

- https://adventofcode.com/2023/day/10