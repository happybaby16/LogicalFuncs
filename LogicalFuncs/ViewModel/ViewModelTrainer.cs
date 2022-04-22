using LogicFuncs.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LogicalFuncs.ViewModel
{
    public class ViewModelTrainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        List<string> inputLogicalFuncs;
        public List<string> InputLogicalFuncs
        {
            get => inputLogicalFuncs;
            set
            {
                inputLogicalFuncs = value;
                CalculateResult();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GetResultCalculation"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPage"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxPages"));
            }
        }
        public int CountInputLogicalFuncs
        {
            get => inputLogicalFuncs.Count;
        }
        List<LogicFuncCalculator> resultCalculation = new List<LogicFuncCalculator>();
        public List<LogicFuncCalculator> GetResultCalculation
        {
            get => resultCalculation;
            set { }
        }
        private void CalculateResult()
        {
            resultCalculation = new List<LogicFuncCalculator>();
            foreach (string logicalFunc in InputLogicalFuncs)
            {
                resultCalculation.Add(new LogicFuncCalculator());
                resultCalculation[resultCalculation.Count - 1].SetLogicFunc(logicalFunc);
                if (resultCalculation[resultCalculation.Count - 1].StartCalculate() == null&&
                    resultCalculation[resultCalculation.Count - 1].IsUserHaveAnswer!=true)
                {
                    MessageBox.Show($"Ошибка в написании логической функции: {logicalFunc}");
                    resultCalculation.Clear();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxPages"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLastPage"));
                    break;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsVisiblePaginationButtons"));
        }

        #region Пагинация
        int currentPage = 0;
        public int CurrentPage {
            get => currentPage;
            set
            {
                if (value > MaxPages) { currentPage = 0; }
                else if (value < 0) { currentPage = MaxPages; }
                else { currentPage = value; }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLastPage"));
            }
        }
        public int MaxPages
        {
            get 
            {
                return GetResultCalculation.Count - 1;
            }
        }
        public bool IsLastPage
        {
            get 
            {
                if (CurrentPage == MaxPages && MaxPages!=-1) return true;
                else return false;
            }
        }
        public bool IsVisiblePaginationButtons
        {
            get 
            {
                if (GetResultCalculation != null && GetResultCalculation.Count > 1) return true;
                else return false;
            }
        }
        #endregion

        #region Работа с классами функций
        bool isClassesOn = false;
        public bool IsClassesOn 
        {
            get
            {
                return isClassesOn;
            }
            set
            {
                isClassesOn = value;
                isEnableAddFuncButton = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsEnableAddFuncButton"));
            }
        }
        bool isEnableAddFuncButton = false;
        public bool IsEnableAddFuncButton
        {
            get
            {
                return isEnableAddFuncButton;
            }
            set
            {
                isEnableAddFuncButton = value;
            }
        }
        #endregion


    }
}
