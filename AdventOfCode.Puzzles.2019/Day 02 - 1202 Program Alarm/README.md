# 🎄 Advent of Code 2019 - Day 2: 1202 Program Alarm

## 📜 Puzzle Overview

This puzzle introduces a simple virtual machine known as the **Intcode computer**.

You are given a list of integers representing memory. The program executes instructions in chunks of 4 values:

- opcode
- parameter 1 position
- parameter 2 position
- output position

The key opcodes are:

- `1` → addition
- `2` → multiplication
- `99` → halt

The program processes instructions sequentially until it encounters opcode `99`.

---

## 🧩 Part 1

Run the Intcode program after applying a specific initial state.

### 💡 Approach

- Parse the input into an integer array
- Before execution:
  - Set position `1` to `12`
  - Set position `2` to `2`
- Process the program in steps of 4:
  - Read opcode
  - Perform operation using referenced positions
  - Store result in the output position
- Stop when opcode `99` is encountered
- Return the value at position `0`

---

## 🧩 Part 2

Find the pair of inputs (noun and verb) that produce a target output.

### 💡 Approach

- Iterate over all possible values for:
  - noun (position `1`)
  - verb (position `2`)
- For each combination:
  - Clone the original program
  - Set noun and verb
  - Run the Intcode program
- Check if position `0` matches the target value (`19690720`)
- Return:

    100 * noun + verb

---

## 🧠 Code Breakdown

### `Day02.cs`

This is the puzzle entry point.

- Sets the puzzle title to `1202 Program Alarm`
- Loads the input
- Executes both parts

For Part 1:

- Parses input into memory
- Applies fixed noun/verb (12, 2)
- Runs the Intcode program
- Returns memory position `0`

For Part 2:

- Brute-force searches noun and verb values
- Runs the program repeatedly
- Returns the computed result when the target output is found

---

### Intcode Execution

The core execution logic processes memory like this:

    while (true)
    {
        opcode = memory[index]

        if opcode == 99 → halt

        read parameters
        perform operation
        store result

        index += 4
    }

---

### Opcode Handling

#### Addition (Opcode 1)

    memory[output] = memory[param1] + memory[param2]

#### Multiplication (Opcode 2)

    memory[output] = memory[param1] * memory[param2]

#### Halt (Opcode 99)

- Stops execution immediately

---

### Memory Access Pattern

Each instruction uses **position-based addressing**:

- Parameters are not values
- They are **indexes into memory**

So:

    memory[memory[index + 1]]

means:

- read the value at position `index + 1`
- use that as an address
- fetch the actual value from that address

---

### Program Mutation

The Intcode program modifies its own memory during execution.

This means:

- operations overwrite existing values
- later instructions may depend on updated data

---

### Brute Force Search (Part 2)

The solver tries all combinations:

    for noun in 0..99
        for verb in 0..99

For each:

- reset memory to original state
- apply noun/verb
- execute program
- check result

Once found:

    return (100 * noun) + verb

---

## 🛠 Implementation Notes

- Memory is represented as an integer array
- Instructions are processed in fixed steps of 4
- Program halts only when opcode `99` is encountered
- Input must be cloned for Part 2 to avoid mutation issues
- Position-based addressing is key to correct execution

---

## 🧪 Behaviour Summary

Given a list of integers:

- the program interprets them as instructions
- processes operations sequentially
- modifies memory in-place
- halts when instructed

Part 1:

- runs with fixed inputs
- returns final memory state at position `0`

Part 2:

- searches for inputs that produce a target output
- uses brute force over a small input range

---

## 🚀 Key Takeaways

- Introduces a simple virtual machine model
- Demonstrates position-based memory addressing
- Highlights importance of state mutation during execution
- Shows brute-force search as a valid approach for small input spaces
- Forms the foundation for more advanced Intcode problems later in Advent of Code 2019

---

## 🔗 References

- https://adventofcode.com/2019/day/2