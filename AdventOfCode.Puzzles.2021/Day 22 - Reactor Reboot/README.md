# 🎄 Advent of Code 2021 - Day 22: Reactor Reboot

## 📜 Puzzle Overview

This puzzle processes a sequence of reboot instructions for a 3D reactor.

Each instruction turns a cuboid region either:

- `on`
- `off`

An input line looks like this:

    on x=10..12,y=10..12,z=10..12

Each instruction defines:

- whether the cuboid should be enabled or disabled
- the minimum and maximum bounds on the X axis
- the minimum and maximum bounds on the Y axis
- the minimum and maximum bounds on the Z axis

Part 1 applies only the instructions that fall inside the small initialisation region.

Part 2 applies the full instruction list with no region restriction.

This implementation does not simulate every single cube individually. Instead, it tracks whole cuboids and uses overlap handling to calculate the final active volume.

---

## 🧩 Part 1

Count how many cubes are on after applying only the initialisation-region instructions.

### 💡 Approach

- Parse every input line into an `Instruction`
- Ignore instructions outside the `-50..50` style initialisation region
- Convert each instruction into a `Cube`
- Track overlap regions between new instructions and already-known regions
- Use separate `On` and `Off` cube lists to apply inclusion-exclusion logic
- Sum the volume of all `On` cubes
- Subtract the volume of all `Off` cubes
- Return the final total

---

## 🧩 Part 2

Count how many cubes are on after applying the full reboot sequence.

### 💡 Approach

- Reuse the same parsed instruction list
- Do not ignore out-of-range instructions
- Process each cuboid instruction in order
- For every new cuboid, calculate how it overlaps with previously tracked cuboids
- Add overlap cuboids into the opposite list to cancel double counting
- Add the new cuboid itself only when the instruction is `on`
- Compute the final answer from cuboid volumes rather than per-point simulation

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Reactor Reboot`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new ReactorReboot(this.Input)`
- calls `Reboot()`

For Part 2:

- creates `new ReactorReboot(this.Input)`
- calls `Reboot(false)`

So the only difference between silver and gold is whether out-of-range instructions are ignored.

---

### `Instruction.cs`

This class parses a single reboot instruction.

It stores:

- `IsOn`
- `Min`
- `Max`
- `Ignore`

The constructor:

- splits the line into the command and coordinate part
- sets `IsOn` based on whether the command is `on`
- parses the X, Y, and Z ranges into long vectors
- sets `Ignore` based on whether the cuboid lies outside the Part 1 initialisation range

At a high level it does:

- parse `on` or `off`
- parse `x=..`
- parse `y=..`
- parse `z=..`
- precompute whether the instruction should be skipped in silver mode

---

### Initialisation Region Filtering

The `Ignore` property is calculated during parsing.

The implementation uses the rule:

    !(this.Min.X > -50 && this.Max.X < 50
    && this.Min.Y > -50 && this.Max.Y < 50
    && this.Min.Z > -50 && this.Max.Z < 50)

So in Part 1:

- instructions fully inside the bounded region are processed
- everything else is skipped

Part 2 bypasses that filter by calling `Reboot(false)`.

---

### `Cube.cs`

This class models a 3D cuboid.

It stores:

- `Min`
- `Max`

It can be created either from:

- explicit min and max vectors
- an `Instruction`

This class provides the key geometry helpers used by the solver.

---

### Cuboid Volume

`Volume()` calculates the number of cubes inside the cuboid.

It uses inclusive bounds:

    (this.Max.X - this.Min.X + 1)
    * (this.Max.Y - this.Min.Y + 1)
    * (this.Max.Z - this.Min.Z + 1)

So a cube from `10..12` on one axis has size `3`, not `2`.

That inclusive `+ 1` behaviour is essential for the puzzle's range semantics.

---

### Cuboid Intersection Check

`Intersects(Cube cube)` tests whether two cuboids overlap.

It checks all three axes:

- this minimum is before the other maximum
- this maximum is after the other minimum

for X, Y, and Z.

Logically:

    ranges overlap on X
    and ranges overlap on Y
    and ranges overlap on Z

Only if all three axes overlap is there a real 3D intersection.

---

### Computing the Overlap Region

`Overlaps(Cube cube)` returns the actual intersecting cuboid between two cubes.

