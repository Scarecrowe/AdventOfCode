namespace AdventOfCode.Test.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class ObjectExtensions
    {
        public static object InvokePrivateMethod<TSource>(this TSource target, string name, params object[] parameters)
        {
            Type type = typeof(TSource);

            MethodInfo member = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            try
            {
                return member.Invoke(target, parameters);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ex.InnerException.Rethrow();
                }

                ex.Rethrow();
            }

            return null;
        }

        public static async Task<object> InvokePrivateMethodAsync<TSource>(this TSource target, string name, params object[] parameters)
        {
            Type type = typeof(TSource);

            MethodInfo member = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            try
            {
                object result = null;

                await Task.Run(() => {
                    result = member.Invoke(target, parameters);
                });

                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ex.InnerException.Rethrow();
                }

                ex.Rethrow();
            }

            return null;
        }

        public static TSource SetPrivateProperty<TSource, TProperty>(this TSource target, Expression<Func<TSource, TProperty>> propertyLambda, TProperty value)
        {
            Type type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            PropertyInfo propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
            {
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
            }

            propertyInfo.SetValue(target, value);

            return target;
        }

        public static void SetPrivateProperty(this object target, string propertyName, object value)
        {
            Type type = target.GetType();

            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (propertyInfo == null)
            {
                propertyInfo = target.GetType().BaseType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Expression '{propertyName}' refers to a field, not a property.");
                }
            }

            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
            {
                throw new ArgumentException(
                    $"Expression '{propertyName}' refers to a property that is not from type {type}.");
            }

            propertyInfo.SetValue(target, value);
        }

        public static T GetPrivateProperty<T>(this object target, string propertyName)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                propertyInfo = target.GetType().BaseType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);

                if(propertyInfo == null)
                {
                    throw new ArgumentException($"Expression '{propertyName}' was not found.");
                }
            }

            return (T)propertyInfo.GetValue(target);
        }

        public static T GetPrivateField<T>(this object target, string fieldName)
        {
            FieldInfo fieldInfo = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null)
            {
                fieldInfo = target.GetType().BaseType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (fieldInfo == null)
                {
                    throw new ArgumentException($"Expression '{fieldName}' was not found.");
                }
            }

            return (T)fieldInfo.GetValue(target);
        }

        public static void SetPrivateField(this object target, string fieldName, object value)
        {
            Type type = target.GetType();

            FieldInfo fieldInfo = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (fieldInfo == null)
            {
                fieldInfo = target.GetType().BaseType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (fieldInfo == null)
                {
                    throw new ArgumentException($"Expression '{fieldName}' refers to a property, not a field.");
                }
            }

            if (type != fieldInfo.ReflectedType && !type.IsSubclassOf(fieldInfo.ReflectedType))
            {
                throw new ArgumentException(
                    $"Expression '{fieldName}' refers to a field that is not from type {type}.");
            }

            fieldInfo.SetValue(target, value);
        }
    }
}
