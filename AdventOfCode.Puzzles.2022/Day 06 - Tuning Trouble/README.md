# 🎄 Advent of Code 2022 - Day 06: Tuning Trouble

## 📜 Puzzle Overview

This puzzle works with a datastream buffer represented as a single string of characters.

The goal is to find the first position where a run of characters contains only distinct values.

The solver uses one shared method that scans the input and checks fixed-size windows for uniqueness.

Different window sizes are used for the two puzzle parts:

- Part 1 uses `4`
- Part 2 uses `14`

When a valid window is found, the solver returns the position immediately after that marker.

---

## 🧩 Part 1

Find the first start-of-packet marker.

A start-of-packet marker is the first sequence of `4` consecutive distinct characters.

### 💡 Approach

- Read the input as one string
- Scan through the string from left to right
- At each index:
  - take a substring of length `4`
  - count how many distinct characters it contains
- As soon as a window has exactly `4` distinct characters:
  - return the current index plus `4`

So the silver answer is the number of characters processed before the first valid packet marker is completed.

---

## 🧩 Part 2

Find the first start-of-message marker.

A start-of-message marker is the first sequence of `14` consecutive distinct characters.

### 💡 Approach

- Reuse the same marker-finding method
- Scan the string again
- At each index:
  - take a substring of length `14`
  - count its distinct characters
- As soon as a window has exactly `14` distinct characters:
  - return the current index plus `14`

So the gold answer is the number of characters processed before the first valid message marker is completed.

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Tuning Trouble`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `TuningTrouble.Marker(this.Input[0], 4)`

For Part 2:

- Calls `TuningTrouble.Marker(this.Input[0], 14)`

So both puzzle parts use the same logic and differ only in the required distinct-window size.

---

### `TuningTrouble.cs`

This class contains the core marker detection logic.

It exposes:

- `Marker(string input, int distinct)`

The method takes:

- the full datastream string
- the required number of distinct characters in the marker

It then scans from left to right until it finds the first valid window.

---

### Main Scan Loop

The method loops over the input with:

- `for (int i = 0; i < input.Length; i++)`

At each position it builds a candidate window called:

- `result`

That window is chosen with two cases:

- if there are enough characters remaining:
  - take `input.Substring(i, distinct)`
- otherwise:
  - take the remainder of the string with `input[i..]`

So the implementation safely handles positions near the end of the input without overrunning the string.

---

### Distinct Character Check

For each candidate window, the solver checks:

    result.Distinct().Count() == distinct

This means:

- collect the unique characters in the window
- count them
- compare that count against the required marker size

If the counts match, then every character in that window is unique.

---

### Return Value

When a valid marker window is found, the method returns:

    i + distinct

This is not the starting index of the window.

It is the position immediately after the marker, which matches the puzzle requirement of reporting how many characters have been processed.

For example:

- if a valid 4-character marker starts at index `3`
- the method returns `7`

because the marker ends after 4 more characters.

---

### End-of-Input Handling

Near the end of the string, the method may produce a shorter trailing window using:

    input[i..]

That shorter window can never satisfy the distinct-character check unless its length still matches the requested marker size.

So it naturally fails the test and the loop continues.

If no valid marker is ever found, the method throws:

    new InvalidOperationException()

---

## 🛠 Implementation Notes

- Both puzzle parts reuse the same `Marker(...)` method
- The only difference between silver and gold is the distinct window size
- Part 1 uses `4`
- Part 2 uses `14`
- The solver checks each candidate window by counting distinct characters
- It returns the processed-character count, not the window start index
- A trailing partial substring is handled safely near the end of the input
- If no marker exists, the method throws an exception

---

## 🧪 Behaviour Summary

Given a datastream string:

- the solver scans it from left to right
- it takes a fixed-size window at each position
- it checks whether all characters in that window are unique
- Part 1 looks for the first valid window of length `4`
- Part 2 looks for the first valid window of length `14`
- the answer is the index just after the first matching window

---

## 🚀 Key Takeaways

- Nice example of solving both parts with one shared helper
- The puzzle reduces cleanly to a sliding distinct-character window check
- The implementation is compact and easy to follow
- LINQ `Distinct()` is used to keep the uniqueness test simple
- The returned value matches processed-character count rather than zero-based index

---

## 🔗 References

- https://adventofcode.com/2022/day/6