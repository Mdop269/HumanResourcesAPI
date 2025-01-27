using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Status
{
    public class StatusValidationException : Exception
    {
        public StatusValidationException(string message ) : base(message) { }
    }
}
