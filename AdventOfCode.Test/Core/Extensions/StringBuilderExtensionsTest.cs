namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Text;

    public class StringBuilderExtensionsTestBase : TheSubject<StringBuilder>
    {
        [SetUp]
        public void Init()
        {
            Subject = new StringBuilder();
        }
    }

    [TestFixture]
    public class StringBuilderExtensionsTest : StringBuilderExtensionsTestBase
    {
        [Test]
        public void When_told_to_replace_with_an_empty_string()
        {
            Subject.Append("Test removing a string for an empty string");
            Subject.Replace("string");
            Subject.ToString().Should().Be("Test removing a  for an empty ");
        }
    }
}
