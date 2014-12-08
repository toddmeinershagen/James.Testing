using System.Security.Cryptography;
using System.Threading;

namespace James.Testing.Messaging
{
    public class Messaging
    {
        private static readonly ThreadLocal<MemoizedBus> ThreadedBus = new ThreadLocal<MemoizedBus>();
 
        public static MemoizedBus With<TBusEnvironment>()
            where TBusEnvironment : IBusEnvironment, new()
        {
            var environment = new TBusEnvironment();
            return With(environment);
        }

        public static MemoizedBus With(IBusEnvironment environment)
        {
            ThreadedBus.Value = new MemoizedBus(environment.CreateBus());
            return Bus;
        }

        public static IResponse<TResponse> CurrentResponse<TResponse>()
            where TResponse : class
        {
            return Bus == null ? null : Bus.CurrentResponse<TResponse>();
        }

        public static MemoizedBus Bus
        {
            get
            {
                return ThreadedBus.IsValueCreated && ThreadedBus.Value != null
                    ? ThreadedBus.Value
                    : null;

            }
        }
    }
}
