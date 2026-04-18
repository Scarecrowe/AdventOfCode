# 🎄 Advent of Code 2021 - Day 21: Dirac Dice

## 📜 Puzzle Overview

This puzzle simulates two versions of a dice game played on a circular track.

Each player has:

- a starting position on spaces `1` to `10`
- a running score

On a turn:

- the player rolls
- moves forward around the 10-space board
- adds their new position to their score

The solver handles two different game modes:

- Part 1 uses a deterministic 100-sided die
- Part 2 uses a quantum die, where every possible roll outcome splits into many universes

The puzzle entry returns:

    new DiracDice(this.Input).Play()
    new DiracDice(this.Input).PlayQuantum()

---

## 🧩 Part 1

Determine the losing player's score multiplied by the total number of die rolls.

### 💡 Approach

- Parse both player starting positions
- Simulate turns with a deterministic die
- Each turn rolls the die three times
- Move the active player around the circular board
- Add the landing position to that player's score
- Stop when one player reaches a score of at least `1000`
- Return:
  - the lower of the two scores
  - multiplied by the number of die rolls made

---

## 🧩 Part 2

Determine in how many universes the winning player wins.

### 💡 Approach

- Reuse the parsed starting positions
- Replace the deterministic die with the 3-roll quantum version
- Consider every possible sum from rolling `1..3` three times
- Count how many universes each possible sum produces
- Recursively evaluate game states
- Swap player roles between recursive calls
- Accumulate win totals for both players
- Return the larger total

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Dirac Dice`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new DiracDice(this.Input)`
- calls `Play()`

For Part 2:

- creates `new DiracDice(this.Input)`
- calls `PlayQuantum()`

---

### `Player.cs`

This class models a single player.

It stores:

- `Number`
- `Position`
- `Score`

The constructor accepts:

- player number
- starting position

There is also a copy constructor so player state can be duplicated safely when needed.

---

### Player Movement

`IncrementPosition(int roll)` advances the player one step at a time.

At a high level it does:

- loop from `1` to the roll amount
- increase `Position`
- if position goes above `10`
  - wrap back to `1`

So the board is treated as a circular track.

---

### Player Scoring

`IncrementScore()` updates score with:

    this.Score += this.Position

So after every move, the player adds their new board position to their running total.

---

### `Scores(...)`

`Scores(Dictionary<int, int> rolls)` builds a list of possible roll-and-score combinations for the current player.

For each possible roll sum it returns:

- the roll total
- the score that would result from landing on the corresponding destination

It uses the shared lookup table in `DiracDice` to work out the destination space.

---

### `Universe.cs`

This class models one game universe containing two players.

It stores:

- `PlayerA`
- `PlayerB`

It provides:

- a constructor from two players
- a copy constructor from another universe

So a universe can be cloned without mutating the original player state.

---

### Playing a Universe Turn

`Play((int a, int b) roll)` applies one roll to each player in order.

It does:

- move `PlayerA`
- increment `PlayerA` score
- if `PlayerA.Score >= 21`, return that player
- otherwise move `PlayerB`
- increment `PlayerB` score
- if `PlayerB.Score >= 21`, return that player
- otherwise return `null`

This class supports the idea of simulating one branch of a quantum game state, even though the main gold solution uses a more compact recursive score-counting method.

---

### `DiracDice.cs`

This class contains the full parsing and solving logic.

It stores:

- `Players`
- `Wins`
- `DieCount`
- `DiceValue`

It also exposes two shared helpers:

- `PossibleRolls`
- `Lookup`

The constructor parses input lines like:

    Player 1 starting position: 4

At a high level it does:

- remove `"Player "`
- split on `" starting position: "`
- convert both tokens to integers
- create a `Player`
- store it in `Players` using the player number as the key

---

### Precomputing Quantum Rolls

`CreateRolls()` generates all sums produced by rolling three 3-sided dice.

