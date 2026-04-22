namespace AdventOfCode.Puzzles._2024.Day_09___Disk_Fragmenter
{
    public sealed class DiskFragmenter
    {
        private readonly Dictionary<long, AmphipodFile> AamphipodFiles = [];
        private readonly List<(long Start, long Length)> EmptyRanges = [];
        private readonly Queue<long> EmptyBlocks = new();
        private readonly Stack<long> FileBlocks = new();
        private readonly long MaxFileId;

        public DiskFragmenter(string[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0 || string.IsNullOrWhiteSpace(input[0]))
            {
                throw new ArgumentException("Input must contain a disk map string.", nameof(input));
            }

            List<long> diskMap = ParseDigits(input[0]);

            long position = 0;
            long fileId = 0;

            for (int i = 0; i < diskMap.Count; i++)
            {
                long length = diskMap[i];

                if ((i & 1) == 0)
                {
                    var amphipodFile = new AmphipodFile(fileId);

                    for (long j = 0; j < length; j++)
                    {
                        amphipodFile.Blocks.Add(position);
                        FileBlocks.Push(fileId);
                        position++;
                    }

                    AamphipodFiles[fileId] = amphipodFile;
                    fileId++;
                }
                else
                {
                    if (length <= 0)
                    {
                        continue;
                    }

                    EmptyRanges.Add((position, length));

                    for (long j = 0; j < length; j++)
                    {
                        EmptyBlocks.Enqueue(position);
                        position++;
                    }
                }
            }

            MaxFileId = fileId - 1;
        }

        public long FilesystemChecksum()
        {
            while (FileBlocks.Count > 0 && EmptyBlocks.Count > 0)
            {
                long fileId = FileBlocks.Pop();
                long emptyBlock = EmptyBlocks.Dequeue();

                if (!AamphipodFiles[fileId].TryMoveLastBlock(emptyBlock, EmptyBlocks))
                {
                    break;
                }
            }

            return AamphipodFiles.Values.Sum(file => file.Checksum);
        }

        public long WholeFilesystemChecksum()
        {
            for (long fileId = MaxFileId; fileId > 0; fileId--)
            {
                var file = AamphipodFiles[fileId];
                long requiredSpace = file.Blocks.Count;
                long fileStart = file.Blocks[0];

                int rangeIndex = EmptyRanges.FindIndex(range =>
                    range.Length >= requiredSpace &&
                    range.Start < fileStart);

                if (rangeIndex < 0)
                {
                    continue;
                }

                var (start, length) = EmptyRanges[rangeIndex];

                file.MoveFile(start);

                EmptyRanges.RemoveAt(rangeIndex);

                long remainingLength = length - requiredSpace;
                if (remainingLength > 0)
                {
                    EmptyRanges.Insert(rangeIndex, (start + requiredSpace, remainingLength));
                }
            }

            return AamphipodFiles.Values.Sum(file => file.Checksum);
        }

        private static List<long> ParseDigits(string value)
        {
            List<long> result = new List<long>(value.Length);

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                result.Add(c - '0');
            }

            return result;
        }

        private sealed class AmphipodFile(long id)
        {
            public long Id { get; } = id;

            public List<long> Blocks { get; private set; } = [];

            public long Checksum => Blocks.Sum(block => block * Id);

            public bool TryMoveLastBlock(long targetBlock, Queue<long> emptyBlocks)
            {
                if (Blocks.Count == 0)
                {
                    return false;
                }

                long lastBlock = Blocks[^1];
                if (lastBlock < targetBlock)
                {
                    return false;
                }

                emptyBlocks.Enqueue(lastBlock);
                Blocks[^1] = targetBlock;
                Blocks.Sort();

                return true;
            }

            public void MoveFile(long startBlock)
            {
                List<long> newBlocks = new(this.Blocks.Count);

                for (int i = 0; i < this.Blocks.Count; i++)
                {
                    newBlocks.Add(startBlock + i);
                }

                this.Blocks = newBlocks;
            }
        }
    }
}