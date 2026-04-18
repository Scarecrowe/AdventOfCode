# 🎄 Advent of Code 2016 - Day 16: Dragon Checksum

## 📜 Puzzle Overview

This puzzle generates data using a modified **dragon curve** and then computes a checksum.

You are given an initial binary string (the input), and a required disk length.

The process has two phases:

1. Expand the data using the dragon curve algorithm  
2. Compute a checksum from the resulting data  

Part 1 uses a smaller disk size, while Part 2 uses a much larger one.

---

## 🧩 Part 1

Determine the checksum for the generated data after filling the disk.

### 💡 Approach

- Start with the initial input string
- Repeat until the data length is at least the required disk size:
  - Let the current data be `a`
  - Create `b` by:
    - reversing `a`
    - flipping all bits (`0 → 1`, `1 → 0`)
  - New data becomes:

```
a + "0" + b
```

- Once long enough:
  - truncate to the required disk length
- Compute the checksum (see below)

---

## 🧩 Part 2

Repeat the process with a much larger disk size.

### 💡 Approach

- Use the same expansion and checksum logic
- Due to large size:
  - avoid building full strings where possible
  - optimise memory and processing
- Efficient approaches may:
  - compute values on demand
  - avoid repeated allocations

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Dragon Checksum`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Expands data to required size
- Computes checksum

For Part 2:

- Uses optimised approach for large input
- Reuses checksum logic

---

### Dragon Curve Expansion

Given input `a`, generate:

```
b = reverse(a)
b = flip bits in b
result = a + "0" + b
```

Examples:

```
1       → 100
0       → 001
11111   → 11111000000
```

Repeat until sufficient length is reached.

---

### Truncating Data

- After expansion:
  - take only the first `N` characters
- Ignore any excess data

---

### Checksum Calculation

- Process data in pairs:

```
00 → 1
11 → 1
01 → 0
10 → 0
```

- This produces a new string half the size
- Repeat while length is even
- Stop when length becomes odd

Example:

```
110010110100 → 100
```

---

### Part 1 Logic

- Expand data until reaching required length
- Trim to exact size
- Repeatedly compute checksum
- Return final checksum string

---

### Part 2 Logic

- Same logic as Part 1
- Must handle very large data efficiently
- Optimisations are key to performance

---

## 🛠 Implementation Notes

- String growth is exponential
- Avoid full string construction for large inputs
- Bit operations can improve performance
- Checksum reduction is iterative and predictable
- Memory efficiency is critical in Part 2

---

## 🧪 Behaviour Summary

Given an initial binary string:

- Data expands using a deterministic rule
- Final data is trimmed to required length
- Checksum reduces data repeatedly
- Part 1 handles manageable size
- Part 2 requires optimisation for large input

---

## 🚀 Key Takeaways

- Fractal-like data expansion (dragon curve)
- Repeated reduction using pairwise comparison
- Strong focus on optimisation for large datasets
- Same logic scales from small to very large inputs

---

## 🔗 References

- https://adventofcode.com/2016/day/16