# 🎄 Advent of Code 2024 - Day 10: Hoof It

## 📜 Puzzle Overview

This puzzle works with a height map made of digits.

The map contains:

- trailhead positions with height `0`
- ascending path tiles `1` through `9`
- impassable cells represented as `.` in the input

The solver loads the input into a numeric grid where:

- digits become their integer height
- `.` becomes `-1`

From every trailhead, it explores cardinal neighbours and only follows steps that increase by exactly `1`.

Part 1 counts how many unique height-`9` endpoints can be reached from all trailheads.

Part 2 counts the total number of valid hiking trails that reach height `9`.

---

## 🧩 Part 1

Determine the total trailhead score.

### 💡 Approach

- Parse the input into a 2D numeric map
- Find every cell with value `0`
- For each trailhead:
  - run a search through cardinal neighbours
  - only move to a neighbour whose value is exactly the current height plus `1`
  - track which height-`9` endpoints are reachable
- Count unique reachable `9` positions per trailhead
- Return the sum of those unique endpoint counts

---

## 🧩 Part 2

Determine the total trailhead rating.

### 💡 Approach

- Reuse the same grid traversal as Part 1
- Start from every trailhead with value `0`
- Follow only strictly ascending steps where each next tile is exactly `+1`
- Every time a path reaches a `9`, count it
- Do not collapse duplicate routes in rating mode
- Return the total number of successful trails

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Hoof It`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new HoofIt(this.Input)`
- Calls `TrailHeads(false)`

For Part 2:

- Creates `new HoofIt(this.Input)`
- Calls `TrailHeads(true)`

---

### `HoofIt.cs`

This class contains the full map parsing and trail traversal logic.

It stores:

- `Map`

The constructor creates the grid with:

    new(input, x => x == '.' ? -1 : int.Parse($"{x}"))

So the input is converted into a `VectorArray<int, int>` where:

- `.` becomes `-1`
- numeric characters become integer heights

---

### Map Representation

The map is stored as:

- `VectorArray<int, int>`

This allows the solver to:

- iterate through every cell
- inspect adjacent cardinal neighbours
- compare neighbour heights directly

Only cells with value `0` are treated as trailheads.

---

### Main Search Method

`TrailHeads(bool rating)` performs the full traversal.

It creates:

- `Queue<(Vector<int> Point, int Distance, List<Vector<int>> Visited)> queue = new();`
- `Dictionary<Vector<int>, HashSet<Vector<int>>> trails = new();`
- `long result = 0;`

Then it scans every cell in the map.

Whenever it finds a trailhead:

- it creates a new endpoint set for that starting position
- clears the queue
- enqueues the starting state with:
  - current point
  - distance `0`
  - an empty visited list

So each trailhead is processed independently.

---

### Movement Rules

For each dequeued state, the solver checks:

- all cardinally adjacent cells

It only follows a neighbour when:

    adjacent.Value == state.Distance + 1

This means the path must rise exactly one height level at each step.

So valid paths look like:

    0 -> 1 -> 2 -> 3 -> ... -> 9

Any neighbour that does not match the next expected height is ignored.

---

### Part 1 Path Handling

When `rating` is `false`, the solver avoids revisiting points already seen in the current path.

It does this with:

    if (!rating && state.Visited.Contains(adjacent.Point))
    {
        continue;
    }

So in Part 1 mode:

- the current path keeps a visited list
- already-visited points are skipped
- reachable `9` endpoints are deduplicated per trailhead

When a `9` is reached:

- it is added to that trailhead's endpoint set
- duplicate endpoint hits for the same trailhead are ignored

This is what makes Part 1 return a score based on unique summit endpoints rather than total route count.

---

### Part 2 Path Handling

When `rating` is `true`, the visited-point skip is disabled.

That means:

- the solver still follows only exact `+1` climbs
- every successful path to a `9` contributes to the total
- duplicate routes to the same `9` are all counted

Whenever the traversal reaches height `9`, it increments:

    result++

So Part 2 measures the total number of successful trails rather than unique destinations.

---

### Reaching a Summit

Inside the traversal loop, when the next valid tile has value `9`, the solver:

- increments `result`
- records that summit in the trailhead's endpoint set if not already present

The key logic is:

    if (adjacent.Value == 9)
    {
        result++;
        if (trails[cell.Point].Contains(adjacent.Point))
        {
            continue;
        }

        trails[cell.Point].Add(adjacent.Point);
    }

So the implementation tracks both:

- total successful route count
- unique `9` destinations per trailhead

Then it chooses which total to return based on the mode.

---

### Continuing a Trail

If a valid adjacent tile is not yet `9`, the solver continues the search by:

- adding that point to the current visited list
- enqueueing a new state with:
  - the adjacent point
  - the adjacent value as the new distance
  - the updated visited list

This means the search expands outward breadth-first across all valid ascending paths.

---

### Part 1 Return Value

When called as:

    TrailHeads(false)

the method returns:

    trails.Sum(pair => pair.Value.Count())

So the silver answer is:

- the sum of unique reachable height-`9` endpoints across all trailheads

---

### Part 2 Return Value

When called as:

    TrailHeads(true)

the method returns:

    result

So the gold answer is:

- the total number of valid ascending trails that reach height `9`

---

## 🛠 Implementation Notes

- The map uses `-1` for impassable `.` cells
- Only cells with value `0` are used as trailheads
- Movement is cardinal only
- A step is valid only when the neighbour is exactly `current + 1`
- The traversal uses a queue-based search
- Part 1 deduplicates summit endpoints per trailhead
- Part 2 counts every successful route to a summit
- The same search method supports both parts via the `rating` flag

---

## 🧪 Behaviour Summary

Given a numeric hiking map:

- the solver parses the map into integer heights
- finds every trailhead with value `0`
- explores all cardinal paths that climb exactly one step at a time
- Part 1 counts unique reachable `9` endpoints for each trailhead
- Part 2 counts every valid trail that reaches a `9`
- the final result is either the total score or the total rating depending on the mode

---

## 🚀 Key Takeaways

- Good example of grid traversal with strict step constraints
- The same search logic is reused for both puzzle parts
- Part 1 focuses on unique destinations
- Part 2 focuses on total route count
- The `rating` flag cleanly switches between deduplicated endpoint scoring and full trail counting

---

## 🔗 References

- https://adventofcode.com/2024/day/10