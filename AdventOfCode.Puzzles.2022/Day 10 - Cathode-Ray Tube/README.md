# 🎄 Advent of Code 2022 - Day 10: Cathode-Ray Tube

## 📜 Puzzle Overview

This puzzle simulates a simple CPU driving a cathode-ray tube (CRT) display.

The input consists of instructions that affect a register over time. Each instruction takes a specific number of cycles to complete, and the system must be evaluated cycle-by-cycle.

There are two instruction types:

- `noop` (takes 1 cycle, does nothing)
- `addx V` (takes 2 cycles, then adds `V` to the register)

The register starts at:

    X = 1

Part 1 samples signal strengths at specific cycles. Part 2 renders a visual output based on the register's position over time.

---

## 🧩 Part 1

Calculate the sum of signal strengths at specific cycle checkpoints.

### 💡 Approach

- Parse each instruction line
- Simulate execution cycle-by-cycle
- Maintain:
  - current cycle number
  - register value (`X`)
- At cycles:
  
      20, 60, 100, 140, 180, 220

  compute:

      signal strength = cycle * X

- Accumulate the total signal strength

---

## 🧩 Part 2

Render the CRT display output based on sprite movement.

### 💡 Approach

- Treat the CRT as a grid:
  - 40 columns wide
  - 6 rows high
- Each cycle draws one pixel
- The horizontal position is:

      column = cycle % 40

- A sprite spans 3 pixels wide and is centered on `X`:

      [X - 1, X, X + 1]

- If the current column falls within the sprite range:

      draw '#'

  otherwise:

      draw '.'

- Continue for all cycles to build the full display

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Cathode-Ray Tube`
- Loads the input instructions
- Calls both parts

For Part 1:

- Executes the CPU simulation
- Samples signal strengths at required cycles
- Returns the accumulated total

For Part 2:

- Executes the same simulation
- Builds a visual string output representing the CRT

---

### Instruction Parsing

Each input line is parsed into an instruction:

- `noop`
- `addx V`

At a high level:

- split the input line
- determine instruction type
- extract value if needed

---

### CPU Simulation

The simulation runs cycle-by-cycle rather than instruction-by-instruction.

Key state:

- `cycle` counter
- register `X`

Execution rules:

- `noop`:
  - advance 1 cycle
- `addx V`:
  - advance 2 cycles
  - after the second cycle, update:

        X += V

This delayed update is critical for correct timing.

---

### Cycle Processing

Each cycle performs:

- increment cycle count
- optionally sample signal strength (Part 1)
- draw a pixel (Part 2)

This ensures both parts share the same underlying timing logic.

---

### Signal Strength Calculation

At specific cycles:

    signal = cycle * X

These values are summed for the final Part 1 answer.

---

### CRT Rendering Logic

The display is built line-by-line.

For each cycle:

- determine horizontal position:

      column = (cycle - 1) % 40

- compare with sprite position:

      if column in [X - 1, X, X + 1]

- append:

      '#'
      or
      '.'

After every 40 pixels:

- move to the next line

---

### Output Formatting

The final CRT output is:

- 6 rows of 40 characters
- typically returned as a multi-line string

Example structure:

    ####..##..#...
    #..#.#..#....
    ...

---

## 🛠 Implementation Notes

- Register updates happen after instruction completion
- Simulation is cycle-accurate, not instruction-based
- Part 1 and Part 2 share the same execution loop
- Rendering depends on precise cycle timing
- Sprite position is always centered on `X`

---

## 🧪 Behaviour Summary

Given a sequence of CPU instructions:

- the system simulates execution cycle-by-cycle
- register values change only after instruction completion
- Part 1 samples signal strength at fixed cycles
- Part 2 renders a CRT display based on sprite alignment
- both parts rely on the same timing model

---

## 🚀 Key Takeaways

- Demonstrates cycle-based simulation rather than step-based execution
- Highlights importance of delayed state updates
- Efficient reuse of logic for multiple outputs
- Combines numerical computation (Part 1) with visual rendering (Part 2)
- A great example of time-dependent state modelling

---

## 🔗 References

- https://adventofcode.com/2022/day/10