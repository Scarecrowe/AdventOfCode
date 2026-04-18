# 🎄 Advent of Code 2022 - Day 9: Rope Bridge

## 📜 Puzzle Overview

This puzzle simulates a rope moving across a grid.

The rope is made up of multiple knots:

- the first knot is the head
- the last knot is the tail
- any knots in between follow the knot directly in front of them

The input is a list of movement instructions such as:

    R 4
    U 2
    L 1

Each instruction contains:

- a direction
- a number of steps

The solver moves the head one step at a time, then updates each following knot if it is no longer adjacent to the knot ahead of it.

The goal is to count how many unique positions are visited by the final knot in the rope.

Part 1 uses a rope with 2 knots. Part 2 uses a rope with 10 knots.

---

## 🧩 Part 1

Determine how many positions are visited by the tail of a 2-knot rope.

### 💡 Approach

- Create a rope with 2 knots, all starting at `(0, 0)`
- Parse each move instruction into direction and step count
- Move the head one step at a time
- After each head movement:
  - check whether the tail is still adjacent to the head
  - if not, move the tail to a position adjacent to the head
- Record each unique position visited by the tail
- Return the number of unique visited positions

---

## 🧩 Part 2

Determine how many positions are visited by the final knot of a 10-knot rope.

### 💡 Approach

- Create a rope with 10 knots
- Reuse the same movement logic as Part 1
- After the head moves:
  - each knot checks whether it is still adjacent to the previous knot
  - if not, it moves to a position that is adjacent to that previous knot
- Only the last knot is counted for visit tracking
- Return the number of unique positions visited by that final knot

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Rope Bridge`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new RopeBridge(this.Input, 2)`
- Calls `Visited()`

For Part 2:

- Creates `new RopeBridge(this.Input, 10)`
- Calls `Visited()`

---

### `RopeBridge.cs`

This class contains the rope simulation.

It stores:

- `Knots`
- `KnotCount`
- `Visits`
- `Input`

The constructor:

- stores the raw movement input
- initialises the knot list
- initialises the visited-position list
- calls `AddKnots(knotCount)`

So the rope length is set up during construction.

---

### Rope State

The rope knots are stored in:

- `List<Vector> Knots`

Each `Vector` represents the position of one knot.

The list is ordered so that:

- `Knots[0]` is the head
- `Knots[KnotCount - 1]` is the tail

Visited tail positions are stored in:

- `List<Vector> Visits`

---

### Adding Knots

`AddKnots(int knotCount)` initialises the rope.

It:

- stores `KnotCount`
- clears the knot list
- adds the requested number of knots
- places every knot at:

    (0, 0)

So the entire rope begins collapsed onto the same starting point.

---

### Parsing Moves

`Moves()` converts the raw input strings into direction and count pairs.

It splits each line on spaces, then maps:

- `U` → `Cardinal.North`
- `D` → `Cardinal.South`
- `R` → `Cardinal.East`
- `L` → `Cardinal.West`

It yields:

- `(Cardinal, int)`

So the movement input is processed lazily as a sequence of parsed instructions.

---

### Moving the Head

`MoveHead(Cardinal direction)` updates only the first knot.

It changes the head position by one step:

- North → `Y - 1`
- South → `Y + 1`
- West → `X - 1`
- East → `X + 1`

This means each multi-step instruction is handled one grid step at a time.

---

### Adjacency Rules

`IsAdjacent(...)` checks whether two knots are touching.

It returns `true` when the second coordinate is one step away in any of the 8 surrounding positions:

- up
- down
- left
- right
- all four diagonals

It does not include the exact same position.

That is why the main loop also separately checks:

- whether the two knots are already on the same coordinate

So a knot only moves when it is neither:

- adjacent
- nor overlapping

with the knot in front of it.

---

### Adjacent Position Generation

`Adjacent((long y, long x) knot)` yields the 8 surrounding coordinates around a knot.

It returns:

- up
- down
- left
- right
- upper-left
- upper-right
- lower-left
- lower-right

This helper is used to find a valid place for a trailing knot to move.

---

### Main Simulation Loop

`Visited()` performs the full rope simulation.

For each parsed move:

- repeat for the specified number of steps
- move the head once
- then process each following knot from left to right through the rope

For each knot `j` after the head:

- compare it to knot `j - 1`
- if it is already adjacent or overlapping, leave it in place
- otherwise:
  - get all adjacent positions around the current knot
  - get all adjacent positions around the previous knot
  - move the current knot to the first coordinate that appears in both sets

So a knot moves to a shared adjacent position that brings it back into contact with the knot ahead.

---

### Tail Visit Tracking

Inside the same loop, after each knot update, the solver checks:

- whether this knot is the final knot in the rope
- whether its current position has already been recorded

If it is the final knot and the position is new, it adds that knot position to:

- `Visits`

At the end, `Visited()` returns:

- `this.Visits.Count`

So the puzzle answer is the number of unique positions reached by the final knot.

---

### Debug Printing

There is also a `Print()` helper method.

It:

- computes bounds from the recorded visits
- prints `#` for visited positions
- prints `.` elsewhere

This appears to be a debugging or visualisation helper and is not used by the main solution flow.

---

## 🛠 Implementation Notes

- The rope is stored as a list of `Vector` positions
- All knots start at `(0, 0)`
- Moves are processed one step at a time
- Only the head moves directly from the input
- Every other knot follows the knot immediately in front of it
- Adjacency includes diagonals
- The implementation uses intersection of adjacent-coordinate sets to reposition trailing knots
- Only the final knot's unique positions are counted
- Visited positions are stored in a `List<Vector>`

---

## 🧪 Behaviour Summary

Given a list of rope movement commands:

- the solver creates a rope of the requested length
- parses each instruction into direction and step count
- moves the head step-by-step
- updates each following knot when it is no longer adjacent
- records the final knot's unique visited positions
- Part 1 uses 2 knots
- Part 2 uses 10 knots
- both parts return the number of unique positions visited by the rope's tail

---

## 🚀 Key Takeaways

- Nice example of simulating linked movement across a grid
- The same logic supports both puzzle parts by changing only the knot count
- Adjacency is handled explicitly, including diagonals
- Trailing-knot movement is resolved by finding a shared adjacent coordinate
- The final answer depends only on the last knot, not the whole rope path

---

## 🔗 References

- https://adventofcode.com/2022/day/9