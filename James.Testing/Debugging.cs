using System;
using System.Diagnostics;

namespace James.Testing
{
    public static class Debugging
    {
        public static T DebugPrint<T>(this T value, Func<T, object> getObject)
        {
            Console.WriteLine(getObject(value));
            return value;
        }

        public static T DebugPrint<T>(this T value)
        {
            Console.WriteLine(value.ToString());
            return value;
        }

        public static T DebugPrint<T>(this T value, string message)
        {
            Console.WriteLine(message);
            return value;
        }

        public static T DebugBreak<T>(this T value)
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            return value;
        }
    }
}
