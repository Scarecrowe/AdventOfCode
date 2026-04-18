# 🎄 Advent of Code 2018 - Day 05: Alchemical Reduction

## 📜 Puzzle Overview

This puzzle works with a **polymer chain** represented as a single string of letters.

Each unit in the polymer:

- has a **type** (letter)
- has a **polarity** (uppercase or lowercase)

Two adjacent units **react and are destroyed** if:

- they are the same letter
- but opposite case (e.g. `a` and `A`)

Examples:

    aA -> reacts -> removed
    abBA -> bB reacts -> aA reacts -> empty
    abAB -> no reaction
    aabAAB -> no reaction

Reactions can cascade as new adjacent pairs form after removals.

---

## 🧩 Part 1

Fully react the polymer and determine how many units remain.

### 💡 Approach

- Treat the input as a single string
- Process units sequentially
- When two adjacent units react:
  - remove them
- Continue until no more reactions occur
- Return the final polymer length

---

## 🧩 Part 2

Find the shortest possible polymer by removing one unit type entirely before reacting.

### 💡 Approach

- For each letter `a` → `z`:
  - remove both lowercase and uppercase versions from the input
  - fully react the resulting polymer
  - track the resulting length
- Return the smallest length found

---

## 🧠 Code Breakdown

### `Day05.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Alchemical Reduction`
- Loads the full polymer string input
- Executes both parts

For Part 1:

- Calls the polymer reaction logic
- Returns the length of the fully reduced polymer

For Part 2:

- Iterates through each unit type
- Removes that type and re-runs the reaction
- Tracks the minimum result

---

### Polymer Representation

The entire input is treated as:

- a single string (or char array)

Example:

    dabAcCaCBAcCcaDA

---

### Reaction Rule

Two units react if:

- they are the same letter ignoring case
- but have different casing

Conceptually:

    unit1 != unit2
    toLower(unit1) == toLower(unit2)

---

### Reaction Process (Stack Approach)

A common and efficient approach is to use a stack-like structure.

High-level logic:

- Create an empty stack
- Iterate through each unit:
  - If the stack is not empty AND the current unit reacts with the top:
    - pop the stack (destroy both)
  - otherwise:
    - push the current unit

This naturally handles cascading reactions.

---

### Example Walkthrough

Given:

    dabAcCaCBAcCcaDA

Steps:

    dabA cC aCBAcCcaDA
    dab Aa CBAcCcaDA
    dabCBA cCc aDA
    dabCBAcaDA

Final result:

    length = 10

---

### Full Reaction Function

The core logic can be reused for both parts.

Conceptually:

    react(polymer):
        stack = []

        for unit in polymer:
            if stack not empty AND reacts(stack[-1], unit):
                stack.pop()
            else:
                stack.push(unit)

        return stack

---

### Part 2 Optimisation Loop

For each unit type:

- Remove all instances of that type:

    polymer without 'a' and 'A'

- Run the same reaction logic
- Track the smallest resulting length

---

### Performance Considerations

- Stack approach runs in **O(n)** per reaction
- Part 2 runs this up to 26 times
- Overall complexity remains efficient for large inputs

---

## 🛠 Implementation Notes

- Input is a single string, not line-based
- Reactions depend on ASCII case differences
- Stack-based processing avoids repeated rescans
- Same reaction logic is reused for both parts
- Part 2 brute-forces all unit types (a–z)

---

## 🧪 Behaviour Summary

Given a polymer string:

- adjacent opposite-polarity units annihilate each other
- reactions cascade until stable
- Part 1 returns the final polymer length
- Part 2 finds the optimal unit type to remove for the shortest result

---

## 🚀 Key Takeaways

- Great example of **stack-based reduction problems**
- Shows how local rules can create cascading global effects
- Efficient single-pass solutions outperform naive repeated scanning
- Reusable core logic simplifies solving both parts
- Brute-force with optimisation is often perfectly acceptable

---

## 🔗 References

- https://adventofcode.com/2018/day/5