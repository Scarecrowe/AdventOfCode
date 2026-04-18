# 🎄 Advent of Code 2017 - Day 17: Spinlock

## 📜 Puzzle Overview

This puzzle simulates a circular buffer that repeatedly inserts increasing numbers after stepping forward by a fixed amount.

The input is a single integer step size.

The solver uses two different approaches:

- Part 1 builds the full circular buffer explicitly
- Part 2 avoids building the full buffer and only tracks the value that appears immediately after `0`

Part 1 asks for the value that appears immediately after a specific value once the insertions are complete. Part 2 asks for the value that ends up immediately after `0` after a much larger number of insertions.

---

## 🧩 Part 1

Determine the value immediately after `2017` once `2017` insertions have been performed.

### 💡 Approach

- Start with a circular buffer containing only `0`
- Track the current position
- For each value from `1` to `2017`:
  - move forward by the input step size
  - insert the new value immediately after that position
  - update the current position to the inserted location
- After all insertions, locate `2017`
- Return the value immediately after it

---

## 🧩 Part 2

Determine the value immediately after `0` after `50,000,000` insertions.

### 💡 Approach

- Do not build the full circular buffer
- Track only:
  - the current logical buffer size
  - the current position
  - the latest value inserted at position `1`
- For each insertion:
  - compute the new position mathematically
  - if the new position is `1`, record that inserted value as the current answer
- Return the last recorded value

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Spinlock`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `SpinLock.Run(2017, this.Input[0].ToInt(), 2017)`

For Part 2:

- Calls `SpinLock.RunAngry(50000000, this.Input[0].ToInt())`

---

### `SpinLock.cs`

This class contains both solution paths.

It exposes:

- `Run(int cycles, int steps, int valueAfter)`
- `RunAngry(int cycles, int steps)`

The first method performs the full buffer simulation. The second method uses a reduced mathematical approach for the much larger cycle count.

---

### Part 1 Buffer Simulation

`Run(int cycles, int steps, int valueAfter)` starts with:

    List<int> values = new() { 0 };

It also tracks:

- `position`

For each value from `1` through `cycles`:

- compute the insertion index
- insert the current value into the list
- update the current position

The insertion index is calculated as:

    ((position + steps) % values.Count) + 1

This means the solver:

- steps forward through the current circular buffer
- wraps around with modulo
- inserts immediately after the landed position

---

### Returning the Value After a Target

Once all insertions are complete, Part 1 finds the requested target value in the list and returns the item immediately after it.

That is done with:

    values[values.IndexOf(valueAfter) + 1]

For Day 17, the target passed in is:

- `2017`

So the silver answer is the value immediately after `2017` in the completed spinlock buffer.

---

### Part 2 Optimised Logic

`RunAngry(int cycles, int steps)` avoids storing the whole buffer.

It tracks:

- `current`
- `position`
- `result`

The idea is that Part 2 only cares about the value immediately after `0`, which is the value at position `1`.

So instead of inserting into a list, the method only computes where each insertion would land.

The update step is:

    position = ((position + steps) % current) + 1

If that computed position is:

    1

then the newly inserted value becomes the new candidate result.

That value is stored with:

    result = i + 1

---

### Why Part 2 Works Without a Full Buffer

The value after `0` only changes when a new insertion lands directly in slot `1`.

So the solver does not need to know the entire buffer contents. It only needs to know:

- the current logical size
- the insertion position
- whether the newest value landed immediately after `0`

This makes the 50,000,000-cycle version practical without the memory cost of storing every inserted number.

---

## 🛠 Implementation Notes

- Part 1 uses a real `List<int>` buffer
- Insertions are done with `values.Insert(index, i)`
- Position updates use modular arithmetic to wrap correctly through the circular structure
- Part 2 does not allocate a large buffer
- Part 2 tracks only the most recent value inserted at position `1`
- The gold solution is heavily optimised compared with the silver solution

---

## 🧪 Behaviour Summary

Given a step size:

- the solver repeatedly advances through a conceptual circular buffer
- each new number is inserted immediately after the current stepped position
- Part 1 stores the entire structure and retrieves the value after `2017`
- Part 2 tracks only whether a new insertion lands immediately after `0`
- the final result depends on position math rather than linked-list style traversal

---

## 🚀 Key Takeaways

- Good example of modelling a circular insertion process with modular arithmetic
- Part 1 uses a direct simulation with a list
- Part 2 identifies that the full structure is unnecessary for the required output
- Tracking only position `1` makes the large input size manageable
- The two parts solve the same process with very different performance strategies

---

## 🔗 References

- https://adventofcode.com/2017/day/17