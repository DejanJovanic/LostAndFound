using Server.Model;

namespace Server.Interfaces
{
    public interface IUpdateValue
    {
        Item OldValue { get; set; }
        Item NewValue { get; set; }
    }
}
