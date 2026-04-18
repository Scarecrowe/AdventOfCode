# 🎄 Advent of Code 2022 - Day 22: Monkey Map

## 📜 Puzzle Overview

This puzzle works with a map of walkable tiles, walls, and empty space, along with a path string containing movement counts and turns.

The map uses:

- `.` for open tiles
- `#` for walls
- blank space for void areas outside the playable region

The movement path is made of:

- numbers for forward movement
- `L` for turn left
- `R` for turn right

The solver parses the map into a `VectorArray`, splits the move string into individual instructions, and then drives a `Human` object across the board.

Part 1 treats the map as a 2D wrapping surface. Part 2 treats the layout as a folded cube and remaps movement across cube faces.

---

## 🧩 Part 1

Determine the final password after navigating the map using 2D wrapping rules.

### 💡 Approach

- Parse the board into a grid
- Parse the instruction string into:
  - move counts
  - `L`
  - `R`
- Start on the leftmost open tile in the top row
- Begin facing east
- For each move:
  - step forward tile by tile
  - wrap across void regions to the opposite valid edge
  - stop if a wall is encountered
- For each turn:
  - rotate the facing direction
- Compute the final password from:
  - row
  - column
  - facing

---

## 🧩 Part 2

Determine the final password after navigating the map as though it were folded into a cube.

### 💡 Approach

- Reuse the same parsed map and move list
- Keep the same starting position and facing
- Detect which cube face the current position belongs to
- When moving across an edge:
  - remap the position onto the corresponding cube face
  - adjust the facing direction when needed
- Stop movement if the destination tile is a wall
- Compute the final password from the final row, column, and facing

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monkey Map`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MonkeyMap(this.Input)`
- Calls `Navigate(MonkeyMapType.TwoDimensional)`
- Calls `Password()`

For Part 2:

- Creates `new MonkeyMap(this.Input)`
- Calls `Navigate(MonkeyMapType.ThreeDimensional)`
- Calls `Password()`

---

### `MonkeyMap.cs`

This class contains the parsed puzzle state.

It stores:

- `Map`
- `Human`
- `Moves`

The constructor:

- finds where the map section ends
- determines the widest row
- creates the map grid
- parses all board tiles
- parses the movement instructions
- creates the `Human` navigator

---

### Parsing the Map

The constructor reads the map section first.

It creates:

- `VectorArray Map`

Then for each character in the map area:

- `.` becomes `MonkeyMapState.Open`
- `#` becomes `MonkeyMapState.Closed`
- anything else remains the default void state

This means the board preserves:

- open tiles
- blocked tiles
- empty off-board regions

---

### Parsing the Moves

The instruction line is split into a list of strings in:

- `Moves`

The parsing logic builds up digits into a number until it sees:

- `L`
- `R`

When a turn is found:

- the accumulated number is added first
- then the turn is added
- the number buffer is reset

So a path like:

    10R5L5

becomes a sequence like:

- `10`
- `R`
- `5`
- `L`
- `5`

---

### `Human.cs`

This class handles the actual movement and facing logic.

It stores:

- `Point`
- `Facing`
- `Turns`
- `Moves`
- `Visited`

The constructor:

- starts the position at the leftmost open tile on the top row
- sets the initial facing to east
- creates lookup tables for:
  - turn direction changes
  - movement vectors

It also records the starting position in `Visited`.

---

### Turning

`Turn(string turn)` updates the current facing using the `Turns` lookup table.

So:

- `R` rotates clockwise
- `L` rotates counterclockwise

The movement position does not change when turning.

---

### Visit Tracking

`AddVisit()` records the current position and facing in:

- `Visited`

This is mainly used by `PrintMap()` so the travelled path can be visualised using direction symbols.

---

### 2D Movement

`MoveTwoDimensional(int spaces, VectorArray map)` performs Part 1 movement.

For each step:

- calculate the next point using the current facing
- wrap around map edges if the coordinates go out of bounds
- inspect the target tile

The tile rules are:

- `Open`
  - move into the tile
- `Closed`
  - stop movement immediately
- `Void`
  - scan to the opposite edge in that direction until the first real tile is found

The void wrapping logic works like this:

- moving north:
  - scan upward from the bottom of the current column
- moving south:
  - scan downward from the top of the current column
- moving east:
  - scan from the left of the current row
