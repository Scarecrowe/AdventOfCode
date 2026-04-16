namespace AdventOfCode.Puzzles._2025.Day_10___Factory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public sealed class Factory
    {
        private readonly Problem[] problems;

        public Factory(string[] input)
        {
            this.problems = input.Select(ParseLine).ToArray();
        }

        public long TotalMinimumPresses()
            => this.problems.Sum(p => MinimumPresses(p));

        public long TotalReducedPresses()
            => this.problems.Sum(p => ReducedPresses(p));

        private static Problem ParseLine(string line)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string targetToken = tokens[0];
            string joltageToken = tokens[^1];
            string[] buttonTokens = tokens[1..^1];

            int lightCount = targetToken.Length - 2;
            int targetMask = 0;

            for (int i = 1; i < targetToken.Length - 1; i++)
            {
                if (targetToken[i] == '#')
                {
                    targetMask |= 1 << (i - 1);
                }
            }

            int[] buttonMasks = new int[buttonTokens.Length];
            int[][] buttons = new int[buttonTokens.Length][];

            for (int i = 0; i < buttonTokens.Length; i++)
            {
                string inner = buttonTokens[i][1..^1];
                int[] indices = inner.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                    .ToArray();

                buttons[i] = indices;

                int mask = 0;
                foreach (int bit in indices)
                {
                    mask |= 1 << bit;
                }

                buttonMasks[i] = mask;
            }

            int[] joltages = joltageToken[1..^1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();

            return new Problem(lightCount, targetMask, buttonMasks, buttons, joltages);
        }

        private static int MinimumPresses(Problem problem)
        {
            int totalStates = 1 << problem.LightCount;
            int[] distance = Enumerable.Repeat(-1, totalStates).ToArray();
            Queue<int> queue = new();

            distance[0] = 0;
            queue.Enqueue(0);

            while (queue.Count > 0)
            {
                int state = queue.Dequeue();
                int nextDistance = distance[state] + 1;

                if (state == problem.TargetMask)
                {
                    return distance[state];
                }

                foreach (int buttonMask in problem.ButtonMasks)
                {
                    int next = state ^ buttonMask;
                    if (distance[next] != -1)
                    {
                        continue;
                    }

                    distance[next] = nextDistance;
                    queue.Enqueue(next);
                }
            }

            throw new InvalidOperationException("Target state is unreachable.");
        }

        private static int ReducedPresses(Problem problem)
        {
            int rows = problem.Joltages.Length;
            int columns = problem.Buttons.Length;

            double[,] matrix = new double[rows, columns + 1];

            for (int r = 0; r < rows; r++)
            {
                matrix[r, columns] = problem.Joltages[r];
            }

            for (int c = 0; c < columns; c++)
            {
                foreach (int r in problem.Buttons[c])
                {
                    matrix[r, c] = 1.0;
                }
            }

            RrefResult rref = ToRref(matrix, rows, columns);

            if (!IsConsistent(matrix, rows, columns))
            {
                throw new InvalidOperationException("No integer solution exists.");
            }

            int[] pivotColumns = rref.PivotColumns.ToArray();
            bool[] isPivot = new bool[columns];
            foreach (int c in pivotColumns)
            {
                isPivot[c] = true;
            }

            int[] freeColumns = Enumerable.Range(0, columns).Where(c => !isPivot[c]).ToArray();

            if (freeColumns.Length == 0)
            {
                int[] solution = SolvePivotVariablesOnly(matrix, rows, columns, pivotColumns);
                return solution.Sum();
            }

            int best = int.MaxValue;

            int[] current = new int[columns];
            int[] upperBounds = ComputeUpperBounds(problem);

            SearchFreeVariables(
                matrix,
                rows,
                columns,
                pivotColumns,
                freeColumns,
                upperBounds,
                0,
                current,
                ref best);

            if (best == int.MaxValue)
            {
                throw new InvalidOperationException("No non-negative integer solution exists.");
            }

            return best;
        }

        private static int[] ComputeUpperBounds(Problem problem)
        {
            int columns = problem.Buttons.Length;
            int[] bounds = new int[columns];

            for (int c = 0; c < columns; c++)
            {
                int min = int.MaxValue;

                foreach (int row in problem.Buttons[c])
                {
                    min = Math.Min(min, problem.Joltages[row]);
                }

                bounds[c] = min == int.MaxValue ? 0 : min;
            }

            return bounds;
        }

        private static void SearchFreeVariables(
            double[,] matrix,
            int rows,
            int cols,
            int[] pivotColumns,
            int[] freeColumns,
            int[] upperBounds,
            int depth,
            int[] current,
            ref int best)
        {
            if (depth == freeColumns.Length)
            {
                int[] solved = (int[])current.Clone();

                foreach (int pivotCol in pivotColumns)
                {
                    int pivotRow = FindPivotRow(matrix, rows, pivotCol);
                    double value = matrix[pivotRow, cols];

                    foreach (int freeCol1 in freeColumns)
                    {
                        value -= matrix[pivotRow, freeCol1] * solved[freeCol1];
                    }

                    if (!IsNearInteger(value))
                    {
                        return;
                    }

                    int intValue = (int)Math.Round(value);
                    if (intValue < 0)
                    {
                        return;
                    }

                    solved[pivotCol] = intValue;
                }

                int total = solved.Sum();
                if (total < best)
                {
                    best = total;
                }

                return;
            }

            int freeCol = freeColumns[depth];

            int partialSum = 0;
            for (int i = 0; i < cols; i++)
            {
                partialSum += current[i];
            }

            int max = upperBounds[freeCol];
            if (partialSum >= best)
            {
                return;
            }

            max = Math.Min(max, best - partialSum - 1);

            for (int value = 0; value <= max; value++)
            {
                current[freeCol] = value;
                SearchFreeVariables(
                    matrix,
                    rows,
                    cols,
                    pivotColumns,
                    freeColumns,
                    upperBounds,
                    depth + 1,
                    current,
                    ref best);
            }

            current[freeCol] = 0;
        }

        private static int[] SolvePivotVariablesOnly(
            double[,] matrix,
            int rows,
            int cols,
            int[] pivotColumns)
        {
            int[] solution = new int[cols];

            foreach (int pivotCol in pivotColumns)
            {
                int pivotRow = FindPivotRow(matrix, rows, pivotCol);
                double value = matrix[pivotRow, cols];

                if (!IsNearInteger(value))
                {
                    throw new InvalidOperationException("Non-integer solution encountered.");
                }

                int intValue = (int)Math.Round(value);
                if (intValue < 0)
                {
                    throw new InvalidOperationException("Negative solution encountered.");
                }

                solution[pivotCol] = intValue;
            }

            return solution;
        }

        private static RrefResult ToRref(double[,] matrix, int rows, int cols)
        {
            const double epsilon = 1e-9;
            List<int> pivotColumns = new();

            int pivotRow = 0;

            for (int col = 0; col < cols && pivotRow < rows; col++)
            {
                int bestRow = -1;
                double bestAbs = epsilon;

                for (int r = pivotRow; r < rows; r++)
                {
                    double abs = Math.Abs(matrix[r, col]);
                    if (abs > bestAbs)
                    {
                        bestAbs = abs;
                        bestRow = r;
                    }
                }

                if (bestRow == -1)
                {
                    continue;
                }

                SwapRows(matrix, cols + 1, pivotRow, bestRow);

                double pivot = matrix[pivotRow, col];
                for (int c = col; c <= cols; c++)
                {
                    matrix[pivotRow, c] /= pivot;
                }

                for (int r = 0; r < rows; r++)
                {
                    if (r == pivotRow)
                    {
                        continue;
                    }

                    double factor = matrix[r, col];
                    if (Math.Abs(factor) <= epsilon)
                    {
                        continue;
                    }

                    for (int c = col; c <= cols; c++)
                    {
                        matrix[r, c] -= factor * matrix[pivotRow, c];
                    }
                }

                pivotColumns.Add(col);
                pivotRow++;
            }

            return new RrefResult(pivotColumns);
        }

        private static bool IsConsistent(double[,] matrix, int rows, int cols)
        {
            const double epsilon = 1e-9;

            for (int r = 0; r < rows; r++)
            {
                bool allZero = true;
                for (int c = 0; c < cols; c++)
                {
                    if (Math.Abs(matrix[r, c]) > epsilon)
                    {
                        allZero = false;
                        break;
                    }
                }

                if (allZero && Math.Abs(matrix[r, cols]) > epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        private static int FindPivotRow(double[,] matrix, int rows, int pivotCol)
        {
            const double epsilon = 1e-9;

            for (int r = 0; r < rows; r++)
            {
                if (Math.Abs(matrix[r, pivotCol] - 1.0) <= epsilon)
                {
                    return r;
                }
            }

            throw new InvalidOperationException($"Pivot row not found for column {pivotCol}.");
        }

        private static bool IsNearInteger(double value)
            => Math.Abs(value - Math.Round(value)) <= 1e-9;

        private static void SwapRows(double[,] matrix, int width, int a, int b)
        {
            if (a == b)
            {
                return;
            }

            for (int c = 0; c < width; c++)
            {
                (matrix[a, c], matrix[b, c]) = (matrix[b, c], matrix[a, c]);
            }
        }

        private sealed record Problem(
            int LightCount,
            int TargetMask,
            int[] ButtonMasks,
            int[][] Buttons,
            int[] Joltages);

        private sealed record RrefResult(IReadOnlyList<int> PivotColumns);
    }
}