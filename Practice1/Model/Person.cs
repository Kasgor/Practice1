using BirthdayCalculator.Tools;
using BirthdayCalculator.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BirthdayCalculator.Model
{
    public class Person : INotifyPropertyChanged
    {

        public Person(){}
        public Person(string firstName, string lastName, string emailAddress, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = emailAddress;
            DateOfBirth = dateOfBirth;
        }
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _dateOfBirth;
        private int _age;
        private bool _isAdult;
        private string _westernSign;
        private string _chineseSign;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public bool IsAdult
        {
            get { return _isAdult; }
            set { _isAdult=value; }
        }
        public string WesternSign
        {
            get
            {
               return _westernSign;
            }
            set { _westernSign = value; }
        }
        public string ChineseSign
        {
            get
            {
                return _chineseSign;
            }
            set
            {
                _chineseSign = value;
            }
        }
         public void Calculate()
        {
            DateTime today = DateTime.Today;
             Age = today.Year - DateOfBirth.Year;
            
            if (DateOfBirth > today.AddYears(-_age))
            {
                _age--;
            }
            if (DateOfBirth > DateTime.Today)
            {
                DateOfBirth = DateTime.MinValue;
                throw new FutureBirthDateException("Person haven`t been born yet!");
            }
            else if (_age > 135)
            {
                DateOfBirth = DateTime.MinValue;
                throw new TooOldException("Persos`s age is over 135, it is possible not true.");

            }
            else if (DateOfBirth.Month == DateTime.Today.Month && DateOfBirth.Day == DateTime.Today.Day&&IsValidEmail(Email))
            {
                MessageBox.Show("Happy birthday");
            }
            if (!IsValidEmail(Email))
            {
                throw new InvalidEmailException("Email is incorrect!");
            }
            _chineseSign= CalculateChinese();
            _westernSign= CalculateWestern();
            _isAdult=CalculateAdult();
        }
        private bool CalculateAdult()
        {
            return (DateTime.Today.Year - DateOfBirth.Year) >= 18;
        }
        public string CalculateChinese()
        {
            int year = DateOfBirth.Year;
            if (year % 12 == 0) { return "Monkey"; }

            else if (year % 12 == 1) { return "Rooster"; }
            else if (year % 12 == 2) { return "Dog"; }
            else if (year % 12 == 3) { return "Pig"; }
            else if (year % 12 == 4) { return "Rat"; }
            else if (year % 12 == 5) { return "Ox"; }
            else if (year % 12 == 6) { return "Tiger"; }
            else if (year % 12 == 7) { return "Rabbit"; }
            else if (year % 12 == 8) { return "Dragon"; }
            else if (year % 12 == 9) { return "Snake"; }
            else if (year % 12 == 10) { return "Horse"; }
            else { return "Sheep"; }
        }
        public string CalculateWestern()
        {
            int month = DateOfBirth.Month;
            int day = DateOfBirth.Day;

            if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
            {
                return "Aries";
            }
            else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
            {
                return "Taurus";
            }
            else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
            {
                return "Gemini";
            }
            else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
            {
                return "Cancer";
            }
            else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
            {
                return "Leo";
            }
            else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
            {
                return "Virgo";
            }
            else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
            {
                return "Libra";
            }
            else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
            {
                return "Scorpio";
            }
            else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
            {
                return "Sagittarius";
            }
            else if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
            {
                return "Capricorn";
            }
            else if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
            {
                return "Aquarius";
            }
            else
            {
                return "Pisces";
            }
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string GetPersonInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Name: {FirstName}");
            sb.AppendLine($"Surname: {LastName}");
            sb.AppendLine($"Email: {Email}");
            sb.AppendLine($"Day of birth: {DateOfBirth:d MMMM yyyy}");
            sb.AppendLine($"Age: {Age}");
            sb.AppendLine($"Western sing: {WesternSign}");
            sb.AppendLine($"Chinese Zodiac: {ChineseSign}");
            sb.AppendLine($"Adult: {IsAdult}");

            return sb.ToString();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
