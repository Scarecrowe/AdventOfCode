# 🎄 Advent of Code 2024 - Day 13: Claw Contraption

## 📜 Puzzle Overview

This puzzle revolves around operating a set of arcade-style claw machines.

Each machine has:

- two buttons (A and B)
- each button moves the claw by a fixed offset in X and Y
- a prize located at a specific coordinate

Your goal is to determine how many times to press each button to land exactly on the prize.

Each machine is defined by three lines:

- Button A movement
- Button B movement
- Prize position

---

## 🧩 Part 1

Determine the minimum number of tokens required to win as many prizes as possible.

### 💡 Approach

- Parse each machine definition
- Extract movement vectors for Button A and Button B
- Extract the target prize position
- Solve a system of linear equations:

      A * (ax, ay) + B * (bx, by) = (px, py)

- Find integer values for A and B that exactly match the prize position
- Only accept solutions where:
  - A >= 0
  - B >= 0
  - both are whole numbers
- Calculate token cost:
  - Button A costs 3 tokens
  - Button B costs 1 token
- Sum the minimum token cost for all solvable machines

---

## 🧩 Part 2

The prize positions are offset by a large constant.

### 💡 Approach

- Adjust each prize coordinate by adding a large value (e.g. 10,000,000,000,000)
- Reuse the same equation-solving logic from Part 1
- Solve using precise arithmetic (avoid floating point errors)
- Sum the token costs for all valid solutions

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Claw Contraption`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses machines from input
- Calls the solver for standard coordinates

For Part 2:

- Parses machines from input
- Applies the large coordinate offset
- Calls the same solver logic

---

### Machine Representation

Each claw machine is represented by:

- Button A vector `(ax, ay)`
- Button B vector `(bx, by)`
- Prize position `(px, py)`

The input is parsed into structured objects that store these values.

---

### Parsing Input

The parser:

- Reads input in groups of three lines
- Extracts numeric values from each line
- Converts them into vector components

At a high level:

- identify Button A movement
- identify Button B movement
- identify Prize position
- construct a machine object

---

### Solving the System

Each machine requires solving:

      ax * A + bx * B = px
      ay * A + by * B = py

This is a system of two linear equations with two unknowns.

The solver:

- uses algebraic elimination or determinant-based solving
- checks if a valid integer solution exists
- ensures no fractional button presses are allowed

---

### Valid Solution Rules

A solution is only valid if:

- A and B are integers
- A >= 0 and B >= 0
- The computed position exactly matches the prize

If no valid solution exists, the machine is ignored.

---

### Token Cost Calculation

Once a valid solution is found:

- cost = (A * 3) + (B * 1)

This reflects:

- Button A is expensive but powerful
- Button B is cheaper but less impactful

---

### Handling Large Numbers (Part 2)

Part 2 introduces very large coordinates.

To handle this safely:

- avoid floating point math
- use integer or long arithmetic
- ensure calculations do not overflow

The logic remains identical, only the scale changes.

---

### Aggregate Result

The solver:

- iterates over all machines
- solves each independently
- accumulates total token cost

---

## 🛠 Implementation Notes

- Each machine is solved independently
- Uses linear algebra to avoid brute force
- Integer validation is critical
- Part 2 requires careful handling of large values
- Same solver reused for both parts

---

## 🧪 Behaviour Summary

Given a list of claw machines:

- parse movement vectors and prize positions
- solve linear equations for button presses
- validate integer-only solutions
- compute token costs
- sum results across all machines

Part 2 simply scales the prize coordinates and reruns the same logic.

---

## 🚀 Key Takeaways

- Clean use of linear algebra to solve grid movement problems
- Avoids brute force by solving equations directly
- Highlights importance of integer validation
- Demonstrates handling of very large numbers safely
- Reusable logic between both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2024/day/13