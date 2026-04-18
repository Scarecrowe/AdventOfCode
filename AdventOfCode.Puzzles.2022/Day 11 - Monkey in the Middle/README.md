# 🎄 Advent of Code 2022 - Day 11: Monkey in the Middle

## 📜 Puzzle Overview

This puzzle simulates a group of monkeys inspecting and passing items between each other.

Each monkey:

- holds a list of items (worry levels)
- performs an operation on each item
- tests the result for divisibility
- throws the item to another monkey based on the test result

The input defines multiple monkeys, each with:

- starting items
- an operation (e.g. multiply or add)
- a divisibility test
- two target monkeys (true / false outcomes)

Part 1 runs a shorter simulation with worry reduction, while Part 2 runs a much longer simulation without reducing worry levels.

---

## 🧩 Part 1

Determine the level of monkey business after 20 rounds.

### 💡 Approach

- Parse each monkey and its configuration
- Loop through 20 rounds
- For each monkey:
  - inspect each item
  - apply the operation
  - divide the result by 3 (worry reduction)
  - test divisibility
  - pass the item to another monkey
- Track how many items each monkey inspects
- Multiply the two highest inspection counts

---

## 🧩 Part 2

Determine the level of monkey business after 10,000 rounds without worry reduction.

### 💡 Approach

- Use the same simulation logic as Part 1
- Remove the division by 3 step
- Prevent numbers from growing too large by:
  - applying modulo of the product of all divisors
- Run the simulation for 10,000 rounds
- Track inspection counts
- Multiply the two highest values

---

## 🧠 Code Breakdown

### `Day11.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monkey in the Middle`
- Loads the puzzle input
- Calls both puzzle parts

For Part 1:

- Creates the monkey simulation
- Runs it for 20 rounds with worry reduction enabled

For Part 2:

- Creates the same simulation
- Runs it for 10,000 rounds without dividing worry levels

---

### `Monkey.cs`

This class represents a single monkey.

It stores:

- `Items` (list of worry levels)
- `Operation` (function applied to each item)
- `TestDivisor`
- `TrueTarget`
- `FalseTarget`
- `InspectionCount`

The constructor:

- parses the monkey definition block
- extracts starting items
- builds the operation logic
- reads the divisor and target monkeys

---

### Monkey Operation

Each item is processed like this:

    newValue = operation(oldValue)

The operation may be:

- addition (e.g. `old + 6`)
- multiplication (e.g. `old * 19`)
- squaring (e.g. `old * old`)

---

### Worry Reduction (Part 1 Only)

After applying the operation:

    newValue /= 3

This reduces growth and keeps numbers manageable.

---

### Divisibility Test

Each monkey tests:

    newValue % TestDivisor == 0

Depending on the result:

- true → send to `TrueTarget`
- false → send to `FalseTarget`

---

### `MonkeyGame.cs` (or equivalent)

This class manages the full simulation.

It stores:

- `List<Monkey>`

The constructor:

- parses all monkeys from the input

---

### Parsing Monkeys

The input is grouped per monkey.

Parsing involves:

- reading blocks of lines
- creating one `Monkey` per block
- preserving order for indexing

Each monkey is referenced by its index in the list.

---

### Main Simulation Loop

The simulation runs for a specified number of rounds.

For each round:

- iterate through each monkey
- process all items currently held
- for each item:
  - apply operation
  - optionally reduce worry
  - apply modulo (Part 2)
  - test divisibility
  - throw item to target monkey
  - increment inspection count

Items thrown are appended to the target monkey's list.

---

### Controlling Number Growth (Part 2)

Without division, numbers grow extremely large.

To prevent overflow:

- calculate the product of all monkey divisors
- apply:

    newValue %= commonDivisor

This keeps values bounded while preserving divisibility behaviour.

---

### Inspection Tracking

Each time a monkey processes an item:

- increment `InspectionCount`

At the end:

- sort inspection counts
- take the top two values
- multiply them together

---

### Part 1 Return Value

After 20 rounds:

- return the product of the two highest inspection counts

---

### Part 2 Return Value

After 10,000 rounds:

- return the product of the two highest inspection counts

---

## 🛠 Implementation Notes

- Monkeys operate in sequence each round
- Items are passed immediately to target monkeys
- Part 1 uses division to reduce worry levels
- Part 2 uses modulo arithmetic to control growth
- Inspection counts are the key metric for scoring
- The same core simulation is reused for both parts

---

## 🧪 Behaviour Summary

Given a set of monkeys:

- each monkey inspects and modifies items
- items are passed between monkeys based on rules
- Part 1 runs 20 rounds with reduced worry
- Part 2 runs 10,000 rounds with controlled large values
- final result is based on inspection activity

---

## 🚀 Key Takeaways

- Good example of simulation with stateful agents
- Demonstrates dynamic routing of data between entities
- Highlights importance of controlling number growth
- Reuses the same logic with slight behavioural differences
- Efficient handling of large iterations is critical

---

## 🔗 References

- https://adventofcode.com/2022/day/11