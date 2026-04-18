# 🎄 Advent of Code 2018 - Day 20: A Regular Map

## 📜 Puzzle Overview

This puzzle interprets a **regex-like movement string** that describes every possible path through a maze of rooms.

The input looks like:

    ^ENWWW(NEEE|SSE(EE|N))$

It uses:

- `N`, `S`, `E`, `W` → movement directions
- `()` → grouped branches
- `|` → alternate routes
- `^` and `$` → start/end markers

The goal is to build a full map of rooms connected by doors, then analyse shortest paths.

---

## 🧩 Part 1

Find the **maximum shortest-path distance** from the starting room to any other room.

### 💡 Approach

- Parse the regex-like input
- Simulate movement through all possible branches
- Build a graph of rooms connected by doors
- Run shortest-path search (BFS) from the start
- Track the furthest reachable room

Return:

- the largest number of doors in any shortest path

---

## 🧩 Part 2

Count how many rooms are at least **1000 doors away** from the start.

### 💡 Approach

- Reuse the same room graph built in Part 1
- Run BFS from the origin `(0,0)`
- Record distance to every reachable room
- Count rooms where:

    distance ≥ 1000

Return that count

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the title to `A Regular Map`
- Loads the regex input string
- Builds the full map graph
- Executes BFS-based analysis for both parts

For Part 1:

- Computes all shortest paths
- Returns the maximum distance found

For Part 2:

- Counts rooms beyond distance threshold (1000)

---

### Graph Representation

The maze is represented as a **graph of rooms**, where:

- each room is a coordinate `(x, y)`
- edges represent doors between adjacent rooms

Example connections:

    (0,0) --N--> (0,-1)
    (0,0) --E--> (1,0)

The graph is typically stored using:

- a dictionary keyed by position
- adjacency lists for each room

---

### Parsing the Regex

The parser walks the input string character-by-character:

- Direction letters update current positions
- `(` pushes state (branch start)
- `|` resets position to branch start
- `)` closes branch group
- `^` and `$` are ignored

A stack is used to manage branching state:

- store starting positions for groups
- restore them when encountering `|`

---

### Building the Map

As the regex is processed:

- every movement adds an edge between rooms
- doors are bidirectional:

    A ↔ B

This ensures the final structure is an undirected graph.

---

### Branch Handling

When encountering:

    (A|B|C)

The logic is:

- store current position on stack
- explore each branch independently
- merge all resulting paths back into graph

This effectively expands all possible routes.

---

### Distance Calculation (BFS)

Once the full graph is built:

- run Breadth-First Search from `(0,0)`
- track distance to every room

BFS guarantees:

- first time a room is visited = shortest path

So we maintain:

    distance[room] = shortest distance from start

---

### Part 1 Result

After BFS:

- scan all distances
- return maximum value

This represents:

- the farthest room (in shortest-path terms)

---

### Part 2 Result

Using the same BFS output:

- count all rooms where:

    distance ≥ 1000

Return that count directly.

---

## 🛠 Implementation Notes

- Input is a single regex-style string
- A stack tracks branching positions
- Graph is built dynamically during parsing
- All edges are bidirectional
- BFS is used for shortest path calculation
- Same graph supports both parts
- No need to simulate paths after graph construction

---

## 🧪 Behaviour Summary

Given a regex describing movement:

- parse all possible routes through branching structure
- build a complete room connectivity graph
- compute shortest paths from origin
- Part 1 finds the farthest room
- Part 2 counts rooms beyond a distance threshold

---

## 🚀 Key Takeaways

- Classic regex-to-graph parsing problem
- Stack-based branching is essential for correctness
- Graph construction and BFS cleanly separate concerns
- Avoids exponential path simulation by building full connectivity
- BFS ensures optimal shortest-path computation
- Elegant example of turning structured text into a navigable graph

---

## 🔗 References

- https://adventofcode.com/2018/day/20