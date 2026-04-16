# 🎄 Advent of Code 2015 - Day 08: Matchsticks

## 📜 Puzzle Overview

Santa needs to work out how much space is required to store a list of string literals.

The puzzle focuses on the difference between:

- The number of characters in the **code representation**
- The number of characters in the **in-memory string value**

Each line contains a quoted string literal and may include escaped characters such as:

- `\\`
- `\"`
- `\x27`

Part 1 asks for the total number of code characters minus the total number of in-memory characters.

Part 2 asks for the total number of characters in the newly escaped representation minus the number of characters in the original code.

---

## 🧩 Part 1

Determine the difference between:

- Total characters of string code
- Total characters in memory after unescaping the string values

### 💡 Approach

- Count every character in the raw input
- Rebuild each string without its wrapping quotes
- Preserve escape sequences while scanning
- Unescape the processed string
- Compare the raw code length to the in-memory length

---

## 🧩 Part 2

Re-encode each original string so it can be written as a new escaped string literal.

Determine the difference between:

- Total characters in the escaped version
- Total characters in the original code

### 💡 Approach

- Walk through the original input character by character
- Build a second escaped representation as the input is read
- Escape every quote and backslash
- Add wrapping quotes for the newly encoded version
- Compare the escaped length to the original code length

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `Matchsticks.CountCharacters(this.Input[0], MatchstickMode.Normal)`

For Part 2:

- Calls `Matchsticks.CountCharacters(this.Input[0], MatchstickMode.Escaped)`

The result is returned as a string in both cases.

---

### `MatchstickMode.cs`

This enum switches between the two puzzle behaviours:

- `Normal`
- `Escaped`

This keeps both parts routed through the same counting method while changing only the final calculation mode.

---

### `Matchsticks.cs`

This class contains the full string-processing logic.

The `CountCharacters()` method tracks three totals:

- `characterCount` - the raw number of characters in the input
- `memoryCount` - the total length after unescaping the strings
- `escapedCount` - the total length of the newly escaped strings

It also uses two `StringBuilder` instances:

- `normal` - builds the content used for in-memory unescaping
- `escaped` - builds the re-encoded version used in Part 2

---

### Input Scanning

The method scans the input one character at a time.

As each character is read:

- Newline characters are skipped
- Carriage returns are used to detect the end of a line
- Every non-carriage-return character contributes to `characterCount`

This allows the entire input blob to be processed in a single pass rather than splitting the file into separate lines first.

---

### Building the In-Memory Representation

The `normal` builder is used to prepare each string for unescaping.

While scanning:

- Wrapping quotes are ignored
- Escaped sequences are preserved
- Characters are appended to `normal` only when they represent the actual content of the string literal

A `last` character tracker is used to detect when a backslash has already been seen so the next character can be treated as part of the escape sequence.

When the end of a line is reached, the code uses:

```csharp
Regex.Unescape(normal.ToString()).Length