# 🎄 Advent of Code 2019 - Day 21: Springdroid Adventure

## 📜 Puzzle Overview

This puzzle runs an Intcode-powered springdroid using ASCII script commands.

The input is a single comma-separated Intcode program.

The springdroid is controlled by a small Boolean instruction set using sensor registers such as:

- `A`
- `B`
- `C`
- `D`

and writable registers:

- `T`
- `J`

The solver loads the program into a shared `IntcodeCpu`, sends a fixed springscript program as ASCII input, then returns the non-ASCII hull damage value produced by the program. The puzzle uses `WALK` for Part 1 and `RUN` for Part 2.

---

## 🧩 Part 1

Run the springdroid in walking mode and return the hull damage value.

### 💡 Approach

- Load the Intcode program into an `IntcodeCpu`
- Start the ASCII prompt by running the CPU once
- Send a short springscript program that jumps when:
  - there is a hole at `A`
  - or there is a hole at `C`
  - and landing tile `D` is safe
- Send the final `WALK` command
- Run the program
- Read the first output value greater than `255`
- Return that value as the silver answer

---

## 🧩 Part 2

Run the springdroid in extended sensor mode and return the hull damage value.

### 💡 Approach

- Load the same Intcode program into an `IntcodeCpu`
- Start the ASCII prompt
- Send a longer springscript program
- Keep the same safe-landing requirement on `D`
- Add an extra forward-safety check using `H`
- Also allow jumping when `B` is unsafe
- Send the final `RUN` command
- Run the program
- Read the first output value greater than `255`
- Return that value as the gold answer

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Springdroid Adventure`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SpringdroidAdventure(this.Input[0])`
- Calls `Run()`

For Part 2:

- Creates `new SpringdroidAdventure(this.Input[0])`
- Calls `RunWihtInreasedSensor()`

---

### `SpringdroidAdventure.cs`

This class is a compact wrapper around the shared Intcode computer.

It stores:

- `Cpu`

The constructor:

- receives the puzzle input as one raw program string
- creates `new IntcodeCpu(program)`

It then exposes two puzzle-specific methods:

- `Run()`
- `RunWihtInreasedSensor()`

So most of the actual work is done by feeding ASCII commands into the shared CPU rather than by building a large puzzle-specific simulation.

---

### Starting the Springdroid Prompt

Both puzzle parts begin with:

    this.Cpu.RunAsciiCommand();

This runs the Intcode machine until it reaches the point where it is ready to accept springscript input.

The shared CPU supports ASCII command execution through:

- `RunAsciiCommand(string command = "", bool print = false)`
- `StringToAscii(string value)`

`StringToAscii(...)` converts each character into its ASCII value and appends newline `10`, which matches the puzzle's command format.

---

### Part 1 Springscript

`Run()` sends this instruction sequence:

    NOT C J
    AND D J
    NOT A T
    OR T J
    WALK

At a high level this means:

- jump if tile `C` is a hole and tile `D` is ground
- also jump immediately if tile `A` is a hole

So the final jump decision becomes:

- `(!C && D) || !A`

The method then returns:

    this.Cpu.Output.FirstOrDefault(c => c > 255);

That means it ignores ordinary ASCII text output and returns the hull damage value, which the puzzle reports as a large integer outside the normal ASCII range.

---

### Part 2 Springscript

`RunWihtInreasedSensor()` sends this instruction sequence:

    NOT C J
    AND D J
    AND H J
    NOT B T
    AND D T
    OR T J
    NOT A T
    OR T J
    RUN

At a high level this means:

- jump if `C` is a hole, `D` is safe, and `H` is safe
- or jump if `B` is a hole and `D` is safe
- or jump immediately if `A` is a hole

So the final logic becomes:

- `(!C && D && H) || (!B && D) || !A`

The only structural difference from Part 1 is that the script is longer and ends with:

    RUN

instead of:

    WALK

This matches the puzzle's extended sensor mode for Part 2.

---

### Sending ASCII Instructions

The solver feeds each springscript line into the Intcode input queue with:

    this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("..."));

So each textual instruction is converted into a newline-terminated ASCII sequence before execution.

This keeps the wrapper very simple:

- queue the instructions
- run the CPU
- inspect the output queue

---

### Reading the Result

Both puzzle methods return:

    this.Cpu.Output.FirstOrDefault(c => c > 255);

This is important because the Intcode program can output two different kinds of values:

- normal ASCII characters for text prompts and visual output
- one large numeric hull damage value on success

The solver specifically selects the first output above the ASCII range, which is the actual puzzle answer.

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

The constructor loads the program, initialises the buses, and builds the opcode handlers. The CPU supports standard Intcode execution through `Run()` and ASCII-oriented interaction through `RunAsciiCommand(...)`.

---

### ASCII Helper Support

The shared CPU adds a thin wrapper for these text-based puzzles.

`RunAsciiCommand(...)` does this logically:

    if command was provided:
        enqueue ASCII for that command
    run the Intcode program
    optionally print and clear ASCII output

That makes it easy for the Day 21 wrapper to interact with the springdroid as if it were sending text commands to a terminal.

---

## 🛠 Implementation Notes

- The puzzle input is treated as one raw Intcode program string
- The solver uses a dedicated `SpringdroidAdventure` wrapper
- Commands are sent as ASCII values with a trailing newline
- Part 1 ends with `WALK`
- Part 2 ends with `RUN`
- Both parts scan the output queue for the first value greater than `255`
- The shared Intcode CPU handles all program execution details
- The Part 2 method name is `RunWihtInreasedSensor` in the implementation

---

## 🧪 Behaviour Summary

Given an ASCII-capable Intcode springdroid program:

- the solver loads it into the shared Intcode CPU
- runs the initial prompt
- sends a fixed springscript program line by line
- Part 1 uses `WALK`
- Part 2 uses `RUN`
- the CPU executes the script and produces text output plus, on success, a large hull damage value
- the solver returns that non-ASCII damage value as the final answer

---

## 🚀 Key Takeaways

- Nice example of using a reusable Intcode machine as a text-driven virtual computer
- The puzzle-specific class stays very small because the logic is encoded as springscript
- Part 1 and Part 2 differ mainly in the Boolean jump logic and the final movement command
- The output handling is clean because ASCII text and numeric answer values are separated by range
- The shared `RunAsciiCommand()` and `StringToAscii()` helpers make these terminal-like Intcode puzzles much easier to solve

---

## 🔗 References

- https://adventofcode.com/2019/day/21