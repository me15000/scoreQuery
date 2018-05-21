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
