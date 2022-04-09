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
    /// Логика взаимодействия для PageGridInput.xaml
    /// </summary>
    public partial class PageGridInput : Page
    {
        ViewModelTrainer VMT;
        LogicFuncCalculator Func;

        List<List<TextBox>> cellGridInput = new List<List<TextBox>>();
        List<TextBlock> headerGridInput = new List<TextBlock>();
        List<DockPanel> backgroundHeaders = new List<DockPanel>();
        List<string> variables = new List<string>() { "A","B", "C"};
        GridLength size = new GridLength(1, GridUnitType.Star);
        public PageGridInput(ViewModelTrainer VMT, LogicFuncCalculator Func)
        {
            InitializeComponent();
            txtFunc.Text = Func.LogicalFunc;
            this.VMT = VMT;
            this.Func = Func;
            DataContext = VMT;
            GenerateGridInput(Func.VariableNames, Func.CalculationLogs[0].Count-Func.VariableNames.Count);
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
                    cell.MinWidth = 30;
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    gridInput.Children.Add(cell);
                    cellGridInput[i - 1].Add(cell);
                }
                
            }


        }

        public List<TrainerError> GetErrors()
        {
            List<TrainerError> detectedErrors = new List<TrainerError>();
            for (int i = 0; i < cellGridInput.Count; i++)
            {
                for (int j = 0; j < cellGridInput[0].Count; j++)
                {
                    if(Convert.ToBoolean(Convert.ToInt32(cellGridInput[i][j].Text)) != Func.CalculationLogs[i][j].ResultValue)
                    {
                        if (j < Func.VariableNames.Count)
                        {
                            detectedErrors.Add(new TrainerError(Func.LogicalFunc, j + 1, i + 1));
                        }
                        else
                        {
                            detectedErrors.Add(new TrainerError(Func.LogicalFunc, Func.CalculationLogs[i][j],j+1,i+1));
                        }
                    }
                }
            }

            if (classSaveZero.IsChecked != Func.IsSavedZero)
            {
                detectedErrors.Add(new TrainerError(Func.LogicalFunc, "K0", "сохраняет константу 0"));
            }

            if (classSaveOne.IsChecked != Func.IsSavedOne)
            {
                detectedErrors.Add(new TrainerError(Func.LogicalFunc, "K1", "сохраняет константу 1"));
            }

            if (classSelfDual.IsChecked != Func.IsSelfDual)
            {
                detectedErrors.Add(new TrainerError(Func.LogicalFunc, "Kс", "самодвойственная функция"));
            }

            if (classMonotony.IsChecked != Func.IsMonotony)
            {
                detectedErrors.Add(new TrainerError(Func.LogicalFunc, "Kм", "монотонная функция"));
            }

            return detectedErrors;
        }

    }
}
