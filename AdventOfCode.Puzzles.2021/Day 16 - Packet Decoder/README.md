# 🎄 Advent of Code 2021 - Day 16: Packet Decoder

## 📜 Puzzle Overview

This puzzle decodes a transmission made of nested binary packets.

The input is provided as a hexadecimal string. The solver first converts that hex input into a binary stream, then parses packets recursively.

Each packet begins with:

- a 3-bit version
- a 3-bit type ID

There are two broad packet kinds:

- literal packets, which store a value
- operator packets, which contain one or more sub-packets

Part 1 adds together all packet version numbers.  
Part 2 evaluates the outermost packet as an expression tree.

The puzzle entry returns:

    new PacketDecoder(this.Input).Sum()
    new PacketDecoder(this.Input).Run()

---

## 🧩 Part 1

Determine the sum of all packet version numbers in the transmission.

### 💡 Approach

- Convert the hexadecimal input into a binary digit stream
- Parse packets recursively from left to right
- Build packet objects containing:
  - version
  - type
  - literal value or sub-packets
- Walk the full packet tree
- Add together the version numbers from every packet

---

## 🧩 Part 2

Evaluate the packet transmission as an expression.

### 💡 Approach

- Reuse the same recursive packet parser
- Build the packet hierarchy from the binary stream
- Evaluate the outermost packet based on its type:
  - sum
  - product
  - minimum
  - maximum
  - literal
  - greater-than
  - less-than
  - equality
