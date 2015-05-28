using System;
using System.Collections;
using System.Globalization;
using Cassowary;
using Cassowary.Constraints;
using Cassowary.Variables;

namespace Cassowary.Parsing
{
    public class Parser
    {
        private const int _EOF = 0;
        private const int _leq = 1;
        private const int _geq = 2;
        private const int _eq = 3;
        private const int _altleq = 4;
        private const int _altgeq = 5;
        private const int _plus = 6;
        private const int _minus = 7;
        private const int _times = 8;
        private const int _divide = 9;
        private const int _lparen = 10;
        private const int _rparen = 11;
        private const int _variable = 12;
        private const int _number = 13;
        private const int maxT = 14;

        private const bool T = true;
        private const bool x = false;
        private const int minErrDist = 2;

        public Scanner scanner;
        public Errors errors;

        public Token t; // last recognized token
        public Token la; // lookahead token
        private int errDist = minErrDist;

        private ClConstraint _constraint;
        private Hashtable _context = new Hashtable();

        public Hashtable Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public ClConstraint Value
        {
            get { return _constraint; }
            set { _constraint = value; }
        }


        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            errors = new Errors();
        }

        private void SynErr(int n)
        {
            if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
            errDist = 0;
        }

        public void SemErr(string msg)
        {
            if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
            errDist = 0;
        }

        private void Get()
        {
            for (;;)
            {
                t = la;
                la = scanner.Scan();
                if (la.kind <= maxT)
                {
                    ++errDist;
                    break;
                }

                la = t;
            }
        }

        private void Expect(int n)
        {
            if (la.kind == n) Get();
            else
            {
                SynErr(n);
            }
        }

        private bool StartOf(int s)
        {
            return set[s, la.kind];
        }

        private void ExpectWeak(int n, int follow)
        {
            if (la.kind == n) Get();
            else
            {
                SynErr(n);
                while (!StartOf(follow)) Get();
            }
        }


        private bool WeakSeparator(int n, int syFol, int repFol)
        {
            int kind = la.kind;
            if (kind == n)
            {
                Get();
                return true;
            }
            else if (StartOf(repFol))
            {
                return false;
            }
            else
            {
                SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind]))
                {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }


        private void Constraint()
        {
            ClLinearExpression e1, e2;
            bool eq = false, geq = false, leq = false;
            Expression(out e1);
            if (la.kind == 3)
            {
                Get();
                eq = true;
            }
            else if (la.kind == 2)
            {
                Get();
                geq = true;
            }
            else if (la.kind == 5)
            {
                Get();
                geq = true;
            }
            else if (la.kind == 1)
            {
                Get();
                leq = true;
            }
            else if (la.kind == 4)
            {
                Get();
                leq = true;
            }
            else SynErr(15);
            Expression(out e2);
            if (eq)
                Value = new ClLinearEquation(e1, e2);
            else if (geq)
                Value = new ClLinearInequality(e1, InequalityType.GreaterThanOrEqual, e2);
            else if (leq)
                Value = new ClLinearInequality(e1, InequalityType.LessThanOrEqual, e2);
        }

        private void Expression(out ClLinearExpression e)
        {
            e = null;
            ClLinearExpression e1;
            Term(out e);
            while (la.kind == 6 || la.kind == 7)
            {
                if (la.kind == 6)
                {
                    Get();
                    Term(out e1);
                    e = CMath.Plus(e, e1);
                }
                else
                {
                    Get();
                    Term(out e1);
                    e = CMath.Minus(e, e1);
                }
            }
        }

        private void Term(out ClLinearExpression e)
        {
            e = null;
            ClLinearExpression e1;
            Factor(out e);
            while (la.kind == 8 || la.kind == 9)
            {
                if (la.kind == 8)
                {
                    Get();
                    Factor(out e1);
                    e = CMath.Times(e, e1);
                }
                else
                {
                    Get();
                    Factor(out e1);
                    e = CMath.Divide(e, e1);
                }
            }
        }

        private void Factor(out ClLinearExpression e)
        {
            e = null;
            ClDouble d;
            ClVariable v;
            bool negate = false;
            if (la.kind == 7)
            {
                Get();
                negate = true;
            }
            if (la.kind == 13)
            {
                Number(out d);
                e = new ClLinearExpression(d.Value);
            }
            else if (la.kind == 12)
            {
                Variable(out v);
                e = new ClLinearExpression(v);
            }
            else if (la.kind == 10)
            {
                Get();
                Expression(out e);
                Expect(11);
            }
            else SynErr(16);
            if (negate)
                e = CMath.Minus(0, e);
        }

        private void Number(out ClDouble d)
        {
            Expect(13);
            double tmpVal = double.Parse(t.val, new CultureInfo("en-US").NumberFormat);
            d = new ClDouble(tmpVal);
        }

        private void Variable(out ClVariable v)
        {
            Expect(12);
            if (Context.ContainsKey(t.val))
            {
                v = (ClVariable) Context[t.val];
            }
            else
            {
                SemErr("Undefined variable: " + t.val);
                v = null;
            }
        }


        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            Constraint();

            Expect(0);
        }

        private bool[,] set =
        {
            {T, x, x, x, x, x, x, x, x, x, x, x, x, x, x, x}
        };
    } // end Parser


    public class Errors
    {
        public int count = 0; // number of errors detected

        public System.IO.TextWriter errorStream = Console.Out;
            // error messages go to this stream

        public string errMsgFormat = "-- line {0} col {1}: {2}";
            // 0=line, 1=column, 2=text

        public void SynErr(int line, int col, int n)
        {
            string s;
            switch (n)
            {
                case 0:
                    s = "EOF expected";
                    break;
                case 1:
                    s = "leq expected";
                    break;
                case 2:
                    s = "geq expected";
                    break;
                case 3:
                    s = "eq expected";
                    break;
                case 4:
                    s = "altleq expected";
                    break;
                case 5:
                    s = "altgeq expected";
                    break;
                case 6:
                    s = "plus expected";
                    break;
                case 7:
                    s = "minus expected";
                    break;
                case 8:
                    s = "times expected";
                    break;
                case 9:
                    s = "divide expected";
                    break;
                case 10:
                    s = "lparen expected";
                    break;
                case 11:
                    s = "rparen expected";
                    break;
                case 12:
                    s = "variable expected";
                    break;
                case 13:
                    s = "number expected";
                    break;
                case 14:
                    s = "??? expected";
                    break;
                case 15:
                    s = "invalid Constraint";
                    break;
                case 16:
                    s = "invalid Factor";
                    break;

                default:
                    s = "error " + n;
                    break;
            }
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(string s)
        {
            errorStream.WriteLine(s);
            count++;
        }

        public void Warning(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
        }

        public void Warning(string s)
        {
            errorStream.WriteLine(s);
        }
    } // Errors


    public class FatalError : Exception
    {
        public FatalError(string m)
            : base(m)
        {
        }
    }
}