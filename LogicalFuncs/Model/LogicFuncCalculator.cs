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
                        case "˜":
                            return 1;
                        case "!":
                            return 1;
                        case "|":
                            return 2;
                        case "↓":
                            return 2;
                        case "∧":
                            return 3;
                        case "•":
                            return 3;
                        case "&":
                            return 3;
                        case "∨":
                            return 4;
                        case "+":
                            return 4;
                        case "⊕":
                            return 4;
                        case "→":
                            return 5;
                        case "⇒":
                            return 5;
                        case "⊃":
                            return 5;
                        case "↔":
                            return 6;
                        case "⇔":
                            return 6;
                        case "≡":
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
                    case "˜":
                        return Operation.Inversion;
                    case "!":
                        return Operation.Inversion;
                    case "∧":
                        return Operation.Conjunction;
                    case "•":
                        return Operation.Conjunction;
                    case "&":
                        return Operation.Conjunction;
                    case "∨":
                        return Operation.Disjunction;
                    case "+":
                        return Operation.Disjunction;
                    case "⊕":
                        return Operation.SumModulo;
                    case "⊻":
                        return Operation.SumModulo;
                    case "→":
                        return Operation.Implication;
                    case "⇒":
                        return Operation.Implication;
                    case "⊃":
                        return Operation.Implication;
                    case "↔":
                        return Operation.Equivalence;
                    case "⇔":
                        return Operation.Equivalence;
                    case "≡":
                        return Operation.Equivalence;
                    }
                return Operation.NullOperation;//Error code (Not operation)
            }
        }
    }

    public class LogicFuncCalculator
    {
        public string LogicalFunc { get; set; }//Содержит входную функцию
        public List<string> VariableNames { get; set; }
        public List<bool> Answer { get; set; }//Содержит вектор функции (ответ)
        public List<string> OperationsPriority { get; set; }//Содержит последовательность действий
        public List<List<LogicalFuncsLogs>> CalculationLogs { get; set; }
        public bool IsUserHaveAnswer 
        {
            get
            {
                if (operations.IsMatch(LogicalFunc)||variablesFunc.Matches(LogicalFunc).Count!=LogicalFunc.Length) return false;
                return true;
            }
        }//Необходимо, если у пользователя есть вектор функции. Т.е. у пользователя нет конкрентной функции, но есть на неё ответ
        
        Regex operations = new Regex(@"→|↔|¬|∧|∨|⊕|\||↓|⇒|⊃|⇔|≡|˜|!|•|&|\+|⊻");//Операции
        Regex variables = new Regex(@"[0-1]");//Переменные
        Regex brackets = new Regex(@"\(|\)");//Скобки
        Regex variablesFunc = new Regex(@"[A-Z]|[a-z]|[0,1]");
     
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
                        if (operationsStack.Count != 0 && operationsStack.Peek().Value.ToString() != "(" && operationsStack.Peek().Value.ToString() != ")" && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count >= 1)
                        {
                            while (operationsStack.Count > 0 && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count >= 1 && operationsStack.Peek().Value.ToString() != "(" && operationsStack.Peek().Value.ToString() != ")")
                            {
                                if (operationsStack.Peek().GetOperation == Operation.Inversion)
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string resultPriority = $"({operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()})";
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
                                else if (valuesStack.Count >= 2)
                                {
                                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                    string previousOperation = operationsPriorityString.Pop();
                                    string sOperation = operationsStack.Peek().Value.ToString();
                                    string fOperation = operationsPriorityString.Pop();
                                    string resultPriority = $"({fOperation + sOperation}{previousOperation})";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool sVal = valuesStack.Pop();
                                    bool fVal = valuesStack.Pop();

                                    temp.FirstValue = fVal;
                                    temp.SecondValue = sVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.OperationPriority = resultPriority;

                                    Token operation = operationsStack.Pop();
                                    bool resultCalc = LogicOperations.Calculate(fVal, sVal, operation.GetOperation);
                                    temp.ResultValue = resultCalc;
                                    valuesStack.Push(resultCalc);

                                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                                }
                                else
                                {
                                    return null;
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
                                if (operationsStack.Peek().GetOperation == Operation.Inversion)
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
                                    string resultPriority = $"({fOperation + sOperation}{previousOperation})";
                                    operationsPriorityString.Push(resultPriority);
                                    OperationsPriority.Add(resultPriority);

                                    bool sVal = valuesStack.Pop();
                                    bool fVal = valuesStack.Pop();

                                    temp.FirstValue = fVal;
                                    temp.SecondValue = sVal;
                                    temp.OperationValue = operationsStack.Peek();
                                    temp.OperationPriority = resultPriority;

                                    Token operation = operationsStack.Pop();
                                    bool resultCalc = LogicOperations.Calculate(fVal, sVal, operation.GetOperation);
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
                    if (operationsStack.Peek().GetOperation == Operation.Inversion)
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
                        string resultPriority = $"({fOperation + sOperation}{previousOperation})";
                        operationsPriorityString.Push(resultPriority);
                        OperationsPriority.Add(resultPriority);

                        bool sVal = valuesStack.Pop();
                        bool fVal = valuesStack.Pop();

                        temp.FirstValue = fVal;
                        temp.SecondValue = sVal;
                        temp.OperationValue = operationsStack.Peek();
                        temp.OperationPriority = resultPriority;

                        Token operation = operationsStack.Pop();
                        bool resultCalc = LogicOperations.Calculate(fVal, sVal, operation.GetOperation);
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
                    CalculationLogs[CalculationLogs.Count - 1].Add(new LogicalFuncsLogs() { ResultValue=Convert.ToBoolean(bit)});
                    logicExpression = logicExpression.Replace(VariableNames[counter], Convert.ToString(bit));
                    counter++;
                }
                //Считает значение логического выражения если у пользователя нет вектора фукнции
                if (!IsUserHaveAnswer)
                {
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
                }
                counter = 0;
            }
            return Answer;
        }

        /// <summary>
        /// Свойство, определяющее сохраняет ли булевая функция ноль
        /// </summary>
        public bool? IsSavedZero
        {
            get
            {
                if (Answer == null || Answer.Count == 0) return null;
                if (Answer[0] == false) return true;
                else return false; 
            }
        }

        /// <summary>
        /// Свойство, определяющее сохраняет ли булевая функция единицу
        /// </summary>
        public bool? IsSavedOne
        {
            get
            {
                if (Answer == null || Answer.Count == 0) return null;
                if (Answer[Answer.Count - 1] == true) return true;
                else return false;
            }
        }

        /// <summary>
        /// Свойство, определяющее является ли булевая функция самодвойственной
        /// </summary>
        public bool? IsSelfDual
        {
            get
            {
                if (Answer == null || Answer.Count == 0) return null;
                for (int i = 0; i < Answer.Count / 2; i++)
                {
                    if (Answer[i] == Answer[Answer.Count - 1 - i]) return false;
                }
                for (int i = Answer.Count / 2; i < Answer.Count; i++)
                {
                    if (Answer[i] == Answer[Answer.Count - 1 - i]) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Свойство, определяющее является ли булевая функция монотонной
        /// </summary>
        public bool? IsMonotony
        {
            get
            {
                if (Answer == null || Answer.Count == 0) return null;
                int step = Convert.ToInt32(Math.Pow(2, VariableNames.Count - 1));
                for (int i = 0; i < VariableNames.Count; i++)
                {
                    for (int k = 0; step + k < Answer.Count; k++)
                    {
                        if (Convert.ToInt32(Answer[k]) > Convert.ToInt32(Answer[step + k])) return false;
                    }
                    step /= 2;
                }
                return true;
            }
        }

        /// <summary>
        /// Свойство, определяющее является ли булевая функция линейной
        /// </summary>
        public bool? IsLiner
        {
            get
            {
                if (Answer == null || Answer.Count == 0) return null;
                List<List<bool>> polinomData = new List<List<bool>>();
                polinomData.Add(Answer);
                for (int i = 1; i < Answer.Count; i++)
                {
                    polinomData.Add(new List<bool>());
                    for (int k = 0; k < polinomData[i - 1].Count - 1; k++)
                    {
                        polinomData[i].Add(LogicOperations.Calculate(polinomData[i - 1][k], polinomData[i - 1][k + 1], Operation.SumModulo));
                    }
                }

                for (int i = 0; i < polinomData.Count; i++)
                {
                    List<int> tempBits = new List<int>();
                    for (int k = VariableNames.Count - 1; k != -1; k--)
                    {
                        int bit = (i >> k) & 1;
                        if (bit == 1)
                        {
                            tempBits.Add(bit);
                        }
                    }

                    if (tempBits.Count < 2)
                    {
                        continue;
                    }
                    else if (polinomData[i][0] == true)
                    { 
                        return false;
                    }
                }
                return true;
            }
        }

        public List<bool> GetClasses
        {
            get
            {
                List<bool> classes = new List<bool>();
                classes.Add(Convert.ToBoolean(IsSavedZero));
                classes.Add(Convert.ToBoolean(IsSavedOne));
                classes.Add(Convert.ToBoolean(IsSelfDual));
                classes.Add(Convert.ToBoolean(IsLiner));
                classes.Add(Convert.ToBoolean(IsMonotony));
                return classes;
            }
        }
    }
}
