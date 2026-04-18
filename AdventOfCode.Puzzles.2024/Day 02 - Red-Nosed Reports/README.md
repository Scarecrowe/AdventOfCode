# 🎄 Advent of Code 2024 - Day 02: Red-Nosed Reports

## 📜 Puzzle Overview

This puzzle analyses reactor reports made up of integer levels.

Each input line is a sequence of space-separated numbers, for example:

    7 6 4 2 1

Each line represents one report.

A report is considered safe when:

- all values are either strictly increasing or strictly decreasing
- the difference between adjacent values is at least 1 and at most 3
- no two adjacent values are equal

The solver checks each report against those rules and counts how many are safe.

---

## 🧩 Part 1

Count how many reports are already safe.

### 💡 Approach

- Parse each line into a list of integers
- Check whether the report is consistently increasing or decreasing
- Check every adjacent pair
- Reject the report if:
  - the difference is greater than 3
  - the direction changes mid-report
  - two adjacent values are equal
- Count the reports that pass all checks

---

## 🧩 Part 2

Count how many reports can be made safe by removing a single level.

### 💡 Approach

- Reuse the same safety check from Part 1
- If a report is already safe, count it immediately
- Otherwise:
  - try removing each value one at a time
  - test the reduced report with the same `IsSafe()` logic
- If any reduced version is safe, count the report

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Red-Nosed Reports`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new RedNosedReports(this.Input)`
- calls `Safe()`

For Part 2:

- creates `new RedNosedReports(this.Input)`
- calls `Safe(true)`

---

### `RedNosedReports.cs`

This class contains the main puzzle logic.

It stores:

- `Levels`

The constructor parses the input with:

    input.Select(x => x.Split(" ").Select(x => int.Parse(x)).ToList()).ToList()

So each input line becomes:

- `List<int>`

and the full input becomes:

- `List<List<int>>`

---

### Safe Report Counting

`Safe(bool tolerate = false)` loops through every report in `Levels`.

It keeps:

    int result = 0;

For each report:

- if `IsSafe(level)` returns `true`
  - increment the result
  - continue to the next report

If the report is not safe and tolerance is enabled:

- try removing one value at each index
- test the shortened copy
- count the report if any edited version becomes safe

---

### Tolerant Mode

When `tolerate` is `true`, the solver uses a brute-force retry approach.

For each index in the report:

- clone the report
- remove one item with:

    clone.RemoveAt(i)

- call:

    IsSafe(clone)

If any clone is safe:

- increment the result
- stop checking further removals for that report

This is the Part 2 "Problem Dampener" behaviour.

---

### `IsSafe(List<int> values)`

This method checks whether a report follows the puzzle rules.

It first determines the overall direction using the first pair:

    bool decreasing = (values[0] - values[1]) > 0;

It then loops through each adjacent pair and calculates:

    int value = values[i] - values[i + 1];

A report is rejected if any of the following are true:

- `Math.Abs(value) > 3`
- the direction changes compared with the initial direction
- two adjacent values are equal

That logic appears as:

    if (Math.Abs(value) > 3
    || decreasing != (value > 0)
    || values[i] == values[i + 1])

If none of those conditions are hit, the report is safe.

---

### Direction Checking

The solver does not separately test for ascending and descending modes.

Instead, it:

- determines the direction from the first pair
- requires every later pair to match that same direction

So a sequence that starts decreasing must remain decreasing throughout.

Likewise, a sequence that starts increasing must remain increasing throughout.

---

### Difference Checking

For each adjacent pair, the solver checks the step size.

Valid differences are:

- 1
- 2
- 3

Invalid differences are:

- 0
- anything greater than 3

This ensures reports change gradually and never stay flat.

---

## 🛠 Implementation Notes

- Input is parsed into `List<List<int>>`
- Part 1 and Part 2 both rely on the same `IsSafe()` method
- Part 2 uses cloning plus single-index removal
- The direction is determined from the first adjacent pair
- Equal adjacent values always make a report unsafe

---

## 🧪 Behaviour Summary

Given a list of reports:

- each line is parsed into a list of integer levels
- Part 1 counts reports that are already safe
- a safe report must be strictly increasing or strictly decreasing
- every step must differ by 1 to 3
- Part 2 allows one value to be removed
- if any single removal produces a safe report, that report is counted

---

## 🚀 Key Takeaways

- Clean example of validating sequences with simple adjacency rules
- Part 1 is a direct rule check over each report
- Part 2 reuses the same validation logic with one-value retrying
- The implementation is compact and easy to follow
- Brute-force removal works well because report sizes are small

---

## 🔗 References

- https://adventofcode.com/2024/day/2