It builds a new cube from:

- the larger of the two minima
- the smaller of the two maxima

At a high level:

    overlapMin = max(minA, minB)
    overlapMax = min(maxA, maxB)

So this method turns an overlap test into a concrete cuboid that can be counted.

---

### `ReactorReboot.cs`

This class contains the main reboot logic.

It stores:

- `Instructions`
- `On`
- `Off`

The constructor:

- parses the input into `Instruction` objects
- creates empty `On` and `Off` cube lists

So the reactor state is managed entirely through cuboid collections rather than a 3D boolean grid.

---

### Main Reboot Loop

`Reboot(bool ignore = true)` processes every instruction in order.

It loops through:

    foreach (Instruction instruction in this.Instructions)

and calls:

    this.Reboot(instruction, ignore);

After all instructions are applied, it returns:

    this.On.Sum(x => x.Volume()) - this.Off.Sum(x => x.Volume())

So the final result is calculated through inclusion-exclusion:

- add all positive cuboid volume
- subtract all cancelling overlap volume

---

### Processing One Instruction

`Reboot(Instruction instruction, bool ignore)` handles a single step.

It first checks:

    if (ignore && instruction.Ignore)
        return;

So Part 1 skips instructions outside the limited region.

Then it creates:

- `List<Cube> overlaps = new();`
- `Cube cube = new(instruction);`

This new `cube` is the current cuboid being applied.

---

### Handling Overlap with Existing `On` Cubes

The method loops through every cube in `On`.

For each one:

- if the new cube intersects it
- compute the overlap region
- add that overlap to the temporary `overlaps` list

This means:

- any region that was already counted as on
- and is now being touched again
- must later be subtracted once to avoid double counting

So overlaps with `On` cubes are turned into future `Off` cube regions.

---

### Handling Overlap with Existing `Off` Cubes

The method also loops through every cube in `Off`.

For each one:

- if the new cube intersects it
- compute the overlap region
- add that overlap directly into `On`

This reverses the previous subtraction when a region has been excluded multiple times.

So overlaps with `Off` cubes are added back in as positive volume.

This is the core inclusion-exclusion pattern in the implementation.

---

### Finalising the Current Instruction

After overlap handling, the method does:

    this.Off.AddRange(overlaps);

So all intersections with previous `On` regions are recorded as subtractive regions.

Then, if the instruction itself is an `on` instruction:

    if (instruction.IsOn)
        this.On.Add(cube);

That means:

- `on` instructions add their full cube after overlap bookkeeping
- `off` instructions do not add the full cube directly
- they only contribute through overlap cancellation

---

### Why Two Lists Are Enough

The implementation never splits cubes into many smaller fragments.

Instead it relies on:

- `On` cubes representing added volume
- `Off` cubes representing subtracted overlap volume

By adding intersections into the opposite list each time, the solver keeps the total correct without tracking every single cube state explicitly.

This makes the large unrestricted Part 2 input practical.

---

## 🛠 Implementation Notes

- `Day22.cs` uses `Reboot()` for silver and `Reboot(false)` for gold
- `Instruction` parses ranges into `Vector<long>`
- The solver works with whole cuboids, not individual cube coordinates
- `Volume()` uses inclusive bounds with `+ 1`
- Overlap handling is based on inclusion-exclusion
- `On` and `Off` are both `List<Cube>`
- Part 1 filtering is precomputed in `Instruction.Ignore`
- `off` instructions do not add their own cube directly, only overlap corrections

---

## 🧪 Behaviour Summary

Given a list of 3D reboot instructions:

- the solver parses each line into an instruction with bounds and state
- it converts instructions into cuboids
- it tracks additive and subtractive cube regions separately
- intersections with prior cuboids are recorded to prevent double counting
- Part 1 skips instructions outside the initial bounded region
- Part 2 processes the full instruction list
- the final result is total `On` volume minus total `Off` volume

---

## 🚀 Key Takeaways

- Good example of solving large 3D region problems without brute-force voxel simulation
- Cuboid intersection logic is enough to support both puzzle parts
- Inclusion-exclusion avoids the need to split every overlapping region into fragments
- Part 1 and Part 2 share the same core algorithm
- The approach scales because it counts volume mathematically instead of enumerating points

---

## 🔗 References

- https://adventofcode.com/2021/day/22