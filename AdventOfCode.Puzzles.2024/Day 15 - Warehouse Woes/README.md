# 🎄 Advent of Code 2024 - Day 15: Warehouse Woes

## 📜 Puzzle Overview

This puzzle simulates a warehouse robot pushing boxes around a map.

The input contains two sections:

- a warehouse layout
- a list of movement instructions

The map uses tiles such as:

- `#` for walls
- `.` for empty space
- `@` for the robot
- `O` for boxes in the normal map
- `[` and `]` for wide boxes in the expanded map used by Part 2

The solver parses the map and movement list, then applies every move in sequence.

Part 1 uses the original map and single-tile boxes.

Part 2 expands the map horizontally so each box becomes a two-cell wide box, then applies special grouped push logic.

---

## 🧩 Part 1

Determine the final GPS coordinate sum after simulating the robot on the normal warehouse map.

### 💡 Approach

- Parse the warehouse map and movement instructions
- Locate the robot position
- Process each move one at a time
- If the next tile is empty, move the robot
- If the next tile contains a box, try to push it forward until an empty space is found
- Ignore moves blocked by walls
- After all moves complete, sum the GPS coordinates of all box positions

---

## 🧩 Part 2

Determine the final GPS coordinate sum after simulating the robot on the widened warehouse map.

### 💡 Approach

- Expand the original map horizontally
- Convert each normal box into a wide box represented by `[]`
- Process the same movement list
- Horizontal pushes move connected box halves as a group
- Vertical pushes collect all linked wide-box pieces in the movement direction
- Only move a group if the full push is valid
- After all moves complete, sum the GPS coordinates of all left box halves

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Warehouse Woes`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new WarehouseWoes(this.Input)`
- Calls `GpsCoordinate()`

For Part 2:

- Creates `new WarehouseWoes(this.Input, true)`
- Calls `GpsCoordinate()`

---

### `WarehouseWoes.cs`

This class contains the full warehouse parsing and movement logic.

It stores:

- `Map`
- `Moves`
- `Robot`
- `Wider`

The constructor:

- initialises the move list
- stores whether the widened mode is enabled
- parses the input
- locates the robot position by finding `@` in the map

So the same class supports both puzzle parts, with the `wider` flag switching the map format and box-handling behaviour.

---

### Parsing the Input

`Parse(string[] input, bool wider)` splits the puzzle input into two sections:

- map lines
- move lines

It does this by:

- collecting lines into the map until it reaches an empty line
- then collecting the remaining lines as movement instructions

It then calls:

- `ParseMap(map, wider)`
- `ParseMoves(moves)`

So the map and movement stream are built separately before the simulation starts.

---

### Parsing the Map

`ParseMap(List<string> map, bool wider)` creates the warehouse grid as:

- `new([...], c => c)`

If `wider` is enabled, it first transforms the input using:

- `WiderMap(map)`

This means:

- Part 1 uses the map as-is
- Part 2 uses an expanded version

---

### Widening the Map

`WiderMap(...)` doubles each map row horizontally.

It converts:

- `#` into `##`
- `.` into `..`
- `@` into `@.`
- `O` into `[]`

So a single-tile box becomes a two-cell wide box made of:

- `[` for the left side
- `]` for the right side

This widened representation is what Part 2 uses for grouped pushing.

---

### Parsing the Moves

`ParseMoves(List<string> moves)` reads every movement character and converts it using:

- `CardinalHelper.SymbolToCardinalMap`

Each move is stored in:

- `Moves`

So the solver ends up with an ordered list of directions to execute.

---

### Main Simulation Loop

`Move()` performs the full warehouse simulation.

For each direction in `Moves`, it looks at the tile immediately in front of the robot and switches on its value.

The possible cases are:

- `.` -> move the robot
- `#` -> do nothing
- `O` -> try to push a normal box
- `[` or `]` -> try to push a wide box group

So every movement instruction is resolved by examining just the next tile and delegating to the correct handler.

---

### Moving the Robot

`MoveRobot(Vector point)` performs a plain robot move.

It:

- writes `@` to the new position
- writes `.` to the old robot position
- updates `Robot`

This is used both for simple moves into empty space and after successful box pushes.

---

### Part 1 Single-Box Pushing

`MoveSingle(Vector point, Cardinal direction)` handles normal box pushes.

It first checks:

- `CanPush(point.Clone(), direction)`

