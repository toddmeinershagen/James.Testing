using System;
using System.ServiceModel;

namespace James.Testing.Wcf.IntegrationTests
{
    public class PersonService : IPersonService
    {
        public GetPersonResult GetPerson(GetPersonRequest request)
        {
            if (request != null && request.Id == Guid.Empty)
            {
                throw new FaultException<GeneralFault>(new GeneralFault{Message = "This is the message.", StatusCode = 404, StatusDescription = "PersonNotFound"});
            }

            return new GetPersonResult {Person = new Person {Id = request.Id, FirstName = "Todd", LastName = "Meinershagen"}};
        }

        public void MarkPersonAsFavorite(Guid id)
        {
            if (id == Guid.Empty)
                throw new FaultException<GeneralFault>(new GeneralFault{Message = "This is the message.", StatusCode = 400, StatusDescription = "BadRequest"});
        }
    }

    [ServiceContract]
    public interface IPersonService
    {
        [OperationContract]
        [FaultContract(typeof(GeneralFault))]
        GetPersonResult GetPerson(GetPersonRequest request);

        [OperationContract]
        [FaultContract(typeof(GeneralFault))]
        void MarkPersonAsFavorite(Guid id);
    }

    public class GetPersonRequest
    {
        public Guid Id { get; set; }
    }

    public class GetPersonResult
    {
        public Person Person { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GeneralFault
    {
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Message { get; set; }
    }
}
