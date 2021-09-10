using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Client.Model
{
    class DataService : IDataService, IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataServiceReference.DataServiceClient proxy;
        IEmergencyLogout emergency;
        private IUserData userData;

        public DataService(IEmergencyLogout emergency, IUserData userData)
        {
            proxy = new DataServiceReference.DataServiceClient("NetTcpBinding_IDataService");
            this.emergency = emergency;
            proxy.ClientCredentials.UserName.UserName = userData.Username;
            proxy.ClientCredentials.UserName.Password = userData.Password;
            this.userData = userData;
        }
        public async Task<IItemReturnValue> AddItemAsync(string title, DateTime dateTime, int location, string description,string finder)
        {
            return await Task.Factory.StartNew(param =>
            {
                var paramArray = (object[])param;
                try
                {
                    var ret = proxy.AddItem((string)paramArray[0], (DateTime)paramArray[1], (int)paramArray[2], (string)paramArray[3], (string)paramArray[4]);
                    var dbValue = new DisplayItem(ret.DatabaseValue);
                    var SubValue = new DisplayItem(ret.SubmittedValue);
                    var code = (Response)Enum.Parse(typeof(Response), ret.Status.ToString());
                    switch (code)
                    {
                        case Response.OK: log.Info($"Item with id: {dbValue.ID} successfully added to database"); break;
                        case Response.INVALIDDATA: log.Error("Invalid item data supplied for addition"); break;
                        case Response.NOTLOGGEDIN:
                            {
                                log.Fatal("Session has been terminated by service");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return new ItemReturnValue() { Response = code, DatabaseValue = dbValue, SubmitedValue = SubValue };
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.INVALIDDATA };
                }
            }, new object[] { title, dateTime, location, description,finder });
        }

        public async Task<IItemReturnValue> AddItemWithOwnerAsync(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound)
        {
            return await Task.Factory.StartNew(param =>
            {
                var paramArray = (object[])param;
                try
                {
                    var ret = proxy.AddItemWithOwner((string)paramArray[0], (DateTime)paramArray[1], (int)paramArray[2], (string)paramArray[3], (string)paramArray[4], (string)paramArray[5], (bool)paramArray[6]);
                    var dbValue = new DisplayItem(ret.DatabaseValue);
                    var SubValue = new DisplayItem(ret.SubmittedValue);
                    var code = (Response)Enum.Parse(typeof(Response), ret.Status.ToString());
                    switch (code)
                    {
                        case Response.OK: log.Info($"Item with id: {dbValue.ID} successfully added to database"); break;
                        case Response.INVALIDDATA: log.Error("Invalid item data supplied for addition"); break;
                        case Response.NOTLOGGEDIN:
                            {
                                log.Fatal("Session has been terminated by service");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return new ItemReturnValue() { Response = code, DatabaseValue = dbValue, SubmitedValue = SubValue };
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.INVALIDDATA };
                }
            }, new object[] { title, dateTime, location, description,finder, owner, isFound });
        }

        public async Task<IItemReturnValue> RemoveItemAsync(IItem item)
        {
            return await Task.Factory.StartNew(param =>
            {
                var itemLambda = param as IItem;
                var value = new DataServiceReference.Item() { Datetime = itemLambda.DateTime, Description = itemLambda.Description, Finder = itemLambda.Finder, ID = itemLambda.ID, Location = itemLambda.Location, Owner = itemLambda.Owner, Title = itemLambda.Title, IsFound = itemLambda.IsFound };
                try
                {
                    var ret = proxy.RemoveItem(value);
                    DisplayItem SubValue = null;
                    if (ret.SubmittedValue != null)
                        SubValue = new DisplayItem(ret.SubmittedValue);
                    DisplayItem dbValue = null;
                    if (ret.DatabaseValue != null)
                        dbValue = new DisplayItem(ret.DatabaseValue);
                    var code = (Response)Enum.Parse(typeof(Response), ret.Status.ToString());
                    switch (code)
                    {
                        case Response.OK: log.Info($"Item with id: {SubValue.ID} successfully removed from database"); break;
                        case Response.INVALIDDATA: log.Error("Invalid item data supplied for removal"); break;
                        case Response.CONFLICT: log.Error($"Conflict achieved when tried to remove item"); break;
                        case Response.NOTLOGGEDIN:
                            {
                                log.Fatal("Session has been terminated by service");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return new ItemReturnValue() { Response = code, DatabaseValue = dbValue, SubmitedValue = SubValue };
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.INVALIDDATA };
                }
            }, item);
        }

        public async Task<IItemReturnValue> UpdateItemAsync(IItem oldItem, IItem newItem)
        {
            return await Task.Factory.StartNew(param =>
            {
                var paramArray = param as object[];
                var itemLambda = paramArray[0] as IItem;
                var oldValue = new DataServiceReference.Item() { Datetime = itemLambda.DateTime, Description = itemLambda.Description, Finder = itemLambda.Finder, ID = itemLambda.ID, Location = itemLambda.Location, Owner = itemLambda.Owner, Title = itemLambda.Title, IsFound = itemLambda.IsFound };
                var itemLambda2 = paramArray[1] as IItem;
                var newValue = new DataServiceReference.Item() { Datetime = itemLambda2.DateTime, Description = itemLambda2.Description, Finder = itemLambda2.Finder, ID = itemLambda2.ID, Location = itemLambda2.Location, Owner = itemLambda2.Owner, Title = itemLambda2.Title, IsFound = itemLambda2.IsFound };
                try
                {
                    var ret = proxy.UpdateItem(oldValue, newValue);
                    DisplayItem dbValue = null;
                    if (ret.DatabaseValue != null)
                        dbValue = new DisplayItem(ret.DatabaseValue);
                    var SubValue = new DisplayItem(ret.SubmittedValue);
                    var code = (Response)Enum.Parse(typeof(Response), ret.Status.ToString());
                    switch (code)
                    {
                        case Response.OK: log.Info($"Item with id: {SubValue.ID} successfully updated in database"); break;
                        case Response.INVALIDDATA: log.Error("Invalid item data supplied for update"); break;
                        case Response.CONFLICT: log.Error($"Conflict achieved when tried to update item"); break;
                        case Response.NOTLOGGEDIN:
                            {
                                log.Fatal("Session has been terminated by service");
                                emergency.NotifyEmergencyLogout();
                                break;
                            }
                    }
                    return new ItemReturnValue() { Response = code, DatabaseValue = dbValue, SubmitedValue = SubValue };
                }
                catch (MessageSecurityException)
                {
                    log.Fatal("Invalid credentials suplied");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (CommunicationObjectFaultedException)
                {
                    log.Fatal("Connection broke up");
                    emergency.NotifyEmergencyLogout();
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.NOTLOGGEDIN };
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return new ItemReturnValue() { SubmitedValue = null, DatabaseValue = null, Response = Response.INVALIDDATA };
                }
            }, new object[] { oldItem, newItem });
        }

        public ICollection<DisplayItem> GetItemsList()
        {
            try
            {
                var items = proxy.GetAllItems();
                var ret = new List<DisplayItem>();
                for (int i = 0; i < items.Length; i++)
                {
                    var displayNewItem = new DisplayItem(items[i])
                    {
                        CanBeOwner = (!items[i].IsFound && userData.Username != items[i].Finder) ? true : false
                    };
                    ret.Add(displayNewItem);
                }
                return ret;
            }
            catch (MessageSecurityException)
            {
                log.Fatal("Invalid credentials suplied");
                emergency.NotifyEmergencyLogout();
                return new List<DisplayItem>() { new DisplayItem() { ID = -1 } };
            }
            catch (CommunicationObjectFaultedException)
            {
                log.Fatal("Connection broke up");
                emergency.NotifyEmergencyLogout();
                return new List<DisplayItem>() { new DisplayItem() { ID = -1 } };
            }
            catch (Exception e)
            {
                log.Error(e);
                return new List<DisplayItem>() { new DisplayItem() { ID = -1 } };
            }

        }

        public void Dispose()
        {
            if (proxy.State == CommunicationState.Faulted)
                proxy.Abort();
            else
                proxy.Close();
        }
    }
}
