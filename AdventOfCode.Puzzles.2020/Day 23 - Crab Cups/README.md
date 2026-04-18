# 🎄 Advent of Code 2020 - Day 23: Crab Cups

## 📜 Puzzle Overview

This puzzle simulates a circular arrangement of numbered cups.

Each move:

- picks up the three cups immediately clockwise of the current cup
- selects a destination cup by counting down
- inserts the picked-up cups immediately clockwise of the destination
- advances to the next current cup

The solver models the cups as a circular linked list of `Cup` objects and also keeps a dictionary that maps cup values directly to their node instances for fast lookup.

Part 1 runs 100 moves on the original input. Part 2 expands the cup circle to 1,000,000 cups and runs 10,000,000 moves.

---

## 🧩 Part 1

Run the short cup game and return the cup labels clockwise after cup `1`.

### 💡 Approach

- Build a circular linked list from the input digits
- Keep a dictionary from cup value to cup node
- Run 100 moves
- Find cup `1`
- Return the clockwise labels after it as a single string

---

## 🧩 Part 2

Run the extended cup game and return the product of the two cups clockwise of cup `1`.

### 💡 Approach

- Build the same circular linked list from the input
- Extend it with consecutive cup values up to `1,000,000`
- Run `10,000,000` moves
- Find cup `1`
- Multiply the values of the next two cups

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Crab Cups`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `CrabCups.Short(this.Input)`

For Part 2:

- Calls `CrabCups.Long(this.Input)`

---

### `CrabCups.cs`

This class contains the full game logic.

It provides:

- `Short(string[] input)`
- `Long(string[] input)`
- `BuildCups(string input, int extras = 0)`
- `Play(Cup? current, int moves, Dictionary map, int max)`

The implementation is built around:

- a circular linked list for cup order
- a dictionary for constant-time cup lookup by value

---

### `Short(...)`

This method solves Part 1.

It:

- builds the cup structure from `input[0]`
- runs the game for `100` moves
- returns the cup ordering after cup `1`

It gets its data from:

- `BuildCups(input[0])`

Then executes:

- `Play(cups.Item1, 100, cups.Item3, 9)`

Finally it returns:

- `cups.Item2?.ToString() ?? string.Empty`

So the silver answer is the clockwise label string starting immediately after cup `1`.

---

### `Long(...)`

This method solves Part 2.

It:

- builds the cup structure from `input[0]`
- appends extra cups until the total reaches `1,000,000`
- runs the game for `10,000,000` moves
- multiplies the two cups after cup `1`

It does this with:

- `BuildCups(input[0], 1000000 - input[0].Length)`
- `Play(cups.Item1, 10000000, cups.Item3, 1000000)`

Then returns:

    (cups.Item2?.Next?.Value ?? 0) * (cups.Item2?.Next?.Next?.Value ?? 0)

So the gold answer is the product of the two cups immediately clockwise of cup `1`.

---

### `BuildCups(...)`

This method creates the circular linked list and lookup map.

It returns a tuple containing:

- the starting cup
- the cup whose value is `1`
- the value-to-cup dictionary

At a high level it does:

- convert the input string into characters
- create one `Cup` node per digit
- link each cup to the next cup
- store each cup in the dictionary by its value
- optionally append extra numbered cups
- connect the final cup back to the start

So the cups become one continuous circular structure.

---

### Tracking Cup `1`

During construction, the solver keeps a reference to the cup whose value is `1`.

It stores this as:

- `cupOne`

This is important because:

- Part 1 needs to print the cups after `1`
- Part 2 needs the two cups after `1`

So the solver does not need to search for cup `1` after the game is complete.

---

### Extra Cups for Part 2

When `extras > 0`, the builder appends sequential cup values starting at:

    10

It creates new cups until the total number of cups reaches the required size.

So if the input has 9 starting cups, Part 2 adds:

- `10`
- `11`
- `12`
- ...
- `1000000`

This preserves the original order first, then extends the ring with consecutive values.

---

### `Play(...)`

This method performs the actual simulation.

It loops for the requested number of moves.

On each move it:

- picks up the next three cups after the current cup
- removes them from the ring
- chooses the destination cup by decreasing the current value
- wraps to `max` when the value reaches `0`
- skips any value currently in the picked-up set
- inserts the three removed cups after the destination cup
- advances the current cup one step clockwise

This is the shared engine used by both puzzle parts.

---

### Picking Up Cups

At the start of each move, the solver identifies:

- `start = current?.Next`
- `end = start?.Next?.Next`

So the picked-up range is exactly three cups:

- `start`
- `start.Next`
- `end`

It stores their values in a set so destination selection can skip them.

Then it removes them from the circle by setting:

    current?.SetNext(end?.Next ?? new())

So the current cup now points directly to the cup after the removed block.

---

### Destination Selection

The solver begins destination search from:

- `current.Value - 1`

It repeatedly decrements and wraps back to `max` when needed.

It continues until the candidate value is not one of the removed cup values.

At a high level the logic is:

- decrement
- wrap from `0` to `max`
- skip removed values
- stop at the first valid remaining cup

Once found, the destination node is retrieved instantly from the dictionary.

---

### Fast Lookup with the Dictionary

The implementation stores every cup in a dictionary keyed by its numeric label.

So after the destination value is chosen, the matching node is found with:

- `Cup destination = map[next];`

This avoids scanning the linked list to find the destination cup and is what makes the Part 2 scale practical.

---

### Reinserting the Removed Cups

Once the destination cup is known, the removed block is inserted back into the ring.

The solver stores the destination's original next cup, then reconnects links in this order:

- destination points to the start of the removed block
- the end of the removed block points to the old destination next cup

Logically this is:

    nextCup = destination.Next
    destination.SetNext(start)
    end.SetNext(nextCup)

So the three removed cups are inserted immediately clockwise of the destination cup.

---

### Advancing the Current Cup

At the end of each move, the solver advances with:

    current = current?.Next

So the next move begins from the cup immediately clockwise of the current one after all reinsertion is complete.

---

### `Cup.cs`

This class models one cup node in the circular list.

It stores:

- `Next`
- `Value`

It provides:

- `SetNext(Cup cup)`
- `SetValue(long value)`
- `ToString()`

The setters return the current or next cup so the builder can chain operations cleanly while constructing the circle.

---

### `ToString()`

`ToString()` prints the cups clockwise starting after the current cup.

It:

- starts from `this.Next`
- appends each cup value
- stops when it loops back to the current cup

So when this method is called on cup `1`, it returns the exact Part 1 output format: all cup labels clockwise after `1`, excluding `1` itself.

---

## 🛠 Implementation Notes

- Cups are stored as a circular linked list
- Each cup value also has a dictionary entry for direct lookup
- Part 1 runs `100` moves with the original cup count
- Part 2 expands to `1,000,000` cups and runs `10,000,000` moves
- The picked-up cups are tracked with a set of their values
- Reinsertion is done by rewiring linked-list pointers rather than rebuilding structures
- A direct reference to cup `1` is preserved during setup

---

## 🧪 Behaviour Summary

Given a starting cup label string:

- the solver creates a circular cup structure
- each move removes the next three cups
- selects a destination by counting down and wrapping
- reinserts the removed cups after that destination
- advances the current cup
- Part 1 returns the labels after cup `1`
- Part 2 returns the product of the two cups after cup `1`

---

## 🚀 Key Takeaways

- Good example of using a circular linked list for efficient local rearrangement
- The dictionary lookup is the key optimisation for large-scale simulation
- The same move engine supports both the short and extended versions
- Part 1 focuses on output ordering
- Part 2 focuses on scaling the same logic to extreme input size

---

## 🔗 References

- https://adventofcode.com/2020/day/23