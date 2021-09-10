using Client.LoginServiceReference;
using Client.Model.Interfaces;
using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Client.Model
{
    class LoginUpdateService : ILoginServiceCallback, IDisposable, ILoginUpdateService, IEmergencyLogout
    {
        private IDataChangeHandler data;
        IUserData userData;
        private LoginServiceClient proxy;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private event EventHandler<bool> LoginSuccessful;
        private event EventHandler<string> LoginFailed;
        private event EventHandler LogoutSuccessful;
        private event EventHandler EmergencyLogout;

        private object eventLock;
        public LoginUpdateService(IDataChangeHandler data, IUserData userData)
        {
            var context = new InstanceContext(this);
            proxy = new LoginServiceClient(context);
            this.data = data;
            this.userData = userData;
            eventLock = new object();
        }

        event EventHandler<bool> ILoginUpdateService.LoginSuccessful
        {
            add
            {
                lock (eventLock)
                {
                    LoginSuccessful += value;
                }
            }

            remove
            {
                lock (eventLock)
                {
                    LoginSuccessful -= value;
                }
            }
        }

        event EventHandler<string> ILoginUpdateService.LoginFailed
        {
            add
            {
                lock (eventLock)
                {
                    LoginFailed += value;
                }
            }

            remove
            {
                lock (eventLock)
                {
                    LoginFailed -= value;
                }
            }
        }

        event EventHandler ILoginUpdateService.LogoutSuccessful
        {
            add
            {
                lock (eventLock)
                {
                    LogoutSuccessful += value;
                }
            }

            remove
            {
                lock (eventLock)
                {
                    LogoutSuccessful -= value;
                }
            }
        }
        event EventHandler ILoginUpdateService.EmergencyLogout
        {
            add
            {
                lock (eventLock)
                {
                    EmergencyLogout += value;
                }
            }

            remove
            {
                lock (eventLock)
                {
                    EmergencyLogout -= value;
                }
            }
        }



        public void Dispose()
        {
            if (proxy.State == CommunicationState.Faulted)
                proxy.Abort();
            else
                proxy.Close();
        }

        public void NotifyFailedLogin(string reason)
        {
            LoginFailed?.Invoke(this, reason);
        }

        public void NotifySuccessfulLogin(Person userDetails)
        {
            LoginSuccessful?.Invoke(this, userDetails.IsAdmin);
        }

        public async Task LoginAsync(string username, string password)
        {
            await Task.Factory.StartNew(() =>
            {
                proxy.ClientCredentials.UserName.UserName = username;
                proxy.ClientCredentials.UserName.Password = password;
                try
                {
                    proxy.Login();
                    userData.Username = username;
                    userData.Password = password;
                    log.Info("User successfully logged in");
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Username and/or password are invalid");
                    NotifyEmergencyLogout();
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Communication error");
                    NotifyEmergencyLogout();
                }
            });
        }

        public async Task LogoutAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    proxy.Logout();
                    log.Info("User successfully logged out");
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Username and/or password are invalid");
                    NotifyEmergencyLogout();
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Communication error");
                    NotifyEmergencyLogout();
                }
            });
        }

        public void UpdateItemData(Delta item)
        {
            if (item.OldValue == null)
            {
                var displayNewItem = new DisplayItem(item.NewValue)
                {
                    CanBeOwner = (!item.NewValue.IsFound && userData.Username != item.NewValue.Finder) ? true : false
                };
                data.TriggerAddItemEvent(displayNewItem);
                log.Info($"Added item with id: {displayNewItem.ID}");
            }
            else if (item.NewValue == null)
            {
                data.TriggerRemoveItemEvent(item.OldValue.ID);
                log.Info($"Removed item with id: {item.OldValue.ID}");
            }
            else
            {
                var displayNewItem = new DisplayItem(item.NewValue)
                {
                    CanBeOwner = (!item.NewValue.IsFound && userData.Username != item.NewValue.Finder) ? true : false

                };
                data.TriggerUpdateItemEvent(displayNewItem);
                log.Info($"Updated item with id: {displayNewItem.ID}");
            }
        }

        public void UpdateUserData(UserDelta item)
        {
            if (item.OldValue == null)
            {
                var displayNewUser = new DisplayPerson()
                {
                    Username = item.NewValue.Username,
                    IsAdmin = item.NewValue.IsAdmin,
                    LastName = item.NewValue.LastName,
                    Name = item.NewValue.Name,
                };
                data.TriggerAddPersonEvent(displayNewUser);
                log.Info($"Added User with username: {displayNewUser.Username}");

            }
            else if (item.NewValue == null)
            {
                data.TriggerRemovePersonEvent(item.OldValue.Username);
                log.Info($"Removed user with username: {item.OldValue.Username}");

            }
            else
            {
                var displayNewUser = new DisplayPerson()
                {
                    Username = item.NewValue.Username,
                    IsAdmin = item.NewValue.IsAdmin,
                    LastName = item.NewValue.LastName,
                    Name = item.NewValue.Name,
                };
                data.TriggerUpdatePersonEvent(displayNewUser);
                log.Info($"Updated user with username: {displayNewUser.Username}");
            }
        }

        public void NotifySuccessfulLogout()
        {
            LogoutSuccessful?.Invoke(this, new EventArgs());
        }
        public void NotifyEmergencyLogout()
        {
            EmergencyLogout?.Invoke(this, new EventArgs());
        }
    }
}
