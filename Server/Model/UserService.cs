using Server.Interfaces;
using Server.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Model
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    class UserService : IUserService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResponseCode AddPerson(Person person, string password)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                var code = (ResponseCode)Database.Database.Instance.AddUser(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name, person, password);
                if (code == ResponseCode.OK)
                    CurrentConnections.Instance.UpdateUser(new UserDelta() { OldValue = null, NewValue = Database.Database.Instance.GetPerson(person.Username) });
                switch (code)
                {
                    case ResponseCode.BADUSERSUPPLIED: log.Error("Invalid person data supplied for add operation"); break;
                    case ResponseCode.INVALIDRIGHT: log.Error("User with invalid privilege tried to add user"); break;
                    case ResponseCode.USERNAMETAKEN: log.Error("Tried to add user who's username is already taken"); break;
                    case ResponseCode.USERNOTFOUND: log.Error("Add operation invoked by non-user"); break;
                    case ResponseCode.OK: log.Info($"Person with username {person.Username} successfully added to database"); break;

                }
                return code;
            }
            else
            {
                log.Warn("Unlogged user tried to add person");
                return ResponseCode.NOTLOGGEDIN;
            }

        }

        public ICollection<Person> GetAllPersons()
        {
            return Database.Database.Instance.GetAllUsers();
        }

        public ResponseCode RemovePerson(string username)
        {
            string user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(user))
            {
                var oldUser = Database.Database.Instance.GetPerson(username);
                var code = (ResponseCode)Database.Database.Instance.RemoveUser(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name, username, out List<IValue> foundItems, out List<IValue> ownedItems);
                if (code == ResponseCode.OK)
                {
                    CurrentConnections.Instance.RemoveSession(username);
                    CurrentConnections.Instance.UpdateUser(new UserDelta() { OldValue = oldUser, NewValue = null });
                    foreach (var a in foundItems)
                    {
                        CurrentConnections.Instance.UpdateItem(new ItemDelta() { NewValue = null, OldValue = (Item)a });
                    }
                    foreach (var a in ownedItems)
                    {
                        CurrentConnections.Instance.UpdateItem(new ItemDelta() { NewValue = (Item)a, OldValue = (Item)a });
                    }

                }
                switch (code)
                {
                    case ResponseCode.BADUSERSUPPLIED: log.Error("Invalid person data supplied for remove operation"); break;
                    case ResponseCode.INVALIDRIGHT: log.Error("User with invalid privilege tried to remove user"); break;
                    case ResponseCode.USERNOTFOUND: log.Error("Remove operation invoked by non-user"); break;
                    case ResponseCode.OK: log.Info($"Person with username {username} successfully removed from database"); break;
                }

                return code;
            }
            else
            {
                log.Warn("Unlogged user tried to remove person");
                return ResponseCode.NOTLOGGEDIN;
            }

        }
        public ResponseCode UpdatePerson(string name, string lastName)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                var oldUser = Database.Database.Instance.GetPerson(username);
                var code = (ResponseCode)Database.Database.Instance.UpdateUser(username, name, lastName);
                if (code == ResponseCode.OK)
                    CurrentConnections.Instance.UpdateUser(new UserDelta() { NewValue = Database.Database.Instance.GetPerson(username), OldValue = oldUser });
                switch (code)
                {
                    case ResponseCode.BADUSERSUPPLIED: log.Error("Invalid person data supplied for update operation"); break;
                    case ResponseCode.USERNOTFOUND: log.Error("Update operation invoked by non-user"); break;
                    case ResponseCode.OK: log.Info($"Person with username {username} successfully updated it's details"); break;
                }
                return code;
            }
            else
            {
                log.Warn("Unlogged user tried to update person");
                return ResponseCode.NOTLOGGEDIN;
            }
        }
    }
}
