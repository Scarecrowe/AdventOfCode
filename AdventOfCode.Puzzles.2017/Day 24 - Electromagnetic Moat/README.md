# 🎄 Advent of Code 2017 - Day 24: Electromagnetic Moat

## 📜 Puzzle Overview

This puzzle involves building bridges from a set of components.

Each component has two ports, defined by numbers:

    A/B

For example:

    3/7

means one side has port `3`, the other has port `7`.

Rules:

- components can connect when port numbers match
- components can be flipped
- each component can only be used once
- the bridge must start with port `0`

The goal is to build valid bridges and evaluate them based on strength and length.

Bridge strength is calculated as:

    sum of all port values in all components

Example:

    0/3 -- 3/7 -- 7/4

strength:

    (0+3) + (3+7) + (7+4) = 24

---

## 🧩 Part 1

Find the strength of the strongest possible bridge.

### 💡 Approach

- Parse all components into usable structures
- Start building bridges from port `0`
- Recursively try every valid component that matches the current open port
- Track total strength as the bridge grows
- Keep the maximum strength found

---

## 🧩 Part 2

Find the strength of the longest possible bridge.

### 💡 Approach

- Reuse the same bridge-building logic
- Track both:
  - length (number of components)
  - strength
- Prefer:
  - longest bridge first
  - strongest bridge if lengths are equal

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Electromagnetic Moat`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- finds the strongest bridge

For Part 2:

- finds the longest bridge, with strength as a tiebreaker

---

### Component Representation

Each component stores:

- `PortA`
- `PortB`

It can:

- match against a required port
- return the opposite port when connected

Example:

    3/7

- matches `3` or `7`
- if connected on `3`, next required port becomes `7`

---

### Parsing Input

Each input line:

    A/B

is parsed into:

- a component object

All components are collected into a list for processing.

---

### Bridge Construction

The core logic is recursive.

At each step:

- find all unused components that match the current port
- for each valid component:
  - mark it as used
  - extend the bridge
  - recurse with the new required port
  - backtrack after exploring

Conceptually:

    build(port, remaining_components)

---

### Recursion Strategy

The recursive function tracks:

- current port to match
- remaining unused components
- current strength
- current length

At each recursive call:

- attempt to extend the bridge
- if no further extensions are possible:
  - evaluate the bridge as a final candidate

---

### Strength Calculation

Strength is accumulated during recursion:

    strength += PortA + PortB

This avoids recalculating from scratch at each leaf.

---

### Part 1 Logic

Track:

- maximum strength seen

At each completed bridge:

- compare current strength with best
- update if higher

---

### Part 2 Logic

Track:

- maximum length
- corresponding strength

At each completed bridge:

- if current length > best length:
  - replace best
- if lengths are equal:
  - keep the stronger bridge

---

### Backtracking

Each recursive branch:

- removes a component from available set
- explores deeper
- restores the component after returning

This ensures:

- all combinations are explored
- no component is reused incorrectly

---

### Search Space

The problem explores:

- all valid permutations of components

This naturally forms a tree:

- root starts at port `0`
- branches represent valid connections
- leaves represent completed bridges

---

## 🛠 Implementation Notes

- Recursion is the cleanest approach for exploring combinations
- Components must not be reused within a single bridge
- Efficient tracking of used components is important
- Strength can be accumulated incrementally
- Part 2 adds an extra comparison layer (length vs strength)

---

## 🧪 Behaviour Summary

Given a list of components:

- the solver builds every valid bridge starting from port `0`
- each step matches ports and extends the bridge
- recursion explores all possible paths
- Part 1 returns the maximum strength
- Part 2 returns the strength of the longest bridge
- ties in length are broken by strength

---

## 🚀 Key Takeaways

- Classic recursion and backtracking problem
- Naturally modelled as a tree search
- Clean separation between generation and evaluation
- Incremental scoring improves performance
- Part 2 adds prioritisation logic without changing the core algorithm

---

## 🔗 References

- https://adventofcode.com/2017/day/24