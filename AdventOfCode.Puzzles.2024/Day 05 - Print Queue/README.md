# 🎄 Advent of Code 2024 - Day 05: Print Queue

## 📜 Puzzle Overview

This puzzle processes page ordering rules and update sequences.

The input is split into two sections.

The first section defines ordering rules:

    X|Y

meaning page `X` must appear before page `Y`.

The second section defines update sequences:

    75,47,61,53,29

Each update is a list of page numbers that must be checked against the ordering rules.

The solver determines which updates are already valid and which need to be reordered.

---

## 🧩 Part 1

Find the sum of the middle page number from every valid update.

### 💡 Approach

- Parse all ordering rules into pairs
- Parse all update lines into page lists
- For each update:
  - compare every page with every later page
  - ensure that the pair exists in the ordering rules
- Keep only the valid updates
- Take the middle value from each valid update
- Sum those middle values

---

## 🧩 Part 2

Reorder the invalid updates, then sum their middle page numbers.

### 💡 Approach

- Reuse the same parsing and validation logic
- Separate updates into:
  - valid
  - invalid
- For each invalid update:
  - sort the pages using the ordering rules
- Take the middle value from each corrected update
- Sum those middle values

---

## 🧠 Code Breakdown

### `Day5.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Print Queue`
- Loads the puzzle input
- Implements both puzzle parts directly inside the day class

For Part 1:

- parses rules and updates
- filters valid updates
- sums their middle values

For Part 2:

- parses rules and updates again
- separates invalid updates
- reorders them
- sums their middle values

---

### Input Parsing

Both `Silver()` and `Gold()` parse the input in the same way.

They use:

    bool parsed = false;

Before the blank line:

- lines are split on `"|"`
- each rule becomes:

    (int X, int Y)

After the blank line:

- lines are split on `","`
- each update becomes:

    List<int>

So the full parsed input becomes:

- `List<(int X, int Y)> ordering`
- `List<List<int>> updates`

---

### Rule Storage

Ordering rules are stored as tuples:

    (X, Y)

This means the code can directly test whether a required ordering exists with:

    ordering.Contains((update[i], update[j]))

So if page `47` appears before page `53`, the solver expects the tuple:

    (47, 53)

to be present in the rules.

---

### Validating Updates

To validate an update, the solver checks every pair of positions:

- outer loop at index `i`
- inner loop at index `j = i + 1`

For each pair it tests:

    ordering.Contains((update[i], update[j]))

If any pair is missing from the rule list:

- the update is marked invalid
- checking stops early for that update

If all ordered pairs are present:

- the update is considered valid

---

### Part 1 Valid Update Collection

`Silver()` builds:

- `List<List<int>> valid`

Each update is checked with the nested loop logic.

If no rule violation is found:

- the update is added to `valid`

After that, the solver calculates the middle value of each valid update and adds them together.

---

### Middle Value Selection

For every accepted update, the code calculates:

    decimal x = Math.Ceiling((decimal)v.Count / 2);

Then uses:

    v[(int)x - 1]

So it selects the centre element of the update list.

That middle page number is added to the running result.

---

### Part 2 Invalid Update Collection

`Gold()` uses the same validation logic, but stores results in two lists:

- `valid`
- `inValid`

If an update fails validation:

- it is added to `inValid`

The original valid list is then cleared and reused to hold corrected updates.

---

### Reordering Invalid Updates

Each invalid update is reordered with:

    invalid.Sort((a, b) => {
        var yAxis = ordering.Where(x => x.X == a).Select(x => x.Y);
        if (yAxis.Contains(b)) {
            return -1;
        }
        return 1;
    });

This custom sort says:

- if there is a rule stating `a` must come before `b`
  - place `a` before `b`
- otherwise
  - place `b` first

After sorting, the corrected update is added to the list of updates to score.

---

### Part 2 Result Calculation

Once all invalid updates have been reordered:

- the solver again finds the middle page from each corrected update
- sums those values
- returns the final total

So Part 2 only scores:

- updates that were originally invalid
- but became correctly ordered after sorting

---

### `PrintQueue.cs`

There is also a `PrintQueue` class in the folder.

It currently contains:

- `Rules`
- a constructor that initialises the dictionary

However, the main puzzle logic is implemented directly in `Day5.cs`.

---

## 🛠 Implementation Notes

- Parsing is duplicated in both `Silver()` and `Gold()`
- Ordering rules are stored as tuple pairs rather than a graph structure
- Update validation uses nested loops over every page pair
- Part 2 fixes invalid updates with a custom comparison sort
- Middle values are selected using the centred list index

---

## 🧪 Behaviour Summary

Given a set of page ordering rules and update sequences:

- the solver parses rules before the blank line
- parses updates after the blank line
- Part 1 keeps updates where every page pair matches the required ordering
- valid updates contribute their middle page value
- Part 2 takes the invalid updates
- reorders them using the rule relationships
- then sums the middle value from each corrected update

---

## 🚀 Key Takeaways

- Clean example of rule checking over ordered sequences
- Pairwise comparison makes update validation straightforward
- Part 2 reuses the same validation idea but adds repair logic
- Tuple-based rule storage keeps the implementation simple
- The dedicated `PrintQueue` class exists, but the real solution lives in `Day5.cs`

---

## 🔗 References

- https://adventofcode.com/2024/day/5