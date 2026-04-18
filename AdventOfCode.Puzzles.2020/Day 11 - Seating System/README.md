# 🎄 Advent of Code 2020 - Day 11: Seating System

## 📜 Puzzle Overview

This puzzle simulates seat occupancy in a ferry waiting area.

The input is a 2D layout made of:

- floor tiles
- empty seats
- occupied seats

The solver represents each cell using an `Entity` enum:

- `Floor = '.'`
- `EmptySeat = 'L'`
- `OccupiedSeat = '#'`

It loads the seating layout into a `VectorArray<long, Entity>` map and then repeatedly applies seat-update rules until the layout stops changing.

Part 1 uses adjacent seats. Part 2 uses the first visible seat in each direction.

---

## 🧩 Part 1

Determine how many seats end up occupied once the adjacent-seat rules stabilise.

### 💡 Approach

- Parse the input grid into the seating map
- Repeatedly scan every position
- For each occupied seat:
  - count adjacent occupied seats
  - empty it if 4 or more adjacent seats are occupied
- For each empty seat:
  - occupy it if no adjacent occupied seats exist
- Keep applying updates until no seat changes state
- Return the final occupied-seat count

---

## 🧩 Part 2

Determine how many seats end up occupied once the visible-seat rules stabilise.

### 💡 Approach

- Precompute the first visible non-floor seat in each direction for every position
- Repeatedly scan non-floor cells
- For each occupied seat:
  - count visible occupied seats
  - empty it if 5 or more visible seats are occupied
- For each empty seat:
  - occupy it if no visible occupied seats are occupied
- Repeat until the layout no longer changes
- Return the final occupied-seat count

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Seating System`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SeatingSystem(this.Input)`
- Calls `SeatCount()`

For Part 2:

- Creates `new SeatingSystem(this.Input)`
- Calls `VisibleSeatCount()`

---

### `Entity.cs`

This enum defines the three possible map values:

- `Floor = '.'`
- `EmptySeat = 'L'`
- `OccupiedSeat = '#'`

This allows the seating map to work with meaningful named values instead of raw characters.

---

### `SeatingSystem.cs`

This class contains the full simulation logic.

It stores:

- `Map`

The constructor converts the input directly into a `VectorArray<long, Entity>`:

    new(input, (chr) => (Entity)chr)

So every character in the input grid becomes one typed cell in the internal seating map.

---

### Part 1 Simulation

`SeatCount()` handles the adjacent-seat rules.

It loops until the map stops changing.

On each pass it:

- clones the current map into a new working copy
- iterates over all cells
- checks seat behaviour based on current value
- writes any changes into the cloned map
- replaces the original map only after the full pass completes

This ensures all seat updates happen simultaneously per round.

---

### Adjacent Seat Rules

For each cell, Part 1 uses:

    this.Map.AdjacentInterCardinal(cell.Point)

This collects neighbouring cells around the current seat.

The rules are:

- occupied seat becomes empty if 4 or more adjacent seats are occupied
- empty seat becomes occupied if no adjacent seats are occupied
- floor never changes

At a high level:

- `OccupiedSeat` checks `>= 4`
- `EmptySeat` checks `== 0`

---

### Stabilisation in Part 1

Each round tracks whether anything changed using:

- `stateChanged`

If no seat changes during a full pass, the method returns:

    this.Map.Count(Entity.OccupiedSeat)

So the silver answer is the number of occupied seats once the adjacent-rule simulation reaches a stable state.

---

### Part 2 Simulation

`VisibleSeatCount()` handles the line-of-sight rules.

Before the simulation loop begins, it builds:

- `Dictionary<Vector<long>, List<VectorCell<long, Entity>>> visible`

using:

- `VisibleSeats()`

This precomputes which seat is first visible in each direction from every point, avoiding repeated directional scanning every round.

---

### Visible Seat Rules

Part 2 only processes non-floor cells:

- `this.Map.AxisEnumerator().Where(cell => cell.Value != Entity.Floor)`

For each seat:

- occupied seat becomes empty if 5 or more visible seats are occupied
- empty seat becomes occupied if 0 visible seats are occupied

This logic is delegated through:

- `SetSeat(...)`

where the threshold check is passed in as a function:

- `(value) => value >= 5`
- `(value) => value == 0`

---

### `SetSeat(...)`

This helper applies the Part 2 seat-change logic.

It receives:

- the precomputed visible-seat lookup
- the current cell
- the cloned output map
- the shared `stateChanged` flag
- a rule function
- the value to write when the rule matches

It then:

- gets the visible seats for the current position
- counts how many are currently occupied in `this.Map`
- applies the supplied rule
- writes the new seat value into the cloned map if needed

This keeps the visible-seat update logic compact and reusable.

---

### `VisibleSeats()`

This method precomputes line-of-sight seat relationships.

For every cell in the map, it:

- creates an empty visible-seat list
- loops through all cardinal/intercardinal directions
- steps outward one position at a time
- skips floor tiles
- stops at the first non-floor seat found
- stores that seat position as visible from the starting cell

So instead of repeatedly scanning outward every simulation round, the solver builds the visibility graph once and reuses it.

---

### Direction Scanning

The directional scan uses:

- `CardinalHelper.AllCells<long, Entity>()`

For each direction it:

- clones the current point
- adds the directional vector repeatedly
- checks `IsVectorInRange(point)`
- stops when it leaves the map or finds a non-floor seat

This gives each seat access to its first visible seat in all supported directions.

---

## 🛠 Implementation Notes

- The seating layout is stored as a `VectorArray<long, Entity>`
- Both parts run until no state changes occur
- Updates are written into a cloned map to preserve simultaneous round behaviour
- Part 1 uses adjacent seat checks
- Part 2 uses precomputed visible seat relationships
- Floor tiles are ignored by the visibility-based simulation
- Final answers are produced by counting occupied seats in the stabilised map

---

## 🧪 Behaviour Summary

Given a seating layout:

- the solver parses each character into a typed seat or floor entity
- Part 1 repeatedly applies adjacent-seat occupancy rules
- Part 2 repeatedly applies visible-seat occupancy rules
- both modes continue until no seats change
- each part returns the final number of occupied seats after stabilisation

---

## 🚀 Key Takeaways

- Good example of grid simulation with repeated state transitions
- Uses a cloned map each round so all seat updates happen simultaneously
- Part 1 solves local neighbour interactions
- Part 2 optimises line-of-sight checks by precomputing visible seats
- Clean separation between adjacent logic, visible logic, and shared map state

---

## 🔗 References

- https://adventofcode.com/2020/day/11