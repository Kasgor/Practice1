using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayCalculator.Tools
{
    public class TooOldException : Exception
    {
        public TooOldException(string message) : base(message)
        {
        }
    }
}
