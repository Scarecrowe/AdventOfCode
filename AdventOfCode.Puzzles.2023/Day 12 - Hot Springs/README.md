# 🎄 Advent of Code 2023 - Day 12: Hot Springs

## 📜 Puzzle Overview

This puzzle works with spring condition records.

Each input line contains:

- a spring pattern made of `.`, `#`, and `?`
- a list of damaged group sizes

A line looks like this:

    ??.### 1,1,3

Where:

- `.` means operational
- `#` means damaged
- `?` means unknown

The solver counts how many valid ways the unknown springs can be resolved so that the damaged spring groups exactly match the required group sizes.

Part 1 evaluates the input as written.  
Part 2 unfolds each record 5 times and counts the valid arrangements again.

---

## 🧩 Part 1

Count how many valid spring arrangements match the damaged group requirements.

### 💡 Approach

- Split each line into:
  - the spring pattern
  - the damaged group list
- Walk through the pattern from left to right
- Track:
  - the current position
  - which damaged group is being matched
  - the current run length of `#`
- Use recursion with memoization to avoid recalculating repeated states
- Return the number of valid completions for each line
- Sum the totals across all input lines

---

## 🧩 Part 2

Repeat the same logic, but first unfold each record.

### 💡 Approach

- Repeat the pattern 5 times, joined by `?`
- Repeat the damaged-group list 5 times
- Reuse the same memoized counting logic
- Sum the arrangement totals across all unfolded records

So a line such as:

    ??.### 1,1,3

becomes:

    ??.###???.###???.###???.###???.### 1,1,3,1,1,3,1,1,3,1,1,3,1,1,3

before solving Part 2.

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Hot Springs`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new HotSprings(this.Input)`
- Calls `Arrangements()`

For Part 2:

- Creates `new HotSprings(this.Input)`
- Calls `UnfoldedArrangements()`

---

### `HotSprings.cs`

This class contains the full solving logic.

It stores:

- `input`

and exposes two public methods:

- `Arrangements()`
- `UnfoldedArrangements()`

Both methods loop through every input line, parse it, count valid arrangements, and sum the results.

---

### Parsing the Input

`Parse(string line, bool unfold)` splits each line into:

- the spring pattern
- the group list

The input line is split on the space between them.

The group list is converted into integers using:

    tokens[1].Split(',').ToInt()

If `unfold` is `false`, the original pattern and groups are returned as-is.

If `unfold` is `true`, the method expands them.

---

### Part 2 Unfolding

For Part 2, the spring pattern is repeated 5 times with `?` separators.

This is done with:

    string.Join("?", Enumerable.Repeat(pattern, 5))

The damaged-group list is also repeated 5 times and flattened into one array.

This produces the unfolded input format required by the puzzle while reusing the same counting logic.

---

### Counting Arrangements

`CountArrangements(string pattern, int[] groups)` creates a memoization cache and starts the recursive search.

The cache key is:

- current pattern index
- current group index
- current run length of damaged springs

So each recursive state is uniquely identified by:

    (Index, GroupIndex, RunLength)

This prevents the solver from recalculating the same partial state many times.

---

### Recursive Solver

The main logic lives in:

    Count(...)

This method processes the pattern one character at a time.

It considers two possible interpretations where allowed:

- treat `?` as `.`
- treat `?` as `#`

It also handles fixed characters:

- `.` must end a damaged run if one is currently in progress
- `#` must continue the current damaged run

---

### State Tracking

The recursion tracks three values:

#### `index`

The current character position in the spring pattern.

#### `groupIndex`

Which damaged group from the input is currently being matched.

#### `runLength`

How many consecutive `#` characters have been seen in the current run.

These values are enough to fully describe the partial state of the scan.

---

### Handling `.`

When the current character is `.` or `?` treated as `.`:

- if no damaged run is active, move on normally
- if a damaged run is active, it must exactly match the current target group length
- if it matches, advance to the next group
- otherwise the branch is invalid

---

### Handling `#`

When the current character is `#` or `?` treated as `#`:

- the current damaged run length increases by 1
- the run is only allowed to continue if it does not exceed the current target group length

This prunes invalid branches early.

---

### End-of-Pattern Validation

When the recursion reaches the end of the pattern:

- if a damaged run is still active, it must match the current group exactly
- then the solver checks whether all required groups have been matched

If they have, that branch contributes:

    1

Otherwise it contributes:

    0

So each successful branch represents one valid arrangement.

---

### Memoization

The cache is defined as:

    Dictionary<(int Index, int GroupIndex, int RunLength), long>

Before solving any recursive state, the solver checks whether it already exists in the cache.

If it does, the stored value is returned immediately.

This is what makes Part 2 feasible, since brute-forcing every possible `?` replacement would grow far too quickly.

---

## 🛠 Implementation Notes

- `Day12.cs` calls `Arrangements()` for Part 1
- `Day12.cs` calls `UnfoldedArrangements()` for Part 2
- Input lines are parsed into:
  - a spring pattern
  - an integer array of damaged group sizes
- Part 2 unfolds both the pattern and the groups 5 times
- The solver uses recursion plus memoization
- Cache keys are based on index, group index, and current run length
- The implementation avoids generating every full candidate string

---

## 🧪 Behaviour Summary

Given a set of spring records:

- the solver parses each pattern and its damaged-group requirements
- recursively walks the pattern left to right
- tracks the active damaged run and required group index
- uses memoization to reuse repeated states
- counts every valid completion of the unknown positions

Part 1 solves the records exactly as written.  
Part 2 unfolds each record 5 times and applies the same counting logic.

---

## 🚀 Key Takeaways

- Good example of replacing brute force with dynamic programming
- Memoization makes the large Part 2 search space practical
- The recursive state is compact and easy to reason about
- The same solver supports both puzzle parts with only a parsing change
- Input unfolding is handled cleanly before counting begins

---

## 🔗 References

- https://adventofcode.com/2023/day/12