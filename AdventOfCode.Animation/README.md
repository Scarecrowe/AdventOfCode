# Advent of Code Animation Engine

This document describes how the Advent of Code animation system in this repository works — the architecture, techniques, rendering pipeline, and how the animation runner ties into your puzzle solutions. It captures the intent, engineering approach, and the low-level mechanics behind the visualisations.

---

## 🎥 Overview
The animation system is a lightweight, custom-built visualisation engine designed to bring Advent of Code puzzle states to life. Rather than producing static outputs, the engine steps through the puzzle logic frame-by-frame, capturing each intermediate state and rendering it to the console or a graphical surface.

The goal is to:
- Visualise grid-based puzzles
- Show algorithmic progress (BFS, Dijkstra, simulations, sand/falling pieces, lights, etc.)
- Debug logic by watching the state unfold
- Make solving AoC more fun and immersive

---

## 🏗 Architecture
The animation system is built around these core components:

### **1. Frame Generator**
Each animated puzzle exposes a function or method that returns a sequence of "frames". A frame represents a snapshot of the puzzle state at a given moment.

A frame typically contains:
- A 2D grid (chars or strings)
- Metadata (step number, active node, score, etc.)
- Optional colour/style hints

The puzzle logic pushes updated frames after each simulation step or algorithmic iteration.

### **2. Renderer**
The Renderer is responsible for taking a frame produced by the puzzle solver and drawing it to the output device.

Common rendering layers include:
- Console text rendering
- ANSI-colour grid drawing
- Clearing the screen between frames
- Timing/delay control for playback speed

The renderer ensures consistent refreshes and smooth playback.

### **3. Animation Loop**
A simple loop runs over all generated frames:

1. Clear screen / prepare buffer
2. Render the frame
3. Delay (e.g. 16–100ms depending on animation speed)
4. Repeat until final frame

This creates a real-time animation of the puzzle solution.

### **4. Puzzle Integration Layer**
Each puzzle that supports animation implements one of these patterns:
- **Simulation loop** → yield a frame each iteration
- **Algorithm visualisation** → yield after visiting nodes
- **Grid evolution** → yield on each update tick

This allows any puzzle to opt-in to animation with minimal overhead.

---

## 🧩 Techniques Used

### **1. Buffered Output Rendering**
To avoid flickering, each frame is composed in memory before being flushed to the output. This ensures each frame redraw is atomic.

### **2. Coordinate-Based Drawing**
Frames use a simple 2D array-based approach:
- `grid[y][x]` contains the character to draw
- Optional mapping from characters → colours/styles

This makes rendering fast, predictable, and language-agnostic.

### **3. ANSI Escape Codes**
Colours, cursor movement, and clearing the screen rely on ANSI escape sequences. This allows visualisation without any external libraries and works cross-platform in compatible terminals.

### **4. Yield-Based Frame Generation**
Puzzle animations use `yield return` style semantics so frames stream naturally during computation. This avoids storing giant histories and keeps memory usage low.

### **5. Time-Stepped Simulation**
The animation engine uses configurable delays between frames, allowing:
- Slow, cinematic playback
- Fast, near-real-time playback
- Pausing or stepping through frames manually

### **6. Non-Blocking Logic Separation**
Puzzle logic and animation logic are separated so the puzzle can still run in a "headless" or non-animated mode.

---

## 🌟 Example Workflow
1. Puzzle logic performs one algorithmic step.
2. Puzzle converts state to a `Frame` object.
3. Puzzle yields the frame.
4. Renderer draws it.
5. Loop continues until done.

This makes animations deterministic and replayable.

---

## 🔧 Why This System Works Well
- Lightweight (no external dependencies)
- Fast enough for large grids
- Debug-friendly
- Works with any puzzle type
- Makes your AoC repo uniquely expressive and fun

Your implementation is particularly nice because it keeps the complexity low while still supporting expressive visuals. The separation of solver → frames → renderer → playback is clean and maintainable.

---

## 📘 Adding a New Animated Puzzle
This section explains how to integrate a new animated puzzle into the system.

### **1. Implement a Frame Model**
Each frame represents one visual snapshot:
```csharp
public class Frame
{
    public char[][] Grid { get; set; }
    public int Step { get; set; }
}
```

### **2. Create a Puzzle Solver That Yields Frames**
Use an iterator to expose animation frames:
```csharp
public IEnumerable<Frame> SolveAnimated()
{
    while (!done)
    {
        UpdateState();
        yield return BuildFrame();
    }
}
```

### **3. Build the Frame**
Typically convert your puzzle state into a 2D grid:
```csharp
private Frame BuildFrame()
{
    var grid = new char[height][];
    for (int y = 0; y < height; y++)
    {
        grid[y] = new char[width];
        for (int x = 0; x < width; x++)
        {
            grid[y][x] = MapCellToChar(cells[y][x]);
        }
    }

    return new Frame
    {
        Grid = grid,
        Step = stepCounter
    };
}
```

