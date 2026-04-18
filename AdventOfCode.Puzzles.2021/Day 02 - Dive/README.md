# 🎄 Advent of Code 2021 - Day 02: Dive!

## 📜 Puzzle Overview

This puzzle simulates submarine movement based on a list of navigation commands.

Each input line is a command in the format:

    forward X
    down X
    up X

Where:

- `forward` increases horizontal position
- `down` increases depth
- `up` decreases depth

The solver parses each instruction and applies it to track the submarine's movement.

Part 1 calculates final horizontal position and depth directly.  
Part 2 introduces an additional concept called **aim**, which changes how movement works.

---

## 🧩 Part 1

Determine the final horizontal position multiplied by the final depth.

### 💡 Approach

- Start with:
  - horizontal = 0
  - depth = 0
- Process each command:
  - `forward X` → horizontal += X
  - `down X` → depth += X
  - `up X` → depth -= X
- Multiply final horizontal position by final depth

---

## 🧩 Part 2

Recalculate movement using an **aim** variable.

### 💡 Approach

- Start with:
  - horizontal = 0
  - depth = 0
  - aim = 0
- Process each command:
  - `down X` → aim += X
  - `up X` → aim -= X
  - `forward X`:
    - horizontal += X
    - depth += aim * X
- Multiply final horizontal position by final depth

---

## 🧠 Code Breakdown

### `Day02.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Dive!`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Processes commands using basic movement rules
- Returns horizontal * depth

For Part 2:

- Processes commands using aim-based movement
- Returns updated horizontal * depth

---

### Command Parsing

Each input line is split into:

- a command (`forward`, `up`, `down`)
- a numeric value

At a high level:

- split on space
- parse the value as an integer
- switch on the command type

Example:

    forward 5

becomes:

- command = "forward"
- value = 5

---

### Movement Logic (Part 1)

For each command:

    if forward:
        horizontal += value

    if down:
        depth += value

    if up:
        depth -= value

This directly updates position without any additional state.

---

### Movement Logic (Part 2)

Introduces **aim**:

    if down:
        aim += value

    if up:
        aim -= value

    if forward:
        horizontal += value
        depth += aim * value

So:

- aim controls how depth changes during forward movement
- forward movement now affects both horizontal and depth

---

### Final Calculation

Both parts return:

    horizontal * depth

The difference lies entirely in how depth is calculated.

---

## 🛠 Implementation Notes

- Input is processed line by line
- Commands are parsed using string splitting
- Integer parsing is required for movement values
- Part 2 introduces state (`aim`) that persists across commands
- Logic is simple but order-dependent

---

## 🧪 Behaviour Summary

Given a sequence of movement commands:

- the solver parses each instruction
- updates position based on rules
- Part 1 uses direct depth movement
- Part 2 modifies depth indirectly using aim
- final output is horizontal multiplied by depth

---

## 🚀 Key Takeaways

- Clear example of stateful vs stateless processing
- Part 2 builds directly on Part 1 with minimal changes
- Demonstrates how introducing a single variable (`aim`) changes behaviour significantly
- Emphasises careful interpretation of problem rules

---

## 🔗 References

- https://adventofcode.com/2021/day/2