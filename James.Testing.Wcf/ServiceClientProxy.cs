using System;
using System.ServiceModel;

namespace James.Testing.Wcf
{
    /// <summary>
    /// Implementation of IServiceClientProxy
    /// </summary>
    /// <typeparam name="TServiceInterface"></typeparam>
    public class ServiceClientProxy<TServiceInterface> :
        IServiceClientProxy<TServiceInterface>,
        IServiceClientExposable
        where TServiceInterface : class
    {
        protected class Client : ClientBase<TServiceInterface>
        {
            public TServiceInterface Service
            {
                get { return Channel; }
            }
        }

        #region IServiceClientProxy<TServiceInterface> Members

        /// <summary>
        /// Implements a safe way to execute an action (no return value) on the given service interface.
        /// </summary>
        /// <param name="action">The action.</param>
        public void CallService(Action<TServiceInterface> action)
        {
            var client = CreateClient();

            try
            {
                action(client.Service);
                client.Close();
            }
            catch (Exception ex)
            {
                client.Abort();
                throw;
            }
        }

        /// <summary>
        /// Implements a safe way to execute a function (has return value) on the given service interface.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public TResult CallService<TResult>(Func<TServiceInterface, TResult> function)
        {
            var client = CreateClient();

            try
            {
                var result = function(client.Service);
                client.Close();
                return result;
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }

        #endregion

        #region IServiceClientExposable<TServiceInterface> Members

        /// <summary>
        /// Gets an instance of the internal service client.
        /// </summary>
        /// <returns></returns>
        public ICommunicationObject GetClient()
        {
            return CreateClient();
        }

        #endregion

        #region Protected Methods

        protected virtual Client CreateClient()
        {
            return new Client();
        }

        #endregion
    }
}
