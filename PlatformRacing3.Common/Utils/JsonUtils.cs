using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Common.Utils;

public static class JsonUtils
{
	private static readonly ConcurrentDictionary<Type, Dictionary<JsonPropertyNameAttribute, Func<object, object>>> CachedProperties = new();

	private static Dictionary<JsonPropertyNameAttribute, Func<object, object>> GetProperties<T>()
	{
		Type type = typeof(T);
		if (!JsonUtils.CachedProperties.TryGetValue(type, out Dictionary<JsonPropertyNameAttribute, Func<object, object>> info))
		{
			info = new Dictionary<JsonPropertyNameAttribute, Func<object, object>>();
			foreach (PropertyInfo property in type.GetRuntimeProperties())
			{
				JsonPropertyNameAttribute jsonProperty = property.GetCustomAttribute<JsonPropertyNameAttribute>();
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
		Dictionary<string, object> userVars = new();

		JsonUtils.GetVars(target, vars, userVars);

		return userVars;
	}
	public static void GetVars<T>(T target, HashSet<string> vars, Dictionary<string, object> to)
	{
		bool all = vars.Contains("*");

		foreach (KeyValuePair<JsonPropertyNameAttribute, Func<object, object>> property in JsonUtils.GetProperties<T>())
		{
			JsonPropertyNameAttribute jsonAttribute = property.Key;
			Func<object, object> getter = property.Value;

			if (all || vars.Contains(jsonAttribute.Name))
			{
				to[jsonAttribute.Name] = getter.Invoke(target);
			}
		}
	}
}