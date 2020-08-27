using System.Collections.Generic;

namespace Bermuda.Core.Mapper
{
    public interface IClassMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);

        void Map<TSource, TDestination>(TSource source, TDestination destination);

        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

        void Map<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination);
    }
}
