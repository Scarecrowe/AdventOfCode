# 🎄 Advent of Code 2016 - Day 04: Security Through Obscurity

## 📜 Puzzle Overview

This puzzle processes a list of encrypted room names.

Each line contains:

- an encrypted name (lowercase letters and dashes)
- a sector ID
- a checksum in square brackets

Example:

```
aaaaa-bbb-z-y-x-123[abxyz]
```

A room is considered **real** if its checksum matches the five most common letters in the encrypted name, sorted by:

- descending frequency
- alphabetical order for ties

Part 1 filters valid rooms and sums their sector IDs.  
Part 2 decrypts room names and finds a specific target.

---

## 🧩 Part 1

Determine the sum of the sector IDs of all real rooms.

### 💡 Approach

- Parse each line into:
  - `name`
  - `sectorId`
  - `checksum`
- Remove dashes from the name
- Count occurrences of each letter
- Sort letters by:
  - highest frequency first
  - alphabetical order for ties
- Take the first five letters to form a calculated checksum
- Compare with the given checksum
- If valid, add the sector ID to the total

---

## 🧩 Part 2

Find the sector ID of the room whose decrypted name contains:

```
northpole
```

### 💡 Approach

- Reuse only the valid rooms from Part 1
- Decrypt each room name using a Caesar cipher:
  - shift each letter forward by `sectorId`
  - wrap around using modulo 26
- Convert dashes into spaces
- Search for the decrypted name containing `northpole`
- Return the corresponding sector ID

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Security Through Obscurity`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses and validates rooms
- Sums sector IDs of valid entries

For Part 2:

- Decrypts valid room names
- Finds the target room

---

### Parsing Input

Each line is split into three parts:

```
encrypted-name-sectorId[checksum]
```

Parsed into:

- `name` (string with dashes)
- `sectorId` (integer)
- `checksum` (string)

---

### Letter Frequency Counting

To validate a room:

- Remove dashes from the name
- Count each letter's frequency

Example:

```
aaaaabbbzyx
```

Produces:

- `a: 5`
- `b: 3`
- `z: 1`
- `y: 1`
- `x: 1`

---

### Sorting for Checksum

Letters are sorted by:

1. descending frequency
2. alphabetical order for ties

Example result:

```
a b x y z
```

The first five letters form the calculated checksum.

---

### Part 1 Logic

- Iterate through all rooms
- Generate checksum from letter frequencies
- Compare with provided checksum
- If equal, add sector ID to total

---

### Decryption Logic

Each letter is shifted forward:

```
newChar = ((char - 'a' + sectorId) % 26) + 'a'
```

- Dashes (`-`) become spaces
- Letters wrap around from `z` to `a`

---

### Part 2 Logic

- Decrypt each valid room name
- Check if it contains:

```
northpole
```

- Return the sector ID of that room

---

## 🛠 Implementation Notes

- Regex or string parsing simplifies extraction
- Frequency counting can use dictionaries or grouping
- Sorting must handle tie-breaking correctly
- Modulo arithmetic is key for decryption
- Part 2 builds directly on Part 1 validation

---

## 🧪 Behaviour Summary

Given a list of encrypted rooms:

- Part 1 filters out decoy rooms using checksum validation
- Valid rooms contribute their sector IDs to a total
- Part 2 decrypts valid room names
- The correct room is identified by matching a keyword

---

## 🚀 Key Takeaways

- Frequency analysis combined with custom sorting
- Clean separation of parsing, validation, and transformation
- Caesar cipher implementation with modular arithmetic
- Part 2 builds naturally on Part 1 results

---

## 🔗 References

- https://adventofcode.com/2016/day/4