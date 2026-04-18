# 🎄 Advent of Code 2019 - Day 17: Set and Forget

## 📜 Puzzle Overview

This puzzle uses an Intcode program to generate a scaffold map for a vacuum robot.

The program outputs an ASCII view of the area, where tiles can represent:

- scaffold
- open space
- the robot facing in one of four directions

The solver first builds the map from the Intcode output.

From there:

- Part 1 finds scaffold intersections and sums their alignment parameters
- Part 2 builds the robot movement path, compresses it into routines, feeds those routines back into the Intcode program, and reads the final dust collection value

---

## 🧩 Part 1

Determine the sum of the alignment parameters of all scaffold intersections.

### 💡 Approach

- Run the Intcode program in map mode
- Read the ASCII output into a coordinate map
- Identify scaffold tiles
- For each scaffold tile:
  - inspect the four cardinal neighbours
  - if all four are also scaffold, it is an intersection
- For each intersection:
  - add `x * y` to the total

That total becomes the alignment parameter answer.

---

## 🧩 Part 2

Control the robot with compressed movement routines and determine how much dust it collects.

### 💡 Approach

- Rebuild the map in vacuum-robot mode
- Locate the robot and its facing direction
- Walk the scaffold to build a full movement path
- Compress that path into:
  - one main routine
  - three subroutines: `A`, `B`, and `C`
- Feed the routines into the Intcode computer as ASCII input
- Disable the live video feed
- Read the final output value greater than `255`
- Return that value as the dust collected

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Set and Forget`
- Loads the puzzle input
- Uses the first input line as the Intcode program

For Part 1:

- Creates `new SetAndForget(this.Input[0])`
- Calls `BuildMap()`
- Returns `AlignmentParameter`

For Part 2:

- Creates `new SetAndForget(this.Input[0])`
- Calls `BuildMap(2)`
- Calls `FindRobots()`
- Returns `DustCollected`

---

### `Entity.cs`

This enum represents the ASCII map tiles emitted by the Intcode program.

It contains:

- `Scaffold = 35`
- `OpenSpace = 46`
- `RobotUp = 94`
- `RobotDown = 118`
- `RobotLeft = 60`
- `RobotRight = 62`
- `RobotTumbling = 88`

So the map is stored using meaningful entity values instead of raw ASCII integers.

---

### `Robot.cs`

This class models the robot while building the movement path.

It stores:

- `Location`
- `Direction`

The constructor:

- receives the robot's starting location
- converts the robot tile entity into a cardinal direction

So a map tile such as:

- `^`
- `v`
- `<`
- `>`

becomes a robot with a matching direction.

---

### Robot Turning

`Turn(Cardinal scaffold)` updates the robot's direction and returns the turn instruction string.

Depending on the robot's current direction and the direction of the next scaffold branch, it returns:

- `"L"`
- `"R"`

It also updates the robot's facing direction internally.

So this method determines the exact turn token used later in the movement path.

---

### `SetAndForget.cs`

This class contains the full map parsing, intersection detection, path generation, compression, and execution logic.

It stores:

- `Cpu`
- `Map`
- `Intersections`
- `Path`
- `CompressedPaths`
- `Movements`
- `AlignmentParameter`
- `DustCollected`

The constructor:

- creates the Intcode CPU
- initialises the map and supporting collections
- prepares the solver state for both puzzle parts

---

### Building the Map

`BuildMap(int mode = 1)` runs the Intcode program and converts the ASCII output into a coordinate map.

It first writes the mode into memory position `0`:

- `1` for normal scaffold output
- `2` for robot control mode

It then runs the CPU and processes output values one by one.

For each value:

- if it is newline (`10`)
  - reset `x`
  - move to the next `y`
- otherwise
  - store the tile entity at `(x, y)`
  - increment `x`

After the map is built, it calls:

- `BuildIntersections()`

---

### Finding Intersections

`BuildIntersections()` scans all scaffold tiles and checks their four cardinal neighbours.

A position is considered an intersection only when:

- north is scaffold
- south is scaffold
- east is scaffold
- west is scaffold

When that happens:

- the coordinate is added to `Intersections`
- `x * y` is added to `AlignmentParameter`

So Part 1 is solved directly from the finished scaffold map.

---

### Printing the Map

`PrintMap()` converts the stored map into readable text.

It maps entities to display characters such as:

- `#`
- `.`
- `^`
- `v`
- `<`
- `>`
- `X`

