# 🎄 Advent of Code 2020 - Day 13: Shuttle Search

## 📜 Puzzle Overview

This puzzle works with a bus schedule.

The input has two lines:

- your earliest possible departure time
- a comma-separated list of bus IDs, where `x` means no bus in that slot

Example structure:

```text
939
7,13,x,x,59,x,31,19
```

The solver handles two different tasks:

- Part 1 finds the earliest bus you can catch after a given time
- Part 2 finds the earliest timestamp where buses depart at offsets matching their positions in the list

---

## 🧩 Part 1

Find the bus you can catch the soonest, then return:

```text
bus ID * waiting time
```

### 💡 Approach

- Parse the earliest departure time from the first input line
- Parse all real bus IDs from the second line
- Ignore every `x`
- For each bus ID:
  - repeatedly add the bus ID to itself until the value is greater than the earliest departure time
  - store that next departure time
- Find the smallest next departure time
- Work out how long the wait is
- Return bus ID multiplied by wait time

---

## 🧩 Part 2

Find the earliest timestamp where every listed bus departs at the correct offset.

### 💡 Approach

- Parse the full bus list
- Convert each `x` to `0`
- Start with:
  - `time = first bus ID`
  - `increment = first bus ID`
- Process each remaining real bus one at a time
- For each bus:
  - keep increasing `time` by `increment`
  - stop when that bus satisfies its required offset rule
- Once aligned, multiply `increment` by that bus ID
- Continue until all buses are aligned
- Return the final timestamp

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Shuttle Search`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
ShuttleSearch.EarliestBusId(this.Input)
```

For Part 2:

```text
ShuttleSearch.EarliestTimestamp(this.Input)
```

---

### `ShuttleSearch.cs`

This class contains the full puzzle logic.

It provides two public methods:

- `EarliestBusId(string[] input)`
- `EarliestTimestamp(string[] input)`

---

### Part 1 Input Parsing

`EarliestBusId(...)` begins by parsing:

```text
input[0]
```

into:

```text
int earliestDepature
```

Then it parses the bus list from:

```text
input[1]
```

using:

- `Split(",")`
- filtering out `x`
- converting the remaining values to integers

So Part 1 only works with real bus IDs.

---

### Part 1 Departure Search

The method creates:

```text
List depatures = new();
```

Then for each bus ID it does:

- start `value = 0`
- repeatedly add the bus ID until `value > earliestDepature`
- store that resulting departure time in `depatures`

At a high level this means:

- compute the first departure strictly after the target time for every bus
- keep all those candidate departure times
- pick the smallest one

---

### Part 1 Final Result

After collecting all departure times, the solver:

- finds the minimum departure value
- finds that value's index in the list
- uses the same index to get the matching bus ID
- calculates:

```text
timeToWait = min - earliestDepature
```

Then returns:

```text
busIds[index] * timeToWait
```

So the silver answer is the chosen bus ID multiplied by the waiting time.

---

### Part 2 Input Parsing

`EarliestTimestamp(...)` parses the second input line differently.

It keeps the full original position layout by converting:

- real bus IDs to `long`
- `x` entries to `0`

So the resulting list preserves offsets exactly.

Example idea:

```text
7,13,x,x,59
```

becomes:

```text
7,13,0,0,59
```

That allows the solver to use list positions as required departure offsets.

---

### Part 2 Incremental Alignment

The method creates:

```text
long time = busIds[0];
long increment = busIds[0];
```

Then loops through the rest of the bus list.

For each real bus value:

- skip it if the value is `0`
- otherwise keep increasing `time` by `increment`
- stop only when:

```text
(time + busIds.IndexOf(value)) % value == 0
```

That condition means the bus departs at exactly the offset matching its position in the input list.

---

### Growing the Step Size

Once a bus has been aligned, the solver does:

```text
increment *= value;
```

This is the key optimisation.

It means:

- every future timestamp candidate already preserves all previous bus alignments
- the search only needs to consider times that keep earlier constraints satisfied

So instead of restarting from scratch for every bus, the method builds the solution one aligned bus at a time.

---

### Part 2 Return Value

After all buses have been processed, the method returns:

```text
time
```

So the gold answer is the earliest timestamp where every real bus departs at the correct positional offset.

---

## 🛠 Implementation Notes

- The variable names are spelled `earliestDepature` and `depatures` in the implementation
- Part 1 ignores `x` entries completely
- Part 2 keeps `x` entries as `0` so offsets remain intact
- Part 2 uses `long` because the timestamp grows large
- The solver uses progressive alignment rather than brute-forcing all timestamps
- The search step grows by multiplying with each aligned bus ID

---

## 🧪 Behaviour Summary

Given an earliest departure time and a bus list:

- Part 1 finds the first available departure for each real bus
- picks the soonest one
- returns bus ID multiplied by wait time

For Part 2:

- preserve every bus position from the input
- align buses one by one against the timestamp
- increase the step size after each successful alignment
- return the first timestamp satisfying all offsets

---

## 🚀 Key Takeaways

- Part 1 is a straightforward next-multiple search
- Part 2 is an incremental constraint-alignment solver
- Replacing `x` with `0` preserves bus offsets cleanly
- Multiplying the increment after each match makes the large search practical
- Nice example of solving modular timing rules without brute force

---

## 🔗 References

- https://adventofcode.com/2020/day/13