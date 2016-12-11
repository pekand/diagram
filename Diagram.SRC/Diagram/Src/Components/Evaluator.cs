using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;
using System.Security;

[assembly: AllowPartiallyTrustedCallers]

namespace Diagram
{
    /// <summary>
    /// Ncalc library for expression evaluating</summary>
    class Evaluator
    {

        public Evaluator()
        {
        }

        /// <summary>
        /// evaluate string expression wit ncalc library</summary>
        public String Evaluate(String expression)
        {
            try
            {
                // NCALC
                Expression e = new Expression(expression);
                return e.Evaluate().ToString();
            }
            catch (Exception ex)
            {
                Program.log.write("evaluation error: " + ex.Message);
            }

            return "";
        }
    }
}
