using System.ComponentModel;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface IItemUndoRedo : INotifyPropertyChanged
    {
        bool IsRedoAvailable { get; }
        bool IsUndoAvailable { get; }
        Task<IItemReturnValue> UndoAsync();
        Task<IItemReturnValue> RedoAsync();
    }
}
