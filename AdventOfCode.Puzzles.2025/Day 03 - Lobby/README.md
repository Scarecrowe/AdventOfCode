# 🎄 Advent of Code 2025 - Day 03: Lobby

## 📜 Puzzle Overview

This puzzle works through a list of battery strings and builds a numeric result from each one.

Each input line is treated as a sequence of characters. For a given target length, the solver removes enough characters to leave only the strongest possible selection while preserving the original order of the remaining characters.

Part 1 uses a target length of `2`.

Part 2 uses a target length of `12`.

The final answer is the sum of all selected values after converting each picked character sequence into a number.

---

## 🧩 Part 1

Determine the total value produced by reducing every battery string down to `2` characters.

### 💡 Approach

- Read each battery string one line at a time
- Work left to right through the characters
- Remove weaker earlier characters when a larger later character appears
- Keep only the best possible `2` characters in order
- Convert the selected characters into a number
- Sum the result across all input lines

---

## 🧩 Part 2

Repeat the same process, but reduce every battery string down to `12` characters instead.

### 💡 Approach

- Reuse the same selection logic
- Increase the required remaining length from `2` to `12`
- Convert each chosen character sequence into a number
- Sum all values

---

## 🧠 Code Breakdown

### `Day3.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Lobby`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Lobby(this.Input)`
- Calls `Joltage(2)`

For Part 2:

- Creates `new Lobby(this.Input)`
- Calls `Joltage(12)`

The only difference between the two parts is the number of characters that should remain in each battery string.

---

### `Lobby.cs`

This class contains the full selection and summing logic.

It stores:

- `Batteries` as the raw input lines

The main method is:

- `Joltage(int count)`

This method loops through every input line and reduces it to the requested number of characters before adding the parsed numeric value to the final result.

---

### Calculating How Many Characters to Remove

For each line, the code first calculates:

    toRemove = line.Length - count

This is the number of characters that must be discarded to leave exactly the requested final length.

That value controls how aggressively the selection logic can remove earlier characters while scanning the line.

---

### Stack-Based Selection

The method uses:

    Stack batteries = new(count);

to build the selected character sequence.

Each character is processed from left to right.

While scanning:

- if the stack is not empty
- and there are still removals available
- and the top of the stack is smaller than the current character

then the top value is popped from the stack and counted as removed.

At a high level, the logic behaves like this:

- keep a working stack of chosen characters
- when a larger character appears later, remove smaller earlier characters if there is still room to do so
- push the current character afterward

This is a greedy approach that prefers larger digits as early as possible while preserving relative order.

---

### Trimming the Final Stack

After the line has been processed, there is one more safeguard:

    while (batteries.Count > count)
        batteries.Pop();

This ensures the stack ends at the exact required size, even if not all removals were used during the main scan.

---

### Rebuilding the Selected Value

Once the chosen characters are in the stack:

- they are copied into an array
- the array is reversed
- the characters are joined into a string
- the string is parsed as a `long`

At a high level:

- stack order is converted back into original reading order
- the selected characters become the final numeric value for that line

That value is then added to the running total.

---

### Final Result

`Joltage(int count)` returns:

- the sum of all parsed numbers created from each reduced battery string

This means the puzzle answer is not a count of matches, but a total built from all selected values across the full input.

---

## 🛠 Implementation Notes

- Input is processed one line at a time
- The same method is reused for both puzzle parts
- A stack is used to support greedy removal of weaker earlier characters
- Character order is preserved among the remaining chosen values
- The only difference between Part 1 and Part 2 is the target output length
- Each reduced character sequence is parsed as a number and added to the final total

---

## 🧪 Behaviour Summary

Given a battery string and a target length:

- characters are read from left to right
- smaller earlier characters can be removed when a larger later character appears
- only a fixed number of characters remain
- the chosen characters stay in their original relative order
- the final character sequence is interpreted as a number

This makes the puzzle behave like a greedy maximum-number selection problem over each input line.

---

## 🚀 Key Takeaways

- Good example of using a stack for greedy fixed-length selection
- Both parts are solved by the same method with a different target size
- Earlier smaller digits are discarded when stronger later digits can improve the final number
- The implementation stays compact by combining selection and summing in one pass over each line

---

## 🔗 References

- https://adventofcode.com/2025/day/3