using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace James.Testing.Wcf
{
    public class Service<TServiceInterface> where TServiceInterface : class
    {
        private static readonly ThreadLocal<IServiceClientProxy<TServiceInterface>> _service = new ThreadLocal<IServiceClientProxy<TServiceInterface>>();
        private static readonly ThreadLocal<IDictionary<Type, object>> _responses = new ThreadLocal<IDictionary<Type, object>>();

        /// <summary>
        /// Implements a safe way to execute an action (no return value) on the given service interface.
        /// </summary>
        /// <remarks>
        /// For use with Call() that does not have a return result.
        /// </remarks>
        /// <param name="action">The action.</param>
        public static IResponse<Result, TFault> Call<TFault>(Action<TServiceInterface> action)
            where TFault : FaultException
        {
            IResponse<Result, TFault> response = null;

            try
            {
                Proxy.CallService(action);
                response = new Response<Result, TFault>(Result.Empty);
            }
            catch (TFault ex)
            {
                response = new Response<Result, TFault>(ex);
            }
            finally
            {
                AddOrUpdate(response);
            }

            return response;
        }


        /// <summary>
        /// Implements a safe way to execute an action (no return value) on the given service interface.
        /// </summary>
        /// <remarks>
        /// For use with Call() that does not have a return result and a custom FaultException of T.
        /// </remarks>
        /// <param name="action">The action.</param>
        public static IResponse<Result, FaultException> Call(Action<TServiceInterface> action)
        {
            return Call<FaultException>(action);
        }

        /// <summary>
        /// Implements a safe way to execute a function (has return value) on the given service interface.
        /// </summary>
        /// <remarks>
        /// For use with Call() that has a return type and a custom FaultException of T.
        /// </remarks>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <typeparam name="TFault">Type of fault</typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static IResponse<TResult, TFault> Call<TResult, TFault>(Func<TServiceInterface, TResult> function)
            where TFault : FaultException
        {
            IResponse<TResult, TFault> response = null;

            try
            {
                var result = Proxy.CallService(function);
                response = new Response<TResult, TFault>(result);
            }
            catch (TFault ex)
            {
                response = new Response<TResult, TFault>(ex);
            }
            finally
            {
                AddOrUpdate(response);
            }

            return response;
        }

        /// <summary>
        /// Implements a safe way to execute a function (has return value) on the given service interface.
        /// </summary>
        /// <remarks>
        /// For use with Call() that has a return type and the default FaultException.
        /// </remarks>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static IResponse<TResult, FaultException> Call<TResult>(Func<TServiceInterface, TResult> function)
        {
            return Call<TResult, FaultException>(function);
        }

        /// <summary>
        /// Returns the response for the last Call() that was made.  It will return a null value, if no previous Call().
        /// </summary>
        /// <remarks>
        /// For use with a function based Call() with a custom FaultException of T specified.
        /// </remarks>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <typeparam name="TFault">Type of fault</typeparam>
        /// <returns></returns>
        public static IResponse<TResult, TFault> CurrentResponse<TResult, TFault>()
        {
            object value;
            if (Responses.TryGetValue(typeof (IResponse<TResult, TFault>), out value))
            {
                return value as IResponse<TResult, TFault>;
            }

            return null;
        }

        /// <summary>
        /// Returns the response for the last Call() that was made.  It will return a null value, if no previous Call().
        /// </summary>
        /// <remarks>
        /// For use with a function based Call() with a default FaultException.
        /// </remarks>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <returns></returns>
        public static IResponse<TResult, FaultException> CurrentResponse<TResult>()
        {
            return CurrentResponse<TResult, FaultException>();
        }

        /// <summary>
        /// Returns the response for the last Call() that was made.  It will return a null value, if no previous Call().
        /// </summary>
        /// <remarks>
        /// This overload is for use with an action based Call().
        /// </remarks>
        /// <returns></returns>
        public static IResponse<Result, FaultException> CurrentResponse()
        {
            return CurrentResponse<Result, FaultException>();
        }

        private static IServiceClientProxy<TServiceInterface> Proxy
        {
            get
            {
                if (!_service.IsValueCreated)
                    _service.Value = new ServiceClientProxy<TServiceInterface>();

                return _service.Value;
            }
        }

        private static IDictionary<Type, object> Responses
        {
            get
            {
                if (!_responses.IsValueCreated)
                    _responses.Value = new Dictionary<Type, object>();

                return _responses.Value;
            }
        }

        private static void AddOrUpdate<TResult, TFault>(IResponse<TResult, TFault> response)
        {
            if (Responses.ContainsKey(typeof(IResponse<TResult, TFault>)))
                Responses[typeof(IResponse<TResult, TFault>)] = response;
            else
                Responses.Add(typeof(IResponse<TResult, TFault>), response);
        }
    }
}