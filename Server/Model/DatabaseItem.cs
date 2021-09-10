using Server.Interfaces;
using Server.Model;
using System;

namespace Server.Model
{
    public class DatabaseItem : IDatabaseValue
    {
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public string Description { get; set; }
        public int Location { get; set; }
        public string Title { get; set; }
        public bool IsFound { get; set; }
        public virtual DatabasePerson Finder { get; set; }
        public virtual DatabasePerson Owner { get; set; }

        public DatabaseItem()
        {
            IsFound = false;
        }
    }
}
