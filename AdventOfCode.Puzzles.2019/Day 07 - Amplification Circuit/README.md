# 🎄 Advent of Code 2019 - Day 7: Amplification Circuit

## 📜 Puzzle Overview

This puzzle builds on the Intcode computer by wiring five amplifiers together and testing different phase setting sequences.

Each amplifier runs the same Intcode program, but receives different inputs:

- its phase setting
- the signal from the previous amplifier

In Part 1, the amplifiers run in a simple chain from A to E.

In Part 2, the amplifiers are connected in a feedback loop, so the output from the last amplifier feeds back into the first amplifier until the programs stop producing output.

The goal in both parts is to find the highest thruster signal possible.

---

## 🧩 Part 1

Determine the highest thruster signal using phase settings `0` to `4`.

### 💡 Approach

- Generate every permutation of:
  - `0, 1, 2, 3, 4`
- For each permutation:
  - start with signal value `0`
  - run the Intcode program once for each amplifier
  - feed in:
    - the amplifier's phase setting
    - the current signal value
  - collect the output and pass it to the next amplifier
- Track the highest final output seen across all permutations

---

## 🧩 Part 2

Determine the highest thruster signal using feedback loop phase settings `5` to `9`.

### 💡 Approach

- Generate every permutation of:
  - `5, 6, 7, 8, 9`
- Create five separate Intcode CPU instances
- Preload each amplifier with its phase setting
- Start with signal value `0`
- Repeatedly:
  - send the current signal into the active amplifier
  - run that amplifier
  - read its next output
  - pass that output to the next amplifier in a circular loop
- Stop when an amplifier no longer produces output
- Track the highest signal produced across all permutations

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Amplification Circuit`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new AmplificationCircuit(this.Input)`
- Calls `HighestThrusterSignal()`

For Part 2:

- Creates `new AmplificationCircuit(this.Input)`
- Calls `HighestFeedbackThrusterSignal()`

---

### `AmplificationCircuit.cs`

This class contains the amplifier search logic.

It stores:

- `Program`

The constructor takes the first input line and stores it as the Intcode program string.

---

### Program Storage

The constructor does this:

    this.Program = input[0];

So the solver expects the puzzle input to be a single comma-separated Intcode program on the first line.

That program is then reused for every amplifier run.

---

### Phase Setting Permutations

For both puzzle parts, the solver generates every possible phase ordering.

For Part 1 it uses:

    new List<int> { 0, 1, 2, 3, 4 }.Permutations(5)

For Part 2 it uses:

    new List<int> { 5, 6, 7, 8, 9 }.Permutations(5)

This ensures every valid amplifier configuration is tested.

---

### Part 1 Amplifier Chain

`HighestThrusterSignal()` handles the simple amplifier sequence.

For each permutation it:

- starts with `value = 0`
- creates an `IntcodeCpu`
- loops over the 5 amplifiers
- resets the CPU before each amplifier run
- enqueues:
  - the phase setting
  - the current signal
- runs the CPU
- dequeues the output to use as the next signal

At a high level the logic is:

    value = 0
    for each phase in permutation
        reset cpu
        enqueue phase
        enqueue value
        run cpu
        value = output

After all 5 amplifiers have run, the final `value` is the thruster signal for that permutation.

---

### Tracking the Best Signal

Both parts keep a running best result in:

    long result = 0;

After each permutation finishes, the solver compares the produced signal with the current best.

If the new signal is larger, it replaces the stored result.

At the end, the highest signal found is returned.

---

### Part 2 Feedback Loop Setup

`HighestFeedbackThrusterSignal()` creates a separate CPU for each amplifier.

It builds:

- `List<IntcodeCpu> cpus = new();`

Then, for each permutation, it creates five fresh CPU instances from the same program.

Before execution begins, it queues each amplifier's phase setting into its own input queue.

So unlike Part 1, the machines persist between runs instead of being reset for every signal pass.

---

### Feedback Loop Execution

The feedback mode uses a circular execution loop.

It starts with:

    long value = 0;
    int index = 0;

Then repeatedly:

- enqueue the current signal into the current amplifier
- run that CPU
- check whether it produced any output
- if it did, dequeue the output and store it in `value`
- move to the next amplifier, wrapping back to the start

That flow looks like this logically:

    while (true)
        enqueue value into current cpu
        run cpu

        if no output exists
            break

        value = output
        move to next cpu

This continues until one of the amplifiers stops producing output, which signals that the feedback process is finished.

---

### Circular Amplifier Progression

To move through the five amplifiers repeatedly, the solver uses:

    index = index.IncrementWrap(cpus.Count);

This allows the current amplifier index to wrap from the last amplifier back to the first, matching the feedback loop behaviour.

---

### Input and Output Handling

The implementation relies on the Intcode CPU exposing queues for both input and output.

For each run it uses:

- `cpu.Input.Enqueue(...)`
- `cpu.Output.Dequeue()`

That makes the amplifier chaining straightforward, because one amplifier's output becomes the next amplifier's input signal.

---

## 🛠 Implementation Notes

- The puzzle input is stored as a single program string
- Part 1 reuses one CPU instance and resets it for each amplifier run
- Part 2 creates five persistent CPU instances
- Both parts brute-force every valid phase permutation
- Input and output are passed through queue-based CPU interfaces
- Feedback mode cycles through amplifiers using wrapped indexing

---

## 🧪 Behaviour Summary

Given one Intcode program:

- the solver tests every valid phase setting order
- each amplifier receives a phase setting and a signal
- Part 1 runs the amplifiers in a straight chain
- Part 2 keeps five CPUs alive and feeds outputs around in a loop
- each permutation produces one candidate thruster signal
- the highest signal across all permutations is returned

---

## 🚀 Key Takeaways

- Good example of brute-forcing permutations to search a small state space
- Part 1 uses simple sequential signal passing
- Part 2 extends the idea into a persistent feedback loop
- Queue-based I/O makes amplifier communication easy to model
- The solution cleanly separates puzzle orchestration from the Intcode CPU itself

---

## 🔗 References

- https://adventofcode.com/2019/day/7