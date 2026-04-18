# 🎄 Advent of Code 2020 - Day 18: Operation Order

## 📜 Puzzle Overview

This puzzle evaluates arithmetic expressions using unusual operator rules.

Each input line is an equation containing:

- numbers
- addition
- multiplication
- parentheses

Example input:

    1 + 2 * 3 + 4 * 5 + 6

The solver supports two evaluation modes:

- Part 1 evaluates expressions strictly from left to right, ignoring normal operator precedence
- Part 2 makes addition happen before multiplication

Both parts process every equation independently, then sum all results.

---

## 🧩 Part 1

Evaluate each expression from left to right, while still respecting parentheses.

### 💡 Approach

- Read each equation line
- Resolve nested parentheses recursively
- Evaluate the flattened expression strictly left to right
- Collect each equation result
- Return the sum of all equation results

---

## 🧩 Part 2

Evaluate each expression so that addition has higher precedence than multiplication.

### 💡 Approach

- Read each equation line
- Resolve nested parentheses recursively
- Before final calculation, rewrite the expression to insert brackets around addition groups
- Evaluate the rewritten expression
- Collect each equation result
- Return the sum of all equation results

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Operation Order`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `OperationOrder.Simple(this.Input)`

For Part 2:

- Calls `OperationOrder.Advanced(this.Input)`

---

### `OperationOrder.cs`

This class contains the full expression parsing and evaluation logic.

It exposes two public methods:

- `Simple(string[] input)`
- `Advanced(string[] input)`

Both methods:

- create a list of per-line results
- parse each equation
- sum the results at the end

So both puzzle parts follow the same outer flow, but use different parsing and calculation paths.

---

### `Simple(string[] input)`

This method solves Part 1.

It:

- creates `List<long> sums = new();`
- loops through every input equation
- calls `ParseEquationSimple(equation)`
- adds each result to the list
- returns `sums.Sum()`

So the silver answer is the total of all left-to-right evaluated equations.

---

### `Advanced(string[] input)`

This method solves Part 2.

It follows the same pattern as Part 1, but calls:

- `ParseEquationAdvanced(equation)`

So the gold answer is the total of all equations after applying the custom precedence rules.

---

### Finding Matching Parentheses

`FindClosingParentheses(string equation, int start)` locates the matching closing bracket for a given opening bracket.

It works by:

- starting with a nesting counter of `1`
- scanning forward through the string
- increasing the count on `(`
- decreasing the count on `)`
- returning once the counter drops back to `0`

This helper is used by both parsing modes when recursively processing nested expressions.

---

### Part 1 Parsing

`ParseEquationSimple(string equation)` handles recursive parsing for the left-to-right mode.

It builds up a temporary string called `current`.

As it scans the equation:

- on `(`:
  - it recursively parses the inner expression
  - appends the resulting numeric value into `current`
  - skips forward to the matching `)`
- on `)`:
  - it immediately evaluates the current accumulated expression
- otherwise:
  - it appends the current character directly

At the end, it evaluates whatever remains in `current`.

This means parentheses are reduced first, and then the flattened expression is calculated.

---

### Part 1 Calculation

`CalculateSimple(string equation)` evaluates a flattened expression.

It:

- splits the expression into tokens using `SplitSpace()`
- pushes operators onto one stack
- pushes numbers onto another stack
- rebuilds both stacks in reverse order
- starts with the first number
- repeatedly applies the next operator to the next number

The key point is that it does not give `+` or `*` any special priority.

So an expression like:

    1 + 2 * 3 + 4

is processed in strict left-to-right order.

---

### Part 2 Parsing

`ParseEquationAdvanced(string equation)` is the recursive parser for the advanced mode.

It uses a `StringBuilder current`.

As it scans the equation:

- on `(`:
  - it recursively parses the inner expression
  - appends the resulting value into `current`
  - skips to the matching `)`
- on `)`:
  - it first calls `Precedence(current.ToString())`
  - if the rewritten result still contains parentheses, it parses again
  - otherwise it evaluates with `CalculateAdvanced(...)`
- otherwise:
  - it appends the character to `current`

After the loop, it again applies `Precedence(...)` and either reparses or evaluates the final expression.

So Part 2 works in two stages:

- rewrite the expression to enforce precedence
- then calculate the rewritten form

---

### Part 2 Calculation

`CalculateAdvanced(string equation)` is structurally very similar to `CalculateSimple(...)`.

It:

- splits the expression into tokens
- stores operators in a stack
- stores numbers in a stack
- reorders the stacks
- evaluates from left to right

The difference is that by the time this method runs, the expression has already been rewritten so that additions are grouped first.

So the calculator itself stays simple because the precedence work was handled earlier.

---

### Enforcing Addition Precedence

`Precedence(string equation)` rewrites a flat expression so additions happen before multiplications.

It first exits early when:

- the expression still contains `(`, or
- the expression does not contain both `+` and `*`

Otherwise it:

- removes spaces
- scans the expression character by character
- inserts `(` before an addition group begins
- inserts `)` before a multiplication boundary ends that group
- appends a closing bracket at the end if needed
- restores spacing around operators before returning

This produces a transformed equation where addition chunks are explicitly grouped.

Conceptually, something like:

    1 + 2 * 3 + 4

becomes grouped so the additions are evaluated first.

---

### Overall Flow

For each equation:

Part 1:

- recursively reduce parentheses
- evaluate the resulting flat expression left to right

Part 2:

- recursively reduce parentheses
- rewrite flat expressions with extra grouping around additions
- recursively reprocess if new parentheses were introduced
- evaluate the final grouped expression

Then both parts:

- add every per-line result together
- return the total

---

## 🛠 Implementation Notes

- Both puzzle parts sum the result of every input line
- Parentheses are handled with explicit recursive parsing
- `FindClosingParentheses(...)` tracks nesting depth manually
- Part 1 uses no operator precedence beyond parentheses
- Part 2 enforces precedence by rewriting the expression string
- Both calculators use stack-based token processing
- Part 2 uses `StringBuilder` while building expressions

---

## 🧪 Behaviour Summary

Given a list of arithmetic expressions:

- the solver reads each equation line
- recursively resolves nested parentheses
- Part 1 evaluates expressions strictly left to right
- Part 2 rewrites expressions so addition is grouped before multiplication
- each line produces a numeric result
- the final answer is the sum of all line results

---

## 🚀 Key Takeaways

- Good example of solving expression parsing without a full parser engine
- Part 1 keeps evaluation simple by ignoring normal precedence
- Part 2 cleverly rewrites equations instead of building a more complex evaluator
- Parentheses are handled with recursion and explicit matching
- The same overall structure supports both puzzle parts with different parsing rules

---

## 🔗 References

- https://adventofcode.com/2020/day/18