# 🎄 Advent of Code 2016 - Day 19: An Elephant Named Joseph

## 📜 Puzzle Overview

This puzzle simulates a circle of elves stealing presents from each other.

Each elf starts with one present and is arranged in a circle:

```
1 → 2 → 3 → ... → N → (back to 1)
```

Elves take turns removing other elves and collecting their presents.

Part 1 removes the next elf in the circle.  
Part 2 removes the elf directly opposite.

The goal is to determine which elf ends up with all the presents.

This puzzle is closely related to the **Josephus problem**, a well-known mathematical problem involving circular elimination.

---

## 🧩 Part 1

Determine which elf gets all the presents when each elf removes the one to their left.

### 💡 Approach

- Model elves in a circular structure
- Starting from elf `1`:
  - each elf removes the next elf (to the left)
  - continue around the circle
- Continue until only one elf remains

A naive simulation works but becomes inefficient for large inputs.

### 🔍 Optimised Insight

This follows a known pattern:

- Let `N` be the number of elves
- Find the largest power of 2 ≤ `N`
- The winning elf is:

```
2 * (N - largestPowerOf2) + 1
```

This avoids simulation entirely.

---

## 🧩 Part 2

Determine which elf gets all the presents when each elf removes the one directly opposite.

### 💡 Approach

- Elves are still arranged in a circle
- Instead of removing the next elf:
  - remove the elf halfway across the circle
- Continue until one elf remains

This is more complex than Part 1 and does not map directly to the same formula.

---

### 🔍 Observing the Pattern

- Let `N` be the number of elves
- Find the largest power of 3 ≤ `N`

Then:

- If `N == powerOf3`:

```
winner = N
```

- If `N <= 2 * powerOf3`:

```
winner = N - powerOf3
```

- Otherwise:

```
winner = powerOf3 + 2 * (N - 2 * powerOf3)
```

This pattern emerges from analysing smaller cases and avoids expensive simulation.

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `An Elephant Named Joseph`
- Loads the puzzle input (number of elves)
- Calls the silver and gold solutions

For Part 1:

- Applies optimised formula or simulation

For Part 2:

- Uses pattern-based calculation or efficient simulation

---

### Naive Simulation

Basic approach:

- Store elves in a list or queue
- Remove elements as rules dictate
- Continue until one remains

This works for small inputs but is too slow for large values.

---

### Efficient Structures

For simulation-based solutions:

- linked lists
- deques
- split queues (for Part 2)

These allow efficient removal from the middle or opposite side.

---

### Part 1 Logic

- Recognise Josephus pattern
- Use mathematical shortcut
- Return computed winner

---

### Part 2 Logic

- Either:
  - simulate using efficient data structures
  - or apply derived pattern using powers of 3
- Return final elf position

---

## 🛠 Implementation Notes

- Part 1 has a direct mathematical solution
- Part 2 requires pattern recognition or advanced data structures
- Simulation alone is too slow for large inputs
- Precomputing patterns can simplify implementation
- Integer math avoids unnecessary overhead

---

## 🧪 Behaviour Summary

Given `N` elves:

- Elves are removed in a circular pattern
- Part 1 removes adjacent elves
- Part 2 removes opposite elves
- Process continues until one remains
- Output is the winning elf number

---

## 🚀 Key Takeaways

- Classic Josephus problem variant
- Mathematical optimisation replaces simulation
- Pattern recognition is key for Part 2
- Large inputs require efficient solutions

---

## 🔗 References

- https://adventofcode.com/2016/day/19