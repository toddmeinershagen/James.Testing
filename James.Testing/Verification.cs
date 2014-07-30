using System;
using System.Linq.Expressions;

namespace James.Testing
{
    public static class Verification
    {
        public static T Verify<T>(this T value, string verification, Predicate<T> predicate)
        {
            if (!predicate(value))
                throw new VerificationException(string.Format("Unable to verify {0}", verification));

            return value;
        }

        public static T Verify<T>(this T value, Predicate<T> predicate)
        {
            return value.Verify("custom verification.", predicate);
        }

        public static IVerifyable<T, TProperty> VerifyThat<T, TProperty>(this T obj, Expression<Func<T, TProperty>> propertyExpression)
        {
            var expression = propertyExpression.Body as MemberExpression;

            if (expression == null)
                return new Verifyable<T, TProperty>(obj, default(TProperty));

            var info = typeof(T).GetProperty(expression.Member.Name);
            if (info == null)
                return new Verifyable<T, TProperty>(obj, default(TProperty));

            var value = info.GetValue(obj, null);
            if (ReferenceEquals(value, default(TProperty)) || (value.Equals(default(TProperty))))
                return new Verifyable<T, TProperty>(obj, default(TProperty));

            return new Verifyable<T, TProperty>(obj, (TProperty) value);
        }
    }

    public interface IVerifyable<out T, out TProperty>
    {
        T ReturnValue { get; }
        TProperty Value { get; }
        T VerifyThat(Predicate<TProperty> predicate, string verification, params object[] args);
    }

    public class Verifyable<T, TProperty> : IVerifyable<T, TProperty>
    {
        public Verifyable(T returnValue, TProperty value)
        {
            ReturnValue = returnValue;
            Value = value;
        }

        public T ReturnValue { get; private set; }
        public TProperty Value { get; private set; }

        public T VerifyThat(Predicate<TProperty> predicate, string verification, params object[] args)
        {
            if (!predicate(Value))
            {
                var message = string.Format(verification, args);
                throw new VerificationException(string.Format("Unable to verify that {0}", message));
            }

            return ReturnValue;
        }
    }

    public static class VerifyableOfGuidExtensions
    {
        public static T IsEmpty<T>(this IVerifyable<T, Guid> verifyable)
        {
            return verifyable.VerifyThat(v => v == Guid.Empty, "guid is empty.", verifyable.Value);
        }

        public static T IsNotEmpty<T>(this IVerifyable<T, Guid> verifyable)
        {
            return verifyable.VerifyThat(v => v != Guid.Empty, "guid is not empty.", verifyable.Value);
        }
    }

    public static class VerifyableOfStringExtensions
    {
        public static T IsEmpty<T>(this IVerifyable<T, string> verifyable)
        {
            return verifyable.VerifyThat(v => v == string.Empty, "string is empty.", verifyable.Value);
        }

        public static T IsNotEmpty<T>(this IVerifyable<T, string> verifyable)
        {
            return verifyable.VerifyThat(v => v != string.Empty, "string is not empty.", verifyable.Value);
        }
    }
}
