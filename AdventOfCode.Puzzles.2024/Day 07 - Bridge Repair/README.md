# 🎄 Advent of Code 2024 - Day 7: Bridge Repair

## 📜 Puzzle Overview

This puzzle works with calibration equations.

Each input line contains:

- a target test value
- a sequence of numbers

An input line looks like this:

    190: 10 19

The solver parses each line into an `Equation` object containing:

- `TestValue`
- `Values`

The goal is to determine which equations can be made true by applying operators between the numbers from left to right.

Part 1 allows:

- addition
- multiplication

Part 2 also allows:

- concatenation

If an equation can produce its target value, that target is added to the final calibration total.

---

## 🧩 Part 1

Determine the total calibration result using only addition and multiplication.

### 💡 Approach

- Parse every input line into an equation
- Start from the first number in the equation
- Explore all possible operator choices between the remaining values
- Evaluate strictly left to right
- If any operator sequence reaches the test value, add that test value to the running total
- Continue until all equations have been checked

---

## 🧩 Part 2

Determine the total calibration result when concatenation is also allowed.

### 💡 Approach

- Reuse the same equation search logic as Part 1
- In addition to `+` and `*`, also allow concatenating the next value onto the current result
- For example, `12` concatenated with `34` becomes `1234`
- If any valid operator path reaches the target value, add that target to the total

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Bridge Repair`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new BridgeRepair(this.Input)`
- Calls `Calibrate()`

For Part 2:

- Creates `new BridgeRepair(this.Input)`
- Calls `Calibrate(true)`

---

### `Equation.cs`

This class models a single calibration equation.

It stores:

- `TestValue`
- `Values`

The constructor simply accepts:

- the target test value
- the list of numbers that must be combined

So a line such as:

    190: 10 19

becomes one `Equation` instance with:

- target `190`
- values `[10, 19]`

---

### `EquationState.cs`

This class represents one step in the operator search.

It stores:

- `Result`
- `Operator`
- `Index`

This lets the solver track:

- the current calculated value
- which operator should be applied next
- which input value should be processed next

---

### `OperatorEnum.cs`

This enum defines the available operations:

- `Add`
- `Multiply`
- `Concat`

Part 1 uses:

- `Add`
- `Multiply`

Part 2 uses all three.

---

### `BridgeRepair.cs`

This class contains the full parsing and calibration logic.

It stores:

- `Equations`

The constructor parses the input with:

- `Parse(input)`

which builds:

- `List<Equation>`

---

### Parsing the Input

`Parse(string[] input)` processes each equation line.

For every input line it:

- splits on `": "`
- parses the left side as the target test value
- splits the right side on spaces
- parses the remaining numbers into a list
- creates a new `Equation`

At a high level it does:

- read each line
- extract target and operands
- create an equation object
- collect them into a list

---

### Queueing Operator States

The helper method `Enqueue(...)` adds possible next operations into a queue.

It always enqueues:

- `Add`
- `Multiply`

And when concatenation is enabled, it also enqueues:

- `Concat`

This is how the solver branches into all valid operator choices for the current point in the equation.

---

### Main Calibration Loop

`Calibrate(bool concat = false)` computes the final answer.

It creates:

- `long result = 0`
- `Queue<EquationState> queue = new()`

Then for each equation it:

- starts from the first value
- enqueues possible operator choices beginning at index `1`
- repeatedly dequeues a state
- applies that state's operator to the next value
- either accepts the equation or branches to the next step

So the solver explores possible operator combinations using a queue-based search.

---

### How a State Is Evaluated

When a state is dequeued, it applies one operation using the next value in the equation.

The switch logic is effectively:

For addition:

    result += nextValue

For multiplication:

    result *= nextValue

For concatenation:

    result = long.Parse($"{result}{nextValue}")

This means the expression is evaluated left to right, with no operator precedence rules.

---

### Matching the Target Value

After applying the operator, the solver checks:

    if (state.Result == equation.TestValue)

If that happens:

- the equation is considered solved
- the matched value is added to the overall calibration total
- the queue is cleared
- processing moves to the next equation

This means only one successful path is needed for an equation to contribute to the final answer.

---

### Continuing the Search

If the current result does not match the target, the solver checks whether more values remain:

    else if (state.Index < equation.Values.Count - 1)

If so, it enqueues more states using:

- the updated result
- the next index
- the allowed operator set

This continues until either:

- a valid path reaches the target
- all possible paths are exhausted

---

### Part 1 Return Value

When called as:

    Calibrate()

the method uses only:

- addition
- multiplication

It returns:

- the sum of all test values from equations that can be satisfied using those two operators

---

### Part 2 Return Value

When called as:

    Calibrate(true)

the method also enables:

- concatenation

It returns:

- the sum of all test values from equations that can be satisfied using addition, multiplication, and concatenation

---

## 🛠 Implementation Notes

- Equations are evaluated left to right
- The solver uses a queue to explore operator combinations
- Each queued state stores the running result, operator, and current value index
- Concatenation is implemented by converting the combined numbers into a string and parsing back to `long`
- Once an equation finds a valid solution path, the remaining queued branches for that equation are discarded
- Part 2 is the same core algorithm as Part 1, with `Concat` added as an extra operator option

---

## 🧪 Behaviour Summary

Given a list of equations:

- the solver parses each equation into a target and number list
- it begins from the first number in each equation
- it explores all valid operator choices across the remaining numbers
- Part 1 allows addition and multiplication
- Part 2 also allows concatenation
- if any path reaches the target value, that target is added to the calibration sum
- the final result is the sum of all solvable equation targets

---

## 🚀 Key Takeaways

- Good example of exploring expression possibilities without building full expression trees
- The queue-based search cleanly models branching operator choices
- `EquationState` keeps the search state compact and easy to process
- Part 2 extends the exact same search by adding one more operator
- The implementation stops searching an equation as soon as one valid path is found

---

## 🔗 References

- https://adventofcode.com/2024/day/7