# 🎄 Advent of Code 2019 - Day 06: Universal Orbit Map

## 📜 Puzzle Overview

This puzzle builds a map of objects in space based on orbit relationships.

Each input line looks like this:

    AAA)BBB

This means:

- `BBB` directly orbits `AAA`

The full input describes a tree of orbiting objects rooted at `COM` (Center of Mass).

The solver builds that orbit hierarchy and then answers two questions:

- Part 1 counts all direct and indirect orbits
- Part 2 finds the minimum number of orbital transfers needed to move from `YOU` to `SAN`

---

## 🧩 Part 1

Determine the total number of direct and indirect orbits in the map.

### 💡 Approach

- Build the orbit tree starting from `COM`
- Link each object to its parent
- Store each object's children
- For every object:
  - count how many ancestors it has
- Sum those counts across the whole tree

This gives the total number of direct and indirect orbits.

---

## 🧩 Part 2

Determine the minimum number of orbital transfers required to move from the object `YOU` are orbiting to the object `SAN` is orbiting.

### 💡 Approach

- Build the same orbit tree from the input
- Find the `YOU` node in the tree
- Start from `YOU`'s parent
- Move upward through parent objects
- At each level, search downward to see whether `SAN` exists in that branch
- Count how many transfers are required to reach the shared path
- Subtract the final adjustment so the transfer count is between the objects orbited by `YOU` and `SAN`, not including `YOU` and `SAN` themselves

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Universal Orbit Map`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new UniversalOrbitMap(this.Input)`
- Calls `DirectInDirectCount()`

For Part 2:

- Creates `new UniversalOrbitMap(this.Input)`
- Calls `MinimumOrbitTransfer()`

---

### `OrbitObject.cs`

This class models a single object in the orbit map.

It stores:

- `Name`
- `Parent`
- `Children`

The constructors allow creation of:

- a root object with just a name
- a child object with both a name and parent reference

So each orbit object knows:

- what it is called
- what it directly orbits
- what directly orbits it

---

### Parent and Child Relationships

The map is represented as a tree structure.

Each `OrbitObject` can have:

- one `Parent`
- many `Children`

This makes it easy to:

- walk upward toward `COM`
- walk downward through orbit branches
- recursively search the map

---

### Counting Orbits

`Orbits()` calculates the total orbit count contributed by a node and its descendants.

It works in two stages:

- walk upward through `Parent` references and count ancestors
- recursively call `Orbits()` on all children and add their totals

Logically, that means:

    current object's orbit count
    + all child subtree orbit counts

This is how the full direct and indirect orbit total is produced for Part 1.

---

### Finding Objects

`Find(string name)` recursively searches the tree.

It:

- returns the current object if the name matches
- otherwise searches each child subtree
- returns the first matching object found
- returns `null` if nothing matches

This is used in Part 2 to locate `YOU` and later to test whether `SAN` exists in a branch.

---

### `UniversalOrbitMap.cs`

This class contains the map-building and puzzle-solving logic.

It stores:

- `Input`

The constructor clones the input array so the working copy can be modified safely during processing.

---

### Building the Orbit Tree

`Process(string key, ref OrbitObject? current)` builds the orbit map recursively.

At a high level it does:

- create the current node if needed
- scan all remaining input lines
- split each line on `")"`
- collect every orbit where the left side matches the current key
- create child objects for those matches
- attach them to the current node
- blank out processed input entries
- recursively process each child

So starting from `COM`, the entire orbit tree is expanded downward.

---

### Input Parsing

Each orbit line is split like this:

    AAA)BBB

becomes:

- `AAA` = parent object
- `BBB` = child object

If the parent matches the current key being processed, the solver creates a new child node and links it into the tree.

---

### Part 1 Solver

`DirectInDirectCount()` does the following:

- creates `com`
- calls `Process("COM", ref com)`
- returns:

    com?.Orbits() ?? 0

So the silver answer is the full recursive orbit count for the tree rooted at `COM`.

---

### Part 2 Solver

`MinimumOrbitTransfer()` also rebuilds the tree from `COM`.

It then:

- finds the `YOU` node
- starts from `YOU`'s parent
- calls:

    TravelTo("SAN")

- subtracts `2` from the result

The subtraction adjusts the route so it counts transfers between the objects orbited by `YOU` and `SAN`, rather than including those labels themselves.

---

### Transfer Logic

`TravelTo(string name)` handles the route search.

It:

- begins from the current object's parent
- repeatedly moves upward through ancestors
- at each ancestor:
  - checks each child subtree
  - uses `Find(name)` to see whether the destination exists there
- when the destination branch is found:
  - adds the downward distance using `OrbitsTo(current.Name)`
  - stops the search

This gives a combined upward-and-downward transfer path.

---

### Distance to an Ancestor

`OrbitsTo(string name)` walks upward from the current object until it reaches the named ancestor.

It increments a counter for each step.

This is used during transfer calculation to determine how far down the destination node is from the common ancestor branch currently being tested.

---

## 🛠 Implementation Notes

- The orbit map is built as a recursive tree rooted at `COM`
- Each object stores both `Parent` and `Children`
- The input array is cloned before processing
- Processed orbit lines are replaced with `string.Empty`
- Part 1 sums direct and indirect orbits recursively
- Part 2 finds `YOU`, then searches for `SAN` through parent traversal and subtree checks
- The final transfer value is adjusted with `- 2`

---

## 🧪 Behaviour Summary

Given a list of orbit relationships:

- the solver starts from `COM`
- recursively builds a full parent-child orbit tree
- Part 1 counts how many ancestors every object has
- Part 2 finds the orbital route between the objects connected to `YOU` and `SAN`
- the same core tree structure supports both puzzle parts

---

## 🚀 Key Takeaways

- Clean example of modelling hierarchical orbit data as a tree
- Parent references make upward traversal simple
- Child collections make recursive searches straightforward
- Part 1 is a recursive accumulation problem
- Part 2 combines ancestor traversal with subtree searching
- Reusing the same orbit map structure keeps both solutions compact

---

## 🔗 References

- https://adventofcode.com/2019/day/6