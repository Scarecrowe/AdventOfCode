namespace AdventOfCode.Core
{
    public static class CardinalHelper
    {
        public static Dictionary<Cardinal, List<Cardinal>> CardinalGroupMap { get; } = new()
        {
            { Cardinal.North, new() { Cardinal.North, Cardinal.NorthWest, Cardinal.NorthEast } },
            { Cardinal.South, new() { Cardinal.South, Cardinal.SouthWest, Cardinal.SouthEast } },
            { Cardinal.West, new() { Cardinal.West, Cardinal.SouthWest, Cardinal.NorthWest } },
            { Cardinal.East, new() { Cardinal.East, Cardinal.SouthEast, Cardinal.NorthEast } },
        };

        public static Dictionary<char, Cardinal> LetterToCardinalMap { get; } = new()
        {
            { 'U', Cardinal.North },
            { 'D', Cardinal.South },
            { 'L', Cardinal.West },
            { 'R', Cardinal.East }
        };

        public static Dictionary<Cardinal, char> CardinalToLetterMap { get; } = new()
        {
            { Cardinal.North, 'U' },
            { Cardinal.South, 'D' },
            { Cardinal.West, 'L' },
            { Cardinal.East, 'R' }
        };

        public static Dictionary<char, Cardinal> SymbolToCardinalMap { get; } = new()
        {
            { '^', Cardinal.North },
            { 'V', Cardinal.South },
            { 'v', Cardinal.South },
            { '<', Cardinal.West },
            { '>', Cardinal.East }
        };

        public static Dictionary<Cardinal, char> CardinalToSymbolMap { get; } = new()
        {
            { Cardinal.North, '^' },
            { Cardinal.South, 'V' },
            { Cardinal.West, '<' },
            { Cardinal.East, '>' }
        };

        public static Dictionary<string, Cardinal> CompassToCardinalMap { get; } = new()
        {
            { "N", Cardinal.North },
            { "S", Cardinal.South },
            { "W", Cardinal.West },
            { "E", Cardinal.East },
            { "SW", Cardinal.SouthWest },
            { "SE", Cardinal.SouthEast },
            { "NW", Cardinal.NorthWest },
            { "NE", Cardinal.NorthEast },
        };

        public static Dictionary<Cardinal, string> CardinalToCompassMap { get; } = new()
        {
            { Cardinal.North, "N" },
            { Cardinal.South, "S" },
            { Cardinal.West, "W" },
            { Cardinal.East, "E" },
            { Cardinal.SouthWest, "SW" },
            { Cardinal.SouthEast, "SE" },
            { Cardinal.NorthWest, "NW" },
            { Cardinal.NorthEast, "NE" },
        };

        public static Dictionary<Cardinal, Cardinal> Clockwise { get; } = new()
        {
            { Cardinal.North, Cardinal.East },
            { Cardinal.South, Cardinal.West },
            { Cardinal.West, Cardinal.North },
            { Cardinal.East, Cardinal.South },
            { Cardinal.NorthWest, Cardinal.North },
            { Cardinal.NorthEast, Cardinal.East },
            { Cardinal.SouthWest, Cardinal.West },
            { Cardinal.SouthEast, Cardinal.South }
        };

        public static Dictionary<Cardinal, Cardinal> AntiClockwise { get; } = new()
        {
            { Cardinal.North, Cardinal.West },
            { Cardinal.South, Cardinal.East },
            { Cardinal.West, Cardinal.South },
            { Cardinal.East, Cardinal.North },
            { Cardinal.NorthWest, Cardinal.West },
            { Cardinal.NorthEast, Cardinal.North },
            { Cardinal.SouthWest, Cardinal.South },
            { Cardinal.SouthEast, Cardinal.East }
        };

        public static Dictionary<Cardinal, Vector<TSize>> CardinalTransform<TSize>() => new()
        {
            { Cardinal.North, Vector<TSize>.North },
            { Cardinal.South, Vector<TSize>.South },
            { Cardinal.West, Vector<TSize>.West },
            { Cardinal.East, Vector<TSize>.East }
        };

        public static Dictionary<Cardinal, Vector<TSize>> InterCardinalTransform<TSize>() => new()
        {
            { Cardinal.NorthWest, Vector<TSize>.NorthWest },
            { Cardinal.SouthWest, Vector<TSize>.SouthWest },
            { Cardinal.NorthEast, Vector<TSize>.NorthEast },
            { Cardinal.SouthEast, Vector<TSize>.SouthEast },
        };

        public static Dictionary<Cardinal, Vector<TSize>> AllTransform<TSize>() => new()
        {
            { Cardinal.North, Vector<TSize>.North },
            { Cardinal.South, Vector<TSize>.South },
            { Cardinal.West, Vector<TSize>.West },
            { Cardinal.East, Vector<TSize>.East },
            { Cardinal.NorthWest, Vector<TSize>.NorthWest },
            { Cardinal.SouthWest, Vector<TSize>.SouthWest },
            { Cardinal.NorthEast, Vector<TSize>.NorthEast },
            { Cardinal.SouthEast, Vector<TSize>.SouthEast },
        };

        public static List<VectorCell<TSize, TValue>> CardinalCells<TSize, TValue>()
            => CardinalTransform<TSize>().Select(x => new VectorCell<TSize, TValue>(x.Value, x.Key)).ToList();

        public static List<VectorCell<TSize, TValue>> InterCardinalCells<TSize, TValue>()
             => InterCardinalTransform<TSize>().Select(x => new VectorCell<TSize, TValue>(x.Value, x.Key)).ToList();

        public static List<VectorCell<TSize, TValue>> AllCells<TSize, TValue>()
            => AllTransform<TSize>().Select(x => new VectorCell<TSize, TValue>(x.Value, x.Key)).ToList();
    }
}
