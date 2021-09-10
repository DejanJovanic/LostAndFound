using System.Runtime.Serialization;

namespace Server.Interfaces
{
    public interface IUserResponse
    {
        ResponseCode ResponseCode { get; set; }
    }

    [DataContract]
    public enum ResponseCode
    {
        [EnumMember]
        OK,
        [EnumMember]
        USERNAMETAKEN,
        [EnumMember]
        USERNOTFOUND,
        [EnumMember]
        INVALIDRIGHT,
        [EnumMember]
        BADUSERSUPPLIED,
        [EnumMember]
        NOTLOGGEDIN
    }
}
