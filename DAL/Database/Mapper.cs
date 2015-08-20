namespace DAL.Database
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource source) where TResult : new();
    }

    public class Mapper : IMapper
    {
        public TResult Map<TSource, TResult>(TSource source) where TResult : new()
        {
            return AutoMapper.Mapper.Map<TSource, TResult>(source);
        }
    }
}
