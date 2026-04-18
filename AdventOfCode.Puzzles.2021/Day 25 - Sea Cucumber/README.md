# 🎄 Advent of Code 2021 - Day 25: Sea Cucumber

## 📜 Puzzle Overview

This puzzle simulates a herd of sea cucumbers moving across a wraparound ocean floor grid.

Each input character is one grid cell:

- `.` means empty
- `>` means an east-facing sea cucumber
- `v` means a south-facing sea cucumber

Example input:

    ...>>>>>...
    ...........
    ...........

On each step:

- east-facing cucumbers try to move first
- south-facing cucumbers move second
- movement only happens into empty cells
- the grid wraps around at the edges

Part 1 asks how many steps occur before no sea cucumbers can move.

There is no real Part 2 puzzle calculation here, because Advent of Code Day 25 only requires the first answer to complete the year.

---

## 🧩 Part 1

Find the first step where the sea cucumber layout stops changing.

### 💡 Approach

- Parse the input grid into numeric cell values
- Repeatedly simulate one full movement step
- First move all east-facing cucumbers that can move
- Then move all south-facing cucumbers that can move
- Use wraparound when movement crosses the right or bottom edge
- Stop once a full step completes with no movement
- Return the number of steps that were executed

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Sea Cucumber`
- Loads the puzzle input
- Calls the silver solution

For Part 1:

- creates `new SeaCucumber(this.Input)`
- calls `Run()`

For the gold method:

- returns the fixed string:
  
    You have enough stars to [Remotely Start The Sleigh]

So this implementation only performs simulation work for the silver answer.

---

### `SeaCucumber.cs`

This class contains the full map parsing and movement simulation.

It stores:

- `Map`

The constructor parses the input directly into a numeric grid with:

    new(input, (c) => ".>v".IndexOf(c))

That means each character becomes:

- `.` -> `0`
- `>` -> `1`
- `v` -> `2`

So the map is stored as integers rather than characters.

---

### Internal Cell Encoding

The implementation uses this string:

    ".>v"

and converts each input character with:

    IndexOf(c)

So the movement logic works with numeric values:

- `0` means empty
- `1` means east-facing cucumber
- `2` means south-facing cucumber

This makes the movement checks compact and easy to compare.

---

### Printing the Grid

`Print()` converts the numeric grid back into characters using:

    string chars = ".>v";

Then for every map cell it writes:

    chars[this.Map[y, x]]

So the class can render the current simulation state back into readable puzzle form.

This method returns the current `SeaCucumber` instance after printing.

---

### Main Simulation Loop

`Run()` performs the full movement simulation.

It begins with:

    int step = 0;
    bool moved = true;

Then it loops while movement is still happening:

    while (moved)

At the start of each step it resets:

    moved = false;

So the loop only continues if at least one cucumber successfully moves during that round.

---

### Cloning State Between Phases

At the start of each simulation step, the implementation creates:

    VectorArray<int, int> state = this.Map.Clone();

This cloned map is used as the writable next-state buffer.

That matters because all east-facing cucumbers must evaluate movement against the same original state for that phase, rather than seeing partial updates from other cucumbers that have already moved.

The same pattern is then reused for the south-facing phase.

---

### East-Facing Movement

The first movement pass scans the map with:

    foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())

For each cell, it checks:

    if (this.Map[cell.Point] == 1 && this.Map[cell.Point.Y, (cell.Point.X + 1) % this.Map.Width] == 0)

So an east-facing cucumber can move when:

- the current cell contains `1`
- the cell immediately to the right is empty
- wrapping is handled by `% this.Map.Width`

When movement is allowed, the code does:

    state[cell.Point] = 0;
    state[cell.Point.Y, (cell.Point.X + 1) % this.Map.Width] = 1;

and sets:

    moved = true;

So all east-facing moves are written into the cloned state map.

---

### Applying the East-Move Result

After the east-facing pass completes, the implementation updates the live map with:

    this.Map = state.Clone();

That means the south-facing pass will evaluate movement against the already-updated post-east state, which matches the puzzle rules.

So the two movement phases are cleanly separated.

---

### South-Facing Movement

The second pass again iterates over the map, this time checking for south-facing cucumbers:

    if (this.Map[cell.Point] == 2 && this.Map[(cell.Point.Y + 1) % this.Map.Height, cell.Point.X] == 0)

So a south-facing cucumber can move when:

- the current cell contains `2`
- the cell immediately below is empty
- wrapping is handled by `% this.Map.Height`

When movement is allowed, the code updates the next state with:

    state[cell.Point] = 0;
    state[(cell.Point.Y + 1) % this.Map.Height, cell.Point.X] = 2;

and again sets:

    moved = true;

So south-facing motion is based on the map after the east-facing step, not the original starting grid for that round.

---

### Step Completion

After both movement phases finish, the solver does:

    this.Map = state.Clone();
    step++;

So one full puzzle step is:

- east-facing movement
- apply east result
- south-facing movement
- apply south result
- increment the step counter

When neither phase moves anything, `moved` stays `false`, the loop ends, and the method returns:

    step

So the answer is the first step count where the herd becomes stable.

---

## 🛠 Implementation Notes

- `Day25.cs` uses `Run()` for silver
- The gold method returns a fixed completion message instead of running extra logic
- The map is stored as `VectorArray<int, int>`
- Grid characters are encoded with `".>v".IndexOf(c)`
- East-facing cucumbers are stored as `1`
- South-facing cucumbers are stored as `2`
- Movement uses cloned state buffers so each phase reads a stable grid
- Horizontal wrapping uses `% this.Map.Width`
- Vertical wrapping uses `% this.Map.Height`

---

## 🧪 Behaviour Summary

Given a grid of sea cucumbers:

- the solver parses the input into a numeric map
- each step first moves east-facing cucumbers into empty wrapped cells
- then it moves south-facing cucumbers into empty wrapped cells
- movement is applied in two distinct phases
- the simulation stops when a full step causes no movement
- the final result is the number of steps needed to reach that stable state

---

## 🚀 Key Takeaways

- Good example of two-phase grid simulation
- Cloned state maps keep each movement phase consistent
- Wraparound movement is handled cleanly with modulo arithmetic
- The puzzle only needs the first answer, so the gold method is just a completion message
- The implementation stays compact by encoding grid entities as integers

---

## 🔗 References

- https://adventofcode.com/2021/day/25