# 🎄 Advent of Code 2020 - Day 1: Report Repair

## 📜 Puzzle Overview

This puzzle works with a list of expense report numbers.

The goal is to find entries that add up to:

```text
2020
```

Then return the product of those matching values.

The solver handles two variations:

- Part 1 finds a pair of numbers
- Part 2 finds a triple of numbers

---

## 🧩 Part 1

Find two numbers in the input that sum to `2020`, then return their product.

### 💡 Approach

- Parse all input lines into integers
- Generate every possible 2-index combination
- Check whether the two indexed values sum to `2020`
- Return the product of the first matching pair

---

## 🧩 Part 2

Find three numbers in the input that sum to `2020`, then return their product.

### 💡 Approach

- Reuse the parsed integer input
- Generate every possible 3-index combination
- Check whether the three indexed values sum to `2020`
- Return the product of the first matching triple

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Report Repair`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
new ReportRepair(this.Input).Pair()
```

For Part 2:

```text
new ReportRepair(this.Input).Tripple()
```

---

### `ReportRepair.cs`

This class contains the full puzzle logic.

It stores:

- `Input`

The constructor converts the raw string input into integers:

```text
this.Input = input.ToInt();
```

So the puzzle works directly with an `int[]`.

---

### Parsed Input

After construction, the solver has:

- `int[] Input`

This means every line from the expense report is converted into an integer once, up front.

---

### Part 1 Search

`Pair()` searches for two numbers that add to `2020`.

It uses:

```text
Enumerator.Range2D(this.Input.Length)
```

This produces index pairs across the input.

The solver then:

- checks whether the values at `i` and `j` sum to `2020`
- selects the product of those same two values
- returns the first match found

At a high level it behaves like:

- try every 2-number combination
- keep the one where the sum is `2020`
- return the multiplication result

---

### Part 2 Search

`Tripple()` searches for three numbers that add to `2020`.

It uses:

```text
Enumerator.Range3D(this.Input.Length)
```

This produces index triples across the input.

The solver then:

- checks whether the values at `i`, `j`, and `k` sum to `2020`
- selects the product of those three values
- returns the first match found

At a high level it behaves like:

- try every 3-number combination
- keep the one where the sum is `2020`
- return the multiplication result

---

### Sum and Product Helpers

The implementation uses extension helpers to keep the search concise.

For Part 1:

```text
this.Input.SumByIndex(x.i, x.j)
this.Input.ProductByIndex(x.i, x.j)
```

For Part 2:

```text
this.Input.SumByIndex(x.i, x.j, x.k)
this.Input.ProductByIndex(x.i, x.j, x.k)
```

So instead of manually indexing and calculating each time, the code uses helper methods to sum and multiply values by their selected indices.

---

### Return Values

`Pair()` returns:

- the product of the first pair whose sum is `2020`

`Tripple()` returns:

- the product of the first triple whose sum is `2020`

Both methods end with:

```text
.First()
```

So the solver assumes a valid solution exists and returns the first matching result.

---

## 🛠 Implementation Notes

- Input is converted once into `int[]`
- Part 1 uses `Range2D(...)` to test pairs
- Part 2 uses `Range3D(...)` to test triples
- Sum and product calculations are delegated to helper extension methods
- The class method is named `Tripple()` in the implementation
- Both parts return the first matching result

---

## 🧪 Behaviour Summary

Given a list of expense report entries:

- parse all values into integers
- Part 1 checks pairs of values for a sum of `2020`
- Part 2 checks triples of values for a sum of `2020`
- when a match is found, multiply those values together
- return that product as the answer

---

## 🚀 Key Takeaways

- Clean brute-force solution using generated index combinations
- Parsing is separated cleanly from the search logic
- Helper methods keep the pair and triple checks very compact
- Part 1 and Part 2 follow the same pattern with only the dimensionality changed
- Nice example of using enumerable combinational helpers to reduce boilerplate

---

## 🔗 References

- https://adventofcode.com/2020/day/1