using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ExStrataServer.Communication.Server
{

    public class Token
    {
        private string code;
        private User user;
        private DateTime expiration;

        public string Code
        {
            get { return code; }
            private set { code = value; }
        }

        public User User
        {
            get { return user; }
            private set { user = value; }
        }

        public DateTime Expiration
        {
            get { return expiration; }
            private set { expiration = value; }
        }

        public Token(User user)
        {
            User = user;

            Random r = new Random();

            string tempCode;
            do
            {
                int num = r.Next(1, 999999);

                string base36 = num.ToString("36");

                HashAlgorithm hasher = SHA1.Create();

                tempCode = Utilities.BytesToString(hasher.ComputeHash(Encoding.Default.GetBytes(base36)));
            }
            while (UserManager.TokenExists(tempCode));

            Code = tempCode;

            Expiration = DateTime.Now.AddHours(1);
        }
    }
}
