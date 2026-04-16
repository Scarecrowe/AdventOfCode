# 🎄 Advent of Code 2015 - Day 06: Probably a Fire Hazard

## 📜 Puzzle Overview

Santa is controlling a 1000x1000 grid of lights.

Each instruction affects a rectangular region of the grid:

- `turn on`
- `turn off`
- `toggle`

Coordinates are given in the format:

- `x1,y1 through x2,y2`

Each instruction modifies all lights within that region.

---

## 🧩 Part 1

Determine how many lights are **on** after processing all instructions.

### 💡 Approach

- Represent the grid as a 2D structure
- Iterate through each instruction
- Apply the operation to every coordinate in the defined range:
  - `turn on` → set to `on`
  - `turn off` → set to `off`
  - `toggle` → invert current state
- Count how many lights are on at the end

---

## 🧩 Part 2

Each light now has a **brightness level** instead of a simple on/off state.

- `turn on` → increase brightness by `1`
- `turn off` → decrease brightness by `1` (minimum `0`)
- `toggle` → increase brightness by `2`

Determine the total brightness of all lights.

### 💡 Approach

- Use the same grid structure
- Apply numeric operations instead of boolean:
  - Clamp brightness to `0` when decreasing
- Sum all values at the end

---

## 🧠 Code Breakdown

### `Day6.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates the solver in boolean mode
- Returns the number of lights that are on

For Part 2:

- Creates the solver in brightness mode
- Returns the total brightness

---

### `ProbablyAFireHazard.cs`

This class handles the grid and instruction processing.

The constructor:

- Receives the input instructions
- Initialises the grid
- Selects the operating mode (on/off or brightness)

Each instruction is parsed into:

- Action (`turn on`, `turn off`, `toggle`)
- Start coordinate
- End coordinate

---

### Instruction Processing

Each instruction is applied by iterating over a rectangular region:

for x in range(x1 → x2)
  for y in range(y1 → y2)

For every coordinate in the region, the selected operation is applied.

---

### Part 1 Logic (On/Off)

- Grid stores boolean values
- Operations:
  - `turn on` → `true`
  - `turn off` → `false`
  - `toggle` → `!value`

The result is calculated by counting all `true` values.

---

### Part 2 Logic (Brightness)

- Grid stores integer values
- Operations:
  - `turn on` → `+1`
  - `turn off` → `-1` (minimum `0`)
  - `toggle` → `+2`

The result is calculated by summing all values in the grid.

---

### Grid Handling

- The grid is processed directly without any spatial optimisation
- Every instruction iterates over its full coordinate range
- This keeps the implementation simple and predictable

---

## 🛠 Implementation Notes

- Input is processed line by line
- Instructions are parsed into structured operations
- The same core logic is reused for both parts
- Behaviour is switched based on the selected mode
- Performance is acceptable due to bounded grid size (1000x1000)

---

## 🧪 Examples

| Instruction | Result |
|------------|--------|
| `turn on 0,0 through 0,0` | 1 light on |
| `toggle 0,0 through 999,999` | all lights toggled |

For brightness mode:

- `turn on` increases total brightness
- `toggle` significantly increases brightness across large regions

---

## 🚀 Key Takeaways

- Good example of separating behaviour using mode switching
- Same grid logic reused for two different rule sets
- Simple nested iteration keeps the implementation easy to follow
- Highlights trade-off between simplicity and performance

---

## 🔗 References

- https://adventofcode.com/2015/day/6