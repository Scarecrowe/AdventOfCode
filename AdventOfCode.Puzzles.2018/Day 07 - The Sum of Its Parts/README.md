# 🎄 Advent of Code 2018 - Day 07: The Sum of Its Parts

## 📜 Puzzle Overview

This puzzle is about resolving step dependencies.

Each input line describes a rule saying one step must be completed before another can begin.

An input line looks like this:

    Step C must be finished before step A can begin.

The solver parses each line into an `Instruction` object with:

- `Before`
- `After`

From those instructions it builds a full set of steps, where each step knows which parent steps must be completed before it becomes available.

Part 1 determines the correct assembly order using alphabetical tie-breaking. Part 2 simulates multiple elves working in parallel and returns the total completion time.

---

## 🧩 Part 1

Determine the correct order of steps.

### 💡 Approach

- Parse each input line into a dependency instruction
- Build a distinct set of steps
- For each step, collect its required parent steps
- Start with all steps that have no parents
- Repeatedly:
  - choose the alphabetically earliest available step
  - add it to the result
  - remove that completed step from other steps' parent lists
  - add newly unlocked steps into the available pool
- Continue until no valid steps remain
- Return the assembled character sequence as a string

---

## 🧩 Part 2

Determine how long the full assembly takes with multiple elves.

### 💡 Approach

- Reuse the same dependency structure
- Find all currently available steps with no remaining parents
- Assign available steps to idle elves until the worker limit is reached
- Each assigned step gets a duration based on its letter
- Advance the simulation one second at a time
- When an elf finishes:
  - remove that completed step from all remaining parent lists
  - free the elf for future work
- Continue until there are no steps left and no elves still working
- Return the total elapsed seconds

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Sum of Its Parts`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TheSumOfItsParts(this.Input)`
- Calls `AssembleyOrder()`

For Part 2:

- Creates `new TheSumOfItsParts(this.Input)`
- Calls `AssemblyTime(5)`

So the gold solution runs the worker simulation using:

- `5` elves

---

### `Instruction.cs`

This class parses one dependency line.

It stores:

- `Before`
- `After`

The constructor splits the line using:

    " must be finished before step "

Then trims the fixed text fragments:

    "Step "
    " can begin."

So a line such as:

    Step C must be finished before step A can begin.

becomes an instruction where:

- `Before = 'C'`
- `After = 'A'`

---

### `ElfStep.cs`

This class represents a single step in the dependency graph.

It stores:

- `Letter`
- `Parents`

`Letter` is the step identifier, such as `A`, `B`, or `C`.

`Parents` is a list of prerequisite step letters that must be completed before this step becomes available.

So if step `A` depends on `C`, then `A` is represented with:

- `Letter = 'A'`
- `Parents = ['C']`

---

### `Elf.cs`

This class models an elf currently working on a step.

It stores:

- `Seconds`
- `Step`

The constructor calculates the task duration using:

    (step.Letter - 64) + 60

That means:

- `A = 61`
- `B = 62`
- `C = 63`

and so on.

So each step takes:

- 60 base seconds
- plus its alphabetical position

---

### `TheSumOfItsParts.cs`

This class contains the full puzzle logic.

It stores:

- `Instructions`
- `Steps`

The constructor:

- parses all input lines into `Instruction` objects
- builds the full distinct step list
- creates one `ElfStep` per letter
- fills each step's `Parents` list from the instructions where that step appears as the `After` target

---

### Building the Step List

The solver first gathers every unique step letter using:

- `DistinctSteps()`

This method scans all instructions and adds both:

- `instruction.After`
- `instruction.Before`

into a `HashSet<char>`

That ensures steps are included whether they appear as:

- a dependency target
- a prerequisite source

---

### Creating Parent Relationships

After collecting the distinct step letters, the constructor creates each `ElfStep` by looking up all instructions where the current step is on the right-hand side.

Conceptually:

- for each step letter
- find all instructions where `After == step`
- collect the matching `Before` letters
- sort those prerequisite letters alphabetically
- store them as the step's `Parents`

