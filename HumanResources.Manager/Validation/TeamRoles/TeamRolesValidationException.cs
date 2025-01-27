using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.TeamRoles
{
    public class TeamRolesValidationException : Exception
    {
        public TeamRolesValidationException(string message ) : base(message) { }
    }
}
