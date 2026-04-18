# 🎄 Advent of Code 2022 - Day 02: Rock Paper Scissors

## 📜 Puzzle Overview

This puzzle scores a list of Rock Paper Scissors rounds.

Each input line contains two tokens separated by a space.

The first token always represents player A's shape:

- `A` = Rock
- `B` = Paper
- `C` = Scissor

The second token is interpreted differently depending on the puzzle part:

- in Part 1 it represents player B's shape
- in Part 2 it represents the desired round result

The solver parses the input into round objects and then sums the score for every round.

---

## 🧩 Part 1

Interpret the second token as player B's shape and calculate the total score.

### 💡 Approach

- Parse each input line into a `RoundWithShape`
- Convert:
  - `A/B/C` into player A's shape
  - `X/Y/Z` into player B's shape
- For each round:
  - work out whether player B wins, draws, or loses
  - add the shape score for player B's chosen shape
  - add the outcome score
- Sum the score from all rounds

### Shape Scores

For player B's shape:

- Rock = `1`
- Paper = `2`
- Scissor = `3`

### Outcome Scores

- Loss = `0`
- Draw = `3`
- Win = `6`

---

## 🧩 Part 2

Interpret the second token as the desired result and calculate the total score.

### 💡 Approach

- Parse each input line into a `RoundWithResult`
- Convert:
  - `A/B/C` into player A's shape
  - `X/Y/Z` into the target result
- For each round:
  - determine which shape player B must play to achieve that result
  - return the correct outcome score plus the chosen shape score
- Sum the score from all rounds

### Desired Result Mapping

- `X` = Lose
- `Y` = Draw
- `Z` = Win

---

## 🧠 Code Breakdown

### `RockPaperScissors.cs`

This class is the main solver.

It stores:

- `Rounds`

The constructor does:

- parse the input into a list of round objects

That happens through:

    RockPaperScissors.Parse(input)

The `Play()` method then returns:

    this.Rounds.Sum(x => x.Score())

So the overall solution is simply the sum of all round scores.

---

### Generic Parsing Behaviour

The parser decides which round type to build based on the generic type parameter.

At a high level it does:

- if the generic type is `PlayerBShape`
  - parse using `RoundWithShape`
- otherwise
  - parse using `RoundWithResult`

So the same outer class can support both parts while changing only how each line is interpreted.

---

### `PlayerAShape.cs`

This enum defines player A's encoded input values:

- `Rock = 'A'`
- `Paper = 'B'`
- `Scissor = 'C'`

So the first token is always mapped directly from the input character into a shape enum.

---

### `PlayerBShape.cs`

This enum defines player B's shape encoding for Part 1:

- `Rock = 'X'`
- `Paper = 'Y'`
- `Scissor = 'Z'`

So in the silver puzzle, the second token is treated as the shape player B actually plays.

---

### `RoundResult.cs`

This enum defines the Part 2 interpretation of the second token:

- `Lose = 'X'`
- `Draw = 'Y'`
- `Win = 'Z'`

So in the gold puzzle, the same input letters no longer mean shapes and instead mean target outcomes.

---

### `RoundWithShape.cs`

This class handles the Part 1 interpretation.

The constructor:

- splits the input line on spaces
- parses the first token into `PlayerAShape`
- parses the second token into `PlayerBShape`

It stores:

- `ValueA`
- `ValueB`

The scoring method does two things:

1. Work out the shape score for player B
2. Add the round result score based on the matchup

---

### Part 1 Shape Score Logic

`ShapeScore()` returns:

- Rock → `1`
- Paper → `2`
- Scissor → `3`

This value is always based on player B's chosen shape.

---

### Part 1 Round Result Logic

`Score()` compares player A's shape against player B's shape.

Examples:

- Rock vs Rock → draw + rock score
- Rock vs Paper → win + paper score
- Rock vs Scissor → loss + scissor score

The same pattern is repeated for all three player A shapes.

The constants used are:

- `Win = 6`
- `Draw = 3`

A loss contributes no extra points beyond the shape score.

---

### `RoundWithResult.cs`

This class handles the Part 2 interpretation.

The constructor:

- splits the input line on spaces
- parses the first token into `PlayerAShape`
- parses the second token into `RoundResult`

It stores:

- `ValueA`
- `ValueB`

Here `ValueB` is not the chosen shape. It is the required round outcome.

---

### Part 2 Score Logic

`Score()` switches first on the desired result:

- Lose
- Draw
- Win

Then inside each branch it checks player A's shape and returns the exact total score needed.

For example:

- if player A plays Rock and the result must be Lose
  - player B must play Scissor
  - score returned is `3`
- if player A plays Rock and the result must be Draw
  - player B must play Rock
  - score returned is `3 + 1`
- if player A plays Rock and the result must be Win
  - player B must play Paper
  - score returned is `6 + 2`

So the gold scoring logic directly encodes the correct response shape and its resulting score.

---

## 🛠 Implementation Notes

- The solver parses all rounds up front
- Both parts use the same `Play()` method to sum scores
- The difference between parts is only how input lines are parsed
- Part 1 interprets `X/Y/Z` as shapes
- Part 2 interprets `X/Y/Z` as desired outcomes
- Scoring is implemented with explicit switch logic
- Shape score and outcome score are combined into one round total

---

## 🧪 Behaviour Summary

Given a list of rounds:

- the first token is always player A's shape
- Part 1 treats the second token as player B's shape
- Part 2 treats the second token as the required result
- each line becomes a round object with its own `Score()` logic
- the final answer is the sum of all round scores

---

## 🚀 Key Takeaways

- Nice example of reusing one outer solver for two different interpretations
- Enums keep the input token mapping very clear
- Part 1 and Part 2 differ only in how the second token is decoded
- The actual round scoring is kept inside dedicated round classes
- `Play()` stays simple because scoring responsibility is pushed down into each round type

---

## 🔗 References

- https://adventofcode.com/2022/day/2