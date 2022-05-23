using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.VOs;
using MediatR;
using System.Collections.Generic;

namespace CleanArchMvc.Application.Products.Commands
{
    public class ProductUploadListProductsCommand : IRequest<List<Product>>
    {
        public FormFile FormFile { get; set; }

        public ProductUploadListProductsCommand(FormFile formFile)
        {
            FormFile = formFile;                    
        }
    }
}