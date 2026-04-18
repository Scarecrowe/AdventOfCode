# 🎄 Advent of Code 2021 - Day 24: Arithmetic Logic Unit

## 📜 Puzzle Overview

This puzzle works with a custom ALU program that validates a 14-digit model number.

Rather than simulating every possible input, this solver analyses the repeating structure of the instruction blocks and derives direct relationships between digit positions.

The input program is made of 14 repeated sections, each 18 instructions long. From each section, the solver extracts two important numeric values and stores them as pairs.

These pairs are then used to determine which digit positions are linked together, allowing the largest and smallest valid model numbers to be constructed directly.

The puzzle entry returns:

    new ArithmeticLogicUnit(this.Input).Largest()
    new ArithmeticLogicUnit(this.Input).Smallest()

---

## 🧩 Part 1

Determine the largest valid 14-digit model number.

### 💡 Approach

- Split the ALU program into 14 instruction blocks
- Extract two key values from each block
- Use a stack to match related digit positions
- Derive the offset that must exist between each linked pair of digits
- Choose the highest valid digits that satisfy all constraints
- Assemble the final 14-digit number

---

## 🧩 Part 2

Determine the smallest valid 14-digit model number.

### 💡 Approach

- Reuse the same extracted instruction pairs
- Reuse the same stack-based pairing logic
- Apply the same digit constraints
- This time choose the lowest valid digits that still satisfy every relationship
- Assemble the final 14-digit number

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Arithmetic Logic Unit`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new ArithmeticLogicUnit(this.Input)`
- calls `Largest()`

For Part 2:

- creates `new ArithmeticLogicUnit(this.Input)`
- calls `Smallest()`

---

### `ArithmeticLogicUnit.cs`

This class contains the full parsing and model-number solving logic.

It stores:

- `Pairs`

The constructor immediately parses the input:

    this.Pairs = Parse(input);

So the raw ALU program is reduced into a compact list of values that drive the real solution.

---

### Parsing the ALU Program

`Parse(string[] input)` reads the instruction stream in 14 fixed-size chunks.

Each chunk is 18 lines long.

For each block `i`, it extracts:

- the value at line offset `5`
- the value at line offset `15`

At a high level it does:

- loop from `0` to `13`
- read:
  
      input[(i * 18) + 5]
      input[(i * 18) + 15]

- slice off the instruction prefix
- convert the remaining text into integers
- store the result as:

      (A, B)

So the full ALU program becomes:

- `List<(int A, int B)>`

This is the key simplification in the implementation.

---

### What the Parsed Pairs Represent

Each of the 14 input digits corresponds to one parsed pair:

- `A`
- `B`

The solver does not interpret every ALU instruction individually after that.

Instead, it uses the sign of `A` to determine whether the current digit position should:

- push onto a stack
- or pop and form a relationship with an earlier digit

This reveals which input positions depend on each other.

---

### Solving the Digit Relationships

`Run(...)` is the core method used by both `Largest()` and `Smallest()`.

It creates:

- `Stack<(int A, int B)> stack = new();`
- a dictionary of linked digit constraints

Then it walks through all parsed pairs with their index.

If:

    pair.A > 0

the solver pushes:

- the current digit index
- the current `B` value

onto the stack.

Otherwise:

- it pops the previous stacked entry
- combines that previous `B` value with the current `A`
- stores the relationship between the two digit positions

So each pop step creates a constraint of the form:

- current digit depends on an earlier digit
- with some numeric offset between them

---

### Building the Constraint Map

When a negative-`A` block is reached, the solver does:

- `var (j, addr) = stack.Pop();`
- `keys[i] = (j, addr + pair.A);`

This means digit position `i` is linked to earlier position `j`, with an offset of:

    addr + pair.A

That offset determines how the two digits must differ for the model number to be valid.

So the solver transforms the ALU program into a set of direct digit-to-digit equations.

---

### Choosing Digits for the Largest Model Number

`Largest()` calls:

    this.Run(new((y) => Math.Min(9, 9 + y)), new((y) => Math.Min(9, 9 - y)))

This means the solver tries to keep digits as high as possible while respecting each offset relationship.

For every linked pair:

- one function computes the digit for one side of the pair
- the other computes the matching digit for the other side

Both are clamped so they stay within the valid digit range:

- `1` to `9`

The result is the highest possible valid 14-digit number.

---

### Choosing Digits for the Smallest Model Number

`Smallest()` calls:

    this.Run(new((y) => Math.Max(1, 1 + y)), new((y) => Math.Max(1, 1 - y)))

This uses the same pairing logic, but now pushes digits as low as possible instead of as high as possible.

Again, both sides of each linked pair are kept within the valid model-number digit range.

The result is the lowest possible valid 14-digit number.

---

### Assembling the Final Number

After all linked pairs are resolved, `Run(...)` builds:

- `Dictionary<int, int> result`

This maps each digit position to its final chosen value.

Then it does:

- order by digit index
- select the digits in sequence
- join them into a string
- convert that string to `long`

So the final output is the completed 14-digit model number.

---

## 🛠 Implementation Notes

- The input is treated as 14 repeated instruction blocks
- Only two values are extracted from each block
- Parsed pairs are stored as `(A, B)` tuples
- A stack is used to match related digit positions
- Positive `A` values push onto the stack
- Non-positive `A` values pop and create a digit constraint
- Part 1 and Part 2 share the same core solver
- The only difference is whether the digit-selection functions maximise or minimise the final number

---

## 🧪 Behaviour Summary

Given the full ALU instruction program:

- the solver breaks it into 14 fixed blocks
- extracts two important integers from each block
- uses stack pairing to detect relationships between input positions
- converts those relationships into digit offsets
- chooses valid digits that satisfy every constraint
- assembles the final model number

Part 1:

- picks the largest valid digits

Part 2:

- picks the smallest valid digits

So the solver avoids brute force entirely and derives both answers directly from the instruction structure.

---

## 🚀 Key Takeaways

- Good example of reducing a large instruction set into a compact mathematical model
- The real logic comes from analysing repeated block structure
- A stack is used to pair dependent digit positions
- The ALU program is transformed into direct digit constraints
- Part 1 and Part 2 differ only in how digits are chosen once the constraints are known
- The final model number is built without simulating every candidate input

---

## 🔗 References

- https://adventofcode.com/2021/day/24