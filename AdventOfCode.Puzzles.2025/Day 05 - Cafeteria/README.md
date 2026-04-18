# 🎄 Advent of Code 2025 - Day 05: Cafeteria

## 📜 Puzzle Overview

This puzzle works with two kinds of input data:

- ranges of fresh ingredient IDs
- a list of available ingredient IDs

The input is split into two sections by a blank line.

The first section contains ranges in the format:

- `start-end`

The second section contains individual numeric IDs, one per line.

Part 1 asks how many available IDs fall inside any fresh range.

Part 2 asks for the total number of distinct fresh IDs covered by the full set of ranges after overlapping and adjacent ranges are merged together.

---

## 🧩 Part 1

Determine how many available ingredient IDs are considered fresh.

### 💡 Approach

- Parse the first section into numeric ranges
- Parse the second section into a list of available IDs
- For each available ID, check whether it falls inside any fresh range
- Count how many IDs match at least one range

---

## 🧩 Part 2

Determine how many distinct ingredient IDs are fresh in total.

### 💡 Approach

- Parse all fresh ranges
- Sort them by start position, then by end position
- Merge any ranges that overlap or touch
- Sum the size of each merged range
- Return the total number of covered IDs

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Cafeteria`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Cafeteria(this.Input)`
- Calls `AvailableCount()`

For Part 2:

- Creates `new Cafeteria(this.Input)`
- Calls `FreshCount()`

The two parts share the same solver and differ only in which result is returned.

---

### `Cafeteria.cs`

This class contains the full parsing and counting logic.

It stores:

- `Fresh` as a list of ingredient ranges
- `Available` as a list of numeric ingredient IDs

Those values are parsed immediately when the class is created by:

- `ParseFresh(input)`
- `ParseAvailable(input)`

---

### Parsing Fresh Ranges

`ParseFresh()` reads every line before the blank separator.

For each line:

- split on `-`
- parse the two values as `Start` and `End`
- create an `IngredientRange`

At a high level, a line like this:

    100-125

becomes a range with:

- `Start = 100`
- `End = 125`

---

### Parsing Available IDs

`ParseAvailable()` reads every line after the blank separator.

It:

- skips the blank separator line
- ignores any extra empty lines
- parses each remaining line as a `long`

This produces the list of individual available ingredient IDs used in Part 1.

---

### Part 1 Logic

`AvailableCount()` checks every available ID against the fresh ranges.

The method works like this:

- loop through the available ID list
- for each ID, check whether any fresh range contains it
- count the IDs that match

This is handled with:

    this.Available.Count(id => this.Fresh.Any(r => r.Contains(id)))

So the Part 1 answer is the number of listed available IDs that fall inside at least one fresh range.

---

### Merging Fresh Ranges

Part 2 uses the private `Merge()` method.

This method:

- sorts the ranges by `Start`
- then sorts by `End`
- walks through the sorted list from left to right

For each new range:

- if there is no previous merged range, add it
- otherwise compare it to the last merged range
- if the new range starts at or before `last.End + 1`, merge them
- otherwise start a new merged range

This means both overlapping and directly adjacent ranges are combined into one continuous block.

At a high level, the merge rule behaves like this:

- `10-20` and `15-25` become `10-25`
- `10-20` and `21-30` also become `10-30`
- `10-20` and `22-30` stay separate

---

### Part 2 Logic

`FreshCount()` first merges the full fresh range list, then sums the size of each merged range using:

    Merge(this.Fresh).Sum(r => r.End - r.Start + 1)

The `+ 1` is important because the ranges are inclusive.

For example:

- `5-5` contains `1` value
- `5-7` contains `3` values

So Part 2 returns the total number of distinct IDs covered by the merged fresh ranges.

---

## 🛠 Implementation Notes

- The input is split into two sections using a blank line
- Fresh IDs are stored as inclusive numeric ranges
- Available IDs are stored as individual numeric values
- Part 1 checks membership against the original range list
- Part 2 merges overlapping and adjacent ranges before summing coverage
- The same parsed data is reused for both puzzle parts

---

## 🧪 Behaviour Summary

Given fresh ranges such as:

    10-20
    18-25
    30-35

the merged result becomes:

- `10-25`
- `30-35`

The total fresh coverage would then be:

- `16` IDs from `10` to `25`
- `6` IDs from `30` to `35`

for a total of:

- `22`

For Part 1, any available ID inside one of those fresh intervals would be counted as fresh.

---

## 🚀 Key Takeaways

- Good example of solving two related parts from the same parsed input
- Part 1 is a direct range-membership check
- Part 2 switches to interval merging and coverage counting
- Adjacent ranges are intentionally merged, not just overlapping ones
- The implementation stays compact by separating parsing, merging, and counting cleanly

---

## 🔗 References

- https://adventofcode.com/2025/day/5