# 🎄 Advent of Code 2017 - Day 04: High-Entropy Passphrases

## 📜 Puzzle Overview

This puzzle validates passphrases based on specific security rules.

Each input line is a passphrase consisting of words separated by spaces.

The goal is to determine how many passphrases are valid.

Part 1 checks for duplicate words.  
Part 2 extends this to detect anagrams.

---

## 🧩 Part 1

Determine how many passphrases contain no duplicate words.

### 💡 Approach

- Split each passphrase into words
- Check if any word appears more than once
- A passphrase is valid if all words are unique

Example:

```
aa bb cc dd ee → valid
aa bb cc dd aa → invalid
aa bb cc dd aaa → valid
```

---

## 🧩 Part 2

Determine how many passphrases contain no anagram pairs.

### 💡 Approach

- Split each passphrase into words
- For each word:
  - sort its characters alphabetically
- Compare transformed words
- A passphrase is valid if no two words become identical after sorting

Example:

```
abcde fghij → valid
abcde xyz ecdab → invalid
a ab abc abd abf abj → valid
iiii oiii ooii oooi oooo → valid
oiii ioii iioi iiio → invalid
```

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `High-Entropy Passphrases`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Checks for duplicate words

For Part 2:

- Checks for anagrams using transformed words

---

### Input Parsing

- Each line is a passphrase
- Words are separated by spaces
- Convert into a list of strings

---

### Part 1 Logic

- Convert words into a set
- Compare:
  - original count vs unique count
- If equal:
  - passphrase is valid

---

### Part 2 Logic

- Transform each word:

```
word → sorted characters
```

- Example:

```
"bca" → "abc"
```

- Apply same uniqueness check as Part 1

---

### Efficient Checking

- Use a set or hash structure
- Insert words (or transformed words)
- Detect duplicates during insertion

---

## 🛠 Implementation Notes

- String splitting is straightforward
- Sorting characters enables easy anagram detection
- Set comparison is efficient for uniqueness checks
- Same structure reused for both parts

---

## 🧪 Behaviour Summary

Given a list of passphrases:

- Each passphrase is validated independently
- Part 1 rejects duplicate words
- Part 2 rejects anagram pairs
- Valid passphrases are counted

---

## 🚀 Key Takeaways

- Set-based uniqueness checking
- String normalisation for anagram detection
- Same validation pattern applied with different rules
- Simple logic with clean reuse between parts

---

## 🔗 References

- https://adventofcode.com/2017/day/4