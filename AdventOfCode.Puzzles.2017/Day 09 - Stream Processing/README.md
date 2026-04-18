# 🎄 Advent of Code 2017 - Day 09: Stream Processing

## 📜 Puzzle Overview

This puzzle processes a stream of characters containing nested groups and garbage.

The stream consists of:

- groups, enclosed in `{ }`
- garbage, enclosed in `< >`
- cancellation characters `!` that ignore the next character

The goal is to correctly interpret the structure while handling garbage and cancellations.

Part 1 calculates the total score of all groups.  
Part 2 counts how many non-cancelled characters appear inside garbage.

---

## 🧩 Part 1

Determine the total score for all groups.

### 💡 Approach

- Process the stream character by character
- Maintain:
  - current nesting depth
  - whether currently inside garbage
  - cancellation state
- Rules:
  - `{` increases depth (if not in garbage)
  - `}` closes a group and adds its depth to total score
  - `<` enters garbage mode
  - `>` exits garbage mode
  - `!` skips the next character
- Accumulate score based on group depth

---

## 🧩 Part 2

Determine how many characters are inside garbage.

### 💡 Approach

- Use the same parsing logic as Part 1
- While inside garbage:
  - count all characters except:
    - cancelled characters
    - the closing `>`
- Return total count of garbage characters

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Stream Processing`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses stream
- Computes group score

For Part 2:

- Counts garbage characters

---

### Stream Parsing

Process input sequentially:

- Use a loop over each character
- Maintain state flags:
  - `inGarbage`
  - `skipNext`

---

### Cancellation Handling

```
!
```

- Causes the next character to be ignored completely
- Works both inside and outside garbage

---

### Garbage Handling

```
< ... >
```

- Entered with `<`
- Exited with `>`
- All characters inside are ignored for group logic
- Counted for Part 2

---

### Group Scoring

- Each group contributes:

```
score = depth level
```

Example:

```
{} → 1
{{{}}} → 1 + 2 + 3 = 6
```

---

### Part 1 Logic

- Track current depth
- Increase on `{`
- Decrease on `}`
- Add depth when closing group

---

### Part 2 Logic

- While in garbage:
  - count characters
- Ignore:
  - `!` and next character
  - closing `>`

---

## 🛠 Implementation Notes

- Single-pass parsing is sufficient
- State management is key to correctness
- Order of checks matters:
  - cancellation first
  - then garbage state
- Avoid processing cancelled characters

---

## 🧪 Behaviour Summary

Given a character stream:

- Groups and garbage are interleaved
- Cancellation affects parsing flow
- Part 1 computes nested group scores
- Part 2 counts garbage characters

---

## 🚀 Key Takeaways

- Stateful parsing problem
- Order of operations is critical
- Same pass solves both parts
- Clear separation of parsing modes

---

## 🔗 References

- https://adventofcode.com/2017/day/9