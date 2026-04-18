# 🎄 Advent of Code 2016 - Day 08: Two-Factor Authentication

## 📜 Puzzle Overview

This puzzle simulates a small screen display controlled by text instructions.

The screen consists of:

- 50 columns (width)
- 6 rows (height)

Each pixel is either:

- `on` (lit)
- `off` (unlit)

Instructions modify the screen by turning on rectangles or rotating rows and columns.

Part 1 counts how many pixels are lit.  
Part 2 reads the message displayed on the screen.

---

## 🧩 Part 1

Determine how many pixels are lit after executing all instructions.

### 💡 Approach

- Initialise a 50x6 grid of `false` (off)
- Process each instruction in order:
  - `rect AxB` → turn on a rectangle in the top-left
  - `rotate row y=A by B` → shift row right
  - `rotate column x=A by B` → shift column down
- After all instructions:
  - count all `true` values

---

## 🧩 Part 2

Read the message displayed on the screen after executing all instructions.

### 💡 Approach

- Reuse the final screen state from Part 1
- Render the grid visually:
  - `#` for lit pixels
  - space (or `.`) for unlit pixels
- The output forms readable letters

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Two-Factor Authentication`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Executes all screen operations
- Counts lit pixels

For Part 2:

- Renders the screen output
- Displays or returns the visual message

---

### Screen Representation

The display is typically represented as:

- a 2D array
- or a boolean grid

Example:

```
bool[6][50]
```

Each cell represents a pixel state.

---

### Instruction Types

#### Rectangle

```
rect AxB
```

- Turns on all pixels in a rectangle:
  - width `A`
  - height `B`
- Starts from the top-left corner

---

#### Rotate Row

```
rotate row y=A by B
```

- Shifts all pixels in row `A` to the right
- Wraps around at the edges

---

#### Rotate Column

```
rotate column x=A by B
```

- Shifts all pixels in column `A` downward
- Wraps around at the edges

---

### Applying Rotations

Rotations require wrapping:

- values shifted off one side reappear on the opposite side
- can be implemented using:
  - temporary arrays
  - modular indexing

---

### Part 1 Logic

- Execute each instruction in sequence
- Update the grid state
- Count all lit pixels at the end

---

### Part 2 Logic

- Convert the final grid into text output:

```
#  ##   ##  #### #  # ####  ##  #  # 
# #  # #  # #    #  #    # #  # #  # 
# #    #    ###  ####   #  #    #### 
# # ## #    #    #  #  #   # ## #  # 
# #  # #  # #    #  # #    #  # #  # 
#  ###  ##  #### #  # ####  ### #  # 
```

- Letters are formed using a fixed-width font pattern

---

## 🛠 Implementation Notes

- Grid updates must be applied in order
- Rotations should not overwrite data mid-operation
- Temporary buffers help avoid mutation issues
- Part 2 output may require manual reading or OCR-style mapping
- Performance is trivial due to small grid size

---

## 🧪 Behaviour Summary

Given a list of display instructions:

- The grid starts empty
- Instructions progressively modify pixel states
- Rectangles add lit regions
- Rotations shift existing pixels
- Part 1 counts lit pixels
- Part 2 interprets the visual output

---

## 🚀 Key Takeaways

- Grid-based simulation problem
- Careful handling of rotations and wrapping
- Sequential state updates are critical
- Visual output can encode readable text

---

## 🔗 References

- https://adventofcode.com/2016/day/8