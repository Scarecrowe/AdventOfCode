# 🎄 Advent of Code 2017 - Day 03: Spiral Memory

## 📜 Puzzle Overview

This puzzle works with a spiral grid of numbers.

The grid starts at the center with:

```
1
```

and expands outward in a square spiral pattern:

```
17  16  15  14  13
18   5   4   3  12
19   6   1   2  11
20   7   8   9  10
21  22  23 ...
```

Each number occupies a coordinate in the grid.

Part 1 calculates the distance from a given number to the center.  
Part 2 generates values based on adjacent sums.

---

## 🧩 Part 1

Determine the Manhattan distance from the given number to the center.

### 💡 Approach

- Identify which "ring" the number belongs to:
  - rings expand outward in layers
  - each ring forms a square with odd side lengths
- The distance is composed of:
  - steps to reach the correct ring
  - plus offset within that ring

Manhattan distance is defined as:

```
distance = abs(x) + abs(y)
```

Key idea:

- Find the closest midpoint on the ring side
- Compute distance from that midpoint
- Add ring level to get total steps

---

## 🧩 Part 2

Find the first value written that is larger than the input.

### 💡 Approach

- Build the spiral step by step
- Each position stores:

```
sum of all adjacent cells
```

Including diagonals

Steps:

- Start with `1` at the center
- Move in spiral order:
  - right, up, left, down
- For each new position:
  - sum values of all neighbouring cells
- Stop when value exceeds input

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Spiral Memory`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Computes distance mathematically

For Part 2:

- Builds spiral grid
- Tracks neighbour sums

---

### Spiral Structure

- Numbers grow outward in square layers
- Each layer increases side length by 2
- Corners of each layer are perfect squares

Example:

```
1, 9, 25, 49, ...
```

---

### Ring Calculation

- Determine which ring contains the input
- Ring index increases with distance from center
- Ring defines minimum distance component

---

### Part 1 Logic

- Find ring level
- Determine closest axis midpoint
- Compute offset from midpoint
- Add ring level to get final distance

---

### Spiral Movement

Movement pattern repeats:

```
right → up → left → down
```

Step lengths follow pattern:

```
1, 1, 2, 2, 3, 3, ...
```

---

### Part 2 Logic

- Maintain grid of computed values
- For each new position:
  - check all 8 neighbours
  - sum their values
- Continue until value exceeds input

---

### Neighbour Calculation

For position `(x, y)`:

- Check all adjacent coordinates:

```
(x-1, y-1) to (x+1, y+1)
```

- Ignore positions not yet filled

---

## 🛠 Implementation Notes

- Part 1 can be solved mathematically without building grid
- Part 2 requires grid or coordinate map
- Dictionary or coordinate map works well for sparse grid
- Spiral traversal logic is reusable
- Neighbour lookup must include diagonals

---

## 🧪 Behaviour Summary

Given a spiral of numbers:

- Part 1 computes distance from center
- Part 2 generates values based on neighbours
- Spiral expands outward indefinitely
- Different strategies used for each part

---

## 🚀 Key Takeaways

- Pattern recognition in spiral structures
- Manhattan distance calculation
- Grid simulation with neighbour dependencies
- Mathematical vs simulation-based solutions

---

## 🔗 References

- https://adventofcode.com/2017/day/3