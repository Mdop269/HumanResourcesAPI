using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Promotion
{
    public class PromotionValidationException : Exception
    {
        public PromotionValidationException(string message ) : base(message) { }
    }
}
