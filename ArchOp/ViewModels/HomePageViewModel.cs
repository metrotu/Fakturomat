using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.ViewModels
{
    class HomePageViewModel
    {
        public HomePageViewModel() { }
        public async void LogOut()
        {
            await App.SupabaseClient.Auth.SignOut();

        }
    }
}
