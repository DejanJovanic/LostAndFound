namespace Client.Model.Interfaces
{
    interface IDataChangeHandler : IPersonDataChange, IItemDataChange
    {

        void TriggerAddItemEvent(DisplayItem item);
        void TriggerUpdateItemEvent(DisplayItem item);
        void TriggerRemoveItemEvent(int id);
        void TriggerAddPersonEvent(DisplayPerson person);
        void TriggerUpdatePersonEvent(DisplayPerson person);
        void TriggerRemovePersonEvent(string username);
    }
}
