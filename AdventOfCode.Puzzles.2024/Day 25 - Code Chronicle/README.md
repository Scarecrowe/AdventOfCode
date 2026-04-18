# 🎄 Advent of Code 2024 - Day 25: Code Chronicle

## 📜 Puzzle Overview

This puzzle deals with matching keys to locks.

The input is split into schematic blocks, where each block represents either:

- a lock
- a key

Each schematic is drawn as a small grid of `#` and `.` characters.

The solver interprets these shapes as pin heights for 5 columns, then tests every key against every lock.

A key fits a lock when the combined height in every column stays within the allowed limit.

Part 1 counts how many lock/key pairs fit together.

There is no Part 2 puzzle for Day 25 in Advent of Code 2024, so the implementation typically only solves the first half.

---

## 🧩 Part 1

Count how many unique lock and key pairs fit together.

### 💡 Approach

- Split the input into separate schematic blocks
- Determine whether each block is a lock or a key
- Convert each block into a height profile
- Compare every lock against every key
- Count each pair where all columns fit within the height limit

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Code Chronicle`
- Loads the puzzle input
- Calls the solver for Part 1

Because Advent of Code 2024 Day 25 only has one puzzle part, the implementation usually returns just the silver answer logic.

---

### Schematic Parsing

The input is divided into groups separated by blank lines.

Each group is a small diagram such as:

    #####
    .####
    .####
    .####
    .#.#.
    .#...
    .....

or:

    .....
    #....
    #....
    #...#
    #.#.#
    #.###
    #####

Each group is classified as either:

- a lock
- a key

A common way to detect this is:

- locks start with a filled top row
- keys start with an empty top row

---

### Converting a Schematic to Heights

Each lock or key is reduced into a column-height profile.

For each of the 5 columns, the solver counts how many `#` cells appear in that column.

This produces values such as:

    [0, 5, 3, 4, 3]

or similar depending on the block.

The profile is then stored in either:

- a list of locks
- a list of keys

---

### Matching a Key to a Lock

A key fits a lock if, for every column:

    lockHeight + keyHeight <= limit

The usual limit for this puzzle is based on the schematic height.

So the solver checks each of the 5 columns and rejects the pair as soon as one column overflows.

If all columns pass, that key/lock pair is counted as valid.

---

### Pair Comparison

After parsing all blocks, the solver performs a full comparison:

- take each lock
- compare it with each key
- check whether the two profiles fit
- increment the result when they do

So this is effectively a Cartesian product of:

- all locks
- all keys

filtered by the fit condition.

---

### Part 1 Return Value

The final answer is:

- the number of valid lock/key pairings

So the silver result is simply the total count of all compatible pairs.

---

## 🛠 Implementation Notes

- Input is grouped by blank lines
- Each group is classified as a lock or a key
- Every schematic is converted into a 5-column height profile
- Matching is done by checking column-wise sums
- A pair is valid only if every column stays within the allowed height
- Day 25 of Advent of Code 2024 only requires a Part 1 answer

---

## 🧪 Behaviour Summary

Given a set of lock schematics and key schematics:

- the solver parses each block
- turns each one into a compact height profile
- compares every lock with every key
- counts the pairs that do not overlap too much in any column

The final result is the total number of fitting lock/key combinations.

---

## 🚀 Key Takeaways

- Nice example of reducing ASCII art into numeric profiles
- Avoids grid-by-grid comparison once heights are computed
- Matching becomes a simple per-column bounds check
- Efficient and straightforward for the final puzzle of the year

---

## 🔗 References

- https://adventofcode.com/2024/day/25