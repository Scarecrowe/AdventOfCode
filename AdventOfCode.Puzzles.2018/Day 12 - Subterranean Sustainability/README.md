# 🎄 Advent of Code 2018 - Day 12: Subterranean Sustainability

## 📜 Puzzle Overview

This puzzle simulates the growth of plants in a one-dimensional row of pots over many generations.

Each input consists of:

- an initial state of pots (`#` = plant, `.` = empty)
- a set of transformation rules

An input looks like:

    initial state: #..#.#..##......###...###

    ...## => #
    #..#. => .
    .#... => #
    ...

Each rule describes how a sliding window of 5 pots transforms into the next state of the center pot.

The solver evolves the system over time and computes the sum of all pot indices that contain plants.

---

## 🧩 Part 1

Determine the sum of plant-containing pot indices after 20 generations.

### 💡 Approach

- Parse the initial state into a string of pots
- Parse rules into a lookup map: pattern → result (`#` or `.`)
- Repeatedly simulate generations:
  - pad the current state with empty pots (`.`) on both sides
  - apply transformation rules using a 5-character sliding window
  - build the next generation string
- Track the index offset of the leftmost pot
- After 20 iterations:
  - compute the sum of all indices containing `#`

---

## 🧩 Part 2

Determine the sum after 50,000,000,000 generations.

### 💡 Approach

- Run the same simulation as Part 1
- Track:
  - pattern of plants
  - shift in left index per generation
- Detect when the pattern stabilises (it becomes a repeating shape shifting steadily)
- Once stable:
  - stop full simulation
  - extrapolate remaining generations using the observed shift
- Compute final score without simulating all generations

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Subterranean Sustainability`
- Loads puzzle input
- Calls silver and gold solutions

For Part 1:

- runs the simulation for 20 iterations
- computes final pot sum

For Part 2:

- runs extended simulation
- detects stabilisation pattern
- extrapolates to 50 billion generations

---

### `Parse(input)`

The input is split into:

- initial state
- transformation rules

Parsing produces:

- `state` → string of `#` and `.`
- `rules` → dictionary of 5-char patterns

Example rule:

    .#.#. => #

becomes:

    rules[".#.#."] = "#"

---

### State Representation

The simulation stores:

- current pot string
- left index offset (to track real indices in an "infinite" line)

Because pots expand left and right over time, the index offset shifts whenever padding or trimming occurs.

---

### Generation Step

Each generation:

- pad the state with empty pots (`.....`)
- slide a 5-character window across the string
- build a new string using rules

For each position:

- take substring of length 5
- look up transformation rule
- append result to next generation

---

### Trimming and Index Tracking

After each generation:

- leading and trailing empty pots are removed
- index offset is adjusted accordingly

This ensures:

- memory stays bounded
- pot indices remain correct for scoring

---

### Detecting Stability (Key Insight for Part 2)

Over time, the pattern stops changing structurally and instead:

- shifts uniformly left or right each generation

When this happens:

- the difference between generations becomes constant

The solver detects:

- same pattern repeat
- consistent left shift delta

Once detected:

- remaining generations are extrapolated mathematically
- no further simulation is required

---

### Scoring Function

Final score is computed as:

- iterate through all pots
- for each `#` pot:
  - add `(index + leftOffset)` to total sum

So:

- each plant contributes its real position to the total

---

## 🛠 Implementation Notes

- The system behaves like a 1D cellular automaton
- Rules map 5-character patterns to next-state values
- State is dynamically trimmed to avoid infinite growth
- Index offset tracks real pot positions
- Part 2 relies on cycle detection / steady-state optimisation
- Once stable, arithmetic replaces simulation

---

## 🧪 Behaviour Summary

Given an initial plant configuration:

- the solver parses initial state and rules
- repeatedly applies transformation rules over generations
- grows and shifts the plant pattern over time
- Part 1 runs for a fixed 20 iterations
- Part 2 detects long-term stabilisation and extrapolates to 50 billion iterations
- final output is the sum of indices containing plants

---

## 🚀 Key Takeaways

- Classic 1D cellular automaton simulation
- Sliding window rule application drives evolution
- Efficient string trimming prevents unbounded growth
- Critical optimisation: recognising stable pattern shift
- Part 2 transforms simulation into mathematical extrapolation
- Great example of detecting convergence in iterative systems

---

## 🔗 References

- https://adventofcode.com/2018/day/12