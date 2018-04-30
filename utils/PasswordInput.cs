using System;
using System.Text;

namespace BuildTools.Utils
{
    public static class PasswordInput
    {
        public static string GetPassword()
        {
            var builder = new StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        builder.Remove(builder.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    builder.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }
            return builder.ToString();
        }
    }
}