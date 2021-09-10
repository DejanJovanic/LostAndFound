using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "Delta")]
    [Serializable]
    public class ItemDelta : IUpdateValue
    {
        [DataMember]
        public Item OldValue { get; set; }
        [DataMember]
        public Item NewValue { get; set; }

        public ItemDelta() { }
    }
}
