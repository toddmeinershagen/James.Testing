using System;

namespace James.Testing.Rest
{
    public static class Storage
    {
        public static T Store<T, TData>(this T value, out TData data, Func<T, TData> expression)
        {
            data = expression(value);
            return value;
        }
    }
}