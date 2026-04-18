# 🎄 Advent of Code 2017 - Day 13: Packet Scanners

## 📜 Puzzle Overview

This puzzle simulates a firewall made of layers, where each layer may contain a scanner moving back and forth through a fixed depth.

Each input line is parsed as a firewall layer with:

- a layer index
- a scanner depth

An input line looks like this:

    0: 3
    1: 2
    4: 4
    6: 4

The solver stores those entries as `(Layer, Depth)` pairs.

Part 1 calculates the total severity when moving through the firewall immediately. Part 2 finds the smallest delay that allows the packet to pass through without being caught at any layer.

---

## 🧩 Part 1

Determine the total severity of traversing the firewall with no delay.

### 💡 Approach

- Parse every input line into a `(layer, depth)` pair
- Iterate through each firewall layer definition
- Check whether the packet is caught at that layer
- If caught, add `layer * depth` to the running total
- Return the final severity

---

## 🧩 Part 2

Determine the fewest picoseconds to delay before starting so the packet is never caught.

### 💡 Approach

- Start testing delays from `1`
- Reuse the same severity logic with the delay applied
- If any scanner catches the packet during that delayed run, stop early
- Keep increasing the delay until a run produces severity `0`
- Return that delay

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Packet Scanners`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PacketScanners(this.Input)`
- Calls `Severity()`

For Part 2:

- Creates `new PacketScanners(this.Input)`
- Calls `Picoseconds()`

---

### `PacketScanners.cs`

This class contains the parsing and scanner detection logic.

It stores:

- `Layers`

The constructor:

- parses the input by calling `Parse(input)`

---

### Parsing the Firewall

The input is parsed into a list of tuples:

    (Layer, Depth)

Each line is split using `": "` and converted to integers.

At a high level, a line behaves like this:

- `0: 3` becomes `(0, 3)`
- `6: 4` becomes `(6, 4)`

The parsed result is stored in:

- `List<(int Layer, int Depth)>`

---

### Catch Detection

The helper method `IsCaught(int layer, int depth)` determines whether the packet is caught when it reaches a scanner.

It calculates the scanner cycle length as:

    2 * (depth - 1)

Then checks:

    (layer % cycle) == 0

If that expression is true, the scanner is at the top position when the packet arrives, so the packet is caught.

---

### Part 1 Logic

`Severity(int? delay = null)` performs the severity calculation.

It starts with:

- `int result = 0`

Then for each `(layer, depth)` in `Layers`:

- it checks `IsCaught(layer + (delay ?? 0), depth)`
- if caught, it adds `layer * depth` to `result`

When no delay is supplied, this produces the full trip severity for Part 1.

---

### Early Exit for Delayed Runs

`Severity()` also supports delayed checks for Part 2.

If a delay is provided and the packet gets caught at any layer, the method immediately returns:

    -1

That means Part 2 does not need to finish calculating the full severity for failed delays. It can reject them as soon as one collision is found.

---

### Part 2 Logic

`Picoseconds()` searches for the first safe delay.

It starts with:

    int result = 1;

Then loops while increasing the delay:

- call `this.Severity(result)`
- if the return value is `0`, that delay is safe
- otherwise increment the delay and try again

As soon as a safe delay is found, it returns that value.

---

## 🛠 Implementation Notes

- Input is stored as `(Layer, Depth)` tuples
- Scanner positions are not simulated step by step
- Instead, scanner movement is solved mathematically using cycle length
- Part 2 reuses Part 1 logic by passing a delay into `Severity()`
- Failed delayed checks return early with `-1`

---

## 🧪 Behaviour Summary

Given a list of firewall layers:

- the solver parses each layer and its scanner depth
- Part 1 checks the trip with no delay and sums all caught-layer severities
- Part 2 tests increasing delays until no layer catches the packet
- scanner timing is determined with modular arithmetic rather than full simulation

---

## 🚀 Key Takeaways

- Good example of replacing simulation with cycle-based maths
- Part 1 computes severity directly from caught layers
- Part 2 brute-forces the delay but avoids unnecessary work with early exit
- The shared `Severity()` method keeps both puzzle parts compact

---

## 🔗 References

- https://adventofcode.com/2017/day/13