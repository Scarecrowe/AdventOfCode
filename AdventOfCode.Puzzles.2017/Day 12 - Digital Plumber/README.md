# 🎄 Advent of Code 2017 - Day 12: Digital Plumber

## 📜 Puzzle Overview

This puzzle models a network of programs connected via bidirectional pipes.

Each line of input describes a program and the other programs it is directly connected to, for example:

    0 <-> 2
    2 <-> 0, 3, 4

This effectively defines an undirected graph where:

- each program is a node
- each connection is an edge

The goal is to explore how these programs are grouped based on connectivity.

Part 1 focuses on the size of the group that contains program `0`, while Part 2 determines how many distinct groups exist in total.

---

## 🧩 Part 1

Determine how many programs are in the group that contains program `0`.

### 💡 Approach

- Parse the input into an adjacency list (graph)
- Start from program `0`
- Traverse all reachable programs using BFS or DFS
- Count how many unique programs are visited

---

## 🧩 Part 2

Determine how many distinct groups (connected components) exist in the full graph.

### 💡 Approach

- Parse the full graph as in Part 1
- Maintain a set of unvisited programs
- While unvisited programs remain:
  - pick one
  - traverse its entire connected component
  - remove all visited nodes from the unvisited set
  - increment group count
- Return the total number of groups found

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Digital Plumber`
- Loads the puzzle input
- Calls both puzzle parts

For Part 1:

- Creates the solver instance with input
- Calls a method to count the group containing program `0`

For Part 2:

- Reuses the same structure
- Calls a method to count total groups

---

### Core Graph Structure

The input is parsed into a dictionary or similar structure:

- key: program ID
- value: list of connected program IDs

Example representation:

    0 -> [2]
    2 -> [0, 3, 4]

This allows fast lookup of neighbours during traversal.

---

### Parsing the Input

Each line is split into two parts:

    <program> <-> <connections>

Then:

- left side becomes the node ID
- right side is split by commas into connected nodes

Example:

    2 <-> 0, 3, 4

becomes:

- `2` connected to `[0, 3, 4]`

---

### Graph Traversal

Traversal is typically implemented using either:

- Breadth-First Search (queue)
- Depth-First Search (stack or recursion)

Basic idea:

    start from node
    mark as visited
    visit all neighbours
    repeat until no nodes left

---

### Part 1 Logic

To find the size of the group containing `0`:

- start traversal from `0`
- keep a `visited` set
- explore all reachable nodes
- return `visited.Count`

---

### Part 2 Logic

To count all groups:

- maintain a global `visited` set
- iterate through all program IDs
- for each unvisited node:
  - run a traversal
  - mark all reachable nodes
  - increment group counter

---

### Example Traversal Flow

Given:

    0 <-> 2
    2 <-> 0, 3
    3 <-> 2
    4 <-> 5
    5 <-> 4

You get two groups:

- Group 1: `0, 2, 3`
- Group 2: `4, 5`

Part 1 result: `3`  
Part 2 result: `2`

---

## 🛠 Implementation Notes

- Graph is undirected, so connections are symmetric
- A `HashSet<int>` is ideal for tracking visited nodes
- BFS is often easier to reason about, but DFS works just as well
- Avoid revisiting nodes to prevent infinite loops
- Input size is small enough that performance is not a concern

---

## 🧪 Behaviour Summary

Given a set of program connections:

- the solver builds a graph representation
- traversal from a node reveals its full connected group
- Part 1 isolates the group containing program `0`
- Part 2 counts how many independent groups exist overall

---

## 🚀 Key Takeaways

- Classic graph traversal problem
- Direct application of connected components
- Reinforces BFS/DFS fundamentals
- Clean separation between "explore one component" and "count all components"

---

## 🔗 References

- https://adventofcode.com/2017/day/12