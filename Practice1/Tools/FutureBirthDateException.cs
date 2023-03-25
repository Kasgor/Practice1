using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayCalculator.Tools
{
    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException(string message) : base(message)
        {
        }
    }
}
