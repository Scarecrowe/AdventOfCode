# 🎄 Advent of Code 2020 - Day 8: Handheld Halting

## 📜 Puzzle Overview

This puzzle simulates a simple handheld game console boot code.

The input is a list of instructions, each containing:

- an operation (`acc`, `jmp`, `nop`)
- a signed argument (e.g. `+4`, `-20`)

Example:

    nop +0
    acc +1
    jmp +4

The goal is to execute the program and detect:

- infinite loops (Part 1)
- how to fix the program so it terminates (Part 2)

The program uses:

- an instruction pointer (current line)
- a single accumulator value (starts at 0)

---

## 🧩 Part 1

Find the accumulator value just before any instruction is executed a second time.

### 💡 Approach

- Parse all instructions into a structured format
- Track visited instruction indices
- Execute instructions sequentially
- Stop when an instruction is about to repeat
- Return the accumulator value at that point

This works because a repeated instruction means the program is stuck in a loop

---

## 🧩 Part 2

Fix the program so it terminates, and return the final accumulator value.

### 💡 Approach

- Iterate through each instruction
- For each `jmp` or `nop`:
  - swap it (`jmp` ↔ `nop`)
  - run the program
- If the program reaches the end (no loop):
  - return the accumulator value

Only one instruction is incorrect, so brute-force swapping works effectively

---

## 🧠 Code Breakdown

### `Day08.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Handheld Halting`
- Loads the input instructions
- Executes both parts

For Part 1:

- Runs the program until a loop is detected
- Returns the accumulator value

For Part 2:

- Attempts instruction swaps
- Runs the program until it terminates successfully

---

### Instruction Model

Each line is parsed into an instruction containing:

- `Operation` (`acc`, `jmp`, `nop`)
- `Argument` (integer)

Example:

    jmp -3

This means:

- jump backwards 3 instructions

---

### Instruction Behaviour

Each operation behaves as follows:

#### `acc`

- Adds the argument to the accumulator
- Moves to the next instruction

    acc +5  → accumulator += 5

---

#### `jmp`

- Jumps relative to the current instruction

    jmp +4 → skip forward 4 lines  
    jmp -3 → go back 3 lines

---

#### `nop`

- Does nothing
- Moves to the next instruction

---

### Program Execution

The program runs with:

- `pointer` → current instruction index
- `accumulator` → running total
- `visited` → set of executed indices

Execution loop:

- If pointer is already in `visited`, stop (loop detected)
- Otherwise:
  - mark pointer as visited
  - execute instruction
  - update pointer accordingly

This loop detection is key to Part 1

---

### Detecting Termination

The program terminates successfully when:

    pointer >= instruction_count

This means execution has moved beyond the last instruction.

---

### Fixing the Program (Part 2)

To repair the program:

- Iterate through all instructions
- Skip `acc` (they are never wrong)
- For each `jmp` or `nop`:
  - clone the instruction list
  - swap the operation
  - run the program

If:

- execution reaches the end → success
- execution loops → try next swap

---

### Brute Force Strategy

At a high level:

- try modifying each candidate instruction
- simulate execution
- stop when a valid termination is found

Because only one instruction is incorrect, this guarantees a solution.

---

## 🛠 Implementation Notes

- Use a `HashSet<int>` or similar to track visited instructions
- Avoid mutating the original instruction list for Part 2
- Cloning the instruction list ensures clean test runs
- `acc` instructions are never swapped
- Loop detection is reused for both parts

---

## 🧪 Behaviour Summary

Given a list of boot instructions:

- the program is executed step-by-step
- Part 1 stops when an instruction repeats
- Part 2 modifies instructions to fix the loop
- execution either loops or terminates
- the accumulator value is returned accordingly

---

## 🚀 Key Takeaways

- Simple virtual machine simulation problem
- Loop detection via visited instruction tracking
- Clear separation between execution and mutation logic
- Brute-force mutation is effective due to small input size
- Reinforces understanding of control flow and program state

---

## 🔗 References

- https://adventofcode.com/2020/day/8