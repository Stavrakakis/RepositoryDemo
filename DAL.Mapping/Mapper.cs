namespace DAL.Mapping
{
    public class Mapper : IMapper
    {
        public TResult Map<TSource, TResult>(TSource source) where TResult : new()
        {
            return AutoMapper.Mapper.Map<TSource, TResult>(source);
        }
    }
}
