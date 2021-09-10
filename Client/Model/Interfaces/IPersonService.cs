using System.Collections.Generic;
using System.Threading.Tasks;


namespace Client.Model.Interfaces
{
    interface IPersonService
    {
        Task<PersonReturnCode> AddPersonAsync(IPerson person, string password);
        Task<PersonReturnCode> UpdatePersonAsync(string name, string lastName);
        Task<PersonReturnCode> RemovePersonAsync(string personUsername);
        ICollection<DisplayPerson> GetPersonsList();
    }
}