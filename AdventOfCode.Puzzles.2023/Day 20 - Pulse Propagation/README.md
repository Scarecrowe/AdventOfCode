# 🎄 Advent of Code 2023 - Day 20: Pulse Propagation

## 📜 Puzzle Overview

This puzzle simulates a network of modules that send pulses to one another.

Each module receives either:

- `0` → low pulse
- `1` → high pulse

The network is made up of several module types:

- broadcaster
- flip-flop
- conjunction
- button
- output

Each input line defines a module and the destinations it sends pulses to.

Part 1 repeatedly presses the button and counts how many low and high pulses are sent.

Part 2 uses the module graph structure to compute the fewest number of button presses needed for the `rx` target path.

---

## 🧩 Part 1

Press the button 1000 times and return the product of:

- total low pulses sent
- total high pulses sent

### 💡 Approach

- Parse the input into module objects
- Build the full network, including implicit output modules
- Add a synthetic `button` module that points to `broadcaster`
- Reset all module state before simulation
- For each button press:
  - enqueue an initial low pulse
  - process pulses in queue order
  - let each module react to its incoming pulse
  - enqueue any outgoing pulses to its destinations
- Count low and high pulses as they are transmitted
- Return `low * high`

---

## 🧩 Part 2

Determine the fewest number of button presses required for the `rx` target path.

### 💡 Approach

- Gather all flip-flop modules
- Build a topological ordering of modules upstream from `rx`
- Walk that ordered list
- For each flip-flop encountered:
  - find its index in the flip-flop list
  - add `2^index` to the running total
- Return the resulting press count

