using BirthdayCalculator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BirthdayCalculator.View
{
    /// <summary>
    /// Interaction logic for AgeCounterView.xaml
    /// </summary>
    public partial class AgeCounterView : UserControl
    {
        private PersonViewModel _viewModel;
        public AgeCounterView()
        {
            InitializeComponent();
            DataContext = _viewModel = new PersonViewModel();
        }
       
}
}
