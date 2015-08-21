namespace DAL.Mapping
{
    public class Mapper : IMapper
    {
        public TResult Map<TSource, TResult>(TSource source)
        {
            var maps = AutoMapper.Mapper.GetAllTypeMaps();

            var s = typeof(TSource);
            var d = typeof(TResult);

            return AutoMapper.Mapper.Map<TResult>(source);
        }
    }
}
