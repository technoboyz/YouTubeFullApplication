namespace YouTubeFullApplication.Mapper
{
    internal class StudenteProfile : GenericProfile<Studente, StudenteDto, StudentePostDto, StudentePutDto>
    {
        public StudenteProfile() : base()
        {
            CreateMap<Studente, StudenteListDto>()
                .ForMember(d => d.Anni, o => o.MapFrom(s => s.Frequenze!.Count));
            CreateMap<Studente, StudenteDetailsDto>();
        }
    }
}
