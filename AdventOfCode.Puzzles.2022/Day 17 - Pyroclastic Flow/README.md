# 🎄 Advent of Code 2022 - Day 17: Pyroclastic Flow

## 📜 Puzzle Overview

This puzzle simulates falling rock formations inside a narrow chamber.

A repeating jet pattern pushes each rock left or right as it falls.

The solver works with:

- a 2D chamber map
- five repeating rock formations
- a repeating jet sequence
- collision detection against walls, floor, and resting rocks

Part 1 drops `2022` rocks and returns the final tower height. Part 2 handles `1000000000000` rocks by detecting a repeating pattern and skipping most of the simulation.

---

## 🧩 Part 1

Determine the tower height after 2022 rocks have fallen.

### 💡 Approach

- Parse the jet pattern into left / right directions
- Create the five repeating rock formations
- Drop rocks in formation order
- For each rock:
  - place it above the current tower
  - apply the next jet push if possible
  - let it fall if possible
  - stop when it collides below
- Track the chamber height after all rocks settle
- Return the final stack height

---

## 🧩 Part 2

Determine the tower height after 1000000000000 rocks.

### 💡 Approach

- Reuse the same falling simulation
- Detect when the jet pattern wraps and the rock formation cycle aligns
- Record a repeating state based on jet index and formation cycle
- Measure:
  - the height before the repeat
  - the height gained during the repeat
  - the number of rocks in the repeat
- Skip forward mathematically instead of simulating every rock
- Simulate only the remainder and combine the values into the final height

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Pyroclastic Flow`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new PyroclasticFlow(this.Input)`
- Calls `Stack(2022)`

For Part 2:

- Creates `new PyroclasticFlow(this.Input)`
- Calls `Stack(1000000000000)`

---

### `JetDirection.cs`

This enum represents the jet push direction.

It contains:

- `Left`
- `Right`

The input string is converted into a dictionary of these directions so the simulation can cycle through them repeatedly.

---

### `StateType.cs`

This enum represents chamber cell contents.

It contains:

- `Air`
- `Rock`
- `HorizontalWall`
- `VerticalWall`
- `Corner`

So the chamber is modelled as a grid with explicit wall and floor cells, not just empty coordinates.

---

### `Rock.cs`

This class models a falling rock shape.

It stores:

- `Map`
- `Point`

The constructor takes a formation string and converts `#` cells into rock cells inside a local shape map.

Examples of supported formations include:

- `####`
- cross-shaped formation
- L-shaped formation
- vertical line
- square block

Each rock also tracks its current position in the chamber.

---

### Rock Placement and Movement

A rock is placed into the chamber with:

    AddToMap(point, map)

This:

- stores the rock's chamber position
- copies all solid rock cells into the main chamber map

Movement works by:

- removing the rock from the map
- updating its position
- drawing it again in the new position

Horizontal movement is handled by:

    Move(direction, map)

Vertical movement is handled by:

    Fall(map)

---

### Collision Detection

`IsCollision(map, point)` checks whether a rock would collide if moved to a target point.

It returns `true` when the rock would hit:

- the left wall
- the right wall
- the floor
- an already-settled rock

The method temporarily removes the rock from the map while testing, then restores it.

That allows collision checks without the rock colliding with its own current cells.

---

### `PyroclasticFlow.cs`

This class contains the full chamber simulation.

It stores:

- `Map`
- `FormationIndex`
- `JetIndex`
- `JetWrapped`
- `Formations`
- `Rocks`
- `JetDirections`
- `JetRepeatIndex`
- `JetRepeatIndexes`

The constructor:

- initialises the chamber to width `9`
- creates the floor row
- creates all rock formations
- parses the jet directions

The outer edges are walls, and the bottom row is built from corners plus horizontal wall cells.

---

### Rock Formations

`CreateRockFormations()` creates five repeating formations.

They are added in this order:

- horizontal line
- cross
- reversed L
- vertical line
- square

The simulation cycles through them with:

    NextFormation()

which increments the formation index and wraps with modulo arithmetic.

---

### Jet Pattern Parsing

`CreateJetDirections(string input)` parses the first input line.

Each character becomes:

