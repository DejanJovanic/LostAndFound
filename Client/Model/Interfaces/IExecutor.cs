using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface IExecutor
    {
        bool IsRedoAvailable { get; }
        bool IsUndoAvailable { get; }

        Task<IItemReturnValue> Undo();
        Task<IItemReturnValue> Do(IItemServiceCommand command);
        Task<IItemReturnValue> Redo();
    }
}