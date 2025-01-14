namespace ArchOp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel => navStore.CurrentViewModel;

        private readonly NavStore navStore;
        public MainWindowViewModel() { }

        public MainWindowViewModel(NavStore navStore)
        {
            this.navStore = navStore;
            this.navStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
