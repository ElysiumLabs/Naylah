using Naylah.Domain;
using System;

namespace Naylah.ConsolePlayground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var p1 = new Person() { Id = "1", Name = "Camila" };
            var p2 = new Person() { Id = "1", Name = "Breno" };

            Console.WriteLine(p1 == p2);
            Console.ReadKey();
        }
    }

    public class Person : Entity
    {
        public string Name { get; set; }
    }
}