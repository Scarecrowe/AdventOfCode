# 🎄 Advent of Code 2019 - Day 14: Space Stoichiometry

## 📜 Puzzle Overview

This puzzle models a set of chemical reactions.

Each input line describes how some input chemicals can be transformed into an output chemical.

A reaction line looks like this:

    7 A, 1 E => 1 FUEL

This means:

- some source chemicals are consumed
- one product chemical is produced
- each reaction has a fixed output quantity

The solver builds a reaction graph from the input and then answers two questions:

- Part 1 calculates how much `ORE` is needed to make `1 FUEL`
- Part 2 finds how much `FUEL` can be produced from `1000000000000` units of `ORE`

---

## 🧩 Part 1

Determine the amount of `ORE` required to produce `1 FUEL`.

### 💡 Approach

- Parse all reactions into a lookup by chemical name
- Track what each reaction depends on
- Track which products each chemical contributes to
- Topologically order the reaction graph
- Start with a demand for `FUEL`
- Expand that demand backward through its dependencies
- For each chemical:
  - determine how many batches must be produced
  - add the required quantities of its source chemicals
- Continue until the required amount of `ORE` is known

---

## 🧩 Part 2

Determine the maximum amount of `FUEL` that can be produced from:

    1000000000000

units of `ORE`.

### 💡 Approach

- Reuse the Part 1 ore calculator
- First estimate a lower production bound from the ore required for `1 FUEL`
- Set a much larger upper bound
- Use binary search between those bounds
- For each midpoint:
  - calculate required ore for that fuel amount
- Narrow the search until the highest affordable fuel amount is found

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Space Stoichiometry`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SpaceStoichiometry(this.Input)`
- Calls `GetRequiredOre()`

For Part 2:

- Creates `new SpaceStoichiometry(this.Input)`
- Calls `GetTotalOre()`

---

### `Reaction.cs`

This class models a single reaction.

It stores:

- `Name`
- `Output`
- input dependencies
- product links

The constructor:

- initialises the reaction name
- sets the output quantity
- creates dictionaries for:
  - source inputs
  - products that depend on this reaction

It provides methods to add:

- source chemicals with `AddSource(string name, long quantity)`
- dependent products with `AddProduct(string name, long quantity)`

It also exposes:

- `GetDependencies()`
- `GetProducts()`

So each reaction knows both:

- what it requires
- what depends on it

---

### `Ore.cs`

This class is a special case reaction.

It inherits from `Reaction` and simply creates:

- `new Reaction("Ore")`

This provides a base node for the reaction system.

---

### Reaction Parsing

`SpaceStoichiometry` parses every line of input in the constructor.

For each line it:

- splits on `"=>"`
- reads the output quantity and output chemical
- creates or updates the matching `Reaction`
- splits the left side on `","`
- adds each source chemical as a dependency
- ensures each source chemical also exists in the global reaction dictionary
- records that the source contributes to the current product

So an input like:

    7 A, 1 E => 1 FUEL

becomes:

- reaction `FUEL`
- output quantity `1`
- dependencies:
  - `A = 7`
  - `E = 1`

---

### `SpaceStoichiometry.cs`

This class contains the main puzzle logic.

It stores:

- `Reaction`

The constructor builds a full reaction dictionary keyed by chemical name.

It starts with:

- `ORE`

already present in the dictionary.

Then it processes every formula line into linked reaction objects.

---

### Topological Ordering

`GetRequiredOre(long fuelTarget = 1)` begins by building a topological order:

- `new Topological(this.Reaction).GetOrdered()`

This ensures chemicals are processed in an order that respects dependency flow.

The method then creates a quantity map and starts with:

    ["FUEL"] = fuelTarget

That means the whole calculation works backward from the requested amount of fuel.

---

### `Topological.cs`

This class builds an ordered traversal of the reaction graph.

It stores:

- `DepthFirstOrder`
- `Marked`

The constructor:

- loops over every reaction key
- runs a depth-first search for unvisited nodes

`DepthFirstSearch(...)`:

- marks the current chemical
- recursively visits every product reachable from it
- appends the current chemical to the ordered list

This produces the processing order used when expanding required quantities.

---

### Ore Calculation

For each item in topological order, `GetRequiredOre(...)` does the following:

- read the reaction output quantity
- read how much of that chemical is needed
- calculate how many batches must be made using ceiling division:

      toMake = ceil(needed / output)

- for each dependency:
  - add `dependency.Value * toMake` to the required quantity map

Logically this means:

- if you need more than one batch worth of a chemical
- the reaction is repeated enough times to cover the demand
- all source requirements are scaled accordingly

When the expansion completes, the result is:

- `quantity["ORE"]`

---

### Part 1 Return Value

When called with the default target, `GetRequiredOre()` returns:

- the total ore needed for `1 FUEL`

So the silver answer is the fully expanded `ORE` requirement after processing the dependency graph.

---

### Part 2 Fuel Search

`GetTotalOre()` solves the inverse problem.

It first calculates:

- `requiredOre = this.GetRequiredOre()`

Then it defines:

- `target = 1000000000000`

It creates a lower and upper search range based on the ore cost of one fuel, then repeatedly performs binary search.

For each midpoint:

- call `GetRequiredOre(mid)`

Then:

- if the guess is too high:
  - move the upper bound down
- if the guess is affordable:
  - move the lower bound up
- if the guess matches exactly:
  - stop

Finally it returns:

- `lower`

So the gold answer is the highest fuel amount found that does not exceed the ore target.

---

## 🛠 Implementation Notes

- Reactions are stored in a dictionary keyed by chemical name
- Each reaction tracks both dependencies and dependent products
- `ORE` is inserted as a base reaction before parsing input
- Required quantities are expanded from `FUEL` backward through the graph
- Batch counts use ceiling division so partial reaction runs are rounded up
- Part 2 reuses the Part 1 ore calculator
- The maximum fuel result is found with binary search

---

## 🧪 Behaviour Summary

Given a set of chemical formulas:

- the solver parses them into linked reaction objects
- each reaction knows what it needs and what it produces toward
- the graph is topologically ordered
- Part 1 expands a demand of `1 FUEL` backward into raw `ORE`
- Part 2 repeatedly tests larger fuel values against the ore limit
- the final answers are:
  - ore required for one fuel
  - maximum fuel possible from one trillion ore

---

## 🚀 Key Takeaways

- Good example of modelling crafting rules as a dependency graph
- The reaction graph stores both inputs and reverse product links
- Topological ordering keeps quantity expansion clean
- Part 1 is a backward resource expansion problem
- Part 2 turns the same calculator into a search problem
- Binary search makes the trillion-ore target practical to solve efficiently

---

## 🔗 References

- https://adventofcode.com/2019/day/14