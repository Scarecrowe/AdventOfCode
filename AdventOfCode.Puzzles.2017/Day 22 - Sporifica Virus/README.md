# 🎄 Advent of Code 2017 - Day 22: Sporifica Virus

## 📜 Puzzle Overview

This puzzle simulates a virus carrier moving across an infinite 2D grid.

Each node in the grid can be in a state:

- clean
- infected

The input provides an initial square grid, centred on the virus carrier.

Example:

    ..#
    #..
    ...

The carrier starts:

- in the middle of the grid
- facing up

Each step (called a "burst") follows a strict set of rules depending on the current node state.

The goal is to simulate these bursts and count how many times a node becomes infected.

The grid is effectively infinite, so new nodes default to `clean` when first visited.

---

## 🧩 Part 1

Count how many bursts cause a node to become infected after 10,000 bursts.

### 💡 Approach

For each burst:

- if current node is **infected**:
  - turn right
  - clean the node
- if current node is **clean**:
  - turn left
  - infect the node (count this)
- move forward one step

Repeat this process 10,000 times and count how many times a node flips from clean -> infected.

---

## 🧩 Part 2

Extend the simulation with more node states and run for 10,000,000 bursts.

### 💡 Approach

Each node now has four states:

- clean
- weakened
- infected
- flagged

Updated rules:

- **clean**:
  - turn left
  - become weakened
- **weakened**:
  - do not turn
  - become infected (count this)
- **infected**:
  - turn right
  - become flagged
- **flagged**:
  - reverse direction
  - become clean

Then:

- move forward one step

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sporifica Virus`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- runs the simulation for 10,000 bursts

For Part 2:

- runs the simulation for 10,000,000 bursts with extended states

---

### Grid Representation

The grid is conceptually infinite.

Instead of storing a full 2D array, the solver typically uses:

- a dictionary or hash set keyed by coordinates

This allows:

- efficient storage of only visited nodes
- defaulting unseen nodes to `clean`

---

### Carrier State

The virus carrier tracks:

- current position `(x, y)`
- current direction (up, right, down, left)

Direction changes are handled via:

- rotation logic (left, right, reverse)

---

### Movement Logic

Each burst follows this structure:

    determine current node state
    update direction
    update node state
    move forward

This order is important:

- turning happens before movement
- state changes affect infection counting

---

### Infection Counting

A counter is incremented when:

- a node transitions into the `infected` state

In Part 1:

- this happens when a clean node becomes infected

In Part 2:

- this happens when a weakened node becomes infected

---

### Direction Handling

Typical implementations use:

- an enum for directions
- or vector offsets like:

    up    = (0, -1)
    right = (1, 0)
    down  = (0, 1)
    left  = (-1, 0)

Turning is handled by:

- rotating through these directions
- or using index arithmetic

---

### State Transitions

#### Part 1

Simple toggle:

    clean <-> infected

#### Part 2

State cycle:

    clean -> weakened -> infected -> flagged -> clean

---

### Simulation Loop

Part 1:

    10,000

iterations

Part 2:

    10,000,000

iterations

Each iteration performs:

- direction change
- state update
- movement
- optional infection count increment

---

## 🛠 Implementation Notes

- The grid must support negative coordinates
- Using a dictionary avoids large sparse arrays
- Direction handling is critical for correctness
- Part 2 significantly increases complexity with extra states
- Performance matters for 10 million iterations

---

## 🧪 Behaviour Summary

Given an initial infected grid:

- the carrier starts at the centre facing up
- each burst modifies direction and node state
- the carrier moves forward after each burst
- Part 1 uses two states (clean/infected)
- Part 2 uses four states (clean/weakened/infected/flagged)
- the result is the number of times infection occurs

---

## 🚀 Key Takeaways

- Classic example of grid traversal with state machines
- Very similar to a 2D Turing machine or Langton's Ant behaviour
- Sparse data structures are ideal for infinite grids
- Clean separation between movement, state transitions, and counting
- Part 2 expands complexity without changing the core loop

---

## 🔗 References

- https://adventofcode.com/2017/day/22