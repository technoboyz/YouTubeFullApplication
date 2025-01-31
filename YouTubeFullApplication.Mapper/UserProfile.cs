namespace YouTubeFullApplication.Mapper
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppIdentityUser, UserDto>()
                .ForMember(d => d.Role, o => o.MapFrom(s => s.UsersRoles!.First().Role!.Name));
            CreateMap<AppIdentityUser, UserListDto>()
                .ForMember(d => d.Role, o => o.MapFrom(s => s.UsersRoles!.First().Role!.Name));
            CreateMap<UserRegisterRequestDto, AppIdentityUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
            CreateMap<UserPutDto, AppIdentityUser>();
        }
    }
}
