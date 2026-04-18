# 🎄 Advent of Code 2024 - Day 22: Monkey Market

## 📜 Puzzle Overview

This puzzle works with a list of starting secret numbers.

Each buyer begins with an initial number, and that number is repeatedly transformed into a new secret number using a fixed sequence of operations.

Part 1 asks for the sum of the 2000th generated secret number for every buyer.

Part 2 changes focus from full secret numbers to prices, using only the final digit of each generated value. It then looks for 4-step change patterns in those digits and finds the pattern that produces the best total banana score across all buyers.

---

## 🧩 Part 1

Generate each buyer's secret number 2000 times and sum the final results.

### 💡 Approach

- Parse every input line into a `long`
- For each starting number:
  - repeatedly generate the next secret number
  - do this exactly `2000` times
- Add the final generated value for each buyer into a running total
- Return the total sum

---

## 🧩 Part 2

Find the 4-change price pattern that gives the highest total value across all buyers.

### 💡 Approach

- Reuse the same starting numbers
- For each buyer:
  - generate the sequence of secret numbers for `2000` steps
  - extract the last digit of each value as the buyer's price history
- Convert each buyer's price history into a list of step-to-step differences
- Find all distinct subsequences of length `4`
- Keep only the sequences that appear in at least two buyers
- For each candidate sequence:
  - scan each buyer's change list
  - find the first occurrence of that exact 4-change pattern
  - add the corresponding sale price to a running total
- Return the highest total found

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Monkey Market`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new MonkeyMarket(this.Input)`
- calls `Sum()`

For Part 2:

- creates `new MonkeyMarket(this.Input)`
- calls `Best()`

---

### `MonkeyMarket.cs`

This class contains the full puzzle logic.

It stores:

- `Numbers`

The constructor parses the input with:

- `input.Select(x => long.Parse(x)).ToList()`

So each line becomes one starting secret number.

---

### Secret Number Generation

The core transformation is handled by:

- `SecretNumber(long number)`

It applies three operations in sequence:

1. multiply by `64`, then XOR with the original number
2. divide by `32`, then XOR again
3. multiply by `2048`, then XOR again

After each major step, the value is reduced modulo:

    16777216

At a high level the method behaves like this:

    value = (number * 64) ^ number
    value %= 16777216

    value = ((value / 32) ^ value) % 16777216
    value = ((value * 2048) ^ value) % 16777216

This produces the next secret number in the sequence.

---

### `Sum()`

This method solves Part 1.

It:

- starts with `result = 0`
- loops through every starting number
- repeatedly applies `SecretNumber(...)` `2000` times
- adds the final value into the total

So the silver answer is the sum of all buyers' 2000th secret numbers.

---

### Price Extraction

Part 2 does not work directly with the full secret values.

Instead, `Digits()` builds a digit history for each buyer.

For each starting number it:

- stores the last digit of the initial value
- generates `2000` new secret numbers
- stores the last digit of each generated value

So each buyer ends up with a list of prices based on:

- the ones digit of the current secret number

The last digit is extracted with logic equivalent to:

    $"{value}"[^1]

and parsed back to an integer.

---

### Price Change Sequences

`Sequeneces(List<List<int>> digits)` converts each buyer's price list into a list of differences.

For every adjacent pair:

    next_digit - current_digit

is added to the result list.

So if a buyer had prices like:

    3, 5, 4, 7

their change sequence would be:

    2, -1, 3

These change lists are what Part 2 searches through.

---

### Finding Candidate Patterns

`FindCommonSubsequences(...)` identifies all distinct subsequences of length `4` that appear across the buyers' change lists.

It works by:

- extracting every length-4 slice from each buyer's sequence
- storing them in a dictionary with a custom list comparer
- counting how many buyer lists contain each subsequence
- returning only those with a match count of at least `2`

This means the search space is reduced to patterns shared by multiple buyers.

---

### `GetSubsequences(...)`

This helper produces all unique contiguous subsequences of a given size.

For `k = 4`, it returns every length-4 window from a buyer's change list.

A `HashSet<List<int>>` with `ListComparer` is used so duplicate subsequences within the same list are only counted once for that buyer.

---

### `ListComparer`

Because `List<int>` normally compares by reference, the implementation includes a custom equality comparer.

It provides:

- `Equals(List<int> x, List<int> y)` using `SequenceEqual`
- `GetHashCode(List<int> obj)` by combining the hash codes of all elements

This allows sequences with the same values to be treated as equal keys in dictionaries and hash sets.

---

### `Best()`

This method solves Part 2.

It does the following:

- builds all buyer price digit lists with `Digits()`
- converts those to difference lists with `Sequeneces(...)`
- finds common 4-step patterns with `FindCommonSubsequences(...)`
- evaluates each candidate pattern across every buyer

For each candidate sequence:

- scan each buyer's change list
- find the first matching 4-step window
- when found, add the digit at position `j + 4` from that buyer's digit list
- stop scanning that buyer once the first match is used

That means each buyer contributes at most one sale price per candidate sequence.

The method keeps the highest total seen and returns it.

---

## 🛠 Implementation Notes

- Secret numbers are stored as `long`
- The modulus used in generation is `16777216`
- Part 1 applies the generator exactly `2000` times per buyer
- Part 2 uses the final digit of each secret number as the buyer price
- Price changes are stored as adjacent differences
- Candidate patterns are fixed-length subsequences of size `4`
- Only patterns appearing in at least two buyers are considered
- Each buyer contributes only the first matching occurrence for a given pattern
- The method name is spelled `Sequeneces` in the implementation

---

## 🧪 Behaviour Summary

Given a list of starting secret numbers:

- the solver parses them into `long` values
- Part 1 repeatedly evolves each number for 2000 steps
- the final values are summed
- Part 2 records the last digit at each step as a price history
- those prices are converted into difference sequences
- all shared 4-step change patterns are examined
- the best banana total is returned

So the final result is either:

- the sum of all 2000th secret numbers
- or the maximum total produced by a shared 4-change price pattern

---

## 🚀 Key Takeaways

- Part 1 is a straightforward repeated-number transformation puzzle
- Part 2 shifts from raw values to derived price behaviour
- The last digit of each secret number becomes the meaningful signal
- Difference sequences make it possible to search for repeating market patterns
- A custom list comparer is needed to treat equal integer sequences as equal keys
- The solution reduces the search space by only testing patterns shared by multiple buyers

---

## 🔗 References

- https://adventofcode.com/2024/day/22