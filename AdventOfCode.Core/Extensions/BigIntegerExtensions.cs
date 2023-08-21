namespace AdventOfCode.Core.Extensions
{
    using System.Numerics;

    public static class BigIntegerExtensions
    {
        public static BigInteger Mod(this BigInteger x, BigInteger m) => ((x % m) + m) % m;

        public static BigInteger Inv(this BigInteger num, BigInteger size) => num.ModPow(size - 2, size);

        public static BigInteger ToBigInteger(this int num) => new(num);

        public static BigInteger ModPow(this BigInteger bigInteger, BigInteger pow, BigInteger mod) => BigInteger.ModPow(bigInteger, pow, mod);
    }
}
