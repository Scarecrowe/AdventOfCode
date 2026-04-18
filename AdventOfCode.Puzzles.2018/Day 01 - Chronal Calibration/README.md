# 🎄 Advent of Code 2018 - Day 01: Chronal Calibration

## 📜 Puzzle Overview

This puzzle revolves around tracking frequency changes.

You are given a list of signed integers representing frequency adjustments.

For example:

    +1
    -2
    +3
    +1

Starting from a frequency of `0`, each value is applied in sequence to calculate a resulting frequency.

Part 1 computes the final frequency after processing the list once.  
Part 2 repeatedly applies the same list until a frequency is reached twice.

---

## 🧩 Part 1

Determine the resulting frequency after applying all changes once.

### 💡 Approach

- Start with a frequency of `0`
- Iterate through each line of input
- Convert each value into an integer
- Add it to the running total
- Return the final frequency after processing all values

---

## 🧩 Part 2

Determine the first frequency that is reached twice.

### 💡 Approach

- Start with a frequency of `0`
- Maintain a set of seen frequencies
- Loop through the input repeatedly (cycling when reaching the end)
- After applying each value:
  - Check if the new frequency has already been seen
  - If yes, return it immediately
  - If not, store it and continue

---

## 🧠 Code Breakdown

### `Day01.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Chronal Calibration`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Processes the input once
- Returns the resulting frequency

For Part 2:

- Reuses the same input
- Continuously loops until a repeated frequency is found

---

### Parsing Input

The input is treated as a simple list of signed integers.

At a high level:

- read each line
- convert it using integer parsing
- collect values into a list or process on the fly

Example:

    +1
    -2
    +3

becomes:

    [1, -2, 3]

---

### Frequency Calculation (Part 1)

The solver maintains a running total:

    frequency = 0

For each value:

    frequency += value

After all values are processed, the result is returned.

---

### Repeated Frequency Detection (Part 2)

This part introduces state tracking.

The solver maintains:

- a running frequency
- a collection of previously seen frequencies (e.g. `HashSet<int>`)

Logic flow:

- initialise with frequency `0`
- store `0` as seen
- loop through input repeatedly
- update frequency:

    frequency += value

- check:

    if frequency already seen → return it

- otherwise:

    add frequency to seen set

This continues until a duplicate is found.

---

### Infinite Loop Handling

Because the input may not produce a duplicate in a single pass:

- the solver cycles through the input indefinitely
- typically implemented with a loop that resets to the start when reaching the end

Conceptually:

    while (true)
        foreach value in input
            process value

---

## 🛠 Implementation Notes

- Uses a running integer to track frequency
- Part 1 is a single-pass accumulation
- Part 2 requires a loop with memory of previously seen values
- A `HashSet<int>` is ideal for fast duplicate detection
- The input is reused repeatedly without modification

---

## 🧪 Behaviour Summary

Given a sequence of frequency adjustments:

- the solver starts at frequency `0`
- Part 1 applies all adjustments once and returns the result
- Part 2 repeatedly applies adjustments
- frequencies are tracked as they are produced
- the first repeated frequency is returned immediately

---

## 🚀 Key Takeaways

- Simple accumulation problem for Part 1
- Introduces cycle detection in Part 2
- Efficient use of a hash set for constant-time lookups
- Demonstrates how to handle infinite iteration safely
- Highlights importance of tracking state over time

---

## 🔗 References

- https://adventofcode.com/2018/day/1