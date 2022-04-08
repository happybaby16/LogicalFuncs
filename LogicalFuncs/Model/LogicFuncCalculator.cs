﻿using System;
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
        string logicFunc = string.Empty;//Переменная, в которой хранится логическая функция
       
        Regex operations = new Regex(@"→|↔|¬|∧|∨|⊕|\||↓");//Операции
        Regex variables = new Regex(@"[0-1]");//Переменные
        Regex brackets = new Regex(@"\(|\)");//Скобки
        Regex variablesFunc = new Regex(@"[A-Z]|[a-z]");
        public List<string> variableNameList;

        List<Token> tokenList;//Список токенов логического выражения
        List<Token> tokenListFucn;//Списк токенов для логической функции

        List<bool> answerList;

        public List<List<int>> operationsResults; //Хранит значение промежуточных действий при решении логического выражения
        public List<string> operationsPriority;//Хранит строковое значение операций по убыванию решения
        public List<List<int>> varibleValues;//Хранит значение переменных функции

        public List<List<LogicalFuncsLogs>> CalculationLogs;


        public void SetLogicFunc(string logicFunc)
        {
            this.logicFunc = logicFunc.Trim().Replace(" ","");
            tokenListFucn = GetTokenListFunc(this.logicFunc);
            variableNameList = LogicFucnsParser.GetVariable(logicFunc);
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
            tokenListFucn = new List<Token>();
            for (int i = 0; i < funcString.Length; i++)
            {
                bool temp = variablesFunc.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFucn.Add(new Token(funcString[i], TypeValue.Variable));
                    continue;
                }
                temp = operations.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFucn.Add(new Token(funcString[i], TypeValue.Operation));
                    continue;
                }
                temp = brackets.IsMatch(Convert.ToString(funcString[i]));
                if (temp)
                {
                    tokenListFucn.Add(new Token(funcString[i], TypeValue.Brackets));
                    continue;
                }
            }
            return tokenListFucn;
        }


        /// <summary>
        /// /Функция, которая возвращает значение логического выражения
        /// </summary>
        /// <param name="logicExpression">Логическое выражение. Например: (0V1|1)</param>
        /// <returns></returns>
        public bool CalculateExpression(string logicExpression)
        {
            List<Token> tokenExpression = GetTokenList(logicExpression);
            Stack<bool> valuesStack = new Stack<bool>();
            Stack<Token> operationsStack = new Stack<Token>();

            operationsPriority = new List<string>();
            Stack<string> operationsPriorityString = new Stack<string>();

            

            for (int i = 0; i < tokenExpression.Count; i++)
            {
                if (tokenExpression[i].Type == TypeValue.Operation)
                {
                    if (operationsStack.Count!=0 && operationsStack.Peek().Value.ToString()!= "(" && operationsStack.Peek().Value.ToString() != ")" && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count > 1)
                    {
                        while (operationsStack.Count > 0 && tokenExpression[i].GetPriority >= operationsStack.Peek().GetPriority && valuesStack.Count > 1 )
                        {
                            if (operationsStack.Peek().Value.ToString() == "¬")
                            {
                                LogicalFuncsLogs temp = new LogicalFuncsLogs();

                                string resultPriority = $"{operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()}";
                                operationsPriorityString.Push(resultPriority);
                                operationsPriority.Add(resultPriority);


                                bool fVal = valuesStack.Pop();
                                bool resultCalc = LogicOperations.Inversion(fVal);
                                valuesStack.Push(resultCalc);
                                operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));
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
                                operationsPriority.Add(resultPriority);

                                bool sVal = valuesStack.Pop();
                                bool fVal = valuesStack.Pop();

                                temp.FirstValue = fVal;
                                temp.SecondValue = sVal;
                                temp.OperationValue = operationsStack.Peek();
                                temp.OperationPriority = resultPriority;

                                bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                                temp.ResultValue = resultCalc;
                                valuesStack.Push(resultCalc);
                                operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));

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
                    operationsPriorityString.Push(tokenListFucn[i].Value.ToString());
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
                                operationsPriority.Add(resultPriority);


                                bool fVal = valuesStack.Pop();
                                bool resultCalc = LogicOperations.Inversion(fVal);
                                valuesStack.Push(resultCalc);
                                operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));
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
                                operationsPriority.Add(resultPriority);

                                bool sVal = valuesStack.Pop();
                                bool fVal = valuesStack.Pop();

                                temp.FirstValue = fVal;
                                temp.SecondValue = sVal;
                                temp.OperationValue = operationsStack.Peek();
                                temp.OperationPriority = resultPriority;

                                bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                                temp.ResultValue = resultCalc;
                                valuesStack.Push(resultCalc);
                                operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));

                                CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                            }
                        }
                        if(operationsStack.Peek().Value.ToString() == "(") operationsStack.Pop();

                    }
                    continue;
                }
            }
            //Завершение решения после прохождения строки
            for (int i = 0;operationsStack.Count!=0; i++)
            {
                if (operationsStack.Peek().Value.ToString() == "¬")
                {
                    LogicalFuncsLogs temp = new LogicalFuncsLogs();

                    string resultPriority = $"{operationsStack.Peek().Value.ToString() + operationsPriorityString.Pop()}";
                    operationsPriorityString.Push(resultPriority);
                    operationsPriority.Add(resultPriority);


                    bool fVal = valuesStack.Pop();
                    bool resultCalc = LogicOperations.Inversion(fVal);
                    valuesStack.Push(resultCalc);
                    operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));
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
                    operationsPriority.Add(resultPriority);

                    bool sVal = valuesStack.Pop();
                    bool fVal = valuesStack.Pop();

                    temp.FirstValue = fVal;
                    temp.SecondValue = sVal;
                    temp.OperationValue = operationsStack.Peek();
                    temp.OperationPriority = resultPriority;

                    bool resultCalc = LogicOperations.Calculate(fVal, sVal, operationsStack.Pop().GetOperation);
                    temp.ResultValue = resultCalc;
                    valuesStack.Push(resultCalc);
                    operationsResults[operationsResults.Count - 1].Add(Convert.ToInt32(resultCalc));

                    CalculationLogs[CalculationLogs.Count - 1].Add(temp);
                }
            }
            return valuesStack.Pop();
        }

        public List<bool> StartCalculate()
        {
            CalculationLogs = new List<List<LogicalFuncsLogs>>();

            operationsResults = new List<List<int>>();
            varibleValues = new List<List<int>>();
            answerList = new List<bool>();
            for (int i = 0; i < Math.Pow(2, variableNameList.Count); i++)
            {
                CalculationLogs.Add(new List<LogicalFuncsLogs>());
                operationsResults.Add(new List<int>());
                varibleValues.Add(new List<int>());
                int counter = 0;
                string logicExpression = logicFunc;
                for (int k = variableNameList.Count-1; k != -1; k--)
                {
                    int bit = (i >> k) & 1;
                    operationsResults[i].Add(bit);
                    varibleValues[i].Add(bit);
                    logicExpression = logicExpression.Replace(variableNameList[counter], Convert.ToString(bit));
                    counter++;
                }
                answerList.Add(CalculateExpression(logicExpression));
                counter = 0;
            }
            return answerList;
        }


    }
}