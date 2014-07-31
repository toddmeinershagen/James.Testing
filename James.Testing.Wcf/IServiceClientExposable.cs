using System.ServiceModel;

namespace James.Testing.Wcf
{
    public interface IServiceClientExposable
    {
        ICommunicationObject GetClient();
    }
}