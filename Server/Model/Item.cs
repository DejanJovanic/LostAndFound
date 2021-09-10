using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "Item")]
    [Serializable]
    public class Item : IValue
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime Datetime { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Location { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public bool IsFound { get; set; }
        [DataMember]
        public string Finder { get; set; }
        [DataMember]
        public string Owner { get; set; }

        public Item()
        {

        }
        public bool CheckFields()
        {
            if (string.IsNullOrWhiteSpace(Description) || Location < 0 || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Finder) || Datetime < new DateTime(1753, 1, 2))
                return false;
            else return true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
