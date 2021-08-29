using Microsoft.Extensions.DependencyInjection;
using Naylah.Data;
using Naylah.Data.Access;
using Naylah.Domain;
using Naylah.Domain.Abstractions;
using Naylah.Rest;
using Naylah.Rest.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Naylah.ConsolePlayground3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Top().Wait();
        }

        public static async Task Top()
        {
            //var services = new ServiceCollection();
            //services.AddScoped<IHandler<Person>, PersonHandler>();
            //services.AddScoped<IHandler<Person>, PersonHandler2>();
            //services.AddScoped<IHandler<Notification>, NotificationHandler>();
            //services.AddScoped<IEventDispatcher, Dispatcher>();

            //var c = services.BuildServiceProvider();
            //var e = c.GetService<IEventDispatcher>();

            //var p1 = new Person() { Id = "1", Name = "Camila", CreatedAt = DateTime.UtcNow };
            //var p2 = new Person() { Id = "2", Name = "Breno", CreatedAt =  DateTime.UtcNow.AddDays(2) };

            //var listOfPeople = new List<Person>() { p1, p2 };

            //var unitOfNothing = new NothingOfWork();
            //var personRepo = new PersonRepo(listOfPeople);

            //var table = new PersonTableService(unitOfNothing, personRepo);

            //var wrapper = table.CreateWrapper();

            //var ttt = wrapper.GetEntities();

            //var q = table.GetAll().ToList();


            ////CadastraPessoa
            //{
            //    e.Dispatch(p1);
            //    //Salvo no banco
            //}

            //foreach (var item in q)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //Console.ReadKey();

            var naylahClient = new NaylahRestClient2(new Uri("http://localhost:5000"));

            try
            {
                //var response = await naylahClient.ExecuteAsync<PersonDTO, string>("", HttpMethod.Put, new PersonDTO() { Name = new PersonNameFull() { FirstName = "teste" } });
                var workshiftTableService = new StringTableService<Workshift>(naylahClient) { Route = "Workshift" };
                var uma = await workshiftTableService.Get(new QueryOptions() { CustomRoute = "?companyId=2C25B65430F649E0BAD4A55C67B3FD7C", Top = 100000 });

                var  ii = uma.Items.FirstOrDefault();
                var r = await naylahClient.ExecuteAsync<Workshift, Workshift>("Workshift", HttpMethod.Post, ii);
            }
            catch (RestException e)
            {
                var errorP = e.AsErrorPresentation();
                throw;
            }
            catch (Exception e)
            {

                throw;
            }

            

            //var q1 = personTableService.AsQueryable().Where(x => x.Name.FirstName == "1");
            //var r = q1.ToList();

            //var loadCollection = await personTableService.ToLoadCollection(new QueryOptions() {Filter = "Name/Full eq 'string'"});

            //while (loadCollection.HasMoreItems)
            //{
            //    await loadCollection.LoadMoreItems();
            //}


            Console.WriteLine();
        }

    }

    public class Workshift : IEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    

    public class NothingOfWork : IUnitOfWork
    {

        public async Task<int> CommitAsync()
        {
            return 1;
        }

        public void Dispose()
        {

        }
    }

    public class PersonRepo : IRepository<Person>
    {
        private readonly IList<Person> people;

        public PersonRepo(IList<Person> people)
        {
            this.people = people;
        }

        public IQueryable<Person> Entities => people.AsQueryable();

        public ValueTask<Person> AddAsync(Person entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Person> EditAsync(Person entity)
        {
            throw new NotImplementedException();
        }

        public Person FindBy(Expression<Func<Person, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Person FindById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> Get(Expression<Func<Person, bool>> filter = null, Func<IQueryable<Person>, IOrderedQueryable<Person>> orderBy = null, Expression<Func<Person, object>>[] includes = null, int? skip = null, int? take = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetAll(Func<IQueryable<Person>, IOrderedQueryable<Person>> orderBy = null, Expression<Func<Person, object>>[] includes = null, int? skip = null, int? take = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Person> GetAllAsQueryable(params Expression<Func<Person, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Person GetById(string id, Expression<Func<Person, object>>[] includes = null)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<Person, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public bool GetExists(Expression<Func<Person, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Person entity)
        {
            throw new NotImplementedException();
        }
    }

    public class PersonTableService : StringTableDataService<Person, PersonM>
    {
        public PersonTableService(IUnitOfWork _unitOfWork, IRepository<Person> repository) : base(repository, _unitOfWork)
        {
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

    public class Person : Entity, IEvent, Naylah.IEntityUpdate<PersonM>
    {
        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public int Age { get; set; }


        public void UpdateFrom(PersonM source, EntityUpdateOptions options = null)
        {
            throw new NotImplementedException();
        }
    }

    public class PersonM : IEntity<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public int Age { get; set; }
    }


    public class PersonName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class PersonNameFull : PersonName
    {
        public string Full { get; set; }


    }

    public class PersonDTO : IEntity<string>
    {
        public string Id { get; set; }

        public PersonNameFull Name { get; set; } = new PersonNameFull();

        public PersonName Test { get; set; } = new PersonName();
    }

    public class PersonDTO2 : IEntity<string>
    {
        public string Id { get; set; }

        public PersonNameFull Name { get; set; } = new PersonNameFull();
    }
}
