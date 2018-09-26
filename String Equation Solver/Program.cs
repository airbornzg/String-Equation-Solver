using System;
using System.Text.RegularExpressions;
/*
 |     Course:  Application Development with .NET
 |     Assignment:  Assignment 1
 |     Author:  Zuguang Chen
 |     Student Number:  13106594
 |     Language:  C#
 |     Due Date:  17th Sep, 2018
 * 
*/
namespace StringEquationSolver
{
    class Solver
    {
        private double x = 0, b = 0, c = 0;

        public Solver(string strLeft, string strRight)
        {
            //The regular expression pattern to find the coefficinet
            string regexB = "([+-][0-9]*)[*]*[xX](?!\\^)(?![*/])";
            string regexBD = "([+-]+)([0-9]*)([xX]+)([/]+)([+-]*[0-9]+)";
            string regexC = "(?<![*/]+)([+-][0-9]+)(?![xX])(?![(])(?![*])(?![/])(?![0-9]*[*xX])(?![*/]*[0-9]+[*/]*)";
            string regexCM = "([+-]*[0-9]+)([(*]+)([+-]*[0-9]+)(?![xX]+)";
            string regexCD = "([+-]*[0-9]+)([/]+)([+-]*[0-9]+)(?![xX]+)";

            //Left equation
            double bLeft = FindAddMinusCoeff(strLeft, regexB);
            double cLeft = FindAddMinusCoeff(strLeft, regexC);
            double bLeftD = FindDivisionCoeff(strLeft, regexBD);
            double cLeftM = FindMultiCoeff(strLeft, regexCM);
            double cLeftD = FindDivisionCoeff(strLeft, regexCD);

            //Right equation
            double bRight = FindAddMinusCoeff(strRight, regexB);
            double cRight = FindAddMinusCoeff(strRight, regexC);
            double bRightD = FindDivisionCoeff(strRight, regexBD);
            double cRightM = FindMultiCoeff(strRight, regexCM);
            double cRightD = FindDivisionCoeff(strRight, regexCD);

            //Add all the coeffcient parts together and get final coefficient
            b = (bLeft + bLeftD) - (bRight + bRightD);
            c = (cLeft + cLeftM + cLeftD) - (cRight + cRightM + cRightD);

            //DivideByZeroException
            if (b == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                //bx+c=0 general root solver formula
                x = -(c / b);

                //Print out the result
                Console.WriteLine("X = {0}", x);
                Console.ReadKey();
            }
        }

        //Addition and Minus calculation handler
        public double FindAddMinusCoeff(string str, string regex)
        {
            //Check first sign of string
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }
            string coeff = "+0";
            double coeSum = 0;

            //Find the matched pattern
            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                coeff = m.Groups[1].Value;
                if (coeff == "-") { coeff = "-1"; }
                if (coeff == "+") { coeff = "1"; }

                //Sums up the coefficient
                coeSum = coeSum + Double.Parse(coeff);

                //Go to next matched pattern
                m = m.NextMatch();
            }
            return coeSum;
        }

        //Multiplication calculation handler
        public double FindMultiCoeff(string str, string regex)
        {
            //Check first sign of string
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }
            string coeff1 = "+0";
            string coeff3 = "+0";
            double coeMul = 0;
            double coeSum = 0;

            //Find the matched pattern
            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                coeff1 = m.Groups[1].Value;
                coeff3 = m.Groups[3].Value;

                //Do multiplication process
                coeMul = Double.Parse(coeff1) * Double.Parse(coeff3);

                //Sums up the coefficient
                coeSum = coeSum + coeMul;

                //Go to next matched pattern
                m = m.NextMatch();
            }
            return coeSum;
        }

        //Division calculation handler
        public double FindDivisionCoeff(string str, string regex)
        {
            //Check first sign of string
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }
            string coeff1 = "+0";
            string coeff3 = "+0";
            double coeMul = 0;
            double coeSum = 0;

            //Find the matched pattern
            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                if (m.Groups[1].Value == "+" && (m.Groups[3].Value == "x" || m.Groups[3].Value == "X"))
                {
                    if (m.Groups[2].Value == "")
                    {
                        coeff1 = "1";
                        coeff3 = m.Groups[5].Value;
                    }
                    else
                    {
                        coeff1 = m.Groups[2].Value;
                        coeff3 = m.Groups[5].Value;
                    }
                }
                else if (m.Groups[1].Value == "-" && (m.Groups[3].Value == "x" || m.Groups[3].Value == "X"))
                {
                    if (m.Groups[2].Value == "")
                    {
                        coeff1 = "-1";
                        coeff3 = m.Groups[5].Value;
                    }
                    else
                    {
                        coeff1 = "-" + m.Groups[2].Value;
                        coeff3 = m.Groups[5].Value;
                    }
                }
                else
                {
                    coeff1 = m.Groups[1].Value;
                    coeff3 = m.Groups[3].Value;
                }

                //Do division process
                coeMul = Double.Parse(coeff1) / Double.Parse(coeff3);

                //Sums up the coefficient
                coeSum = coeSum + coeMul;

                //Go to next matched pattern
                m = m.NextMatch();
            }
            return coeSum;
        }
    }
    // Class for main()
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Variable declaration
            string strLeft, strRight;
            string equ;

            //Exception Handling
            try
            {
                if (args.Length > 1)
                {
                    //Remove all the space and blank from the string
                    equ = string.Join("", args);
                }
                else
                {
                    equ = args[0];
                }

                //Check if contains x and = sign
                if ((Regex.IsMatch(equ, "x") || Regex.IsMatch(equ, "X")) && Regex.IsMatch(equ, "=") && Regex.IsMatch(equ, "/0") == false)
                {
                    //Print out the input equation
                    Console.WriteLine("The equation is: {0}", equ);

                    //Divide the equation into 2 respect to the equal sign
                    string[] equDivided = equ.Split('=');
                    strLeft = equDivided[0];
                    strRight = equDivided[1];

                    //Solve this equation
                    Solver solver = new Solver(strLeft, strRight);
                }
                else if (Regex.IsMatch(equ, "/0") == false)
                {
                    throw new Exception("Function need \"X\" and \"=\" sign");
                }
                else if (Regex.IsMatch(equ, "/0"))
                {
                    throw new DivideByZeroException();
                }
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("The Error is {0}", e);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("The Error is {0}", e);
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("Please close the window!");
            }
        }
    }
}