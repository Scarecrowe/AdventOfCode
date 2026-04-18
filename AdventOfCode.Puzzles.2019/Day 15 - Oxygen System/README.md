# 🎄 Advent of Code 2019 - Day 15: Oxygen System

## 📜 Puzzle Overview

This puzzle explores an unknown maze using an Intcode-controlled repair droid.

The input is a single comma-separated Intcode program.

The droid accepts movement commands:

- `1` for north
- `2` for south
- `3` for west
- `4` for east

After each move command, the program returns a status code:

- `0` if the droid hit a wall
- `1` if the droid moved successfully
- `2` if the droid moved onto the oxygen system

The solver builds a full map of the area, records the shortest distance to the oxygen system, then simulates how long oxygen takes to fill the explored space.

---

## 🧩 Part 1

Find the fewest movement commands needed to reach the oxygen system.

### 💡 Approach

- Load the Intcode program into a shared `IntcodeCpu`
- Start at `(0, 0)` and treat that as empty space
- Explore outward in all four directions
- Clone the CPU state for each branch of the search
- Use the returned status code to determine whether the next location is:
  - a wall
  - open space
  - the oxygen system
- Record the first distance at which the oxygen system is found
- Return that distance

---

## 🧩 Part 2

Find how many minutes it takes to fill the whole area with oxygen.

### 💡 Approach

- Reuse the map built in Part 1
- Mark the oxygen system position as oxygen-filled
- Repeatedly spread oxygen to all adjacent empty locations
- Count how many expansion rounds are required
- Return the total minutes once no empty locations remain

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Oxygen System`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new OxygenSystem(this.Input[0])`
- Calls `BuildMap()`
- Returns `.Distance`

For Part 2:

- Creates `new OxygenSystem(this.Input[0])`
- Calls `BuildMap()`
- Calls `FillMap()`
- Returns `.Minutes`

---

### `Entity.cs`

This enum describes the type of map tile.

It defines:

- `Empty`
- `Wall`
- `Oxygen`

These values are stored in the internal map as the area is explored and later filled.

---

### `Status.cs`

This enum maps the Intcode droid response values.

It defines:

- `Wall`
- `Moved`
- `Found`

These correspond to the movement results reported by the puzzle program after each command.

---

### `OxygenSystem.cs`

This class contains the full maze exploration and oxygen fill logic.

It stores:

- `Distance`
- `Minutes`
- `Droid`
- `Map`
- `Cpu`

The constructor:

- creates a new `IntcodeCpu` from the puzzle input
- initialises the map with `(0, 0)` as `Entity.Empty`
- stores the droid start position at `(0, 0)`

---

### Building the Map

`BuildMap()` performs the maze exploration.

It starts with a queue of states:

- one state for north
- one for south
- one for west
- one for east

Each queued state contains:

- the direction to try
- the current droid position
- a cloned CPU state
- the distance travelled so far

At a high level each step does this:

- dequeue one search state
- calculate the next position from the current direction
- skip it if that map location was already explored
- enqueue the movement command into that state's CPU
- run the Intcode program
- read the returned `Status`

If the result is:

- `Moved`, the new location is stored as `Entity.Empty` and all four directions are queued from there
- `Found`, the oxygen system location is recorded and `Distance` is set
- `Wall`, the location is stored as `Entity.Wall`

This makes the exploration behave like a breadth-first search over the maze.

---

### Why CPU Cloning Is Used

The droid is controlled by stateful Intcode execution, so each explored path needs its own machine state.

The shared `IntcodeCpu` supports:

- `Input`
- `Output`
- `Run()`
- `Clone()`

`Clone()` creates a copy of:

- memory
- instruction pointer
- relative base
- input queue
- output queue
- state

That allows each branch in the maze search to continue from the exact program state that would exist if the droid had physically travelled along that path.

---

### Direction Commands

The search stores directions using a `Cardinal` value, but the Intcode program expects numeric commands.

So movement is sent with:

    ((int)state.Direction) + 1

This lines up the internal direction enum with the puzzle command values:

    North = 1
    South = 2
    West  = 3
    East  = 4

---

### Recording the Oxygen Distance

When a move returns `Status.Found`, the solver does not add the oxygen location to the map as empty.

Instead it records:

- `this.Droid = new(location);`
- `this.Distance = state.Distance + 1;`

So the silver answer is the shortest breadth-first-search distance from the start to the oxygen system.

---

### Printing the Map

`PrintMap()` is a helper for visualising the explored area.

It builds a display dictionary:

- `Empty -> "."`
- `Wall -> "#"`
- `Oxygen -> "O"`

Then it prints the known map bounds row by row.

Unseen points are displayed as `#` in this helper.

This method is for inspection and debugging and is not required for the returned puzzle answers.

---

### Filling the Area with Oxygen

`FillMap()` performs the Part 2 simulation.

It first marks the oxygen system position:

    this.Map[this.Droid] = Entity.Oxygen;

Then it counts how many empty spaces remain and starts expanding outward from the oxygen point.

It keeps two working lists:

- `points` for the current oxygen frontier
- `newPoints` for the next minute's frontier

For each point in the current frontier:

- inspect adjacent cardinal cells
- if a cell is `Entity.Empty`
- change it to `Entity.Oxygen`
- add it to `newPoints`
- reduce the remaining empty count

After one full frontier expansion:

- replace `points` with `newPoints`
- clear `newPoints`
- increment `Minutes`

When no empty spaces remain, the gold answer is stored in `Minutes`.

---

### Adjacent Expansion

The oxygen fill uses cardinal adjacency only.

That means oxygen spreads:

- north
- south
- west
- east

Diagonal expansion is not used.

This matches the movement model used by the droid during map exploration.

---

## 🛠 Implementation Notes

- The solver uses a breadth-first exploration queue
- Each search branch carries its own cloned `IntcodeCpu`
- The map starts with `(0, 0)` marked as empty
- Walls and open space are stored as `Entity` values
- The oxygen system position is recorded separately when first found
- Part 1 returns the shortest path distance discovered by the search
- Part 2 performs a multi-source style flood fill starting from the oxygen system
- Time is measured by counting expansion rounds in `Minutes`

---

## 🧪 Behaviour Summary

Given an Intcode-controlled repair droid:

- the solver explores the maze one move at a time
- status codes reveal whether each attempted move hits a wall, enters empty space, or finds oxygen
- cloned CPU states allow independent exploration of every reachable branch
- Part 1 records the shortest route to the oxygen system
- Part 2 spreads oxygen through the completed map minute by minute
- the final result is either the shortest movement count or the oxygen fill time

---

## 🚀 Key Takeaways

- Good example of combining graph search with a stateful virtual machine
- Cloning the Intcode computer makes branching exploration very clean
- Breadth-first search naturally gives the shortest route to oxygen
- The flood-fill phase reuses the generated map without re-exploration
- Part 1 and Part 2 are neatly split into map discovery and map propagation

---

## 🔗 References

- https://adventofcode.com/2019/day/15