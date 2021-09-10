using System;

namespace Client.Model.Interfaces
{
    public interface IItemDataChange
    {
        event EventHandler<ItemUpdatedEventArgs> ItemUpdated;
        event EventHandler<ItemRemovedEventArgs> ItemDeleted;
        event EventHandler<ItemAddedEventArgs> ItemAdded;
    }

    public class ItemAddedEventArgs : EventArgs
    {
        public DisplayItem Item { get; set; }
    }
    public class ItemUpdatedEventArgs : EventArgs
    {
        public DisplayItem Item { get; set; }
    }
    public class ItemRemovedEventArgs : EventArgs
    {
        public int ID { get; set; }
    }



}
