using System.Security.Cryptography;
using System.Threading;

namespace James.Testing.Messaging
{
    public class Messaging
    {
        private static readonly ThreadLocal<MemoizedBus> Bus = new ThreadLocal<MemoizedBus>();
 
        public static MemoizedBus With<TBusEnvironment>()
            where TBusEnvironment : IBusEnvironment, new()
        {
            var environment = new TBusEnvironment();
            return With(environment);
        }

        public static MemoizedBus With(IBusEnvironment environment)
        {
            Bus.Value = new MemoizedBus(environment.CreateBus());
            return Bus.Value;
        }

        public static IResponse<TResponse> CurrentResponse<TResponse>()
            where TResponse : class
        {
            return Bus.IsValueCreated ? Bus.Value.CurrentResponse<TResponse>() : null;
        }
    }
}
