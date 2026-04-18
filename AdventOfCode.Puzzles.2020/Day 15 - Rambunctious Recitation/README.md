# 🎄 Advent of Code 2020 - Day 15: Rambunctious Recitation

## 📜 Puzzle Overview

This puzzle is a number memory game.

You start with a list of numbers, spoken in order. After that:

- if the last number is new, the next number is `0`
- if the last number has been spoken before, the next number is the difference between the current turn and the previous turn it was spoken

Example starting numbers:

    0,3,6

The solver must determine which number is spoken at a specific turn count.

Part 1 asks for the number spoken on turn `2020`.  
Part 2 asks for the number spoken on turn `30000000`.

---

## 🧩 Part 1

Determine the number spoken on turn `2020`.

### 💡 Approach

- Parse the starting numbers
- Track when each number was last spoken
- Replay the memory game turn by turn
- Stop when turn `2020` is reached
- Return the last number spoken

---

## 🧩 Part 2

Determine the number spoken on turn `30000000`.

### 💡 Approach

- Reuse the same memory game logic
- Use an efficient lookup structure to track previous turns
- Continue the sequence until turn `30000000`
- Return the final spoken number

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Rambunctious Recitation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates the game solver
- runs it until turn `2020`

For Part 2:

- creates the same solver
- runs it until turn `30000000`

---

### Input Parsing

The input is usually:

- a single line
- comma-separated integers

Example:

    0,3,6

Typical parsing flow:

- read the first input line
- split on commas
- convert each token to an integer
- store the starting sequence

---

### Core Game Rule

At each turn:

- look at the previously spoken number
- if it has not been spoken before:
  - the next number is `0`
- otherwise:
  - the next number is the gap between the current turn and the last turn it appeared

So the solver needs to remember:

- the last spoken number
- when each number was previously seen

---

### Tracking Spoken Numbers

The efficient way to solve this is to store:

- a mapping from number to the most recent turn it was spoken

This allows each turn to be processed in constant time.

High-level flow:

- seed the map with the starting values, usually excluding the final starting number
- keep the last starting number as the current spoken value
- for each new turn:
  - check whether the current value has been seen before
  - compute the next value
  - update the map with the current turn for the current value
  - continue

---

### Part 1 Simulation

For the silver solution:

- run the memory game until turn `2020`
- return the spoken number at that point

This is small enough that performance is not a concern.

---

### Part 2 Simulation

For the gold solution:

- run the exact same algorithm until turn `30000000`

Because this is a much larger number of turns:

- a brute force list scan would be too slow
- a dictionary or array-backed lookup is the practical solution

The puzzle is really about scaling the same logic efficiently.

---

### Example Walkthrough

Starting numbers:

    0,3,6

Turns begin as:

    1: 0
    2: 3
    3: 6

Then:

- `6` is new, so next is `0`
- `0` was previously spoken on turn `1`, so next is `3`
- `3` was previously spoken on turn `2`, so next is `3`
- `3` was just spoken on the previous tracked turn, so next is `1`

And so on until the required target turn is reached.

---

## 🛠 Implementation Notes

- Input is parsed from a comma-separated line
- The same game logic can solve both parts
- Performance depends on fast previous-turn lookup
- Part 1 and Part 2 differ only in the target turn count
- A dictionary is a natural fit for sparse tracking
- An array can be even faster if the implementation chooses it

---

## 🧪 Behaviour Summary

Given a starting number sequence:

- the solver parses the opening values
- plays the memory game one turn at a time
- tracks when each number was last seen
- Part 1 stops at turn `2020`
- Part 2 stops at turn `30000000`
- the final spoken number is returned for each mode

---

## 🚀 Key Takeaways

- Good example of turning a simple rule into an efficient iterative solver
- Naive replay becomes too slow for large turn counts
- Tracking the last-seen turn is the key optimisation
- Both puzzle parts share the same logic
- The challenge is more about data structures than maths

---

## 🔗 References

- https://adventofcode.com/2020/day/15