This implementation solves the second part through graph ordering and bit-value accumulation rather than by simulating repeated button presses until a condition is met.

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Pulse Propagation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PulsePropagation(this.Input)`
- Calls `LowAndHighMultiplied()`

For Part 2:

- Creates `new PulsePropagation(this.Input)`
- Calls `FewestPresses()`

---

### `Pulse.cs`

This class represents a single pulse travelling through the network.

It stores:

- `Source`
- `Target`
- `Value`

So each queued pulse knows:

- where it came from
- where it is going
- whether it is low (`0`) or high (`1`)

---

### `Module.cs`

This class models a module in the pulse network.

It stores:

- `Name`
- `Type`
- `Destinations`
- `On`
- `Inputs`

`On` is used by flip-flop modules.

`Inputs` is used by conjunction modules to remember the most recent pulse received from each upstream module.

---

### Module Types

The implementation defines these module types:

- `FlipFlop`
- `Conjunction`
- `Broadcaster`
- `Button`
- `Output`

The `Output` type is created automatically for destination names that appear in the input but do not have their own explicit module definition.

The `Button` type is also created manually and wired to:

- `broadcaster`

---

### Parsing Modules

`Module.FromInput(string line)` parses each input line.

It splits on:

    " -> "

Then it inspects the left side:

- if it starts with `%`, it becomes a flip-flop
- if it starts with `&`, it becomes a conjunction
- otherwise it becomes a broadcaster

The `%` or `&` prefix is removed from the stored module name.

The right side is split on:

    ", "

to produce the destination list.

---

### Building the Network

The `PulsePropagation` constructor performs several setup steps.

It:

- parses every input line into a module
- stores each module in a dictionary by name
- adds any missing destination modules as `Output`
- creates a `button` module whose only destination is `broadcaster`
- pre-populates conjunction input memory with `0` for every incoming connection
- caches all flip-flop modules into `FlipFlops`

This means the network is fully wired before either puzzle part runs.

---

### Module Behaviour

`ReceivePulse(string from, int pulse)` handles the logic for each module type.

#### Button and Broadcaster

These simply pass the incoming pulse through unchanged.

So if they receive low, they send low.
If they receive high, they send high.

#### Flip-Flop

A flip-flop behaves differently depending on the incoming pulse.

- if it receives a high pulse:
  - it ignores it
  - returns `-1`
  - sends nothing

- if it receives a low pulse:
  - it toggles `On`
  - sends high if it is now on
  - sends low if it is now off

#### Conjunction

A conjunction stores the latest pulse received from the sending module in:

- `Inputs[from]`

It then checks whether all remembered inputs are high.

- if all inputs are high:
  - it sends low
- otherwise:
  - it sends high

#### Output

The default path just returns the pulse unchanged, but since output modules have no destinations, nothing further is queued from them.

---

### Part 1 Simulation

`LowAndHighMultiplied(int iterations = 1000)` performs the pulse simulation.

Before starting, it resets module state:

- all flip-flops are turned off
- all conjunction remembered inputs are reset to `0`

Then for each button press it creates:

- `Queue<Pulse> queue = new();`

and seeds it with:

    new Pulse(ButtonName, ButtonName, 0)

This means each cycle begins by sending a low pulse into the synthetic button module.

---

### Queue Processing

While the queue is not empty:

- dequeue the next pulse
- find the target module
- call `ReceivePulse(...)`
- if the returned pulse is `-1`, do nothing further
- otherwise:
  - send that outgoing pulse to every destination
  - increment either the low or high counter
  - enqueue one new `Pulse` per destination

So the network expands naturally in breadth-first style as pulses propagate.

---

### Counting Pulses

The solver counts pulses at send time.

For each destination:

- increment `low` if the outgoing pulse is `0`
- increment `high` if the outgoing pulse is `1`

At the end of 1000 button presses it returns:

    low * high

So the silver answer is the product of total transmitted low and high pulses across the full simulation.

---

### Part 2 Logic

`FewestPresses()` does not simulate repeated pulse traffic.

Instead it works structurally.

It starts by:

- counting and indexing all flip-flops
- building a topological ordering upstream from `rx`

Then for each module in that ordered list:

- if it is not a flip-flop, skip it
- otherwise find that flip-flop's original index
- add:

    1UL << idx

to the running total

The final sum is returned as the fewest press count.

So this implementation effectively treats the relevant flip-flops as bit positions and reconstructs the result as a binary-weighted total.

---

### Topological Ordering

`TopologicalOrder(string target)` walks backward through the graph from a target module.

It uses:

- `HashSet<string> visited`
- `List<Module> order`

The local `Visit(...)` function:

- skips already visited modules
- finds all modules whose destinations include the current module name
- recursively visits those source modules
- appends the current module to the order list

After traversal, it reverses the list and returns it.

Calling:

    TopologicalOrder("rx")

therefore produces an ordered view of the dependency chain leading into `rx`.

---

## 🛠 Implementation Notes

- Modules are stored in a dictionary keyed by name
- Missing destinations are converted into `Output` modules automatically
- A synthetic `button` module is added manually
- Flip-flops ignore incoming high pulses
- Conjunction modules remember the latest pulse from each input source
- Part 1 uses queue-driven pulse propagation
- Part 1 resets module state before running
- Part 2 uses graph structure plus bit-value accumulation
- `rx` is treated as the target node for the gold calculation

---

## 🧪 Behaviour Summary

Given a pulse network:

- the input is parsed into typed modules
- destinations are wired together
- a `button` module feeds the broadcaster
- Part 1 presses the button 1000 times
- pulses are propagated through the network with a queue
- outgoing low and high transmissions are counted
- Part 2 walks backward from `rx`
- upstream flip-flops are interpreted as binary-weighted positions
- the final result is returned as the computed press total

---

## 🚀 Key Takeaways

- Nice example of modelling a signal network with typed node behaviour
- Queue-based propagation makes branching pulse flow easy to simulate
- Conjunction modules require remembered input state from multiple sources
- The silver solution is a direct network simulation
- The gold solution is a structural graph-based shortcut rather than brute-force repetition
- Flip-flops in the `rx` dependency chain are treated like bit positions in the final press count

---

## 🔗 References

- https://adventofcode.com/2023/day/20