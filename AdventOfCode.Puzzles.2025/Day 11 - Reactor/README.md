# 🎄 Advent of Code 2025 - Day 11: Reactor

## 📜 Puzzle Overview

This puzzle models a reactor system as a directed graph of connected devices.

Each line of input describes:

- a source device
- a list of destination devices it connects to

At a high level, the system represents a **directed acyclic graph (DAG)** where signals flow from one node to another.

The goal is to count how many distinct paths exist between specific nodes.

Part 1 focuses on counting all valid paths from a starting node to an end node.

Part 2 introduces constraints that require paths to pass through specific intermediate nodes.

---

## 🧩 Part 1

Determine the total number of distinct paths from `you` to `out`.

### 💡 Approach

- Parse the input into a directed graph:
  - each node maps to a list of outgoing connections
- Perform a depth-first search (DFS):
  - recursively explore all possible paths
- Stop when reaching `out`
- Count every valid path

To make this efficient:

- use memoization (caching)
- store the number of paths from each node to `out`

This avoids recomputing overlapping subproblems and keeps the solution fast even with heavy branching.

---

## 🧩 Part 2

Determine the number of distinct paths from `svr` to `out` that pass through both `fft` and `dac`.

### 💡 Approach

- Reuse the same graph
- Introduce constraints:
  - paths must include both required nodes
- Two common strategies:

**Option 1: Split into segments**

- Count paths:
  - `svr → fft`
  - `fft → dac`
  - `dac → out`
- Multiply the results together

**Option 2: Single DFS with state tracking**

- Track whether:
  - `fft` has been visited
  - `dac` has been visited
- Only count paths that reach `out` with both conditions satisfied

The graph structure guarantees that only one valid ordering between `fft` and `dac` contributes to the result.

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Reactor`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new `Reactor` instance
- Calls the Part 1 solver method

For Part 2:

- Creates a new `Reactor` instance
- Calls the Part 2 solver method

---

### `Reactor.cs`

This class contains the full logic for:

- parsing the graph
- storing node relationships
- performing path counting
- solving both puzzle parts

It typically stores:

- a dictionary mapping each node → list of connected nodes

The constructor:

- parses each input line
- builds the adjacency list representation of the graph

---

### Parsing the Input

Each line follows a structure like:

    source: dest1 dest2 dest3

Parsing involves:

- splitting on `:`
- trimming and splitting destination nodes
- storing results in a dictionary

At a high level:

- each node becomes a key
- each value is a list of outgoing edges

---

### Building the Graph

The parsed data forms a directed graph:

- edges point from source → destination
- traversal only follows outgoing connections
- no reverse traversal is required

This structure is ideal for DFS-based path counting.

---

### Depth-First Search (DFS)

A recursive DFS function is used to count paths.

Base case:

- if the current node is `out`, return `1`

Recursive case:

- sum the number of paths from all connected nodes

Conceptually:

    paths(node) = sum(paths(next)) for each next node

Memoization ensures each node is only computed once.

---

### Part 1 Logic

The Part 1 solver:

- starts DFS from `you`
- counts all paths to `out`

It uses:

- recursion
- caching of results per node

The final result is:

- total number of valid paths from `you` to `out`

---

### Part 2 Logic

The Part 2 solver:

- changes the starting node to `svr`
- enforces that paths must pass through:
  - `fft`
  - `dac`

Two valid implementations:

**Segment multiplication**

- compute:
  - paths(`svr`, `fft`)
  - paths(`fft`, `dac`)
  - paths(`dac`, `out`)
- multiply results

**State-aware DFS**

- track flags:
  - visited `fft`
  - visited `dac`
- only count paths when both flags are true at `out`

The final result is:

- total number of valid constrained paths

---

## 🛠 Implementation Notes

- Input is parsed into an adjacency list graph
- DFS is used to explore all paths
- Memoization is critical for performance
- Part 1 is pure path counting
- Part 2 introduces constrained traversal
- Graph properties allow decomposition into smaller path segments

---

## 🧪 Behaviour Summary

Given a directed graph of devices:

- nodes represent components
- edges represent signal flow
- DFS explores all valid routes

Part 1:

- counts all paths from `you` to `out`

Part 2:

- counts only paths from `svr` to `out`
- requires passing through specific nodes
- may decompose into multiple path calculations

---

## 🚀 Key Takeaways

- Classic **graph traversal + path counting** problem
- DFS with memoization is the core technique
- Demonstrates reuse of logic across puzzle parts
- Shows how constraints can transform traversal problems
- Efficient solutions rely on caching and graph structure

---

## 🔗 References

- https://adventofcode.com/2025/day/11