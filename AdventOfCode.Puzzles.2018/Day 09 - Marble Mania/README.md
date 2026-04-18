# 🎄 Advent of Code 2018 - Day 09: Marble Mania

## 📜 Puzzle Overview

This puzzle simulates a marble game played in a circle by multiple players.

The input is a single line describing:

- the number of players
- the value of the last marble

An input line looks like this:

    10 players; last marble is worth 1618 points

The solver parses that line, creates a score array for all players, and plays the game using a circular marble structure.

Part 1 runs the game with the given last marble value.

Part 2 runs the same game again, but multiplies the marble count by `100`.

---

## 🧩 Part 1

Determine the highest score after the marble game finishes.

### 💡 Approach

- Parse the number of players from the input line
- Parse the last marble value
- Create a circular marble list starting with `0`
- Iterate through each new marble value
- For normal turns:
  - move one step clockwise
  - insert the new marble after that position
- For marbles divisible by `23`:
  - move seven steps counter-clockwise
  - add the current marble value and the removed marble value to the current player's score
  - remove that marble from the circle
- Return the maximum player score

---

## 🧩 Part 2

Determine the highest score when the last marble value is multiplied by `100`.

### 💡 Approach

- Reuse the exact same game logic
- Multiply the marble count by `100`
- Run the same placement and scoring rules
- Return the maximum score after the larger simulation completes

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Marble Mania`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MarbleMania(this.Input[0])`
- Calls `Play()`

For Part 2:

- Creates `new MarbleMania(this.Input[0])`
- Calls `Play(100)`

---

### `MarbleMania.cs`

This class contains the full marble game logic.

It stores:

- `PlayerCount`
- `MarbleCount`
- `Players`

The constructor:

- splits the input line on `" players; last marble is worth "`
- parses the player count
- removes `" points"` from the second token
- parses the marble count
- creates a `long[]` score array sized to the number of players

So a line such as:

    10 players; last marble is worth 1618 points

becomes a fully initialised game configuration.

---

### Player Score Storage

Player scores are stored in:

    long[] Players

This allows the solver to accumulate large scores safely, especially for Part 2 where the marble count becomes much larger.

Each array index represents a player.

---

### Main Game Loop

`Play(int multiplier = 1)` performs the simulation.

It first scales the marble count:

    this.MarbleCount *= multiplier;

So:

- Part 1 uses the original marble count
- Part 2 uses `100` times that value

It then creates the marble circle with:

    LinkedList<int> used = new();

and starts the circle with:

    LinkedListNode<int> current = used.AddFirst(0);

The main loop runs as:

    for (int i = 0; i < this.MarbleCount; i++)

Each iteration places or scores marble value:

    i + 1

---

### Normal Marble Placement

When the marble value is **not** divisible by `23`, the solver inserts the new marble with:

    current = used.AddAfter(Next(ref current, ref used), i + 1);

This means it:

- moves one position clockwise from the current marble
- inserts the new marble after that node
- makes the inserted marble the new current marble

This matches the standard placement rule for the puzzle.

---

### Special Scoring Turns

When the marble value is divisible by `23`, the solver performs a scoring turn:

    if (((i + 1) % 23) == 0)

It then moves seven marbles counter-clockwise:

    for (int j = 1; j <= 7; j++)
    {
        current = Previous(ref current, ref used);
    }

After that, it adds to the active player's score:

- the current marble value: `i + 1`
- the value of the marble being removed: `current.Value`

This is done with:

    this.Players[i % this.PlayerCount] += i + 1 + current.Value;

So the active player is selected using:

    i % this.PlayerCount

---

### Removing the Scoring Marble

After scoring, the solver removes the marble at the current position.

It stores that node:

    LinkedListNode<int> previous = current;

Then advances clockwise once:

    current = Next(ref current, ref used);

And removes the stored node:

    used.Remove(previous);

So after a special turn:

- the removed marble is deleted from the circle
- the marble clockwise from it becomes the new current marble

---

### Circular Navigation Helpers

The marble ring is implemented with two helper methods:

### `Next(...)`

    private static LinkedListNode<int> Next(ref LinkedListNode<int> current, ref LinkedList<int> used)
        => current.Next ?? used.First ?? new(0);

This returns:

- `current.Next` when available
- otherwise wraps to `used.First`

So moving past the end of the linked list wraps back to the start.

---

### `Previous(...)`

    private static LinkedListNode<int> Previous(ref LinkedListNode<int> current, ref LinkedList<int> used)
        => current.Previous ?? used.Last ?? new(0);

This returns:

- `current.Previous` when available
- otherwise wraps to `used.Last`

So moving backward from the first node wraps to the end.

---

### Part 1 Return Value

At the end of the simulation, `Play()` returns:

    this.Players.Max()

So the silver answer is the highest score achieved by any player during the game.

---

### Part 2 Return Value

Part 2 uses the same return logic:

    this.Players.Max()

The only difference is that `Play(100)` multiplies the marble count before the simulation starts.

So the gold answer is the maximum score from the larger version of the same game.

---

## 🛠 Implementation Notes

- The input is parsed from a single descriptive line
- Scores are stored in a `long[]`
- The marble circle is implemented with `LinkedList<int>`
- `current` tracks the active marble position
- Standard turns insert a new marble after moving clockwise
- Special turns occur when the marble value is divisible by `23`
- Part 2 is implemented by scaling the marble count with a multiplier
- Circular movement is handled by explicit wraparound helper methods

---

## 🧪 Behaviour Summary

Given a player count and a last marble value:

- the solver parses the input line into game settings
- creates a circular linked list containing marble `0`
- inserts marbles one by one
- normal turns place a marble into the circle
- every `23rd` marble triggers a scoring turn
- the active player scores both the new marble and a removed marble
- the game continues until all marbles have been processed
- the final result is the highest score in the player array

---

## 🚀 Key Takeaways

- Good example of using `LinkedList<T>` for efficient circular insertion and removal
- Circular traversal is handled cleanly with helper methods
- The same core game logic solves both parts
- Part 2 is achieved by scaling the marble count rather than rewriting the algorithm
- Using `long` for scores avoids overflow in the extended simulation

---

## 🔗 References

- https://adventofcode.com/2018/day/9