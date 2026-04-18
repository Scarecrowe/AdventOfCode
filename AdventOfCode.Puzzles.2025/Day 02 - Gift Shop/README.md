# 🎄 Advent of Code 2025 - Day 02: Gift Shop

## 📜 Puzzle Overview

This puzzle works through ranges of product IDs and identifies which IDs are considered invalid. The solver reads the input as a single comma-separated string of numeric ranges such as `start-end`, then checks every number inside each range.

Part 1 treats an ID as invalid when the full number is made from a smaller repeating block repeated **exactly twice**. Part 2 broadens the rule so an ID is invalid when that same repeating-block pattern appears **two or more times**.

---

## 🧩 Part 1

Determine the sum of all invalid IDs using the stricter matching rule.

### 💡 Approach

- Split the input into comma-separated numeric ranges
- Expand each range from start to end
- Convert each number to a string
- Check whether the full string is made from a repeated substring
- For Part 1, only count values where the substring repeats exactly twice
- Add every invalid ID to the running total

---

## 🧩 Part 2

Determine the sum of all invalid IDs using the relaxed rule.

### 💡 Approach

- Reuse the same range parsing and iteration logic
- Check each numeric ID for full-string repetition
- For Part 2, count values where the substring repeats two or more times
- Add every matching ID to the running total

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Gift Shop`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new GiftShop(this.Input)`
- Calls `SumInvalidIds()`

For Part 2:

- Creates `new GiftShop(this.Input)`
- Calls `SumInvalidIds(false)`

---

### `GiftShop.cs`

This class contains the full parsing and invalid-ID detection logic.

It stores:

- `Input` as the first input line

The main methods are:

- `IsInvalidId(string id, bool single)`
- `SumInvalidIds(bool single = true)`

---

### Parsing the Input

`SumInvalidIds()` treats the input as a comma-separated list of ranges.

For each product range:

- split on `,` to get each range block
- split each block on `-` to get `start` and `end`
- parse both values as integers
- iterate from `start` to `end` inclusive

At a high level, the input behaves like this:

- `100-120,300-305`

which becomes:

- one loop from `100` to `120`
- another loop from `300` to `305`

---

### Detecting Invalid IDs

`IsInvalidId()` checks whether the entire numeric string can be built by repeating a smaller substring.

It works like this:

- get the full string length
- try every possible substring length from `1` up to half the full length
- skip any substring length that does not divide the full length evenly
- take the first substring as the candidate repeating block
- compare every following block of the same length against that original substring
- if every block matches, the ID is considered invalid depending on the repetition rule

The matching is done with span comparisons:

    id.AsSpan(i, subLen).SequenceEqual(sub)

This keeps the repeated-block check efficient without building extra substring objects.

---

### Part 1 Rule

When `single` is `true`, the method only returns `true` when the repeating block occurs exactly twice. That means values such as:

- `1212`
- `9999` when treated as `99` repeated twice

match the Part 1 rule.

This branch is controlled by:

- `if (repetitions == 2)`

---

### Part 2 Rule

When `single` is `false`, the method accepts any full-string repetition with at least two copies of the repeating block. That means Part 2 also accepts values with more than two repetitions, such as patterns like:

- `101010`
- `777777`

This branch is controlled by:

- `if (repetitions >= 2)`

---

### Summing the Result

Inside `SumInvalidIds()`:

- each number in each range is converted to a string
- `IsInvalidId()` decides whether it matches the current puzzle rule
- matching IDs are added directly to `result`
- the final sum is returned as a `long`

This means the puzzle answer is not the count of invalid IDs, but the **sum of their numeric values**.

---

## 🛠 Implementation Notes

- Input is read from a single line rather than multiple independent rows
- Range expansion is inclusive from start to end
- Invalid IDs are detected using full-string repeating-pattern checks
- Span-based comparison is used instead of repeatedly slicing strings
- Part 1 and Part 2 share the same solver and differ only by the repetition rule
- Both puzzle parts return a summed total of matching IDs, not just a match count

---

## 🧪 Behaviour Summary

For a number string to be considered invalid, the entire string must be composed of equal-sized matching chunks.

Examples of strings that fit the repeated-pattern rule include:

- `1212` as `12` repeated twice
- `3333` as `3` repeated four times
- `101010` as `10` repeated three times

Examples that do not fit include:

- `1234`
- `1223`
- `1231234`

because the full string is not made entirely from one repeated block. This matches the structure checked by `IsInvalidId()`.

---

## 🚀 Key Takeaways

- Good example of solving both parts with one shared validation method
- The main rule difference is only `exactly twice` versus `two or more times`
- Span-based block comparison keeps the repeated-pattern test compact
- Range parsing and value expansion are separated cleanly from the ID-validation logic

---

## 🔗 References

- https://adventofcode.com/2025/day/2