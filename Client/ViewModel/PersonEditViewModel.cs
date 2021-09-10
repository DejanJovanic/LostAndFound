using Client.Model;
using Client.Model.Interfaces;
using System.ComponentModel;

namespace Client.ViewModel
{
    class PersonEditViewModel : BindableBase
    {
        private DisplayPerson person;
        private DisplayPerson newPerson;
        public DisplayPerson Person
        {
            get { return person; }
            set { SetProperty(ref person, value); }
        }
        public DisplayPerson NewPerson
        {
            get { return newPerson; }
            set { SetProperty(ref newPerson, value); }
        }

        public bool IsEditAvailable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NewPerson.Name) && string.IsNullOrWhiteSpace(NewPerson.LastName))
                    return false;
                else if (NewPerson.Name == Person.Name && NewPerson.LastName == Person.LastName)
                    return false;
                return true;

            }
        }

        private void SubPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewPerson.Name) || e.PropertyName == nameof(NewPerson.LastName))
            {
                OnPropertyChanged(nameof(IsEditAvailable));
            }
        }

        public PersonEditViewModel(IPerson person)
        {
            Person = new DisplayPerson() { Name = person.Name, LastName = person.LastName };
            NewPerson = new DisplayPerson();
            NewPerson.PropertyChanged += new PropertyChangedEventHandler(SubPropertyChanged);
        }

    }
}
