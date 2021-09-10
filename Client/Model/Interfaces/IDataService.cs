using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Client.Model.Interfaces
{
    interface IDataService
    {
        Task<IItemReturnValue> AddItemAsync(string title, DateTime dateTime, int location, string description,string finder);
        Task<IItemReturnValue> AddItemWithOwnerAsync(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound);
        ICollection<DisplayItem> GetItemsList();
        Task<IItemReturnValue> RemoveItemAsync(IItem item);
        Task<IItemReturnValue> UpdateItemAsync(IItem oldItem, IItem newItem);
    }
}