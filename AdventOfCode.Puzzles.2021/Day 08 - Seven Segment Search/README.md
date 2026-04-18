# 🎄 Advent of Code 2021 - Day 08: Seven Segment Search

## 📜 Puzzle Overview

This puzzle works with scrambled seven-segment display signals.

Each input line contains two parts separated by ` | `:

- ten scrambled signal patterns
- four scrambled output values

A line looks like this:

    be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe

The solver parses each line into an `Entry` object containing:

- `SignalPatterns`
- `OutputValues`

Part 1 counts how often output digits with unique segment counts appear.  
Part 2 fully decodes each display configuration and sums the resulting output numbers.

---

## 🧩 Part 1

Count how many output digits are clearly identifiable by segment count alone.

### 💡 Approach

- Parse every input line into an entry
- Look only at the four output values for each entry
- Count outputs whose length matches a uniquely identifiable digit:
  - `2` segments = digit `1`
  - `3` segments = digit `7`
  - `4` segments = digit `4`
  - `7` segments = digit `8`
- Return the total count across all entries

---

## 🧩 Part 2

Decode every scrambled display and sum all four-digit output values.

### 💡 Approach

- Parse each line into signal patterns and output values
- Deduce which scrambled wire maps to each of the seven display positions
- Build the segment pattern for digits `0` through `9`
- Match each output value against the decoded patterns
- Convert the four decoded digits into a number
- Sum those numbers across all entries

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Seven Segment Search`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new SevenSegmentDisplay(this.Input)`
- calls `UniqueOutputValues()`

For Part 2:

- creates `new SevenSegmentDisplay(this.Input)`
- calls `DisplayValue()`

---

### `Entry.cs`

This class models one line of puzzle input.

It stores:

- `SignalPatterns`
- `OutputValues`

The constructor:

- splits the input on ` | `
- splits the left side into signal patterns
- splits the right side into output values
- stores both collections as lists of strings

So one puzzle line becomes one parsed entry containing the ten scrambled training patterns and the four values to decode.

---

### `SevenSegmentDisplay.cs`

This class contains the full parsing and solving logic.

It stores:

- `Entries`
- `Configuration`

The constructor:

- converts every input line into `new Entry(x)`
- stores the parsed results in `Entries`
- initialises `Configuration`

The main solving methods are:

- `UniqueOutputValues()`
- `DisplayValue()`

---

### Parsing the Display Data

The constructor performs the full parse step:

- read each input line
- create a new `Entry`
- collect all entries into a list

At a high level it does:

    this.Entries = input.Select(x => new Entry(x)).ToList();

This means all later logic works from structured entry objects instead of raw strings.

---

### Part 1 Counting Logic

`UniqueOutputValues()` counts output values whose segment length is unique.

It uses this list of known unique lengths:

    2, 4, 3, 7

These correspond to the digits:

- `1`
- `4`
- `7`
- `8`

For each entry:

- inspect every string in `OutputValues`
- if its length is in that set, increment the total

So the silver answer is simply the number of output digits that can be recognised without fully decoding the display.

---

### Decoding a Scrambled Display

`CalculateDisplay(List<string> signalInput)` works out which scrambled character maps to each of the seven segment positions.

It builds:

- `List<char> display = new() { 'z', 'z', 'z', 'z', 'z', 'z', 'z' };`

This `display` list represents the seven segment slots of a normal seven-segment display.

The method then deduces segment wiring step by step.

---

### Finding the Top Segment

The solver starts by finding the patterns for digits `1` and `7`:

- digit `1` is the only pattern with length `2`
- digit `7` is the only pattern with length `3`

It removes the characters of `1` from `7` to find the extra segment, which must be the top segment.

Logically:

    top = seven - one

That character is stored in:

    display[0]

---

### Resolving the Right-Side Segments

The two characters in digit `1` must map to the upper-right and lower-right segments.

The solver initially assigns them as:

- `display[2]`
- `display[5]`

It then checks which patterns do not contain the candidate upper-right segment.

Using the length-5 and length-6 patterns that exclude that character, it can detect when the two assignments need to be swapped.

So the implementation uses pattern elimination to decide which of the two wires is really upper-right and which is lower-right.

---

### Finding the Bottom-Left Segment

Once the solver has identified likely patterns corresponding to `5` and `6`, it removes the already-known `1` and `7` segments from both.

The difference between those reduced patterns identifies the bottom-left segment.

That character is stored in:

    display[4]

---

### Counting Remaining Character Frequency

The solver then strips already-known characters from a temporary copy of the signal patterns and counts how often the remaining characters occur.

It builds:

- `Dictionary<char, int> counts = new();`

Then it finds the least frequent remaining character and assigns it to:

    display[1]

This is used as the upper-left segment.

---

### Finding the Middle and Bottom Segments

The pattern for digit `4` is the only signal with length `4`.

After removing already-known characters from that pattern, the remaining character becomes the middle segment:

    display[3]

Then one final remaining counted character becomes the bottom segment:

    display[6]

At that point all seven segment positions have been deduced.

---

### Building the Digits

`DisplayValue()` calls `CalculateDisplay(entry.SignalPatterns)` for each entry.

Once the seven segment positions are known, it constructs the canonical scrambled pattern for every digit from `0` to `9`.

For example:

- `0` uses segments `0,1,2,4,5,6`
- `1` uses segments `2,5`
- `2` uses segments `0,2,3,4,6`
- `3` uses segments `0,2,3,5,6`
- and so on

Each constructed pattern is sorted so it can be compared reliably against sorted input strings.

---

### Mapping Patterns to Numbers

The solver creates:

- `List<int> numbers = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };`

It then walks through the ten signal patterns for the entry.

For each pattern:

- sort its characters
- compare it to the generated patterns for digits `0` through `9`
- store the matching digit at the same index in `numbers`

So each original signal pattern position gets associated with its decoded numeric value.

---

### Decoding the Output Value

To decode the four output values:

- sort each output string
- find the matching sorted signal pattern
- use that index to retrieve the already-decoded digit from `numbers`

Those digits are appended into a final string such as:

    8394

That string is then converted to an integer and added to the running total.

So the gold answer is the sum of every decoded four-digit display value.

---

### `RemoveChars(...)`

This helper method removes all characters from one string that appear in another string.

At a high level it does:

- iterate each character in the first string
- keep only characters not found in the second string
- return the filtered result

This helper is used repeatedly during segment deduction.

---

## 🛠 Implementation Notes

- Each input line is parsed into an `Entry`
- Part 1 relies only on unique output string lengths
- Part 2 deduces segment wiring for each entry independently
- Segment positions are stored in a 7-character list
- Decoded digit patterns are sorted before comparison
- Output digits are resolved by matching output strings back to decoded signal patterns
- The final Part 2 total is accumulated as a `long`

---

## 🧪 Behaviour Summary

Given a scrambled seven-segment input line:

- the solver splits the line into signal patterns and output values
- Part 1 counts outputs with unique lengths
- Part 2 deduces the actual segment layout
- builds the pattern for digits `0` to `9`
- matches the four output values to decoded digits
- converts those digits into a full number
- adds that number to the running total

The final result is either:

- the number of uniquely identifiable output digits
- or the sum of all decoded output values

---

## 🚀 Key Takeaways

- Good example of solving a wiring puzzle through elimination
- `Entry` cleanly separates parsed signal patterns and output values
- Part 1 is a simple length-based count
- Part 2 reconstructs the full display configuration for each line
- Sorting strings makes pattern comparison reliable
- The implementation decodes outputs by linking them back to the original signal pattern positions

---

## 🔗 References

- https://adventofcode.com/2021/day/8