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


        LinearGradientBrush linerColorFalse;
        LinearGradientBrush linerColorTrue;
        GridLength size = new GridLength(1, GridUnitType.Star);
        public PageGridInput(ViewModelTrainer VMT, LogicFuncCalculator Func)
        {
            InitializeComponent();
            txtFunc.Text = Func.LogicalFunc;
            this.VMT = VMT;
            this.Func = Func;
            DataContext = VMT;
            if (!Func.IsUserHaveAnswer)
            {
                GenerateGridInput(Func.VariableNames, Func.CalculationLogs[0].Count - Func.VariableNames.Count);
            }
            else
            {
                GenerateGridInput(Func.VariableNames, 1);
            }
        }


        private void ContentCell(object sender, EventArgs e)
        {
            TextBox obj = (TextBox)sender;
            if (obj.Text == string.Empty || obj.Text == "1" || obj.Text == "0")
            {
            }
            else
            {
                obj.Text = string.Empty;
            }
           
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
                    cell.TextChanged += ContentCell;
                    cell.MinWidth = 30;
                    if (j < variables.Count)
                    {
                        cell.Text = Convert.ToString(Convert.ToInt32(Func.CalculationLogs[i-1][j].ResultValue));
                    }
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    gridInput.Children.Add(cell);
                    cellGridInput[i - 1].Add(cell);
                }
            }


        }

        public List<TrainerError> GetErrors()
        {
            GradientStopCollection colorFalse = new GradientStopCollection();
            colorFalse.Add(new GradientStop() { Color = Color.FromArgb(100, 255, 0, 0), Offset = 0.0 });
            colorFalse.Add(new GradientStop() { Color = Color.FromArgb(0, 255, 0, 0), Offset = 0.1 });
            colorFalse.Add(new GradientStop() { Color = Color.FromArgb(0, 0, 0, 0), Offset = 1 });

            GradientStopCollection colorTrue = new GradientStopCollection();
            colorTrue.Add(new GradientStop() { Color = Color.FromArgb(100, 0, 255, 0), Offset = 0.0 });
            colorTrue.Add(new GradientStop() { Color = Color.FromArgb(0, 0, 255, 0), Offset = 0.1 });
            colorTrue.Add(new GradientStop() { Color = Color.FromArgb(0, 0, 0, 0), Offset = 1 });

            linerColorFalse = new LinearGradientBrush(colorFalse, 0)
            {
                StartPoint = new Point(0.75, 1),
                EndPoint = new Point(0, 0),
            };

            linerColorTrue = new LinearGradientBrush(colorTrue, 0)
            {
                StartPoint = new Point(0.75, 1),
                EndPoint = new Point(0, 0),
            };

            List<int> columnsWithError = new List<int>();
            List<TrainerError> detectedErrors = new List<TrainerError>();
            if (!Func.IsUserHaveAnswer)
            {
                for (int i = 0; i < cellGridInput.Count; i++)
                {
                    for (int j = 0; j < cellGridInput[0].Count; j++)
                    {
                        try
                        {
                            if (Convert.ToBoolean(Convert.ToInt32(cellGridInput[i][j].Text)) != Func.CalculationLogs[i][j].ResultValue)
                            {
                                cellGridInput[i][j].Background = linerColorFalse;
                                if (j < Func.VariableNames.Count)
                                {
                                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, j + 1, i + 1));
                                }
                                else
                                {
                                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, Func.CalculationLogs[i][j], j + 1, i + 1));
                                }
                                columnsWithError.Add(j);
                            }
                            else
                            {
                                cellGridInput[i][j].Background = linerColorTrue;
                            }
                        }
                        catch
                        {
                            columnsWithError.Add(j);
                            detectedErrors.Add(new TrainerError(Func.LogicalFunc, j + 1, i + 1));
                        }
                    }
                }

                columnsWithError = columnsWithError.Distinct().ToList();
                foreach (int columError in columnsWithError)
                {
                    SetErrorColumn(columError);
                }
            }
            else
            {
                int lastColumn = Func.VariableNames.Count;
                Func.Answer.Clear();
                for (int i = 0; i < Math.Pow(2, Func.VariableNames.Count); i++)
                {
                    try
                    {
                        Func.Answer.Add(Convert.ToBoolean(Convert.ToInt32(cellGridInput[i][lastColumn].Text)));
                    }
                    catch
                    {
                        detectedErrors.Add(new TrainerError(Func.LogicalFunc, TypeError.ErrorAnswerNull, "Заполнение таблицы. Проверьте правильность введенных даннных."));
                        SetErrorColumn(lastColumn);
                        Func.Answer.Clear();
                        break;
                    }
                }
                if (Func.Answer.Count != 0)
                {
                    SetNullColorColumn(lastColumn);
                }
            }

            if (Func.Answer.Count == Math.Pow(2, Func.VariableNames.Count))
            {
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
            }

            return detectedErrors;
        }



        private void SetErrorColumn(int columWithError)
        {
            for (int i = 0; i < Math.Pow(2, Func.VariableNames.Count); i++)
            {
                cellGridInput[i][columWithError].Background = linerColorFalse;
            }
        }

        private void SetNullColorColumn(int columWithError)
        {
            for (int i = 0; i < Math.Pow(2, Func.VariableNames.Count); i++)
            {
                cellGridInput[i][columWithError].Background = null ;
            }
        }

    }
}
