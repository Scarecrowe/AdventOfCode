# 🎄 Advent of Code 2019 - Day 16: Flawed Frequency Transmission

## 📜 Puzzle Overview

This puzzle works with a signal made of decimal digits and repeatedly transforms it through 100 phases.

Each phase produces a new output digit for every input position using a repeating pattern derived from:

- `0`
- `1`
- `0`
- `-1`

In Part 1, the solver applies the transformation directly to the input signal and returns the first 8 digits after 100 phases.

In Part 2, the input signal is repeated 10,000 times, the message offset is taken from the first 7 digits, and the solver extracts the 8-digit message after 100 phases.

---

## 🧩 Part 1

Apply 100 phases to the input signal and return the first 8 digits.

### 💡 Approach

- Parse the input string into an integer array of digits
- Build a transformation lookup table
- Repeat for 100 phases:
  - calculate the next signal values for the first half using pattern logic
  - calculate the remaining values using a reverse running-sum shortcut
- Replace the current signal with the phased result
- Return the first 8 digits as a string

---

## 🧩 Part 2

Repeat the signal 10,000 times, use the message offset, and return the 8-digit message.

### 💡 Approach

- Read the first 7 digits as the message offset
- Expand the input signal 10,000 times into one large array
- Repeat for 100 phases:
  - start processing from the offset
  - calculate the lower section with pattern logic
  - calculate the remaining suffix with the reverse running-sum optimisation
- Return the 8 digits starting at the offset

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Flawed Frequency Transmission`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `FlawedFrequencyTransmission.Single(this.Input)`

For Part 2:

- Calls `FlawedFrequencyTransmission.Multiple(this.Input)`

---

### `FlawedFrequencyTransmission.cs`

This class contains the full signal processing logic.

It defines:

- `Modifier`
- `Single(string[] input)`
- `Multiple(string[] input)`
- `Transform()`
- `CalculatePhasedData(...)`

The implementation is entirely static.

---

### Parsing the Signal

Both parts begin by converting the input string into an array of digits.

At a high level that looks like:

    input[0]
        -> char array
        -> each char parsed as an integer digit
        -> int[]

So the signal is stored as:

- `int[] data`

---

### `Modifier`

The implementation defines:

    private static readonly int[] Modifier = new int[] { 1, 2, 1, 0 };

This is used to map the repeating FFT pattern into column indexes for the transform lookup table.

It effectively represents the repeating sign sequence needed by the algorithm.

---

### `Transform()`

`Transform()` precomputes a small multiplication lookup table.

It builds:

- `int[,] result = new int[10, 3];`

For every digit `0` to `9` and every multiplier column, it stores:

    result[i, j] = (i * (j - 1)) % 10;

That means the table represents multiplying each digit by:

- `-1`
- `0`
- `1`

and keeping the last digit contribution.

This avoids repeating that small calculation inside the main phase loops.

---

### Part 1 Signal Processing

`Single(string[] input)` handles the normal puzzle input.

It creates:

- `data`
- `transform`
- `phasedData`

Then it runs:

    100

phases.

For each phase:

- process the first half of the signal using the pattern logic
- process the rest using `CalculatePhasedData(...)`
- replace `data` with the newly generated phase
- reset the output buffer for the next round

At the end it returns:

- the first 8 digits of the final signal

---

### First-Half Digit Calculation

For the earlier positions in the signal, the code calculates each new digit directly.

For each output position `d` it:

- walks the remaining input digits from `d` onward
- advances a modifier index when crossing pattern boundaries
- adds the transformed contribution into `phasedData[d]`

At a high level it behaves like:

    for each output position d
        for each input position i from d to end
            update pattern section when needed
            add transformed contribution

After the sum is complete, the value is normalised to a single digit by taking its absolute value modulo 10.

---

### Reverse Running-Sum Optimisation

For the latter part of the signal, the solver uses:

    CalculatePhasedData(...)

This avoids recomputing the full repeating pattern when the pattern simplifies into a suffix sum.

The helper works backwards from the end of the array:

- keep a running total
- add the current input digit
- store the last digit of the running total

That logic is:

    current += data[e]
    phasedData[e] = current % 10

This is much faster for the tail section of the signal.

---

### `CalculatePhasedData(...)`

This helper takes:

- the starting index
- the current signal
- the output buffer

It then fills the remaining values from right to left.

So once the direct calculations are done up to a certain point, the rest of the phase can be completed efficiently using cumulative sums.

---

### Part 2 Expanded Signal

`Multiple(string[] input)` handles the repeated signal version.

It first calculates the message offset with:

    int offset = input[0][..7].ToInt();

Then it parses the raw input digits and expands them into a much larger array:

- original signal repeated `10,000` times

That expanded data becomes the working signal for the 100 phase transformations.

---

### Part 2 Phase Processing

The Part 2 method uses the same overall structure as Part 1, but starts the direct work from:

    offset

rather than from the start of the array.

For each phase it:

- calculates direct values beginning at the offset
- stops that direct work halfway through the signal
- hands the remainder to `CalculatePhasedData(...)`

So the implementation focuses only on the part of the signal relevant to the requested message.

---

### Part 2 Return Value

After 100 phases, the method returns:

    data.Join().Substring(offset, 8)

So the gold answer is the 8-digit message beginning at the offset derived from the first 7 digits of the original input.

---

## 🛠 Implementation Notes

- The solver stores the signal as an `int[]`
- Both parts run exactly 100 phases
- A lookup table is used to avoid repeated digit-by-multiplier work
- The `Modifier` array controls the repeating pattern section selection
- Early positions are computed directly with nested loops
- Later positions are computed with a reverse cumulative sum optimisation
- Part 2 repeats the input 10,000 times
- Part 2 begins processing from the message offset rather than from index `0`

---

## 🧪 Behaviour Summary

Given a string of digits:

- the solver parses it into a numeric signal
- each phase produces a new signal from the old one
- Part 1 transforms the original signal 100 times
- Part 2 expands the signal 10,000 times and extracts a message from a large offset
- both parts combine direct phase calculation with a faster suffix-based optimisation
- the final result is an 8-digit string

---

## 🚀 Key Takeaways

- Good example of combining brute-force pattern logic with a targeted optimisation
- The lookup table reduces repeated arithmetic inside the inner loops
- The suffix calculation is the key performance improvement for the latter half of the signal
- Part 2 reuses the same core phase structure while focusing only on the relevant section
- The implementation balances correctness with speed by splitting the work into two calculation strategies

---

## 🔗 References

- https://adventofcode.com/2019/day/16