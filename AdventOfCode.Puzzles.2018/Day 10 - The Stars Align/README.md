# 🎄 Advent of Code 2018 - Day 10: The Stars Align

## 📜 Puzzle Overview

This puzzle simulates points of light moving through space until they align into readable text.

Each input line defines a point with:

- position (x, y)
- velocity (vx, vy)

An example line looks like:

    position=< 9,  1> velocity=< 0,  2>

Over time, each point moves according to its velocity, and at a certain moment they cluster into a message in the sky.

Part 1 determines what message appears when the points align.  
Part 2 determines how many seconds it takes for that alignment to occur.

---

## 🧩 Part 1

Determine the message formed when the points align.

### 💡 Approach

- Parse each line into a `Point` object
- Store:
  - position
  - velocity
- Simulate movement over time
- At each tick:
  - update all point positions using velocity
  - compute the bounding box of all points
- Detect the moment when points are most tightly clustered
- Render the points at that moment into a grid
- Read the resulting letters from the grid

---

## 🧩 Part 2

Determine how many seconds pass before the message appears.

### 💡 Approach

- Use the same simulation as Part 1
- Track each iteration (second)
- Measure how “tight” the point cluster is using:
  - width of bounding box
  - height of bounding box
- The correct moment is when the bounding box is at its minimum
- Return the number of seconds at that point

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Stars Align`
- Loads input data
- Calls both puzzle parts

For Part 1:

- Runs simulation until alignment is detected
- Renders final message

For Part 2:

- Returns the timestamp of alignment

---

### `Point.cs`

This class represents a moving light point.

It stores:

- `X`, `Y` → current position
- `VX`, `VY` → velocity components

Each point updates every tick using:

    X += VX
    Y += VY

So each second moves the point linearly in 2D space.

---

### Point Movement

Movement is deterministic:

- velocity never changes
- position accumulates over time

So the motion equation is effectively:

    position(t) = position + velocity * t

This makes simulation straightforward: just step forward second by second.

---

### Detecting Alignment

The key insight in this puzzle is that the message appears when:

- the points are most tightly packed

To detect this, the solver tracks:

- minimum bounding box area (or width/height spread)

At each tick:

- find minX, maxX, minY, maxY
- compute spread:

    width = maxX - minX  
    height = maxY - minY  

When this value is smallest, the message is forming.

---

### Rendering the Message (Part 1)

Once the correct time is found:

- build a grid from min/max bounds
- mark each point position with `#`
- empty space with `.`
- read the resulting ASCII letters visually

This is where the hidden message appears.

---

### Finding the Correct Time (Part 2)

The solver runs a loop like:

- simulate 1 second
- compute bounding box size
- store best (smallest) configuration
- continue until pattern starts expanding again

The time at the minimum spread is the answer.

---

### Simulation Loop

At a high level:

- initialise all points
- repeat:
  - move all points
  - compute bounding box
  - check if this is the smallest seen so far
  - increment seconds

Stop condition is typically based on:

- bounding box starting to grow again after minimum

---

## 🛠 Implementation Notes

- Points are independent and do not interact
- No collision handling is required
- Velocity is constant throughout simulation
- Bounding box heuristics are key to solving efficiently
- The message is only visible at a very specific moment
- Rendering is typically done once the optimal time is found

---

## 🧪 Behaviour Summary

Given a set of moving points:

- each point shifts by its velocity every second
- the system is simulated step-by-step
- the points gradually converge into a tight cluster
- at peak convergence, they form readable text
- after that, they diverge again

Part 1 extracts the message.  
Part 2 returns the time of formation.

---

## 🚀 Key Takeaways

- Classic physics simulation with constant velocity vectors
- No complex math needed beyond bounding box tracking
- The key optimisation is detecting convergence instead of brute-forcing indefinitely
- ASCII rendering reveals the hidden message
- Elegant example of pattern emergence in simulation data

---

## 🔗 References

- https://adventofcode.com/2018/day/10