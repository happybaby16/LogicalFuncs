using LogicFuncs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalFuncs.ViewModel.Patterns
{
    public enum TypeError { ErrorTable=0, ErrorClasses=1}
    public class TrainerError
    {
        public TypeError Type { get; set; }
        public TrainerError(LogicalFuncsLogs errorObject)
        {
            Type = TypeError.ErrorTable;

        }

        public TrainerError(string d)
        {
        }
    }
}
