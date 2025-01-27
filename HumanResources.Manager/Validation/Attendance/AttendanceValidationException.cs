using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Manager.Validation.Attendance
{
    public class AttendanceValidationException : Exception
    {
        public AttendanceValidationException(string message) : base (message) { }
    }
}
