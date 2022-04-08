using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFuncs.Model
{
    /// <summary>
    /// Перечисление, с помощью которого будет определяться какое дейсвие выполнять
    /// </summary>
    public enum Operation { Inversion, Conjunction, Disjunction, 
                            Implication, Equivalence, SumModulo, 
                            PierArrow, SchaefferStroke, NullOperation }
    /// <summary>
    /// Содержит в себе логические операции
    /// </summary>
    public static class LogicOperations
    {
        public static bool Inversion(bool val)
        {
            return !val;
        }
        public static bool Conjunction(bool fval, bool sval)
        {
            return fval & sval;
        }
        public static bool Disjunction(bool fval, bool sval)
        {
            return fval | sval;
        }
        public static bool Implication(bool fval, bool sval)
        {
            return !fval || sval;
        }
        public static bool Equivalence(bool fval, bool sval)
        {
            return fval == sval;
        }
        public static bool SumModulo(bool fval, bool sval)
        {
            return fval != sval;
        }
        public static bool PierArrow(bool fval, bool sval)
        {
            return !Disjunction(fval, sval);
        }
        public static bool SchaefferStroke(bool fval, bool sval)
        {
            return !Conjunction(fval, sval);
        }

        /// <summary>
        /// Вычисляет логическое значение в соответствии с операцией
        /// </summary>
        public static bool Calculate(bool fVal, bool sVal, Operation operation)
        {
            switch (operation)
            {
                case Operation.Inversion:
                    return Inversion(fVal);
                case Operation.Conjunction:
                    return Conjunction(fVal, sVal);
                case Operation.Disjunction:
                    return Disjunction(fVal, sVal);
                case Operation.Implication:
                    return Implication(fVal, sVal);
                case Operation.Equivalence:
                    return Equivalence(fVal, sVal);
                case Operation.PierArrow:
                    return PierArrow(fVal, sVal);
                case Operation.SumModulo:
                    return SumModulo(fVal, sVal);
                case Operation.SchaefferStroke:
                    return SchaefferStroke(fVal, sVal);
            }
            return false;
        }

    }
}
