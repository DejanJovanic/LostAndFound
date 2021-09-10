using Server.Interfaces;
using System;
using System.ServiceModel;

namespace Server.Model
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    class LoginService : ILoginService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Login()
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            try
            {
                if (CurrentConnections.Instance.AddSession(username, OperationContext.Current.GetCallbackChannel<ILoginServiceCallback>()))
                {
                    var ret = CurrentConnections.Instance.NotifySuccessfulLogin(Database.Database.Instance.GetPerson(username));
                    if (ret)
                        log.Info($"User with username: {username} has successfully logged in");
                    else
                        log.Info($"Failed to notify user with username: {username} about login");
                }
                else
                {
                    CurrentConnections.Instance.NotifyFailedLogin(username, "Given user is already logged in");
                    log.Info($"User with live session {username} attempted to login ");

                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }

        }

        public void Logout()
        {
            var username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            var ret = CurrentConnections.Instance.RemoveSession(username);
            if (ret)
                log.Info($"User with username: {username} has successfully logged out");
            else
                log.Info($"Failed to logout user with username: {username}");
        }
    }
}
