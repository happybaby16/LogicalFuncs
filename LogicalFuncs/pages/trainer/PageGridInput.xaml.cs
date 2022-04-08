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
    /// Логика взаимодействия для PageGridInput.xaml
    /// </summary>
    public partial class PageGridInput : Page
    {
        List<List<TextBox>> cellGridInput = new List<List<TextBox>>();
        List<TextBlock> headerGridInput = new List<TextBlock>();
        List<DockPanel> backgroundHeaders = new List<DockPanel>();
        List<string> variables = new List<string>() { "A","B", "C"};
        GridLength size = new GridLength(1, GridUnitType.Star);
        public PageGridInput()
        {
            InitializeComponent();
            GenerateGridInput(variables, 5);
        }

        private async void GenerateGridInput(List<string> variables, int countMoves)
        {
            //Очищаем предыдущую генерацию
            gridInput.Children.Clear();
            headerGridInput.Clear();
            backgroundHeaders.Clear();
            cellGridInput.Clear();

            //Генерируем столбцы таблицы 
            for (int i = 0; i < variables.Count+countMoves; i++)
            {
                gridInput.ColumnDefinitions.Add(new ColumnDefinition() { Width = size });
            }

            //Генерируем строки таблицы 
            for (int i = 0; i < Math.Pow(2, variables.Count)+1; i++)
            {
                if (i == 0)
                {
                    gridInput.RowDefinitions.Add(new RowDefinition() { Height = size });
                }
                else
                {
                    gridInput.RowDefinitions.Add(new RowDefinition() { Height = size });
                }
            }


            //Генерируем заголовки таблицы

            for (int j = 0; j < variables.Count + countMoves; j++)
            {
                DockPanel background = new DockPanel();
                TextBlock header = new TextBlock();
                background.Children.Add(header);
                try
                {
                    header.Text = variables[j];
                }
                catch
                {
                    header.FontSize = 12;
                    header.Text = "№"+Convert.ToString(j - (variables.Count - 1));
                }
               
                Grid.SetRow(background, 0);
                Grid.SetColumn(background, j);
                gridInput.Children.Add(background);
                headerGridInput.Add(header);
                backgroundHeaders.Add(background);
            }

           
            //Генерируем контент таблицы
            for (int i = 1; i < Math.Pow(2, variables.Count) + 1; i++)
            {
                cellGridInput.Add(new List<TextBox>());
                for (int j = 0; j < variables.Count + countMoves; j++)
                {
                    TextBox cell = new TextBox();
                    cell.Text = $"{i};{j}";
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    gridInput.Children.Add(cell);
                    cellGridInput[i - 1].Add(cell);
                }
                
            }


        }

    }
}
