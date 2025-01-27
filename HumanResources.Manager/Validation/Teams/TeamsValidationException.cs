using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Teams
{
    public class TeamsValidationException : Exception
    {
        public TeamsValidationException(string message) : base(message) { }
    }
}
