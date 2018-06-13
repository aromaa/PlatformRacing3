using Newtonsoft.Json;
using Platform_Racing_3_Common.Level;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Common.Utils
{
    public class JsonUtils
    {
        public static IReadOnlyDictionary<string, object> GetVars(object target, params string[] vars) => JsonUtils.GetVars(target, vars.ToHashSet());
        public static IReadOnlyDictionary<string, object> GetVars(object target, HashSet<string> vars)
        {
            Dictionary<string, object> userVars = new Dictionary<string, object>();
            foreach (PropertyInfo propertyInfo in target.GetType().GetRuntimeProperties())
            {
                JsonPropertyAttribute jsonProperty = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonProperty != null && (vars.Contains("*") || vars.Contains(jsonProperty.PropertyName)))
                {
                    object value = propertyInfo.GetValue(target);
                    if (value is Color color) //SPECIAL CASE OMG
                    {
                        value = color.ToArgb();
                    }
                    else if (value is LevelMode levelMode) //SPECIAL CASE OMG
                    {
                        string mode = levelMode.ToString();

                        value = Char.ToLowerInvariant(mode[0]) + mode.Substring(1);
                    }

                    userVars.Add(jsonProperty.PropertyName, value);
                }
            }

            return userVars;
        }
        
        public static IReadOnlyDictionary<string, object> GetVars(List<object> targets, params string[] vars) => JsonUtils.GetVars(targets.ToArray(), vars.ToHashSet());
        public static IReadOnlyDictionary<string, object> GetVars(object[] targets, params string[] vars) => JsonUtils.GetVars(targets, vars.ToHashSet());
        public static IReadOnlyDictionary<string, object> GetVars(object[] targets, HashSet<string> vars)
        {
            Dictionary<string, object> userVars = new Dictionary<string, object>();
            foreach(object target in targets)
            {
                foreach(KeyValuePair<string, object> vars_ in JsonUtils.GetVars(target, vars))
                {
                    userVars[vars_.Key] = vars_.Value;
                }
            }

            return userVars;
        }
    }
}
