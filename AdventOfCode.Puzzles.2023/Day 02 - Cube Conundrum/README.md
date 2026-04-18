# 🎄 Advent of Code 2023 - Day 2: Cube Conundrum

## 📜 Puzzle Overview

This puzzle processes a list of cube-drawing games.

Each input line describes one game and contains:

- a game ID
- a sequence of rounds
- cube counts for the colours red, green, and blue

A line looks like this:

    Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

The solver parses each line into a `Game` object containing:

- `Id`
- `Rounds`

Each round is stored as a `Round` object with:

- `Red`
- `Green`
- `Blue`

Part 1 checks which games are possible using fixed cube limits. Part 2 finds the minimum cube set required for each game and sums their powers.

---

## 🧩 Part 1

Determine which games are possible if the bag contains:

- 12 red
- 13 green
- 14 blue

### 💡 Approach

- Parse every input line into a game with multiple rounds
- Check every round in each game
- If any round exceeds the allowed red, green, or blue count, mark that game as impossible
- Collect the IDs of the remaining possible games
- Return the sum of those IDs

---

## 🧩 Part 2

Find the power of each game and return the total.

### 💡 Approach

- For each game, inspect all of its rounds
- Track the maximum number of red cubes seen in any round
- Track the maximum number of green cubes seen in any round
- Track the maximum number of blue cubes seen in any round
- Multiply those three maxima together
- Sum the resulting power values for all games

---

## 🧠 Code Breakdown

### `Day2.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Cube Conundrum`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new CubeConundrum(this.Input)`
- Calls `SumOfIds(12, 13, 14)`

For Part 2:

- Creates `new CubeConundrum(this.Input)`
- Calls `SumOfPower()`

---

### `CubeConundrum.cs`

This class coordinates the full puzzle logic.

The constructor:

- creates `this.Games = new();`
- loops over every input line
- creates `new Game(line)`
- stores each parsed game in `Games`

So the full input becomes:

- `List<Game> Games`

---

### `Game.cs`

This class parses one game line.

The constructor:

- initialises `Rounds`
- splits the line on `":"`
- extracts the game ID from the left side
- splits the right side on `";"` to separate rounds

Then for each round string it:

- splits on `","` to separate colour entries
- creates a new `Round`
- parses entries like `3 blue` or `4 red`
- assigns the values into the correct colour property
- adds that round to the game's round list

So a line such as:

    Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

becomes one `Game` object with an ID and a list of parsed `Round` objects.

---

### `Round.cs`

This class is a simple data container.

It stores:

- `Red`
- `Green`
- `Blue`

Each property is an `int` with get/set accessors.

If a colour is missing from a round, that property simply remains at its default value.

---

### Part 1 Validation Logic

`SumOfIds(int red, int green, int blue)` checks which games are possible.

It creates:

    List<int> possible = new();

Then for each game it:

- assumes the game is possible
- loops through all rounds
- compares `round.Red`, `round.Green`, and `round.Blue` against the allowed limits
- marks the game impossible if any round exceeds a limit
- skips impossible games
- adds the game ID for valid games

At the end it returns:

    possible.Sum()

So the silver answer is the sum of all game IDs that never exceed the supplied cube counts.

---

### Part 2 Power Calculation

`SumOfPower()` calculates the minimum required cube set for each game.

For each game it starts with:

    int red = 0;
    int green = 0;
    int blue = 0;

Then for every round it updates each colour maximum if that round contains a larger value.

After scanning all rounds in a game, it calculates:

    red * green * blue

That value is added to a list, and after all games are processed the method returns:

    possible.Sum()

So the gold answer is the sum of each game's power value.

---

## 🛠 Implementation Notes

- `Day2.cs` calls `SumOfIds(12, 13, 14)` for Part 1
- `Day2.cs` calls `SumOfPower()` for Part 2
- The input is parsed into `Game` objects, each with a `List<Round>`
- Each round stores red, green, and blue counts separately
- Part 1 rejects a game as soon as one round exceeds the allowed cube limit
- Part 2 finds the maximum red, green, and blue counts seen across all rounds
- The power of a game is `red * green * blue`

---

## 🧪 Behaviour Summary

Given a list of cube games:

- the solver parses each game into rounds
- each round records how many red, green, and blue cubes were shown
- Part 1 checks whether the game could be played with a fixed cube supply
- Part 2 computes the smallest required cube set by taking per-colour maxima
- the final answers are either the sum of valid game IDs or the sum of game powers

---

## 🚀 Key Takeaways

- Good example of parsing structured text into small domain objects
- Clean separation between input parsing and puzzle evaluation
- Part 1 uses straightforward limit checking
- Part 2 uses per-colour maxima across rounds
- The implementation stays simple by modelling rounds explicitly

---

## 🔗 References

- https://adventofcode.com/2023/day/2