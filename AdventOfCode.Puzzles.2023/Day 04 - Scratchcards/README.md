# 🎄 Advent of Code 2023 - Day 4: Scratchcards

## 📜 Puzzle Overview

This puzzle processes a set of scratchcards.

Each input line describes one card and contains:

- a card number
- a list of winning numbers
- a list of numbers on your card

A line looks like this:

    Card 1: 41 48 83 86 17 | 83 86 6 31 17 9 48 53

The solver parses each line into a `Card` object containing:

- `Number`
- `Winning`
- `Numbers`

Part 1 scores each card by doubling points for each additional match. Part 2 recursively counts how many extra cards are won from matching numbers.

---

## 🧩 Part 1

Calculate the total scratchcard points.

### 💡 Approach

- Parse every input line into a card
- Compare each number on the card against the winning numbers
- Start at 0 points for the card
- On the first match, set points to 1
- On each later match, double the current points
- Add each card's points to the final total

---

## 🧩 Part 2

Calculate how many total scratchcards you end up with.

### 💡 Approach

- Parse every card and store it by card number
- For each card, count it as one card scratched
- For every matching number on that card:
  - win the next card
  - then recursively scratch that won card too
- Sum the total number of scratched cards across all originals

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Scratchcards`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Scratchcards(this.Input)`
- Calls `TotalPoints()`

For Part 2:

- Creates `new Scratchcards(this.Input)`
- Calls `TotalCards()`

---

### `Card.cs`

This class parses a single scratchcard line.

The constructor:

- initialises `Winning`
- initialises `Numbers`
- splits the input line on `":"`
- parses the left side to get the card number
- splits the right side on `"|"`

It then:

- reads all winning values into `Winning`
- reads all card values into `Numbers`
- ignores empty split results caused by spacing

So a line such as:

    Card 1: 41 48 83 86 17 | 83 86 6 31 17 9 48 53

becomes one fully parsed `Card` instance.

---

### Card Data

Each `Card` stores:

- `Winning`
- `Numbers`
- `Number`

These are exposed as properties:

- `List<int> Winning`
- `List<int> Numbers`
- `int Number`

---

### `Scratchcards.cs`

This class coordinates the full puzzle logic.

The constructor:

- creates `this.Cards = new();`
- loops through every input line
- creates `new Card(input[i])`
- stores each card in a dictionary using `i + 1` as the key

So the full input becomes:

- `Dictionary<int, Card> Cards`

---

### Part 1 Scoring Logic

`TotalPoints()` calculates the total points for all cards.

It starts with:

    int result = 0;

Then for each card it:

- starts `points` at `0`
- loops through `card.Value.Numbers`
- checks whether each number exists in `card.Value.Winning`

When a match is found:

- if points is `0`, set it to `1`
- otherwise double it with:

    points = points == 0 ? 1 : points * 2;

After processing all numbers on that card, the card score is added to the total.

---

### Part 1 Return Value

After all cards are processed, `TotalPoints()` returns:

- the sum of all individual card point totals

So the silver answer is the total scratchcard score across the entire input.

---

### Part 2 Total Card Counting

`TotalCards()` calculates the total number of scratched cards.

It starts with:

    int result = 0;

Then for each original card it calls:

    this.Scratch(card.Value);

and adds that returned value to the total.

---

### Recursive Scratch Logic

The recursive logic lives in:

    Scratch(Card card)

This method begins by counting the current card itself:

    result++;

It then:

- starts `i = 0`
- loops through every number in `card.Numbers`
- checks whether that number appears in `card.Winning`

For each match, it recursively scratches the next card using:

    this.Cards[card.Number + ++i]

and adds that returned count into the result.

So if a card has multiple matches, it recursively processes the next several cards in sequence.

---

### How the Card Copies Work

The implementation uses the card's parsed `Number` property to decide which later cards are won.

For each winning match:

- increment `i`
- fetch the card at `card.Number + i`
- recursively count all cards won from that card as well

This means the gold solution is implemented as a full recursive expansion of all won cards, rather than tracking copy counts iteratively.

---

## 🛠 Implementation Notes

- `Day4.cs` calls `TotalPoints()` for Part 1
- `Day4.cs` calls `TotalCards()` for Part 2
- Input lines are parsed into `Card` objects
- `Card` stores card number, winning numbers, and card numbers
- Part 1 doubles the score on every additional match
- Part 2 recursively scratches won cards
- The card collection is stored as `Dictionary<int, Card>`

---

## 🧪 Behaviour Summary

Given a list of scratchcards:

- the solver parses each line into a card object
- Part 1 checks matches and builds a doubling score for each card
- Part 2 recursively follows all won cards based on the number of matches
- the final answers are either the total point score or the total number of scratched cards including copies

---

## 🚀 Key Takeaways

- Good example of parsing fixed-format text into a small domain object
- Part 1 uses a compact doubling-score rule
- Part 2 uses straightforward recursion to model won-card chains
- The implementation keeps parsing and puzzle logic separated cleanly
- Card lookup by number is handled through a dictionary

---

## 🔗 References

- https://adventofcode.com/2023/day/4