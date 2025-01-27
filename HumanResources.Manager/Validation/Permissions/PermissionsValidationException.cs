using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Permissions
{
    public class PermissionsValidationException : Exception
    {
        public PermissionsValidationException(string message) : base(message) { }
    }
}
