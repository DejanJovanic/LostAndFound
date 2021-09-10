using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "UserDelta")]
    [Serializable]
    public class UserDelta : IUpdateUser
    {
        [DataMember]
        public Person OldValue { get; set; }
        [DataMember]
        public Person NewValue { get; set; }

        public UserDelta() { }
    }
}
