using Client.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

namespace Client.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        private ServiceGateway gateway;
        private BindableBase currentViewModel;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }
        public SnackbarMessageQueue SnackbarQueue { get; set; }
        public MainWindowViewModel()
        {
            SnackbarQueue = new SnackbarMessageQueue();
            log.Info("Application started");
            gateway = new ServiceGateway(UserData.Instance);
            CurrentViewModel = new LoginViewModel(gateway, UserData.Instance,SnackbarQueue);
            gateway.LoginSuccessful += LoginSuccessfull;
            gateway.LogoutSuccessful += LogoutSuccessfull;
            gateway.EmergencyLogout += EmergencyLogout;
        }

        private void LoginSuccessfull(object sender, bool e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { gateway.StartService(); CurrentViewModel = new MainViewModel(gateway, UserData.Instance, e,SnackbarQueue); }));
        }

        private void LogoutSuccessfull(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                gateway.Dispose();
                gateway = new ServiceGateway(UserData.Instance);
                gateway.LoginSuccessful += LoginSuccessfull;
                gateway.LogoutSuccessful += LogoutSuccessfull;
                gateway.EmergencyLogout += EmergencyLogout;
                CurrentViewModel = new LoginViewModel(gateway, UserData.Instance, SnackbarQueue);
            }));
        }
        private void EmergencyLogout(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SnackbarQueue.Enqueue("Connection error encountered. Please try logging in again");
                gateway.Dispose();
                gateway = new ServiceGateway(UserData.Instance);
                gateway.LoginSuccessful += LoginSuccessfull;
                gateway.LogoutSuccessful += LogoutSuccessfull;
                gateway.EmergencyLogout += EmergencyLogout;
                CurrentViewModel = new LoginViewModel(gateway, UserData.Instance, SnackbarQueue);
            }));
        }

    }

}

