using Server.Interfaces;
using Server.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Model
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class DataService : IDataService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ItemOperationReturnValue AddItem(string title, DateTime dateTime, int location, string description,string finder)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                var item = new Item() { Datetime = dateTime, Description = description, Finder = finder, Location = location, Title = title };
                switch (Database.Database.Instance.AddItem(item, out int id))
                {
                    case DBResponseDatabaseData.OK:
                        {
                            item.ID = id;
                            CurrentConnections.Instance.UpdateItem(new ItemDelta() { NewValue = item, OldValue = null });
                            log.Info($"Item with id: {id} is successfully added to database");
                            return new ItemOperationReturnValue() { DatabaseValue = item, SubmittedValue = item, Status = Status.OK };
                        }
                    case DBResponseDatabaseData.INVALIDDATA:
                        {
                            log.Error("Invalid item data supplied for addition");
                            return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.INVALIDDATA };
                        }
                    default:
                        return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.INVALIDDATA };
                }
            }
            else
            {
                log.Warn("Unlogged user tried to add item");
                return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = null, Status = Status.NOTLOGGEDIN };
            }
        }
        public ItemOperationReturnValue AddItemWithOwner(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                var item = new Item() { Datetime = dateTime, Description = description, Finder = finder, Location = location, Title = title, Owner = owner, IsFound = isFound };
                switch (Database.Database.Instance.AddItemWithOwner(item, out int id))
                {
                    case DBResponseDatabaseData.OK:
                        {
                            item.ID = id;
                            CurrentConnections.Instance.UpdateItem(new ItemDelta() { NewValue = item, OldValue = null });
                            log.Info($"Item with id: {id} and owner {owner} is successfully added to database");
                            return new ItemOperationReturnValue() { DatabaseValue = item, SubmittedValue = item, Status = Status.OK };
                        }
                    case DBResponseDatabaseData.INVALIDDATA:
                        {
                            log.Error("Invalid item data supplied for addition");
                            return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.INVALIDDATA };
                        }
                    default:
                        return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.INVALIDDATA };
                }
            }
            else
            {
                log.Warn("Unlogged user tried to add item");
                return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = null, Status = Status.NOTLOGGEDIN };
            }
        }

        public ItemOperationReturnValue RemoveItem(Item item)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                switch (Database.Database.Instance.DeleteItem(item))
                {
                    case DBResponseDatabaseData.OK:
                        {
                            CurrentConnections.Instance.UpdateItem(new ItemDelta() { NewValue = null, OldValue = item });
                            log.Info($"Item with id: {item.ID} is successfully removed to database");
                            return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.OK };
                        }
                    case DBResponseDatabaseData.INVALIDDATA:
                        {
                            log.Error("Invalid item data supplied for removal");
                            return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = item, Status = Status.INVALIDDATA };
                        }
                    case DBResponseDatabaseData.CONFLICT:
                        {
                            var val = Database.Database.Instance.GetItem(item.ID);
                            Item userVal = null;
                            if (val != null)
                                userVal = new Item() { ID = val.ID, Datetime = val.Datetime, Description = val.Description, Finder = val.Finder, Location = val.Location, Owner = val.Owner, Title = val.Title };
                            log.Error($"Conflict achieved when tried to remove item with id {item.ID}");
                            return new ItemOperationReturnValue() { DatabaseValue = userVal, SubmittedValue = null, Status = Status.CONFLICT };
                        }
                    default:
                        return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = null, Status = Status.INVALIDDATA };

                }
            }
            else
            {
                log.Warn("Unlogged user tried to remove item");
                return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = null, Status = Status.NOTLOGGEDIN };
            }
        }

        public ItemOperationReturnValue UpdateItem(Item oldItem, Item newItem)
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                switch (Database.Database.Instance.UpdateItem(oldItem, newItem))
                {
                    case DBResponseDatabaseData.OK:
                        {
                            var itemDB = Database.Database.Instance.GetItem(oldItem.ID);
                            var item = new Item() { ID = itemDB.ID, Datetime = itemDB.Datetime, Description = itemDB.Description, Finder = itemDB.Finder, Owner = itemDB.Owner, Location = itemDB.Location, Title = itemDB.Title, IsFound = itemDB.IsFound };
                            CurrentConnections.Instance.UpdateItem(new ItemDelta() { OldValue = oldItem, NewValue = item });
                            log.Info($"Item with id: {oldItem.ID} is successfully updated in database");
                            return new ItemOperationReturnValue() { DatabaseValue = item, SubmittedValue = newItem, Status = Status.OK };
                        }
                    case DBResponseDatabaseData.INVALIDDATA:
                        {
                            log.Error("Invalid item data supplied for update");
                            return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = oldItem, Status = Status.INVALIDDATA };
                        }
                    case DBResponseDatabaseData.CONFLICT:
                        var val = Database.Database.Instance.GetItem(oldItem.ID);
                        Item userVal = null;
                        if (val != null)
                            userVal = new Item() { ID = val.ID, Datetime = val.Datetime, Description = val.Description, Finder = val.Finder, Location = val.Location, Owner = val.Owner, Title = val.Title };
                        log.Error($"Conflict achieved when tried to update item with id {oldItem.ID}");
                        return new ItemOperationReturnValue() { DatabaseValue = userVal, SubmittedValue = newItem, Status = Status.CONFLICT };
                    default:
                        return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = oldItem, Status = Status.INVALIDDATA };

                }
            }
            else
            {
                log.Warn("Unlogged user tried to update item");
                return new ItemOperationReturnValue() { DatabaseValue = null, SubmittedValue = null, Status = Status.NOTLOGGEDIN };
            }
        }

        ICollection<Item> IDataService.GetAllItems()
        {
            string username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            if (CurrentConnections.Instance.IsConnectionRegistered(username))
            {
                return Database.Database.Instance.GetAllItems();
            }
            else
                return null;
        }
    }
}
