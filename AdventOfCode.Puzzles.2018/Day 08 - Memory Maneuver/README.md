# 🎄 Advent of Code 2018 - Day 08: Memory Maneuver

## 📜 Puzzle Overview

This puzzle works with a flat sequence of integers that encodes a tree structure.

Each node begins with a header containing:

- number of child nodes
- number of metadata entries

After that:

- all child nodes appear
- then the node's metadata entries appear

A sample encoded sequence looks like this:

    2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2

The solver parses this number stream into a tree of `MemoryNode` objects and then uses that tree for both puzzle parts.

---

## 🧩 Part 1

Find the sum of all metadata entries in the tree.

### 💡 Approach

- Parse the input into integer values
- Build the full tree node-by-node
- As metadata entries are read, accumulate their total
- Return the final metadata sum

---

## 🧩 Part 2

Calculate the value of the root node using the puzzle's special node-value rules.

### 💡 Approach

- Reuse the same parsed tree
- If a node has no children:
  - its value is the sum of its metadata
- If a node has children:
  - each metadata entry is treated as a 1-based child index
  - only referenced children contribute to the node value
- Recursively compute the value starting from the root node

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Memory Maneuver`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MemoryManeuver(this.Input[0])`
- Calls `BuildTree()`
- Returns `MetadataSum`

For Part 2:

- Creates `new MemoryManeuver(this.Input[0])`
- Calls `BuildTree()`
- Calls `RootValue()`

---

### `MemoryManeuver.cs`

This class contains the parsing and evaluation logic.

It stores:

- `Values`
- `Tree`
- `MetadataSum`

The constructor takes the single input string and converts it into an integer array by splitting on spaces.

So the raw input becomes a sequence like:

    2 3 0 3 10 11 12 ...

which is then consumed while constructing the tree.

---

### `MemoryNode.cs`

This class models a node in the memory tree.

Each node stores:

- `Letter`
- `Parent`
- `Children`
- `Metadata`
- `ChildrenCount`
- `MetadataCount`

The constructor receives:

- parent node
- display letter
- expected child count
- expected metadata count

Children are stored in a dictionary keyed by insertion order, and metadata entries are stored in a list.

---

### `MemoryParseType.cs`

This enum controls which kind of data the parser expects next.

It has two values:

- `Header`
- `MetaData`

The parser switches between these modes while walking the integer input stream.

---

### Building the Tree

`BuildTree()` constructs the tree from the flat integer array.

It begins by:

- creating the root node from the first two values
- assigning it the letter `A`

Then it tracks:

- `parser`
- `index`
- `metaIndex`
- `current`

At a high level:

- if the parser is in `Header` mode:
  - create a new child node from the next two numbers
  - move `current` down into that child
- if the parser is in `MetaData` mode:
  - read a metadata value
  - add it to the current node
  - add it to `MetadataSum`

This continues until the full integer stream has been consumed.

---

### Header Parsing

When the parser expects a header, it reads:

    childCount metadataCount

and creates a new node with those values.

That node is added using:

    current.AddNode(...)

The parser then decides what comes next:

- if the new node has children:
  - stay in `Header` mode
- otherwise:
  - switch to `MetaData` mode

The input index advances by 2 because a node header always contains two integers.

---

### Metadata Parsing

When the parser expects metadata, it:

- adds the current value to `MetadataSum`
- stores that value in `current.Metadata`
- advances the input index
- increments `metaIndex`

Once enough metadata values have been read for the current node:

- move back to the parent node
- determine whether the parent still needs more children or is ready for metadata
- reset `metaIndex`

If the parser moves above the root, the build is complete.

---

### Tree Navigation

The parser keeps track of where it is using the `current` node reference and each node's `Parent`.

This allows it to:

- descend into newly created child nodes
- return upward once a node's metadata is complete

So the flat input is effectively converted into a proper linked tree structure during a single pass.

---

### `AddNode()`

`MemoryNode.AddNode(MemoryNode node)` inserts a child into the current node's `Children` dictionary.

It uses:

    this.Children.Count

as the key, so children are stored in zero-based insertion order.

That matters later in Part 2, because metadata entries are interpreted as 1-based references and the code converts them with:

    index - 1

before checking the child dictionary.

---

### Part 1 Metadata Sum

Part 1 does not need any additional traversal after parsing.

While building the tree, every metadata value is immediately added to:

- `MetadataSum`

So once `BuildTree()` finishes, the silver answer is already available.

---

### Root Node Value

`RootValue(MemoryNode? current = null)` calculates the Part 2 answer recursively.

If no node is supplied:

- it starts from `this.Tree`

If the current node has no children:

- return the sum of its metadata entries

If the current node has children:

- iterate through its metadata entries
- treat each entry as a 1-based child reference
- look up the matching child
- recursively add that child's value

Metadata entries that do not point to a valid child are ignored.

---

### Part 2 Child Referencing

Because children are stored zero-based but metadata uses one-based indexing, the lookup is performed logically like this:

    childIndex = metadataValue - 1

If that child exists:

- recurse into it
- add its computed value to the result

This matches the puzzle rule that parent metadata acts like references rather than raw values when children are present.

---

## 🛠 Implementation Notes

- Input is read as a single space-separated string
- Tree construction happens in one pass through the integer array
- Nodes store both parent and child relationships
- `MetadataSum` is accumulated during parsing
- Child nodes are indexed by insertion order
- Part 2 uses recursion to evaluate node values
- Invalid child references are safely ignored

---

## 🧪 Behaviour Summary

Given an encoded sequence of integers:

- the solver reads the header for each node
- builds a linked tree structure
- stores metadata on each node
- sums all metadata for Part 1 during parsing
- recursively evaluates the root node for Part 2

The final result is either:

- the total metadata sum
- or the computed value of the root node

---

## 🚀 Key Takeaways

- Good example of turning a flat integer stream into a tree structure
- Parsing is state-driven using header and metadata modes
- Parent links make upward traversal simple
- Part 1 is solved during parse time with no extra pass required
- Part 2 cleanly applies recursion over the built tree
- Child reference handling is neatly mapped from 1-based metadata to zero-based storage

---

## 🔗 References

- https://adventofcode.com/2018/day/8