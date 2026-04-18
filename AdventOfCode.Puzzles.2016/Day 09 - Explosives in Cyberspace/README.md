# 🎄 Advent of Code 2016 - Day 09: Explosives in Cyberspace

## 📜 Puzzle Overview

This puzzle works with a compressed string format.

The input contains normal characters and **compression markers** of the form:

```
(AxB)
```

Where:

- `A` = number of characters to take
- `B` = number of times to repeat them

Example:

```
A(1x5)BC
```

Expands to:

```
ABBBBBC
```

Part 1 performs simple expansion.  
Part 2 introduces recursive expansion where markers can appear inside repeated data.

---

## 🧩 Part 1

Determine the length of the decompressed string.

### 💡 Approach

- Iterate through the input string
- When encountering a marker `(AxB)`:
  - read the next `A` characters
  - add `A * B` to the total length
  - skip those `A` characters
- For normal characters:
  - increment length by 1
- Do not expand nested markers

This avoids building the full string and instead tracks length directly.

---

## 🧩 Part 2

Determine the length of the fully decompressed string with recursive expansion.

### 💡 Approach

- Process the string similarly to Part 1
- When encountering a marker `(AxB)`:
  - recursively evaluate the next `A` characters
  - multiply the result by `B`
- Continue until all input is processed

This handles nested markers correctly without constructing large strings.

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Explosives in Cyberspace`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Iterates through the string
- Computes decompressed length without recursion

For Part 2:

- Uses recursive or stack-based evaluation
- Computes full decompressed length

---

### Parsing Markers

Markers follow this format:

```
(AxB)
```

Parsing involves:

- reading until `)`
- splitting on `x`
- converting values to integers

---

### Part 1 Logic

- Walk through the string index by index
- If a marker is found:
  - parse `A` and `B`
  - add `A * B` to total length
  - skip forward by `A`
- Otherwise:
  - increment length by 1

---

### Part 2 Logic

- Define a function to compute length of a segment
- When a marker is found:
  - recursively process the next `A` characters
  - multiply by `B`
- Sum results across the entire string

---

### Recursive Processing

Example:

```
(3x3)XYZ
```

- `XYZ` length = 3
- repeated 3 times → total = 9

Nested example:

```
X(8x2)(3x3)ABCY
```

- `(3x3)ABC` expands first
- then repeated by `(8x2)`

---

### Avoiding Full Expansion

- Strings can grow extremely large in Part 2
- Always compute length instead of building the output
- Use recursion or a stack to track segments

---

## 🛠 Implementation Notes

- Index management is critical when skipping segments
- Parsing markers must be precise
- Recursion depth depends on nesting level
- Use `long` (or equivalent) for large lengths
- Avoid string concatenation for performance

---

## 🧪 Behaviour Summary

Given a compressed string:

- Markers define repeated segments
- Part 1 ignores nested markers
- Part 2 evaluates markers recursively
- Output is the total decompressed length

---

## 🚀 Key Takeaways

- String parsing with embedded instructions
- Recursive evaluation vs linear scanning
- Avoiding full expansion is essential for performance
- Same format, different interpretation rules per part

---

## 🔗 References

- https://adventofcode.com/2016/day/9