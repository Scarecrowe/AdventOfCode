# 🎄 Advent of Code 2016 - Day 25: Clock Signal

## 📜 Puzzle Overview

This puzzle returns to the assembunny interpreter and introduces output behaviour.

You are given a program that:

- operates on registers `a`, `b`, `c`, `d`
- includes instructions from previous days
- adds a new instruction that produces output

The goal is to find the smallest initial value for register `a` that causes the program to output a repeating clock signal:

```
0, 1, 0, 1, 0, 1, ...
```

The signal must alternate indefinitely.

---

## 🧩 Part 1

Determine the lowest positive integer for register `a` that produces a valid clock signal.

### 💡 Approach

- Reuse the assembunny interpreter
- Add support for the new instruction:

```
out x
```

- Execute the program for increasing values of `a`
- For each run:
  - capture output values
  - verify the pattern alternates between `0` and `1`
- Stop when a valid repeating pattern is detected
- Return the corresponding value of `a`

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Clock Signal`
- Loads the puzzle input
- Calls the solution

---

### Instruction Set

Includes previous instructions:

```
cpy x y
inc x
dec x
jnz x y
tgl x
```

Plus the new instruction:

```
out x
```

---

### Output Instruction

```
out x
```

- Outputs the value of `x`
- `x` can be:
  - a register
  - a literal value

---

### Signal Validation

To validate a clock signal:

- Track output values
- Ensure they follow the pattern:

```
0 → 1 → 0 → 1 → ...
```

- If any value breaks the pattern:
  - terminate the current attempt

---

### Execution Strategy

- Try increasing values of `a` starting from `0`
- For each value:
  - run the program
  - monitor output
- Stop early if:
  - pattern fails
- Accept if:
  - pattern holds for a sufficiently long sequence

---

### Detecting Infinite Behaviour

Since true infinite execution is not feasible:

- run until a threshold is reached
- if pattern remains valid:
  - assume it continues indefinitely

---

### Part 1 Logic

- Loop over candidate values of `a`
- Execute program with current value
- Validate output pattern
- Return first valid value

---

### Optimisation Insight

- The program often encodes a pattern:
  - repeated division or bit shifting
- Output corresponds to binary representation of a number
- Instead of full simulation:
  - analyse instruction behaviour
  - derive required input directly

---

## 🛠 Implementation Notes

- Interpreter must support dynamic instructions
- Output checking must be efficient
- Early termination improves performance
- Pattern detection avoids infinite loops
- Analytical shortcut can replace brute-force

---

## 🧪 Behaviour Summary

Given an assembunny program:

- It produces a stream of output values
- Valid output must alternate between `0` and `1`
- Different initial values produce different outputs
- Goal is to find the smallest valid starting value

---

## 🚀 Key Takeaways

- Interpreter extended with output instruction
- Pattern detection replaces full execution
- Brute-force with early exit is effective
- Underlying behaviour often reducible to bit patterns

---

## 🔗 References

- https://adventofcode.com/2016/day/25