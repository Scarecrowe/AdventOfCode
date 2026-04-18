# 🎄 Advent of Code 2022 - Day 25: Full of Hot Air

## 📜 Puzzle Overview

This puzzle works with a custom number system called **SNAFU**.

SNAFU is a base-5 style number system, but its digits can represent negative values as well as positive ones.

The allowed digits are:

- `2` = 2
- `1` = 1
- `0` = 0
- `-` = -1
- `=` = -2

Each input line is a number written in SNAFU format.

The solver converts every SNAFU number into decimal, sums them, then converts the total back into SNAFU.

Part 1 returns that final SNAFU sum. Part 2 does not perform any further calculation and instead returns the completion message.

---

## 🧩 Part 1

Determine the SNAFU number produced by summing all input values.

### 💡 Approach

- Parse every input line as a SNAFU number
- Convert each SNAFU value into decimal
- Sum all decimal values
- Build a SNAFU representation of that total
- Return the matching SNAFU string

---

## 🧩 Part 2

Return the final completion message.

### 💡 Approach

- No extra numeric processing is performed
- Return the fixed completion text directly

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Full of Hot Air`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `FullOfHotAir.Snafu(this.Input)`

For Part 2:

- Returns:

    You have enough stars to [Start The Blender]

---

### `FullOfHotAir.cs`

This class contains the SNAFU conversion logic.

It stores:

- `Map`
- `Digits`

`Map` defines the SNAFU digit values:

- `'0' => 0`
- `'1' => 1`
- `'2' => 2`
- `'=' => -2`
- `'-' => -1`

`Digits` defines the digit order used while searching for the final SNAFU representation:

- `0`
- `1`
- `2`
- `=`
- `-`

---

### Converting SNAFU to Decimal

`ToInt(string value)` converts a SNAFU string into a normal integer.

It works from right to left and uses powers of 5.

At each digit it does this logically:

    result += digitValue * pow
    pow *= 5

So the method behaves like base 5, except the digit values can be negative.

For example:

- the rightmost digit contributes `value * 1`
- the next contributes `value * 5`
- the next contributes `value * 25`

and so on.

---

### Summing the Input

`Snafu(string[] input)` begins by converting every input line into decimal with:

    input.Sum(x => ToInt(x))

This produces one total decimal sum across all SNAFU values.

That total is the value that must be converted back into SNAFU for Part 1.

---

### Building the Final SNAFU Result

After summing the input, the solver estimates the number of digits needed with:

    Math.Ceiling(Math.Log(sum, 5))

It then creates a starting candidate made entirely of `'='` characters.

So if the estimated digit count is `log`, the initial value is effectively:

- the smallest all-`=` candidate of that length

This gives the solver a starting point for its search.

---

### Incremental SNAFU Search

The solver then enters a loop.

On each pass it:

- converts the current SNAFU candidate back to decimal
- checks whether it matches the required sum
- if not, computes the remaining difference
- estimates which digit position should change next
- advances that digit to the next entry in `Digits`

The key steps are:

    long val = ToInt(value.Join())

and:

    long diff = (sum - val).Abs()

Then it updates one position with:

    value[log - nlog] = Digits[(Digits.IndexOf(value[log - nlog]) + 1) % 5]

So rather than using a direct decimal-to-SNAFU formula, this implementation searches by adjusting digits until the converted value exactly matches the target sum.

---

### Part 1 Return Value

When the current candidate converts back to the exact summed decimal value, the solver returns:

    value.Join()

So the silver answer is the final SNAFU string representing the total of all input numbers.

---

### Part 2 Return Value

The gold solution does not calculate a second numeric answer.

It simply returns:

    You have enough stars to [Start The Blender]

So Part 2 acts as the puzzle completion message.

---

## 🛠 Implementation Notes

- SNAFU digits include negative values
- Conversion to decimal is done with a digit map and powers of 5
- The input is summed in decimal form
- The decimal total is converted back into SNAFU by iterative search
- The search starts from an all-`=` candidate of estimated length
- Candidate digits are advanced using the fixed `Digits` list
- Part 2 returns a fixed string rather than performing a second algorithm

---

## 🧪 Behaviour Summary

Given a list of SNAFU numbers:

- the solver maps each SNAFU digit to its signed numeric value
- converts every input line into decimal
- sums all decimal values together
- searches for a SNAFU representation of that total
- returns that SNAFU string for Part 1
- returns the completion message for Part 2

---

## 🚀 Key Takeaways

- Good example of working with a custom positional number system
- Negative digit values make SNAFU different from normal base conversion
- The `ToInt()` method is simple and clean
- Part 1 solves the reverse conversion by iterative digit adjustment
- Part 2 is intentionally just a completion message
- The implementation keeps the whole puzzle compact and focused

---

## 🔗 References

- https://adventofcode.com/2022/day/25