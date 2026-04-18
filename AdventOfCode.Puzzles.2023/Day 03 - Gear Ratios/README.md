# 🎄 Advent of Code 2023 - Day 03: Gear Ratios

## 📜 Puzzle Overview

This puzzle works with a schematic represented as a 2D grid.

The grid contains:

- digits that form part numbers
- dots (`.`) representing empty space
- symbols such as `*` and others

A number only counts as a valid part number if at least one of its digits touches a symbol in any adjacent direction, including diagonals.

Part 1 sums all valid part numbers.

Part 2 looks specifically for gear positions. A gear is effectively identified when two part numbers are associated with the same symbol position, and their product contributes to the final total.

---

## 🧩 Part 1

Find every part number that touches a symbol and sum them.

### 💡 Approach

- Read the input into a 2D map
- Scan the grid left to right, top to bottom
- Collect consecutive digits into a temporary number buffer
- When the number ends:
  - inspect the surrounding cells of each digit
  - check whether any adjacent cell contains a symbol
- If a symbol is found:
  - add the full number to the recognised parts list
- Return the sum of all recognised part numbers

---

## 🧩 Part 2

Find matching part numbers linked to the same symbol position and sum their gear ratios.

### 💡 Approach

- Reuse the parsed parts discovered in Part 1
- Each recognised part stores:
  - the full number string
  - the symbol character it touched
  - the symbol's grid position
- Group recognised parts by symbol position
- Keep only groups with more than one associated part
- Multiply the first two numbers in each group
- Sum those products

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Gear Ratios`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new GearRatios(this.Input)`
- calls `Count()`

For Part 2:

- creates `new GearRatios(this.Input)`
- calls `Ratio()`

---

### `GearRatios.cs`

This class contains the full parsing and solving logic.

It stores:

- `Map`
- `Part`
- `Parts`

`Map` holds the schematic grid.

`Part` is a temporary list used while reading one multi-digit number from the grid.

`Parts` stores recognised numbers as tuples containing:

- `Number`
- `Symbol`
- `Point`

So once a valid number is found, the implementation records both the number itself and the symbol location it touches.

---

### Grid Representation

The constructor creates:

- `new VectorArray<int, char>(input, new Func<char, char>(x => x))`

This means the puzzle input is loaded directly into a 2D character map.

The solver then immediately calls:

- `Parse()`

to walk through the grid and identify valid part numbers.

---

### `Count()`

This method solves Part 1.

It returns:

- the sum of every recognised part number

using:

    this.Parts.Sum(x => int.Parse(x.Number))

So Part 1 is entirely based on the numbers collected during parsing.

---

### `Ratio()`

This method solves Part 2.

It:

- groups recognised parts by symbol position
- keeps only groups with more than one associated number
- multiplies the first two numbers in each group
- adds the products together

At a high level it behaves like this:

    foreach matching symbol group
        result += first_number * second_number

So the gold answer is the sum of these grouped products.

---

### Parsing the Schematic

`Parse()` scans the grid cell by cell using:

- `this.Map.AxisEnumerator()`

It tracks the current row with:

- `int y = 0`

As it walks the grid:

- if the row changes, it finishes the current buffered number
- if the current cell is a digit, it adds that digit to the current number buffer
- if the current cell is not a digit, it attempts to finalise the buffered number

This allows the solver to build each multi-digit number from consecutive cells.

---

### Temporary Number Buffer

The temporary number buffer is stored in:

- `List<Part> Part`

Each `Part` object stores:

- the digit position
- the digit character

So a number like:

    467

is temporarily held as three digit entries, each with its own coordinate, until the solver decides whether the full number should be accepted.

---

### `TryAddPart()`

This method decides whether the currently buffered number is a real part number.

It only runs when:

- the digit sequence ends
- the row changes
- or parsing reaches the end of the grid

If the buffer contains digits:

- inspect every digit in the buffered number
- get all adjacent and diagonal neighbours with:

    this.Map.AdjacentInterCardinal(number.Point)

- check whether any adjacent cell is:
  - not a dot
  - not a digit

If such a symbol exists:

- join the buffered digits into a full number string
- record the symbol character
- record the symbol position
- add the tuple to `Parts`
- stop checking further digits for that number

Then the temporary buffer is cleared.

This means a number is only added once, even if multiple digits touch symbols.

---

### Symbol Detection

A cell is treated as a valid symbol when:

- it is not `'.'`
- it is not a numeric digit

So symbols are detected with logic equivalent to:

    x.Value != '.' && !char.IsDigit(x.Value)

That means any non-dot, non-digit character can qualify a number as a valid part.

---

### Adjacent Checks

The solver checks adjacency using:

- `AdjacentInterCardinal(...)`

This includes all surrounding directions, not just up, down, left, and right.

So diagonal contact also counts.

That matches the puzzle rule that a part number is valid if any digit touches a symbol in any neighbouring cell.

---

### `Part.cs`

This is a small helper model used while buffering digits.

It stores:

- `Point`
- `Value`

The constructor accepts:

- a `Vector<int>` point
- a `char` digit value

So each digit in a multi-digit number is tracked with both its coordinate and character.

---

## 🛠 Implementation Notes

- The puzzle title is `Gear Ratios`
- The input is loaded into a `VectorArray<int, char>`
- Parsing happens immediately in the constructor
- Multi-digit numbers are built incrementally from consecutive digit cells
- A number is only recorded if at least one digit touches a symbol
- Symbols are defined as any non-dot, non-digit character
- Each stored result keeps the number string, symbol character, and symbol position
- Part 2 groups by symbol position, not by the symbol character itself
- `Ratio()` multiplies the first two numbers in each qualifying group
- The temporary digit holder class is named `Part`

---

## 🧪 Behaviour Summary

Given a schematic grid:

- the solver scans every cell in reading order
- consecutive digits are collected into a temporary number
- when the number ends, surrounding cells are checked
- if any digit touches a symbol, that full number is stored
- Part 1 sums all stored numbers
- Part 2 groups stored numbers by symbol location and multiplies pairs

So the final result is either:

- the sum of all valid part numbers
- or the sum of grouped number products for shared symbol positions

---

## 🚀 Key Takeaways

- The solution uses a grid abstraction instead of manual index arithmetic
- Multi-digit numbers are assembled incrementally from adjacent digit cells
- Symbol detection is done by checking neighbours around each digit
- A number is only stored once, even if several of its digits touch symbols
- Part 2 reuses the parsed part data instead of rescanning the whole grid
- Grouping by symbol position is the key step for computing gear ratios

---

## 🔗 References

- https://adventofcode.com/2023/day/3