using AutoMapper;
using Bermuda.Core.Mapper;
using System.Collections.Generic;

namespace Bermuda.Infrastructure.Mapper
{
    public class ClassMapper : IClassMapper
    {
        private readonly IMapper mapper;

        public ClassMapper(
            IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            mapper.Map<TSource, TDestination>(source, destination);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }

        public void Map<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination)
        {
            mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source, destination);
        }
    }
}
