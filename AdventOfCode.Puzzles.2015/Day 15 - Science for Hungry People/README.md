# 🎄 Advent of Code 2015 - Day 15: Science for Hungry People

## 📜 Puzzle Overview

Santa is trying to bake the perfect cookie.

Each ingredient has five properties:

- capacity
- durability
- flavor
- texture
- calories

A cookie recipe is created by assigning a number of teaspoons to each ingredient, with the total needing to equal `100`.

For scoring:

- multiply the total capacity, durability, flavor, and texture values together
- if any of those totals are negative, treat them as `0` before multiplying

Part 1 asks for the highest possible cookie score using exactly `100` teaspoons.

Part 2 adds one more rule:

- the total calories must equal exactly `500`

---

## 🧩 Part 1

Determine the highest cookie score that can be made using exactly `100` teaspoons.

### 💡 Approach

- Parse each ingredient and its properties
- Generate all possible mixtures that total `100` teaspoons
- Calculate the combined property totals for each mixture
- Clamp negative totals to `0`
- Multiply the four scoring properties together
- Return the highest score found

---

## 🧩 Part 2

Determine the highest cookie score using exactly `100` teaspoons, where the total calories equal `500`.

### 💡 Approach

- Reuse the same mixture generation logic
- Calculate calories alongside the four scoring properties
- Ignore any mixture that does not total exactly `500` calories
- Return the highest remaining score

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ScienceForHungryPeople(this.Input)`
- Calls `HighestRankingMixture(100)`

For Part 2:

- Creates `new ScienceForHungryPeople(this.Input)`
- Calls `HighestRankingMixture(100, 500)`

The only difference between the two parts is the extra calorie requirement in Part 2.

---

### `Ingridents.cs`

This file defines the `Ingrident` model used by the solver.

Each ingredient stores:

- `Name`
- `Capacity`
- `Durability`
- `Flavor`
- `Texture`
- `Calories`

This keeps the parsed input strongly structured and makes mixture scoring easier to read.

---

### `ScienceForHungryPeople.cs`

This class contains the full parsing and scoring logic.

It stores the ingredients in:

- `Ingridents`

The constructor parses the raw input immediately and builds that ingredient collection.

The class exposes three main methods:

- `Parse(string[] input)`
- `Mix(int[] mixture, int requiredCalories = -1)`
- `HighestRankingMixture(int teaspoons, int requiredCalories = -1)`

---

### Parsing Ingredients

The `Parse()` method reads each input line and splits it into:

- the ingredient name
- the numeric property values

Each line is converted into an `Ingrident` object and added to the dictionary.

At a high level, a line like this:

    Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8

becomes a structured object containing all five property values.

This keeps the raw text handling separate from the mixture logic.

---

### Scoring a Mixture

The `Mix()` method takes an integer array representing how many teaspoons have been assigned to each ingredient.

For each ingredient, it calculates:

- total capacity
- total durability
- total flavor
- total texture
- total calories

Each property is multiplied by the number of teaspoons assigned to that ingredient and added to a running total.

Once all ingredients have been processed:

- negative totals are clamped to `0`
- the four scoring properties are multiplied together
- calories are checked only if a required calorie target has been supplied

If a calorie target is present and does not match exactly, the method returns `0`.

That means Part 2 reuses the same scoring method but filters out invalid calorie combinations automatically.

---

### Generating Mixtures

The `HighestRankingMixture()` method searches for the best valid mixture.

It uses:

- an integer array called `mixture`
- a dictionary called `mixtures`

The `mixture` array acts like a rolling counter across all ingredient slots.

Each loop:

- increments the mixture values
- skips any combination where the total does not equal the required teaspoon count
- scores valid combinations using `Mix()`
- stores the mixture score using a joined string key

At a high level, the loop behaves like a multi-position counter that keeps advancing until all valid combinations have been seen.

Only mixtures where:

    mixture.Sum() == teaspoons

are considered.

This guarantees that every accepted recipe uses exactly `100` teaspoons.

---

### Detecting Completion

A string key is built from the current mixture values and stored in the `mixtures` dictionary.

When the same key appears again, the generation loop stops.

This acts as the termination check for the rolling counter logic.

Once all valid mixtures have been evaluated, the method returns:

- the maximum score from `mixtures.Values`

---

## 🛠 Implementation Notes

- Ingredients are parsed once during construction
- Mixtures are represented as integer arrays
- The same solver logic is reused for both parts
- Negative property totals are clamped using `Math.Max(0, value)`
- Calories are included in the calculation only as a filter, not as part of the final score
- The search checks every valid `100`-teaspoon combination and returns the highest score found

---

## 🧪 Examples

Given the example ingredients:

    Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
    Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3

A mixture of:

- `44` teaspoons of Butterscotch
- `56` teaspoons of Cinnamon

produces:

- capacity = `68`
- durability = `80`
- flavor = `152`
- texture = `76`

Final score:

- `68 * 80 * 152 * 76 = 62842880`

For the calorie-constrained version, the best score with exactly `500` calories is:

- `57600000`

---

## 🚀 Key Takeaways

- Good example of brute-forcing bounded combinations
- Keeps parsing, scoring, and mixture generation separated cleanly
- Part 2 is solved by adding a calorie filter rather than rewriting the scoring logic
- Array-based mixture tracking keeps the search compact and easy to reason about

---

## 🔗 References

- https://adventofcode.com/2015/day/15