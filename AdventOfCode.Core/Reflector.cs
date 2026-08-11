namespace AdventOfCode.Core
{
    using System.Configuration;
    using System.Reflection;

    public class Reflector
    {
        public static void SetConfigurationProperty<TConfig, TValue>(
            PropertyInfo info,
            TConfig config,
            TValue value)
        {
            string propertyName = info.Name;
            string setterMethodName = $"Set{propertyName}";

            MethodInfo? method = config?.GetType().GetMethod(
                setterMethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method == null)
            {
                throw new InvalidOperationException($"No method found called {setterMethodName}");
            }

            if (value == null)
            {
                throw new InvalidOperationException($"Value cannot be null");
            }

            method.Invoke(config, new object[] { value });
        }

        public static bool IsGenericList(Type type)
        {
            if (type.IsGenericType)
            {
                var genericDefinition = type.GetGenericTypeDefinition();

                if (genericDefinition == typeof(List<>) ||
                    genericDefinition == typeof(IList<>))
                {
                    return true;
                }
            }

            return type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IList<>));
        }

        public static bool IsTuple(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();

            return type.FullName?.StartsWith("System.ValueTuple`") == true
                || type.FullName?.StartsWith("System.Tuple`") == true;
        }
    }
}
