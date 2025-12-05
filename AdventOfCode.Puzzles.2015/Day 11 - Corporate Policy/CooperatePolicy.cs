namespace AdventOfCode.Puzzles._2015.Day_11___Corporate_Policy
{
    public class CooperatePolicy
    {
        private static readonly List<int> DisallowedAscii = new() { 105, 108, 111 };

        public static string Generate(string password)
        {
            int[] characters = password.Select(x => (int)x).ToArray();

            bool isValid = false;

            while (!isValid)
            {
                Increment(ref characters, characters.Length - 1);

                if (HasDisallowedAscii(ref characters))
                {
                    continue;
                }

                if (HasStraight(ref characters) && HasDouble(ref characters))
                {
                    return string.Join(string.Empty, characters.Select(x => (char)x));
                }
            }

            return string.Empty;
        }

        private static void Increment(ref int[] characters, int position)
        {
            if (position < 0)
            {
                position = characters.Length - 1;
            }

            characters[position]++;

            if (DisallowedAscii.Contains(characters[position]))
            {
                characters[position]++;
            }

            if (characters[position] <= 122)
            {
                return;
            }

            characters[position] = 97;

            Increment(ref characters, position - 1);
        }

        private static bool HasStraight(ref int[] characters)
        {
            int last = characters[0];
            int count = 1;

            for (int i = 1; i < characters.Length; i++)
            {
                count = characters[i] - last == 1 ? ++count : 1;

                if (count == 3)
                {
                    return true;
                }

                last = characters[i];
            }

            return false;
        }

        private static bool HasDisallowedAscii(ref int[] characters)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (DisallowedAscii.Contains(characters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasDouble(ref int[] characters)
        {
            int last = characters[0];
            int index = -1;
            int count = 0;

            for (int i = 1; i < characters.Length; i++)
            {
                if (characters[i] == last && i > index + 1)
                {
                    count++;
                    index = i;
                }

                if (count == 2)
                {
                    return true;
                }

                last = characters[i];
            }

            return false;
        }
    }
}
