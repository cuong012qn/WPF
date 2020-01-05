using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyKho_MVVM.ViewModels
{
    public interface BaseCommand
    {
        ICommand AddCommand { get; set; }
        ICommand EditCommand { get; set; }
        ICommand DeleteCommand { get; set; }
        ICommand LoadedWindowCommand { get; set; }
    }
}
