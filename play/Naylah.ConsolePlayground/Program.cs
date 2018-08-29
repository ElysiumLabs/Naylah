using Microsoft.Extensions.DependencyInjection;
using Naylah.Domain;
using Naylah.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Naylah.ConsolePlayground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<IHandler<Person>, PersonHandler>();
            services.AddScoped<IHandler<Person>, PersonHandler2>();
            services.AddScoped<IHandler<Notification>, NotificationHandler>();
            services.AddScoped<IEventDispatcher, Dispatcher>();

            var c = services.BuildServiceProvider();
            var e = c.GetService<IEventDispatcher>();

            var p1 = new Person() { Id = "1", Name = "Camila" };
            var p2 = new Person() { Id = "2", Name = "Breno" };

            //CadastraPessoa
            {
                e.Dispatch(p1);
                //Salvo no banco
            }

            Console.WriteLine(p1.Age);
            Console.ReadKey();
        }
    }

    public class NotificationHandler : IHandler<Notification>
    {
        public List<Notification> notifications = new List<Notification>();

        public void Handle(Notification @event)
        {
            notifications.Add(@event);
        }

        public bool HasEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Notification> Notify()
        {
            return notifications;
        }
    }

    public class PersonHandler : IHandler<Person>
    {
        public void Handle(Person @event)
        {
            Console.WriteLine("Handled");

            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - @event.BirthDay.Year;
            // Go back to the year the person was born in case of a leap year
            if (@event.BirthDay > today.AddYears(-age)) age--;

            @event.Age = age;

            //DomainEvent.Raise(new Notification("Erro"));
            //throw new NotImplementedException();
        }

        public bool HasEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> Notify()
        {
            throw new NotImplementedException();
        }
    }

    public class PersonHandler2 : IHandler<Person>
    {
        public void Handle(Person @event)
        {
            Console.WriteLine("Handled2");
            //throw new NotImplementedException();
        }

        public bool HasEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> Notify()
        {
            throw new NotImplementedException();
        }
    }

    public class Person : Entity, IEvent
    {
        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public int Age { get; set; }
    }
}