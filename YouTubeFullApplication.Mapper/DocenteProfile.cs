namespace YouTubeFullApplication.Mapper
{
    internal class DocenteProfile : Profile
    {
        public DocenteProfile()
        {
            CreateMap<Docente, DocenteDto>();
            CreateMap<Docente, DocenteListDto>()
                .ForMember(d => d.AbbinamentiCount, o => o.MapFrom(s => s.Abbinamenti!.Count));
            CreateMap<Docente, DocenteDetailsDto>()
                .ForMember(d => d.AbbinamentiCount, o => o.MapFrom(s => s.Abbinamenti!.Count));
            CreateMap<DocentePostDto, Docente>();
            CreateMap<DocentePutDto, Docente>();
        }
    }
}
