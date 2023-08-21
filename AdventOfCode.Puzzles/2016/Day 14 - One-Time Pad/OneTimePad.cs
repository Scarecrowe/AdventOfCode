namespace AdventOfCode.Puzzles._2016.Day_14___One_Time_Pad
{
    using AdventOfCode.Core.Extensions;

    public class OneTimePad
    {
        private const string Hex = "0123456789abcdef";

        public OneTimePad(string salt)
        {
            this.Salt = salt;
            this.Keys = new();
            this.Indexes = new();
        }

        private string Salt { get; }

        private List<Key> Keys { get; }

        private List<int> Indexes { get; }

        private List<string> Triples { get; } = Hex.Select(x => new string(x, 3)).ToList();

        private List<string> Quints { get; } = Hex.Select(x => new string(x, 5)).ToList();

        public int Generate(bool stretch = false)
        {
            int index = 0;

            while (this.Indexes.Count < 63)
            {
                string hash = this.GenerateHash(index, stretch);
                this.CheckQuints(hash);
                this.CheckTriples(hash, index);
                index++;
            }

            if (this.Indexes.Count == 63)
            {
                return this.Indexes[62];
            }

            return this.Indexes[63];
        }

        private static string Contains(List<string> values, string hash, char? @char = null)
        {
            for (int i = 0; i < hash.Length - values[0].Length; i++)
            {
                string value = hash.Substring(i, values[0].Length);

                if (@char != null && !value.Contains(@char.Value))
                {
                    continue;
                }

                if (values.Contains(value))
                {
                    return value;
                }
            }

            return string.Empty;
        }

        private static string Stretch(string hash)
        {
            for (int i = 0; i < 2016; i++)
            {
                hash = hash.ToMd5("x2");
            }

            return hash;
        }

        private string GenerateHash(int index, bool stretch)
            => stretch ? Stretch($"{this.Salt}{index}".ToMd5("x2")) : $"{this.Salt}{index}".ToMd5("x2");

        private void CheckTriples(string key, int index)
        {
            string value = Contains(this.Triples, key);

            if (!string.IsNullOrEmpty(value))
            {
                this.Keys.Add(new Key(value[0], index));
            }
        }

        private void CheckQuints(string hash)
        {
            foreach (Key key in this.Keys.ToArray())
            {
                key.Increment();

                if (key.Current > 1000)
                {
                    this.Keys.Remove(key);
                    continue;
                }

                if (!string.IsNullOrEmpty(Contains(this.Quints, hash, key.Char)))
                {
                    this.Indexes.Add(key.Index);
                    this.Keys.Remove(key);
                }
            }
        }
    }
}