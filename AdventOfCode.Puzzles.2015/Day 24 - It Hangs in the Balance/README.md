# 🎄 Advent of Code 2015 - Day 24: It Hangs in the Balance

## 📜 Puzzle Overview

Santa needs to load the sleigh so that the packages are split into groups of equal total weight.

In Part 1:

- the packages must be split into `3` groups of equal weight

In Part 2:

- the packages must be split into `4` groups of equal weight

The first group is the important one. It should:

- use as few packages as possible
- and, if there is still more than one valid option, have the smallest quantum entanglement

Quantum entanglement is the product of all package weights in that first group.

---

## 🧩 Part 1

Determine the quantum entanglement of the ideal first group when the packages are split into `3` equal-weight groups.

### 💡 Approach

- Parse all package weights
- Calculate the total weight of all packages
- Divide by `3` to get the required weight for each group
- Search combinations in ascending group size
- For each combination:
  - check whether its total matches the target group weight
  - if it does, return the product of that combination

Because combinations are checked from smallest group size upward, the first valid match automatically satisfies the minimum-package requirement.

---

## 🧩 Part 2

Determine the quantum entanglement of the ideal first group when the packages are split into `4` equal-weight groups.

### 💡 Approach

- Reuse the same logic as Part 1
- Divide the total weight by `4` instead of `3`
- Search for the first valid group whose sum matches that target
- Return its quantum entanglement

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ItHangsInTheBalance(this.Input)`
- Calls `IdealConfiguration(3)`

For Part 2:

- Creates `new ItHangsInTheBalance(this.Input)`
- Calls `IdealConfiguration(4)`

The only difference between the two parts is the number of required groups.

---

### `ItHangsInTheBalance.cs`

This class contains the full package search logic.

The constructor:

- converts the raw input into a numeric weight list
- stores the sum of all package weights

It stores:

- `Weights`
- `TotalWeight`

The main public method is:

- `IdealConfiguration(int groupCount)`

---

### Parsing the Weights

The input is converted into a numeric list using:

    input.ToLongList()

This gives a list of package weights that can be used for combination generation and product calculation.

The total weight of all packages is also calculated immediately:

    this.TotalWeight = this.Weights.Sum();

That total is then used to determine the required weight for each group.

---

### Calculating the Target Group Weight

Inside `IdealConfiguration()` the required weight for one group is calculated as:

    long groupWeight = this.TotalWeight / groupCount;

This is the sum every group must reach in order for the sleigh to balance correctly.

---

### Searching for the Ideal Group

The solver checks possible first groups by size, starting from the smallest:

    for (int i = 1; i <= this.Weights.Count; i++)

For each size, it generates every possible combination of that many weights:

    this.Weights.Combinations(i)

Each combination is then checked to see whether its sum equals the required group weight.

If the combination matches the target weight, the solver immediately returns its product.

At a high level, the logic works like this:

- try all 1-item groups
- then all 2-item groups
- then all 3-item groups
- continue until a valid target-weight group is found

This guarantees that the first match uses the fewest packages.

---

### Calculating Quantum Entanglement

When a valid group is found, its quantum entanglement is returned using:

    combination.ToList().Product()

This multiplies all package weights in the candidate group together.

That product becomes the answer returned by the solver.

---

### Important Behaviour

The implementation stops at the first valid group it finds for the smallest matching group size.

That means the code is explicitly optimised around:

- minimum number of packages first

It does not continue searching all same-sized combinations once a valid one has been found.

So the returned result is the first valid minimum-size combination encountered by the combination generator.

---

## 🛠 Implementation Notes

- Input is parsed once during construction
- Package weights are stored as a numeric list
- Group target weight is derived from total weight divided by the required number of groups
- Combinations are tested in ascending group size order
- Quantum entanglement is calculated as the product of the selected weights
- Part 1 and Part 2 reuse the same solver with different group counts

---

## 🧪 Examples

Given the example package weights:

    1
    2
    3
    4
    5
    7
    8
    9
    10
    11

For a 3-way split, the ideal first group is:

- `11` and `9`

This group:

- reaches the required target weight
- uses only `2` packages
- has quantum entanglement:

    11 * 9 = 99

So the example answer is:

- `99`

---

## 🚀 Key Takeaways

- Good example of solving a partitioning problem with combination search
- Keeps the logic compact by checking candidate groups in increasing size order
- Part 2 is solved by reusing the same method with a different target group count
- The implementation focuses on finding the first minimum-size valid group and returning its product

---

## 🔗 References

- https://adventofcode.com/2015/day/24