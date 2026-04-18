# 🎄 Advent of Code 2024 - Day 14: Restroom Redoubt

## 📜 Puzzle Overview

This puzzle tracks a swarm of robots moving around a rectangular bathroom floor.

Each input line defines one robot with:

- a starting position
- a velocity

An input line looks like this:

    p=0,4 v=3,-3

The robots move once per second and wrap around the edges of the map, so stepping past one edge teleports them to the opposite side.

The solver models each robot with a position and velocity, simulates movement on a fixed grid, and then solves two different tasks:

- Part 1 calculates a safety factor after 100 seconds
- Part 2 finds the number of seconds until the robots form the hidden Christmas tree pattern

---

## 🧩 Part 1

Determine the safety factor after exactly 100 seconds.

### 💡 Approach

- Parse every input line into a robot with:
  - current position
  - velocity
- Use the puzzle dimensions:
  - width = 101
  - height = 103
- Move every robot forward one step per second for 100 seconds
- After movement completes, count how many robots are in each of the four quadrants
- Ignore any robot exactly on the middle row or middle column
- Multiply the four quadrant counts together to get the safety factor

---

## 🧩 Part 2

Determine when the robots form the Christmas tree pattern.

### 💡 Approach

- Reuse the same robot simulation
- Move all robots forward one second at a time
- After each step, collect all robot positions into a fast lookup set
- For each robot, test whether surrounding positions form a symmetric triangular pattern underneath it
- As soon as that pattern is found, return the number of elapsed seconds

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Restroom Redoubt`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new RestroomRedoubt(this.Input)`
- Calls `SafetyFactor(100)`

For Part 2:

- Creates `new RestroomRedoubt(this.Input)`
- Calls `XmasTree()`

---

### `Robot.cs`

This class models a single robot.

It stores:

- `Point`
- `Velocity`

The constructor:

- removes the `p=` and `v=` labels
- splits the line into position and velocity sections
- parses both coordinate pairs into vectors

So a line such as:

    p=0,4 v=3,-3

becomes one fully initialised robot instance.

---

### Robot Movement

`Move(int width, int height)` advances the robot by its velocity.

At a high level it does:

    x = (x + vx) % width
    y = (y + vy) % height

If either coordinate becomes negative after modulo wrapping, it adds the grid width or height back on.

This means robots wrap cleanly around the map instead of stopping at the boundaries.

---

### `RestroomRedoubt.cs`

This class contains the main simulation logic.

It stores:

- `Robots`
- `Width`
- `Height`
- `CenterX`
- `CenterY`

The constructor:

- parses every input line into a `Robot`
- stores them in a `HashSet<Robot>`
- sets:
  - `Width = 101`
  - `Height = 103`
- calculates the centre lines used for quadrant checks

---

### Initialising the Grid

The constructor calculates the centre lines with:

    CenterX = ceil(width / 2) - 1
    CenterY = ceil(height / 2) - 1

These centre coordinates define the vertical and horizontal middle lines.

Any robot landing exactly on one of those lines is excluded from quadrant counting in Part 1.

---

### Moving All Robots

`MoveAll()` handles one full simulation tick.

It simply:

- loops through every robot
- calls `robot.Move(this.Width, this.Height)`

Both puzzle parts reuse this same shared movement step.

---

### Part 1 Simulation

`SafetyFactor(int seconds)` runs the simulation for the requested duration.

It:

- loops from `1` to `seconds`
- calls `MoveAll()` each time

After the simulation finishes, it counts robots in four quadrants:

- top-left
- top-right
- bottom-left
- bottom-right

The logic compares each robot against `CenterX` and `CenterY` and increments one of:

- `q1`
- `q2`
- `q3`
- `q4`

Finally it returns:

    q1 * q2 * q3 * q4

So the silver answer is the product of the robot counts in all four quadrants after 100 seconds.

---

### Part 2 Pattern Search

`XmasTree()` searches for the first second where the robots form the expected tree-like arrangement.

It tracks:

- `result = 0`

Then repeatedly:

- moves all robots forward once
- increments the elapsed second counter
- builds a `HashSet<Vector<int>>` of all robot positions

For each robot, it checks whether a specific set of downward diagonal offsets also exists.

Those offsets are:

    (-1,1), (1,1)
    (-2,2), (2,2)
    (-3,3), (3,3)
    (-4,4), (4,4)

This means the solver is looking for a symmetric widening shape beneath a robot, which acts as the signal that the Christmas tree has appeared.

As soon as one robot satisfies all of those offset checks, the method returns the current elapsed second count.

---

### Why the Part 2 Check Works

The gold solution does not render the map or search visually.

Instead, it recognises the tree by checking for a structure like this conceptually:

- one point at the top
- matching points one row lower
- matching points two rows lower
- continuing outward symmetrically

That creates a compact triangular signature which is treated as the tree marker.

---

## 🛠 Implementation Notes

- Robots are parsed directly from lines like `p=x,y v=x,y`
- Movement uses wrapping modulo arithmetic
- Negative wrapped positions are corrected manually
- Part 1 runs for exactly 100 seconds
- Part 1 ignores robots on the central horizontal or vertical line
- Part 2 searches for a symmetric tree-shaped arrangement using offset matching
- Robot positions are collected into a hash set for fast Part 2 lookups

---

## 🧪 Behaviour Summary

Given a list of moving robots on a wrapping grid:

- the solver parses each robot's start position and velocity
- each tick moves every robot with edge wrapping
- Part 1 simulates 100 seconds and multiplies the quadrant populations
- Part 2 simulates until a Christmas tree pattern is detected
- both parts rely on the same shared movement logic

---

## 🚀 Key Takeaways

- Nice example of 2D movement on a toroidal grid
- Wrapping behaviour is handled cleanly with modulo arithmetic
- Part 1 reduces the final layout to quadrant counts
- Part 2 uses pattern detection instead of brute-force rendering
- Shared simulation logic keeps both puzzle parts simple

---

## 🔗 References

- https://adventofcode.com/2024/day/14