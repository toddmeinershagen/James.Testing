using System;

namespace James.Testing.Messaging
{
    public class Messaging
    {
        public static IBus With<TBusEnvironment>()
            where TBusEnvironment : IBusEnvironment, new()
        {
            var environment = new TBusEnvironment();
            return With(environment);
        }

        public static IBus With(IBusEnvironment environment)
        {
            return environment.CreateBus();
        }
    }
}
