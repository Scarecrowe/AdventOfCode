# 🎄 Advent of Code 2021 - Day 06: Lanternfish

## 📜 Puzzle Overview

This puzzle simulates the growth of a school of lanternfish.

Each fish has an internal reproduction timer:

- each day the timer decreases by 1
- when it drops below 0, the fish resets to `6`
- that fish also creates a new fish with timer `8`

A naive simulation would become far too large, especially for Part 2, so this solver groups fish by timer value and tracks how many fish exist in each group instead of modelling every fish individually.

The puzzle entry runs the simulation for:

- `80` days in Part 1
- `256` days in Part 2

The silver and gold answers are returned by calling:

    new LanternFishSpawner(this.Input).Run(80).TotalFish()
    new LanternFishSpawner(this.Input).Run(256).TotalFish()

---

## 🧩 Part 1

Determine how many lanternfish exist after 80 days.

### 💡 Approach

- Read the starting fish timers from the input
- Group fish with the same timer together
- Store each timer group with a running total
- Simulate 80 days of reproduction
- Return the sum of all grouped fish totals

---

## 🧩 Part 2

Determine how many lanternfish exist after 256 days.

### 💡 Approach

- Reuse the exact same grouped simulation
- Run it for much longer
- Avoid modelling fish one-by-one
- Track only:
  - timer value
  - total fish with that timer
- Return the final total after 256 days

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Lanternfish`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new LanternFishSpawner(this.Input)`
- calls `Run(80)`
- then calls `TotalFish()`

For Part 2:

- creates `new LanternFishSpawner(this.Input)`
- calls `Run(256)`
- then calls `TotalFish()`

---

### `LanternFish.cs`

This class models a grouped lanternfish state rather than an individual fish instance.

It stores:

- `InternalTimer`
- `Total`

The constructor accepts:

- the timer value
- the number of fish currently sharing that timer

So a grouped state such as:

    timer = 3
    total = 12

means there are 12 fish currently sitting on internal timer 3.

---

### `LanternFishSpawner.cs`

This class contains the full parsing and simulation logic.

It stores:

- `List<LanternFish> LanternFish`

The constructor immediately parses the input:

- `this.LanternFish = this.ParseInput(input)`

So the class starts with grouped timer buckets already prepared.

---

### Parsing the Input

`ParseInput(string[] input)` reads the first input line and splits it on commas.

At a high level it does this:

- split the input string by `","`
- convert each token into an integer timer
- count how many fish exist for each timer value
- create one `LanternFish` object per distinct timer

It uses a dictionary called:

- `Dictionary<int, int> sorter = new();`

For each timer:

- if the timer is not in the dictionary, add it with count `1`
- otherwise increment the count

Then each dictionary entry becomes:

- `new LanternFish(kvp.Key, kvp.Value)`

So the parsed result is a compact grouped representation of the school.

---

### Daily Simulation

`Run(int days)` performs the lanternfish simulation.

It loops from day 1 up to the requested number of days.

For each day it:

- records the number of current timer groups
- tracks whether any new infant group has already been added that day
- creates a placeholder newborn group:

    LanternFish infants = new(8, 1);

Then it iterates through the existing timer groups and decreases each group's timer by 1.

---

### Reproduction Logic

When a group's timer falls below 0:

- that group's timer is reset to `6`
- the same number of fish must also be added to timer `8`

This is handled by checking:

    if (this.LanternFish[i].InternalTimer < 0)

Then:

- reset the current group to `6`
- either create a new infant bucket at timer `8`
- or add to the already-created infant bucket for that day

The first reproducing group on that day creates:

    infants = new(8, this.LanternFish[i].Total)

and adds it to the list.

Any later reproducing groups on the same day do:

    infants.Total += this.LanternFish[i].Total

So all newborn fish for that day are accumulated into a single timer-8 group.

---

### Why This Works Efficiently

The solver does not expand into one object per fish.

Instead it keeps grouped buckets of fish counts, which makes long simulations practical.

Rather than storing millions or billions of separate fish, it stores a much smaller set of timer groups and updates their totals as reproduction happens.

This is the key idea that makes 256 days manageable.

---

### Counting the Final Fish Total

After simulation, `TotalFish()` returns:

    this.LanternFish.Sum(x => x.Total)

So the answer is the sum of all fish totals across every timer bucket.

---

## 🛠 Implementation Notes

- Fish are grouped by timer value during parsing
- Each group stores:
  - current timer
  - total fish in that group
- Simulation decrements each group's timer once per day
- Groups that reproduce reset to timer `6`
- All newborn fish for a day are collected into a single timer-`8` group
- Part 1 and Part 2 differ only by the number of days simulated
- Fish totals use `long`, which is important for the larger 256-day result

---

## 🧪 Behaviour Summary

Given an input like:

    3,4,3,1,2

the solver:

- groups matching timer values together
- simulates each day by decrementing timers
- resets reproducing groups to `6`
- creates a grouped newborn bucket at `8`
- repeats for either 80 or 256 days
- sums all grouped fish totals at the end

So the final answer is the total number of fish after the requested number of days.

---

## 🚀 Key Takeaways

- Good example of replacing a naive expanding simulation with grouped counting
- `LanternFish` represents a timer bucket plus population size
- `LanternFishSpawner` handles both parsing and day-by-day updates
- Part 1 and Part 2 share the exact same logic
- Using grouped totals and `long` values keeps the solution scalable

---

## 🔗 References

- https://adventofcode.com/2021/day/6