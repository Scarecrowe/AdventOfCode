# 🎄 Advent of Code 2019 - Day 13: Care Package

## 📜 Puzzle Overview

This puzzle uses the Intcode computer to run a small arcade game.

The program outputs values in groups of three:

- `x` position
- `y` position
- tile id

These triples describe what should appear on the screen.

The tile types are:

- `0` → empty
- `1` → wall
- `2` → block
- `3` → horizontal paddle
- `4` → ball

In Part 1, the goal is to count how many block tiles are present after the program draws the screen.

In Part 2, the game is switched into free play mode and the solver automatically controls the paddle to follow the ball.

---

## 🧩 Part 1

Count how many block tiles are shown on the screen.

### 💡 Approach

- Run the Intcode program once
- Read all output values in groups of three
- Convert each triple into a screen update
- Store each tile in a display dictionary using its `(x, y)` position
- Count how many stored tiles are `Block`

---

## 🧩 Part 2

Play the game automatically and return the final score.

### 💡 Approach

- Set memory position `0` to `2` to enable free play
- Run the Intcode program
- Read output triples into the display
- Track:
  - the ball position
  - the paddle position
  - the current score
- Repeatedly choose joystick input:
  - move right if the paddle is left of the ball
  - move left if the paddle is right of the ball
  - stay neutral if aligned
- Continue until the Intcode CPU terminates
- Return the final score

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Care Package`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new CarePackage(this.Input[0])`
- Calls `Play()`
- Calls `CountBlocks()`

For Part 2:

- Creates `new CarePackage(this.Input[0])`
- Calls `Play(2)`
- Returns `Score`

---

### `CarePackage.cs`

This class contains the arcade game logic.

It stores:

- `Cpu`
- `Display`
- `Score`

The constructor:

- creates a new `IntcodeCpu`
- creates a new display dictionary

So the game state is maintained through the Intcode CPU output and a coordinate-based tile map.

---

### Program Setup

The constructor takes the Intcode program as a single string:

    new CarePackage(this.Input[0])

It then initialises:

    this.Cpu = new(program);
    this.Display = new();

So the puzzle input is expected to be one comma-separated Intcode program line.

---

### Tile Definitions

`Tile.cs` defines the possible tile ids:

- `Empty = 0`
- `Wall = 1`
- `Block = 2`
- `Paddle = 3`
- `Ball = 4`

These values are used when decoding the Intcode output into the screen display.

---

### Joystick Input

`Joystick.cs` defines the control input values:

- `Neutral = 0`
- `Left = -1`
- `Right = 1`

These values are pushed into the Intcode CPU input queue during Part 2.

---

### Running the Game

`Play(int quaters = 1)` handles both puzzle parts.

It begins by writing the supplied value into memory position `0`:

    this.Cpu.Memory.Write(0, quaters);

Then it runs the CPU and builds the display:

    this.Cpu.Run();
    this.GetDisplay();

When called with the default value `1`, it simply returns the populated game state for Part 1.

When called with `2`, it enables free play mode and continues running the game with automatic joystick input.

---

### Reading the Display Output

`GetDisplay()` consumes CPU output values in groups of three.

It builds triples like:

- x
- y
- tile value

At a high level the logic is:

    while output exists
        read 3 values
        interpret as x, y, value

For normal screen updates:

- `(x, y)` becomes the display key
- the tile id becomes the stored tile type

If the coordinate is:

    (-1, 0)

then the third value is not a tile at all.

Instead, it is treated as the current score.

---

### Display Storage

The screen is stored in:

- `VectorDictionary Display`

Each coordinate maps to a `Tile`.

If a position already exists, it is updated.
If it does not yet exist, it is added.

So the display always reflects the latest known game state after each batch of Intcode output has been processed.

---

### Counting Block Tiles

`CountBlocks()` is used for Part 1.

It returns:

    this.Display.Count(c => c.Value == Tile.Block)

So the silver answer is the number of display cells currently containing block tiles after the program finishes drawing the screen.

---

### Finding the Ball and Paddle

The auto-player relies on two helper methods:

- `GetBall()`
- `GetPaddle()`

These search the display for the current positions of:

- `Tile.Ball`
- `Tile.Paddle`

The returned coordinates are then compared to decide how the joystick should move.

---

### Automatic Paddle Control

In free play mode, the solver keeps running until the CPU terminates.

After each execution step it:

- refreshes the display
- finds the ball position
- finds the paddle position

Then it chooses joystick input like this:

    if paddle.X < ball.X
        move right
    else if paddle.X > ball.X
        move left
    else
        stay neutral

This makes the paddle continuously follow the horizontal position of the ball.

---

### Part 2 Game Loop

The free play loop works like this logically:

    enqueue neutral
    while cpu is not terminated
        run cpu
        update display
        inspect ball and paddle
        enqueue next joystick move

The initial neutral input is queued before the loop starts so the game can begin processing input immediately.

---

### Printing the Screen

`Print()` renders the current game board to the console.

It:

- calculates the min and max display bounds
- maps tiles to characters
- writes score and coordinates
- draws the board row by row

The tile rendering uses:

- empty → space
- ball → `.`
- wall → `#`
- paddle → `-`
- block → `^`

This appears to be a debugging or visualisation helper rather than something required for the final puzzle answers.

---

## 🛠 Implementation Notes

- The puzzle input is passed in as a single Intcode program string
- Output is processed in triples of `x`, `y`, and tile or score value
- The display is stored as a coordinate-to-tile dictionary
- Score updates are identified by the special coordinate `(-1, 0)`
- Part 1 counts `Block` tiles after the initial full draw
- Part 2 writes `2` to memory position `0` to enable free play
- The paddle is controlled automatically by comparing its `X` position to the ball's `X` position
- The method parameter is named `quaters` in the implementation

---

## 🧪 Behaviour Summary

Given one Intcode arcade program:

- the solver runs the program and reads screen updates in triples
- each triple either updates a tile or updates the score
- Part 1 builds the display and counts block tiles
- Part 2 switches to free play mode
- the paddle is steered automatically toward the ball
- the final result is either:
  - the number of block tiles
  - or the ending score after the game finishes

---

## 🚀 Key Takeaways

- Good example of interpreting streamed Intcode output as structured screen data
- The display logic is cleanly separated from the CPU execution
- Part 1 is a straightforward tile counting problem once the output is decoded
- Part 2 uses a simple but effective auto-player strategy
- The score handling and tile updates are both managed through the same output processing pipeline

---

## 🔗 References

- https://adventofcode.com/2019/day/13