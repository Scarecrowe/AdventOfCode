namespace AdventOfCode.Puzzles._2022.Day_06___Tuning_Trouble
{
    public class TuningTrouble
    {
        public static int Marker(string input, int distinct)
        {
            for (int i = 0; i < input.Length; i++)
            {
                string result = input.Length < i + distinct
                    ? input[i..]
                    : input.Substring(i, distinct);

                if (result.Distinct().Count() == distinct)
                {
                    return i + distinct;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
