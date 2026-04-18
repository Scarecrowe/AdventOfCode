# 🎄 Advent of Code 2016 - Day 05: How About a Nice Game of Chess

## 📜 Puzzle Overview

This puzzle generates a password using repeated hashing.

You are given a Door ID (your input), and must compute MD5 hashes of:

```
DoorID + index
```

Where `index` starts at `0` and increases.

A hash is considered valid if:

- its hexadecimal representation starts with `00000`

Valid hashes contribute characters toward an 8-character password.

Part 1 builds the password sequentially, while Part 2 uses positional placement.

---

## 🧩 Part 1

Determine the password by collecting characters from valid hashes.

### 💡 Approach

- Start with:
  - `index = 0`
  - empty password
- For each index:
  - compute MD5 hash of `DoorID + index`
  - check if hash starts with `00000`
- If valid:
  - take the 6th character of the hash
  - append it to the password
- Repeat until password length is 8

---

## 🧩 Part 2

Determine the password using positional placement.

### 💡 Approach

- Start with an empty 8-character password (initially unset)
- For each valid hash:
  - use the 6th character as a position (`0-7`)
  - use the 7th character as the value
- Only apply if:
  - position is valid (`0-7`)
  - that position is not already filled
- Continue until all positions are filled

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `How About a Nice Game of Chess`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Iterates over increasing indices
- Builds password sequentially

For Part 2:

- Uses positional assignment logic
- Tracks filled positions

---

### Hash Generation

Each iteration computes:

```
MD5(DoorID + index)
```

- Output is converted to hexadecimal
- Only hashes starting with `00000` are considered valid

This is effectively a brute-force search problem.

---

### Valid Hash Detection

A hash qualifies if:

```
hash.StartsWith("00000")
```

These hashes are relatively rare, making performance an important factor.

---

### Part 1 Logic

- Maintain a growing string
- For each valid hash:
  - append `hash[5]`
- Stop when length reaches 8

---

### Part 2 Logic

- Maintain an array of 8 characters
- For each valid hash:
  - parse `hash[5]` as position
  - if valid and empty:
    - assign `hash[6]` to that position
- Stop when all positions are filled

---

### Handling Positions

For Part 2:

- Positions must be numeric (`0-7`)
- Ignore invalid or already-filled positions
- Prevent overwriting existing values

---

## 🛠 Implementation Notes

- MD5 hashing is the core operation
- Performance matters due to the large number of hashes
- Early exit once password is complete
- Part 2 requires validation before assignment
- Optional optimisation: parallel processing

---

## 🧪 Behaviour Summary

Given a Door ID:

- Generate hashes incrementally
- Filter hashes based on prefix condition
- Extract characters from valid hashes
- Part 1 builds password sequentially
- Part 2 fills positions based on hash-derived indices

---

## 🚀 Key Takeaways

- Brute-force hashing problem similar to proof-of-work
- Low probability filter drives performance cost
- Same hash stream, two extraction strategies
- Part 2 introduces positional constraints

---

## 🔗 References

- https://adventofcode.com/2016/day/5