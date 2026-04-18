# 🎄 Advent of Code 2022 - Day 7: No Space Left On Device

## 📜 Puzzle Overview

This puzzle simulates parsing a terminal session that describes a small filesystem.

The input contains commands such as:

- `$ cd /`
- `$ cd ..`
- `$ cd a`
- `$ ls`

and listing output such as:

- `dir photos`
- `14848514 b.txt`

The solver builds an in-memory tree of filesystem entities, consisting of:

- directories
- files

Each entity knows:

- its name
- its parent
- its children
- its size
- whether it is a file or directory

Part 1 sums the sizes of all directories whose total size is less than `100000`.

Part 2 finds the smallest directory that can be deleted to free enough space for the update.

---

## 🧩 Part 1

Determine the sum of all directory sizes smaller than `100000`.

### 💡 Approach

- Parse the terminal transcript into a filesystem tree
- Start from the root directory
- Recursively walk all directories
- For each directory:
  - if its size is less than `100000`, add it to the total
- Return the recursive sum

---

## 🧩 Part 2

Determine the size of the smallest directory that can be deleted to free enough space.

### 💡 Approach

- Calculate total free space from the root directory size
- Work out how much extra space is required
- Recursively visit every directory
- Collect directory sizes that are at least as large as the required space
- Return the minimum matching size

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `No Space Left On Device`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new NoSpaceLeftOnDevice(this.Input)`
- Calls `Sum()`

For Part 2:

- Creates `new NoSpaceLeftOnDevice(this.Input)`
- Calls `Min()`

---

### `NoSpaceLeftOnDevice.cs`

This class contains the parsing and directory-size logic.

It stores:

- `Root`
- `Input`

The constructor:

- stores the input
- creates the root directory as `new("/", null)`
- immediately calls `ParseInput()`

So the full filesystem tree is built during construction.

---

### Root Directory

The root of the filesystem is stored in:

- `Root`

It is created as a directory entity with:

- name `/`
- no parent

All other files and directories are attached somewhere beneath this root node.

---

### `Entity.cs`

This class models a filesystem entry.

It stores:

- `Name`
- `Parent`
- `Type`
- `Size`
- `Children`

It also provides:

- `IsFile`

So one `Entity` can represent either:

- a directory
- or a file

---

### Entity Types

`EntityType.cs` defines two values:

- `Directory`
- `File`

New entities default to `Directory`.

The file constructor changes the type to `File`.

---

### Directory Construction

The directory constructor is:

    Entity(string name, Entity? parent)

It:

- stores the name
- stores the parent
- sets the type to `Directory`
- creates an empty child list
- adds itself to the parent's children when a parent exists

So directory relationships are built automatically as entities are created.

---

### File Construction and Size Propagation

The file constructor is:

    Entity(string name, Entity? parent, int size)

It first uses the directory constructor, then:

- changes the type to `File`
- sets `Size`
- walks up through every parent directory
- adds the file size into each ancestor's total size

That means directory sizes are accumulated incrementally during parsing rather than being calculated afterward.

So when a file is added, its size is immediately reflected in:

- its direct parent
- that parent's parent
- all the way up to the root

---

### Parsing the Terminal Input

`ParseInput()` processes the input line by line.

It keeps track of:

- `current`

which is the current directory entity.

Each line is split on spaces into an instruction array.

The parser then handles three main cases.

#### Command lines

If the line starts with:

    $

then it is treated as a terminal command.

The code ignores any command except:

- `cd`

So `$ ls` is effectively skipped.

#### `cd /`

Moves back to:

- `Root`

#### `cd ..`

Moves to:

- `current.Parent`

#### `cd name`

Searches the current directory's children for a matching name and moves into that entity.

---

### Parsing Directory Listings

If a non-command line begins with:

    dir

then the parser creates a new directory entity beneath the current directory.

So a line like:

    dir a

becomes a child directory named `a`.

---

### Parsing File Listings

Any other non-command line is treated as a file listing.

The parser reads:

- the file size from `instruction[0]`
- the file name from `instruction[1]`

Then it creates a file entity under the current directory.

Because file creation propagates size upward through parents, directory totals are updated immediately.

---

### Part 1 Recursive Sum

`Sum(Entity? current = null)` computes the silver answer.

If no directory is provided, it starts from:

- `Root`

It then:

- checks whether the current directory size is less than `100000`
- adds that size if it qualifies
- recursively visits all child directories
- skips file children

So the final result is the sum of all qualifying directory sizes in the whole tree.

---

### Part 2 Space Calculation

`Min()` computes how much space needs to be freed.

It calculates:

- `freeSpace = 70000000 - this.Root.Size`
- `requiredSpace = 30000000 - freeSpace`

Then it recursively searches for directories whose size is at least `requiredSpace`.

Those candidate sizes are stored in a list, and the method returns:

- the minimum candidate size

So the gold answer is the smallest directory large enough to solve the space shortfall.

---

### Recursive Minimum Search

`MinRecursive(Entity current, ref List<int> results, int requiredSpace)` performs the search.

For each directory:

- if `current.Size >= requiredSpace`
  - add that size to `results`

Then it recursively visits all child directories and skips file children.

After the full traversal, `Min()` returns the smallest value collected.

---

## 🛠 Implementation Notes

- The filesystem is represented as a tree of `Entity` objects
- Directories and files share the same `Entity` class
- File sizes are propagated upward immediately when files are created
- `$ ls` lines are ignored by the parser
- Directory lookup for `cd name` searches current child entities by name
- Part 1 recursively sums directory sizes below the threshold
- Part 2 recursively collects directory sizes above the required deletion threshold
- Directory sizes are not recomputed later because they are maintained during parsing

---

## 🧪 Behaviour Summary

Given a terminal transcript:

- the solver creates a root directory
- processes `cd` commands to move around the tree
- creates child directories from `dir` listings
- creates files from size-and-name listings
- updates directory sizes immediately when files are added

Then:

- Part 1 sums all directory sizes under `100000`
- Part 2 finds the smallest directory that can free enough space for the update

---

## 🚀 Key Takeaways

- Nice example of building a tree structure directly from command-style input
- File creation automatically updates all ancestor directory sizes
- Part 1 is a recursive filtered sum over directories
- Part 2 is a recursive minimum search over sufficiently large directories
- The implementation avoids a separate post-parse size calculation pass

---

## 🔗 References

- https://adventofcode.com/2022/day/7