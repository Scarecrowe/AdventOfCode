# 🎄 Advent of Code 2024 - Day 21: Keypad Conundrum

## 📜 Puzzle Overview

This puzzle is about entering door codes through a chain of robots, where each robot controls another keypad.

The implementation models two keypad types:

- a numeric keypad
- a directional keypad

Each code from the input is expanded into button press sequences, then repeatedly expanded again through additional robot layers.

Part 1 uses 2 robot layers. Part 2 uses 25 robot layers.

The final complexity score is calculated by:

- finding the shortest expanded sequence length for each code
- extracting the first 3 digits of the code as its numeric value
- multiplying sequence length by numeric value
- summing the results for all input codes

---

## 🧩 Part 1

Calculate the total complexity using 2 directional robot layers.

### 💡 Approach

- Expand each code on the numeric keypad into possible movement sequences
- For each resulting sequence, recursively measure how long it becomes when entered through 2 layers of directional keypads
- Take the minimum expanded length
- Multiply that length by the numeric part of the code
- Add the result to the running total

---

## 🧩 Part 2

Calculate the total complexity using 25 directional robot layers.

### 💡 Approach

- Reuse the same logic as Part 1
- Increase the recursive expansion depth from 2 to 25
- Use memoization to avoid recomputing repeated button-to-button transitions
- Sum the complexity values across all codes

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Keypad Conundrum`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new KeypadConundrum(this.Input)`
- Calls `Complexity(2)`

For Part 2:

- Creates `new KeypadConundrum(this.Input)`
- Calls `Complexity(25)`

---

### `KeypadConundrum.cs`

This class coordinates the full solution.

It defines two keypad layouts:

#### Numeric keypad

    7 8 9
    4 5 6
    1 2 3
      0 A

#### Directional keypad

      ^ A
    < v >

It also creates:

- `numericExpander`
- `directionalExpander`

These are both instances of `KeypadExpander`, each built from one of the keypad layouts.

---

### Complexity Calculation

`Complexity(int robots)` processes every input code.

For each code it:

- expands the code using the numeric keypad expander
- calculates the expanded length through the directional keypad layers
- finds the minimum possible expanded length
- parses the first 3 characters of the code as an integer
- multiplies the shortest length by that numeric value
- adds that to the total

It finally returns the total as a string.

So the solver is not returning the actual sequence itself, only the total complexity score.

---

### Recursive Expansion Length

The heavy lifting happens in:

    ComputeExpandedSequenceLength(KeypadExpander expander, string sequence, int count)

This method works recursively.

If:

    count == 0

then it simply returns:

    sequence.Length

Otherwise it:

- starts from button `A`
- walks through each next button in the sequence
- looks up all valid expansions for that button-to-button transition
- recursively computes the cost of each possible expansion with one fewer robot layer
- keeps the minimum length
- adds those minimum lengths together

This lets the solver compute the shortest total sequence length without materialising every full expansion at deep recursion levels.

---

### Memoization

The solver uses two caches.

#### `SequenceLengthCache`

This cache stores computed lengths for:

- expander
- previous button
- next button
- recursion depth

That means repeated transitions such as going from one button to another at the same depth are only solved once.

#### `SequenceScoreCache`

This cache stores the score for a sequence used when filtering candidate paths.

---

### `KeypadExpander.cs`

This class precomputes valid movement expansions between every pair of buttons on a keypad.

It stores:

- `Expansions`
- `GeneratedSequenceCache`

The constructor receives a keypad layout and immediately calls:

    BuildExpander()

---

### Building the Expansion Map

`BuildExpander()` loops through:

- every valid start button
- every valid destination button

For each pair it:

- calculates horizontal distance
- calculates vertical distance
- determines movement directions such as `<`, `>`, `^`, `v`
- generates all possible orderings of those moves
- removes invalid paths that cross keypad gaps
- removes non-optimal paths
- stores the remaining sequences in `Expansions`

Each stored sequence ends with:

    A

to represent pressing the destination button.

---

### Generating Movement Sequences

`GenerateAllSequences(...)` recursively builds all possible combinations of:

- horizontal moves
- vertical moves

For example, if a move requires both horizontal and vertical travel, this method generates every possible ordering of those moves.

If no movement is needed, it returns:

    A

This method also uses `GeneratedSequenceCache` so repeated distance patterns are reused.

---

### Removing Invalid Sequences

Not every move ordering is legal because some keypads contain gaps.

`RemoveInvalidSequences(...)` filters sequences by checking them with:

    IsValidSequence(...)

A sequence is invalid if it:

- moves off the keypad
- steps onto a blank space

This is especially important for the irregular keypad layouts used in this puzzle.

---

### Removing Non-Optimal Sequences

After generating all valid move orders, the solver trims the set with:

    RemoveNonoptimalSequences(...)

This compares sequence scores using:

    KeypadConundrum.ComputeSequenceScore(sequence)

The score increases whenever consecutive characters differ, starting from `A`.

So a sequence with fewer direction changes is preferred.

The method keeps only sequences whose score matches the minimum accepted score.

---

### `ComputeSequenceScore(...)`

This method evaluates how often a sequence changes direction or button.

It works by:

- starting with previous character `A`
- scanning each character in the sequence
- incrementing the score whenever the current character differs from the previous one

This favours movement strings with longer runs of the same direction.

It also memoizes scores in `SequenceScoreCache`.

---

### `Keypad.cs`

This class represents a keypad layout and provides utility helpers.

It contains:

- the keypad `layout`
- `IsValidPosition(...)`
- `GetButton(...)`
- `FindButton(...)`
- `GenerateSequences(...)`

`FindButton(...)` scans the keypad to locate a specific button.

`IsValidPosition(...)` checks:

- row bounds
- column bounds
- that the position is not a blank space

---

### Sequence Generation in `Keypad.cs`

`GenerateSequences(...)` performs an iterative breadth-first search from a start position to a target position.

It uses a queue containing:

- row
- column
- sequence so far

For each state it:

- checks whether the target has been reached
- if so, returns the sequence plus `A`
- otherwise tries moving:
  - up
  - down
  - left
  - right

Only valid keypad positions are enqueued.

This provides a general way to generate keypad movement sequences, although the main Day 21 solution relies on the precomputed expansion logic in `KeypadExpander`.

---

## 🛠 Implementation Notes

- Part 1 calls `Complexity(2)`
- Part 2 calls `Complexity(25)`
- The numeric keypad and directional keypad are both hard-coded as 2D char arrays
- Button transitions are expanded using precomputed lookup tables
- Recursive length calculation avoids building gigantic full strings for deeper robot chains
- Memoization is essential for Part 2 performance
- Sequence filtering prefers paths with fewer direction changes
- The code extracts the numeric value with `code.Substring(0, 3)`

---

## 🧪 Behaviour Summary

Given a list of keypad codes:

- the solver expands each code into possible numeric keypad movement sequences
- each sequence is then evaluated through repeated directional keypad expansions
- recursive memoized length calculation finds the shortest possible button press length
- that minimum length is multiplied by the numeric part of the code
- all code complexities are summed into the final answer

Part 1 uses 2 robot layers.  
Part 2 uses 25 robot layers.

---

## 🚀 Key Takeaways

- Good example of separating keypad layout, expansion logic, and scoring logic
- Uses precomputed button-to-button expansions for efficiency
- Avoids brute-force full expansion at high recursion depths
- Memoization makes deep recursive evaluation practical
- Prefers smoother movement patterns by scoring direction changes
- Reuses the same solver for both puzzle parts by changing only recursion depth

---

## 🔗 References

- https://adventofcode.com/2024/day/21