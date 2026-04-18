# 🎄 Advent of Code 2022 - Day 04: Camp Cleanup

## 📜 Puzzle Overview

This puzzle works with pairs of section assignment ranges.

Each input line contains two ranges separated by a comma, such as:

    2-4,6-8

Each range describes a continuous block of section IDs.

The solver converts each range into an explicit list of section numbers and then turns that list into a single joined string representation.

It uses those generated strings to test:

- whether one assignment fully contains the other
- whether the two assignments overlap at all

---

## 🧩 Part 1

Count how many assignment pairs have one range fully containing the other.

### 💡 Approach

- Split each input line into its two range tokens
- Parse each token into:
  - start section
  - end section
- Expand every range into all of its section IDs
- Convert each expanded range into one joined string
- Compare the two strings for each pair
- If the longer string contains the shorter string:
  - count it as a full containment

So the silver answer is the number of pairs where one fully expanded assignment appears inside the other.

---

## 🧩 Part 2

Count how many assignment pairs overlap at all.

### 💡 Approach

- Reuse the same range expansion logic
- Convert each range into a joined string of bracketed section IDs
- For each pair:
  - split the first expanded string back into individual section markers
  - check whether any one of those markers exists in the second string
- As soon as one shared section is found:
  - count that pair
  - stop checking that pair further

So the gold answer is the number of pairs that share at least one section ID.

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Camp Cleanup`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `CampCleanup.Contains(this.Input)`

For Part 2:

- Calls `CampCleanup.Overlap(this.Input)`

---

### `CampCleanup.cs`

This class contains the full puzzle logic.

It exposes two public methods:

- `Contains(string[] input)`
- `Overlap(string[] input)`

Both methods follow the same first stage:

- parse each line
- split the two ranges
- expand each range into all section IDs
- store each expanded assignment as a joined string

So the main difference between silver and gold is how those generated strings are compared afterwards.

---

### Parsing Each Assignment Range

For each input line, the solver does:

- split on `","`
- process each token separately
- split the token on `"-"`
- convert both numbers to integers

So a token such as:

    2-4

becomes:

- start = `2`
- end = `4`

---

### Expanding a Range

After parsing the numeric bounds, the solver builds every section ID in that interval.

At a high level it does:

- loop from start to end inclusive
- wrap each number as a string like:
  
    [2]

- collect those strings into a list
- join the full list into one combined string

So:

    2-4

becomes logically:

    [2][3][4]

This exact string-based representation is what the later containment and overlap checks operate on.

---

### Part 1 Containment Logic

`Contains(string[] input)` stores every expanded assignment string in order.

That means each input pair produces two consecutive entries in the results list.

It then processes those results two at a time.

For each pair:

- compare the string lengths
- treat the longer string as the possible container
- check whether the longer string contains the shorter string using `IndexOf(...)`

If it does:

- increment the count

So full containment is detected by checking whether one fully expanded assignment string appears inside the other.

---

### Why String Length Works Here

Because each section ID is stored in a consistent bracketed format:

    [n]

a larger range usually produces a longer joined string.

The implementation uses that length difference to decide which side should be checked as the container.

Then it uses substring matching to confirm whether the smaller expanded range appears fully within the larger one.

---

### Part 2 Overlap Logic

`Overlap(string[] input)` begins exactly the same way:

- split lines into two tokens
- expand each token into bracketed section strings
- store the two results for each pair

After that, it processes the results in pairs.

For each pair:

- split the first string on `"]"` to recover its section markers
- iterate over those pieces
- append `"]"` back when testing
- check whether that exact marker appears in the second string

As soon as one match is found:

- increment the overlap count
- stop checking that pair

So gold only needs one shared section ID to count the pair.

---

### Example of the Overlap Check

If the first expanded assignment is:

    [2][3][4]

and the second is:

    [4][5][6]

the solver checks markers from the first against the second.

When it reaches:

    [4]

it finds that exact section marker in the second string and counts the pair as overlapping.

---

## 🛠 Implementation Notes

- Both puzzle parts fully expand numeric ranges before comparison
- Expanded ranges are stored as joined bracketed strings
- Part 1 uses substring containment to detect full inclusion
- Part 2 checks whether any single section marker appears in both assignments
- The same parsing and expansion logic is duplicated in both methods
- Range bounds are inclusive on both ends
- Results are processed in pairs because each input line produces exactly two expanded assignments

---

## 🧪 Behaviour Summary

Given a list of paired section ranges:

- the solver splits each line into two assignments
- each assignment is expanded into all section IDs it covers
- those IDs are converted into a bracketed joined string
- Part 1 counts pairs where one full expanded assignment contains the other
- Part 2 counts pairs where at least one expanded section appears in both
- the final answer is either the containment count or the overlap count

---

## 🚀 Key Takeaways

- Straightforward example of solving interval problems by explicit expansion
- The implementation chooses string comparison instead of direct numeric interval math
- Part 1 and Part 2 share the same expansion stage
- Full containment is checked with substring matching
- Overlap is checked by searching for any shared section marker
- The approach is simple and easy to follow, even if it is not the most compact possible representation

---

## 🔗 References

- https://adventofcode.com/2022/day/4