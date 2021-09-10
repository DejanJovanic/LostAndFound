using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Client.Model
{
    public class ObservableCollectionFilterFriendly<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += Item_PropertyChanged;
        }

        protected override void RemoveItem(int index)
        {

            var item = this[index];
            base.RemoveItem(index);
            item.PropertyChanged -= Item_PropertyChanged;

        }

        protected override void SetItem(int index, T item)
        {

            var oldItem = this[index];
            oldItem.PropertyChanged -= Item_PropertyChanged;
            base.SetItem(index, item);
            item.PropertyChanged += Item_PropertyChanged;

        }

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // generate a "replace" event
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender));
            });

        }
    }
}
