# 🎄 Advent of Code 2015 - Day 18: Like a GIF For Your Yard

## 📜 Puzzle Overview

Santa is decorating a grid of lights for the holiday display.

Each light is either:

- on, shown as `#`
- off, shown as `.`

The grid updates in steps using rules similar to Conway's Game of Life.

For each light:

- a light that is on stays on when it has `2` or `3` neighbours on
- otherwise it turns off
- a light that is off turns on only when it has exactly `3` neighbours on

Part 1 asks how many lights are on after `100` steps.

Part 2 changes the rules so the four corner lights are stuck on for the entire animation.

---

## 🧩 Part 1

Determine how many lights are on after `100` animation steps.

### 💡 Approach

- Parse the input grid into a numeric map
- For each step, create a copy of the current state
- Count how many adjacent lights are on for every cell
- Apply the update rules to the copied grid
- Replace the current map with the updated version
- Count the lit cells after `100` steps

---

## 🧩 Part 2

Repeat the same animation, but keep the four corner lights permanently on.

### 💡 Approach

- Force the four corners on before the animation starts
- Apply the same update rules as Part 1
- After each step, force the corners back on
- Count the lit cells after `100` steps

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new LikeAGIFForYourYard(this.Input)`
- Calls `Animate(100)`
- Returns `CountLit()`

For Part 2:

- Creates `new LikeAGIFForYourYard(this.Input)`
- Calls `AnimateGameOfLife(100)`
- Returns `CountLit()`

The only difference between the two parts is which animation method is used.

---

### `LikeAGIFForYourYard.cs`

This class contains the full grid simulation.

The constructor converts the input into a `VectorArray` map where:

- `#` becomes `1`
- `.` becomes `0`

This allows the grid to be processed numerically while still matching the puzzle input format.

The class exposes:

- `Animate(int steps)`
- `AnimateGameOfLife(int steps)`
- `CountLit()`

It also includes helper methods for checking all eight neighbouring positions around a cell.

---

### Neighbour Checks

The solver defines a dedicated method for each surrounding direction:

- `IsNorthLit`
- `IsNorthEastLit`
- `IsEastLit`
- `IsSouthEastLit`
- `IsSouthLit`
- `IsSouthWestLit`
- `IsWestLit`
- `IsNorthWestLit`

Each method:

- checks whether the neighbour would fall outside the map
- returns `0` when out of bounds
- otherwise returns the value stored in that neighbour cell

This keeps boundary handling simple and avoids invalid grid access.

---

### Counting Adjacent Lights

The `LitAdjacent()` method sums all eight directional checks for a given point.

At a high level, it adds:

- north
- north-east
- east
- south-east
- south
- south-west
- west
- north-west

The result is the total number of lit neighbours around the current light.

This value is then used to decide whether the light stays on, turns off, or turns on in the next step.

---

### Part 1 Animation

`Animate(int steps)` handles the standard update rules.

For each step:

- create a copy of the current map
- loop through every cell using `AxisEnumerator()`
- count lit neighbours using `LitAdjacent()`
- apply the rules to the copied grid
- replace the original map with the updated copy

For a lit cell:

- it stays lit if adjacent count is between `2` and `3`
- otherwise it turns off

For an unlit cell:

- it turns on only when adjacent count is exactly `3`

Using a copied grid ensures that updates are based only on the previous step, not partially updated values.

---

### Part 2 Animation

`AnimateGameOfLife(int steps)` uses the same core update logic as Part 1, but keeps the four corners permanently on.

Before the loop starts, it forces these positions to `1`:

- top-left
- top-right
- bottom-left
- bottom-right

After every step is calculated and the map is replaced, those same four corners are forced back to `1` again.

This guarantees that corner lights never switch off, even if the normal rules would have turned them off.

---

### Counting Lit Lights

The `CountLit()` method totals every lit cell in the map by summing the values returned from `AxisEnumerator()`.

Because the map stores:

- `1` for on
- `0` for off

the total sum is the number of lights currently lit.

---

### Debug Printing

The `Print(int step)` method outputs the current map as characters:

- `#` for lit
- `.` for unlit

This is useful for visualising the animation state during debugging, although it is not required for the final puzzle answers.

---

## 🛠 Implementation Notes

- The grid is stored as a numeric `VectorArray`
- Boundary checks are handled explicitly in neighbour helper methods
- Updates are applied to a copied grid to avoid corrupting the current step
- Part 2 reuses the Part 1 logic with additional corner-forcing behaviour
- Both parts run for exactly `100` steps
- Final answers are based on summing all lit cells in the resulting grid

---

## 🧪 Examples

Given the example starting grid from the puzzle, after `4` steps the standard version leaves:

- `4` lights on

For the stuck-corners version, after `5` steps the example leaves:

- `17` lights on

These examples show the difference between the normal simulation and the version where the corners remain permanently lit.

---

## 🚀 Key Takeaways

- Good example of grid-based simulation using neighbour rules
- Clear separation between neighbour counting and step updates
- Part 2 is solved by extending the same animation logic with forced corner states
- Copy-on-update keeps each animation step consistent and easy to reason about

---

## 🔗 References

- https://adventofcode.com/2015/day/18