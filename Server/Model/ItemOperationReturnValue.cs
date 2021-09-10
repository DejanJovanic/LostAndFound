using Server.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract(Name = "ItemOperationReturnValue")]
    [Serializable]
    public class ItemOperationReturnValue : IItemOperationReturnValue
    {
        [DataMember]
        public Item DatabaseValue { get; set; }
        [DataMember]
        public Item SubmittedValue { get; set; }
        [DataMember]
        public Status Status { get; set; }

        public ItemOperationReturnValue() { }
    }
}
