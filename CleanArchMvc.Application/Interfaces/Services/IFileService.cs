using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.VOs;
using System.Collections.Generic;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IFileService
    {
        List<Product> ReadFile(FormFile file);
    }
}
