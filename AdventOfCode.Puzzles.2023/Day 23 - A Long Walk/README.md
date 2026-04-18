# 🎄 Advent of Code 2023 - Day 23: A Long Walk

## 📜 Puzzle Overview

This puzzle is about exploring a hiking trail map and finding the longest possible walk.

The map is made of characters representing:

- open paths
- forest
- directional slopes

Movement is performed in the four cardinal directions:

- up
- down
- left
- right

The implementation parses the input into a 2D character grid and then explores possible paths using an explicit stack.

It supports two modes:

- a slope-respecting mode for Part 1
- a slope-ignoring mode for Part 2

---

## 🧩 Part 1

Find the longest hike while obeying the slope directions.

### 💡 Approach

- Parse the input into a 2D character map
- Scan the top row for walkable starting positions
- For each valid start:
  - begin a depth-first style search using a stack
- Track:
  - current row
  - current column
  - visited cells
  - current path length
- When moving:
  - stay inside bounds
  - do not revisit a cell already used in the current path
  - allow open ground `.`
  - allow slope tiles only when their symbol matches the movement direction
- Keep updating the maximum path length found

---

## 🧩 Part 2

Find the longest hike when slopes no longer restrict direction.

### 💡 Approach

Reuse the same search engine, but disable slope enforcement:

- open ground is still allowed
- slope tiles are also treated as traversable regardless of arrow direction
- the search still avoids revisiting cells in the same path
- the maximum path length seen during exploration is returned

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `A Long Walk`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ALongWalk(this.Input)`
- Calls `LongestHike()`

For Part 2:

- Creates `new ALongWalk(this.Input)`
- Calls `UniqueLongestHike()`

---

### `ALongWalk.cs`

This class contains all puzzle logic.

It stores:

- `map`
- `rows`
- `cols`
- `directions`

Where:

- `map` is a 2D `char[,]`
- `rows` is the map height
- `cols` is the map width
- `directions` contains the four cardinal movements paired with slope symbols

The direction table is:

- `(-1, 0, '^')`
- `(1, 0, 'v')`
- `(0, -1, '<')`
- `(0, 1, '>')`

So each move knows both:

- how coordinates change
- which slope character matches that move direction

---

### Parsing the Map

The constructor reads the input grid into a 2D array.

At a high level it does:

- store the number of rows
- store the number of columns
- create `new char[rows, cols]`
- copy every input character into the grid

So each tile can later be accessed with:

- `map[r, c]`

---

### Part 1 Entry Point

`LongestHike()` simply calls:

    Solve(true)

So Part 1 runs the shared solver with slope rules enabled.

---

### Part 2 Entry Point

`UniqueLongestHike()` calls:

    Solve(false)

So Part 2 runs the same traversal logic with slope rules disabled.

---

### Main Solver

`Solve(bool obeySlopes)` performs the full path exploration.

It begins with:

- `int maxLength = 0`

Then it scans every column of the top row looking for a starting tile equal to:

    '.'

Only those open top-row cells are used as starting points.

For each such start, it creates a stack of path states:

    Stack<(int row, int col, bool[,] visited, int length)>

Each stack item stores:

- current row
- current column
- visited map for that path
- current length

This means every candidate path carries its own visited-state snapshot.

---

### Stack-Based Search

For each starting point, the solver pushes:

- row `0`
- the chosen column
- a fresh `bool[rows, cols]`
- length `0`

Then while the stack is not empty:

- pop the current state
- mark the current cell as visited
- update `maxLength`
- inspect all four cardinal directions
- push valid next states back onto the stack

This gives the implementation a depth-first style search using an explicit stack instead of recursion.

---

### Tracking Visited Cells

The solver uses:

- `bool[,] visited`

to prevent revisiting cells in the same candidate path.

Before moving to a neighbour, it checks:

- `visited[newRow, newCol]`

If that position is already true, the move is skipped.

When a move is accepted, the visited array is cloned:

    var newVisited = (bool[,])visited.Clone();

Then the cloned version is pushed with the next state.

So each branch has its own independent history of visited tiles.

---

### Movement Validation

For each direction, the solver computes:

- `newRow = row + dx`
- `newCol = col + dy`

It rejects moves that:

- leave the map bounds
- go to a cell already visited in the current path

Then it reads:

- `char tile = map[newRow, newCol]`

A move is allowed when:

- `tile == '.'`
- or slopes are being ignored
- or slopes are enforced and the tile matches the direction symbol

So the exact rule is logically:

    tile == '.'
    OR !obeySlopes
    OR (obeySlopes AND tile == symbol)

This means:

- walls such as `#` are never accepted in Part 1
- in Part 2 any non-`.` non-visited in-bounds tile becomes traversable because slope restrictions are disabled

---

### Slope Handling

When `obeySlopes` is `true`, a slope tile is only valid if the move direction matches that slope's symbol.

For example:

- moving up allows `'^'`
- moving down allows `'v'`
- moving left allows `'<'`
- moving right allows `'>'`

So the slope check is tied directly to the movement direction being attempted.

---

### Maximum Length Tracking

The solver updates:

    maxLength = Math.Max(maxLength, length);

on every popped state.

So it records the largest path length seen anywhere during the exploration.

That value is returned after all starting positions and path branches have been processed.

---

### Important Behaviour Detail

This implementation does **not** explicitly check for a bottom-row exit or any specific finish coordinate.

Instead, it:

- explores all reachable non-revisiting paths from each valid top-row start
- keeps the largest path length encountered
- returns that maximum value

So the result comes from longest explored path length under the movement rules used by the solver.

---

## 🛠 Implementation Notes

- The map is stored as a 2D character array
- The solver scans the top row for `.` starting positions
- Search is performed with an explicit stack
- Each path branch carries its own cloned visited grid
- Movement is always cardinal
- Part 1 enforces slope direction matching
- Part 2 disables slope restrictions
- Path length is incremented by `1` per accepted move
- The method returns the maximum explored path length found

---

## 🧪 Behaviour Summary

Given a hiking map:

- the solver parses it into a 2D grid
- it finds walkable start cells on the top row
- it explores all non-revisiting paths using stack-based search
- each branch tracks its own visited cells
- Part 1 only allows slope entry when the direction matches the arrow
- Part 2 treats slopes as unrestricted walkable tiles
- the final answer is the greatest path length encountered during exploration

---

## 🚀 Key Takeaways

- Good example of using an explicit stack for path exploration
- Per-branch visited-state cloning keeps each candidate path independent
- The same core solver supports both puzzle parts via a boolean mode flag
- Direction symbols are neatly paired with movement vectors
- Slope rules are implemented directly in the move validation step
- The solution focuses on longest explored simple paths rather than shortest-path search

---

## 🔗 References

- https://adventofcode.com/2023/day/23