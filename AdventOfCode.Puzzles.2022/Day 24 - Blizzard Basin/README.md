# 🎄 Advent of Code 2022 - Day 24: Blizzard Basin

## 📜 Puzzle Overview

This puzzle is a pathfinding problem on a valley map filled with moving blizzards.

The input is a grid containing:

- walls
- empty ground
- directional blizzards:
  - `^`
  - `v`
  - `<`
  - `>`

The solver parses all blizzards, precomputes the repeating valley states over time, and then searches for the fastest safe route through those timed maps.

Part 1 finds the fewest minutes needed to travel from the entrance to the exit. Part 2 extends that to a full round trip:

- start to finish
- finish back to start
- start to finish again

---

## 🧩 Part 1

Determine the fewest number of minutes needed to reach the exit while avoiding moving blizzards.

### 💡 Approach

- Parse the input grid into:
  - static walls
  - moving blizzards
- Simulate blizzard movement over time
- Precompute every unique valley state until the map repeats
- Use timed pathfinding from the start to the finish
- At each minute, consider:
  - waiting in place
  - moving up
  - moving down
  - moving left
  - moving right
- Only allow moves onto tiles that will be empty on the next minute
- Return the minute count when the finish is first reached

---

## 🧩 Part 2

Determine the fewest number of minutes needed to complete the full round trip.

### 💡 Approach

- Reuse the same repeating timed map states
- Run the same pathfinding three times in sequence:
  - from start to finish
  - from finish back to start
  - from start to finish again
- Carry the accumulated minute count forward between legs
- Return the final total minutes after the third journey completes

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Blizzard Basin`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new BlizzardBasin(this.Input)`
- Calls `FewestMinutes()`

For Part 2:

- Creates `new BlizzardBasin(this.Input)`
- Calls `FewestMinutesWithRoundTrip()`

---

### `Blizzard.cs`

This class models a moving blizzard.

It stores:

- `Point`
- `Direction`
- `Row`
- `Column`

It also defines a static direction lookup for:

- `>`
- `<`
- `^`
- `v`

as well as neutral handling for:

- `#`
- `.`

The parser reads the input grid and creates one `Blizzard` object for every directional blizzard tile.

---

### Parsing the Blizzards

`Parse(string[] input)` scans the full input grid.

At a high level it does:

- loop through every row
- loop through every column
- check whether the current character is a directional blizzard
- create a `Blizzard` object with:
  - its current position
  - its movement direction
  - total row count
  - total column count

Walls and empty tiles are ignored at this stage.

---

### Blizzard Movement

`Move()` advances one blizzard by one minute.

It:

- adds the direction to the current position
- wraps vertically if it would move into the top or bottom wall
- wraps horizontally if it would move into the left or right wall

So blizzards move continuously inside the inner valley and reappear on the opposite side when they cross an edge.

This wrapping ignores the outer walls and keeps movement inside the traversable basin.

---

### `BlizzardBasin.cs`

This class contains the full puzzle setup.

It stores:

- `Start`
- `Finish`
- `Maps`

The constructor:

- initialises the fields
- parses the input
- builds the repeating timed map states

The final start and finish positions are set as:

- `Start = (1, 0)`
- `Finish = (width - 2, height - 1)`

So the entrance is the top opening and the exit is the bottom opening.

---

### Precomputing the Timed Maps

`Parse(string[] input)` builds every unique blizzard layout until the pattern repeats.

It does this by:

- parsing the initial list of blizzards
- building a map from their current positions
- storing that map
- moving every blizzard forward by one minute
- rebuilding the next map
- repeating until the map string matches the initial map again

This means the valley is treated as a cycle of map states.

Once the cycle is known, the solver can query the map at any minute using modulo arithmetic instead of simulating blizzards from scratch every time.

---

### `BlizzardBasinMaps.cs`

This class stores the repeating sequence of basin maps.

It stores:

- `Point`
- `Maps`

The constructor records the map dimensions and keeps the full list of precomputed timed states.

It also provides:

- `Build(...)`
- `IsValid(...)`
- `GetMap(...)`

---

### Building a Timed Map

`Build(string[] input, List<Blizzard> blizzards)` creates one `VectorArray` for a specific minute.

At a high level it does:

- convert the original input into a grid
- replace directional blizzard characters with `Empty`
- preserve rocks as walls
- place the current blizzard positions onto the map as `Blizzard`

So the stored timed maps combine:

- static terrain
- current dynamic blizzard occupancy

