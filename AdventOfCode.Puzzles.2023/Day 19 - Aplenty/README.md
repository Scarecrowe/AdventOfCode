# 🎄 Advent of Code 2023 - Day 19: Aplenty

## 📜 Puzzle Overview

This puzzle is about routing machine parts through a set of named workflows.

Each part has four ratings:

- `x`
- `m`
- `a`
- `s`

Each workflow contains ordered rules that test one of those values and decide where the part goes next.

A rule can:

- send the part to another workflow
- accept it with `A`
- reject it with `R`

The first matching rule always wins, and the part never returns to the remaining rules in that workflow.

The input has two sections:

- workflow definitions
- part ratings

The solver parses both sections in the `Aplenty` constructor.

---

## 🧩 Part 1

Determine the total of all rating values for every accepted part.

### 💡 Approach

- Parse all workflows into a dictionary keyed by workflow name
- Parse each part into a tuple of:
  - `X`
  - `M`
  - `A`
  - `S`
- Start each part in workflow:
  - `in`
- Evaluate workflow rules in order until the part reaches:
  - `A`
  - or `R`
- If accepted:
  - add `x + m + a + s` to the running total

---

## 🧩 Part 2

Determine how many distinct rating combinations would be accepted.

### 💡 Approach

Instead of checking every possible part individually, the solver works with ranges.

It starts with the full valid space:

- `x = 1..4000`
- `m = 1..4000`
- `a = 1..4000`
- `s = 1..4000`

It then recursively walks the workflows and splits those ranges whenever a rule partially matches.

For each branch:

- send the matching slice to the rule destination
- keep the remaining slice to continue through later rules

If a range reaches:

- `A` → count all combinations in that range
- `R` → discard it

This turns a huge brute-force search into recursive range partitioning.

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Aplenty`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Aplenty(this.Input)`
- Calls `TotalRatings()`

For Part 2:

- Creates `new Aplenty(this.Input)`
- Calls `CombinationRatings()`

---

### `Aplenty.cs`

This class contains all parsing and solving logic.

It stores:

- `Workflows`
- `Ratings`
- `AcceptedRanges`

`Workflows` is a dictionary keyed by workflow name.

Each workflow stores a list of rules shaped like:

- input field
- comparison symbol
- comparison value
- destination

So each rule is effectively stored as:

    (char In, char Symbol, int Value, string Out)

There is also a special fallback rule stored with:

    ('z', 'z', 0, destination)

which represents the unconditional final rule in a workflow.

---

### Parsing the Input

The constructor reads the input line by line.

It uses an empty line to switch between:

- workflow parsing
- ratings parsing

For workflows:

- split on `{`
- use the left side as the workflow name
- split the rule list on commas
- parse conditional rules like:
  - `x<100:a1`
  - `m>500:R`
- store unconditional fallthrough rules separately

For ratings:

- strip `{` and `}`
- split on commas
- parse into tuples of:
  - `X`
  - `M`
  - `A`
  - `S`

This builds the full workflow graph plus the list of concrete parts for Part 1.

---

### Part 1 Workflow Evaluation

`TotalRatings()` processes each rating tuple one at a time.

For every part:

- set `current = "in"`
- loop until `current` becomes:
  - `A`
  - or `R`
- fetch the current workflow from `Workflows`
- evaluate each rule in order
- as soon as one matches:
  - update `current`
  - break to the next workflow step

If the final result is `A`, the solver adds:

    rating.X + rating.M + rating.A + rating.S

to the result total.

---

### Rule Matching Logic

The implementation checks each field explicitly.

For example:

- if `rule.In == 'x'`
- compare `rating.X`
- honour either:
  - `<`
  - or `>`

The same pattern is repeated for:

- `m`
- `a`
- `s`

If the rule is the fallback rule:

- `rule.In == 'z'`

then the part immediately moves to that destination.

This mirrors the puzzle behaviour where the first satisfied rule wins and the final rule acts as the default branch.

---

### `Sort((int X, int M, int A, int S) rating)`

This method performs the same workflow traversal as Part 1, but returns only the final outcome:

- `A`
- or `R`

It starts from:

    "in"

and follows the workflow chain until termination.

