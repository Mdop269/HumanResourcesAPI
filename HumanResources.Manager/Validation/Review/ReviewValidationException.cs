using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Review
{
    public class ReviewValidationException : Exception
    {
        public ReviewValidationException(string message) : base(message) { }
    }
}
