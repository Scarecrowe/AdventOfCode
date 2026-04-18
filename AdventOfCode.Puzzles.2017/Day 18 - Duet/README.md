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
# 🎄 Advent of Code 2017 - Day 18: Duet

## 📜 Puzzle Overview

This puzzle interprets a small assembly-like instruction set.

The input is parsed into instructions such as:

    snd X
    set a 1
    add a 2
    mul a a
    mod a 5
    rcv a
    jgz a -1

The solver supports two execution modes:

- Part 1 runs a single program and recovers the last played sound
- Part 2 runs two programs together and counts how many times program `1` sends a value

The implementation models the instruction stream with a `Duet` class and coordinates the two-program version through a `Motherboard` class.

---

## 🧩 Part 1

Determine the recovered frequency from the first `rcv` instruction that succeeds.

### 💡 Approach

- Parse the input into typed instructions
- Execute them in order using a register array
- Keep a queue of sent values
- `snd` records a value
- `rcv` returns the most recent recorded value when its condition succeeds
- `jgz` changes the instruction pointer when its first operand is greater than `0`

---

## 🧩 Part 2

Determine how many values program `1` sends before the dual-program system stops making progress.

### 💡 Approach

- Create two `Duet` instances
- Initialise register `p` to the program ID for each one
- Let each program send values into the other program's queue
- Run both programs on separate threads
- Count how many times program `1` executes `snd`
- Stop when both programs are waiting with no data to receive

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Duet`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Duet(this.Input, 0)`
- Calls `Process(null)`

For Part 2:

- Creates `new Motherboard(this.Input)`
- Calls `Run()`

---

### `InstructionType.cs`

This enum defines the supported instruction kinds:

- `Snd`
- `Set`
- `Add`
- `Mul`
- `Mod`
- `Rcv`
- `Jgz`

These are used by the parser and execution engine to dispatch instruction behaviour.

---

### `Instruction.cs`

This class parses a single instruction line.

It stores:

- `Type`
- `IsRegisterA`
- `IsRegisterB`
- `IsRegisterC`
- `ValueA`
- `ValueB`
- `ValueC`

The parser:

- reads the three-letter opcode
- maps it to an `InstructionType`
- splits the remaining operands
- detects whether each operand is a register or a literal number
- stores register operands as zero-based indexes into the register array

So a line like:

    set a 1

becomes:

- type `Set`
- first operand is register `a`
- second operand is literal `1`

Negative numeric operands are also supported.

---

### `Duet.cs`

This class contains the main execution logic.

It stores:

- `Instructions`
- `Registers`
- `Data`
- `Pointer`
- `Count`
- `Instruction`
- `Cpu`

The constructor:

- creates the message queue
- allocates a register array of length `26`
- sets register `p` to the supplied program ID
- parses every input line into an `Instruction`
- starts with pointer `0`

The registers are stored in a `long[26]`, so register names from `a` to `z` map directly to array positions.

---

### Operator Dispatch

The implementation defines one function per instruction type and stores them in a dictionary keyed by `InstructionType`.

That means execution works by:

- reading the current instruction
- looking up the matching operator
- applying that operator to the current `Duet` instance

This keeps the main execution loop compact.

---

### Operand Handling

For most instructions, the code checks whether an operand is a register or a literal.

At a high level, values are resolved like this:

- if the operand is a register, read `Registers[index]`
- otherwise use the literal numeric value directly

This allows instructions such as:

    set a 1
    add a b
    jgz 1 3
    jgz a -2

to all work through the same instruction model.

---

### `snd`

The `snd` operator:

- increments `Count`
- resolves operand `A`
- if there is no paired CPU, enqueues the value into the current program's own `Data`
- otherwise enqueues the value into the other program's `Data`

This means:

- in Part 1, sent values are recorded locally
- in Part 2, sent values are transmitted to the other program

---

### `set`, `add`, `mul`, and `mod`

These instructions all update a register using the resolved value of operand `B`.

At a high level:

- `set X Y` assigns `Y` into register `X`
- `add X Y` adds `Y` to register `X`
- `mul X Y` multiplies register `X` by `Y`
- `mod X Y` applies modulus `Y` to register `X`

All four operations write back into the register addressed by operand `A`.

---

### `rcv`

`rcv` behaves differently depending on which puzzle part is running.

For Part 1, where there is no paired CPU:

- it reads the last value currently stored in `Data`
- if that value is greater than `0`, it returns that value immediately

That returned value becomes the silver answer.

For Part 2, where a paired CPU exists:

- it waits while the current queue is empty
- if both programs are sitting on `rcv` with empty queues, it returns the current send count
- otherwise it dequeues the next incoming value and stores it in the target register

So `rcv` is both the receive instruction and the mechanism used to detect that the paired-program run has stalled.

---

### `jgz`

`jgz` resolves operands `A` and `B`.

If operand `A` is greater than `0`:

- it adds `B` to the instruction pointer
- returns `-1`

The `-1` result tells the main loop not to apply its normal pointer increment, because the jump has already moved execution.

If the jump condition is not met, execution continues normally to the next instruction.

---

### Main Execution Loop

`Process(Duet? cpu)` is the central run method.

It:

- resets `Pointer`
- resets `Count`
- stores the paired CPU reference
- loops while `Pointer` remains within the instruction list

For each step:

- load the current instruction
- execute the operator for that instruction
- if the operator returns a positive value, stop and return it
- if the operator returns `-1`, continue immediately because a jump already changed the pointer
- otherwise increment the pointer by one

If execution runs off the end of the program, it returns `Count`.

That means:

- Part 1 returns the recovered frequency
- Part 2 returns the send count from the terminating or stalled program

---

### `Motherboard.cs`

This class coordinates the two-program version.

It stores:

- `CpuA`
- `CpuB`

The constructor creates:

- `new Duet(input, 0)`
- `new Duet(input, 1)`

`Run()` then:

- starts one thread for `CpuA.Process(CpuB)`
- starts one thread for `CpuB.Process(CpuA)`
- waits for both threads to finish
- returns the result from program `1`

So the gold answer is the total number of sends performed by the program whose `p` register starts at `1`.

---

## 🛠 Implementation Notes

- Registers are stored in a fixed 26-element array
- Instructions support both register and literal operands
- Part 1 records sent values in the local queue and recovers the last one when `rcv` succeeds
- Part 2 uses two `Duet` instances connected through each other's queues
- A send counter is maintained through the `snd` instruction
- The two-program solution is coordinated with threads in `Motherboard`

---

## 🧪 Behaviour Summary

Given an instruction list:

- the solver parses each line into a typed instruction
- a single-program run executes until an `rcv` returns the recovered value
- a dual-program run passes sent values between two message queues
- each program has its own registers and instruction pointer
- the gold result is the send count from program `1`

---

## 🚀 Key Takeaways

- Good example of building a tiny assembly interpreter
- Clean separation between parsed instruction data and execution
- Part 1 and Part 2 share the same instruction engine
- `snd` and `rcv` change meaning slightly depending on whether a partner CPU exists
- The threaded `Motherboard` wrapper allows the paired-program behaviour to reuse the same `Duet` class

---

## 🔗 References

- https://adventofcode.com/2017/day/18