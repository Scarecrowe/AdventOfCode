# 🎄 Advent of Code 2015 - Day 04: The Ideal Stocking Stuffer

## 📜 Puzzle Overview

Santa needs help mining AdventCoins using MD5 hashes.

The puzzle provides a secret key, and you must find the lowest positive number that, when appended to that key, produces an MD5 hash starting with a required number of leading zeroes.

For example:

- `abcdef609043` produces a hash starting with five zeroes
- `pqrstuv1048970` also produces a hash starting with five zeroes

---

## 🧩 Part 1

Find the lowest positive number that, when appended to the secret key, produces an MD5 hash that starts with **five zeroes**.

### 💡 Approach

- Start at `0`
- Append the current number to the secret key
- Generate the MD5 hash
- Check how many zeroes appear at the start of the hash
- Stop when the hash starts with the required number of leading zeroes

---

## 🧩 Part 2

Repeat the process, but this time the hash must start with **six zeroes**.

### 💡 Approach

- Reuse the same brute-force search
- Increase the required number of leading zeroes from `5` to `6`
- Continue until a valid hash is found

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TheIdealStockingStuffer(this.Input[0], 5)`
- Returns the resulting `Number`

For Part 2:

- Creates `new TheIdealStockingStuffer(this.Input[0], 6)`
- Returns the resulting `Number`

---

### `TheIdealStockingStuffer.cs`

This class performs the AdventCoin search.

The constructor takes:

- `secret`
- `leadingZeros`

A loop runs until a valid value is found:

1. Concatenate `secret` and the current number
2. Generate the MD5 hash
3. Count how many `0` characters appear from the start of the hash
4. Stop when that count matches the required number of leading zeroes
5. Otherwise increment the number and continue

The result is exposed via the `Number` property once a match is found.

---

### MD5 Check

- MD5 hashes are generated using `ToMd5()`
- Leading zeroes are counted using:

```csharp
TakeWhile(c => c == '0').Count()

## 🔗 References

- https://adventofcode.com/2015/day/4