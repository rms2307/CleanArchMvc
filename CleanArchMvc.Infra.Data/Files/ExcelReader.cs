using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.VOs;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CleanArchMvc.Infrastructure.Files
{
    public class ExcelReader : IFileService
    {
        public List<Product> ReadFile(FormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<Product> products = new();

            using var ms = new MemoryStream();
            file.FileContent.CopyTo(ms);

            using var reader = ExcelReaderFactory.CreateReader(ms);
            DataSet result = reader.AsDataSet();

            if (result != null)
            {
                DataTable table = result.Tables[0];

                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        products.Add(GetProduct(row));
                    }
                }
            }

            return products;
        }

        private Product GetProduct(DataRow row)
        {
            return new Product(
                row.ItemArray[0].ToString(),
                row.ItemArray[1].ToString(),
                decimal.Parse(row.ItemArray[2].ToString()),
                int.Parse(row.ItemArray[3].ToString()),
                row.ItemArray[4].ToString(),
                int.Parse(row.ItemArray[5].ToString())
            );
        }
    }
}
