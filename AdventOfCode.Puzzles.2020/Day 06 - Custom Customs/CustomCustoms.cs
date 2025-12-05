namespace AdventOfCode.Puzzles._2020.Day_06___Custom_Customs
{
    public class CustomCustoms
    {
        public static int Anyone(string[] input)
        {
            Dictionary<char, int> answers = new();

            int count = 0;

            foreach (string value in input)
            {
                if (string.IsNullOrEmpty(value))
                {
                    count += answers.Count;
                    answers = new();
                }

                for (int i = 0; i < value.Length; i++)
                {
                    if (!answers.ContainsKey(value[i]))
                    {
                        answers.Add(value[i], 1);
                    }
                }
            }

            return count;
        }

        public static int Everyone(string[] input)
        {
            Dictionary<char, int> answers = new();

            int count = 0;
            int people = 0;

            foreach (string value in input)
            {
                if (string.IsNullOrEmpty(value))
                {
                    foreach (KeyValuePair<char, int> answer in answers)
                    {
                        if (answer.Value == people)
                        {
                            count++;
                        }
                    }

                    answers = new();
                    people = 0;
                    continue;
                }

                for (int i = 0; i < value.Length; i++)
                {
                    if (!answers.ContainsKey(value[i]))
                    {
                        answers.Add(value[i], 1);
                    }
                    else
                    {
                        answers[value[i]]++;
                    }
                }

                people++;
            }

            return count;
        }
    }
}
