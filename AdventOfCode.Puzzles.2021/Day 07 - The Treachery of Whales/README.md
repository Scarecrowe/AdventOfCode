# 🎄 Advent of Code 2021 - Day 07: The Treachery of Whales

## 📜 Puzzle Overview

This puzzle works with a list of crab submarine positions along a horizontal line.

The input is a single comma-separated line of integers, where each value is the position of one crab.

Example input:

    16,1,2,0,4,2,7,1,2,14

Part 1 uses a constant fuel cost, where each step costs `1` fuel.

Part 2 uses an increasing fuel cost, where moving `n` steps costs:

    n * (n + 1) / 2

The solver tests candidate alignment positions and returns the least total fuel required.

---

## 🧩 Part 1

Find the minimum total fuel needed when each crab pays a constant cost per step.

### 💡 Approach

- Parse the single input line into an integer array
- Try each possible target position from `0` up to the maximum crab position
- For each crab, calculate the distance to that target
- Add that distance directly to the running fuel total
- Track the smallest fuel total seen
- Return the minimum result

---

## 🧩 Part 2

Find the minimum total fuel needed when fuel cost increases for each extra step.

### 💡 Approach

- Reuse the same parsed crab positions
- Try each possible target position from `0` up to the maximum crab position
- For each crab, calculate how many moves are needed
- Convert that move count into triangular fuel cost
- Sum the fuel for all crabs
- Track the smallest total
- Return the minimum result

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Treachery of Whales`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new TheTreacheryOfWhales(this.Input)`
- calls `Calculate(true)`

For Part 2:

- creates `new TheTreacheryOfWhales(this.Input)`
- calls `Calculate(false)`

---

### `TheTreacheryOfWhales.cs`

This class contains the full solution logic.

It stores:

- `Input`

The constructor parses the first input line with:

    input[0].Split(',').ToInt()

So the full crab-position list is loaded immediately into an integer array.

---

### Parsing the Positions

The input is not processed line by line in this implementation.

Instead it assumes:

- the data is entirely on the first line
- values are comma-separated
- each value can be converted directly to an integer

That means the solver works from a single integer array of crab positions for both puzzle parts.

---

### Main Search Logic

`Calculate(bool isConstant)` performs a brute-force search over candidate alignment points.

It begins with:

    int leastFuel = int.MaxValue;

Then it loops over target positions from:

    0

up to:

    this.Input.Max()

For each target position, it calculates the total fuel required to move every crab to that point.

---

### Constant Fuel Mode

When `isConstant` is `true`, the fuel cost for a crab is simply the number of moves needed.

At the core of the loop:

    int moves = Math.Abs(i - this.Input[j]);

Then:

    fuel += moves;

So Part 1 uses standard absolute distance as the fuel contribution for each crab.

---

### Increasing Fuel Mode

When `isConstant` is `false`, the solver uses triangular numbers to calculate fuel cost.

After finding the move distance, it adds:

    moves * (moves + 1) / 2

This gives the sum:

    1 + 2 + 3 + ... + moves

So each extra step costs more than the last, which matches the Part 2 rules.

---

### Early Exit Optimisation

Inside the inner loop, the implementation includes an optimisation:

    if (fuel >= leastFuel)
        break;

As soon as the current candidate target is already worse than the best solution found so far, it stops evaluating that target position.

This avoids unnecessary work when a position can no longer beat the current minimum.

---

### Final Result Tracking

After evaluating a target position, the solver updates the running minimum with:

    leastFuel = Math.Min(leastFuel, fuel);

Once all candidate positions have been tested, it returns:

    leastFuel

So both puzzle parts share the same search loop, with only the fuel formula changing.

---

## 🛠 Implementation Notes

- `Day7.cs` uses `Calculate(true)` for silver and `Calculate(false)` for gold
- The input is parsed from `input[0]` only
- Candidate target positions are tested from `0` to `this.Input.Max() - 1` because the loop condition is `i < this.Input.Max()`
- Fuel cost is based on `Math.Abs(i - this.Input[j])`
- Part 2 uses the triangular number formula `moves * (moves + 1) / 2`
- The solver includes an early-break optimisation when a candidate target already exceeds the current best

---

## 🧪 Behaviour Summary

Given a comma-separated list of crab positions:

- the solver parses them into an integer array
- it tests many possible alignment positions
- for each target, it sums the fuel needed for every crab
- Part 1 uses direct distance as fuel
- Part 2 uses increasing triangular fuel cost
- it keeps the smallest fuel value found
- the final result is the minimum total fuel required

---

## 🚀 Key Takeaways

- Good example of using brute-force search over a bounded range
- Both puzzle parts reuse the same outer structure
- The only real difference between parts is the fuel-cost formula
- The early-break check improves efficiency without changing the result
- The implementation stays compact by parsing directly into an integer array

---

## 🔗 References

- https://adventofcode.com/2021/day/7