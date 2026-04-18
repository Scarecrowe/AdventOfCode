# 🎄 Advent of Code 2024 - Day 24: Crossed Wires

## 📜 Puzzle Overview

This puzzle models a network of wires and logic gates.

The input is split into two sections:

- initial wire values
- gate expressions that define how other wires are computed

Wires such as `x00`, `y00`, and `z00` form part of a binary circuit. Each derived wire is defined by a bitwise expression using:

- `AND`
- `OR`
- `XOR`

Part 1 evaluates all `z` output wires and combines them into a final binary value.

Part 2 analyses the wiring and identifies which wire outputs must be swapped to make the circuit behave like a proper binary adder.

---

## 🧩 Part 1

Evaluate the circuit and return the decimal value represented by the `z` wires.

### 💡 Approach

- Parse the initial wire assignments
- Parse the gate expressions and store them by output wire name
- Recursively evaluate each `z` wire
- Process the `z` wires in descending order
- Build the final number as a binary value

---

## 🧩 Part 2

Identify the incorrectly wired outputs and return their names.

### 💡 Approach

- Treat the circuit like a ripple-carry adder
- Walk through matching `xNN`, `yNN`, and `zNN` bit positions
- At each step:
  - locate the expected XOR and AND expressions
  - verify that the expected output wire is connected correctly
- If a required expression is missing or the wrong output wire is used:
  - swap the stored expressions for the two affected wires
  - restart validation from the beginning
- Continue until all required swaps are found
- Return the swapped wire names sorted alphabetically and joined with commas

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Crossed Wires`
- Loads the puzzle input from file
- Calls the silver and gold solutions

For Part 1:

- creates `new CrossedWires(this.FilePath)`
- calls `ZOutput()`

For Part 2:

- creates `new CrossedWires(this.FilePath)`
- calls `WireNames()`

---

### `CrossedWires.cs`

This class contains the full circuit parsing and evaluation logic.

It stores:

- `registers`
- `cache`

`registers` holds wire definitions.

That means a wire can store either:

- a literal value such as `0` or `1`
- an expression such as:

      x03 XOR y03

`cache` stores previously evaluated results so recursive lookups do not recompute the same wires repeatedly.

---

### Parsing the Input

The constructor reads the full file and splits it into two sections.

The first section contains direct wire assignments such as:

    x00: 1

These are stored directly in `registers`.

The second section contains gate definitions such as:

    x00 AND y00 -> z00

These are also stored in `registers`, but keyed by the destination wire name.

So the parser effectively creates a lookup of:

- wire name -> literal value or expression string

---

### Recursive Evaluation

`Evaluate(string name)` resolves the value of any wire.

It works like this:

- if the name is already a number, return it directly
- if the value is cached, return the cached result
- otherwise:
  - fetch the wire definition from `registers`
  - split the expression
  - recursively evaluate its operands
  - apply the required bitwise operation

Supported operations are:

- `AND`
- `OR`
- `XOR`

The computed result is then stored in `cache`.

This allows the circuit to be evaluated lazily, only when a wire is actually needed.

---

### `ZOutput()`

This method solves Part 1.

It:

- finds every register whose name starts with `z`
- sorts them in descending order
- evaluates each one
- builds the result as a binary number

The logic is effectively:

    result = result * 2 + bit

for each `z` register in order.

So the final silver answer is the decimal interpretation of the `z` output bits.

---

### `WireNames()`

This method solves Part 2.

It tries to verify that the circuit behaves like a binary adder.

It keeps track of:

- `index`
- `carryReg`
- `swaps`

The method begins at bit position `0` and checks whether the expected adder structure exists.

For bit `0`:

- the carry register is expected to come from:

      x00 AND y00

For later bits, it expects logic equivalent to:

- XOR between `xNN` and `yNN`
- AND between `xNN` and `yNN`
- XOR between the previous carry and the XOR result to produce `zNN`
- AND between the previous carry and the XOR result
- OR between the two AND-related wires to produce the next carry

---

### Finding Expressions

`FindExpression(string op1, string op, string op2)` searches `registers` for a wire whose stored expression matches:

    op1 op op2

or the reversed equivalent:

    op2 op op1

This matters because expressions such as:

    a XOR b

and

    b XOR a

are logically identical.

The method returns the output wire name for the matching expression.

---

### Swap Detection

When the expected circuit shape is not found, the solver assumes two outputs are crossed.

There are two main correction cases:

1. the expected XOR/carry structure cannot be found
2. the expression exists, but it is attached to the wrong `zNN` output wire

In either case:

- the two wire names are added to `swaps`
- their expression values in `registers` are exchanged
- validation restarts from index `0`

This continues until eight swapped wire names have been collected or all positions validate successfully.

---

### `Bitwise.cs`

This file defines:

- `Bitwise`

with values:

- `And`
- `Or`
- `Xor`

It exists as a simple enum for the puzzle folder, although the core evaluation logic in `CrossedWires.cs` uses string-based operator matching directly.

---

## 🛠 Implementation Notes

- Input is read from `filePath`, not directly from a string array
- Wire definitions are stored as strings in a dictionary
- Recursive evaluation uses memoisation through `cache`
- Part 1 reads all `z` wires in descending order
- The binary result is built left to right with multiply-by-2 accumulation
- Part 2 assumes the circuit should behave like a ripple-carry adder
- Expression matching treats operand order as interchangeable
- Incorrect outputs are corrected by swapping stored register expressions
- The final gold answer is a comma-separated sorted list of swapped wire names

---

## 🧪 Behaviour Summary

Given a logic circuit description:

- the solver parses literal inputs and gate outputs into a register map
- Part 1 recursively evaluates every `z` output wire
- those output bits are combined into one decimal number
- Part 2 walks the circuit as though it were a binary adder
- when wires appear to be crossed, their stored expressions are swapped
- once the bad outputs are identified, their names are returned

So the final result is either:

- the evaluated decimal output of the `z` wires
- or the sorted list of swapped wire names needed to correct the circuit

---

## 🚀 Key Takeaways

- The circuit is represented as a dictionary of wire names to expression strings
- Recursive evaluation with caching keeps Part 1 efficient
- Part 1 treats the `z` wires as bits of a binary number
- Part 2 reverse-engineers the intended adder structure
- Incorrect wiring is fixed by swapping output definitions and restarting validation
- Commutative gate matching is important when searching for equivalent expressions

---

## 🔗 References

- https://adventofcode.com/2024/day/24