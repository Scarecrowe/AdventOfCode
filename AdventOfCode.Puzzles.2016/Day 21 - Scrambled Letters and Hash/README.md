# 🎄 Advent of Code 2016 - Day 21: Scrambled Letters and Hash

## 📜 Puzzle Overview

This puzzle implements a string scrambling system.

You are given:

- an initial password string
- a list of instructions that transform it step-by-step

Each instruction modifies the string using operations such as:

- swapping characters
- rotating the string
- reversing sections
- moving characters

Part 1 applies all instructions to produce a scrambled password.  
Part 2 reverses the process to recover the original password.

---

## 🧩 Part 1

Determine the final scrambled password after applying all instructions.

### 💡 Approach

- Start with the initial string
- Parse each instruction
- Apply transformations in order
- Return the final string

---

## 🧩 Part 2

Determine the original password by reversing the scrambling process.

### 💡 Approach

- Start with the scrambled password
- Process instructions in reverse order
- Apply the inverse of each operation
- Return the resulting string

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Scrambled Letters and Hash`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Applies transformations sequentially

For Part 2:

- Reverses transformations
- Processes instructions in reverse order

---

### Instruction Types

#### Swap Position

```
swap position X with position Y
```

- Swap characters at index `X` and `Y`

---

#### Swap Letter

```
swap letter X with letter Y
```

- Replace all occurrences of `X` with `Y` and vice versa

---

#### Rotate Left / Right

```
rotate left X
rotate right X
```

- Shift all characters left or right
- Wrap around the string

---

#### Rotate Based on Position

```
rotate based on position of letter X
```

- Find index of `X`
- Rotate right:
  - `1 + index`
  - plus 1 extra if index ≥ 4

---

#### Reverse Positions

```
reverse positions X through Y
```

- Reverse substring between indices `X` and `Y` (inclusive)

---

#### Move Position

```
move position X to position Y
```

- Remove character at `X`
- Insert it at `Y`

---

### Part 1 Logic

- Iterate through instructions
- Apply each transformation directly
- Maintain updated string state

---

### Part 2 Logic

- Reverse instruction order
- Apply inverse operations:

Examples:

- swap operations → same as forward
- rotate left → rotate right
- move X → Y → becomes move Y → X

The tricky case:

- "rotate based on position"
  - must be reversed manually
  - often solved by:
    - testing all possible rotations
    - selecting the one that matches

---

### Reversing Rotation Based on Position

This operation is not directly invertible.

Typical approach:

- try all possible left rotations
- apply forward rule
- find which produces the current string

---

## 🛠 Implementation Notes

- String manipulation is central
- Parsing instructions cleanly is important
- Part 2 requires careful inversion logic
- Some operations are symmetric, others are not
- Brute-force inversion is acceptable for small strings

---

## 🧪 Behaviour Summary

Given a set of string operations:

- Part 1 applies transformations in order
- Part 2 reverses those transformations
- Final result depends on correct interpretation of each rule

---

## 🚀 Key Takeaways

- Instruction-driven string manipulation
- Reversible vs non-reversible operations
- Importance of inversion logic
- Small input size allows brute-force where needed

---

## 🔗 References

- https://adventofcode.com/2016/day/21