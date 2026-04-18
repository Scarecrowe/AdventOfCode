# 🎄 Advent of Code 2020 - Day 10: Adapter Array

## 📜 Puzzle Overview

This puzzle works with a list of joltage adapters.

Each adapter has a rating, and adapters can connect when the joltage difference is at most:

```text
3
```

The charging outlet starts at:

```text
0
```

and the device's built-in adapter is always:

```text
max + 3
```

The solver handles two tasks:

- Part 1 counts joltage differences in a valid adapter chain
- Part 2 counts how many distinct valid arrangements are possible

---

## 🧩 Part 1

Build a valid chain and return:

```text
(number of 1-jolt differences) * (number of 3-jolt differences)
```

### 💡 Approach

- Parse the input into integers
- Sort the adapter list
- Start from joltage `0`
- Repeatedly pick the next adapter that can connect within `+3`
- Track:
  - how many `1`-jolt jumps occur
  - how many `3`-jolt jumps occur
- Start the `3`-jolt count at `1` to include the device adapter
- Return the product of the two counts

---

## 🧩 Part 2

Count how many distinct valid adapter arrangements exist.

### 💡 Approach

- Parse and sort the adapters
- Add:
  - `0` at the start
  - `max + 3` at the end
- Identify adapter positions that cannot be skipped
- Split the full chain into independent segments between those fixed points
- Use a lookup table for how many valid combinations each segment length allows
- Multiply the segment counts together

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Adapter Array`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
AdapterArray.JoltDifference(this.Input)
```

For Part 2:

```text
AdapterArray.Distinct(this.Input)
```

---

### `AdapterArray.cs`

This class contains the full solution logic.

It defines a lookup table named:

```text
Valid
```

with these values:

```text
2 -> 1
3 -> 2
4 -> 4
5 -> 7
```

This table is used in Part 2 to convert segment sizes into arrangement counts.

---

### Input Parsing

Both methods begin by converting the string input into integers.

For Part 1:

```text
List adaptors = input.ToIntList();
```

For Part 2:

```text
List adapters = input.ToIntList();
```

Then the list is sorted before any further logic is applied.

---

### Part 1 Chain Building

`JoltDifference(string[] input)` works through the sorted adapter list while maintaining:

- `joltage = 0`
- `diff1 = 0`
- `diff3 = 1`

The `diff3` count starts at `1` so the device's built-in final jump is already included.

The method then loops until all adapters have been consumed.

At each step it:

- scans the remaining adapters
- finds the first one where:

```text
adaptors[i] <= joltage + 3
```

- calculates the difference from the current joltage
- increments:
  - `diff1` if the jump is `1`
  - `diff3` if the jump is `3`
- updates the current joltage
- removes that adapter from the list

Once the list is empty, it returns:

```text
diff1 * diff3
```

---

### Part 1 Behaviour

At a high level, the Part 1 solver does this:

- start at joltage `0`
- keep attaching the next reachable adapter
- count the size of each jump
- include the final device jump automatically
- multiply the number of `1`-jolt jumps by the number of `3`-jolt jumps

---

### Part 2 Preparation

`Distinct(string[] input)` starts by:

- parsing the input into integers
- sorting the adapters
- inserting the outlet joltage at the front:

```text
adapters.Insert(0, 0);
```

- adding the device adapter at the end:

```text
adapters.Add(adapters.Max() + 3);
```

This creates the full chain boundary needed for the arrangement logic.

---

### Tracking Fixed Points

The method then builds a list named:

```text
unskippables
```

It starts with:

```text
0
```

which is the index of the charging outlet.

For each adapter position in the middle of the list, it calculates:

- `before = adapters[i] - adapters[i - 1]`
- `after = adapters[i + 1] - adapters[i]`

An adapter is marked as unskippable when:

- `before == 3`
- or `after == 3`
- or `before == 2 && after == 2`

These are positions where removing that adapter would break the chain.

After the loop:

- the final `3`-jolt difference is counted
- the last adapter index is also added to `unskippables`

---

### Segment Counting

Once all unskippable boundaries are known, the solver treats each gap between them as an independent segment.

It initialises:

```text
long result = 1;
```

Then for each neighbouring pair of unskippable indices it multiplies by:

```text
Valid[unskippables[i + 1] - unskippables[i] + 1]
```

So each segment length maps to a known number of valid internal arrangements.

The final result is the product of all segment possibilities.

---

### Why the Lookup Table Works

The implementation does not brute-force every possible adapter arrangement.

Instead, it relies on the fact that independent segments between fixed points have known valid counts.

The lookup table encodes those counts directly:

```text
2 -> 1
3 -> 2
4 -> 4
5 -> 7
```

This keeps the Part 2 solution compact and fast.

---

## 🛠 Implementation Notes

- Part 1 uses the spelling `adaptors` in the local variable name
- Part 2 uses `adapters`
- Part 1 mutates the list by removing adapters as they are chained
- Part 2 inserts the charging outlet and appends the device adapter explicitly
- The final answer for Part 2 is built multiplicatively from independent chain segments
- The `Valid` dictionary is the core shortcut that replaces brute-force enumeration

---

## 🧪 Behaviour Summary

Given a list of adapter joltage ratings:

- sort the adapters
- Part 1 builds a valid chain from `0`
- counts `1`-jolt and `3`-jolt jumps
- returns their product
- Part 2 finds fixed adapter positions that cannot be skipped
- splits the chain into segments
- multiplies the number of valid arrangements for each segment
- returns the total number of distinct valid arrangements

---

## 🚀 Key Takeaways

- Part 1 is a straightforward chain-walk with difference counting
- Part 2 avoids brute force by splitting the chain into independent segments
- Unskippable adapters act as natural boundaries
- A small lookup table is enough to compute the total arrangement count
- Nice example of turning a combinatorics problem into segment multiplication

---

## 🔗 References

- https://adventofcode.com/2020/day/10