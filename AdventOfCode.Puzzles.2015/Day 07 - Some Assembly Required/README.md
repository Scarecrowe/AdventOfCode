# 🎄 Advent of Code 2015 - Day 07: Some Assembly Required

## 📜 Puzzle Overview

Santa has a set of wires and bitwise logic gates that form a circuit.

Each instruction describes how a signal is assigned to a wire. Wires can receive values from:

- A direct numeric signal
- Another wire
- A bitwise operation involving one or two inputs

Supported operations include:

- `AND`
- `OR`
- `NOT`
- `LSHIFT`
- `RSHIFT`
- Direct assignment using `->`

Each wire carries a 16-bit signal, and the goal is to determine the final value on wire `a`.

---

## 🧩 Part 1

Determine the signal ultimately provided to wire `a`.

### 💡 Approach

- Parse each instruction into a structured representation
- Resolve instructions only when their required input values are available
- Store resolved wire signals as they are calculated
- Continue until the circuit has been fully assembled
- Return the final signal on wire `a`

---

## 🧩 Part 2

Take the signal from wire `a` found in Part 1 and override wire `b` with that value.

Then run the circuit again and determine the new value on wire `a`.

### 💡 Approach

- Fully assemble the circuit once
- Store the signal from wire `a`
- Reset the circuit state
- Replace the instruction for wire `b` with the saved signal
- Re-run the assembly process
- Return the new value on wire `a`

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SomeAssemblyRequired(this.Input)`
- Calls `Assemble()`
- Returns `WireA()`

For Part 2:

- Creates `new SomeAssemblyRequired(this.Input)`
- Calls `Assemble().Reassemble()`
- Returns `WireA()`

---

### `Instruction.cs`

This class parses a single instruction line into a structured object.

It identifies the operation type and stores:

- `LogicGate`
- `InputA`
- `InputB`
- `Output`

The parser supports:

- Direct assignment, such as `123 -> x`
- Unary operations, such as `NOT x -> h`
- Binary operations, such as `x AND y -> z`

It also provides:

- `IsSetter()` to identify direct assignments from constant values
- `Print()` for debugging output

---

### `Wire.cs`

This class represents either:

- A wire address, such as `x`
- A constant numeric value, such as `123`

When a `Wire` is created:

- If the value parses as a number, it is stored as a constant
- Otherwise it is treated as a wire address

This allows instruction inputs to be handled consistently regardless of whether they reference a wire or a literal value.

---

### `SomeAssemblyRequired.cs`

This class performs the circuit assembly.

It stores:

- `Wires` - resolved wire signals
- `Instructions` - remaining instructions to process
- `OriginalInstructions` - a copy used for rebuilding the circuit in Part 2

The constructor parses every input line into an `Instruction` and stores them by index.

---

### Initial Setters

Before processing the full instruction list, direct constant assignments are handled first.

Examples include:

- `123 -> x`
- `456 -> y`

These values are added straight into the wire map and removed from the remaining instruction list.

This gives the rest of the circuit a starting set of known signals.

---

### Assembly Process

The `Assemble()` method repeatedly searches for instructions whose inputs are ready.

An instruction is ready when:

- `InputA` is a constant, or its source wire already has a value
- `InputB` is either not required, a constant, or its source wire already has a value

Once an instruction is ready:

- Its output is calculated using `Process()`
- The result is stored in `Wires`
- The instruction is removed from the remaining list

This repeats until all instructions have been resolved.

---

### Signal Processing

The `Process()` method handles the actual bitwise operations.

Supported operations include:

- `Set`
- `Not`
- `And`
- `Or`
- `LShift`
- `RShift`

The result of each operation is cast to `ushort` so the signal stays within the 16-bit range used by the puzzle.

---

### Reassembly for Part 2

The `Reassemble()` method handles the second part of the puzzle.

It:

1. Reads the signal already calculated for wire `a`
2. Resets `Wires`
3. Restores the original instruction set
4. Finds the instruction that outputs to wire `b`
5. Replaces that instruction input with the saved signal
6. Runs `Assemble()` again

This makes it possible to rerun the same circuit with the required override in place.

---

## 🛠 Implementation Notes

- Instructions are parsed once into strongly structured objects
- Constants and wire references are handled through the same `Wire` model
- Direct constant assignments are resolved before the main processing loop
- The circuit is assembled incrementally based on dependency readiness
- Part 2 reuses the same solver by restoring the original instruction set and overriding wire `b`

---

## 🧪 Examples

Given the example circuit:

| Instruction |
|------------|
| `123 -> x` |
| `456 -> y` |
| `x AND y -> d` |
| `x OR y -> e` |
| `x LSHIFT 2 -> f` |
| `y RSHIFT 2 -> g` |
| `NOT x -> h` |
| `NOT y -> i` |

The resulting signals are:

| Wire | Signal |
|------|--------|
| `d`  | `72` |
| `e`  | `507` |
| `f`  | `492` |
| `g`  | `114` |
| `h`  | `65412` |
| `i`  | `65079` |
| `x`  | `123` |
| `y`  | `456` |

---

## 🚀 Key Takeaways

- Good example of parsing text instructions into structured logic objects
- Dependency-based processing keeps the circuit evaluation simple
- The `Wire` model cleanly handles both literals and addresses
- Part 2 is solved by restoring the original instruction set and overriding a single wire

---

## 🔗 References

- https://adventofcode.com/2015/day/7