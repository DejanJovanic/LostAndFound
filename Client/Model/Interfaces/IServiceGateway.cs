using System;

namespace Client.Model.Interfaces
{
    interface IServiceGateway : IDisposable, IDataService, IPersonService, ILoginUpdateService, IDataChangeHandler
    {
    }
}
