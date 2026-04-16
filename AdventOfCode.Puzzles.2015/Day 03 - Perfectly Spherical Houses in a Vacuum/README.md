# 🎄 Advent of Code 2015 - Day 03: Perfectly Spherical Houses in a Vacuum

## 📜 Puzzle Overview

Santa is delivering presents by following a sequence of movement instructions.

Each character in the input represents a move:

- `^` moves **north**
- `v` moves **south**
- `>` moves **east**
- `<` moves **west**

Each time Santa visits a house, that house receives a present.

The goal is to determine how many unique houses receive at least one present. The puzzle defines the movement symbols and asks for the number of houses visited by Santa alone in Part 1, then by Santa and Robo-Santa alternating turns in Part 2.

---

## 🧩 Part 1

Determine how many **unique houses** receive at least one present when Santa follows the full instruction sequence alone.

### 💡 Approach

- Start at house `(0,0)`
- Process each direction one character at a time
- Move Santa to the next location
- Track each visited house
- Count the total number of unique positions visited

---

## 🧩 Part 2

Now Santa is joined by **Robo-Santa**.

They take turns following the instructions:

- Santa moves on the 1st, 3rd, 5th... instructions
- Robo-Santa moves on the 2nd, 4th, 6th... instructions

Determine how many **unique houses** receive at least one present.

### 💡 Approach

- Start both at `(0,0)`
- Alternate which Santa moves after each instruction
- Track all visited houses in the same collection
- Count the total number of unique positions visited

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `PerfectlySphericalHousesInAVacuum(this.Input)`
- Calls `Deliver()`
- Returns `Houses.Count`

For Part 2:

- Creates `PerfectlySphericalHousesInAVacuum(this.Input, true)`
- Calls `Deliver()`
- Returns `Houses.Count` again

The `true` flag enables Robo-Santa mode.

### `PerfectlySphericalHousesInAVacuum.cs`

This class contains the delivery logic.

In the constructor:

- The full direction string is stored from `input[0]`
- A house collection is created starting with `(0,0)`
- A list of Santa positions is created
- If Robo-Santa mode is enabled, a second Santa is added

Your starting house is preloaded into `Houses`, and when Robo-Santa mode is enabled it starts with a value of `2`, which reflects both Santa and Robo-Santa beginning at the same house.

### Delivery Logic

The `Deliver()` method processes the instruction string one character at a time.

For each direction:

1. Move the current Santa using `SymbolTransform(direction)`
2. Add or increment the current house in `Houses`
3. Advance to the next Santa using wrapped indexing

That wrapped index logic is what makes Santa and Robo-Santa alternate turns cleanly in Part 2.

### House Tracking

Your solution stores visited locations in `Houses`, which behaves like a coordinate dictionary.

- A new house is added when first visited
- An existing house is incremented when visited again
- The final answer is the number of unique coordinates in the collection

That means you do not need any duplicate filtering later - `Houses.Count` already gives the answer directly.

---

## 🛠 Implementation Notes

- Input is processed as a single direction string
- Movement is handled through `SymbolTransform(direction)`
- House visits are tracked in a coordinate-based dictionary structure
- Part 2 reuses the same logic by switching to multiple Santa positions
- Alternating turns is handled with wrapped index incrementing rather than splitting the input first

---

## 🧪 Examples

| Input   | Part 1 | Part 2 |
|---------|--------|--------|
| `>`     | 2      | 2      |
| `^>v<`  | 4      | 3      |
| `^v^v^v^v^v` | 2 | 11     |

These example results come from the Advent of Code puzzle description.

---

## 🚀 Key Takeaways

- Nice use of shared logic for both parts
- Part 2 is handled cleanly by tracking multiple Santa positions
- House counting is efficient because uniqueness is built into the storage structure
- The wrapped index approach keeps turn alternation simple and readable

---

## 🔗 References

- https://adventofcode.com/2015/day/3