namespace AdventOfCode.Puzzles._2019.Day_17___Set_and_Forget
{
    using AdventOfCode.Core;

    public class Robot
    {
        private static readonly Dictionary<Entity, Cardinal> EntityToCardinalMap = new()
        {
            { Entity.RobotUp, Cardinal.North },
            { Entity.RobotDown, Cardinal.South },
            { Entity.RobotLeft, Cardinal.West },
            { Entity.RobotRight, Cardinal.East },
        };

        public Robot(Vector<int> location, Entity value)
        {
            this.Location = new(location);
            this.Direction = EntityToCardinalMap[value];
        }

        public Vector<int> Location { get; set; }

        public Cardinal Direction { get; private set; }

        public string Turn(Cardinal scaffold)
        {
            string turn = string.Empty;

            switch (scaffold)
            {
                case Cardinal.North:
                    switch (this.Direction)
                    {
                        case Cardinal.West:
                            turn = "R";
                            break;
                        case Cardinal.East:
                            turn = "L";
                            break;
                    }

                    this.Direction = Cardinal.North;
                    break;
                case Cardinal.South:
                    switch (this.Direction)
                    {
                        case Cardinal.West:
                            turn = "L";
                            break;
                        case Cardinal.East:
                            turn = "R";
                            break;
                    }

                    this.Direction = Cardinal.South;
                    break;
                case Cardinal.West:
                    switch (this.Direction)
                    {
                        case Cardinal.North:
                            turn = "L";
                            break;
                        case Cardinal.South:
                            turn = "R";
                            break;
                    }

                    this.Direction = Cardinal.West;
                    break;
                case Cardinal.East:
                    switch (this.Direction)
                    {
                        case Cardinal.North:
                            turn = "R";
                            break;
                        case Cardinal.South:
                            turn = "L";
                            break;
                    }

                    this.Direction = Cardinal.East;
                    break;
            }

            return turn;
        }
    }
}
