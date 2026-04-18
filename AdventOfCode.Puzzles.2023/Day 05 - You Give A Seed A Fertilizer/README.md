# 🎄 Advent of Code 2023 - Day 05: You Give A Seed A Fertilizer

## 📜 Puzzle Overview

This puzzle processes an almanac that maps seed numbers through a chain of category conversions.

The input contains:

- a list of seed values
- several mapping tables
- each table converting one category into the next

The mapping chain is:

- seed to soil
- soil to fertilizer
- fertilizer to water
- water to light
- light to temperature
- temperature to humidity
- humidity to location

Part 1 maps each individual seed through the full chain and returns the lowest resulting location.

Part 2 treats the seeds as `(start, range)` pairs and searches across those ranges for the lowest possible location.

---

## 🧩 Part 1

Map every seed through the almanac and return the lowest location value.

### 💡 Approach

- Parse the first line into a list of seed numbers
- Parse the remaining blocks into ordered mapping tables
- For each seed:
  - pass it through every map stage in order
  - if a mapping range matches, translate the value
  - otherwise leave it unchanged for that stage
- Track the minimum final location

---

## 🧩 Part 2

Treat the seed list as seed ranges and search for the lowest resulting location.

### 💡 Approach

- Interpret the seed values as pairs:
  - start
  - range length
- Convert each pair into:
  - start
  - end
  - range
- Walk through each range using a skip value
- For each current seed candidate:
  - pass it through all map stages
  - when a mapping range matches, compute how far that match remains valid
  - use that to skip ahead instead of checking every single seed one by one
- Keep the lowest location found across all ranges

This reduces the amount of brute-force work needed for large seed intervals.

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `You Give A Seed A Fertilizer`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new YouGiveASeedAFertilizer([.. this.Input])`
- calls `LowestSeed()`

For Part 2:

- creates `new YouGiveASeedAFertilizer([.. this.Input])`
- calls `LowestRange()`

---

### `MapType.cs`

This enum defines the ordered conversion stages used by the almanac.

It contains:

- `SeedToSoil`
- `SoilToFertilizer`
- `FertilizerToWater`
- `WaterToLight`
- `LightToTemperature`
- `TemperatureToHumidity`
- `HumidityToLocation`

The enum order is important because the solver walks the maps in this exact sequence.

---

### `YouGiveASeedAFertilizer.cs`

This class contains the full parsing and conversion logic.

It stores:

- `Seeds`
- `Almanac`
- `Times`

`Seeds` contains the parsed seed values.

`Almanac` stores the mapping tables as a list of map groups.

Each mapping row is stored as:

- destination start
- source start
- range length

`Times` is created in the constructor but is not used elsewhere in this implementation.

---

### Parsing the Input

`Parse(List<string> input)` builds the internal seed list and map structure.

It does the following:

- reads the first input line
- splits on:

      seeds:

- parses the space-separated seed numbers into `Seeds`
- removes the header and blank line with:

      input.RemoveRange(0, 2)

Then it parses the mapping sections.

It uses:

- `MapType current = 0`
- `result.Add(new())`

As it reads the remaining lines:

- if the line is empty:
  - move to the next map group
  - add a new list for that stage
- if the line starts with a digit:
  - split it into three `long` values
  - store that row in the current mapping group

So the almanac becomes an ordered list of map tables, where each row is a `long[]`.

---

### Mapping a Single Seed

`SeedToLocation(long seed)` solves the full mapping path for one seed.

It starts with:

- `MapType current = MapType.SeedToSoil`
- `long value = seed`

Then it loops through each map stage.

At each stage:

- find the first row where the current value falls inside the source range
- if found:
  - translate the value into the destination range
- otherwise:
  - keep the value unchanged

The translation logic is:

    value = map[0] + (value - map[1])

where:

- `map[0]` = destination start
- `map[1]` = source start
- `map[2]` = range length

Once all seven map stages have been processed, the final value is returned as the location.

---

### `LowestSeed()`

This method solves Part 1.

It:

- starts with `result = long.MaxValue`
- loops through every seed in `Seeds`
- converts each seed to a location using `SeedToLocation(seed)`
- keeps the smallest location seen

So the silver answer is the minimum location generated from the individual seed list.

---

### Seed Ranges

Part 2 does not treat the seed list as individual values.

Instead, `SeedRanges()` interprets the seed list as pairs.

It returns tuples of:

- `start`
- `end`
- `range`

The method is:

    this.Seeds.Skip(1).Zip(this.Seeds, (a, b) => (b, a + b, a)).Where((x, i) => i % 2 == 0)

So for an input sequence like:

    start1 range1 start2 range2

it produces ranges describing:

- where the seed interval starts
- where it ends
- how large the interval is

The implementation uses `end = start + range`.

---

### `LowestRange()`

This method solves Part 2.

It searches all seed ranges for the lowest location.

For each seed range:

- start at `current = start`
- continue while `current <= end`

For each candidate seed value:

- set `value = current`
- walk through all map groups
- for each matching map row:
  - translate the value
  - compute how far the current source range remains valid
  - shrink `skip` to the smallest safe jump

The important optimisation is:

- once a map row matches, the solver knows the value will continue behaving consistently until that row boundary ends
- it can therefore jump forward by `skip` instead of moving one seed at a time

The skip logic is built around:

    map[1] + map[2] - value

and later adjusted with:

    current += skip

After each step:

- update the minimum location found
- advance `current`
- recompute the remaining safe range window

So the gold answer is the lowest location discovered while scanning the seed intervals with jump-based optimisation.

---

### Mapping Structure

Each map row is stored as a `long[]` of length three:

- index `0` = destination start
- index `1` = source start
- index `2` = range length

A value matches a row when it falls inside:

    source_start <= value < source_start + range_length

For Part 1, matching is checked with:

    value >= x[1] && value <= x[1] + (x[2] - 1)

For Part 2, matching is checked with:

    map[1] <= value && value < map[1] + map[2]

These are equivalent range checks written in slightly different forms.

---

## 🛠 Implementation Notes

- The puzzle title is `You Give A Seed A Fertilizer`
- Seeds are stored as `List<long>`
- Almanac data is stored as `List<List<long[]>>`
- Each map row contains destination start, source start, and range length
- Part 1 maps every seed independently through all seven stages
- Part 2 interprets the seed list as `(start, range)` pairs
- `SeedRanges()` returns tuples of `(start, end, range)`
- `LowestRange()` uses skip logic to avoid checking every seed value individually
- The `Times` list exists in the class but is not used elsewhere in the implementation

---

## 🧪 Behaviour Summary

Given an almanac and a seed list:

- the solver parses the raw seeds
- it builds an ordered collection of conversion maps
- Part 1 converts each seed through the full chain and keeps the smallest location
- Part 2 converts seed pairs into ranges
- those ranges are scanned using jump logic based on mapping boundaries
- the smallest resulting location is returned

So the final result is either:

- the minimum location for the listed seeds
- or the minimum location reachable from the listed seed ranges

---

## 🚀 Key Takeaways

- The solution models the almanac as an ordered sequence of mapping stages
- Part 1 is a direct value-by-value conversion pipeline
- Part 2 improves performance by skipping through stable mapping regions
- Each map row stores enough information to translate source values into destination values
- The enum-based stage order keeps the conversion chain explicit and readable
- The same parsed almanac structure supports both puzzle parts cleanly

---

## 🔗 References

- https://adventofcode.com/2023/day/5