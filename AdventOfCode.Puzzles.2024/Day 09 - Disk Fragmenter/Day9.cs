namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
        {
            this.DayTitle = "Disk Fragmenter";
            this.GetPuzzleData(9, this.DayTitle);

            var asLongs = ToLongList(this.Input[0]);
            long pointer = 0;
            long curFileId = 0;
            for (int i = 0; i < asLongs.Count; i++)
            {
                if (i % 2 == 0)
                {
                    AmphiFile newFile = new(curFileId);
                    AmphiFile newFile2 = new(curFileId);

                    for (int j = 0; j < asLongs[i]; j++)
                    {
                        newFile.blocks.Add(pointer);
                        newFile2.blocks.Add(pointer);
                        pointer++;
                        fileBlocks.Push(curFileId);
                    }
                    filesp1[curFileId] = newFile;
                    filesp2[curFileId] = newFile2;
                    curFileId++;
                }
                else
                {
                    if (asLongs[i] > 0)
                    {
                        emptyRanges.Add((pointer, asLongs[i]));
                        for (int j = 0; j < asLongs[i]; j++)
                        {
                            emptyBlocks.Enqueue(pointer);
                            pointer++;
                        }
                    }
                }
            }

            maxFileID = curFileId - 1;
        }

        Dictionary<long, AmphiFile> filesp1 = new();
        Dictionary<long, AmphiFile> filesp2 = new();
        static Queue<long> emptyBlocks = new();
        static Stack<long> fileBlocks = new();
        List<(long start, long length)> emptyRanges = new();
        long maxFileID;

        protected object SolvePartOne()
        {
            bool isMoved;
            do
            {
                var fb = fileBlocks.Pop();
                isMoved = filesp1[fb].MoveBlock(emptyBlocks.Dequeue());
            } while (isMoved);

            return filesp1.Values.Sum(a => a.checksum);
        }

        protected object SolvePartTwo()
        {
            for (long i = maxFileID; i > 0; i--)
            {
                var reqSpace = filesp2[i].blocks.Count;
                int bestBlock = emptyRanges.FindIndex(a => a.length >= reqSpace && a.start < filesp2[i].blocks[0]);
                if (bestBlock >= 0)
                {
                    (var start, var length) = emptyRanges[bestBlock];
                    filesp2[i].moveFile(start);
                    emptyRanges.RemoveAt(bestBlock);
                    if (length - reqSpace > 0) emptyRanges.Insert(bestBlock, (start + reqSpace, length - reqSpace));
                }
            }
            return filesp2.Values.Sum(a => a.checksum);
        }

        public List<long> ToLongList(string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                List<long> result = new();
                foreach (char c in str) if (long.TryParse(c.ToString(), out long n)) result.Add(n);
                return result.ToList();
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => long.TryParse(n, out long v))
                    .Select(n => Convert.ToInt64(n))
                    .ToList();
            }

        }

        private class AmphiFile
        {
            public long id { get; set; }
            public List<long> blocks = new();

            public long checksum => blocks.Sum(a => a * id);

            public AmphiFile() { }
            public AmphiFile(long id) { this.id = id; }

            public bool MoveBlock(long targetBlock)
            {
                var tmp = blocks.TakeLast(1).First();
                if (tmp < targetBlock) return false;
                emptyBlocks.Enqueue(tmp);
                blocks = blocks.SkipLast(1).Prepend(targetBlock).ToList();
                return true;
            }

            public void moveFile(long startBlock)
            {
                blocks = LongRange(startBlock, blocks.Count).ToList();
            }
        }

        public static IEnumerable<long> LongRange(long start, long count)
        {
            var end = start + count;
            for (var current = start; current < end; ++current)
            {
                yield return current;
            }
        }

        private int[] DiskMap(string input)
        {
            int[] disk = input.Select(x => int.Parse($"{x}")).ToArray();
            int fileId = 0;

            List<int> result = new();

            for (int i = 0; i < disk.Length; i++)
            {
                int length = disk[i];

                if (i % 2 == 0)
                {
                    for (int j = 0; j < length; j++)
                    {
                        result.Add(fileId);
                    }

                    fileId++;
                }
                else
                {
                    for (int j = 0; j < length; j++)
                    {
                        result.Add('.');
                    }
                }
            }

            return result.ToArray();
        }

        private int FindNext(int space, int[] disk)
        {
            for(int i = space + 1; i < disk.Length; i++)
            {
                if (disk[i] == '.')
                {
                    return i;
                }
            }

            return -1;
        }

        public long calculateChecksum(int[] nd)
        {
            long checksum = 0;
            for (int i = 0; i < nd.Length; i++)
            {
                if (nd[i] >= 0)
                {
                    checksum += nd[i] * i;
                }
            }
            return checksum;
        }

        private void Compact(int[] disk)
        {
            int space = 0;

            while (true)
            {
                int firstGap = FindNext(space, disk);

                if (firstGap == -1)
                {
                    break;
                }

                int lastFilePos = disk.Length - 1;

                while (lastFilePos >= 0 && disk[lastFilePos] == '.')
                {
                    lastFilePos--;
                }

                if (lastFilePos <= firstGap)
                {
                    break;
                }

                disk[firstGap] = disk[lastFilePos];
                disk[lastFilePos] = '.';
            }
        }

        public string Silver()
        {
            return $"{SolvePartOne()}";


            ////string input = this.Input[0];
            ////string harddisk = string.Empty;

            ////for(int i = 0; i < input.Length; i++)
            ////{
            ////    if (i % 2 == 0)
            ////    {
            ////        int x = i / 2;

            ////        for (int k = 0; k < int.Parse($"{input[i]}"); k++)
            ////        {
            ////            harddisk += x.ToString();
            ////        }
            ////    }
            ////    else
            ////    {
            ////        harddisk += new string('.', int.Parse($"{input[i]}"));
            ////    }
            ////}

            ////int countA = 0;

            ////foreach (char c in harddisk)
            ////{
            ////    if (c == '.')
            ////    {
            ////        countA++;
            ////    }
            ////}

            ////int space = harddisk.IndexOf('.');
            ////char[] ordered = harddisk.ToCharArray();

            //////Console.WriteLine(new string(ordered));

            ////for (int i = ordered.Length - 1; i >=0; i--)
            ////{
            ////    ordered[space] = ordered[i];
            ////    ordered[i] = '.';
            ////    space = new string(ordered).IndexOf('.');

            ////    if (space >= i)
            ////    {
            ////        break;
            ////    }

            ////    //Console.WriteLine(new string(ordered));
            ////}

            //////Console.WriteLine(new string(ordered));

            ////int countB = 0;

            ////foreach(char c in ordered)
            ////{
            ////    if (c == '.')
            ////    {
            ////        countB++;
            ////    }
            ////}

            ////harddisk = new string(ordered);

            ////long result = 0;

            ////for(int i = 0; i < ordered.Length; i++)
            ////{
            ////    if (ordered[i] == '.')
            ////    {
            ////        break;
            ////    }

            ////    result += i * long.Parse(ordered[i].ToString());
            ////}

            ////return $"{result}";
        }

        public string Gold()
        {
            return $"{SolvePartTwo()}";
        }
    }
}
