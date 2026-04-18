# 🎄 Advent of Code 2017 - Day 14: Disk Defragmentation

## 📜 Puzzle Overview

This puzzle turns a single input key into a 128x128 disk grid.

For each row from `0` to `127`, the solver appends `-rowNumber` to the input, computes a Knot Hash, and converts the hexadecimal hash into a 128-character binary string. Each binary digit represents whether a square is used:

- `1` = used
- `0` = free

Part 1 counts how many used squares appear across the whole grid. Part 2 counts how many connected regions of used squares exist, using only up, down, left, and right adjacency.

---

## 🧩 Part 1

Determine how many squares are marked as used in the full 128x128 grid.

### 💡 Approach

- Generate 128 Knot Hash values using `input-0` through `input-127`
- Convert each hex character in each hash into 4 binary digits
- Join all rows together into one long binary sequence
- Count how many characters are `1`

---

## 🧩 Part 2

Determine how many connected regions of used squares exist in the grid.

### 💡 Approach

- Reuse the same parsed 128 binary rows
- Walk every position in the grid
- Skip cells that are already visited or contain `0`
- When a new used square is found, recursively visit all connected neighbours
- Increment the region count after each full flood-fill

---

## 🧠 Code Breakdown

### `Day14.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Disk Defragmentation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `DiskDefragmentation.Squares(this.Input[0])`

For Part 2:

- Calls `DiskDefragmentation.Regions(this.Input[0])`

---

### `DiskDefragmentation.cs`

This class contains the full grid generation and region traversal logic.

It defines:

- a `Binary` lookup table for hex-to-binary conversion
- a fixed `Size` of `128`

It exposes two public methods:

- `Squares(string input)`
- `Regions(string input)`

---

### Hex to Binary Conversion

The solver uses a dictionary that maps each hexadecimal character to a 4-bit binary string.

Examples:

    '0' -> "0000"
    '1' -> "0001"
    'a' -> "1010"
    'f' -> "1111"

This lets each Knot Hash expand into a full binary row without manual bit math.

---

### Building the Grid

`Parse(string input)` generates the 128 disk rows.

For each index from `0` to `127` it creates:

    $"{input}-{i}"

That string is passed to `KnotHash.Hash(...)`.

Each character in the resulting hexadecimal hash is then converted through the `Binary` lookup, and the 4-bit chunks are joined into a single 128-character row.

The final parsed structure is:

- `List<string>`

where each string is one binary row of the disk.

---

### Part 1 Logic

`Squares(string input)` works by parsing the grid and flattening all rows into one sequence.

It does this with:

    Parse(input).Join().Count(x => x == '1')

So Part 1 is simply:

- build all 128 binary rows
- concatenate them
- count the used squares

---

### Part 2 Logic

`Regions(string input)` also starts by calling `Parse(input)`.

It then creates:

- `bool[,] visited = new bool[Size, Size]`
- `int regions = 0`

Next it scans the full grid with nested loops over `y` and `x`.

For each position:

- if that square has already been visited, skip it
- if the corresponding grid value is `0`, skip it
- otherwise call `Visit(x, y, hashes, visited)`
- after that visit completes, increment `regions`

The final `regions` value is returned.

---

### Flood-Fill Traversal

The recursive helper `Visit(int x, int y, List<string> hashes, bool[,] visited)` handles region expansion.

It first stops if:

- the square has already been visited
- the square contains `0`

Otherwise it:

- marks the square as visited
- recursively checks left
- recursively checks right
- recursively checks up
- recursively checks down

Boundary checks ensure recursion stays inside the 128x128 grid.

This means every connected block of `1` values is visited exactly once as a region.

---

### Coordinate Access Pattern

The implementation uses:

    hashes[x][y]

together with:

    visited[x, y]

and loops over `y` outside and `x` inside.

So the solver consistently treats the parsed binary strings and visited array using the same coordinate convention throughout the region scan and recursive traversal.

---

## 🛠 Implementation Notes

- The grid is generated from Knot Hash values, not read directly from input lines
- Hexadecimal hashes are expanded using a lookup dictionary rather than bitwise conversion
- Part 1 counts all `1` bits across the entire parsed disk
- Part 2 uses recursive flood-fill to identify connected regions
- Connectivity is only orthogonal, not diagonal
- The grid size is fixed at `128`

---

## 🧪 Behaviour Summary

Given a single input key:

- the solver generates 128 Knot Hash rows
- each row becomes a 128-character binary string
- Part 1 counts all used squares in the disk
- Part 2 scans the grid for unvisited used squares
- each discovered group is explored fully through recursion
- the total number of those groups is returned as the region count

---

## 🚀 Key Takeaways

- Good example of transforming hash output into a binary grid
- Uses a simple lookup table to convert hex digits into bits
- Part 1 is a straightforward count of used squares
- Part 2 is a classic flood-fill connected-region problem
- The Knot Hash from Day 10 is reused as the core row-generation step

---

## 🔗 References

- https://adventofcode.com/2017/day/14