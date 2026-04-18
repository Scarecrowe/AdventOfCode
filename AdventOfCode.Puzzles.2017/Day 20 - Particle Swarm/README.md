# 🎄 Advent of Code 2017 - Day 20: Particle Swarm

## 📜 Puzzle Overview

This puzzle simulates a swarm of particles moving in 3D space.

Each input line defines a particle with:

- position
- velocity
- acceleration

An input line looks like this:

    p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>

The solver parses each line into a `Particle` object and gives each particle its original input index as a `Number`.

Part 1 repeatedly advances the swarm and tracks which particle stays closest to the origin most often. Part 2 runs the same movement logic but removes particles that collide, then returns how many remain.

---

## 🧩 Part 1

Determine which particle stays closest to the origin in the long term.

### 💡 Approach

- Parse every line into a particle with position, velocity, and acceleration
- Simulate particle movement repeatedly
- After each tick, find which particle is currently closest to the origin
- Count how often each particle is the closest
- Return the particle number with the highest count after the simulation window completes

---

## 🧩 Part 2

Determine how many particles remain after collisions are removed during the simulation.

### 💡 Approach

- Reuse the same particle simulation
- After each tick, compare all particle positions
- Collect particles that share a position with any other particle
- Remove those collided particles from the swarm
- After the simulation window completes, return the number of remaining particles

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Particle Swarm`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ParticleSwam(this.Input)`
- Calls `Run()`

For Part 2:

- Creates `new ParticleSwam(this.Input)`
- Calls `Run(true)`

---

### `Particle.cs`

This class models a single particle.

It stores:

- `Number`
- `Position`
- `Velocity`
- `Acceleration`

The constructor:

- stores the particle index as `Number`
- splits the input line on `", "`
- parses the `p=<...>`, `v=<...>`, and `a=<...>` sections into 3D vectors

So a line such as:

    p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>

becomes one fully initialised particle instance.

---

### Particle Movement

`Move()` updates a particle in two steps:

- add acceleration to velocity
- add velocity to position

That means each tick does this logically:

    velocity += acceleration
    position += velocity

So acceleration influences future motion through velocity, and velocity then shifts the particle's position.

---

### Distance from the Origin

`Distance()` returns the particle's current distance metric from the origin by calling:

    this.Position.Absolute()

This is the value used when deciding which particle is currently closest during the Part 1 simulation.

---

### `ParticleSwam.cs`

This class contains the full swarm simulation logic.

It stores:

- `Particles`

The constructor parses the input with:

- `Parse(input)`

which builds:

- `List<Particle>`

---

### Parsing the Swarm

`Parse(string[] input)` creates one `Particle` per input line and preserves the original line index as that particle's identifier.

At a high level it does:

- read each line
- pass the line and its index into `new Particle(line, i)`
- collect the results into a list

That index is later returned for Part 1 when identifying the winning particle.

---

### Main Simulation Loop

`Run(bool removeCollisions = false)` performs the full simulation.

It creates:

- `Dictionary<int, int> closest = new();`

Then loops for:

    500

iterations.

On each iteration it:

- moves every particle once
- optionally removes collisions
- finds the currently closest particle
- increments that particle's count in the `closest` dictionary

So Part 1 is based on repeated closest-particle sampling across the simulation window rather than just checking once at the end.

---

### Finding the Closest Particle

After movement, the solver picks the nearest particle with:

    this.Particles.Aggregate((a, b) => a.Distance() < b.Distance() ? a : b)

It then uses that particle's `Number` as the key in the `closest` dictionary.

If the particle has not been seen before, it is added with count `0`, then incremented.

At the end of the run, Part 1 returns the particle number with the highest accumulated closest count.

---

### Collision Removal

When `removeCollisions` is `true`, the solver checks every particle against every other particle.

It creates:

- `List<Particle> remove = new();`

Then for every pair of indices `j` and `k`:

- skip when `j == k`
- compare `this.Particles[j].Position` and `this.Particles[k].Position`
- if the positions match, add that particle to the removal list if it is not already there

Once all comparisons are complete, every particle collected in `remove` is deleted from the swarm.

This means any particle involved in a position clash is removed before the next iteration begins.

---

### Part 1 Return Value

When collisions are not being removed, the method returns:

- the particle number with the highest closest-count total

This is selected with:

    closest.Aggregate((a, b) => a.Value > b.Value ? a : b).Key

So the silver answer is the particle that most consistently appears nearest to the origin during the 500-step run.

---

### Part 2 Return Value

When collision removal is enabled, the method instead returns:

- `this.Particles.Count`

So the gold answer is simply the number of particles left after the 500-step simulation with collision cleanup enabled.

---

## 🛠 Implementation Notes

- The class is named `ParticleSwam` in the implementation
- Particles are identified by their original input index
- Movement updates velocity first, then position
- Distance is measured from the current position using `Absolute()`
- Part 1 tracks which particle is closest most often across 500 iterations
- Part 2 uses a nested comparison loop to detect collisions
- Collided particles are removed after each tick

---

## 🧪 Behaviour Summary

Given a list of particles in 3D space:

- the solver parses them into particle objects
- each tick updates velocity and then position
- Part 1 repeatedly records which particle is nearest to the origin
- Part 2 repeatedly removes particles that occupy the same position
- both modes run for 500 simulation steps
- the final result is either the most frequently closest particle number or the number of surviving particles

---

## 🚀 Key Takeaways

- Good example of modelling motion with position, velocity, and acceleration vectors
- Particle state is cleanly encapsulated in a dedicated class
- Part 1 uses repeated sampling to identify the long-term closest particle
- Part 2 removes collisions by comparing particle positions after each move
- The same simulation loop supports both puzzle parts with a mode flag

---

## 🔗 References

- https://adventofcode.com/2017/day/20