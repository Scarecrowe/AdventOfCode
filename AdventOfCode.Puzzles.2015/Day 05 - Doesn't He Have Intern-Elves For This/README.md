# 🎄 Advent of Code 2015 - Day 05: Doesn't He Have Intern-Elves For This?

## 📜 Puzzle Overview

Santa needs help figuring out which strings in his text file are naughty or nice.

For Part 1, a string is considered nice if it:

- Contains at least **three vowels**
- Contains at least **one letter that appears twice in a row**
- Does **not** contain the strings `ab`, `cd`, `pq`, or `xy`

For Part 2, the rules change completely. A string is nice if it:

- Contains a pair of any two letters that appears at least twice **without overlapping**
- Contains at least one letter which repeats with exactly **one letter between them**

These are the rules defined by the Advent of Code puzzle for Day 5.

---

## 🧩 Part 1

Count how many strings are **nice** using the original rule set.

### 💡 Approach

- Scan each string character by character
- Count vowels
- Check for a repeated letter
- Stop early if a disallowed pair is found
- Count the string as nice only if all required conditions are met

---

## 🧩 Part 2

Count how many strings are **nice** using the updated rule set.

### 💡 Approach

- Track pairs of adjacent characters
- Detect whether any pair appears at least twice without overlapping
- Check for a repeating character with one character in between
- Count the string as nice only if both conditions are satisfied

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new DoesntHeHaveInternElvesForThis(this.Input, ChristmasListModel.Normal)`
- Returns the `Nice` count

For Part 2:

- Creates `new DoesntHeHaveInternElvesForThis(this.Input, ChristmasListModel.Advanced)`
- Returns the `Nice` count

### `ChristmasListModel.cs`

This enum is used to switch between the two rule sets:

- `Normal`
- `Advanced`

### `DoesntHeHaveInternElvesForThis.cs`

This class processes every input line and counts how many strings are:

- `Nice`
- `Naughty`

The constructor loops through each string and applies either:

- `IsNiceNormal(line)`
- `IsNiceAdvanced(line)`

depending on the selected `ChristmasListModel`.

### Normal Rule Check

The normal rule check uses:

- A vowel counter
- A `repeat` flag to detect double letters
- A `disallowed` flag for banned pairs
- A `last` character tracker to compare adjacent characters

As the string is scanned:

1. Vowels are counted using a `HashSet` of valid vowel characters
2. Adjacent repeated letters are detected by comparing the current character to the previous one
3. Disallowed pairs are checked using a `HashSet` containing `ab`, `cd`, `pq`, and `xy`

The method returns true only when:

- No disallowed pair is found
- At least 3 vowels are present
- At least one repeated adjacent character exists

### Advanced Rule Check

The advanced rule check uses:

- A dictionary of character pairs and their first valid positions
- A `repeat` flag for patterns like `xyx`

As the string is scanned:

1. Each two-character pair is recorded
2. A matching pair only counts if it appears again without overlapping
3. A repeating letter with one character between is detected using:

```csharp
line[i - 1] == line[i + 1]

## 🔗 References

- https://adventofcode.com/2015/day/5