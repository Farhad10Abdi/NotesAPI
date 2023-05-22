using AutoMapper;
using Notes_API.Models;
using Notes_API.Models.Dto;

namespace Notes_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Notes,NoteDTO>().ReverseMap();

            CreateMap<Notes, NoteCreateDTO>().ReverseMap();
            CreateMap<Notes, NoteUpdateDTO>().ReverseMap();

            CreateMap<NoteDTO, NoteCreateDTO>().ReverseMap();
            CreateMap<NoteDTO, NoteCreateDTO>().ReverseMap();
        }
    }
}
