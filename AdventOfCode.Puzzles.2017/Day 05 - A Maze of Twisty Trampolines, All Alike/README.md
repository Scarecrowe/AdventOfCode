# 🎄 Advent of Code 2017 - Day 05: A Maze of Twisty Trampolines, All Alike

## 📜 Puzzle Overview

This puzzle simulates a list of jump instructions.

You are given a list of integers where:

- each value represents an offset to jump
- jumps are relative to the current position

Execution starts at index `0`.

At each step:

- read the current offset
- jump to a new index using that offset
- modify the offset at the current position

The goal is to determine how many steps are required to exit the list.

A jump exits when the index moves outside the bounds of the list.

---

## 🧩 Part 1

Determine the number of steps required to exit the list.

### 💡 Approach

- Start at index `0`
- Repeat until index is out of bounds:
  - read current offset
  - increment the offset at that position by `1`
  - jump to new index using the original offset
- Count the number of steps taken

---

## 🧩 Part 2

Modify the jump rules and determine the number of steps required to exit.

### 💡 Approach

- Same process as Part 1, except:

- When updating the offset:
  - if offset is **3 or more**:
    - decrement it by `1`
  - otherwise:
    - increment it by `1`

- Continue until the index exits the list

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `A Maze of Twisty Trampolines, All Alike`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Simulates jumps with simple increment rule

For Part 2:

- Applies conditional increment or decrement

---

### Input Parsing

- Each line contains a single integer
- Convert input into a list or array of integers

---

### Execution Loop

Maintain:

- current index
- step counter

Loop:

- read offset at current index
- modify offset
- update index using original offset
- increment step counter

---

### Part 1 Logic

- Always increment offset after reading it
- Continue until index exits bounds

---

### Part 2 Logic

- Modify offset based on its value:
  - `>= 3` → decrement
  - `< 3` → increment
- Continue until index exits bounds

---

### Mutation Behaviour

- The list is modified during execution
- Each visit changes future behaviour
- This makes the process stateful and non-reversible

---

## 🛠 Implementation Notes

- Use a mutable array or list
- Avoid copying data for performance
- Keep original offset before modifying it
- Loop continues until index is out of bounds
- Step counter tracks total operations

---

## 🧪 Behaviour Summary

Given a list of offsets:

- Execution jumps through the list
- Offsets are modified after each step
- Process continues until exiting the list
- Part 1 uses simple increment rule
- Part 2 uses conditional update rule

---

## 🚀 Key Takeaways

- Stateful simulation problem
- In-place mutation affects future execution
- Simple rules produce non-trivial behaviour
- Part 2 introduces conditional branching

---

## 🔗 References

- https://adventofcode.com/2017/day/5