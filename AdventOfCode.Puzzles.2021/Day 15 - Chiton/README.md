# 🎄 Advent of Code 2021 - Day 15: Chiton

## 📜 Puzzle Overview

This puzzle works with a grid of risk levels.

Each input line is a row of single-digit numbers, where each number is the risk of entering that position.

Example input:

    1163751742
    1381373672
    2136511328
    3694931569
    7463417111

Part 1 finds the lowest total risk path from the top-left corner to the bottom-right corner of the map.

Part 2 first enlarges the map into a 5x5 tiled version with wrapped risk values, then finds the lowest total risk path on that expanded grid.

The implementation models each grid cell as a `Chiton` node and uses a priority queue to navigate the map.

---

## 🧩 Part 1

Find the minimum total risk path across the original map.

### 💡 Approach

- Parse the input into a 2D map of `Chiton` objects
- Start from the top-left corner
- Use a priority queue to always process the currently lowest-risk position first
- Explore only cardinal neighbours
- Track the best known total risk for each cell
- Stop once the bottom-right corner is reached
- Return the final total risk, excluding the starting cell

---

## 🧩 Part 2

Find the minimum total risk path across the enlarged map.

### 💡 Approach

- Reuse the same parsed map
- Expand it into a 5x5 tiled version
- Increase the risk in each tile based on its tile offset
- Wrap risk values back into the `1` to `9` range
- Run the same navigation algorithm on the enlarged map
- Return the final minimum total risk

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Chiton`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new ChitonNavigator(this.Input)`
- calls `Navigate()`

For Part 2:

- creates `new ChitonNavigator(this.Input)`
- calls `Enlarge()`
- then calls `Navigate()`

So the gold solution reuses the same pathfinding logic after replacing the original map with a larger one.

---

### `Chiton.cs`

This class models a single map node.

It stores:

- `Point`
- `Risk`
- `Visited`
- `TotalRisk`

The constructor sets:

- the node position
- the node's base risk
- `TotalRisk = int.MaxValue`

So every cell starts as unvisited and with an effectively unknown best path cost.

---

### `ChitonNavigator.cs`

This class contains the full parsing, expansion, and pathfinding logic.

It stores:

- `Map`

The constructor parses the input immediately with:

    this.Map = Parse(input);

So both puzzle parts begin with the same original risk map.

---

### Parsing the Map

`Parse(string[] input)` builds a `VectorArray<int, Chiton>` sized from:

- `input[0].Length`
- `input.Length`

It then loops through the full grid and creates one `Chiton` per coordinate.

At a high level it does:

- create the result grid
- iterate over every coordinate
- convert the digit at that position to an integer
- create a `Chiton` for that point

So the textual puzzle input becomes a full node map ready for traversal.

---

### Enlarging the Map

`Enlarge()` creates the expanded Part 2 grid.

It starts with:

    int width = (int)Math.Sqrt(this.Map.Width * this.Map.Height);

Then creates:

    new((int)this.Map.Width * 5, (int)this.Map.Height * 5);

After that it loops through tile offsets `i` and `j` from `0` to `4`, and for every original cell it writes a translated copy into the enlarged map.

The new risk is calculated with:

    ((cell.Value.Risk + i + j - 1) % 9) + 1

So risk values increase as tiles move right or down, and wrap back around after `9`.

---

### Pathfinding Strategy

`Navigate()` uses a priority queue:

    PriorityQueue<Chiton, int> queue = new();

It begins by enqueueing the start node with priority `0`.

Then it repeatedly:

- dequeues the next best candidate
- skips it if it was already visited
- marks it as visited
- checks whether it is the target cell
- explores its cardinal neighbours

This is a Dijkstra-style shortest path search over the grid.

---

### Visiting the Target

As soon as the current node reaches:

    new Vector<int>(this.Map.Width - 1, this.Map.Height - 1)

the method returns:

    this.Map[chiton.Point.Y, chiton.Point.X].TotalRisk - this.Map[0, 0].TotalRisk;

So the implementation explicitly subtracts the starting node's risk from the final total, matching the puzzle rule that the starting position does not count.

---

### Risk Relaxation

For each cardinal neighbour, the solver calculates:

    int risk = chiton.TotalRisk + this.Map[adjacent.Point].Risk;

If that new total is lower than the neighbour's current `TotalRisk`, it updates the neighbour.

Logically:

    if (risk < adjacent.Value.TotalRisk)
        adjacent.Value.TotalRisk = risk

If the neighbour now has a valid total risk, it is pushed into the priority queue.

This is the core shortest-path relaxation step.

---

### Cardinal Movement Only

Neighbour traversal is done with:

    this.Map.AdjacentCardinal(chiton.Point)

So movement is limited to:

- up
- down
- left
- right

Diagonal travel is not considered in this implementation.

---

### Fallback Return

If the queue empties without early return, the method still returns:

    this.Map[this.Map.Height - 1, this.Map.Width - 1].TotalRisk - this.Map[0, 0].TotalRisk;

So even without hitting the explicit target branch, the solver falls back to the bottom-right node's stored total risk.

---

## 🛠 Implementation Notes

- `Day15.cs` uses `Navigate()` for silver and `Enlarge().Navigate()` for gold
- Each map cell is represented by a `Chiton` object
- `TotalRisk` starts at `int.MaxValue`
- The solver uses a priority queue for efficient lowest-risk processing
- Only cardinal neighbours are explored
- Part 2 enlarges the map to 5 times the original width and height
- Enlarged risk values wrap with `((risk + i + j - 1) % 9) + 1`
- The returned total subtracts the starting cell risk so the origin does not contribute to the answer

---

## 🧪 Behaviour Summary

Given a grid of risk digits:

- the solver parses the input into a grid of node objects
- each node stores its own position, risk, visited state, and best known path cost
- Part 1 runs a shortest-path search on the original map
- Part 2 first builds a 5x5 enlarged map with wrapped risk values
- both parts use the same navigation algorithm
- the final result is the minimum total risk from the top-left to the bottom-right corner

---

## 🚀 Key Takeaways

- Good example of grid pathfinding with weighted movement costs
- The implementation models each grid cell as a dedicated object
- Dijkstra-style traversal is handled with a priority queue
- Part 2 reuses the same navigation logic after transforming the map
- The start-cell risk is explicitly removed from the final answer

---

## 🔗 References

- https://adventofcode.com/2021/day/15