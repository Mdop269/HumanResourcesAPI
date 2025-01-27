using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.HR
{
    public class HrValidationException : Exception
    {
        public HrValidationException(string message) : base(message) { }
    }
}
