using Server.Interfaces;
using Server.Model;
using System.Collections.Generic;

namespace Server.Interfaces
{
    interface ICurrentConnectionOperations
    {
        bool AddSession(string name, ILoginServiceCallback callback, IDictionary<string, ILoginServiceCallback> currentClients);
        void UpdateUser(UserDelta item, IDictionary<string, ILoginServiceCallback> currentClients);
        bool RemoveSession(string name, IDictionary<string, ILoginServiceCallback> currentClients);
        void UpdateItem(IUpdateValue item, IDictionary<string, ILoginServiceCallback> currentClients);
        bool NotifySuccessfulLogin(Person userDetails, IDictionary<string, ILoginServiceCallback> currentClients);
        bool NotifyFailedLogin(string username, string reason, IDictionary<string, ILoginServiceCallback> currentClients);
    }
}