Then it writes the full rectangle bounded by the map coordinates.

This is mainly for visual inspection of the generated scaffold layout.

---

### Finding the Robot

`GetRobot()` locates the robot by finding the map tile that is neither:

- scaffold
- open space

That tile is used to construct a `Robot` instance with:

- start position
- initial direction

This is the starting point for path generation.

---

### Building the Movement Path

`BuildPath()` walks the scaffold network and records the robot's full movement instructions.

It keeps:

- the robot state
- a visited set
- the growing `Path`

The loop works like this:

- inspect adjacent cardinal tiles
- if there is no scaffold left to follow:
  - stop
- if the robot cannot continue forward:
  - turn toward the next scaffold branch
  - add `"L"` or `"R"` to the path
- otherwise:
  - move forward while scaffold continues
  - count the number of steps
  - add that count to the path

So the path becomes a sequence like:

    R,8,L,10,R,8,...

The implementation also marks non-intersection scaffold tiles as open space after they are traversed, helping avoid retracing the same path.

---

### Compressing the Path

`CompressPath()` tries to split the full movement path into three reusable routines.

It works by searching for candidate subpaths:

- `A`
- `B`
- `C`

For each candidate set it checks:

- each routine must fit within 20 characters
- the full path must be representable entirely as a sequence of `A`, `B`, and `C`
- the main movement routine must also fit within 20 characters

When a valid compression is found:

- the three routines are stored in `CompressedPaths`
- the main routine is stored in `Movements`

If no valid compression exists, the method throws an exception.

---

### Constructing the Main Routine

`ConstructPaths(string a, string b, string c, string path)` tries to rebuild the full path by repeatedly matching:

- `a`
- `b`
- `c`

As it matches each segment, it appends:

- `A`
- `B`
- `C`

If the entire path can be consumed this way, it returns the main routine string.

Otherwise it returns:

- `null`

So this method validates whether a chosen set of compressed functions actually covers the whole route.

---

### Executing the Robot Program

`FindRobots()` performs the Part 2 execution.

It first:

- builds the full path
- compresses it

It then feeds input into the Intcode computer in this order:

- main routine
- function `A`
- function `B`
- function `C`
- `"n"` for no live video feed

Each string is converted to ASCII using:

- `IntcodeCpu.StringToAscii(...)`

After the final run, it scans CPU output.

The first value greater than `255` is stored as:

- `DustCollected`

That becomes the gold answer.

---

### Part 1 Return Value

Part 1 returns:

- `AlignmentParameter`

This is the sum of:

    x * y

for every scaffold intersection found in the map.

---

### Part 2 Return Value

Part 2 returns:

- `DustCollected`

This is the final large output value emitted by the Intcode program after the movement routines are executed.

---

## 🛠 Implementation Notes

- The map is built from ASCII Intcode output
- Tiles are stored using the `Entity` enum
- Intersections are scaffold tiles with scaffold on all four sides
- The robot path is built as alternating turn and move tokens
- Path compression searches for three reusable routines
- Both the main routine and subroutines must fit the Intcode input limits
- Part 2 reads the final output value greater than `255` as the dust total

---

## 🧪 Behaviour Summary

Given an Intcode program that emits a scaffold layout:

- the solver runs the CPU and builds a 2D map
- Part 1 detects intersections and sums their alignment parameters
- Part 2 finds the robot, traces the scaffold route, compresses that route into functions, and feeds them back into the CPU
- the final result is either:
  - the total alignment parameter
  - the dust collected after the robot completes its route

---

## 🚀 Key Takeaways

- Nice combination of Intcode execution and grid parsing
- The map is modelled cleanly with coordinate-based storage
- Intersections are detected with simple neighbour checks
- The robot path is derived directly from scaffold connectivity
- Compression into `A`, `B`, and `C` is the key extra challenge in Part 2
- The final dust result comes from the last non-ASCII Intcode output

---

## 🔗 References

- https://adventofcode.com/2019/day/17