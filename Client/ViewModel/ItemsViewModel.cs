
using Client.Model;
using Client.Model.Interfaces;
using Client.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WPFCustomMessageBox;

namespace Client.ViewModel
{
    class ItemsViewModel : BindableBase
    {
        public ObservableCollectionFilterFriendly<DisplayItem> DisplayItems { get; set; }
        private IDataService service;
        private IItemUndoRedo undoRedo;
        private object lockItems;
        private IUserData userData;
        public ICollectionView Items { get; set; }
        public string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        public int location;
        public int Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }
        public DateTime datetime;
        public DateTime Datetime
        {
            get { return datetime; }
            set { SetProperty(ref datetime, value); }
        }
        public bool isCheckedLocation;
        public bool IsCheckedLocation
        {
            get { return isCheckedLocation; }
            set { SetProperty(ref isCheckedLocation, value); }
        }
        public bool isCheckedDate;
        public bool IsCheckedDate
        {
            get { return isCheckedDate; }
            set { SetProperty(ref isCheckedDate, value); }
        }
        public bool IsRedoAvailable
        {
            get
            {
                return undoRedo.IsRedoAvailable;
            }
        }
        public bool IsUndoAvailable
        {
            get
            {
                return undoRedo.IsUndoAvailable;
            }
        }
        public SnackbarMessageQueue SnackbarQueue { get; set; }
        public ItemsViewModel(IDataService service, IItemUndoRedo undoRedo, IItemDataChange data, IUserData userData,SnackbarMessageQueue queue)
        {
            DisplayItems = new ObservableCollectionFilterFriendly<DisplayItem>();
            this.service = service;
            lockItems = new object();
            this.undoRedo = undoRedo;
            this.userData = userData;
            SnackbarQueue = queue;
            this.undoRedo.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(this.undoRedo.IsRedoAvailable))
                    OnPropertyChanged("IsRedoAvailable");
                else if (e.PropertyName == nameof(this.undoRedo.IsUndoAvailable))
                    OnPropertyChanged("IsUndoAvailable");
            };
            var items = service.GetItemsList();
            BindingOperations.EnableCollectionSynchronization(DisplayItems, lockItems);

            if (items != null)
            {
                foreach (var a in items)
                {
                    lock (lockItems)
                    {
                        DisplayItems.Add(a);
                    }

                }
            }

            Items = (CollectionView)CollectionViewSource.GetDefaultView(DisplayItems);
            data.ItemAdded += (o, e) =>
            {

                foreach (var a in DisplayItems)
                {
                    if (a.ID == e.Item.ID)
                    {
                        //greska
                        return;
                    }
                }
                lock (lockItems)
                {
                    DisplayItems.Add(e.Item);
                }

            };
            data.ItemDeleted += (o, e) =>
            {

                for (int i = 0; i < DisplayItems.Count; i++)
                {
                    if (DisplayItems[i].ID == e.ID)
                    {
                        lock (lockItems)
                        {
                            DisplayItems.RemoveAt(i);
                        }
                        return;
                    }
                }

            };
            data.ItemUpdated += (o, e) =>
            {
                for (int i = 0; i < DisplayItems.Count; i++)
                {
                    if (DisplayItems[i].ID == e.Item.ID)
                    {
                        lock (lockItems)
                        {
                            DisplayItems.RemoveAt(i);
                            DisplayItems.Add(e.Item);
                        }
                        return;
                    }

                }
            };
            AddItemCommand = new ViewCommandAsync(AddCommand);
            RemoveItemCommand = new ViewCommandAsync<DisplayItem>(RemoveCommand);
            FoundItemCommand = new ViewCommandAsync<DisplayItem>(FoundCommand);
            EditItemCommand = new ViewCommandAsync<DisplayItem>(EditCommand);
            UndoItemCommand = new ViewCommandAsync(UndoCommand);
            RedoItemCommand = new ViewCommandAsync(RedoCommand);
            DuplicateItemCommand = new ViewCommandAsync<DisplayItem>(DuplicateCommand);
            SearchItemCommand = new ViewCommand(SearchCommand);
        }


        public IAsyncCommand AddItemCommand { get; set; }
        public IAsyncCommand<DisplayItem> RemoveItemCommand { get; set; }
        public IAsyncCommand<DisplayItem> FoundItemCommand { get; set; }
        public IAsyncCommand<DisplayItem> EditItemCommand { get; set; }
        public IAsyncCommand UndoItemCommand { get; set; }
        public IAsyncCommand RedoItemCommand { get; set; }
        public IAsyncCommand<DisplayItem> DuplicateItemCommand { get; set; }
        public ICommand SearchItemCommand { get; set; }
        private void SearchCommand()
        {
            Items.Filter = null;
            Items.Filter = i =>
            {
                var item = i as DisplayItem;
                var title = string.IsNullOrWhiteSpace(Title) ? true : item.Title.ToLower().Contains(Title.ToLower());
                var description = string.IsNullOrWhiteSpace(Description) ? true : item.Description.ToLower().Contains(Description.ToLower());
                var location = !IsCheckedLocation ? true : item.Location == Location;
                var date = !IsCheckedDate ? true : item.DateTime.Date == Datetime.Date;
                return title && description && location && date;


            };
            Items.Refresh();
        }
        private async Task DuplicateCommand(DisplayItem item)
        {
            var itemCopy = item.Clone() as DisplayItem;
            IItemReturnValue ret = null;
            if (itemCopy.IsFound)
                ret = await service.AddItemWithOwnerAsync(itemCopy.Title, itemCopy.DateTime, itemCopy.Location, itemCopy.Description, itemCopy.Finder, itemCopy.Owner, itemCopy.IsFound);
            else
                ret =  await service.AddItemAsync(itemCopy.Title, itemCopy.DateTime, itemCopy.Location, itemCopy.Description, itemCopy.Finder);
                switch (ret.Response)
                {
                    case Response.OK:
                        {
                            SnackbarQueue.Enqueue("Item successfully duplicated");
                            return;
                        }

                    case Response.INVALIDDATA: SnackbarQueue.Enqueue("Invalid date sent to service"); return;
                }    
        }
        private async Task FoundCommand(DisplayItem item)
        {
            var itemCopy = item.Clone() as DisplayItem;

            var foundItem = item.Clone() as DisplayItem;
            foundItem.Owner = userData.Username;
            foundItem.IsFound = true;
            await Edit(itemCopy, foundItem);

        }
        private async Task RemoveCommand(DisplayItem item)
        {
            var ret = await service.RemoveItemAsync(item);

            switch (ret.Response)
            {
                case Response.OK:
                    {
                        SnackbarQueue.Enqueue("Item successfully removed");
                        break;
                    }
                case Response.INVALIDDATA:
                    {
                        SnackbarQueue.Enqueue("Invalid item submited for removal");
                        break;
                    }
                case Response.CONFLICT:
                    {
                        ResolveConflict(ret);
                        break;
                    }
            }
        }

        private async Task EditCommand(DisplayItem item)
        {
            var itemCopy = item.Clone() as DisplayItem;
            var view = new ItemEditView()
            {
                DataContext = new ItemEditViewModel(itemCopy)
            };
            var result = await DialogHost.Show(view, "MainDialog");

            if (result != null)
            {
                var itemEdited = result as DisplayItem;
                if (string.IsNullOrWhiteSpace(itemEdited.Description)) itemEdited.Description = itemCopy.Description;
                if (string.IsNullOrWhiteSpace(itemEdited.Title)) itemEdited.Title = itemCopy.Title;
                await Edit(itemCopy, itemEdited);
            }
        }

        private async Task Edit(IItem oldItem, IItem newItem)
        {
            await Task.Factory.StartNew(async param =>
            {
                var paramArray = (object[])param;
                var res = await service.UpdateItemAsync((IItem)paramArray[0], (IItem)paramArray[1]);
                switch (res.Response)
                {
                    case Response.OK:
                        {
                            SnackbarQueue.Enqueue("Item successfully updated");
                            return;
                        }
                    case Response.INVALIDDATA:
                        {
                            SnackbarQueue.Enqueue("Invalid data provided");
                            return;
                        }
                    case Response.CONFLICT:
                        {
                            ResolveConflict(res);
                            break;
                        }

                }
            }, new object[] { oldItem, newItem });
        }

        private async Task UndoCommand()
        {
            var ret = await undoRedo.UndoAsync();
            if (ret != null && ret.Response == Response.CONFLICT)
                ResolveConflict(ret);
        }
        private async Task RedoCommand()
        {
            var ret = await undoRedo.RedoAsync();
            if (ret != null && ret.Response == Response.CONFLICT)
                ResolveConflict(ret);
        }

        private void ResolveConflict(IItemReturnValue value)
        {
            if (value.SubmitedValue == null)
            {
                MessageBoxResult res = MessageBoxResult.Cancel;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    res = CustomMessageBox.ShowYesNo("You are trying to remove item that someone has updated. What to do?",
                              "Remove Conflict", "Remove item", "Keep item");
                });
                if (res == MessageBoxResult.Yes)
                {
                    Task.Factory.StartNew(async i =>
                    {
                        await RemoveCommand((DisplayItem)i);
                    }, value.DatabaseValue);

                }
            }
            else
            {
                Task.Factory.StartNew(async i =>
                {
                    var res = i as IItemReturnValue;
                    if (res.DatabaseValue == null)
                    {
                        MessageBoxResult resBox = MessageBoxResult.Cancel;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            resBox = CustomMessageBox.ShowYesNo("You are trying to update item that someone has deleted. What to do?",
                           "Update Conflict", "Update item", "Do nothing");
                        });

                        if (resBox == MessageBoxResult.Yes)
                        {
                            if (string.IsNullOrWhiteSpace(res.SubmitedValue.Owner))
                                await service.AddItemAsync(res.SubmitedValue.Title, res.SubmitedValue.DateTime, res.SubmitedValue.Location, res.SubmitedValue.Description,userData.Username);
                            else
                                await service.AddItemWithOwnerAsync(res.SubmitedValue.Title, res.SubmitedValue.DateTime, res.SubmitedValue.Location, res.SubmitedValue.Description,res.SubmitedValue.Finder ,res.SubmitedValue.Owner, res.SubmitedValue.IsFound);
                        }
                    }
                    else
                    {
                        MessageBoxResult resBox = MessageBoxResult.Cancel;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            resBox = CustomMessageBox.ShowYesNo("You are trying to update item that someone else has changed .What to do?",
                   "Update Conflict", "Update item", "Keep item");
                        });
                        if (resBox == MessageBoxResult.Yes)
                            await Task.Factory.StartNew(async param =>
                            {
                                var val = param as IItemReturnValue;
                                await Edit(val.DatabaseValue, val.SubmitedValue);
                            }, res);
                    }
                }, value);

            }
        }

        private async Task AddCommand()
        {
           
            var view = new ItemPreviewView
            {
                DataContext = new ItemAddViewModel()
            };

            var result = await DialogHost.Show(view, "MainDialog");

            if (result != null)
            {
                var item = result as DisplayItem;
                var ret = await service.AddItemAsync(item.Title, item.DateTime, item.Location, item.Description,userData.Username);
                switch (ret.Response)
                {
                    case Response.OK:
                        {
                        SnackbarQueue.Enqueue("Item successfully added");
                            return;
                        } 

                    case Response.INVALIDDATA: SnackbarQueue.Enqueue("Invalid date sent to service"); return;
                }
            }
        }

    }
    public class BoolToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && !(bool)value)
                return new BrushConverter().ConvertFromString("#b71c1c");
            else
                return new BrushConverter().ConvertFromString("#1b5e20");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
