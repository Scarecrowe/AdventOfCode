# 🎄 Advent of Code 2016 - Day 20: Firewall Rules

## 📜 Puzzle Overview

This puzzle processes a list of blocked IP ranges.

Each line of input defines a range:

```
start-end
```

These ranges represent **blacklisted IP addresses** within the full range:

```
0 to 4294967295
```

All values are inclusive.

The goal is to determine:

- which IPs are **allowed** (not in any blocked range)

Part 1 finds the lowest allowed IP.  
Part 2 counts how many IPs are allowed in total.

---

## 🧩 Part 1

Determine the lowest-valued IP address that is not blocked.

### 💡 Approach

- Parse all ranges into `(start, end)` pairs
- Sort ranges by starting value
- Begin scanning from `0`
- For each range:
  - if current value is within the range:
    - move to `end + 1`
  - otherwise:
    - current value is allowed → return it

---

## 🧩 Part 2

Determine the total number of allowed IP addresses.

### 💡 Approach

- Sort and merge overlapping or adjacent ranges
- After merging:
  - iterate through the gaps between ranges
- For each gap:
  - calculate how many IPs are not blocked
- Sum all valid gaps

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Firewall Rules`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Finds the first valid IP not covered by any range

For Part 2:

- Counts all valid IPs across the full range

---

### Parsing Input

Each line:

```
5-8
```

Becomes:

- `start = 5`
- `end = 8`

Ranges are inclusive.

---

### Sorting Ranges

- Sort by `start` value
- This allows efficient linear scanning

---

### Merging Ranges

Overlapping or adjacent ranges should be merged:

Example:

```
[0, 2] and [3, 5] → [0, 5]
[4, 7] and [5, 8] → [4, 8]
```

This simplifies processing and avoids double counting.

---

### Part 1 Logic

- Start with `current = 0`
- For each range:
  - if `current` is within range:
    - move to `range.end + 1`
  - if `current` is before the next range:
    - return `current`

---

### Part 2 Logic

- After merging ranges:
  - iterate through consecutive ranges
- For each pair:

```
gap = next.start - current.end - 1
```

- Add gap size to total
- Also account for:
  - values before first range
  - values after last range

---

### Edge Handling

- Ensure bounds are respected:
  - minimum = `0`
  - maximum = `4294967295`
- Be careful with off-by-one errors due to inclusive ranges

---

## 🛠 Implementation Notes

- Sorting is essential for efficiency
- Merging reduces complexity
- Use `long` or equivalent for large values
- Inclusive ranges require careful arithmetic
- Avoid scanning full IP space directly

---

## 🧪 Behaviour Summary

Given a list of blocked ranges:

- Ranges define invalid IPs
- Valid IPs exist in gaps between ranges
- Part 1 finds the first gap
- Part 2 counts all gaps

---

## 🚀 Key Takeaways

- Classic interval merging problem
- Sorting enables linear scanning
- Inclusive bounds require careful handling
- Gap detection is key to both parts

---

## 🔗 References

- https://adventofcode.com/2016/day/20