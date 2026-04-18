# 🎄 Advent of Code 2024 - Day 01: Historian Hysteria

## 📜 Puzzle Overview

This puzzle involves comparing two lists of location IDs.

Each input line contains two integers:

    3   4
    4   3
    2   5

These represent two separate lists:

- left list
- right list

The goal is to analyse how these lists relate to each other using different rules.

---

## 🧩 Part 1

Calculate the total distance between the two lists.

### 💡 Approach

- Parse the input into two separate lists of integers
- Sort both lists in ascending order
- Pair values at matching indices
- For each pair:
  - calculate the absolute difference
- Sum all differences

This effectively matches the smallest values together, then the next smallest, and so on

---

## 🧩 Part 2

Calculate the similarity score between the two lists.

### 💡 Approach

- Count how many times each number from the left list appears in the right list
- For each number in the left list:
  - multiply the number by its occurrence count in the right list
- Sum all results

This produces a weighted similarity score based on matching values between the lists

---

## 🧠 Code Breakdown

### `Day01.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Historian Hysteria`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- computes total distance between sorted lists

For Part 2:

- computes similarity score using frequency matching

---

### Input Parsing

Each line contains two integers separated by whitespace.

The solver:

- splits each line into two values
- builds:
  - `List<int> left`
  - `List<int> right`

---

### Sorting Lists (Part 1)

Both lists are sorted independently:

    left.Sort()
    right.Sort()

This ensures:

- smallest values are paired together
- differences are minimised correctly

---

### Distance Calculation

After sorting:

- iterate over both lists by index
- compute:

    abs(left[i] - right[i])

- accumulate the total

---

### Frequency Counting (Part 2)

Instead of sorting, Part 2 relies on counting.

The solver typically:

- builds a frequency map for the right list:

    Dictionary<int, int>

- for each value in the left list:
  - look up how many times it appears in the right list

---

### Similarity Score

For each value:

    value * frequency

Then:

- sum all results

This rewards values that appear frequently in both lists.

---

### Algorithm Characteristics

Part 1:

- sorting + linear scan
- time complexity dominated by sort

Part 2:

- hash map lookup
- linear time after parsing

---

## 🛠 Implementation Notes

- Input parsing is straightforward but must be consistent
- Sorting both lists is key for Part 1 correctness
- Dictionary lookup avoids nested loops in Part 2
- Both parts operate independently on the same parsed data
- Efficient for large input sizes

---

## 🧪 Behaviour Summary

Given two lists of numbers:

- Part 1:
  - sorts both lists
  - pairs values by index
  - sums absolute differences

- Part 2:
  - counts occurrences of values
  - multiplies values by frequency
  - sums results

The final outputs represent:

- total distance (Part 1)
- similarity score (Part 2)

---

## 🚀 Key Takeaways

- Clean introduction puzzle focused on data manipulation
- Highlights sorting and pairing techniques
- Demonstrates use of frequency maps for efficient counting
- Avoids brute-force comparisons with better data structures
- Clear separation of logic between both parts

---

## 🔗 References

- https://adventofcode.com/2024/day/1