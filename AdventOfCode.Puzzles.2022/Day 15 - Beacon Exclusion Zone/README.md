# 🎄 Advent of Code 2022 - Day 15: Beacon Exclusion Zone

## 📜 Puzzle Overview

This puzzle works with sensors and beacons laid out on a 2D grid.

Each sensor knows the location of its closest beacon, which defines the sensor's detection range using Manhattan distance.

An input line looks like this:

    Sensor at x=2, y=18: closest beacon is at x=-2, y=15

The solver parses each line into:

- a sensor position
- a beacon position
- a Manhattan distance between them

It stores both sensor and beacon coordinates in a map, and stores each sensor with its detection distance.

Part 1 calculates how many positions on row `y = 2000000` cannot contain a beacon. Part 2 searches for the one valid location outside all sensor ranges and returns its tuning frequency.

---

## 🧩 Part 1

Determine how many positions on row `2000000` cannot contain a beacon.

### 💡 Approach

- Parse every sensor and its nearest beacon
- Compute each sensor's coverage on row `y = 2000000`
- Sweep across the possible x-range
- Skip ahead across covered intervals instead of checking every point one by one
- Subtract any known sensor or beacon positions already stored on that row
- Return the total blocked count

---

## 🧩 Part 2

Find the distress beacon and calculate its tuning frequency.

### 💡 Approach

- Reuse the parsed sensor list
- For each sensor, examine the perimeter exactly one step outside its detection range
- Clamp candidate coordinates to the valid search area `0` to `4000000`
- For each candidate point, check whether it lies outside every sensor's range
- As soon as one valid point is found, return:

    x * 4000000 + y

This avoids brute forcing the entire 4,000,000 by 4,000,000 search space.

---

## 🧠 Code Breakdown

### `Day15.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Beacon Exclusion Zone`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new BeaconExclusionZone(this.Input)`
- Calls `NonBeacon()`

For Part 2:

- Creates `new BeaconExclusionZone(this.Input)`
- Calls `TuningFrequency()`

The gold solution is also marked with `[Slow]`.

---

### `Sensor.cs`

This class models a sensor.

It stores:

- `Point`
- `Distance`

The constructor takes:

- a sensor position
- the Manhattan distance to its closest beacon

So each `Sensor` instance represents a sensor centre plus its full coverage radius.

---

### `BeaconExclusionZone.cs`

This class contains the main puzzle logic.

It stores:

- `Map`
- `Sensors`

The constructor:

- creates the map
- creates the sensor list
- calls `ParseSensors(input)`

---

### Parsing Input

`ParseSensors(string[] input)` loops through every line and calls:

- `ParseSensor(x)`

`ParseSensor(string input)`:

- strips commas and colons
- splits the input into tokens
- extracts sensor coordinates
- extracts beacon coordinates
- stores both in `Map`
- calculates Manhattan distance from sensor to beacon
- creates a `Sensor` object and adds it to `Sensors`

So a line such as:

    Sensor at x=2, y=18: closest beacon is at x=-2, y=15

becomes:

- a sensor point
- a beacon point
- a distance value used for future range checks

---

### Stored Map Data

The map records both:

- beacon positions
- sensor positions

using:

    this.Map.Add(beacon, 1)
    this.Map.Add(sensor, 1)

That lets Part 1 subtract already-known occupied positions from the blocked total on the target row.

---

### Part 1 Target Row

Part 1 is hard-coded to work on:

    y = 2000000

The helper method `GetDistances()` calculates the leftmost x-reach for each sensor on that target row.

This gives the scan bounds used by `NonBeacon()`.

---

### Scanning for Blocked Positions

`NonBeacon()` does not test each point independently in the naive way.

It first computes:

- `min`
- `max`

from the sensor coverage projections on row `2000000`.

Then it loops from `min` to `max` and calls:

- `MaxX(i, min)`

For each current x-position, `MaxX()` finds the furthest x-value still covered by any sensor at that row.

If coverage exists, the solver:

- adds the full covered span to the total
- subtracts any map entries already present on row `2000000`
- jumps `i` forward to the end of that covered segment

So Part 1 effectively processes row coverage as intervals, not isolated points.

---

### `MaxX(long i, long min)`

This helper checks which sensors cover the current x-position on row `2000000`.

It filters sensors where:

    abs(sensorX - i) + abs(sensorY - 2000000) <= distance

Then it computes the furthest reachable x-value for each matching sensor on that row and returns the maximum one.

If no sensor covers the current position, it returns:

    min - 1

That acts as a sentinel meaning "no coverage here".

---

### Part 1 Return Value

`NonBeacon()` returns:

- the total number of blocked positions on row `2000000`

This is the silver answer.

---

### Part 2 Perimeter Search

`TuningFrequency()` searches around each sensor, but not through its whole area.

For each sensor it scans x-values from:

    sensor.Point.X - sensor.Distance - 1

to:

    sensor.Point.X + sensor.Distance + 1

clamped into:

    0 .. 4000000

For each x-value it computes two y-values:

- `positiveY`
- `negativeY`

These represent the upper and lower points on the diamond-shaped perimeter exactly one step outside the sensor's exclusion zone.

So instead of checking every point in the full search space, it only checks the perimeter where the valid beacon is most likely to exist.

---

### Valid Candidate Check

For each perimeter candidate, the solver tests:

    this.Sensors.All(sensor => distanceToCandidate > sensor.Distance)

If that condition is true, the point lies outside every sensor's range and is therefore the valid distress beacon location.

The method returns immediately when the first such point is found.

If no point is found, it throws:

    InvalidOperationException

---

### Tuning Frequency Formula

When a valid point is found, the solver returns:

    (x * 4000000) + y

This is the gold answer.

---

## 🛠 Implementation Notes

- The solver uses Manhattan distance throughout
- Sensors store only their point and coverage distance
- Both sensors and beacons are recorded in the map
- Part 1 works specifically on row `2000000`
- Part 1 skips across contiguous covered intervals for efficiency
- Part 2 searches only the perimeter just outside each sensor range
- Candidate coordinates are clamped to `0` through `4000000`
- The gold solution is marked as slow, but is still much better than brute force

---

## 🧪 Behaviour Summary

Given a list of sensors and their nearest beacons:

- the solver parses sensor and beacon coordinates
- computes each sensor's Manhattan coverage distance
- Part 1 scans row `2000000` and totals covered non-beacon positions
- Part 2 checks perimeter candidates just outside sensor ranges
- the first point outside every sensor range is used to compute the tuning frequency

---

## 🚀 Key Takeaways

- Good example of solving a grid problem with Manhattan geometry
- Sensor coverage is treated as diamond-shaped exclusion areas
- Part 1 is optimised by scanning intervals rather than single points
- Part 2 avoids brute force by checking only sensor perimeters
- The implementation keeps the model simple with a lightweight `Sensor` class

---

## 🔗 References

- https://adventofcode.com/2022/day/15