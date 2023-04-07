
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
        private Person _person;
        private bool valid = false;
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public bool _enabled=true;

        public string SearchTerm { get; set; }
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




        private  ObservableCollection<Person> _users;
        private Person _selectedUser;

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
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public RelayCommand<object> AddUserCommand { get; set; }
        public RelayCommand<object> EditUserCommand { get; set; }
        public RelayCommand<object> DeleteUserCommand { get; set; }
        public RelayCommand<object> SortCommandA { get; set; }
        public RelayCommand<object> SortCommandD { get; set; }
        public RelayCommand<object> FilterCommand { get; set; }
        public RelayCommand<object> RefreshCommand { get; set; }
        public PersonViewModel()
        {
            ProceedCommand = new RelayCommand<object>(ProceedCommandExecute, CanProceed);
            AddUserCommand = new RelayCommand<object>(AddUser,  CanProceed);
            EditUserCommand = new RelayCommand<object>(EditUser, CanProceedWithSelected);
            DeleteUserCommand = new RelayCommand<object>(DeleteUser, WithSelected);
            FilterCommand = new RelayCommand<object>(Filter);
            RefreshCommand = new RelayCommand<object>(Refresh);

            Birthday = DateTime.Today;
            Person = new Person(Name, LastName, Email, Birthday);
            try
            {
                Users = LoadUsers();
            }
            catch (Exception)
            {
                Users = GenerateUsers();
                SaveUsers();
            }


        }

        public void Refresh(object parameter)
        {
            Users = LoadUsers();
        }


        public void Filter(object parameter)
        {
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
        public void Sort(string propertyName, ListSortDirection sortDirection)
        {
        
        }

        private async void AddUser(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);



            await CreateInfo(person);
            if (valid)
            {
                Users.Add(person);
                SaveUsers();
            }
           

        }

        private async void EditUser(object parameter)
        {
            var person = new Person(Name, LastName, Email, Birthday);



            await CreateInfo(person);
            if (valid)
            {
                Users[Users.IndexOf(SelectedUser)]=person;
            }
            SaveUsers();
        }

        private void DeleteUser(object parameter)
        {
           Users.Remove(SelectedUser);
            SaveUsers();
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
        public ObservableCollection<Person> GenerateUsers()
        {
            ObservableCollection<Person> users = new ObservableCollection<Person>();
            Random rnd = new Random();

            for (int i = 0; i < 50; i++)
            {
                string firstName = "First" + i.ToString();
                string lastName = "Last" + i.ToString();
                string email = "email" + i.ToString() + "@example.com";
                DateTime dateOfBirth = new DateTime(rnd.Next(1950, 2005), rnd.Next(1, 13), rnd.Next(1, 29));

                Person user = new Person(firstName, lastName, email, dateOfBirth);
                user.Calculate();

                users.Add(user);
            }

            return users;
        }
        public void SaveUsers()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            using (FileStream stream = new FileStream("users1.xml", FileMode.Create))
            {
                serializer.Serialize(stream, Users);
            }
        }
        public ObservableCollection<Person> LoadUsers()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            using (FileStream stream = new FileStream("users1.xml", FileMode.Open))
            {
                return (ObservableCollection<Person>)serializer.Deserialize(stream);
            }
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
          
            if (person.DateOfBirth != DateTime.MinValue&&EmailCorrect)
                StatusMessage = person.GetPersonInfo();
            else
                StatusMessage = "";
        }


    }

 


    

   


}
