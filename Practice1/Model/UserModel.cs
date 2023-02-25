using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayCalculator.Model
{
    internal class UserModel
    {

        private DateTime birthday;

        public DateTime Birthday
        {
            get { return  birthday; }
            set {  birthday = value; }
        }

    }
}
