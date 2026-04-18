# 🎄 Advent of Code 2023 - Day 13: Point of Incidence

## 📜 Puzzle Overview

This puzzle works with a collection of pattern blocks made of:

- ash (`.`)
- rock (`#`)

Each block is separated by a blank line.

The goal is to find a reflection line inside each pattern.

In this implementation, the solver checks for reflections where the mirrored halves differ by exactly one cell in total. That means it is looking for a smudged reflection rather than a perfect one.

For each pattern block, the solver checks:

- horizontal reflection positions between columns
- vertical reflection positions between rows

The final score is the sum of:

- the horizontal match value
- the vertical match value

with vertical reflections contributing `100 * row_index`.

---

## 🧩 Reflection Search

Determine where each pattern reflects with exactly one mismatch.

### 💡 Approach

- Split the input into separate pattern blocks using blank lines
- For each block:
  - test every possible column split
  - compare mirrored columns moving outward
  - count all character differences
  - accept the split if the total mismatch count is exactly `1`
- Then:
  - test every possible row split
  - compare mirrored rows moving outward
  - count all character differences
  - accept the split if the total mismatch count is exactly `1`
- Add both scores into the final total

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Point of Incidence`
- Loads the puzzle input
- Calls the solver for both silver and gold

For both parts it does:

- creates `new PointOfIncidence(this.Input)`
- converts that object to a string

So the visible entry point uses the same solver call for both answers.

---

### `PointOfIncidence.cs`

This class contains the pattern parsing and reflection logic.

The constructor:

- converts the input into a mutable list
- appends an extra blank line
- uses that blank line to flush the final pattern block
- loops through every line
- groups lines until a blank line is found
- for each completed block:
  - calculates a horizontal score
  - calculates a vertical score
  - adds both into a running total

At a high level it behaves like this:

- gather one pattern
- score it
- clear the buffer
- move to the next pattern

---

### Pattern Block Parsing

The constructor uses:

- `List<string> lines = input.ToList();`
- `lines.Add(string.Empty);`

That extra empty line guarantees the final pattern is processed even if the original input does not end with a blank separator.

As it loops:

- non-empty lines are added to the current block
- an empty line triggers scoring for the collected block

So each blank line acts as a pattern boundary.

---

### `Diff(string a, string b)`

This helper compares two equal-length strings character by character.

It:

- loops across all indices
- increments a counter every time the characters differ
- returns the total mismatch count

So for two rows or columns, `Diff(...)` tells the solver how many cells do not mirror correctly.

---

### `Horizontal(string[] input)`

This method searches for a reflection line between columns.

It works by:

- taking `count = input[0].Length`
- trying every split between column `i` and `i + 1`
- moving outward from that split with:
  - `l = i`
  - `r = i + 1`
- building the full left and right columns as strings
- comparing them with `Diff(...)`
- accumulating mismatch counts across the whole mirrored span

The relevant column strings are built like this:

    string lCol = string.Join(string.Empty, input.Select(x => x[l]));
    string rCol = string.Join(string.Empty, input.Select(x => x[r]));

If the total mismatch count for that split is exactly:

    1

then the method returns:

    i + 1

If no such split is found, it returns:

    0

So the horizontal score is the 1-based column split position.

---

### `Vertical(string[] input)`

This method searches for a reflection line between rows.

It works in the same way as `Horizontal(...)`, but uses full rows directly instead of constructing columns.

For each possible split:

- set:
  - `l = i`
  - `r = i + 1`
- compare `input[l]` and `input[r]`
- keep moving outward while both sides remain in range
- sum all mismatches across the mirrored row pairs

If the total mismatch count is exactly:

    1

then it returns:

    (i + 1) * 100

If no valid split is found, it returns:

    0

So vertical reflections are weighted by `100`.

---

### Smudged Reflection Logic

A key detail in this implementation is the acceptance rule:

- `mismatch == 1`

Both `Horizontal(...)` and `Vertical(...)` use that exact condition.

That means the solver is not looking for a perfect mirror with zero differences.

Instead, it is looking for a reflection that becomes valid when exactly one incorrect cell is tolerated across the full mirrored comparison.

So this implementation matches the smudged-reflection style logic.

---

### Result Accumulation

Inside the constructor, each completed pattern contributes:

- `a = Horizontal([.. lines])`
- `b = Vertical([.. lines])`

Then:

    result += a + b;

So each block adds either:

- a horizontal reflection score
- a vertical reflection score
- or both, if both methods return non-zero values

---

## 🛠 Implementation Notes

- The puzzle title is `Point of Incidence`
- Input is processed as blocks separated by blank lines
- The constructor appends an extra blank line so the final block is always handled
- `Diff(...)` counts character mismatches between two equal-length strings
- `Horizontal(...)` checks reflections between columns
- `Vertical(...)` checks reflections between rows
- Horizontal reflections return the split index as a 1-based value
- Vertical reflections return `(split index + 1) * 100`
- Both reflection checks require exactly one mismatch
- Based on the visible code, both `Silver()` and `Gold()` instantiate the same solver in the same way

---

## 🧪 Behaviour Summary

Given a set of pattern blocks:

- the solver groups lines into individual maps
- checks every possible column reflection split
- checks every possible row reflection split
- compares mirrored pairs outward from each split
- counts total mismatches across the mirrored region
- accepts only reflections with exactly one mismatch
- sums the resulting horizontal and vertical scores

So the final result is the combined reflection score across all pattern blocks.

---

## 🚀 Key Takeaways

- The implementation processes one pattern block at a time using blank lines as separators
- Reflection testing is done by expanding outward from each possible split
- Column reflections are converted into strings before comparison
- Row reflections can be compared directly
- The crucial rule is not perfect symmetry but symmetry with exactly one mismatch
- The vertical score is weighted by `100`, matching the puzzle scoring style

---

## 🔗 References

- https://adventofcode.com/2023/day/13