using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Products.Commands;

namespace CleanArchMvc.Application.Mappings
{
    public class DTOToCommandMappingProfile:Profile
    {
        public DTOToCommandMappingProfile()
        {
            CreateMap<CreateProductDTO, ProductCreateCommand>();
            CreateMap<UpdateProductDTO, ProductUpdateCommand>();
        }
    }
}