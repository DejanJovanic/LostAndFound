using Client.Model.Interfaces;
using System;

namespace Client.Model
{
    public class DataChange : IDataChangeHandler
    {

        public event EventHandler<ItemUpdatedEventArgs> ItemUpdated;
        public event EventHandler<ItemRemovedEventArgs> ItemDeleted;
        public event EventHandler<ItemAddedEventArgs> ItemAdded;
        public event EventHandler<PersonUpdatedEventArgs> PersonUpdated;
        public event EventHandler<PersonRemovedEventArgs> PersonDeleted;
        public event EventHandler<PersonAddedEventArgs> PersonAdded;



        public void TriggerAddItemEvent(DisplayItem item)
        {
            ItemAdded?.Invoke(this, new ItemAddedEventArgs() { Item = item });
        }

        public void TriggerUpdateItemEvent(DisplayItem item)
        {
            ItemUpdated?.Invoke(this, new ItemUpdatedEventArgs() { Item = item });
        }

        public void TriggerRemoveItemEvent(int id)
        {
            ItemDeleted?.Invoke(this, new ItemRemovedEventArgs() { ID = id });
        }

        public void TriggerAddPersonEvent(DisplayPerson person)
        {
            PersonAdded?.Invoke(this, new PersonAddedEventArgs() { Person = person });
        }

        public void TriggerUpdatePersonEvent(DisplayPerson person)
        {
            PersonUpdated?.Invoke(this, new PersonUpdatedEventArgs() { Person = person });
        }

        public void TriggerRemovePersonEvent(string username)
        {
            PersonDeleted?.Invoke(this, new PersonRemovedEventArgs() { Username = username });
        }
    }
}
