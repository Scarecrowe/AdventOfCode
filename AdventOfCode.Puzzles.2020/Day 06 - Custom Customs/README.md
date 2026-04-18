# 🎄 Advent of Code 2020 - Day 06: Custom Customs

## 📜 Puzzle Overview

This puzzle is about analysing group answers to a set of yes/no questions.

Each group consists of multiple people, and each person lists the questions they answered "yes" to using letters `a` to `z`.

Groups are separated by blank lines.

Example input:

    abc

    a
    b
    c

    ab
    ac

Part 1 counts how many unique questions were answered "yes" by **anyone** in each group.  
Part 2 counts how many questions were answered "yes" by **everyone** in each group.

---

## 🧩 Part 1

Determine the total number of questions to which **anyone** answered "yes" across all groups.

### 💡 Approach

- Split the input into groups using blank lines
- For each group:
  - Combine all answers into a single set of characters
  - Count the number of unique letters
- Sum these counts across all groups

---

## 🧩 Part 2

Determine the total number of questions to which **everyone** in each group answered "yes".

### 💡 Approach

- Split the input into groups
- For each group:
  - Start with the first person's answers
  - Intersect with each subsequent person's answers
  - Count the remaining common letters
- Sum these counts across all groups

---

## 🧠 Code Breakdown

### `Day06.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Custom Customs`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Processes groups to count unique answers
- Returns the total sum

For Part 2:

- Processes groups to count shared answers
- Returns the total sum

---

### Input Parsing

The input is structured as:

- multiple groups
- each group separated by a blank line
- each line within a group represents one person

Typical parsing flow:

- join input into a single string
- split on double newline (`"\n\n"`)
- split each group into individual lines

---

### Group Representation

Each group is effectively:

- a list of strings (each person's answers)

Example:

    ["ab", "ac"]

---

### Part 1 Logic

For each group:

- create an empty set of characters
- loop through each person's answers
- add each character to the set
- count the final set size

This represents all questions where **at least one person said yes**.

---

### Part 2 Logic

For each group:

- take the first person's answers as a starting set
- for each additional person:
  - intersect the current set with their answers
- count the remaining characters

This represents questions where **everyone said yes**.

---

### Example Walkthrough

Group:

    ab
    ac

Part 1:

- unique answers = {a, b, c}
- count = 3

Part 2:

- intersection = {a}
- count = 1

---

## 🛠 Implementation Notes

- Groups are split using blank lines
- Part 1 uses a union-style approach (set of all answers)
- Part 2 uses an intersection-style approach (shared answers)
- Character sets are ideal for both operations
- Input is small enough for straightforward iteration

---

## 🧪 Behaviour Summary

Given grouped answers:

- the solver splits input into groups
- Part 1 collects all unique answers per group
- Part 2 finds common answers per group
- results are summed across all groups
- final outputs are total counts for each mode

---

## 🚀 Key Takeaways

- Simple but effective use of set operations
- Demonstrates union vs intersection clearly
- Input parsing is critical due to grouping
- Clean separation of logic between Part 1 and Part 2
- Great example of reducing problems to set math

---

## 🔗 References

- https://adventofcode.com/2020/day/6