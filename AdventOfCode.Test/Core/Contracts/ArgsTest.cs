namespace AdventOfCode.Test.Core.Contracts
{
    using AdventOfCode.Core.Contracts;
    using AdventOfCode.Test;
    using NUnit.Framework;

    [TestFixture]
    public class ArgsTest : TheSubject<string>
    {
        [Test]
        public void When_the_should_extension_method_is_called()
        {
            Subject = string.Empty;

            ArgsShould result = Subject.Should();

            Assert.IsInstanceOf<ArgsShould>(result);
        }
    }
}
