# 🎄 Advent of Code 2016 - Day 03: Squares With Three Sides

## 📜 Puzzle Overview

This puzzle works with triangle side lengths.

Each input line contains three integers representing the sides of a triangle.

A triangle is considered valid if:

- the sum of any two sides is greater than the third side

In practice, this simplifies to:

- sort the sides
- check if the two smallest values sum to more than the largest

Part 1 evaluates triangles row-by-row, while Part 2 changes how the input is interpreted entirely.

---

## 🧩 Part 1

Determine how many of the listed triangles are valid.

### 💡 Approach

- Parse each line into three integers
- Sort the three side lengths
- Check:
  
  ```
  a + b > c
  ```

- Count how many triplets satisfy this condition

This works because only the largest side needs to be compared once the values are sorted.

---

## 🧩 Part 2

Determine how many triangles are valid when reading the input **column-wise instead of row-wise**.

### 💡 Approach

- Parse all rows into triplets
- Group input into chunks of three rows
- For each group:
  - Form three new triangles using vertical columns:
    - column 1 → triangle 1
    - column 2 → triangle 2
    - column 3 → triangle 3
- Apply the same triangle validation logic as Part 1
- Count valid triangles

This effectively transposes the input and creates new triangle groupings.

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Squares With Three Sides`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses each line into side lengths
- Counts valid triangles directly

For Part 2:

- Reinterprets input in vertical groups
- Counts valid triangles after transformation

---

### Parsing Input

Each line is split into three integers:

```
101 301 501
```

Becomes:

- `a = 101`
- `b = 301`
- `c = 501`

Whitespace handling is important due to uneven spacing in the input.

---

### Triangle Validation

Each triangle is validated using the triangle inequality:

```
a + b > c
```

After sorting:

- `a <= b <= c`
- Only one comparison is required

This is the core check used in both parts.

---

### Part 1 Logic

- Iterate through each parsed row
- Sort the three values
- Count how many satisfy the triangle condition

---

### Part 2 Transformation

Instead of reading rows directly, the input is restructured:

Given:

```
101 301 501
102 302 502
103 303 503
```

You form triangles as:

```
(101, 102, 103)
(301, 302, 303)
(501, 502, 503)
```

This is done by:

- processing rows in groups of three
- extracting columns into new triplets

---

### Counting Valid Triangles

For both parts:

- each valid triangle increments a counter
- final result is the total count

---

## 🛠 Implementation Notes

- Sorting simplifies triangle validation
- Input spacing must be handled carefully
- Part 2 requires regrouping rather than re-parsing
- Logic for validation is reused across both parts
- Processing in chunks of three rows is key for Part 2

---

## 🧪 Behaviour Summary

Given a list of side-length triplets:

- Part 1 evaluates each row independently
- Part 2 reorganizes the input into vertical triangles
- Both parts apply the same triangle inequality rule
- Only valid triangles are counted

---

## 🚀 Key Takeaways

- Classic use of triangle inequality
- Sorting reduces validation complexity
- Input transformation is the real challenge in Part 2
- Same validation logic, different data interpretation

---

## 🔗 References

- https://adventofcode.com/2016/day/3