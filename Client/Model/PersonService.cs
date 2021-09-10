using Client.Model.Interfaces;
using Client.UserServiceReference;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Client.Model
{
    class PersonService : IDisposable, IPersonService
    {
        UserServiceClient proxy;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IUserData data;
        IEmergencyLogout emergency;
        public PersonService(IEmergencyLogout emergency, IUserData userData)
        {
            proxy = new UserServiceClient("NetTcpBinding_IUserService");
            this.data = userData;
            this.emergency = emergency;
            proxy.ClientCredentials.UserName.UserName = userData.Username;
            proxy.ClientCredentials.UserName.Password = userData.Password;
        }

        public async Task<PersonReturnCode> AddPersonAsync(IPerson person, string password)
        {
            return await Task.Factory.StartNew(param =>
            {
                var paramArray = (object[])param;
                var personSend = new Person() { Username = ((IPerson)paramArray[0]).Username, IsAdmin = ((IPerson)paramArray[0]).IsAdmin, LastName = ((IPerson)paramArray[0]).LastName, Name = ((IPerson)paramArray[0]).Name };
                try
                {
                    var ret = (PersonReturnCode)Enum.Parse(typeof(PersonReturnCode), proxy.AddPerson(personSend, (string)paramArray[1]).ToString());
                    switch (ret)
                    {
                        case PersonReturnCode.BADUSERSUPPLIED: log.Error("Invalid data supplied for person addition"); break;
                        case PersonReturnCode.INVALIDRIGHT: log.Error("Invalid right"); break;
                        case PersonReturnCode.USERNAMETAKEN: log.Error("User with supplied username already exists"); break;
                        case PersonReturnCode.USERNOTFOUND:
                            {
                                log.Fatal("Current user does not exist");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                        case PersonReturnCode.OK: log.Info($"User with username: {personSend.Username} successfully added"); break;
                        case PersonReturnCode.NOTLOGGEDIN:
                            {
                                log.Fatal("Current user is not logged in");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return ret;
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return PersonReturnCode.NOTLOGGEDIN;
                }
            }, new object[] { person, password });
        }

        public void Dispose()
        {
            if (proxy.State == CommunicationState.Faulted)
                proxy.Abort();
            else
                proxy.Close();
        }

        public async Task<PersonReturnCode> RemovePersonAsync(string personUsername)
        {
            return await Task.Factory.StartNew(param =>
            {
                try
                {
                    var ret = (PersonReturnCode)Enum.Parse(typeof(PersonReturnCode), proxy.RemovePerson((string)param).ToString());
                    switch (ret)
                    {
                        case PersonReturnCode.BADUSERSUPPLIED: log.Error("Invalid data supplied for person removal"); break;
                        case PersonReturnCode.INVALIDRIGHT: log.Error("Invalid right"); break;
                        case PersonReturnCode.USERNAMETAKEN: log.Error("User with supplied username already exists"); break;
                        case PersonReturnCode.USERNOTFOUND:
                            {
                                log.Fatal("Current user does not exist");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                        case PersonReturnCode.OK: log.Info($"User with username: {personUsername} successfully removed"); break;
                        case PersonReturnCode.NOTLOGGEDIN:
                            {
                                log.Fatal("Current user is not logged in");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return ret;

                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return PersonReturnCode.NOTLOGGEDIN;
                }
            }, personUsername);
        }

        public async Task<PersonReturnCode> UpdatePersonAsync(string name, string lastName)
        {
            return await Task.Factory.StartNew(param =>
            {

                var paramArray = (object[])param;
                try
                {
                    var ret = (PersonReturnCode)Enum.Parse(typeof(PersonReturnCode), proxy.UpdatePerson((string)paramArray[0], (string)paramArray[1]).ToString());
                    switch (ret)
                    {
                        case PersonReturnCode.BADUSERSUPPLIED: log.Error("Invalid data supplied for person update"); break;
                        case PersonReturnCode.INVALIDRIGHT: log.Error("Invalid right"); break;
                        case PersonReturnCode.USERNAMETAKEN: log.Error("User with supplied username already exists"); break;
                        case PersonReturnCode.USERNOTFOUND:
                            {
                                log.Fatal("Current user does not exist");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                        case PersonReturnCode.OK: log.Info($"User details successfully updated"); break;
                        case PersonReturnCode.NOTLOGGEDIN:
                            {
                                log.Fatal("Current user is not logged in");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return ret;
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return PersonReturnCode.NOTLOGGEDIN;
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return PersonReturnCode.NOTLOGGEDIN;
                }
            }, new object[] { name, lastName });
        }

        public ICollection<DisplayPerson> GetPersonsList()
        {
            try
            {
                var items = proxy.GetAllPersons();
                var ret = new List<DisplayPerson>();
                for (int i = 0; i < items.Length; i++)
                {
                    var displayPerson = new DisplayPerson()
                    {
                        IsAdmin = items[i].IsAdmin,
                        LastName = items[i].LastName,
                        Name = items[i].Name,
                        Username = items[i].Username

                    };
                    ret.Add(displayPerson);
                }
                return ret;
            }
            catch (MessageSecurityException)
            {
                log.Fatal("Invalid credentials suplied");
                emergency.NotifyEmergencyLogout();
                return new List<DisplayPerson>() { new DisplayPerson() { Username = "" } };
            }
            catch (CommunicationObjectFaultedException)
            {
                log.Fatal("Connection broke up");
                emergency.NotifyEmergencyLogout();
                return new List<DisplayPerson>() { new DisplayPerson() { Username = "" } };
            }
            catch (Exception e)
            {
                log.Error(e);
                return new List<DisplayPerson>() { new DisplayPerson() { Username = "" } };
            }

        }
    }
}
