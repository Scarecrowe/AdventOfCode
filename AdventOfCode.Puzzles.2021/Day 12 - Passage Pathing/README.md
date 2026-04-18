# 🎄 Advent of Code 2021 - Day 12: Passage Pathing

## 📜 Puzzle Overview

This puzzle explores all valid paths through a cave system.

Each input line defines a bidirectional link between two caves:

    start-A
    A-b
    b-end

Caves are identified by name:

- uppercase cave names are large caves and can be revisited freely
- lowercase cave names are small caves and have visit limits

The solver parses each input line into a `Link` object and then builds a traversal tree of possible routes starting from `start`.

Part 1 counts unique valid paths where small caves can only be visited once.  
Part 2 allows one chosen small cave to be visited twice, while all other small caves still follow the normal rule.

---

## 🧩 Part 1

Determine how many unique paths lead from `start` to `end`.

### 💡 Approach

- Parse every cave connection into a bidirectional link
- Find all small caves in the graph
- Exclude:
  - `start`
  - `end`
- For each remaining small cave:
  - build the traversal tree using normal visit rules
  - collect all distinct paths that reach `end`
- Keep a shared list of unique completed path strings
- Return the total number of unique paths discovered

Although Part 1 does not allow double visits, the implementation still loops over the list of small caves and rebuilds the tree each time, relying on path deduplication to avoid counting duplicates.

---

## 🧩 Part 2

Determine how many unique paths lead from `start` to `end` when one small cave may be visited twice.

### 💡 Approach

- Reuse the same parsing and traversal logic
- Identify all small caves except `start` and `end`
- Treat each small cave in turn as the candidate cave allowed a second visit
- Rebuild the full traversal tree for that candidate
- Collect completed path strings
- Deduplicate paths across all candidate runs
- Return the total number of unique valid paths

So the gold solution works by trying every possible eligible small cave as the special double-visit cave.

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Passage Pathing`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new PassagePathing(this.Input)`
- calls `UniquePathCount()`

For Part 2:

- creates `new PassagePathing(this.Input)`
- calls `UniquePathCount(true)`

The gold method is also marked with:

    [Slow]

which fits the implementation's repeated tree-building strategy.

---

### `Link.cs`

This class models a connection between two caves.

It stores:

- `A`
- `B`

The constructor takes two cave names and stores them directly.

It also provides:

    Not(string value)

This returns the opposite end of the link.

So if the link is:

    A-b

then:

- `Not("A")` returns `b`
- `Not("b")` returns `A`

This is how the traversal logic moves from one cave to its connected neighbour.

---

### `Node.cs`

This class models one node in the traversal tree.

It stores:

- `Value`
- `Parent`
- `Children`

The constructor:

- stores the cave name in `Value`
- stores the parent node
- initialises `Children`

The class provides:

- `AddChild(string value)`
- `AddChildren(string[] values)`

`AddChild(...)` creates a new child node, adds it to the current node's children, and returns it.

This allows the traversal to grow a full tree of possible routes from `start`.

---

### `PassagePathing.cs`

This class contains the main parsing, tree-building, and path-counting logic.

It stores:

- `Links`
- `Tree`

The constructor parses the input immediately:

- `this.Links = ParseInput(input)`

So the traversal logic always works from a parsed list of `Link` objects.

---

### Parsing the Cave Graph

`ParseInput(string[] input)` reads each puzzle line and splits it on `"-"`.

At a high level it does:

- read each line
- split into two cave names
- create `new Link(tokens[0], tokens[1])`
- collect the results into a list

This produces a flat list of bidirectional cave connections.

---

### Finding Small Caves

`GetSmallCaves()` scans every link and collects cave names that are not fully uppercase.

Uppercase detection uses:

    IsAllUpper(string input)

which checks every character and returns `true` only if they are all uppercase.

So any cave name that is not entirely uppercase is treated as a small cave.

Later, `UniquePathCount(...)` removes:

- `start`
- `end`

from that small-cave list before testing possible traversal rules.

---

### Counting Parent Visits

`HasParent(Node node, string value)` walks up the current node's parent chain and counts how many times a given cave name has already appeared.

At a high level it does:

