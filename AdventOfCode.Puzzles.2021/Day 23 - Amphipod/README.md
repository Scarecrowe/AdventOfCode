# 🎄 Advent of Code 2021 - Day 23: Amphipod

## 📜 Puzzle Overview

This puzzle is about organising amphipods into their correct side rooms while spending the least possible energy.

You have:

- A hallway with 11 positions
- 4 side rooms (A, B, C, D)
- Amphipods that must move into their correct room

Each amphipod type has a different movement cost:

- A = 1
- B = 10
- C = 100
- D = 1000

The goal is to reach a state where:

- Each room contains only its correct amphipod type
- Total energy spent is minimised

---

## 🧩 Part 1

Solve using the standard room depth (2).

### 💡 Approach

- Parse the input into a structured map
- Represent:
  - Corridor (11 positions)
  - Rooms (4 stacks)
- Use a priority queue (Dijkstra-style search)
- Always explore the lowest-energy state first
- Generate valid moves:
  - Corridor → Room (preferred)
  - Room → Corridor (when blocked)
- Stop when all rooms are complete

---

## 🧩 Part 2

Solve using expanded rooms (depth 4).

### 💡 Approach

- Same algorithm as Part 1
- Only difference is room construction:
  - Insert extra amphipods into the middle layers
- Search space is larger but handled the same way

---

## 🧠 Code Breakdown

### `Day23.cs`

Entry point for the puzzle.

- Sets title to `Amphipod`
- Loads input
- Runs both parts

Part 1:

```text
new Amphipod(this.Input).Run()
```

Part 2:

```text
new Amphipod(this.Input, true).Run()
```

---

### `Amphipod.cs`

Main solver class.

Responsibilities:

- Holds the initial map
- Runs the search

Core logic:

- Uses a priority queue ordered by `TotalEnergy`
- Tracks visited states using a hash set
- Loop:
  - Dequeue lowest-energy state
  - Skip if already seen
  - If complete → return energy
  - Otherwise expand moves:
    - `ProcessCorridors`
    - `ProcessRooms`

---

### `AmphipodType.cs`

Defines all tile types:

- `A`, `B`, `C`, `D`
- `Empty`
- `Forbidden`

`Forbidden` is used for corridor spots directly outside rooms.

---

### `AmphipodMap.cs`

Represents a full puzzle state.

Stores:

- `TotalEnergy`
- `Corridor` (length 11)
- `Rooms` (4 stacks)
- `RoomSize`

Also contains all movement logic.

---

### Corridor Layout

Corridor has 11 positions:

```text
0 1 2 3 4 5 6 7 8 9 10
    ^   ^   ^   ^
```

Positions:

- 2, 4, 6, 8 = `Forbidden` (cannot stop here)
- Others = valid resting spots

---

### Room Setup

Rooms are stacks.

Part 1:

- Depth = 2

Part 2:

- Depth = 4
- Extra amphipods inserted into middle

Each room corresponds to a type:

- Room 0 → A
- Room 1 → B
- Room 2 → C
- Room 3 → D

---

### State Identity

Each state generates a unique key using:

- Corridor contents
- Room contents (padded)

Used in:

```text
HashSet<string> visited
```

Prevents reprocessing identical states.

---

### Goal Detection

A state is complete when:

- Every room is full
- Every amphipod matches its room type

---

### Movement System

Moves are built by cloning the current map and applying changes.

Key helpers:

- `Move(steps, type)` → adds energy
- `Pop(room)` → removes from room
- `Push(room, type)` → adds to room
- `Set(index, type)` → place in corridor
- `Empty(index)` → clear corridor

---

### Corridor → Room Moves

Attempt to move amphipods from corridor into their target room.

Conditions:

- Path must be clear
- Room must contain only correct types (or be empty)

If valid:

- Move into deepest available slot
- Add energy cost
- Enqueue new state

---

### Room → Corridor Moves

Used when amphipods are blocking others.

Process:

- Take top amphipod from a room
- Explore left and right in corridor
- Stop when blocked
- Only allow stopping on valid (non-forbidden) positions

Each valid position:

- Creates a new state
- Adds movement cost
- Enqueues into priority queue

---

### Path Checking

Ensures no amphipods block movement:

```text
IsPathEmpty(start, end)
```

- Walks corridor step-by-step
- Returns false if any occupied slot is encountered

---

### Energy Calculation

Energy = steps × type cost

Costs:

- A = 1
- B = 10
- C = 100
- D = 1000

Steps include:

- Horizontal corridor movement
- Vertical movement in/out of rooms

---

## 🛠 Implementation Notes

- Uses Dijkstra-style search via priority queue
- State cloning keeps transitions clean
- Forbidden tiles prevent illegal stops
- Visited set avoids exponential blow-up
- Same solver handles both parts via room size

---

## 🧪 Behaviour Summary

- Start with initial map
- Always expand lowest-energy state
- Prefer moving amphipods into final rooms
- Move blockers out when needed
- Track visited states
- Stop when all rooms are correct
- Return total energy

---

## 🚀 Key Takeaways

- Classic shortest-path problem over state space
- Clean separation between state (`Map`) and solver
- Priority queue guarantees optimal solution
- Efficient pruning via visited state tracking
- Same logic scales from Part 1 to Part 2

---

## 🔗 References

- https://adventofcode.com/2021/day/23