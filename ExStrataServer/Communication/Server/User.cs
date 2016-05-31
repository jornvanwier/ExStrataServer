using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.Communication.Server
{
    public class User
    {
        private string username;
        private string displayname;
        private byte[] password;

        public string Username
        {
            get { return username; }
            private set { username = value; }
        }

        public string Displayname
        {
            get { return displayname; }
            private set { displayname = value; }
        }

        public byte[] Password
        {
            get { return password; }
            private set { password = value; }
        }

        public User(string username, string displayname, byte[] password)
        {
            Username = username;
            Displayname = displayname;
            Password = password;
        }
    }
}
