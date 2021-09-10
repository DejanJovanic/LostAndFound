using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "Person")]
    [Serializable]
    public class Person : IUser
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }

        public Person()
        {
            IsAdmin = false;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Username)) return false;
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (string.IsNullOrWhiteSpace(LastName)) return false;
            return true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
