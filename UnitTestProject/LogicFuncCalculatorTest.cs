using LogicFuncs.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class LogicFuncCalculatorTest
    {
        LogicFuncCalculator calculator = new LogicFuncCalculator();
        [TestMethod]
        public void MainLogicalOperationsTest()
        {
            string funcInversion = "¬A";
            calculator.SetLogicFunc(funcInversion);
            List<bool> answerInversion = calculator.StartCalculate();
            List<bool> patternInversion = new List<bool>() { true, false };

            string funcConjunction = "A∧B";
            calculator.SetLogicFunc(funcConjunction);
            List<bool> answerConjunction = calculator.StartCalculate();
            List<bool> patternConjunction = new List<bool>() {false,false,false,true};

            string funcDisjunction = "A∨B";
            calculator.SetLogicFunc(funcDisjunction);
            List<bool> answerDisjunction = calculator.StartCalculate();
            List<bool> patternDisjunction = new List<bool>() { false, true, true, true };

            string funcImplication = "A→B";
            calculator.SetLogicFunc(funcImplication);
            List<bool> answerImplication = calculator.StartCalculate();
            List<bool> patternImplication = new List<bool>() { true, true, false, true };

            string funcEquivalence = "A≡B";
            calculator.SetLogicFunc(funcEquivalence);
            List<bool> answerEquivalence = calculator.StartCalculate();
            List<bool> patternEquivalence = new List<bool>() { true, false, false, true };

            string funcPierArrow = "A↓B";
            calculator.SetLogicFunc(funcPierArrow);
            List<bool> answerPierArrow = calculator.StartCalculate();
            List<bool> patternPierArrow = new List<bool>() { true, false, false, false};

            string funcSchaefferStroke = "A|B";
            calculator.SetLogicFunc(funcSchaefferStroke);
            List<bool> answerSchaefferStroke = calculator.StartCalculate();
            List<bool> patternSchaefferStroke = new List<bool>() { true, true, true, false };

            string funcSumModulo = "A⊕B";
            calculator.SetLogicFunc(funcSumModulo);
            List<bool> answerSumModulo = calculator.StartCalculate();
            List<bool> patternSumModulo = new List<bool>() { false, true, true, false };


            for (int i = 0; i < answerInversion.Count; i++)
            {
                Assert.AreEqual(answerInversion[i],patternInversion[i]);
            }

            for (int i = 0; i < answerConjunction.Count; i++)
            {
                Assert.AreEqual(answerConjunction[i], patternConjunction[i]);
            }

            for (int i = 0; i < answerDisjunction.Count; i++)
            {
                Assert.AreEqual(answerDisjunction[i], patternDisjunction[i]);
            }

            for (int i = 0; i < answerImplication.Count; i++)
            {
                Assert.AreEqual(answerImplication[i], patternImplication[i]);
            }

            for (int i = 0; i < answerEquivalence.Count; i++)
            {
                Assert.AreEqual(answerEquivalence[i], patternEquivalence[i]);
            }

            for (int i = 0; i < answerPierArrow.Count; i++)
            {
                Assert.AreEqual(answerPierArrow[i], patternPierArrow[i]);
            }

            for (int i = 0; i < answerSchaefferStroke.Count; i++)
            {
                Assert.AreEqual(answerSchaefferStroke[i], patternSchaefferStroke[i]);
            }

            for (int i = 0; i < answerSumModulo.Count; i++)
            {
                Assert.AreEqual(answerSumModulo[i], patternSumModulo[i]);
            }

        }


        [TestMethod]
        public void OneFuncTest()
        {
            string func = "¬(A∧B)→C";
            calculator.SetLogicFunc(func);
            List<bool> answer= calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { false, true, false, true, false, true,true,true };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }


        [TestMethod]
        public void TwoFuncTest()
        {
            string func = "A→¬(A∧B)→C";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { false, true, false, true, false, true, true, true };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }

        [TestMethod]
        public void ThreeFuncTest()
        {
            string func = "A→(¬(A∧B)→¬(¬C⊕A))";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, true, true, true, true, false, true, true };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }

        [TestMethod]
        public void FourFuncTest()
        {
            string func = "A→(C↓(¬(A∧B)→¬(¬C⊕A)))";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, true, true, true, false, false, false, false };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }

        [TestMethod]
        public void FiveFuncTest()
        {
            string func = "¬(¬A⊕¬B)→(C↓(¬(A∧B)→¬(¬C⊕A)))";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, false, true, true, true, true, false, false };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }

        [TestMethod]
        public void SixFuncTest()
        {
            string func = "(¬(¬A⊕¬B)→(C↓(¬(A∧B)→¬(¬C⊕A)))|C)";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, true, true, true, true, true, true, true };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }

        [TestMethod]
        public void SevenFuncTest()
        {
            string func = "(¬(¬A⊕¬B)→(C↓(¬(A∧B)→¬(¬C⊕A)))|C)⊕A∧B→C≡A";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, false, true, false, false, true, true, true };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }


        [TestMethod]
        public void EightFuncTest()
        {
            string func = "(¬(¬A⊕¬B)→(C↓(¬(A∧B)→¬(¬C⊕A)))| C)⊕A∧B→C≡A↔(A | B)";
            calculator.SetLogicFunc(func);
            List<bool> answer = calculator.StartCalculate();
            List<bool> pattern = new List<bool>() { true, false, true, false, false, true, false, false };

            for (int i = 0; i < answer.Count; i++)
            {
                Assert.AreEqual(answer[i], pattern[i]);
            }
        }
    }
}
