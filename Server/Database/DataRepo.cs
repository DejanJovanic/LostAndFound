using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database
{
    class DataRepo : IDisposable
    {
        LostAndFoundContext context;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataRepo()
        {
            context = new LostAndFoundContext();
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public DBResponseDatabaseData AddItem(IValue item, out int id)
        {
            var finders = context.Users.Where(i => i.Username == item.Finder);
            var finder = finders.FirstOrDefault(i => i.Username == item.Finder);
            if (finder != null && item.CheckFields())
            {
                var input = new DatabaseItem() { Datetime = item.Datetime, Description = item.Description, Location = item.Location, Title = item.Title, Owner = null, Finder = finder, IsFound = false };
                finder.FoundItems.Add(input);
                try
                {
                    context.SaveChanges();
                    id = input.ID;
                    log.Info($"Successfully added item with id: {id}");
                    return DBResponseDatabaseData.OK;
                }
                catch (Exception e)
                {
                    log.Fatal(e);
                    id = -1;
                    return DBResponseDatabaseData.INVALIDDATA;

                }

            }
            else
            {
                id = -1;
                return DBResponseDatabaseData.INVALIDDATA;
            }

        }

        public DBResponseDatabaseData AddItemWithOwner(IValue item, out int id)
        {
            var finders = context.Users.Where(i => i.Username == item.Finder);
            var finder = finders.FirstOrDefault(i => i.Username == item.Finder);
            if (finder != null && item.CheckFields())
            {
                if (string.IsNullOrWhiteSpace(item.Owner) && item.IsFound)
                {
                    var input = new DatabaseItem() { Datetime = item.Datetime, Description = item.Description, Location = item.Location, Title = item.Title, Owner = null, Finder = finder, IsFound = true };
                    finder.FoundItems.Add(input);
                    try
                    {
                        context.SaveChanges();
                        id = input.ID;
                        log.Info($"Successfully added found item with id: {id} with no owner");
                        return DBResponseDatabaseData.OK;
                    }
                    catch (Exception e)
                    {
                        log.Fatal(e);
                        id = -1;
                        return DBResponseDatabaseData.INVALIDDATA;

                    }
                }
                var owners = context.Users.Where(i => i.Username == item.Owner);
                var owner = owners.FirstOrDefault(i => i.Username == item.Owner);
                if (owner != null && finder.Username != owner.Username)
                {
                    var input = new DatabaseItem() { Datetime = item.Datetime, Description = item.Description, Location = item.Location, Title = item.Title, Owner = owner, Finder = finder, IsFound = item.IsFound };
                    finder.FoundItems.Add(input);
                    owner.OwnedItems.Add(input);
                    try
                    {
                        context.SaveChanges();
                        id = input.ID;
                        log.Info($"Successfully added item with id: {id} with owner: {item.Owner}");
                        return DBResponseDatabaseData.OK;
                    }
                    catch (Exception e)
                    {
                        log.Fatal(e);
                        id = -1;
                        return DBResponseDatabaseData.INVALIDDATA;

                    }


                }
                else

                {
                    id = -1;
                    return DBResponseDatabaseData.INVALIDDATA;
                }
            }
            else
            {
                id = -1;
                return DBResponseDatabaseData.INVALIDDATA;
            }

        }

        public DBResponseDatabaseData UpdateItem(IValue currentValue, IValue newValue)
        {
            if (currentValue.CheckFields() && newValue.CheckFields() && currentValue.ID == newValue.ID && currentValue.Finder == newValue.Finder && newValue.Finder != newValue.Owner && !currentValue.IsEqual(newValue))
            {
                var item = context.Items.Where(i => i.ID == currentValue.ID).SingleOrDefault();
                if (item != null)
                {
                    if (item.IsEqual(currentValue))
                    {
                        item.Datetime = newValue.Datetime;
                        item.Description = newValue.Description;
                        item.Title = newValue.Title;
                        item.Location = newValue.Location;
                        if (newValue.Owner != currentValue.Owner)
                        {

                            if (!string.IsNullOrWhiteSpace(newValue.Owner) && string.IsNullOrWhiteSpace(currentValue.Owner))
                            {

                                var newOwners = context.Users.Where(i => i.Username == newValue.Owner);
                                var newOwner = newOwners.FirstOrDefault(i => i.Username == newValue.Owner);
                                newOwner.OwnedItems.Add(item);
                                item.Owner = newOwner;
                                item.IsFound = true;

                            }
                            else if (string.IsNullOrWhiteSpace(newValue.Owner) && !string.IsNullOrWhiteSpace(currentValue.Owner))
                            {

                                var oldOwners = context.Users.Where(i => i.Username == currentValue.Owner);
                                var oldOwner = oldOwners.FirstOrDefault(i => i.Username == currentValue.Owner);
                                oldOwner.OwnedItems.Remove(item);
                                item.Owner = null;
                                item.IsFound = false;
                            }

                        }
                        try
                        {
                            context.SaveChanges();
                            log.Info($"Item with id: {currentValue.ID} updated");
                            return DBResponseDatabaseData.OK;
                        }
                        catch (Exception e)
                        {
                            log.Fatal(e);
                            return DBResponseDatabaseData.INVALIDDATA;
                        }

                    }
                    else
                        return DBResponseDatabaseData.CONFLICT;
                }
                else
                    return DBResponseDatabaseData.CONFLICT;

            }
            else
                return DBResponseDatabaseData.INVALIDDATA;
        }

        public DBResponseDatabaseData DeleteItem(IValue value)
        {
            var item = context.Items.Where(i => i.ID == value.ID).SingleOrDefault();

            if (item != null && value.CheckFields())
            {
                if (item.IsEqual(value))
                {
                    context.Items.Remove(item);
                    try
                    {
                        context.SaveChanges();
                        log.Info($"Item with id: {value.ID} deleted");
                        return DBResponseDatabaseData.OK;
                    }
                    catch (Exception e)
                    {
                        log.Fatal(e);
                        return DBResponseDatabaseData.INVALIDDATA;
                    }


                }
                else
                    return DBResponseDatabaseData.CONFLICT;
            }
            else
                return DBResponseDatabaseData.INVALIDDATA;
        }

        public DatabaseItem GetItem(int ID)
        {
            var item = context.Items.Include("Finder").Include("Owner").Where(i => i.ID == ID).SingleOrDefault();
            if (item != null)
                return new DatabaseItem() { ID = item.ID, IsFound = item.IsFound, Datetime = item.Datetime, Description = item.Description, Finder = item.Finder, Location = item.Location, Owner = item.Owner, Title = item.Title };
            else
                return null;
        }

        public ICollection<DatabaseItem> GetAllItems()
        {
            ICollection<DatabaseItem> ret = new List<DatabaseItem>();
            var items = context.Items.Include("Finder").Include("Owner");
            foreach (var a in items)
            {
                var finder = new DatabasePerson() { FoundItems = null, IsAdmin = a.Finder.IsAdmin, LastName = a.Finder.LastName, Name = a.Finder.Name, Password = a.Finder.Password, Username = a.Finder.Username };
                var owner = (a.Owner != null) ? new DatabasePerson() { FoundItems = null, IsAdmin = a.Owner.IsAdmin, LastName = a.Owner.LastName, Name = a.Owner.Name, Password = a.Owner.Password, Username = a.Owner.Username } : null;
                ret.Add(new DatabaseItem() { Finder = finder, IsFound = a.IsFound, Owner = owner, Datetime = a.Datetime, Description = a.Description, ID = a.ID, Location = a.Location, Title = a.Title });
            }

            return ret;
        }
    }
}
