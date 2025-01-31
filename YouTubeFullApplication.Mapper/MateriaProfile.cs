namespace YouTubeFullApplication.Mapper
{
    internal class MateriaProfile : Profile
    {
        public MateriaProfile()
        {
            CreateMap<Materia, MateriaDto>();
            CreateMap<Materia, MateriaListDto>()
                .ForMember(d => d.AbbinamentiCount, o => o.MapFrom(s => s.Abbinamenti!.Count));
            CreateMap<Materia, MateriaDetailsDto>()
                .ForMember(d => d.AbbinamentiCount, o => o.MapFrom(s => s.Abbinamenti!.Count));
            CreateMap<MateriaPostDto, Materia>();
            CreateMap<MateriaPutDto, Materia>();
        }
    }
}
