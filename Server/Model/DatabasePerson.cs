using Server.Interfaces;
using System.Collections.Generic;

namespace Server.Model
{
    public class DatabasePerson : IDatabasePerson
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<DatabaseItem> FoundItems { get; set; }
        public virtual ICollection<DatabaseItem> OwnedItems { get; set; }

        public DatabasePerson()
        {
            IsAdmin = false;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Username)) return false;
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (string.IsNullOrWhiteSpace(LastName)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            return true;
        }
    }

    public enum DBResponseDatabasePerson
    {
        OK,
        USERNAMETAKEN,
        USERNOTFOUND,
        INVALIDRIGHT,
        BADUSERSUPPLIED
    }
}
