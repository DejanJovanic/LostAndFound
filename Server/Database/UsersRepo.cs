using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database
{
    class UsersRepo : IDisposable
    {
        LostAndFoundContext context;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UsersRepo()
        {
            context = new LostAndFoundContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }


        public DBResponseDatabasePerson AddUser(string currentUser, IUser newUser, string password)
        {
            var users = context.Users.Where(i => i.Username.Equals(currentUser)).ToList();
            var user = users.FirstOrDefault(i => i.Username.Equals(currentUser));

            if (user != null)
            {
                if (user.IsAdmin)
                {
                    if (newUser.IsValid())
                    {
                        var usersDP = context.Users.Where(i => i.Username.Equals(newUser.Username)).ToList();
                        var userDP = usersDP.FirstOrDefault(i => i.Username.Equals(newUser.Username));
                        if (userDP == null)
                        {
                            DatabasePerson person = new DatabasePerson() { IsAdmin = newUser.IsAdmin, LastName = newUser.LastName, Name = newUser.Name, Username = newUser.Username, Password = password };
                            context.Users.Add(person);
                            try
                            {
                                context.SaveChanges();
                                log.Info($"Admin with username: {currentUser} successfully added user {newUser.Username} as {(newUser.IsAdmin ? "Admin" : "Regular user")}");
                                return DBResponseDatabasePerson.OK;
                            }
                            catch (Exception e)
                            {
                                log.Fatal(e);
                                return DBResponseDatabasePerson.BADUSERSUPPLIED;
                            }

                        }
                        else
                            return DBResponseDatabasePerson.USERNAMETAKEN;
                    }
                    else
                        return DBResponseDatabasePerson.BADUSERSUPPLIED;
                }
                else
                    return DBResponseDatabasePerson.INVALIDRIGHT;
            }
            else
                return DBResponseDatabasePerson.USERNOTFOUND;
        }

        public DBResponseDatabasePerson AddUserServer(IDatabasePerson newUser)
        {
            if (newUser.IsValid())
            {
                var users = context.Users.Where(i => i.Username.Equals(newUser.Username)).ToList();
                var user = users.FirstOrDefault(i => i.Username.Equals(newUser.Username));
                if (user == null)
                {
                    DatabasePerson person = new DatabasePerson() { IsAdmin = newUser.IsAdmin, LastName = newUser.LastName, Name = newUser.Name, Username = newUser.Username, Password = newUser.Password };
                    context.Users.Add(person);
                    try
                    {
                        log.Info($"Server successfully auto added user {newUser.Username} as {(newUser.IsAdmin ? "Admin" : "Regular user")}");
                        context.SaveChanges();
                        return DBResponseDatabasePerson.OK;
                    }
                    catch (Exception e)
                    {
                        log.Fatal(e);
                        return DBResponseDatabasePerson.BADUSERSUPPLIED;
                    }

                }
                else
                    return DBResponseDatabasePerson.USERNAMETAKEN;
            }
            else
                return DBResponseDatabasePerson.BADUSERSUPPLIED;
        }

        public DBResponseDatabasePerson DeleteUser(string currentUser, string userDeleting, out List<DatabaseItem> foundItems, out List<DatabaseItem> ownedItems)
        {
            var users = context.Users.Where(i => i.Username.Equals(currentUser)).ToList();
            var user = users.FirstOrDefault(i => i.Username.Equals(currentUser));
            foundItems = new List<DatabaseItem>();
            ownedItems = new List<DatabaseItem>();
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    if (!string.IsNullOrWhiteSpace(userDeleting))
                    {

                        var usersDP = context.Users.Where(i => i.Username.Equals(userDeleting)).ToList();
                        var databasePerson = usersDP.FirstOrDefault(i => i.Username.Equals(userDeleting));
                        if (databasePerson != null && !databasePerson.IsAdmin)
                        {

                            foreach (var a in databasePerson.FoundItems.ToArray())
                            {
                                var temp = new DatabaseItem()
                                {
                                    Datetime = a.Datetime,
                                    Description = a.Description,
                                    ID = a.ID,
                                    IsFound = a.IsFound,
                                    Location = a.Location,
                                    Title = a.Title,
                                    Finder = new DatabasePerson() { Username = a.Finder.Username }
                                };
                                foundItems.Add(temp);
                            }
                            foreach (var a in databasePerson.OwnedItems.ToArray())
                            {
                                var temp = new DatabaseItem()
                                {
                                    Datetime = a.Datetime,
                                    Description = a.Description,
                                    ID = a.ID,
                                    IsFound = a.IsFound,
                                    Location = a.Location,
                                    Title = a.Title,
                                    Finder = new DatabasePerson() { Username = a.Finder.Username }
                                };
                                ownedItems.Add(temp);
                            }
                            context.Items.RemoveRange(databasePerson.FoundItems.ToArray());
                            context.Users.Remove(databasePerson);
                            try
                            {
                                context.SaveChanges();
                                log.Info($"Admin with username: {currentUser} successfully deleted user {userDeleting}");
                                return DBResponseDatabasePerson.OK;
                            }
                            catch (Exception e)
                            {
                                log.Fatal(e);
                                return DBResponseDatabasePerson.BADUSERSUPPLIED;
                            }

                        }
                        else
                            return DBResponseDatabasePerson.BADUSERSUPPLIED;
                    }
                    else
                        return DBResponseDatabasePerson.BADUSERSUPPLIED;
                }
                else
                    return DBResponseDatabasePerson.INVALIDRIGHT;
            }
            else
                return DBResponseDatabasePerson.USERNOTFOUND;
        }

        public bool DoesUserExist(string username)
        {
            var users = context.Users.Where(i => i.Username.Equals(username)).ToList();
            return (users.FirstOrDefault(i => i.Username.Equals(username)) == null) ? false : true;
        }

        public bool DoesUserExist(string username, string password)
        {
            var users = context.Users.Where(i => i.Username.Equals(username) && i.Password.Equals(password)).ToList();
            return (users.FirstOrDefault(i => i.Username.Equals(username) && i.Password.Equals(password)) == null) ? false : true;
        }
        public DBResponseDatabasePerson UpdateUser(string currentUser, string newName, string newLastName)
        {
            var users = context.Users.Where(i => i.Username.Equals(currentUser)).ToList();
            var user = users.FirstOrDefault(i => i.Username.Equals(currentUser));
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newLastName))
                {
                    user.Name = newName;
                    user.LastName = newLastName;
                    try
                    {
                        context.SaveChanges();
                        log.Info($"{currentUser} changed name {newName} and last name to {newLastName}");
                        return DBResponseDatabasePerson.OK;
                    }
                    catch (Exception e)
                    {
                        log.Fatal(e);
                        return DBResponseDatabasePerson.BADUSERSUPPLIED;

                    }

                }
                else
                    return DBResponseDatabasePerson.BADUSERSUPPLIED;
            }
            else
                return DBResponseDatabasePerson.USERNOTFOUND;

        }

        public DatabasePerson GetUser(string username)
        {
            var users = context.Users.Where(i => i.Username.Equals(username)).ToList();
            var user = users.FirstOrDefault(i => i.Username.Equals(username));
            if (user == null)
                return null;
            else
                return new DatabasePerson() { Username = user.Username, IsAdmin = user.IsAdmin, LastName = user.LastName, Name = user.Name, Password = user.Password };

        }

        public ICollection<DatabasePerson> GetAllUsers()
        {
            ICollection<DatabasePerson> ret = new List<DatabasePerson>();
            var items = context.Users.ToList();
            foreach (var a in items)
            {
                ret.Add(new DatabasePerson() { FoundItems = null, IsAdmin = a.IsAdmin, LastName = a.LastName, Name = a.Name, Password = a.Password, Username = a.Username });
            }

            return ret;
        }
    }
}
