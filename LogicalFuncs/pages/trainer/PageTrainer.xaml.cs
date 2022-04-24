using LogicalFuncs.ViewModel;
using LogicalFuncs.ViewModel.Patterns;
using LogicalFuncs.windows.trainer;
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

        PageStartTrainer startTrainer = new PageStartTrainer();//Страница с анимацией загрузки

        List<dynamic> inputGrid = new List<dynamic>();//Контейнер для страниц

        List<string> parsedFuncs = new List<string>();
        public PageTrainer()
        {
            InitializeComponent();
            DataContext = VMT;
            GridInputContener.Navigate(new PageStartTrainer());
        }

        //Функция для записи символа нажатой кнопки в строку функции
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            txtFunc.Text += obj.Content;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            inputGrid.Clear();
            txtFunc.Text = string.Empty;
            VMT.InputLogicalFuncs = new List<string>();
            VMT.CurrentPage = 0;
            GridInputContener.Content = null;
            GridInputContener.Navigate(startTrainer);

            //Очищаем память от предыдущих примеров
            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            inputGrid = new List<dynamic>();
            foreach (LogicFuncCalculator func in VMT.GetResultCalculation)
            {
                if (func.Answer != null || func.IsUserHaveAnswer)
                {
                    inputGrid.Add(new PageGridInput(VMT, func));
                }
            }

            if (VMT.IsClassesOn && inputGrid.Count!=0)
            {
                inputGrid.Add(new PageClasses(VMT, inputGrid));
            }

            //Перемещение кнопки при проверки результата при одной введенной функции
            if (inputGrid.Count != 0 && inputGrid.Count == 1)
            {
                ReplaceForwardButton();
            }
            else
            {
                ReplaceForwardButton();
            }

            //Выбираем первую таблицу первой функции
            if (inputGrid.Count != 0)
            {
                GoToPage(0);
            }
            else
            {
                GridInputContener.Content = null;
                GridInputContener.Navigate(startTrainer);
            }

            //Очищаем память от предыдущих примеров
            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //Загружает следующую или предыдущую страницу из inputGrid
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
        //Загружает определенную страницу из inputGrid
        private void GoToPage(int numberPage)
        {
            VMT.CurrentPage = numberPage;
            GridInputContener.Navigate(inputGrid[VMT.CurrentPage]);
        }
        //Перемещает кнопку поиска ошибок в зависимоти от количества страниц в inputGrid
        private void ReplaceForwardButton()
        {
            if (inputGrid.Count != 0 && inputGrid.Count == 1)
            {
                //Кнопка на месте слайдеров страниц
                Grid.SetRow(btnForward, 1);
            }
            else
            {
                //Кнопка выше слайдеров страниц
                Grid.SetRow(btnForward, 0);
            }
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

        //Получает список ошибок в виде List<TrainerError>
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
            WindowTrainerErrors errorsWindow = new WindowTrainerErrors(parsedFuncs, errors, VMT);
            errorsWindow.Show();
        }

        private void Classes_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                GoToPage(0);
                inputGrid.Add(new PageClasses(VMT, inputGrid));
                ReplaceForwardButton();
            }
            catch{}
        }

        private void Classes_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                GoToPage(0);
                inputGrid.RemoveAt(inputGrid.Count-1);
                ReplaceForwardButton();
            }
            catch{}
        }
    }
}
