using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Department
{
    public class DepartmentValidationException : Exception
    {
        public DepartmentValidationException(string message) : base(message) { }
    }
}
