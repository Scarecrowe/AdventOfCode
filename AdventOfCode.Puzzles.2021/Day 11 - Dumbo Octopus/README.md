# 🎄 Advent of Code 2021 - Day 11: Dumbo Octopus

## 📜 Puzzle Overview

This puzzle works with a grid of octopuses, where each input digit is that octopus's starting energy level.

Each input line is one row in the cavern map.

Example input:

    5483143223
    2745854711
    5264556173
    6141336146
    6357385478

Part 1 counts the total number of flashes after 100 simulation steps.

Part 2 finds the first step where every octopus flashes at the same time.

The implementation models the cavern as a map of `DumboOctopus` objects, then repeatedly runs step-based energy propagation until it reaches the requested condition.

---

## 🧩 Part 1

Count how many flashes occur after 100 steps.

### 💡 Approach

- Parse the input grid into a map of octopus objects
- For each step:
  - increment every octopus
  - when an octopus goes above `9`, it flashes
  - a flash resets that octopus to `0`
  - every adjacent neighbour is incremented
- Keep a running total of flashes on the map
- After 100 steps, return the total flash count

---

## 🧩 Part 2

Find the first step where the whole cavern flashes simultaneously.

### 💡 Approach

- Reuse the same cavern simulation
- Run one step at a time
- After each step, check whether every octopus flashed
- Return the step number where that happens first
- If the sync never appears within the implementation limit, return `-1`

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Dumbo Octopus`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new Cavern(this.Input)`
- calls `RunFor(100).Map.Flashes`

For Part 2:

- creates `new Cavern(this.Input)`
- calls `RunUntil()`

That means the silver answer comes from the flash counter stored on the map, while the gold answer comes from the step number returned by the cavern runner.

---

### `Cavern.cs`

This class coordinates the simulation.

It stores:

- `Map`

The constructor creates the map with:

    new(this, input)

So the cavern owns a `DumboOctopusMap`, and each octopus also keeps a reference back to the parent cavern.

---

### `DumboOctopusMap.cs`

This class stores all octopuses in a coordinate-based map.

It inherits from:

- `VectorDictionary<int, DumboOctopus>`

It also stores:

- `Flashes`
- `Cavern`

During parsing it loops through every `y` and `x` coordinate, converts each input character into an integer, and adds a new `DumboOctopus` at that position.

At a high level it does:

    for each row
        for each column
            add octopus at x,y with parsed energy

This is the structure used by the step simulation and neighbour lookups.

---

### `DumboOctopus.cs`

This class models a single octopus.

It stores:

- `Point`
- `EnegryLevel`
- `Flashed`
- `Cavern`

The property is spelled `EnegryLevel` in the implementation.

Each octopus knows its own position, current energy, whether it has already flashed this step, and which cavern map it belongs to.

---

### Running a Fixed Number of Steps

`RunFor(int iterations)` loops from `1` to the requested count and calls:

    this.RunStep();

on each iteration.

It then returns the cavern instance, which is why `Day11.cs` can immediately read:

    .Map.Flashes

after the 100-step run completes.

---

### Running Until Synchronisation

`RunUntil()` starts with:

    int steps = 1;

Then it repeatedly calls `RunStep()` until that method returns `true`.

If that happens, it returns the current step count.

This implementation also includes a hard stop:

    if (steps > 300)
        break;

and returns `-1` if no fully synchronised flash is found within that limit.

---

### Step Simulation

`RunStep()` performs the main simulation for one cavern step.

First it loops over the whole map and calls:

    this.Map[new(x, y)].Increment();

for every coordinate.

After that, it checks whether all octopuses flashed during the step by scanning the map and testing each `Flashed` flag.

Finally it resets every octopus with:

    octopus.Value.Reset();

and returns whether all octopuses had flashed in that step.

---

### Flash Propagation

The core flash logic lives in `DumboOctopus.Increment()`.

It works like this:

- if the octopus already flashed this step, return immediately
- otherwise increment its energy
- if energy rises above `9`:
  - mark `Flashed = true`
  - set `EnegryLevel = 0`
  - increment the map-wide flash counter
  - increment every intercardinal adjacent neighbour

The neighbour propagation uses:

    this.Cavern.Map.AdjacentInterCardinal(this.Point)

So flashes spread to all surrounding neighbours, including diagonals.

---

### Preventing Multiple Flashes in One Step

Once an octopus has flashed, later calls to `Increment()` immediately return because of:

    if (this.Flashed)
        return;

That prevents the same octopus from flashing multiple times in a single step, even if neighbours continue to propagate energy into it after it has already flashed.

---

### Resetting Step State

At the end of each step, every octopus is reset with:

    public void Reset() => this.Flashed = false;

This clears only the per-step flash state.

The total flash count is not reset, because that is stored separately on the map in `Flashes`.

---

## 🛠 Implementation Notes

- `Day11.cs` uses `RunFor(100).Map.Flashes` for silver and `RunUntil()` for gold
- The octopus energy property is named `EnegryLevel` in the code
- A flash sets energy to `0` immediately
- Flash propagation uses `AdjacentInterCardinal(...)`, so diagonal neighbours are included
- The map tracks total flashes with `IncrementFlashes()`
- `RunUntil()` has a built-in limit of 300 steps and returns `-1` if no synchronised flash is found before then
- `RunStep()` iterates using `x < this.Map.Width + 1`, which is how this implementation traverses the full row range in the project code

---

## 🧪 Behaviour Summary

Given a grid of octopus energy levels:

- the solver parses the grid into coordinate-based octopus objects
- each step increments every octopus once
- octopuses that exceed `9` flash, reset to `0`, and propagate energy to all adjacent neighbours
- flashed octopuses cannot flash again in the same step
- Part 1 returns the total number of flashes after 100 steps
- Part 2 returns the first step where every octopus flashes together, or `-1` if the implementation limit is reached first

---

## 🚀 Key Takeaways

- Good example of modelling chain reactions with object-based grid cells
- Flash propagation is handled recursively through neighbour increments
- The per-step `Flashed` flag prevents duplicate flashes in the same round
- The map keeps a persistent flash total for Part 1
- Synchronisation detection for Part 2 is built directly into the step runner

---

## 🔗 References

- https://adventofcode.com/2021/day/11