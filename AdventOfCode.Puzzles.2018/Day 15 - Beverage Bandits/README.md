# 🎄 Advent of Code 2018 - Day 15: Beverage Bandits

## 📜 Puzzle Overview

This puzzle simulates a tactical combat system between two factions inside a grid-based cave system:

- Elves (`E`)
- Goblins (`G`)
- Walls (`#`)
- Open space (`.`)

Each input is a full map layout like:

    #######
    #.G...#
    #...EG#
    #.#.#G#
    #..G#E#
    #.....#
    #######

Units take turns moving and attacking based on strict rules including:

- reading order (top-to-bottom, left-to-right)
- shortest path movement
- target prioritisation by HP and position

The battle continues until one side is eliminated.

Part 1 computes the outcome score after a full battle.

Part 2 adjusts Elf attack power until no Elf dies.

---

## 🧩 Part 1

Determine the outcome of the battle after completion.

### 💡 Approach

- Parse the grid into:
  - walls
  - open tiles
  - unit objects (Elf or Goblin)
- Each unit takes turns in reading order
- On each turn:
  - check for enemies in range
  - otherwise move using shortest path rules
  - attack adjacent enemy with lowest HP
- Continue rounds until one faction is eliminated
- Compute outcome score:

    full rounds completed × sum of remaining HP

---

## 🧩 Part 2

Find the minimum Elf attack power so that no Elf dies.

### 💡 Approach

- Re-run the full simulation multiple times
- Increment Elf attack power each run
- Stop early if any Elf dies
- Once a valid run is found:
  - compute outcome score
- Return result for first successful attack power

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets title to `Beverage Bandits`
- Loads the grid input
- Calls simulation for Part 1
- Repeats simulation for Part 2 with modified Elf power

---

### `Outcome(...)`

Core simulation method returning:

- final score
- whether any Elf died (for Part 2 validation)

It controls:

- map state
- unit list
- round counter
- combat resolution

---

### Grid Representation

The map is parsed into a 2D structure containing:

- `Wall`
- `Empty`
- `Player` (Elf or Goblin)

Each `Player` stores:

- position
- hit points
- attack power
- faction type

---

### Turn Order (Reading Order)

Each round:

- units are sorted by:
  - row (Y)
  - then column (X)

This ensures consistent deterministic turn order.

---

### Movement Logic

If a unit cannot attack immediately:

- it performs a BFS-style search
- identifies all reachable squares adjacent to enemies
- chooses:
  1. shortest path
  2. tie-break by reading order
- moves one step toward chosen target

---

### Attack Logic

After movement:

- check adjacent squares (up, left, right, down)
- collect enemy units
- select target by:
  - lowest HP
  - tie-break by reading order
- reduce HP by attack power
- remove unit if HP ≤ 0

---

### Combat Loop

The simulation runs in rounds:

- each unit acts once per round
- dead units are removed immediately
- round ends when all units have acted

The battle ends when:

- one faction has no remaining units

---

### Outcome Calculation

At the end of the simulation:

    outcome = fullRounds * sum(all remaining unit HP)

This value is returned for Part 1.

---

### Part 2 Elf Survival Check

Part 2 repeatedly calls:

- same `Outcome(...)` method

but varies:

- Elf attack power

If any Elf dies:

- simulation is discarded

If no Elf dies:

- result is accepted immediately

---

## 🛠 Implementation Notes

- Heavy use of BFS for movement resolution
- Strict ordering rules dominate logic complexity
- Grid is mutated in-place during simulation
- Units are removed immediately on death
- Same engine powers both Part 1 and Part 2
- Part 2 is essentially parameter search over attack power

---

## 🧪 Behaviour Summary

Given a cave map:

- the solver parses a full combat grid
- simulates turn-based battle with strict ordering rules
- units move, pathfind, and attack each round
- battle continues until one faction remains
- Part 1 returns combat outcome score
- Part 2 finds minimal Elf attack power for no casualties

---

## 🚀 Key Takeaways

- Classic grid-based combat simulation problem
- Heavy emphasis on deterministic ordering rules
- BFS pathfinding is central to movement logic
- Combat resolution depends on multiple tie-break rules
- Part 2 transforms simulation into a parameter search problem
- Same engine reused with different constraints

---

## 🔗 References

- https://adventofcode.com/2018/day/15