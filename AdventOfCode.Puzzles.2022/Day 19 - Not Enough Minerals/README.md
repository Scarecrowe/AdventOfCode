# 🎄 Advent of Code 2022 - Day 19: Not Enough Minerals

## 📜 Puzzle Overview

This puzzle is about managing robot production to maximise geodes within a time limit.

Each blueprint defines the mineral cost for building four robot types:

- ore robot
- clay robot
- obsidian robot
- geode robot

Each robot produces one mineral of its own type per minute.

The solver parses each blueprint into cost dictionaries, then explores possible build paths over time.

Part 1 evaluates every blueprint over `24` minutes and sums their quality levels. Part 2 evaluates the first three blueprints over `32` minutes and multiplies their best geode results.

---

## 🧩 Part 1

Determine the total quality level across all blueprints after 24 minutes.

### 💡 Approach

- Parse each blueprint into robot build-cost dictionaries
- For each blueprint:
  - start with one ore robot
  - simulate possible choices minute by minute
  - track minerals, robots, and time remaining
- Use a queue of states to explore build paths
- Keep the highest geode count found for that blueprint
- Multiply each blueprint result by its 1-based blueprint index
- Sum the values

---

## 🧩 Part 2

Determine the product of the best geode totals from the first three blueprints after 32 minutes.

### 💡 Approach

- Reuse the same blueprint search logic
- Only evaluate the first three blueprints
- Run each one with `32` minutes instead of `24`
- Multiply the three best geode counts together

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Not Enough Minerals`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new GeodeCracker(this.Input)`
- Calls `Run24()`

For Part 2:

- Creates `new GeodeCracker(this.Input)`
- Calls `Run32()`

Both puzzle parts are marked with `[Slow]`.

---

### `MineralType.cs`

This enum defines the four mineral and robot categories.

It contains:

- `Ore`
- `Clay`
- `Obsidian`
- `Geode`

This enum is used as the key type throughout the solver when storing robot counts, mineral counts, and build costs.

---

### `BluePrint.cs`

This class parses and stores the robot cost rules for a single blueprint.

It stores:

- `Ore`
- `Clay`
- `Obsidian`
- `Geode`

Each of those is a dictionary mapping required mineral type to required quantity.

The constructor:

- splits the blueprint after `": "`
- splits the robot rules on `.`
- parses each robot description
- builds the correct cost dictionary for each robot type

So a blueprint line is converted into four build-cost definitions.

---

### Blueprint Cost Storage

The cost dictionaries work like this:

- ore robot → ore cost only
- clay robot → ore cost only
- obsidian robot → ore and clay costs
- geode robot → ore and obsidian costs

`BuildQuantitiesFor(MineralType type)` returns the correct dictionary for the requested robot type.

That allows the simulation to ask for the build requirements of any robot in a consistent way.

---

### `GeodeCracker.cs`

This class manages the full search across all blueprints.

It stores:

- `BluePrints`
- `States`

The constructor:

- parses every input line into a `BluePrint`
- stores them in a list
- initialises the state cache

It provides:

- `Run24()`
- `Run32()`
- `RunBluePrint(BluePrint bluePrint, int totalTime)`

---

### Part 1 Calculation

`Run24()` processes all blueprints and does:

- run each blueprint for `24` minutes
- multiply the result by `(i + 1)`
- sum all values

So the silver answer is the total quality level, not just the raw number of geodes.

---

### Part 2 Calculation

`Run32()` does:

- take only the first three blueprints
- run each one for `32` minutes
- multiply the three results together

So the gold answer is a product of best geode totals.

---

### `GeodeCrackerState.cs`

This class represents one simulation state.

It stores:

- `Robots`
- `Minerals`
- `BluePrint`
- `Minutes`

A new starting state begins with:

- `1` ore robot
- `0` clay robots
- `0` obsidian robots
- `0` geode robots
- `0` minerals of every type
- the full duration remaining

It also supports cloning so the search can branch safely into multiple future actions.

---

### Mineral Collection

`CollectMinerals()` performs one production step.

It loops through every robot type and adds that robot count to the matching mineral count.

Logically this means:

- ore robots produce ore
- clay robots produce clay
- obsidian robots produce obsidian
- geode robots produce geodes

one unit per robot per minute.

---

### Checking Whether a Robot Can Be Built

`CanBuildRobotFor(MineralType type)` checks whether the current mineral stockpile meets the blueprint cost for that robot.

