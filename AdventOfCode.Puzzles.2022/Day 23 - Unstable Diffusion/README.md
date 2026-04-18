# 🎄 Advent of Code 2022 - Day 23: Unstable Diffusion

## 📜 Puzzle Overview

This puzzle simulates a group of elves moving across a grid.

The input is a map where:

- `#` represents an elf
- `.` represents empty ground

The solver parses every elf position into a coordinate map.

Each round:

- every elf checks adjacent positions
- elves only consider moving if another elf is nearby
- movement proposals follow a rotating priority order
- if multiple elves propose the same destination, none of them move there

Part 1 runs the simulation for 10 rounds and counts the empty ground inside the smallest rectangle containing all elves. Part 2 keeps running until no elf moves, then returns the round number where the system stabilises.

---

## 🧩 Part 1

Determine how many empty ground tiles are inside the smallest rectangle containing all elves after 10 rounds.

### 💡 Approach

- Parse the input grid into elf coordinates
- Simulate rounds of movement
- For each elf:
  - check surrounding adjacent cells
  - skip movement if no neighbours are nearby
  - otherwise test movement directions in the current priority order
- Collect all proposed destinations
- Only apply moves where exactly one elf chose that destination
- Rotate the direction priority
- After 10 rounds, calculate the bounding rectangle of all elves
- Count how many cells inside it are empty

---

## 🧩 Part 2

Determine the first round where no elf moves.

### 💡 Approach

- Reuse the same movement simulation
- Continue running beyond 10 rounds
- Stop when no movement proposals are made in a round
- Return the round number where that happens

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Unstable Diffusion`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new UnstableDiffusion(this.Input)`
- Calls `Run()`
- Calls `EmptyGround()`

For Part 2:

- Creates `new UnstableDiffusion(this.Input)`
- Calls `Run(true)`
- Returns `Round`

---

### `UnstableDiffusion.cs`

This class contains the full simulation.

It stores:

- `Map`
- `Round`

The constructor:

- creates a new map
- scans the input grid
- adds every `#` position into the map

So the map only stores occupied elf coordinates.

---

### Parsing the Input

The constructor loops through:

- every row `y`
- every column `x`

When it finds:

    '#'

it adds that coordinate into `Map`.

This means the simulation works from a sparse set of occupied points rather than storing the full grid.

---

### Printing the Map

`PrintMap()` is a debug helper.

It:

- finds the minimum and maximum occupied coordinates
- loops through that bounding box
- prints `#` for elves
- prints `.` for empty space

This lets the current elf layout be visualised at any point.

---

### Calculating Empty Ground

`EmptyGround()` finds:

- the minimum occupied x and y
- the maximum occupied x and y

It then scans the full bounding rectangle and counts every coordinate that is not present in `Map`.

So Part 1 measures empty ground only inside the smallest rectangle containing all elves.

---

### Direction Priority

Inside `Run()` the solver creates a direction list from:

- `CardinalHelper.CardinalTransform()`

These directions are checked in order when an elf wants to move.

At the end of each round:

- the first direction is removed
- it is added to the end of the list

So movement priority rotates every round.

---

### Detecting Nearby Elves

For each elf, the solver first gets:

- `AdjacentInterCardinal(elf.Key)`

This gathers neighbouring positions around that elf.

If no adjacent elves exist:

- that elf does not propose a move

So isolated elves remain where they are.

---

### Choosing a Move

If an elf has nearby neighbours, it tests directions in the current priority order.

For each candidate direction, the solver calls:

- `HasGroupMove(cardinal, adjacent)`

This checks whether the relevant group of tiles for that direction is clear.

If the direction is valid:

- the solver computes the target destination
- records that elf under the destination in `moves`
- stops checking further directions for that elf

So each elf can propose at most one move per round.

---

### `HasGroupMove(...)`

This helper checks whether a directional movement group is free.

It looks at:

- `CardinalHelper.CardinalGroupMap[direction]`

and returns `false` if any adjacent elf occupies one of the blocked positions for that move.

Otherwise it returns `true`.

So a move is only allowed when the required directional group is clear.

---

### Proposal Resolution

`Run()` stores proposals in:

- `Dictionary<Vector, List<Vector>> moves`

The key is the proposed destination.

The value is the list of elves trying to move there.

After all proposals are collected:

- if a destination has exactly one proposer, that move is applied
- if a destination has multiple proposers, no elf moves there

Successful movement is done by:

- removing the old elf position from `Map`
- adding the new destination to `Map`

This cleanly resolves collisions by cancelling contested moves.

---

### Main Simulation Loop

`Run(bool infinite = false)` drives the full process.

It starts with:

- `Round = 1`

Then each round it:

- scans all elves
- gathers movement proposals
- checks whether any moves were proposed
- applies only uncontested moves
- rotates direction priority
- clears the proposal dictionary
- increments the round counter

The loop stops when either:

- no moves were proposed
- or 10 rounds have completed in normal mode

---

### Part 1 Stop Condition

When `infinite` is `false`, the solver stops after round 10 using:

    this.Round > 10

Then `EmptyGround()` is called to calculate the silver answer.

---

### Part 2 Stop Condition

When `infinite` is `true`, the solver ignores the 10-round limit.

Instead it only stops when:

- `moves.Any()` is false

That means no elf proposed any move in that round, so the layout has stabilised.

It then returns the current `Round` value as the gold answer.

---

## 🛠 Implementation Notes

- The map stores only occupied elf positions
- The simulation uses sparse coordinates instead of a full grid
- Elves only attempt movement when neighbours are nearby
- Each elf proposes at most one move per round
- Movement priorities rotate every round
- Proposed destinations are grouped before movement is applied
- Contested moves are cancelled automatically
- Part 1 measures empty ground after 10 rounds
- Part 2 runs until no movement proposals exist

---

## 🧪 Behaviour Summary

Given an input grid of elves:

- the solver parses all elf positions into a coordinate map
- each round, elves check nearby occupancy
- elves propose moves using a rotating directional priority
- only uncontested proposed moves are applied
- Part 1 stops after 10 rounds and counts empty tiles in the bounding box
- Part 2 stops when no elf wants to move and returns that round number

---

## 🚀 Key Takeaways

- Good example of agent-based grid simulation
- Sparse coordinate storage keeps the representation simple
- The proposal phase cleanly separates decision-making from movement
- Rotating movement priority prevents directional bias
- Collision handling is solved naturally by grouping destination proposals
- The same simulation supports both puzzle parts with different stop conditions

---

## 🔗 References

- https://adventofcode.com/2022/day/23