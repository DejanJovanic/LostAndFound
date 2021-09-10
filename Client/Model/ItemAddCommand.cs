using Client.Model.Interfaces;
using System;
using System.Threading.Tasks;

namespace Client.Model
{
    class ItemAddCommand : IItemServiceCommand
    {
        private IDataService service;
        private IItem item;
        private string title;
        private int location;
        private string description;
        private DateTime dateTime;
        private string finder;

        public ItemAddCommand(IDataService service, string title, int location, string description, DateTime dateTime,string finder)
        {
            this.service = service;
            this.title = title;
            this.location = location;
            this.description = description;
            this.dateTime = dateTime;
            this.finder = finder;
        }

        public async Task<IItemReturnValue> ExecuteAsync()
        {
            var ret = await service.AddItemAsync(title, dateTime, location, description,finder);
            item = ret.DatabaseValue.Clone() as IItem;
            return ret;
        }

        public async Task<IItemReturnValue> UnexecuteAsync()
        {
            return await service.RemoveItemAsync(item);
        }
    }
}
