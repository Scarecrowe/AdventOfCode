# 🎄 Advent of Code 2020 - Day 7: Handy Haversacks

## 📜 Puzzle Overview

This puzzle works with a set of bag containment rules.

Each rule describes:

- an outer bag colour
- the inner bag colours it can contain
- how many of each inner bag it holds

A rule looks like this:

```text
light red bags contain 1 bright white bag, 2 muted yellow bags.
```

The solver parses these rules into a bag graph, then answers two questions about the special bag colour:

```text
shiny gold
```

Part 1 asks how many outer bag colours can eventually contain a shiny gold bag.

Part 2 asks how many bags are required inside a single shiny gold bag.

---

## 🧩 Part 1

Count how many bag colours can eventually contain at least one shiny gold bag.

### 💡 Approach

- Parse every rule into a parent bag with child bags
- Track bag colours that are already known to lead to `shiny gold`
- For each outer bag:
  - recursively inspect its children
  - stop as soon as a shiny-gold path is found
- Count how many outer bags can eventually reach that target

---

## 🧩 Part 2

Count how many total bags are required inside a shiny gold bag.

### 💡 Approach

- Start from the `shiny gold` bag
- Recursively count all of its children
- For each contained child:
  - multiply its quantity by the full recursive size of that child bag
- Add everything together to get the total required inner bag count

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Handy Haversacks`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
new HandyHaversacks(this.Input).ShinyGoldCount()
```

For Part 2:

```text
new HandyHaversacks(this.Input).RequiredBags()
```

---

### `HandyHaversacks.cs`

This class contains the full parsing and traversal logic.

It defines two important constants:

```text
ShinyGold = "shiny gold"
NoBag = "no other bags."
```

It also maintains a list named:

```text
available
```

This starts with:

```text
shiny gold
```

and is used during the containment search.

The constructor immediately parses the full input:

```text
new HandyHaversacks(input) => this.Parse(input)
```

---

### Parsed Structure

Each input line is split using:

```text
" contain "
```

The left side becomes the outer bag colour.

The right side becomes the contained bag list.

At a high level the parser does this:

- read the outer bag colour
- create that bag entry if it does not already exist
- split the contained section by commas
- skip rules containing `no other bags.`
- parse each child count and child colour
- add each child bag to the outer bag's `Children`

So the rules become a graph of bags and their nested contents.

---

### Parsing Bag Colours

For the outer bag, the parser removes the trailing:

```text
" bags"
```

For child entries, it removes:

```text
" bags"
" bag"
"."
```

Then trims the result and splits off the leading number.

So a child entry like:

```text
2 muted yellow bags.
```

becomes:

- count = `2`
- colour = `muted yellow`

---

### Direct Shiny Gold Tracking

During parsing, if a child colour is exactly:

```text
shiny gold
```

the parent colour is immediately added to the `available` list.

That means the solver keeps an initial set of bag colours that directly contain a shiny gold bag before deeper recursion begins.

---

### Part 1 Search

`ShinyGoldCount()` loops through every outer bag in the structure.

For each one it creates:

- a local `counted` list
- a `hasBag` flag set to `false`

It then calls:

```text
HasShinyGold(...)
```

If the recursive search finds a valid path to shiny gold, that outer bag is counted.

So Part 1 works by asking, for every bag:

- do any descendants eventually reach `shiny gold`?

If yes, increment the result.

---

### Recursive Containment Check

`HasShinyGold(...)` walks through a bag's children recursively.

It stops early when a match is found.

The logic is:

- if `hasBag` is already true, return
- inspect each child
- if the child's name is already in `available`, mark success
- otherwise, if that child exists in the main bag dictionary and is not an outer placeholder, recurse into it

This allows the solver to find indirect shiny gold containment through multiple nesting levels.

---

### Part 2 Count

`RequiredBags()` starts from:

```text
this[ShinyGold]
```

Then for each child of the shiny gold bag it adds:

```text
child.Count * this.RequiredCount(this[child.Name], this)
```

So the total includes:

- every directly contained bag
- every nested bag inside those children
- multiplied by the required quantity at each level

---

### Recursive Nested Count

`RequiredCount(Bag current, Dictionary bags)` begins with:

```text
count = 1
```

It then recursively adds each child's multiplied requirement.

This means each recursive call returns:

- the bag itself
- plus everything inside it

Because `RequiredBags()` multiplies each shiny gold child by its recursive size, the final answer becomes the total number of contained bags required inside the shiny gold bag.

---

## 🛠 Implementation Notes

- The solver stores rules in a dictionary-like bag lookup
- Parsing is done once in the constructor
- `available` is seeded with `shiny gold`
- Parent bags that directly contain shiny gold are added to `available` during parsing
- Part 1 uses recursive containment checks
- Part 2 uses recursive quantity expansion
- `RequiredCount(...)` counts the current bag as `1` before adding descendants
- `RequiredBags()` multiplies each shiny gold child by its full recursive size

---

## 🧪 Behaviour Summary

Given a list of bag rules:

- parse each outer bag and its children
- build a nested bag structure
- Part 1 checks which outer bags can eventually lead to `shiny gold`
- Part 2 counts how many total bags are required inside `shiny gold`
- both parts rely on recursive traversal of the parsed containment graph

---

## 🚀 Key Takeaways

- Good example of turning text rules into a navigable graph structure
- Part 1 is a recursive reachability problem
- Part 2 is a recursive counting problem
- Direct shiny-gold parents are tracked during parsing to speed up checks
- The same parsed structure supports both puzzle parts cleanly

---

## 🔗 References

- https://adventofcode.com/2020/day/7