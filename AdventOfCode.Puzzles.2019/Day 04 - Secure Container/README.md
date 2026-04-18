# 🎄 Advent of Code 2019 - Day 04: Secure Container

## 📜 Puzzle Overview

This puzzle checks a range of six-digit numbers to find how many passwords meet a set of rules.

The input is a single range in this format:

    240298-784956

The solver parses that line into a lower and upper bound, then tests every number in the range.

Part 1 looks for numbers whose digits never decrease and which contain at least one repeated digit.

Part 2 uses the same non-decreasing rule, but the repeated digit requirement is stricter: there must be at least one digit that appears exactly twice.

---

## 🧩 Part 1

Count how many numbers in the given range are valid passwords under the basic rules.

### 💡 Approach

- Parse the input range into a start and end number
- Check every number in that inclusive range
- Convert the number to a string
- Keep only numbers whose digits are already in sorted order
- From those, keep only numbers containing at least one digit that appears two or more times
- Return the total count of matching passwords

---

## 🧩 Part 2

Count how many numbers in the range are valid passwords under the stricter repeated-digit rule.

### 💡 Approach

- Reuse the same parsed input range
- Check every number in that inclusive range
- Keep only numbers whose digits never decrease
- From those, keep only numbers containing at least one digit that appears exactly twice
- Return the total count of matching passwords

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Secure Container`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `SecureContainer.Simple(this.Input)`

For Part 2:

- Calls `SecureContainer.Advanced(this.Input)`

---

### `SecureContainer.cs`

This class contains the password validation logic.

It provides:

- `Simple(string[] input)` for Part 1
- `Advanced(string[] input)` for Part 2
- `Range(string[] input)` to parse the numeric bounds

Both puzzle parts follow the same overall structure:

- parse the range
- iterate through every value
- keep only numbers whose digits never decrease
- apply the duplicate-digit rule for the relevant part
- return the count of valid matches

---

### Parsing the Range

`Range(string[] input)` reads the first input line and splits it on `"-"`.

At a high level it does:

- read `input[0]`
- split the text into the lower and upper bounds
- convert both values to integers
- store them in a `Vector`

So this input:

    240298-784956

becomes a numeric range used by both puzzle parts.

---

### Non-Decreasing Digit Check

Before testing repeated digits, the solver first checks whether a number's digits are already in ascending order.

It does this with logic equivalent to:

    $"{i}" == $"{i}".OrderBy(c => c).Join()

If the sorted version of the number matches the original string, then the digits never decrease from left to right.

Examples that pass this rule:

    111123
    135679

Examples that fail this rule:

    223450
    987654

Only numbers that pass this first filter move on to the duplicate check.

---

### Part 1 Duplicate Rule

For the silver solution, the solver checks each digit in the number and counts how many times that digit appears.

It uses logic equivalent to:

    count = number.Split(digit).Length - 1

If any digit appears at least twice, the number is accepted.

So values like these would qualify for the duplicate rule:

    111111
    122345
    123455

This means Part 1 accepts larger groups as well as simple pairs.

---

### Part 2 Exact-Pair Rule

For the gold solution, the solver uses the same counting approach, but changes the requirement.

Instead of:

    count >= 2

it uses:

    count == 2

That means the password must contain at least one digit appearing exactly twice.

Examples that satisfy this stricter rule:

    112233
    111122

Examples that do not satisfy it:

    123444

So Part 2 rejects numbers where the only repeated digits are part of larger groups.

---

### Filtering Flow

Both `Simple()` and `Advanced()` use a two-stage filtering process.

First stage:

- iterate through every number in the range
- keep only numbers with non-decreasing digits
- store them in `first`

Second stage:

- inspect each remaining number digit by digit
- count digit occurrences
- add valid numbers to `second`
- return `second.Count`

The only difference between Part 1 and Part 2 is the duplicate condition:

- Part 1 uses `count >= 2`
- Part 2 uses `count == 2`

---

## 🛠 Implementation Notes

- The input is parsed from a single range string
- Both parts iterate through the full inclusive range
- Digits are checked as strings rather than with arithmetic
- Non-decreasing order is verified by sorting the digits and comparing to the original
- Duplicate detection is done by counting occurrences of each digit in the string
- Part 1 accepts any repeated digit group of size two or greater
- Part 2 requires at least one group of exactly two matching digits

---

## 🧪 Behaviour Summary

Given one password range:

- the solver parses the lower and upper bounds
- checks every number in the range
- filters out any number whose digits decrease
- Part 1 keeps numbers with at least one repeated digit
- Part 2 keeps numbers with at least one exact pair
- each part returns the total number of valid passwords found

---

## 🚀 Key Takeaways

- Nice example of solving a number puzzle with string-based digit inspection
- The sorted-string comparison makes the non-decreasing rule very compact
- Both puzzle parts share the same structure with only one rule difference
- Part 2 becomes stricter by requiring an exact pair rather than any repeated group
- The implementation is simple and easy to follow because it filters in stages

---

## 🔗 References

- https://adventofcode.com/2019/day/4