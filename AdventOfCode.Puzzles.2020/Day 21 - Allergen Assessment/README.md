# 🎄 Advent of Code 2020 - Day 21: Allergen Assessment

## 📜 Puzzle Overview

This puzzle analyses a list of foods.

Each input line contains:

- a set of ingredients
- an optional allergen list in parentheses

An input line looks like this:

    mxmxvkd kfcds sqjhc nhms (contains dairy, fish)

The solver parses each line into a `Food` object containing:

- `Ingredients`
- `Allergens`
- `Raw`

Part 1 identifies ingredients that cannot possibly contain any allergen and counts how many times they appear.  
Part 2 determines the exact ingredient-to-allergen mapping and returns the canonical dangerous ingredient list.

---

## 🧩 Part 1

Determine how many times ingredients that cannot contain any allergen appear in the input.

### 💡 Approach

- Parse all foods into structured objects
- For each allergen:
  - find all foods that mention it
  - identify ingredients common to every one of those foods
  - remove those possible allergen ingredients from all foods
- After processing all allergens:
  - everything left in the ingredient lists is treated as non-allergenic
- Count how many remaining ingredient occurrences there are

---

## 🧩 Part 2

Determine the canonical dangerous ingredient list.

### 💡 Approach

- Reuse the same parsing logic
- First identify and count the non-allergen ingredients
- Reparse the input to restore the original food data
- Remove all known non-allergen ingredients from every food
- Repeatedly resolve allergens by finding cases where only one ingredient can match
- Store each resolved `(ingredient, allergen)` pair
- Sort the pairs by allergen name
- Join the ingredients with commas

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Allergen Assessment`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `AllergenAssessment.NonAllergens(this.Input)`

For Part 2:

- Calls `AllergenAssessment.Dangerous(this.Input)`

---

### `Food.cs`

This class models a single food entry.

It stores:

- `Allergens`
- `Ingredients`
- `Raw`

The constructor:

- initialises empty allergen and ingredient lists
- stores the original raw input line

So each line becomes one `Food` object with parsed ingredient and allergen data.

---

### `Foods.cs`

This class is a `List<Food>` with a parser helper.

`Parse(string[] input)` builds the full food collection.

For each input line it:

- creates a `Food`
- scans the line character by character
- builds ingredient names until a space is reached
- when `(` is encountered:
  - finds the closing `)`
  - extracts the allergen section
  - removes the text `contains `
  - splits allergens on commas
  - trims and stores them
- adds the completed `Food` to the list

This preserves ingredients and allergens separately for later analysis.

---

### `AllergenAssessment.cs`

This class contains both puzzle solutions.

It exposes:

- `NonAllergens(string[] input)`
- `Dangerous(string[] input)`

Both methods start by parsing the full input into `Foods`.

---

### Part 1: `NonAllergens(string[] input)`

This method solves the silver puzzle.

It first parses:

- `Foods foods = Foods.Parse(input);`

Then for each distinct allergen it:

- collects all foods containing that allergen
- checks each ingredient in those foods
- keeps ingredients that appear in every matching food
- removes those shared ingredients from every food in the full collection

After all allergens have been processed, the remaining ingredients are treated as non-allergens.

The method then:

- counts every remaining ingredient occurrence across all foods
- returns that total

So the silver answer is not the number of unique safe ingredients, but the total number of times safe ingredients still appear.

---

### How Possible Allergen Ingredients Are Found

For each allergen:

- gather every food that lists it
- inspect ingredients from those foods
- test whether each ingredient exists in all of them

Logically the check is:

    ingredient is possible if every matching food contains it

Any ingredient that passes this test is considered a possible allergen carrier and removed from the working food lists in Part 1.

---

### Part 2: `Dangerous(string[] input)`

This method solves the gold puzzle.

It starts similarly:

- parse the foods
- run the same allergen comparison process used earlier
- determine which ingredients are non-allergenic by what remains

Then it reparses the input so it can work again from a clean dataset.

Next it:

- removes all identified non-allergen ingredients from every food

At this point, only allergen-candidate ingredients remain.

---

### Resolving Exact Allergen Matches

Part 2 then repeatedly narrows the mapping.

It creates:

- a result list of `(ingredient, allergen)` pairs
- a processed allergen list
- a list of all distinct allergens

Then inside a loop, for each unprocessed allergen it:

- gathers all foods containing that allergen
- collects distinct remaining ingredients from those foods
- keeps only ingredients present in every matching food

If exactly one ingredient remains:

- that ingredient is paired with the allergen
- the ingredient is removed from every food
- the allergen is marked as processed

This repeats until all allergens have been resolved.

---

### Canonical Dangerous Ingredient List

Once every allergen has been matched, the method returns:

- the resolved pairs sorted by allergen name
- the ingredient names joined by commas

So the final output is the canonical dangerous ingredient list in allergen-name order.

---

## 🛠 Implementation Notes

- `Food` stores parsed ingredients, allergens, and the raw line
- `Foods.Parse(...)` uses manual character scanning rather than regex
- Part 1 mutates the working food list by removing possible allergen ingredients
- Remaining ingredients after that process are counted as non-allergens
- Part 2 reparses the input to restore the original ingredient lists
- Non-allergen ingredients are stripped out before final allergen resolution
- Exact mappings are resolved by repeated elimination
- Final dangerous ingredients are sorted by allergen and joined with commas

---

## 🧪 Behaviour Summary

Given a list of foods with ingredients and allergens:

- the solver parses each line into a `Food`
- Part 1 removes ingredients that could carry allergens
- the remaining ingredient occurrences are counted
- Part 2 removes known safe ingredients from a fresh parse
- allergens are resolved one by one through intersection and elimination
- the final result is either:
  - the safe ingredient occurrence count, or
  - the canonical dangerous ingredient list

---

## 🚀 Key Takeaways

- Nice example of constraint solving through repeated set intersection
- Parsing is done manually and cleanly without regex
- Part 1 works by removing allergen candidates and counting what remains
- Part 2 uses elimination to reduce each allergen to a single ingredient
- Re-parsing the input keeps the destructive Part 1-style logic from corrupting Part 2

---

## 🔗 References

- https://adventofcode.com/2020/day/21