using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TelephoneBook
{
    class Subscriber
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Subscriber(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"Имя: {this.Name}, номер телефона: {this.PhoneNumber}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Subscriber))
                return false;

            //сравнить имя и номер


            return base.Equals(obj);
        }

        public bool CompareNames(string name)
        {
            if (String.Compare(Name, name) == 0)
                return true;
            return false;
        }

        public bool ComparePhoneNumbers(string phoneNumber)
        {
            if (String.Compare(PhoneNumber, phoneNumber) == 0)
                return true;
            return false;
        }
    }
}

