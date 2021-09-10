using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Model
{
    class DataServiceProxy : BindableBase, IDataService, IItemUndoRedo
    {
        CommandMachine commandMachine;
        IDataService service;
        public DataServiceProxy(CommandMachine commandMachine, IDataService service)
        {
            this.service = service;
            this.commandMachine = commandMachine;
            commandMachine.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(commandMachine.IsRedoAvailable))
                    OnPropertyChanged("IsRedoAvailable");
                else if (e.PropertyName == nameof(commandMachine.IsUndoAvailable))
                    OnPropertyChanged("IsUndoAvailable");
            };
        }

        public bool IsRedoAvailable { get { return commandMachine.IsRedoAvailable; } }

        public bool IsUndoAvailable { get {return commandMachine.IsUndoAvailable; } }

        public async Task<IItemReturnValue> AddItemAsync(string title, DateTime dateTime, int location, string description,string finder)
        {
            var command = new ItemAddCommand(service, title, location, description, dateTime,finder);
            return await commandMachine.Do(command);
        }

        public async Task<IItemReturnValue> AddItemWithOwnerAsync(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound)
        {
            var command = new ItemAddWithOwnerCommand(service, title, location, description, dateTime,finder, owner, isFound);
            return await commandMachine.Do(command);
        }

        public async Task<IItemReturnValue> RedoAsync()
        {
            return await commandMachine.Redo();
        }

        public async Task<IItemReturnValue> RemoveItemAsync(IItem item)
        {
            var command = new ItemDeleteCommand(service, item);
            return await commandMachine.Do(command);
        }

        public async Task<IItemReturnValue> UndoAsync()
        {
            return await commandMachine.Undo();
        }

        public async Task<IItemReturnValue> UpdateItemAsync(IItem oldItem, IItem newItem)
        {
            var command = new ItemUpdateCommand(service, oldItem, newItem);
            return await commandMachine.Do(command);
        }

        public ICollection<DisplayItem> GetItemsList()
        {
            return service.GetItemsList();
        }

    }
}
