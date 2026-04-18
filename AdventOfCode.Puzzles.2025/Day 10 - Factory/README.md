# 🎄 Advent of Code 2025 - Day 10: Factory

## 📜 Puzzle Overview

This puzzle models a factory full of broken machines that must be configured using button presses.

Each machine consists of:

- a row of indicator lights
- a set of buttons, where each button affects specific lights
- (Part 2) a set of joltage counters

Pressing a button affects multiple outputs at once:

- in Part 1, buttons toggle lights on/off
- in Part 2, buttons increment numeric counters

The goal is to determine the **minimum number of button presses** required to reach a desired configuration.

---

## 🧩 Part 1

Determine the minimum number of button presses required to match the target light pattern for each machine.

### 💡 Approach

- Parse each machine's:
  - target light configuration
  - button wiring definitions
- Model each button as toggling specific light indices
- Treat the system as a **binary (on/off) state problem**
- Explore combinations of button presses:
  - using BFS, DFS, or bitmasking
- Find the minimum sequence of presses that transforms all lights to the target pattern
- Sum or return the required presses based on puzzle requirements

This behaves similarly to a **Lights Out** style puzzle using XOR logic.

---

## 🧩 Part 2

Determine the minimum number of button presses required to reach the exact joltage targets.

### 💡 Approach

- Reuse the same machine definitions
- Instead of toggling, each button now **adds values** to counters
- Model the system as a **linear equation problem**:

  - each button contributes a vector
  - total presses form a solution vector

- Solve:

  - `A * x = b`

  where:
  - `A` = button effects
  - `x` = number of presses per button
  - `b` = target joltages

- Use:

  - Gaussian elimination
  - integer solving
  - or constraint solving

- Find the **minimum valid non-negative solution**

This turns the puzzle into a **linear algebra / Diophantine equation problem**.

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Factory`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new `Factory` instance
- Calls the Part 1 solver method

For Part 2:

- Creates a new `Factory` instance
- Calls the Part 2 solver method

---

### `Factory.cs`

This class contains the full logic for:

- parsing machine definitions
- building button-effect mappings
- solving both puzzle parts

It typically stores:

- machine configurations
- button mappings
- target outputs

The constructor:

- parses each input line
- extracts:
  - light patterns
  - button definitions
  - joltage targets

---

### Parsing the Input

Each line describes a machine and contains:

- a light pattern (e.g. `#` and `.`)
- multiple button definitions
- (Part 2) a joltage target list

Parsing involves:

- stripping surrounding brackets
- splitting sections into components
- converting:
  - light patterns into indices or bitmasks
  - button definitions into lists of affected indices
  - joltages into integers

At a high level:

- lights define the target state
- buttons define transformations
- joltages define numeric constraints

---

### Building Button Effects

Each button is mapped to:

- a set of indices it affects

For Part 1:

- toggling means flipping bits (XOR)

For Part 2:

- each button contributes a numeric vector

This allows:

- fast simulation of button presses
- matrix construction for solving

---

### Part 1 Logic

The Part 1 solver:

- represents the light state as a bitmask or boolean array
- starts from an initial state (usually all off)
- applies button presses to reach the target pattern

It then:

- explores possible press combinations
- tracks visited states to avoid repetition
- finds the minimum number of presses required

This is effectively:

- a shortest-path search in state space
- or a linear system over GF(2)

---

### Part 2 Logic

The Part 2 solver:

- models the system as a matrix equation

Each button contributes to:

- one or more joltage values

The solver:

- constructs matrix `A`
- constructs target vector `b`
- solves for vector `x` (button presses)

It then:

- ensures all values in `x` are non-negative integers
- minimizes total presses

The final result is derived from:

- the sum (or required calculation) of the press vector

---

## 🛠 Implementation Notes

- Input parsing extracts structured machine definitions
- Button mappings drive both puzzle parts
- Part 1 operates in a binary toggle system (XOR logic)
- Part 2 operates in an integer linear system
- Efficient solving requires:
  - pruning or BFS for Part 1
  - linear algebra for Part 2

---

## 🧪 Behaviour Summary

Given a set of machines:

- each machine defines a target configuration
- buttons act as transformations on that configuration
- Part 1 searches for the minimal toggle sequence
- Part 2 solves a numeric system of equations
- both parts aim to minimize total button presses

---

## 🚀 Key Takeaways

- Classic **Lights Out** style problem in Part 1
- Transition to **linear algebra** in Part 2
- Demonstrates how the same input can represent:
  - boolean systems
  - numeric systems
- Highlights the importance of:
  - state modeling
  - matrix solving techniques

---

## 🔗 References

- https://adventofcode.com/2025/day/10