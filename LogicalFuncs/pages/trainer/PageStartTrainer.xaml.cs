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
        List<Line> vertLines = new List<Line>();
        List<Line> horizLines = new List<Line>();
        public PageStartTrainer()
        {
            InitializeComponent();
            LoadAnimation();
        }

        private async void LoadAnimation()
        {
            vertLines.Add(verticalLineOne);
            vertLines.Add(verticalLineTwo);

            horizLines.Add(horizontalLineOne);
            horizLines.Add(horizontalLineTwo);

            while (true)
            {
                for (int i = 0; i < 20; i+=2)
                {
                    vertLines[0].Y2 = i;
                    vertLines[1].Y1 = i;
                    horizLines[0].X1 = i;
                    horizLines[1].X1 = i;
                    await Task.Delay(20);
                }

                for (int i = 20; i > -1; i -= 2)
                {
                    vertLines[0].Y2 = i;
                    vertLines[1].Y1 = i;
                    horizLines[0].X1 = i;
                    horizLines[1].X1 = i;
                    await Task.Delay(20);
                }
            }
        }

    }
}
