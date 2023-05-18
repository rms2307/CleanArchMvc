using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.VOs;
using System;
using System.Collections.Generic;
using System.IO;

namespace CleanArchMvc.Infrastructure.Files
{
    public class CSVReader : IFileService
    {
        public List<Product> ReadFile(FormFile file)
        {
            try
            {
                StreamReader sr = new(file.FileContent);
                List<Product> products = new();

                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine().Split(";");
                    var product = new Product(
                        row[0].ToString(),
                        row[1].ToString(),
                        decimal.Parse(row[2].ToString()),
                        int.Parse(row[3].ToString()),
                        row[4].ToString(),
                        row[5].ToString(),
                        int.Parse(row[6].ToString()));

                    products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"Erro ao importar arquivo: {ex.Message}");
            }
        }
    }
}