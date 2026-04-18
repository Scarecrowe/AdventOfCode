# 🎄 Advent of Code 2023 - Day 6: Wait For It

## 📜 Puzzle Overview

This puzzle is about winning boat races by choosing how long to hold a button before moving.

Each race provides:

- a total race time
- a record distance to beat

Holding the button for `i` milliseconds causes the boat to travel for the remaining time at speed `i`.

So the travelled distance is:

    i * (time - i)

The solver parses the input into `Race` objects and then counts how many button-hold values beat the target distance.

Part 1 treats the input as multiple separate races. Part 2 removes whitespace and treats the values as one large race.

---

## 🧩 Part 1

Find how many winning button-hold times exist for each race, then multiply those counts together.

### 💡 Approach

- Parse the time line into a list of race times
- Parse the distance line into a list of target distances
- Pair them into `Race` objects
- For each race, try every possible hold time from `0` to `Time`
- Count how many values produce a travelled distance greater than the race record
- Multiply all race win-counts together

---

## 🧩 Part 2

Treat the input as one large race by removing spaces before parsing.

### 💡 Approach

- Reuse the same logic as Part 1
- Parse the numbers again, but ignore spaces completely
- This combines the visible values into one long time and one long distance
- Count the winning hold times for that single race
- Return the result

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Wait For It`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new WaitForIt(this.Input)`
- Calls `Race()`

For Part 2:

- Creates `new WaitForIt(this.Input, true)`
- Calls `Race()`

So the only difference between the two parts is whether whitespace is removed during parsing.

---

### `Race.cs`

This class is a simple data model for one race.

It stores:

- `Time`
- `Distance`

The constructor takes:

- `long time`
- `long distance`

and assigns them to read-only properties.

So each parsed race becomes a compact object containing the total race duration and the record to beat.

---

### `WaitForIt.cs`

This class contains the main puzzle logic.

The constructor:

- accepts the input
- accepts an optional `removeWhiteSpace` flag
- calls `Parse(input, removeWhiteSpace)`

It stores the result in:

- `List<Race> Races`

---

### Race Simulation

`Race()` runs the full calculation.

It creates:

    List<long> result = new();

Then for each race it:

- starts a `combinations` counter at `0`
- loops from `0` to `race.Time`
- calculates whether the hold time beats the distance record

The winning check is:

    i * (race.Time - i) > race.Distance

If true, that hold time is counted as a valid winning option.

After checking every possible hold time for that race:

- the count is added to `result`

When all races are complete, the method returns:

    result.Product()

So the final answer is the product of all winning-count totals.

---

### Parsing the Input

`Parse(string[] input, bool removeWhiteSpace)` builds the race list.

It creates:

- `List<long> time`
- `List<long> distance`

These are read using:

- `StripNumbers(input[0], removeWhiteSpace)`
- `StripNumbers(input[1], removeWhiteSpace)`

Then it loops through the parsed values by index and creates:

    new Race(time[i], distance[i])

So Part 1 produces several race objects, while Part 2 produces a single large one when the spacing is removed.

---

### Number Extraction

`StripNumbers(string line, bool removeWhiteSpace)` parses all numeric values from a line.

It works character by character.

It:

- skips spaces when `removeWhiteSpace` is `true`
- accumulates digits into a temporary string
- converts completed numbers into `long`
- adds them to a result list

This means:

- with normal parsing, spaced values become separate numbers
- with whitespace removal enabled, separated digits merge into one large number

That behaviour is what makes Part 2 work without needing a separate parser.

---

### Part 1 Behaviour

When `removeWhiteSpace` is left as `false`:

- the time line becomes multiple race times
- the distance line becomes multiple race distances
- each pair becomes a separate `Race`
- each race is brute-forced independently
- the win counts are multiplied together

---

### Part 2 Behaviour

When `removeWhiteSpace` is `true`:

- spaces are ignored entirely while reading digits
- the time values collapse into one long number
- the distance values collapse into one long number
- only one `Race` object is created
- the same brute-force logic is reused on that single race

---

## 🛠 Implementation Notes

- `Day6.cs` calls `Race()` for both puzzle parts
- Part 2 passes `true` into the `WaitForIt` constructor
- `Race` stores `Time` and `Distance` as `long`
- Winning distances are checked with:

    i * (race.Time - i) > race.Distance

- The parser uses a shared digit-extraction method for both parts
- Part 1 and Part 2 differ only in whether spaces are removed before parsing
- The final result is produced with `result.Product()`

---

## 🧪 Behaviour Summary

Given race times and record distances:

- the solver parses them into `Race` objects
- for each race it tests every possible button-hold duration
- it counts the hold times that travel farther than the record
- Part 1 multiplies the winning counts across several races
- Part 2 re-parses the input as one large race and applies the same logic

---

## 🚀 Key Takeaways

- Good example of solving two puzzle parts with one shared code path
- The parser behaviour switch cleanly handles the Part 2 input twist
- The implementation uses straightforward brute force for each race
- `Race` is kept as a minimal data container
- The overall structure is simple and easy to follow

---

## 🔗 References

- https://adventofcode.com/2023/day/6