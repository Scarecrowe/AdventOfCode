# 🎄 Advent of Code 2016 - Day 10: Balance Bots

## 📜 Puzzle Overview

This puzzle simulates a network of bots passing microchips.

Each instruction describes either:

- assigning a value to a bot
- or how a bot distributes its low and high value chips

Bots can:

- hold up to two chips
- compare them
- pass the lower and higher values to other bots or output bins

Part 1 identifies which bot compares specific values.  
Part 2 computes a result from output bins.

---

## 🧩 Part 1

Determine which bot is responsible for comparing two specific chip values.

### 💡 Approach

- Parse all instructions into:
  - initial value assignments
  - bot distribution rules
- Simulate the system:
  - bots receive chips
  - once a bot has two chips:
    - it sorts them into low and high
    - distributes them according to its rule
- Track when a bot compares the target values
- Return the bot ID when the match is found

---

## 🧩 Part 2

Determine the product of the values in output bins `0`, `1`, and `2`.

### 💡 Approach

- Continue the same simulation as Part 1
- Track values placed into output bins
- Once bins `0`, `1`, and `2` all contain values:
  - multiply those values together
- Return the result

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Balance Bots`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Runs the simulation
- Detects the bot comparing target values

For Part 2:

- Uses the same simulation
- Extracts values from output bins

---

### Instruction Types

#### Value Assignment

```
value X goes to bot Y
```

- Assigns a chip with value `X` to bot `Y`

---

#### Distribution Rule

```
bot X gives low to bot/output A and high to bot/output B
```

- Defines how a bot distributes its chips
- Low and high destinations can be:
  - another bot
  - an output bin

---

### Data Structures

Typical structures include:

- dictionary of bots → list of chips
- dictionary of bot rules
- dictionary of outputs → stored values

---

### Simulation Logic

- Process initial value assignments
- Maintain a queue or loop over bots
- When a bot has two chips:
  - sort values into low/high
  - apply its rule
  - pass values to targets
- Continue until no more actions are possible

---

### Part 1 Logic

- During processing:
  - check if a bot is holding the target pair
- If found:
  - return that bot's ID

---

### Part 2 Logic

- Track values placed into outputs
- After simulation:
  - retrieve values from bins `0`, `1`, and `2`
  - multiply them together

---

### Handling Chip Flow

- Bots may receive values at different times
- Ensure rules are applied only when two chips are present
- Avoid reprocessing bots unnecessarily

---

## 🛠 Implementation Notes

- Order of execution matters for correctness
- Queue-based processing helps manage active bots
- Sorting two values is trivial but essential
- Separate bot logic from output storage
- Simulation continues until all movements are resolved

---

## 🧪 Behaviour Summary

Given a list of instructions:

- Values are assigned to bots
- Bots process chips when holding two values
- Chips are distributed through the network
- Part 1 identifies a specific comparison event
- Part 2 extracts results from output bins

---

## 🚀 Key Takeaways

- Event-driven simulation problem
- State changes triggered by conditions (two chips)
- Clear separation of rules and execution
- Reusable simulation for both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2016/day/10