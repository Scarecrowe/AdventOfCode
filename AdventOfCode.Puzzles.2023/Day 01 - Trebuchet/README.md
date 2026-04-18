# 🎄 Advent of Code 2023 - Day 01: Trebuchet?!

## 📜 Puzzle Overview

This puzzle works line by line through a calibration document.

Each line contains a mixture of characters, but hidden inside are calibration digits.

For every line, the solver extracts:

- the first digit-like value
- the last digit-like value

Those two values are combined into a two-digit number, and all such numbers are summed.

Part 1 only uses numeric characters already present in the text.

Part 2 expands the rules so spelled-out numbers such as `one`, `two`, and `nine` are also recognised.

---

## 🧩 Part 1

Sum the calibration values using only numeric digits found in each line.

### 💡 Approach

- Read each input line
- Scan the string for numeric characters
- Record each digit using its character position in the line
- Sort all found digits by position
- Take:
  - the first digit
  - the last digit
- Combine them into a two-digit number
- Sum all parsed values

If a line only contains one digit, that digit is used twice.

---

## 🧩 Part 2

Sum the calibration values again, but also recognise number words.

### 💡 Approach

- Reuse the same digit scan from Part 1
- Also search the line for the words:
  - `one`
  - `two`
  - `three`
  - `four`
  - `five`
  - `six`
  - `seven`
  - `eight`
  - `nine`
- Record each matched word using its starting index in the string
- Combine numeric digits and word-digits into one position-based map
- Sort by position
- Use the first and last values to form the calibration number
- Sum the results for all lines

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Trebuchet?!`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new Trebuchet(this.Input, false)`
- calls `Sum()`

For Part 2:

- creates `new Trebuchet(this.Input)`
- calls `Sum()`

That means the difference between silver and gold is controlled by the `includeWords` constructor parameter.

---

### `Trebuchet.cs`

This class contains the parsing and summing logic.

It stores:

- `IncludeWords`
- `Values`
- `Map`

The constructor:

- stores whether word parsing is enabled
- creates the internal lists and dictionary
- parses every input line immediately
- stores each parsed calibration value in `Values`

The final answer is returned by:

- `Sum() => this.Values.Sum()`

---

### Parsing a Single Line

`Parse(string input)` handles one line of the calibration document.

It does the following:

- clears the shared `Map`
- reads numeric digits with `GetDigits(input)`
- optionally reads number words if `IncludeWords` is `true`
- sorts the collected values by their string position
- takes the first and last entries
- combines them into one two-digit integer

The return logic is:

- first digit = `this.Map.ElementAt(0).Value`
- last digit = `this.Map.Last().Value`

If there is only one entry in the map, the first digit is reused as the second digit.

---

### `GetDigits(string input)`

This method scans the input one character at a time.

For each position `i`:

- if `input[i]` is a digit
- add it to `Map` using:
  - key = index in the string
  - value = parsed integer digit

So a line like:

    a1b2c3

would record values at the positions of `1`, `2`, and `3`.

This lets the solver preserve the true left-to-right ordering of all discovered values.

---

### `GetWord(string input, string word, int value)`

This method searches for a spelled-out number inside the input.

If the word exists:

- find its first index using `IndexOf`
- add that index and numeric value to `Map`
- continue searching from the next position
- repeat until no more matches are found

Because the next search starts at `i + 1`, overlapping words can still be found correctly.

That behaviour is important for cases such as:

    twone

where one match can begin before the previous word has fully ended.

---

### Positional Mapping

The core of the implementation is:

- `Dictionary<int, int> Map`

This dictionary stores:

- key = character index in the original string
- value = resolved numeric value

Both numeric digits and number words are inserted into the same map.

After parsing, the dictionary is reordered with:

    this.Map = this.Map.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

This guarantees that the first and last entries correspond to the earliest and latest recognised values in the line.

---

### Word Parsing Toggle

The constructor is defined as:

    public Trebuchet(string[] input, bool includeWords = true)

So:

- Part 1 passes `false`
- Part 2 relies on the default `true`

This is how the same parsing pipeline supports both puzzle parts without needing separate classes or methods.

---

## 🛠 Implementation Notes

- The puzzle title is `Trebuchet?!`
- Part 1 disables word parsing with `false`
- Part 2 enables word parsing through the default constructor argument
- Parsed line values are stored in `List<int> Values`
- The running result is produced by summing `Values`
- The parser uses a shared `Dictionary<int, int>` keyed by character position
- Number words are searched individually with repeated `IndexOf`
- A single discovered value becomes a doubled digit such as `7 -> 77`
- The recognised number words are `one` through `nine` only; `zero` is not included in the implementation.

---

## 🧪 Behaviour Summary

Given a list of calibration strings:

- the solver parses each line independently
- numeric digits are always collected
- number words are optionally collected
- all discovered values are ordered by their position in the line
- the first and last values form a two-digit number
- those numbers are summed across the whole input

So the final result is either:

- the sum using only literal digits
- or the sum using both digits and spelled-out numbers

---

## 🚀 Key Takeaways

- The same solver supports both parts through a simple constructor flag
- Position-based parsing keeps digit ordering accurate
- Numeric characters and number words are merged into one unified map
- Overlapping word matches are handled by continuing searches from the next character
- The implementation is compact but still cleanly separates digit scanning and word scanning
- Reusing the first digit when only one value exists keeps the parsing logic simple

---

## 🔗 References

- https://adventofcode.com/2023/day/1