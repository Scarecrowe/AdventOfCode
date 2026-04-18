# 🎄 Advent of Code 2020 - Day 20: Jurassic Jigsaw

## 📜 Puzzle Overview

This puzzle reconstructs a large image from many square tiles.

Each tile:

- has a numeric ID
- contains a 10x10 grid of `.` and `#`
- can be rotated or flipped

The solver parses the input into `JigsawPiece` objects, builds every orientation for each tile, identifies the corner tiles, assembles the full jigsaw, removes tile borders, joins the inner tile data into one final image, and then searches that image for sea monsters.

Part 1 multiplies the IDs of the corner tiles. Part 2 assembles the image and counts how many `#` pixels are not part of any sea monster.

---

## 🧩 Part 1

Determine the product of the four corner tile IDs.

### 💡 Approach

- Parse every tile block into a `JigsawPiece`
- Generate all tile variations through rotation and flipping
- Compare tile edges against every other tile
- Count how many other tiles each tile can match
- Identify the corner tiles as those with only two matching neighbours
- Multiply their IDs together

---

## 🧩 Part 2

Assemble the complete image, detect sea monsters, and return the number of `#` pixels not part of any monster.

### 💡 Approach

- Reuse the parsed tile set
- Pick one corner tile
- Find the corner variation that works as the top-left start
- Assemble all tile variations into a full jigsaw by matching edges
- Remove tile borders
- Join the inner 8x8 tile regions into one large image
- Generate variations of that final image
- Scan for the sea monster pattern
- Mark every matched monster cell
- Return the total `#` count minus monster cells

---

## 🧠 Code Breakdown

### `Day20.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Jurassic Jigsaw`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new JurrassicJigsaw(this.Input)`
- Calls `Corners()`

For Part 2:

- Creates `new JurrassicJigsaw(this.Input)`
- Calls `NotSeaMonster()`

---

### `JurrassicJigsaw.cs`

This class provides the two puzzle-facing operations:

- `Corners()`
- `NotSeaMonster()`

It also contains:

- the hard-coded `SeaMonster` pattern
- `DrawMonster(...)`
- `Print(...)`

The sea monster is stored as a `3 x 20` integer pattern where:

- `1` marks required monster pixels
- `0` marks irrelevant positions

---

### `Corners()`

This method solves Part 1.

It:

- parses the input into a `JigsawSolver`
- gets the corner pieces with `solver.Corners()`
- multiplies the returned tile IDs together

It builds the result incrementally, using the first corner ID as the starting value and multiplying by each later corner ID.

So the silver answer is the product of the detected corner tile IDs.

---

### `NotSeaMonster()`

This method solves Part 2.

It:

- parses the tiles with `JigsawSolver.Parse(this.Input)`
- loops through the detected corners
- finds a valid top-left orientation for one corner
- assembles the whole puzzle
- removes borders from every tile
- joins the pieces into a single image
- creates a `JigsawPiece` from that final image
- scans the final image variations for sea monsters
- returns the roughness value once monsters are found

If no sea monster is found in any checked orientation, it returns:

    -1

---

### Sea Monster Detection

The final combined image is treated as a single `JigsawPiece`:

    new JigsawPiece(0, puzzle, 24)

The solver then loops through that image's variations and scans every possible `3 x 20` window.

For each candidate window it:

- compares the image against the `SeaMonster` pattern
- counts how many positions match where the monster requires `1`
- treats a full match as:

    15

monster pixels

When a match is found, it calls:

- `DrawMonster(puzzle, row, column)`

which rewrites those monster cells to `2`.

At the end, the method returns:

    total - (count * 15)

So the gold answer is the number of `#` cells not belonging to any sea monster.

---

### `DrawMonster(...)`

This helper writes matched sea monsters into the puzzle image.

For every `1` in the sea monster pattern, it changes the corresponding cell in the final image to:

    2

This is used purely for marking detected monster pixels after a successful match.

---

### `JigsawSolver.cs`

This class inherits from a dictionary of tile ID to `JigsawPiece`.

It contains the core solving logic:

- `Parse(string[] input)`
- `Corners()`
- `FindTopLeftVariation(...)`
- `Assemble(...)`

This class is responsible for:

- loading the tiles
- identifying corners
- orienting the start tile
- building the full jigsaw layout

---

### Parsing Tiles

`Parse(string[] input)` reads the raw tile blocks.

It tracks:

- `tileId`
- `y`
- `squares`

For each input line:

- blank line finishes the current tile and adds it to the solver
- `Tile ####:` starts a new tile
- `#` is stored as `1`
- `.` remains `0`

Each finished tile becomes:

- `new JigsawPiece(tileId, squares)`

So the input is converted into a set of 10x10 binary tile grids.

---

### Finding Corner Tiles

`Corners()` compares every piece against every other piece.

For each tile pair, it tests all tile variations and checks whether any edges match using:

