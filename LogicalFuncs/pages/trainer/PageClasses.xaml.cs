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
    /// Логика взаимодействия для PageClasses.xaml
    /// </summary>
    public partial class PageClasses : Page
    {
        List<string> headerOutputGrid = new List<string> { "К0", "К1", "Кс", "Кл", "Км"};
        GridLength size = new GridLength(1, GridUnitType.Star);
        public PageClasses(ViewModelTrainer VMT, List<PageGridInput> selectedClassesFuncs)
        {
            InitializeComponent();
            GenerateOutputGridClasses(VMT, selectedClassesFuncs);
        }

        private void GenerateOutputGridClasses(ViewModelTrainer VMT, List<PageGridInput> selectedClassesFuncs)
        {
            //Генерируем колонки таблицы
            for (int i = 0; i < headerOutputGrid.Count+1; i++)
            {
                gridOutputSelectedClasses.ColumnDefinitions.Add(new ColumnDefinition() { Width=size});
            }

            //Генерируем столбцы таблицы (+1 для шапки таблицы)
            for (int i = 0; i < VMT.GetResultCalculation.Count + 1; i++)
            {
                gridOutputSelectedClasses.RowDefinitions.Add(new RowDefinition() { Height = size });
            }

            //Заполняем шапку таблицы
            for (int j = 1; j < headerOutputGrid.Count+1; j++)
            {
                DockPanel background = new DockPanel();
                TextBlock header = new TextBlock();
                background.Children.Add(header);
                header.Text = headerOutputGrid[j-1];
                Grid.SetRow(background, 0);
                Grid.SetColumn(background, j);
                gridOutputSelectedClasses.Children.Add(background);
            }

            //Заполняем таблицу контентом
            for (int i = 1; i < VMT.GetResultCalculation.Count + 1; i++)
            {
                List<bool> answerClasses = selectedClassesFuncs[i - 1].GetClassesAnswer();
                for (int j = 0; j < headerOutputGrid.Count+1; j++)
                {
                    if (j == 0)
                    {
                        TextBlock func = new TextBlock() { Text = VMT.GetResultCalculation[i - 1].LogicalFunc };
                        Grid.SetRow(func, i);
                        Grid.SetColumn(func, j);
                        gridOutputSelectedClasses.Children.Add(func);
                    }
                    else
                    {
                        TextBlock answer = new TextBlock();
                        if (answerClasses[j - 1])
                        {
                            answer.Text = "✓";
                            answer.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            Grid.SetRow(answer, i);
                            Grid.SetColumn(answer, j);
                            gridOutputSelectedClasses.Children.Add(answer);
                        }
                        else
                        {
                            answer.Text = "❌";
                            answer.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            Grid.SetRow(answer, i);
                            Grid.SetColumn(answer, j);
                            gridOutputSelectedClasses.Children.Add(answer);
                        }
                    }
                }
            }
        }
    }
}
