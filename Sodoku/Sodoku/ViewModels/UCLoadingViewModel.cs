using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;

namespace Sodoku.ViewModels
{
    class UCLoadingViewModel : BaseViewModel
    {
        public ICommand CloseButtonCommand { get; set; }

        public UCLoadingViewModel()
        {
            CloseButtonCommand = new RelayCommand<object>(p => true, (p) =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
    }
}
