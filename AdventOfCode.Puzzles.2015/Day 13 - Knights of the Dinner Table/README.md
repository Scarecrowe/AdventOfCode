# 🎄 Advent of Code 2015 - Day 13: Knights of the Dinner Table

## 📜 Puzzle Overview

The holiday feast needs a seating plan that keeps everyone as happy as possible.

Each input line describes how many happiness units a person would gain or lose by sitting next to someone else.

For example:

- `Alice would gain 54 happiness units by sitting next to Bob.`
- `Alice would lose 79 happiness units by sitting next to Carol.`

Because the table is circular:

- Every person has exactly two neighbours
- The first and last people in the arrangement also sit next to each other

Part 1 asks for the seating arrangement with the highest total happiness.

Part 2 adds one extra guest with a neutral happiness score against everyone else, then asks for the new best arrangement.

---

## 🧩 Part 1

Determine the seating arrangement that produces the **highest total happiness**.

### 💡 Approach

- Parse each line into a directional happiness value between two people
- Store each person's happiness impact toward every other person
- Generate every possible seating order
- For each arrangement, add the happiness contribution from both neighbours for every person
- Return the highest total

---

## 🧩 Part 2

Add one more guest with zero happiness impact in both directions for every pairing.

Then determine the new optimal seating arrangement.

### 💡 Approach

- Reuse the same guest map
- Add one extra person with `0` against everyone
- Generate seating permutations again
- Return the highest total happiness

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new KnightsOfTheDinnerTable(this.Input)`
- Calls `OptimalSeating()`

For Part 2:

- Creates `new KnightsOfTheDinnerTable(this.Input)`
- Calls `AddPerson("Scarecrowe").OptimalSeating()`

Both answers are returned as strings.

---

### `KnightsOfTheDinnerTable.cs`

This class contains the full seating logic.

It stores guest relationships in:

- `People`

This is a nested dictionary structure where:

- the outer key is a person's name
- the inner key is their neighbour's name
- the value is the happiness gained or lost by that pairing

At a high level, it works like this:

- `People["Alice"]["Bob"] = 54`
- `People["Alice"]["Carol"] = -79`

This keeps all directional happiness values easy to look up during scoring.

---

### Parsing the Guest List

The constructor calls:

- `ParseGuestList(input)`

Each line is split into tokens and converted into a directional happiness value.

Parsing handles both:

- `gain`
- `lose`

If the line says `gain`, the value stays positive.

If the line says `lose`, the value is multiplied by `-1`.

The trailing full stop is removed before splitting so the neighbour name can be read cleanly.

---

### Generating Seating Arrangements

The `PossibleSeating()` method builds a list of all guest names and generates every possible permutation.

Each permutation represents one full seating order around the table.

This means the solver tries every possible arrangement and scores each one separately.

---

### Scoring an Arrangement

The `OptimalSeating()` method loops through every possible arrangement and calculates its total happiness.

For each person in the arrangement:

- find the next seat using wrapped indexing
- add the happiness from person A sitting next to person B
- add the happiness from person B sitting next to person A

The wrapped indexing makes the table circular, so the last person is still linked back to the first.

At a high level, the neighbour lookup behaves like this:

    next = ((i + 1) + arrangement.Count) % arrangement.Count

This keeps the final seat connected to the start of the arrangement.

Each arrangement total is stored, and the maximum value is returned.

---

### Adding an Extra Guest

The `AddPerson(string name)` method is used for Part 2.

It:

- adds a new person into `People`
- creates a zero-value relationship from the new person to everyone else
- creates a zero-value relationship from everyone else back to the new person

This allows the same arrangement logic to be reused without changing the scoring method.

In this solution, the added guest is:

- `Scarecrowe`

with a neutral value of `0` for all pairings.

---

## 🛠 Implementation Notes

- Input is parsed once into a nested dictionary structure
- Happiness is directional, so both sides of a neighbour pairing are stored separately
- Seating orders are generated using permutations
- Circular seating is handled with wrapped indexing
- Part 2 reuses the same solver by adding one neutral guest before recalculating
- The final answer is the maximum arrangement score

---

## 🧪 Examples

Given the example guest list from the puzzle, the best arrangement produces a total happiness score of:

- `330`

This is the highest value after checking every circular seating arrangement.

---

## 🚀 Key Takeaways

- Good example of modelling directional relationships with nested dictionaries
- Circular neighbour logic is handled cleanly with wrapped indexing
- Part 2 is solved by extending the same structure rather than rewriting the algorithm
- Permutation-based search keeps the implementation simple and easy to follow

---

## 🔗 References

- https://adventofcode.com/2015/day/13