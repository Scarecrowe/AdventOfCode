# 🎄 Advent of Code 2022 - Day 16: Proboscidea Volcanium

## 📜 Puzzle Overview

This puzzle models a network of pressure valves connected by tunnels.

Each valve has:

- a two-letter name
- a flow rate
- one or more tunnel connections to other valves

The solver parses the input into `Valve` objects, builds the shortest travel distances between useful valves, and then searches for the best order to open them.

Part 1 finds the best pressure release for a single worker starting at:

    AA

Part 2 reuses the same state search, but combines two non-overlapping valve-opening paths to represent you and the elephant working independently.

---

## 🧩 Part 1

Determine the maximum pressure that can be released by one worker within 30 minutes.

### 💡 Approach

- Parse every valve and its tunnel links
- Compute shortest distances between valves
- Keep only:
  - the start valve `AA`
  - valves with positive flow
- Use recursive search to try all valid valve-opening orders
- Track:
  - remaining time
  - currently opened valves
  - accumulated pressure released
- Return the best total pressure found

---

## 🧩 Part 2

Determine the maximum pressure that can be released by two workers within 26 minutes each.

### 💡 Approach

- Reuse the same recursive state search
- Record the best pressure result for every opened-valve combination
- Compare pairs of recorded states
- Only combine pairs where the opened-valve masks do not overlap
- Return the highest combined pressure total

This effectively treats the two workers as handling disjoint sets of valves.

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Proboscidea Volcanium`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ProboscideaVolcanium(this.Input)`
- Calls `Single()`

For Part 2:

- Creates `new ProboscideaVolcanium(this.Input)`
- Calls `Pair()`

---

### `Valve.cs`

This class models a single valve.

It stores:

- `Name`
- `FlowRate`
- `Tunnels`
- `Paths`
- `Closed`
- `Visited`

The main constructor accepts:

- valve name
- flow rate

and initialises the valve with empty tunnel and path collections.

There is also a copy constructor that clones the valve state fields into a new instance.

The helper methods are:

- `Open()`
- `Visit()`

These flip the `Closed` and `Visited` flags.

---

### `ProboscideaVolcanium.cs`

This class contains the full optimisation logic.

It stores:

- `Valves`
- `Distance`
- `OpenValves`
- `Masks`

The constructor:

- initialises the distance array
- initialises the valve-open mask array
- parses the input
- generates the travel paths used by the solver

It also defines the constant start valve as:

    AA

---

### Parsing the Valve Network

`ParseValves(string[] input)` reads each line of the input and creates a dictionary of valves.

At a high level it does:

- split each line on `"; "`
- parse the valve name
- parse the flow rate
- create a `Valve` object
- add it into a dictionary keyed by valve name

It then makes a second pass through the input to populate the tunnel connections.

This supports both line formats:

- multiple tunnels:

      tunnels lead to valves ...

- single tunnel:

      tunnel leads to valve ...

So after parsing, each valve knows which neighbouring valves it connects to.

---

### Preparing the Search Graph

`GeneratePaths()` prepares the reduced graph used during the optimisation.

It does three key things:

- sorts all valve names
- generates the shortest-distance matrix
- keeps only the valves worth opening

The filtered valve list becomes:

- `AA`
- every valve with `FlowRate > 0`

This means zero-flow valves are kept only where needed for path calculation, then removed from the final search matrix unless the valve is the start node.

---

### Distance Matrix Generation

`GenerateDistances(List<string> valves)` builds the all-pairs shortest path matrix.

It starts by assigning:

- `0` when comparing a valve to itself
- `1` when two valves are directly connected
- `1000` when they are not directly connected

It then refines the matrix by repeatedly checking whether routing through an intermediate valve gives a shorter path.

So the implementation effectively builds the full shortest travel cost between every pair of valves.

After that, the matrix is trimmed to remove valves whose flow rate is `0`, except for `AA`.

