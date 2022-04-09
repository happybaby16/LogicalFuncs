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
    /// Логика взаимодействия для PageStartTrainer.xaml
    /// </summary>
    public partial class PageStartTrainer : Page
    {
        List<TextBlock> animationRectangle = new List<TextBlock>();
        public PageStartTrainer()
        {
            InitializeComponent();
            LoadAnimation();
        }

        private async void LoadAnimation()
        {
            animationRectangle.Add(rectOne);
            animationRectangle.Add(rectTwo);
            animationRectangle.Add(rectThree);
            while (true)
            {
                foreach (TextBlock itemFocuse in animationRectangle)
                {
                    foreach (TextBlock itemUnfocuse in animationRectangle)
                    {
                        itemUnfocuse.Opacity = 0.5;
                    }
                    itemFocuse.Opacity = 1;
                    await Task.Delay(100);
                }
            }
        }
    }
}
