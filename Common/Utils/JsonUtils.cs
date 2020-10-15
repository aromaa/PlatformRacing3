using Newtonsoft.Json;
using Platform_Racing_3_Common.Level;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using log4net.Util;
using Platform_Racing_3_Common.User;

namespace Platform_Racing_3_Common.Utils
{
    public static class JsonUtils
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<JsonPropertyAttribute, Func<object, object>>> CachedProperties = new ConcurrentDictionary<Type, Dictionary<JsonPropertyAttribute, Func<object, object>>>();

        private static Dictionary<JsonPropertyAttribute, Func<object, object>> GetProperties<T>()
        {
            Type type = typeof(T);
            if (!JsonUtils.CachedProperties.TryGetValue(type, out Dictionary<JsonPropertyAttribute, Func<object, object>> info))
            {
                info = new Dictionary<JsonPropertyAttribute, Func<object, object>>();
                foreach (PropertyInfo property in type.GetRuntimeProperties())
                {
                    JsonPropertyAttribute jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
                    if (jsonProperty != null)
                    {
                        ParameterExpression parameter = Expression.Parameter(type);
                        MemberExpression expression = Expression.Property(parameter, property); 
                        UnaryExpression convert = Expression.Convert(expression, typeof(object));

                        info[jsonProperty] = Unsafe.As<Func<object, object>>(Expression.Lambda<Func<T, object>>(convert, parameter).Compile());
                    }
                }

                JsonUtils.CachedProperties.TryAdd(type, info);
            }

            return info;
        }

        public static IReadOnlyDictionary<string, object> GetVars<T>(T target, params string[] vars) => JsonUtils.GetVars(target, vars.ToHashSet());
        public static IReadOnlyDictionary<string, object> GetVars<T>(T target, HashSet<string> vars)
        {
            Dictionary<string, object> userVars = new Dictionary<string, object>();

            JsonUtils.GetVars(target, vars, userVars);

            return userVars;
        }
        public static void GetVars<T>(T target, HashSet<string> vars, Dictionary<string, object> to)
        {
            bool all = vars.Contains("*");

            foreach (KeyValuePair<JsonPropertyAttribute, Func<object, object>> property in JsonUtils.GetProperties<T>())
            {
                JsonPropertyAttribute jsonAttribute = property.Key;
                Func<object, object> getter = property.Value;

                if (all || vars.Contains(jsonAttribute.PropertyName))
                {
                    object value = getter.Invoke(target);
                    if (value is Color color) //SPECIAL CASE OMG
                    {
                        value = color.ToArgb();
                    }
                    else if (value is LevelMode levelMode) //SPECIAL CASE OMG
                    {
                        string mode = levelMode.ToString();

                        value = Char.ToLowerInvariant(mode[0]) + mode[1..];
                    }

                    to[jsonAttribute.PropertyName] = value;
                }
            }
        }
    }
}
