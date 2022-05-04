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
using System.Windows.Shapes;

namespace LogicalFuncs.windows.trainer
{
    /// <summary>
    /// Логика взаимодействия для WindowTrainerErrors.xaml
    /// </summary>
    public partial class WindowTrainerErrors : Window
    {
        List<string> logicalFuncs;
        List<List<TrainerError>> errors;
        ViewModelTrainer VMT;
        public WindowTrainerErrors(List<string> logicalFuncs, List<List<TrainerError>> errors, ViewModelTrainer VMT)
        {
            InitializeComponent();
            this.logicalFuncs = logicalFuncs;
            this.errors = errors;
            this.VMT = VMT;
            GenerateErrorsList();
        }
        private async void GenerateErrorsList()
        {
            if (logicalFuncs != null)
            {
                for (int i = 0; i < logicalFuncs.Count; i++)
                {
                    StackPanel headerRow = new StackPanel() { Orientation = Orientation.Horizontal };
                    TextBlock errorSymbolHeader;
                    if (errors[i].Count == 0)
                    {
                        errorSymbolHeader = new TextBlock() { Text = "✓", Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0)), Margin = new Thickness(10, 0, 0, 0), FontSize = 16, FontWeight = FontWeights.Bold };
                    }
                    else
                    {
                        errorSymbolHeader = new TextBlock() { Text = "❌", Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Margin = new Thickness(5, 0, 0, 0), FontSize = 16 };
                    }
                    TextBlock errorHeader = new TextBlock() { Text = logicalFuncs[i], VerticalAlignment = VerticalAlignment.Center, FontSize = 16 };
                    headerRow.Children.Add(errorSymbolHeader);
                    headerRow.Children.Add(errorHeader);
                    errorsContener.Children.Add(headerRow);

                    //Показывает в чём именно ошибка заключается
                    if (cmbErrorsMode.SelectedIndex == 1)
                    {
                        AdvancedMode(i);
                    }
                }

                if (VMT.IsClassesOn)
                {
                    if (errors[errors.Count - 1].Count != 0 && errors[errors.Count - 1][0].Type == TypeError.ErrorFullFunc)
                    {
                        TextBlock errorSymbol = new TextBlock() { Text = "❌", Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Margin = new Thickness(5, 0, 0, 0), FontSize = 16 };
                        StackPanel errorRow = new StackPanel() { Orientation = Orientation.Horizontal };
                        TextBlock errorText = new TextBlock() { Text = errors[errors.Count - 1][0].ErrorMessage, VerticalAlignment = VerticalAlignment.Center, FontSize = 16 };
                        errorRow.Children.Add(errorSymbol);
                        errorRow.Children.Add(errorText);
                        errorsContener.Children.Add(errorRow);
                    }
                    else
                    {
                        TextBlock errorSymbol = new TextBlock() { Text = "✓", Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0)), Margin = new Thickness(10, 0, 0, 0), FontSize = 18, FontWeight = FontWeights.Bold };
                        StackPanel errorRow = new StackPanel() { Orientation = Orientation.Horizontal };
                        TextBlock errorText = new TextBlock() { Text = "Полнота системы функций", VerticalAlignment = VerticalAlignment.Center, FontSize = 16 };
                        errorRow.Children.Add(errorSymbol);
                        errorRow.Children.Add(errorText);
                        errorsContener.Children.Add(errorRow);
                    }
                }
            }

        }

        private void AdvancedMode(int indexFunc)
        {
            for (int k = 0; k < errors[indexFunc].Count; k++)
            {
                TextBlock errorSymbol = new TextBlock() { Text = "❌", Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Margin = new Thickness(25, 0, 0, 0) };
                StackPanel errorRow = new StackPanel() { Orientation = Orientation.Horizontal };
                TextBlock errorText = new TextBlock() { Text = errors[indexFunc][k].ErrorMessage, VerticalAlignment = VerticalAlignment.Center };
                errorRow.Children.Add(errorSymbol);
                errorRow.Children.Add(errorText);
                errorsContener.Children.Add(errorRow);
            }
        }
        private void AdvancedMode_Selected(object sender, RoutedEventArgs e)
        {
            errorsContener?.Children.Clear();
            GenerateErrorsList();
        }
    }
}
