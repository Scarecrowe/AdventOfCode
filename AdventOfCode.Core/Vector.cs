namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Extensions;

    public class Vector<TSize> : IEquatable<Vector<TSize>>
    {
        public Vector()
        {
            this.X = 0.ToGeneric<TSize>();
            this.Y = 0.ToGeneric<TSize>();
            this.Z = 0.ToGeneric<TSize>();
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(TSize x, TSize y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0.ToGeneric<TSize>();
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(TSize x, TSize y, TSize z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(TSize x, TSize y, TSize z, TSize t)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.T = t;
        }

        public Vector(int x, int y)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(0);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(0);
                this.T = (TSize)(object)Convert.ToInt32(0);

                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(0);
            this.T = (TSize)(object)Convert.ToInt64(0);
        }

        public Vector(long x, long y)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(0);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(0);
                this.T = (TSize)(object)Convert.ToInt32(0);

                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(0);
            this.T = (TSize)(object)Convert.ToInt64(0);
        }

        public Vector(double x, double y)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(0);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(0);
                this.T = (TSize)(object)Convert.ToInt32(0);

                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(0);
            this.T = (TSize)(object)Convert.ToInt64(0);
        }

        public Vector(int x, int y, int z)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = 0.ToGeneric<TSize>();

                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(long x, long y, long z)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = 0.ToGeneric<TSize>();
                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(double x, double y, double z)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(0);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = 0.ToGeneric<TSize>();
                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = 0.ToGeneric<TSize>();
        }

        public Vector(int x, int y, int z, int t)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(t);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = (TSize)(object)Convert.ToInt32(t);

                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = (TSize)(object)Convert.ToInt64(t);
        }

        public Vector(long x, long y, long z, long t)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(t);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = (TSize)(object)Convert.ToInt32(t);
                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = (TSize)(object)Convert.ToInt64(t);
        }

        public Vector(double x, double y, double z, double t)
        {
            if (typeof(TSize) == typeof(double))
            {
                this.X = (TSize)(object)Convert.ToDouble(x);
                this.Y = (TSize)(object)Convert.ToDouble(y);
                this.Z = (TSize)(object)Convert.ToDouble(z);
                this.T = (TSize)(object)Convert.ToDouble(t);

                return;
            }

            if (typeof(TSize) == typeof(int))
            {
                this.X = (TSize)(object)Convert.ToInt32(x);
                this.Y = (TSize)(object)Convert.ToInt32(y);
                this.Z = (TSize)(object)Convert.ToInt32(z);
                this.T = (TSize)(object)Convert.ToInt32(t);
                return;
            }

            this.X = (TSize)(object)Convert.ToInt64(x);
            this.Y = (TSize)(object)Convert.ToInt64(y);
            this.Z = (TSize)(object)Convert.ToInt64(z);
            this.T = (TSize)(object)Convert.ToInt64(t);
        }

        public Vector(IEnumerable<TSize> axis)
            : this()
        {
            if (axis.Count() <= 1
                || axis.Count() > 4)
            {
                throw new ArgumentException();
            }

            if (axis.Count() >= 2)
            {
                this.X = axis.ElementAt(0);
                this.Y = axis.ElementAt(1);
            }

            if (axis.Count() >= 3)
            {
                this.Z = axis.ElementAt(2);
            }

            if (axis.Count() >= 4)
            {
                this.T = axis.ElementAt(3);
            }
        }

        public Vector(Vector<TSize> point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.T = point.T;
        }

        public static Vector<TSize> North { get; } = new(0, -1);

        public static Vector<TSize> South { get; } = new(0, 1);

        public static Vector<TSize> West { get; } = new(-1, 0);

        public static Vector<TSize> East { get; } = new(1, 0);

        public static Vector<TSize> NorthWest { get; } = new(-1, -1);

        public static Vector<TSize> NorthEast { get; } = new(1, -1);

        public static Vector<TSize> SouthWest { get; } = new(-1, 1);

        public static Vector<TSize> SouthEast { get; } = new(1, 1);

        public static List<Vector<TSize>> Outer { get; } = new() { new(0, 0), new(0, 1), new(0, 2), new(0, 3), new(0, 4), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(4, 1), new(2, 2), new(3, 3), new(4, 4), new(1, 4), new(2, 4), new(3, 4) };

        public static List<Vector<TSize>> Inner { get; } = new() { new(1, 1), new(2, 1), new(3, 1), new(3, 2), new(3, 3), new(2, 3), new(1, 3), new(1, 2) };

        public TSize X { get; set; }

        public TSize Y { get; set; }

        public TSize Z { get; set; }

        public TSize T { get; set; }

        public static bool operator ==(Vector<TSize>? pointA, Vector<TSize>? pointB)
            => pointA?.GetHashCode() == pointB?.GetHashCode();

        public static bool operator !=(Vector<TSize> pointA, Vector<TSize> pointB)
            => pointA?.GetHashCode() != pointB?.GetHashCode();

        public static Vector<TSize> operator -(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (typeof(TSize) == typeof(double))
            {
                return SubtractDouble(pointA, pointB);
            }
            else if (typeof(TSize) == typeof(int))
            {
                return SubtractInt(pointA, pointB);
            }

            return SubtractLong(pointA, pointB);
        }

        public static Vector<TSize> operator -(Vector<TSize> pointA, TSize value)
        {
            if (typeof(TSize) == typeof(double))
            {
                return SubtractDouble(pointA, new(value, value, value, value));
            }
            else if (typeof(TSize) == typeof(int))
            {
                return SubtractInt(pointA, new(value, value, value, value));
            }

            return SubtractLong(pointA, new(value, value, value, value));
        }

        public static Vector<TSize> operator +(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (typeof(TSize) == typeof(double))
            {
                return AddDouble(pointA, pointB);
            }

            if (typeof(TSize) == typeof(int))
            {
                return AddInt(pointA, pointB);
            }

            return AddLong(pointA, pointB);
        }

        public static Vector<TSize> operator +(Vector<TSize> pointA, TSize value)
        {
            if (typeof(TSize) == typeof(double))
            {
                return AddDouble(pointA, new(value, value, value, value));
            }
            else if (typeof(TSize) == typeof(int))
            {
                return AddInt(pointA, new(value, value, value, value));
            }

            return AddLong(pointA, new(value, value, value, value));
        }

        public static Vector<TSize> operator *(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (typeof(TSize) == typeof(double))
            {
                return ProductDouble(pointA, pointB);
            }
            else if (typeof(TSize) == typeof(int))
            {
                return ProductInt(pointA, pointB);
            }

            return ProductLong(pointA, pointB);
        }

        public static Vector<TSize> operator *(Vector<TSize> pointA, TSize value)
        {
            if (typeof(TSize) == typeof(double))
            {
                return ProductDouble(pointA, new(value, value, value, value));
            }
            else if (typeof(TSize) == typeof(int))
            {
                return ProductInt(pointA, new(value, value, value, value));
            }

            return ProductLong(pointA, new(value, value, value, value));
        }

        public static Vector<TSize> operator /(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (typeof(TSize) == typeof(double))
            {
                return DivideDouble(pointA, pointB);
            }
            else if (typeof(TSize) == typeof(int))
            {
                return DivideInt(pointA, pointB);
            }

            return DivideLong(pointA, pointB);
        }

        public static Vector<TSize> operator /(Vector<TSize> pointA, TSize value)
        {
            if (typeof(TSize) == typeof(double))
            {
                return DivideDouble(pointA, new(value, value, value, value));
            }
            else if (typeof(TSize) == typeof(int))
            {
                return DivideInt(pointA, new(value, value, value, value));
            }

            return DivideLong(pointA, new(value, value, value, value));
        }

        public static IEnumerable<Vector<TSize>> AxisEnumerator(TSize width, TSize height)
        {
            for (long y = 0; y < height.ToLong(); y++)
            {
                for (long x = 0; x < width.ToLong(); x++)
                {
                    yield return new(x, y);
                }
            }
        }

        public static IEnumerable<Vector<TSize>> AxisEnumerator(Vector<TSize> start, TSize width, TSize height)
        {
            for (long y = start.Y.ToLong(); y < height.ToLong(); y++)
            {
                for (long x = start.X.ToLong(); x < width.ToLong(); x++)
                {
                    yield return new(x, y);
                }
            }
        }

        public static Vector<TSize> GetPointByCardinal(Cardinal cardinal)
        {
            return cardinal switch
            {
                Cardinal.North => Vector<TSize>.North,
                Cardinal.NorthWest => Vector<TSize>.NorthWest,
                Cardinal.NorthEast => Vector<TSize>.NorthEast,
                Cardinal.South => Vector<TSize>.South,
                Cardinal.SouthWest => Vector<TSize>.SouthWest,
                Cardinal.SouthEast => Vector<TSize>.SouthEast,
                Cardinal.East => Vector<TSize>.East,
                Cardinal.West => Vector<TSize>.West,
                _ => throw new InvalidOperationException(),
            };
        }

        public Vector<TSize> Clone() => new(this);

        public (TSize X, TSize Y) ToTuple2D() => (this.X, this.Y);

        public (TSize X, TSize Y, TSize Z) ToTuple3D() => (this.X, this.Y, this.Z);

        public (TSize X, TSize Y, TSize Z, TSize T) ToTuple4D() => (this.X, this.Y, this.Z, this.T);

        public string ToKey2D() => $"{this.X}:{this.Y}";

        public string ToKey3D() => $"{this.X}:{this.Y}:{this.Z}";

        public string ToKey4D() => $"{this.X}:{this.Y}:{this.Z}:{this.T}";

        public List<TSize> ToList2D() => new() { this.X, this.Y };

        public List<TSize> ToList3D() => new() { this.X, this.Y, this.Z };

        public List<TSize> ToList4D() => new() { this.X, this.Y, this.Z, this.T };

        public int[] ToIntArray()
        {
            int x = Convert.ToInt32(this.X);
            int y = Convert.ToInt32(this.Y);
            int z = Convert.ToInt32(this.Z);
            int t = Convert.ToInt32(this.T);

            return new int[] { x, y, z, t };
        }

        public long[] ToLongArray()
        {
            long x = Convert.ToInt64(this.X);
            long y = Convert.ToInt64(this.Y);
            long z = Convert.ToInt64(this.Z);
            long t = Convert.ToInt64(this.T);

            return new long[] { x, y, z, t };
        }

        public double[] ToDoubleArray()
        {
            double x = Convert.ToDouble(this.X);
            double y = Convert.ToDouble(this.Y);
            double z = Convert.ToDouble(this.Z);
            double t = Convert.ToDouble(this.T);

            return new double[] { x, y, z, t };
        }

        public new string ToString()
            => $"x: {this.X}, y: {this.Y}, z: {this.Z}, z: {this.T}";

        public bool Equals(Vector<TSize>? point)
            => point?.GetHashCode() == this.GetHashCode();

        public override bool Equals(object? obj)
            => this.Equals((Vector<TSize>)(obj ?? new()));

        public override int GetHashCode()
            => (this.X, this.Y, this.Z, this.T).GetHashCode();

        public TSize Absolute()
        {
            if (typeof(TSize) == typeof(double))
            {
                return this.AbsoluteDouble();
            }
            else if (typeof(TSize) == typeof(int))
            {
                return this.AbsoluteInt();
            }

            return this.AbsoluteLong();
        }

        public TSize Distance(Vector<TSize> point)
        {
            if (typeof(TSize) == typeof(double))
            {
                return this.DistanceDouble(point);
            }
            else if (typeof(TSize) == typeof(int))
            {
                return this.DistanceInt(point);
            }

            return this.DistanceLong(point);
        }

        public Vector<TSize> Negate()
        {
            if (typeof(TSize) == typeof(double))
            {
                return this.NegateDouble();
            }
            else if (typeof(TSize) == typeof(int))
            {
                return this.NegateInt();
                }

            return this.NegateLong();
        }

        public List<TSize> AbsoluteValues()
        {
            if (typeof(TSize) == typeof(double))
            {
                return this.AbsoluteValuesDouble();
            }
            else if (typeof(TSize) == typeof(int))
            {
                return this.AbsoluteValuesInt();
            }

            return this.AbsoluteValuesLong();
        }

        public IEnumerable<Vector<TSize>> Around(TSize minX, TSize minY, TSize maxX, TSize maxY)
        {
            if (typeof(TSize) == typeof(double))
            {
                return this.AroundDouble(minX, minY, maxX, maxY);
            }
            else if (typeof(TSize) == typeof(int))
            {
                return this.AroundInt(minX, minY, maxX, maxY);
            }

            return this.AroundLong(minX, minY, maxX, maxY);
        }

        public bool IsLineOfSight(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (this.Equals(pointA) || this.Equals(pointB) || pointA.Equals(pointB))
            {
                return false;
            }

            return this.IsPointsInLine(pointA, pointB)
                && ((this.X.ToLong() <= pointA.X.ToLong() && pointA.X.ToLong() <= pointB.X.ToLong()) || (this.X.ToLong() >= pointA.X.ToLong() && pointA.X.ToLong() >= pointB.X.ToLong()))
                && ((this.Y.ToLong() <= pointA.Y.ToLong() && pointA.Y.ToLong() <= pointB.Y.ToLong()) || (this.Y.ToLong() >= pointA.Y.ToLong() && pointA.Y.ToLong() >= pointB.Y.ToLong()));
        }

        public Vector<TSize> Rotate(int degrees)
        {
            double rads = degrees * (Math.PI / 180);
            int x = (int)Math.Round((this.X.ToInt() * Math.Cos(rads)) - (this.Y.ToInt() * (int)Math.Sin(rads)), MidpointRounding.ToEven);
            int y = (int)Math.Round((this.X.ToInt() * Math.Sin(rads)) + (this.Y.ToInt() * (int)Math.Cos(rads)), MidpointRounding.ToEven);

            return new(x, y);
        }

        public Vector<TSize> Transform(Cardinal direction)
        {
            Vector<TSize> transform = this + CardinalHelper.AllTransform<TSize>()[direction];
            this.X = transform.X;
            this.Y = transform.Y;

            return transform;
        }

        public Vector<TSize> Transform(Vector<TSize> point)
        {
            Vector<TSize> transform = this + point;
            this.X = transform.X;
            this.Y = transform.Y;

            return transform;
        }

        public void SymbolTransform(char direction)
        {
            Vector<TSize> transform = this + CardinalHelper.CardinalTransform<TSize>()[CardinalHelper.SymbolToCardinalMap[direction]];
            this.X = transform.X;
            this.Y = transform.Y;
        }

        public void LetterTransform(char direction)
        {
            Vector<TSize> transform = this + CardinalHelper.CardinalTransform<TSize>()[CardinalHelper.LetterToCardinalMap[direction]];
            this.X = transform.X;
            this.Y = transform.Y;
        }

        public void CompassTransform(string direction)
        {
            Vector<TSize> transform = this + CardinalHelper.AllTransform<TSize>()[CardinalHelper.CompassToCardinalMap[direction]];
            this.X = transform.X;
            this.Y = transform.Y;
        }

        private static Vector<TSize> SubtractInt(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            int[] a = pointA.ToIntArray();
            int[] b = pointB.ToIntArray();

            return new Vector<TSize>((TSize)(object)(a[0] - b[0]), (TSize)(object)(a[1] - b[1]), (TSize)(object)(a[2] - b[2]), (TSize)(object)(a[3] - b[3]));
        }

        private static Vector<TSize> SubtractLong(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            long[] a = pointA.ToLongArray();
            long[] b = pointB.ToLongArray();

            return new Vector<TSize>((TSize)(object)(a[0] - b[0]), (TSize)(object)(a[1] - b[1]), (TSize)(object)(a[2] - b[2]), (TSize)(object)(a[3] - b[3]));
        }

        private static Vector<TSize> SubtractDouble(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            double[] a = pointA.ToDoubleArray();
            double[] b = pointB.ToDoubleArray();

            return new Vector<TSize>((TSize)(object)(a[0] - b[0]), (TSize)(object)(a[1] - b[1]), (TSize)(object)(a[2] - b[2]), (TSize)(object)(a[3] - b[3]));
        }

        private static Vector<TSize> AddInt(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            int[] a = pointA.ToIntArray();
            int[] b = pointB.ToIntArray();

            return new Vector<TSize>((TSize)(object)(a[0] + b[0]), (TSize)(object)(a[1] + b[1]), (TSize)(object)(a[2] + b[2]), (TSize)(object)(a[3] + b[3]));
        }

        private static Vector<TSize> AddLong(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            long[] a = pointA.ToLongArray();
            long[] b = pointB.ToLongArray();

            return new Vector<TSize>((TSize)(object)(a[0] + b[0]), (TSize)(object)(a[1] + b[1]), (TSize)(object)(a[2] + b[2]), (TSize)(object)(a[3] + b[3]));
        }

        private static Vector<TSize> AddDouble(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            double[] a = pointA.ToDoubleArray();
            double[] b = pointB.ToDoubleArray();

            return new Vector<TSize>((TSize)(object)(a[0] + b[0]), (TSize)(object)(a[1] + b[1]), (TSize)(object)(a[2] + b[2]), (TSize)(object)(a[3] + b[3]));
        }

        private static Vector<TSize> ProductInt(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            int[] a = pointA.ToIntArray();
            int[] b = pointB.ToIntArray();

            return new Vector<TSize>((TSize)(object)(a[0] * b[0]), (TSize)(object)(a[1] * b[1]), (TSize)(object)(a[2] * b[2]), (TSize)(object)(a[3] * b[3]));
        }

        private static Vector<TSize> ProductLong(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            long[] a = pointA.ToLongArray();
            long[] b = pointB.ToLongArray();

            return new Vector<TSize>((TSize)(object)(a[0] * b[0]), (TSize)(object)(a[1] * b[1]), (TSize)(object)(a[2] * b[2]), (TSize)(object)(a[3] * b[3]));
        }

        private static Vector<TSize> ProductDouble(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            double[] a = pointA.ToDoubleArray();
            double[] b = pointB.ToDoubleArray();

            return new Vector<TSize>((TSize)(object)(a[0] * b[0]), (TSize)(object)(a[1] * b[1]), (TSize)(object)(a[2] * b[2]), (TSize)(object)(a[3] * b[3]));
        }

        private static Vector<TSize> DivideInt(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            int[] a = pointA.ToIntArray();
            int[] b = pointB.ToIntArray();

            return new Vector<TSize>(
                (TSize)(object)(a[0] / b[0]),
                (TSize)(object)(a[1] / b[1]),
                (TSize)(object)(a[2] == 0 && b[2] == 0 ? 0 : a[2] / b[2]),
                (TSize)(object)(a[3] == 0 && b[3] == 0 ? 0 : a[3] / b[3]));
        }

        private static Vector<TSize> DivideLong(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            long[] a = pointA.ToLongArray();
            long[] b = pointB.ToLongArray();

            return new Vector<TSize>(
                (TSize)(object)(a[0] / b[0]),
                (TSize)(object)(a[1] / b[1]),
                (TSize)(object)(a[2] == 0 && b[2] == 0 ? 0 : a[2] / b[2]),
                (TSize)(object)(a[3] == 0 && b[3] == 0 ? 0 : a[3] / b[3]));
        }

        private static Vector<TSize> DivideDouble(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            double[] a = pointA.ToDoubleArray();
            double[] b = pointB.ToDoubleArray();

            return new Vector<TSize>(
                (TSize)(object)(a[0] / b[0]),
                (TSize)(object)(a[1] / b[1]),
                (TSize)(object)(a[2] == 0 && b[2] == 0 ? 0 : a[2] / b[2]),
                (TSize)(object)(a[3] == 0 && b[3] == 0 ? 0 : a[3] / b[3]));
        }

        private bool IsPointsInLine(Vector<TSize> pointA, Vector<TSize> pointB)
        {
            if (this.X.ToLong() == pointA.X.ToLong() && this.X.ToLong() == pointB.X.ToLong())
            {
                return true;
            }
            else if (this.X.ToLong() == pointA.X.ToLong() || this.X.ToLong() == pointB.X.ToLong())
            {
                return false;
            }

            return Math.Abs((double)((pointA.Y.ToLong() - this.Y.ToLong()) / (double)(pointA.X.ToLong() - this.X.ToLong()))) ==
                Math.Abs((double)((pointB.Y.ToLong() - this.Y.ToLong()) / (double)(pointB.X.ToLong() - this.X.ToLong())));
        }

        private TSize DistanceInt(Vector<TSize> point)
        {
            int result = (Convert.ToInt32(this.X) - Convert.ToInt32(point.X)).Abs()
                + (Convert.ToInt32(this.Y) - Convert.ToInt32(point.Y)).Abs()
                + (Convert.ToInt32(this.Z) - Convert.ToInt32(point.Z)).Abs()
                + (Convert.ToInt32(this.T) - Convert.ToInt32(point.T)).Abs();

            return (TSize)(object)result;
        }

        private TSize DistanceLong(Vector<TSize> point)
        {
            long result = (Convert.ToInt64(this.X) - Convert.ToInt64(point.X)).Abs()
                + (Convert.ToInt64(this.Y) - Convert.ToInt64(point.Y)).Abs()
                + (Convert.ToInt64(this.Z) - Convert.ToInt64(point.Z)).Abs()
                + (Convert.ToInt64(this.T) - Convert.ToInt64(point.T)).Abs();

            return (TSize)(object)result;
        }

        private TSize DistanceDouble(Vector<TSize> point)
        {
            double result = (Convert.ToDouble(this.X) - Convert.ToDouble(point.X)).Abs()
                + (Convert.ToDouble(this.Y) - Convert.ToDouble(point.Y)).Abs()
                + (Convert.ToDouble(this.Z) - Convert.ToDouble(point.Z)).Abs()
                + (Convert.ToDouble(this.T) - Convert.ToDouble(point.T)).Abs();

            return (TSize)(object)result;
        }

        private TSize AbsoluteInt()
        {
            int x = Convert.ToInt32(this.X);
            int y = Convert.ToInt32(this.Y);
            int z = Convert.ToInt32(this.Z);
            int t = Convert.ToInt32(this.T);

            return (TSize)(object)(Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(t));
        }

        private TSize AbsoluteLong()
        {
            long x = Convert.ToInt64(this.X);
            long y = Convert.ToInt64(this.Y);
            long z = Convert.ToInt64(this.Z);
            long t = Convert.ToInt64(this.T);

            return (TSize)(object)(Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(t));
        }

        private TSize AbsoluteDouble()
        {
            double x = Convert.ToDouble(this.X);
            double y = Convert.ToDouble(this.Y);
            double z = Convert.ToDouble(this.Z);
            double t = Convert.ToDouble(this.T);

            return (TSize)(object)(Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(t));
        }

        private Vector<TSize> NegateInt()
        {
            int x = Convert.ToInt32(this.X);
            int y = Convert.ToInt32(this.Y);
            int z = Convert.ToInt32(this.Z);
            int t = Convert.ToInt32(this.T);

            return new Vector<TSize>((TSize)(object)-x, (TSize)(object)-y, (TSize)(object)-z, (TSize)(object)-t);
        }

        private Vector<TSize> NegateLong()
        {
            long x = Convert.ToInt64(this.X);
            long y = Convert.ToInt64(this.Y);
            long z = Convert.ToInt64(this.Z);
            long t = Convert.ToInt64(this.T);

            return new Vector<TSize>((TSize)(object)-x, (TSize)(object)-y, (TSize)(object)-z, (TSize)(object)-t);
        }

        private Vector<TSize> NegateDouble()
        {
            double x = Convert.ToDouble(this.X);
            double y = Convert.ToDouble(this.Y);
            double z = Convert.ToDouble(this.Z);
            double t = Convert.ToDouble(this.T);

            return new Vector<TSize>((TSize)(object)-x, (TSize)(object)-y, (TSize)(object)-z, (TSize)(object)-t);
        }

        private List<TSize> AbsoluteValuesInt() => this.ToIntArray().Select(x => (TSize)(object)Math.Abs(x)).ToList();

        private List<TSize> AbsoluteValuesLong() => this.ToLongArray().Select(x => (TSize)(object)Math.Abs(x)).ToList();

        private List<TSize> AbsoluteValuesDouble() => this.ToDoubleArray().Select(x => (TSize)(object)Math.Abs(x)).ToList();

        private IEnumerable<Vector<TSize>> AroundInt(TSize minX, TSize minY, TSize maxX, TSize maxY)
        {
            int x = Convert.ToInt32(this.X);
            int y = Convert.ToInt32(this.Y);

            if (x + 1 >= minX.ToInt() && x + 1 <= maxX.ToInt())
            {
                yield return new(x + 1, y);
            }

            if (x - 1 >= minX.ToInt() && x - 1 <= maxX.ToInt())
            {
                yield return new(x - 1, y);
            }

            if (y + 1 >= minY.ToInt() && y + 1 <= maxY.ToInt())
            {
                yield return new(x, y + 1);
            }

            if (y - 1 >= minY.ToInt() && y - 1 <= maxY.ToInt())
            {
                yield return new(x, y - 1);
            }
        }

        private IEnumerable<Vector<TSize>> AroundLong(TSize minX, TSize minY, TSize maxX, TSize maxY)
        {
            long x = Convert.ToInt64(this.X);
            long y = Convert.ToInt64(this.Y);

            if (x + 1 >= minX.ToLong() && x + 1 <= maxX.ToLong())
            {
                yield return new(x + 1, y);
            }

            if (x - 1 >= minX.ToLong() && x - 1 <= maxX.ToLong())
            {
                yield return new(x - 1, y);
            }

            if (y + 1 >= minY.ToLong() && y + 1 <= maxY.ToLong())
            {
                yield return new(x, y + 1);
            }

            if (y - 1 >= minY.ToLong() && y - 1 <= maxY.ToLong())
            {
                yield return new(x, y - 1);
            }
        }

        private IEnumerable<Vector<TSize>> AroundDouble(TSize minX, TSize minY, TSize maxX, TSize maxY)
        {
            double x = Convert.ToDouble(this.X);
            double y = Convert.ToDouble(this.Y);

            if (x + 1 >= minX.ToDouble() && x + 1 <= maxX.ToDouble())
            {
                yield return new(x + 1, y);
            }

            if (x - 1 >= minX.ToDouble() && x - 1 <= maxX.ToDouble())
            {
                yield return new(x - 1, y);
            }

            if (y + 1 >= minY.ToDouble() && y + 1 <= maxY.ToDouble())
            {
                yield return new(x, y + 1);
            }

            if (y - 1 >= minY.ToDouble() && y - 1 <= maxY.ToDouble())
            {
                yield return new(x, y - 1);
            }
        }
    }
}
