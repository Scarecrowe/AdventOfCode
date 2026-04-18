# 🎄 Advent of Code 2021 - Day 13: Transparent Origami

## 📜 Puzzle Overview

This puzzle works with a sheet of transparent paper marked with dots.

The input has two sections:

- a list of dot coordinates
- a list of fold instructions

A blank line separates the two parts.

Example input:

    6,10
    0,14
    9,10

    fold along y=7
    fold along x=5

Each coordinate marks a visible dot on the paper.

Each fold instruction tells the solver to fold the paper along either:

- `x=<value>`
- `y=<value>`

Part 1 asks for the number of visible dots after the first fold.

Part 2 applies every fold and prints the final pattern.

---

## 🧩 Part 1

Count how many dots are visible after the first fold.

### 💡 Approach

- Parse all dot coordinates into a set of points
- Parse all fold instructions into fold objects
- Build a 2D grid large enough to contain every dot
- Apply all folds in sequence
- Read the dot count recorded on the first fold
- Return that count

---

## 🧩 Part 2

Apply every fold and print the final paper pattern.

### 💡 Approach

- Reuse the same parsed dots and fold instructions
- Build the starting grid
- Fold the grid repeatedly
- After all folds are complete, render the final grid as text
- Use `#` for visible dots
- Use spaces for empty cells
- Return the printed pattern

---

## 🧠 Code Breakdown

### `Day13.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Transparent Origami`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new TransparentOrigami(this.Input)`
- calls `Fold()`
- returns `Folds.ElementAt(0).Dots`

For Part 2:

- creates `new TransparentOrigami(this.Input)`
- calls `Fold()`
- calls `Print()`

So the silver answer comes from the first recorded fold result, while the gold answer comes from printing the fully folded paper.

---

### `OrigamiFold.cs`

This class models a single fold instruction.

It stores:

- `IsHorizontal`
- `Value`
- `Dots`

The constructor takes:

- whether the fold is horizontal
- the fold position
- the running visible dot count for that fold

`Dots` is updated during the fold operation to record how many visible points remain after that fold has been applied.

---

### `TransparentOrigami.cs`

This class contains the full parsing, folding, and printing logic.

It stores:

- `Folds`
- `Dots`
- `Map`

The constructor:

- creates empty dot and fold collections
- creates an empty map
- parses the input
- builds the starting grid

So the full puzzle state is prepared as soon as the class is initialised.

---

### Parsing the Input

`ParseInput(string[] input)` reads the puzzle data in two phases.

It begins with:

    bool isFolds = false;

Then for each line:

- if the line is empty, switch to fold parsing mode
- otherwise, while still in dot mode:
  - split on `","`
  - add the point to `Dots`
- once fold mode starts:
  - remove `"fold along "`
  - split on `"="`
  - create a new `OrigamiFold`

A fold is created with:

    new(tokens[0] == "x", tokens[1].ToInt(), 0)

So in this implementation:

- `x` folds set `IsHorizontal` to `true`
- `y` folds set `IsHorizontal` to `false`

---

### Building the Grid

`BuildGrid()` sizes the map from the maximum dot coordinates.

It calculates:

- `width = this.Dots.Max(c => c.X) + 1`
- `height = this.Dots.Max(c => c.Y) + 1`

Then it creates:

    this.Map = new(width, height);

After that, every parsed point is marked on the grid with:

    this.Map[point] = 1;

So the initial paper is represented as a 2D map where:

- `1` means a visible dot
- `0` means empty space

---

### Main Fold Loop

`Fold()` applies every fold instruction in sequence.

It loops through:

    foreach (OrigamiFold fold in this.Folds)

For each fold it does:

    this.Map = fold.IsHorizontal ? this.FoldHorizontally(fold) : this.FoldVertically(fold);

So the paper is replaced with a newly folded map after each step.

The method then returns the current `TransparentOrigami` instance so the caller can continue reading fold results or print the final output.

---

### Vertical Fold Logic

`FoldVertically(OrigamiFold fold)` handles folds created from `y=<value>` instructions.

It creates a new map with:

    new(this.Map.Width, fold.Value)

Then it loops over rows above the fold line and checks whether either side of the fold contains a dot.

At a high level the logic is:

    if top side has a dot OR mirrored bottom side has a dot
        mark result as a dot
        increment fold.Dots

So the lower half of the paper is folded upward onto the upper half.

---

### Horizontal Fold Logic

`FoldHorizontally(OrigamiFold fold)` handles folds created from `x=<value>` instructions.

It creates a new map with:

    new(fold.Value, this.Map.Height)

Then it loops over columns to the left of the fold line and checks whether either side contains a dot.

At a high level the logic is:

    if left side has a dot OR mirrored right side has a dot
        mark result as a dot
        increment fold.Dots

So the right half of the paper is folded left onto the remaining visible section.

---

### Dot Counting During Folds

Each fold object starts with:

    Dots = 0

Then every time a folded cell becomes visible, the implementation does:

    fold.Dots++;

This means the fold object itself stores the visible dot total for that specific fold result.

That is why Part 1 can simply return:

    Folds.ElementAt(0).Dots

after the full fold sequence has run.

---

### Printing the Final Pattern

`Print()` converts the current grid into a string.

It:

- returns an empty string if `Map` is null
- builds a `StringBuilder`
- loops over every row and column
- appends `"#"` when the grid cell is `1`
- appends `" "` otherwise

At the core of the rendering loop:

    result.Append(this.Map[y, x] == 1 ? "#" : " ");

So the gold solution is returned as a block of text representing the folded code or letters.

---

## 🛠 Implementation Notes

- `Day13.cs` returns the first fold's `Dots` value for silver
- `Day13.cs` returns the printed final grid for gold
- Dots are stored in `HashSet<Vector<int>>`
- Fold instructions are stored in `HashSet<OrigamiFold>`
- The initial paper map is built from the maximum input coordinates
- Fold methods create a brand new map rather than modifying the old one in place
- A visible folded cell is counted once even if both mirrored positions contain dots
- Printing uses `#` for dots and a space for blank cells

---

## 🧪 Behaviour Summary

Given a set of dot coordinates and fold instructions:

- the solver parses the dots into point positions
- it parses fold instructions into fold objects
- it builds a 2D paper grid
- each fold creates a new folded map
- overlapping dots remain a single visible dot
- Part 1 reports the visible dot count after the first fold
- Part 2 prints the final folded paper pattern

---

## 🚀 Key Takeaways

- Good example of converting coordinate input into a 2D grid representation
- Folding is implemented by creating new mapped views of the paper
- Each fold object tracks its own resulting visible dot count
- Part 1 and Part 2 both reuse the same fold pipeline
- The final output is rendered directly from the folded grid

---

## 🔗 References

- https://adventofcode.com/2021/day/13