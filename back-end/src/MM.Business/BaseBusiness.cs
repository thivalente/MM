using System;
using System.Linq;

namespace MM.Business
{
    public class BaseBusiness
    {
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string GetFirstName(string nome)
        {
            if (String.IsNullOrEmpty(nome))
                return String.Empty;

            var name_words = nome.Split(' ');

            return FirstCharToUpper(name_words[0]);
        }

        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
