# 🎄 Advent of Code 2023 - Day 22: Sand Slabs

## 📜 Puzzle Overview

This puzzle simulates a stack of 3D bricks falling downward under gravity.

Each input line defines one brick as two endpoints:

    x1,y1,z1~x2,y2,z2

Each brick occupies every coordinate between those endpoints across its footprint and height.

The solver:

- parses all bricks
- sorts them by starting height
- lets them settle downward onto either:
  - the ground
  - or other already-settled bricks
- builds support relationships between bricks

Part 1 counts how many bricks can be safely removed without causing any brick above to lose its only support.

Part 2 tests removing each brick and counts how many additional bricks would fall in the resulting chain reaction.

---

## 🧩 Part 1

Determine how many bricks can be disintegrated safely.

### 💡 Approach

- Parse each brick from the input
- Sort bricks by their initial `Z1`
- Settle them from lowest to highest
- While settling, record:
  - which bricks support the current brick
  - which bricks are above each settled brick
- After all bricks are settled, count bricks where every brick above them has more than one supporter

So a brick is safe to remove if no brick resting on it depends on it alone.

---

## 🧩 Part 2

Determine the total number of bricks that would fall if each brick were disintegrated one at a time.

### 💡 Approach

- Reuse the settled support graph from Part 1
- For each brick:
  - mark it as removed
  - use a queue to process all bricks above it
  - if a brick's supporters are all already removed, it falls too
  - continue until no more bricks fall
- Count all falling bricks across all starting removals
- Return the total

This gives the full chain-reaction count.

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sand Slabs`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SandSlabs(this.Input)`
- Calls `Fall()`

For Part 2:

- Creates `new SandSlabs(this.Input)`
- Calls `Disintergrate()`

---

### `Brick.cs`

This class models a single brick.

It stores:

- `X1`
- `Y1`
- `Z1`
- `X2`
- `Y2`
- `Z2`

It also stores relationship data:

- `Above`
- `Supporters`

`Above` contains the bricks resting directly on this brick.

`Supporters` contains the bricks directly supporting this brick.

---

### Brick Footprint

`Coords` returns all `(x, y)` positions covered by the brick footprint.

It loops from:

- `X1` to `X2`
- `Y1` to `Y2`

and yields every coordinate pair in that rectangular area.

So the solver treats support in terms of horizontal footprint overlap, while vertical placement is handled separately during settling.

---

### Brick Height

`Height` is calculated as:

    Z2 - Z1 + 1

This is preserved when the brick falls.

So settling changes the brick's vertical position, but not its shape.

---

### `SandSlabs.cs`

This class contains the parsing, settling, and result logic.

It stores:

- `Bricks`
- `Heightmap`

`Bricks` holds the full list of parsed bricks.

`Heightmap` maps each `(x, y)` position to:

- the brick currently occupying the top at that coordinate
- the top `z` value at that coordinate

This lets the solver efficiently determine what is directly underneath a new falling brick.

---

### Parsing the Input

The constructor parses each input line with:

- `line.Split('~', ',')`
- `Select(int.Parse).ToArray()`

This produces six integers:

- `x1`
- `y1`
- `z1`
- `x2`
- `y2`
- `z2`

These are used to create a `Brick`.

After parsing, all bricks are ordered by:

- `b.Z1`

Then the constructor immediately calls:

- `this.Settle()`

So all later calculations operate on already-settled bricks.

---

### Settling the Bricks

`Settle()` performs the gravity simulation.

It begins by clearing all existing relationship data:

- `brick.Above.Clear()`
- `brick.Supporters.Clear()`

It also clears the heightmap.

Then it processes bricks in order from lowest starting height upward.

For each brick it:

- checks every `(x, y)` coordinate in its footprint
- looks up the current top occupied height in `Heightmap`
- finds the highest supporting level beneath any part of the brick

That maximum supporting height becomes:

    maxBelow

The brick is then moved so its new bottom sits at:

    maxBelow + 1

and its new top is set using its original height.

---

### Finding Supporters

While scanning the footprint, the solver collects candidate supporting bricks.

For each occupied `(x, y)` cell below the brick it stores:

- the brick found there
- that brick's top `z` at that coordinate

After the final `maxBelow` is known, only candidate bricks whose top height equals that exact `maxBelow` become real supporters.

Those bricks are added to:

- `brick.Supporters`

And the reverse relationship is recorded by adding the current brick to:

- `candidate.Above`

So support is only created for bricks that actually touch the settled brick from directly beneath.

---

### Updating the Heightmap

Once the brick has been repositioned, the solver writes it back into the heightmap.

For every `(x, y)` in the brick footprint it stores:

- the current brick
- its new top `z`

This means later bricks can use it as support.

---

### Part 1 Safe Removal Check

`Fall()` returns:

- the number of bricks that can be safely removed

It does this with:

    this.Bricks.Count(b => b.Above.All(a => a.Supporters.Count > 1))

So a brick is safe if every brick above it has at least one other supporter besides that brick.

If removing it would leave any brick above with zero supports, it is not counted.

---

### Part 2 Chain Reaction Logic

`Disintergrate()` checks what happens when each brick is removed.

For each starting brick it creates:

- `HashSet<Brick> removed = new() { brick };`
- `Queue<Brick> queue = new();`

It enqueues the removed brick, then repeatedly processes bricks above it.

For each `above` brick:

- if all of its supporters are already in `removed`
- then it also falls

When that happens:

- it is added to `removed`
- `total` is incremented
- it is enqueued so bricks above it can also be checked

This continues until the chain reaction stops.

---

### Part 2 Return Value

After trying every brick as the initial removed brick, `Disintergrate()` returns:

- the total number of secondary falling bricks across all tests

The starting brick itself is not added to `total`.

Only additional bricks that fall because of the removal are counted.

---

## 🛠 Implementation Notes

- Bricks are parsed once and settled immediately in the constructor
- Settling is done in ascending order of initial `Z1`
- Support is tracked with explicit `Above` and `Supporters` lists
- The heightmap stores only the current top brick at each `(x, y)`
- Brick height is preserved when falling
- Part 1 is a support-graph query after settling
- Part 2 uses a queue-based chain-reaction traversal
- The method name is `Disintergrate()` in the implementation

---

## 🧪 Behaviour Summary

Given a list of 3D bricks:

- the solver parses each brick from its endpoint coordinates
- sorts bricks by starting height
- drops them into their settled positions
- records which bricks support which others
- Part 1 counts bricks that can be removed safely
- Part 2 simulates cascading falls after each possible removal
- the final result is either:
  - the safe brick count
  - or the total chain-reaction count

---

## 🚀 Key Takeaways

- Nice example of reducing a 3D falling problem into a 2D heightmap plus support graph
- The settling phase builds all structural relationships needed for both parts
- Part 1 becomes a simple dependency check
- Part 2 becomes a graph traversal over support loss
- Tracking only topmost occupied cells keeps the falling simulation efficient

---

## 🔗 References

- https://adventofcode.com/2023/day/22