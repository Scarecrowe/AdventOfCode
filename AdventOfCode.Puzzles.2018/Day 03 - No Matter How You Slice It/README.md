# 🎄 Advent of Code 2018 - Day 03: No Matter How You Slice It

## 📜 Puzzle Overview

This puzzle revolves around a large piece of fabric and a set of rectangular claims made on it.

Each claim defines:

- an ID
- a position (offset from the top-left corner)
- a width
- a height

An input line looks like this:

    #123 @ 3,2: 5x4

This means:

- Claim ID: 123
- Left offset: 3
- Top offset: 2
- Width: 5
- Height: 4

The solver parses each claim and maps it onto a shared grid representing the fabric.

Part 1 determines how many square inches of fabric are claimed by more than one rectangle.

Part 2 finds the single claim that does not overlap with any others.

---

## 🧩 Part 1

Determine how many square inches of fabric are within two or more claims.

### 💡 Approach

- Parse each claim into a structured object
- Represent the fabric as a coordinate map or grid
- For every claim:
  - iterate over its covered area
  - increment a counter for each coordinate
- After processing all claims:
  - count how many coordinates have a value greater than 1

---

## 🧩 Part 2

Find the ID of the only claim that does not overlap with any others.

### 💡 Approach

- Reuse the populated fabric grid from Part 1
- For each claim:
  - check every coordinate it covers
  - ensure all coordinates have a count of exactly 1
- The claim that satisfies this condition is the answer

---

## 🧠 Code Breakdown

### `Day03.cs`

This is the puzzle entry point.

- Sets the puzzle title to `No Matter How You Slice It`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses the claims
- Builds the fabric grid
- Counts overlapping squares

For Part 2:

- Reuses the grid
- Finds the non-overlapping claim

---

### `Claim.cs`

This class models a single fabric claim.

It stores:

- `Id`
- `Left`
- `Top`
- `Width`
- `Height`

The constructor:

- parses the input string
- extracts numeric values for position and size
- assigns the claim ID

So a line such as:

    #123 @ 3,2: 5x4

becomes a fully initialised claim object.

---

### Claim Coverage

Each claim covers a rectangular region of the fabric.

This region is iterated using nested loops:

    for x in Left -> Left + Width
    for y in Top -> Top + Height

Each coordinate within this region is marked on the fabric grid.

---

### Fabric Representation

The fabric is typically stored as:

- a dictionary keyed by coordinates, or
- a 2D array

Each coordinate stores:

- the number of claims that include that square

Example:

    (3,2) -> 2

means two claims overlap at that square.

---

### Building the Grid

The solver processes all claims:

- iterate through each claim
- for each coordinate it covers:
  - increment the value in the grid

At the end:

- every square inch knows how many claims touch it

---

### Counting Overlaps

To solve Part 1:

- iterate through all grid values
- count how many are greater than 1

This gives the number of overlapping square inches.

---

### Finding the Non-Overlapping Claim

For Part 2:

- iterate through each claim
- check all coordinates it covers
- verify each coordinate has a value of exactly 1

If all coordinates pass this check:

- that claim does not overlap with any others
- return its ID

---

## 🛠 Implementation Notes

- Claims are parsed from a structured string format
- Fabric is tracked using coordinate-based counting
- Nested loops are used to map claim areas
- Part 1 is a counting problem over the grid
- Part 2 is a validation pass over each claim
- Grid reuse avoids recomputation between parts

---

## 🧪 Behaviour Summary

Given a list of rectangular fabric claims:

- the solver parses them into structured objects
- each claim maps its area onto a shared grid
- Part 1 counts how many squares are claimed multiple times
- Part 2 identifies the single claim with no overlaps
- both parts rely on the same underlying grid representation

---

## 🚀 Key Takeaways

- Great example of 2D spatial mapping
- Efficient reuse of computed grid data
- Clear separation between parsing, mapping, and analysis
- Demonstrates how counting overlaps can simplify collision detection
- Part 2 builds directly on Part 1's results

---

## 🔗 References

- https://adventofcode.com/2018/day/3