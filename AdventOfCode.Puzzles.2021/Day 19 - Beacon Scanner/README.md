# 🎄 Advent of Code 2021 - Day 19: Beacon Scanner

## 📜 Puzzle Overview

This puzzle works with multiple 3D scanners, each reporting beacon coordinates relative to its own unknown position and rotation.

The input is split into scanner blocks.

Each block begins with a header like:

    --- scanner 0 ---

followed by beacon coordinates such as:

    404,-588,-901
    528,-643,409

Part 1 determines how many unique beacons exist after all scanners are aligned into one global coordinate system.

Part 2 finds the largest Manhattan distance between any two scanner positions after alignment.

The implementation builds scanner objects, locates them relative to each other, then uses their resolved positions to answer both parts.

---

## 🧩 Part 1

Count how many distinct beacons exist once all scanners have been aligned.

### 💡 Approach

- Parse the input into a list of scanner objects
- Treat scanner `0` as the initial known reference scanner
- Try to match unresolved scanners against already-located scanners
- For each potential match:
  - test candidate beacon alignments
  - try all 24 rotations
  - translate the rotated scanner into position
  - confirm whether at least 12 beacons overlap
- Once all scanners are located, gather every mapped beacon point
- Remove duplicates
- Return the total count

---

## 🧩 Part 2

Find the largest Manhattan distance between any two resolved scanner positions.

### 💡 Approach

- Reuse the fully aligned scanners from Part 1
- Compare every scanner position against every other scanner position
- Calculate the distance between each pair
- Track the largest value found
- Return that maximum distance

---

## 🧠 Code Breakdown

### `Day19.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Beacon Scanner`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new BeaconScanner(this.Input)`
- calls `BeaconCount()`

For Part 2:

- creates `new BeaconScanner(this.Input)`
- calls `LargestDistance()`

So both puzzle parts rely on the same resolved scanner list built during construction.

---

### `BeaconScanner.cs`

This class acts as the puzzle wrapper around the aligned scanner set.

It stores:

- `Scanners`

The constructor does:

    this.Scanners = Scanner.Build(input);

So all scanner parsing and alignment happens before either puzzle answer is requested.

It exposes:

- `BeaconCount()`
- `LargestDistance()`

---

### Counting Beacons

`BeaconCount()` returns:

    this.Scanners.SelectMany(x => x.Map()).Distinct().Count();

So the silver answer is produced by:

- mapping every scanner's beacons into global coordinates
- flattening them into one sequence
- removing duplicates
- counting what remains

This means multiple scanners can report the same beacon, but it is counted only once after alignment.

---

### Largest Scanner Distance

`LargestDistance()` compares every scanner against every other scanner.

It:

- loops through all scanner pairs
- skips comparisons where both references are the same scanner
- calculates `scannerA.Point.Distance(scannerB.Point)`
- keeps the largest value seen

So the gold answer is the maximum scanner-to-scanner Manhattan distance after all scanners have been located.

---

### `Scanner.cs`

This class contains the main alignment logic.

It stores:

- `Point`
- `Rotation`
- `Beacons`

A scanner consists of:

- its global position
- its current rotation index
- its locally reported beacon coordinates

The constructor takes:

    Vector point, int rotation, List<Vector<int>> beacons

So each scanner can be cloned, rotated, translated, and tested against another scanner.

---

### Building the Scanner Set

`Build(string[] input)` is the main resolution pipeline.

It begins by parsing the raw input into scanner objects, then uses a queue-based search to locate unresolved scanners.

At a high level it does:

- parse all scanner blocks
- mark the first scanner as already found
- enqueue that first scanner
- remove it from the unresolved list
- while the queue is not empty:
  - dequeue one located scanner
  - try to find each unresolved scanner relative to it
  - when a match is found:
    - add it to the found list
    - enqueue it
    - remove it from unresolved scanners

This lets already-located scanners help resolve the rest.

---

### Parsing the Input

`Parse(List<string> input)` reads the scanner blocks.

It:

- appends an empty string to force the final scanner to flush
- starts a new scanner when a line begins with `"---"`
- adds beacon coordinates for normal data lines
- pushes the current scanner into the result when a blank line is reached