---

### Removing Unused Valves

`GenerateIndex(List<string> valves)` identifies which valves should be removed from the final search matrix.

It adds any valve index where:

- `FlowRate == 0`
- and the valve is not `AA`

Those indices are later used to trim the distance matrix down to only the meaningful search nodes.

---

### Bitmask Generation

`GenerateMask()` creates one bitmask per valve index.

It does this logically as:

    1 << i

for each valve position.

These masks are used to track which valves have already been opened.

This is what makes the recursive search and the Part 2 state-pair comparison efficient.

---

### Part 1 Solver

`Single()` creates:

- `Dictionary<int, int> states = new();`

It then starts the recursive search with:

- node `0`
- `30` minutes
- state mask `0`
- current flow total `0`

Finally it returns:

- the maximum recorded pressure total in `states`

So the silver answer is the best pressure release achievable by one path through the valve network.

---

### Part 2 Solver

`Pair()` also creates:

- `Dictionary<int, int> states = new();`

It then runs the same recursive search, but with:

- `26` minutes

Once all reachable states are recorded, it compares every pair of state entries.

For each pair:

- if the bitmasks overlap, skip it
- otherwise, combine the two pressure totals
- keep the maximum combined result

The overlap check is:

    (pairA.Key & pairB.Key) != 0

So the gold answer is the best combined total from two independent, non-conflicting valve-opening routes.

---

### Recursive Search

`Cycle(int node, int minute, int state, int flow, Dictionary<int, int> states)` performs the core search.

At each call it first records the best known pressure for the current bitmask state.

Then it tries every valve in `OpenValves`.

For each candidate valve:

- compute the remaining time after travelling there and opening it
- skip it if:
  - that valve is already open in the bitmask
  - or there is no time left

The remaining time is calculated as:

    next = minute - this.Distance[node, i] - 1

That includes:

- travel time
- 1 extra minute to open the valve

If the move is valid, the solver recurses with:

- the new valve as the current node
- the reduced remaining time
- the updated opened-valve bitmask
- the increased pressure total

The pressure increase is:

    next * this.Valves[this.OpenValves[i]].FlowRate

So opening a valve earlier is more valuable, because its flow contributes for more remaining minutes.

---

### State Recording

A very important detail is that the solver stores the best pressure seen for each opened-valve bitmask.

That means:

- multiple different traversal orders can collapse to the same logical opened-valve state
- only the best score for that state is kept

This is what makes Part 2 possible, since the solver can later combine two best-performing disjoint states rather than having to simulate both workers together.

---

## 🛠 Implementation Notes

- The entry class is `Day16`
- The main solver class is `ProboscideaVolcanium`
- The start valve is hardcoded as `AA`
- Input parsing is split into:
  - valve creation
  - tunnel linking
- Shortest paths are precomputed before the recursive search
- Zero-flow valves are removed from the final optimisation graph unless the valve is `AA`
- Opened valves are tracked using integer bitmasks
- Part 1 uses 30 minutes
- Part 2 uses 26 minutes and combines non-overlapping states

---

## 🧪 Behaviour Summary

Given a network of valves and tunnels:

- the solver parses every valve and its flow rate
- it builds the tunnel graph
- it computes shortest travel distances between valves
- it trims the graph to only the useful valves plus the start
- it recursively explores valve-opening orders
- each opened-valve set is stored as a bitmask state
- Part 1 returns the best single-route pressure release
- Part 2 returns the best combined pressure from two disjoint routes

---

## 🚀 Key Takeaways

- Good example of reducing a graph before performing an expensive search
- Uses all-pairs shortest paths to avoid repeated route traversal work
- Uses bitmasks to represent opened-valve states compactly
- Part 2 avoids full dual-agent simulation by combining cached disjoint states
- The recursive search rewards opening high-value valves as early as possible

---

## 🔗 References

- https://adventofcode.com/2022/day/16