# 🎄 Advent of Code 2018 - Day 02: Inventory Management System

## 📜 Puzzle Overview

This puzzle focuses on analysing a list of box ID strings.

Each line of input is a string made up of lowercase letters, for example:

    abcdef
    bababc
    abbcde

The challenge is split into two parts:

- Part 1 computes a checksum based on letter frequency
- Part 2 finds two nearly identical box IDs and extracts their common letters

---

## 🧩 Part 1

Calculate a checksum based on how many IDs contain letters that appear exactly two or three times.

### 💡 Approach

- Iterate through each box ID
- Count how many times each letter appears
- Check if any letter appears:
  - exactly **2 times**
  - exactly **3 times**
- Track two counters:
  - number of IDs with a letter appearing twice
  - number of IDs with a letter appearing three times
- Multiply those two totals together to get the checksum

---

## 🧩 Part 2

Find the two box IDs that differ by exactly one character in the same position.

### 💡 Approach

- Compare each ID with every other ID
- For each pair:
  - Compare characters position by position
  - Count how many positions differ
- Identify the pair where:
  - exactly **one character differs**
- Extract the common characters (matching positions only)
- Return the resulting string

---

## 🧠 Code Breakdown

### `Day02.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Inventory Management System`
- Loads the input data
- Executes both parts of the puzzle

For Part 1:

- Calls the checksum calculation logic

For Part 2:

- Calls the string comparison logic to find the correct pair

---

### Letter Frequency Counting

For each ID, the solver determines how often each character appears.

At a high level:

- Loop through each character in the string
- Count occurrences using a dictionary or grouping logic

Conceptually:

    aabcc
    a = 2
    b = 1
    c = 2

From this:

- This ID contributes to the "twos" count
- It does not contribute to the "threes" count

---

### Checksum Calculation

After processing all IDs:

- Let:
  - `twos` = number of IDs with a letter appearing exactly twice
  - `threes` = number of IDs with a letter appearing exactly three times

The checksum is:

    checksum = twos * threes

---

### Comparing Box IDs

For Part 2, the solver compares every pair of IDs.

For two strings:

    abcde
    abfde

Comparison logic:

- Compare character-by-character
- Track differences

Example:

    a == a ✔
    b == b ✔
    c != f ✖
    d == d ✔
    e == e ✔

Only one difference -> valid match

---

### Extracting Common Letters

Once the correct pair is found:

- Build a new string using only matching characters

From:

    abcde
    abfde

Result:

    abde

---

### Pair Comparison Strategy

At a high level:

- Nested loop through all IDs
- For each pair:
  - Skip comparing an ID with itself
  - Count character differences
  - Stop early if differences exceed 1 (optimisation)

---

## 🛠 Implementation Notes

- Input is treated as a list of strings
- Character frequency is calculated per ID
- Part 1 uses simple counting logic with condition checks
- Part 2 uses pairwise comparison across all IDs
- Early exit during comparison improves performance
- The final Part 2 result is built from matching characters only

---

## 🧪 Behaviour Summary

Given a list of box IDs:

- Part 1:
  - counts how many IDs contain letters appearing exactly twice and three times
  - multiplies those counts to produce a checksum

- Part 2:
  - compares all IDs to find the closest matching pair
  - identifies the pair differing by one character
  - extracts and returns their shared characters

---

## 🚀 Key Takeaways

- Demonstrates frequency counting within strings
- Shows how to derive aggregate metrics (checksum) from simple rules
- Uses brute-force comparison effectively with early exit optimisation
- Highlights pattern matching between strings
- Clean separation between counting logic and comparison logic

---

## 🔗 References

- https://adventofcode.com/2018/day/2