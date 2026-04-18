# 🎄 Advent of Code 2019 - Day 23: Category Six

## 📜 Puzzle Overview

This puzzle simulates a network of `50` Intcode computers.

Each computer has:

- its own input queue
- its own output queue
- a fixed network address from `0` to `49`

Packets are sent across the network as groups of three values:

- destination address
- `X`
- `Y`

There is also a special destination:

- `255`

Part 1 returns the first `Y` value sent to address `255`. Part 2 enables NAT handling and returns the first `Y` value delivered to computer `0` twice in a row.

---

## 🧩 Part 1

Determine the first `Y` value sent to address `255`.

### 💡 Approach

- Create `50` Intcode computers
- Give each computer its own network address as initial input
- Run each one once to initialise it
- Repeatedly cycle through all computers
- If a computer has no input waiting:
  - enqueue `-1`
- Run the computer
- Read its outputs in groups of three:
  - destination
  - `X`
  - `Y`
- If the destination is `255`:
  - immediately return `Y`

This is the silver answer.

---

## 🧩 Part 2

Enable NAT support and determine the first `Y` value the NAT sends to computer `0` twice in a row.

### 💡 Approach

- Reuse the same `50`-computer network
- Track the most recent packet sent to address `255`
- Treat that packet as the NAT packet
- Continue running the network normally
- Detect when the network becomes idle
- When idle:
  - send the stored NAT packet to computer `0`
- Track the `Y` values sent to computer `0`
- Return the first NAT `Y` value that repeats consecutively

This is the gold answer.

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Category Six`
- Loads the puzzle input
- Uses the first input line as the Intcode program

For Part 1:

- Creates `new CategorySix(this.Input[0])`
- Calls `Run()`

For Part 2:

- Creates `new CategorySix(this.Input[0])`
- Calls `Run(true)`

So both parts use the same solver with a boolean flag to enable NAT behaviour.

---

### `CategorySix.cs`

This class contains the full network simulation.

It stores:

- `Nics`
- `Nat`

The constructor:

- creates a list of `50` Intcode CPUs
- gives each CPU its address as input
- runs each CPU once after assigning the address

So the whole network is initialised up front before packet processing begins.

---

### Network Initialisation

The constructor loops from:

    0
    to
    49

On each iteration it:

- adds `new IntcodeCpu(program)`
- enqueues the loop index as that computer's address
- calls `Run()`

So each NIC boots with its own unique address exactly once.

---

### NIC Storage

The network is stored as:

- `List<IntcodeCpu>`

This means each machine keeps its own:

- input queue
- output queue
- execution state

The solver simply iterates over that list repeatedly to simulate the full network.

---

### NAT Storage

The NAT packet is stored as:

- `Vector? Nat`

This holds the most recent packet sent to address `255`, specifically:

- `X`
- `Y`

That stored packet is only used in Part 2 when NAT mode is enabled.

---

### Main Network Loop

`Run(bool runWithNat = false)` performs the full simulation.

It initialises:

- `result = 0`
- `lastZero = -1`

Then it loops while:

    result == 0

Inside that loop it starts with:

- `idle = true`

Then it processes every CPU in `Nics`.

---

### Handling Empty Input

Before running a CPU, the solver checks:

- `cpu.Input.Count == 0`

If true, it enqueues:

    -1

This matches the network rule that an idle machine receives `-1` when no packet data is waiting.

---

### Reading Output Packets

After `cpu.Run()`, the solver processes output while any values remain.

Outputs are consumed in groups of three:

- `address`
- `x`
- `y`

Logically that means each complete packet is:

    destination, X, Y

Whenever any output is produced, the loop also sets:

- `idle = false`

because the network is clearly active.

---

### Normal Packet Routing

For ordinary packet destinations, the solver forwards the packet directly to the target NIC.

It does this with:

- `this.Nics[address].Input.Enqueue(x)`
- `this.Nics[address].Input.Enqueue(y)`

So packets are delivered by appending their `X` and `Y` values to the recipient's input queue.

---

### Address `255`

When a packet is sent to:

    255

the solver behaves differently depending on the mode.

For Part 1:

- it immediately returns `y`

For Part 2:

- it stores the packet in `Nat`
- it does not forward it to a normal NIC

So address `255` is either:

- the direct answer source
- or the NAT packet source

depending on `runWithNat`.

---

### Tracking Address `0`

The implementation also tracks packets sent to address `0` using:

- `lastZero`

Whenever a normal packet is routed to address `0`, the code checks:

- if `y == lastZero`
  - return `y`

Otherwise it updates:

- `lastZero = y`

So repeated `Y` delivery to NIC `0` is treated as the terminating condition.

---

### Idle Network Detection

After all NICs have been processed, the solver checks whether the network stayed idle for the whole pass.

If:

- no CPU produced output
- and `idle` remains `true`

then the NAT logic runs.

This is how the implementation detects a fully quiet network cycle.

---

### NAT Injection

When the network is idle, the solver:

- checks whether `this.Nat?.Y == lastZero`
  - if so, returns that `Y`
- otherwise updates:
  - `lastZero = this.Nat?.Y ?? 0`
- enqueues the NAT packet into computer `0`:

      this.Nics[0].Input.Enqueue(this.Nat?.X ?? 0)
      this.Nics[0].Input.Enqueue(this.Nat?.Y ?? 0)

So the NAT wakes the network by resending its stored packet to NIC `0`.

---

### Part 1 Return Value

When NAT mode is not enabled, `Run()` returns:

- the first `Y` value from a packet addressed to `255`

So the silver answer comes directly from the first special packet observed.

---

### Part 2 Return Value

When NAT mode is enabled, `Run(true)` returns:

- the first `Y` value sent to computer `0` twice in a row

This can happen either:

- during normal routing to address `0`
- or during NAT resend handling when the network is idle

In practice, the NAT path is the intended Part 2 mechanism in this implementation.

---

## 🛠 Implementation Notes

- The solver creates exactly `50` Intcode NICs
- Each NIC is initialised with its network address
- Empty input queues receive `-1`
- Outputs are always processed as triples
- Address `255` is handled specially
- NAT mode stores the latest `255` packet
- Idle detection is based on whether any NIC produced output during a full pass
- Repeated `Y` delivery to address `0` ends the search

---

## 🧪 Behaviour Summary

Given one Intcode program:

- the solver boots a `50`-computer network
- each computer receives its own address
- the network runs by feeding idle inputs and routing packet triples
- Part 1 stops at the first packet sent to `255`
- Part 2 stores `255` packets in NAT memory
- when the network goes idle, NAT sends its stored packet to computer `0`
- the final result is either:
  - the first `Y` sent to `255`
  - or the first repeated NAT-delivered `Y` seen at address `0`

---

## 🚀 Key Takeaways

- Nice example of simulating a packet-switched Intcode network
- The solver keeps the design compact by iterating over a list of NICs
- Output triples naturally model packet routing
- Part 1 is a direct packet interception problem
- Part 2 adds idle detection and NAT replay logic
- The same `Run` method supports both puzzle parts with a mode flag

---

## 🔗 References

- https://adventofcode.com/2019/day/23