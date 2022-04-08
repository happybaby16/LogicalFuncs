using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFuncs.Model
{
    public class LogicalFuncsLogs
    {
        public bool FirstValue { get; set; }
        public bool SecondValue { get; set; }
        public Token OperationValue { get; set; }
        public bool ResultValue { get; set; }
        public string OperationPriority { get; set; }
    }
}
