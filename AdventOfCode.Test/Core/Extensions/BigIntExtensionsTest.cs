namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Numerics;

    public class BigIntExtensionsTestBase : TheSubject<BigInteger>
    {
        [SetUp]
        public void Init()
        {
            Subject = new BigInteger(100);
        }
    }

    [TestFixture]
    public class BigIntExtensionsTest : BigIntExtensionsTestBase
    {
        [Test]
        public void When_told_to_mod_with_another_big_integer()
        {
            Subject.Mod(new BigInteger(100)).Should().Be(0);
            Subject.Mod(new BigInteger(3)).Should().Be(1);
        }

        [Test]
        public void When_told_to_inv_with_another_big_integer()
        {
            Subject.Inv(new BigInteger(100)).Should().Be(0);
            Subject.Inv(new BigInteger(3)).Should().Be(1);
        }

        [Test]
        public void When_told_to_cast_an_int_to_a_big_integer()
        {
            100000000.ToBigInteger().Should().Be(100000000);
        }

        [Test]
        public void When_told_to_mod_power()
        {
            BigIntegerExtensions.ModPow(new BigInteger(100), new BigInteger(50), new BigInteger(25)).Should().Be(0);
        }
    }
}
