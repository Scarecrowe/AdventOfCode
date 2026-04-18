# 🎄 Advent of Code 2019 - Day 01: The Tyranny of the Rocket Equation

## 📜 Puzzle Overview

This puzzle focuses on calculating fuel requirements for spacecraft modules.

Each line of input represents the mass of a module.

For example:

    12
    14
    1969
    100756

The goal is to determine how much fuel is required, first for the modules alone, and then accounting for the fuel's own mass.

---

## 🧩 Part 1

Calculate the total fuel requirement for all modules.

### 💡 Approach

- Read each line as an integer mass
- Apply the fuel formula:

      fuel = floor(mass / 3) - 2

- Sum the fuel values for all modules
- Return the total

---

## 🧩 Part 2

Calculate the total fuel requirement, including fuel for the fuel itself.

### 💡 Approach

- Start with the base fuel calculation from Part 1
- For each fuel value:
  - Repeatedly apply the same formula to the fuel itself
  - Stop when the result is zero or negative
- Accumulate all fuel values for each module
- Return the total

---

## 🧠 Code Breakdown

### `Day01.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The Tyranny of the Rocket Equation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses input into integers
- Applies the fuel formula to each mass
- Sums the results

For Part 2:

- Parses input into integers
- Calls a helper method to calculate recursive fuel requirements
- Sums the results

---

### Fuel Calculation Logic

The core formula used is:

    fuel = (mass / 3) - 2

This is integer division, so the result is automatically floored.

---

### Recursive Fuel Calculation

For Part 2, fuel itself requires fuel.

This is handled by repeatedly applying the same formula:

    while (fuel > 0)
    {
        fuel = (fuel / 3) - 2;
        if (fuel > 0)
            total += fuel;
    }

This ensures:

- Only positive fuel values are added
- The calculation stops naturally when fuel becomes zero or negative

---

### Input Parsing

The input is processed as:

- Read each line
- Convert to integer
- Store in a collection for iteration

---

## 🛠 Implementation Notes

- Integer division is used to implicitly floor results
- Negative fuel values are ignored
- Part 2 builds on Part 1 logic by looping until exhaustion
- The recursive calculation is typically implemented iteratively for performance and simplicity

---

## 🧪 Behaviour Summary

Given a list of module masses:

- Part 1 calculates fuel directly from mass
- Part 2 accounts for additional fuel required by the fuel itself
- Each module is processed independently
- Results are accumulated into a single total

---

## 🚀 Key Takeaways

- Simple arithmetic problem with an important recursive twist
- Demonstrates how small formulas can grow in complexity when reapplied
- Highlights the importance of stopping conditions in iterative calculations
- Clean separation between base calculation and extended logic

---

## 🔗 References

- https://adventofcode.com/2019/day/1