# 🎄 Advent of Code 2022 - Day 14: Regolith Reservoir

## 📜 Puzzle Overview

This puzzle simulates sand falling through a cave made of rock paths.

Each input line describes connected rock coordinates such as:

    498,4 -> 498,6 -> 496,6

The solver parses these paths into a 2D cave map and then repeatedly drops sand from the reservoir at:

    500,0

Part 1 runs the simulation with no floor, so sand can eventually fall out of bounds. Part 2 adds a solid floor below the cave, allowing sand to pile up until the source becomes blocked. 

---

## 🧩 Part 1

Determine how many units of sand come to rest before sand starts falling into the abyss.

### 💡 Approach

- Parse all rock path segments into a cave grid
- Start dropping sand from the reservoir at `500,0`
- Let each grain fall vertically as far as possible
- If blocked, try:
  - down-left
  - down-right
- If both diagonals are blocked, settle the sand in place
- Stop when sand would move beyond the map bounds
- Count all settled sand tiles

---

## 🧩 Part 2

Determine how many units of sand come to rest when an infinite-style floor is added below the cave.

### 💡 Approach

- Reuse the same rock parsing logic
- Add an artificial rock floor at:

      max rock Y + 2

- Expand the map width to allow the pile to spread
- Continue dropping sand from `500,0`
- Stop when the source itself becomes filled with sand
- Count all settled sand tiles

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Regolith Reservoir`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new RegolithReservoir(this.Input, false)`
- Calls `Run().SumOfSand()`

For Part 2:

- Creates `new RegolithReservoir(this.Input, true)`
- Calls `Run().SumOfSand()`

---

### `RegolithReservoir.cs`

This class contains the full cave simulation.

It stores:

- `Map`

The constructor:

- accepts the input
- accepts a `hasFloor` flag
- builds the cave map with:

      Parse(input, hasFloor)

---

### Counting Sand

`SumOfSand()` returns the final total of settled sand by scanning the whole map and counting every tile whose value is:

    State.Sand

---

### Main Simulation Loop

`Run()` performs the sand simulation.

It creates:

- `Queue<Vector> queue = new();`
- `Vector reservoir = new(500, 0);`

Then it starts by enqueueing the reservoir position.

The simulation repeatedly:

- dequeues the next active point
- moves sand straight downward as far as possible
- checks whether it can continue diagonally
- settles sand if no diagonal move is available
- enqueues the reservoir again to begin the next grain

This means the solver models one sand unit at a time, always restarting from the source after a grain comes to rest.

---

### Falling Straight Down

`MoveSouth(Vector point)` handles the vertical fall.

It:

- starts one row below the current point
- keeps moving downward while the map contains `State.Air`
- stops just above the first non-air tile
- returns that resting candidate position

So each grain fast-forwards vertically rather than stepping downward one row at a time.

---

### Diagonal Movement Rules

After dropping vertically, the solver checks:

- down-left first
- down-right second

The logic is:

- if down-left is air, enqueue that position and continue
- otherwise if down-right is air, enqueue that position and continue
- otherwise settle sand at the current point

That mirrors the puzzle's falling priority order.

---

### Settling Sand

When a grain cannot move down-left or down-right, the solver writes:

    this.Map[point.Y, point.X] = State.Sand

Then it enqueues the reservoir again so the next grain starts from the source.

---

### Part 1 Stopping Condition

When running without a floor, the simulation stops if the falling position would move beyond the map size.

The check is effectively whether the next downward or diagonal step would go outside the cave array.

At that point, `Run()` exits and the final settled sand total is returned by `SumOfSand()`.

---

### Part 2 Stopping Condition

When running with a floor, the simulation ends when the reservoir becomes blocked.

The solver checks:

    if (current == reservoir && this.Map[0, 500] == State.Sand)

When that becomes true, no more sand can be dropped, so the simulation finishes.

---

### Parsing the Rock Map

`Parse(string[] input, bool hasFloor)` builds the cave structure.

At a high level it does:

- split each input line on `" -> "`
- parse each coordinate pair
- store each path as a list of vectors
- track maximum width and height encountered

It then creates:

- `VectorArray result = new(width, height);`

and fills in every horizontal and vertical rock segment between consecutive points.

---

### Drawing Rock Segments

For each pair of adjacent rock points, the solver checks whether the segment is:

- vertical
- horizontal

If vertical:

- fill every `Y` coordinate between the two points at the same `X`

If horizontal:

- fill every `X` coordinate between the two points at the same `Y`

Each filled cell is assigned:

    State.Rock

---

### Artificial Floor Handling

When `hasFloor` is `true`, the parser adds one extra rock path representing a floor.

It computes:

    max rock Y + 2

Then adds rock points for:

    x = 0 to 999

at that Y coordinate.

It also expands the cave dimensions so the simulation has enough space to spread across the floor.

---

### `PrintCave()`

There is also a helper method for visualising the cave.

It prints:

- `.` for air
- `#` for rock
- `o` for sand

This is a debugging or display helper and is not required for the final puzzle answers.

---

## 🛠 Implementation Notes

- The puzzle entry class is `Day14`
- The main solver class is `RegolithReservoir`
- The simulation uses a `VectorArray` map
- Sand always starts from `500,0`
- Vertical movement is fast-forwarded with `MoveSouth(...)`
- Diagonal priority is down-left first, then down-right
- Part 1 stops when sand would leave the mapped cave
- Part 2 adds a floor and stops when the source becomes blocked

---

## 🧪 Behaviour Summary

Given a list of rock path definitions:

- the solver parses all rock coordinates into a cave map
- it repeatedly drops sand from the reservoir
- each grain falls vertically as far as possible
- if blocked, it tries down-left and then down-right
- if neither move is available, the grain settles
- Part 1 ends when sand escapes the cave bounds
- Part 2 ends when sand fills the source
- the final answer is the number of settled sand tiles

---

## 🚀 Key Takeaways

- Good example of a 2D cellular-style simulation
- Rock paths are expanded into full horizontal and vertical segments
- The solver optimises downward motion with a direct vertical scan
- Both puzzle parts reuse the same simulation with a floor mode flag
- The final result is derived by counting all settled sand after the run completes

---

## 🔗 References

- https://adventofcode.com/2022/day/14