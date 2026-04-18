# 🎄 Advent of Code 2020 - Day 22: Crab Combat

## 📜 Puzzle Overview

This puzzle simulates a card game between two players using decks of numbered cards.

Each player starts with a deck defined in the input, for example:

    Player 1:
    9
    2
    6
    3
    1

    Player 2:
    5
    8
    4
    7
    10

The solver parses the input into two decks and then plays rounds according to the rules of the game.

Part 1 implements standard "Combat", while Part 2 introduces a recursive variant called "Recursive Combat".

---

## 🧩 Part 1

Play the standard version of Combat and determine the winning player's score.

### 💡 Approach

- Parse the input into two queues (one per player)
- Repeat rounds until one player has no cards left
- On each round:
  - Each player draws the top card
  - The player with the higher card wins the round
  - The winner places both cards at the bottom of their deck (winner's card first)
- Once the game ends:
  - Take the winning deck
  - Reverse it
  - Multiply each card by its position (starting at 1)
  - Sum the result to produce the final score

---

## 🧩 Part 2

Play Recursive Combat, a more complex version of the game with sub-games and loop detection.

### 💡 Approach

- Use the same deck setup as Part 1
- Track previous round states to prevent infinite loops
- On each round:
  - If the current deck configuration has been seen before, Player 1 wins immediately
  - Each player draws a card
  - If both players have at least as many cards remaining as the value of their drawn card:
    - Start a sub-game using copies of the next cards
    - The winner of the sub-game wins the round
  - Otherwise:
    - The higher card wins as normal
  - The round winner places both cards at the bottom of their deck
- Continue until one player has all the cards
- Score is calculated the same way as Part 1

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Crab Combat`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates a new game instance with the input
- Runs the standard combat logic

For Part 2:

- Creates a new game instance with the input
- Runs the recursive combat logic

---

### Deck Representation

The decks are typically stored as:

- `Queue<int>` or similar structure

This allows:

- efficient removal from the front (draw)
- efficient addition to the back (winning cards)

---

### Parsing the Input

The parser:

- splits input into two sections (Player 1 and Player 2)
- skips header lines (`Player X:`)
- converts each remaining line into integers
- builds two decks preserving order

---

### Standard Combat Logic

Each round performs:

    p1 = player1.Dequeue()
    p2 = player2.Dequeue()

Then:

- compare `p1` and `p2`
- winner appends cards:

    winner.Enqueue(highCard)
    winner.Enqueue(lowCard)

The game continues until one deck is empty.

---

### Recursive Combat Logic

Recursive Combat introduces two key mechanics:

#### 1. State Tracking

- A history of previous deck configurations is stored
- If a configuration repeats:
  - Player 1 instantly wins the game

This prevents infinite recursion loops.

---

#### 2. Sub-Games

When both players have enough cards:

- create new decks using:
  - the next `p1` cards from Player 1
  - the next `p2` cards from Player 2
- start a new game instance (recursively)
- the winner of the sub-game wins the round

If not enough cards exist:

- fall back to normal high-card comparison

---

### Game Loop

The main loop continues while:

- both players have cards

Each iteration:

- checks for repeated states (Part 2 only)
- draws cards
- determines round winner (standard or recursive)
- appends cards to the winner's deck

---

### Scoring the Winning Deck

Once a winner is determined:

- reverse the deck
- multiply each card by its position:

    score += card * position

Where:

- position starts at 1 for the bottom card

---

### Part 1 Return Value

- The final score of the winning deck after standard Combat

---

### Part 2 Return Value

- The final score of the winning deck after Recursive Combat

---

## 🛠 Implementation Notes

- Decks are processed in FIFO order
- Recursive Combat requires careful copying of deck slices
- State tracking typically uses a `HashSet<string>` or similar
- Deck state must uniquely represent both players' decks
- Infinite loop prevention is critical for correctness
- Sub-games reuse the same logic recursively

---

## 🧪 Behaviour Summary

Given two decks of cards:

- the solver parses them into player decks
- each round draws and compares top cards
- Part 1 uses direct comparison
- Part 2 may spawn recursive sub-games
- repeated states in Part 2 force a Player 1 win
- the game ends when one player has all cards
- scoring is based on the final deck order

---

## 🚀 Key Takeaways

- Clean example of queue-based simulation
- Recursive problem structure with shared logic
- Importance of state tracking to prevent infinite loops
- Demonstrates copying subsets of data safely for recursion
- Same scoring logic reused across both parts

---

## 🔗 References

- https://adventofcode.com/2020/day/22