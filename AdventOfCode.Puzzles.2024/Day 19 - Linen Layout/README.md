# 🎄 Advent of Code 2024 - Day 19: Linen Layout

## 📜 Puzzle Overview

This puzzle works with:

- a set of towel patterns
- a set of target designs

The input is split into two sections:

- the first section defines the available patterns as a comma-separated list
- the second section lists the designs to test

The solver parses those into:

- `Patterns`
- `Designs`

Then it uses a recursive depth-first search with memoization to determine:

- whether a design can be built from the available patterns
- how many different ways that design can be constructed

Part 1 counts how many designs are possible.

Part 2 sums the total number of valid arrangements across all possible designs.

---

## 🧩 Part 1

Determine how many target designs can be formed from the available towel patterns.

### 💡 Approach

- Parse the comma-separated pattern list
- Parse the set of design strings
- For each design:
  - recursively test whether it can be reduced by removing valid pattern prefixes
  - cache previously solved suffixes
- If a design has at least one valid construction path, count it as possible
- Return the total number of possible designs

---

## 🧩 Part 2

Determine the total number of valid arrangements across all target designs.

### 💡 Approach

- Reuse the same recursive search as Part 1
- For each design:
  - count every valid way the design can be built by consuming matching prefixes
- Use memoization so repeated suffix checks are not recalculated
- Sum all valid arrangement counts for every design that can be formed

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Linen Layout`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new LinenLayout(this.Input)`
- Calls `Possible()`

For Part 2:

- Creates `new LinenLayout(this.Input)`
- Calls `Arrangements()`

---

### `LinenLayout.cs`

This class contains the full parsing and recursive matching logic.

It stores:

- `Patterns`
- `Designs`
- `memo`
- `counts`
- `p2res`

`Patterns` is a list of available towel pattern strings.

`Designs` is a set of target design strings.

The recursive solver uses:

- `memo` to remember whether a design suffix is solvable
- `counts` to remember how many ways that suffix can be formed

---

### Parsing the Input

`Parse(string[] input)` reads the puzzle input in two phases.

It uses:

- a `designs` flag to detect when the blank separator line has been reached
- a temporary `patterns` string to collect the pattern definition line(s)

Before the blank line:

- all text is appended into the pattern string

When the blank line is reached:

- the collected pattern string is split on `", "`
- the resulting list becomes `Patterns`

After the blank line:

- each line is added to `Designs`

So the final parsed structure is:

- one list of reusable towel patterns
- one collection of target design strings to validate

---

### Part 1 Main Logic

`Possible()` loops through every design in `Designs`.

For each design it calls:

    DFS(design)

That returns:

- whether the design can be formed
- how many ways it can be formed

If the design is possible:

- increment the Part 1 count
- also add its arrangement count into `p2res`

The method returns:

- the number of designs that have at least one valid construction

---

### Part 2 Main Logic

`Arrangements()` also loops through every design and calls:

    DFS(design)

If the design is possible:

- add its arrangement count to the running result

The method returns:

- the total number of valid arrangements across all solvable designs

So both parts rely on the exact same recursive search.

---

### Recursive Search

`DFS(string design)` is the core solver.

It returns a tuple containing:

- `match`
- `ways`

The method answers two questions for the current design suffix:

- can it be formed at all
- if so, how many ways can it be formed

---

### Memoization

At the start of `DFS(...)`, the solver checks:

    if (memo.TryGetValue(design, out bool value))

If the suffix has already been solved, it returns:

- the cached success value from `memo`
- the cached arrangement count from `counts`

This prevents repeated work when many designs share the same remaining suffix.

---

### Exact Pattern Match

The first direct success case is:

    if (this.Patterns.Contains(design))

If the full remaining suffix exactly matches one available pattern:

- `memo[design] = true`
- `counts[design]++`

So a whole remaining suffix that is itself a valid pattern contributes one complete arrangement.

---

### Prefix Expansion

After checking the full direct match, the solver tries all shorter patterns that match the start of the current design.

It filters patterns with:

- pattern length shorter than the current design
- prefix equality against the start of the design

At a high level it does:

- find every pattern `t` where the design starts with `t`
- remove that prefix
- recursively solve the remainder

This means the design is built from left to right by repeatedly consuming valid prefixes.

---

### Combining Arrangement Counts

For each valid prefix match:

- compute the remaining suffix
- recursively call `DFS(sub)`

If the suffix is solvable:

- mark the current design as solvable
- add the suffix arrangement count into `counts[design]`

So every recursive branch that succeeds contributes to the total number of ways for the current design.

This is how the solver turns a boolean prefix match search into a full arrangement counter.

---

### Final DFS Result

At the end of `DFS(...)`, the solver sets:

    memo[design] = counts[design] > 0

Then it returns:

- whether the design has any valid construction
- how many constructions were found

So every suffix is cached with both:

- its success state
- its arrangement total

---

### Part 1 Return Value

When called as:

    Possible()

the method returns:

- the number of designs that can be formed from the available patterns

A design counts if `DFS(design)` finds at least one valid construction path.

---

### Part 2 Return Value

When called as:

    Arrangements()

the method returns:

- the sum of all valid arrangement counts across every solvable design

So the gold answer is not just whether a design is possible, but how many different constructions exist in total.

---

## 🛠 Implementation Notes

- Patterns are parsed from a comma-separated string
- Designs are stored in a `HashSet<string>`
- The solver uses recursive prefix matching
- Memoization caches whether each suffix is solvable
- A parallel count structure stores how many arrangements each suffix has
- Exact full-pattern matches count as one arrangement
- Shorter prefix matches recurse into the remaining suffix
- Both puzzle parts reuse the same `DFS(...)` logic

---

## 🧪 Behaviour Summary

Given a set of towel patterns and target designs:

- the solver parses the available patterns
- parses every requested design
- recursively strips matching prefixes from each design
- caches repeated suffix results
- Part 1 counts how many designs are constructible
- Part 2 sums how many valid constructions exist in total
- the final answers come from the same memoized recursive search

---

## 🚀 Key Takeaways

- Good example of solving a string construction problem with recursive suffix reduction
- Memoization is the key optimisation that avoids repeated work
- The same DFS returns both feasibility and total arrangement count
- Part 1 and Part 2 differ only in how they aggregate the DFS results
- Prefix matching keeps the implementation simple and closely aligned to the puzzle rules

---

## 🔗 References

- https://adventofcode.com/2024/day/19