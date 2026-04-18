# 🎄 Advent of Code 2025 - Day 08: Playground

## 📜 Puzzle Overview

This puzzle works with a list of 3D junction coordinates.

Each input line is parsed as a point with:

- `X`
- `Y`
- `Z`

From those points, the solver builds every possible connection between pairs of junctions and sorts those connections by squared Euclidean distance.

Part 1 uses those sorted connections to build connected groups and returns the product of the sizes of the three largest groups once a specific connection threshold is reached. Part 2 keeps joining components until the whole structure becomes one connected set, then returns the product of the `X` coordinates of the final edge that completed the merge.

---

## 🧩 Part 1

Determine the product of the three largest connected group sizes after processing the sorted connections up to the solver's stopping point. 

### 💡 Approach

- Parse every line into a 3D junction
- Generate every possible pair of junctions
- Sort the pairs by squared distance
- Join junctions in that order using a disjoint-set structure
- When the configured stopping condition is reached, count the size of each remaining connected group
- Take the three largest sizes and multiply them together, padding with `1` values if fewer than three groups exist.

---

## 🧩 Part 2

Determine the product of the `X` coordinates from the final edge that makes the entire playground become one connected set.

### 💡 Approach

- Reuse the same sorted edge list
- Join junctions in ascending distance order
- Track connected components with a disjoint-set union structure
- As soon as only one set remains, return the product of the `X` coordinates from the two junctions used by that successful merge.

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Playground`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new Playground(this.Input)`
- Calls `MultiplyTopThreeCircuit()`

For Part 2:

- Creates `new Playground(this.Input)`
- Calls `MultiplyXCoordinates()` 

---

### `Playground.cs`

This class contains the full parsing, edge generation, sorting, and union logic.

It stores:

- `Junctions`
- `Edges`

The constructor:

- parses the input into junction vectors
- builds the full edge list by calling `GetEdges()`

---

### Parsing the Junctions

The input is parsed with:

    input.Select(x => new Vector(x.Split(',').Select(int.Parse).ToArray())).ToList()

That means each line is expected to be a comma-separated coordinate triple, and each one becomes a `Vector`. 

At a high level, a line behaves like this:

- `1,2,3` becomes a junction with `X = 1`, `Y = 2`, `Z = 3` 

---

### Building the Edge List

`GetEdges()` generates every unique pair of junction indices.

It loops with:

- `i` from the first junction to the last
- `j` from `i + 1` onward

This means the edge list contains each pair only once and never includes self-links. 

---

### Sorting by Distance

After building the pair list, `GetEdges()` sorts it by squared Euclidean distance.

For each edge it calculates:

- `dx = pointA.X - pointB.X`
- `dy = pointA.Y - pointB.Y`
- `dz = pointA.Z - pointB.Z`

Then it compares:

    dx * dx + dy * dy + dz * dz

Using squared distance avoids needing a square root while still preserving the correct ordering of edge lengths.

---

### Part 1 Logic

`MultiplyTopThreeCircuit()` creates:

- a dictionary called `sets`
- a `DisjointSetUnion`
- a counter called `connections`

It then walks through the sorted edge list.

For each edge:

- if the stopping condition has already been reached, count the size of every disjoint-set root
- otherwise union the two junction indices and increment `connections`

When the stopping condition triggers, the method:

- counts how many junctions belong to each root
- orders the component sizes descending
- takes the largest three
- pads with `1` if needed
- multiplies them together and returns the result.

---

### Counting Component Sizes

When Part 1 finishes, each junction index is mapped through:

    set.Find(i)

That root value is used as the key in the `sets` dictionary, and the corresponding count is incremented.

This produces the final connected-component sizes used in the result calculation.

---

### Part 2 Logic

`MultiplyXCoordinates()` also creates a `DisjointSetUnion`, then iterates through the same sorted edge list.

For each edge:

- attempt to union the two junctions
- if the union succeeds and the disjoint set reports `Sets == 1`, the structure is now fully connected
- return the product of the `X` coordinates for the two junctions used by that final merge.

The returned value is:

    Junctions[indexA].X * Junctions[indexB].X

---

## 🛠 Implementation Notes

- Input is parsed into `Vector` instances from comma-separated coordinates
- Every unique pair of junctions is converted into an edge
- Edges are processed in ascending squared-distance order
- Both puzzle parts use a disjoint-set union structure
- Part 1 derives connected-component sizes from the DSU roots
- Part 2 stops as soon as the graph becomes a single connected component.

---

## 🧪 Behaviour Summary

Given a list of junction points:

- the solver first builds all pairwise edges
- shorter edges are always considered before longer ones
- unions gradually merge separate components together
- Part 1 measures component sizes at its stopping point
- Part 2 waits until the final merge that connects the entire structure, then uses the two endpoint `X` values from that edge.

---

## 🚀 Key Takeaways

- Good example of building a complete graph from coordinate input
- Squared-distance sorting keeps edge ordering simple and efficient
- A disjoint-set union structure drives both puzzle parts
- Part 1 focuses on connected-group sizes
- Part 2 focuses on the exact edge that completes full connectivity.

---

## 🔗 References

- https://adventofcode.com/2025/day/8