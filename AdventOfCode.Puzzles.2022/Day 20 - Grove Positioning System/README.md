# 🎄 Advent of Code 2022 - Day 20: Grove Positioning System

## 📜 Puzzle Overview

This puzzle works with a sequence of numbers that must be "mixed" by moving each value forward or backward through a circular list.

The solver parses the input into `GroveCoordinate` objects and links them into a doubly linked circular structure.

Part 1 performs one mixing cycle, then finds the grove coordinates relative to the value `0`. Part 2 first applies a decryption key to every value, performs ten mixing cycles, and then calculates the same coordinate sum.

---

## 🧩 Part 1

Determine the sum of the grove coordinates after one full mixing cycle.

### 💡 Approach

- Parse each input line into a `GroveCoordinate`
- Link all coordinates into a circular doubly linked list
- For each coordinate in original input order:
  - remove it temporarily from the ring
  - move forward or backward based on its value
  - insert it back into its new position
- Rebuild the list starting from the coordinate with value `0`
- Sum the values at offsets:

      1000, 2000, 3000

  wrapping around the circular list as needed

---

## 🧩 Part 2

Determine the sum of the grove coordinates after decrypting and mixing ten times.

### 💡 Approach

- Reuse the same linked list structure
- Multiply every coordinate value by the decryption key:

      811589153

- Run the same mixing logic 10 times
- Rebuild the list starting from the value `0`
- Sum the values at offsets:

      1000, 2000, 3000

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Grove Positioning System`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new GrovePositioningSystem(this.Input)`
- Calls `Cycle().Sum()`

For Part 2:

- Creates `new GrovePositioningSystem(this.Input)`
- Calls `Decrypt().Sum()`

---

### `GroveCoordinate.cs`

This class models one coordinate in the circular list.

It stores:

- `Value`
- `Next`
- `Previous`

The constructor:

- accepts the numeric value

It also provides:

- `SetPrevious(...)`
- `SetNext(...)`
- `Decrypt(long key)`

`Decrypt(...)` multiplies the coordinate value by the supplied key.

---

### `GrovePositioningSystem.cs`

This class contains the full mixing logic.

It stores:

- `Coordinates`
- `DecryptionKey`

The constructor:

- accepts the input
- builds the coordinate list with:

      BuildCoordinates(input)

The decryption key is stored as:

    811589153

---

### Building the Circular List

`BuildCoordinates(string[] input)` parses the input values into:

- `List<GroveCoordinate>`

At a high level it does:

- convert each input line into a `long`
- create a `GroveCoordinate` for each value
- link each item to its previous and next neighbour
- connect the first and last items together

So the final structure is a circular doubly linked list.

---

### Mixing the Coordinates

`Cycle()` performs one full mixing pass.

It processes every coordinate in the original `Coordinates` list order.

For each coordinate it:

- detaches the coordinate from the circular list
- stores its current neighbours
- calculates how many positions to move using:

      abs(value) % (count - 1)

- moves backward when the value is negative
- moves forward when the value is positive
- reinserts the coordinate between the newly selected `previous` and `next` nodes

This means each number shifts through the ring by its own value, with wrapping handled by the circular links.

---

### Why the Modulo Is Used

The movement count is reduced with:

    value.Abs() % (this.Coordinates.Count - 1)

This avoids unnecessary full loops around the circular structure.

Because one item is temporarily removed before reinsertion, the effective movement range is based on:

    count - 1

rather than the full list length.

---

### Re-Linking During a Move

For each coordinate, the solver first removes it logically by reconnecting its neighbours:

- previous points to next
- next points to previous

Then after the movement step, it inserts the coordinate back by updating four links:

- `previous.Next`
- `coordinate.Previous`
- `next.Previous`
- `coordinate.Next`

This keeps the circular chain intact throughout the mix.

---

### Sorting from Zero

`Sort()` rebuilds the `Coordinates` list into traversal order starting at the coordinate whose value is:

    0

It creates a new list beginning with that node, then follows `Next` until it loops back to the same starting point.

This gives a stable ordered view of the current circular arrangement for the final sum calculation.

---

### Grove Coordinate Sum

`Sum()` first calls:

    this.Sort()

Then it adds the values at positions:

- `1000`
- `2000`
- `3000`

using wraparound indexing:

    index % this.Coordinates.Count

So the solver finds the values 1000, 2000, and 3000 steps after `0` in the mixed list and returns their total.

---

### Part 2 Decryption

`Decrypt()` performs the Part 2 setup.

It first multiplies every coordinate value by `DecryptionKey` by calling:

    coordinate.Decrypt(this.DecryptionKey)

Then it runs:

    this.Cycle()

ten times.

After that, the list is ready for the same final coordinate sum logic used in Part 1.

---

## 🛠 Implementation Notes

- The entry class is `Day20`
- The main solver class is `GrovePositioningSystem`
- The node class is `GroveCoordinate`
- The input is stored as a circular doubly linked list
- Coordinates are mixed in original input order
- Movement distance is reduced with modulo `count - 1`
- `Sort()` rebuilds the list starting from the value `0`
- Part 1 performs one cycle
- Part 2 multiplies values by `811589153` and performs ten cycles

---

## 🧪 Behaviour Summary

Given a list of numbers:

- the solver parses them into linked coordinate nodes
- it connects them into a circular list
- each coordinate is moved through the ring based on its value
- Part 1 performs one full mixing pass
- Part 2 first decrypts all values, then mixes ten times
- the final arrangement is read starting from `0`
- the answer is the sum of the values 1000, 2000, and 3000 steps after zero

---

## 🚀 Key Takeaways

- Good example of solving a reordering puzzle with linked-list style structures
- Circular links make wraparound movement natural
- Reusing original node order during mixing preserves puzzle rules
- Part 2 is built cleanly by adding a decryption step and repeated cycles
- The final coordinate lookup becomes simple once the list is reordered from `0`

---

## 🔗 References

- https://adventofcode.com/2022/day/20