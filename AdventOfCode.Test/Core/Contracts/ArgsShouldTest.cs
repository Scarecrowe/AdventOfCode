namespace AdventOfCode.Test.Core.Contracts
{   
    using System;
    using System.Collections.Generic;
    using AdventOfCode.Core.Contracts;
    using AdventOfCode.Test;
    using FluentAssertions;
    using NUnit.Framework;

    public class ArgsShouldTestBase : TheSubject<ArgsShould>
    {
    }

    [TestFixture]
    public class ArgsShouldTest : ArgsShouldTestBase
    {
        public class Constructor : ArgsShouldTestBase
        {
            [Test]
            public void When_created()
            {
                var valueToTest = new object();

                Subject = new ArgsShould(valueToTest);

                Assert.IsNotNull(Subject);
                Assert.AreSame(Subject.Subject, valueToTest);
                Assert.IsFalse(Subject.IsNot);
            }
        }

        public class Not : ArgsShouldTestBase
        {
            [Test]
            public void When_told_to_not()
            {
                Subject = new ArgsShould(new object());
                Subject = Subject.Not();

                Assert.IsTrue(Subject.IsNot);
            }

            [Test]
            public void When_told_to_not_it_should_reset_after_test()
            {
                Subject = new ArgsShould(new object()).Not();

                Subject.BeNull();

                Assert.IsFalse(Subject.IsNot);
            }
        }

        public class BeNull : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_null_with_a_null_object()
            {
                Subject = new ArgsShould(null);

                Action action = () => Subject.Not().BeNull(message: "Value cannot be null");

                action.Should()
                    .Throw<ArgumentNullException>();
            }

            [Test]
            public void When_testing_be_null_with_a_valid_object()
            {
                Subject = new ArgsShould(true);

                ArgsShould result = null;

                Action action = () => result = Subject.Not().BeNull();

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }
        }

        public class BeNullOrEmpty : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_null_or_empty_with_a_null_object()
            {
                string valueToTest = null;
                Subject = new ArgsShould(valueToTest);
                var message = "Value cannot be null";
                var paramName = nameof(valueToTest);

                Action action = () => Subject.Not().BeNullOrEmpty(message, paramName);

                action.Should()
                    .Throw<ArgumentNullException>();
            }

            [Test]
            public void When_testing_be_null_or_empty_with_an_empty_string()
            {
                string valueToTest = string.Empty;
                Subject = new ArgsShould(valueToTest);
                var message = "Value cannot be empty";
                var paramName = nameof(valueToTest);

                Action action = () => Subject.Not().BeNullOrEmpty(message, paramName);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_be_null_or_empty_with_a_valid_string()
            {
                ArgsShould result = null;
                Subject = new ArgsShould("i am not empty or null");

                Action action = () => result = Subject.Not().BeNullOrEmpty();

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }
        }

        public class BeDefault : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_default_with_a_default_value()
            {
                DateTime valueToTest = default;
                Subject = new ArgsShould(valueToTest);
                var message = "DateTime cannot be default";
                var paramName = nameof(valueToTest);

                Action action = () => Subject.Not().BeDefault<DateTime>(message, paramName);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_be_default_with_a_valid_value()
            {
                ArgsShould result = null;
                Subject = new ArgsShould(DateTime.Now);

                Action action = () => result = Subject.Not().BeDefault<DateTime>();

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }
        }

        public class BeOfType : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_to_be_of_type_with_a_different_type()
            {
                bool valueToTest = true;
                Subject = new ArgsShould(valueToTest);
                var message = "Type mismatch";
                var paramName = nameof(valueToTest);

                Action action = () => Subject.BeOfType<int>(message, paramName);

                action.Should()
                    .Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_to_be_of_type_with_the_same_type()
            {
                bool valueToTest = true;
                Subject = new ArgsShould(valueToTest);
                ArgsShould result = null;

                Action action = () => result = Subject.BeOfType<bool>();

                action.Should().NotThrow<ArgumentException>();

                Assert.AreSame(Subject, result);
            }

            [Test]
            public void When_testing_to_be_of_type_with_multiple_types_that_are_different()
            {
                bool valueToTest = true;
                Subject = new ArgsShould(valueToTest);
                var message = "Type mismatch";
                var paramName = nameof(valueToTest);
                var types = new List<Type> { typeof(int), typeof(double) };

                Action action = () => Subject.BeOfType(types, message, paramName);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_to_be_of_type_with_multiple_types_with_a_matching_type()
            {
                bool valueToTest = true;
                Subject = new ArgsShould(valueToTest);
                ArgsShould result = null;

                Action action = () => result = Subject.BeOfType(new List<Type> { typeof(int), typeof(bool) });

                action.Should().NotThrow<ArgumentException>();

                Assert.AreSame(Subject, result);
                Assert.IsFalse(Subject.IsNot);
            }
        }

        public class Be : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_with_a_null_non_matching_object()
            {
                Subject = new ArgsShould(null);

                Action action = () => Subject.Be(1);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_be_with_a_matching_object()
            {
                Subject = new ArgsShould(1);

                ArgsShould result = null;

                Action action = () => result = Subject.Be(1);

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }

            [Test]
            public void When_testing_be_true_with_a_non_matching_object()
            {
                Subject = new ArgsShould(1);

                Action action = () => Subject.Be(2);

                action.Should().Throw<ArgumentException>();
            }
        }

        public class BeTrue : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_true_with_a_null_object()
            {
                Subject = new ArgsShould(null);

                Action action = () => Subject.BeTrue();

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_testing_be_true_with_a_matching_boolean()
            {
                Subject = new ArgsShould(true);

                ArgsShould result = null;

                Action action = () => result = Subject.BeTrue();

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }

            [Test]
            public void When_testing_be_true_with_a_non_matching_boolean()
            {
                Subject = new ArgsShould(false);

                Action action = () => Subject.BeTrue();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_be_true_with_a_non_boolean()
            {
                Subject = new ArgsShould(new DateTime(2020, 5, 3));

                Action action = () => Subject.BeTrue();

                action.Should().Throw<ArgumentException>();
            }
        }

        public class BeFalse : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_be_false_with_a_null_object()
            {
                Subject = new ArgsShould(null);

                Action action = () => Subject.BeFalse();

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_testing_be_false_with_a_matching_boolean()
            {
                Subject = new ArgsShould(false);

                ArgsShould result = null;

                Action action = () => result = Subject.BeFalse();

                action.Should().NotThrow();

                Assert.AreSame(Subject, result);
            }

            [Test]
            public void When_testing_be_false_with_a_non_matching_boolean()
            {
                Subject = new ArgsShould(true);

                Action action = () => Subject.BeFalse();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_testing_be_false_with_a_non_boolean()
            {
                Subject = new ArgsShould(new DateTime(2020, 5, 3));

                Action action = () => Subject.BeFalse();

                action.Should().Throw<ArgumentException>();
            }
        }

        public class Chaining: ArgsShouldTestBase
        {
            [Test]
            public void When_chaining_multiple_assertions_that_should_pass()
            {
                string valueToTest = "i am not empty or null";
                Subject = new ArgsShould(valueToTest);
                ArgsShould result = null;

                Action action = () => result = Subject
                                        .Not().BeNullOrEmpty()
                                        .BeOfType(new List<Type> { typeof(string), typeof(bool) });

                action.Should().NotThrow<ArgumentException>();

                Assert.AreSame(Subject, result);
            }

            [Test]
            public void When_chaining_multiple_assertions_that_should_fail()
            {
                string valueToTest = "i am not empty or null";
                Subject = new ArgsShould(valueToTest);
                ArgsShould result = null;
                var message = "Type mismatch";
                var paramName = nameof(valueToTest);
                var types = new List<Type> { typeof(int), typeof(bool) };

                Action action = () => result = Subject
                                        .Not().BeNullOrEmpty()
                                        .BeOfType(types, message, paramName);

                action.Should()
                    .Throw<ArgumentException>();
            }
        }

        public class ContainSpecialCharacters : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_with_a_match()
            {
                Subject = new ArgsShould("ABCD1*23");

                Action action = () => Subject.ContainSpecialCharacters();
                action.Should().NotThrow<ArgumentException>();
            }

            [Test]
            public void When_testing_with_a_non_match()
            {
                Subject = new ArgsShould("ABCD123");

                Action action = () => Subject.ContainSpecialCharacters();
                action.Should().Throw<ArgumentException>();
            }
        }

        public class BeGreaterThanZero : ArgsShouldTestBase
        {
            [Test]
            public void When_testing_with_a_match()
            {
                Subject = new ArgsShould(100);

                Action action = () => Subject.BeGreaterThanZero();
                action.Should().NotThrow<ArgumentException>();
            }

            [Test]
            public void When_testing_with_a_non_match()
            {
                Subject = new ArgsShould(-100);

                Action action = () => Subject.BeGreaterThanZero();
                action.Should().Throw<ArgumentException>();
            }
        }
    }
}
