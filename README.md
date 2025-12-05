# AdventOfCode 🎄

![Solved](https://img.shields.io/badge/Solved-215%2B_puzzles-brightgreen)
![Years](https://img.shields.io/badge/Years-2015→2025-blue)
![Language](https://img.shields.io/badge/Language-C%23-purple)
![Last Update](https://img.shields.io/badge/Updated-2025-lightgrey)

A collection of my solutions for **Advent of Code**, implemented in **C#**.  
This repository includes every year of Advent of Code from **2015 → 2024 fully completed**,  
and **2025 is in progress up to Day 5**.

---

## ✅ Progress Overview

### **Solved Status by Year**

| Year | Days Solved | Status |
|------|-------------|--------|
| **2025** | 5 / 25 | 🚧 In Progress |
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

If more years are added in the future, the table can be extended easily.

---

## 📁 Repository Structure

- **AdventOfCode.Core** – Shared utilities used across puzzles.  
- **AdventOfCode.Puzzles** – All puzzle implementations, organised by year/day.  
- **AdventOfCode.Animations** – Optional visualisations for selected puzzles.  
- **AdventOfCode.Runner** – CLI interface for running puzzles directly.  
- **AdventOfCode.Test** – Unit tests and input validation helpers.  
- Standard `.gitignore`, `.ruleset` and `.sln` structure for a multi-project C# solution.

---

## ▶️ Running Puzzles

You can run puzzles using the runner project.

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