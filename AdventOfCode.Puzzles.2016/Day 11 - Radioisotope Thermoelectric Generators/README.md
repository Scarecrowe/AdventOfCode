# 🎄 Advent of Code 2016 - Day 11: Radioisotope Thermoelectric Generators

## 📜 Puzzle Overview

This puzzle models a multi-floor facility containing:

- microchips
- generators

An elevator is used to move items between floors with the goal of bringing everything to the top floor.

### Rules

- The elevator can carry **one or two items**
- It can move **up or down one floor at a time**
- A microchip is **fried** if:
  - it is on the same floor as any generator
  - **unless** its corresponding generator is also present

The challenge is to find the **minimum number of steps** required to move all items safely to the top floor.

Part 2 increases the number of items, making the search space significantly larger.

---

## 🧩 Part 1

Determine the minimum number of steps to bring all items to the top floor.

### 💡 Approach

- Represent the state of the building:
  - elevator position
  - positions of all generators and microchips
- Use a search algorithm (typically BFS):
  - explore all valid moves level-by-level
- Generate possible moves:
  - move one or two items
  - move up or down
- Validate each resulting state:
  - ensure no microchips are fried
- Stop when all items are on the top floor

---

## 🧩 Part 2

Repeat the process with additional items added to the initial state.

### 💡 Approach

- Extend the initial configuration with extra generator/microchip pairs
- Use the same search strategy as Part 1
- Optimisation becomes critical due to increased state space:
  - avoid revisiting equivalent states
  - normalise states to reduce duplicates

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Radioisotope Thermoelectric Generators`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Builds initial state
- Executes search to find minimum steps

For Part 2:

- Extends initial state
- Re-runs the search with optimisations

---

### State Representation

A state typically includes:

- current elevator floor
- positions of each generator
- positions of each microchip

Example concept:

```
Elevator: Floor 1
Hydrogen Generator: Floor 2
Hydrogen Microchip: Floor 1
```

---

### Valid State Rules

A state is valid if:

- for every microchip:
  - it is either:
    - with its generator
    - or not sharing a floor with any other generator

---

### Generating Moves

From a given state:

- choose one or two items on the current floor
- attempt to move:
  - up (preferred)
  - down (when necessary)
- create new states for each valid move

---

### Breadth-First Search

- Use BFS to guarantee shortest path
- Maintain a queue of states
- Track visited states to avoid repetition
- Stop when goal state is reached

---

### State Normalisation

To reduce duplicate states:

- treat equivalent configurations as identical
- ignore specific element names where possible
- compare relative positions instead of labels

This dramatically reduces the search space.

---

### Part 1 Logic

- Initialise starting configuration
- Perform BFS
- Return number of steps when all items reach top floor

---

### Part 2 Logic

- Add extra item pairs to initial state
- Apply same BFS logic
- Use optimisations to keep runtime manageable

---

## 🛠 Implementation Notes

- State comparison is critical for performance
- BFS can grow large without pruning
- Normalisation significantly reduces complexity
- Moving two items up is often optimal
- Avoid unnecessary downward moves

---

## 🧪 Behaviour Summary

Given an initial building layout:

- Elevator moves items between floors
- Safety rules restrict valid configurations
- BFS explores all valid states
- Part 1 finds shortest solution
- Part 2 increases complexity with more items

---

## 🚀 Key Takeaways

- Classic state-space search problem
- BFS guarantees shortest path
- State normalisation is key optimisation
- Constraint validation drives correctness
- Problem complexity grows rapidly with added elements

---

## 🔗 References

- https://adventofcode.com/2016/day/11