It returns `false` if any required mineral is missing.

So build decisions are always constrained by the current mineral inventory.

---

### Building a Robot

`BuildRobotFor(MineralType type)`:

- subtracts the required minerals from the inventory
- increments the robot count for that type

So the state is updated immediately to reflect the purchase.

---

### Deciding Whether a Robot Type Is Still Worth Building

`ShouldBuildRobotFor(MineralType type)` is a pruning helper.

It checks the blueprint requirements and returns `true` when the current number of robots producing a needed mineral is still low enough to justify more of them.

This is used to avoid building unnecessary extra robots, especially for lower-tier resources.

---

### State Search

`RunBluePrint()` performs the search for a single blueprint.

It creates:

- `int result = 0`
- `Queue<GeodeCrackerState> queue = new();`

Then it:

- clears the state cache
- enqueues the initial state
- repeatedly dequeues states until the queue is empty

For each state:

- if no minutes remain:
  - compare its geode total against the current best result
- otherwise:
  - call `Cycle(queue, this.States, totalTime)`

So the solver explores many possible build sequences using a queue-driven search.

---

### Main State Transition Logic

`Cycle(Queue queue, Dictionary states, int totalTime)` performs one minute of branching.

It first:

- decrements `Minutes`

Then it always tries the "build nothing" branch by:

- cloning the state
- collecting minerals
- enqueueing the result

After that it tries build branches in priority order.

---

### Build Priority

The solver strongly prioritises better robots.

Inside `Cycle()` it does:

1. if a geode robot can be built:
   - collect minerals
   - build the geode robot
   - enqueue that state
   - return immediately

2. otherwise, if an obsidian robot can be built:
   - collect minerals
   - build the obsidian robot
   - enqueue that state

3. conditionally try clay robot builds

4. conditionally try ore robot builds

This means geode robot construction short-circuits the rest of the options for that minute.

---

### Time-Based Pruning

Ore and clay robot builds are further restricted by remaining time.

The solver only considers them when:

- for `24` minutes total, more than `5` minutes remain
- for `32` minutes total, more than `10` minutes remain

This prevents wasting late turns on lower-tier robot production that is unlikely to pay off in time.

---

### State Caching

`TryEnqueue()` uses a cache keyed by:

- robot counts only

The key is generated by `ToKey()`, which concatenates all robot types and their counts into a string.

If the same robot-count configuration has been seen before, the solver compares total mineral counts and only re-enqueues the state when the new one is better.

So repeated weak states are pruned away, reducing search growth.

---

### `ToKey()` and `TotalMinerals()`

`ToKey()` builds a compact state identity from robot counts.

`TotalMinerals()` sums all currently held minerals.

Together these are used by `TryEnqueue()` to decide whether a newly generated state is worth exploring compared with one already seen.

---

### Part 1 Return Value

For each blueprint, `RunBluePrint()` returns:

- the maximum number of geodes collected by the end of the `24`-minute search

`Run24()` then converts those into quality levels and sums them.

---

### Part 2 Return Value

For each of the first three blueprints, `RunBluePrint()` returns:

- the maximum number of geodes collected by the end of the `32`-minute search

`Run32()` multiplies those three values together.

---

## 🛠 Implementation Notes

- Blueprints are parsed into per-robot mineral cost dictionaries
- The search starts with one ore robot and no minerals
- States branch through cloning
- Mineral collection happens before robot build application in each generated branch
- Geode robot builds take top priority
- Obsidian builds are considered next
- Clay and ore builds are pruned late in the search
- Repeated robot-count states are cached and filtered using total minerals
- Both puzzle parts reuse the same core search with different durations

---

## 🧪 Behaviour Summary

Given a set of robot blueprints:

- the solver parses robot costs into dictionaries
- starts with a single ore robot
- explores possible minute-by-minute build decisions
- collects minerals from existing robots each cycle
- builds new robots when affordable
- prunes repeated or low-value states
- Part 1 sums blueprint quality levels after 24 minutes
- Part 2 multiplies the best results from the first three blueprints after 32 minutes

---

## 🚀 Key Takeaways

- Good example of resource optimisation through state-space search
- Blueprint parsing is kept separate from simulation logic
- State cloning makes branching straightforward
- Build priority helps focus the search toward geode production
- State caching reduces repeated work and keeps the search manageable
- Time-based pruning avoids spending late turns on low-value robot types

---

## 🔗 References

- https://adventofcode.com/2022/day/19