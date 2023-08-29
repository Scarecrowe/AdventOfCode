namespace AdventOfCode.Core.Extensions
{
    public static class IntArrayExtensions
    {
        public static int[,] RotateClockWise(this int[,] matrix, int length)
        {
            int[,] result = new int[length, length];

            for (int y = 0; y < length; ++y)
            {
                for (int x = 0; x < length; ++x)
                {
                    result[y, x] = matrix[length - x - 1, y];
                }
            }

            return result;
        }

        public static int[,] FlipHorizontally(this int[,] matrix, int length)
        {
            int[,] result = new int[length, length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    result[y, x] = matrix[y, (length - 1) - x];
                }
            }

            return result;
        }

        public static int[,] FlipVertically(this int[,] matrix, int length)
        {
            int[,] result = new int[length, length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    result[y, x] = matrix[(length - 1) - y, x];
                }
            }

            return result;
        }

        public static int[,] Trim(this int[,] array, int row, int column)
        {
            int[,] result = new int[array.GetLength(0) - 1, array.GetLength(1) - 1];

            for (int i = 0, j = 0; i < array.GetLength(0); i++)
            {
                if (i == row)
                {
                    continue;
                }

                for (int k = 0, u = 0; k < array.GetLength(1); k++)
                {
                    if (k == column)
                    {
                        continue;
                    }

                    result[j, u] = array[i, k];
                    u++;
                }

                j++;
            }

            return result;
        }

        public static int SumByIndex(this int[] array, params int[] indexes)
        {
            int result = 0;

            foreach(int index in indexes)
            {
                if (index >= 0 && index < array.Length)
                {
                    result += array[index];
                }
            }

            return result;
        }

        public static int ProductByIndex(this int[] array, params int[] indexes)
        {
            if (array.Length == 0)
            {
                return 0;
            }

            int result = array[indexes[0]];

            for(int i = 1; i < indexes.Length; i++)
            {
                if (i >= 1 && i < array.Length)
                {
                    result *= array[indexes[i]];
                }
            }

            return result;
        }

        public static int SumRange(this int[] array, int start, int end)
        {
            int result = 0;

            for (int i = start; i <= end; i++)
            {
                if (i >= start && i < array.Length)
                {
                    result += array[i];
                }
            }

            return result;
        }

        public static int ProductRange(this int[] array, int start, int end)
        {
            if (array.Length == 0)
            {
                return 0;
            }

            int result = array[start];

            for (int i = start + 1; i <= end; i++)
            {
                if (i >= start && i < array.Length)
                {
                    result *= array[i];
                }
            }

            return result;
        }
    }
}
