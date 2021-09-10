using Server.Model;
using System.Collections.Generic;

namespace Server.Interfaces
{
    public interface IDatabasePerson
    {
        ICollection<DatabaseItem> FoundItems { get; set; }
        ICollection<DatabaseItem> OwnedItems { get; set; }
        int ID { get; set; }
        bool IsAdmin { get; set; }
        string LastName { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string Username { get; set; }

        bool IsValid();
    }
}