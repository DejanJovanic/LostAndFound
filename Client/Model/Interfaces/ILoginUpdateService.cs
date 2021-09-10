using System;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface ILoginUpdateService
    {
        Task LoginAsync(string username, string password);
        Task LogoutAsync();
        event EventHandler<bool> LoginSuccessful;
        event EventHandler<string> LoginFailed;
        event EventHandler LogoutSuccessful;
        event EventHandler EmergencyLogout;

    }
}