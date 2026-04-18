# 🎄 Advent of Code 2021 - Day 03: Binary Diagnostic

## 📜 Puzzle Overview

This puzzle works with a diagnostic report made up of binary strings.

Each input line is a sequence of `0` and `1` characters of equal length.

Example input:

    00100
    11110
    10110
    10111
    10101

Part 1 calculates the submarine's power consumption using the gamma and epsilon rates.

Part 2 calculates the life support rating using the oxygen generator rating and CO2 scrubber rating.

---

## 🧩 Part 1

Determine the power consumption from the diagnostic report.

### 💡 Approach

- Read every binary line in the input
- For each bit position, count how many values contain `1`
- Infer how many contain `0`
- Build the `gamma` binary string using the most common bit in each column
- Build the `epilson` binary string using the least common bit in each column
- Convert both binary strings to integers
- Multiply them to get the final power consumption

---

## 🧩 Part 2

Determine the life support rating from the diagnostic report.

### 💡 Approach

- Start with the full list of binary values
- For the oxygen generator rating:
  - inspect one bit position at a time
  - keep only values matching the most common bit
  - when counts are equal, keep values with `1`
- Repeat until only one value remains
- For the CO2 scrubber rating:
  - restart with the full list
  - inspect one bit position at a time
  - keep only values matching the least common bit
  - when counts are equal, keep values with `0`
- Convert both remaining binary strings to integers
- Multiply them to get the life support rating

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Binary Diagnostic`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `BinaryDiagnostic.PowerConsumption(this.Input)`

For Part 2:

- Calls `BinaryDiagnostic.LifeSupport(this.Input)`

---

### `BinaryDiagnostic.cs`

This class contains the full solution logic for both parts.

It provides:

- `PowerConsumption(string[] input)`
- `LifeSupport(string[] input)`
- `Find(List<string> remaining, int counter, int mode)`

---

### Power Consumption Calculation

`PowerConsumption(string[] input)` builds two binary strings:

- `gamma`
- `epilson`

For each column:

- count how many lines contain `1`
- infer the number of `0` values
- append the most common bit to `gamma`
- append the opposite bit to `epilson`

At a high level it works like this:

    for each bit position
        count ones
        zero = total - ones
        gamma += most common bit
        epilson += least common bit

Both strings are then converted from base 2 and multiplied.

---

### Gamma and Epsilon Behaviour

If a column contains more zeroes than ones:

- `gamma` gets `0`
- `epilson` gets `1`

Otherwise:

- `gamma` gets `1`
- `epilson` gets `0`

So Part 1 is effectively column-by-column frequency analysis across the full input.

---

### Life Support Calculation

`LifeSupport(string[] input)` performs two filtering passes.

First pass:

- copies all input lines into `remaining`
- repeatedly calls `Find(remaining, counter, 0)`
- stops when only one value remains
- stores that as `oxygen`

Second pass:

- resets `remaining` back to the full input
- repeatedly calls `Find(remaining, counter, 1)`
- stops when only one value remains
- stores that as `co2`

It then converts both binary strings to integers and multiplies them.

---

### Filtering Helper

`Find(List<string> remaining, int counter, int mode)` performs one filtering step for the current bit position.

It:

- counts zeroes and ones at index `counter`
- determines whether `1` is currently the most common bit
- detects whether the counts are equal
- returns a new filtered list based on the requested mode

It builds:

- `result`

which contains only the values still eligible after that step.

---

### Oxygen Filtering Rules

When `mode == 0`, the helper keeps the most common bit.

If the counts are equal:

- it treats `1` as the preferred bit

So the logic becomes:

    if equal
        use 1
    else
        use most common bit

Only lines matching that selected bit at the current position are kept.

---

### CO2 Filtering Rules

When `mode != 0`, the helper keeps the least common bit.

If the counts are equal:

- it still flags `1` as the tie state internally
- but then chooses the opposite bit during filtering
- which means `0` is kept on ties

So the logic becomes:

    if equal
        keep 0
    else
        keep least common bit

Only matching lines survive into the next round.

---

### Bit-by-Bit Elimination

Both rating calculations work progressively from left to right.

For each step:

- inspect the current bit column
- filter the list
- increment the column counter
- continue until exactly one binary string remains

This makes the Part 2 solution a repeated narrowing process rather than a single full-report calculation.

---

## 🛠 Implementation Notes

- The class uses strings directly rather than converting the whole input into numeric arrays
- Binary-to-integer conversion is done with `ToInt(2)`
- The epsilon variable is named `epilson` in the implementation
- Part 2 duplicates the input into a mutable `remaining` list before filtering
- Ties favour `1` for oxygen and `0` for CO2
- The same helper method supports both rating modes through a `mode` flag

---

## 🧪 Behaviour Summary

Given a list of equal-length binary strings:

- Part 1 counts bit frequency by column
- It builds gamma from the most common bits
- It builds epsilon from the opposite bits
- Part 2 repeatedly filters the input to isolate oxygen and CO2 values
- Oxygen keeps the most common bit each round
- CO2 keeps the least common bit each round
- The final outputs are produced by binary conversion and multiplication

---

## 🚀 Key Takeaways

- Good example of solving binary-report problems with column-based analysis
- Part 1 is a frequency aggregation problem
- Part 2 is a repeated filtering and elimination problem
- The helper method cleanly centralises the bit-selection rules
- Tie handling is an important detail for correct life support calculation

---

## 🔗 References

- https://adventofcode.com/2021/day/3