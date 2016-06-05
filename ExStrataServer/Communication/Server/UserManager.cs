﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ExStrataServer.Communication.Server
{
    public static class UserManager
    {
        private static readonly List<User> users = new List<User>()
        {
            new User("jfop", "Jos Foppele", CreatePassword("appel"))
        };

        private static List<Token> tokens = new List<Token>();

        public static Token Authenticate(string username, string password)
        {
            CleanupTokens();

            IEnumerable<User> userList = users.Where(a => a.Username == username.ToLower());

            if (userList.Count() == 0) return null;
            else
            {
                User user = userList.First();
                HashAlgorithm hasher = SHA256.Create();
                byte[] hashPassword = hasher.ComputeHash(Utilities.StringToBytes(password));

                if (Enumerable.SequenceEqual(user.Password, hashPassword))
                {
                    Token token = new Token(user);
                    tokens.Add(token);

                    return token;
                }
                else return null;
            }
        }

        public static User CheckToken(string token)
        {
            CleanupTokens();
            Token foundToken = FindToken(token);
            if (foundToken == null) return null;
            else return foundToken.User;
        }

        public static bool TokenExists(string token)
        {
            return FindToken(token) != null;
        }

        public static Token FindToken(string token)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Code == token) return tokens[i];
            }

            return null;
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
