using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace Platform_Racing_3_UnsafeHelpers.IL
{
    internal static class InterlockedEnum<T>
    {
        internal delegate T CompareExchangeDelegate(ref T location, T value, T comparand);
        internal static readonly CompareExchangeDelegate CompareExchange = InterlockedEnum<T>.CreateCompareExchange();

        private static CompareExchangeDelegate CreateCompareExchange()
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            DynamicMethod dynamicMethod = new(string.Empty, typeof(T), new[]
            {
                typeof(T).MakeByRefType(),
                typeof(T),
                typeof(T)
            });

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Call, typeof(Interlocked).GetMethod("CompareExchange", BindingFlags.Static | BindingFlags.Public, null, new[]
            {
                underlyingType.MakeByRefType(),
                underlyingType,
                underlyingType
            }, null));

            ilGenerator.Emit(OpCodes.Ret);

            return (CompareExchangeDelegate)dynamicMethod.CreateDelegate(typeof(CompareExchangeDelegate));
        }
    }
}
