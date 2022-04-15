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
        public WindowTrainerErrors(List<string> logicalFuncs, List<List<TrainerError>> errors)
        {
            InitializeComponent();
            GenerateErrorsList(logicalFuncs, errors);
        }
        private async void GenerateErrorsList(List<string> logicalFuncs, List<List<TrainerError>> errors)
        {
            for (int i = 0; i < logicalFuncs.Count; i++)
            {
                StackPanel headerRow = new StackPanel() { Orientation = Orientation.Horizontal };
                TextBlock errorSymbolHeader;
                if (errors[i].Count == 0)
                {
                    errorSymbolHeader = new TextBlock() { Text = "✓", Foreground = new SolidColorBrush(Color.FromRgb(0, 128, 0)), Margin = new Thickness(5, 0, 0, 0), FontSize=18};
                }
                else
                {
                    errorSymbolHeader = new TextBlock() { Text = "✕", Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Margin = new Thickness(5, 0, 0, 0), FontSize = 18 };
                }
                TextBlock errorHeader = new TextBlock() { Text = logicalFuncs[i],VerticalAlignment = VerticalAlignment.Center};
                headerRow.Children.Add(errorSymbolHeader);
                headerRow.Children.Add(errorHeader);
                errorsContener.Children.Add(headerRow);
                for (int k = 0; k < errors[i].Count; k++)
                {
                    TextBlock errorSymbol = new TextBlock() { Text = "✕", Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0)), Margin = new Thickness(15, 0, 0, 0), FontSize = 18 };
                    StackPanel errorRow = new StackPanel() { Orientation=Orientation.Horizontal};
                    TextBlock errorText = new TextBlock() { Text = errors[i][k].ErrorMessage,VerticalAlignment = VerticalAlignment.Center};
                    errorRow.Children.Add(errorSymbol);
                    errorRow.Children.Add(errorText);
                    errorsContener.Children.Add(errorRow);
                }
            }
        }
    }
}
