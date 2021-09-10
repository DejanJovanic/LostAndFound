using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "UserResponse")]
    [Serializable]
    public class UserResponse : IUserResponse
    {
        [DataMember]
        public ResponseCode ResponseCode { get; set; }

        public UserResponse() { }
    }
}
