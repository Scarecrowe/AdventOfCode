# 🎄 Advent of Code 2021 - Day 10: Syntax Scoring

## 📜 Puzzle Overview

This puzzle analyses lines of bracket-like syntax and scores them in two different ways.

Each input line is made up of opening and closing characters:

- `(`
- `[`
- `{`
- `<`
- `)`
- `]`
- `}`
- `>`

The solver walks through each line while tracking nested chunks.

There are two possible outcomes:

- a line is **corrupted** if it closes with the wrong character
- a line is **incomplete** if it ends before all opened chunks are closed

Part 1 scores corrupted lines.  
Part 2 scores incomplete lines by calculating the completion sequence that would finish each line correctly.

---

## 🧩 Part 1

Determine the total syntax error score for all corrupted lines.

### 💡 Approach

- Read each line character by character
- Track the currently open nested chunk structure
- When a closing character appears:
  - check whether it is valid in the current position
  - if not, mark the line as corrupted
- Count each illegal closing character by type
- Multiply those counts by their fixed error values
- Return the total combined syntax error score

The error values used are:

- `)` = `3`
- `]` = `57`
- `}` = `1197`
- `>` = `25137`

---

## 🧩 Part 2

Determine the middle autocomplete score for all incomplete lines.

### 💡 Approach

- Reuse the same line parsing logic
- Ignore corrupted lines
- For each incomplete line:
  - find the remaining unclosed opening nodes
  - work backwards toward the root
  - generate the required closing sequence
- Score the completion string using the running multiply-by-5 rule
- Sort all completion scores
- Return the middle value

The autocomplete values used are:

- `)` = `1`
- `]` = `2`
- `}` = `3`
- `>` = `4`

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Syntax Scoring`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new SyntaxScoring(this.Input)`
- calls `ErrorScore()`

For Part 2:

- creates `new SyntaxScoring(this.Input)`
- calls `MiddleScore()`

---

### `Opening.cs`

This file defines the opening token enum.

It contains:

- `Round = '('`
- `Square = '['`
- `Curly = '{'`
- `Tag = '<'`

This gives the parser a named representation for all valid opening characters.

---

### `Closing.cs`

This file defines the closing token enum.

It contains:

- `Round = ')'`
- `Square = ']'`
- `Curly = '}'`
- `Tag = '>'`

These values are also used in the scoring dictionaries for Part 1 and Part 2.

---

### `Node.cs`

This class models one node in the current nested syntax tree.

It stores:

- `Value`
- `Parent`
- `Child`
- `Illegalvalues`

The constructor:

- stores the current character
- links the node to its parent
- calculates which closing characters would be illegal at this point

So instead of using a plain stack, the implementation builds a linked parent-child structure while also tracking invalid closing options.

---

### Illegal Closing Tracking

When a `Node` is created for an opening character, it builds its `Illegalvalues` list.

It does this by:

- finding the matching closing character for that opening token
- taking all other closing characters as illegal
- optionally adding the parent's inverted value when needed

This means the parser can later check a closing character with:

    current.Illegalvalues.Contains(line[j])

If that returns true, the line is corrupted.

---

### `AddChild(...)` and `RemoveChild()`

`AddChild(char value)` creates a new child node and returns it.

At a high level:

- create `new Node(value, this)`
- assign it to `Child`
- return that child

`RemoveChild()` clears the current node's child reference.

This is used when the parser successfully closes the current nested chunk and moves back up to the parent.

---

### `SyntaxScoring.cs`

This class contains the full parsing and scoring logic.

It defines:

- `OpeningChars`
- `ClosingChars`
- `ErrorPoints`
- `MiddlePoints`

It also stores:

- `Incomplete`
- `CorruptedRound`
- `CorruptedSquare`
- `CorruptedCurly`
- `CorruptedTag`

The constructor:

- initialises the incomplete-line list
- calls `BuildIncomplete(input)`

So all corruption counting and incomplete-line capture happens during construction.

