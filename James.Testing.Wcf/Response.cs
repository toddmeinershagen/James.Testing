namespace James.Testing.Wcf
{
    public interface IResponse<out TResult, out TFault>
    {
        TResult Result { get; }
        TFault Fault { get; }
    }

    public class Response<TResult, TFault> : IResponse<TResult, TFault>
    {
        public Response(TResult result)
        {
            Result = result;
        }

        public Response(TFault fault)
        {
            Fault = fault;
        }

        public TResult Result { get; private set; }
        public TFault Fault { get; private set; }
    }
}
