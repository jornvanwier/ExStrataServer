using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ExStrataServer.Communication.Server
{
    public static class UserManager
    {
        private static readonly List<User> users = new List<User>
        {
            new User("jfop", "Jos Foppele", CreatePassword("appel"))
        };

        private static readonly List<Token> tokens = new List<Token>();

        public static Token Authenticate(string username, string password)
        {
            CleanupTokens();

            IEnumerable<User> userList = users.Where(a => a.Username == username.ToLower()).ToArray();

            if (!userList.Any()) return null;
            User user = userList.First();
            HashAlgorithm hasher = SHA256.Create();
            byte[] hashPassword = hasher.ComputeHash(Utilities.StringToBytes(password));

            if (user.Password.SequenceEqual(hashPassword))
            {
                Token token = new Token(user);
                tokens.Add(token);

                return token;
            }
            return null;
        }

        public static User CheckToken(string token)
        {
            CleanupTokens();
            Token foundToken = FindToken(token);
            return foundToken?.User;
        }

        public static bool TokenExists(string token)
        {
            return FindToken(token) != null;
        }

        public static Token FindToken(string token)
        {
            return tokens.FirstOrDefault(t => t.Code == token);
        }

        private static void CleanupTokens()
        {
            DateTime now = DateTime.Now;

            int i = 0;
            while (i < tokens.Count)
            { 
                if (tokens[i].Expiration < now) tokens.RemoveAt(i);
                else i++;
            }
        }

        private static byte[] CreatePassword(string password)
        {
            HashAlgorithm hasher = SHA256.Create();
            return hasher.ComputeHash(Utilities.StringToBytes(password));
        }

    }
}
