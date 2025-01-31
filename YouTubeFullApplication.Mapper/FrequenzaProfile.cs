namespace YouTubeFullApplication.Mapper
{
    internal class FrequenzaProfile : Profile
    {
        public FrequenzaProfile()
        {
            CreateMap<Frequenza, FrequenzaDto>();
            CreateMap<Frequenza, FrequenzaListDto>()
                .ForMember(d => d.Classe_Nome, o => o.MapFrom(s => s.Classe!.Nome))
                .ForMember(d => d.Studente_CognomeNome, o => o.MapFrom(s => s.Studente!.CognomeNome));
            CreateMap<Frequenza, FrequenzaDetailsDto>();
            CreateMap<FrequenzaPostDto, Frequenza>();
            CreateMap<FrequenzaPutDto, Frequenza>();
            CreateMap<Frequenza, FrequenzaStudenteDto>();
        }
    }
}
