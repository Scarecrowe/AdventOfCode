# 🎄 Advent of Code 2025 - Day 06: Trash Compactor

## 📜 Puzzle Overview

This puzzle processes a text layout of numbers arranged into vertical blocks.

Each block represents a calculation column, and each column ends with an operator on the final row.

Two different interpretations are used:

- Part 1 reads the input top-to-bottom by splitting rows into space-separated values
- Part 2 reads the input left-to-right by treating vertical character blocks as separate number groups

The supported operators are:

- `+` for addition
- `*` for multiplication

Part 1 evaluates each constructed column using the operator found at the bottom of that column.

Part 2 scans the input as fixed-width blocks separated by empty vertical columns, extracts numbers vertically, then applies the operator found on the bottom row of each block.

---

## 🧩 Part 1

Determine the total by evaluating each parsed column from the standard row-based input layout.

### 💡 Approach

- Split each input row into space-separated values
- Rebuild the data into columns
- Treat the last value in each column as the operator
- Parse all earlier values in that column as numbers
- Apply either addition or multiplication
- Sum the result of every column

---

## 🧩 Part 2

Determine the total by reading the layout from left to right as vertical blocks.

### 💡 Approach

- Pad all rows to the same width
- Detect column ranges separated by fully blank vertical columns
- For each block, read digits vertically to form numbers
- Read the operator from the last row of the block
- Apply either addition or multiplication
- Sum the result of every block

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Trash Compactor`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TrashCompactor(this.Input)`
- Calls `Sum()`

For Part 2:

- Creates `new TrashCompactor(this.Input)`
- Calls `RightToLeftSum()`

The two parts share the same solver and differ only in how the input is interpreted.

---

### `TrashCompactor.cs`

This class contains the full parsing and calculation logic.

It stores:

- `Input`

The main public methods are:

- `Sum()`
- `RightToLeftSum()`

Part 1 works from row tokens.

Part 2 works from character positions across the full text layout.

---

### Part 1 Parsing

`Sum()` begins by splitting every line into space-separated entries:

- each row becomes a list of tokens
- rows can have different lengths
- missing values are treated as empty when columns are rebuilt

The method then transposes the row data into columns.

At a high level, the process behaves like this:

- read each row left to right
- group values by shared column index
- evaluate one column at a time

The final entry in each column is treated as the operator.

All earlier entries are parsed as numbers.

---

### Part 1 Evaluation

For each rebuilt column:

- if the last token is `*`, multiply all numeric values
- otherwise sum all numeric values

That means each column behaves as a self-contained calculation.

The result of every column is then added into the final total returned by `Sum()`.

---

### Part 2 Layout Processing

`RightToLeftSum()` interprets the input differently.

Instead of splitting on spaces, it treats the input as a fixed-width character grid.

It first finds:

- the maximum row width
- the number of rows

Then it pads every input line to the same width using spaces.

This makes vertical scanning safe across the full layout.

---

### Detecting Vertical Blocks

Part 2 uses separator columns to split the layout into blocks.

A separator column is defined as a column where every row contains a space.

`Seperators()` scans from left to right and collects each non-empty block as:

- `start`
- `end`

At a high level:

- skip fully blank columns
- mark the start of a block
- continue until the next fully blank column
- store the block bounds

Each block is then processed independently.

---

### Reading Numbers Vertically

`Numbers(start, end, rows)` reads each column inside a detected block.

For each x-position in the block:

- scan downward through every row except the final row
- collect non-space characters
- keep only the digit characters
- build a number string
- parse it into a `long`

This means a number is assembled from the visible digits stacked vertically in that column.

If a vertical slice contains no digits, it is ignored.

---

### Reading the Operator

`Operator(start, end, rows)` reads the bottom row of the current block.

It scans left to right across the block range and returns the first non-space character it finds.

That character is used as the operator for the entire block.

If no operator is found, the method returns a space.

---

### Part 2 Evaluation

Once a block has been parsed:

- if the operator is `+`, add all numbers in the block
- otherwise multiply all numbers in the block

Blocks with no numbers are skipped.

The result from each block is added to the final total returned by `RightToLeftSum()`.

---

### Supporting Helpers

The solver uses a few helper methods to keep the block logic clean:

- `IsSeparatorColumn()` checks whether a full vertical column is blank
- `PadInput()` ensures every row has the same width
- `Seperators()` finds each non-empty vertical block
- `Operator()` extracts the operator from the last row
- `Numbers()` builds numeric values from vertical digit scans

These helpers separate layout detection from numeric evaluation.

---

## 🛠 Implementation Notes

- Part 1 reads the input as tokenised rows and rebuilds columns
- Part 2 reads the input as a fixed-width character grid
- Empty vertical columns act as separators between calculation blocks
- Numbers in Part 2 are built from vertically stacked digits
- The last row is reserved for the operator in Part 2
- Both parts return the sum of all evaluated block or column results

---

## 🧪 Behaviour Summary

Part 1 behaves like a row-to-column transpose:

- rows are split into tokens
- tokens are regrouped by column index
- the last token in each column becomes the operator

Part 2 behaves like a visual layout parser:

- the text is padded into a grid
- blank vertical columns separate blocks
- each vertical slice inside a block becomes a number
- the bottom row supplies the block operator

This means the same input concept is processed in two different structural ways across the two puzzle parts.

---

## 🚀 Key Takeaways

- Good example of solving two parts with two very different parsing strategies
- Part 1 is based on tokenised row data and column transposition
- Part 2 switches to full character-grid analysis and vertical number extraction
- Helper methods keep the block detection and evaluation logic separated cleanly
- The implementation stays compact while supporting both row-based and layout-based interpretation

---

## 🔗 References

- https://adventofcode.com/2025/day/6