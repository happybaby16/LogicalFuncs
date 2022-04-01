using LogicalFuncs.ViewModel;
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

namespace LogicalFuncs.pages.theory
{
    /// <summary>
    /// Логика взаимодействия для PageTheoryMenu.xaml
    /// </summary>
    public partial class PageTheoryMenu : Page
    {
        ViewModelMenu VMM;
        public PageTheoryMenu(ViewModelMenu VMM)
        {
            InitializeComponent();
            this.VMM = VMM;
            DataContext = VMM;
        }



        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image tImg = (Image)sender;
            PagesNavigation.PageContener.Navigate(new PageTheoryInfo(tImg.Uid));
        }

    }
}
