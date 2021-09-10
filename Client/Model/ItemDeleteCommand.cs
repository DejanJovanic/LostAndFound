using Client.Model.Interfaces;
using System.Threading.Tasks;

namespace Client.Model
{
    class ItemDeleteCommand : IItemServiceCommand
    {
        IDataService service;
        IItem item;

        public ItemDeleteCommand(IDataService service, IItem item)
        {
            this.service = service;
            this.item = (IItem)item.Clone();
        }

        public async Task<IItemReturnValue> ExecuteAsync()
        {
            return await service.RemoveItemAsync(item);
        }

        public async Task<IItemReturnValue> UnexecuteAsync()
        {
            if (string.IsNullOrWhiteSpace(item.Owner))
                return await service.AddItemAsync(item.Title, item.DateTime, item.Location, item.Description,item.Finder);
            else
                return await service.AddItemWithOwnerAsync(item.Title, item.DateTime, item.Location, item.Description,item.Finder, item.Owner, item.IsFound);
        }
    }
}
