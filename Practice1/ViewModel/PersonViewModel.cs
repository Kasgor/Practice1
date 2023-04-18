
using BirthdayCalculator.Model;
using BirthdayCalculator.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BirthdayCalculator.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        ServiceCollection usersControl = new ServiceCollection();
        private bool valid = false;
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        private Person _selectedUser = new Person();
        public bool _enabled=true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public string SearchTerm { get; set; }
        private ObservableCollection<Person> _users;
        public ObservableCollection<Person> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public Person SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    Name = value.FirstName;
                    LastName = value.LastName;
                    Email = value.Email;
                    Birthday = value.DateOfBirth;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(Birthday));
                }
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public PersonViewModel()
        {
            ProceedCommand = new RelayCommand<object>(ProceedCommandExecute, CanProceed);
            AddUserCommand = new RelayCommand<object>(AddUser,  CanProceed);
            EditUserCommand = new RelayCommand<object>(EditUser, CanProceedWithSelected);
            DeleteUserCommand = new RelayCommand<object>(DeleteUser, WithSelected);
            FilterCommand = new RelayCommand<object>(Filter);
            RefreshCommand = new RelayCommand<object>(Refresh);

            Users = usersControl.Persons;
        }
        public RelayCommand<object> AddUserCommand { get; set; }
        public RelayCommand<object> EditUserCommand { get; set; }
        public RelayCommand<object> DeleteUserCommand { get; set; }
        public RelayCommand<object> FilterCommand { get; set; }
        public RelayCommand<object> RefreshCommand { get; set; }
        private async void AddUser(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);

            await CreateInfo(person);
            if (valid)
            {
                usersControl.Persons.Add(person);
                usersControl.SaveUsers();
                Refresh();
            }
        }
        private async void EditUser(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);
            await CreateInfo(person);
            if (valid)
            {
                var item = usersControl.Persons.FirstOrDefault(i => i.Equals(SelectedUser));
                if (item != null)
                {
                    usersControl.Edit(person, usersControl.Persons.IndexOf(item));
                }
            }
            usersControl.SaveUsers();
            Refresh();
        }
        private void DeleteUser(object parameter)
        {
            usersControl.Persons.Remove(SelectedUser);
            usersControl.SaveUsers();
            Refresh();
        }
        public void Refresh(object parameter)
        {
            Users = usersControl.Persons;
        }
        public void Refresh()
        {
            Users = usersControl.Persons;
        }
        public void Filter(object parameter)
        {
            Users=usersControl.Persons;
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Users = new ObservableCollection<Person>(_users);
            }
            else
            {
                Enabled = false;
                Users = new ObservableCollection<Person>(_users.Where(u =>
                u.FirstName.ToLower().Contains(SearchTerm.ToLower()) ||
                u.LastName.ToLower().Contains(SearchTerm.ToLower()) ||
                u.Age.ToString().Contains(SearchTerm.ToLower()) ||
                u.LastName.ToLower().Contains(SearchTerm.ToLower()) ||
                u.WesternSign.ToLower().Contains(SearchTerm.ToLower()) ||
                u.ChineseSign.ToLower().Contains(SearchTerm.ToLower()) ||
                u.Email.ToLower().Contains(SearchTerm.ToLower())
                
                ));
                Enabled = true;
            }

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
        private bool CanProceedWithSelected(object parameter)
        {
            if (!string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(Email) &&
                Birthday != default(DateTime)&&SelectedUser!=null)
            return true;
            return false;
        } 
        private bool WithSelected(object parameter)
        {
            if (SelectedUser!=null)
            return true;
            return false;
        }
        private async void ProceedCommandExecute(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);  
              await CreateInfo(person);
        }
        async private Task CreateInfo(Person person)

        {
            bool EmailCorrect=true;
            valid = true;
            try
            {
             Enabled = false;
                OnPropertyChanged(nameof(Enabled));
                await Task.Run(() => person.Calculate());
            }
            catch(FutureBirthDateException e)
            {
                MessageBox.Show("Person haven`t been born yet!");
                valid = false;
            }
            catch(TooOldException e)
            {
                MessageBox.Show("Persos`s age is over 135, it is possible not true.");
                valid = false;
            }
            catch (InvalidEmailException e)
            {
                MessageBox.Show("Email is incorrect!");
                EmailCorrect=false;
                valid = false;
            }
            finally
            {
                Enabled = true;
                OnPropertyChanged(nameof(Enabled));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
