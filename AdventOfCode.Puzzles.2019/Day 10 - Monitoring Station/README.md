# 🎄 Advent of Code 2019 - Day 10: Monitoring Station

## 📜 Puzzle Overview

This puzzle works with a 2D asteroid field made of `#` and `.` characters.

The goal is to analyse which asteroid can detect the most other asteroids using direct line of sight.

In Part 1, the solver finds the best location for the monitoring station.

In Part 2, it places the laser at that best location, simulates vaporising asteroids in rotational order, and returns a score based on the 200th asteroid destroyed.

---

## 🧩 Part 1

Determine which asteroid can see the highest number of other asteroids.

### 💡 Approach

- Parse the map into a list of asteroid coordinates
- For each asteroid:
  - compare it against every other asteroid
  - check whether another asteroid blocks the line of sight
- Count how many targets remain visible
- Keep track of:
  - the best asteroid location
  - the highest visible asteroid count

---

## 🧩 Part 2

Simulate the laser from the best station location and find the 200th asteroid vaporised.

### 💡 Approach

- Reuse the best location found in Part 1
- Remove the station asteroid from the asteroid list
- Split all remaining asteroids into:
  - those on or to the right of the station
  - those to the left of the station
- Map every asteroid to a slope relative to the station
- Repeatedly:
  - sort by slope
  - for matching slopes, take the nearest asteroid first
  - remove one asteroid per visible slope
- Continue until the 200th asteroid is vaporised
- Return:

    (x * 100) + y

---

## 🧠 Code Breakdown

### `Day10.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monitoring Station`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new MonitoringStation(this.Input)`
- Calls `FindBestLocation()`
- Returns `MaxTargets`

For Part 2:

- Creates `new MonitoringStation(this.Input)`
- Calls `FindBestLocation()`
- Calls `ClearAsteroidField()`
- Calls `VaporizedScore()`

---

### `Entity.cs`

This file defines the map entity enum.

It contains:

- `Empty`
- `Asteroid`

Although the solver parses directly from `#` characters, this enum still represents the two possible map cell types for the puzzle.

---

### `MonitoringStation.cs`

This class contains the full asteroid field logic.

It stores:

- `Asteroids`
- `MaxTargets`
- `BestLocation`
- `VaporizedLocation`

The constructor:

- parses the input into asteroid coordinates
- initialises `BestLocation`
- initialises `VaporizedLocation`

---

### Parsing the Asteroid Map

`Parse(string[] input)` builds the asteroid list by enumerating every coordinate in the grid.

At a high level it does this:

- iterate over all `(x, y)` points
- check whether the map character at that point is `#`
- collect those points into a list of asteroid vectors

So the map is converted into:

- `List<Vector>`

containing only asteroid positions.

---

### Finding the Best Station Location

`FindBestLocation()` checks every asteroid as a possible station.

For each candidate asteroid A:

- compare it with every other asteroid B
- skip when A and B are the same asteroid
- scan all asteroids again to see whether any asteroid C blocks line of sight
- if no blocker exists, count asteroid B as visible

This means visibility is determined by repeatedly checking:

- source asteroid
- target asteroid
- possible blocking asteroid

If the visible count for the current asteroid is greater than the current best:

- update `MaxTargets`
- update `BestLocation`

The method returns the current `MonitoringStation` instance so the calls can be chained.

---

### Line of Sight Detection

The visibility test relies on:

    asteroidA.IsLineOfSight(asteroidB, asteroidC)

This is used to determine whether asteroid C lies directly between asteroid A and asteroid B.

If so:

- asteroid B is blocked from asteroid A
- it is not counted as visible

So Part 1 is effectively a triple nested comparison over all asteroid positions.

---

### Clearing the Asteroid Field

`ClearAsteroidField()` performs the Part 2 setup and laser simulation.

It first removes the monitoring station asteroid from the asteroid list:

    this.Asteroids.Remove(this.BestLocation);

Then it maps the remaining asteroids into two slope dictionaries:

- `slopeMappingsRight`
- `slopeMappingsLeft`

After that it calls:

    this.BlastAsteroids(...)

---

### Mapping Asteroids to Slopes

`MapPointsToSlopes(...)` splits asteroids based on whether they are to the right of, or left of, the station.

The split is:

- right side when `asteroid.X >= origin.X`
- left side otherwise

Each asteroid is stored with a slope value relative to the station.

The slope is calculated by:

    (asteroid.Y - origin.Y) / (float)(asteroid.X - origin.X)

Vertical lines are handled specially:

- above or below the origin use extreme float values
- this avoids division by zero and keeps vertical directions sortable

---

### Laser Sweep Logic

`BlastAsteroids(...)` performs repeated rotational passes.

It keeps a running vaporisation count:

    int count = 0;

For each pass it processes:

1. all right-side slope groups
2. all left-side slope groups

For each side it:

- orders asteroids by slope
- then orders equal-slope asteroids by distance from the station
- groups by slope
- selects only the first asteroid from each slope group

That means during one sweep:

- only the nearest visible asteroid on each slope is vaporised
- blocked asteroids on the same slope wait for later passes

Each selected asteroid is then removed from its slope dictionary.

When the count reaches `200`:

- `VaporizedLocation` is set
- the simulation stops

---

### Vaporized Score

`VaporizedScore()` returns:

    (this.VaporizedLocation.X * 100) + this.VaporizedLocation.Y

So the gold answer is based on the coordinate of the 200th vaporised asteroid.

---

## 🛠 Implementation Notes

- Asteroids are stored as `Vector` coordinates
- Parsing keeps only `#` positions from the input grid
- Part 1 uses repeated line-of-sight checks for every asteroid pair
- `FindBestLocation()` updates both the best coordinate and the visible target count
- Part 2 removes the station asteroid before vaporisation begins
- Asteroids are split into right and left slope maps
- Within each sweep, only the nearest asteroid per slope is removed
- The final score is based on the 200th vaporised asteroid position

---

## 🧪 Behaviour Summary

Given a 2D asteroid map:

- the solver parses all asteroid coordinates
- Part 1 checks visibility from every asteroid
- the asteroid with the largest visible count becomes the station
- Part 2 simulates repeated laser sweeps from that station
- asteroids are removed in slope order, nearest first per direction
- the final result is either:
  - the highest visible asteroid count
  - or the score of the 200th vaporised asteroid

---

## 🚀 Key Takeaways

- Good example of solving visibility using geometric line-of-sight checks
- The best station location is found by comparing every asteroid against every other
- Part 2 converts asteroid positions into ordered slope groups for laser rotation
- Distance sorting ensures the nearest visible asteroid is removed first
- The solution reuses the Part 1 result directly for the Part 2 simulation

---

## 🔗 References

- https://adventofcode.com/2019/day/10