# 🎄 Advent of Code 2025 - Day 12: Christmas Tree Farm

## 📜 Puzzle Overview

This puzzle models a Christmas tree farm where presents must be placed under trees.

Each section of the input represents:

- a region with given dimensions
- a set of presents with specific sizes or shapes

The challenge is to determine whether the presents can fit within each region without overlapping.

At its core, this is a **2D packing / fitting problem**, where objects must be arranged inside a bounded grid.

Part 1 focuses on determining which regions are valid.

Part 2 expands on this with additional constraints or stricter validation.

---

## 🧩 Part 1

Determine how many regions can successfully fit all required presents.

### 💡 Approach

- Parse each region:
  - extract dimensions (width and height)
  - extract present sizes or shapes
- For each region:
  - calculate total available area
  - calculate total required area of presents
- Apply quick feasibility checks:
  - if total present area exceeds region area → impossible
  - otherwise → potentially valid
- Count all regions that satisfy the conditions

In practice, the puzzle is designed so that simple checks are often sufficient to determine validity without full packing simulation.

---

## 🧩 Part 2

Apply stricter rules to determine valid regions under more constrained placement requirements.

### 💡 Approach

- Reuse the parsed region and present data
- Introduce additional constraints such as:
  - tighter packing limits
  - shape orientation (rotation / flipping)
- For each region:
  - evaluate whether all presents can still fit under the new rules
- Count the number of valid regions

Depending on implementation, this may involve:

- more precise space estimation
- or limited simulation of placement

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Christmas Tree Farm`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new `ChristmasTreeFarm` instance
- Calls the Part 1 solver method

For Part 2:

- Creates a new `ChristmasTreeFarm` instance
- Calls the Part 2 solver method

---

### `ChristmasTreeFarm.cs`

This class contains the full logic for:

- parsing region definitions
- evaluating packing feasibility
- solving both puzzle parts

It typically stores:

- a collection of regions
- each region’s dimensions
- associated present requirements

The constructor:

- parses the input into structured region objects
- prepares data for evaluation

---

### Parsing the Input

The input is divided into sections.

Each section represents a region and contains:

- region dimensions
- a list of presents

Parsing involves:

- splitting input into blocks
- extracting numeric values
- storing them in structured objects

At a high level:

- each region becomes a container
- each present contributes to required space

---

### Representing Regions

Each region can be modeled as:

- a rectangular grid (width × height)

Key derived values:

- total area = width * height

This value is used for feasibility checks.

---

### Representing Presents

Presents are represented as:

- sizes (area values)
- or shapes (collections of cells)

Key derived value:

- total required area = sum of all present areas

---

### Feasibility Checking

The core logic determines whether presents can fit.

Basic rule:

    total_present_area <= region_area

Additional checks may include:

- shape constraints
- packing density limits
- orientation allowances

---

### Part 1 Logic

The Part 1 solver:

- iterates through all regions
- performs feasibility checks
- counts how many regions can fit all presents

The final result is:

- the number of valid regions

---

### Part 2 Logic

The Part 2 solver:

- applies stricter placement rules
- re-evaluates each region

This may involve:

- tighter constraints on packing
- considering shape transformations

The final result is:

- the number of regions that still satisfy all conditions

---

## 🛠 Implementation Notes

- Input is parsed into region-based structures
- Each region is evaluated independently
- Area-based pruning is the primary optimisation
- Full packing simulation is typically unnecessary
- Part 2 introduces stricter constraints but reuses the same core logic

---

## 🧪 Behaviour Summary

Given a set of regions:

- each region defines available space
- presents define required space
- feasibility checks determine valid placements

Part 1:

- uses simple area-based validation

Part 2:

- applies stricter placement rules
- reduces the number of valid regions

---

## 🚀 Key Takeaways

- Classic **bin-packing style problem**
- Efficient solutions rely on pruning rather than brute force
- Area comparison is a powerful early filter
- Demonstrates how constraints evolve between puzzle parts
- Encourages thinking about spatial feasibility rather than explicit placement

---

## 🔗 References

- https://adventofcode.com/2025/day/12