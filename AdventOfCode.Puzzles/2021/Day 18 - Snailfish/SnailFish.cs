namespace AdventOfCode.Puzzles._2021.Day_18___Snailfish
{
    using System.Text;
    using AdventOfCode.Core;

    public class SnailFish
    {
        public SnailFish(int? value, SnailFish? parent)
        {
            this.RegularNumber = value;
            this.Parent = parent;
        }

        public SnailFish(int? value, SnailFish? parent, SnailFish? fishA, SnailFish? fishB)
        {
            this.RegularNumber = value;
            this.Parent = parent;

            if (fishA != null)
            {
                this.A = fishA;
                this.A.Parent = this;
            }

            if (fishB != null)
            {
                this.B = fishB;
                this.B.Parent = this;
            }
        }

        public int? RegularNumber { get; private set; }

        public SnailFish? A { get; private set; }

        public SnailFish? B { get; private set; }

        public SnailFish? Parent { get; private set; }

        public bool IsRegularNumber() => this.RegularNumber.HasValue;

        public bool HasA() => this.A != null;

        public bool HasB() => this.B != null;

        public bool IsPair() => this.HasA() && (this.A?.IsRegularNumber() ?? false) && (this.HasB() && (this.B?.IsRegularNumber() ?? false));

        public bool IsSplitA() => this.HasA() && (this.A?.RegularNumber ?? 0) >= 10;

        public bool IsSplitB() => this.HasB() && (this.B?.RegularNumber ?? 0) >= 10;

        public SnailFish? Addition()
        {
            this.Addition(this);
            return this;
        }

        public SnailFish? Explode()
        {
            Explode(this);
            return this;
        }

        public SnailFish? Split()
        {
            this.Split(this);
            return this;
        }

        public long Magnitude() => this.Magnitude(this);

        public void ToRegularNumber()
        {
            this.A = null;
            this.B = null;
            this.RegularNumber = 0;
        }

        public SnailFish? SetParent(SnailFish fish)
        {
            this.Parent = fish;

            return this;
        }

        public SnailFish? SetA(SnailFish fish)
        {
            this.A = fish;

            return this;
        }

        public SnailFish? SetB(SnailFish fish)
        {
            this.B = fish;

            return this;
        }

        public SnailFish? IncrementRegularNumber(int number)
        {
            this.RegularNumber += number;

            return this;
        }

        public bool Equal(SnailFish fish) => this.A == fish || this.B == fish;

        public new string ToString()
        {
            StringBuilder sb = new();

            sb.Append('[');

            if (this.A?.IsRegularNumber() ?? false)
            {
                sb.Append(this.A.RegularNumber);
            }
            else
            {
                sb.Append(this.A?.ToString());
            }

            sb.Append(',');

            if (this.B?.IsRegularNumber() ?? false)
            {
                sb.Append(this.B?.RegularNumber);
            }
            else
            {
                sb.Append(this.B?.ToString());
            }

            sb.Append(']');

            return sb.ToString();
        }

        public SnailFish Print()
        {
            PuzzleConsole.WriteLine(this.ToString());

            return this;
        }

        private static SnailFish? SearchLeft(SnailFish fish)
        {
            SnailFish? parent = fish.Parent;
            SnailFish? last = fish;
            bool searchForward = false;

            while (parent != null)
            {
                if (searchForward && parent.IsRegularNumber())
                {
                    return parent;
                }

                if (searchForward && (parent?.B?.IsRegularNumber() ?? false))
                {
                    return parent.B;
                }

                if (!searchForward && (parent?.A?.IsRegularNumber() ?? false))
                {
                    return parent.A;
                }

                if (last == parent?.B)
                {
                    searchForward = true;
                    parent = parent?.A;
                }

                last = parent;
                parent = !searchForward ? parent?.Parent : parent?.B;
            }

            return null;
        }

        private static SnailFish? SearchRight(SnailFish fish)
        {
            SnailFish? parent = fish.Parent;
            SnailFish last = fish;
            bool searchForward = false;

            while (true)
            {
                if (parent?.Parent == null)
                {
                    if (last == parent?.A)
                    {
                        last = parent;
                        parent = parent?.B;
                    }
                    else
                    {
                        return null;
                    }

                    searchForward = true;

                    continue;
                }

                if (parent.IsRegularNumber())
                {
                    return parent;
                }

                if (!searchForward && parent?.A == last)
                {
                    last = parent;
                    parent = parent.B;
                    searchForward = true;
                    continue;
                }
                else if (!searchForward && parent?.B == last)
                {
                    last = parent;
                    parent = parent?.Parent;
                    continue;
                }

                parent = parent?.A;
            }
        }

        private static void Explode(SnailFish fish)
        {
            SearchLeft(fish)?.IncrementRegularNumber(fish.A?.RegularNumber ?? 0);
            SearchRight(fish)?.IncrementRegularNumber(fish.B?.RegularNumber ?? 0);
            fish.ToRegularNumber();
        }

        private long Magnitude(SnailFish fish)
        {
            if (fish.IsPair())
            {
                return (3 * fish.A?.RegularNumber ?? 0) + (2 * fish.B?.RegularNumber ?? 0);
            }

            long left = 0;
            long right = 0;

            if (fish.A != null)
            {
                left = fish.A.IsRegularNumber() ? fish.A.RegularNumber ?? 0 : this.Magnitude(fish.A);
            }

            if (fish.B != null)
            {
                right = fish.B.IsRegularNumber() ? fish.B.RegularNumber ?? 0 : this.Magnitude(fish.B);
            }

            return (3 * left) + (2 * right);
        }

        private void Split(SnailFish? fish)
        {
            int left = (int)Math.Floor((double)(fish?.RegularNumber ?? 0) / 2);
            int right = (int)Math.Ceiling((double)(fish?.RegularNumber ?? 0) / 2);

            SnailFish child = new(null, fish?.Parent);
            child.A = new(left, child);
            child.B = new(right, child);

            if (fish == fish?.Parent?.A
                && fish?.Parent?.A != null)
            {
                fish.Parent.A.RegularNumber = 0;
                fish.Parent.A = child;
            }

            if (fish == fish?.Parent?.B
                && fish?.Parent?.B != null)
            {
                fish.Parent.B.RegularNumber = 0;
                fish.Parent.B = child;
            }
        }

        private SnailFish Addition(SnailFish fish)
        {
            while (true)
            {
                SnailFish? explode = this.NextExplode(fish, 0)?.Explode();
                SnailFish? split = null;

                if (explode == null)
                {
                    split = this.NextSplit(fish)?.Split();
                }

                if (explode == null && split == null)
                {
                    return fish;
                }
            }
        }

        private SnailFish? NextExplode(SnailFish fish, int depth)
        {
            depth++;

            if (fish.IsPair() && depth > 4)
            {
                return fish;
            }

            if (fish.A != null)
            {
                SnailFish? result = this.NextExplode(fish.A, depth);

                if (result != null)
                {
                    return result;
                }
            }

            if (fish.B != null)
            {
                return this.NextExplode(fish.B, depth);
            }

            return null;
        }

        private SnailFish? NextSplit(SnailFish? fish)
        {
            if (fish == null)
            {
                return null;
            }

            if (fish.IsSplitA())
            {
                return fish.A;
            }
            else
            {
                SnailFish? result = this.NextSplit(fish.A);

                if (result != null)
                {
                    return result;
                }
            }

            if (fish.IsSplitB())
            {
                return fish.B;
            }
            else
            {
                return this.NextSplit(fish.B);
            }
        }
    }
}
