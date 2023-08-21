namespace AdventOfCode.Core.Contracts
{
    using AdventOfCode.Core.Extensions;

    public class ArgsShould
    {
        public ArgsShould(object subject)
        {
            this.Subject = subject;
        }

        public object Subject { get; }

        public bool IsNot { get; private set; }

        public ArgsShould Not()
        {
            this.IsNot = true;

            return this;
        }

        public ArgsShould BeNull(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value must {(this.IsNot ? "not " : string.Empty)}be null";
            }

            this.Test<ArgumentNullException>(this.Subject == null, message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeNullOrEmpty(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value must {(this.IsNot ? "not " : string.Empty)}be null or empty";
            }

            this.Test<ArgumentNullException>(this.Subject == null, message, paramName);
            this.Test<ArgumentException>(string.IsNullOrEmpty(this.Subject?.ToString() ?? string.Empty), message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeDefault<T>(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value of type {typeof(T)} must {(this.IsNot ? "not " : string.Empty)}be default";
            }

            this.Test<ArgumentException>(Equals(this.Subject, default(T)), message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeOfType<T>(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value must {(this.IsNot ? "not " : string.Empty)}be of type {typeof(T)}";
            }

            this.Test<ArgumentException>(this.Subject.GetType() == typeof(T), message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeOfType(IEnumerable<Type> types, string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                string typeMessage = string.Join(" or ", types);

                message = $"Value must {(this.IsNot ? "not " : string.Empty)}be of type {typeMessage}";
            }

            foreach (Type type in types)
            {
                if (this.Subject.GetType() == type)
                {
                    this.ResetNot();

                    return this;
                }
            }

            throw new ArgumentException(message, paramName);
        }

        public ArgsShould Be(object value, string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)}be \"{value}\"";
            }

            this.Test<ArgumentException>(this.Subject?.Equals(value) == true, message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeTrue(string message = "", string paramName = "")
        {
            this.Subject.Should().Not().BeNull().BeOfType<bool>();

            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)}be true";
            }

            this.Test<ArgumentException>(Convert.ToBoolean(this.Subject), message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeFalse(string message = "", string paramName = "")
        {
            this.Subject.Should().Not().BeNull().BeOfType<bool>();

            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)}be false";
            }

            this.Test<ArgumentException>(!Convert.ToBoolean(this.Subject), message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould ContainSpecialCharacters(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)}contain special characters\"";
            }

            this.Test<ArgumentException>(this.Subject?.ToString()?.ContainsSpecialCharacters() ?? false, message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeGreaterThanZero(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)} be greater than zero";
            }

            decimal value = Convert.ToDecimal(this.Subject);

            this.Test<ArgumentException>(value > 0, message, paramName);

            this.ResetNot();

            return this;
        }

        public ArgsShould BeLessThanZero(string message = "", string paramName = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = $"Value \"{this.Subject ?? "null"}\" must {(this.IsNot ? "not " : string.Empty)} be less than zero";
            }

            decimal value = Convert.ToDecimal(this.Subject);

            this.Test<ArgumentException>(value < 0, message, paramName);

            this.ResetNot();

            return this;
        }

        private void ResetNot() => this.IsNot = false;

        private void Test<TException>(bool result, string message, string paramName)
            where TException : Exception
        {
            result = this.IsNot
                ? !result
                : result;

            if (!result)
            {
                TException? exception = Activator.CreateInstance(typeof(TException), new object[] { message, paramName }) as TException;

                throw exception ?? new Exception(message);
            }
        }
    }
}
