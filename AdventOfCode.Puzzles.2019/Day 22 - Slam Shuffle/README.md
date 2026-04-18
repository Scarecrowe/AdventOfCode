# 🎄 Advent of Code 2019 - Day 22: Slam Shuffle

## 📜 Puzzle Overview

This puzzle simulates shuffling a deck of cards using three techniques:

- deal into new stack
- cut N
- deal with increment N

In Part 1, the solver applies these shuffles directly to a deck of `10007` cards and finds the position of card `2019`.

In Part 2, the deck is far too large to simulate directly, so the shuffle process is converted into modular arithmetic and evaluated mathematically for a huge number of repetitions.

---

## 🧩 Part 1

Determine the position of card `2019` after applying all shuffle instructions once.

### 💡 Approach

- Create a deck containing:
  - `0` through `10006`
- Read each shuffle instruction in order
- Apply the matching deck operation:
  - reverse the deck
  - rotate cards for `cut`
  - distribute cards into stepped positions for `deal with increment`
- After all shuffles complete, return the index of card `2019`

---

## 🧩 Part 2

Determine which card ends up in position `2020` after the shuffle process is repeated many times on a massive deck.

### 💡 Approach

- Treat the shuffle process as a linear transformation instead of simulating cards directly
- Track two values:
  - increment multiplier
  - offset difference
- Process each shuffle instruction and update those values modulo the deck size
- Raise the transformation to the required number of iterations
- Evaluate the final card value at position `2020`

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Slam Shuffle`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new SlamShuffle(this.Input)`
- Calls `Shuffle()`

For Part 2:

- Creates `new SlamShuffle(this.Input)`
- Calls `ShuffleLargerDeck()`

---

### `SlamShuffle.cs`

This class contains the full shuffle logic.

It stores:

- `Deck`
- `Input`

The constructor:

- sets the deck size to `10007`
- stores the input lines
- creates a new deck
- fills it with card values from `0` to `10006`

At a high level that setup is:

    int deckSize = 10007;
    this.Deck = new(deckSize);
    this.Deck.Fill(0, deckSize - 1);

So the direct simulation deck is prepared immediately when the class is created.

---

### `Shuffle()`

This method performs the Part 1 simulation.

It reads every input line and applies one of three operations:

- `ShuffleStack()`
- `ShuffleCut(...)`
- `ShuffleIncrement(...)`

The instruction handling works like this logically:

    foreach line in input
        if line == "deal into new stack"
            reverse deck
        else if line starts with "cut"
            cut deck by amount
        else
            deal with increment amount

After all instructions are processed, it returns:

    this.Deck.IndexOf(2019)

So the silver answer is the final position of card `2019`.

---

### `ShuffleStack()`

This handles:

- `deal into new stack`

The method simply reverses the full deck:

    this.Deck.Reverse();

So the top card becomes the bottom card and vice versa.

---

### `ShuffleCut(int index)`

This handles:

- `cut N`

If `N` is positive:

- take the first `N` cards
- move them to the end of the deck

If `N` is negative:

- take the last `|N|` cards
- move them to the front of the deck

So the deck is rotated left or right depending on the sign of the cut value.

At a high level:

    positive cut -> front section moves to end
    negative cut -> end section moves to front

---

### `ShuffleIncrement(int increment)`

This handles:

- `deal with increment N`

It creates a new array with the same deck size, then places each card into its new stepped position:

    deck[(increment * i) % this.Deck.Count] = this.Deck[i];

So card positions are redistributed by repeatedly advancing through the target deck with the given increment.

Once complete, the rebuilt array becomes the new deck.

---

### Part 1 Return Value

After every shuffle instruction has been applied once, the method returns:

- the index of card `2019`

So Part 1 is a direct deck simulation followed by a lookup.

---

### `ShuffleLargerDeck()`

This method performs the Part 2 calculation.

Instead of building a gigantic deck, it uses modular arithmetic with:

- deck size `119315717514047`
- iteration count `101741582076661`

It starts with:

    BigInteger offset_diff = 0;
    BigInteger increment_mul = 1;

Then it processes every instruction through the helper:

    ShuffleLargerDeck(ref increment_mul, ref offset_diff, size, line);

This builds a mathematical representation of one complete shuffle sequence.

---

### Large Deck Transformation

For the huge deck, each shuffle updates the running transformation values.

The helper handles the instructions like this:

#### `cut N`

    offset_diff += N * inc_mul

#### `deal into new stack`

    inc_mul *= -1
    offset_diff += inc_mul

#### `deal with increment N`

    inc_mul *= inverse(N)

So instead of moving cards explicitly, the solver updates the shuffle transformation in modular space.

After each instruction, both values are reduced modulo the deck size.

---

### Repeating the Shuffle Sequence

Once the single-pass transformation is known, the solver raises it to the required number of iterations using:

    GetSequence(iter, increment_mul, offset_diff, size)

This method computes:

- the repeated increment
- the repeated offset

It uses modular exponentiation and modular inversion, allowing the solver to jump directly to the final repeated shuffle state without simulating every pass.

---

### `GetSequence(...)`

This helper calculates the accumulated transformation after many repetitions.

It computes:

    increment = inc_mul.ModPow(iterations, size)

and then derives the combined offset using the geometric-series style modular formula.

So the repeated shuffle sequence is compressed into one final linear mapping.

---

### Final Card Lookup

Once the repeated transformation is known, the solver returns:

    Get(offset, increment, 2020, size)

`Get(...)` evaluates:

    (offset + (i * increment)) % size

using:

- `i = 2020`

So the gold answer is the card value that ends up at position `2020`.

---

## 🛠 Implementation Notes

- Part 1 uses a real deck of `10007` cards
- The deck is stored as a list of integers
- The three shuffle types are implemented as separate helper methods
- Part 1 returns the position of card `2019`
- Part 2 uses `BigInteger` throughout
- Part 2 does not simulate the deck directly
- Large-deck shuffling is represented as modular linear transformations
- Repeated shuffles are combined using modular exponentiation and inversion

---

## 🧪 Behaviour Summary

Given a list of shuffle instructions:

- the solver parses and applies each instruction in order
- Part 1 directly mutates a physical deck representation
- Part 2 converts the same shuffle rules into modular arithmetic
- the final result is either:
  - the position of card `2019`
  - or the card found at position `2020` in the massive repeated shuffle case

---

## 🚀 Key Takeaways

- Good example of solving a small case with direct simulation
- The three shuffle types map cleanly to dedicated helper methods
- Part 2 avoids impossible-scale simulation by modelling shuffles mathematically
- Modular arithmetic is the key to handling the huge deck and repeat count
- The solution cleanly separates the direct deck logic from the large-number optimisation

---

## 🔗 References

- https://adventofcode.com/2019/day/22