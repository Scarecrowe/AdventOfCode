# 🎄 Advent of Code 2021 - Day 20: Trench Map

## 📜 Puzzle Overview

This puzzle revolves around enhancing an infinite image using an image enhancement algorithm.

The input consists of:

- An **enhancement algorithm** string (512 characters of `.` and `#`)
- An **initial image** grid (also `.` and `#`)

Each pixel in the image is updated based on a 3x3 grid around it, which forms a binary number used to index into the enhancement algorithm.

Key challenge:

- The image is effectively **infinite**, and pixels outside the known grid must still be considered.

---

## 🧩 Part 1

Enhance the image twice and count how many pixels are lit (`#`).

### 💡 Approach

- Parse the enhancement algorithm
- Parse the input image into a coordinate-based structure
- Expand the image bounds to account for edge growth
- For each pixel:
  - Look at its 3x3 neighbourhood
  - Convert that to a binary string (`.` = 0, `#` = 1)
  - Convert binary to an integer index
  - Use that index to look up the new pixel value in the algorithm
- Repeat for 2 iterations
- Count lit pixels

---

## 🧩 Part 2

Enhance the image 50 times and count the lit pixels.

### 💡 Approach

- Same logic as Part 1
- Run for **50 iterations instead of 2**
- Carefully handle the **infinite background flipping behaviour**

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Trench Map`
- Loads the input
- Calls both parts

For Part 1:

- Runs enhancement for 2 iterations

For Part 2:

- Runs enhancement for 50 iterations

---

### Input Parsing

The input is split into:

1. Enhancement algorithm (single line)
2. Image grid (remaining lines)

The algorithm is stored as a string, while the image is typically represented as:

- A coordinate-based structure (e.g. `HashSet<(int x, int y)>` or similar)

Only **lit pixels (`#`)** need to be explicitly stored.

---

### Image Representation

Instead of storing the full infinite grid:

- Only track **lit pixels**
- Maintain current bounds (min/max X and Y)

This allows efficient iteration over relevant areas.

---

### Pixel Enhancement Logic

Each pixel is recalculated based on its 3x3 neighbourhood:

Neighbourhood order (top-left to bottom-right):

```text
(-1,-1) (0,-1) (1,-1)
(-1, 0) (0, 0) (1, 0)
(-1, 1) (0, 1) (1, 1)
```

For each neighbour:

- `#` becomes `1`
- `.` becomes `0`

This forms a 9-bit binary number:

```text
e.g. 000100010
```

Converted to decimal:

```text
binary -> index -> lookup in algorithm
```

---

### Infinite Grid Handling

This is the trickiest part.

Because the algorithm can map:

- `000000000` (all dark) → `#`

the infinite background may **flip between lit and unlit** each iteration.

To handle this:

- Track a `defaultPixel` value for "outside the known grid"
- Update it each iteration based on:
  - index `0` (all dark)
  - index `511` (all lit)

This ensures correct behaviour when expanding beyond known bounds.

---

### Iteration Process

For each iteration:

- Expand bounds by 1 in all directions
- For every coordinate in the expanded area:
  - Build its 3x3 binary value
  - Convert to index
  - Determine if the pixel is lit in the next state
- Replace the current image with the new one
- Update the default infinite pixel if needed

---

### Counting Lit Pixels

After completing all iterations:

- Count how many pixels are `#`

If using a `HashSet`, this is simply:

```text
set.Count
```

---

## 🛠 Implementation Notes

- Only lit pixels are stored for efficiency
- Bounds must expand each iteration
- Infinite grid behaviour must be explicitly tracked
- Binary conversion is central to the logic
- The algorithm string is always 512 characters long

---

## 🧪 Behaviour Summary

Given an initial image:

- Each iteration enhances every pixel using its 3x3 neighbourhood
- The image expands outward each step
- Infinite background may toggle between lit and dark
- Part 1 runs 2 iterations
- Part 2 runs 50 iterations
- Final result is the count of lit pixels

---

## 🚀 Key Takeaways

- Great example of **grid processing with neighbourhood transforms**
- Efficient use of sparse data structures by tracking only lit pixels
- Handling infinite space requires careful state tracking
- Binary-to-index mapping is the core mechanic
- The same logic scales from small (2 iterations) to large (50 iterations)

---

## 🔗 References

- https://adventofcode.com/2021/day/20