using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.Model
{
    class CurrentConnectionOperations : ICurrentConnectionOperations
    {
        private object lockObj;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CurrentConnectionOperations()
        {
            lockObj = new object();
        }
        public bool AddSession(string name, ILoginServiceCallback callback, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            if (currentClients.ContainsKey(name))
                return false;

            lock (lockObj)
            {
                currentClients.Add(name, callback);
            }
            return true;
        }

        public bool NotifySuccessfulLogin(Person userDetails, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            try
            {
                currentClients[userDetails.Username].NotifySuccessfulLogin(userDetails);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool NotifyFailedLogin(string username, string reason, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            try
            {
                currentClients[username].NotifyFailedLogin(reason);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void UpdateItem(IUpdateValue item, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            log.Info("Updating users with item-changing data");
            new TaskFactory().StartNew(new Action<object>(val =>
            {
                IDictionary<string, ILoginServiceCallback> forRemoval = new Dictionary<string, ILoginServiceCallback>();
                lock (lockObj)
                {
                    foreach (var client in currentClients)
                    {
                        var state = ((ICommunicationObject)client.Value).State;
                        if (state != CommunicationState.Opened)
                            forRemoval.Add(new KeyValuePair<string, ILoginServiceCallback>(client.Key, client.Value));
                        else
                        {
                            ILoginServiceCallback temp = client.Value;
                            new TaskFactory().StartNew(new Action<object>(
                                a => temp.UpdateItemData((ItemDelta)a)), val);
                        }
                    }
                    foreach (var client in forRemoval)
                        currentClients.Remove(client);
                }
            }), item);
        }

        public void UpdateUser(UserDelta item, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            log.Info("Updating users with user-changing data");
            new TaskFactory().StartNew(new Action<object>(val =>
            {
                IDictionary<string, ILoginServiceCallback> forRemoval = new Dictionary<string, ILoginServiceCallback>();
                lock (lockObj)
                {
                    foreach (var client in currentClients)
                    {
                        var state = ((ICommunicationObject)client.Value).State;
                        if (state != CommunicationState.Opened)
                            forRemoval.Add(new KeyValuePair<string, ILoginServiceCallback>(client.Key, client.Value));
                        else
                        {
                            ILoginServiceCallback temp = client.Value;
                            new TaskFactory().StartNew(new Action<object>(
                                a => temp.UpdateUserData((UserDelta)a)), val);
                        }
                    }
                    foreach (var client in forRemoval)
                        currentClients.Remove(client);
                }
            }), item);
        }

        public bool RemoveSession(string name, IDictionary<string, ILoginServiceCallback> currentClients)
        {
            if (currentClients.ContainsKey(name))
            {
                try
                {
                    currentClients[name].NotifySuccessfulLogout();
                }
                catch
                {
                    log.Warn($"User : {name} logged out without properly closing connection");
                }
                currentClients.Remove(name);
                return true;
            }
            else return false;

        }
    }
}
