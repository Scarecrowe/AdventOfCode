# 🎄 Advent of Code 2015 - Day 16: Aunt Sue

## 📜 Puzzle Overview

Aunt Sue sent a gift, but there are far too many possible Aunt Sues to know which one it came from.

A machine called the MFCSAM detected a list of compounds from the gift wrapping:

- children: `3`
- cats: `7`
- samoyeds: `2`
- pomeranians: `3`
- akitas: `0`
- vizslas: `0`
- goldfish: `5`
- trees: `3`
- cars: `2`
- perfumes: `1`

Each Aunt Sue in the input only lists a few known compounds and amounts.

Part 1 asks for the Sue whose known compounds exactly match the detected values.

Part 2 changes the matching rules for some compounds:

- `cats` and `trees` must be greater than the detected value
- `pomeranians` and `goldfish` must be less than the detected value
- everything else must still match exactly

---

## 🧩 Part 1

Determine which Aunt Sue is the correct match using exact comparisons for every known compound.

### 💡 Approach

- Parse each Aunt Sue into a collection of known compounds
- Compare only the compounds that are present for that Sue
- Ignore any compounds that are missing from that Sue's record
- Return the first Sue whose known values all match exactly

---

## 🧩 Part 2

Determine which Aunt Sue matches when the updated comparison rules are applied.

### 💡 Approach

- Reuse the same parsed input
- Compare known compounds using conditional rules:
  - `cats` and `trees` must be higher
  - `pomeranians` and `goldfish` must be lower
  - all other compounds must match exactly
- Return the first Sue that satisfies those rules

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new AuntSue(this.Input)`
- Calls `FindExactSue()`

For Part 2:

- Creates `new AuntSue(this.Input)`
- Calls `FindSue()`

Both results are returned as strings.

---

### `AuntSue.cs`

This class contains the full parsing and matching logic.

It stores:

- `DetectedCompounds` - the fixed MFCSAM readings
- `AuntSues` - all parsed Aunt Sue records

The constructor immediately parses the input and stores each Sue as a dictionary of compound names to numeric values.

---

### Detected Compounds

The MFCSAM readings are stored as a fixed dictionary containing the known target values.

This includes:

- `children`
- `cats`
- `samoyeds`
- `pomeranians`
- `akitas`
- `vizslas`
- `goldfish`
- `trees`
- `cars`
- `perfumes`

This dictionary acts as the reference data for both matching methods.

---

### Parsing the Input

The `Parse()` method builds the full Aunt Sue list.

It:

- creates a numbered dictionary of Sues
- assigns each input line to an incrementing Sue number
- parses each line using `ParseSue()`

Each Sue is stored as:

- key = Sue number
- value = dictionary of compounds and amounts

---

### Parsing a Single Sue

The `ParseSue()` method reads one input line and extracts the compound values.

It:

- removes the leading `Sue X:` portion
- splits the remaining text by commas
- splits each compound entry by `: `
- converts the numeric value into an integer

A line like this:

    Sue 1: cars: 9, akitas: 3, goldfish: 0

becomes a dictionary containing:

- `cars = 9`
- `akitas = 3`
- `goldfish = 0`

---

### Exact Matching

`FindExactSue()` is used for Part 1.

For each Sue:

- loop through every detected compound
- if that Sue contains the compound, compare it directly
- if the values do not match exactly, reject that Sue
- if the Sue does not contain the compound, skip it

The first Sue whose known values all match is returned.

If no match is found, the method returns `0`.

---

### Range-Based Matching

`FindSue()` is used for Part 2.

It follows the same overall structure as Part 1, but changes the comparison logic for four compounds.

Special rules:

- `cats` and `trees` must be greater than the detected value
- `pomeranians` and `goldfish` must be less than the detected value

All other compounds must still match exactly.

As soon as one rule fails, that Sue is rejected and the search moves on.

The first matching Sue number is returned.

If no match is found, the method returns `0`.

---

## 🛠 Implementation Notes

- Input is parsed once during construction
- Each Sue is represented as a dictionary of known compounds
- Missing compounds are ignored rather than treated as zero
- Part 1 and Part 2 reuse the same parsed data
- The only difference between the two parts is the comparison logic
- The search stops as soon as a valid match is found

---

## 🧪 Examples

Given a detected value of:

- `children: 3`

Then:

- a Sue with `children: 3` matches Part 1
- a Sue with no `children` entry is still possible because the value is unknown
- a Sue with `children: 2` is rejected

For Part 2:

- `cats: 8` can match because `cats` must be greater than `7`
- `goldfish: 4` can match because `goldfish` must be less than `5`
- `trees: 3` does not match because `trees` must be greater than `3`

---

## 🚀 Key Takeaways

- Good example of solving two puzzle parts with shared parsed data
- Dictionary-based storage keeps compound lookups simple
- Part 2 is handled cleanly by changing only the matching rules
- Early rejection keeps the search easy to follow and efficient

---

## 🔗 References

- https://adventofcode.com/2015/day/16