using Server.Model;
using System;

namespace Server.Interfaces
{
    public interface IDatabaseValue
    {
        int ID { get; set; }
        DateTime Datetime { get; set; }
        string Description { get; set; }
        int Location { get; set; }
        string Title { get; set; }
        bool IsFound { get; set; }
        DatabasePerson Finder { get; set; }
        DatabasePerson Owner { get; set; }
    }

    public static class IDatabaseValueEx
    {
        public static bool IsEqual(this IDatabaseValue value1, IValue value2)
        {
            if (value1 != null && value2 != null)
            {
                if (value1.ID != value2.ID) return false;
                if (value1.Datetime.Date != value2.Datetime.Date) return false;
                if (value1.Description != value2.Description) return false;
                if (value1.Location != value2.Location) return false;
                if (value1.Title != value2.Title) return false;
                if (value1.Finder.Username != value2.Finder) return false;
                if (value1.Owner == null && value2.Owner != null)
                    return false;
                else if (value1.Owner != null && value2.Owner == null)
                    return false;
                else if (value1.Owner != null && value2.Owner != null)
                    if (value1.Owner.Username != value2.Owner) return false;
            }
            else return false;

            return true;
        }

        public static bool IsEqual(this IDatabaseValue value1, IDatabaseValue value2)
        {
            if (value1 != null && value2 != null)
            {
                if (value1.ID != value2.ID) return false;
                if (value1.Datetime != value2.Datetime) return false;
                if (value1.Description != value2.Description) return false;
                if (value1.Location != value2.Location) return false;
                if (value1.Title != value2.Title) return false;
                if (value1.Finder.Username != value2.Finder.Username) return false;
                if (value1.Owner == null && value2.Owner != null)
                    return false;
                else if (value1.Owner != null && value2.Owner == null)
                    return false;
                else if (value1.Owner != null && value2.Owner != null)
                    if (value1.Owner.Username != value2.Owner.Username) return false;

            }
            else return false;

            return true;
        }
    }

    public enum DBResponseDatabaseData
    {
        OK,
        CONFLICT,
        INVALIDDATA
    }
}
