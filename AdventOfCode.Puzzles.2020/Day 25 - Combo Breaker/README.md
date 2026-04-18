# 🎄 Advent of Code 2020 - Day 25: Combo Breaker

## 📜 Puzzle Overview

This puzzle is about reverse-engineering a simple handshake process.

The input contains two public keys:

- the card public key
- the door public key

The solver uses modular transformation logic to:

- determine a loop size
- derive the shared encryption key

Unlike most Advent of Code days, this implementation only computes the Part 1 answer directly. Part 2 is represented with a completion message instead of additional logic.

---

## 🧩 Part 1

Determine the shared encryption key produced by the card and door handshake.

### 💡 Approach

- Parse the two public keys from the input
- Find the loop size that produces one of the public keys using subject number `7`
- Use the other public key together with that discovered loop size
- Repeatedly apply the transformation formula modulo `20201227`
- Return the resulting shared encryption key

---

## 🧩 Part 2

There is no extra computational solver here.

### 💡 Approach

- Return the fixed completion message shown by the implementation

---

## 🧠 Code Breakdown

### `Day25.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Combo Breaker`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `ComboBreaker.EncryptiongKey(this.Input)`

For Part 2:

- Returns a fixed string indicating enough stars have been earned

---

### `ComboBreaker.cs`

This class contains the handshake logic.

It provides:

- `EncryptiongKey(string[] input)`
- `FindLoopSize(int subject, long publicKey)`
- `PrivateKey(long subject, long loopSize)`

---

### Parsing the Input

`EncryptiongKey(string[] input)` starts by converting the input into numeric values:

    long[] keys = input.ToLong();

So the two input lines become:

- `keys[0]`
- `keys[1]`

These represent the two public keys used by the handshake process.

---

### Main Encryption Flow

The main method returns:

    PrivateKey(keys[0], FindLoopSize(7, keys[1]))

That means it:

- finds the loop size for the second public key using subject number `7`
- uses the first public key as the subject
- computes the shared encryption key from that loop size

Because the handshake is symmetric, using one public key with the other device's loop size produces the same final encryption key.

---

### Finding the Loop Size

`FindLoopSize(int subject, long publicKey)` brute-forces the transformation process until it matches the target public key.

It begins with:

    long value = 1;
    int loopSize = 1;

Then repeatedly performs:

    value *= subject;
    value %= 20201227;

After each transformation, it checks:

    if (value == publicKey)

When the generated value matches the target public key, it returns the current loop size.

So this method effectively asks:

- how many times must subject `7` be transformed to produce the given public key?

---

### Computing the Encryption Key

`PrivateKey(long subject, long loopSize)` performs the modular transformation a fixed number of times.

It starts with:

    long key = 1;

Then loops from `1` to `loopSize`, applying:

    key *= subject;
    key %= 20201227;

At the end it returns:

    key

So this method computes the shared encryption key by transforming the chosen public key exactly as many times as required by the discovered loop size.

---

### Transformation Rule

Both helper methods rely on the same core operation:

    value *= subject
    value %= 20201227

This is the full mathematical engine behind the puzzle.

The implementation does not use any advanced optimisation here. It simply applies the transformation repeatedly until the desired result is reached.

---

### Part 1 Return Value

The silver answer is:

- the derived shared encryption key

This is returned as a string by `Day25.Silver()`.

---

### Part 2 Return Value

The gold answer is not computed from input.

Instead the method returns:

    You have enough stars to [Check On Your Deposit]

So this puzzle implementation only includes the handshake calculation needed for Part 1.

---

## 🛠 Implementation Notes

- Input is parsed directly into a `long[]`
- The loop size search is a brute-force modular simulation
- Subject number `7` is used to discover the loop size
- The final key uses one public key combined with the other key's loop size
- Modulus `20201227` is applied on every transformation step
- `Day25.Gold()` returns a fixed message rather than solving an additional problem
- The method name is spelled `EncryptiongKey` in the implementation

---

## 🧪 Behaviour Summary

Given two public keys:

- the solver parses both as long integers
- it repeatedly transforms subject `7` until one public key is reproduced
- that reveals the matching loop size
- it then transforms the other public key using that loop count
- the final result is the shared encryption key
- Part 2 simply returns a completion message

---

## 🚀 Key Takeaways

- Nice example of modular arithmetic used in a handshake algorithm
- Uses direct brute-force loop-size discovery rather than optimisation
- The same transformation rule powers both loop-size search and key generation
- Very compact implementation with only one core solver class
- Part 2 is intentionally represented as a fixed message in this codebase

---

## 🔗 References

- https://adventofcode.com/2020/day/25