---

### Map Queries Over Time

`GetMap(int time, Vector point)` returns the map tile for a specific minute and position.

It works by:

- checking whether the point is inside bounds
- returning `Rock` if it is outside the map
- otherwise selecting the correct timed map with:

      time % this.Maps.Count

This is what makes the repeating blizzard cycle efficient to use during pathfinding.

---

### `BlizzardBasinType.cs`

This enum defines the map tile types:

- `North`
- `South`
- `East`
- `West`
- `Rock`
- `Empty`
- `Blizzard`

The directional values mirror the input characters, while `Blizzard` is used as a generic occupied state in the generated timed maps.

---

### `BlizzardBasinState.cs`

This class models one pathfinding state.

It stores:

- `Minutes`
- `Point`
- `Queue`
- `Visited`

The constructor accepts:

- current minute count
- current position

It also provides helper methods:

- `Clone()`
- `SetPoint(...)`
- `Tick()`
- `Distance(...)`
- `Adjacent(...)`
- `Move(...)`

---

### Movement Options

`Directions()` returns the five allowed actions from a state:

- stay in place
- move north
- move south
- move west
- move east

Allowing the current position to remain in the list is important, because sometimes the best move is to wait for a blizzard to clear.

---

### Timed Adjacency

`Adjacent(BlizzardBasinMaps maps)` generates the next valid states.

For each possible action:

- look at the candidate tile on:

      this.Minutes + 1

- only allow it if that tile will be:

      BlizzardBasinType.Empty

If the space is empty, the method yields a cloned state with:

- the minute incremented
- the point updated

So pathfinding always reasons about where the blizzards will be on the next minute, not just where they are now.

---

### Priority-Based Search

`Move(Vector finish, BlizzardBasinMaps maps)` performs the actual route search.

It:

- clears the visited set
- clears the priority queue
- enqueues the current state
- repeatedly dequeues the best candidate
- stops as soon as the finish point is reached

For each adjacent state:

- skip it if that exact `(point, minute)` pair has already been seen
- otherwise record it as visited
- enqueue it using a priority score

The priority score is based on:

    current minutes + distance to finish

So the search prefers states that are both early in time and closer to the target.

---

### Distance Heuristic

`Distance(Vector finish)` returns:

    this.Minutes + finish.Distance(this.Point)

This acts as the queue priority.

So although time is the real cost being minimised, the solver also biases toward positions that are geometrically nearer to the goal.

---

### Part 1 Result

`FewestMinutes()` starts from:

- minute `0`
- the entrance point

It then performs one move search to the finish and returns the resulting minute count.

So the silver answer is the first successful arrival time at the exit.

---

### Part 2 Result

`FewestMinutesWithRoundTrip()` chains three move searches together.

It starts with:

- minute `0`
- the entrance point

Then it performs:

- `Move(this.Finish, this.Maps)`
- `Move(this.Start, this.Maps)`
- `Move(this.Finish, this.Maps)`

Because each returned state includes the updated minute count, time continues naturally across all three journeys.

So the gold answer is the total elapsed time after the full round trip and final return to the exit.

---

## 🛠 Implementation Notes

- The entry class is `Day24`
- The main solver class is `BlizzardBasin`
- Moving hazards are represented by `Blizzard`
- Timed valley states are stored in `BlizzardBasinMaps`
- Search nodes are represented by `BlizzardBasinState`
- Blizzard layouts are precomputed until they repeat
- Map lookup uses modulo indexing over the repeating cycle
- Pathfinding allows waiting in place as a valid action
- Part 1 runs one trip
- Part 2 runs three consecutive trips

---

## 🧪 Behaviour Summary

Given a valley map with moving blizzards:

- the solver parses the initial blizzard positions
- it simulates and stores every unique future valley state until the pattern repeats
- it searches across both space and time
- each move checks whether the destination will be safe on the next minute
- waiting in place is allowed when movement would be unsafe
- Part 1 finds the shortest safe trip to the exit
- Part 2 finds the shortest safe out-and-back-and-out trip
- the final result is the total minute count for the required journey

---

## 🚀 Key Takeaways

- Good example of pathfinding on a time-dependent grid
- Precomputing repeating hazard states avoids repeated full simulation during search
- The solver treats time as part of the search state
- Waiting in place is an important legal move for dynamic obstacle puzzles
- Part 2 is handled cleanly by chaining the same search method three times

---

## 🔗 References

- https://adventofcode.com/2022/day/24