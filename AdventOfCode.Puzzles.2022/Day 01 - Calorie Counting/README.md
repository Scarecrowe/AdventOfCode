# 🎄 Advent of Code 2022 - Day 1: Calorie Counting

## 📜 Puzzle Overview

This puzzle groups calorie values by elf.

Each line contains either:

- a calorie number
- or a blank line separating one elf from the next

The solver parses the input into a list where each entry is the total calories carried by one elf.

Part 1 finds the single highest calorie total. Part 2 finds the sum of the top three calorie totals.

---

## 🧩 Part 1

Determine the largest calorie total carried by any single elf.

### 💡 Approach

- Read the input line by line
- Keep a running total for the current elf
- When a blank line is reached:
  - store the current total
  - reset the running total
- After parsing all input, return the maximum total from the list

---

## 🧩 Part 2

Determine the sum of the top three calorie totals.

### 💡 Approach

- Reuse the parsed elf totals from Part 1
- Sort them in descending order
- Take the top three values
- Sum them

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Calorie Counting`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new CalorieCounting(this.Input)`
- Calls `MaxCallories()`

For Part 2:

- Creates `new CalorieCounting(this.Input)`
- Calls `TopThreeMaxCallories()`

---

### `CalorieCounting.cs`

This class contains the parsing and calculation logic.

It stores:

- `Elves`

The constructor parses the input immediately with:

- `Parse(input)`

So by the time the class is created, `Elves` already contains one calorie total per elf.

---

### Elf Totals

`Elves` is stored as:

- `List<int>`

Each integer represents the total calories carried by one elf.

The input is not kept as separate line groups after parsing. Instead, each elf's numbers are combined into a single sum.

---

### Parsing the Input

`Parse(string[] input)` reads the file line by line.

It creates:

- `List<int> result = new();`
- `int callories = 0;`

Then for each line:

- if the line is empty:
  - add the current running total to `result`
  - reset `callories` to `0`
- otherwise:
  - convert the line to an integer
  - add it to the current running total

The numeric conversion uses:

- `line.ToInt()`

So blank lines act as separators between elves.

---

### Final Elf Handling

After the loop finishes, the parser checks:

    if (callories > 0)

If true, it adds the final accumulated total to the result list.

This ensures the last elf is still included even if the input does not end with a blank line.

---

### Part 1 Calculation

`MaxCallories()` returns:

- `this.Elves.Max()`

So the silver answer is simply the largest total found among all parsed elves.

---

### Part 2 Calculation

`TopThreeMaxCallories()` returns:

- `this.Elves.OrderByDescending(x => x).Take(3).Sum()`

So the gold answer is produced by:

- sorting elf totals from highest to lowest
- taking the first three
- summing them

---

## 🛠 Implementation Notes

- The class stores total calories per elf rather than the original grouped lines
- Blank lines are used as elf separators
- Parsing uses a running total accumulator
- The final elf is added after the loop if needed
- Part 1 uses `Max()`
- Part 2 uses descending sort plus `Take(3).Sum()`
- The implementation uses the spelling `Callories` in method and variable names

---

## 🧪 Behaviour Summary

Given an input of calorie numbers separated by blank lines:

- the solver accumulates numbers for one elf at a time
- each blank line ends the current elf group
- the final result is a list of total calories per elf
- Part 1 returns the highest single total
- Part 2 returns the sum of the three highest totals

---

## 🚀 Key Takeaways

- Nice simple example of grouping input by blank lines
- Parsing is done in a single pass
- The solver stores compact per-elf totals instead of raw groups
- Part 1 is a direct maximum lookup
- Part 2 is a straightforward top-three aggregation

---

## 🔗 References

- https://adventofcode.com/2022/day/1