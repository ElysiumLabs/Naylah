using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;

namespace Naylah.ConsoleAspNetCore.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class BookController : ControllerBase
    {
        private readonly StringTableDataServiceCustom<Book> service;

        public BookController(StringTableDataServiceCustom<Book> service)
        {
            this.service = service;
        }

        [HttpGet("[action]")]
        public IEnumerable<BookResponse> GetBooksWithAuthor()
        {
            return service.GetAll<BookResponse>();
        }

        [HttpGet("[action]")]
        public IEnumerable<BookBaseResponse> GetBooksBase()
        {
            return service.GetAll<BookBaseResponse>();
        }

        [HttpPost("[action]")]
        public async Task<BookBaseResponse> Upsert1(
            BookRequest book)
        {
            return await service.UpsertEntityAsync<BookRequest, BookBaseResponse>(book);
        }

        [HttpPost("[action]")]
        public async Task<BookResponse> Upsert2(
            BookRequest book)
        {
            return await service.UpsertEntityAsync<BookRequest, BookResponse>(book);
        }
    }
}