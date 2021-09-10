using Server.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Interfaces
{
    [ServiceContract(Name = "IDataService")]
    [ServiceKnownType(typeof(Item))]
    [ServiceKnownType(typeof(ItemOperationReturnValue))]

    public interface IDataService
    {
        [OperationContract]
        ICollection<Item> GetAllItems();

        [OperationContract]
        ItemOperationReturnValue AddItem(string title, DateTime dateTime, int location, string description,string finder);
        [OperationContract]
        ItemOperationReturnValue AddItemWithOwner(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound);

        [OperationContract]
        ItemOperationReturnValue RemoveItem(Item item);

        [OperationContract]
        ItemOperationReturnValue UpdateItem(Item oldItem, Item newItem);
    }
}
