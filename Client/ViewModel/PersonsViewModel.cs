using Client.Model;
using Client.Model.Interfaces;
using Client.View;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Client.ViewModel
{
    class PersonsViewModel : BindableBase
    {
        protected IPersonService service;
        protected IUserData data;
        protected object lockPersons;
        public SnackbarMessageQueue SnackbarQueue { get; set; }
        public ObservableCollectionFilterFriendly<DisplayPerson> DisplayPersons { get; set; }
        public ICollectionView Persons { get; set; }
        public PersonsViewModel(IPersonService service, IPersonDataChange dataChange, IUserData data,SnackbarMessageQueue queue)
        {
            SnackbarQueue = new SnackbarMessageQueue();
            this.service = service;
            this.data = data;
            SnackbarQueue = queue;
            lockPersons = new object();
            DisplayPersons = new ObservableCollectionFilterFriendly<DisplayPerson>();
            var persons = service.GetPersonsList();
            BindingOperations.EnableCollectionSynchronization(DisplayPersons, lockPersons);
            if (persons != null)
            {
                foreach (var a in persons)
                {
                    lock (lockPersons)
                    {
                        DisplayPersons.Add(a);
                    }
                }
            }
            Persons = (CollectionView)CollectionViewSource.GetDefaultView(DisplayPersons);

            dataChange.PersonAdded += (o, e) =>
            {

                foreach (var a in DisplayPersons)
                {
                    if (a.Username == e.Person.Username)
                    {
                        //greska
                        return;
                    }
                }
                lock (lockPersons)
                {
                    DisplayPersons.Add(e.Person);
                }
            };
            dataChange.PersonDeleted += (o, e) =>
            {

                for (int i = 0; i < DisplayPersons.Count; i++)
                {
                    if (DisplayPersons[i].Username == e.Username)
                    {
                        lock (lockPersons)
                        {
                            DisplayPersons.RemoveAt(i);
                        }
                        return;
                    }
                }
            };
            dataChange.PersonUpdated += (o, e) =>
            {
                for (int i = 0; i < DisplayPersons.Count; i++)
                {
                    if (DisplayPersons[i].Username == e.Person.Username)
                    {
                        lock (lockPersons)
                        {
                            DisplayPersons.RemoveAt(i);
                            DisplayPersons.Add(e.Person);
                        }
                        return;
                    }
                }
            };
            EditPersonCommand = new ViewCommandAsync(EditPerson);
        }
        public IAsyncCommand EditPersonCommand { get; set; }

        private async Task EditPerson()
        {
            DisplayPerson person = null;
            foreach (var a in DisplayPersons)
            {
                if (a.Username == data.Username)
                {
                    person = new DisplayPerson() { Username = a.Username, IsAdmin = a.IsAdmin, LastName = a.LastName, Name = a.Name };
                    break;
                }
            }
            if (person != null)
            {
                var view = new PersonEditView()
                {
                    DataContext = new PersonEditViewModel(person)
                };
                var result = await DialogHost.Show(view, "MainDialog");
                if (result != null)
                {
                    var temp = (DisplayPerson)result;
                    if (string.IsNullOrWhiteSpace(temp.Name)) temp.Name = person.Name;
                    if (string.IsNullOrWhiteSpace(temp.LastName)) temp.LastName = person.LastName;
                    var res = await service.UpdatePersonAsync(temp.Name, temp.LastName);
                    switch (res)
                    {
                        case PersonReturnCode.OK:
                            {
                                SnackbarQueue.Enqueue("Account successfully updated");
                                return;
                            }
                        case PersonReturnCode.BADUSERSUPPLIED:
                            {
                                SnackbarQueue.Enqueue("Invalid details specified");
                                return;
                            }
                    }
                }
            }
        }
    }
    public class BoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return "Admin";
            else
                return "Regular user";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
