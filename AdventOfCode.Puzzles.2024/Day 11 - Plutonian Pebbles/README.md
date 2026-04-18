# 🎄 Advent of Code 2024 - Day 11: Plutonian Pebbles

## 📜 Puzzle Overview

This puzzle works with a line of stone values that change over a number of blinks.

The input is a single line of space-separated numbers representing stones.

On each blink, every stone transforms according to these rules:

- if the stone is `0`, it becomes `1`
- if the stone has an even number of digits, it splits into two stones
- otherwise, it is multiplied by `2024`

The solver does not simulate a giant expanding list directly.

Instead, it recursively computes how many stones a given starting value will produce after a given number of blinks and memoizes those results.

---

## 🧩 Part 1

Determine how many stones exist after `25` blinks.

### 💡 Approach

- Parse the input line into starting stone values
- For each stone, recursively calculate how many stones it produces after `25` blinks
- Cache repeated `(stone, remaining blinks)` states
- Sum the results for all starting stones

---

## 🧩 Part 2

Determine how many stones exist after `75` blinks.

### 💡 Approach

- Reuse the exact same recursive logic as Part 1
- Increase the blink count from `25` to `75`
- Rely on memoization to avoid recomputing identical states
- Sum the final counts for all starting stones

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Plutonian Pebbles`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PlutonianPebbles()`
- Calls `Blink(this.Input, 25)`

For Part 2:

- Creates `new PlutonianPebbles()`
- Calls `Blink(this.Input, 75)`

---

### `PlutonianPebbles.cs`

This class contains the full blink-counting logic.

It stores:

- `Blinked`

which is defined as:

- `Dictionary<PlutonianKey, long>`

This dictionary acts as the memoization cache for previously calculated states.

---

### `PlutonianKey.cs`

This readonly struct is used as the cache key.

It stores:

- `Stone`
- `Count`

So each memoized entry represents:

- a specific stone value
- a specific number of remaining blinks

It also implements:

- equality comparison
- `GetHashCode()`
- `==`
- `!=`

This makes it safe to use as a dictionary key.

---

### Main Blink Entry Point

`Blink(string[] input, int count)` starts the calculation.

It does this:

- reads `input[0]`
- splits the line on spaces
- parses each value into a number
- calls `Depth(x, count)` for each starting stone
- sums all returned values

So the solver never builds the full expanded stone list.

It only calculates how many stones each starting value eventually contributes.

---

### Recursive Counting

`Depth(long stone, int count)` returns:

- how many stones exist after applying the blink rules to this stone for `count` more steps

It first builds a cache key:

    new(stone, count)

Then it checks:

- if `count == 0`, return `1`
- if the state is already cached, return the cached value
- otherwise, calculate the result with `Rule(stone, count)`

After calculating, it stores the result in `Blinked` before returning it.

This is what makes the larger Part 2 blink count practical.

---

### Base Case

The recursion stops at:

    if (count == 0)
    {
        return 1;
    }

This means:

- once no blinks remain, the current stone counts as exactly one stone

The method is counting stones, not their numeric values.

---

### Blink Rules

The actual transformation logic lives in:

- `Rule(long stone, int count)`

It applies three cases.

#### Rule 1: Stone `0`

If the stone is:

    0

then it becomes:

    1

and the solver continues with:

    Depth(1, count - 1)

---

#### Rule 2: Even Number of Digits

The solver calculates digit count with:

    (int)Math.Log10(stone) + 1

If that count is even:

- split the number into left and right halves
- recursively evaluate both halves
- add their resulting stone counts together

It does this by computing:

    divisor = 10^(digits / 2)

Then:

    left = stone / divisor
    right = stone % divisor

And returns:

    Depth(left, count - 1) + Depth(right == 0 ? 0 : right, count - 1)

So an even-length stone branches into two recursive paths.

---

#### Rule 3: Odd Number of Digits

If the stone is not `0` and does not have an even digit count, the solver uses:

    Depth(stone * 2024, count - 1)

So the stone is multiplied by `2024` and continues as a single recursive branch.

---

### Memoization

The cache dictionary is:

- `Dictionary<PlutonianKey, long> Blinked`

This avoids recalculating repeated states such as:

- the same stone value
- with the same number of remaining blinks

Without that cache, the recursive branching in Part 2 would grow far too quickly.

---

### Part 1 Return Value

When called as:

    Blink(this.Input, 25)

the method returns:

- the total number of stones present after `25` blinks

This is the silver answer.

---

### Part 2 Return Value

When called as:

    Blink(this.Input, 75)

the method returns:

- the total number of stones present after `75` blinks

This is the gold answer.

---

## 🛠 Implementation Notes

- The input is read from the first line only
- Starting stones are parsed by splitting on spaces
- The solution counts resulting stones rather than simulating a full list
- Recursion is driven by `(stone, remaining blink count)`
- Memoization uses a custom `PlutonianKey` struct
- Even-digit stones split into two recursive branches
- Odd-digit stones multiply by `2024`
- The base case returns `1` because each final stone contributes one count

---

## 🧪 Behaviour Summary

Given a list of starting stone values:

- the solver reads each starting stone
- recursively applies the blink rules
- splits even-digit stones into two branches
- transforms `0` into `1`
- multiplies odd-digit stones by `2024`
- caches repeated states to avoid duplicate work
- sums the number of resulting stones after the required number of blinks

---

## 🚀 Key Takeaways

- Good example of replacing explosive simulation with recursive counting
- Memoization is the key optimisation that makes large blink counts feasible
- `PlutonianKey` keeps the cache state compact and explicit
- The same logic solves both parts by changing only the blink count
- The implementation focuses on counting outcomes, not storing every generated stone

---

## 🔗 References

- https://adventofcode.com/2024/day/11