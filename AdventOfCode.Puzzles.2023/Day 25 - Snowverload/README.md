# 🎄 Advent of Code 2023 - Day 25: Snowverload

## 📜 Puzzle Overview

This puzzle is about splitting a wiring graph into two separate groups by removing exactly three connections.

Each input line describes one component and the components it connects to.

A line looks like this:

    jqt: rhn xhk nvd

This means:

- `jqt` is connected to `rhn`
- `jqt` is connected to `xhk`
- `jqt` is connected to `nvd`

The solver parses the input into a graph of component IDs and edge pairs.

The goal is to find a partition of the graph such that:

- exactly 3 edges cross between the two groups

Once that happens, the answer is:

- size of group 1
- multiplied by size of group 2

---

## 🧩 Part 1

Find the product of the sizes of the two groups created by cutting the three critical wires.

### 💡 Approach

- Parse each named component into an integer ID
- Build a list of graph edges
- Repeatedly run a random graph contraction process
- Merge components until only 2 groups remain
- Count how many original edges still cross between those 2 groups
- If the crossing edge count is exactly `3`:
  - count the size of each group
  - return their product
- Otherwise:
  - try again with another random contraction run

This is a probabilistic minimum-cut style solution.

---

## 🧩 Part 2

There is no separate computational gold solution in this implementation.

### 💡 Approach

The gold method returns a fixed message:

    Push The Big Red Button Again

So all of the actual puzzle-solving logic is contained in Part 1.

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Snowverload`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Snowverload(this.Input)`
- Calls `BigRedReset()`

For Part 2:

- returns the fixed text:
  
    Push The Big Red Button Again

---

### `Snowverload.cs`

This class contains the parsing logic and the graph cut search.

It stores:

- `Edges`
- `Map`
- `ReverseMap`
- `Random`

Where:

- `Edges` is a list of graph connections as integer pairs
- `Map` converts component names to integer IDs
- `ReverseMap` stores the original component names by ID
- `Random` is used to pick random edges during contraction

The constructor simply calls:

    ParseInput(input)

---

### Parsing the Input

`ParseInput(string[] input)` reads each component line and converts it into graph edges.

At a high level it does:

- split each line on `": "`
- use the left side as the source component name
- split the right side on spaces
- create one edge from the source component to each listed destination

So a line such as:

    jqt: rhn xhk nvd

becomes:

- `(jqt, rhn)`
- `(jqt, xhk)`
- `(jqt, nvd)`

after all names have been mapped to integer IDs.

---

### Name to ID Conversion

`GetId(string name)` assigns a stable integer ID to each unique component name.

It works like this:

- if the name already exists in `Map`
  - return the stored ID
- otherwise:
  - create a new ID using `ReverseMap.Count`
  - store it in `Map`
  - append the name to `ReverseMap`

This means every component string is normalised into a compact numeric graph representation.

---

### Main Solver

`BigRedReset()` performs the random contraction search.

It starts with:

- `n = this.ReverseMap.Count`

Then it loops forever until it finds a valid cut.

For each attempt it creates:

- `UnionFind dsu = new(n)`
- `groups = n`

The solver then repeatedly contracts random edges until only:

- `2`

groups remain.

---

### Random Edge Contraction

While there are more than 2 groups:

- pick a random edge from `Edges`
- inspect its two endpoints
- if they are already in the same union-find group:
  - ignore that edge
- otherwise:
  - union the two groups
  - decrement the group count

So each run randomly merges graph vertices until only two super-nodes remain.

This is the core idea behind Karger's random contraction style of minimum cut search.

---

### Counting the Cut Size

After contraction finishes, the solver checks every original edge.

It counts an edge as crossing the cut when:

- its two endpoints belong to different union-find groups

This is stored in:

    cuts

If:

    cuts == 3

then the desired partition has been found.

---

### Calculating Group Sizes

When a valid 3-edge cut is found, the solver builds:

- `Dictionary sizes = new();`

Then for every component ID:

- find its union-find root
- increment that root's size count

This produces the size of each of the two final groups.

It then computes:

    product *= size

across the group sizes and returns the result as a string.

So the silver answer is the product of the sizes of the two graph partitions formed by the 3-edge cut.

---

### Why the Solver Retries

Because the contraction step is random, a single run may not find the desired cut.

So the method is wrapped in:

    while (true)

This means it keeps performing fresh random contractions until one of them produces exactly 3 crossing edges.

Once that happens, it immediately returns the answer.

---

### `UnionFind`

The nested `UnionFind` class handles the component merging.

It stores:

- `parent`
- `rank`

The constructor initialises every node as its own parent.

`Find(int x)`:

- follows parent links to the root
- applies path compression

`Union(int a, int b)`:

- finds the roots of both sets
- does nothing if they are already the same
- otherwise merges them by rank

This makes repeated connectivity checks and merges efficient during contraction.

---

## 🛠 Implementation Notes

- Component names are mapped to compact integer IDs
- The graph is stored as a list of edge pairs
- The solver uses a randomised contraction algorithm
- Union-find is used to track merged groups efficiently
- Each run contracts until exactly 2 groups remain
- The candidate cut size is measured by counting crossing original edges
- The solver repeats until it finds a cut of size `3`
- Part 2 is not separately implemented and returns a fixed message

---

## 🧪 Behaviour Summary

Given a component wiring graph:

- the solver parses each component and its connections
- all component names are converted to integer IDs
- random edges are repeatedly contracted using union-find
- contraction continues until only 2 groups remain
- the original edge list is checked to count crossing edges
- if exactly 3 edges cross the partition:
  - the sizes of the two groups are counted
  - their product is returned
- otherwise:
  - a new random contraction run begins

---

## 🚀 Key Takeaways

- Good example of solving a graph cut problem with random contraction
- String component names are cleanly normalised into integer node IDs
- Union-find keeps contraction operations efficient
- The solver uses repeated attempts rather than deterministic cut construction
- The final answer comes from partition sizes, not from listing the removed edges
- Part 1 contains the real graph logic, while Part 2 is just a fixed completion message

---

## 🔗 References

- https://adventofcode.com/2023/day/25