# 🎄 Advent of Code 2021 - Day 04: Giant Squid

## 📜 Puzzle Overview

This puzzle simulates a game of **bingo** played against a giant squid.

You are given:

- A sequence of numbers that are drawn in order
- Multiple bingo boards (5x5 grids)

Each time a number is drawn:

- It is marked on every board where it appears
- A board **wins** when an entire row or column is marked (no diagonals)

The final score is calculated as:

- sum of all **unmarked numbers**
- multiplied by the **last number drawn**

Part 1 finds the **first board to win**, while Part 2 finds the **last board to win**.

---

## 🧩 Part 1

Determine the score of the first board to win.

### 💡 Approach

- Parse the drawn numbers
- Parse all bingo boards into structured objects
- For each number drawn:
  - mark the number on every board
  - check if any board has a complete row or column
- As soon as a board wins:
  - calculate its score:
  
        sum(unmarked numbers) * last called number

- Return that value

---

## 🧩 Part 2

Determine the score of the last board to win.

### 💡 Approach

- Reuse the same simulation
- Track which boards have already won
- Continue drawing numbers until all boards have won
- Keep removing or ignoring winning boards as you go
- When only one board remains:
  - let it win
  - calculate its score

This effectively flips the logic from "first winner" to "last survivor"

---

## 🧠 Code Breakdown

### `Day04.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Giant Squid`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Runs the bingo simulation until the first win
- Returns the winning board score

For Part 2:

- Continues simulation until the last board wins
- Returns the final board score

---

### Input Parsing

The input consists of:

1. A single line of comma-separated numbers
2. Multiple bingo boards separated by blank lines

Parsing typically involves:

- splitting the first line into an integer list
- grouping subsequent lines into 5x5 grids
- constructing board objects

---

### `Board` Representation

Each board usually stores:

- a 5x5 grid of numbers
- a matching 5x5 structure tracking marked values
- possibly:
  - last number marked
  - win state

A common structure:

- numbers layer
- marked layer (bool flags)

---

### Marking Numbers

When a number is drawn:

- iterate all boards
- for each board:
  - locate matching values
  - mark them

Conceptually:

    if board[x][y] == number:
        marked[x][y] = true

---

### Win Detection

A board wins when:

- any row is fully marked
- OR any column is fully marked

Example logic:

    for each row:
        if all marked → win

    for each column:
        if all marked → win

No diagonal checks are required

---

### Score Calculation

When a board wins:

1. Sum all unmarked values:

        sum_unmarked = total of all cells where marked == false

2. Multiply by last drawn number:

        score = sum_unmarked * last_number

---

### Simulation Loop

The core loop:

- iterate through drawn numbers
- update all boards
- check for winners

Part 1:

- stop at first winning board

Part 2:

- continue until all boards win
- track winners and remove or skip them

---

### Handling Multiple Winners

In Part 2:

- boards that win are:
  - either removed from the list
  - or flagged and skipped
- continue until one board remains
- that final board determines the answer

---

## 🛠 Implementation Notes

- Boards are typically stored as collections of grids
- Marking and win-checking are separate concerns
- Efficient lookup can be done with:
  - direct scanning
  - or precomputed maps (optional)
- Mutation of board state is central to the solution
- Part 2 requires careful handling of already-winning boards

---

## 🧪 Behaviour Summary

Given:

- a sequence of numbers
- multiple bingo boards

The solver:

- parses all boards
- iterates through drawn numbers
- marks numbers across all boards
- checks for winning conditions

Part 1:

- stops at the first winning board
- returns its score

Part 2:

- continues until the last board wins
- returns the final score

---

## 🚀 Key Takeaways

- Classic grid-based problem with state tracking
- Clear separation between:
  - parsing
  - marking
  - win detection
- Part 2 builds directly on Part 1 by extending the simulation
- Good example of managing collections while mutating state
- Highlights importance of tracking completion state across entities

---

## 🔗 References

- https://adventofcode.com/2021/day/4