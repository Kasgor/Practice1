using BirthdayCalculator.Model;
using BirthdayCalculator.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BirthdayCalculator.ViewModel
{
    internal class AgeCounterViewModel : INotifyPropertyChanged
    {

       
        private DateTime _birthday = DateTime.Now;
        private int _age;
        private string _westernZodiacSign;
        private string _chineseZodiacSign;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public string WesternZodiacSign
        {
            get { return _westernZodiacSign; }
            set
            {
                _westernZodiacSign = value;
                OnPropertyChanged("WesternZodiacSign");
            }
        }

        public string ChineseZodiacSign
        {
            get { return _chineseZodiacSign; }
            set
            {
                _chineseZodiacSign = value;
                OnPropertyChanged("ChineseZodiacSign");
            }
        }

        public ICommand CalculateCommand { get; private set; }

        public AgeCounterViewModel()
        {
            CalculateCommand = new RelayCommand<object>(Calculate, CanCalculate);
        }

        private bool CanCalculate(object parameter)
        {
            return Birthday != DateTime.MinValue;
        }

        private void Calculate(object parameter)
        {
            Age = CalculateAge(Birthday);

            if (Age < 0 || Age > 135)
            {
                return;
            }

            if (IsBirthdayToday(Birthday))
            {
                MessageBox.Show("Happy Birthday!");
            }

            WesternZodiacSign = CalculateWesternZodiac(Birthday);
            ChineseZodiacSign = CalculateChineseZodiac(Birthday);
        }

        private int CalculateAge(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthday.Year;

            if (birthday > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private bool IsBirthdayToday(DateTime birthday)
        {
            return birthday.Day == DateTime.Today.Day && birthday.Month == DateTime.Today.Month;
        }

        private string CalculateWesternZodiac(DateTime birthday)
        {
            int month = birthday.Month;
            int day = birthday.Day;

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

        private string CalculateChineseZodiac(DateTime birthday)
        {
      
            string[] chineseZodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Mouse", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            int startYear = 1900;
            int currentYear = DateTime.Now.Year;
            int offset = (currentYear - startYear) % 12;
            int zodiacIndex = (birthday.Year - startYear) % 12;
            zodiacIndex = (zodiacIndex + offset) % 12;
            string chineseZodiacSign = chineseZodiacSigns[zodiacIndex];

            return chineseZodiacSign;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