- `<` → `JetDirection.Left`
- `>` → `JetDirection.Right`

The directions are stored by index so they can be replayed in a loop.

`NextJetDirection()` advances the index and wraps when the end is reached.

When the jet index wraps back to `0`, the solver sets:

- `JetWrapped = true`

This is important for the Part 2 repeat detection logic.

---

### Chamber Resizing

Before spawning a new rock, the solver ensures there is enough empty space above the stack.

`ResizeMap(int increase)`:

- finds the current topmost rock
- ensures there is room for:
  - three empty rows
  - the next rock's height
- expands the chamber upward if needed
- preserves existing rock positions
- rebuilds the side walls on new rows

So the map grows dynamically as the tower rises.

---

### Finding the Spawn Height

`GetStart(Rock rock)` computes the y-position where a new rock should appear.

It uses:

- the current topmost settled rock
- three empty rows above it
- the height of the incoming rock

This matches the puzzle's spawn behaviour while fitting the solver's top-origin grid representation.

---

### Main Simulation Loop

`Stack(long count)` performs the full simulation.

It begins by:

- resetting repeat-tracking data
- selecting the first rock formation
- resizing the map
- placing the first rock

Then for each rock:

- apply the next jet direction
- move left or right if no collision occurs
- check whether falling one row would collide
- if so, the rock settles
- otherwise let it fall and continue

After settling:

- add the rock to the `Rocks` list
- spawn the next formation
- resize the chamber if another rock is still to come

So each rock repeatedly alternates:

- jet push
- gravity fall

until downward collision stops it.

---

### Part 1 Return Value

For smaller runs, `Stack()` returns:

    this.Map.Height - rock.Map.Height - 1

This gives the final visible tower height after the requested number of rocks have settled.

So the silver answer is the stack height after `2022` rocks.

---

### Part 2 Repeat Detection

For huge runs, the solver avoids simulating all rocks one by one.

Inside `Stack()` it watches for a repeat only when:

- `count > 2022`
- the jet sequence has wrapped
- the rock formation cycle is back at index `0`

At that point it uses:

- `JetRepeatIndexes`
- `JetRepeatIndex`

to identify a repeated jet state aligned with the rock cycle.

When a first matching state is found, it records:

- `bottomHeight`
- `bottomRockCount`

When that same state is seen again, it computes:

- `repeatHeight`
- `repeatRockCount`

Then it reduces the remaining work to only the remainder:

    targetRockFallIndex = rockIndex + ((count - bottomRockCount) % repeatRockCount)

So the solver only simulates:

- the pre-repeat section
- one repeat measurement
- the final remainder section

---

### Final Height Calculation for Part 2

Once the repeat is known, the solver calculates:

- how many full repeats fit
- how much height each repeat contributes
- how much extra height comes from the remaining simulated rocks

It returns:

    bottomHeight + (repeatCount * repeatHeight) + moduloHeight

This produces the height for `1000000000000` rocks without simulating them all directly.

---

## 🛠 Implementation Notes

- The chamber width is `9`, including side walls
- The floor is explicitly stored in the map
- Rock movement is handled by removing and redrawing the rock
- Collision checks treat walls, floor, and settled rocks uniformly
- Rock formations repeat in a fixed 5-shape cycle
- Jet directions repeat in a fixed input-driven cycle
- Part 2 detects a repeat when jet wrapping aligns with formation reset
- The repeat optimisation turns an impossible simulation into a manageable calculation

---

## 🧪 Behaviour Summary

Given a repeating jet pattern:

- the solver creates the chamber and five rock formations
- rocks spawn one at a time above the current tower
- each step applies jet movement and then gravity
- rocks settle when they can no longer fall
- Part 1 simulates 2022 rocks directly
- Part 2 detects a repeating cycle and skips forward mathematically
- the final result is the tower height after the requested number of rocks

---

## 🚀 Key Takeaways

- Good example of grid-based simulation with collision detection
- Rock shapes are cleanly encapsulated in a dedicated class
- Chamber growth is handled dynamically as the tower rises
- Part 2 is solved with cycle detection rather than brute force
- Reusing the same simulation core makes both puzzle parts consistent

---

## 🔗 References

- https://adventofcode.com/2022/day/17