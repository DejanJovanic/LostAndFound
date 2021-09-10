using Client.Model;
using Client.Model.Interfaces;
using Client.View;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class PersonsAdminViewModel : PersonsViewModel
    {
        public PersonsAdminViewModel(IPersonService service, IPersonDataChange dataChange, IUserData data,SnackbarMessageQueue queue) : base(service, dataChange, data,queue)
        {
            AddPersonCommand = new ViewCommandAsync(AddPerson);
            RemovePersonCommand = new ViewCommandAsync<DisplayPerson>(RemovePerson);
        }
        public IAsyncCommand AddPersonCommand { get; set; } 
        public IAsyncCommand<DisplayPerson> RemovePersonCommand { get; set; } 
        private async Task RemovePerson(DisplayPerson person)
        {
            var res = await service.RemovePersonAsync(person.Username);
            switch (res)
            {
                case PersonReturnCode.OK:
                    {
                        SnackbarQueue.Enqueue("Person successfully removed");
                        return;
                    }
                case PersonReturnCode.INVALIDRIGHT:
                    {
                        SnackbarQueue.Enqueue("You are not admin");
                        return;
                    }
                case PersonReturnCode.BADUSERSUPPLIED:
                    {
                        SnackbarQueue.Enqueue("Invalid details specified");
                        return;
                    }
            }
        }


        private async Task AddPerson()
        {
            var view = new PersonAddView()
            {
                DataContext = new PersonAddViewModel()
            };

            var result = await DialogHost.Show(view, "MainDialog");

            if (result != null)
            {
                var temp = (DisplayPersonPassword)result;
                var ret = await service.AddPersonAsync(new DisplayPerson() { IsAdmin = temp.IsAdmin, LastName = temp.LastName, Name = temp.Name, Username = temp.Username }, temp.Password);
                switch (ret)
                {
                    case PersonReturnCode.OK:
                        {
                            SnackbarQueue.Enqueue("Item successfully added");
                            return;
                        }
                    case PersonReturnCode.INVALIDRIGHT:
                        {
                            SnackbarQueue.Enqueue("You are not admin");
                            return;
                        }
                    case PersonReturnCode.USERNAMETAKEN:
                        {
                            SnackbarQueue.Enqueue("Username is already taken");
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
