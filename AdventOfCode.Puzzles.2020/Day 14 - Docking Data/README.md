# 🎄 Advent of Code 2020 - Day 14: Docking Data

## 📜 Puzzle Overview

This puzzle simulates a memory system that applies bitmasks before writing values.

The input contains two kinds of instructions:

- mask updates
- memory writes

Example instructions:

    mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
    mem[8] = 11

The solver has two separate implementations:

- `Version1(...)` for value masking
- `Version2(...)` for address masking with floating bits

Part 1 applies the mask to the value being written. Part 2 applies the mask to the memory address and writes the value to every generated address.

---

## 🧩 Part 1

Apply the mask to each value before writing it to memory, then return the sum of all stored values.

### 💡 Approach

- Track the current mask
- Convert the mask into:
  - an AND mask
  - an OR mask
- For each memory write:
  - parse the address
  - parse the value
  - apply the AND mask
  - apply the OR mask
  - store the result in memory
- Sum all non-zero memory values at the end

---

## 🧩 Part 2

Apply the mask to memory addresses, expanding floating bits into every possible address, then return the sum of all stored values.

### 💡 Approach

- Track the current mask
- Count how many floating bits (`X`) are present
- For each memory write:
  - parse the base address
  - parse the value
  - generate every possible floating-bit combination
  - apply each combination to the address
  - write the value to every generated address
- Sum all stored memory values at the end

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Docking Data`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `DockingData.Version1(this.Input)`

For Part 2:

- Calls `DockingData.Version2(this.Input)`

---

### `DockingData.cs`

This class contains both puzzle solutions as static methods:

- `Version1(string[] input)`
- `Version2(string[] input)`

Each method parses the instruction list and returns the final memory-value sum.

---

### Part 1 Memory Model

`Version1(...)` creates:

- `long[] memory = new long[100000];`

It also tracks:

- `mask`
- `maskAnd`
- `maskOr`

So the current mask is preprocessed into two numeric masks that can be applied efficiently to every written value.

---

### Parsing Instructions in Part 1

Each instruction is split on:

    =

If the left side starts with `mask`, the solver updates the current bitmask.

Otherwise it treats the line as a memory write and parses:

- the memory address from `mem[...]`
- the value from the right-hand side

At a high level it does:

- `tokens[0].Replace("mem[").Replace("]")`
- convert the address to an integer
- trim and parse the value

---

### Building the Masks

When a new mask is read, Part 1 creates:

- `maskAnd`
- `maskOr`

using string replacement:

- replace `X` with `1` for the AND mask
- replace `X` with `0` for the OR mask

So:

- `maskAnd` preserves bits unless the mask forces a `0`
- `maskOr` forces bits to `1` where required

---

### Applying the Mask to Values

For each memory write in Part 1:

- apply the AND mask first
- then apply the OR mask

Logically this is:

    result = value & maskAnd
    result |= maskOr

The final masked value is then stored in the target memory slot.

---

### Part 1 Return Value

After all instructions are processed, the method returns:

- the sum of all positive values in memory

So the silver answer is the total of the masked values left in memory after the full instruction list completes.

---

### Part 2 Memory Model

`Version2(...)` uses:

- `Dictionary<long, long> memory = new();`

instead of a fixed array.

This is necessary because the address expansion can generate many sparse memory locations.

It also tracks:

- `mask`
- `numberOfCombinations`

---

### Counting Floating Combinations

Whenever a new mask is read, Part 2 counts how many `X` characters it contains.

It then computes:

    2 ^ number_of_X

This gives the total number of floating-bit combinations that must be generated for each subsequent memory write.

---

### Parsing Instructions in Part 2

Part 2 uses regular expressions to parse the instructions.

For mask lines it extracts the mask text.

For memory writes it extracts:

- the memory address
- the value

So a line such as:

    mem[42] = 100

becomes:

- `memoryAddress = 42`
- `value = 100`

---

### Generating Floating Addresses

For each memory write, Part 2 loops through every possible floating-bit combination.

For each combination it:

- copies the original memory address
- walks through the mask from right to left
- applies bit logic per character

The rules are:

- `0` → leave the address bit unchanged
- `1` → force that bit to `1`
- `X` → set the bit according to the current combination

---

### Handling Floating Bits

For each `X` in the mask, the solver pulls one bit from the current combination counter.

It tracks this using:

- `offset`

Then for each floating position:

- if the current combination bit is `0`, clear that address bit
- if the current combination bit is `1`, set that address bit

This produces one fully expanded memory address per combination.

---

### Writing Expanded Addresses

Once a generated address is ready:

- add it to the dictionary if it does not exist
- write the value to that address

That means later writes can overwrite earlier ones at the same generated address, matching the puzzle rules.

---

### Part 2 Return Value

After all instructions are processed, the method returns:

- `memory.Values.Sum()`

So the gold answer is the sum of all values remaining in expanded-address memory after every write completes.

---

## 🛠 Implementation Notes

- Part 1 uses a fixed `long[]` memory array
- Part 2 uses a `Dictionary<long, long>` for sparse expanded addresses
- Part 1 masks written values
- Part 2 masks memory addresses
- Part 1 builds numeric AND/OR masks from the current mask string
- Part 2 enumerates every floating-bit permutation
- Bit operations are performed from the least significant side of the mask

---

## 🧪 Behaviour Summary

Given a list of mask and memory instructions:

- the solver reads the current mask
- Part 1 applies that mask to values before storing them
- Part 2 applies that mask to addresses and expands floating bits
- memory is updated as instructions are processed
- each part returns the final sum of values left in memory

---

## 🚀 Key Takeaways

- Good example of practical bitmask manipulation
- Part 1 uses efficient AND/OR mask composition
- Part 2 uses combinational expansion for floating address bits
- The two puzzle parts reuse the same input style with very different masking logic
- Clean separation between value-masking and address-masking implementations

---

## 🔗 References

- https://adventofcode.com/2020/day/14