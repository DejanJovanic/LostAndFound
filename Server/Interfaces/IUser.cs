using System;

namespace Server.Interfaces
{

    public interface IUser : ICloneable
    {
        string Name { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        bool IsAdmin { get; set; }
        bool IsValid();
    }

    public static class IUserEx
    {
        public static bool IsEqual(this IUser user1, IUser user2)
        {
            if (user1 != null && user2 != null)
            {
                if (user1.Name != user2.Name) return false;
                if (user1.LastName != user2.LastName) return false;
                if (user1.Username != user2.Username) return false;
                if (user1.IsAdmin != user2.IsAdmin) return false;
            }
            else return false;

            return true;

        }
    }
}
