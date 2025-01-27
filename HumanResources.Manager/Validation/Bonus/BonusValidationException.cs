using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Bonus
{
    public class BonusValidationException : Exception
    {
        public BonusValidationException(string message) : base(message) { }
    }
}
