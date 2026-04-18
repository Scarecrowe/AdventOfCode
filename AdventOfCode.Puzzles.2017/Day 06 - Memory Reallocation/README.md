# 🎄 Advent of Code 2017 - Day 06: Memory Reallocation

## 📜 Puzzle Overview

This puzzle simulates redistribution of memory blocks across a set of banks.

You are given a list of integers where:

- each value represents the number of blocks in a memory bank

The system repeatedly redistributes blocks until a configuration repeats.

Part 1 counts how many cycles occur before a repeat.  
Part 2 determines the size of the loop between repeated configurations.

---

## 🧩 Part 1

Determine how many redistribution cycles occur before a configuration is seen again.

### 💡 Approach

- Track previously seen configurations
- Each cycle:
  - find the bank with the most blocks
  - if tied, choose the lowest index
  - set that bank to `0`
  - redistribute its blocks one by one:
    - move forward through the list
    - wrap around when reaching the end
- Stop when a configuration repeats
- Return the number of cycles

---

## 🧩 Part 2

Determine the size of the loop formed by repeated configurations.

### 💡 Approach

- Continue tracking configurations
- When a configuration repeats:
  - record when it was first seen
- Compute:

```
loopSize = currentCycle - firstSeenCycle
```

- Return the loop size

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Memory Reallocation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Simulates redistribution cycles
- Detects first repeated configuration

For Part 2:

- Measures cycle distance between repeats

---

### Configuration Tracking

- Represent memory state as:
  - array of integers
- Convert to a string or key for storage:

```
state = join(values)
```

- Store in a set or dictionary

---

### Redistribution Logic

Steps per cycle:

- Find index of maximum value
- Store number of blocks
- Set that index to `0`
- Distribute blocks:

```
index = (index + 1) % length
```

- Continue until all blocks are placed

---

### Part 1 Logic

- Track all seen configurations
- Increment cycle counter each iteration
- Stop when a configuration repeats
- Return total cycles

---

### Part 2 Logic

- Store cycle index for each configuration
- When a repeat is found:
  - compute difference between indices
- Return loop size

---

### State Comparison

- Configurations must be compared exactly
- String representation simplifies hashing
- Ensures quick lookup for repeats

---

## 🛠 Implementation Notes

- Use a hash set or dictionary for tracking states
- String conversion is simple but effective
- Redistribution must wrap around correctly
- Tie-breaking rules must be consistent
- Performance is acceptable due to small input size

---

## 🧪 Behaviour Summary

Given a list of memory banks:

- Blocks are redistributed repeatedly
- Each cycle produces a new configuration
- Process continues until a repeat is found
- Part 1 counts cycles to first repeat
- Part 2 measures loop size

---

## 🚀 Key Takeaways

- Cycle detection using state tracking
- Hashing configurations for fast lookup
- Circular redistribution logic
- Same simulation reused for both parts

---

## 🔗 References

- https://adventofcode.com/2017/day/6