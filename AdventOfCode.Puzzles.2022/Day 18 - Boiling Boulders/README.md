# 🎄 Advent of Code 2022 - Day 18: Boiling Boulders

## 📜 Puzzle Overview

This puzzle works with a collection of lava cubes in 3D space.

Each input line defines one cube as:

    x,y,z

The solver parses those coordinates into 3D vectors and then calculates exposed cube faces.

Part 1 counts every face that is not touching another lava cube. Part 2 refines that by excluding faces that touch enclosed air pockets, so only the true exterior surface area is counted.

---

## 🧩 Part 1

Determine the total surface area of the lava droplet.

### 💡 Approach

- Parse every input line into a 3D cube position
- For each lava cube:
  - check all 6 adjacent directions
  - count how many neighbours are also lava
- Start each cube with 6 faces
- Subtract the number of touching neighbours
- Sum the exposed faces across all cubes

---

## 🧩 Part 2

Determine the exterior surface area of the lava droplet.

### 💡 Approach

- Build a full 3D volume around the lava droplet
- Mark each position as:
  - lava
  - air
- Flood-fill from outside the droplet
- Mark all reachable outside air as non-enclosed space
- Then count lava faces that touch:
  - empty space outside the map
  - or flood-filled external air
- Ignore faces touching trapped internal air pockets

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Boiling Boulders`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new BoilingBoulders(this.Input)`
- Calls `SurfaceArea()`

For Part 2:

- Creates `new BoilingBoulders(this.Input)`
- Calls `ExteriorSurfaceArea()`

---

### `BoilingBoulders.cs`

This class contains the main puzzle logic.

It stores:

- `Lava`
- `DefaultLava`

It also defines the 6 cardinal directions in 3D:

- `(+1, 0, 0)`
- `(-1, 0, 0)`
- `(0, +1, 0)`
- `(0, -1, 0)`
- `(0, 0, +1)`
- `(0, 0, -1)`

The constructor:

- parses the input cubes
- stores them in `DefaultLava`
- creates a `Lava` volume from that list

---

### Parsing the Input

`ParseCubes(string[] input)` converts each line into a `Vector`.

At a high level it does:

- split the line on `","`
- convert the three parts to integers
- create a 3D vector from those coordinates

So an input like:

    2,2,2

becomes one cube position in 3D space.

---

### Adjacent Cube Checking

`GetAdjacent(Cube cube)` yields the 6 neighbouring positions around a cube:

- left
- right
- up
- down
- front
- back

This method is used by the Part 1 surface area logic.

---

### `SurfaceArea()`

This method calculates the raw exposed surface area.

For each cube in `DefaultLava`:

- start a neighbour count at `0`
- inspect all 6 adjacent positions
- if an adjacent position also exists in `DefaultLava`, increment the count
- add:

      6 - touchingNeighbours

to the total

So if a cube touches two other lava cubes, it contributes:

    4

exposed faces.

---

### `Cube.cs`

This class models one cube in the 3D volume.

It stores:

- `Point`
- `Type`

The constructor accepts:

- a `Vector point`

It also has:

- `SetType(CubeType type)`

which marks the cube as one of the supported cube states.

---

### `CubeType.cs`

This enum defines the three cube states:

- `None`
- `Lava`
- `Air`

These are used during the exterior-air flood fill and final face counting.

---

### `Lava.cs`

This class extends a dictionary of:

- `Vector`
- `Cube`

It represents the full 3D space around the lava droplet, not just the lava cubes themselves.

The constructor:

- finds the minimum lava coordinates
- finds the maximum lava coordinates
- expands the bounds by `1` in every direction
- fills the whole bounded 3D region with cubes

Each cube is initialised as:

- `CubeType.Lava` if its position is in the original droplet
- `CubeType.Air` otherwise

This creates a padded volume around the droplet so outside air can be identified cleanly.

---

### Bounding Box Creation

The `Lava` constructor computes:

- `Min`
- `Max`

using the minimum and maximum coordinates from the input, then expands those bounds by `1`.

So if the lava occupies a range like:

- `minX .. maxX`
- `minY .. maxY`
- `minZ .. maxZ`

the full volume becomes slightly larger in every direction.

This padding is important because it guarantees there is a known external starting point for the flood fill.

---

### Sorting Air Pockets

`SortAirPockets()` performs a flood fill through the outside air.

It creates:

- `List<Vector> processed = new();`
- `Queue<Cube> queue = new();`

Then it begins from:

    Min.X, Min.Y, Min.Z

which is guaranteed to be outside the lava droplet because the volume was padded.

The method then:

- dequeues the next cube
- skips it if already processed
- marks that cube as:

      CubeType.None

- checks all 6 cardinal neighbours
- enqueues any neighbouring cube that is currently `CubeType.Air`

This means:

- reachable external air becomes `None`
- enclosed air pockets remain as `Air`

---

### `ExteriorSurfaceArea()`

This method calculates the true external surface area.

It first rebuilds the `Lava` volume from the original cube list, then calls:

    this.Lava.SortAirPockets();

After that, it iterates through every cube in the volume.

For each cube:

- skip it if its type is `None`
- otherwise inspect all 6 neighbouring directions
- increment the total whenever:
  - the neighbour is outside the volume
  - or the neighbour type is `None`

Because only outside-reachable air was turned into `None`, this counts only faces exposed to the exterior.

Internal trapped air pockets do not contribute to the total.

---

### Why Internal Pockets Are Ignored

A key detail of the implementation is the meaning of the cube types after flood fill:

- `Lava` stays lava
- `Air` becomes trapped internal air
- `None` becomes outside-reachable space

So when the exterior area is counted, only faces next to `None` are included.

That prevents enclosed cavities from inflating the Part 2 result.

---

## 🛠 Implementation Notes

- The entry class is `Day18`
- The main solver class is `BoilingBoulders`
- Lava cube coordinates are stored as `Vector`
- Part 1 checks direct adjacency only
- Part 2 builds a full bounded 3D map
- The bounds are expanded by 1 in each direction
- External air is identified with a flood fill
- `CubeType.None` represents outside-reachable space after processing
- Trapped air pockets remain `CubeType.Air`

---

## 🧪 Behaviour Summary

Given a list of lava cube coordinates:

- the solver parses them into 3D positions
- Part 1 counts all exposed cube faces by checking neighbour occupancy
- Part 2 builds a full surrounding 3D volume
- it flood-fills from outside the droplet
- it marks reachable outer air separately from enclosed air
- it counts only lava faces touching the exterior
- the final result is either total exposed faces or true exterior surface area

---

## 🚀 Key Takeaways

- Good example of 3D neighbour-based surface counting
- Part 1 is a straightforward adjacency calculation
- Part 2 uses flood fill to distinguish outside air from trapped cavities
- The padded bounding box makes the external search reliable
- The cube type system cleanly separates lava, air, and confirmed exterior space

---

## 🔗 References

- https://adventofcode.com/2022/day/18