# 🎄 Advent of Code 2016 - Day 14: One-Time Pad

## 📜 Puzzle Overview

This puzzle generates keys using repeated hashing and pattern detection.

You are given a salt (input), and must compute MD5 hashes of:

```
salt + index
```

Where `index` starts at `0` and increases.

A hash contributes to a key if it contains specific repeating character patterns:

- a **triplet** (three of the same character in a row)
- followed by a **quintuplet** (five of the same character) within the next 1000 hashes

Part 1 uses standard hashing.  
Part 2 introduces hash stretching for increased complexity.

---

## 🧩 Part 1

Determine the index that produces the 64th key.

### 💡 Approach

- Iterate over increasing indices
- For each index:
  - compute MD5 hash of `salt + index`
  - search for the first occurrence of a triplet (e.g. `aaa`)
- If a triplet is found:
  - look ahead at the next 1000 hashes
  - check if any contain a matching quintuplet (e.g. `aaaaa`)
- If a match is found:
  - count this index as a valid key
- Continue until 64 keys are found
- Return the index of the 64th key

---

## 🧩 Part 2

Repeat the process using **stretched hashes**.

### 💡 Approach

- Instead of a single MD5:
  - hash the result **2017 times total**
- For each index:
  - compute initial MD5
  - repeatedly hash the hex output 2016 additional times
- Apply the same triplet/quintuplet logic as Part 1
- Continue until 64 keys are found

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `One-Time Pad`
- Loads the puzzle input (salt)
- Calls the silver and gold solutions

For Part 1:

- Generates hashes
- Detects keys based on pattern rules

For Part 2:

- Applies hash stretching
- Reuses the same key detection logic

---

### Hash Generation

Each index produces:

```
MD5(salt + index)
```

For Part 2:

```
hash = MD5(salt + index)
repeat 2016 times:
    hash = MD5(hash)
```

---

### Detecting Triplets

Scan the hash string for:

```
xxx
```

Where all three characters are the same.

- Only the first triplet in the hash is considered

---

### Detecting Quintuplets

Search for:

```
xxxxx
```

- Must match the same character as the triplet
- Must occur within the next 1000 hashes

---

### Key Validation

For a given index:

- find first triplet character `c`
- check next 1000 hashes for `ccccc`
- if found:
  - index is a valid key

---

### Part 1 Logic

- Iterate indices
- Detect triplets
- Validate using future hashes
- Count keys until reaching 64
- Return index of final key

---

### Part 2 Logic

- Apply hash stretching before pattern checks
- Use same validation process
- Significantly increases computation time

---

### Performance Considerations

- Cache computed hashes to avoid recomputation
- Store hashes in a lookup structure
- Precompute or lazily compute future hashes
- Efficient string scanning improves performance

---

## 🛠 Implementation Notes

- Hash caching is critical for performance
- String scanning for patterns should be efficient
- Part 2 dramatically increases workload
- Avoid recalculating hashes unnecessarily
- Early exit once 64 keys are found

---

## 🧪 Behaviour Summary

Given a salt:

- Generate hashes incrementally
- Detect repeating character patterns
- Validate keys using future hashes
- Part 1 uses standard hashing
- Part 2 uses stretched hashing
- Output is the index of the 64th valid key

---

## 🚀 Key Takeaways

- Pattern detection within hash outputs
- Sliding window lookahead (1000 hashes)
- Heavy reliance on caching for performance
- Same logic with more expensive hashing in Part 2

---

## 🔗 References

- https://adventofcode.com/2016/day/14