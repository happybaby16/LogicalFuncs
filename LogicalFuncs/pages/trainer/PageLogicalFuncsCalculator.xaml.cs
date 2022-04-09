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

namespace LogicalFuncs.pages.trainer
{
    /// <summary>
    /// Логика взаимодействия для PageLogicalFuncsCalculator.xaml
    /// </summary>
    public partial class PageLogicalFuncsCalculator : Page
    {
        ViewModelTrainer VMT;
        List<string> parsedFuncs = new List<string>();
        public PageLogicalFuncsCalculator(ViewModelTrainer VMT)
        {
            InitializeComponent();
            this.VMT = VMT;
            DataContext = VMT;
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

        private void AddFunc(object sender, RoutedEventArgs e)
        {
            //Проверка на невозможность добовления новой функции, если предыдущая строка не была заполнена
            if (txtFunc.Text!=string.Empty && 
                txtFunc.Text[txtFunc.Text.Length - 1] != '\n' && 
                txtFunc.Text[txtFunc.Text.Length - 1] != '\0')
            {
                txtFunc.Text += '\n';
            }
        }

        private void ResultAdding(object sender, RoutedEventArgs e)
        {
            parsedFuncs = new List<string>();
            string[] tempMassFuncs = txtFunc.Text.Replace("\r", string.Empty).Replace(" ", string.Empty).Split('\n');
            foreach (string funcString in tempMassFuncs)
            {
                if (funcString != string.Empty)
                {
                    parsedFuncs.Add(funcString);
                }
            }
        }
    }
}
