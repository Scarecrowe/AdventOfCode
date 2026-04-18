# 🎄 Advent of Code 2017 - Day 16: Permutation Promenade

## 📜 Puzzle Overview

This puzzle simulates a dance performed by programs named from `a` to `p`.

The starting arrangement is built as:

    abcdefghijklmnop

The input is a comma-separated list of dance moves. Each move changes the ordering of the programs in one of three ways:

- `Spin`
- `Exchange`
- `Partner`

Part 1 applies the full move list once and returns the final arrangement. Part 2 repeats the same dance sequence 1,000,000,000 times and returns the final arrangement after all repetitions.

---

## 🧩 Part 1

Determine the final arrangement after applying the full dance move list once.

### 💡 Approach

- Build the initial string from `a` through `p`
- Parse the input into a list of promenade moves
- Run each move in order against the current string
- Return the final arrangement

---

## 🧩 Part 2

Determine the final arrangement after repeating the dance sequence 1,000,000,000 times.

### 💡 Approach

- Reuse the same parsed move list
- Cache previously seen full-string states in a dictionary
- For each step:
  - if the current arrangement has not been seen before, compute and store its next arrangement
  - otherwise reuse the cached transition
- Repeat until the requested number of dance cycles has been applied
- Return the final arrangement

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Permutation Promenade`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PermutationPromenade(this.Input)`
- Calls `Sort(PermutationPromenade.InitialValue())`

For Part 2:

- Creates `new PermutationPromenade(this.Input)`
- Calls `Sort(PermutationPromenade.InitialValue(), 1000000000)`

---

### `PermutationPromenade.cs`

This class contains the parsing and dance execution logic.

It stores:

- `Moves`

The constructor:

- parses the input by calling `Parse(input)`

It also exposes:

- `InitialValue()`
- `Sort(string value)`
- `Sort(string value, int steps)`

---

### Building the Initial Arrangement

`InitialValue()` creates the starting program order using a `StringBuilder`.

It loops from character code `a` to `p` and appends each letter.

That produces:

    abcdefghijklmnop

This is the starting state for both puzzle parts.

---

### Parsing the Moves

`Parse(string[] input)` reads the first input line and splits it by commas.

Each token becomes:

- `new PromenadeMove(move)`

The parsed result is stored as:

- `List<PromenadeMove>`

So an input such as:

    s1,x3/4,pe/b

becomes a list of move objects ready to run in sequence.

---

### `PromenadeMove.cs`

This class parses and executes a single dance move.

Each move stores:

- `Type`
- `ValueA`
- `ValueB`

The constructor:

- determines the move type from the first character
- removes that first character
- splits the remaining text on `/`
- parses numbers as integers
- stores letters as character values

If the move is a `Spin`, only `ValueA` is needed.

---

### Move Types

`PromenadeMoveType.cs` defines the three supported move kinds:

- `Spin`
- `Exchange`
- `Partner`

The move type is determined from the first letter of the input token:

- `s` => `Spin`
- `x` => `Exchange`
- `p` => `Partner`

---

### Running a Move

`Run(string value)` executes a move using a switch expression.

It applies one of these operations:

- `Spin` => `value.RotateRight(this.ValueA)`
- `Exchange` => `value.SwapPosition(this.ValueA, this.ValueB)`
- `Partner` => `value.SwapLetter((char)this.ValueA, (char)this.ValueB)`

So each move transforms the current program ordering and returns the updated string.

---

### What Each Move Means

At a high level:

#### Spin

Moves the last `X` programs to the front.

Example:

    abcde -> eabcd

for a spin of `1`.

#### Exchange

Swaps programs by position.

Example:

    eabcd

with positions `3` and `4` swapped becomes:

    eabdc

#### Partner

Swaps programs by letter, regardless of where they currently are.

Example:

    eabdc

partnering `e` and `b` becomes:

    baedc

---

### Part 1 Logic

`Sort(string value)` performs one full dance.

It loops through every move in `Moves` and updates the current string with:

    value = move.Run(value)

After all moves have been applied, it returns the final arrangement.

---

### Part 2 Logic

`Sort(string value, int steps)` performs the dance repeatedly.

It creates:

- `Dictionary<string, string> states = new();`

Then for each step:

- check whether the current arrangement already exists in the dictionary
- if not:
  - store the current arrangement as the key
  - compute the next arrangement with `this.Sort(value)`
  - add the transition to the dictionary
- if it does exist:
  - jump directly to the cached next arrangement

This avoids recalculating the full dance for states that have already been seen before.

---

### State Caching

The dictionary stores transitions in this form:

- current arrangement => next arrangement

That means once a state has been processed once, future visits to that same state can skip rerunning the whole move list and immediately reuse the previously computed result.

This makes the repeated dance in Part 2 much more practical than blindly recalculating every move sequence from scratch each time.

---

## 🛠 Implementation Notes

- The initial value is generated programmatically from `a` to `p`
- Input parsing only uses the first line of puzzle data
- Moves are represented as typed objects instead of being interpreted inline every time
- `Spin`, `Exchange`, and `Partner` are handled through one `Run()` method
- Part 1 performs one direct pass through the move list
- Part 2 caches state transitions to avoid repeated full recomputation

---

## 🧪 Behaviour Summary

Given a comma-separated list of dance instructions:

- the solver builds the starting arrangement `abcdefghijklmnop`
- each move transforms the current ordering
- Part 1 applies the full sequence once
- Part 2 repeats the same sequence many times
- cached state transitions let repeated arrangements be reused instead of recalculated
- the final output is always the resulting string arrangement

---

## 🚀 Key Takeaways

- Good example of modelling instructions as typed move objects
- Clean separation between parsing and execution
- `Spin`, `Exchange`, and `Partner` each represent a distinct transformation style
- Part 1 is a straightforward sequential application of moves
- Part 2 speeds up repeated processing by caching known state transitions

---

## 🔗 References

- https://adventofcode.com/2017/day/16