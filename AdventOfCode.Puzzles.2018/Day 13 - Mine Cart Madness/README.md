# 🎄 Advent of Code 2018 - Day 13: Mine Cart Madness

## 📜 Puzzle Overview

This puzzle simulates mine carts moving on a 2D track system.

The input is a grid containing:

- straight tracks: `|` and `-`
- curves: `/` and `\`
- intersections: `+`
- carts: `^`, `v`, `<`, `>`

Example input:

    /->-\
    |   |  /----\
    | /-+--+-\  |
    | | |  | v  |
    \-+-/  \-+--/
      \------/

Each cart moves step-by-step along the track, following strict movement rules.

- Carts move in reading order (top to bottom, left to right)
- Each tick, every cart moves exactly once
- Collisions remove carts (Part 1 uses first crash, Part 2 removes carts until one remains)

---

## 🧩 Part 1

Find the location of the first crash.

### 💡 Approach

- Parse the grid into:
  - track layout
  - list of carts with positions and directions
- Simulate movement tick by tick:
  - sort carts in reading order
  - move each cart one step
  - update direction based on track piece
  - detect collisions immediately after movement
- First collision encountered ends the simulation
- Return crash coordinates `(x, y)`

---

## 🧩 Part 2

Find the location of the last remaining cart.

### 💡 Approach

- Use the same simulation loop as Part 1
- Instead of stopping on first crash:
  - remove both carts involved in a collision
- Continue simulation until only one cart remains
- Return the final cart position

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets title to `Mine Cart Madness`
- Loads input grid
- Calls both solver parts

For Part 1:

- Runs simulation until first collision
- Returns crash position

For Part 2:

- Runs full simulation with cart removal
- Returns last remaining cart position

---

### Core Data Model

Each cart typically stores:

- `X`, `Y` → position
- `Direction` → one of `^ v < >`
- `TurnState` → cycles through:
  - left → straight → right → repeat

Each tick:

1. move cart one step
2. update direction based on track:
   - `/` and `\` curves
   - `+` intersection logic
3. handle collisions (depending on part)

---

### Track Behaviour

Each track tile modifies cart direction:

#### Straight tracks

- `-` and `|` → no change

#### Curves

- `/` and `\` → rotate direction depending on entry direction

#### Intersections

At `+`, carts rotate based on internal cycle:

1. turn left
2. go straight
3. turn right
4. repeat

This state is stored per cart and advances after each intersection.

---

### Movement Logic

Each tick:

- carts are sorted in reading order
- each cart moves:

    position += direction

Then track logic is applied:

- adjust direction if curve
- adjust direction + cycle if intersection

---

### Collision Detection (Part 1)

After each cart moves:

- check if any other cart is already at the same position
- if collision occurs:
  - immediately return that coordinate

This makes Part 1 a “first crash wins” simulation.

---

### Collision Handling (Part 2)

Instead of stopping:

- mark both carts as crashed
- remove them from active simulation
- continue processing remaining carts in the same tick

Important detail:

- carts are still processed in correct order
- collisions can chain within a single tick

---

### Simulation Loop

High-level structure:

- while more than one cart remains:
  - sort carts by Y then X
  - move each cart
  - resolve direction changes
  - resolve collisions
  - remove crashed carts

When only one cart remains:

- return its position

---

## 🛠 Implementation Notes

- Reading order sorting is critical for correct behaviour
- Direction updates depend on both track type and previous state
- Intersection logic is stateful per cart (turn cycle)
- Collision detection differs between Part 1 and Part 2
- Removing carts mid-tick must be handled carefully to avoid skipping updates
- Curves (`/` and `\`) behave differently depending on entry direction

---

## 🧪 Behaviour Summary

Given a grid of tracks and carts:

- carts move one step per tick in strict order
- track pieces modify direction dynamically
- collisions occur when two carts occupy the same position
- Part 1 returns the first crash coordinate
- Part 2 removes crashed carts until only one remains
- final result depends entirely on simulation state evolution

---

## 🚀 Key Takeaways

- Classic grid-based simulation with ordered updates
- Direction system is stateful and rule-driven
- Reading-order processing is essential for correctness
- Two distinct collision models:
  - stop at first crash (Part 1)
  - eliminate and continue (Part 2)
- Demonstrates careful handling of mutable state during iteration

---

## 🔗 References

- https://adventofcode.com/2018/day/13