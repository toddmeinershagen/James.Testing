namespace James.Testing.Messaging
{
    /// <summary>
    /// The IBusEnvironment encapsulates all of the details needed to set up a particular IBus instance.
    /// </summary>
    public interface IBusEnvironment
    {
        /// <summary>
        /// Creates the bus.
        /// </summary>
        /// <returns>A bus instance.</returns>
        IBus CreateBus();
    }
}