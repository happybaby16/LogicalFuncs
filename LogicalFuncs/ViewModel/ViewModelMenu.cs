using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicalFuncs.ViewModel
{
    public class ViewModelMenu:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Menu animation
        double widthMenu = 150; //Menu is open 
        public double WidthMenu
        {
            get => widthMenu;
            set => widthMenu = value;
        }
        public bool IsOpenMenu { get; set; } = true;
        
        public RoutedCommand MenuMoveCommand { get; } = new RoutedCommand();
        public CommandBinding MenuMoveBinding;
        bool MenuMoveCanExecute { get; set; } = true;
        private async void MenuMove(object sender, EventArgs e)
        {
            if (MenuMoveCanExecute)
            {
                MenuMoveCanExecute = false;
                if (IsOpenMenu)
                {
                    for (int i = 0; widthMenu > 60; i++)
                    {
                        widthMenu-=2;
                        await Task.Delay(1);
                        PropertyChanged(this, new PropertyChangedEventArgs("WidthMenu"));
                    }
                    IsOpenMenu = false;
                }
                else
                {
                    for (int i = 0; widthMenu < 150; i++)
                    {
                        widthMenu+=2;
                        await Task.Delay(1);
                        PropertyChanged(this, new PropertyChangedEventArgs("WidthMenu"));
                    }
                    IsOpenMenu = true;
                }
            }
            MenuMoveCanExecute = true;
        }
        #endregion

        public ViewModelMenu()
        {
            MenuMoveBinding = new CommandBinding(MenuMoveCommand);
            MenuMoveBinding.Executed += MenuMove;
        }


    }
}
