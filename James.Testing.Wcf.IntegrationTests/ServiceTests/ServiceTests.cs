using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class ServiceTests : HostFixture<PersonService>
    {
        [Test]
        public void given_service_without_a_call_when_getting_current_response_should_return_null()
        {
            Action action = () =>
            {
                Service<IPersonService>
                    .CurrentResponse<GetPersonResult, FaultException<GeneralFault>>()
                    .Verify(r => r == null);
            };

            var operation = new ThreadStart(action);
            var thread = new Thread(operation);
            thread.Start();
            thread.Join();
        }

        [Test]
        public void given_service_calls_in_multiple_threads_when_calling_should_return_thread_appropriate_responses()
        {
            Action action1 = () =>
            {
                Service<IPersonService>
                    .Call(x => x.GetPerson(new GetPersonRequest {Id = Guid.NewGuid()}))
                    .Verify(r => r.Result.Person.FirstName == "Todd");
            };

            Action action2 = () => { 
                Service<IPersonService>
                    .Call(x => x.MarkPersonAsFavorite(Guid.NewGuid()))
                    .Verify(r => r.Result == Result.Empty)
                    .Verify(r => r.Fault == null);
            };

            Parallel.Invoke(action1, action2);
        }

        [Test]
        public void
            given_service_calls_in_multiple_threads_getting_current_response_should_return_thread_appropriate_responses()
        {
            Action action1 = () =>
            {
                Service<IPersonService>
                    .Call(x => x.GetPerson(new GetPersonRequest {Id = Guid.NewGuid()}));

                Service<IPersonService>
                    .CurrentResponse<GetPersonResult>()
                    .Verify(r => r.Result.Person.FirstName == "Todd");
            };

            Action action2 = () =>
            {
                Service<IPersonService>
                    .Call(x => x.MarkPersonAsFavorite(Guid.NewGuid()));

                Service<IPersonService>
                    .CurrentResponse<Result>()
                    .Verify(r => r.Result == Result.Empty)
                    .Verify(r => r.Fault == null);
            };

            Parallel.Invoke(action1, action2);
        }
    }
}