---

### Opening and Closing Helpers

The class exposes helper methods:

- `IsOpening(char value)`
- `IsClosing(char value)`
- `Invert(char value)`
- `Not(char value)`

`Invert(...)` swaps between matching opening and closing characters.

For example:

- `(` becomes `)`
- `[` becomes `]`
- `)` becomes `(`
- `>` becomes `<`

This helper is used both during parsing and while generating completion strings.

---

### Part 1 Scoring

`ErrorScore()` returns the total syntax error score by combining the recorded corruption counts.

It calculates:

    (ErrorPoints[')'] * this.CorruptedRound)
    + (ErrorPoints[']'] * this.CorruptedSquare)
    + (ErrorPoints['}'] * this.CorruptedCurly)
    + (ErrorPoints['>'] * this.CorruptedTag)

So the silver answer is based on how many times each wrong closing character was encountered during parsing.

---

### Part 2 Scoring

`MiddleScore()` calculates the autocomplete score for each incomplete line.

For every stored incomplete node:

- start from the deepest remaining node
- walk upward through `Parent`
- invert each opening character into its required closing character
- append those closing characters into a completion string

Then it scores the completion string with:

    total = (total * 5) + MiddlePoints[value]

Each line produces one score.

After all scores are collected:

- sort the totals
- return the middle element

So the gold answer is the median autocomplete score.

---

### Building Corrupted and Incomplete Results

`BuildIncomplete(string[] input)` processes the full input.

It begins by resetting all corruption counters:

- `CorruptedRound`
- `CorruptedSquare`
- `CorruptedCurly`
- `CorruptedTag`

Then for each line it:

- starts with `current = null`
- tracks whether the line becomes corrupt with `isCorrupt`

For each character in the line:

- if there is no current node yet, create the root node
- if the character is a closing character:
  - check whether it appears in `current.Illegalvalues`
  - if yes:
    - mark the line as corrupt
    - increment the matching corruption counter
    - stop processing that line
  - otherwise:
    - move back to `current.Parent`
    - remove the child link from the parent if needed
- if the character is an opening character:
  - descend by calling `current.AddChild(line[j])`

After the line is fully processed:

- if `current != null`
- and the line was not corrupt

then the remaining `current` node is added to:

- `this.Incomplete`

That stored node becomes the starting point for Part 2 completion scoring.

---

### How Corruption Is Detected

The parser does not compare only against one expected closing token.

Instead, it stores a list of illegal closing values for the current node.

So when a closing character arrives, the check is:

- invalid closing for this point in the tree -> corrupted
- otherwise -> close the current node and move upward

This gives the solver a slightly different structure from a traditional stack-based solution, but it still models the same nesting rules.

---

## 🛠 Implementation Notes

- The implementation uses enums for opening and closing character types
- Nested chunks are represented with linked `Node` objects
- Each node stores a parent reference and optional child reference
- Corrupted lines increment one of four dedicated counters
- Incomplete lines store the final unresolved node
- Part 1 returns a weighted corruption total
- Part 2 builds completion strings by walking back up the parent chain
- The final autocomplete answer is the median sorted score

---

## 🧪 Behaviour Summary

Given a list of bracket sequences:

- the solver reads each line character by character
- builds nested nodes for opening characters
- validates closing characters against the current node state
- corrupted lines contribute to the syntax error score
- incomplete lines are preserved for later completion scoring
- Part 1 returns the total error score
- Part 2 returns the middle autocomplete score

---

## 🚀 Key Takeaways

- Good example of solving a nesting problem with linked nodes instead of a plain stack
- `Node` stores both tree structure and illegal closing information
- Part 1 and Part 2 share the same parsing pass
- Corrupted lines are counted immediately during parsing
- Incomplete lines are revisited later to generate closing sequences
- The completion score is built incrementally using the puzzle's multiply-by-5 rule

---

## 🔗 References

- https://adventofcode.com/2021/day/10