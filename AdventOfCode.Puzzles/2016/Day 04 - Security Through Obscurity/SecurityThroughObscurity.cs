namespace AdventOfCode.Puzzles._2016.Day_04___Security_Through_Obscurity
{
    public class SecurityThroughObscurity
    {
        public static List<Room> ValidRooms(string[] input)
        {
            List<Room> valid = new();

            foreach (string line in input)
            {
                Room room = new(line);

                if (!room.IsDecoy())
                {
                    valid.Add(room);
                }
            }

            return valid;
        }

        public static string Decrypt(string[] input)
        {
            List<Room> valid = ValidRooms(input);

            if (valid.Any())
            {
                return valid.First().Decrypt();
            }

            return string.Empty;
        }

        public static int SumOfValidSectors(string[] input) => ValidRooms(input).Select(x => x.SectorId).Sum();

        public static int NorthPoleSectorId(string[] input)
        {
            const string northpole = "northpole object storage";

            List<Room> valid = ValidRooms(input);

            foreach (Room room in valid)
            {
                if (room.Decrypt() == northpole)
                {
                    return room.SectorId;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
