# 🎄 Advent of Code 2022 - Day 21: Monkey Math

## 📜 Puzzle Overview

This puzzle is built around a network of named monkeys.

Each monkey either:

- holds a fixed number
- or performs an operation using the results of two other monkeys

So the input defines an expression tree using monkey names as references.

A line can look like this:

    dbpl: 5

or:

    root: pppw + sjmn

The solver parses all monkeys into linked objects, then evaluates the tree rooted at `root`.

Part 1 evaluates the final result normally. Part 2 changes the problem into finding the correct value for `humn` so that the two sides of `root` become equal.

---

## 🧩 Part 1

Determine what the `root` monkey yells.

### 💡 Approach

- Parse each monkey definition
- Build links between monkeys by name
- Treat number monkeys as leaf values
- Treat operation monkeys as expression nodes
- Build the full equation rooted at `root`
- Evaluate the expression and return the result

---

## 🧩 Part 2

Determine what value `humn` must yell so that both sides of `root` are equal.

### 💡 Approach

- Reuse the same parsed monkey graph
- Treat `humn` as the changing input value
- Repeatedly try candidate values for `humn`
- Evaluate the left and right branches of `root`
- Use binary search to move toward equality
- Return the value when both sides match

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monkey Math`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MonkeyMath(this.Input)`
- Calls `Root()`

For Part 2:

- Creates `new MonkeyMath(this.Input)`
- Calls `RootEqual()`

---

### `MonkeyOperator.cs`

This enum defines the supported monkey operations.

It contains:

- `NONE`
- `ADD`
- `SUBTRACT`
- `MULTIPLY`
- `DIVIDE`
- `EQUALS`

The arithmetic operators are stored using their character values, such as:

- `'+'`
- `'-'`
- `'*'`
- `'/'`

This makes it easy to rebuild expression strings later.

---

### `Monkey.cs`

This class models a single monkey node in the expression graph.

It stores:

- `Value`
- `Left`
- `Right`
- `Operation`

A monkey can therefore represent either:

- a plain number
- or a calculation involving two child monkeys

---

### Monkey Evaluation

`Eval()` evaluates a monkey recursively.

It handles:

- addition
- subtraction
- multiplication
- division

It also includes support for:

- `EQUALS`

For `EQUALS`, the method returns:

    left - right

If the monkey has no operation, it simply returns:

    Value

So `Eval()` treats the monkey structure like a recursive expression tree.

---

### `MonkeyMath.cs`

This class contains the main parsing and solving logic.

It stores:

- `Monkeys`

The constructor:

- parses the input
- builds a dictionary of named monkeys

That dictionary is keyed by monkey name, such as:

- `root`
- `humn`
- other referenced monkey identifiers

---

### Parsing the Monkeys

`ParseMonkeys(string[] input)` reads every input line and splits it using:

- spaces
- colons

If the line contains more than two tokens, it is treated as an operation monkey.

For those lines it:

- gets or creates the current monkey
- gets or creates the left child monkey
- gets or creates the right child monkey
- assigns the operation
- stores all involved monkeys in the dictionary

If the line contains only a name and a number, it is treated as a value monkey.

For those lines it:

- gets or creates the monkey
- sets its `Value`

This approach allows monkeys to be linked even if referenced before their own definition appears.

---

### Building the Equation String

`Equation(Monkey monkey)` converts the monkey tree into a string expression.

If the monkey is a value node, it returns:

    value.0

If the monkey is an operation node, it returns:

    (left op right)

recursively.

So a monkey graph becomes a fully parenthesised arithmetic expression string.

---

### Evaluating with `Yell()`

`Yell(Monkey monkey)` evaluates the expression string by calling:

    new DataTable().Compute(...)

The result is converted to `decimal`.

So instead of manually walking the tree for the final numeric calculation, the solver builds the equation as text and lets `DataTable` evaluate it.

---

### Part 1 Return Value

`Root()` returns:

- the evaluated result of the `root` monkey

It does this by calling:

    this.Yell(this.Monkeys["root"])

and converting the result to `long`.

So the silver answer is the value produced by the full monkey expression tree.

---

### Part 2 Binary Search

`RootEqual()` searches for the correct value of `humn`.

It starts with:

- `low = 0`
- `high = 10000000000000`

Then it repeatedly:

- sets `humn` to the midpoint
- evaluates the left side of `root`
- evaluates the right side of `root`

If both sides are equal, it returns the current `humn` value.

Otherwise:

- if left is greater than right, move `low`
- else move `high`

So the solver performs a binary search over the possible `humn` values until the two root branches match.

---

### Root Equality Logic

For Part 2, the solver does not directly use an equals operator on `root`.

Instead it explicitly compares:

- `this.Monkeys["root"].Left`
- `this.Monkeys["root"].Right`

and searches for a `humn` value where both evaluate to the same result.

That means the gold solution is based on balancing the two sides of the root expression.

---

## 🛠 Implementation Notes

- Monkeys are stored in a dictionary keyed by name
- Each monkey is either a value node or an operation node
- Child references are linked during parsing
- Expression strings are built recursively
- Final arithmetic evaluation uses `DataTable().Compute(...)`
- Part 1 evaluates the full `root` expression directly
- Part 2 binary-searches the `humn` value
- The search compares the left and right branches of `root`

---

## 🧪 Behaviour Summary

Given a list of named monkey definitions:

- the solver parses them into linked monkey objects
- value monkeys store numbers
- operation monkeys reference two child monkeys and an operator
- Part 1 builds and evaluates the full `root` expression
- Part 2 varies `humn` until both sides of `root` are equal
- the final result is either the root value or the matching human input

---

## 🚀 Key Takeaways

- Good example of representing expressions as linked objects
- Parsing supports forward references by creating placeholder monkeys
- The recursive equation builder keeps evaluation simple
- Part 1 is straightforward expression evaluation
- Part 2 turns the tree into a search problem
- Binary search avoids brute forcing the full input range

---

## 🔗 References

- https://adventofcode.com/2022/day/21