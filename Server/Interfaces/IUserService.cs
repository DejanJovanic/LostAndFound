using Server.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Interfaces
{
    [ServiceContract(Name = "IUserService")]
    [ServiceKnownType(typeof(Person))]
    public interface IUserService
    {
        [OperationContract]
        ResponseCode AddPerson(Person person, string password);

        [OperationContract]
        ResponseCode UpdatePerson(string name, string lastName);

        [OperationContract]
        ResponseCode RemovePerson(string username);
        [OperationContract]
        ICollection<Person> GetAllPersons();
    }
}