- `TopToBottomEdge(...)`
- `BottomToTopEdge(...)`
- `LeftToRightEdge(...)`
- `RightToLeftEdge(...)`

It tracks processed tile pairs so the same tile pair is not counted repeatedly.

If a tile matches no more than two other tiles, it is treated as a corner and added to the result.

So corner detection is based on edge-match count across all tile orientations.

---

### `FindTopLeftVariation(...)`

This method determines which variation of a corner tile should be used as the top-left tile.

It checks each variation of the chosen corner against every other tile variation.

If a candidate corner variation has no matching tile:

- above it
- to its left

then that variation is returned as the valid top-left orientation.

This is what lets the assembly begin from `(0, 0)` in a consistent layout.

---

### `Assemble(...)`

This method builds the full jigsaw from the chosen starting variation.

It:

- creates a new `Jigsaw`
- places the start tile at `(0, 0)`
- repeatedly loops until every tile has been placed

For each unplaced tile and each of its variations, it compares that variation against already-placed pieces.

If an edge matches, it places the new tile adjacent to the matched piece:

- below if `TopToBottomEdge(...)`
- above if `BottomToTopEdge(...)`
- right if `LeftToRightEdge(...)`
- left if `RightToLeftEdge(...)`

The method stops once the number of placed pieces matches the total number of tiles.

---

### `Jigsaw.cs`

This class stores the assembled puzzle layout.

It tracks:

- `Pieces`
- `Rows`
- `Columns`
- `Size`

Key methods are:

- `AddPiece(...)`
- `RemoveEdges()`
- `JoinPieces()`
- `Print(...)`

---

### `AddPiece(...)`

This method stores a placed tile variation at a `(y, x)` coordinate.

It also updates:

- `Rows`
- `Columns`

so the jigsaw knows its current assembled bounds.

---

### `RemoveEdges()`

Once the tile layout is complete, this method strips away the borders from every tile.

It changes the working tile size from:

    10

to:

    8

Then, for each tile, it copies only the inner cells:

- rows `1` through `8`
- columns `1` through `8`

into `piece.NoBorders`

So the outer tile frame is discarded before building the final image.

---

### `JoinPieces()`

This method creates one large image by stitching together the borderless tile interiors.

It allocates a final square sized from:

- tile inner size
- number of assembled rows
- number of assembled columns

Then for every placed piece it copies `NoBorders` into the correct position in the output array.

The result is a single combined image that Part 2 later scans for sea monsters.

---

### `JigsawPiece.cs`

This class represents one tile and precomputes its variations.

It stores:

- `Id`
- `Variations`

The constructor creates these variations:

- initial
- flipped horizontally
- flipped vertically
- rotated 90
- rotated 90 flipped horizontally
- rotated 90 flipped vertically
- rotated 180
- rotated 270

So each tile is prepared in every orientation needed by the solver.

---

### `JigsawPieceVariation.cs`

This class represents one specific tile orientation.

It stores:

- `Id`
- `Square`
- `Orientation`
- `Size`
- `NoBorders`

It also provides the edge-comparison helpers:

- `TopToBottomEdge(...)`
- `BottomToTopEdge(...)`
- `RightToLeftEdge(...)`
- `LeftToRightEdge(...)`

Each helper compares the corresponding border cells between two tile variations and returns `true` only if the full edge matches.

---

### `Orientation.cs`

This enum labels the tile states:

- `Initial`
- `FlippedVertically`
- `FlippedHorizontally`
- `Rotated90`
- `Rotated90FlippedVertically`
- `Rotated90FlippedHorizontally`
- `Rotated180`
- `Rotated270`

This gives each generated tile variation an explicit orientation label.

---

## 🛠 Implementation Notes

- The puzzle uses binary tile grids where `# = 1` and `.= 0`
- Tiles are represented as `JigsawPiece` objects with precomputed variations
- Corner detection is based on how many other tiles can match a tile's edges
- The full image is assembled by edge matching against already placed pieces
- Tile borders are removed before building the final image
- Sea monsters are marked by writing `2` into the joined image
- The final roughness is total filled pixels minus all sea-monster pixels

---

## 🧪 Behaviour Summary

Given a set of image tiles:

- the solver parses each tile into a square grid
- generates rotated and flipped variations
- identifies which tiles are corners
- assembles the full image from matching edges
- removes tile borders
- joins the inner tile content into one final image
- searches that image for sea monsters
- returns either the corner-ID product or the remaining roughness value

---

## 🚀 Key Takeaways

- Good example of solving a tile-matching puzzle with rotations and flips
- Separates tile data, tile variations, solver logic, and assembled puzzle state
- Part 1 uses neighbour-match counting to identify corners
- Part 2 converts the full solved jigsaw into one image and scans for patterns
- Edge matching and image stitching are kept in focused helper classes

---

## 🔗 References

- https://adventofcode.com/2020/day/20