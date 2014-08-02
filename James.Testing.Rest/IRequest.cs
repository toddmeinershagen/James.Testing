namespace James.Testing.Rest
{
    public interface IRequest<out TResponse, out TError>
    {
        IResponse<TResponse, TError> Execute();
    }
}