Each beacon line is parsed with:

    new(line.Split(",").ToInt())

So every scanner is built from a list of 3D beacon vectors.

---

### Mapping a Scanner

`Map()` returns the scanner's beacons transformed into global space:

    this.Beacons.Select(this.Transform)

So every beacon is:

- rotated according to the scanner's current rotation
- offset by the scanner's global `Point`

This is the coordinate set used for overlap checks and final beacon counting.

---

### Rotation Handling

`Transform(Vector<int> point)` applies the scanner's current rotation and then adds the scanner position.

The implementation represents the 24 possible orientations by combining two switch blocks:

- one based on `this.Rotation % 6`
- one based on `(this.Rotation / 6) % 4`

This changes the beacon coordinates by permuting axes and flipping signs.

After rotation, the transformed beacon is returned as:

    this.Point + new Vector<int>(x, y, z)

So rotation and translation are combined into one mapping step.

---

### Rotating and Translating Scanners

The class provides two helpers:

- `Rotate()`
- `Translate(Vector<int> translation)`

`Rotate()` increments the scanner's rotation index.

`Translate(...)` returns a new scanner positioned at:

    this.Point + translation

while keeping the same rotation and beacon list.

This makes it easy to generate candidate scanner placements during alignment.

---

### Matching Candidate Beacons

`Matches(Scanner scanner)` tries to identify promising beacon pairs between two scanners.

For each scanner it:

- maps the beacon coordinates
- takes a reduced subset with `TakeCount(11)`
- translates each scanner so one candidate beacon is moved to the origin
- compares the resulting absolute coordinate values

The match test is:

    if (pointsB.Count(d => pointsA.Contains(d)) >= 3 * 12)

Because each 3D point contributes three absolute coordinate values, this acts as a fast pre-check for a possible 12-beacon overlap before the full rotation test is attempted.

---

### Finding a Scanner Alignment

`Find(Scanner scanner)` performs the full alignment attempt against another scanner.

It first caches this scanner's mapped beacons:

    Vector<int>[] beaconsA = this.Map().ToArray();

Then for every candidate pair from `Matches(scanner)`:

- start from the unresolved scanner
- try all 24 rotations
- translate the rotated scanner so the candidate beacons line up
- compare the mapped beacon sets

The decisive check is:

    if (locatedB.Map().Intersect(beaconsA).Count() >= 12)

If that succeeds, the method returns the newly located scanner.

If no orientation and translation works, it returns:

    null

So scanner alignment is based on explicit overlap confirmation after candidate filtering.

---

### Cloning During Search

`Clone()` returns a fresh scanner with the same:

- `Point`
- `Rotation`
- `Beacons`

This is used during rotation search so the implementation can try different orientations without mutating the original candidate scanner in place.

---

## 🛠 Implementation Notes

- `Day19.cs` uses `BeaconCount()` for silver and `LargestDistance()` for gold
- Scanner alignment is performed during `new BeaconScanner(this.Input)`
- The first parsed scanner is treated as the initial located reference
- Scanner rotation is represented by an integer index covering 24 orientations
- `Map()` applies both rotation and translation
- `Matches(...)` acts as a pre-filter before full overlap testing
- Final alignment requires at least 12 intersecting beacon coordinates
- Part 2 uses the resolved scanner `Point` values to measure distance

---

## 🧪 Behaviour Summary

Given a set of scanner reports in 3D space:

- the solver parses each scanner block into a scanner object
- it resolves scanner positions relative to scanner `0`
- candidate overlaps are screened using reduced beacon comparisons
- all 24 scanner rotations are tested when needed
- a scanner is accepted when at least 12 beacons overlap after rotation and translation
- Part 1 counts distinct global beacon positions
- Part 2 finds the largest distance between any two resolved scanners

---

## 🚀 Key Takeaways

- Good example of solving 3D alignment through rotation plus translation search
- The implementation separates quick candidate matching from final overlap validation
- Scanner discovery is incremental, using already-located scanners to resolve the rest
- Beacon counting becomes simple once every scanner shares the same coordinate system
- Part 2 reuses the resolved scanner positions with no extra alignment work

---

## 🔗 References

- https://adventofcode.com/2021/day/19