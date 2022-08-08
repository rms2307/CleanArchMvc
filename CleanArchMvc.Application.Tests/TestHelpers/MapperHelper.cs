using AutoMapper;
using System;

namespace CleanArchMvc.Application.Tests.TestHelpers
{
    public class MapperHelper
    {
        public static IMapper GetMapper(params Type[] types)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                foreach (var type in types)
                {
                    config.AddProfile(type);
                }
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
