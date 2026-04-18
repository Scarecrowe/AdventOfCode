# 🎄 Advent of Code 2020 - Day 09: Encoding Error

## 📜 Puzzle Overview

This puzzle involves analysing a sequence of numbers encoded using the XMAS encryption system.

The data stream begins with a **preamble** (typically 25 numbers). After that, each number must be the sum of **two different numbers from the previous 25 values**.

If a number does not follow this rule, it is considered invalid.

Part 1 finds the first invalid number.  
Part 2 finds a weakness in the sequence based on that invalid number.

---

## 🧩 Part 1

Find the first number that is **not the sum of two of the previous N numbers**.

### 💡 Approach

- Use a sliding window of size `preamble` (e.g. 25)
- For each number after the preamble:
  - check all pairs within the previous window
  - ensure the pair uses two different values
  - if no valid pair sums to the current number:
    - return this number as the invalid value
- Slide the window forward and repeat

---

## 🧩 Part 2

Find a contiguous range of numbers that sum to the invalid number from Part 1.

Then return:

- smallest number in the range
- plus largest number in the range

### 💡 Approach

- Take the invalid number from Part 1 as the target
- Search for a contiguous subarray whose sum equals the target
- Once found:
  - determine `min` and `max` in that range
  - return `min + max`

---

## 🧠 Code Breakdown

### `Day09.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Encoding Error`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Identifies the first invalid number in the sequence

For Part 2:

- Uses the invalid number to locate the contiguous range
- Returns the encryption weakness

---

### Input Parsing

The input is:

- a list of integers
- one number per line

Typical parsing:

- read all lines
- convert each to `long` or `int`
- store in a list for sequential processing

---

### Sliding Window Validation

To validate each number:

- take the previous `N` numbers (preamble window)
- check if any pair sums to the current number

This can be done with:

- nested loops (simple)
- or a `HashSet` for faster lookups

Key rule:

- the two numbers must be **different values**

---

### Finding the Invalid Number

The algorithm:

- iterate from index `preamble` to end
- for each number:
  - validate using the previous window
  - if invalid:
    - return immediately

This is the Part 1 answer.

---

### Contiguous Range Search

For Part 2, we search for a contiguous sequence:

- use two pointers (start and end)
- maintain a running sum

Typical logic:

- expand the window (increase end) if sum < target
- shrink the window (increase start) if sum > target
- stop when sum == target

This is more efficient than brute force and avoids recalculating sums repeatedly.

---

### Extracting the Weakness

Once the correct range is found:

- find the smallest value in the range
- find the largest value in the range
- return:

    min + max

This is the final answer for Part 2.

---

### Example Walkthrough

Given:

    35 20 15 25 47
    40 62 55 65 95
    102 117 150 182
    127 ...

With preamble size 5:

- `127` is invalid (no two previous numbers sum to it)

Then:

- contiguous range: `15 + 25 + 47 + 40 = 127`
- min = 15
- max = 47
- result = 62

---

## 🛠 Implementation Notes

- Use a sliding window for Part 1 validation
- Ensure pairs use different values
- Part 1 can be O(n * preamble²) with brute force
- Part 2 benefits from a two-pointer approach (linear time)
- Input size is manageable, so clarity is preferred over micro-optimisation

---

## 🧪 Behaviour Summary

Given a sequence of numbers:

- the solver scans using a fixed-size preamble
- identifies the first invalid number (Part 1)
- searches for a contiguous range summing to that number (Part 2)
- returns:
  - invalid number (silver)
  - sum of min and max in range (gold)

---

## 🚀 Key Takeaways

- Classic sliding window problem
- Introduces pair-sum validation within a moving window
- Part 2 highlights efficient subarray sum techniques
- Demonstrates progression from brute force to optimised approaches
- Clean separation between validation and search logic

---

## 🔗 References

- https://adventofcode.com/2020/day/9