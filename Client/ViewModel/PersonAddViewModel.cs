using Client.Model;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Client.ViewModel
{
    class PersonAddViewModel : BindableBase
    {

        private DisplayPersonPassword person;
        public DisplayPersonPassword Person
        {
            get { return person; }
            set { SetProperty(ref person, value); OnPropertyChanged("IsAddAvailable"); }
        }


        public bool IsAddAvailable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Person.Username) || string.IsNullOrWhiteSpace(Person.Password) || string.IsNullOrWhiteSpace(Person.Name) || string.IsNullOrWhiteSpace(Person.LastName))
                    return false;
                else
                    return !Person.HasErrors;

            }
        }
        public PersonAddViewModel()
        {
            Person = new DisplayPersonPassword();
            Person.PropertyChanged += new PropertyChangedEventHandler(SubPropertyChanged);
        }

        private void SubPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Person.Username) || e.PropertyName == nameof(Person.Password) || e.PropertyName == nameof(Person.Name) || e.PropertyName == nameof(Person.LastName) || e.PropertyName == nameof(Person.IsAdmin))
            {
                OnPropertyChanged(nameof(IsAddAvailable));
            }
        }
    }
    public class BoolToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0) ? true : false;
        }
    }
}

