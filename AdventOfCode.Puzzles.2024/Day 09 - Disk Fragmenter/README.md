# 🎄 Advent of Code 2024 - Day 9: Disk Fragmenter

## 📜 Puzzle Overview

This puzzle models a fragmented disk layout encoded as a compact digit string.

The input alternates between:

- file block lengths
- empty space lengths

For example, the digits describe a sequence of file segments and gaps across the disk.

The solver expands that layout into file block positions and then handles compaction in two different ways:

- Part 1 moves individual file blocks from the end of the disk into the earliest gaps
- Part 2 moves whole files into the earliest empty range that can contain them

The final answer in both parts is a checksum based on:

- file id
- block position

---

## 🧩 Part 1

Compact the disk by moving single blocks one at a time from the right into the earliest available empty positions.

### 💡 Approach

- Parse the input digits into alternating file lengths and gap lengths
- Build file objects and track every occupied block position
- Record empty block positions in a queue
- Record file ids in a stack in reverse block order
- Repeatedly:
  - take the next file block from the right
  - move it into the next empty block on the left
- Stop when a block would have to move to the right instead of left
- Sum the checksum of all files

---

## 🧩 Part 2

Compact the disk by moving whole files instead of individual blocks.

### 💡 Approach

- Reuse the parsed file layout
- Track contiguous empty ranges as `(start, length)`
- Process files from highest file id down to lowest
- For each file:
  - find the earliest empty range large enough to hold it
  - only allow the move if that empty range starts before the file's current first block
- Move the entire file in one operation
- Shrink or remove the empty range that was used
- Sum the checksum of all files

---

## 🧠 Code Breakdown

### `Day9.cs`

This is the puzzle entry point and the full implementation.

- Sets the puzzle title to `Disk Fragmenter`
- Loads the puzzle input
- Parses the compact disk map from `this.Input[0]`
- Builds separate file state for:
  - Part 1
  - Part 2

The constructor prepares:

- `filesp1`
- `filesp2`
- `emptyBlocks`
- `fileBlocks`
- `emptyRanges`

So both puzzle parts share the same initial parsed layout, but each uses its own copy of the file data.

---

### Parsing the Input

The constructor calls:

    ToLongList(this.Input[0])

This converts the input string into a list of digits.

It then walks through those digits by index:

- even indices represent file lengths
- odd indices represent empty space lengths

For each file segment it:

- creates an `AmphiFile` in `filesp1`
- creates a second `AmphiFile` in `filesp2`
- assigns a file id
- records every occupied block position
- pushes that file id into `fileBlocks` once per block

For each empty segment it:

- adds `(start, length)` to `emptyRanges`
- enqueues every empty block position into `emptyBlocks`

This means the parser builds both:

- per-block empty space tracking for Part 1
- per-range empty space tracking for Part 2

---

### File Storage

The solver stores files in:

- `Dictionary<long, AmphiFile> filesp1`
- `Dictionary<long, AmphiFile> filesp2`

Each `AmphiFile` contains:

- `id`
- `blocks`

where `blocks` is the list of disk positions currently occupied by that file.

It also exposes:

    checksum => blocks.Sum(a => a * id)

So each file contributes its own weighted checksum directly from its occupied block positions.

---

### `AmphiFile`

This inner class models one file on disk.

It stores:

- file id
- all occupied block positions

It provides two movement methods:

- `MoveBlock(long targetBlock)`
- `moveFile(long startBlock)`

This lets the same file representation support both:

- single-block moves for Part 1
- whole-file relocation for Part 2

---

### Part 1 Block Movement

`SolvePartOne()` performs the block-by-block compaction.

It repeatedly does this:

- pop a file id from `fileBlocks`
- dequeue the earliest empty block from `emptyBlocks`
- call:

    filesp1[fb].MoveBlock(emptyBlocks.Dequeue())

Inside `MoveBlock(...)`:

