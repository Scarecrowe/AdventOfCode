# 🎄 Advent of Code 2019 - Day 09: Sensor Boost

## 📜 Puzzle Overview

This puzzle runs an Intcode program that now supports relative parameter mode.

The input is a single comma-separated Intcode program.

The solver wraps that program in a shared `IntcodeCpu` and runs it in two different modes:

- test mode with input `1`
- boost mode with input `2`

Part 1 returns the first output produced by the program in test mode.

Part 2 returns the first output produced by the program in boost mode.

---

## 🧩 Part 1

Run the BOOST program in test mode and return the keycode it produces.

### 💡 Approach

- Parse the input program into an `IntcodeCpu`
- Enqueue the input value `1`
- Run the Intcode computer
- Read the first value from the output queue
- Return that value as the silver answer

---

## 🧩 Part 2

Run the BOOST program in boost mode and return the distress signal coordinates.

### 💡 Approach

- Parse the same input program into an `IntcodeCpu`
- Enqueue the input value `2`
- Run the Intcode computer
- Read the first value from the output queue
- Return that value as the gold answer

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sensor Boost`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SensorBoost(this.Input[0])`
- Calls `RunTestMode()`

For Part 2:

- Creates `new SensorBoost(this.Input[0])`
- Calls `RunBoost()`

---

### `SensorBoost.cs`

This class is a very small wrapper around the shared Intcode computer.

It stores:

- `Cpu`

The constructor:

- receives the puzzle input as a single string
- creates `new IntcodeCpu(input)`

It then exposes two puzzle-specific methods:

- `RunTestMode()`
- `RunBoost()`

---

### Running Test Mode

`RunTestMode()` performs the Part 1 logic.

It does this:

    this.Cpu.Input.Enqueue(1);
    this.Cpu.Run();
    return this.Cpu.Output.Dequeue();

So the solver:

- sends `1` into the input bus
- runs the full Intcode program
- returns the first output value

---

### Running Boost Mode

`RunBoost()` performs the Part 2 logic.

It does this:

    this.Cpu.Input.Enqueue(2);
    this.Cpu.Run();
    return this.Cpu.Output.Dequeue();

So the gold solution uses the exact same machine, but with a different input value.

---

### `IntcodeCpu.cs`

This is the shared Intcode implementation used by the puzzle.

It stores:

- `Memory`
- `Program`
- `Input`
- `Output`
- `State`

It also keeps internal execution state such as:

- `InstructionPointer`
- `RelativeBase`
- `Opcodes`

The constructor:

- loads the program into memory
- initialises the relative base to `0`
- creates input and output queues
- builds the opcode handlers

---

### Main Execution Loop

`Run()` is responsible for processing the Intcode program.

At a high level it does:

- mark the CPU as processing
- read the opcode at the current instruction pointer
- dispatch to the correct opcode handler
- continue until termination
- stop early if the machine is waiting for input

The main loop repeatedly reads:

    this.Memory.Read(this.InstructionPointer)

and interprets that value as an `IntcodeOpcode`.

If opcode `99` is reached, the CPU enters the terminated state.

---

### Parameter Modes

The CPU supports three parameter modes when reading values.

`ReadValue(...)` handles:

- position mode
- immediate mode
- relative mode

Logically those behave like this:

    Position  -> read from the address stored in the parameter
    Immediate -> read the parameter value directly
    Relative  -> read from relativeBase + parameter

This relative mode support is the key new feature required by Day 9.

---

### Writing Values

`WriteValue(...)` also supports multiple target modes.

It can write using:

- position mode
- immediate mode
- relative mode

For relative writes, the destination address is based on:

    this.RelativeBase + parameter

That allows the program to address memory relative to the current base offset.

---

### Relative Base Adjustment

The shared CPU includes a dedicated opcode handler for relative base updates.

The `Relative` opcode does:

    this.RelativeBase += this.ReadValue(opcode.A);

This matches the Day 9 requirement that opcode `9` shifts the relative base during execution.

---

### Supported Opcodes

`BuildOpcodes()` registers handlers for all supported Intcode instructions, including:

- add
- multiply
- input
- output
- jump-if-true
- jump-if-false
- less-than
- equal
- relative base adjustment
- terminate

So Day 9 is solved mostly by reusing the upgraded Intcode machine rather than by adding puzzle-specific logic in the wrapper itself.

---

### Input and Output Buses

The CPU uses queues for communication.

- `Input` is an `IntcodeBus`
- `Output` is an `IntcodeBus`

For this puzzle the wrapper simply:

- enqueues one input value
- runs the program
- dequeues one result

That keeps the `SensorBoost` class very compact.

---

## 🛠 Implementation Notes

- The puzzle input is treated as one raw Intcode program string
- Part 1 uses input `1`
- Part 2 uses input `2`
- The heavy lifting is done by the shared `IntcodeCpu`
- Relative mode is supported for both reading and writing
- The CPU tracks a mutable `RelativeBase`
- Outputs are returned through a queue and the solver takes the first value produced

---

## 🧪 Behaviour Summary

Given a BOOST Intcode program:

- the solver loads it into a shared Intcode CPU
- Part 1 sends input `1` and runs test mode
- Part 2 sends input `2` and runs boost mode
- the CPU executes using position, immediate, and relative parameter modes
- opcode `9` adjusts the relative base during execution
- each part returns the first output value produced by the program

---

## 🚀 Key Takeaways

- Nice example of puzzle logic being almost entirely delegated to a reusable virtual machine
- Day 9 mainly extends the Intcode computer with relative addressing
- The wrapper class stays very small because the shared CPU already exposes clean input and output queues
- Relative base support makes the Intcode implementation significantly more flexible
- Both puzzle parts differ only by the single input value provided to the same program

---

## 🔗 References

- https://adventofcode.com/2019/day/9