# 🎄 Advent of Code 2024 - Day 23: LAN Party

## 📜 Puzzle Overview

This puzzle models a network of computers connected by direct links.

Each input line describes a bidirectional connection between two computers:

    aa-bb

The solver parses the full input into an adjacency map so every computer knows which other computers it is directly connected to.

Part 1 counts all fully connected groups of three computers, but only where at least one computer name starts with `t`.

Part 2 finds the largest fully connected group in the network and returns its members as a comma-separated password string.

---

## 🧩 Part 1

Count how many interconnected triples exist in the network.

### 💡 Approach

- Parse each input line into two computer names
- Store both directions of every connection
- Generate every unique 3-computer combination
- Skip combinations where none of the names start with `t`
- Check whether all 3 computers are connected to each other
- Count the valid groups

---

## 🧩 Part 2

Find the largest fully connected group of computers and return it as the LAN party password.

### 💡 Approach

- Reuse the same parsed network graph
- Search for the largest clique
- Collect the biggest fully interconnected set of computers
- Sort the names alphabetically
- Join them with commas

---

## 🧠 Code Breakdown

### `Day23.cs`

This is the puzzle entry point.

- Sets the puzzle title to `LAN Party`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new LANParty(this.Input)`
- Calls `Interconnections()`

For Part 2:

- Creates `new LANParty(this.Input)`
- Calls `Password()`

---

### `LANParty.cs`

This class contains the full network logic.

It stores:

- `Connections`

This is defined as:

    Dictionary<string, HashSet<string>>

So each computer maps to the set of computers directly connected to it.

---

### Parsing the Input

`Parse(string[] input)` reads every line of the puzzle input.

For each line it:

- splits on `"-"`
- gets the two computer names
- adds the connection from A to B
- adds the connection from B to A

This ensures the graph is bidirectional.

So a line such as:

    kh-tc

creates both:

- `kh -> tc`
- `tc -> kh`

---

### Adding Connections

`AddConnection(string computerA, string computerB)` updates the adjacency map.

It:

- creates a new `HashSet<string>` when the computer is first seen
- otherwise adds the destination computer to the existing set

Using a `HashSet` keeps neighbour lookups efficient and avoids duplicate links.

---

### `KeyCombinations()`

This method generates every unique combination of 3 different computers.

It:

- copies all computer names into a list
- uses three nested loops over indices `i`, `j`, and `k`
- creates tuples of `(A, B, C)`

Before storing a triple, it checks whether at least one of the names starts with:

    t

If all three names fail that test, the combination is skipped immediately.

So this method prefilters combinations to match the Part 1 rule.

---

### Part 1 Triangle Check

`Interconnections()` loops through every triple produced by `KeyCombinations()`.

For each `(a, b, c)` it checks that every pair is connected:

- `a` contains `b`
- `a` contains `c`
- `b` contains `a`
- `b` contains `c`
- `c` contains `a`
- `c` contains `b`

If all six checks pass, the triple forms a fully connected group and the result is incremented.

Because the graph is stored bidirectionally, this is effectively checking whether the three computers form a clique of size 3.

---

### Part 1 Return Value

After all valid triples have been tested, `Interconnections()` returns:

- the total number of fully connected 3-computer groups
- restricted to combinations where at least one computer starts with `t`

So the silver answer is the count of qualifying interconnected triples.

---

### Largest Clique Search

Part 2 uses:

    LargestConnection(Dictionary<string, HashSet<string>> connections)

This method searches for the largest fully connected group in the graph.

It maintains:

- `results`
- `keys`

and defines a local recursive function:

    Search(HashSet<string> connection, HashSet<string> remainingKeys, HashSet<string> skipKeys)

---

### Recursive Search Logic

The recursive search builds candidate cliques step by step.

For each candidate computer:

- add that computer to the current connection set
- reduce `remainingKeys` to only nodes connected to that computer
- reduce `skipKeys` the same way
- recurse deeper

This means the search only continues along computers that remain compatible with the current clique.

When both:

- `remainingKeys.Count == 0`
- `skipKeys.Count == 0`

the current group is treated as a maximal clique.

If that clique has not already been recorded, it is added to `results`.

---

### Selecting the Password Group

After the full search completes, the method:

- finds the maximum clique size
- takes the first recorded clique with that size
- converts it to a list
- sorts it alphabetically
- returns the sorted names

Then `Password()` returns:

    string.Join(",", this.LargestConnection(this.Connections))

So the gold answer is a comma-separated list of the largest clique's members.

---

## 🛠 Implementation Notes

- The network is stored as a bidirectional adjacency map
- Neighbours are kept in `HashSet<string>`
- Part 1 only considers 3-node combinations where at least one computer starts with `t`
- Part 1 checks full mutual connectivity explicitly
- Part 2 performs a recursive clique search
- Maximal cliques are collected into `results`
- The final password is alphabetically sorted before joining with commas

---

## 🧪 Behaviour Summary

Given a list of computer-to-computer links:

- the solver parses them into an undirected graph
- Part 1 generates all qualifying triples and counts those that are fully connected
- Part 2 searches for the largest fully connected subset of computers
- the final password is the sorted comma-separated list of that largest group

---

## 🚀 Key Takeaways

- Good example of representing a network as an adjacency map
- Uses `HashSet` membership checks for efficient connectivity testing
- Part 1 uses brute-force triple generation with an early name-based filter
- Part 2 performs a recursive clique search over compatible neighbours
- The same parsed graph supports both puzzle parts cleanly

---

## 🔗 References

- https://adventofcode.com/2024/day/23