- it takes the file's last block
- checks whether that block is already left of the target empty block
- if so, it returns `false`
- otherwise:
  - the old block position is added back into `emptyBlocks`
  - the file's final block is removed
  - the target empty block is inserted at the front of the file's block list

So the algorithm moves the rightmost remaining file block into the leftmost available gap.

---

### Part 1 Stop Condition

`MoveBlock(...)` contains this key check:

    if (tmp < targetBlock) return false;

That means compaction stops when the next candidate file block is already to the left of the next empty block.

At that point, no more valid leftward block moves are possible.

---

### Part 1 Return Value

After compaction completes, Part 1 returns:

    filesp1.Values.Sum(a => a.checksum)

So the silver answer is the total checksum of all files after individual block compaction.

---

### Part 2 Whole File Movement

`SolvePartTwo()` handles file-level compaction.

It loops from the highest file id downwards:

    for (long i = maxFileID; i > 0; i--)

For each file it calculates:

- the file size from `filesp2[i].blocks.Count`

Then it searches `emptyRanges` for the first range that:

- is large enough for the file
- starts before the file's current first block

This is done with:

    emptyRanges.FindIndex(a => a.length >= reqSpace && a.start < filesp2[i].blocks[0])

If such a range exists:

- move the whole file to that range start
- remove the old empty range entry
- insert any leftover remainder back into `emptyRanges`

This matches the whole-file movement rule for the gold puzzle.

---

### Whole File Relocation

`moveFile(long startBlock)` simply replaces the file's block list with a new contiguous block range:

    blocks = LongRange(startBlock, blocks.Count).ToList();

So a file keeps the same length, but all of its blocks are relocated to a new continuous destination.

---

### `LongRange(...)`

This helper yields a contiguous sequence of block positions:

- starts at `start`
- returns `count` positions

It is used when rebuilding a moved file's block list during Part 2.

---

### Additional Helper Methods

The file also includes several extra helper methods:

- `DiskMap(string input)`
- `FindNext(int space, int[] disk)`
- `calculateChecksum(int[] nd)`
- `Compact(int[] disk)`

These provide an alternate array-based disk representation and compaction approach, but they are not used by the final `Silver()` and `Gold()` methods.

The active solution paths are:

- `SolvePartOne()`
- `SolvePartTwo()`

---

### `Silver()` and `Gold()`

The public puzzle outputs are:

For silver:

    return $"{SolvePartOne()}";

For gold:

    return $"{SolvePartTwo()}";

So the final answers come directly from the two main compaction implementations.

---

## 🛠 Implementation Notes

- The compact disk input is parsed digit by digit
- Even indices represent files, odd indices represent empty space
- Part 1 tracks empty positions with a queue and movable file blocks with a stack
- Part 2 tracks contiguous empty ranges as `(start, length)`
- `AmphiFile` stores explicit block positions rather than just file length
- The checksum is calculated as `position * fileId` summed across all blocks
- Part 1 moves one block at a time
- Part 2 moves an entire file only if a suitable earlier range exists
- The file contains some unused alternative helper methods that are not part of the final solver path

---

## 🧪 Behaviour Summary

Given a compressed disk map:

- the solver expands files and gaps into explicit block positions
- each file is assigned an increasing file id
- Part 1 repeatedly moves the rightmost available file block into the leftmost gap
- Part 2 repeatedly tries to move whole files into the earliest fitting empty range
- both parts finish by summing block position multiplied by file id
- the final result is the compacted disk checksum

---

## 🚀 Key Takeaways

- Good example of solving the same puzzle with two different movement models
- Part 1 uses simple block-level compaction with queue and stack structures
- Part 2 switches to range-based whole-file relocation
- The `AmphiFile` class keeps file state clean and reusable across both parts
- Storing explicit block positions makes checksum calculation very direct
- Separate parsed file dictionaries allow both parts to run independently from the same input

---

## 🔗 References

- https://adventofcode.com/2024/day/9