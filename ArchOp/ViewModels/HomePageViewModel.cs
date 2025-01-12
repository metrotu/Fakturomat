using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArchOp.ViewModels
{
    internal class HomePageViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        
        public Page CurrentPage { get; set; }

        public ICommand NavToInvoiceMakerCommand { get; }
        public ICommand NavToSentInvoiceCommand { get; }
        public ICommand NavToSetCompanyCommand { get; }
        public ICommand NavToAddCompanyCommand { get; }
        public ICommand LogOutCommand { get; }


        public HomePageViewModel(NavStore navStore)
        {
            this.navStore = navStore;
            NavToInvoiceMakerCommand = new RelayCommand(ExecuteNavToInvoice);
            NavToSentInvoiceCommand = new RelayCommand(ExecuteNavToSentInvoice);
            NavToSetCompanyCommand = new RelayCommand(ExecuteNavToSetCompany);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
            NavToAddCompanyCommand = new RelayCommand(ExecuteNavToAddCompany);
        }
        private void ExecuteNavToInvoice()
        {
            CurrentPage = new InvoiceMakerPage(navStore);
            OnPropertyChanged(nameof(CurrentPage));
        }
        private void ExecuteNavToSentInvoice()
        {
            CurrentPage = new SentInvoicesWindow(navStore);
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void ExecuteNavToSetCompany()
        {
            CurrentPage = new SetCompanyPage(navStore);
            OnPropertyChanged(nameof(CurrentPage));
        }
        private void ExecuteLogOut()
        {
            App.SupabaseClient.Auth.SignOut();
            navStore.CurrentViewModel = new DashboardViewModel(navStore);
        }
        private void ExecuteNavToAddCompany()
        {
            CurrentPage = new AddCompanyPage(navStore);
            OnPropertyChanged(nameof(CurrentPage));
        }
       
    }
}
