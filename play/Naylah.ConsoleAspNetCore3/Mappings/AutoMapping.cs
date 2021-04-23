using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.Mappings
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //CreateMap<DTOs.PersonDTO, DTOs.PersonDTO>().ReverseMap(); 
            CreateMap<Entities.Person, DTOs.PersonDTO>().ReverseMap(); 
            CreateMap<Entities.Person, DTOs.PersonDTO2>().ReverseMap(); 
        }
    }
}
