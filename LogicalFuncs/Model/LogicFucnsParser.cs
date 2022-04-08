using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicFuncs.Model
{

    /// <summary>
    /// Определяет количество перменных в функции и возвращает их строку
    /// *(A∨B→B). Вернёт List<string>{"A","B"} 
    /// </summary>
    public static class LogicFucnsParser
    {
        /// <summary>
        /// Обозначение переменных одной буквой большого или маленького регистра
        /// </summary>
        static Regex variables = new Regex(@"[A-Z]|[a-z]");
        public static List<string> GetVariable(string LogicFunc)
        {
            List<string> variableNames = new List<string>();
            List<Match> collection = variables.Matches(LogicFunc).ToList();
            foreach (Match match in collection)
            {
                if (!variableNames.Contains(match.Value))
                {
                    variableNames.Add(match.Value);
                }
            }
            variableNames.Sort();
            return variableNames;
        }
    }
}
