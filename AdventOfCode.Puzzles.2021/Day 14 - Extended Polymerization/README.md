# 🎄 Advent of Code 2021 - Day 14: Extended Polymerization

## 📜 Puzzle Overview

This puzzle simulates polymer growth using pair insertion rules.

The input contains:

- an initial polymer template
- a set of insertion rules

A rule looks like this:

    CH -> B

Which means:

- whenever the pair `CH` appears
- the character `B` is inserted between them
- so `CH` becomes:
  
    CBH

Instead of rebuilding the full polymer string each step, this solver tracks how many times each pair appears. That makes large step counts practical.

The puzzle entry runs the process for:

- `10` steps in Part 1
- `40` steps in Part 2

The silver and gold answers are returned by calling:

    new ExtendedPolymerization(this.Input).Process(10)
    new ExtendedPolymerization(this.Input).Process(40)

---

## 🧩 Part 1

Determine the difference between the most common and least common element after 10 steps.

### 💡 Approach

- Parse the initial polymer template
- Parse every insertion rule
- Count how many times each adjacent pair appears in the template
- For 10 steps:
  - replace each pair count with the two new pairs produced by that rule
- Convert pair counts into element totals
- Return:
  - most common element count
  - minus least common element count

---

## 🧩 Part 2

Determine the same most-common minus least-common difference after 40 steps.

### 💡 Approach

- Reuse the same pair-counting logic
- Run it for much longer
- Avoid building the full polymer string explicitly
- Keep only:
  - pair frequencies
- Derive character totals from those pair frequencies at the end

This makes the 40-step simulation feasible.

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Extended Polymerization`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new ExtendedPolymerization(this.Input)`
- calls `Process(10)`

For Part 2:

- creates `new ExtendedPolymerization(this.Input)`
- calls `Process(40)`

---

### `ExtendedPolymerization.cs`

This class contains the full parsing and simulation logic.

It stores:

- `Template`
- `Rules`

The constructor:

- reads the initial template
- skips the blank separator line
- parses each rule into a dictionary entry

So after construction, the solver has:

- the starting polymer string
- a lookup table describing how each pair expands

---

### Parsing the Input

The constructor reads the input line by line.

It uses a flag to switch from template parsing into rule parsing once it reaches the blank line.

At a high level it does:

- before the blank line:
  - store the polymer template
- after the blank line:
  - split each rule on `" -> "`
  - store the pair expansion result

For a rule such as:

    CH -> B

the implementation stores:

- `CB`
- `BH`

So the rule dictionary maps one input pair to the two output pairs created after insertion.

---

### Rule Storage

`Rules` is a dictionary keyed by the original pair.

Each entry stores the two pairs that replace it after one step.

So:

    CH -> B

is stored logically as:

    CH => [ "CB", "BH" ]

This is the key optimisation of the solution.

Instead of inserting characters into a string, the solver transforms pair counts into new pair counts.

---

### Counting the Initial Pairs

`CountPairs()` builds the initial pair frequency dictionary from the template.

At a high level it does:

- walk from left to right across the template
- read each adjacent two-character pair
- increment that pair in a dictionary

For example, a template like:

    NNCB

produces the pairs:

- `NN`
- `NC`
- `CB`

Each pair is counted into:

- `Dictionary<string, long> counts`

So the simulation starts from pair frequencies rather than the full expanded polymer.

---

### Running the Simulation

`Process(int steps)` performs the full polymer growth process.

It begins with:

    Dictionary<string, long> counts = this.CountPairs();

Then for each step it does:

    counts = this.CountStep(ref counts);

So every iteration replaces the current pair-frequency map with the next one.

After all requested steps are complete, it returns:

    SumOfMinAndMax(ref counts)

---

### Expanding One Step

`CountStep(ref Dictionary<string, long> counts)` calculates the next generation of pair totals.

For every current pair and its count:

- look up the two replacement pairs in `Rules`
- add the original count to both new pairs in the next dictionary

So if a pair appears 25 times, both resulting pairs receive 25 additional counts.

This means the implementation never builds the polymer text directly.  
It only moves counts from old pairs into new pairs.

---

### Converting Pair Counts into Character Totals

After all steps are complete, `SumOfMinAndMax(ref Dictionary<string, long> counts)` converts pair counts into element totals.

It creates:

- `Dictionary<char, long> totals = new();`

Then for every counted pair, it adds the pair count to:

    kvp.Key[1]

So it counts the second character of each pair.

This works because, across the chain of overlapping pairs, each character except the first one in the original template appears as the second character of some pair.

The method then returns:

    totals.Max(x => x.Value) - totals.Min(x => x.Value)

So the final answer is the difference between the most common and least common element totals derived from the pair frequencies.

---

## 🛠 Implementation Notes

- The solution uses pair counting instead of string expansion
- Rules map one pair to exactly two replacement pairs
- Pair totals are stored in `Dictionary<string, long>`
- `long` is important because counts grow very large
- Each simulation step builds a fresh next-state dictionary
- Final element totals are derived from pair counts
- Part 1 and Part 2 use the same logic with different step counts

---

## 🧪 Behaviour Summary

Given:

- a starting polymer template
- a set of insertion rules

the solver:

- parses the template
- converts it into pair frequencies
- repeatedly applies pair-replacement rules
- tracks only how many times each pair exists
- derives final element totals from the resulting pair counts
- returns the difference between the largest and smallest totals

So the final result is the polymer score after either 10 or 40 growth steps.

---

## 🚀 Key Takeaways

- Good example of replacing impossible string growth with frequency tracking
- Rules are precomputed as pair-to-two-pairs transformations
- The polymer is represented by pair counts, not by full text
- Each step is a dictionary remapping operation
- The same compact algorithm supports both 10-step and 40-step runs
- Using `long` keeps the large-count Part 2 result safe

---

## 🔗 References

- https://adventofcode.com/2021/day/14