using Client.Model.Interfaces;
using System;
using System.Threading.Tasks;

namespace Client.Model
{
    class ItemAddWithOwnerCommand : IItemServiceCommand
    {
        private IDataService service;
        private IItem item;
        private string title;
        private int location;
        private string description;
        private DateTime dateTime;
        private bool isFound;
        private string owner;
        private string finder;

        public ItemAddWithOwnerCommand(IDataService service, string title, int location, string description, DateTime dateTime,string finder, string owner, bool isFound)
        {
            this.service = service;
            this.title = title;
            this.location = location;
            this.description = description;
            this.dateTime = dateTime;
            this.owner = owner;
            this.isFound = isFound;
            this.finder = finder;
        }

        public async Task<IItemReturnValue> ExecuteAsync()
        {
            var ret = await service.AddItemWithOwnerAsync(title, dateTime, location, description,finder, owner, isFound);
            item = ret.DatabaseValue.Clone() as IItem;
            return ret;
        }

        public async Task<IItemReturnValue> UnexecuteAsync()
        {
            return await service.RemoveItemAsync(item);
        }
    }
}
