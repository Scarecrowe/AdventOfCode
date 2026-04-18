# 🎄 Advent of Code 2017 - Day 25: The Halting Problem

## 📜 Puzzle Overview

This puzzle simulates a simple Turing machine.

The input describes:

- an initial state
- a number of steps to run (diagnostic checksum)
- a set of states
- rules for each state based on the current tape value

The machine operates on an infinite tape of values:

    0 or 1

It maintains:

- a current position (cursor)
- a current state

Each step:

- reads the current tape value
- writes a new value
- moves left or right
- switches to a new state

After a fixed number of steps, the result is:

- the checksum (number of `1` values on the tape)

This is a direct simulation of a Turing machine, a fundamental model of computation.

---

## 🧩 Part 1

Compute the diagnostic checksum after the specified number of steps.

### 💡 Approach

- Parse the input into structured state rules
- Represent the tape as a sparse structure (infinite in both directions)
- Track:
  - current position
  - current state
- Execute the machine for the given number of steps
- Count how many tape positions contain `1`

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Halting Problem`
- Loads the puzzle input
- Calls the solution

There is only one part for this day.

---

### Tape Representation

The tape is infinite, so it is not stored as an array.

Instead, the solver typically uses:

- a `HashSet<int>` for positions containing `1`

This means:

- any position not in the set is implicitly `0`
- memory usage stays minimal

---

### State Representation

Each state contains rules for two cases:

- when current value is `0`
- when current value is `1`

Each rule defines:

- value to write (0 or 1)
- direction to move (left or right)
- next state

---

### Parsing Input

The input is descriptive text.

The parser extracts:

- initial state
- number of steps
- state definitions

Each state is mapped into a structure like:

    state -> (rule for 0, rule for 1)

---

### Execution Logic

For each step:

    read current value
    apply matching rule
    write value
    move cursor
    change state

The cursor moves:

- left  -> position - 1
- right -> position + 1

---

### Simulation Loop

The machine runs for a fixed number of steps:

    N steps

Where `N` is provided in the input.

Each iteration performs:

- read
- write
- move
- state transition

---

### Writing to Tape

When writing:

- writing `1`:
  - add position to set
- writing `0`:
  - remove position from set

This keeps the structure clean and efficient.

---

### Final Checksum

After all steps:

- count the number of positions in the set

This represents:

    total number of 1s on the tape

---

## 🛠 Implementation Notes

- Sparse storage is critical due to infinite tape
- State transitions are deterministic
- Parsing is the most verbose part of the problem
- Execution itself is simple and linear
- No optimisation tricks required beyond efficient data structures

---

## 🧪 Behaviour Summary

Given a Turing machine description:

- the solver builds a rule set for each state
- simulates the machine step-by-step
- updates the tape and cursor position
- runs for a fixed number of steps
- returns the number of `1` values on the tape

---

## 🚀 Key Takeaways

- Direct implementation of a Turing machine simulation
- Efficient use of sparse data structures for infinite space
- Clear separation between parsing and execution
- Demonstrates how simple rules can produce complex behaviour
- Strong example of state-driven computation

---

## 🔗 References

- https://adventofcode.com/2017/day/25