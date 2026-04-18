# 🎄 Advent of Code 2022 - Day 3: Rucksack Reorganization

## 📜 Puzzle Overview

This puzzle works with a list of rucksack contents, where each line is a string of item types.

Each item is represented by a single character:

- `a` to `z`
- `A` to `Z`

Each character has a priority value:

- lowercase letters map to `1` through `26`
- uppercase letters map to `27` through `52`

Part 1 looks for the first item type that appears in both halves of the same rucksack.

Part 2 groups input lines in sets of three and finds the shared badge item that appears in all three rucksacks.

---

## 🧩 Part 1

Determine the sum of priorities for the duplicated item in each rucksack.

### 💡 Approach

- Read each input line
- Split the line into two equal halves
- Scan characters from the first half
- Find the first character that also appears in the second half
- Convert that character into its priority value
- Add it to the running total

---

## 🧩 Part 2

Determine the sum of badge priorities for each group of three rucksacks.

### 💡 Approach

- Process the input in groups of three lines
- Look at the unique characters from the first rucksack in the group
- Find the first character that also appears in:
  - the second rucksack
  - the third rucksack
- Convert that character into its priority value
- Add it to the running total

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Rucksack Reorganization`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `RucksackReorganization.Sum(this.Input)`

For Part 2:

- Calls `RucksackReorganization.GroupSum(this.Input)`

---

### `RucksackReorganization.cs`

This class contains the full solution logic.

It provides two static methods:

- `Sum(string[] input)`
- `GroupSum(string[] input)`

So the puzzle is solved entirely through static helper methods rather than through instance state.

---

### Part 1 Duplicate Search

`Sum(...)` loops through each input line.

For each line it takes:

- the first half with:

    line[..(line.Length / 2)]

- the second half with:

    line[(line.Length / 2)..]

Then it checks each character from the first half in order.

As soon as a character is found in the second half, it:

- adds that character value to `sum`
- converts the ASCII value into the puzzle priority
- stops scanning that line

So only the first matching duplicated character per line is counted.

---

### Priority Conversion

The code converts a matching character into its priority by doing:

    sum += current;
    sum -= char.IsLower(current) ? 96 : 38;

This means:

- lowercase letters use ASCII offset `96`
- uppercase letters use ASCII offset `38`

So the results become:

- `a` = 1
- `z` = 26
- `A` = 27
- `Z` = 52

---

### Part 2 Grouped Badge Search

`GroupSum(...)` processes input in steps of three:

    for (int i = 0; i < input.Length; i += 3)

For each group it examines the first line using:

    input[i].GroupBy(x => x)

This groups repeated characters together so the method checks each unique character from the first rucksack only once.

For each grouped character it tests whether that character appears in both:

- `input[i + 1]`
- `input[i + 2]`

As soon as a shared character is found, it:

- adds that character value to `sum`
- applies the same lowercase or uppercase priority offset
- stops scanning that group

So only the first valid shared badge character in each three-line group is counted.

---

### Character Matching

Both puzzle parts use the same style of membership test:

- `IndexOf(current) > -1`

That means the implementation checks whether a given character exists anywhere in the other string.

It does not build sets or intersections explicitly.
Instead, it performs direct string lookups while scanning candidate characters.

---

## 🛠 Implementation Notes

- The solution uses static methods only
- Part 1 scans the first half of each line against the second half
- Part 2 scans groups of three lines at a time
- Part 2 uses `GroupBy(x => x)` so duplicate candidates from the first line are not checked repeatedly
- Priority calculation is done with ASCII arithmetic
- Matching stops as soon as the first valid character is found
- Both parts use `IndexOf(...)` for character membership checks

---

## 🧪 Behaviour Summary

Given a list of rucksack strings:

- Part 1 splits each line into two compartments
- it finds the first item type that appears in both halves
- it converts that item to its priority and adds it to the total

For the gold solution:

- the input is processed in groups of three lines
- the first shared character across all three rucksacks is found
- that badge character is converted to its priority
- all group badge priorities are summed

---

## 🚀 Key Takeaways

- Nice compact solution using direct string scanning
- Part 1 relies on half-string comparison
- Part 2 relies on three-line grouping
- ASCII arithmetic is used to map characters to puzzle priorities
- `GroupBy` helps avoid repeated checks for duplicate characters in the first group line
- The implementation stops at the first valid match in both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2022/day/3