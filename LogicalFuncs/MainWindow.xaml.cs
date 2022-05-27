using LogicalFuncs.ViewModel;
using System.Windows;
using System.Windows.Controls;


using LogicalFuncs.pages.theory;
using LogicalFuncs.pages.practice;
using LogicalFuncs.pages.trainer;
using System.Threading.Tasks;
using System.Collections.Generic;
using LogicalFuncs.pages.information;

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
        PageTrainer pageTrainerMenu;

        List<StackPanel> menuPoints;

        public MainWindow()
        {
            InitializeComponent();
            PagesNavigation.PageContener = PagesFrameContener;
            PagesNavigation.PageContener.Navigate(new PageTheoryMenu(VMM));
            DataContext = VMM;
            CommandBindings.Add(VMM.MenuMoveBinding);

            pageTheoryMenu = new PageTheoryMenu(VMM);
            pagePracticeMenu = new PagePracticeMenu();
            pageTrainerMenu = new PageTrainer();

            menuPoints = new List<StackPanel>() { spTheory, spCalculator, spPractice, spTrainer };
        }

        //Загрузка пунктов меню
        private void Point_Menu_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
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
                case "Калькулятор":
                    PagesNavigation.PageContener.Navigate(new PageTrainer(true));
                    break;
                case "Информация":
                    PagesNavigation.PageContener.Navigate(new PageInformation());
                    break;
            }

        }

        //Анимация плавного появления страницы
        private async void PagesFrameContener_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            int startMarginTop = 9;
            PagesFrameContener.Opacity = 0;
            PagesFrameContener.Margin = new Thickness(0, startMarginTop, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                PagesFrameContener.Opacity += 0.10;
                PagesFrameContener.Margin = new Thickness(0, startMarginTop-i, 0, 0);
                await Task.Delay(10);
            }
        }
    }
}
