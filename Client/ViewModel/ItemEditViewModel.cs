using Client.Model;
using Client.Model.Interfaces;
using System.ComponentModel;

namespace Client.ViewModel
{
    class ItemEditViewModel : BindableBase
    {
        private DisplayItem item;
        private DisplayItem newItem;
        public DisplayItem Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }
        public DisplayItem NewItem
        {
            get { return newItem; }
            set { SetProperty(ref newItem, value); }
        }

        public bool IsEditAvailable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NewItem.Title) && string.IsNullOrWhiteSpace(NewItem.Description) && NewItem.Location == Item.Location)
                    return false;
                else if (NewItem.Title == Item.Title && NewItem.Location == Item.Location && NewItem.Description == Item.Description && NewItem.DateTime == Item.DateTime)
                    return false;
                return true;

            }
        }

        private void SubPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewItem.Title) || e.PropertyName == nameof(NewItem.Description) || e.PropertyName == nameof(Item.DateTime) || e.PropertyName == nameof(Item.Location))
            {
                OnPropertyChanged(nameof(IsEditAvailable));
            }
        }

        public ItemEditViewModel(IItem item)
        {
            Item = new DisplayItem() { DateTime = item.DateTime, Description = item.Description, Finder = item.Finder, ID = item.ID, Location = item.Location, Owner = item.Owner, Title = item.Title };
            NewItem = new DisplayItem() { DateTime = Item.DateTime, ID = item.ID, Finder = item.Finder, Owner = item.Owner };
            NewItem.PropertyChanged += new PropertyChangedEventHandler(SubPropertyChanged);
        }

    }
}
