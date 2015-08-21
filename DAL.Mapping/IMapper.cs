namespace DAL.Mapping
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource source);
    }
}