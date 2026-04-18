# 🎄 Advent of Code 2020 - Day 5: Binary Boarding

## 📜 Puzzle Overview

This puzzle decodes airplane boarding passes using binary space partitioning.

Each boarding pass is a string of characters:

- `F` / `B` → front / back (rows)
- `L` / `R` → left / right (columns)

Example:

    FBFBBFFRLR

This encodes:

- a row (0–127)
- a column (0–7)

From this, a **seat ID** is calculated:

    seat_id = row * 8 + column

---

## 🧩 Part 1

Find the highest seat ID from all boarding passes.

### 💡 Approach

- Read each boarding pass string
- Convert it into a row and column using binary logic
- Compute the seat ID
- Track and return the maximum seat ID found

---

## 🧩 Part 2

Find the missing seat ID.

### 💡 Approach

- Generate all seat IDs from the input
- Sort the seat IDs
- Scan for a gap where:

    next_id != current_id + 1

- Return the missing seat ID in that gap

---

## 🧠 Code Breakdown

### `Day05.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Binary Boarding`
- Loads the input
- Executes both parts

For Part 1:

- Parses all boarding passes
- Computes seat IDs
- Returns the maximum

For Part 2:

- Reuses parsed seat IDs
- Sorts them
- Finds the missing ID

---

### Boarding Pass Decoding

Each boarding pass is split into two parts:

- First 7 characters → row
- Last 3 characters → column

Example:

    FBFBBFFRLR

- `FBFBBFF` → row
- `RLR` → column

---

### Binary Conversion

The characters are treated as binary instructions:

#### Row (7 bits)

- `F` = 0
- `B` = 1

So:

    FBFBBFF → 0101100 → 44

#### Column (3 bits)

- `L` = 0
- `R` = 1

So:

    RLR → 101 → 5

---

### Seat ID Calculation

Once row and column are known:

    seat_id = row * 8 + column

Example:

    row = 44
    column = 5

    seat_id = 44 * 8 + 5 = 357

---

### Input Parsing

At a high level:

- Read each line
- Convert row portion to binary
- Convert column portion to binary
- Calculate seat ID
- Store results in a collection

---

### Finding the Maximum (Part 1)

The solution:

- Iterates all seat IDs
- Tracks the highest value
- Returns it as the answer

---

### Finding the Missing Seat (Part 2)

After sorting the seat IDs:

- Iterate sequentially
- Compare adjacent values

If:

    ids[i + 1] != ids[i] + 1

Then:

    missing_id = ids[i] + 1

This works because:

- Your seat is not at the very front or back
- The gap will appear somewhere in the middle

---

## 🛠 Implementation Notes

- Boarding passes are treated as binary numbers
- String replacement or bitwise logic can be used
- Sorting is required for Part 2
- Seat IDs are unique
- The missing seat is guaranteed to have neighbours

---

## 🧪 Behaviour Summary

Given a list of boarding pass strings:

- each string is split into row and column instructions
- both are converted into binary numbers
- seat IDs are calculated
- Part 1 finds the highest seat ID
- Part 2 finds the missing seat ID in the ordered list

---

## 🚀 Key Takeaways

- Classic binary space partitioning problem
- Clean mapping from characters to binary digits
- Efficient solution using sorting and linear scan
- Demonstrates how encoding schemes can be decoded into numeric values

---

## 🔗 References

- https://adventofcode.com/2020/day/5