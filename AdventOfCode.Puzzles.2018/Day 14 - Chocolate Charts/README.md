# 🎄 Advent of Code 2018 - Day 14: Chocolate Charts

## 📜 Puzzle Overview

This puzzle simulates a growing scoreboard of “recipes” created by two elves.

The scoreboard starts with:

    3, 7

Two pointers (elves) move across the list, creating new values based on the sum of the current recipes they point at.

Each step:

- Sum the two current recipe values
- Split digits of the sum (e.g. 10 → 1, 0)
- Append them to the scoreboard
- Move each elf forward by:
  - current value + 1 (wrapping around)

Example progression:

    3, 7, 1, 0, 1, 0, 1, ...

The puzzle has two different goals depending on the part.

---

## 🧩 Part 1

After generating enough recipes, find the **10 recipes immediately after a given number of recipes**.

### 💡 Approach

- Maintain a dynamic list (scoreboard)
- Track two elf positions
- Repeatedly:
  - compute new recipes
  - append them
  - move elves forward
- Stop when the scoreboard reaches:

    input + 10

- Extract the 10 values starting at index `input`

---

## 🧩 Part 2

Find how many recipes appear on the scoreboard **before a target sequence first appears**.

### 💡 Approach

- Use the same generation process as Part 1
- After each new digit is added:
  - check if the target sequence appears at the end of the scoreboard
- Because new digits are appended one or two at a time:
  - check both last `n` and `n+1` positions
- Stop when the sequence is found
- Return the index where it first appears

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the title to `Chocolate Charts`
- Reads the input (single integer or string pattern)
- Runs both parts using the same simulator

For Part 1:

- Runs simulation until scoreboard length reaches `input + 10`
- Returns the next 10 digits as a string

For Part 2:

- Runs simulation until target pattern is found
- Returns index of first occurrence

---

### Scoreboard Representation

The scoreboard is stored as:

- a dynamically growing list of integers

Initial state:

    [3, 7]

This structure supports:

- fast append operations
- indexed access for elf movement
- substring checks for pattern matching

---

### Elf Movement Logic

Each elf tracks an index:

    elfIndex

After generating new recipes:

- move using:

    elfIndex = (elfIndex + currentValue + 1) % scoreboardLength

This ensures circular movement around the list.

---

### Recipe Generation

Each iteration:

1. Read values at elf positions
2. Compute sum
3. Split digits:

Example:

    7 + 8 = 15 → append [1, 5]

4. Append to scoreboard

This means each loop adds 1 or 2 new recipes.

---

### Part 1 Extraction

Once enough recipes exist:

- slice the scoreboard:

    scoreboard[input : input + 10]

- convert to string result

Example output:

    5158916779

---

### Part 2 Pattern Search

After each update:

- convert the latest portion of the scoreboard to a string
- check for target substring match

Because new digits may overlap boundaries:

- always check last:
  - `length - targetLength`
  - `length - targetLength - 1`

This ensures no match is missed.

---

### Efficient Checking Strategy

Instead of scanning the whole list each time:

- only check recent additions
- since new digits append at the end, matches can only occur there

This keeps performance manageable even for large runs.

---

## 🛠 Implementation Notes

- Scoreboard starts with `[3, 7]`
- Elves move independently using modular arithmetic
- Each loop adds 1–2 new digits
- Part 1 uses fixed-length slicing
- Part 2 uses streaming substring detection
- Both parts share the same generation engine

---

## 🧪 Behaviour Summary

Given an input:

- simulate recipe creation step-by-step
- scoreboard grows indefinitely
- Part 1 extracts a fixed slice after N recipes
- Part 2 searches for a pattern in the growing sequence
- both rely on the same underlying generator

---

## 🚀 Key Takeaways

- Classic simulation with a dynamically growing list
- Efficient pointer movement using modulo arithmetic
- Small optimisation: checking only tail for pattern matching
- Demonstrates streaming pattern search on generated data
- Same engine reused for both positional and pattern-based queries
- Growth-driven algorithms can avoid full rescans with local checks

---

## 🔗 References

- https://adventofcode.com/2018/day/14