- moving west:
  - scan from the right of the current row

If the first real tile found is a wall, movement stops. If it is open, the position jumps there.

---

### 3D Cube Face Detection

`Face()` determines which cube face the current point belongs to.

It returns one of:

- `Top`
- `Bottom`
- `North`
- `South`
- `East`
- `West`

The implementation hardcodes the coordinate ranges for your input layout, using 50x50 face regions.

That means the cube folding logic is tailored to the specific puzzle input shape rather than being inferred dynamically.

---

### 3D Movement

`MoveThreeDimensional(int spaces, VectorArray map)` performs Part 2 movement.

For each step it:

- determines the current cube face
- computes the next tile from the current facing
- checks whether the step crosses a cube edge
- remaps:
  - `next`
  - `facing`

depending on which face and direction are involved

After remapping, the tile rules are:

- `Closed`
  - stop movement
- `Open`
  - apply the new position and facing
- `Void`
  - do nothing further for that step

So Part 2 is driven by explicit edge-transition rules rather than generic wrapping.

---

### Cube Edge Remapping

The cube transition logic is implemented with nested switches based on:

- current face
- current facing

Examples of what happens during a transition:

- position may shift to a different face
- row and column may be recalculated
- facing may rotate to a different direction

This models how movement behaves when a flat map is folded into a cube.

Because the face layout is hardcoded, the mappings directly reflect the arrangement of your specific input net.

---

### `MonkeyMapFace.cs`

This enum defines the recognised cube faces:

- `Top`
- `Bottom`
- `North`
- `South`
- `East`
- `West`

These values are used by `Face()` and the Part 2 movement logic.

---

### `MonkeyMapState.cs`

This enum defines the possible map cell states:

- `Void`
- `Open`
- `Closed`

These support both the normal movement and the wrapping logic.

---

### `MonkeyMapType.cs`

This enum selects the navigation mode:

- `TwoDimensional`
- `ThreeDimensional`

It is passed into:

- `Navigate(MonkeyMapType mapType)`

to decide whether Part 1 or Part 2 movement rules should be used.

---

### Main Navigation Loop

`Navigate(MonkeyMapType mapType)` processes every parsed move in sequence.

For each instruction:

- if it is `L` or `R`
  - turn the human
  - record the visit
- otherwise
  - parse it as a movement count
  - call either:
    - `MoveTwoDimensional(...)`
    - `MoveThreeDimensional(...)`

So the same move list is reused for both puzzle parts, with only the movement mode changing.

---

### Final Password

`Password()` is calculated by the `Human` class as:

    (1000 * (row + 1)) + (4 * (column + 1)) + facing

where:

- rows and columns are converted to 1-based values
- facing is stored as the cardinal enum value

So the final answer depends on:

- final row
- final column
- final facing direction

---

### `PrintMap()`

There is also a helper method that prints the current map.

It shows:

- spaces for void
- `.` for open tiles
- `#` for walls
- directional symbols for visited path positions

This is a debugging and visualisation helper rather than part of the core puzzle answer.

---

## 🛠 Implementation Notes

- The entry class is `Day22`
- The main solver class is `MonkeyMap`
- Navigation is performed by the `Human` class
- The starting point is the leftmost open tile of the top row
- Initial facing is east
- Part 1 uses wraparound scanning across void regions
- Part 2 uses hardcoded cube-face coordinate mappings
- The cube logic assumes a 50x50 face layout specific to the input structure
- The final password is computed from row, column, and facing

---

## 🧪 Behaviour Summary

Given a board and a path string:

- the solver parses the board into open, wall, and void tiles
- it splits the path into movement and turn instructions
- it places the navigator at the first open tile in the top row
- Part 1 moves using flat-map wrapping rules
- Part 2 moves using cube face transitions
- movement stops early when a wall blocks the path
- the final result is a password derived from the ending row, column, and facing

---

## 🚀 Key Takeaways

- Good example of separating parsing, navigation, and scoring logic
- Part 1 uses straightforward wraparound traversal on a sparse 2D board
- Part 2 upgrades the same navigation flow with explicit cube-net transitions
- The `Human` class cleanly encapsulates turning, movement, and password calculation
- Visit tracking and map printing make the path easy to debug visually

---

## 🔗 References

- https://adventofcode.com/2022/day/22