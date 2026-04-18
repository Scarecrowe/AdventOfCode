# 🎄 Advent of Code 2015 - Day 17: No Such Thing as Too Much

## 📜 Puzzle Overview

The elves bought too much eggnog again and need to fit exactly `150` liters into a selection of containers. Each container has a fixed capacity, and the goal is to find combinations of containers that add up to the target exactly.

Part 1 asks for the total number of valid combinations.

Part 2 asks how many of those valid combinations use the **fewest possible containers**.

---

## 🧩 Part 1

Determine how many different combinations of containers can exactly hold `150` liters of eggnog.

### 💡 Approach

- Parse the input into a list of container sizes
- Generate every combination of containers that sums to the target amount
- Count how many valid combinations exist

This solution delegates the combination search to a shared extension method that returns only combinations matching the required total.

---

## 🧩 Part 2

Determine how many valid combinations use the **minimum number of containers** needed to reach the target.

### 💡 Approach

- Reuse the full list of valid combinations from Part 1
- Find the smallest container count used by any valid combination
- Count how many combinations use that minimum

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `NoSuchThingAsTooMuch.ContainerCount(this.Input, 150)`

For Part 2:

- Calls `NoSuchThingAsTooMuch.CombinationCount(this.Input, 150)`

Both answers are returned as strings.

---

### `NoSuchThingAsTooMuch.cs`

This class contains the full solution for both parts.

It exposes two static methods:

- `ContainerCount(string[] input, int liters)`
- `CombinationCount(string[] input, int liters)`

---

### Parsing the Input

Both methods begin by converting the raw string input into numeric container sizes using:

    input.ToLongList()

This produces a numeric list that can be passed into the combination search.

---

### Part 1 Logic

`ContainerCount()` works by generating all combinations of containers whose total equals the requested number of liters:

    input.ToLongList().CombinationsOfTotal(liters).Count

This means the method does not manually loop through subsets itself. Instead, it relies on the `CombinationsOfTotal()` helper and simply returns the number of matching combinations.

---

### Part 2 Logic

`CombinationCount()` starts by generating the same list of valid combinations used in Part 1:

    List<List<long>> combinations = input.ToLongList().CombinationsOfTotal(liters);

It then finds the minimum number of containers used by any valid combination and counts how many combinations match that minimum:

    combinations.Count(x => x.Count == combinations.Min(x => x.Count))

This keeps Part 2 compact by building directly on top of the Part 1 result set.

---

## 🛠 Implementation Notes

- Both parts reuse the same combination-generation helper
- Part 1 returns the total number of matching combinations
- Part 2 filters those combinations by the smallest container count
- Input parsing is handled through `ToLongList()`
- The target amount is passed in as a parameter, with Day 17 using `150` liters in both parts

---

## 🧪 Examples

From the puzzle description, if the available containers are:

    20
    15
    10
    5
    5

and the target is `25` liters, there are four valid combinations:

- `15` and `10`
- `20` and the first `5`
- `20` and the second `5`
- `15`, `5`, and `5`

---

## 🚀 Key Takeaways

- Clean example of reusing one combination search for both puzzle parts
- Part 2 stays simple by filtering the already-valid combinations
- The solver remains compact because the heavy lifting is delegated to shared extension methods
- The only real difference between the two parts is how the valid combination list is evaluated afterward

---

## 🔗 References

- https://adventofcode.com/2015/day/17