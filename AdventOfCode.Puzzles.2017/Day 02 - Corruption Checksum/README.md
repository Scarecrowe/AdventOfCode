# 🎄 Advent of Code 2017 - Day 01: Inverse Captcha

## 📜 Puzzle Overview

This puzzle processes a sequence of digits to produce a checksum.

You are given a circular list of digits, meaning:

- the digit after the last digit is the first digit

The task is to compare digits based on specific rules and sum matching values.

Part 1 compares adjacent digits.  
Part 2 compares digits halfway around the list.

---

## 🧩 Part 1

Determine the sum of all digits that match the next digit in the list.

### 💡 Approach

- Iterate through each digit
- Compare it with the next digit:
  - wrap around at the end
- If they match:
  - add the digit to the total

Example:

```
1122 → 3
1111 → 4
1234 → 0
91212129 → 9
```

---

## 🧩 Part 2

Determine the sum of all digits that match the digit halfway around the list.

### 💡 Approach

- Calculate offset:

```
offset = length / 2
```

- For each digit:
  - compare it with the digit at `index + offset`
  - wrap around using modulo
- If they match:
  - add the digit to the total

Example:

```
1212 → 6
1221 → 0
123425 → 4
123123 → 12
12131415 → 4
```

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Inverse Captcha`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Iterates through digits
- Compares adjacent values

For Part 2:

- Uses offset comparison
- Applies circular indexing

---

### Input Handling

- Input is a single string of digits
- Convert into a list or array of integers

---

### Circular Comparison

To handle wrap-around:

```
nextIndex = (currentIndex + step) % length
```

This works for both parts:

- Part 1 → step = 1
- Part 2 → step = length / 2

---

### Part 1 Logic

- Loop through all digits
- Compare with next digit
- Accumulate matching values

---

### Part 2 Logic

- Loop through all digits
- Compare with digit halfway around
- Accumulate matching values

---

## 🛠 Implementation Notes

- Modulo arithmetic simplifies circular logic
- Input size is small, so performance is trivial
- Same loop structure works for both parts
- Only the comparison offset changes

---

## 🧪 Behaviour Summary

Given a circular sequence of digits:

- Each digit is compared with another digit
- Matching digits contribute to a sum
- Part 1 compares adjacent digits
- Part 2 compares halfway-around digits

---

## 🚀 Key Takeaways

- Circular array processing
- Modulo arithmetic for wrapping
- Same algorithm reused with different offsets
- Clean and simple iteration problem

---

## 🔗 References

- https://adventofcode.com/2017/day/1