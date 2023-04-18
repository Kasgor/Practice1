using BirthdayCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BirthdayCalculator.Tools
{
    public class ServiceCollection
    {
        public ServiceCollection()
        {
            try
            {
                persons = LoadUsers();
            }
            catch (Exception)
            {
                persons = GenerateUsers();
                SaveUsers();
            }
        }
        public ServiceCollection(ObservableCollection<Person> users)
        {
            persons=users;
        }
        private ObservableCollection<Person> persons;
        public ObservableCollection<Person> Persons
        {
            get { return persons; }
            set { persons = value; }
        }
        public void Edit(Person p, int index)
        {
            persons[index] = p;
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
                serializer.Serialize(stream, persons);
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
    }
}
