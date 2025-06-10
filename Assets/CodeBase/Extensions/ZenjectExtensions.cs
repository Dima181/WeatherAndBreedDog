using System;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.Extensions
{
    public static class ZenjectExtensions
    {
        public static Type[] GetArgumentsOfInheritedOpenGenericClass(this Type type, Type openGenericType)
        {
            var currentType = type;
            while (currentType.BaseType != null)
            {
                currentType = currentType.BaseType;
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == openGenericType)
                    return currentType.GetGenericArguments();
            }

            return new Type[0];
        }

        public static string SplitPascalCase(this string str)
        {
            StringBuilder builder = new(str.Length);

            builder.Append(str[0]);

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]) && !char.IsUpper(str[i - 1]))
                    builder.Append(" ");

                builder.Append(str[i]);
            }

            return builder.ToString();
        }
    }
}
