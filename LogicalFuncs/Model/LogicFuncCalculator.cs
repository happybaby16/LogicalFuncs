using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicFuncs.Model
{

    public enum TypeValue { Operation, Variable, Brackets }
    public struct Token
    {
        public object Value { get; set; }
        public TypeValue Type { get; set; }
        public Token(object _value, TypeValue _type)
        {
            Value = _value;
            Type = _type;
        }


        /// <summary>
        /// Свойство, которое по значению Value указывает на приоритет выполнения
        /// </summary>
        public int GetPriority
        {
            get 
            {
                if(TypeValue.Operation == Type)
                switch (Value.ToString())
                {
                    case "¬":
                        return 1;
                    case "|":
                        return 2;
                    case "↓":
                        return 2;
                    case "∧":
                        return 3;
                    case "∨":
                        return 4;
                    case "⊕":
                        return 4;
                    case "→":
                        return 5;
                    case "↔":
                        return 6;
                }
                return -1;//Error code (Not operation)
            }
        }

        public Operation GetOperation
        {
            get
            {
                if (TypeValue.Operation == Type)
                switch (Value.ToString())
                {
                    case "|":
                        return Operation.SchaefferStroke;
                    case "↓":
                        return Operation.PierArrow;
                    case "¬":
                        return Operation.Inversion;
                    case "∧":
                        return Operation.Conjunction;
                    case "∨":
                        return Operation.Disjunction;
                    case "⊕":
                        return Operation.SumModulo;
                    case "→":
                        return Operation.Implication;
                    case "↔":
                        return Operation.Equivalence;
                }
                return Operation.NullOperation;//Error code (Not operation)
            }
        }
    }

    class LogicFuncCalculator
    {
        public string LogicalFunc { get; set; }//Переменная, в которой хранится логическая функция
        public List<string> VariableNames { get; set; }
        public List<bool> Answer { get; set; }
        public List<string> OperationsPriority { get; set; }//Хранит строковое значение операций по убыванию решения
        public List<List<LogicalFuncsLogs>> CalculationLogs { get; set; }

        Regex operations = new Regex(@"→|↔|¬|∧|∨|⊕|\||↓");//Операции
        Regex variables = new Regex(@"[0-1]");//Переменные
        Regex brackets = new Regex(@"\(|\)");//Скобки
        Regex variablesFunc = new Regex(@"[A-Z]|[a-z]");
     
        List<Token> tokenList;//Список токенов логического выражения
        List<Token> tokenListFunc;//Списк токенов для логической функции

        public void SetLogicFunc(string logicFunc)
        {
            this.LogicalFunc = logicFunc.Trim().Replace(" ","");
            tokenListFunc = GetTokenListFunc(this.LogicalFunc);
            VariableNames = LogicFucnsParser.GetVariable(logicFunc);
        }
        public List<Token> GetTokenList(string logicExpression)
        {
            tokenList = new List<Token>();
            for (int i = 0; i < logicExpression.Length; i++)
            {
                bool temp = variables.IsMatch(Convert.ToString(logicExpression[i]));
                if (temp)
                {
                    tokenList.Add(new Token(logicExpression[i], TypeValue.Variable));
                    continue;
                }
                temp = operations.IsMatch(Convert.ToString(logicExpression[i]));
                if (temp)
                {
                    tokenList.Add(new Token(logicExpression[i], TypeValue.Operation));
                    continue;
                }
                temp = brackets.IsMatch(Convert.ToString(logicExpression[i]));
                if (temp)
                {
                    tokenList.Add(new Token(logicExpression[i], TypeValue.Brackets));
                    continue;
                }
            }
            return tokenList;
        }

        public List<Token> GetTokenListFunc(string funcString)
        {
            tokenListFunc = new List<Token>();
            for (int i = 0; i < funcString.Length; i++)
            {
                bool temp = variablesFunc.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFunc.Add(new Token(funcString[i], TypeValue.Variable));
                    continue;
                }
                temp = operations.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFunc.Add(new Token(funcString[i], TypeValue.Operation));
                    continue;
                }
                temp = brackets.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFunc.Add(new Token(funcString[i], TypeValue.Brackets));
                    continue;
                }
            }
            return tokenListFunc;
        }


        /// <summary>
        /// /Функция, которая возвращает значение логического выражения
        /// </summary>
        /// <param name="logicExpression">Логическое выражение. Например: (0V1|1)</param>
        /// <returns></returns>
        public bool? CalculateExpression(string logicExpression)
        {
            List<Token> tokenExpression = GetTokenList(logicExpression);

            Stack<bool> valuesStack = new Stack<bool>();//Стек со значениями для решения
            Stack<Token> operationsStack = new Stack<Token>();//Стек с операциями для решения
            OperationsPriority = new List<string>();
            Stack<string> operationsPriorityString = new Stack<string>();
            try
            {
                for (int i = 0; i < tokenExpression.Count; i++)
                {
                    if (tokenExpression[i].Type == TypeValue.Operation)
                    {
                        if (operationsStack.Count != 0 && operationsStack.Peek().Value.ToString() != "(" && operationsStack.Peek().Value.ToString() != ")" && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count > 1)
                        {
                            while (operationsStack.Count > 0 && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count > 1)
                            {
                                if (operationsStack.Peek().Value.ToString() == "¬")
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string resultPriority = $"{operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()}";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool fVal = valuesStack.Pop();
                                    bool resultCalc = LogicOperations.Inversion(fVal);
                                    valuesStack.Push(resultCalc);
                                    temp.FirstValue = fVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.ResultValue = resultCalc;
                                    temp.OperationPriority = resultPriority;
                                    operationsStack.Pop();

                                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                                }
                                else
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string previousOperation = operationsPriorityString.Pop();
                                    string sOperation = operationsStack.Peek().Value.ToString();
                                    string fOperation = operationsPriorityString.Pop();
                                    string resultPriority = $"{fOperation + sOperation}({previousOperation})";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool sVal = valuesStack.Pop();
                                    bool fVal = valuesStack.Pop();

                                    temp.FirstValue = fVal;
                                    temp.SecondValue = sVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.OperationPriority = resultPriority;

                                    bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                                    temp.ResultValue = resultCalc;
                                    valuesStack.Push(resultCalc);

                                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                                }
                            }
                            operationsStack.Push(tokenExpression[i]);
                            continue;
                        }
                        else
                        {
                            operationsStack.Push(tokenExpression[i]);
                            continue;
                        }
                    }

                    if (tokenExpression[i].Type == TypeValue.Variable)
                    {
                        valuesStack.Push(Convert.ToBoolean(Convert.ToInt32(tokenExpression[i].Value.ToString())));
                        operationsPriorityString.Push(tokenListFunc[i].Value.ToString());
                        continue;
                    }
                    if (tokenExpression[i].Type == TypeValue.Brackets)
                    {
                        if (tokenExpression[i].Value.ToString() == "(") operationsStack.Push(tokenExpression[i]);
                        if (tokenExpression[i].Value.ToString() == ")")
                        {
                            while (operationsStack.Peek().Value.ToString() != "(" && valuesStack.Count > 1)
                            {
                                if (operationsStack.Peek().Value.ToString() == "¬")
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string resultPriority = $"{operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()}";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool fVal = valuesStack.Pop();
                                    bool resultCalc = LogicOperations.Inversion(fVal);
                                    valuesStack.Push(resultCalc);
                                    temp.FirstValue = fVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.ResultValue = resultCalc;
                                    temp.OperationPriority = resultPriority;
                                    operationsStack.Pop();

                                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                                }
                                else
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string previousOperation = operationsPriorityString.Pop();
                                    string sOperation = operationsStack.Peek().Value.ToString();
                                    string fOperation = operationsPriorityString.Pop();
                                    string resultPriority = $"{fOperation + sOperation}({previousOperation})";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool sVal = valuesStack.Pop();
                                    bool fVal = valuesStack.Pop();

                                    temp.FirstValue = fVal;
                                    temp.SecondValue = sVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.OperationPriority = resultPriority;

                                    bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                                    temp.ResultValue = resultCalc;
                                    valuesStack.Push(resultCalc);

                                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                                }
                            }
                            if (operationsStack.Peek().Value.ToString() == "(") operationsStack.Pop();

                        }
                        continue;
                    }
                }
                //Завершение решения после прохождения строки
                for (int i = 0; operationsStack.Count != 0; i++)
                {
                    if (operationsStack.Peek().Value.ToString() == "¬")
                    {
                        LogicalFuncsLogs temp = new LogicalFuncsLogs();

                        string resultPriority = $"{operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()}";
                        operationsPriorityString.Push(resultPriority);
                        OperationsPriority.Add(resultPriority);

                        bool fVal = valuesStack.Pop();
                        bool resultCalc = LogicOperations.Inversion(fVal);
                        valuesStack.Push(resultCalc);
                        temp.FirstValue = fVal;
                        temp.OperationValue = operationsStack.Peek();
                        temp.ResultValue = resultCalc;
                        temp.OperationPriority = resultPriority;
                        operationsStack.Pop();

                        CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                    }
                    else
                    {
                        LogicalFuncsLogs temp = new LogicalFuncsLogs();

                        string previousOperation = operationsPriorityString.Pop();
                        string sOperation = operationsStack.Peek().Value.ToString();
                        string fOperation = operationsPriorityString.Pop();
                        string resultPriority = $"{fOperation + sOperation}({previousOperation})";
                        operationsPriorityString.Push(resultPriority);
                        OperationsPriority.Add(resultPriority);

                        bool sVal = valuesStack.Pop();
                        bool fVal = valuesStack.Pop();

                        temp.FirstValue = fVal;
                        temp.SecondValue = sVal;
                        temp.OperationValue = operationsStack.Peek();
                        temp.OperationPriority = resultPriority;

                        bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                        temp.ResultValue = resultCalc;
                        valuesStack.Push(resultCalc);

                        CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                    }
                }
            }
            catch
            {
                return null;
            }
            if (valuesStack.Count == 1 && operationsStack.Count==0)
            {
                return valuesStack.Pop();
            }
            return null;
           
        }

        public List<bool> StartCalculate()
        {
            CalculationLogs = new List<List<LogicalFuncsLogs>>();
            Answer = new List<bool>();
            for (int i = 0; i < Math.Pow(2, VariableNames.Count); i++)
            {
                CalculationLogs.Add(new List<LogicalFuncsLogs>());
                int counter = 0;
                string logicExpression = LogicalFunc;
                for (int k = VariableNames.Count - 1; k != -1; k--)
                {
                    int bit = (i >> k) & 1;
                    logicExpression = logicExpression.Replace(VariableNames[counter], Convert.ToString(bit));
                    counter++;
                }
                bool? answerExpression = CalculateExpression(logicExpression);
                if (answerExpression != null)
                {
                    Answer.Add(Convert.ToBoolean(answerExpression));
                }
                else
                {
                    //Ошибка в вводе логической функции
                    Answer = null;
                    return null;
                }
                counter = 0;
            }
            return Answer;
        }


    }
}