- Return the final value produced by the root packet

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Packet Decoder`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- creates `new PacketDecoder(this.Input)`
- calls `Sum()`

For Part 2:

- creates `new PacketDecoder(this.Input)`
- calls `Run()`

---

### `PacketType.cs`

This enum defines the packet operation types.

It contains:

- `Sum = 0`
- `Product = 1`
- `Min = 2`
- `Max = 3`
- `Literal = 4`
- `Greater = 5`
- `Less = 6`
- `Equal = 7`

This allows parsed type IDs to be converted directly into named packet behaviours.

---

### `Packet.cs`

This class models a parsed packet.

It stores:

- `Version`
- `TypeId`
- `Value`
- `Packets`

The constructor accepts:

- packet version
- packet type
- literal value

It also initialises:

- `Packets = new();`

So literal packets keep their value directly, while operator packets collect nested sub-packets.

---

### Adding Sub-Packets

`AddSubPacket(Packet packet)` appends a child packet into:

- `this.Packets`

This is used while recursively parsing operator packets.

As the parser discovers nested packets, they are attached to their parent packet through this method.

---

### Packet Evaluation

`Run(Packet packet)` evaluates one packet and returns its result as a list.

It switches on:

- `packet.TypeId`

and dispatches to the relevant operation:

- `Sum()`
- `Product()`
- `Min()`
- `Max()`
- literal value
- `Greater()`
- `Less()`
- `Equal()`

So this method acts as the operation router for packet execution.

---

### Operator Methods

The operator methods all evaluate child packets first.

For example:

- `Sum()` gathers all child results and returns their sum
- `Product()` multiplies all child results
- `Min()` returns the smallest child result
- `Max()` returns the largest child result

The comparison methods:

- `Greater()`
- `Less()`
- `Equal()`

also evaluate child packets first, then compare the first two results and return:

- `1` when true
- `0` when false

So the full packet tree behaves like an expression tree built from nested operations.

---

### `PacketDecoder.cs`

This class coordinates parsing and final result calculation.

It stores:

- `Packets`

The constructor does:

- `this.Packets = new PacketParser().Parse(input)`

So parsing happens immediately when the decoder is created.

It exposes two main methods:

- `Run()`
- `Sum(Packet? packet = null)`

---

### Part 2 Final Evaluation

`Run()` returns:

    this.Packets[0].Run(this.Packets[0]).First()

So the gold answer is produced by evaluating the first parsed top-level packet and taking its first result.

---

### Part 1 Version Summation

`Sum(Packet? packet = null)` recursively adds version numbers.

It works like this:

- if no packet is passed in:
  - sum over all top-level packets
- otherwise:
  - sum the versions of all child packets
  - then add the current packet's own version

Logically, that means:

- visit every packet in the tree
- accumulate all `Version` values
- return the total

So the silver answer is the recursive version-number sum.

---

### `PacketParser.cs`

This class contains the full transmission parsing logic.

It stores:

- `Decoded`

The constructor initialises:

- `this.Decoded = new();`

The main public method is:

- `Parse(string[] input)`

This method:

- converts the input into a binary stream
- reads packets from that stream
- builds packet objects recursively
- returns the parsed packet list

---

### Converting Hex to Binary

`BinaryStream(string[] input)` converts the single hex input line into a list of binary digits.

At a high level it does:

- read each hex character
- convert it to base 16
- convert that value into binary
- left-pad to 4 bits
- append each bit into a list

So every input hex character becomes exactly four binary digits in the decoded stream.

---

### Main Parse Loop

`Parse(...)` begins by building the binary stream:

    this.Decoded = BinaryStream(input);

It then reads packets while there is enough remaining data:

    while (index < (this.Decoded.Count - 11))

For each packet it reads:

- version via `VersionOrType(index)`
- type ID via `VersionOrType(...)`

Then it decides:

- literal packet -> `LiteralValue(...)`
- operator packet -> `Operator(...)`

The parsed packet is added to the result list, and the parse index moves forward.

---

### Reading Literal Packets

`LiteralValue(int version, int index)` parses a literal packet value.

It reads the binary stream in 5-bit groups.

For each group:

- the first bit indicates whether more groups follow
- the remaining 4 bits contribute to the literal value

The method builds the final number by repeatedly doing:

- multiply the current value by 16
- add the next 4-bit chunk

Once a group starts with `0`, the literal is complete.

It then returns:

- a new literal `Packet`
- the updated stream index

---

### Reading Operator Packets

`Operator(int version, PacketType typeId, int index)` parses an operator packet.

It first reads:

- the length type ID
- then the associated length value

If the length type ID is `0`:

- the next 15 bits describe the total bit length of the sub-packets
- the parser keeps reading sub-packets until that length is consumed

If the length type ID is `1`:

- the next 11 bits describe how many sub-packets follow
- the parser reads exactly that many child packets

Each parsed child packet is added with:

    packet.AddSubPacket(...)

So operator packets are built recursively until the full nested structure has been decoded.

---

### Binary Helpers

The parser uses a few small helpers:

- `VersionOrType(int index)`
- `LengthTypeId(int index)`
- `Length(int index, int lengthTypeId)`
- `ToBinary(int index, int count)`

These methods extract fixed-width slices from the decoded binary list and convert them into integers.

So the parser logic stays focused on packet structure while the helpers handle bit slicing.

---

## 🛠 Implementation Notes

- The input hex string is converted into a binary digit list before parsing
- Packets are parsed recursively
- Literal packets store a direct numeric value
- Operator packets store sub-packets
- Type IDs are mapped through the `PacketType` enum
- Part 1 recursively sums packet version numbers
- Part 2 evaluates the root packet as an expression tree
- Comparison packets return `1` or `0`

---

## 🧪 Behaviour Summary

Given a hexadecimal transmission:

- the solver converts it into binary digits
- reads packet version and type headers
- parses either literal values or nested operator packets
- builds a packet tree structure

Part 1:

- walks the tree and sums all packet versions

Part 2:

- evaluates the tree according to packet type rules
- returns the final value from the outermost packet

So the final result is either the total version sum or the evaluated expression result.

---

## 🚀 Key Takeaways

- Good example of recursive parsing over a binary stream
- `PacketParser` separates bit-level decoding from packet evaluation
- `Packet` cleanly represents both literal and operator packet forms
- `PacketType` gives readable names to the operation IDs
- Part 1 and Part 2 reuse the same parsed packet tree
- The packet tree behaves like a nested expression evaluator

---

## 🔗 References

- https://adventofcode.com/2021/day/16