So each step begins with a full list of prerequisites that will be removed as work completes.

---

### Part 1 Ordering Logic

`AssembleyOrder()` builds the silver answer.

It creates:

- `List<char> result = new();`

Then it builds an initial available list using all steps with:

- `Parents.Count == 0`

It chooses the first current step, then loops while a valid step exists.

On each pass it:

- adds the step letter to the result if not already present
- removes that step from the available list
- scans all steps
- removes the completed letter from any matching parent list
- adds those updated steps into the available pool
- selects the next available step using alphabetical order

The next step is chosen with:

    steps.OrderBy(x => x.Letter).FirstOrDefault(x => x.Parents.Count == 0)

So the solver always prefers the alphabetically earliest unlocked step.

At the end it returns:

    result.Join()

which produces the final assembly order string.

---

### Unlocking New Steps

The core dependency update is done by removing the completed step letter from other steps' parent lists.

Logically this is:

    if step depends on current step
        remove current step from that step's parents

Once a step has no parents left, it becomes eligible to run.

That is how the solver gradually unlocks the graph as progress is made.

---

### Part 2 Worker Simulation

`AssemblyTime(int elfCount)` performs the timed multi-elf simulation.

It creates:

- `List<Elf> elves = new();`

and tracks:

- `seconds`

The method repeatedly performs three main phases:

1. assign available steps to idle elves
2. decrement each elf's remaining time
3. process completed work and unlock more steps

This continues until:

- there are no active elves
- and there are no steps left to assign

---

### Assigning Work to Elves

At the start of each loop, the solver scans all remaining steps with:

- `Parents.Count == 0`

For each such step:

- if the number of working elves is still below `elfCount`
- create a new `Elf(step)`
- remove that step from `this.Steps`

Removing the step from the main step list prevents it from being assigned again.

So once a step is claimed by an elf, it is effectively in progress and no longer sits in the waiting pool.

---

### Advancing Time

After assignment, the solver loops over every active elf and decrements:

    elf.Seconds--

If an elf reaches zero seconds, that step is complete.

Finished elves are collected into:

- `List<Elf> finished = new();`

---

### Completing Steps

When an elf finishes a step, the solver scans every remaining step and removes the completed letter from its parent list.

That means completion instantly unlocks any dependent steps whose last prerequisite has now been cleared.

Conceptually:

    foreach remaining step
        remove completed letter from parents

After that, the finished elves are removed from the active worker list.

---

### Completion Condition

At the end of each simulation tick:

- `seconds++`

The method returns once both of these are true:

- `elves.Count == 0`
- `this.Steps.Count == 0`

So the gold answer is the total number of elapsed seconds required to finish all work with the specified number of elves.

---

## 🛠 Implementation Notes

- The method name is `AssembleyOrder()` in the implementation
- Part 1 uses alphabetical ordering when multiple steps are available
- Each `ElfStep` stores its prerequisites in a mutable `Parents` list
- Step durations are calculated as letter position plus 60 seconds
- Part 2 removes steps from the main step list as soon as they are assigned
- Finished steps unlock dependent work by removing their letter from other parent lists
- `AssemblyTime(5)` is used for the gold solution

---

## 🧪 Behaviour Summary

Given a set of dependency instructions:

- the solver parses each rule into a `Before` and `After` step
- builds a distinct set of all step letters
- assigns each step its prerequisite parent list
- Part 1 repeatedly chooses the alphabetically earliest available step
- completed steps are removed from dependency lists to unlock later work
- Part 2 simulates multiple elves working in parallel
- each step takes 60 seconds plus its alphabetical value
- the final result is either the ordered step string or the total elapsed assembly time

---

## 🚀 Key Takeaways

- Clean dependency-graph style solution using parent lists
- Part 1 is a topological ordering with alphabetical tie-breaking
- Part 2 extends the same model into a worker-based time simulation
- `Instruction`, `ElfStep`, and `Elf` each represent a clear piece of the puzzle
- Unlocking work is handled by mutating prerequisite lists as steps complete
- The same parsed input structure supports both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2018/day/7