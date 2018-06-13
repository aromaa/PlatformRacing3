using Platform_Racing_3_UnsafeHelpers.IL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Platform_Racing_3_UnsafeHelpers.Extensions
{
    public static class InterlockedExtansions
    {
        public static unsafe uint CompareExchange(ref uint location, uint value, uint comparand)
        {
            fixed (uint* pointer = &location)
            {
                return (uint)Interlocked.CompareExchange(ref *(int*)pointer, (int)value, (int)comparand);
            }
        }

        public static unsafe uint Increment(ref uint location)
        {
            fixed (uint* pointer = &location)
            {
                return (uint)Interlocked.Increment(ref *(int*)pointer);
            }
        }

        public static unsafe uint Decrement(ref uint location)
        {
            fixed (uint* pointer = &location)
            {
                return (uint)Interlocked.Decrement(ref *(int*)pointer);
            }
        }

        public static unsafe T CompareExchange<T>(ref T location, T value, T comparand) 
        {
            return InterlockedEnum<T>.CompareExchange(ref location, value, comparand);
        }
    }
}
