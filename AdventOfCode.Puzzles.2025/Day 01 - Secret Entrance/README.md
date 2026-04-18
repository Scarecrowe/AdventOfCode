# 🎄 Advent of Code 2025 - Day 01: Secret Entrance

## 📜 Puzzle Overview

This puzzle processes a sequence of turns around a circular dial.

Each input line contains:

- a direction: `L` or `R`
- a distance as an integer value

The dial starts at position `50` and wraps around a range of `0` to `99`. A move to the left decreases the dial value, while a move to the right increases it. When the dial moves past either end, it wraps back around to the other side.

Part 1 counts how many instructions finish exactly on dial position `0`. Part 2 instead processes movement one step at a time and counts how many times the dial passes through or lands on `0` during those stepwise moves.

---

## 🧩 Part 1

Determine how many completed turns leave the dial exactly on position `0`.

### 💡 Approach

- Parse each line into a direction and distance
- Adjust the dial by the full distance in one move
- Wrap the result into the `0` to `99` range
- Count how many moves end with the dial at `0`

---

## 🧩 Part 2

Determine how many times the dial reaches or crosses position `0` when movement is processed one step at a time.

### 💡 Approach

- Reuse the same parsed turn list
- Move the dial one step at a time for each instruction
- Wrap immediately when stepping below `0` or above `99`
- Count every time the dial lands on `0`
- Also count the explicit wrap from `99` to `0` during rightward movement

---

## 🧠 Code Breakdown

### `Day1.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Secret Entrance`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SecretEntrance(this.Input)`
- Calls `Password()`

For Part 2:

- Creates `new SecretEntrance(this.Input)`
- Calls `AdvancedPassword()`

---

### `SecretEntrance.cs`

This class contains the full parsing and dial logic.

It stores:

- `Turns` as a list of direction and distance pairs
- `Dial` starting at `50`

The input is parsed immediately using `ParseTurns(input)`.

---

### Parsing the Input

`ParseTurns()` converts each input line into:

- a direction stored as `Cardinal.West` for `L`
- a direction stored as `Cardinal.East` for `R`
- a numeric distance parsed from the rest of the string

At a high level, a line behaves like this:

- `L12` becomes west, distance `12`
- `R7` becomes east, distance `7`

---

### Dial Wrapping

Part 1 uses a helper method:

    private static int Wrap(int value) => ((value % 100) + 100) % 100;

This ensures the dial always remains within the `0` to `99` range, even when moving left into negative values or right beyond `99`.

---

### Part 1 Logic

`Password()` processes each instruction as one full move.

For each turn:

- move left by subtracting the distance when the direction is west
- move right by adding the distance when the direction is east
- wrap the dial back into range
- increment the result if the dial finishes at `0`

This means Part 1 only checks the dial once per instruction, after the full distance has been applied.

---

### Part 2 Logic

`AdvancedPassword()` processes each move step by step.

For each instruction:

- determine the step direction as `-1` for west or `+1` for east
- repeat once per unit of distance
- update the dial by one step each time
- wrap manually when the dial falls below `0` or rises above `99`
- count every time the dial reaches `0`

There is also a special case during rightward wrapping:

- if the dial moves from `99` past the upper limit, it is set to `0`
- the result is incremented immediately
- the loop continues to the next step

This makes Part 2 count crossings that happen during the movement itself rather than only checking the final position of each instruction.

---

## 🛠 Implementation Notes

- Input is parsed once into a strongly structured turn list
- The dial always operates on a circular range of `100` positions
- Part 1 applies each instruction as a single wrapped move
- Part 2 simulates every individual step
- Part 1 uses a reusable wrap helper
- Part 2 uses explicit boundary handling for stepwise traversal

---

## 🧪 Behaviour Summary

Given a dial starting at `50`:

- a left move decreases the dial position
- a right move increases the dial position
- values below `0` wrap to the top of the range
- values above `99` wrap back to `0`

This means the puzzle behaves like movement around a circular 100-position dial rather than movement on a straight number line.

---

## 🚀 Key Takeaways

- Good example of solving two related parts with different movement granularity
- Part 1 treats each instruction as a single wrapped jump
- Part 2 switches to per-step simulation to count intermediate crossings
- The implementation stays compact by reusing the same parsed turn list for both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2025/day/1