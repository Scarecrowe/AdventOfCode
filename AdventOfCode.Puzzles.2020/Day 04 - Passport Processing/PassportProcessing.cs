namespace AdventOfCode.Puzzles._2020.Day_04___Passport_Processing
{
    using AdventOfCode.Core.Extensions;

    public class PassportProcessing
    {
        private static readonly List<string> Valid = new() { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        private static readonly List<string> HairColours = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        private static readonly Dictionary<string, Func<string, bool>> Validate = new()
        {
            { "byr", ValidateBirthYear },
            { "iyr", ValidateIssueYear },
            { "eyr", ValidateExpirationYear },
            { "hgt", ValidateHeight },
            { "hcl", ValidateHairColor },
            { "ecl", ValidateEyeColour },
            { "pid", ValidatePassportID }
        };

        public static int Simple(string[] input) => Run(input, Fields);

        public static int Advanced(string[] input) => Run(input, ValidFields);

        private static int Run(string[] input, Func<string[], string[]> fields)
        {
            int result = 0;
            string passport = string.Empty;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    result += Valid.Intersect(fields(passport.SplitSpace())).Count() == Valid.Count ? 1 : 0;
                    passport = string.Empty;
                }
                else
                {
                    passport += $"{line} ";
                }
            }

            return result;
        }

        private static string[] Fields(string[] fields)
            => fields.Select(field => field.Split(":"))
                .Where(pair => pair[0] != "cid")
                .Select(pair => pair[0])
                .ToArray();

        private static string[] ValidFields(string[] fields)
            => fields.Select(field => field.Split(":"))
                .Where(pair => pair[0] != "cid")
                .Where(pair => Validate[pair[0]](pair[1]))
                .Select(pair => pair[0])
                .ToArray();

        private static bool ValidateBirthYear(string value) => value.Length == 4 && value.ToInt() >= 1920 && value.ToInt() <= 2002;

        private static bool ValidateIssueYear(string value) => value.Length == 4 && value.ToInt() >= 2010 && value.ToInt() <= 2020;

        private static bool ValidateExpirationYear(string value) => value.Length == 4 && value.ToInt() >= 2020 && value.ToInt() <= 2030;

        private static bool ValidateEyeColour(string value) => HairColours.Contains(value);

        private static bool ValidateHeight(string value)
        {
            if (value.Length <= 2)
            {
                return false;
            }

            string unit = value[^2..];

            if (unit != "cm" && unit != "in")
            {
                return false;
            }

            int x = value[..^2].ToInt();

            return unit == "cm"
                ? x >= 150 && x <= 193
                : x >= 59 && x <= 76;
        }

        private static bool ValidateHairColor(string value)
        {
            if (value[0] != '#' || value.Length != 7)
            {
                return false;
            }

            bool isValid = true;

            for (int i = 1; i < value.Length; i++)
            {
                if (char.IsNumber(value[i]))
                {
                    continue;
                }

                if (value[i] >= 97 && value[i] <= 102)
                {
                    continue;
                }

                isValid = false;

                break;
            }

            return isValid;
        }

        private static bool ValidatePassportID(string value)
        {
            if (value.Length != 9)
            {
                return false;
            }

            bool isValid = true;

            foreach (char letter in value)
            {
                if (!char.IsNumber(letter))
                {
                    isValid = false;

                    break;
                }
            }

            return isValid;
        }
    }
}
