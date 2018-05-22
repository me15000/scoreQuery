using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;

namespace TestQ
{
    class Program
    {
        static void Main(string[] args)
        {

            string oldhtml = "";

            oldhtml = System.Text.RegularExpressions.Regex.Replace(oldhtml, @"href\=""(?<link>[^""]+)""", (o) =>
            {

                string link = o.Groups["link"].Value;

                if (link.IndexOf("//") == 0 || link.IndexOf("http") == 0)
                {
                    return o.Value;
                }
                else
                {
                    if (link.IndexOf("/") == 0)
                    {
                        return @"href=""http://www.52miji.com" + link + @"""";

                    }
                    else
                    {
                        return @"href=""http://www.52miji.com/" + link + @"""";
                    }

                }


                return null;
            });






            string password = "o0q0otL8aEzpcZL/FT9WsQ==";
            string filename = "{\"foo\":\"bar\"}";

            var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(password));
            hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(filename));

            foreach (byte test in hmacsha256.Hash)
            {
                Console.Write(test.ToString("x2"));
            }

            Console.Read();
        }
    }
}
