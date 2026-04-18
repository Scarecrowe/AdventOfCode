# 🎄 Advent of Code 2023 - Day 17: Clumsy Crucible

## 📜 Puzzle Overview

This puzzle is about finding the path through a heat-loss grid while obeying movement constraints.

Each input line is a row of digits, where each digit represents the heat cost of entering that cell.

The goal is to move from the top-left corner to the bottom-right corner while minimising total heat loss.

The twist is that the crucible cannot move however it wants:

- it cannot reverse direction
- it can only move straight for a limited number of steps
- in Part 2 it must also move a minimum number of steps before turning

The solver models this as a state-based pathfinding problem.

---

## 🧩 Part 1

Find the minimum heat loss for the normal crucible.

### 💡 Approach

- Parse the input grid into a 2D map of integers
- Start from the origin with two possible initial directions:
  - east
  - south
- Use a priority queue to explore the cheapest valid states first
- Track:
  - current position
  - current direction
  - straight-line distance travelled in that direction
- Reject moves that:
  - reverse direction
  - continue straight beyond 3 steps
- Return the cost when the destination is reached

---

## 🧩 Part 2

Find the minimum heat loss for the ultra crucible.

### 💡 Approach

Reuse the same search logic, but change the movement rules:

- you cannot turn until you have moved at least 4 steps straight
- you cannot continue straight beyond 10 steps
- you still cannot reverse direction

This means the search explores the same grid, but under stricter movement rules.

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Clumsy Crucible`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ClumsyCrucible(this.Input)`
- Calls `CrucibleHeatLoss()`

For Part 2:

- Creates `new ClumsyCrucible(this.Input)`
- Calls `UltraCrucibleHeatLoss()`

---

### `ClumsyCrucible.cs`

This class contains the core pathfinding logic.

It stores:

- `Map`

The constructor builds the map from the input using:

    new(input, (c) => int.Parse($"{c}"))

So each character digit becomes an integer heat-loss value in a 2D grid.

---

### Grid Representation

The map is stored as:

- `VectorArray<int, int>`

This allows the solver to:

- access cell values by coordinate
- inspect cardinal neighbours
- determine the map width and height

The target position is created as:

    new(this.Map.Width - 1, this.Map.Height - 1)

So the finish point is always the bottom-right cell.

---

### Part 1 Movement Rules

`CrucibleHeatLoss()` calls:

    this.Move((state, cell) =>
    {
        return cell.Direction == state.Direction && state.Distance == 3;
    });

This means a move is rejected when:

- the next move continues in the same direction
- and the current straight-line distance is already `3`

So the normal crucible can move at most 3 tiles straight before it must turn.

---

### Part 2 Movement Rules

`UltraCrucibleHeatLoss()` calls:

    this.Move((state, cell) =>
    {
        return (cell.Direction != state.Direction && state.Distance < 4)
            || (cell.Direction == state.Direction && state.Distance == 10);
    });

This rejects moves when:

- you try to turn before travelling 4 steps straight
- or you try to continue straight after already travelling 10 steps

So the ultra crucible must go straight for at least 4 tiles before turning, and can go straight for at most 10.

---

### `State.cs`

This class models a single search state.

It stores:

- `Point`
- `Direction`
- `Distance`

Where:

- `Point` is the current grid position
- `Direction` is the direction of travel used to reach that position
- `Distance` is how many consecutive steps have been taken in that direction

This is important because the same grid coordinate can behave differently depending on direction and streak length.

---

### Advancing State

`Next(VectorCell<int, int> cell)` creates the next search state.

It returns:

- a new position
- the new direction
- an updated distance count

The distance logic is:

- if continuing in the same direction → `distance + 1`
- otherwise → `1`

So turning resets the straight-line counter.

---

### Main Search Logic

`Move(Func<State, VectorCell<int, int>, bool> comparer)` performs the actual pathfinding.

It creates:

- `PriorityQueue<State, int> queue = new();`
- a finish coordinate
- `Dictionary<State, int> states = new()`

It seeds the search with two starting states:

- `(0,0)` facing east with distance `0`
- `(0,0)` facing south with distance `0`

Each starts with accumulated cost `0`.

---

### Priority Queue Search

The solver repeatedly processes states from the priority queue.

At a high level it does:

- inspect the next queued state
- if that state's point is the finish, return its priority
- otherwise dequeue it
- generate valid cardinal neighbours
- evaluate the cost to reach each next state
- store and enqueue improved states

This is effectively a best-first path search over movement-constrained states.

---

### Preventing Reverse Movement

Before exploring neighbours, the solver computes:

    Cardinal turn = CardinalHelper.Flip(state.Direction);

It then filters adjacent cells with:

- `x.Direction != turn`

So the crucible is never allowed to move directly back the way it came.

---

### Generating Valid Moves

Neighbour exploration uses:

    this.Map
        .AdjacentCardinal(state.Point)
        .Where(x => x.Direction != turn && !comparer(state, x))
        .Select(x => new VectorCell<int, int>(state.Point.Clone().Transform(x.Direction), x.Direction))

This means each candidate move must:

- be cardinally adjacent
- not reverse direction
- not violate the rule set supplied by the comparer

Only then is it transformed into a next position and direction for search.

---

### Cost Tracking

For each valid next move, the solver computes:

    int value = states[state] + this.Map[cell.Point];

So the heat loss increases by the value of the destination cell being entered.

It then compares that cost against any previously known cost for the same state:

    int current = states.ContainsKey(state.Next(cell)) ? states[state.Next(cell)] : int.MaxValue;

If the new value is better, it replaces the stored one and queues the new state.

---

### Queue Priority

Improved states are enqueued with:

    value + cell.Point.Distance(finish)

So the solver uses:

- actual heat loss so far
- plus distance to the finish

This gives the priority queue a heuristic to help favour promising paths.

---

### Return Value

As soon as the next queued state is at the finish position, the solver returns its priority.

So both parts ultimately return:

- the minimum heat loss found under that part's movement constraints

If no path is found, the method throws:

    new InvalidOperationException()

---

## 🛠 Implementation Notes

- The map is parsed directly from digit characters
- Search states include position, direction, and straight-line distance
- The solver starts by trying both east and south
- Reverse movement is explicitly disallowed
- Part 1 limits straight runs to 3
- Part 2 requires at least 4 steps before turning and allows up to 10
- State cost is stored in a dictionary keyed by `State`
- A priority queue is used to explore promising paths first

---

## 🧪 Behaviour Summary

Given a grid of heat-loss values:

- the solver parses the grid into a 2D integer map
- it searches from the top-left to the bottom-right
- each search state remembers direction and straight-line streak length
- invalid moves are filtered out before expansion
- heat loss is accumulated from entered cells
- Part 1 and Part 2 reuse the same search engine with different rule checks
- the final answer is the minimum valid heat loss for the chosen crucible mode

---

## 🚀 Key Takeaways

- Good example of pathfinding where position alone is not enough to define a state
- Direction and movement streak are essential parts of the search key
- Both puzzle parts share one generic movement engine
- A comparer callback cleanly switches between silver and gold rule sets
- Priority-queue search keeps the solution efficient even with constrained movement rules

---

## 🔗 References

- https://adventofcode.com/2023/day/17