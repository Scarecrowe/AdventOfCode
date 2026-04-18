# 🎄 Advent of Code 2020 - Day 03: Toboggan Trajectory

## 📜 Puzzle Overview

This puzzle works with a repeating map of open squares and trees.

Each input line is one row of terrain made from:

- `.` for open space
- `#` for a tree

Example input:

    ..##.......
    #...#...#..
    .#....#..#.
    ..#.#...#.#

The map repeats infinitely to the right.

Part 1 follows a single slope and counts how many trees are hit.

Part 2 checks multiple slopes and multiplies those tree counts together.

---

## 🧩 Part 1

Count how many trees are encountered when moving right 3 and down 1.

### 💡 Approach

- Read the terrain rows as strings
- Start near the top-left of the map
- Move right 3 and down 1 on each step
- Wrap horizontally when moving past the end of a row
- Count each position that contains `#`
- Return the total number of trees encountered

---

## 🧩 Part 2

Multiply the tree counts from several different slopes.

### 💡 Approach

- Reuse the same terrain input
- Traverse the map with these slopes:
  - right 1, down 1
  - right 3, down 1
  - right 5, down 1
  - right 7, down 1
  - right 1, down 2
- Count the trees for each slope
- Multiply all five results together
- Return the final product

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Toboggan Trajectory`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new TobogganTrajectory(this.Input)`
- calls `Single()`

For Part 2:

- creates `new TobogganTrajectory(this.Input)`
- calls `Multiple()`

So both puzzle parts use the same terrain traversal class.

---

### `TobogganTrajectory.cs`

This class contains the full slope traversal logic.

It stores:

- `Input`

The constructor simply keeps the puzzle rows with:

    public TobogganTrajectory(string[] input) => this.Input = input;

So the solver operates directly on the raw string map.

---

### Part 1 Logic

`Single()` returns:

    this.TraverseSlope(3, 1);

So the silver answer is just one specific traversal:

- move 3 columns right
- move 1 row down

The returned value is the number of trees hit on that route.

---

### Part 2 Logic

`Multiple()` calculates the product of five slope traversals.

It starts with:

    long trees = this.TraverseSlope(1, 1);

Then multiplies in the remaining slopes:

    trees *= this.TraverseSlope(3, 1);
    trees *= this.TraverseSlope(5, 1);
    trees *= this.TraverseSlope(7, 1);
    trees *= this.TraverseSlope(1, 2);

So the gold answer is built from repeated calls to the same helper method.

---

### Core Slope Traversal

`TraverseSlope(int right, int down)` performs the actual map walk.

It begins with:

    int x = right;
    int trees = 0;

Then loops down the map with:

    for (int i = down; i < this.Input.Length; i += down)

So:

- `i` tracks the current row
- `x` tracks the current column
- the row advances by the `down` value each time

This means the traversal skips rows correctly for slopes like down 2.

---

### Tree Detection

On each visited row, the method checks:

    if (this.Input[i][x] == '#')

If that condition is true, it increments:

    trees++;

So every visited coordinate that contains `#` contributes one tree hit.

---

### Horizontal Wrapping

After checking the current position, the code advances `x` with wraparound logic:

    x = x > this.Input[i].Length - (right + 1)
        ? right - (this.Input[i].Length - x)
        : x + right;

This keeps the horizontal position inside the row width while simulating the map repeating to the right.

So instead of extending the string, the implementation reuses the existing row and manually wraps the X position.

---

### Special Handling for `down == 2`

At the end of `TraverseSlope(...)`, the implementation includes an extra block:

    if (down == 2)
    {
        if (this.Input[^1][x + 1] == '#')
        {
            trees++;
        }
    }

So the slope that moves down 2 has a special final-row check after the main loop completes.

This is a specific detail of this implementation and applies only to the `right 1, down 2` traversal used in Part 2.

---

## 🛠 Implementation Notes

- `Day3.cs` uses `Single()` for silver and `Multiple()` for gold
- The map is stored directly as `string[]`
- Part 1 always uses the slope `right 3, down 1`
- Part 2 multiplies the results of five different slope traversals
- Horizontal repetition is handled by wrapping the X coordinate
- Tree hits are counted whenever the current map cell is `#`
- The `down == 2` path includes an extra final-row tree check
- Part 2 uses `long` for the multiplied result

---

## 🧪 Behaviour Summary

Given a repeating map of open squares and trees:

- the solver reads the terrain as an array of strings
- it walks the map according to a chosen right/down slope
- it wraps horizontally when moving past the row width
- it counts tree hits on visited positions
- Part 1 uses one slope
- Part 2 uses five slopes and multiplies their tree totals
- the final results come directly from repeated calls to the same traversal helper

---

## 🚀 Key Takeaways

- Good example of solving a repeating-grid problem without building an infinite map
- The core logic is centralised in one reusable slope traversal method
- Part 1 and Part 2 differ only in which slopes are evaluated
- Horizontal wrapping is handled with direct index arithmetic
- The gold solution is simply the product of multiple independent traversals

---

## 🔗 References

- https://adventofcode.com/2020/day/3