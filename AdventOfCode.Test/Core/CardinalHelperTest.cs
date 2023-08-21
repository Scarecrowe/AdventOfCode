namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public class CardinalHelperTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class CardinalHelperTest : CardinalHelperTestBase
    {
        public class CardinalGroupMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north()
            {
                List<Cardinal> result = CardinalHelper.CardinalGroupMap[Cardinal.North];

                result.Should().Contain(Cardinal.North);
                result.Should().Contain(Cardinal.NorthWest);
                result.Should().Contain(Cardinal.NorthEast);
            }

            [Test]
            public void When_told_to_get_south()
            {
                List<Cardinal> result = CardinalHelper.CardinalGroupMap[Cardinal.South];

                result.Should().Contain(Cardinal.South);
                result.Should().Contain(Cardinal.SouthWest);
                result.Should().Contain(Cardinal.SouthEast);
            }

            [Test]
            public void When_told_to_get_west()
            {
                List<Cardinal> result = CardinalHelper.CardinalGroupMap[Cardinal.West];

                result.Should().Contain(Cardinal.West);
                result.Should().Contain(Cardinal.NorthWest);
                result.Should().Contain(Cardinal.SouthWest);
            }

            [Test]
            public void When_told_to_get_east()
            {
                List<Cardinal> result = CardinalHelper.CardinalGroupMap[Cardinal.East];

                result.Should().Contain(Cardinal.East);
                result.Should().Contain(Cardinal.NorthEast);
                result.Should().Contain(Cardinal.SouthEast);
            }
        }

        public class LetterToCardinalMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_up() => CardinalHelper.LetterToCardinalMap['U'].Should().Be(Cardinal.North);

            [Test]
            public void When_told_to_get_down() => CardinalHelper.LetterToCardinalMap['D'].Should().Be(Cardinal.South);

            [Test]
            public void When_told_to_get_left() => CardinalHelper.LetterToCardinalMap['L'].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_right() => CardinalHelper.LetterToCardinalMap['R'].Should().Be(Cardinal.East);
        }

        public class CardinalToLetterMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.CardinalToLetterMap[Cardinal.North].Should().Be('U');

            [Test]
            public void When_told_to_get_south() => CardinalHelper.CardinalToLetterMap[Cardinal.South].Should().Be('D');

            [Test]
            public void When_told_to_get_west() => CardinalHelper.CardinalToLetterMap[Cardinal.West].Should().Be('L');

            [Test]
            public void When_told_to_get_east() => CardinalHelper.CardinalToLetterMap[Cardinal.East].Should().Be('R');
        }

        public class SymbolToCardinalMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_up() => CardinalHelper.SymbolToCardinalMap['^'].Should().Be(Cardinal.North);

            [Test]
            public void When_told_to_get_lowercase_down() => CardinalHelper.SymbolToCardinalMap['v'].Should().Be(Cardinal.South);

            [Test]
            public void When_told_to_get_uppercase_down() => CardinalHelper.SymbolToCardinalMap['V'].Should().Be(Cardinal.South);

            [Test]
            public void When_told_to_get_left() => CardinalHelper.SymbolToCardinalMap['<'].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_right() => CardinalHelper.SymbolToCardinalMap['>'].Should().Be(Cardinal.East);
        }

        public class CardinalToSymbolMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.CardinalToSymbolMap[Cardinal.North].Should().Be('^');

            [Test]
            public void When_told_to_get_south() => CardinalHelper.CardinalToSymbolMap[Cardinal.South].Should().Be('V');

            [Test]
            public void When_told_to_get_west() => CardinalHelper.CardinalToSymbolMap[Cardinal.West].Should().Be('<');

            [Test]
            public void When_told_to_get_east() => CardinalHelper.CardinalToSymbolMap[Cardinal.East].Should().Be('>');
        }

        public class CompassToCardinalMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.CompassToCardinalMap["N"].Should().Be(Cardinal.North);

            [Test]
            public void When_told_to_get_south() => CardinalHelper.CompassToCardinalMap["S"].Should().Be(Cardinal.South);

            [Test]
            public void When_told_to_get_west() => CardinalHelper.CompassToCardinalMap["W"].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_east() => CardinalHelper.CompassToCardinalMap["E"].Should().Be(Cardinal.East);

            [Test]
            public void When_told_to_get_northwest() => CardinalHelper.CompassToCardinalMap["NW"].Should().Be(Cardinal.NorthWest);

            [Test]
            public void When_told_to_get_northeast() => CardinalHelper.CompassToCardinalMap["NE"].Should().Be(Cardinal.NorthEast);

            [Test]
            public void When_told_to_get_southwest() => CardinalHelper.CompassToCardinalMap["SW"].Should().Be(Cardinal.SouthWest);

            [Test]
            public void When_told_to_get_southeast() => CardinalHelper.CompassToCardinalMap["SE"].Should().Be(Cardinal.SouthEast);
        }

        public class CardinalToCompassMap : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.CardinalToCompassMap[Cardinal.North].Should().Be("N");

            [Test]
            public void When_told_to_get_south() => CardinalHelper.CardinalToCompassMap[Cardinal.South].Should().Be("S");

            [Test]
            public void When_told_to_get_west() => CardinalHelper.CardinalToCompassMap[Cardinal.West].Should().Be("W");

            [Test]
            public void When_told_to_get_east() => CardinalHelper.CardinalToCompassMap[Cardinal.East].Should().Be("E");

            [Test]
            public void When_told_to_get_northwest() => CardinalHelper.CardinalToCompassMap[Cardinal.NorthWest].Should().Be("NW");

            [Test]
            public void When_told_to_get_northeast() => CardinalHelper.CardinalToCompassMap[Cardinal.NorthEast].Should().Be("NE");

            [Test]
            public void When_told_to_get_southwest() => CardinalHelper.CardinalToCompassMap[Cardinal.SouthWest].Should().Be("SW");

            [Test]
            public void When_told_to_get_southeast() => CardinalHelper.CardinalToCompassMap[Cardinal.SouthEast].Should().Be("SE");
        }

        public class Clockwise : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.Clockwise[Cardinal.North].Should().Be(Cardinal.East);

            [Test]
            public void When_told_to_get_south() => CardinalHelper.Clockwise[Cardinal.South].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_west() => CardinalHelper.Clockwise[Cardinal.West].Should().Be(Cardinal.North);


            [Test]
            public void When_told_to_get_east() => CardinalHelper.Clockwise[Cardinal.East].Should().Be(Cardinal.South);


            [Test]
            public void When_told_to_get_northwest() => CardinalHelper.Clockwise[Cardinal.NorthWest].Should().Be(Cardinal.North);

            [Test]
            public void When_told_to_get_northeast() => CardinalHelper.Clockwise[Cardinal.NorthEast].Should().Be(Cardinal.East);

            [Test]
            public void When_told_to_get_southwest() => CardinalHelper.Clockwise[Cardinal.SouthWest].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_southeast() => CardinalHelper.Clockwise[Cardinal.SouthEast].Should().Be(Cardinal.South);
        }

        public class AntiClockwise : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.AntiClockwise[Cardinal.North].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_south() => CardinalHelper.AntiClockwise[Cardinal.South].Should().Be(Cardinal.East);

            [Test]
            public void When_told_to_get_west() => CardinalHelper.AntiClockwise[Cardinal.West].Should().Be(Cardinal.South);


            [Test]
            public void When_told_to_get_east() => CardinalHelper.AntiClockwise[Cardinal.East].Should().Be(Cardinal.North);


            [Test]
            public void When_told_to_get_northwest() => CardinalHelper.AntiClockwise[Cardinal.NorthWest].Should().Be(Cardinal.West);

            [Test]
            public void When_told_to_get_northeast() => CardinalHelper.AntiClockwise[Cardinal.NorthEast].Should().Be(Cardinal.North);

            [Test]
            public void When_told_to_get_southwest() => CardinalHelper.AntiClockwise[Cardinal.SouthWest].Should().Be(Cardinal.South);

            [Test]
            public void When_told_to_get_southeast() => CardinalHelper.AntiClockwise[Cardinal.SouthEast].Should().Be(Cardinal.East);
        }

        public class CardinalTransform : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_north() => CardinalHelper.CardinalTransform<int>()[Cardinal.North].Should().Be(new Vector<int>(0, -1));

            [Test]
            public void When_told_to_get_south() => CardinalHelper.CardinalTransform<int>()[Cardinal.South].Should().Be(new Vector<int>(0, 1));

            [Test]
            public void When_told_to_get_west() => CardinalHelper.CardinalTransform<int>()[Cardinal.West].Should().Be(new Vector<int>(-1, 0));

            [Test]
            public void When_told_to_get_east() => CardinalHelper.CardinalTransform<int>()[Cardinal.East].Should().Be(new Vector<int>(1, 0));
        }

        public class InterCardinalTransform : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_northwest() => CardinalHelper.InterCardinalTransform<int>()[Cardinal.NorthWest].Should().Be(new Vector<int>(-1, -1));

            [Test]
            public void When_told_to_get_northeast() => CardinalHelper.InterCardinalTransform<int>()[Cardinal.NorthEast].Should().Be(new Vector<int>(1, -1));

            [Test]
            public void When_told_to_get_southwest() => CardinalHelper.InterCardinalTransform<int>()[Cardinal.SouthWest].Should().Be(new Vector<int>(-1, 1));

            [Test]
            public void When_told_to_get_southeast() => CardinalHelper.InterCardinalTransform<int>()[Cardinal.SouthEast].Should().Be(new Vector<int>(1, 1));
        }

        public class CardinalCells : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_cardinal_cells()
            {
                List<VectorCell<int, int>> result = CardinalHelper.CardinalCells<int, int>();

                result.Should().Contain(x => x.Direction == Cardinal.North);
                result.Should().Contain(x => x.Direction == Cardinal.South);
                result.Should().Contain(x => x.Direction == Cardinal.West);
                result.Should().Contain(x => x.Direction == Cardinal.East);
            }
        }

        public class InterCardinalCells : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_inter_cardinal_cells()
            {
                List<VectorCell<int, int>> result = CardinalHelper.InterCardinalCells<int, int>();

                result.Should().Contain(x => x.Direction == Cardinal.NorthWest);
                result.Should().Contain(x => x.Direction == Cardinal.NorthEast);
                result.Should().Contain(x => x.Direction == Cardinal.SouthWest);
                result.Should().Contain(x => x.Direction == Cardinal.SouthEast);
            }
        }

        public class AllCells : CardinalHelperTestBase
        {
            [Test]
            public void When_told_to_get_all_cardinal_cells()
            {
                List<VectorCell<int, int>> result = CardinalHelper.AllCells<int, int>();

                result.Should().Contain(x => x.Direction == Cardinal.North);
                result.Should().Contain(x => x.Direction == Cardinal.South);
                result.Should().Contain(x => x.Direction == Cardinal.West);
                result.Should().Contain(x => x.Direction == Cardinal.East);
                result.Should().Contain(x => x.Direction == Cardinal.NorthWest);
                result.Should().Contain(x => x.Direction == Cardinal.NorthEast);
                result.Should().Contain(x => x.Direction == Cardinal.SouthWest);
                result.Should().Contain(x => x.Direction == Cardinal.SouthEast);
            }
        }
    }
}
