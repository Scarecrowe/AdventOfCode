# 🎄 Advent of Code 2015 - Day 02: I Was Told There Would Be No Math

## 📜 Puzzle Overview

The elves need to calculate how much wrapping paper and ribbon is required for a list of presents.

Each present is a box with dimensions:

- `l` = length
- `w` = width
- `h` = height

For wrapping paper, each box needs:

- Total surface area
- Plus extra slack equal to the area of the smallest side

For ribbon, each box needs:

- The smallest perimeter around any face
- Plus extra ribbon equal to the cubic volume of the box

The puzzle input is provided as dimensions in the format:

- `2x3x4`
- `1x1x10`

The Advent of Code puzzle defines wrapping paper as surface area plus the area of the smallest side.

---

## 🧩 Part 1

Calculate the total square feet of **wrapping paper** required for all presents.

### 💡 Approach

For each present:

1. Parse the dimensions
2. Calculate the surface area
3. Add the smallest side area as slack
4. Sum the total for all presents

---

## 🧩 Part 2

Calculate the total feet of **ribbon** required for all presents.

### 💡 Approach

For each present:

1. Parse the dimensions
2. Find the smallest perimeter from any two sides
3. Add the volume of the box for the bow
4. Sum the total for all presents

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the input data
- Calls the silver and gold solutions

`Silver()` returns the total wrapping paper required.

`Gold()` returns the total ribbon required.

### `IWasToldThereWouldBeNoMath.cs`

This class acts as the coordinator for the puzzle logic.

- Stores the raw input dimensions
- Creates a `Present` object for each line
- Sums the result of `WrappingPaper()`
- Sums the result of `Ribbon()`

This keeps the main puzzle logic clean and pushes the actual calculations into the `Present` model.

### `Present.cs`

This is where the real work happens.

Each input line is split on `x` and converted into:

- `Length`
- `Width`
- `Height`

The class then exposes two methods:

- `WrappingPaper()`
- `Ribbon()`

For wrapping paper, your code uses helper methods to calculate:

- The full surface area
- The smallest face area

For ribbon, it uses helper methods to calculate:

- The smallest perimeter
- The volume

This makes the implementation neat and reusable by keeping the maths out of the higher-level puzzle classes.

---

## 🛠 Implementation Notes

- Input is processed one line at a time
- Each line is converted into a strongly typed `Present`
- Totals are calculated using `Sum(...)`
- Mathematical calculations are delegated to shared helper methods
- The solution is cleanly separated into:
  - Puzzle entry point
  - Aggregation logic
  - Present-level calculations

---

## 🧪 Examples

| Input    | Wrapping Paper | Ribbon |
|----------|----------------|--------|
| `2x3x4`  | 58             | 34     |
| `1x1x10` | 43             | 14     |

These examples come directly from the puzzle description.

---

## 🚀 Key Takeaways

- Good example of splitting logic into small focused classes
- Keeps parsing and calculation separate from puzzle orchestration
- Reuses helper methods for common maths operations
- Easy to extend and easy to read

---

## 🔗 References

- https://adventofcode.com/2015/day/2