using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
namespace ArchOp.ViewModels
{

    public class DashboardViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public ICommand NavToLoginCommand { get; }
        public ICommand NavToRegisterCommand { get; }

        public DashboardViewModel(NavStore navStore)
        {
            this.navStore = navStore;
            NavToRegisterCommand = new RelayCommand(ExecuteNavToRegister);
            NavToLoginCommand = new RelayCommand(ExecuteNavToLogin);
        }

        private void ExecuteNavToRegister()
        {
            navStore.CurrentViewModel = new RegisterViewModel(navStore);
        }
        private void ExecuteNavToLogin()
        {
            navStore.CurrentViewModel = new LoginViewModel(navStore);
        }
    }

}
