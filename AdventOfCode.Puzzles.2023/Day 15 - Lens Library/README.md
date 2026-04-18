# 🎄 Advent of Code 2023 - Day 15: Lens Library

## 📜 Puzzle Overview

This puzzle revolves around a custom hashing algorithm and a simulated lens storage system.

You are given a single line of comma-separated instructions. Each instruction represents either:

- adding a lens to a box
- updating an existing lens
- removing a lens

The key mechanic is a **HASH algorithm** that determines which box a lens belongs to.

---

## 🔢 The HASH Algorithm

Each instruction starts as a string. To determine its box:

1. Start with a value of `0`
2. For each character:
   - Add its ASCII value
   - Multiply the result by `17`
   - Take the remainder when divided by `256`

So logically:

    value = ((value + char) * 17) % 256

This produces a number between `0` and `255`, which is the box index.

---

## 🧩 Part 1

Calculate the sum of the HASH values for every instruction.

### 💡 Approach

- Split the input string on commas
- For each instruction:
  - Run the HASH algorithm
  - Add the result to a running total
- Return the final sum

This part does not simulate boxes or lenses, it only evaluates the hash values.

---

## 🧩 Part 2

Simulate a set of 256 boxes, each capable of holding lenses.

Each instruction modifies the boxes based on its format:

### Instruction Types

1. **Add / Update Lens**

    label=number

- Compute the box index using HASH(label)
- If a lens with that label already exists in the box:
  - update its focal length
- Otherwise:
  - append it to the end of the box

---

2. **Remove Lens**

    label-

- Compute the box index using HASH(label)
- Remove the lens with that label from the box (if it exists)

---

### 💡 Approach

- Initialise 256 boxes (each a list)
- Parse each instruction:
  - detect whether it is `=` or `-`
  - extract the label
  - compute the box index via HASH
- Apply the operation:
  - insert/update lens
  - or remove lens

---

### 🔍 Focusing Power Calculation

After processing all instructions:

For each box:

- Each lens has:
  - a position (1-based index)
  - a focal length

The focusing power is calculated as:

    (box_index + 1) * (slot_index + 1) * focal_length

Sum this value across all lenses in all boxes.

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Lens Library`
- Loads the input
- Calls both parts

For Part 1:

- Processes the input as a single string
- Splits on commas
- Applies HASH to each segment
- Sums the results

For Part 2:

- Uses the same parsed instructions
- Passes them into the lens simulation logic
- Returns the computed focusing power

---

### HASH Logic

The HASH function is central to both parts.

It:

- iterates over each character in the string
- applies the rolling calculation
- returns a value between `0` and `255`

This determines which box a label maps to.

---

### Lens Representation

Each lens consists of:

- `Label` (string)
- `FocalLength` (int)

Boxes store lenses in order.

Order matters for the final focusing power calculation.

---

### Box Structure

- There are exactly `256` boxes
- Each box behaves like a list
- Lenses are stored in insertion order unless updated or removed

---

### Processing Instructions

Each instruction is parsed into:

- label
- operation (`=` or `-`)
- optional focal length

Then:

- compute box index using HASH(label)
- perform the corresponding operation

---

### Add / Update Logic

When handling:

    label=number

- Find the box using HASH(label)
- Check if the label already exists:
  - if yes → update focal length
  - if no → append new lens

---

### Remove Logic

When handling:

    label-

- Find the box using HASH(label)
- Remove the lens with that label if it exists
- Do nothing if it does not exist

---

### Final Calculation

After all instructions:

- Iterate over each box
- For each lens:
  - compute focusing power contribution
- Sum everything

---

## 🛠 Implementation Notes

- HASH is deterministic and reused across both parts
- Boxes are indexed `0–255`, but scoring uses `1–256`
- Lens order is preserved unless explicitly modified
- Updates do not change position, only focal length
- Removal shifts remaining lenses forward

---

## 🧪 Behaviour Summary

Given a sequence of instructions:

- Part 1:
  - computes hash values only
  - returns their total

- Part 2:
  - simulates a storage system
  - applies add/update/remove operations
  - computes final focusing power

---

## 🚀 Key Takeaways

- Clean example of a custom hashing algorithm
- Demonstrates stateful simulation using simple data structures
- Highlights importance of order preservation in lists
- Shows how small parsing differences drive different behaviours
- Efficient use of arrays/lists for predictable indexing

---

## 🔗 References

- https://adventofcode.com/2023/day/15