`CanPush(...)` keeps stepping forward until it finds either:

- `.` -> push is possible
- `#` -> push is blocked

If the push is valid:

- the robot moves into the box's old position
- the box is pushed forward until the first empty tile is found
- that empty tile becomes `O`

So the implementation effectively slides the chain forward until the open space is filled.

---

### Part 2 Grouped Box Pushing

`MoveGroup(Vector point, Cardinal direction)` handles wide boxes.

It first checks whether the move is:

- horizontal
- vertical

Then it builds the box group differently depending on direction.

For horizontal moves it uses:

- `BoxGroupHorizontal(...)`

For vertical moves it uses:

- `BoxGroupVertical(...)`

If the group can be pushed, all box pieces are moved one step in the chosen direction, then the robot advances.

---

### Horizontal Wide-Box Grouping

`BoxGroupHorizontal(Vector point)` finds all connected wide-box cells horizontally.

It uses:

- a queue
- a visited set

It explores only east-west neighbours and collects any cells containing:

- `[`
- `]`

So when the robot pushes sideways, all connected box halves in that row are treated as one movable group.

---

### Vertical Wide-Box Grouping

`BoxGroupVertical(Vector point, Cardinal direction)` builds a vertical push group.

It starts from the impacted box half and immediately adds its matching partner:

- if starting on `]`, also include the cell to the west
- otherwise include the cell to the east

Then it explores only in the push direction.

When it encounters:

- `[` -> include that cell and its right half
- `]` -> include that cell and its left half

This ensures a vertical push moves complete wide boxes, not just individual halves.

---

### Checking Vertical Push Validity

`CanPushVertical(List<Vector> group, Cardinal move)` verifies that every box cell in the group can move one step in the given direction.

For each point in the group it checks:

- the transformed target cell

If any target cell is `#`, the whole push fails.

Otherwise, the move is allowed.

So vertical wide-box pushes are all-or-nothing.

---

### Applying Group Movement

When a grouped push succeeds, `MoveGroup(...)` does this:

- reverse the group list
- for each item:
  - store its current value
  - clear the old position to `.`
  - move the coordinate one step
  - write the stored value into the new position
- move the robot into the just-freed front position

Reversing the group ensures pieces are moved from the far side first so they do not overwrite each other during the update.

---

### GPS Coordinate Calculation

`GpsCoordinate()` runs the full movement simulation by calling:

- `Move()`

Then it sums the coordinates of all remaining box positions.

It chooses the tile to count as:

- `O` in normal mode
- `[` in widened mode

The returned sum is:

    100 * y + x

for each counted box position.

So in Part 2, only the left half of each wide box contributes to the final GPS total.

---

### Part 1 Return Value

When created with:

    new WarehouseWoes(this.Input)

the solver uses the original map and returns:

- the sum of GPS coordinates for all `O` boxes after all moves complete

---

### Part 2 Return Value

When created with:

    new WarehouseWoes(this.Input, true)

the solver widens the map and returns:

- the sum of GPS coordinates for all `[` box positions after all moves complete

---

## 🛠 Implementation Notes

- The input is split into map lines and move lines
- The robot position is discovered after parsing by locating `@`
- Part 1 uses single-cell boxes marked `O`
- Part 2 expands the map and uses two-cell boxes marked `[]`
- Horizontal and vertical wide-box pushes use different grouping logic
- Vertical pushes explicitly include both halves of every wide box
- Group movement is applied in reverse order to avoid overwrite problems
- The final score is computed from box positions using `100 * y + x`

---

## 🧪 Behaviour Summary

Given a warehouse layout and movement instructions:

- the solver parses the map and direction list
- finds the robot start position
- applies each move in order
- Part 1 pushes single boxes through open space
- Part 2 expands the map and pushes linked wide-box groups
- blocked moves are ignored
- the final answer is the GPS sum of the box positions after all movement is complete

---

## 🚀 Key Takeaways

- Good example of using the same simulation framework for two related grid puzzles
- Part 1 uses simple push-ahead box logic
- Part 2 adds a widened map and grouped box movement rules
- The horizontal and vertical push cases are handled separately for correctness
- Reversing the movement order is a clean way to prevent box overwrites during group moves
- The widened representation keeps paired box halves explicit and easy to reason about

---

## 🔗 References

- https://adventofcode.com/2024/day/15