using LogicalFuncs.ViewModel;
using LogicalFuncs.ViewModel.Patterns;
using LogicFuncs.Model;
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
        ViewModelTrainer VMT = new ViewModelTrainer();

        List<PageGridInput> inputGrid=new List<PageGridInput>();

        List<string> parsedFuncs = new List<string>();
        public PageTrainer()
        {
            InitializeComponent();
            DataContext = VMT;
            GridInputContener.Navigate(new PageStartTrainer());
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
            if (txtFunc.Text != string.Empty &&
                txtFunc.Text[txtFunc.Text.Length - 1] != '\n' &&
                txtFunc.Text[txtFunc.Text.Length - 1] != '\0')
            {
                txtFunc.Text += '\n';
            }
        }

        private async void ResultAdding(object sender, RoutedEventArgs e)
        {
           
            //Парсим введенные функции
            parsedFuncs = new List<string>();
            string[] tempMassFuncs = txtFunc.Text.Replace("\r", string.Empty).Replace(" ", string.Empty).Split('\n');
            foreach (string funcString in tempMassFuncs)
            {
                if (funcString != string.Empty)
                {
                    parsedFuncs.Add(funcString);
                }
            }
            //Валидация введенных функций
            VMT.InputLogicalFuncs = parsedFuncs;
            inputGrid = new List<PageGridInput>();
            foreach (LogicFuncCalculator func in VMT.GetResultCalculation)
            {
                if (func.Answer != null)
                {
                    inputGrid.Add(new PageGridInput(VMT, func));
                }
            }
            //Выбираем первую таблицу первой функции
            if (inputGrid.Count != 0)
            {
                VMT.CurrentPage = 0;
                GridInputContener.Navigate(inputGrid[VMT.CurrentPage]);
            }
            else
            {
                GridInputContener.Content = null;
            }
        }

        private void Pagination(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            try
            {
                switch(obj.Uid)
                {
                    case "forward":
                        VMT.CurrentPage++;
                        break;
                    case "backward":
                        VMT.CurrentPage--;
                        break;
                }
                GridInputContener.Navigate(inputGrid[VMT.CurrentPage]);
            }
            catch
            { }
        }

        //Аниманция появления
        private async void GridInputContener_LoadCompleted(object sender, NavigationEventArgs e)
        {
            int startMarginTop = 9;
            GridInputContener.Opacity = 0;
            GridInputContener.Margin = new Thickness(0, startMarginTop, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                GridInputContener.Opacity += 0.10;
                GridInputContener.Margin = new Thickness(0, startMarginTop - i, 0, 0);
                await Task.Delay(10);
            }
        }

        private void CheckResults(object sender, RoutedEventArgs e)
        {
            List<List<TrainerError>> errors = new List<List<TrainerError>>();
            for (int i = 0; i < inputGrid.Count; i++)
            {
                var resultCheck = inputGrid[i].GetErrors();
                if (resultCheck != null)
                {
                    errors.Add(resultCheck);
                }
            }
        }
    }
}
