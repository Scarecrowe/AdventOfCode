# 🎄 Advent of Code 2016 - Day 06: Signals and Noise

## 📜 Puzzle Overview

This puzzle reconstructs a message from a noisy signal.

You are given multiple strings of equal length. Each column represents repeated transmissions of the same character, but noise has introduced errors.

To recover the original message, you must analyse the frequency of characters in each column.

Part 1 selects the most common character per column, while Part 2 selects the least common.

---

## 🧩 Part 1

Determine the message by selecting the most frequent character in each column.

### 💡 Approach

- Read all input lines
- For each character position:
  - collect all characters from that column
  - count occurrences of each character
  - select the most frequent character
- Append each selected character to build the final message

---

## 🧩 Part 2

Determine the message by selecting the least frequent character in each column.

### 💡 Approach

- Reuse the same column-based grouping
- For each column:
  - count character frequencies
  - select the least frequent character
- Append results to form the final message

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Signals and Noise`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Groups characters by column index
- Selects most frequent character per column

For Part 2:

- Uses the same grouping
- Selects least frequent character per column

---

### Input Structure

Example input:

```
eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
```

Each column represents multiple observations of a single character.

---

### Column Processing

Given fixed-length strings:

- Iterate from index `0` to `length - 1`
- For each index:
  - extract all characters at that position across all lines

This produces a collection of characters per column.

---

### Frequency Counting

For each column:

- Count occurrences of each character
- This can be done using:
  - dictionaries
  - grouping functions

---

### Part 1 Logic

- For each column:
  - select the character with the highest frequency
- Append to result string

---

### Part 2 Logic

- For each column:
  - select the character with the lowest frequency
- Append to result string

---

### Handling Ties

If multiple characters have the same frequency:

- Selection depends on implementation details
- Typically resolved by ordering (e.g. alphabetical)

---

## 🛠 Implementation Notes

- All input lines must be the same length
- Column-wise processing is the core idea
- Frequency counting is reused across both parts
- Only the selection rule changes between parts
- Efficient grouping keeps the solution clean

---

## 🧪 Behaviour Summary

Given a set of equal-length strings:

- Each column is analysed independently
- Frequencies determine the correct character
- Part 1 favours the most common signal
- Part 2 favours the least common signal

---

## 🚀 Key Takeaways

- Column-based data processing pattern
- Frequency analysis applied to error correction
- Same dataset, different interpretation rules
- Clean separation of grouping and selection logic

---

## 🔗 References

- https://adventofcode.com/2016/day/6