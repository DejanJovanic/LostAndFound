using Server.Model;
using System.Runtime.Serialization;

namespace Server.Interfaces
{
    public interface IItemOperationReturnValue
    {
        Item DatabaseValue { get; set; }
        Item SubmittedValue { get; set; }
        Status Status { get; set; }
    }

    [DataContract]
    public enum Status
    {
        [EnumMember]
        OK,
        [EnumMember]
        INVALIDDATA,
        [EnumMember]
        CONFLICT,
        [EnumMember]
        NOTLOGGEDIN
    }
}
