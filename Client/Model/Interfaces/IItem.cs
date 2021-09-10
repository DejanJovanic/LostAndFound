using System;

namespace Client.Model.Interfaces
{
    public interface IItem : ICloneable
    {
        DateTime DateTime { get; set; }
        string Description { get; set; }
        string Finder { get; set; }
        int ID { get; set; }
        int Location { get; set; }
        bool IsFound { get; set; }
        bool CanBeOwner { get; set; }
        string Owner { get; set; }
        string Title { get; set; }
    }
}