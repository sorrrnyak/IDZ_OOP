using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace tyaf
{
    class User
    {
        private int id { get; set; }
        private string login, pass;

        public User() { }

        public User(string login, string pass)
        {
            this.login = login;
            this.pass = pass; 
        }
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("login");
            }
        }
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                OnPropertyChanged("pass");
            }
        }

        public int id_user { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string str = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(str));
        }
    }
}
