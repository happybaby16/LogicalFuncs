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

namespace LogicalFuncs.pages.trainer
{
    /// <summary>
    /// Логика взаимодействия для PageTrainer.xaml
    /// </summary>
    public partial class PageTrainer : Page
    {
        public PageTrainer()
        {
            InitializeComponent();
            CalculatorContener.Navigate(new PageLogicalFuncsCalculator());
            GridInputContener.Navigate(new PageGridInput());
        }
    }
}
