# 🎄 Advent of Code 2022 - Day 5: Supply Stacks

## 📜 Puzzle Overview

This puzzle simulates moving crates between stacks.

The input has two sections:

- a visual drawing of crate stacks
- a list of move instructions

Each move instruction follows the format:

    move X from Y to Z

The solver first parses the drawing into a list of stacks, then executes the moves.

Part 1 moves crates one at a time, which reverses their order during transfer.

Part 2 moves groups of crates while preserving their original order.

---

## 🧩 Part 1

Determine the final top crate of each stack after moving crates one at a time.

### 💡 Approach

- Parse the stack drawing into `Stack<char>` collections
- Read each move instruction as:
  - amount
  - source stack
  - destination stack
- For each move:
  - pop one crate at a time from the source
  - push it onto the destination
- After all moves, pop the top crate from every stack
- Join those characters into the final answer string

---

## 🧩 Part 2

Determine the final top crate of each stack after moving groups of crates without changing their order.

### 💡 Approach

- Reuse the same parsed stacks
- For each move:
  - pop the requested number of crates into a temporary list
  - reverse that temporary list
  - push them onto the destination stack
- After all moves, pop the top crate from every stack
- Join those characters into the final answer string

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Supply Stacks`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `SupplyStacks.Single(this.Input)`

For Part 2:

- Calls `SupplyStacks.Multiple(this.Input)`

---

### `SupplyStacks.cs`

This class contains the full puzzle logic.

It provides three main methods:

- `Single(string[] input)`
- `Multiple(string[] input)`
- `ParseStacks(string[] input)`

It also includes:

- `ParseMove(string line)`

So the solution is implemented entirely through static helper methods.

---

### Stack Representation

The crate stacks are stored as:

- `List<Stack<char>>`

Each entry in the list is one crate stack.

Each `Stack<char>` stores crate labels as single characters.

---

### Parsing the Input Sections

Both puzzle parts begin by splitting the input into two sections:

- the crate drawing
- the move instructions

The code does this with:

    input.ToList().Split(string.Empty)

Then it processes the second section as the move list.

So the blank line in the puzzle input acts as the separator between drawing and instructions.

---

### Parsing the Stacks

`ParseStacks(...)` walks through the input until it reaches the blank line.

It keeps track of how many lines belong to the drawing.

Once it finds the separator, it processes those drawing lines from bottom to top.

That means it starts with:

- the stack number row
- then works upward through the crate rows in reverse order

When it reads the stack number line, it creates one empty `Stack<char>` for each non-empty token found.

---

### Reading Crate Positions

For each crate row, the parser reads the line in blocks of 4 characters:

- `[A] `
- or similar spacing

It trims each chunk and checks whether it contains a crate.

If it does, it removes the square brackets and pushes the crate letter onto the appropriate stack.

Because the rows are processed from bottom to top, the stacks end up in the correct order for later pop and push operations.

---

### Parsing Move Instructions

`ParseMove(string line)` converts a move line into three integers.

It removes the words:

- `move `
- `from `
- `to `

Then it splits the remaining text and converts it to integers.

So a line like:

    move 3 from 1 to 2

becomes:

- amount = `3`
- source = `1`
- destination = `2`

These values are returned as an `int[]`.

---

### Part 1 Movement Logic

`Single(...)` executes the moves one crate at a time.

For each parsed move it loops from `1` to the move count and does:

- pop from the source stack
- push onto the destination stack

This means multi-crate transfers are performed as repeated single moves.

As a result, the order of moved crates is reversed.

After all moves complete, the method returns:

- the top crate popped from each stack
- joined into one string

---

### Part 2 Movement Logic

`Multiple(...)` preserves crate order during transfer.

For each move it creates:

- `List<char> toMove = new();`

It then:

- pops the requested number of crates from the source stack
- stores them in `toMove`
- reverses `toMove`
- pushes them onto the destination stack in that reversed order

This extra reverse step restores the original ordering of the moved group.

So Part 2 behaves like moving a block of crates together.

---

### Final Answer Construction

Both puzzle parts end the same way.

They compute:

- `crates.Select(x => x.Pop()).Join()`

So the final result is built by:

- popping the top crate from each stack
- concatenating those crate letters into a single string

---

## 🛠 Implementation Notes

- The solution uses `List<Stack<char>>` for crate storage
- The input is split into drawing and move sections using a blank line
- Stack parsing processes rows from bottom to top
- Crate rows are read in fixed-width 4-character chunks
- Part 1 moves crates directly between stacks one at a time
- Part 2 uses a temporary list plus `Reverse()` to preserve group order
- Move instructions are parsed by stripping words and converting the remaining numbers

---

## 🧪 Behaviour Summary

Given a crate drawing and a list of move commands:

- the solver parses the drawing into stack structures
- each move is converted into amount, source, and destination
- Part 1 performs repeated single-crate moves
- Part 2 performs grouped moves with order preservation
- both parts return the string formed by the top crate of each stack after all moves

---

## 🚀 Key Takeaways

- Nice example of using stack data structures for a puzzle simulation
- The same parsed input supports two movement rules
- Part 1 reverses order naturally through repeated pop and push operations
- Part 2 preserves order by buffering moved crates temporarily
- The visual stack drawing is handled by fixed-width chunk parsing

---

## 🔗 References

- https://adventofcode.com/2022/day/5