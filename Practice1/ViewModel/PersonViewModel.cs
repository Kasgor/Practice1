
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
    public class PersonViewModel : INotifyPropertyChanged
    {
        private Person _person;
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public bool _enabled=true;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public Person Person
        {
            get { return _person; }
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }



     

        public PersonViewModel()
        {
            ProceedCommand = new RelayCommand<object>(ProceedCommandExecute, CanProceed);
            Birthday = DateTime.Today;
            Person = new Person(Name, LastName, Email, Birthday);
           
            
          
        }

     

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand ProceedCommand { get; private set; }

        private bool CanProceed(object parameter)
        {
            if (!string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(Email) &&
                Birthday != default(DateTime))
            return true;
            return false;
        }

        private string statusMessage;

        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private async void ProceedCommandExecute(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);

            
            
              await CreateInfo(person);
            

          
        }
       async private Task CreateInfo(Person person)

        {
            bool EmailCorrect=true;
            try
            {
             Enabled = false;
                OnPropertyChanged(nameof(Enabled));
                await Task.Run(() => person.Calculate());
            }
            catch(FutureBirthDateException e)
            {
                MessageBox.Show("Person haven`t been born yet!");
            }
            catch(TooOldException e)
            {
                MessageBox.Show("Persos`s age is over 135, it is possible not true.");
            }
            catch (InvalidEmailException e)
            {
                MessageBox.Show("Email is incorrect!");
                EmailCorrect=false;
            }
            finally
            {
                Enabled = true;
                OnPropertyChanged(nameof(Enabled));
            }
          
            if (person.DateOfBirth != DateTime.MinValue&&EmailCorrect)
                StatusMessage = person.GetPersonInfo();
            else
                StatusMessage = "";
        }


    }

 


    

   


}
