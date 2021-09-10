using Client.Model;
using Client.Model.Interfaces;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class LoginViewModel : BindableBase
    {
        private ILoginUpdateService service;
        IUserData userData;
        public IAsyncCommand<object> LoginCommand { get; set; }
        private bool isIndeterminate;
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set
            {
                SetProperty(ref isIndeterminate, value);
            }
        }
        public SnackbarMessageQueue SnackbarQueue { get; set; }
        public string Username { get; set; }
        private bool IsBusy { get; set; }
        public LoginViewModel(ILoginUpdateService service, IUserData userData,SnackbarMessageQueue queue)
        {
            this.service = service;
            this.userData = userData;
            SnackbarQueue = queue;
            IsIndeterminate = false;
            service.LoginFailed += (o, e) => SnackbarQueue.Enqueue(e);
            service.LoginSuccessful += (o, e) => SnackbarQueue.Enqueue("Login is sucessfull");
            LoginCommand = new ViewCommandAsync<object>(LoginActionAsync, _ => !IsBusy);

        }


        private async Task LoginActionAsync(object passwordBox)
        {
            try
            {
                IsBusy = true;
                var password = (passwordBox as System.Windows.Controls.PasswordBox).Password;
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
                {
                    SnackbarQueue.Enqueue("Both Username and Password have to be supplied.");
                    return;
                }

                IsIndeterminate = true;
                await service.LoginAsync(Username, password);
            }
            finally
            {
                IsBusy = false;
            }


        }
    }
}