It does this with three nested loops over:

- `1`
- `2`
- `3`

So it produces totals from:

- `3` through `9`

Then it counts how many times each total appears.

That produces a frequency map for the quantum die outcomes.

---

### Precomputing Position Lookup

`CreateLookup()` builds:

- `Dictionary<(int start, int roll), int>`

For every starting position `1..10` and every possible roll sum `3..9`, it calculates the landing position after wrapping around the circular board.

So later logic can look up a destination instantly instead of recalculating movement every time.

---

### Deterministic Die Logic

`RollDice()` handles one deterministic die roll.

It:

- increments `DieCount`
- increments `DiceValue`
- wraps back to `1` after `100`

So the die cycles:

    1, 2, 3, ..., 100, 1, 2, ...

`RollDie()` then performs exactly three deterministic rolls and returns their sum.

---

### Part 1 Main Simulation

`Play()` performs the deterministic game.

It starts by selecting:

- player 1
- player 2

Then it loops until a player wins.

On each iteration it:

- rolls the die three times with `RollDie()`
- moves the active player
- increments that player's score
- checks whether the score has reached `1000`
- switches to the other player if not

At the end it calculates:

    minScore * this.DieCount

So the silver answer is the losing score multiplied by the total number of deterministic die rolls.

---

### Part 2 Recursive Quantum Solver

`PlayQuantum()` starts the recursive universe count with:

    this.RecursiveQuantum(this.Players[1].Position - 1, 21, this.Players[2].Position - 1, 21)

It then returns whichever result is larger.

This means the gold solution works with:

- zero-based positions for the recursive state
- remaining score-to-win values rather than accumulated scores

---

### Recursive State Meaning

`RecursiveQuantum(long positionA, long t1, long positionB, long t2)` represents:

- current player A position
- how many points player A still needs to win
- current player B position
- how many points player B still needs to win

The base case is:

    if (t2 <= 0) return (0, 1);

This works because the recursive call swaps player roles each time. If the second player's remaining target is already zero or less, that player has won in this branch.

---

### Quantum Recursion Flow

For each possible quantum roll total and its frequency:

- update player A's new position with:
  
    (positionA + roll) % 10

- reduce player A's remaining score target by:
  
    1 + newPosition

- recurse with players swapped
- receive the win counts from the deeper branch in reversed order
- multiply those win counts by the frequency of that roll total
- add them into the running totals

The method returns:

- total universes won by player A
- total universes won by player B

So the gold answer is the larger of those two totals.

---

## 🛠 Implementation Notes

- Input is parsed into a `Dictionary<int, Player>`
- Player movement wraps around a 10-space board
- Score increases by the landing position after every move
- Part 1 uses a deterministic 100-sided die rolled three times per turn
- Part 2 uses precomputed frequencies for sums from three 3-sided dice
- The recursive quantum solver swaps player roles on each call
- Positions are treated as zero-based inside the recursive solver
- The `Universe` class exists as a game-state model, even though the final quantum solution is handled directly in `DiracDice`

---

## 🧪 Behaviour Summary

Given two starting positions:

- the solver parses both players
- Part 1 simulates a normal alternating dice game
- each turn moves the current player and updates score
- play stops once someone reaches `1000`
- the answer is the losing score times the number of die rolls

For Part 2:

- every possible 3-roll quantum total is considered
- recursive branching counts wins across all universes
- the answer is the larger of the two total universe win counts

---

## 🚀 Key Takeaways

- Good example of modelling two related puzzle parts with very different scaling requirements
- `Player` cleanly encapsulates board position and score updates
- Part 1 is a direct turn-by-turn simulation
- Part 2 avoids brute force by counting repeated quantum outcomes by frequency
- Recursive role-swapping keeps the quantum solver compact
- Precomputed roll frequencies and landing positions reduce repeated work

---

## 🔗 References

- https://adventofcode.com/2021/day/21