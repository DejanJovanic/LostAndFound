using Server.Model;

namespace Server.Interfaces
{
    interface IUpdateUser
    {
        Person OldValue { get; set; }
        Person NewValue { get; set; }
    }
}
