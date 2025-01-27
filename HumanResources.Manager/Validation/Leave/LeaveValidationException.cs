using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Leave
{
    public class LeaveValidationException : Exception
    {
        public LeaveValidationException(string message) : base(message) { }
    }
}
