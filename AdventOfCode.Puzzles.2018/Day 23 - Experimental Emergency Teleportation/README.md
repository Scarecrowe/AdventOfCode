# 🎄 Advent of Code 2018 - Day 23: Experimental Emergency Teleportation

## 📜 Puzzle Overview

This puzzle simulates a swarm of **nanobots in 3D space**.

Each nanobot has:

- a position in 3D space `(x, y, z)`
- a signal radius `r`

Example input:

    pos=<10,12,12>, r=2
    pos=<12,14,12>, r=2
    pos=<16,12,12>, r=4

A nanobot can communicate with any point whose **Manhattan distance** is within its radius.

---

## 🧩 Part 1

Find how many nanobots are in range of the nanobot with the largest signal radius.

### 💡 Approach

- Parse all nanobots into structured objects
- Identify the nanobot with the maximum radius
- For each other nanobot:
  - compute Manhattan distance to the strongest nanobot
  - check if distance ≤ radius
- Count how many are in range

Return:

- number of nanobots within range of the strongest one

---

## 🧩 Part 2

Find the coordinate that is in range of the **largest number of nanobots**, and among ties choose the one closest to `(0,0,0)`.

### 💡 Approach

This is a much harder optimisation problem.

- Each nanobot defines a 3D “coverage region”
- We want a point covered by the most overlapping regions
- Among equal overlaps:
  - choose smallest Manhattan distance to origin

Common strategy used in solutions:

- convert nanobot ranges into **distance constraints**
- search for best point using:
  - space partitioning (octree / grid subdivision)
  - or binary search over distance ranges
  - or prioritised search (heap / BFS expansion)
- score each candidate point by:
  - number of bots in range
  - distance to origin

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets title to `Experimental Emergency Teleportation`
- Parses nanobot input into a list of bot objects
- Executes both parts using shared parsed data

For Part 1:

- finds strongest nanobot
- counts bots in range of it

For Part 2:

- runs optimisation search over 3D space
- returns best coordinate under constraints

---

### Nanobot Representation

Each nanobot stores:

- position `(x, y, z)`
- radius `r`

Distance metric used:

    Manhattan distance:
    |x1 - x2| + |y1 - y2| + |z1 - z2|

This is critical because it defines “signal range”.

---

### Part 1 Logic

Straightforward filtering problem:

- locate bot with maximum `r`
- compute distance from it to all others
- count those satisfying:

    distance ≤ r_max

No graph or search required.

---

### Part 2 Core Idea

This is a **maximisation under geometric constraints**:

We want:

- maximise number of overlapping nanobot ranges
- minimise distance to origin in ties

Each nanobot contributes a constraint:

    |x - bx| + |y - by| + |z - bz| ≤ r

So the solution is a **multi-constraint optimisation problem in 3D space**.

---

### Search Strategy (Typical Implementation Pattern)

The solver usually works like this:

1. Convert each nanobot into a “distance influence range”
2. Start with a broad search region
3. Repeatedly refine:
   - subdivide space into smaller cubes
   - score each cube by how many bots it could still intersect
4. Prioritise regions with:
   - higher overlap potential
   - closer distance to origin

This effectively becomes a **best-first search in 3D space**.

---

### Scoring Function

For a candidate point `(x, y, z)`:

- count nanobots satisfying:

    ManhattanDistance(point, bot) ≤ bot.r

That count is the “strength” of the point.

Tie-breaker:

- Manhattan distance to `(0,0,0)`

---

### Part 2 Output

Final answer is:

- the best point found by the search strategy
- often represented as:
  - just the distance to origin (depending on implementation)

---

## 🛠 Implementation Notes

- Input parsed into nanobot objects
- Manhattan distance is core metric everywhere
- Part 1 is direct counting from a single reference bot
- Part 2 requires heuristic or search-based optimisation
- Common approach uses:
  - space subdivision
  - priority queue / best-first expansion
- Exact brute force is infeasible due to 3D range size

---

## 🧪 Behaviour Summary

Given a set of nanobots:

- Part 1 finds how many are in range of the strongest bot
- Part 2 finds the optimal point in space with:
  - maximum overlap coverage
  - minimal distance to origin

This transforms the problem from simple geometry checking into a **global optimisation problem over 3D space**.

---

## 🚀 Key Takeaways

- Manhattan distance defines spherical (octahedral) ranges in 3D
- Part 1 is a straightforward filtering task
- Part 2 is a constraint optimisation problem
- Requires intelligent search strategy (not brute force)
- Demonstrates spatial reasoning + heuristic search design
- Classic example of turning geometry into search space optimisation

---

## 🔗 References

- https://adventofcode.com/2018/day/23