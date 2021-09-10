using Client.Model.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Model
{
    class ItemDuplicateCommand : IItemServiceCommand
    {
        private List<IItem> items;
        private List<IItem> returnValues;
        private IDataService service;

        public ItemDuplicateCommand(IDataService service, List<IItem> items)
        {
            this.items = new List<IItem>();
            returnValues = new List<IItem>();
            foreach (var a in items)
            {
                this.items.Add(a.Clone() as IItem);
                this.service = service;
            }
        }

        public async Task<IItemReturnValue> ExecuteAsync()
        {
            foreach (var a in items)
            {
                IItemReturnValue ret = null;
                if (string.IsNullOrWhiteSpace(a.Owner) && !a.IsFound)
                    ret = await service.AddItemAsync(a.Title, a.DateTime, a.Location, a.Description,a.Finder);
                else
                    ret = await service.AddItemWithOwnerAsync(a.Title, a.DateTime, a.Location, a.Description,a.Finder, a.Owner, a.IsFound);
                returnValues.Add(ret.DatabaseValue.Clone() as IItem);
            }
            return new ItemReturnValue() { DatabaseValue = null, Response = Response.OK, SubmitedValue = null };
        }

        public async Task<IItemReturnValue> UnexecuteAsync()
        {
            foreach (var a in returnValues)
            {
                await service.RemoveItemAsync(a);
            }
            return new ItemReturnValue() { DatabaseValue = null, Response = Response.OK, SubmitedValue = null };
        }
    }
}
