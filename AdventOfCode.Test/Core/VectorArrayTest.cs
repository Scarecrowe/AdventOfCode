namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_17___Two_Steps_Forward;
    using FluentAssertions;
    using NUnit.Framework;

    public class VectorArrayTestBase : TheSubject<VectorArray<int, EntityType>>
    {
        protected static string[] Input 
        { 
            get {
                string[] input = new string[9];
                input[0] = "#########";
                input[1] = "#S| | | #";
                input[2] = "#-#-#-#-#";
                input[3] = "# | | | #";
                input[4] = "#-#-#-#-#";
                input[5] = "# | | | #";
                input[6] = "#-#-#-#-#";
                input[7] = "# | | |  ";
                input[8] = "####### V";

                return input;
            }
        }

        protected const string Key = "#-|SV ";

        protected static void AssertVectorCell(VectorCell<int, EntityType> result, Vector<int> point, EntityType value, Cardinal direction)
        {
            result.Point.Should().Be(point);
            result.Value.Should().Be(value);
            result.Direction.Should().Be(direction);
        }

        [SetUp]
        public void Init()
        {
            Subject = new VectorArray<int, EntityType>(Input, Key);
        }
    }

    [TestFixture]
    public class VectorArrayTest : VectorArrayTestBase
    {
        public class Constructor : VectorArrayTestBase
        {
            [Test]
            public void When_created()
            {
                Subject = new VectorArray<int, EntityType>();
                Subject.Width.Should().Be(0);
                Subject.Height.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_width_and_height()
            {
                Subject = new(45, 99);
                Subject.Width.Should().Be(45);
                Subject.Height.Should().Be(99);
                Subject[0, 0].Should().Be(0);
                Subject[98, 44].Should().Be(0);
            }

            [Test]
            public void When_created_with_a_width_height_and_a_default_value()
            {
                Subject = new VectorArray<int, EntityType>(45, 99, EntityType.Human);
                Subject.Width.Should().Be(45);
                Subject.Height.Should().Be(99);
                Subject[0, 0].Should().Be(EntityType.Human);
                Subject[98, 44].Should().Be(EntityType.Human);
            }

            [Test]
            public void When_created_with_a_non_indexed_comparer()
            {
                VectorArray<int, int> subject = new(Input, (c) => (int)c);
                subject.Width.Should().Be(9);
                subject.Height.Should().Be(9);
                subject[0, 0].Should().Be('#');
                subject[8, 8].Should().Be('V');
            }

            [Test]
            public void When_created_with_an_indexed_comparer()
            {
                VectorArray<int, int> subject = new(Input, (c, x, y) => x * y);
                subject.Width.Should().Be(9);
                subject.Height.Should().Be(9);
                subject[0, 0].Should().Be(0);
                subject[8, 8].Should().Be(64);
            }

            [Test]
            public void When_created_with_a_string_to_enum()
            {
                Subject = new(Input, "#-|SV ");
                Subject.Width.Should().Be(9);
                Subject.Height.Should().Be(9);
                Subject[0, 0].Should().Be(EntityType.Wall);
                Subject[1, 1].Should().Be(EntityType.Human);
                Subject[1, 2].Should().Be(EntityType.DoorVertical);
                Subject[2, 1].Should().Be(EntityType.DoorHorizontal);
                Subject[8, 8].Should().Be(EntityType.Vault);
            }

            [Test]
            public void When_created_with_another_vector_array()
            {
                Subject = new(Input, "#-|SV ");

                Subject = new(Subject);

                Subject.Width.Should().Be(9);
                Subject.Height.Should().Be(9);
                Subject[0, 0].Should().Be(EntityType.Wall);
                Subject[1, 1].Should().Be(EntityType.Human);
                Subject[1, 2].Should().Be(EntityType.DoorVertical);
                Subject[2, 1].Should().Be(EntityType.DoorHorizontal);
                Subject[8, 8].Should().Be(EntityType.Vault);
            }

            [Test]
            public void When_created_with_a_string_and_key()
            {
                Subject = new VectorArray<int, EntityType>(string.Join(string.Empty, Input), Key);

                Subject.Height.Should().Be(1);
                Subject.Width.Should().Be(81);
            }
        }

        public class IndexesGet : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_an_index_with_a_x_and_y_that_does_not_exist()
            {
                Func<EntityType> func = () => Subject[1000, 1000];

                func.Should().Throw<IndexOutOfRangeException>();
            }

            [Test]
            public void When_told_to_get_an_index_with_a_x_and_y_that_exists()
            {
                EntityType result = Subject[3, 4];

                result.Should().Be(EntityType.DoorVertical);
            }

            [Test]
            public void When_told_to_get_an_index_with_a_vector_that_does_not_exist()
            {
                Func<EntityType> func = () => Subject[new Vector<int>(1000, 1000)];

                func.Should().Throw<IndexOutOfRangeException>();
            }

            [Test]
            public void When_told_to_get_an_index_with_a_vector_that_exists()
            {
                EntityType result = Subject[new Vector<int>(4, 3)];

                result.Should().Be(EntityType.DoorVertical);
            }
        }

        public class IndexesSet : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_set_an_index_with_a_x_and_y_that_does_not_exist()
            {
                Action action = () => Subject[1000, 1000] = EntityType.Human;

                action.Should().Throw<IndexOutOfRangeException>();
            }

            [Test]
            public void When_told_to_set_an_index_with_a_x_and_y_that_exists()
            {
                Subject[3, 4] = EntityType.Human;
                Subject[3, 4].Should().Be(EntityType.Human);
            }

            [Test]
            public void When_told_to_set_an_index_with_a_vector_that_does_not_exist()
            {
                Action action = () => Subject[new Vector<int>(1000, 1000)] = EntityType.Human;

                action.Should().Throw<IndexOutOfRangeException>();
            }

            [Test]
            public void When_told_to_set_an_index_with_a_vector_that_exists()
            {
                Subject[new Vector<int>(4, 3)] = EntityType.Human;
                Subject[new Vector<int>(4, 3)].Should().Be(EntityType.Human);
            }
        }

        public class Resize : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_resize_larger()
            {
                Subject.Resize(1000, 1000);
                Subject.Width.Should().Be(1000);
                Subject.Height.Should().Be(1000);
                Subject[1, 1].Should().Be(EntityType.Human);
                Subject[999, 999].Should().Be(EntityType.Wall);
            }

            [Test]
            public void When_told_to_resize_smaller()
            {
                Subject.Resize(4, 5);
                Subject.Width.Should().Be(4);
                Subject.Height.Should().Be(5);
                Subject[1, 1].Should().Be(EntityType.Human);
                Subject[1, 2].Should().Be(EntityType.DoorVertical);

                Func<EntityType> func = () => Subject[8, 8];

                func.Should().Throw<IndexOutOfRangeException>();
            }
        }

        public class Clear : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_clear()
            {
                Subject.Clear();

                foreach(VectorCell<int, EntityType> cell in Subject.AxisEnumerator())
                {
                    cell.Value.Should().Be(EntityType.Wall);
                }
            }
        }

        public class AdjacentCardinal : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_north_edge_adjacent_cells()
            {
                var result = Subject.AdjacentCardinal(new(4, 0)).ToList();

                result.Count.Should().Be(3);

                AssertVectorCell(result[0], new Vector<int>(4, 1), EntityType.DoorVertical, Cardinal.South);
                AssertVectorCell(result[1], new Vector<int>(3, 0), EntityType.Wall, Cardinal.West);
                AssertVectorCell(result[2], new Vector<int>(5, 0), EntityType.Wall, Cardinal.East);
            }

            [Test]
            public void When_told_to_get_a_south_edge_adjacent_cells()
            {
                var result = Subject.AdjacentCardinal(new(4, 8)).ToList();

                result.Count.Should().Be(3);

                AssertVectorCell(result[0], new Vector<int>(4, 7), EntityType.DoorVertical, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(3, 8), EntityType.Wall, Cardinal.West);
                AssertVectorCell(result[2], new Vector<int>(5, 8), EntityType.Wall, Cardinal.East);
            }

            [Test]
            public void When_told_to_get_a_west_edge_adjacent_cells()
            {
                var result = Subject.AdjacentCardinal(new(0, 4)).ToList();

                result.Count.Should().Be(3);

                AssertVectorCell(result[0], new Vector<int>(0, 3), EntityType.Wall, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(0, 5), EntityType.Wall, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(1, 4), EntityType.DoorHorizontal, Cardinal.East);
            }

            [Test]
            public void When_told_to_get_an_east_edge_adjacent_cells()
            {
                var result = Subject.AdjacentCardinal(new(8, 4)).ToList();

                result.Count.Should().Be(3);

                AssertVectorCell(result[0], new Vector<int>(8, 3), EntityType.Wall, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(8, 5), EntityType.Wall, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(7, 4), EntityType.DoorHorizontal, Cardinal.West);
            }

            [Test]
            public void When_told_to_get_adjacent_cells()
            {
                var result = Subject.AdjacentCardinal(new(4, 4)).ToList();

                result.Count.Should().Be(4);

                AssertVectorCell(result[0], new Vector<int>(4, 3), EntityType.DoorVertical, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(4, 5), EntityType.DoorVertical, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(3, 4), EntityType.DoorHorizontal, Cardinal.West);
                AssertVectorCell(result[3], new Vector<int>(5, 4), EntityType.DoorHorizontal, Cardinal.East);
            }

            [Test]
            public void When_told_to_get_adjacent_cells_for_an_out_of_range_vector()
            {
                var result = Subject.AdjacentCardinal(new(100, 100)).ToList();

                result.Count.Should().Be(0);
            }
        }

        public class AdjacentInterCardinal : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_north_edge_adjacent_cells()
            {
                var result = Subject.AdjacentInterCardinal(new(4, 0)).ToList();

                result.Count.Should().Be(5);

                AssertVectorCell(result[0], new Vector<int>(4, 1), EntityType.DoorVertical, Cardinal.South);
                AssertVectorCell(result[1], new Vector<int>(3, 0), EntityType.Wall, Cardinal.West);
                AssertVectorCell(result[2], new Vector<int>(5, 0), EntityType.Wall, Cardinal.East);
                AssertVectorCell(result[3], new Vector<int>(3, 1), EntityType.Space, Cardinal.SouthWest);
                AssertVectorCell(result[4], new Vector<int>(5, 1), EntityType.Space, Cardinal.SouthEast);
            }

            [Test]
            public void When_told_to_get_a_south_edge_adjacent_cells()
            {
                var result = Subject.AdjacentInterCardinal(new(4, 8)).ToList();

                result.Count.Should().Be(5);

                AssertVectorCell(result[0], new Vector<int>(4, 7), EntityType.DoorVertical, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(3, 8), EntityType.Wall, Cardinal.West);
                AssertVectorCell(result[2], new Vector<int>(5, 8), EntityType.Wall, Cardinal.East);
                AssertVectorCell(result[3], new Vector<int>(3, 7), EntityType.Space, Cardinal.NorthWest);
                AssertVectorCell(result[4], new Vector<int>(5, 7), EntityType.Space, Cardinal.NorthEast);
            }

            [Test]
            public void When_told_to_get_a_west_edge_adjacent_cells()
            {
                var result = Subject.AdjacentInterCardinal(new(0, 4)).ToList();

                result.Count.Should().Be(5);

                AssertVectorCell(result[0], new Vector<int>(0, 3), EntityType.Wall, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(0, 5), EntityType.Wall, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(1, 4), EntityType.DoorHorizontal, Cardinal.East);
                AssertVectorCell(result[3], new Vector<int>(1, 3), EntityType.Space, Cardinal.NorthEast);
                AssertVectorCell(result[4], new Vector<int>(1, 5), EntityType.Space, Cardinal.SouthEast);
            }

            [Test]
            public void When_told_to_get_an_east_edge_adjacent_cells()
            {
                var result = Subject.AdjacentInterCardinal(new(8, 4)).ToList();

                result.Count.Should().Be(5);

                AssertVectorCell(result[0], new Vector<int>(8, 3), EntityType.Wall, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(8, 5), EntityType.Wall, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(7, 4), EntityType.DoorHorizontal, Cardinal.West);
                AssertVectorCell(result[3], new Vector<int>(7, 3), EntityType.Space, Cardinal.NorthWest);
                AssertVectorCell(result[4], new Vector<int>(7, 5), EntityType.Space, Cardinal.SouthWest);
            }

            [Test]
            public void When_told_to_get_adjacent_cells()
            {
                var result = Subject.AdjacentInterCardinal(new(4, 4)).ToList();

                result.Count.Should().Be(8);

                AssertVectorCell(result[0], new Vector<int>(4, 3), EntityType.DoorVertical, Cardinal.North);
                AssertVectorCell(result[1], new Vector<int>(4, 5), EntityType.DoorVertical, Cardinal.South);
                AssertVectorCell(result[2], new Vector<int>(3, 4), EntityType.DoorHorizontal, Cardinal.West);
                AssertVectorCell(result[3], new Vector<int>(5, 4), EntityType.DoorHorizontal, Cardinal.East);
                AssertVectorCell(result[4], new Vector<int>(3, 3), EntityType.Space, Cardinal.NorthWest);
                AssertVectorCell(result[5], new Vector<int>(3, 5), EntityType.Space, Cardinal.SouthWest);
                AssertVectorCell(result[6], new Vector<int>(5, 3), EntityType.Space, Cardinal.NorthEast);
                AssertVectorCell(result[7], new Vector<int>(5, 5), EntityType.Space, Cardinal.SouthEast);
            }

            [Test]
            public void When_told_to_get_adjacent_cells_for_an_out_of_range_vector()
            {
                var result = Subject.AdjacentInterCardinal(new(100, 100)).ToList();

                result.Count.Should().Be(0);
            }
        }

        public class Letters : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_return_letters_only()
            {
                var subject = new VectorArray<int, char>(Input, (c) => c);

                var result = subject.Letters().ToList();

                result.Count.Should().Be(2);

                result[0].Value.Should().Be('S');
                result[1].Value.Should().Be('V');
            }
        }

        public class Print : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_print()
            {
                var result = Subject.Print((value) => Key[(int)value]);

                var expectedResult = 
@"

#########
#S| | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | |  
####### V

";

                result.Should().Be(expectedResult);
            }
        }

        public class Clone : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_create_a_clone()
            {
                var result = Subject.Clone();

                result.Width.Should().Be(9);
                result.Height.Should().Be(9);
                result[0, 0].Should().Be(EntityType.Wall);
                result[1, 1].Should().Be(EntityType.Human);
                result[1, 2].Should().Be(EntityType.DoorVertical);
                result[2, 1].Should().Be(EntityType.DoorHorizontal);
                result[8, 8].Should().Be(EntityType.Vault);

                result.Should().NotBe(Subject);
            }
        }

        public class Values : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_all_matching_values_for_a_single_value()
            {
                var result = Subject.Values(EntityType.DoorHorizontal).ToList();
                result.Count.Should().Be(12);
            }

            [Test]
            public void When_told_to_get_all_matching_values_for_multiple_values()
            {
                var result = Subject.Values(new List<EntityType> { EntityType.Human, EntityType.Vault}).ToList();
                result.Count.Should().Be(2);
            }
        }

        public class Count : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_all_matching_values_for_a_single_value()
            {
                Subject.Count(EntityType.DoorHorizontal).Should().Be(12);
            }

            [Test]
            public void When_told_to_get_all_matching_values_for_multiple_values()
            {
                Subject.Count(new List<EntityType> { EntityType.Human, EntityType.Vault }).Should().Be(2);
            }
        }

        public new class Equals : VectorArrayTestBase
        {
            [Test]
            public void When_asked_if_two_matching_vector_arrays_match()
            {
                var vector = new VectorArray<int, EntityType>(Input, Key);
                var result = Subject.Equals(vector);

                result.Should().BeTrue();
            }

            [Test]
            public void When_asked_if_two_non_matching_vector_arrays_match()
            {
                var vector = new VectorArray<int, EntityType>(100, 100);
                var result = Subject.Equals(vector);

                result.Should().BeFalse();
            }
        }

        public class AxisEnumerator : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_enumerate_a_2D_axis()
            {
                var result = Subject.AxisEnumerator().ToList();

                result.Count.Should().Be(Subject.Width * Subject.Height);
                result[0].Point.Should().Be(new Vector<int>(0, 0));
                result[^1].Point.Should().Be(new Vector<int>(8, 8));
            }

            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_an_action_at_the_end_of_each_column_iteration()
            {
                var count = 0;
                void action() => count++;

                var result = Subject.AxisEnumerator(action).ToList();

                result.Count.Should().Be(Subject.Width * Subject.Height);
                result[0].Point.Should().Be(new Vector<int>(0, 0));
                result[^1].Point.Should().Be(new Vector<int>(8, 8));

                count.Should().Be(Subject.Height);
            }
        }

        public class RowEnumerator : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_enumerate_rows()
            {
                var result = Subject.RowEnumerator().ToList();

                result.Count.Should().Be(Subject.Height);
                
                result[0].Count.Should().Be(Subject.Width);
                result[0][0].Point.Should().Be(new Vector<int>(0, 0));
                result[0][^1].Point.Should().Be(new Vector<int>(8, 0));

                result[^1].Count.Should().Be(Subject.Width);
                result[^1][0].Point.Should().Be(new Vector<int>(0, 8));
                result[^1][^1].Point.Should().Be(new Vector<int>(8, 8));
            }
        }

        public class ColumnEnumerator : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_enumerate_columns()
            {
                var result = Subject.ColumnEnumerator().ToList();

                result.Count.Should().Be(Subject.Width);

                result[0].Count.Should().Be(Subject.Height);
                result[0][0].Point.Should().Be(new Vector<int>(0, 0));
                result[0][^1].Point.Should().Be(new Vector<int>(0, 8));

                result[^1].Count.Should().Be(Subject.Height);
                result[^1][0].Point.Should().Be(new Vector<int>(8, 0));
                result[^1][^1].Point.Should().Be(new Vector<int>(8, 8));
            }
        }

        public class EdgeEnumerator : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_enumerate_edges()
            {
                var result = Subject.EdgeEnumerator().ToList();

                result.Count.Should().Be(((Subject.Width * 2) + (Subject.Height * 2)) - 4);

                for (int x = 0; x < Subject.Width; x++)
                {
                    result.Any(c => c.Point == new Vector<int>(x, 0)).Should().BeTrue();
                    result.Any(c => c.Point == new Vector<int>(x, Subject.Height - 1)).Should().BeTrue();
                }

                for (int y = 0; y < Subject.Height; y++)
                {
                    result.Any(c => c.Point == new Vector<int>(0, y)).Should().BeTrue();
                    result.Any(c => c.Point == new Vector<int>(Subject.Width - 1, y)).Should().BeTrue();
                }
            }
        }

        public class GetValue : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_value_by_vector()
            {
                EntityType result = Subject.GetValue(new Vector<int>(1, 1));

                result.Should().Be(EntityType.Human);
            }

            [Test]
            public void When_told_to_get_a_value_by_a_x_and_y_coordinate()
            {
                EntityType result = Subject.GetValue(1, 1);

                result.Should().Be(EntityType.Human);
            }
        }

        public class GetRow : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_row()
            {
                var result = Subject.GetRow(4).ToList();

                result.Count.Should().Be(Subject.Width);

                for (int x = 0; x < Subject.Width; x++)
                {
                    result.Any(c => c.Point == new Vector<int>(x, 4)).Should().BeTrue();
                }
            }
        }

        public class GetColumn : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_column()
            {
                var result = Subject.GetColumn(4).ToList();

                result.Count.Should().Be(Subject.Height);

                for (int y = 0; y < Subject.Height; y++)
                {
                    result.Any(c => c.Point == new Vector<int>(4, y)).Should().BeTrue();
                }
            }
        }

        public class FirstOrDefault : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_get_the_first_of_a_value_that_exists()
            {
                var result = Subject.FirstOrDefault(EntityType.Vault);

                result.Should().NotBe(default);
                result.Point.Should().Be(new Vector<int>(8, 8));
                result.Value.Should().Be(EntityType.Vault);
            }

            [Test]
            public void When_told_to_get_the_first_of_a_non_existant_value()
            {
                var result = Subject.FirstOrDefault((EntityType)99);

                result.Should().Be(default);
            }
        }

        public class Flatten : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_flatten()
            {
                var result = Subject.Flatten().ToList();

                result.Count.Should().Be(Subject.Width * Subject.Height);
                result[0].Should().Be(EntityType.Wall);
                result[^1].Should().Be(EntityType.Vault);
            }
        }

        public class BreadthFirstSearch : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_find_the_shortest_path_from_the_human_to_the_vault()
            {
                var human = Subject.FirstOrDefault(EntityType.Human);
                var vault = Subject.FirstOrDefault(EntityType.Vault);
                var walls = Subject.Values(EntityType.Wall).Select(x => x.Point).ToList();

                var result = Subject.BreadthFirstSearch(human.Point, vault.Point, walls);

                result.Distance.Should().Be(15);
                result.Path.Count.Should().Be(15);
                result.Path[0].Should().Be(new Vector<int>(1, 1));
                result.Path[1].Should().Be(new Vector<int>(1, 2));
                result.Path[^1].Should().Be(new Vector<int>(8, 8));
            }

            [Test]
            public void When_told_to_find_the_shortest_path_from_the_human_to_the_vault_with_a_locked_door()
            {
                var human = Subject.FirstOrDefault(EntityType.Human);
                var vault = Subject.FirstOrDefault(EntityType.Vault);
                var walls = Subject.Values(EntityType.Wall).Select(x => x.Point).ToList();

                walls.Add(new Vector<int>(1, 2));

                var result = Subject.BreadthFirstSearch(human.Point, vault.Point, walls);

                result.Distance.Should().Be(15);
                result.Path.Count.Should().Be(15);
                result.Path[0].Should().Be(new Vector<int>(1, 1));
                result.Path[1].Should().Be(new Vector<int>(2, 1));
                result.Path[^1].Should().Be(new Vector<int>(8, 8));
            }

            [Test]
            public void When_told_to_find_the_shortest_path_with_no_possible_route()
            {
                var human = Subject.FirstOrDefault(EntityType.Human);
                var vault = Subject.FirstOrDefault(EntityType.Vault);
                var walls = Subject.Values(EntityType.Wall).Select(x => x.Point).ToList();

                walls.Add(new Vector<int>(1, 2));
                walls.Add(new Vector<int>(2, 1));

                var result = Subject.BreadthFirstSearch(human.Point, vault.Point, walls);

                result.Distance.Should().Be(0);
                result.Path.Count.Should().Be(0);
            }
        }

        public new class ToString : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_return_as_a_string_with_a_comparer()
            {
                var result = Subject.ToString((value) => Key[(int)value]);

                result.Should().Be(string.Join(string.Empty, Input));
            }
        }

        public class SumInt : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_sum_the_value_collection()
            {
                var subject = new VectorArray<int, int>(15, 15, 5);

                var result = subject.Sum();

                result.Should().Be(15 * 15 * 5);
            }
        }

        public class SumLong : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_sum_the_value_collection()
            {
                var subject = new VectorArray<long, long>(15L, 15L, 25L);

                var result = subject.Sum();

                result.Should().Be(15 * 15 * 25);
            }
        }

        public class Add : VectorArrayTestBase
        {
            [Test]
            public void When_told_to_add_a_value_that_is_within_the_dimensions_of_the_array()
            {
                var human = Subject.FirstOrDefault(EntityType.Human);

                Subject.Add(human.Point, EntityType.Space);

                Subject[human.Point].Should().Be(EntityType.Space);
            }

            [Test]
            public void When_told_to_add_a_value_that_is_not_within_the_dimensions_of_the_array()
            {
                Action action = () => Subject.Add(new(45, 45), EntityType.Human);

                action.Should().NotThrow();
            }
        }

        public class IsVectorInRange : VectorArrayTestBase
        {
            [Test]
            public void When_asked_if_a_x_and_y_coordinate_is_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange(4, 4).Should().BeTrue();
            }

            [Test]
            public void When_asked_if_a_x_and_y_coordinate_is_not_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange(45, 45).Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_vector_is_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange(new Vector<int>(4, 4)).Should().BeTrue();
            }

            [Test]
            public void When_asked_if_a_vector_is_not_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange(new Vector<int>(45, 45)).Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_tuple_is_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange((4, 4)).Should().BeTrue();
            }

            [Test]
            public void When_asked_if_a_tuple_is_not_within_the_dimensions_of_the_array()
            {
                Subject.IsVectorInRange((45, 45)).Should().BeFalse();
            }
        }
        
        public class IsEdge : VectorArrayTestBase
        {
            [Test]
            public void When_asked_if_a_value_is_an_edge()
            {
                Subject.IsEdge(EntityType.Wall).Should().BeTrue();
            }

            [Test]
            public void When_asked_if_a_value_is_not_an_edge()
            {
                Subject.IsEdge(EntityType.Human).Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_vector_is_an_edge()
            {
                Subject.IsEdge(new Vector<int>(4, 0)).Should().BeTrue();
            }

            [Test]
            public void When_asked_if_a_vector_is_not_an_edge()
            {
                Subject.IsEdge(new Vector<int>(4, 4)).Should().BeFalse();
            }
        }
    }
}
