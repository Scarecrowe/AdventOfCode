# 🎄 Advent of Code 2015 - Day 20: Infinite Elves and Infinite Houses

## 📜 Puzzle Overview

Santa has assigned elves to deliver presents to an infinite row of houses.

Each elf is numbered, and each elf delivers presents to houses based on its own number:

- Elf `1` visits every house
- Elf `2` visits every second house
- Elf `3` visits every third house
- and so on

In Part 1:

- each elf delivers `10` times its number in presents to every house it visits

In Part 2:

- each elf delivers `11` times its number in presents
- each elf only visits `50` houses

The goal is to determine the **lowest house number** that receives at least as many presents as the target value in the puzzle input.

---

## 🧩 Part 1

Determine the lowest house number that receives at least the target number of presents.

### 💡 Approach

- Start from house `1`
- For each house, find all divisors of that house number
- Add those divisor values together
- Multiply the total by `10`
- Stop when the result reaches or exceeds the puzzle input

This works because every divisor of a house number represents an elf that visits that house.

---

## 🧩 Part 2

Repeat the search, but with the updated delivery rules:

- each elf delivers `11` times its number
- each elf only visits `50` houses

### 💡 Approach

- Reuse the same divisor-based logic
- Only count an elf if that elf would still be within its first `50` deliveries
- Multiply the valid total by `11`
- Stop when the result reaches or exceeds the puzzle input

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new InfiniteElvesAndInfiniteHouses(this.Input)`
- Calls `Lowest(false)`

For Part 2:

- Creates `new InfiniteElvesAndInfiniteHouses(this.Input)`
- Calls `Lowest(true)`

The boolean flag controls whether the Part 2 delivery limit is applied.

---

### `InfiniteElvesAndInfiniteHouses.cs`

This class contains the full search logic.

The constructor reads the puzzle input and stores it as:

- `NumberOfHouses`

This is the target number of presents that a house must reach.

The class exposes:

- `Lowest(bool infinite)`

This method searches upward from house `1` until it finds the first house whose calculated present total is high enough.

---

### Finding the Lowest House

The `Lowest()` method works like this:

- start with `min = 1`
- calculate the present total for that house using `Sum(min, infinite)`
- if the total is too small, increment the house number
- repeat until the total reaches the target

At a high level, the loop behaves like this:

    min = 1
    while Sum(min, mode) < target
        min += 1

Once the loop finishes, `min` is the answer.

---

### Summing Elf Deliveries

The `Sum(int value, bool limit)` method calculates how many presents a given house receives.

It does this by finding divisor pairs for the house number.

For each divisor `i`:

- if `value % i == 0`, then `i` is a divisor
- `value / i` is the matching paired divisor

Both values represent elves that visit that house.

To avoid checking every number up to the house value, the method only iterates up to the square root:

    int root = (int)Math.Sqrt(value) + 1;

This makes divisor lookup much more efficient than scanning the full range.

---

### Part 1 Delivery Rules

When the limit flag is off:

- every divisor pair contributes to the sum
- both `i` and `value / i` are added
- the final sum is multiplied by `10`

At a high level:

    sum += i
    sum += value / i

This matches the Part 1 rule where every elf visits every valid multiple of its elf number.

---

### Part 2 Delivery Rules

When the limit flag is on:

- only elves that are still within their first `50` house visits are counted
- if `i <= 50`, then elf `value / i` can still contribute
- if `value / i <= 50`, then elf `i` can still contribute

This works because:

- elf `n` visits house `k * n`
- so the visit count is determined by how many times the elf number fits into the house number

After summing the valid contributors, the result is multiplied by `11`.

---

## 🛠 Implementation Notes

- Input is a single integer target value
- The solver searches one house at a time from lowest upward
- Divisor pairs are used to identify which elves visit a house
- Square-root iteration keeps divisor lookup efficient
- Part 1 and Part 2 share the same search logic
- The only differences between the two parts are:
  - present multiplier
  - whether the 50-house delivery limit is enforced

---

## 🧪 Examples

For the first few houses in Part 1:

| House | Presents |
|-------|----------|
| `1`   | `10`     |
| `2`   | `30`     |
| `3`   | `40`     |
| `4`   | `70`     |
| `5`   | `60`     |
| `6`   | `120`    |
| `7`   | `80`     |
| `8`   | `150`    |
| `9`   | `130`    |

These totals come from summing the divisors of each house number and multiplying by `10`.

For example:

- House `4` has divisors `1`, `2`, and `4`
- Total elf value = `7`
- Presents = `70`

---

## 🚀 Key Takeaways

- Good example of turning a delivery puzzle into a divisor-sum problem
- Uses square-root factor searching instead of checking every possible elf directly
- Part 2 is solved by extending the same divisor logic with a delivery cap
- Keeps the implementation compact by sharing one search path for both puzzle parts

---

## 🔗 References

- https://adventofcode.com/2015/day/20