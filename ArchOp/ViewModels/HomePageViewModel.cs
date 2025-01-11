using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.ViewModels
{
    internal class HomePageViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        public HomePageViewModel(NavStore navStore)
        {
            this.navStore = navStore;
        }
    }
}
