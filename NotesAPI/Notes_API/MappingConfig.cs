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



            CreateMap<NoteBook, NoteBookDTO>().ReverseMap();

            CreateMap<NoteBook, NoteBookCreateDTO>().ReverseMap();
            CreateMap<NoteBook, NoteBookUpdateDTO>().ReverseMap();

            CreateMap<NoteBookDTO, NoteBookCreateDTO>().ReverseMap();
            CreateMap<NoteBookDTO, NoteBookUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
