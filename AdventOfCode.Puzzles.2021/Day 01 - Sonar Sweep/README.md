# 🎄 Advent of Code 2021 - Day 01: Sonar Sweep

## 📜 Puzzle Overview

This puzzle analyses a sequence of depth measurements collected by a submarine sonar sweep.

Each input line is a single integer representing the depth at a given time.

Example input:

    199
    200
    208
    210
    200

Part 1 counts how many times the depth increases compared to the previous measurement.

Part 2 introduces a sliding window of three measurements and counts how many times the sum of the window increases.

---

## 🧩 Part 1

Count the number of times a depth measurement increases from the previous value.

### 💡 Approach

- Parse each line into an integer
- Iterate through the list of measurements
- Compare each value with the previous one
- Count how many times the current value is greater than the previous value
- Return the total count

---

## 🧩 Part 2

Count how many times the sum of a three-measurement sliding window increases.

### 💡 Approach

- Use the same parsed list of integers
- Create a sliding window of size 3
- Compute the sum of each window
- Compare each window sum with the previous window sum
- Count how many times the sum increases
- Return the total count

---

## 🧠 Code Breakdown

### `Day01.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sonar Sweep`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses the input into integers
- Calls the method that counts increases

For Part 2:

- Reuses the parsed input
- Calls the method that evaluates sliding window increases

---

### Parsing Input

The input is processed into a list of integers.

At a high level:

- read each line
- convert to integer
- store in a list

This allows efficient sequential comparisons.

---

### Depth Comparison Logic

The core logic for Part 1 is a simple comparison:

    if (current > previous)
        count++

This runs across the full dataset once.

---

### Sliding Window Logic

For Part 2, instead of comparing individual values:

- compare sums of overlapping windows of size 3

Example:

    Window 1: 199 + 200 + 208 = 607
    Window 2: 200 + 208 + 210 = 618

Then:

    if (window2 > window1)
        count++

---

### Optimisation Insight

Instead of summing each window fully:

    A + B + C
    B + C + D

You can simplify the comparison to:

    D > A

Because B and C cancel out.

This reduces unnecessary computation and improves performance.

---

### Iteration Strategy

- Start from index 1 for Part 1 comparisons
- Start from index 3 for Part 2 comparisons
- Ensure bounds are respected to avoid out-of-range errors

---

## 🛠 Implementation Notes

- Input is treated as a simple integer sequence
- No complex data structures are required
- Part 2 can be optimised using direct comparisons instead of summing
- The same dataset is reused for both parts
- Time complexity is linear O(n)

---

## 🧪 Behaviour Summary

Given a list of depth measurements:

- Part 1 counts direct increases between consecutive values
- Part 2 counts increases between overlapping 3-value windows
- Both parts iterate through the data once
- The result is the number of increases detected

---

## 🚀 Key Takeaways

- Simple iteration problems can often be solved with linear scans
- Sliding window techniques are powerful for sequence analysis
- Recognising overlapping computations can lead to optimisations
- Clean and minimal logic leads to highly efficient solutions

---

## 🔗 References

- https://adventofcode.com/2021/day/1