So this acts like a reusable single-part classifier built from the same rule engine.

---

### Range-Based Counting for Part 2

The gold solution is built around these methods:

- `RangeSize(int[] range)`
- `BatchSize(Dictionary<char, int[]> range)`
- `Accepted(Dictionary<char, int[]> range, string workflow)`

The ranges are stored as half-open intervals:

    [start, end)

So:

    new int[] { 1, 4001 }

represents values from `1` through `4000`.

---

### Counting the Size of a Range

`RangeSize(int[] range)` returns:

    range[1] - range[0]

`BatchSize(...)` multiplies the sizes of the four dimensions:

- `x`
- `m`
- `a`
- `s`

So the total number of parts represented by a range batch is:

    size(x) * size(m) * size(a) * size(s)

That lets the solver count huge accepted regions instantly instead of enumerating individual parts.

---

### Recursive Acceptance Logic

`Accepted(range, workflow)` is the core of Part 2.

It handles three cases immediately:

- if workflow is `R` → return `0`
- if workflow is `A` → compute the batch size and return it
- otherwise → inspect the current workflow rules

When a rule only partially matches the current range, the solver splits the range into:

- a matching portion
- a remaining portion

It then:

- recursively sends the matching portion to the rule destination
- keeps the leftover portion in the current workflow for later rules

This is how the implementation explores all valid accepted combinations without duplication.

---

### Splitting on `<` Rules

For a rule like:

    x < value

the solver checks the current range for `x`.

There are three outcomes:

- the whole range matches
- only part of the range matches
- none of it matches

When the match is partial, it creates a copied range where:

- the matched branch becomes:
  
    [current[0], value)

- the remaining branch becomes:
  
    [value, current[1])

The matched branch is sent recursively to the rule destination, while the remaining branch continues through the current workflow.

---

### Splitting on `>` Rules

For a rule like:

    x > value

the same idea is used, but the matching range becomes:

    [value + 1, current[1])

and the remainder becomes:

    [current[0], value + 1)

So `>` rules also divide the search space cleanly into accepted-path and continue-processing portions.

---

### Workflow Fallthrough in Part 2

After all conditional rules are processed, any leftover range is sent to the final unconditional destination using:

    this.Workflows[workflow][^1].Out

So the recursive range solver preserves the same rule-order semantics as Part 1:

- first matching branch goes immediately
- leftovers continue
- the final default rule catches whatever remains


---

### Part 2 Entry Point

`CombinationRatings()` creates the full starting search space as:

- `x: [1, 4001)`
- `m: [1, 4001)`
- `a: [1, 4001)`
- `s: [1, 4001)`

It then calls:

    this.Accepted(range, "in")

So the gold answer is the total number of accepted combinations reachable from the initial workflow over the full 4D rating space.

---

## 🛠 Implementation Notes

- Workflows are stored in a dictionary by name
- Rules are represented as tuples instead of dedicated classes
- The fallback workflow rule is encoded with sentinel values:
  - `'z'`
  - `'z'`
  - `0`
- Part 1 evaluates concrete rating tuples directly
- Part 2 works on 4D ranges instead of individual parts
- Ranges are half-open intervals
- Accepted range sizes are multiplied across all four rating dimensions
- `AcceptedRanges` also stores each accepted batch and its size during the gold calculation

---

## 🧪 Behaviour Summary

Given a workflow system and a set of part ratings:

- the solver parses named workflows and ordered rules
- Part 1 sends each concrete part through the workflow graph
- accepted parts contribute `x + m + a + s` to the total
- Part 2 starts from the full `1..4000` range in all four dimensions
- each rule splits the active space when needed
- accepted branches contribute the number of combinations they represent
- rejected branches contribute nothing
- the final answer is either the accepted ratings total or the accepted combination count

---

## 🚀 Key Takeaways

- Good example of modelling workflow rules as ordered branching logic
- Part 1 is a direct rule-engine simulation
- Part 2 avoids brute force by recursively partitioning value ranges
- The same workflow semantics are preserved in both parts
- Half-open intervals make range splitting and size calculation clean
- A tuple-based rule structure keeps the implementation compact

---

## 🔗 References

- https://adventofcode.com/2023/day/19