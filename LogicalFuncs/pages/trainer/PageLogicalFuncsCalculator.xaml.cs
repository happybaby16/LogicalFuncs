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
    /// Логика взаимодействия для PageLogicalFuncsCalculator.xaml
    /// </summary>
    public partial class PageLogicalFuncsCalculator : Page
    {
        public PageLogicalFuncsCalculator()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            txtFunc.Text += obj.Content;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            txtFunc.Text = string.Empty;
        }

        private void RemoveLastElement(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFunc.Text = txtFunc.Text.Remove(txtFunc.Text.Length - 1, 1);
            }
            catch
            { }
        }
    }
}
