# 🎄 Advent of Code 2015 - Day 19: Medicine for Rudolph

## 📜 Puzzle Overview

The medicine machine uses replacement rules to build and transform molecules.

Each replacement rule has the form:

- `H => HO`
- `H => OH`
- `O => HH`

A starting molecule is then provided separately.

Part 1 asks how many distinct molecules can be created by applying exactly one replacement anywhere in the starting molecule.

Part 2 asks how many steps are required to fabricate the target molecule starting from `e`.

---

## 🧩 Part 1

Determine how many distinct molecules can be generated after applying exactly one replacement.

### 💡 Approach

- Parse all replacement rules into a lookup structure
- Read the target molecule from the final input line
- For each replacement source, find every matching position in the molecule
- Replace that occurrence with each possible output value
- Store every generated molecule uniquely
- Return the total count of distinct results

---

## 🧩 Part 2

Determine the minimum number of steps required to build the target molecule starting from `e`.

### 💡 Approach

- Work backwards from the target molecule instead of forwards from `e`
- Reverse the replacement process by reducing larger fragments into smaller ones
- First reduce compound sections that contain `Rn` and `Ar`
- Then continue reducing until the molecule reaches `e`
- Count how many reverse replacement steps were required

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MedicineForRudolph(this.Input)`
- Calls `CreateMolecules()`
- Returns the count of generated molecules

For Part 2:

- Creates `new MedicineForRudolph(this.Input)`
- Calls `FabricateMolecule()`

---

### `MedicineForRudolph.cs`

This class contains the full parsing, molecule generation, and reverse-reduction logic.

The constructor:

- parses the replacement rules
- stores them in `Replacements`
- stores the final input line as `Molecule`

The two main public methods are:

- `CreateMolecules()`
- `FabricateMolecule()`

---

### Parsing Replacements

`ParseRequirements()` reads each replacement rule until the blank line is reached.

It builds:

- a dictionary keyed by the source token
- where each key maps to a list of possible replacement values

This supports rules like:

- `H => HO`
- `H => OH`

where a single source can expand into multiple outputs.

The final molecule is not part of the replacement list and is taken from the last input line.

---

### Creating Molecules

`CreateMolecules()` is used for Part 1.

It works like this:

- loop through every replacement source
- find every occurrence of that source in the target molecule
- split the molecule into:
  - the text before the match
  - the replacement value
  - the text after the match
- build a new molecule from those parts
- store it only if it has not already been seen

At a high level, each generated molecule follows this pattern:

    start + replacement + end

The final result is a dictionary of unique molecules, and its count is the answer for Part 1.

---

### Fabricating the Molecule

`FabricateMolecule()` is used for Part 2.

Instead of growing from `e`, it starts from the target molecule and reduces it backwards.

It does this in two phases:

- `ReduceCompound(ref molecule)`
- `Reduce(ref molecule, x => x == "e")`

The total steps returned by both phases are added together.

This keeps the reduction logic manageable by shrinking more complex compound structures first.

---

### Reducing Compound Sections

`ReduceCompound()` looks for sections involving:

- `Rn`
- `Ar`

These tokens mark more structured regions inside the molecule.

While the molecule still contains `Ar`:

- find the current `Ar`
- find the nearest matching `Rn`
- isolate that compound section
- reduce that section until it no longer contains `Ar`
- place the reduced result back into the main molecule
- add the number of reduction steps used

This breaks the larger molecule into smaller nested pieces before the final reduction back to `e`.

---

### General Reduction

`Reduce()` performs the reverse replacement search.

It starts with a dictionary containing:

- the current molecule
- the number of steps taken so far

Then it repeatedly:

- picks the shortest current molecule candidate
- tries every replacement in reverse
- finds every occurrence of each replacement output
- swaps that output back to its source
- adds the new reduced molecule into the working set with an incremented step count

At a high level, reverse replacement follows this pattern:

    larger_value -> smaller_key

As soon as a newly generated molecule matches the end condition, the method returns the current step count plus one.

For the final Part 2 reduction, the end condition is:

- molecule equals `e`

---

## 🛠 Implementation Notes

- Replacement rules are stored in a dictionary of source token to replacement list
- Part 1 generates all distinct one-step results from the target molecule
- Part 2 works backwards from the target molecule rather than forwards from `e`
- Reverse reduction prefers shorter molecules first
- Compound sections containing `Rn` and `Ar` are reduced separately before the final reduction to `e`
- Uniqueness is handled through dictionary-based storage

---

## 🧪 Examples

Given the rules:

    H => HO
    H => OH
    O => HH

and the molecule:

    HOH

The distinct one-step results are:

- `HOOH`
- `HOHO`
- `OHOH`
- `HHHH`

This gives a total of:

- `4`

For the molecule:

    HOHOHO

the number of distinct one-step molecules is:

- `7`

---

## 🚀 Key Takeaways

- Good example of solving the two parts with very different strategies
- Part 1 uses direct expansion and uniqueness tracking
- Part 2 avoids forward explosion by working backwards through reverse replacements
- Compound-aware reduction keeps the larger molecule search more manageable
- Parsing, generation, and reduction are kept separated cleanly

---

## 🔗 References

- https://adventofcode.com/2015/day/19