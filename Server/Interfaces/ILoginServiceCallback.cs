using Server.Model;
using System.ServiceModel;

namespace Server.Interfaces
{
    [ServiceContract]
    [ServiceKnownType(typeof(ItemDelta))]
    [ServiceKnownType(typeof(ItemOperationReturnValue))]
    [ServiceKnownType(typeof(Item))]
    [ServiceKnownType(typeof(UserDelta))]
    public interface ILoginServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateItemData(ItemDelta item);

        [OperationContract(IsOneWay = true)]
        void UpdateUserData(UserDelta item);

        [OperationContract(IsOneWay = true)]
        void NotifySuccessfulLogin(Person userDetails);

        [OperationContract(IsOneWay = true)]
        void NotifyFailedLogin(string reason);
        [OperationContract(IsOneWay = true)]
        void NotifySuccessfulLogout();
    }
}
