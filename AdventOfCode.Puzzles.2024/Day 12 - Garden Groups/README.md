# 🎄 Advent of Code 2024 - Day 12: Garden Groups

## 📜 Puzzle Overview

This puzzle works with a garden map made of plot letters.

Each cell in the map contains a plant type, and connected cells with the same character form a region.

The solver loads the input into a 2D character map and then:

- finds every connected region of matching characters
- calculates a price for each region
- sums all region prices

Both parts use the same region-building logic.

The difference is how the edge cost is calculated:

- Part 1 uses a perimeter-style cost
- Part 2 uses a bulk fence-style cost

---

## 🧩 Part 1

Determine the total fence cost using area multiplied by perimeter.

### 💡 Approach

- Parse the input into a 2D character grid
- Traverse the map and group connected matching cells into regions
- For each region:
  - calculate its area as the number of cells
  - calculate its perimeter contribution from neighbouring cells
- Multiply area by perimeter for that region
- Sum the results for all regions

---

## 🧩 Part 2

Determine the total fence cost using the bulk pricing logic.

### 💡 Approach

- Reuse the same region detection as Part 1
- For each region:
  - calculate its area
  - calculate its bulk edge cost using the alternative neighbour count formula
- Multiply area by that bulk edge value
- Sum the results for all regions

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Garden Groups`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new GardenGroups(this.Input)`
- Calls `Cost()`

For Part 2:

- Creates `new GardenGroups(this.Input)`
- Calls `Bulk()`

The file also contains an additional `D12P02` implementation, but the public gold answer does not use it.

---

### `GardenGroups.cs`

This class contains the active puzzle logic.

It stores:

- `Map`

The constructor creates the map with:

    new(input, c => c)

So the puzzle input is loaded directly into a `VectorArray<int, char>` grid.

---

### Map Representation

The garden is stored as:

- `VectorArray<int, char>`

This allows the solver to:

- iterate through every map cell
- check cardinal neighbours
- compare plant types directly

Each unique connected group of the same character becomes one region.

---

### Region Discovery

Both `Cost()` and `Bulk()` begin by building a list of regions.

They create:

- `HashSet<Vector<int>> visited = new();`
- `List<HashSet<Vector<int>>> regions = new();`

Then they scan every cell in the map.

If a cell has not yet been visited:

- create a new temporary region set
- start a queue-based flood fill from that cell
- follow only cardinal neighbours with the same character value
- mark all matching connected cells as visited
- add the completed region to the region list

So the solver first breaks the garden into connected same-letter regions before doing any pricing.

---

### Flood Fill Logic

The region-building loop uses:

- `Queue<Vector<int>>`

For each dequeued point it checks:

- `this.Map.AdjacentCardinal(point)`

A neighbour is added to the current region only when:

- it has not already been visited
- its value matches the original region character

This means regions are connected only by:

- up
- down
- left
- right

Diagonal touching does not merge regions.

---

### Part 1 Pricing Logic

`Cost()` calculates the result using a perimeter-style formula.

After collecting all regions, it loops through each one and calculates:

- region area as `region.Count`
- perimeter contribution by inspecting each cell's same-value cardinal neighbours

For each point it does:

    adjacent = this.Map.AdjacentCardinal(point)
        .Where(x => x.Value == value && !visited.Contains(x.Point));

Then it adds:

    4 - (adjacent.Count() * 2)

to the region perimeter total.

Once all cells in the region have been processed, it adds:

    region.Count * perimeter

to the final result.

So the silver answer is based on:

- area multiplied by the computed perimeter value

---

### Why the Part 1 Perimeter Uses `* 2`

In `Cost()`, the method clears a local `visited` set before processing each region.

That region-local visited set is then used so that shared edges are not double-counted during the perimeter calculation.

The formula:

    4 - (adjacent.Count() * 2)

works because each same-region neighbour relationship removes exposed edge contribution from the current total.

---

### Part 2 Pricing Logic

`Bulk()` uses the same region-building code as Part 1.

The difference is in the second pass over each region.

For each point it calculates:

    adjacent = this.Map.AdjacentCardinal(point)
        .Where(x => x.Value == value);

Then it adds:

    4 - adjacent.Count()

to the region total.

After processing all cells in the region, it adds:

    region.Count * perimeter

to the final answer.

So the gold answer uses:

- the same area
- a different edge-counting formula

---

### Debug Output in `Bulk()`

Inside `Bulk()`, the code also writes:

    PuzzleConsole.WriteLine($"{value} -> {perimeter}");

This appears to be debug or diagnostic output showing the plant type and the computed region edge total while processing.

It does not affect the returned answer.

---

### Part 1 Return Value

When called as:

    Cost()

the method returns:

- the sum of `region area * perimeter` across all discovered regions using the Part 1 perimeter calculation

This is the silver answer.

---

### Part 2 Return Value

When called as:

    Bulk()

the method returns:

- the sum of `region area * perimeter` across all discovered regions using the bulk calculation

This is the gold answer used by `Day12.cs`.

---

### Additional `D12P02` Class

`Day12.cs` also contains a separate helper implementation named:

- `D12P02`

It includes:

- its own grid storage
- its own flood fill
- perimeter tracking using directional perimeter positions
- side-following logic with `FollowPerimeter(...)` and `CountSides(...)`

However, this class is only instantiated in the constructor and its result is not returned by `Gold()`.

So the actual puzzle answers exposed by the Day 12 class come from:

- `GardenGroups.Cost()`
- `GardenGroups.Bulk()`

---

## 🛠 Implementation Notes

- The active solution logic lives in `GardenGroups.cs`
- The map is stored as a `VectorArray<int, char>`
- Regions are built with a queue-based flood fill
- Connectivity is cardinal only
- Both parts share the same region detection pass
- Part 1 uses `4 - (adjacent.Count() * 2)` during perimeter calculation
- Part 2 uses `4 - adjacent.Count()`
- The final price in both parts is `region.Count * perimeter`
- `Day12.cs` includes an extra unused `D12P02` implementation

---

## 🧪 Behaviour Summary

Given a garden map of plot letters:

- the solver groups connected matching cells into regions
- each region records all cells of the same plant type
- Part 1 calculates a perimeter-style fence cost for each region
- Part 2 applies an alternate bulk edge formula
- both parts multiply region area by the calculated edge total
- the final answer is the sum of all region prices

---

## 🚀 Key Takeaways

- Good example of separating region discovery from region pricing
- Both puzzle parts reuse the same flood fill logic
- The only real difference between silver and gold is the edge-count formula
- Regions are stored cleanly as sets of coordinates
- The implementation also contains an alternative experimental Part 2-style approach, but the active solution uses `Bulk()`

---

## 🔗 References

- https://adventofcode.com/2024/day/12