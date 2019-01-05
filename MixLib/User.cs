using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixLib
{
    public class User
    {
        public string name;
        public string surname;
        public string username;
        public User(string name, string surname, string username)
        {
            this.name = name;
            this.surname = surname;
            this.username = username;
        }
    }
    
}
