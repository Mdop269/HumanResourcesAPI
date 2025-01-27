using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Employee
{
    public class EmployeeValidationException : Exception
    {
        public EmployeeValidationException(string message) : base(message) { }
    }
}
