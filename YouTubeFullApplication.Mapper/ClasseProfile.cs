namespace YouTubeFullApplication.Mapper
{
    internal class ClasseProfile : Profile
    {
        public ClasseProfile()
        {
            CreateMap<Classe, ClasseDto>();
            CreateMap<Classe, ClasseListDto>();
            CreateMap<Classe, ClasseDetailsDto>();
            CreateMap<ClassePostDto, Classe>();
            CreateMap<ClassePutDto, Classe>();
        }
    }
}
