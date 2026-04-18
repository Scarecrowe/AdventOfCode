# 🎄 Advent of Code 2020 - Day 19: Monster Messages

## 📜 Puzzle Overview

This puzzle works with a small grammar system and a list of messages.

The input is split into two sections:

- numbered rules
- messages to test

Rules can be:

- a direct character match such as `"a"`
- a sequence of other rule numbers
- multiple alternative possibilities separated by `|`

The solver parses the rules into a structured dictionary, then recursively checks which messages match rule `0`.

---

## 🧩 Part 1

Count how many messages completely match rule `0`.

### 💡 Approach

- Parse the rules section into a dictionary keyed by rule number
- Parse the remaining lines as messages
- For each message:
  - recursively attempt to match rule `0` from index `0`
  - collect all possible match end positions
- Count the message only when a match consumes the full message length

---

## 🧩 Part 2

Count how many messages match rule `0` after modifying rules `8` and `11`.

### 💡 Approach

- Mutate the input so rule `8` becomes recursive
- Mutate the input so rule `11` becomes recursive
- Reparse the rules and messages
- Reuse the same recursive matcher from Part 1
- Count only full-length matches

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monster Messages`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
new MonsterMessages(this.Input).Simple()
```

For Part 2:

```text
new MonsterMessages(this.Input).Advanced()
``` 

---

### `MonsterMessages.cs`

This class contains the full parsing and matching logic.

It stores:

- `Input`

It exposes two main methods:

- `Simple()`
- `Advanced()`

Both methods:

- parse rules and messages
- test every message against rule `0`
- count only matches that consume the full message length

---

### Rule Representation

The rules are parsed into a dictionary keyed by rule number.

At a high level the structure is:

- rule number
- list of possible alternatives
- each alternative is a list of sub-rules

Each sub-rule is stored as:

- `RuleType`
- `string value`

So a rule can represent either:

- a character literal
- a reference to another rule number

---

### Parsing Rules and Messages

`ParseRulesAndMessages()` processes the raw input in two phases.

It uses a flag:

```text
rulesParsed
```

to switch mode once it reaches the blank line separating rules from messages.

Before the blank line:

- split each rule at `:`
- split the right side on `|`
- parse each possibility into sub-rules
- store everything in the rule dictionary

After the blank line:

- treat each remaining line as a message
- add it to the message list

---

### Parsing Individual Sub-Rules

`ParseSubRule(string subRule)` determines whether a token is:

- a character rule such as `"a"`
- or a nested rule reference such as `42`

If the token contains quotes, it becomes a character rule.

Otherwise it becomes a rule reference.

So the parser turns the raw grammar text into typed rule components that the matcher can evaluate recursively.

---

### Part 1 Matching Flow

`Simple()` does this:

- parse rules and messages
- loop over every message
- call:

```text
Match(message, ref rules, rules[0], 0)
```

- inspect the returned list of matched indexes
- count the message if at least one match exists and the first matched index equals the full message length

So a message only counts if rule `0` matches from the start all the way to the end.

---

### Recursive Matching

`Match(...)` is the core of the solver.

It returns:

- a `List<int>` of possible end indexes after matching a rule

This is important because one rule can have:

- multiple alternatives
- nested recursive expansions
- multiple valid match lengths

For each alternative in the rule:

- start with the current input index
- process each sub-rule in sequence
- carry forward every possible index produced so far
- merge all successful end indexes into the final result list

That makes it a recursive backtracking matcher over the rule graph.

---

### Matching Rule References vs Characters

Inside `Match(...)`, each sub-rule is handled in one of two ways.

If the sub-rule is another rule reference:

- recursively call `Match(...)` on that referenced rule
- add all returned indexes into the current candidate list

If the sub-rule is a character:

- compare the current message character with the expected literal
- if it matches, advance the index by `1`

This lets the same matcher handle both terminal and non-terminal grammar elements.

---

### Part 2 Rule Changes

`Advanced()` modifies the raw input before reparsing it.

It replaces:

```text
8: 42
```

with:

```text
8: 42 | 42 8
```

and replaces:

```text
11: 42 31
```

with:

```text
11: 42 31 | 42 11 31
```

In the implementation these are written directly into:

- `this.Input[9]`
- `this.Input[10]`

Then the same parse-and-match flow runs again.

---

## 🛠 Implementation Notes

- The solver parses rules into nested lists of typed sub-rules
- Messages are processed only after the blank separator
- Matching returns all possible end positions, not just a single boolean
- Full message validity is checked by comparing a returned match index to `message.Length`
- Part 2 reuses the exact same matching engine after rewriting two input lines to make the grammar recursive

---

## 🧪 Behaviour Summary

Given a set of numbered grammar rules and a list of messages:

- parse rules into alternatives and sub-rules
- parse messages into a list of candidate strings
- recursively test each message against rule `0`
- keep track of every possible match end position
- count only messages fully consumed by a valid match
- for Part 2, rewrite rules `8` and `11` to be recursive and run the same matcher again

---

## 🚀 Key Takeaways

- Nice example of representing a grammar as nested rule alternatives
- Recursive descent style matching is handled by returning possible end indexes
- The same matcher supports both normal and recursive rules
- Part 2 is solved by changing the grammar, not by rewriting the algorithm
- The parser and matcher are compact but flexible enough for branching rule sets

---

## 🔗 References

- https://adventofcode.com/2020/day/19