using System;
using System.Text.RegularExpressions;


namespace assignment1_ver3
{
    class Solver
    {
        public double FindAddMinusCoeff(string str, string regex)
        {
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }
            //char[] c = str.ToCharArray();
            string coeff = "+0";
            double coeSum = 0;

            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                coeff = m.Groups[1].Value;
                if (coeff == "-") { coeff = "-1"; }
                if (coeff == "+") { coeff = "1"; }
                coeSum = coeSum + Double.Parse(coeff);
                m = m.NextMatch();
            }
            return coeSum;
        }

        public double FindMultiCoeff(string str, string regex)
        {
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }
            string coeff1 = "+0";
            string coeff3 = "+0";
            double coeMul = 0;
            double coeSum = 0;
            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                coeff1 = m.Groups[1].Value;
                coeff3 = m.Groups[3].Value;
                coeMul = Double.Parse(coeff1) * Double.Parse(coeff3);
                coeSum = coeSum + coeMul;
                m = m.NextMatch();
            }
            return coeSum;
        }

        public double FindDivisionCoeff(string str, string regex)
        {
            if (str[0] != '+' && str[0] != '-')
            {
                str = "+" + str;
            }            
            string coeff1 = "+0";
            string coeff3 = "+0";
            double coeMul = 0;
            double coeSum = 0;
            Match m = Regex.Match(str, regex);
            while (m.Success)
            {
                if (m.Groups[1].Value == "+x")
                {
                    coeff1 = "1";
                    coeff3 = m.Groups[3].Value;
                }
                else if (m.Groups[1].Value == "-x")
                {
                    coeff1 = "-1";
                    coeff3 = m.Groups[3].Value;
                }
                else
                {
                    coeff1 = m.Groups[1].Value;
                    coeff3 = m.Groups[3].Value;
                }
                coeMul = Double.Parse(coeff1) / Double.Parse(coeff3);
                coeSum = coeSum + coeMul;
                m = m.NextMatch();
            }
            return coeSum;
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Variable declaration
            string strLeft, strRight;
            double x = 0, b = 0, c = 0;
            string equ;
            //Exception Handling
            try
            {
                if (args[0] == "calc")
                {
                    if (args.Length > 2)
                    {
                        equ = string.Join("", args);
                        equ = equ.Replace("calc", "");
                    }
                    else
                    {
                        Console.WriteLine("Input Equation is {0}", args[1]);
                        equ = args[1];
                    }
                    Console.WriteLine("The equation is: {0}", equ);
                    //Divide the equation into 2 respect to the equal sign
                    string[] equDivided = equ.Split('=');
                    strLeft = equDivided[0];
                    strRight = equDivided[1];

                    //The regular expression pattern to find the coefficinet
                    string regexB = "([+-][0-9]*)[*]*x(?!\\^)(?![*/])";
                    string regexBD = "([+-]+[x]+)([/]+)([+-]*[0-9]+)";
                    string regexC = "(?<![*/]+)([+-][0-9]+)(?!x)(?![*])(?![/])(?![0-9]*[*x])(?![*/]*[0-9]+[*/]*)";
                    string regexCM = "([+-]*[0-9]+)([*]+)([+-]*[0-9]+)";
                    string regexCD = "([+-]*[0-9]+)([/]+)([+-]*[0-9]+)";

                    Solver solver = new Solver();

                    //Left equation
                    double bLeft = solver.FindAddMinusCoeff(strLeft, regexB);
                    double cLeft = solver.FindAddMinusCoeff(strLeft, regexC);

                    double bLeftD = solver.FindDivisionCoeff(strLeft, regexBD);
                    double cLeftM = solver.FindMultiCoeff(strLeft, regexCM);
                    double cLeftD = solver.FindDivisionCoeff(strLeft, regexCD);

                    //Right equation
                    double bRight = solver.FindAddMinusCoeff(strRight, regexB);
                    double cRight = solver.FindAddMinusCoeff(strRight, regexC);

                    double bRightD = solver.FindDivisionCoeff(strRight, regexBD);
                    double cRightM = solver.FindMultiCoeff(strRight, regexCM);
                    double cRightD = solver.FindDivisionCoeff(strRight, regexCD);

                    Console.WriteLine("Coefficient bl = {0}", bLeft);
                    Console.WriteLine("Coefficient bld = {0}", bLeftD);
                    Console.WriteLine("Coefficient brd = {0}", bRightD);
                    Console.WriteLine("Coefficient cl = {0}", cLeft);
                    Console.WriteLine("Coefficient cr = {0}", cRight);
                    Console.WriteLine("Coefficient clm = {0}", cLeftM);
                    Console.WriteLine("Coefficient cld = {0}", cLeftD);
                    Console.WriteLine("Coefficient crm = {0}", cRightM);
                    Console.WriteLine("Coefficient crd = {0}", cRightD);

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

                        //Print out the coefficient
                        Console.WriteLine("Coefficient b = {0}", b);
                        Console.WriteLine("Coefficient c = {0}", c);

                        //Print out the result
                        Console.WriteLine("Result x = {0}", x);
                        Console.ReadKey();
                    }
                }
                else
                {
                    throw new Exception("Please input correct equation format!");
                }
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("The Error is '{0}'", e);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("The Error is '{0}'", e);
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("Please close the window!");
            }
        }
    }
}