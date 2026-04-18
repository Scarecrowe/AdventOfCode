# 🎄 Advent of Code 2023 - Day 8: Haunted Wasteland

## 📜 Puzzle Overview

This puzzle is about traversing a network of named nodes using a repeating sequence of directions.

The input contains:

- a direction string made of `L` and `R`
- a blank line
- a list of node definitions

A node definition looks like this:

    AAA = (BBB, CCC)

Each node has:

- a name
- a left destination
- a right destination

Part 1 starts at `AAA` and follows the repeating directions until reaching `ZZZ`.

Part 2 starts from every node whose name ends with `A` and finds how many steps it takes until they all align on nodes ending with `Z`.

---

## 🧩 Part 1

Find how many steps it takes to move from `AAA` to `ZZZ`.

### 💡 Approach

- Parse the first input line as the instruction sequence
- Parse each node line into a graph structure
- Start at `AAA`
- Read instructions in a loop, repeating from the beginning when needed
- Move left on `L` and right on `R`
- Count steps until the current node becomes `ZZZ`

---

## 🧩 Part 2

Find how many steps it takes for all ghost paths to land on nodes ending with `Z` at the same time.

### 💡 Approach

- Reuse the same parsed instruction sequence and node graph
- Find every starting node whose name ends with `A`
- For each start node:
  - follow the repeating instructions
  - count steps until a node ending with `Z` is reached
- Combine those cycle lengths using lowest common multiple
- Return the combined result

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Haunted Wasteland`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates the puzzle solver with the input
- Runs the logic that starts from `AAA`
- Returns the number of steps needed to reach `ZZZ`

For Part 2:

- Reuses the same parsed network
- Runs the ghost-path logic from all nodes ending with `A`
- Returns the combined step count

---

### Node Representation

Each node stores:

- its name
- its left destination
- its right destination

A line such as:

    AAA = (BBB, CCC)

means:

- `L` sends you to `BBB`
- `R` sends you to `CCC`

The solver typically stores nodes in a lookup structure keyed by node name so movement is fast.

---

### Parsing the Input

The parser reads:

- the instruction sequence from the first line
- the node definitions from the remaining lines

For each node line it extracts:

- the node name
- the left target
- the right target

That produces a full navigation map for the wasteland.

---

### Instruction Handling

The instruction string is treated as repeating forever.

So if the instruction string is:

    LLR

the solver uses it like this:

    LLRLLRLLRLLR...

This is usually implemented by advancing an instruction index and resetting it back to the start when the end is reached.

---

### Part 1 Traversal

For the silver solution the solver:

- starts at `AAA`
- follows one instruction per step
- updates the current node using either the left or right branch
- increments a step counter
- stops when the node becomes `ZZZ`

The returned value is the total number of steps taken.

---

### Part 2 Ghost Traversal

For the gold solution the solver:

- finds all nodes whose names end with `A`
- treats each of them as an independent starting point
- follows the same repeating instruction sequence for each one
- records how many steps it takes for each path to first reach a node ending with `Z`

Because these paths usually repeat in cycles, the final answer is found by combining their step counts using lowest common multiple rather than simulating all paths forever in lockstep.

---

### Lowest Common Multiple

Part 2 works because each starting path eventually repeats on a stable cycle.

So instead of checking every ghost step forever, the solver can:

- determine the repeating length for each path
- compute the lowest common multiple of those lengths

That gives the first step where all ghost paths align on valid ending nodes at the same time.

---

## 🛠 Implementation Notes

- The puzzle uses a repeating instruction sequence
- Each node has exactly two exits: left and right
- Part 1 is a straightforward graph walk from `AAA` to `ZZZ`
- Part 2 starts from every node ending with `A`
- Part 2 is typically solved by combining per-path cycle lengths
- A dictionary-style lookup is ideal for node navigation
- Lowest common multiple is the key optimisation for the second half

---

## 🧪 Behaviour Summary

Given a direction string and a map of named nodes:

- the solver parses the graph
- repeatedly applies `L` and `R` instructions
- Part 1 counts steps from `AAA` until `ZZZ`
- Part 2 tracks multiple starting nodes ending with `A`
- each ghost path is reduced to a cycle length
- the final answer is the point where all those cycles align

---

## 🚀 Key Takeaways

- Good example of modelling a graph with named transitions
- The repeating instruction string acts like a cyclic driver
- Part 1 is a simple deterministic traversal
- Part 2 becomes manageable by using cycle detection and lowest common multiple
- The same parsed network supports both puzzle parts cleanly

---

## 🔗 References

- https://adventofcode.com/2023/day/8