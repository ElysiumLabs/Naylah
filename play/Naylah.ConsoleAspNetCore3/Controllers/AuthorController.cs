using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Naylah.ConsoleAspNetCore.DTOs;
using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;

namespace Naylah.ConsoleAspNetCore.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly StringTableDataServiceCustom<Author> service;

        public AuthorController(
            StringTableDataServiceCustom<Author> service)
        {
            this.service = service;
        }

        // TODO: needs review on object mappings
        [HttpGet("[action]")]
        public IEnumerable<AuthorResponse> GetAuthorWithBooks()
        {
            return service.GetAll<AuthorResponse>();
        }

        [HttpGet("[action]")]
        public IEnumerable<AuthorBaseResponse> GetAuthorBase()
        {
            return service.GetAll<AuthorBaseResponse>();
        }

        [HttpPost("[action]")]
        public async Task<AuthorResponse> Upsert1(AuthorRequest author)
        {
            return await service.UpsertEntityAsync<AuthorRequest, AuthorResponse>(author);
        }

        [HttpPost("[action]")]
        public async Task<AuthorBaseResponse> Upsert2(AuthorRequest author)
        {
            return await service.UpsertEntityAsync<AuthorRequest, AuthorResponse>(author);
        }
    }
}