# 🎄 Advent of Code 2016 - Day 02: Bathroom Security

## 📜 Puzzle Overview

This puzzle simulates movement across a keypad using directional instructions.

Each input line represents a sequence of moves:

- `U` (up)
- `D` (down)
- `L` (left)
- `R` (right)

Starting from an initial position, each line produces a single digit (or character), forming a multi-digit bathroom code.

Part 1 uses a standard 3x3 keypad, while Part 2 switches to a more complex diamond-shaped keypad.

---

## 🧩 Part 1

Determine the bathroom code using a standard keypad:

```
1 2 3
4 5 6
7 8 9
```

### 💡 Approach

- Start at position `5`
- For each line of input:
  - Process each movement instruction
  - Clamp movement to remain within keypad bounds
- After processing a full line:
  - Record the current button
- Combine all recorded buttons into the final code

---

## 🧩 Part 2

Determine the bathroom code using an irregular keypad:

```
    1
  2 3 4
5 6 7 8 9
  A B C
    D
```

### 💡 Approach

- Start at position `5`
- For each line of input:
  - Process movement instructions
  - Ignore moves that would land on invalid positions
- After processing a full line:
  - Record the current button (which may now be a letter)
- Combine all recorded buttons into the final code

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Bathroom Security`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates the keypad solver
- Processes all instruction lines
- Builds the numeric code

For Part 2:

- Uses the alternate keypad layout
- Processes the same instruction set
- Builds the alphanumeric code

---

### Keypad Representation

Two different keypad layouts are used:

#### Standard Keypad (Part 1)

- Represented as a 2D grid
- Movement is constrained by grid boundaries

#### Complex Keypad (Part 2)

- Represented as a sparse layout
- Invalid positions must be checked before moving

---

### Movement Logic

Each instruction updates the current position:

- `U` decreases row
- `D` increases row
- `L` decreases column
- `R` increases column

For Part 1:

- Movement is clamped within bounds

For Part 2:

- Movement is only applied if the target position exists

---

### Processing Input

Each line is processed independently:

- Start from the last known position
- Apply all moves in sequence
- Record final position as a digit/character

---

### Building the Code

After processing all lines:

- Each line contributes one character
- Characters are appended in order
- Final result is returned as a string

---

## 🛠 Implementation Notes

- Input is processed line-by-line
- Position is preserved between lines
- Movement rules differ between parts
- Part 2 requires explicit validation of positions
- Output is constructed incrementally

---

## 🧪 Behaviour Summary

Given a list of movement instructions:

- Each line navigates the keypad
- Movement is constrained by keypad shape
- Final positions form the code
- Part 1 uses simple bounds checking
- Part 2 uses explicit valid-position checks

---

## 🚀 Key Takeaways

- Grid navigation with constrained movement
- State persists across instruction sequences
- Same input, different rules per part
- Clean separation of keypad logic simplifies implementation

---

## 🔗 References

- https://adventofcode.com/2016/day/2