# 🎄 Advent of Code 2019 - Day 5: Sunny with a Chance of Asteroids

## 📜 Puzzle Overview

This puzzle builds directly on the Intcode computer from Day 2, but significantly expands its capabilities.

You are now running a diagnostic program using the **Thermal Environment Supervision Terminal (TEST)**, which requires new features in your Intcode interpreter.

New functionality includes:

- input handling
- output handling
- parameter modes (position vs immediate)
- additional conditional and comparison opcodes

---

## 🧩 Part 1

Run the diagnostic program using system ID `1`.

### 💡 Approach

- Parse input into an integer array (memory)
- Extend the Intcode interpreter to support:
  - Opcode `3` → input
  - Opcode `4` → output
- Provide input value `1` when requested
- Execute the program until halt
- Ignore all outputs except the final diagnostic code

---

## 🧩 Part 2

Run the diagnostic program using system ID `5`.

### 💡 Approach

- Reuse the same interpreter
- Add support for new opcodes:
  - `5` → jump-if-true
  - `6` → jump-if-false
  - `7` → less-than
  - `8` → equals
- Provide input value `5`
- Execute program
- Return the final output value

---

## 🧠 Code Breakdown

### `Day05.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sunny with a Chance of Asteroids`
- Loads the input
- Executes both parts

For Part 1:

- Runs Intcode with input `1`
- Captures diagnostic output

For Part 2:

- Runs Intcode with input `5`
- Uses extended opcode logic

---

### Intcode Enhancements

Day 5 extends the original Intcode computer with:

#### Input (Opcode 3)

    memory[position] = input

- Takes a single input value
- Stores it at the given memory position

---

#### Output (Opcode 4)

    output = value

- Outputs the value of a parameter

---

### Parameter Modes

This is the biggest conceptual change.

Each parameter can now be interpreted in two ways:

- **Position mode (0)** → treat parameter as an address
- **Immediate mode (1)** → treat parameter as a literal value

Example:

    1002,4,3,4,33

Breakdown:

    opcode = 2 (multiply)
    param1 mode = 0 (position)
    param2 mode = 1 (immediate)

So:

- first parameter → value at address 4
- second parameter → literal 3

---

### Instruction Decoding

Instructions now embed both:

- opcode (last 2 digits)
- parameter modes (preceding digits)

Example:

    ABCDE
    1002

- DE → opcode
- C → mode of param1
- B → mode of param2
- A → mode of param3

---

### New Opcodes (Part 2)

#### Jump-if-True (Opcode 5)

    if param1 != 0 → pointer = param2

---

#### Jump-if-False (Opcode 6)

    if param1 == 0 → pointer = param2

---

#### Less Than (Opcode 7)

    memory[output] = (param1 < param2) ? 1 : 0

---

#### Equals (Opcode 8)

    memory[output] = (param1 == param2) ? 1 : 0

---

### Instruction Pointer Behaviour

Unlike Day 2:

- instruction size is no longer always 4
- jumps can directly modify the instruction pointer
- execution flow becomes dynamic

---

### Input / Output Flow

Programs can now:

- request input multiple times
- produce multiple outputs

Example:

    3,0,4,0,99

- takes input
- immediately outputs it
- halts

---

## 🛠 Implementation Notes

- Instruction parsing must separate opcode and parameter modes
- Default parameter mode is `0` (position)
- Memory is still mutated in-place
- Input should be injectable (not hardcoded deep in logic)
- Output should be captured, not just printed
- Instruction pointer may jump or increment depending on opcode

---

## 🧪 Behaviour Summary

Given an Intcode program:

- it now supports input/output operations
- parameters can behave differently depending on mode
- execution flow can branch using jumps
- comparison operations allow decision making

Part 1:

- acts as a basic diagnostic run
- outputs validation codes

Part 2:

- introduces conditional logic
- produces a final diagnostic result based on input

---

## 🚀 Key Takeaways

- Major evolution of the Intcode computer
- Introduction of parameter modes adds complexity to parsing
- Execution is no longer strictly linear due to jumps
- Enables conditional logic and branching
- Forms the foundation for even more advanced Intcode puzzles later

---

## 🔗 References

- https://adventofcode.com/2019/day/5