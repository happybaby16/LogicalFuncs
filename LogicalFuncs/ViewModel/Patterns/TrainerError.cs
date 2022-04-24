using LogicFuncs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalFuncs.ViewModel.Patterns
{
    public enum TypeError { ErrorTable=0, ErrorClasses=1, ErrorCompleteness=2, ErrorAnswerNull=3, ErrorFullFunc=4}
    public class TrainerError
    {
        public TypeError Type { get; set; }
        public string ErrorMessage { get; set; }
        public TrainerError(string logicalFunc, LogicalFuncsLogs errorObject, int column, int row)
        {
            Type = TypeError.ErrorTable;
            ErrorMessage = $"Заполнение таблицы функции {logicalFunc}. Строка {row}, столбец {column}: {errorObject.OperationPriority}={Convert.ToInt32(errorObject.FirstValue)}{errorObject.OperationValue.Value}{Convert.ToInt32(errorObject.SecondValue)}";
        }

        public TrainerError(string logicalFunc, int column, int row)
        {
            Type = TypeError.ErrorTable;
            ErrorMessage = $"Заполнение таблицы функции {logicalFunc}. Строка {row}, столбец {column}: ошибка заполнения данных";
        }

        public TrainerError(string logicalFunc, string classes, string classesAbout)
        {
            Type = TypeError.ErrorClasses;
            ErrorMessage = $"Определение замкнутого класса {classes} функции {logicalFunc}: {classesAbout}.";
        }

        public TrainerError(string logicalFunc, TypeError typeError, string messageError)
        {
            Type = typeError;
            ErrorMessage = messageError;
        }

        public TrainerError(TypeError typeError, string messageError)
        {
            Type = typeError;
            ErrorMessage = messageError;
        }
    }
}
