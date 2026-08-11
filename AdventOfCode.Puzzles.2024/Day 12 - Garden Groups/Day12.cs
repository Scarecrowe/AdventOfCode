namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_12___Garden_Groups;

    public class Day12 : Puzzle, IPuzzle
    {

        public Day12()
            : base(2024, 12,  "Garden Groups")
        {
            ////TODO - sort this
            var x = new D12P02();
            x.Process(this.Input);
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new GardenGroups(this.Input).Cost()}";

        public string Gold() => $"{new GardenGroups(this.Input).Bulk()}";

        public struct D12Position
        {
            public int x { get; set; }
            public int y { get; set; }
            public int d { get; set; }

            public override int GetHashCode()
            {
                return System.HashCode.Combine(x, y, d);
            }

        }

        public class D12P02
        {
            public int RowCount { get; set; }
            public int ColumnCount { get; set; }

            public List<string> Rows { get; set; } = new List<string>();

            HashSet<int> Visited = new HashSet<int>();


            public long Process(string[] input)
            {
                foreach (string line in input)
                {
                    this.Rows.Add(line);
                    this.RowCount++;
                    this.ColumnCount = line.Length;
                }

                return IterateLands();
            }

            private long IterateLands()
            {
                long sum = 0;
                for (var i = 0; i < RowCount; i++)
                {
                    for (var j = 0; j < ColumnCount; j++)
                    {
                        sum += ProcessGarden(i, j);



                        //return sum;




                    }
                }
                return sum;
            }

            private long ProcessGarden(int x, int y)
            {
                int key = x * 1000 + y;
                if (Visited.Contains(key))
                    return 0;

                char searchChar = this.Rows[x][y];
                Queue<int> queue = new Queue<int>();
                HashSet<D12Position> perimeter = new HashSet<D12Position>();
                queue.Enqueue(key);
                int size = ProcessQueue(perimeter, queue, searchChar);

                int sides = 0;
                HashSet<D12Position> perimterVisited = new HashSet<D12Position>();
                //foreach(var pKey in perimeter){            
                //Console.WriteLine($"Perimeter  {pKey.d} - {pKey.x} , {pKey.y}");
                //}

                foreach (var pKey in perimeter)
                {
                    //Console.WriteLine();
                    sides += CountSides(perimeter, pKey, perimterVisited);
                }
                //Console.WriteLine($"Char {searchChar} = {size} x {sides} = {size * sides}");
                return size * sides;

            }

            private int CountSides(HashSet<D12Position> perimeter, D12Position key, HashSet<D12Position> perimterVisited)
            {
                int sides = 0;
                do
                {
                    if (perimterVisited.Contains(key))
                        return sides;
                    //if (sides == 0)
                    //  sides++;

                    //Console.WriteLine($"Perimeter Visited Add  {key.d} - {key.x } , {key.y }");
                    perimterVisited.Add(new D12Position { d = key.d, x = key.x, y = key.y });
                    if (FollowPerimeter(ref key, perimeter))
                    {
                        sides++;
                        //Console.WriteLine($"Turned to  {key.d} - {key.x } , {key.y }");
                    }
                    else
                    {
                        //Console.WriteLine($"Moved d to  {key.d} - {key.x } , {key.y }");
                    }

                    if (!perimeter.Contains(key))
                        return sides;

                } while (true);
            }

            private bool FollowPerimeter(ref D12Position key, HashSet<D12Position> perimeter)
            {
                var y = key.y;
                var x = key.x;
                var d = key.d;

                D12Position searchStraight;
                D12Position searchTurn;



                // Top
                if (d == 0)
                {
                    searchStraight = new D12Position { d = 0, x = x, y = y + 1 }; // continue to right 
                    searchTurn = new D12Position { d = 2, x = x + 1, y = y + 1 }; // turn and head down on right
                                                                                  //yoffset2 = 1;
                } // Right
                else if (d == 2)
                {
                    searchStraight = new D12Position { d = 2, x = x + 1, y = y };
                    searchTurn = new D12Position { d = 3, x = x + 1, y = y - 1 };
                }
                else if (d == 3)
                { // down
                    searchStraight = new D12Position { d = 3, x = x, y = y - 1 };
                    searchTurn = new D12Position { d = 1, x = x - 1, y = y - 1 };
                }
                else
                {//if (d == 1){ // up
                    searchStraight = new D12Position { d = 1, x = x - 1, y = y };
                    searchTurn = new D12Position { d = 0, x = x - 1, y = y + 1 };
                    //xoffset = 1;
                    //yoffset = -1;
                }

                // Console.WriteLine($"At D:{d} X:{x} Y:{y}");
                // Console.WriteLine($"Looking for D:{searchStraight.d} X:{searchStraight.x} Y:{searchStraight.y}");
                if (perimeter.Contains(new D12Position { d = searchStraight.d, x = searchStraight.x, y = searchStraight.y }))
                {
                    //Console.WriteLine($"Found");
                    key = searchStraight;
                    return false;
                }

                //Console.WriteLine($"Not Found.  Turning.");
                key = searchTurn;
                return true;

            }

            private int ProcessQueue(HashSet<D12Position> perimeter, Queue<int> queue, char searchChar)
            {
                int size = 0;

                do
                {
                    if (!queue.TryDequeue(out int key))
                    {
                        //Console.WriteLine($"Char {searchChar} = {size} x {perimeter.Count} = {size * perimeter.Count}");
                        return size;
                    }

                    if (Visited.Contains(key))
                        continue;

                    size++;
                    Visited.Add(key);
                    var y = key % 1000;
                    var x = (key - y) / 1000;

                    TrySpreadWrapped(queue, searchChar, x - 1, y, ref perimeter, 0); // Up
                    TrySpreadWrapped(queue, searchChar, x, y - 1, ref perimeter, 1); // Left
                    TrySpreadWrapped(queue, searchChar, x, y + 1, ref perimeter, 2); // Right
                    TrySpreadWrapped(queue, searchChar, x + 1, y, ref perimeter, 3); // Down

                } while (true);
            }

            private void TrySpreadWrapped(Queue<int> queue, char searchChar, int x, int y, ref HashSet<D12Position> perimeter, int direction)
            {
                var key = x * 1000 + y;
                var result = TrySpread(queue, searchChar, x, y);
                if (result == 1)
                {
                    queue.Enqueue(key);
                }
                else if (result < 0)
                {

                    // key
                    // millions digits represents direction of perimter
                    // thousands digits represents row
                    // rest represents column
                    perimeter.Add(new D12Position { x = x, y = y, d = direction });
                }
            }

            private int TrySpread(Queue<int> queue, char searchChar, int x, int y)
            {
                var key = x * 1000 + y;
                if (x >= 0 && x < RowCount && y >= 0 && y < ColumnCount)
                {
                    char c = this.Rows[x][y];
                    if (c == searchChar)
                    {
                        if (this.Visited.Contains(key))
                            return 0; // already visited
                        return 1;
                    }
                    return -2;
                }
                return -1; // Off grid, it's a perimter
            }
        }
    }
}
