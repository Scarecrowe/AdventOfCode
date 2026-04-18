# 🎄 Advent of Code 2019 - Day 18: Many-Worlds Interpretation

## 📜 Puzzle Overview

This puzzle explores a maze containing:

- walls
- open passages
- doors
- keys
- a starting position

The input is a 2D character map using tiles such as:

    #
    .
    @
    a-z
    A-Z

Where:

- `#` is a wall
- `.` is open space
- `@` is the starting position
- lowercase letters are keys
- uppercase letters are doors

Part 1 finds the fewest steps needed to collect every key.

Part 2 modifies the map into four separate starting regions, then collects keys across all four vault sections.

---

## 🧩 Part 1

Find the minimum number of steps required to collect all keys.

### 💡 Approach

- Parse the input into a 2D map
- Find the starting position marked by `@`
- Perform a breadth-first search from that location
- Track both:
  - current position
  - which keys have already been collected
- Skip walls
- Refuse to pass through a door unless its matching key has already been collected
- When stepping onto a key tile, mark that key as collected
- Stop once every key has been marked as visited
- Return the distance travelled

---

## 🧩 Part 2

Split the vault into four quadrants and collect all keys across the four new start positions.

### 💡 Approach

- Modify the original map around the starting point
- Replace the central cross with walls
- Place four new `@` start positions on the diagonals
- Store all four robot locations
- Pre-mark keys that do not belong to each robot's quadrant
- Run the same key-collection search once per quadrant
- Sum the four path lengths
- Return the combined total

---

## 🧠 Code Breakdown

### `Day18.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Many-Worlds Interpretation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new ManyWorldsInterpretation(this.Input)`
- Calls `CollectKeys()`

For Part 2:

- Creates `new ManyWorldsInterpretation(this.Input)`
- Calls `SplitMap()`
- Calls `CollectVaultKeys()`

---

### `ManyWorldsInterpretation.cs`

This class contains both the maze representation and the search logic.

It stores:

- `Locations`
- `Inventory`
- `Map`

The constructor:

- loads the input into a `VectorArray`
- finds the first `@` on the map
- stores that point as the initial entry in `Locations`

So the solver begins with one robot position for Part 1, then expands that into four positions for Part 2.

---

### Map Representation

The map is stored as a 2D character grid.

The solver works directly with the original puzzle symbols:

- `#` for walls
- `@` for start points
- lowercase letters for keys
- uppercase letters for doors

Neighbour traversal is done with cardinal movement only:

- north
- south
- west
- east

Diagonal movement is not used during the search.

---

### Key and Door Detection

The class uses two helper checks:

- `IsDoor(char value)`
- `IsKey(char value)`

A door is any character in the uppercase `A-Z` range.

A key is any character in the lowercase `a-z` range.

This means door access can be tested by converting the door letter into its alphabet index and checking whether that key has already been collected.

---

### Tracking Collected Keys

Collected keys are represented as:

    bool[26]

Each index corresponds to one letter:

- `0` for `a`
- `1` for `b`
- ...
- `25` for `z`

When the search steps onto a key tile, it marks that entry as `true`.

The helper `AllVisited(bool[] visited)` returns `true` only when every slot in the 26-element array is marked.

So this implementation treats the puzzle goal as:

- all possible key slots visited

rather than counting only the keys present in the map.

---

### State Caching

The search does not only track position.

It also tracks the full key-collection state.

Each cached state is stored as:

- current `Vector` location
- a string built from the visited-key array

`VisitedKey(bool[] visited)` converts the boolean array into a binary-style string such as:

    001001100...

This lets the solver distinguish between:

- reaching the same tile with different sets of collected keys

which is essential for this puzzle.

---

### Breadth-First Search

`CollectKeys(Vector? location = null, List<bool>? visitedState = null)` performs the main search.

It uses:

- `Queue<QueueItem> queue`
- `HashSet<(Vector Point, string Visited)> cache`

Each queue item stores:

- current location
- distance travelled
- current key state

At a high level the loop does:

- dequeue the next state
- skip it if that `(position, keys)` state was already seen
- return the distance if all keys are marked visited
- inspect all adjacent non-wall cells
- block doors whose keys are not yet collected
- clone the key-state array for each move
- mark a key if one is collected
- enqueue the next search state

Because it uses a queue, the first successful completion gives the minimum distance for that search.

---

### Splitting the Map for Part 2

`SplitMap()` rewrites the area around the original start position.

It changes the cross centred on the original `@` like this:

- left becomes `#`
- up becomes `#`
- down becomes `#`
- right becomes `#`
- centre becomes `#`

Then it places four new start positions on the diagonals around the original centre.

After that it clears the original single location and stores four new robot starting points in `Locations`.

So Part 2 is handled by physically rewriting the maze into the four-vault layout before any searches begin.

---

### Collecting Vault Keys

`CollectVaultKeys()` handles the Part 2 flow.

It starts by creating four separate visited-key arrays:

- one for each robot region

Then it calls:

- `IgnoreDoors(visitedStart)`

After that it runs:

- `CollectKeys(this.Locations[i], visitedStart[i].ToList())`

for each of the four start points and adds the four results together.

So this implementation does not run one combined multi-robot search state.

Instead it splits the map and solves each quadrant independently, then sums the distances.

---

### Quadrant Pre-Marking

`IgnoreDoors(List<bool[]> visitedStart)` pre-fills each robot's key-state array.

It scans the whole map and, for every key found, decides whether that key belongs inside a robot's quadrant relative to that robot's start position.

If the key is outside a robot's region, that key is marked as already visited for that robot.

This means each robot only needs to search for keys that lie in its own quadrant.

Effectively, the method prevents doors and keys in unrelated regions from blocking that robot's local search.

---

## 🛠 Implementation Notes

- The solver uses a `VectorArray` map and cardinal movement
- Part 1 runs a breadth-first search from the single `@` start
- Search state includes both position and collected keys
- Keys are tracked with a `bool[26]` array
- Doors are blocked until their matching key has been collected
- Part 2 rewrites the map into four separate vault regions
- The gold solution solves each region independently and sums the four results
- Quadrant filtering is handled by pre-marking irrelevant keys as already collected

---

## 🧪 Behaviour Summary

Given a maze of walls, doors, and keys:

- the solver loads the map and finds the starting location
- Part 1 searches for the shortest route that collects every key
- each search state remembers both location and inventory state
- doors can only be crossed once their key is collected
- Part 2 splits the centre of the map into four separate start areas
- each quadrant is solved independently
- the final result is either the shortest single-maze collection distance or the sum of the four vault distances

---

## 🚀 Key Takeaways

- Good example of pathfinding where state matters just as much as position
- Caching `(location, collected-keys)` is the key to avoiding repeated work
- The implementation uses a compact 26-slot boolean array for inventory tracking
- Part 2 is solved by physically rewriting the maze and partitioning the search
- The gold solution avoids a more complex combined multi-agent search by treating each vault region separately

---

## 🔗 References

- https://adventofcode.com/2019/day/18