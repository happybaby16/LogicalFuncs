using LogicalFuncs.ViewModel;
using LogicalFuncs.ViewModel.Patterns;
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
    /// Логика взаимодействия для PageClasses.xaml
    /// </summary>
    public partial class PageClasses : Page
    {
        ViewModelTrainer VMT;
        List<dynamic> selectedClassesFuncs;

        List<string> headerOutputGrid = new List<string> { "F", "К", "К", "К", "К", "К" };
        List<string> headersIndexOutputGrid = new List<string> { "", "0", "1", "с", "л", "м" };
        GridLength size = new GridLength(1, GridUnitType.Star);

        public PageClasses(ViewModelTrainer VMT, List<dynamic> selectedClassesFuncs)
        {
            InitializeComponent();
            this.VMT = VMT;
            this.selectedClassesFuncs = selectedClassesFuncs;
        }

        private void GenerateOutputGridClasses(ViewModelTrainer VMT, List<dynamic> selectedClassesFuncs)
        {
            //Очищаем от предыдущего ответа
            gridOutputSelectedClasses.Children.Clear();
            gridOutputSelectedClasses.RowDefinitions.Clear();
            gridOutputSelectedClasses.ColumnDefinitions.Clear();

            //Генерируем колонки таблицы
            for (int i = 0; i < headerOutputGrid.Count; i++)
            {
                gridOutputSelectedClasses.ColumnDefinitions.Add(new ColumnDefinition() { Width=size});
            }

            //Генерируем строки таблицы (+1 для шапки таблицы)
            for (int i = 0; i < VMT.GetResultCalculation.Count + 1; i++)
            {
                gridOutputSelectedClasses.RowDefinitions.Add(new RowDefinition() { Height = size });
            }

            //Заполняем шапку таблицы
            for (int j = 0; j < headerOutputGrid.Count; j++)
            {
                DockPanel background = new DockPanel();
                TextBlock header = new TextBlock() { Style = (Style)this.Resources["txtHeader"]};
                background.Children.Add(header);
                header.Text = headerOutputGrid[j];
                header.Typography.Variants = FontVariants.Subscript;
                header.Text += headersIndexOutputGrid[j];
                Grid.SetRow(background, 0);
                Grid.SetColumn(background, j);
                gridOutputSelectedClasses.Children.Add(background);
            }

            //Заполняем таблицу контентом
            for (int i = 1; i < VMT.GetResultCalculation.Count + 1; i++)
            {
                List<bool> answerClasses = selectedClassesFuncs[i - 1].GetClassesAnswer();
                for (int j = 0; j < headerOutputGrid.Count; j++)
                {
                    Border border = new Border() { Style = (Style)this.Resources["brdrCellGrid"]};
                    if (j == 0)
                    {
                        TextBlock func = new TextBlock() { Text = VMT.GetResultCalculation[i - 1].LogicalFunc };
                        border.Child = func;
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                        gridOutputSelectedClasses.Children.Add(border);
                    }
                    else
                    {
                        TextBlock answer = new TextBlock();
                        border.Child = answer;
                        if (answerClasses[j - 1])
                        {
                            answer.Text = "✓";
                            answer.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                            answer.FontWeight = FontWeights.Bold;
                            border.Child = answer;
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            gridOutputSelectedClasses.Children.Add(border);
                        }
                        else
                        {
                            answer.Text = "❌";
                            answer.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                            gridOutputSelectedClasses.Children.Add(border);
                        }
                    }
                }
            }

            if (VMT.IsCalculator)
            {
                checkBoxFullFunc.IsChecked = GetClassesAnswer();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateOutputGridClasses(VMT, selectedClassesFuncs);
        }

        private bool GetClassesAnswer()
        {
            bool globalAnswer = true;//Является ли функция полной?
            List<bool> checkList = new List<bool>() { false, false, false, false, false };
            for (int i = 0; i < VMT.GetResultCalculation.Count; i++)
            {
                List<bool> answersFunc = VMT.GetResultCalculation[i].GetClasses;
                for (int j = 0; j < answersFunc.Count; j++)
                {
                    if (checkList[j] != true && answersFunc[j] == true)
                    {
                        checkList[j] = true;
                    }
                }
            }
            for (int i = 0; i < checkList.Count; i++)
            {
                if (checkList[i] == false)
                {
                    globalAnswer = false;
                    break;
                }
            }
            return globalAnswer;
        }

        public List<TrainerError> GetErrors()
        {
            List<TrainerError> errors = new List<TrainerError>();
            if (checkBoxFullFunc.IsChecked!=GetClassesAnswer())
            {
                errors.Add(new TrainerError(TypeError.ErrorFullFunc, "Полнота системы функций. Ошибка определения полноты системы функций."));
            }

            return errors;
        }
    }
}
