# 🎄 Advent of Code 2021 - Day 18: Snailfish

## 📜 Puzzle Overview

This puzzle works with **snailfish numbers**, which are nested pairs.

A snailfish number can contain:

- regular numbers
- other snailfish pairs

Examples:

    [1,2]
    [[1,2],3]
    [[[[1,1],[2,2]],[3,3]],[4,4]]

The solver parses each input line into a linked `SnailFish` tree structure.

Two main operations drive the puzzle:

- **addition**
- **reduction**

Reduction repeatedly applies:

- **explode**
- **split**

until the snailfish number is stable.

Part 1 adds all input numbers together in sequence and returns the final magnitude.  
Part 2 tests every ordered pair of input lines and returns the largest magnitude produced by any addition.

The puzzle entry returns:

    new SnailFishMaths(this.Input).FinalMagnitude()
    new SnailFishMaths(this.Input).LargestMagnitude()

---

## 🧩 Part 1

Determine the magnitude of the final snailfish sum.

### 💡 Approach

- Parse every input line into a snailfish tree
- Add the first two numbers together
- Reduce the result
- Continue adding each remaining number in sequence
- Reduce after each addition
- Compute the magnitude of the final reduced number

---

## 🧩 Part 2

Determine the largest magnitude produced by adding any two different input numbers.

### 💡 Approach

- Try every ordered pair of input lines
- Parse both numbers fresh each time
- Add them together
- Reduce the result
- Compute its magnitude
- Track the largest magnitude found

This checks both directions of addition, which matters because snailfish addition is not commutative.

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Snailfish`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new SnailFishMaths(this.Input)`
- calls `FinalMagnitude()`

For Part 2:

- creates `new SnailFishMaths(this.Input)`
- calls `LargestMagnitude()`

---

### `SnailFish.cs`

This class models a single snailfish node.

It stores:

- `RegularNumber`
- `Parent`
- `A`
- `B`

A node can therefore be either:

- a regular number
- or a pair containing left child `A` and right child `B`

The constructors support both cases:

- a single regular value with parent reference
- or a pair with left and right child fish

When pair children are assigned, their `Parent` references are also updated.

---

### Tree Structure Helpers

The class provides helpers that describe the current node shape:

- `IsRegularNumber()`
- `HasA()`
- `HasB()`
- `IsPair()`
- `IsSplitA()`
- `IsSplitB()`

So the reduction logic can quickly detect:

- whether a node is a plain number
- whether it is a pair of regular numbers
- whether either child needs splitting

---

### Addition

`Addition()` performs snailfish reduction on the current number after two numbers have been combined under a new parent pair.

At a high level:

- wrap two existing fish in a new root pair
- repeatedly reduce that structure
- stop only when no more explosions or splits are possible

This means the actual "addition" result is the reduced combined tree, not just the raw pair.

---

### Exploding

`Explode()` delegates to the internal explode logic for the current fish.

When a deeply nested pair must explode, the implementation:

- finds the nearest regular number on the left
- adds the exploding pair's left value to it
- finds the nearest regular number on the right
- adds the exploding pair's right value to it
- replaces the exploding pair with a regular number `0`

That final replacement is handled by:

    ToRegularNumber()

which clears child nodes and sets the current node value to `0`.

---

### Searching Left and Right

The explode logic uses neighbour-search helpers:

- `SearchLeft(...)`
- `SearchRight(...)`

These walk upward through parent links and then back down the opposite side to find the closest regular number in each direction.

That is how exploded values are propagated to the nearest valid neighbours instead of just to adjacent tree nodes.

---

### Splitting

`Split()` delegates to the internal split logic.

When a regular number reaches `10` or more:

- it is replaced with a pair
- the left side gets `floor(value / 2)`
- the right side gets `ceil(value / 2)`

The split checks are exposed through:

- `IsSplitA()`
- `IsSplitB()`

So the reduction process can detect when either side must be expanded into a new pair.

---

### Magnitude

`Magnitude()` returns the score of the current snailfish number.

For a simple pair of regular numbers it uses:

    (3 * left) + (2 * right)

For nested pairs it evaluates both sides recursively and then applies the same rule.

So magnitude is always built from:

- 3 times the left side
- 2 times the right side

all the way down the tree.

---

### String Rendering

`ToString()` rebuilds the snailfish number as its bracketed text form.

It writes:

- `[`
- left side
- `,`
- right side
- `]`

This is useful both for debugging and for visualising the reduced tree state.

---

### `SnailFishMaths.cs`

This class handles parsing, accumulation, and magnitude calculations across the full puzzle input.

It stores:

- `Input`
- `AllFish`

The constructor:

- stores the raw input lines
- parses them into snailfish objects with `Parse(...)`

So the class starts with a tree representation of every input snailfish number.

---

### Parsing Snailfish Numbers

`Parse(string[] input)` builds a `List<SnailFish>`.

For each input line it:

- creates a root snailfish node
- calls `ParseFish(...)`
- stores the parsed result

`ParseFish(...)` walks through the input characters recursively.

It reacts to:

- `[` by descending into a new pair
- digits by creating regular-number nodes
- `,` by switching to the right side
- `]` by moving back to the parent

So each text line becomes a linked tree of parent and child fish nodes.

---

### Part 1: Final Magnitude

`FinalMagnitude()` adds all parsed snailfish numbers from left to right.

It starts with:

    new SnailFish(null, null, this.AllFish[0], this.AllFish[1]).Addition()

Then for every remaining input fish it does:

- keep the current reduced sum
- wrap that sum with the next fish in a new root pair
- reduce again via `Addition()`

At the end it returns:

    addition?.Parent?.Magnitude() ?? 0

So the silver answer is the magnitude of the fully accumulated, fully reduced snailfish sum.

---

### Part 2: Largest Pair Magnitude

`LargestMagnitude()` checks every ordered pair of distinct input lines.

For each pair `i` and `j` where `i != j` it:

- reparses both lines into fresh snailfish trees
- combines them under a new pair
- reduces with `Addition()`
- computes `Magnitude()`
- stores that result

Finally it returns the maximum magnitude found.

This ensures each test uses a clean parse rather than mutated objects from earlier additions.

---

## 🛠 Implementation Notes

- Snailfish numbers are stored as linked tree nodes
- Each node keeps a `Parent` reference
- Pair nodes store `A` and `B`
- Regular-number nodes store `RegularNumber`
- Explosion uses neighbour searches to the left and right
- Splitting replaces a large regular number with a new pair
- Magnitude is computed recursively
- Part 2 reparses raw input strings for each ordered pair combination

---

## 🧪 Behaviour Summary

Given a list of snailfish numbers:

- the solver parses each one into a tree
- combines numbers by wrapping them in a new pair
- reduces with repeated explode and split operations
- computes the final magnitude of the result

Part 1:

- adds all numbers in order
- returns the final magnitude

Part 2:

- tries every ordered pair of different inputs
- reduces each addition result
- returns the largest magnitude produced

---

## 🚀 Key Takeaways

- Good example of modelling nested structured data with parent-linked trees
- `SnailFish` encapsulates both regular values and pair nodes
- Explosion relies on searching for the nearest regular neighbour in each direction
- Splitting transforms large regular numbers into new pairs
- Part 1 is a repeated add-and-reduce pipeline
- Part 2 brute-forces every ordered pair and keeps the maximum magnitude

---

## 🔗 References

- https://adventofcode.com/2021/day/18