# 🎄 Advent of Code 2020 - Day 02: Password Philosophy

## 📜 Puzzle Overview

This puzzle validates passwords against two different policy rules.

Each input line contains:

- a numeric policy range
- a required letter
- the password itself

A line looks like this:

    1-3 a: abcde

The solver parses each line into:

- a minimum value
- a maximum value
- a required letter
- a password string

Part 1 checks how many times the required letter appears.  
Part 2 treats the two numbers as character positions and checks whether the required letter appears in exactly one of those positions.

---

## 🧩 Part 1

Determine how many passwords are valid under the occurrence-count policy.

### 💡 Approach

- Read each input line
- Split the line into:
  - policy range
  - required letter
  - password
- Parse the range into minimum and maximum values
- Count how many times the required letter appears in the password
- If the count is between the minimum and maximum inclusive:
  - mark the password as valid
- Return the total number of valid passwords

---

## 🧩 Part 2

Determine how many passwords are valid under the position-based policy.

### 💡 Approach

- Reuse the same parsed line structure
- Treat the two numbers as 1-based character positions
- Check the required letter at both positions
- A password is valid only when:
  - the first position matches and the second does not
  - or the second position matches and the first does not
- Return the total number of valid passwords

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Password Philosophy`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- calls `PasswordPhilosophy.Simple(this.Input)`

For Part 2:

- calls `PasswordPhilosophy.Advanced(this.Input)`

---

### `PasswordPhilosophy.cs`

This class contains both validation methods.

It exposes:

- `Simple(string[] input)`
- `Advanced(string[] input)`

Both methods:

- iterate through every input line
- split the line into tokens
- parse the policy values
- extract the policy letter
- read the password text
- increment a running valid-password count when the rule passes

---

### Input Parsing

Each line is split into three main tokens:

- the range, such as `1-3`
- the letter section, such as `a:`
- the password, such as `abcde`

At a high level the implementation does:

- split on spaces
- split the first token on `-`
- convert both values to integers
- remove `:` from the letter token
- trim the password string

So the input:

    1-3 a: abcde

becomes:

- `min = 1`
- `max = 3`
- `letter = 'a'`
- `password = "abcde"`

---

### Part 1 Validation

`Simple(string[] input)` applies the original policy.

For each password it:

- counts how many characters match the required letter
- checks whether that count is between `min` and `max`

Logically, the rule is:

    count >= min && count <= max

If that condition is true, the password is counted as valid.

So the silver answer is the total number of passwords whose required letter appears within the allowed inclusive range.

---

### Part 2 Validation

`Advanced(string[] input)` applies the second policy.

For each password it:

- checks whether the required letter matches the character at `min - 1`
- checks whether the required letter matches the character at `max - 1`

The implementation stores those results in:

- `minMatch`
- `maxMatch`

Then it validates using exclusive-or style logic:

    (minMatch && !maxMatch) || (!minMatch && maxMatch)

So the password is valid only if exactly one of the two positions contains the required letter.

---

## 🛠 Implementation Notes

- Both parts parse the input line in the same way
- The first number and second number are reused differently between parts
- Part 1 uses them as inclusive minimum and maximum occurrence counts
- Part 2 uses them as 1-based positions in the password
- The required letter is extracted by removing the trailing colon
- The solution counts valid passwords with a simple running integer total

---

## 🧪 Behaviour Summary

Given a list of password-policy lines:

- the solver parses each line into policy values, letter, and password
- Part 1 counts matching letters and applies a min/max rule
- Part 2 checks exactly one of two indexed positions
- both methods return the total number of valid passwords

So the final result is either:

- the number of passwords valid by occurrence count
- or the number of passwords valid by position matching

---

## 🚀 Key Takeaways

- Good example of reusing the same parsed input for two different rule sets
- Part 1 is a straightforward frequency check
- Part 2 switches to a positional exclusive-or rule
- The implementation keeps both solutions compact and easy to follow
- The puzzle highlights how the same data can be interpreted in two different ways

---

## 🔗 References

- https://adventofcode.com/2020/day/2