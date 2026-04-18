# 🎄 Advent of Code 2019 - Day 12: The N-Body Problem

## 📜 Puzzle Overview

This puzzle simulates moons moving in 3D space under gravity.

Each input line describes a moon's starting position in this format:

    <x=-1, y=0, z=2>

The solver parses each line into a `Moon` object with:

- `Location`
- `Velocity`

Every moon starts with zero velocity:

    <0, 0, 0>

Part 1 simulates a fixed number of steps and returns the total energy in the system.

Part 2 keeps simulating until the system repeats, then returns the cycle length.

---

## 🧩 Part 1

Simulate the moons for 1000 steps and calculate the total energy.

### 💡 Approach

- Parse each input line into a moon with a 3D location
- Initialise every moon's velocity to zero
- Repeatedly apply gravity
- Then apply velocity
- After 1000 steps, calculate each moon's:
  - potential energy from its position
  - kinetic energy from its velocity
- Multiply those two values per moon
- Sum the results across all moons

---

## 🧩 Part 2

Find how many steps it takes for the moon system to repeat.

### 💡 Approach

- Reuse the same moon simulation
- Continue stepping indefinitely
- Track when all moon velocities on each axis return to zero:
  - `X`
  - `Y`
  - `Z`
- Record the first step where each axis reaches that state
- Compute the least common multiple of those three step counts
- Double the result to get the full repeat cycle used by this implementation

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title to `The N-Body Problem`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new TheNBodyProblem(this.Input)`
- Calls `Simulate(1000)`

For Part 2:

- Creates `new TheNBodyProblem(this.Input)`
- Calls `Simulate()`

---

### `Moon.cs`

This class models a single moon.

It stores:

- `Location`
- `Velocity`

The constructor:

- removes the angle brackets
- splits the line on `", "`
- extracts the numeric values from `x=`, `y=`, and `z=`
- creates a `Vector` for the location
- initialises velocity to `new(0, 0, 0)`

So a line such as:

    <x=-1, y=0, z=2>

becomes a moon with a starting location and zero velocity.

The class also provides:

- `PotentialEnergy()`
- `KineticEnergy()`

These both use `Absolute()` on the underlying vector.

---

### `TheNBodyProblem.cs`

This class contains the full simulation logic.

It stores:

- `Moons`

The constructor parses the full input with:

- `Parse(input)`

which builds:

- `List<Moon>`

It exposes one main method:

- `Simulate(int steps = -1)`

That single method supports both puzzle parts.

---

### Parsing the Moons

`Parse(string[] input)` creates one `Moon` per input line.

At a high level it does:

- read each line
- pass it into `new Moon(line)`
- collect the moons into a list

This gives the simulation a working collection of bodies with positions and velocities.

---

### Main Simulation Loop

`Simulate(int steps = -1)` runs the whole motion system.

It starts with:

    int step = 0;
    Vector<long> point = new(-1, -1, -1);

Then it loops forever and on each iteration:

- applies gravity
- applies velocity
- increments the step counter

If a fixed step count was provided and the current step matches it, the method returns total energy.

So Part 1 is simply the same simulation loop with a stopping point at step `1000`.

---

### Applying Gravity

`ApplyGravity()` compares every moon against every other moon.

For each pair of moons, it checks each axis independently:

- if one moon's coordinate is greater, its velocity on that axis is decreased
- the other moon's velocity on that axis is increased

This is done for:

- `X`
- `Y`
- `Z`

The method updates both moons immediately during the comparison.

Because the implementation loops over every ordered pair of moons, each moon pair is encountered twice during a full gravity pass.

---

### Applying Velocity

After gravity has adjusted the velocities, `ApplyVelocity()` moves each moon with:

    this.Moons.ForEach(x => x.Location += x.Velocity);

So every step is:

- update velocities from gravity
- then update positions from velocity

---

### Total Energy

`TotalEnergy()` calculates the system energy with:

    this.Moons.Sum(c => c.PotentialEnergy() * c.KineticEnergy())

For each moon:

- potential energy comes from the absolute position values
- kinetic energy comes from the absolute velocity values

The total is the sum of all moon energy products.

---

### Part 2 Axis Tracking

When `Simulate()` is called without a step limit, it searches for the repeat cycle.

It records the first step where all moon velocities are zero on each axis:

    if (point.X == -1 && this.Moons.All(c => c.Velocity.X == 0))
    if (point.Y == -1 && this.Moons.All(c => c.Velocity.Y == 0))
    if (point.Z == -1 && this.Moons.All(c => c.Velocity.Z == 0))

Once all three axis values have been captured, it returns:

    MathHelper.LeastCommonMultiple(point.ToList3D()) * 2

So this implementation treats the first all-zero-velocity point on each axis as the half-cycle marker, then doubles the least common multiple to produce the final repeat length.

---

### Optional Debug Output

The class also contains `PrintStep(int step)`.

It prints:

- the current step number
- every moon's position
- every moon's velocity

This helper is present for inspection and debugging, though it is not part of the returned answers.

---

## 🛠 Implementation Notes

- The solver uses a dedicated `Moon` class for state
- Positions and velocities are both stored as vectors
- Every moon begins with zero velocity
- Part 1 uses `Simulate(1000)`
- Part 2 uses `Simulate()` with no step limit
- Cycle detection is axis-based
- The final cycle result is `LCM(x, y, z) * 2`
- Gravity updates both moons during each comparison pass

---

## 🧪 Behaviour Summary

Given a list of moon positions in 3D space:

- the solver parses them into moon objects
- each simulation step adjusts velocity from gravity
- then moves each moon by its velocity
- Part 1 stops after 1000 steps and returns total energy
- Part 2 watches for the first zero-velocity state on each axis
- the final result is either the system energy or the repeat cycle length

---

## 🚀 Key Takeaways

- Good example of modelling moving bodies with separate position and velocity vectors
- Part 1 and Part 2 share the same simulation core
- Energy is calculated cleanly from absolute position and velocity magnitudes
- Part 2 reduces the repeat search to independent axis timings
- The implementation derives the final cycle by doubling the axis LCM result

---

## 🔗 References

- https://adventofcode.com/2019/day/12