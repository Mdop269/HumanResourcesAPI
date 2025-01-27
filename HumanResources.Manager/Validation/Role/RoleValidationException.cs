using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Role
{
    public class RoleValidationException : Exception
    {
        public RoleValidationException(string message ) : base (message) { }

    }
}
