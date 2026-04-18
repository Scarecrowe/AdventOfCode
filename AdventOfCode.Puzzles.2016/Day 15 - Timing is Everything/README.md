# 🎄 Advent of Code 2016 - Day 15: Timing is Everything

## 📜 Puzzle Overview

This puzzle models a set of rotating discs.

Each disc:

- has a fixed number of positions
- starts at a given position
- advances by one position per time unit

A capsule is dropped at time `t = 0` and reaches:

- disc 1 at time `t + 1`
- disc 2 at time `t + 2`
- ...
- disc `n` at time `t + n`

The goal is to find the earliest time `t` such that:

- each disc is aligned at position `0` exactly when the capsule reaches it

---

## 🧩 Part 1

Determine the earliest time to press the button so the capsule passes through all discs.

### 💡 Approach

- Parse each disc:
  - total positions
  - starting position
- For a given time `t`, each disc must satisfy:

```
(startPosition + t + discIndex) % positions == 0
```

- Iterate `t` from `0` upward
- Check all discs for alignment
- Return the first valid `t`

---

## 🧩 Part 2

Repeat the process with one additional disc.

### 💡 Approach

- Add a new disc:
  - with specified number of positions
  - starting at position `0`
- Apply the same logic as Part 1
- Find the new earliest valid time

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Timing is Everything`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses disc data
- Searches for valid time

For Part 2:

- Adds an extra disc
- Re-runs the search

---

### Disc Representation

Each disc includes:

- `positions` (total slots)
- `startPosition`
- `index` (1-based order in sequence)

---

### Alignment Condition

For a disc to align:

```
(startPosition + t + index) % positions == 0
```

This ensures the disc is at position `0` when the capsule reaches it.

---

### Iterative Search

- Start from `t = 0`
- For each `t`:
  - check all discs
- Stop when all conditions are satisfied

---

### Optimisation Considerations

- Brute force works due to manageable input size
- Can be optimised using:
  - step increments
  - least common multiple (LCM) logic
- Each satisfied disc can reduce the search space

---

### Part 1 Logic

- Parse discs from input
- Iterate time values
- Return first `t` where all discs align

---

### Part 2 Logic

- Add new disc to configuration
- Repeat the same search process
- Return updated result

---

## 🛠 Implementation Notes

- Modulo arithmetic is central to the solution
- Disc indexing must match arrival timing
- Efficient checking improves performance
- Incremental optimisation can reduce iterations
- Problem resembles solving simultaneous congruences

---

## 🧪 Behaviour Summary

Given a set of rotating discs:

- Each disc advances over time
- Capsule reaches discs at increasing offsets
- Valid time aligns all discs at position `0`
- Part 2 adds an extra constraint

---

## 🚀 Key Takeaways

- Modular arithmetic problem
- Time-offset alignment across multiple conditions
- Brute-force solution with optimisation potential
- Equivalent to solving a system of congruences

---

## 🔗 References

- https://adventofcode.com/2016/day/15