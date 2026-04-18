# 🎄 Advent of Code 2020 - Day 12: Rain Risk

## 📜 Puzzle Overview

This puzzle simulates ship navigation using a list of movement instructions.

Each instruction begins with a letter followed by a number, for example:

    F10
    N3
    R90
    F11

The solver supports two movement modes:

- Part 1 moves the ship directly based on its current facing direction
- Part 2 moves the ship relative to a waypoint

Both parts return the Manhattan distance from the starting position after all instructions have been processed.

---

## 🧩 Part 1

Determine how far the ship ends up from its starting point after following the navigation instructions directly.

### 💡 Approach

- Start the ship at `(0, 0)`
- Start facing east
- Process each instruction in order
- Move north, south, east, or west for directional instructions
- Rotate the current facing direction for left and right turns
- For forward instructions, move in the ship's current facing direction
- Return the Manhattan distance from the origin

---

## 🧩 Part 2

Determine how far the ship ends up from its starting point when movement is controlled by a waypoint.

### 💡 Approach

- Start the ship at `(0, 0)`
- Start the waypoint at `(10, 1)`
- Process each instruction in order
- Move the waypoint for north, south, east, and west instructions
- Rotate the waypoint around the ship for left and right turns
- For forward instructions, move the ship toward the waypoint multiple times
- Return the Manhattan distance from the origin

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Rain Risk`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new RainRisk(this.Input)`
- Calls `Distance()`

For Part 2:

- Creates `new RainRisk(this.Input)`
- Calls `DistanceWithWaypoint()`

---

### `RainRisk.cs`

This class contains the full navigation logic.

It stores:

- `Input`
- `Point`
- `WayPoint`
- `Direction`
- `Directions`

The constructor:

- stores the input
- sets the initial facing direction to east using index `1`
- initialises the ship position to `(0, 0)`
- initialises the waypoint to `(0, 0)` until Part 2 resets it

The direction array is:

    N, E, S, W

So direction index `1` means the ship starts facing east.

---

### Ship Position and Waypoint

The solver uses vectors for both:

- `Point` for the ship position
- `WayPoint` for the waypoint position

For Part 1:

- only `Point` and `Direction` matter

For Part 2:

- `Point` tracks the ship
- `WayPoint` tracks the relative waypoint used for movement

---

### `Distance()`

This method solves Part 1.

It resets:

- `Point = (0, 0)`
- `Direction = 1`

Then it processes every instruction using:

    this.Input.ForEach(this.Process);

After all instructions are handled, it returns:

    this.Point.Absolute()

So the silver answer is the Manhattan distance of the final ship position from the origin.

---

### Part 1 Instruction Processing

`Process(string instruction)` handles direct ship navigation.

It extracts:

- the instruction mode as the first character
- the numeric value from the rest of the string

Then it switches on the mode.

Directional movement updates the ship position directly:

- `N` increases `Point.Y`
- `S` decreases `Point.Y`
- `E` increases `Point.X`
- `W` decreases `Point.X`

Rotation updates the direction index:

- `L` subtracts `value / 90`
- `R` adds `value / 90`

Forward movement is handled by converting the current direction back into a compass instruction:

    this.Process($"{(char)this.Directions[this.Direction]}{value}");

So `F` reuses the same directional movement logic rather than duplicating it.

---

### `DistanceWithWaypoint()`

This method solves Part 2.

It resets:

- `Point = (0, 0)`
- `WayPoint = (10, 1)`
- `Direction = 1`

Then it processes every instruction using:

    this.Input.ForEach(this.ProcessWithWaypoint);

After all instructions are handled, it returns:

    this.Point.Absolute()

So the gold answer is again the Manhattan distance, but this time using waypoint-based movement.

---

### Part 2 Instruction Processing

`ProcessWithWaypoint(string instruction)` handles waypoint navigation.

It extracts:

- the instruction mode
- the numeric value

Then it switches on the mode.

Directional instructions move the waypoint:

- `N` increases `WayPoint.Y`
- `S` decreases `WayPoint.Y`
- `E` increases `WayPoint.X`
- `W` decreases `WayPoint.X`

Rotation instructions rotate the waypoint around the ship:

- `L` uses `this.WayPoint.Rotate(value)`
- `R` uses `this.WayPoint.Rotate(-value)`

Forward movement moves the ship toward the waypoint:

    this.Point += this.WayPoint * value;

So if the waypoint is `(10, 1)` and the instruction is `F7`, the ship moves by `(70, 7)`.

---

### Rotation Handling

Waypoint rotation is delegated to the vector helper:

    this.WayPoint.Rotate(value)

and

    this.WayPoint.Rotate(-value)

This keeps the rotation logic out of the puzzle class and allows the waypoint to be turned cleanly around the ship position.

The sign of the angle is used to distinguish:

- positive rotation for left turns
- negative rotation for right turns

---

## 🛠 Implementation Notes

- The ship position is stored in `Point`
- The waypoint is stored in `WayPoint`
- The ship starts facing east using direction index `1`
- Part 1 reuses `Process()` recursively for forward instructions
- Part 2 rotates the waypoint instead of the ship direction
- Both parts return Manhattan distance using `Absolute()`
- The solver uses vector arithmetic for concise movement updates

---

## 🧪 Behaviour Summary

Given a sequence of navigation instructions:

- the solver parses each instruction into a mode and numeric value
- Part 1 moves the ship directly based on direction and facing
- Part 2 moves the ship indirectly using a rotating waypoint
- both parts start from `(0, 0)`
- both parts return the Manhattan distance from the origin

---

## 🚀 Key Takeaways

- Clean separation between direct navigation and waypoint navigation
- Part 1 uses facing-direction state with indexed compass directions
- Part 2 uses vector maths for elegant waypoint movement
- Forward movement in Part 1 is reused through the existing direction handler
- Waypoint rotation is delegated to a vector helper for simpler puzzle logic

---

## 🔗 References

- https://adventofcode.com/2020/day/12