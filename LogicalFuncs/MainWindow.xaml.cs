using LogicalFuncs.ViewModel;
using System.Windows;
using System.Windows.Controls;


using LogicalFuncs.pages.theory;

namespace LogicalFuncs
{

    public static class PagesNavigation
    {
        public static Frame PageContener;
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModelMenu VMM = new ViewModelMenu();
        public MainWindow()
        {
            InitializeComponent();
            PagesNavigation.PageContener = PagesFrameContener;
            PagesNavigation.PageContener.Navigate(new PageTheoryMenu());
            DataContext = VMM;
            CommandBindings.Add(VMM.MenuMoveBinding);
            
        }
    }
}
