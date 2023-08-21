namespace AdventOfCode.Puzzles._2021.Day_04___Giant_Squid
{
    using AdventOfCode.Core.Extensions;

    public class GiantSquid
    {
        public static int First(string[] input)
        {
            List<int> numbers = ParseNumbers(input);
            List<int[,]> boards = ParseBoards(input);

            foreach (int number in numbers)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        foreach (int[,] board in boards)
                        {
                            if (board[row, column] == number)
                            {
                                board[row, column] *= -1;
                            }
                        }
                    }
                }

                int winningBoard = MatchRow(boards, false);

                if (winningBoard == -1)
                {
                    winningBoard = MatchColumn(boards, false);
                }

                if (winningBoard > -1)
                {
                    return Score(boards[winningBoard], number);
                }
            }

            return -1;
        }

        public static int Last(string[] input)
        {
            List<int> numbers = ParseNumbers(input);
            List<int[,]> boards = ParseBoards(input);

            var b = boards[0];
            int lastNumber = 0;
            bool zero = false;

            foreach (int number in numbers)
            {
                if (number == 0 && !zero)
                {
                    zero = true;
                }

                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        foreach (int[,] board in boards)
                        {
                            if (board[row, column] == number)
                            {
                                board[row, column] *= -1;
                            }
                        }
                    }
                }

                bool canLoop = true;

                while (canLoop)
                {
                    int winningBoard = MatchRow(boards, zero);

                    if (winningBoard == -1)
                    {
                        winningBoard = MatchColumn(boards, zero);
                    }

                    if (winningBoard > -1)
                    {
                        b = (int[,])boards[winningBoard].Clone();
                        lastNumber = number;

                        boards.RemoveAt(winningBoard);

                        if (boards.Count == 0)
                        {
                            return Score(b, number);
                        }
                    }
                    else
                    {
                        canLoop = false;
                    }
                }
            }

            return Score(b, lastNumber);
        }

        private static List<int> ParseNumbers(string[] input) => input[0].Split(',').ToIntList();

        private static List<int[,]> ParseBoards(string[] input)
        {
            List<int[,]> boards = new();

            int[,] board = new int[5, 5];
            int row = 0;

            for (int i = 2; i < input.Length; i++)
            {
                if (input[i] == string.Empty)
                {
                    row = 0;
                    boards.Add(board);
                    board = new int[5, 5];
                    continue;
                }

                int[] boardNumbers = input[i].SplitSpace().ToInt();

                for (int column = 0; column < 5; column++)
                {
                    board[row, column] = boardNumbers[column];
                }

                row++;
            }

            return boards;
        }

        private static int Score(int[,] board, int winningNumber)
        {
            int unmarked = 0;

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    if (board[row, column] > -1)
                    {
                        unmarked += board[row, column];
                    }
                }
            }

            return unmarked * winningNumber;
        }

        private static int MatchRow(List<int[,]> boards, bool zero)
        {
            bool isMatch = true;
            int index = 0;

            foreach (int[,] board in boards)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        if (board[row, column] == 0 && zero)
                        {
                            continue;
                        }

                        if (board[row, column] > -1)
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        return index;
                    }

                    isMatch = true;
                }

                index++;
                isMatch = true;
            }

            return -1;
        }

        private static int MatchColumn(List<int[,]> boards, bool zero)
        {
            bool isMatch = true;
            int index = 0;

            foreach (int[,] board in boards)
            {
                for (int column = 0; column < 5; column++)
                {
                    for (int row = 0; row < 5; row++)
                    {
                        if (board[row, column] == 0 && zero)
                        {
                            continue;
                        }

                        if (board[row, column] > -1)
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        return index;
                    }

                    isMatch = true;
                }

                index++;
                isMatch = true;
            }

            return -1;
        }
    }
}
