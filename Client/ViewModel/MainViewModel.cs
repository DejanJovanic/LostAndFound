using Client.Model;
using Client.Model.Interfaces;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModel
{
    class MainViewModel : BindableBase
    {
        private IServiceGateway service;
        private BindableBase itemsViewModel;
        private BindableBase personsViewModel;
        private BindableBase currentViewModel;
        private BindableBase logViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }
        public MainViewModel(IServiceGateway service, IUserData userData, bool isAdmin,SnackbarMessageQueue queue)
        {
            this.service = service;
            var commandMachine = new CommandMachine();
            var proxy = new DataServiceProxy(commandMachine, service);
            itemsViewModel = new ItemsViewModel(proxy, proxy, service, userData,queue);
            personsViewModel = isAdmin ? new PersonsAdminViewModel(service, service, userData,queue) : new PersonsViewModel(service, service, userData,queue);
            logViewModel = new LogViewModel();
            CurrentViewModel = itemsViewModel;
            LogoutCommand = new ViewCommandAsync(Logout);
            SelectItemsCommand = new ViewCommand(SelectItem);
            SelectPersonsCommand = new ViewCommand(SelectPersons);
            SelectLogCommand = new ViewCommand(SelectLog);
            Username = userData.Username;
        }

        public IAsyncCommand LogoutCommand { get; set; }
        public ICommand SelectItemsCommand { get; set; }
        public ICommand SelectPersonsCommand { get; set; }
        public ICommand SelectLogCommand { get; set; }
        public string Username { get; set; }
        private void SelectItem()
        {
            CurrentViewModel = itemsViewModel;
        }
        private void SelectPersons()
        {
            CurrentViewModel = personsViewModel;
        }
        private void SelectLog()
        {
            CurrentViewModel = logViewModel;
        }
        private async Task Logout()
        {
            await service.LogoutAsync();
        }
    }
}
