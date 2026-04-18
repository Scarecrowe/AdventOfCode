# 🎄 Advent of Code 2017 - Day 19: A Series of Tubes

## 📜 Puzzle Overview

This puzzle follows a path through an ASCII tube diagram.

The input is parsed into a 2D map of characters containing:

- vertical pipes `|`
- horizontal pipes `-`
- junctions `+`
- letters
- spaces

The solver stores that map in a `VectorDictionary<int, char>` and finds the starting point by locating the first `|` encountered while parsing.

Part 1 walks the route and collects every letter seen along the way. Part 2 walks the same route but returns the total number of steps taken before the path ends.

---

## 🧩 Part 1

Determine the sequence of letters encountered while following the tube path.

### 💡 Approach

- Parse the input into a coordinate-based diagram
- Record the first `|` as the starting point
- Begin moving south from that position
- At each step:
  - read the current character
  - append letters to the result
  - turn at `+` by choosing the valid unvisited neighbour
- Stop when the next position in the current direction is a space
- Return the collected letters

---

## 🧩 Part 2

Determine how many steps are taken before the route ends.

### 💡 Approach

- Reuse the same path-following logic as Part 1
- Track a `steps` counter during traversal
- Return the step count instead of the collected letters when the path terminates

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `A Series of Tubes`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ASeriesOfTubes(this.Input)`
- Calls `Move()`

For Part 2:

- Creates `new ASeriesOfTubes(this.Input)`
- Calls `Move(true)`

---

### `ASeriesOfTubes.cs`

This class contains the diagram parsing and path traversal logic.

It stores:

- `Diagram`
- `Start`

The constructor:

- initialises `Start` to `(0, 0)`
- parses the input into the diagram by calling `ParseDiagram(input)`

---

### Parsing the Diagram

`ParseDiagram(string[] input)` builds a `VectorDictionary<int, char>` from the raw input.

During parsing it checks each character, and the first time it sees a `|` while `Start` is still `(0, 0)`, it records that coordinate as the starting point.

So the solver automatically discovers where the path begins from the input itself.

---

### Starting State

`Move(bool returnSteps = false)` begins with:

- `StringBuilder result = new();`
- `HashSet<Vector<int>> visited = new();`
- `Vector<int> point = new(this.Start);`
- `Cardinal direction = Cardinal.South;`
- `int steps = 0;`

That means traversal always starts at the parsed start position and initially moves downward.

---

### Traversal Loop

The solver then enters a `while (true)` loop.

On each iteration it:

- increments `steps`
- gets the cardinal neighbours of the current point
- reads the current character from the diagram
- marks the current point as visited

This gives the solver everything it needs to decide whether to collect a letter, turn, continue forward, or stop.

---

### Collecting Letters

If the current diagram value is a letter, the solver appends it to the result:

    if (char.IsLetter(value))
    {
        result.Append(value);
    }

This is the core of Part 1.

As the path passes over letters in the diagram, they are accumulated in order and returned as the final string.

---

### Turning at Junctions

When the current value is `+`, the solver changes direction.

It does that by looking at the adjacent cardinal positions and selecting the one that:

- is not the current point
- is not a space
- has not already been visited

That chosen neighbour's direction becomes the new travel direction.

So instead of hard-coding turn rules, the implementation discovers the valid onward route directly from the diagram and visited set.

---

### Detecting the End

After handling junctions and letters, the solver checks the next cell in the current direction.

If that next value is a space, traversal is finished and the method returns:

- the collected letters when `returnSteps == false`
- the step count when `returnSteps == true`

This is how the same method supports both puzzle parts.

---

### Moving Forward

If the path has not ended, the current point is updated with:

    point += CardinalHelper.CardinalTransform<int>()[direction];

So movement is always done by applying the vector offset for the current cardinal direction.

---

### Shared Logic for Both Parts

Both puzzle parts use exactly the same traversal method:

- Part 1 returns the accumulated letters
- Part 2 returns the total number of steps

That behaviour is controlled entirely by the `returnSteps` flag passed into `Move(...)`.

---

## 🛠 Implementation Notes

- The diagram is stored as a coordinate dictionary rather than a plain 2D array
- The starting point is discovered during parsing from the first `|` character
- Traversal always begins facing south
- A visited set prevents the junction logic from turning back onto the already travelled route
- The same traversal routine solves both parts
- Path termination is detected by checking whether the next tile in the current direction is a space

---

## 🧪 Behaviour Summary

Given an ASCII tube diagram:

- the solver parses it into a coordinate map
- finds the entry pipe automatically
- follows the route step by step
- turns at `+` intersections by selecting the valid unvisited continuation
- collects letters encountered on the path
- stops when the path would continue into a space
- returns either the collected letters or the total steps, depending on the requested mode

---

## 🚀 Key Takeaways

- Good example of path traversal over an ASCII map
- Uses a coordinate dictionary instead of manual row and column indexing
- Junction handling is driven by neighbour inspection rather than special-case turn rules
- Part 1 and Part 2 share the exact same traversal logic
- A simple mode flag switches the return value from letters to step count

---

## 🔗 References

- https://adventofcode.com/2017/day/19