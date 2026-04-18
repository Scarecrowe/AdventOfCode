# 🎄 Advent of Code 2024 - Day 03: Mull It Over

## 📜 Puzzle Overview

This puzzle scans corrupted memory for valid multiplication instructions.

The input is a noisy string containing many characters, but the solver only cares about operations that match:

    mul(X,Y)

where `X` and `Y` are integers.

For Part 1:

- every valid `mul(X,Y)` instruction is processed

For Part 2:

- the input also contains control instructions:
  - `do()`
  - `don't()`

These enable or disable whether future `mul(...)` instructions should be included.

The goal is to extract the valid multiplication instructions and sum their results.

---

## 🧩 Part 1

Find the sum of all valid multiplication instructions.

### 💡 Approach

- join the input into one continuous string
- use a regex to find every valid `mul(number,number)` token
- extract both numbers from each match
- multiply them together
- sum all products

---

## 🧩 Part 2

Only count multiplication instructions that are currently enabled.

### 💡 Approach

- parse:
  - `mul(X,Y)`
  - `do()`
  - `don't()`
- start with instructions enabled
- when `don't()` appears:
  - ignore future `mul(...)` instructions
- when `do()` appears:
  - enable them again
- sum only the enabled multiplication operations

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Mull It Over`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new MullItOver(this.Input, false)`
- calls `Calculate()`

For Part 2:

- creates `new MullItOver(this.Input, true)`
- calls `Calculate()`

---

### `MullItOver.cs`

This class contains the parsing and calculation logic.

It stores:

- `Operations`
- `Values`

The constructor:

- parses operations with `ParseOperations(input, flags)`
- extracts number pairs with `GetValues()`

---

### Parsing Operations

`ParseOperations(string[] input, bool flags)` first joins the entire input into one string:

    string.Join(string.Empty, input)

It then uses regex to extract recognised instructions.

For Part 1 it uses:

    mul\(\d+,\d+\)

For Part 2 it uses:

    mul\(\d+,\d+\)|do\(\)|don't\(\)

So only valid instructions are kept, while all corrupted noise is ignored.

---

### Part 1 Parsing Behaviour

When `flags` is `false`:

- only `mul(...)` instructions are collected
- the parsed list is returned immediately

So Part 1 is simply:

- find all valid multiplication tokens
- evaluate all of them

---

### Part 2 Parsing Behaviour

When `flags` is `true`, the solver performs a second filtering pass.

It starts with:

    bool include = true;

Then loops through the parsed operations.

Rules:

- if the token is `mul(...)` and `include` is `true`
  - keep it
- if the token is `don't()`
  - set `include = false`
- if the token is `do()`
  - set `include = true`

This means multiplication instructions are only retained while inclusion is enabled.

---

### Extracting Equation Values

`GetValues()` loops through every stored operation and checks it with:

    IsEquation(x)

A valid equation is one that:

- starts with `mul(`
- ends with `)`
- contains `,`

The numeric values are then extracted by:

- removing `mul(` and `)`
- splitting on `,`
- parsing both tokens as integers

So:

    mul(12,34)

becomes:

- `12`
- `34`

These are stored separately in:

- `ValuesA`
- `ValuesB`

---

### `Values`

The class stores multiplication operands as:

    (List<int> ValuesA, List<int> ValuesB)

So each multiplication is represented as matching positions in the two lists.

Example:

- `ValuesA[i]`
- `ValuesB[i]`

Together these form one multiplication pair.

---

### Calculation

`Calculate()` loops through all parsed value pairs:

    result += this.Values.ValuesA[i] * this.Values.ValuesB[i];

It accumulates the total and returns the final sum.

So the answer is simply:

- the sum of all parsed multiplication results

---

### Helper Methods

#### `IsEquation(string equation)`

Checks whether a token is a multiplication instruction by confirming it:

- starts with `mul(`
- ends with `)`
- contains a comma

#### `GetValuesFromEquation(string equation)`

Extracts the inner text:

    equation[4..^1]

Then splits it into two numbers and parses them.

---

## 🛠 Implementation Notes

- The input is flattened into one continuous string before parsing
- Regex is used to isolate only valid instructions
- Part 2 reuses the same parsing flow with extra control tokens
- Multiplication values are stored in two parallel integer lists
- Calculation is a simple indexed multiplication and sum

---

## 🧪 Behaviour Summary

Given corrupted instruction text:

- the solver ignores invalid noise
- Part 1 extracts every valid `mul(X,Y)` instruction
- Part 2 also processes `do()` and `don't()` toggles
- enabled multiplication instructions are converted into numeric pairs
- each pair is multiplied
- the final result is the sum of all included products

---

## 🚀 Key Takeaways

- Clean regex-driven parsing solution
- Good example of filtering structured tokens from noisy input
- Part 2 adds a simple state toggle without changing the core calculation
- Separating parsing from evaluation keeps the implementation tidy
- Parallel lists make the final summation straightforward

---

## 🔗 References

- https://adventofcode.com/2024/day/3