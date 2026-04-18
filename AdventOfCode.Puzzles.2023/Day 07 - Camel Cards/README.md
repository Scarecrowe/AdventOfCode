# 🎄 Advent of Code 2023 - Day 07: Camel Cards

## 📜 Puzzle Overview

This puzzle ranks poker-like hands and calculates total winnings based on their ordering.

Each input line contains:

- a five-card hand
- a bid value

The solver parses every hand, determines its strength, sorts all hands from weakest to strongest, assigns rank positions, and then multiplies each bid by its final rank.

Part 1 uses normal card ordering.

Part 2 enables joker wildcard behaviour, where `J` can stand in for other cards when determining hand strength.

---

## 🧩 Part 1

Rank all hands using the normal card rules and sum the resulting winnings.

### 💡 Approach

- Parse each input line into:
  - hand value
  - bid
- Evaluate the hand type:
  - five of a kind
  - four of a kind
  - full house
  - three of a kind
  - two pair
  - one pair
  - high card
- Build a sortable representation of the cards based on the configured card order
- Sort all hands by:
  - hand strength
  - then card ordering
- Reverse the sorted sequence so the weakest hand gets rank `1`
- Multiply each bid by its 1-based rank
- Sum the results

---

## 🧩 Part 2

Repeat the ranking, but treat jokers as wildcards when evaluating hand strength.

### 💡 Approach

- Reuse the same parsing and ranking pipeline
- Change the card ordering so `J` becomes the weakest card for tie-breaking
- When a hand contains `J`:
  - try replacing all jokers with each non-joker card in the configured order
  - compute the strength of each replacement
  - keep the best possible strength
- Sort and score the hands exactly as before

This means jokers improve hand classification, but still remain low for direct card-order comparisons.

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Camel Cards`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new CamelCards(this.Input)`
- calls `Play()`

For Part 2:

- creates `new CamelCards(this.Input, true)`
- calls `Play()`

So the difference between silver and gold is controlled by the `wildcards` constructor parameter.

---

### `CamelCards.cs`

This class contains the parsing and scoring logic.

It stores:

- `Hands`

The constructor chooses the card priority string based on whether wildcard mode is enabled.

For normal play it uses:

    AKQJT98765432

For wildcard play it uses:

    AKQT98765432J

That second ordering moves `J` to the end, making it the weakest card for tie-breaking.

The constructor then parses the full input into a dictionary of hands.

---

### Parsing the Input

`Parse(string order, string[] input)` converts each input line into a `Hand`.

It does this by:

- splitting each line on a space
- using the first value as the hand
- using the second value as the bid

At a high level it behaves like this:

- read `"32T3K 765"`
- create `new Hand("32T3K", "765", order)`

The parsed result is stored as:

- key = hand string
- value = `Hand` object

---

### `Play()`

This method solves both parts.

It sorts all parsed hands using:

- `OrderBy(x => x.Value.Strength)`
- `ThenBy(x => x.Value.Ordered)`
- `Reverse()`

Then it calculates the final winnings with:

- `bid * (rank)`

using a 1-based index.

So the weakest hand ends up multiplied by `1`, the next weakest by `2`, and so on.

The final answer is the sum of all ranked bid values.

---

### `Hand.cs`

This class models one hand and computes everything needed for sorting.

It stores:

- `Value`
- `Ordered`
- `Bid`
- `Strength`

The constructor accepts:

- the five-card hand string
- the bid string
- the active card ordering string

It then:

- stores the hand text
- computes the hand strength
- parses the bid into an integer
- converts the hand into a sortable ordered string

---

### Hand Strength Detection

`GetStrength(string hand)` determines the category of a hand by grouping identical cards.

It builds the count pattern for the hand, sorts the counts descending, and converts them to a compact string.

Examples of these patterns are:

- `5`
- `41`
- `32`
- `311`
- `221`
- `2111`
- `11111`

Those patterns are mapped to `HandStrength` values using:

    new[] { "5", "41", "32", "311", "221", "2111", "11111" }

This corresponds to:

- `FiveOfAKind`
- `FourOfAKind`
- `FullHouse`
- `ThreeOfAKind`
- `TwoPair`
- `OnePair`
- `HighCard`

So the enum order directly matches the grouped-count patterns.

---

### Wildcard Handling

When wildcard mode is active, the constructor checks:

- whether the configured order ends with `J`
- whether the hand actually contains `J`

If both are true, the solver evaluates multiple possible replacements.

It does this by:

- iterating over every non-joker card in the order string
- replacing every `J` in the hand with that card
- calling `GetStrength(...)` on the resulting hand
- taking the minimum enum value

Because stronger hands appear earlier in the enum, the minimum value represents the best possible hand category.

So a hand such as:

    QJJQ2

can be upgraded by replacing both jokers with the same candidate card during evaluation.

---

### Ordered Card Comparison

In addition to strength, hands also need a stable card-by-card comparison.

This is stored in:

- `Ordered`

It is built using:

    this.Value.Select(x => 'A' + order.IndexOf(x)).ToStringX();

That means each card is converted into a sortable character based on its position in the chosen order string.

This lets the solver compare hands of equal strength using the original hand layout and the current card ranking rules.

---

### `HandStrength.cs`

This enum defines the possible hand categories.

It contains:

- `FiveOfAKind`
- `FourOfAKind`
- `FullHouse`
- `ThreeOfAKind`
- `TwoPair`
- `OnePair`
- `HighCard`

The enum order matters because lower enum values represent stronger hands in this implementation.

That is why wildcard evaluation takes the minimum strength, and why the final sort is reversed before ranking.

---

## 🛠 Implementation Notes

- The puzzle title is `Camel Cards`
- The solver stores parsed hands in a `Dictionary<string, Hand>`
- Part 1 uses card order:
  - `AKQJT98765432`
- Part 2 uses card order:
  - `AKQT98765432J`
- Jokers only act as wildcards for strength evaluation
- Jokers remain the weakest card for tie-breaking in Part 2
- Hand categories are determined from grouped card-count patterns
- Equal-strength hands are compared using the derived `Ordered` string
- Final winnings are calculated from the reversed sorted order
- Lower `HandStrength` enum values represent stronger hand types

---

## 🧪 Behaviour Summary

Given a list of card hands and bids:

- the solver parses each hand and bid
- computes the hand category
- derives a sortable representation for tie-breaking
- sorts all hands by strength and card order
- assigns ranks from weakest to strongest
- multiplies each bid by its rank
- sums the totals

So the final result is either:

- the total winnings under normal card rules
- or the total winnings when jokers act as wildcards

---

## 🚀 Key Takeaways

- The solution keeps the full ranking logic compact by pushing most behaviour into the `Hand` model
- Grouped card counts provide a clean way to classify hand types
- Wildcard support is implemented by testing joker replacements and choosing the strongest result
- Tie-breaking is separated from strength calculation through the `Ordered` value
- Enum ordering is intentionally used as part of the ranking logic
- Both parts share the same scoring pipeline, with only the card order and wildcard behaviour changing

---

## 🔗 References

- https://adventofcode.com/2023/day/7