- compare the current node value
- move to `Parent`
- continue until the root is reached

This is the mechanism used to enforce visit limits for small caves.

---

### Building the Traversal Tree

`BuildTree(string multipleCave = "", bool multipleVisits = false)` clears the current tree and starts recursion from:

    "start"

It calls:

    BuildTreeRecursive(this.Tree, "start", multipleCave, multipleVisits)

If the current node is null, the recursive method creates the root node:

    new Node("start", null)

and stores it in `Tree`.

From there it finds all connected links for the current cave and expands child nodes for each possible join.

---

### Expanding Neighbours

`GetLinks(string value)` returns every link whose `A` or `B` matches the current cave.

Then `BuildTreeRecursive(...)` converts those links into adjacent cave names with:

    nodes.Select(x => x.Not(value)).ToArray()

So from a cave such as `A`, the solver finds every connected cave and attempts to descend into each one.

---

### Visit Rules for Small Caves

Before descending into a neighbouring cave, the solver checks whether that cave is a small cave and whether it has already been visited too many times.

It calculates:

- `searchCount = 1` when:
  - multiple visits are enabled
  - a special `multipleCave` has been chosen
  - the candidate join matches that cave
- otherwise `searchCount = 0`

Then it checks:

- if the cave is small
- and `HasParent(current, join) > searchCount`

then that branch is skipped.

This means:

- normal small caves may appear only once in the current path
- the chosen special small cave may appear twice when Part 2 mode is enabled
- large caves are never blocked by this rule

---

### Collecting Unique Paths

`UniquePaths(List<string> unique)` starts a traversal from the built tree and counts completed routes.

It calls:

    TraverseUniquePaths(this.Tree, unique, string.Empty, ref total)

`TraverseUniquePaths(...)` builds a path string by appending:

    current.Value + "-"

As recursion continues, this creates a full textual representation of the route, such as:

    start-A-b-end-

When the current node reaches:

    "end"

the solver checks whether that exact path string is already in the shared `unique` list.

If not:

- add the path string to `unique`
- increment the total

This is how duplicate paths from repeated tree builds are filtered out.

---

### How `UniquePathCount(...)` Works

`UniquePathCount(bool multipleVisits = false)` drives the whole solution.

It:

- gets all small caves
- removes `start` and `end`
- creates a shared `List<string> unique = new();`
- loops over each remaining small cave
- builds the tree using that cave as the optional double-visit candidate
- adds the number of newly discovered unique paths

For Part 1:

- `multipleVisits` is `false`
- so the chosen cave name does not actually get special treatment
- repeated runs still happen, but duplicates are removed by the shared unique-path list

For Part 2:

- `multipleVisits` is `true`
- so each small cave is tested in turn as the one allowed a second visit

The returned total is the number of distinct valid `start` to `end` routes.

---

## 🛠 Implementation Notes

- The graph is stored as a flat `List<Link>`
- Traversal paths are represented as a tree of `Node` objects
- Parent links are used to count prior visits to caves
- Large caves are detected by checking whether the full cave name is uppercase
- Small caves are gathered first, then `start` and `end` are excluded from special handling
- Paths are deduplicated by storing their full text representation
- Part 2 is slower because it rebuilds the traversal tree once per eligible small cave

---

## 🧪 Behaviour Summary

Given a list of cave connections:

- the solver parses them into `Link` objects
- identifies all eligible small caves
- repeatedly builds a traversal tree from `start`
- walks each tree to produce full route strings
- counts only unique routes that end at `end`

Part 1:

- uses normal small-cave rules

Part 2:

- allows one selected small cave to be visited twice
- tests each possible selected cave separately
- merges the results through path deduplication

So the final result is the total number of distinct valid paths through the cave system.

---

## 🚀 Key Takeaways

- Good example of representing route exploration as an explicit tree
- `Link` keeps cave connections simple and bidirectional
- `Node` stores parent-child traversal structure for path building
- Small-cave visit checks are handled by walking back up the parent chain
- Part 2 is implemented by trying every eligible small cave as the special case
- Duplicate completed paths are removed by storing full path strings

---

## 🔗 References

- https://adventofcode.com/2021/day/12