# 🎄 Advent of Code 2017 - Day 07: Recursive Circus

## 📜 Puzzle Overview

This puzzle models a tower of programs.

Each program has:

- a name
- a weight
- optionally, a list of child programs it supports

The structure forms a tree where:

- one program sits at the bottom (the root)
- all others are stacked above it

Part 1 identifies the bottom program.  
Part 2 finds an incorrect weight causing imbalance.

---

## 🧩 Part 1

Determine the name of the bottom program.

### 💡 Approach

- Parse all programs:
  - store each program name
  - track child relationships
- The bottom program is:
  - the one that is never listed as a child
- Find the program that appears only as a parent

---

## 🧩 Part 2

Determine the correct weight needed to balance the tower.

### 💡 Approach

- Build the full tree structure
- Recursively compute total weight for each node:

```
totalWeight = ownWeight + sum(childWeights)
```

- Traverse the tree to find imbalance:
  - compare child weights at each node
  - identify the one that differs
- Once found:
  - determine the correct weight adjustment

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Recursive Circus`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Identifies root node

For Part 2:

- Computes weights
- Detects imbalance

---

### Parsing Input

Each line contains:

```
name (weight) -> child1, child2, child3
```

Or:

```
name (weight)
```

Extract:

- program name
- weight
- optional children list

---

### Tree Construction

- Store nodes in a dictionary
- Link parent and child relationships
- Build full tree structure

---

### Part 1 Logic

- Collect all program names
- Collect all child names
- The root is:

```
root = programNames - childNames
```

---

### Total Weight Calculation

For each node:

```
totalWeight = ownWeight + sum(child.totalWeight)
```

Use recursion to compute values bottom-up

---

### Detecting Imbalance

At each node:

- collect child total weights
- check if all are equal

If not:

- identify the outlier
- compare against correct value

---

### Fixing the Weight

- Determine expected weight for balanced node
- Compute difference:

```
difference = correctWeight - incorrectWeight
```

- Adjust node’s own weight accordingly

---

## 🛠 Implementation Notes

- Tree traversal is recursive
- Memoisation can improve performance
- Only one node will be incorrect
- Detect imbalance from bottom up
- Use grouping to compare child weights

---

## 🧪 Behaviour Summary

Given a set of programs:

- A tree structure is formed
- Part 1 finds the root program
- Part 2 finds the imbalance in weights
- Correct value restores balance

---

## 🚀 Key Takeaways

- Tree construction from flat input
- Recursive weight aggregation
- Detecting anomalies in hierarchical data
- Bottom-up analysis simplifies problem

---

## 🔗 References

- https://adventofcode.com/2017/day/7