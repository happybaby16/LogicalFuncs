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
    /// Логика взаимодействия для PageTrainerMenu.xaml
    /// </summary>
    public partial class PageTrainerMenu : Page
    {
        public PageTrainerMenu()
        {
            InitializeComponent();
        }

        //Анимация появления страницы
        private async void pageObject_Loaded(object sender, RoutedEventArgs e)
        {
            pageObject.Opacity = 0;
            for (int i = 0; i < 10; i++)
            {
                pageObject.Opacity += 0.10;
                await Task.Delay(30);
            }
        }
    }
}
