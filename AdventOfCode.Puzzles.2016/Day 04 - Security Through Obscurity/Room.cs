namespace AdventOfCode.Puzzles._2016.Day_04___Security_Through_Obscurity
{
    using System.Text;

    public class Room
    {
        public Room(string input)
        {
            this.Letters = new();
            this.Encrypted = input[..input.LastIndexOf('-')];

            bool letters = true;
            StringBuilder sectorId = new();
            StringBuilder checksum = new();

            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];

                if (char.IsNumber(character))
                {
                    sectorId.Append(character);
                    continue;
                }

                if (character == '[')
                {
                    letters = false;
                    continue;
                }

                if (character == ']')
                {
                    break;
                }

                if (letters)
                {
                    if (character == '-')
                    {
                        continue;
                    }

                    if (this.Letters.ContainsKey(character))
                    {
                        this.Letters[character]++;
                        continue;
                    }

                    this.Letters.Add(character, 1);

                    continue;
                }

                checksum.Append(character);
            }

            this.Checksum = checksum.ToString();
            this.SectorId = int.Parse(sectorId.ToString());
        }

        public Dictionary<char, int> Letters { get; }

        public string Encrypted { get; }

        public int SectorId { get; }

        public string Checksum { get; }

        public bool IsDecoy()
        {
            int index = 0;

            string match = string.Empty;

            foreach (KeyValuePair<char, int> pair in this.Letters.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
            {
                if (match == this.Checksum)
                {
                    return false;
                }

                if (this.Checksum[index] != pair.Key)
                {
                    return true;
                }

                match += pair.Key;

                index++;
            }

            return false;
        }

        public string Decrypt()
        {
            StringBuilder decrypted = new();

            for (int i = 0; i < this.Encrypted.Length; i++)
            {
                char character = this.Encrypted[i];

                if (character == '-')
                {
                    decrypted.Append(' ');
                    continue;
                }

                decrypted.Append(this.CaesarDecipher(character));
            }

            return decrypted.ToString();
        }

        private char CaesarDecipher(char character) => (char)(((character + (this.SectorId - 26) - 97) % 26) + 97);
    }
}
