using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface IItemServiceCommand
    {
        Task<IItemReturnValue> ExecuteAsync();
        Task<IItemReturnValue> UnexecuteAsync();
    }
}
