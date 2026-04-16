# 🎄 Advent of Code 2015 - Day 10: Elves Look, Elves Say

## 📜 Puzzle Overview

The Elves are playing a look-and-say game using a sequence of digits.

Starting with the puzzle input, each round reads the current sequence and builds the next one by describing each group of repeated digits.

For example:

- `1` becomes `11`
- `11` becomes `21`
- `21` becomes `1211`
- `1211` becomes `111221`

Each new value is created by counting how many times a digit appears in a consecutive run, then appending the count followed by the digit itself.

Part 1 asks for the length of the sequence after **40 rounds**.

Part 2 asks for the length of the sequence after **50 rounds**.

---

## 🧩 Part 1

Determine the length of the result after applying the look-and-say process **40 times**.

### 💡 Approach

- Start with the input value
- Read the sequence from left to right
- Count consecutive repeated digits
- Append the count and digit to a new sequence
- Repeat for 40 rounds
- Return the final length

---

## 🧩 Part 2

Repeat the same process, but this time run it for **50 rounds**.

### 💡 Approach

- Reuse the same transformation logic
- Increase the number of rounds from `40` to `50`
- Return the length of the final sequence

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `ElvesLookElvesSay.Play(this.Input[0], 40)`
- Returns the resulting string length

For Part 2:

- Calls `ElvesLookElvesSay.Play(this.Input[0], 50)`
- Returns the resulting string length

This keeps the puzzle class very small and pushes the sequence logic into a dedicated helper.

---

### `ElvesLookElvesSay.cs`

This class contains the full look-and-say implementation.

The `Play()` method takes:

- `input` - the starting sequence
- `rounds` - how many times to apply the transformation

It stores the current sequence in `value` and uses a `StringBuilder` to construct the next sequence for each round.

---

### Round Processing

For each round:

- Start with a count of `1`
- Track the current digit using `last`
- Scan through the sequence from left to right
- If the next digit matches the current one, increment the count
- If it changes, append the count and digit to the builder, then reset the counter

At the end of the scan, the final run is appended as well.

This transforms the full sequence into the next value in the series.

---

### Sequence Construction

The next sequence is built using:

    sb.Append($"{count}{last}");

Each completed run contributes:

- The number of repeated digits
- Followed by the digit itself

So a run like:

    111

becomes:

    31

because it contains three `1` characters.

---

### Reusing the Builder

After each round:

- The built value becomes the new current sequence
- The `StringBuilder` is cleared
- The next round starts again using the updated value

This avoids creating unnecessary intermediate builders for every pass.

---

### Final Result

Once all rounds are complete, `Play()` returns the final generated sequence.

The puzzle answers use:

- `.Length` after 40 rounds for Part 1
- `.Length` after 50 rounds for Part 2

Only the final length is needed, not the full sequence output.

---

## 🛠 Implementation Notes

- The same logic is reused for both parts
- The only difference between Part 1 and Part 2 is the number of rounds
- `StringBuilder` is used to efficiently construct each next sequence
- Consecutive runs are processed in a single pass per round
- The final answer is based on sequence length rather than numeric value

---

## 🧪 Examples

Starting with:

    1

The sequence progresses as:

    1
    11
    21
    1211
    111221
    312211

This matches the standard look-and-say progression used by the puzzle.

---

## 🚀 Key Takeaways

- Good example of iterative string transformation
- Clean separation between puzzle setup and sequence generation
- Efficient run-length style processing using a single scan per round
- Part 2 is solved by reusing the same logic with a larger round count

---

## 🔗 References

- https://adventofcode.com/2015/day/10