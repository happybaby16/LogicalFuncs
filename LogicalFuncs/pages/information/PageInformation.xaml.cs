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

namespace LogicalFuncs.pages.information
{
    /// <summary>
    /// Логика взаимодействия для PageInformation.xaml
    /// </summary>
    public partial class PageInformation : Page
    {
        bool imageClickIsLocked = false;

        BitmapImage imgTheory = new BitmapImage(new Uri("../../images/information/Theory.png", UriKind.Relative));
        BitmapImage imgPractice = new BitmapImage(new Uri("../../images/information/Practice.png", UriKind.Relative));
        BitmapImage imgTrainer = new BitmapImage(new Uri("../../images/information/Trainer.png", UriKind.Relative));
        BitmapImage imgErrors = new BitmapImage(new Uri("../../images/information/Errors.png", UriKind.Relative));
        BitmapImage imgCalculator = new BitmapImage(new Uri("../../images/information/Calculator.png", UriKind.Relative));

        List<TextBlock> txtPoints;
        int startMarginTop = 19;
        int selectedImageIndex = 0;


        List<BitmapImage> bitmapImages;
        public PageInformation()
        {
            InitializeComponent();

            txtPoints = new List<TextBlock> { btnTheory, btnPractice, btnTrainer, btnErrors, btnCalculator};

            bitmapImages = new List<BitmapImage> { imgTheory, imgPractice, imgTrainer, imgErrors, imgCalculator };
            imgCurrentImage.Source = imgTheory;
        }

        private async void Information_Pagination(object sender, MouseButtonEventArgs e)
        {
            
            imgCurrentImage.Opacity = 1;
            imgCurrentImage.Margin = new Thickness(0, 0, 0, 0);
            for (int i = 0; i < 20; i++)
            {
                imgCurrentImage.Opacity -= 0.05;
                imgCurrentImage.Margin = new Thickness(startMarginTop - (i), 0, 0, 0);
                await Task.Delay(10);
            }

            TextBlock obj = (TextBlock)sender;
            string path = obj.Uid;
            selectedImageIndex = bitmapImages.IndexOf(bitmapImages.Single(x => x.UriSource.OriginalString == path));
            imgCurrentImage.Source = new BitmapImage(new Uri(path, UriKind.Relative));

            txtPoints.ForEach(x => x.Opacity = 0.45);
            txtPoints[selectedImageIndex].Opacity = 1;

            imgCurrentImage.Opacity = 0;
            imgCurrentImage.Margin = new Thickness(0, 0, startMarginTop, 0);
            for (int i = 0; i < 20; i++)
            {
                imgCurrentImage.Opacity += 0.05;
                imgCurrentImage.Margin = new Thickness(0, 0, startMarginTop - (i), 0);
                await Task.Delay(10);
            }
        }

        private async void imgCurrentImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!imageClickIsLocked)
            {
                imgCurrentImage.Opacity = 1;
                imgCurrentImage.Margin = new Thickness(0, 0, 0, 0);
                for (int i = 0; i < 20; i++)
                {
                    imgCurrentImage.Opacity -= 0.05;
                    imgCurrentImage.Margin = new Thickness(startMarginTop - (i), 0, 0, 0);
                    await Task.Delay(10);
                }


                imageClickIsLocked = true;
                if (selectedImageIndex + 1 < bitmapImages.Count)
                {
                    selectedImageIndex += 1;
                }
                else
                {
                    selectedImageIndex = 0;
                }

                imgCurrentImage.Source = bitmapImages.ElementAt(selectedImageIndex);

                txtPoints.ForEach(x => x.Opacity = 0.45);
                txtPoints[selectedImageIndex].Opacity = 1;

                imgCurrentImage.Opacity = 0;
                imgCurrentImage.Margin = new Thickness(0, 0, startMarginTop, 0);
                for (int i = 0; i < 20; i++)
                {
                    imgCurrentImage.Opacity += 0.05;
                    imgCurrentImage.Margin = new Thickness(0, 0, startMarginTop - (i), 0);
                    await Task.Delay(10);
                }
            }
            imageClickIsLocked = false;
        }
    }
}
