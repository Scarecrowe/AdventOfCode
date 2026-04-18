# 🎄 Advent of Code 2023 - Day 09: Mirage Maintenance

## 📜 Puzzle Overview

This puzzle works with a report made up of number sequences.

Each input line contains a history of values, and the goal is to extrapolate what value should come next or what value must have come before.

The solver does this by repeatedly taking the difference between neighbouring values until the sequence collapses down to nothing, then rebuilding the missing value from the bottom back up.

Part 1 predicts the next value at the end of each history.

Part 2 predicts the missing value at the start of each history.

---

## 🧩 Part 1

Predict the next value for every history and sum the results.

### 💡 Approach

- Parse each input line into an integer array
- For each history:
  - repeatedly compute the difference between adjacent values
  - recursively continue until the sequence becomes empty
  - rebuild the predicted value back up the chain
- For Part 1:
  - add the predicted value onto the end of the current history
- Sum the predicted end values for all histories

---

## 🧩 Part 2

Predict the previous value for every history and sum the results.

### 💡 Approach

- Reuse the same recursive difference logic
- Instead of extending the history at the end:
  - work backwards from the first value
- For Part 2:
  - subtract the recursively predicted lower-level value from the start of the current history
- Sum the predicted starting values for all histories

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Mirage Maintenance`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new MirageMaintenance(this.Input)`
- calls `End()`

For Part 2:

- creates `new MirageMaintenance(this.Input)`
- calls `Start()`

So the two parts use the same solver class and differ only in whether the extrapolation happens at the end or the beginning of each history.

---

### `MirageMaintenance.cs`

This class contains the full parsing and extrapolation logic.

It stores:

- `Report`

The constructor parses the input with:

    input.Select(x => x.Split(" ").ToInt()).ToList();

So each line becomes an `int[]` containing one history of values.

---

### `Report`

`Report` is defined as:

- `List<int[]>`

Each array represents one sequence from the puzzle input.

For example, a line such as:

    0 3 6 9 12 15

becomes one integer array stored in the report.

---

### `End()`

This method solves Part 1.

It calls:

    this.ExtrapolateHistory((history, value) => history[^1] + value);

That means:

- recursively determine the extrapolated value from the lower-level differences
- add that value to the last number in the current history

So Part 1 predicts the next value to the right of each sequence.

---

### `Start()`

This method solves Part 2.

It calls:

    this.ExtrapolateHistory((history, value) => history[0] - value);

That means:

- recursively determine the extrapolated value from the lower-level differences
- subtract that value from the first number in the current history

So Part 2 predicts the missing value to the left of each sequence.

---

### `Diff(int[] history)`

This helper creates the next difference layer.

It is defined as:

    history[0..^1].Select((x, i) => history[i + 1] - x).ToArray();

So for each adjacent pair:

- take the next value
- subtract the current value

For example:

    0 3 6 9 12 15

becomes:

    3 3 3 3 3

This method is applied repeatedly during recursion.

---

### Recursive Prediction

The core recursive logic is in:

    Predict(int[] history, Func<int[], int, int> result)

It behaves like this:

- if the history is empty:
  - return `0`
- otherwise:
  - calculate the difference layer with `Diff(history)`
  - recursively predict the value for that smaller history
  - combine it with the current history using the supplied `result` function

The base case is:

    history.Length == 0 ? 0

So once the sequence has been reduced all the way down, the solver begins rebuilding the missing value upward through each previous layer.

---

### `ExtrapolateHistory(...)`

This method applies the recursive prediction to every sequence in the report.

It is defined as:

    this.Report.Aggregate(0, (sum, history) => sum += Predict(history, result));

So it:

- starts at `0`
- processes each history
- predicts one missing value for that history
- adds it to the running total

This is used by both `End()` and `Start()`.

---

### Shared Solver Design

A nice detail in this implementation is that both puzzle parts share the exact same recursive engine.

The only thing that changes is the function passed into:

- `ExtrapolateHistory(...)`

For Part 1:

- append to the end with:

      history[^1] + value

For Part 2:

- prepend to the start with:

      history[0] - value

So the difference between predicting forwards and backwards is handled by a small strategy function rather than separate logic trees.

---

## 🛠 Implementation Notes

- The puzzle title is `Mirage Maintenance`
- Input lines are parsed into `List<int[]>`
- `End()` solves Part 1
- `Start()` solves Part 2
- `Diff(...)` computes adjacent differences
- `Predict(...)` is recursive
- The recursion stops when the history length becomes `0`
- `ExtrapolateHistory(...)` sums one predicted value per input line
- The implementation uses `int`, not `long`
- Both parts reuse the same prediction pipeline with different result functions

---

## 🧪 Behaviour Summary

Given a report of number histories:

- the solver parses each line into an integer sequence
- repeatedly computes difference layers
- recursively works down until no values remain
- rebuilds the missing value from the bottom back up
- Part 1 extends each sequence to the right
- Part 2 extends each sequence to the left
- all predicted values are summed

So the final result is either:

- the sum of all predicted next values
- or the sum of all predicted previous values

---

## 🚀 Key Takeaways

- The solution is compact because both parts share one recursive prediction function
- Difference sequences are the key idea behind the extrapolation
- The recursion naturally mirrors the repeated layering described in the puzzle
- Part 1 and Part 2 differ only in how the reconstructed value is applied
- The implementation is clean and functional, with very little duplication
- Passing the extrapolation rule as a function keeps the solver simple and flexible

---

## 🔗 References

- https://adventofcode.com/2023/day/9