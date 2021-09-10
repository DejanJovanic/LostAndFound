using Client.DataServiceReference;
using Client.Model.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Model
{
    public class DisplayItem : ValidatableBindableBase, IItem
    {

        private int id;

        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private DateTime dateTime;
        [Required]

        public DateTime DateTime
        {
            get { return dateTime; }
            set { SetProperty(ref dateTime, value); }
        }

        private string description;
        [Required]
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private int location;
        [Required]
        public int Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        private bool isFound;

        public bool IsFound
        {
            get { return isFound; }
            set { SetProperty(ref isFound, value); }
        }
        private bool canBeOwner;

        public bool CanBeOwner
        {
            get { return canBeOwner; }
            set { SetProperty(ref canBeOwner, value); }
        }
        private string title;
        [Required]
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string finder;
        [Required]
        public string Finder
        {
            get { return finder; }
            set { SetProperty(ref finder, value); }
        }

        private string owner;

        public string Owner
        {
            get { return owner; }
            set { SetProperty(ref owner, value); }
        }

        public DisplayItem() { }

        public DisplayItem(LoginServiceReference.Item value)
        {
            id = value.ID;
            owner = value.Owner;
            finder = value.Finder;
            description = value.Description;
            title = value.Title;
            dateTime = value.Datetime;
            location = value.Location;
            isFound = value.IsFound;
        }
        public DisplayItem(Item value)
        {
            id = value.ID;
            owner = value.Owner;
            finder = value.Finder;
            description = value.Description;
            title = value.Title;
            dateTime = value.Datetime;
            location = value.Location;
            isFound = value.IsFound;

        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
