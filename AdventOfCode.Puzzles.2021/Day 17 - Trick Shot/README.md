# 🎄 Advent of Code 2021 - Day 17: Trick Shot

## 📜 Puzzle Overview

This puzzle simulates firing a probe toward a rectangular target area.

The input is a single line describing the target bounds.

Example input:

    target area: x=20..30, y=-10..-5

The probe starts at:

    0,0

Each shot uses an initial velocity.

On every simulation step:

- the probe moves by its current velocity
- horizontal velocity is reduced toward zero
- vertical velocity decreases by `1`

Part 1 finds the highest reachable shot that still lands in the target area.

Part 2 counts how many initial velocities hit the target at all.

---

## 🧩 Part 1

Find the highest Y position reached by any successful shot.

### 💡 Approach

- Parse the target area from the input line
- Try a wide range of possible initial velocities
- Simulate each shot step by step
- Track the highest Y position reached during that shot
- If the shot lands inside the target, record it
- Return the largest successful height

---

## 🧩 Part 2

Count how many distinct initial velocities hit the target area.

### 💡 Approach

- Reuse the same parsed target bounds
- Simulate many candidate X and Y velocity pairs
- Record every velocity that reaches the target
- Count the total number of successful shots
- Return that count

---

## 🧠 Code Breakdown

### `Day17.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Trick Shot`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new TrickShot(this.Input)`
- calls `Simulate(true)`

For Part 2:

- creates `new TrickShot(this.Input)`
- calls `Simulate(false)`

So both puzzle parts use the same simulation code, with a mode flag deciding which result to return.

---

### `TrickShot.cs`

This class contains the full target parsing and projectile simulation logic.

It stores:

- `Min`
- `Max`

These two vectors represent the target bounds after parsing.

The constructor begins with:

    this.Min = new(0, 0);
    this.Max = new(0, 0);

Then it calls:

    this.Parse(input);

So the object is initialised entirely from the single target-area input line.

---

### Parsing the Target Area

`Parse(string[] input)` reads the target description from:

    input[0]

It removes the prefix:

    target area:

then splits the remaining string into X and Y ranges.

At a high level it does:

- split on `", "`
- remove `"x="` and `"y="`
- split each range on `"..""`
- convert the values to integers

The final bounds are stored as:

    this.Min = new(tokensX.Min(), tokensY.Max());
    this.Max = new(tokensX.Max(), tokensY.Min());

So in this implementation:

- `Min.X` is the left edge
- `Max.X` is the right edge
- `Min.Y` is the upper Y bound
- `Max.Y` is the lower Y bound

---

### Firing a Single Probe

`Fire(Vector<int> velocity)` simulates one initial velocity.

It begins with:

- `current = new(0, 0)`
- `step = new(0, velocity.Y)`
- `highest = 0`

Then it repeatedly updates the current probe position until the shot is clearly out of range.

The method returns:

- the velocity if the probe hits the target
- `new(0, 0)` if it misses
- the highest point reached during that shot

---

### Position Update Logic

Inside the loop, the probe position is updated with:

    current.X += velocity.X - step.X;
    current.Y -= step.Y;

Then vertical motion is advanced with:

    step.Y--;

This implementation tracks horizontal slowdown through `step.X` and vertical fall through `step.Y`.

So each loop step moves the probe first, then updates the step state for the next iteration.

---

### Tracking the Highest Point

The method records the highest successful arc using:

    if (current.Y < highest)
        highest = current.Y;

Because this implementation treats upward movement with decreasing Y values, the most negative `current.Y` represents the highest physical point.

That is why the Part 1 result later multiplies by `-1` before returning it.

---

### Hit Detection

A shot is considered successful when the probe enters the target area.

The check is:

    current.Y >= this.Min.Y * -1 && current.Y <= this.Max.Y * -1
    current.X >= this.Min.X && current.X <= this.Max.X

If that succeeds, the method returns:

    (velocity, highest)

So each successful shot records both:

- which initial velocity worked
- how high that shot travelled

---

### Early Miss Detection

The method abandons a shot when it has clearly gone past the target.

That check is:

    if (current.Y > this.Max.Y * -1 || current.X > this.Max.X)
        return (new(0, 0), 0);

So a shot is considered lost once it goes too far horizontally or too far below the usable vertical range.

---

### Horizontal Drag Handling

Horizontal slowdown is handled with:

    if (step.X < velocity.X)
        step.X++;

This means the X movement contribution shrinks by `1` each step until it reaches zero.

So the implementation models drag by increasing `step.X` toward the original X velocity and subtracting that offset from the horizontal movement term.

---

### Trying Many Velocities

`Simulate(bool highest)` tests a fixed search range of candidate velocities.

It loops through:

    for (int x = 0; x < 200; x++)
    for (int y = -150; y < 150; y++)

For each pair it calls:

    this.Fire(new(x, y));

If the returned point is not:

    new Vector<int>(0, 0)

then that velocity is counted as a hit and added to the `hits` list.

So the solver uses brute-force search over a hard-coded velocity window.

---

### Final Return Values

At the end of `Simulate(bool highest)`, the method returns:

    highest ? hits.Min(x => x.Highest) * -1 : hits.Count();

So:

- Part 1 finds the best height from all successful shots
- Part 2 returns the number of successful velocity pairs

Because stored heights are negative during upward travel, Part 1 converts the result back to a positive height by multiplying by `-1`.

---

## 🛠 Implementation Notes

- `Day17.cs` uses `Simulate(true)` for silver and `Simulate(false)` for gold
- The target area is parsed from a single input line
- Successful shots are collected in a `List<(Vector<int> Point, long Highest)>`
- The brute-force search tries X velocities from `0` to `199`
- The brute-force search tries Y velocities from `-150` to `149`
- Misses return `new(0, 0)` as a sentinel value
- The implementation stores upward height as a negative Y value, then flips it for the Part 1 answer

---

## 🧪 Behaviour Summary

Given a rectangular target area:

- the solver parses the X and Y bounds
- it tests many possible starting velocities
- each velocity is simulated step by step
- successful shots are recorded when the probe enters the target
- Part 1 returns the highest successful arc
- Part 2 returns the number of successful initial velocities
- both puzzle parts share the same brute-force simulation pipeline

---

## 🚀 Key Takeaways

- Good example of solving projectile motion with direct simulation
- The implementation uses brute-force velocity search rather than closed-form maths
- A single `Fire(...)` method handles both hit detection and peak tracking
- Part 1 and Part 2 reuse the same result set of successful shots
- Sentinel return values are used to distinguish hits from misses

---

## 🔗 References

- https://adventofcode.com/2021/day/17