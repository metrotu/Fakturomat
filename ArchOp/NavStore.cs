using ArchOp.ViewModels;

namespace ArchOp
{
    public class NavStore
    {

        public event Action CurrentViewModelChanged;

        private ViewModelBase currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        // Store the MainWindowViewModel instance
        public static MainWindowViewModel MainWindowViewModel { get; set; }


        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

    }
}
