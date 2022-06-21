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
                    gridInput.RowDefinitions.Add(new RowDefinition() { Height = size});
                }
                else
                {
                    gridInput.RowDefinitions.Add(new RowDefinition() { Height = size});
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
                    if (VMT.IsCalculator && !Func.IsUserHaveAnswer)
                    {
                        header.Text = Func.OperationsPriority[j - (variables.Count)];
                    }
                    else
                    {
                        header.Text = "№" + Convert.ToString(j - (variables.Count - 1));
                    }
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
            }
            for (int j = 0; j < variables.Count + countMoves; j++)
            {
                for (int i = 0; i < Math.Pow(2, variables.Count); i++)
                {
                    TextBox cell = new TextBox();
                    cell.TextChanged += ContentCell;
                    cell.MinWidth = 30;

                    //VMT.IsCalculator заполняет все ячейки ответами, если режим калькулятора
                    if (j < variables.Count || VMT.IsCalculator)
                    {
                        try
                        {
                            cell.Text = Convert.ToString(Convert.ToInt32(Func.CalculationLogs[i][j].ResultValue));
                        }
                        catch 
                        {
                            cell.TextChanged += IsUserHaveAnswer_Calculator_TextChanged;
                        }
                    }
                    Grid.SetRow(cell, i+1);
                    Grid.SetColumn(cell, j);
                    gridInput.Children.Add(cell);
                    cellGridInput[i].Add(cell);
                }
            }

            //Указываем классы, к которым принадлежит функция (функция только для калькулятора)
            if(VMT.IsCalculator)
            {
                classSaveZero.IsChecked = Func.IsSavedZero == null ? false : Func.IsSavedZero;
                classSaveOne.IsChecked = Func.IsSavedOne == null ? false: Func.IsSavedOne;
                classSelfDual.IsChecked = Func.IsSelfDual==null ? false : Func.IsSelfDual;
                classLinear.IsChecked = Func.IsLiner==null ? false : Func.IsLiner;
                classMonotony.IsChecked = Func.IsMonotony==null ? false : Func.IsMonotony;
            }


        }

        //Метод для вывода выбранных классов (PageClasses)
        public List<bool> GetClassesAnswer()
        {
            List<bool> answers = new List<bool>();
            answers.Add(Convert.ToBoolean(classSaveZero.IsChecked));
            answers.Add(Convert.ToBoolean(classSaveOne.IsChecked));
            answers.Add(Convert.ToBoolean(classSelfDual.IsChecked));
            answers.Add(Convert.ToBoolean(classLinear.IsChecked));
            answers.Add(Convert.ToBoolean(classMonotony.IsChecked));
            return answers;
        }

        //Возвращает список ошибок 
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

            if (Func.Answer.Count == Math.Pow(2, Func.VariableNames.Count) && VMT.IsClassesOn)
            {
                if (classSaveZero.IsChecked != Func.IsSavedZero)
                {
                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, "K0"));
                }

                if (classSaveOne.IsChecked != Func.IsSavedOne)
                {
                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, "K1"));
                }

                if (classSelfDual.IsChecked != Func.IsSelfDual)
                {
                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, "Kс"));
                }

                if (classLinear.IsChecked != Func.IsLiner)
                {
                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, "Kл" ));
                }

                if (classMonotony.IsChecked != Func.IsMonotony)
                {
                    detectedErrors.Add(new TrainerError(Func.LogicalFunc, "Kм"));
                }
            }

            return detectedErrors;
        }

        //Подсвечивает столбец с ошибкой
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

        //Анимация появления панели замкнутых классов
        private async void ClassesPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StackPanel obj = (StackPanel)sender;
            int startMarginTop = 9;
            obj.Opacity = 0;
            obj.Margin = new Thickness(0, startMarginTop, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                obj.Opacity += 0.10;
                obj.Margin = new Thickness(0, startMarginTop - i, 0, 0);
                await Task.Delay(10);
            }
        }

        //Генерирует ответ принадлежности к замкнутым классам для неполных функций на ходу (режим калькулятора)
        private void IsUserHaveAnswer_Calculator_TextChanged(object sender, TextChangedEventArgs e)
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
                    Func.Answer.Clear();
                    break;
                }
            }

            classSaveZero.IsChecked = Func.IsSavedZero == null ? false : Func.IsSavedZero;
            classSaveOne.IsChecked = Func.IsSavedOne == null ? false : Func.IsSavedOne;
            classSelfDual.IsChecked = Func.IsSelfDual == null ? false : Func.IsSelfDual;
            classLinear.IsChecked = Func.IsLiner == null ? false : Func.IsLiner;
            classMonotony.IsChecked = Func.IsMonotony == null ? false : Func.IsMonotony;
        }
    }
}
