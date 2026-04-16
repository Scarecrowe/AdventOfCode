# 🎄 Advent of Code 2015 - Day 14: Reindeer Olympics

## 📜 Puzzle Overview

Santa is hosting a reindeer race.

Each reindeer has three attributes:

- a flying speed in km/s
- a number of seconds it can fly before needing rest
- a number of seconds it must rest before flying again

Each reindeer repeats this cycle for the full duration of the race.

Part 1 asks which reindeer has travelled the **furthest distance** after the race ends.

Part 2 changes the scoring system:

- after each second, the reindeer currently in the lead earns one point
- if multiple reindeer are tied for the lead, each of them earns a point

The goal is then to find the highest final score.

---

## 🧩 Part 1

Determine the greatest distance travelled after the full race duration.

### 💡 Approach

- Parse each input line into a reindeer model
- Simulate the race second by second
- Update each reindeer's movement state on every tick
- Track total distance travelled
- Return the maximum distance at the end

---

## 🧩 Part 2

Determine the highest score after awarding points every second to the reindeer currently in the lead.

### 💡 Approach

- Reuse the same race simulation
- After each second, find the maximum distance reached so far
- Award one point to every reindeer tied for the lead
- Return the highest final score

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ReindeerOlympics(this.Input, 2503)`
- Calls `RaceByDistance()`

For Part 2:

- Creates `new ReindeerOlympics(this.Input, 2503)`
- Calls `RaceByScore()`

The race length is fixed at `2503` seconds for both parts.

---

### `Reindeer.cs`

This class represents a single reindeer.

It stores:

- `Name`
- `Distance` - speed travelled per flying second
- `Seconds` - how long it can fly before resting
- `Rest` - how long it must rest
- `TotalDistance` - distance travelled so far
- `TotalSeconds` - flying seconds used in the current cycle
- `TotalRest` - rest seconds used in the current cycle
- `Score` - points earned in Part 2

It also exposes:

- `Travel()` - advances the reindeer by one second
- `IncrementScore()` - adds one point

---

### Reindeer Movement

The `Travel()` method handles the fly/rest cycle.

The logic works like this:

- if the current rest period is complete, reset the flying and resting counters
- if the reindeer still has flying time left, increase flying time and add distance
- otherwise, if it still has rest time left, increase the rest counter

At a high level, each call to `Travel()` represents exactly one second of race time.

This makes the simulation easy to follow because every reindeer advances in lockstep with the race timer.

---

### `ReindeerOlympics.cs`

This class contains the race logic.

The constructor:

- parses the input into a dictionary of reindeer
- stores the total race duration

The internal collection maps:

- reindeer name
- to its corresponding `Reindeer` object

This allows the race to update and score all reindeer by iterating through that collection.

---

### Parsing Input

The `Parse()` method reads each input line and splits it into tokens.

Each line contains:

- the reindeer name
- the speed
- the flying duration
- the resting duration

Those values are used to create a new `Reindeer` instance.

This keeps all raw input handling in one place before the race begins.

---

### Distance Race

`RaceByDistance()` handles Part 1.

It:

- loops through every second of the race
- calls `Travel()` on every reindeer each second
- returns the maximum `TotalDistance` once the race is over

This produces the furthest distance travelled after the full race duration.

---

### Score Race

`RaceByScore()` handles Part 2.

It runs the race one second at a time using `Race()`.

Each second:

- every reindeer advances by one step using `Travel()`
- the current maximum travelled distance is calculated
- every reindeer tied at that distance receives one point

This means the score system is based on the live race position after every second, not just the final distance.

---

### Result Calculation

Two helper methods are used to produce the final answers:

- `MaxDistance()` returns the furthest travelled distance
- `MaxScore()` returns the highest score

This keeps the public race methods small and focused.

---

## 🛠 Implementation Notes

- The race is simulated second by second rather than using a formula
- Both parts reuse the same `Reindeer` movement logic
- Part 1 returns the maximum final distance
- Part 2 awards points dynamically during the race
- Ties are handled naturally by scoring every reindeer currently at the maximum distance
- Input parsing is kept separate from race execution

---

## 🧪 Examples

Given the example race:

- Comet can fly `14 km/s` for `10` seconds, then rest for `127` seconds
- Dancer can fly `16 km/s` for `11` seconds, then rest for `162` seconds

After `1000` seconds:

- Comet has travelled `1120 km`
- Dancer has travelled `1056 km`

For the point-based version of the same example:

- Dancer finishes with `689` points

---

## 🚀 Key Takeaways

- Good example of modelling stateful entities with simple per-tick updates
- The reindeer model cleanly encapsulates movement behaviour
- Part 2 is solved by building scoring on top of the same race simulation
- The implementation stays easy to reason about by treating every loop as one second of elapsed time

---

## 🔗 References

- https://adventofcode.com/2015/day/14