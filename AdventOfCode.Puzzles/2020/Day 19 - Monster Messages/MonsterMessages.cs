namespace AdventOfCode.Puzzles._2020.Day_19___Monster_Messages
{
    using AdventOfCode.Core.Extensions;

    public class MonsterMessages
    {
        public MonsterMessages(string[] input) => this.Input = input;

        public string[] Input { get; }

        public int Simple()
        {
            (Dictionary<int, List<List<(RuleType, string)>>> rules, List<string> messages) = this.ParseRulesAndMessages();

            int count = 0;

            foreach (string message in messages)
            {
                List<int> matches = this.Match(message, ref rules, rules[0], 0);

                if (matches.Count > 0 && message.Length == matches[0])
                {
                    count++;
                }
            }

            return count;
        }

        public int Advanced()
        {
            this.Input[9] = "8: 42 | 42 8";
            this.Input[10] = "11: 42 31 | 42 11 31";

            (Dictionary<int, List<List<(RuleType, string)>>> rules, List<string> messages) = this.ParseRulesAndMessages();

            int count = 0;

            foreach (string message in messages)
            {
                List<int> matches = this.Match(message, ref rules, rules[0], 0);

                if (matches.Count > 0 && message.Length == matches[0])
                {
                    count++;
                }
            }

            return count;
        }

        private static (RuleType, string) ParseSubRule(string subRule)
            => subRule.Contains('"')
                    ? (RuleType.Char, $"{subRule[1]}")
                    : (RuleType.Rule, subRule);

        private (Dictionary<int, List<List<(RuleType, string)>>>, List<string>) ParseRulesAndMessages()
        {
            Dictionary<int, List<List<(RuleType, string)>>> rules = new();
            List<string> messages = new();

            bool rulesParsed = false;

            for (int i = 0; i < this.Input.Length; i++)
            {
                string line = this.Input[i];

                if (string.IsNullOrEmpty(line))
                {
                    rulesParsed = true;
                    continue;
                }

                if (!rulesParsed)
                {
                    string[] tokens = line.Split(":");
                    string[] possibilities = tokens[1].Split("|");
                    int ruleNumber = tokens[0].ToInt();

                    if (!rules.ContainsKey(ruleNumber))
                    {
                        rules.Add(ruleNumber, new());
                    }

                    foreach (string possiblity in possibilities)
                    {
                        List<(RuleType, string)> values = new();

                        foreach (string subRule in possiblity.SplitSpace())
                        {
                            values.Add(ParseSubRule(subRule));
                        }

                        rules[ruleNumber].Add(values);
                    }

                    continue;
                }

                messages.Add(line);
            }

            return (rules, messages);
        }

        private List<int> Match(string message, ref Dictionary<int, List<List<(RuleType, string)>>> rules, List<List<(RuleType, string)>> rule, int index)
        {
            List<int> result = new();

            foreach (List<(RuleType, string)> possiblities in rule)
            {
                List<int> indexes = new() { index };

                foreach ((RuleType, string) subRule in possiblities)
                {
                    List<int> currentIndexes = new();

                    foreach (int currentIndex in indexes)
                    {
                        if (subRule.Item1 == RuleType.Rule)
                        {
                            currentIndexes.AddRange(this.Match(message, ref rules, rules[int.Parse(subRule.Item2)], currentIndex));
                        }
                        else
                        {
                            if (index < message.Length && message[index] == subRule.Item2[0])
                            {
                                currentIndexes.Add(currentIndex + 1);
                            }
                        }
                    }

                    indexes = currentIndexes;
                }

                foreach (int currentIndex in indexes)
                {
                    result.Add(currentIndex);
                }
            }

            return result;
        }
    }
}
