using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;

namespace Server.Database
{
    public class Database
    {
        private static readonly Lazy<Database>
      lazy =
      new Lazy<Database>
          (() => new Database());

        private static object lockDb;

        public static Database Instance { get { return lazy.Value; } }


        private Database()
        {
            lockDb = new object();
        }

        public bool DoesUserExist(string name)
        {
            bool ret = false;
            lock (lockDb)
            {
                using (UsersRepo repo = new UsersRepo())
                {
                    ret = repo.DoesUserExist(name);
                }
            }
            return ret;
        }

        public Person GetPerson(string username)
        {
            DatabasePerson user = null;
            using (UsersRepo repo = new UsersRepo())
            {
                user = repo.GetUser(username);
            }

            return new Person() { Username = user.Username, IsAdmin = user.IsAdmin, LastName = user.LastName, Name = user.Name };
        }

        public bool DoesUserExist(string name, string password)
        {
            bool ret = false;
            lock (lockDb)
            {
                using (UsersRepo repo = new UsersRepo())
                {
                    ret = repo.DoesUserExist(name, password);
                }
            }
            return ret;
        }

        public DBResponseDatabasePerson AddUser(string currentUser, IUser user, string password)
        {
            DBResponseDatabasePerson ret;
            lock (lockDb)
            {
                using (var repo = new UsersRepo())
                {
                    ret = repo.AddUser(currentUser, user, password);
                }
            }
            return ret;
        }
        public DBResponseDatabasePerson UpdateUser(string currentUser, string newName, string newLastName)
        {
            DBResponseDatabasePerson ret;
            lock (lockDb)
            {
                using (var repo = new UsersRepo())
                {
                    ret = repo.UpdateUser(currentUser, newName, newLastName);
                }
            }
            return ret;
        }

        public DBResponseDatabasePerson RemoveUser(string currentUser, string userDeleting, out List<IValue> foundItems, out List<IValue> ownedItems)
        {
            DBResponseDatabasePerson ret;
            List<DatabaseItem> found;
            List<DatabaseItem> owned;
            lock (lockDb)
            {
                using (var repo = new UsersRepo())
                {
                    ret = repo.DeleteUser(currentUser, userDeleting, out found, out owned);
                }
            }
            foundItems = new List<IValue>();
            ownedItems = new List<IValue>();
            foreach (var a in found)
            {
                var temp = new Item()
                {
                    Datetime = a.Datetime,
                    Description = a.Description,
                    Finder = a.Finder.Username,
                    ID = a.ID,
                    IsFound = a.IsFound,
                    Location = a.Location,
                    Owner = null,
                    Title = a.Title
                };
                foundItems.Add(temp);
            }
            foreach (var a in owned)
            {
                var temp = new Item()
                {
                    Datetime = a.Datetime,
                    Description = a.Description,
                    Finder = a.Finder.Username,
                    ID = a.ID,
                    IsFound = a.IsFound,
                    Location = a.Location,
                    Owner = null,
                    Title = a.Title
                };
                ownedItems.Add(temp);
            }
            return ret;
        }

        public DBResponseDatabaseData AddItem(IValue item, out int id)
        {
            DBResponseDatabaseData ret;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    ret = repo.AddItem(item, out id);
                }
            }
            return ret;
        }
        public DBResponseDatabaseData AddItemWithOwner(IValue item, out int id)
        {
            DBResponseDatabaseData ret;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    ret = repo.AddItemWithOwner(item, out id);
                }
            }
            return ret;
        }

        public DBResponseDatabaseData UpdateItem(IValue oldItem, IValue newItem)
        {
            DBResponseDatabaseData ret;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    ret = repo.UpdateItem(oldItem, newItem);
                }
            }
            return ret;
        }

        public DBResponseDatabaseData DeleteItem(IValue item)
        {
            DBResponseDatabaseData ret;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    ret = repo.DeleteItem(item);
                }
            }
            return ret;
        }

        public IValue GetItem(int ID)
        {
            IDatabaseValue value = null;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    value = repo.GetItem(ID);
                }
            }
            if (value != null)
                return new Item() { IsFound = value.IsFound, Owner = value.Owner?.Username, Datetime = value.Datetime, Description = value.Description, Finder = value.Finder.Username, ID = value.ID, Location = value.Location, Title = value.Title };
            else
                return null;

        }

        public ICollection<Item> GetAllItems()
        {
            ICollection<DatabaseItem> items = null;
            lock (lockDb)
            {
                using (var repo = new DataRepo())
                {
                    items = repo.GetAllItems();
                }
            }
            List<Item> ret = new List<Item>();
            foreach (var a in items)
            {
                ret.Add(new Item() { Datetime = a.Datetime, IsFound = a.IsFound, Description = a.Description, Finder = a.Finder.Username, ID = a.ID, Location = a.Location, Owner = a.Owner?.Username, Title = a.Title });
            }

            return ret;
        }
        public ICollection<Person> GetAllUsers()
        {
            ICollection<DatabasePerson> users = null;
            lock (lockDb)
            {
                using (var repo = new UsersRepo())
                {
                    users = repo.GetAllUsers();
                }
            }
            List<Person> ret = new List<Person>();
            foreach (var a in users)
            {
                ret.Add(new Person() { IsAdmin = a.IsAdmin, LastName = a.LastName, Name = a.Name, Username = a.Username });
            }

            return ret;
        }
    }
}
