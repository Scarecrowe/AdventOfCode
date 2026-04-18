# 🎄 Advent of Code 2016 - Day 17: Two Steps Forward

## 📜 Puzzle Overview

This puzzle navigates a 4x4 grid using dynamically changing doors.

You start at:

```
(0, 0)
```

and need to reach:

```
(3, 3)
```

Movement is constrained by doors that open or close based on an MD5 hash.

At each step:

- compute `MD5(passcode + path)`
- use the first four characters of the hash to determine door states

Door order:

```
Up, Down, Left, Right
```

A door is **open** if the corresponding character is:

```
b, c, d, e, f
```

Otherwise, it is **closed**.

Part 1 finds the shortest path to the goal.  
Part 2 finds the length of the longest possible path.

---

## 🧩 Part 1

Determine the shortest path from start to goal.

### 💡 Approach

- Use Breadth-First Search (BFS)
- Each state includes:
  - current position `(x, y)`
  - path taken so far (string of moves)
- For each state:
  - compute MD5 hash of `passcode + path`
  - determine which doors are open
  - generate valid moves:
    - stay within grid bounds
- Stop when `(3, 3)` is reached
- Return the path string

---

## 🧩 Part 2

Determine the length of the longest path that reaches the goal.

### 💡 Approach

- Explore **all possible paths** (not just shortest)
- Use BFS or DFS:
  - continue exploring even after reaching the goal
- When reaching `(3, 3)`:
  - record path length
  - do not continue from that state
- Track the maximum path length found

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Two Steps Forward`
- Loads the puzzle input (passcode)
- Calls the silver and gold solutions

For Part 1:

- Performs BFS to find shortest path
- Returns path string

For Part 2:

- Explores all paths
- Returns maximum path length

---

### State Representation

Each state consists of:

- `x`, `y` position
- `path` string (sequence of moves)

Example:

```
Path: "UDLR"
Position: derived from moves
```

---

### Door Logic

For a given state:

```
hash = MD5(passcode + path)
```

Take first four characters:

- char 0 → Up
- char 1 → Down
- char 2 → Left
- char 3 → Right

Door is open if:

```
char in ['b', 'c', 'd', 'e', 'f']
```

---

### Movement Rules

From `(x, y)`:

- Up → `(x, y - 1)`
- Down → `(x, y + 1)`
- Left → `(x - 1, y)`
- Right → `(x + 1, y)`

Valid only if:

- within bounds (`0 ≤ x, y ≤ 3`)
- corresponding door is open

---

### Part 1 Logic

- Use BFS queue
- Expand states level-by-level
- First time reaching `(3, 3)`:
  - return path

---

### Part 2 Logic

- Explore all reachable states
- Track path lengths for all successful routes
- Return maximum length

---

### Search Considerations

- States are not revisited based purely on position:
  - path affects door states
- Cannot prune purely by coordinates
- Path string is part of state identity

---

## 🛠 Implementation Notes

- MD5 hashing required per state
- BFS ensures shortest path for Part 1
- DFS can be more memory efficient for Part 2
- Avoid pruning states incorrectly
- Performance depends on efficient hashing and state handling

---

## 🧪 Behaviour Summary

Given a passcode:

- Maze changes dynamically based on path
- Door states depend on hash values
- Part 1 finds shortest valid path
- Part 2 finds longest possible valid path

---

## 🚀 Key Takeaways

- Path-dependent state (position alone is insufficient)
- Combination of hashing and pathfinding
- BFS vs DFS trade-offs
- Dynamic graph exploration

---

## 🔗 References

- https://adventofcode.com/2016/day/17