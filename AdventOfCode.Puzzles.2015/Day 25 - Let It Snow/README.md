# 🎄 Advent of Code 2015 - Day 25: Let It Snow

## 📜 Puzzle Overview

Santa needs a code from a weather machine manual, but the values are filled into the grid diagonally rather than row by row.

The sequence starts with the code:

- `20151125`

Each following code is generated from the previous one using:

- multiply by `252533`
- then take the remainder when divided by `33554393`

The puzzle input provides a target row and column, and the goal is to determine the code that appears at that exact grid position.

Part 1 asks for that generated code.

Part 2 does not require a second numeric solution in this implementation. The gold answer returns a completion message instead.

---

## 🧩 Part 1

Determine the code found at the requested row and column in the diagonal grid.

### 💡 Approach

- Parse the target row and column from the input sentence
- Start from the first grid position at row `1`, column `1`
- Walk the grid diagonally in the same order described by the puzzle
- Generate the next code value at every step
- Stop when the target position is reached
- Return the current code value

---

## 🧩 Part 2

This implementation does not calculate a second puzzle value.

### 💡 Approach

- Return the completion message for Day 25 instead of running another solver

The gold result is:

- `You have enough stars to [Turn It Off and On]`

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new LetItSnow(this.Input)`
- Calls `Generate()`

For Part 2:

- Returns the completion message directly instead of solving another computation path.

---

### `LetItSnow.cs`

This class contains the coordinate parsing and code-generation logic.

It defines two constants used during code generation:

- `Mulitplier = 252533`
- `Divisor = 33554393`

The constructor parses the input sentence and extracts:

- `Row`
- `Column`

The main public method is:

- `Generate()`

---

### Parsing the Target Coordinates

The constructor reads the first input line and splits it into tokens.

From those tokens it extracts:

- the row value
- the column value

Those values are stored as integer properties and used later to stop the traversal when the target grid position is reached.

---

### Grid Traversal

`Generate()` starts with:

- `value = 20151125`
- `y = 1`
- `x = 1`

It then walks through the diagonal pattern one position at a time.

For each step:

- decrement the row
- increment the column
- if the row would drop to `0`, move to the start of the next diagonal by setting:
  - `y = x`
  - `x = 1`

At a high level, the traversal behaves like this:

    start at (1, 1)
    move to (2, 1)
    then (1, 2)
    then (3, 1)
    then (2, 2)
    then (1, 3)

This matches the diagonal numbering used by the puzzle.

---

### Generating the Next Code

After each grid move, the next value is calculated using:

    value = (value * Mulitplier) % Divisor

This repeats until the current coordinates match the target row and column.

Once the coordinates match, the loop stops and the current value is returned.

---

### Ending Condition

The generator checks this condition each loop:

- current row equals target row
- current column equals target column

As soon as both match, generation stops and the result is returned.

This means the algorithm does not precompute the whole grid. It only advances until the required position is found.

---

## 🛠 Implementation Notes

- Input is parsed from the puzzle sentence rather than from a simple numeric format
- The solver walks the diagonal grid directly instead of calculating an index formula
- Code generation uses modular multiplication at each step
- Part 1 is the only computed puzzle answer in this implementation
- Part 2 returns a completion message rather than a second generated value

---

## 🧪 Examples

The code sequence begins:

- row `1`, column `1` = `20151125`
- row `2`, column `1` = `31916031`
- row `1`, column `2` = `18749137`

These values follow the diagonal traversal order and the same modular generation rule used in the solver.

---

## 🚀 Key Takeaways

- Good example of combining coordinate traversal with iterative value generation
- Keeps the solution simple by walking the grid directly
- Uses modular arithmetic to generate each new code value
- Stops as soon as the requested position is reached, so no unnecessary grid storage is required

---

## 🔗 References

- https://adventofcode.com/2015/day/25