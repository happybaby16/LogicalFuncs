using LogicalFuncs.ViewModel;
using System.Windows;
using System.Windows.Controls;


using LogicalFuncs.pages.theory;
using LogicalFuncs.pages.practice;
using LogicalFuncs.pages.trainer;
using System.Threading.Tasks;

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
        PageTheoryMenu pageTheoryMenu;
        PagePracticeMenu pagePracticeMenu;
        PageTrainerMenu pageTrainerMenu;

        public MainWindow()
        {
            InitializeComponent();
            PagesNavigation.PageContener = PagesFrameContener;
            PagesNavigation.PageContener.Navigate(new PageTheoryMenu(VMM));
            DataContext = VMM;
            CommandBindings.Add(VMM.MenuMoveBinding);
        }

        private void Point_Menu_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            pageTheoryMenu = new PageTheoryMenu(VMM);
            pagePracticeMenu = new PagePracticeMenu();
            pageTrainerMenu = new PageTrainerMenu();

            StackPanel obj = (StackPanel)sender;
            switch (obj.Uid)
            {
                case "Теория":
                    PagesNavigation.PageContener.Navigate(pageTheoryMenu);
                    break;
                case "Практика":
                    PagesNavigation.PageContener.Navigate(pagePracticeMenu);
                    break;
                case "Тренажер":
                    PagesNavigation.PageContener.Navigate(pageTrainerMenu);
                    break;
            }
        }


        //Анимация плавного появления страницы
        private async void PagesFrameContener_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            PagesFrameContener.Opacity = 0;
            for (int i = 0; i < 10; i++)
            {
                PagesFrameContener.Opacity += 0.10;
                await Task.Delay(30);
            }
        }
    }
}
