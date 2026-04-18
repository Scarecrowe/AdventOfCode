# 🎄 Advent of Code 2022 - Day 13: Distress Signal

## 📜 Puzzle Overview

This puzzle revolves around comparing and ordering nested packet data.

Each packet is a structure made up of:

- integers
- lists (which may contain integers or other lists)

The input is organised into pairs of packets, separated by blank lines.

Example:

    [1,1,3,1,1]
    [1,1,5,1,1]

    [[1],[2,3,4]]
    [[1],4]

The challenge is to determine how these packets should be ordered based on a custom comparison logic.

---

## 🧩 Part 1

Determine which packet pairs are in the correct order.

### 💡 Approach

- Parse the input into pairs of packet structures
- Compare each pair using recursive comparison rules
- If a pair is in the correct order, record its 1-based index
- Return the sum of all valid indices

---

## 🧩 Part 2

Sort all packets and find the decoder key.

### 💡 Approach

- Parse all packets into a single collection
- Add two special divider packets:

      [[2]]
      [[6]]

- Sort all packets using the same comparison logic
- Find the positions (1-based) of the divider packets
- Multiply those positions to get the final result

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Distress Signal`
- Loads the puzzle input
- Calls both parts

For Part 1:

- Parses packet pairs
- Sums indices of correctly ordered pairs

For Part 2:

- Parses all packets
- Inserts divider packets
- Sorts packets
- Calculates decoder key

---

### `Packet.cs` (or equivalent)

This class models a packet.

A packet can be:

- a single integer
- a list of packets

So conceptually:

- `Packet` is a recursive structure

Example:

- `1` → integer packet
- `[1,2]` → list packet
- `[1,[2,3]]` → nested packet

---

### Parsing Packets

The input is a string representation of nested lists.

Parsing involves:

- reading characters (`[`, `]`, `,`, digits)
- building nested packet structures
- using recursion or a stack-based approach

Each line becomes one packet object.

---

### Packet Comparison Rules

The core of this puzzle is a custom comparison function.

When comparing two values:

#### Integer vs Integer

- lower value comes first
- equal → continue comparison

#### List vs List

- compare elements one-by-one
- first difference determines order
- if one list runs out first → it is smaller

#### Integer vs List

- convert integer to a single-item list
- retry comparison

Example:

    compare(3, [3]) → compare([3], [3])

These rules define a recursive comparison algorithm.

---

### Recursive Comparison

Comparison naturally becomes recursive:

- compare current elements
- if equal → move to next
- if not → return result
- if one list ends → decide based on length

This is effectively a custom "less-than" operation for nested structures.

---

### Sorting Packets (Part 2)

For Part 2:

- all packets are placed into one list
- sorting uses the custom comparison function

This allows standard sorting algorithms to order complex nested structures correctly.

---

### Divider Packets

Two special packets are added:

    [[2]]
    [[6]]

After sorting:

- find their positions in the list
- multiply positions to produce the decoder key

---

### Part 1 Return Value

- sum of indices (1-based) of correctly ordered packet pairs

---

### Part 2 Return Value

- product of the positions of the divider packets after sorting

---

## 🛠 Implementation Notes

- Packet structure is recursive (tree-like)
- Comparison is also recursive
- Parsing can be done via:
  - recursion
  - stack-based parsing
  - or language helpers (e.g. JSON-style parsing)
- Sorting relies entirely on the custom comparator
- Divider packets must be included before sorting

---

## 🧪 Behaviour Summary

Given a set of packet pairs:

- each packet is parsed into a nested structure
- pairs are compared using recursive rules
- Part 1 identifies correctly ordered pairs
- Part 2 sorts all packets and finds divider positions
- results are derived from ordering logic

---

## 🚀 Key Takeaways

- Strong example of recursive data structures
- Custom comparison logic drives both parts
- Demonstrates how to sort complex nested data
- Clean separation between parsing, comparison, and execution
- Reusable comparator simplifies both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2022/day/13