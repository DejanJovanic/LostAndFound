using System.ServiceModel;

namespace Server.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ILoginServiceCallback), SessionMode = SessionMode.Required, Name = "ILoginService")]

    public interface ILoginService
    {
        [OperationContract(IsOneWay = true)]
        void Login();

        [OperationContract(IsOneWay = true)]
        void Logout();

    }
}
