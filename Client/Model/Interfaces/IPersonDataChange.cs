using System;

namespace Client.Model.Interfaces
{
    interface IPersonDataChange
    {
        event EventHandler<PersonUpdatedEventArgs> PersonUpdated;
        event EventHandler<PersonRemovedEventArgs> PersonDeleted;
        event EventHandler<PersonAddedEventArgs> PersonAdded;
    }

    public class PersonAddedEventArgs : EventArgs
    {
        public DisplayPerson Person { get; set; }
    }
    public class PersonUpdatedEventArgs : EventArgs
    {
        public DisplayPerson Person { get; set; }
    }
    public class PersonRemovedEventArgs : EventArgs
    {
        public string Username { get; set; }
    }
}
