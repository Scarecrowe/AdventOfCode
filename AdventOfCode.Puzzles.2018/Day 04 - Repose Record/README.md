# 🎄 Advent of Code 2018 - Day 04: Repose Record

## 📜 Puzzle Overview

This puzzle is based on analysing guard sleep logs.

You are given a list of timestamped records describing:

- when a guard begins their shift
- when they fall asleep
- when they wake up

An example input looks like:

    [1518-11-01 00:00] Guard #10 begins shift
    [1518-11-01 00:05] falls asleep
    [1518-11-01 00:25] wakes up

However, the records are **not guaranteed to be in chronological order**, so sorting is required before processing.

The goal is to analyse sleep patterns across all guards.

- Part 1 finds the guard who sleeps the most
- Part 2 finds the guard most frequently asleep on the same minute

---

## 🧩 Part 1

Determine which guard has the most total minutes asleep, and which minute they are most frequently asleep on.

### 💡 Approach

- Parse all log entries
- Sort them chronologically
- Track the current guard on duty
- For each sleep period:
  - record the range of minutes asleep
- Build:
  - total minutes asleep per guard
  - frequency of each minute (0–59) per guard
- Find:
  - the guard with the highest total sleep time
  - the minute they are most often asleep
- Return:

    guard_id * minute

---

## 🧩 Part 2

Determine which guard is most frequently asleep on the same minute.

### 💡 Approach

- Reuse the same parsed and sorted data
- Track sleep minutes per guard as before
- Instead of total sleep:
  - find the highest frequency of any minute across all guards
- Return:

    guard_id * minute

---

## 🧠 Code Breakdown

### `Day04.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Repose Record`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Processes guard sleep data
- Finds the guard with the highest total sleep time

For Part 2:

- Reuses the same processed data
- Finds the guard most frequently asleep on a specific minute

---

### Input Structure

Each line represents a log entry with:

- timestamp
- action (begin shift / sleep / wake)

Important detail:

- Guard ID only appears on "begins shift" lines
- Sleep/wake lines must be associated with the current guard

---

### Sorting the Logs

The first step is to sort all input lines.

Because timestamps follow a consistent format:

    [YYYY-MM-DD HH:MM]

A simple lexicographical sort correctly orders them chronologically.

---

### Parsing Records

Each line is interpreted into structured data:

- timestamp (minute is most important)
- event type:
  - begins shift
  - falls asleep
  - wakes up
- guard ID (when available)

Typical parsing flow:

- if line contains "Guard":
  - extract guard ID
- if "falls asleep":
  - record start minute
- if "wakes up":
  - record end minute

---

### Tracking Sleep Data

The solver builds two key structures:

1. **Total sleep per guard**

    guardId -> total minutes asleep

2. **Minute frequency per guard**

    guardId -> array[60] (count per minute)

For each sleep interval:

    for minute in start..end:
        increment guard's minute counter
        increment total sleep

---

### Finding the Sleepiest Guard (Part 1)

Steps:

- identify guard with maximum total sleep
- within that guard:
  - find the minute with the highest count

This gives:

    guard_id * most_common_minute

---

### Finding the Most Frequent Minute (Part 2)

Instead of total sleep:

- iterate all guards
- find the (guard, minute) pair with the highest frequency

This directly gives:

    guard_id * minute

---

### State Tracking

The solver effectively operates as a simple state machine:

- current guard ID
- current sleep start minute

Transitions:

- "Guard begins shift" → update guard
- "falls asleep" → store start minute
- "wakes up" → process interval

This pattern is commonly used when parsing log-style data.

---

## 🛠 Implementation Notes

- Input must be sorted before processing
- Guard ID persists across subsequent log lines
- Sleep intervals are always within the midnight hour (00:00–00:59)
- A fixed-size array (60) is ideal for minute tracking
- Part 2 reuses the same data structures as Part 1
- Efficient counting avoids recomputation

---

## 🧪 Behaviour Summary

Given unordered guard logs:

- the solver sorts all records
- assigns sleep intervals to the correct guard
- accumulates total sleep and per-minute frequencies
- Part 1 finds the guard with the most total sleep
- Part 2 finds the guard most frequently asleep on a specific minute
- both answers return:

    guard_id * minute

---

## 🚀 Key Takeaways

- Classic log parsing problem with ordering requirements
- Sorting simplifies the entire problem space
- State-machine style parsing is highly effective here
- Frequency counting enables both puzzle parts
- Reusing computed data avoids duplication
- Strong example of transforming messy input into structured data

---

## 🔗 References

- https://adventofcode.com/2018/day/4