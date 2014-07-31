using System;

namespace James.Testing.Wcf
{
    /// <summary>
    /// Proxy of a service client
    /// </summary>
    /// <typeparam name="TServiceInterface"></typeparam>
    public interface IServiceClientProxy<out TServiceInterface>
    {
        /// <summary>
        /// Invoke one or more methods on a service that returns no results.
        /// </summary>
        /// <param name="action">service => { service.DoSomething(parameters); }</param>
        void CallService(Action<TServiceInterface> action);

        /// <summary>
        /// Invoke a method on a service that returns a result.
        /// </summary>
        /// <typeparam name="TResult">Type is inferred, do not specify!</typeparam>
        /// <param name="function">service => service.GetSomething(parameters)</param>
        /// <returns>The result of the service call.</returns>
        TResult CallService<TResult>(Func<TServiceInterface, TResult> function);
    }
}