using Client.Model;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Client.ViewModel
{
    class ItemAddViewModel : BindableBase
    {
        private DisplayItem item;
        public DisplayItem Item
        {
            get { return item; }
            set { SetProperty(ref item, value); OnPropertyChanged("IsAddAvailable"); }
        }


        public bool IsAddAvailable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Item.Title) || string.IsNullOrWhiteSpace(Item.Description))
                    return false;
                else
                    return !Item.HasErrors;

            }
        }
        public ItemAddViewModel()
        {
            Item = new DisplayItem() { DateTime = DateTime.Now };
            Item.PropertyChanged += new PropertyChangedEventHandler(SubPropertyChanged);
        }

        private void SubPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Item.Title) || e.PropertyName == nameof(Item.Description) || e.PropertyName == nameof(Item.DateTime) || e.PropertyName == nameof(Item.Location))
            {
                OnPropertyChanged(nameof(IsAddAvailable));
            }
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
