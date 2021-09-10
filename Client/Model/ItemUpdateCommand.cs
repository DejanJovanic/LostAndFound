using Client.Model.Interfaces;
using System.Threading.Tasks;

namespace Client.Model
{
    class ItemUpdateCommand : IItemServiceCommand
    {
        IDataService service;
        IItem oldValue;
        IItem newValue;

        public ItemUpdateCommand(IDataService service, IItem oldValue, IItem newValue)
        {
            this.service = service;
            this.oldValue = (IItem)oldValue.Clone();
            this.newValue = (IItem)newValue.Clone();
        }

        public async Task<IItemReturnValue> ExecuteAsync()
        {
            return await service.UpdateItemAsync(oldValue, newValue);
        }

        public async Task<IItemReturnValue> UnexecuteAsync()
        {
            return await service.UpdateItemAsync(newValue, oldValue);
        }
    }
}
