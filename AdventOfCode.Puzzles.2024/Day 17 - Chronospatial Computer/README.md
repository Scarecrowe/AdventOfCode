# 🎄 Advent of Code 2024 - Day 17: Chronospatial Computer

## 📜 Puzzle Overview

This puzzle simulates a small custom computer with:

- three registers:
  - `A`
  - `B`
  - `C`
- an instruction pointer
- a program made of integer opcodes and operands

The input provides:

- the starting value of each register
- a comma-separated program

The solver parses those values into a `ChronospatialComputer` instance and then executes the program two different ways.

Part 1 runs the program normally and returns the output stream.

Part 2 searches for the lowest positive value of register `A` that causes the program output to match the program itself. This matches the structure of the repo implementation, where `FinalOutput()` calls `Execute()` and `LowestPostive()` delegates to `FindLowestPostive(...)`. 

---

## 🧩 Part 1

Run the chronospatial computer and return its output values as a comma-separated string.

### 💡 Approach

- Parse the initial register values
- Parse the program into a list of integers
- Execute instructions two integers at a time:
  - opcode
  - operand
- Update registers and instruction pointer according to each instruction
- Collect values emitted by the `Out` instruction
- Return the output list joined with commas

---

## 🧩 Part 2

Find the lowest positive value for register `A` that makes the output match the program.

### 💡 Approach

- Reuse the same execution engine as Part 1
- Build the answer recursively from the end of the program backwards
- At each step:
  - test eight possible values derived from the current partial answer
  - run the program with that test value loaded into register `A`
  - compare the first output digit with the expected program value at the current index
- Continue until every required output value has been matched
- Return the first successful lowest value

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Chronospatial Computer`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ChronospatialComputer(this.Input)`
- Calls `FinalOutput()`

For Part 2:

- Creates `new ChronospatialComputer(this.Input)`
- Calls `LowestPostive()`

---

### `ChronospatialComputer.cs`

This class contains the full parsing, execution, and search logic.

It stores:

- `A`
- `B`
- `C`
- `Pointer`
- `Program`

The constructor:

- initialises `Program`
- calls `Parse(input)`

So the full machine state is held directly on the class instance.

---

### Parsing the Input

`Parse(string[] input)` reads the register values and program from the puzzle input.

It does this by parsing:

- `input[0]` as `Register A`
- `input[1]` as `Register B`
- `input[2]` as `Register C`
- `input[4]` as the comma-separated program line

At a high level it does:

- strip the register labels
- parse the numeric values
- split the program on commas
- convert the program into `List<int>`

So the machine starts with a fully populated register state and instruction list.

---

### Combo Operands

`Combo(int operand)` resolves combo operands used by several instructions.

It returns:

- `A` when operand is `4`
- `B` when operand is `5`
- `C` when operand is `6`
- otherwise the operand value itself

So the combo system lets instructions refer either to:

- literal small values
- register contents

---

### Instruction Enum

The instruction set is defined by:

- `Adv = 0`
- `Bxl = 1`
- `Bst = 2`
- `Jnz = 3`
- `Bxc = 4`
- `Out = 5`
- `Bdv = 6`
- `Cdv = 7`

Each program opcode is cast into this enum during execution.

---

### Program Execution

`Execute(long a = -1)` runs the full program and returns the emitted output values.

It first:

- optionally overrides register `A` if an argument is provided
- resets `Pointer` to `0`
- creates a result list

Then it loops until the instruction pointer goes past the end of the program.

Each cycle reads:

- the current opcode at `Program[Pointer]`
- the operand at `Program[Pointer + 1]`

It also computes:

- `combo = Combo(operand)`

Then it advances the pointer by `2` before applying the instruction.

So the program is interpreted as opcode/operand pairs.

---

### Instruction Behaviour

The switch inside `Execute(...)` handles all instruction logic.

#### `Adv`

    this.A = (long)(this.A / Math.Pow(2, combo));

This divides register `A` by `2^combo`.

---

#### `Bxl`

    this.B ^= operand;

This XORs register `B` with the literal operand.

---

#### `Bst`

    this.B = combo & 0b111;

This stores the combo operand modulo `8` into register `B`.

---

#### `Jnz`

    if (this.A != 0)
    {
        this.Pointer = operand;
    }

This jumps to the operand position when `A` is not zero.

---

#### `Bxc`

    this.B ^= this.C;

This XORs register `B` with register `C`.

---

#### `Out`

    result.Add(combo % 8);

This emits the combo operand modulo `8` into the output list.

---

#### `Bdv`

    this.B = (long)(this.A / Math.Pow(2, combo));

This divides `A` by `2^combo` and stores the result in `B`.

---

#### `Cdv`

    this.C = (long)(this.A / Math.Pow(2, combo));

This divides `A` by `2^combo` and stores the result in `C`.

---

### Part 1 Output

`FinalOutput()` is simply:

    string.Join(",", this.Execute())

So the silver answer is the full output stream from the program, joined into a comma-separated string.

---

### Part 2 Search

`LowestPostive()` starts the gold search with:

    this.FindLowestPostive(0, this.Program.Count - 1)

So the solver works backwards from the final program index.

---

### Recursive Search Logic

`FindLowestPostive(long value, int index)` builds the register `A` value recursively.

It works like this:

1. If `index < 0`, return the accumulated value
2. Multiply the current partial value by `8`
3. Test all eight possibilities in that next base-8 range
4. Execute the program with each candidate as register `A`
5. Compare the first output value with `Program[index]`
6. Recurse into the next index when a match is found
7. Return the first positive successful result

The tested range is:

    value * 8
    through
    value * 8 + 7

So the search grows the answer one 3-bit chunk at a time.

---

### Why the Part 2 Search Works

The implementation only checks:

    this.Execute(testValue)[0] == this.Program[index]

This means the search is not comparing the full output at once.

Instead, it reconstructs the required `A` value backwards so that each recursive stage matches one expected output digit.

That keeps the search much smaller than brute-forcing all possible values directly.

---

### Part 1 Return Value

When called as:

    FinalOutput()

the method returns:

- the program's emitted output values as a comma-separated string

---

### Part 2 Return Value

When called as:

    LowestPostive()

the method returns:

- the lowest positive register `A` value found by the recursive search

Note that the method name in the implementation is spelled:

- `LowestPostive`

rather than `LowestPositive`

---

## 🛠 Implementation Notes

- The machine stores all state directly in the `ChronospatialComputer` instance
- Programs are processed as opcode/operand pairs
- Combo operands can resolve to literals or register values
- The instruction pointer normally advances by `2`
- `Jnz` can overwrite the pointer after that advance
- Output values are always reduced modulo `8`
- Part 2 uses recursive search rather than brute force
- The gold search tests candidate `A` values in groups of eight
- The implementation name uses the typo `LowestPostive`

---

## 🧪 Behaviour Summary

Given a register state and program:

- the solver parses the starting registers
- loads the program as integers
- executes the instructions pair by pair
- updates registers according to opcode rules
- collects values from `Out`
- Part 1 returns the final output stream
- Part 2 recursively searches for the smallest `A` that recreates the desired output pattern

---

## 🚀 Key Takeaways

- Good example of implementing a compact custom virtual machine
- The instruction set is small but expressive enough for looping and output generation
- Combo operands let instructions mix register references with literals
- Part 1 is straightforward program execution
- Part 2 is solved with a clever backwards recursive search instead of naive brute force
- Reusing the same `Execute(...)` method keeps both parts tied to one source of truth

---

## 🔗 References

- https://adventofcode.com/2024/day/17