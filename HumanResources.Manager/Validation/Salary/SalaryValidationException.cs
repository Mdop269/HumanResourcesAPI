using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation
{
    public class SalaryValidationException : Exception
    {
        public SalaryValidationException(string message) : base(message) { }
    }
}