### **🖥️ 4. Rendering the Frame**
Your renderer clears the screen, draws the grid, and applies optional ANSI colours:
```csharp
public void Render(Frame frame)
{
    Console.Clear();
    foreach (var row in frame.Grid)
    {
        Console.WriteLine(new string(row));
    }
    Thread.Sleep(50); // frame delay
}
```

---

## 🧩 Architecture Diagram
```
┌───────────────────┐     yields     ┌────────────────┐     draws     ┌───────────────┐
│   Puzzle Solver    │──────────────▶│ Frame Sequence │──────────────▶│   Renderer    │
└───────────────────┘                └────────────────┘               └───────────────┘
         ▲                                   │                                │
         │                                   ▼                                │
         │                           Frame Objects                             │
         │                                   │                                ▼
         └──────────────────────── updates ──┴────────────────────────> Terminal Output
```

---

## ⚙️ Performance Tips
- Prefer fixed-size arrays over dynamic lists for grid rendering.
- Reuse buffers when possible.
- Avoid per-character Console operations — build whole strings instead.
- Keep rendering and logic separated so slow rendering doesn’t affect correctness.

---

## 🎛️ Procedural Generatioion
#### 1.1 Perlids
🌍 World Generationerlin 🏞️ 1.1se controls elevation, heat, and moisture maps.
- These maps are blended to decide biome types (forest, tundra, desert, etc.).
- Smooth gradients ensure natural, non-repeating environments.

#### 1.2 Procedurally Drawn Environment
- Trees, mountains, cl🎨 1.2s, storms, snow, and dust are drawn based on biome.
- Particle systems (snowfall, sparks, sand) react to seed and puzzle context.
- Parallax layers create depth using seed-adjusted offsets.

---

### 2. Seed System
#### 2.1 Seed-Driven World State
The seed aff🎲 Seed Systemrain l🔧 2.1ut
- Weather + lighting
- Camera motion
- NPC spawning rules
- Animation timings
- Puzzle-specific effects

#### 2.2 Deterministic but Unique
Runs with the same seed repr♾️ 2.2ce exactly the same world.
Different seeds → entirely new animations.

---

### 3. NPC Behaviour & Animation
#### 3.1 NPC Generation
- NPCs 🧍‍♂️ NPC Behavioureed-driven pseudo-👤 3.1domness.
- Biome dictates type, density, and behaviour profile.

#### 3.2 Motion + Behaviour
NPCs use:
- Perlin-noise‑sampled d👣 3.2ction vectors for natural wandering
- State loops (idle, walk, observe)
- Interactions with terrain or events

Each run feels alive and organic but stays deterministic per seed.

---

### 4. Rendering & Animation Flow
- Frame interpolation smooths all movement.
- Procedural events trigger based on puzzle input.
- Lightweight rendering ensures consistent playback.

---

## 📊 Diagram: World Generation Pipeline
```
        ┌──────────────┐       ┌──────────────┐
        │  Seed Value   │──────▶│  PRNG Engine  │
        └──────┬───────┘       └──────┬───────┘
               │                      │
               ▼                      ▼
      ┌────────────────┐     ┌──────────────────┐
      │ Perlin Noise   │     │  Puzzle Input    │
      │ (Multi-Octave) │     └───────┬──────────┘
      └──────┬─────────┘             │
             │                       │
             ▼                       ▼
   ┌─────────────────┐      ┌──────────────────────┐
   │ Biome Maps      │      │ Procedural Events    │
   └──────┬──────────┘      └──────────┬───────────┘
          │                             │
          ▼                             ▼
   ┌─────────────────┐       ┌─────────────────────┐
   │ World Renderer  │◀──────┤ NPC Behaviour Engine │
   └─────────────────┘       └─────────────────────┘
```

## 🏞️ Diagram: Biome Classification Flow
```
          Perlin Noise Layers
      ┌────────┬────────┬────────┐
      │ Height │ Heat   │ Moist. │
      └────┬───┴───┬────┴───┬────┘
           │       │        │
           ▼       ▼        ▼
    ┌────────────────────────────┐
    │  Biome Rules / Thresholds  │
    └──────────────┬─────────────┘
                   ▼
      ┌────────────────────────┐
      │   Final Biome Type     │
      └────────────────────────┘
```

## 🧍 Diagram: NPC Behaviour State Machine
```
           ┌──────────┐
           │   Idle    │
           └────┬─────┘
                │
      Perlin Noise Direction
                │
                ▼
         ┌───────────┐
         │   Walk     │
         └────┬──────┘
              │
          Random Event
              │
      ┌───────▼────────┐
      │   Observe       │
      └────────┬────────┘
               │
               ▼
           (loops back)
```
