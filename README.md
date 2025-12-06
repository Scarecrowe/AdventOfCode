# AdventOfCode 🎄

![Solved](https://img.shields.io/badge/Solved-256_puzzles-brightgreen)
![Years](https://img.shields.io/badge/Years-2015→2025-blue)
![Language](https://img.shields.io/badge/Language-C%23-purple)
![Last Update](https://img.shields.io/badge/Updated-2025-lightgrey)

A collection of my solutions for **Advent of Code**, implemented in **C#**.  
This repository includes every year of Advent of Code from **2015 → 2024 fully completed**,  
and **2025 is in progress up to Day 6**.

---

## ✅ Progress Overview

### **Solved Status by Year**

| Year | Days Solved | Status |
|------|-------------|--------|
| **2025** | 6 / 12 | 🚧 In Progress |
| **2024** | 25 / 25 | ✔️ Complete |
| **2023** | 25 / 25 | ✔️ Complete |
| **2022** | 25 / 25 | ✔️ Complete |
| **2021** | 25 / 25 | ✔️ Complete |
| **2020** | 25 / 25 | ✔️ Complete |
| **2019** | 25 / 25 | ✔️ Complete |
| **2018** | 25 / 25 | ✔️ Complete |
| **2017** | 25 / 25 | ✔️ Complete |
| **2016** | 25 / 25 | ✔️ Complete |
| **2015** | 25 / 25 | ✔️ Complete |

---

## 📁 Repository Structure

- **AdventOfCode.Core** – Shared utilities used across puzzles.  
  ➜ 📘 [Core README](./AdventOfCode.Core/README.md)

- **AdventOfCode.Puzzles** – All puzzle implementations, organised by year/day.

- **AdventOfCode.Animations** – Optional visualisations for selected puzzles.  
  ➜ 🎞️ [Animations README](./AdventOfCode.Animations/README.md)

- **AdventOfCode.Runner** – CLI interface for running puzzles directly.  
  ➜ 🚀 [Runner README](./AdventOfCode.Runner/README.md)

- **AdventOfCode.Test** – Unit tests and input validation helpers.

- Standard `.gitignore`, `.ruleset` and `.sln` structure for a multi-project C# solution.

---

## ▶️ Running Puzzles

You can run puzzles using the runner project → see the [AdventOfCode.Runner README](./AdventOfCode.Runner/README.md) for usage instructions.

### **1. Interactive Mode**
Simply run the Runner project in your IDE.

### **2. Command-Line Arguments**

You can execute a *specific* puzzle directly:

```
-year <YEAR> -day <DAY> -batch <BATCH>
```

**Example:**

```
-year 2025 -day 5 -batch 1
```

**Argument meanings:**

- `-year` → The Advent of Code year  
- `-day` → Puzzle day  
- `-batch` → Which input batch to use (useful when testing variations or performance)  

This makes running puzzles scriptable and ideal for quick testing.

---

## 🎬 Running Animations

Some puzzles include optional **visual animations** to help understand the solution or illustrate the problem.  
These animations are implemented in the [`AdventOfCode.Animations`](./AdventOfCode.Animation/README.md) library.

### How to Run

1. Make sure the `AdventOfCode.Animations` project is built.  
2. Use the runner or directly run the animation project.  
3. Specify the year, day, and batch if needed:

```bash
-year 2018 -day 17 -batch 1
```

The animation will open in a window or generate an output file depending on the puzzle.

### Example Animation

A sample animation for one of the puzzles is available on YouTube:

[![Advent of Code Animation](https://img.youtube.com/vi/DI0rE_SX3Rk/0.jpg)](https://www.youtube.com/watch?v=DI0rE_SX3Rk)

Click the image or the link above to watch the video demonstrating how the animations work.

---

**Notes:**

- Not all puzzles have animations.  
- Animations are mainly for visual understanding; puzzle logic is still solved via the standard Runner/Core workflow.  

---

## 🔧 Adding New Puzzles

- Add puzzle code under the appropriate year/day in `Puzzles`.  
- Add test inputs or examples under `Test`.  
- Add animations if the puzzle benefits from visualisation.  
- Update progress in the table above as desired.  

---

## 📜 License

This project is licensed under the MIT License.  
See `license.txt` for details.

---

## 🎅 What is Advent of Code?

Advent of Code is a yearly December programming challenge by Eric Wastl.  
It provides 25 days (50 parts) of algorithmic puzzles each year.  
This repository serves as my complete archive of solutions, experiments, and animations.