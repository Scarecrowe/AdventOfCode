# 🎄 Advent of Code 2015 - Day 09: All in a Single Night

## 📜 Puzzle Overview

Santa has been given the distances between pairs of locations.

The goal is to work out the total distance of a route that:

- Visits each location exactly once
- Can start at any location
- Can end at any location

Each input line is in the format:

- `London to Dublin = 464`

This creates a set of bidirectional routes between locations.

Part 1 asks for the **shortest** possible route.

Part 2 asks for the **longest** possible route.

---

## 🧩 Part 1

Determine the total distance of the **shortest route** that visits every location exactly once.

### 💡 Approach

- Parse each input line into a route between two locations
- Store distances in a structure that can be looked up from either direction
- Generate every possible ordering of the locations
- Calculate the total distance for each full route
- Return the smallest total

---

## 🧩 Part 2

Determine the total distance of the **longest route** that visits every location exactly once.

### 💡 Approach

- Reuse the same route generation logic
- Calculate the total distance for every valid permutation
- Return the largest total

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new AllInASingleNight(this.Input)`
- Calls `Shortest()`

For Part 2:

- Creates `new AllInASingleNight(this.Input)`
- Calls `Longest()`

Both results are returned as strings.

---

### `AllInASingleNight.cs`

This class contains the main route-building and route-sorting logic.

The constructor:

- Creates a location map
- Parses every input line
- Adds the distance in both directions

That means a line like:

- `London to Dublin = 464`

is stored as:

- `London -> Dublin = 464`
- `Dublin -> London = 464`

This makes later distance lookups simple regardless of travel direction.

---

### Parsing Input

Each line is parsed by splitting on spaces.

For example:

- `London to Dublin = 464`

becomes tokens that provide:

- Source location
- Destination location
- Distance

A parsed journey is then added into the location map.

---

### Location Storage

Routes are stored in a nested lookup structure.

At a high level this works like:

- Outer key = location name
- Inner key = connected location
- Value = distance

This allows distances to be accessed directly using two location names.

---

### Route Generation

Once all locations are loaded, the solver builds a list of all location names and generates every permutation.

Each permutation represents one possible complete route.

For example, if the locations are:

- London
- Dublin
- Belfast

then permutations include routes such as:

- `London -> Dublin -> Belfast`
- `Dublin -> Belfast -> London`

Every possible visit order is considered.

---

### Distance Calculation

For each generated route:

- Start with distance `0`
- Walk through the route one step at a time
- Look up the distance between the current location and the next one
- Add that distance to the running total

The completed route and its total distance are then stored together.

---

### Finding the Result

The solver exposes two methods:

- `Shortest()` returns the minimum route distance
- `Longest()` returns the maximum route distance

Both methods rely on the same internal route-generation logic and simply select a different final value.

---

## 🛠 Implementation Notes

- Input is parsed once during construction
- Routes are treated as bidirectional
- All possible visit orders are generated using permutations
- Every route distance is calculated explicitly
- Part 1 and Part 2 reuse the same underlying logic
- This approach is simple and works well for the puzzle input size

---

## 🧪 Examples

Given the following input:

    London to Dublin = 464
    London to Belfast = 518
    Dublin to Belfast = 141

The possible routes are:

    Dublin -> London -> Belfast = 982
    London -> Dublin -> Belfast = 605
    London -> Belfast -> Dublin = 659
    Dublin -> Belfast -> London = 659
    Belfast -> Dublin -> London = 605
    Belfast -> London -> Dublin = 982

From these routes:

- Shortest distance = `605`
- Longest distance = `982`

---

## 🚀 Key Takeaways

- Good example of turning text input into a graph-style lookup
- Solves both parts by reusing the same route generation
- Permutation-based search keeps the logic straightforward
- Clean separation between puzzle setup and route evaluation

---

## 🔗 References

- https://adventofcode.com/2015/day/9