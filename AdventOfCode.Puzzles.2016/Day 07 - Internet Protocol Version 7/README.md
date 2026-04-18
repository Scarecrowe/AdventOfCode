# 🎄 Advent of Code 2016 - Day 07: Internet Protocol Version 7

## 📜 Puzzle Overview

This puzzle analyses IPv7 addresses to determine support for specific protocols.

Each address consists of sequences:

- supernet sequences (outside square brackets)
- hypernet sequences (inside square brackets)

Example:

```
abba[mnop]qrst
```

- `abba`, `qrst` → supernet
- `mnop` → hypernet

Part 1 checks for TLS support using ABBA patterns.  
Part 2 checks for SSL support using ABA and BAB patterns.

---

## 🧩 Part 1

Determine how many IPs support TLS (Transport-Layer Snooping).

### 💡 Approach

An IP supports TLS if:

- it contains an **ABBA pattern** in any supernet sequence
- it does **not** contain an ABBA pattern in any hypernet sequence

An ABBA is defined as:

```
xyyx
```

Where:

- `x != y`

Examples:

- `abba` → valid
- `aaaa` → invalid

Steps:

- Split input into supernet and hypernet sequences
- Check each segment for ABBA patterns
- Validate conditions:
  - at least one ABBA outside brackets
  - no ABBA inside brackets

---

## 🧩 Part 2

Determine how many IPs support SSL (Super-Secret Listening).

### 💡 Approach

An IP supports SSL if:

- it contains an **ABA pattern** in a supernet sequence
- a corresponding **BAB pattern** exists in any hypernet sequence

Definitions:

```
ABA → xyx
BAB → yxy
```

Example:

- ABA: `aba`
- Corresponding BAB: `bab`

Steps:

- Extract all ABA patterns from supernet sequences
- Convert each ABA into its corresponding BAB
- Check if any BAB exists within hypernet sequences

---

## 🧠 Code Breakdown

### `Day7.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Internet Protocol Version 7`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Parses address segments
- Checks for ABBA patterns

For Part 2:

- Extracts ABA patterns
- Searches for matching BAB patterns

---

### Parsing the Address

Each line is split into:

- supernet sequences (outside brackets)
- hypernet sequences (inside brackets)

This can be done by:

- iterating through characters
- tracking whether currently inside brackets

---

### Detecting ABBA

Scan each sequence for four-character windows:

```
s[i] == s[i+3]
s[i+1] == s[i+2]
s[i] != s[i+1]
```

If all conditions are met, an ABBA is found.

---

### Part 1 Logic

- Check all supernet sequences for ABBA
- Ensure no hypernet sequence contains ABBA
- Count valid IPs

---

### Detecting ABA

Scan for three-character windows:

```
s[i] == s[i+2]
s[i] != s[i+1]
```

Each match produces an ABA pattern.

---

### Generating BAB

From each ABA:

```
ABA: xyx
BAB: yxy
```

These BAB patterns are used to search hypernet sequences.

---

### Part 2 Logic

- Collect all ABA patterns from supernet sequences
- Convert to BAB patterns
- Check if any BAB exists in hypernet sequences
- Count valid IPs

---

## 🛠 Implementation Notes

- String parsing is central to the solution
- Sliding window checks simplify pattern detection
- Separate handling of supernet and hypernet sequences is critical
- Pattern generation in Part 2 avoids repeated scanning logic
- Efficient string searching improves performance

---

## 🧪 Behaviour Summary

Given a list of IPv7 addresses:

- Each address is split into bracketed and non-bracketed parts
- Part 1 checks for ABBA patterns with exclusion rules
- Part 2 checks for ABA/BAB pattern relationships
- Only addresses meeting the criteria are counted

---

## 🚀 Key Takeaways

- Pattern recognition using sliding windows
- Importance of separating different input contexts
- Reuse of pattern logic across parts
- Part 2 builds on Part 1 with transformed pattern matching

---

## 🔗 References

- https://adventofcode.com/2016/day/7