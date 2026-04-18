# 🎄 Advent of Code 2025 - Day 09: Movie Theater

## 📜 Puzzle Overview

This puzzle operates on a structured input representing a movie theater system.

The input describes relationships between seats, rows, or booking groupings (depending on how your implementation models it).

From this data, the solver constructs an internal representation that allows efficient grouping, traversal, or merging of related elements.

Part 1 evaluates the structure at an intermediate state and calculates a value based on grouped elements.

Part 2 continues processing until a final condition is met, then derives a result from the final transformation step.

---

## 🧩 Part 1

Determine the required value based on grouped or processed theater elements once the solver reaches its stopping condition.

### 💡 Approach

- Parse the input into a structured representation (e.g. seats, rows, or nodes)
- Build relationships between elements
- Process those relationships in a defined order
- Track groups or state transitions
- When the stopping condition is reached:
  - evaluate the current structure
  - extract relevant group sizes or values
  - compute the final result

---

## 🧩 Part 2

Continue processing until the entire structure reaches its final unified or completed state, then compute a value derived from the final operation.

### 💡 Approach

- Reuse the same parsed input and relationship structure
- Continue processing beyond Part 1 limits
- Track state changes or merges
- Detect when the final condition is met (e.g. single structure, full occupancy, or completed grouping)
- Extract values from the final operation that triggered completion
- Return the computed result

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Movie Theater`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new `MovieTheater` instance
- Calls the Part 1 solver method

For Part 2:

- Creates a new `MovieTheater` instance
- Calls the Part 2 solver method

---

### `MovieTheater.cs`

This class contains the core logic for:

- parsing input
- building relationships
- processing state transitions
- solving both parts of the puzzle

It typically stores:

- parsed elements (seats, nodes, etc.)
- relationships or connections between them

The constructor:

- parses the input
- initializes internal structures
- prepares any precomputed relationships

---

### Parsing the Input

The input is parsed into structured elements.

At a high level:

- each line represents a unit of data
- values are extracted and converted into strongly typed objects
- those objects are stored in a collection for processing

Example interpretation:

- a line may represent a seat, row, or connection
- values are split and parsed into usable fields

---

### Building Relationships

The solver builds relationships between parsed elements.

This may include:

- adjacency links
- pairwise connections
- grouping rules

These relationships are used to drive the logic for both puzzle parts.

---

### Processing Logic

The main algorithm processes relationships in a defined order.

Typical steps include:

- iterating over relationships
- applying transformations or unions
- tracking state changes
- updating group or structure information

---

### Part 1 Logic

The Part 1 solver:

- initializes tracking structures
- processes relationships up to a stopping condition
- evaluates the current grouped state

It then:

- calculates group sizes or values
- selects the required subset (e.g. largest groups)
- computes and returns the final result

---

### Part 2 Logic

The Part 2 solver:

- continues processing beyond Part 1
- tracks when the system reaches its final state

Once complete:

- identifies the final operation that triggered completion
- extracts required values from that step
- returns the computed result

---

## 🛠 Implementation Notes

- Input is parsed into structured objects
- Relationships are built between elements
- A central processing loop drives both puzzle parts
- Part 1 evaluates an intermediate state
- Part 2 evaluates the final state of the system

---

## 🧪 Behaviour Summary

Given the input:

- elements are parsed and connected
- relationships are processed in sequence
- state evolves as processing continues
- Part 1 captures a snapshot during processing
- Part 2 waits for full completion and uses the final transition

---

## 🚀 Key Takeaways

- Demonstrates structured parsing and relationship building
- Shows how a single processing pipeline can support multiple puzzle goals
- Highlights differences between intermediate and final state evaluation
- Encourages reuse of logic between puzzle parts

---

## 🔗 References

- https://adventofcode.com/2025/day/9