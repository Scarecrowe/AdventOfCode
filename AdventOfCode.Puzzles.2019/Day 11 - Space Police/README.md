# 🎄 Advent of Code 2019 - Day 11: Space Police

## 📜 Puzzle Overview

This puzzle controls a painting robot driven by an Intcode program.

The robot moves around an infinite grid of hull panels.

Each panel can be:

- black
- white

The robot repeatedly does the following:

- reads the colour of its current panel
- sends that colour into the Intcode computer
- receives two output values
- paints the current panel
- turns left or right
- moves forward one step

Part 1 asks how many panels are painted at least once. Part 2 starts on a white panel and renders the final painted registration identifier.

---

## 🧩 Part 1

Determine how many hull panels are painted at least once.

### 💡 Approach

- Create the robot and Intcode CPU
- Start the robot at `(0,0)`
- Set the initial panel colour to black
- Run the Intcode program until it terminates
- For each cycle:
  - read the current panel colour
  - feed that into the CPU
  - read two outputs:
    - paint colour
    - turn direction
  - paint the current panel
  - turn and move forward
- Count how many distinct panel positions were stored in the hull map

---

## 🧩 Part 2

Paint the hull starting from a white panel and render the final image.

### 💡 Approach

- Reuse the same painting logic
- Start the robot on a white panel instead of black
- Let the program paint the hull to completion
- Find the bounding box of all painted panels
- Print each row:
  - black panel as a space
  - white panel as `#`

This reveals the registration number as block text.

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Space Police`
- Loads the puzzle input
- Uses the first input line as the Intcode program

For Part 1:

- Creates `new SpacePolice(this.Input[0])`
- Calls `PaintHull(0, 0, PaintColour.Black)`
- Returns `Hull.Count`

For Part 2:

- Creates `new SpacePolice(this.Input[0])`
- Calls `PaintHull(0, 1, PaintColour.White)`
- Calls `Print()`

So the two parts differ only by:

- starting position
- initial panel colour
- whether the result is counted or rendered

---

### `PaintColour.cs`

This enum defines the possible panel colours.

It contains:

- `Black`
- `White`

These values are also sent into and read back from the Intcode program.

---

### `RobotTurn.cs`

This enum defines the turn instruction returned by the Intcode program.

It contains:

- `Left`
- `Right`

After painting a panel, the robot uses this output to change direction.

---

### `Robot.cs`

This class models the painting robot itself.

It stores:

- `Point`
- `Direction`
- `Cpu`

The constructor:

- sets the starting point to `(0,0)`
- sets the starting direction to `North`
- creates a new `IntcodeCpu` from the input program

So the robot always begins facing upward before any painting starts.

---

### Robot Turning and Movement

`Turn(RobotTurn turn)` handles both rotation and movement.

It first updates the direction using:

- `CardinalHelper.AntiClockwise` for `Left`
- `CardinalHelper.Clockwise` for `Right`

It then immediately moves one step forward using the direction transform map.

Logically that means:

    rotate
    move forward 1 tile

So the method both turns the robot and advances it to the next panel.

---

### Setting the Robot Position

`SetPosition(Vector point)` directly places the robot at a given coordinate.

This is used by the hull painter before execution begins so each puzzle mode can start from a chosen position.

---

### `SpacePolice.cs`

This class contains the full painting and rendering logic.

It stores:

- `Robot`
- `Hull`

The constructor:

- creates a new `Robot`
- creates an empty hull dictionary

The hull is stored in a `VectorDictionary`, keyed by panel position.

---

### Hull Representation

The hull map records only panels that have been painted or explicitly initialised.

That means:

- each key is a grid coordinate
- each value is a `PaintColour`

If a panel is missing from the dictionary, it is treated as black by default.

---

### Main Painting Loop

`PaintHull(int x, int y, PaintColour initialColour)` performs the full simulation.

It first:

- resets the Intcode CPU
- clears the hull map
- sets the robot position
- adds the starting panel with the supplied initial colour

Then it loops until:

- `this.Robot.Cpu.State == IntcodeCpuState.Terminated`

On each iteration it:

- checks whether the current panel has already been visited
- reads the current panel colour, defaulting to black if absent
- enqueues that colour into the CPU input
- runs the CPU
- dequeues two outputs:
  - new paint colour
  - turn instruction
- updates or adds the current panel in the hull
- turns and moves the robot

So one complete robot action cycle is:

    read panel
    send input
    run CPU
    paint
    turn
    move

---

### CPU Interaction

The robot communicates with the Intcode computer through queues.

Input:

- current panel colour as an integer

Output:

- first output = panel colour to paint
- second output = turn direction

The code casts those output integers into:

- `PaintColour`
- `RobotTurn`

This keeps the painting logic readable and strongly typed.

---

### Painting Logic

When the robot receives a paint instruction:

- if the current panel already exists in the hull map:
  - overwrite its colour
- otherwise:
  - add it as a new painted panel

This means `Hull.Count` represents how many unique panels were ever tracked during the run.

That is the value returned for Part 1.

---

### Rendering the Registration Number

`Print()` converts the hull into text output.

It first calculates the bounds of all painted coordinates:

- minimum X
- maximum X
- minimum Y
- maximum Y

It then loops through every coordinate in that rectangle.

For each panel:

- if the panel exists and is black:
  - print a space
- if the panel exists and is white:
  - print `#`
- if the panel does not exist:
  - print a space

The result is returned as a multi-line string with blank lines before and after the image.

---

### Part 1 Return Value

Part 1 returns:

- `Hull.Count`

after painting completes from a black starting panel.

So the silver answer is the number of unique hull panels the robot painted.

---

### Part 2 Return Value

Part 2 returns:

- the rendered hull image from `Print()`

after painting completes from a white starting panel.

So the gold answer is a visible registration pattern rather than a numeric total.

---

## 🛠 Implementation Notes

- The robot starts facing north
- Turning also performs the forward movement
- The hull is stored as a coordinate-to-colour dictionary
- Missing panels are treated as black
- The Intcode program outputs exactly two values per robot action
- Part 1 starts from black
- Part 2 starts from white
- The final image is rendered using spaces and `#`

---

## 🧪 Behaviour Summary

Given an Intcode-controlled painting robot:

- the solver initialises the robot and hull map
- the robot reads the current panel colour
- the Intcode program outputs a paint instruction and turn instruction
- the robot paints the panel, turns, and moves forward
- this continues until the program halts
- Part 1 counts painted panels
- Part 2 prints the painted registration pattern

---

## 🚀 Key Takeaways

- Nice example of combining Intcode execution with grid movement
- The robot state is cleanly separated from the hull painting logic
- Enums make the paint and turn outputs much clearer
- Using a coordinate dictionary keeps the infinite grid efficient
- Part 2 is just a different initial condition plus rendering
- The solution stays compact by reusing the same core painting loop

---

## 🔗 References

- https://adventofcode.com/2019/day/11