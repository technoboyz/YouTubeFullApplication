namespace YouTubeFullApplication.Mapper
{
    internal class GenericProfile<TEntity, TModel, TPost, TPut> : Profile
    {
        public GenericProfile()
        {
            CreateMap<TEntity, TModel>();
            CreateMap<TPost, TEntity>();
            CreateMap<TPut, TEntity>();
        }
    }
}
