# AdventOfCode.Runner

## Overview

The `AdventOfCode.Runner` project is the command-line interface responsible for executing Advent of Code puzzles in this repository.  
It provides both **interactive mode** and **direct command-line execution** for any year, day, and input batch.

This runner is designed to make testing, debugging, and benchmarking puzzles fast and consistent across all years.

---

## Features

### ✔️ Run any puzzle directly  
Specify year, day and batch using simple command-line arguments:

```
-year <YEAR> -day <DAY> -batch <BATCH>
```

### ✔️ Interactive mode  
When no arguments are supplied, the runner can prompt the user to select:

- Year  
- Day  
- Batch  

…making it easy to explore puzzles without scripting.

### ✔️ Integration with Core  
The runner uses `AdventOfCode.Core` to handle:

- Input loading  
- Batch selection  
- Common puzzle interfaces  
- Shared utilities

### ✔️ Automatic puzzle discovery  
Depending on the implementation, the runner automatically detects puzzle classes through your chosen pattern (reflection, naming conventions, or registration).

---

## Command-Line Usage

### **Direct Execution**

Run a specific puzzle:

```
-year <YEAR> -day <DAY> -batch <BATCH>
```

Example:

```
-year 2025 -day 5 -batch 1
```

**Arguments meaning:**

| Argument | Description |
|---------|-------------|
| `-year` | Advent of Code year (e.g., 2025) |
| `-day`  | Puzzle day (1–25) |
| `-batch` | Which input batch to load (useful for variations, performance tests, etc.) |

### **Interactive Mode**

Simply run the Runner without arguments:

```
AdventOfCode.Runner.exe
```

The runner will prompt you for:

- Year (list of available years)  
- Day (list of implemented days for that year)  
- Batch  

---

## Typical Flow

1. Parse command-line options or prompt the user.  
2. Load puzzle metadata and locate the correct puzzle class.  
3. Load input via `AdventOfCode.Core` (with correct batch).  
4. Execute part 1 (“Silver”), then part 2 (“Gold”).  
5. Output results, timings and optional diagnostics.

---

## Example Output

```

     *        *        *        *        *        *        *        *        *        *        *        *        *
    /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\      /.\
   /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\    /..'\
   /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\    /'.'\
  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\  /.''.'\
  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\  /.'.'.\
 /'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\/'.''.'.\
 ^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^^^^[_]^^^

 Advent Of Code 2025 Day 5: Cafeteria

 Silver: 652

        Avg: 9.7999ms
        Min: 9.7999ms
        Max: 9.7999ms

 Gold: 341753674214273

        Avg: 12.9363ms
        Min: 12.9363ms
        Max: 12.9363ms

 Executed once:

```

---

## Extending the Runner

If you add new years, days or puzzle types:

- Ensure the puzzle classes follow the required interface (or naming pattern).  
- The runner will automatically discover them (if using reflection).  
- Otherwise, add them to the registration table factory inside the Runner.

You can also enhance the runner to include:

- Benchmark mode  
- Animation triggering  
- Custom output formatting  
- Local caching of results  

---

## License

This project follows the same MIT License as the rest of the repository.  
See the root `license.txt` for details.

