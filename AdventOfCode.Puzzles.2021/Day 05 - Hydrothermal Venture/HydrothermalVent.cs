namespace AdventOfCode.Puzzles._2021.Day_05___Hydrothermal_Venture
{
    using AdventOfCode.Core;

    public class HydrothermalVent
    {
        public HydrothermalVent(Vector<int> origin, Vector<int> destination)
        {
            this.Origin = origin;
            this.Destination = destination;
        }

        public Vector<int> Origin { get; set; }

        public Vector<int> Destination { get; set; }

        public bool IsHorizontal => this.Origin.X == this.Destination.X;

        public bool IsVertical => this.Origin.Y == this.Destination.Y;

        public bool IsDiagional => !this.IsHorizontal && !this.IsVertical;

        public Cardinal Direction
        {
            get
            {
                if (this.Origin.X > this.Destination.X && this.Destination.Y > this.Origin.Y)
                {
                    return Cardinal.SouthWest;
                }

                if (this.Origin.X > this.Destination.X && this.Destination.Y < this.Origin.Y)
                {
                    return Cardinal.NorthWest;
                }

                if (this.Origin.X < this.Destination.X && this.Destination.Y > this.Origin.Y)
                {
                    return Cardinal.SouthEast;
                }

                return Cardinal.NorthEast;
            }
        }
    }
}
