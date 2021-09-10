using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;

namespace Server.Model
{
    public class CurrentConnections
    {
        private static readonly Lazy<CurrentConnections>
        lazy =
        new Lazy<CurrentConnections>
            (() => new CurrentConnections());

        private static Dictionary<string, ILoginServiceCallback> currentClients;
        private static ICurrentConnectionOperations operations;

        public static CurrentConnections Instance { get { return lazy.Value; } }


        private CurrentConnections()
        {
            currentClients = new Dictionary<string, ILoginServiceCallback>();
            operations = new CurrentConnectionOperations();
        }

        public bool AddSession(string name, ILoginServiceCallback callback)
        {
            return operations.AddSession(name, callback, currentClients);

        }

        public bool NotifySuccessfulLogin(Person userDetails)
        {
            return operations.NotifySuccessfulLogin(userDetails, currentClients);

        }

        public bool NotifyFailedLogin(string username, string reason)
        {
            return operations.NotifyFailedLogin(username, reason, currentClients);

        }
        public void UpdateItem(IUpdateValue val)
        {
            operations.UpdateItem(val, currentClients);
        }

        public bool IsConnectionRegistered(string username)
        {
            return currentClients.ContainsKey(username);
        }

        public void UpdateUser(UserDelta val)
        {
            operations.UpdateUser(val, currentClients);
        }

        public bool RemoveSession(string username)
        {
            return operations.RemoveSession(username, currentClients);
        }
    }
}
