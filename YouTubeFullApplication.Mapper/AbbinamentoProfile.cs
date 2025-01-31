namespace YouTubeFullApplication.Mapper
{
    internal class AbbinamentoProfile : Profile
    {
        public AbbinamentoProfile()
        {
            CreateMap<Abbinamento, AbbinamentoDto>();
            CreateMap<Abbinamento, AbbinamentoListDto>()
                .ForMember(d => d.Classe, o => o.MapFrom(s => s.Classe!.Nome))
                .ForMember(d => d.Materia, o => o.MapFrom(s => s.Materia!.Nome))
                .ForMember(d => d.Docente, o => o.MapFrom(s => s.Docente!.CognomeNome));
            CreateMap<Abbinamento, AbbinamentoDetailsDto>();
            CreateMap<AbbinamentoPostDto, Abbinamento>();
            CreateMap<AbbinamentoPutDto, Abbinamento>();
        }
    }
}
