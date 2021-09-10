using Server.Model;
using System;
using System.ServiceModel;

namespace Server.Interfaces
{
    [ServiceKnownType(typeof(Item))]
    public interface IValue : ICloneable
    {
        int ID { get; set; }
        DateTime Datetime { get; set; }
        string Description { get; set; }
        int Location { get; set; }
        string Title { get; set; }
        bool IsFound { get; set; }
        string Finder { get; set; }
        string Owner { get; set; }
        bool CheckFields();
    }

    public static class IValueEqual
    {
        public static bool IsEqual(this IValue value1, IValue value2)
        {
            if (value1 != null && value2 != null)
            {
                if (value1.ID != value2.ID) return false;
                if (value1.Datetime != value2.Datetime) return false;
                if (value1.Description != value2.Description) return false;
                if (value1.Location != value2.Location) return false;
                if (value1.Title != value2.Title) return false;
                if (value1.Finder != value2.Finder) return false;
                if (value1.Owner != value2.Owner) return false;

            }
            else return false;

            return true;
        }

        public static bool IsEqual(this IValue value1, IDatabaseValue value2)
        {
            if (value1 != null && value2 != null)
            {
                if (value1.ID != value2.ID) return false;
                if (value1.Datetime != value2.Datetime) return false;
                if (value1.Description != value2.Description) return false;
                if (value1.Location != value2.Location) return false;
                if (value1.Title != value2.Title) return false;
                if (value1.Finder != value2.Finder.Username) return false;
                if (value1.Owner == null && value2.Owner != null)
                    return false;
                else if (value1.Owner != null && value2.Owner == null)
                    return false;
                else if (value1.Owner != null && value2.Owner != null)
                    if (value1.Owner != value2.Owner.Username) return false;

            }
            else return false;

            return true;
        }
    }
}
